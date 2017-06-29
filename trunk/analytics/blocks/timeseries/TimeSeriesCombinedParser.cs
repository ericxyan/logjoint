﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using TSA = LogJoint.Analytics.TimeSeries;
using LogJoint.Analytics;
using TS = LogJoint.Analytics.TimeSeries.TimeSeriesData;

namespace LogJoint.Analytics.TimeSeries
{
	public class ParserCounter
	{
		public int Calls { get; set; }

		public int Matches { get; set; }

		public TimeSpan Total { get; set; }

		internal Stopwatch Sw;
	}

	public interface ICombinedParser
	{
		/// <summary>
		/// If set to true the calls to regex functions will be measured and
		/// it is possible to get a performance report.
		/// </summary>
		bool ProfilingEnabled { get; set; }
		/// <summary>
		/// If profiling was enabled before parsing this function returns data for all parsers
		/// </summary>
		IDictionary<Type, ParserCounter> GetPerformanceReport();
		IEnumerable<TimeSeriesData> GetParsedTimeSeries();
		IEnumerable<EventBase> GetParsedEvents();
		Task FeedLogMessages<M>(IEnumerableAsync<M[]> messages) 
			where M: ITriggerStreamPosition, ITriggerTime, ITriggerText;
		Task FeedLogMessages<M>(IEnumerableAsync<M[]> messages,
			Func<M, string> getPrefix, Func<M, string> getText)
				where M : ITriggerStreamPosition, ITriggerTime;
	};


	public class TimeSeriesCombinedParser : TSA.ILineParserVisitor, ICombinedParser
	{
		readonly PrefixMatcher _prefixMatcher;
		readonly ILookup<int, TSA.ILineParser> _parsers;
		readonly ILookup<UInt32, TSA.ILineParser> _numericIdCapableParsers;

		private Dictionary<Tuple<TSA.TimeSeriesDescriptor, string>, TimeSeriesData> _timeSeriesMap;
		private List<EventBase> _genericEventsList;

		private bool _profilingEnabled;
		private Dictionary<Type, ParserCounter> _profileData;
		private bool _lastParserSucceeded;

		private long _currentPosition;
		private DateTime _currentTimestamp;


		public TimeSeriesCombinedParser(IEnumerable<Type> eventTypes)
		{
			// Each type can have multiple expressions and hence result in more than one parser instance
			var parsers = new List<TSA.ILineParser>();
			foreach (var t in eventTypes)
			{
				List<Regex> regexps;
				List<string> prefixes;
				List<UInt32> numericIds;
				ExtractExpressions(t, out regexps, out prefixes, out numericIds);

				if (regexps.Count == 0)
					throw new ArgumentException(string.Format("Type {0} is not marked with any of attribute [Expression]", t.Name));

				for (var i = 0; i < regexps.Count; ++i)
				{
					var timeSeriesParser = TSA.TimeSeriesEventParser.TryCreate(t, regexps[i], prefixes[i], numericIds[i]);
					if (timeSeriesParser != null)
						parsers.Add(timeSeriesParser);
					var eventParser = TSA.GenericEventParser.TryCreate(t, regexps[i], prefixes[i], numericIds[0]);
					if (eventParser != null)
						parsers.Add(eventParser);
				}
			}

			_prefixMatcher = new PrefixMatcher();
			_parsers = parsers.Where(p => p.GetNumericId() == 0).ToLookup(p => _prefixMatcher.RegisterPrefix(p.GetPrefix()));
			_numericIdCapableParsers = parsers.Where(p => p.GetNumericId() != 0).ToLookup(p => p.GetNumericId());
		}

		bool ICombinedParser.ProfilingEnabled
		{
			get { return _profilingEnabled; }
			set
			{
				_profilingEnabled = value;
				if (_profilingEnabled)
					_profileData = null;
			}
		}

		IDictionary<Type, ParserCounter> ICombinedParser.GetPerformanceReport()
		{
			return _profileData;
		}

		IEnumerable<TimeSeriesData> ICombinedParser.GetParsedTimeSeries()
		{
			if (_timeSeriesMap == null)
				return null;
			return _timeSeriesMap.Values;
		}

		IEnumerable<EventBase> ICombinedParser.GetParsedEvents()
		{
			return _genericEventsList;
		}

