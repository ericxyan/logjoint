﻿using System;
using System.Threading;
using System.Threading.Tasks;

namespace LogJoint
{
	public interface IShutdown
	{
		Task Shutdown();
		CancellationToken ShutdownToken { get; }
		void AddCleanupTask(Task task);

		event EventHandler Cleanup;
	}
}
