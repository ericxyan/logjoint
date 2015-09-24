﻿using System;
using MonoMac.Foundation;
using LogJoint.UI.Presenters.SourcesList;
using MonoMac.AppKit;

namespace LogJoint.UI
{
	public class SourcesListItem: NSObject, IViewItem
	{
		public string key;
		public ILogSource logSource;
		public Preprocessing.ILogSourcePreprocessing logSourcePreprocessing;
		public string text;
		public bool isSelected;
		public bool? isChecked;
		public Action<SourcesListItem> updater;

		void IViewItem.SetText(string value)
		{
			text = value;
			Update();
		}

		void IViewItem.SetBackColor(ModelColor color)
		{
			// todo
		}

		ILogSource IViewItem.LogSource
		{
			get { return logSource; }
		}

		Preprocessing.ILogSourcePreprocessing IViewItem.LogSourcePreprocessing
		{
			get { return logSourcePreprocessing; }
		}

		bool IViewItem.Selected
		{
			get
			{
				return isSelected;
			}
			set
			{
				isSelected = value;
				Update();
			}
		}

		bool? IViewItem.Checked
		{
			get
			{
				return isChecked;
			}
			set
			{
				isChecked = value;
				Update();
			}
		}

		void Update()
		{
			if (updater != null)
				updater(this);
		}
	}
}