		Task ICombinedParser.FeedLogMessages<M>(IEnumerableAsync<M[]> messages) 
		{
			return FeedLogMessages(messages,  m => m.Text, m => m.Text);
		}

		Task ICombinedParser.FeedLogMessages<M>(IEnumerableAsync<M[]> messages,
			Func<M, string> getPrefix, Func<M, string> getText)
		{
			return FeedLogMessages(messages, getPrefix, getText);
		}

		async Task FeedLogMessages<M>(IEnumerableAsync<M[]> messages, 
			Func<M, string> getPrefix, Func<M, string> getText) where M : ITriggerStreamPosition, ITriggerTime
		{
			PrepareParsing();

			var matchedLogMessages = messages.Select(msgs => msgs.Select(
				m => new KeyValuePair<M, IMatchedPrefixesCollection>(
					m, _prefixMatcher.Match(getPrefix(m)))).ToArray());

			await matchedLogMessages.ForEach(batch =>
			{
				foreach (var m in batch)
				{
					_currentPosition = m.Key.StreamPosition;
					_currentTimestamp = m.Key.Timestamp;
					foreach (var prefix in m.Value)
					{
						foreach (var parser in _parsers[prefix])
						{
							var c = StartMeasure(parser.GetMetadataSource());
							parser.Parse(getText(m.Key), this, null);
							EndMeasure(c);
						}
					}
				}

				return Task.FromResult(true);
			});
		}

		private void PrepareParsing()
		{
			_timeSeriesMap = new Dictionary<Tuple<TSA.TimeSeriesDescriptor, string>, TimeSeriesData>();
			_genericEventsList = new List<EventBase>();
			if (_profilingEnabled)
			{
				_profileData = (from g in _parsers from p in g select p).ToDictionary(p => p.GetMetadataSource(), _ => new ParserCounter());
			}
		}

		private TS GetOrCreateTimeSeries(Tuple<TSA.TimeSeriesDescriptor, string> tsKey)
		{
			TimeSeriesData ts;
			if (!_timeSeriesMap.TryGetValue(tsKey, out ts))
			{
				ts = new TimeSeriesData();
				ts.Descriptor = tsKey.Item1;
				ts.ObjectId = tsKey.Item2;
				ts.Name = ts.Descriptor.Name;
				ts.ObjectType = ts.Descriptor.ObjectType;
				_timeSeriesMap.Add(tsKey, ts);
			}

			return ts;
		}

		void TSA.ILineParserVisitor.VisitTimeSeries(TSA.TimeSeriesDescriptor descriptor, string objectId, double value)
		{
			_lastParserSucceeded = true;
			var tsKey = Tuple.Create(descriptor, objectId);
			TS ts = GetOrCreateTimeSeries(tsKey);

			ts.DataPoints.Add(new TSA.DataPoint()
			{
				LogPosition = _currentPosition,
				Timestamp = _currentTimestamp,
				Value = value
			});
		}

		void TSA.ILineParserVisitor.VisitEvent(TSA.EventBase baseEvt)
		{
			var e = baseEvt;
			_lastParserSucceeded = true;
			e.Timestamp = _currentTimestamp;
			e.LogPosition = _currentPosition;
			_genericEventsList.Add(e);
		}

		private ParserCounter StartMeasure(Type metadataSourceType)
		{
			if (!_profilingEnabled)
				return null;

			var c = _profileData[metadataSourceType];
			++c.Calls;
			_lastParserSucceeded = false;
			c.Sw = Stopwatch.StartNew();
			return c;
		}

		private void EndMeasure(ParserCounter c)
		{
			if (c == null)
				return;

			c.Total += c.Sw.Elapsed;
			if (_lastParserSucceeded)
				c.Matches++;
		}

		static void ExtractExpressions(Type descriptor, out List<Regex> regexps, out List<string> prefixes, out List<UInt32> numericIds)
		{
			var exprAttrs = descriptor.GetCustomAttributes(typeof(ExpressionAttribute), true).OfType<ExpressionAttribute>();

			regexps = new List<Regex>();
			prefixes = new List<string>();
			numericIds = new List<uint>();
			foreach (var exprAttr in exprAttrs)
			{
				regexps.Add(RegexBuilder.Create(exprAttr.Expression));
				prefixes.Add(exprAttr.Prefix);
				numericIds.Add(0);
			}
		}
	}
}
