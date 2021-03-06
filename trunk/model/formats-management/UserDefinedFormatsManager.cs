﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LogJoint
{
	public class UserDefinedFormatsManager : IUserDefinedFormatsManager
	{
		public UserDefinedFormatsManager(IFormatDefinitionsRepository repository, ILogProviderFactoryRegistry registry, ITempFilesManager tempFilesManager)
		{
			if (repository == null)
				throw new ArgumentNullException("repository");
			if (registry == null)
				throw new ArgumentNullException("registry");

			this.repository = repository;
			this.registry = registry;
			this.tempFilesManager = tempFilesManager;
		}

		IFormatDefinitionsRepository IUserDefinedFormatsManager.Repository
		{
			get { return repository; }
		}

		void IUserDefinedFormatsManager.RegisterFormatType(string configNodeName, Type formatType)
		{
			if (string.IsNullOrEmpty(configNodeName))
				throw new ArgumentException("Node name must be a not-null not-empty string", "formatConfigType");

			if (!typeof(IUserDefinedFactory).IsAssignableFrom(formatType))
				throw new ArgumentException("Type must be inherited from " + typeof(IUserDefinedFactory).Name, "formatType");

			nodeNameToType.Add(configNodeName, formatType);
		}

		int IUserDefinedFormatsManager.ReloadFactories()
		{
			tracer.Info("reloading factories");
			int ret = 0;

			MarkAllFactoriesAsNonExisting();

			foreach (IFormatDefinitionRepositoryEntry entry in repository.Entries)
			{
				string location = entry.Location;
				var factory = factories.Where(f => f.factory.Location == location).FirstOrDefault();
				if (factory != null
				 && factory.lastModified == entry.LastModified)
				{
					factory.markedForDeletion = false;
					tracer.Info("factory '{0}' did not change", location);
					continue;
				}
				tracer.Info("factory '{0}' needs (re)loading", location);
				factory = LoadFactory(entry);
				if (factory != null)
				{
					factory.markedForDeletion = false;
					factories.Add(factory);
				}
				++ret;
			}

			ret += DeleteNotExistingFactories();

			tracer.Info("factories changed: {0}", ret);

			return ret;
		}

		IEnumerable<IUserDefinedFactory> IUserDefinedFormatsManager.Items
		{
			get
			{
				return factories.Select(f => f.factory);
			}
		}


		void MarkAllFactoriesAsNonExisting()
		{
			foreach (var f in factories)
			{
				f.markedForDeletion = true;
			}
		}

		int DeleteNotExistingFactories()
		{
			foreach (var f in factories)
			{
				if (f.markedForDeletion)
				{
					tracer.Info("factory '{0}' does not exist anymore. disposing it.", f.factory.Location);
					f.factory.Dispose();
				}
			}
			return ListUtils.RemoveAll(factories, f => f.markedForDeletion);
		}

		FactoryRecord LoadFactory(IFormatDefinitionRepositoryEntry entry)
		{
			var root = entry.LoadFormatDescription();
			return (
				from factoryNodeCandidate in root.Elements()
				where nodeNameToType.ContainsKey(factoryNodeCandidate.Name.LocalName)
				let createParams = new UserDefinedFactoryParams()
				{
					Entry = entry,
					FactoryRegistry = registry,
					TempFilesManager = tempFilesManager,
					FormatSpecificNode = factoryNodeCandidate,
					RootNode = root
				}
				select new FactoryRecord()
				{
					factory = (IUserDefinedFactory)Activator.CreateInstance(
						nodeNameToType[factoryNodeCandidate.Name.LocalName], createParams),
					lastModified = entry.LastModified,
					markedForDeletion = false
				}
			).FirstOrDefault();
		}

		class FactoryRecord
		{
			public IUserDefinedFactory factory;
			public DateTime lastModified;
			public bool markedForDeletion;
		};

		readonly IFormatDefinitionsRepository repository;
		readonly ILogProviderFactoryRegistry registry;
		readonly ITempFilesManager tempFilesManager;
		readonly LJTraceSource tracer = new LJTraceSource("UserDefinedFormatsManager", "udfm");
		readonly Dictionary<string, Type> nodeNameToType = new Dictionary<string, Type>();
		readonly List<FactoryRecord> factories = new List<FactoryRecord>();
	}
}
