﻿
using System;
using System.Collections.Generic;
using System.Linq;
using MonoMac.Foundation;
using MonoMac.AppKit;

namespace LogJoint.UI
{
	public partial class MainWindowAdapter : MonoMac.AppKit.NSWindowController
	{
		#region Constructors

		// Called when created from unmanaged code
		public MainWindowAdapter (IntPtr handle) : base (handle)
		{
			Initialize ();
		}
		
		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public MainWindowAdapter (NSCoder coder) : base (coder)
		{
			Initialize ();
		}
		
		// Call to load from the XIB/NIB file
		public MainWindowAdapter () : base ("MainWindow")
		{
			Initialize ();
		}
		
		// Shared initialization code
		void Initialize ()
		{
		}

		public void Init(IModel model)
		{
		}

		#endregion


		public LoadedMessagesControlAdapter LoadedMessagesControlAdapter
		{
			get { return loadedMessagesControlAdapter; }
		}

		public SourcesManagementControlAdapter SourcesManagementControlAdapter
		{
			get { return sourcesManagementControlAdapter; }
		}

		public SearchPanelControlAdapter SearchPanelControlAdapter
		{
			get { return searchPanelControlAdapter; }
		}

		//strongly typed window accessor
		public new MainWindow Window {
			get {
				return (MainWindow)base.Window;
			}
		}

		public override void AwakeFromNib()
		{
			base.AwakeFromNib();

			loadedMessagesControlAdapter = new LoadedMessagesControlAdapter ();
			loadedMessagesControlAdapter.View.MoveToPlaceholder(loadedMessagesPlaceholder);

			sourcesManagementControlAdapter = new SourcesManagementControlAdapter();
			sourcesManagementControlAdapter.View.MoveToPlaceholder(sourcesManagementViewPlaceholder);

			searchPanelControlAdapter = new SearchPanelControlAdapter();
			searchPanelControlAdapter.View.MoveToPlaceholder(searchPanelViewPlaceholder);

			ComponentsInitializer.WireupDependenciesAndInitMainWindow(this);
		}
			
		partial void OnCurrentTabSelected (NSObject sender)
		{
			this.tabView.SelectAt(this.toolbarTabsSelector.SelectedSegment); 
		}


		LoadedMessagesControlAdapter loadedMessagesControlAdapter;
		SourcesManagementControlAdapter sourcesManagementControlAdapter;
		SearchPanelControlAdapter searchPanelControlAdapter;
	}
}
