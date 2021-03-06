﻿using System.Collections.Generic;

namespace LogJoint.Analytics.Correlation
{

	public class SameNodeEqualityComparer : IEqualityComparer<ISameNodeDetectionToken>
	{
		bool IEqualityComparer<ISameNodeDetectionToken>.Equals(ISameNodeDetectionToken x, ISameNodeDetectionToken y)
		{
			return x.DetectSameNode(y) != null;
		}

		int IEqualityComparer<ISameNodeDetectionToken>.GetHashCode(ISameNodeDetectionToken obj)
		{
			return 0; // all tokens will have same hash code to force slow comparision via Equals(x, y)
		}
	};
}
