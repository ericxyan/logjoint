﻿using System;

using Foundation;
using AppKit;

namespace LogJoint.UI
{
	public partial class EditRegexDialog : NSWindow
	{
		public EditRegexDialog (IntPtr handle) : base (handle)
		{
		}

		[Export ("initWithCoder:")]
		public EditRegexDialog (NSCoder coder) : base (coder)
		{
		}

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();
		}
	}
}
