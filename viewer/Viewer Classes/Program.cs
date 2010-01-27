using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace LogJoint
{
	static class Program
	{
		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			ListUtils.Tests();
			PositionedMessagesUtils.Tests();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new UI.MainForm());
		}
	}
}