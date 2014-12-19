using System;
using System.Collections.Generic;
using System.Text;
using LogJoint.RegularExpressions;
using System.Linq;
using System.Diagnostics;

namespace LogJoint
{
	[DebuggerDisplay("Time={Time}, Hash={MessageHash}")]
	public class Bookmark : IBookmark
	{
		public Bookmark(MessageTimestamp time, int hash, IThread thread, string displayName, long? position) :
			this(time, hash, thread, thread != null && !thread.IsDisposed && thread.LogSource != null ? thread.LogSource.ConnectionId : "", displayName, position)
		{ }
		public Bookmark(IMessage line)
			: this(line.Time, line.GetHashCode(), line.Thread, line.Text.Value, line.Position)
		{ }
		public Bookmark(MessageTimestamp time)
			: this(time, 0, null, null, null)
		{ }

		MessageTimestamp IBookmark.Time { get { return time; } }
		int IBookmark.MessageHash { get { return lineHash; } }
		IThread IBookmark.Thread { get { return thread; } }
		string IBookmark.LogSourceConnectionId { get { return logSourceConnectionId; } }
		long? IBookmark.Position { get { return position; } }
		string IBookmark.DisplayName { get { return displayName; } }
		IBookmark IBookmark.Clone()
		{
			return new Bookmark(time, lineHash, thread, logSourceConnectionId, displayName, position);
		}

		public override string ToString()
		{
			return string.Format("{0} {1}", time.ToUserFrendlyString(false), displayName ?? "");
		}

		internal Bookmark(MessageTimestamp time, int hash, IThread thread, string logSourceConnectionId, string displayName, long? position)
		{
			this.time = time;
			this.lineHash = hash;
			this.thread = thread;
			this.displayName = displayName;
			this.position = position;
			this.logSourceConnectionId = logSourceConnectionId;
		}

		MessageTimestamp time;
		int lineHash;
		IThread thread;
		string logSourceConnectionId;
		long? position;
		string displayName;
	}
}