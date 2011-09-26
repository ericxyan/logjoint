namespace LogJoint.UI
{
	partial class LogViewerControl
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogViewerControl));
			this.warnPictureBox = new System.Windows.Forms.PictureBox();
			this.errPictureBox = new System.Windows.Forms.PictureBox();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.copyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.collapseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.recursiveCollapseMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.gotoParentFrameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.gotoEndOfFrameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.gotoNextMessageInTheThreadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.gotoPrevMessageInTheThreadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toggleBmkStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.showTimeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.propertiesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.bookmarkPictureBox = new System.Windows.Forms.PictureBox();
			this.smallBookmarkPictureBox = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.warnPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.errPictureBox)).BeginInit();
			this.contextMenuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.bookmarkPictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.smallBookmarkPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// warnPictureBox
			// 
			this.warnPictureBox.Cursor = System.Windows.Forms.Cursors.Default;
			this.warnPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("warnPictureBox.Image")));
			this.warnPictureBox.Location = new System.Drawing.Point(0, 0);
			this.warnPictureBox.Name = "warnPictureBox";
			this.warnPictureBox.Size = new System.Drawing.Size(11, 11);
			this.warnPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.warnPictureBox.TabIndex = 0;
			this.warnPictureBox.TabStop = false;
			// 
			// errPictureBox
			// 
			this.errPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("errPictureBox.Image")));
			this.errPictureBox.Location = new System.Drawing.Point(0, 0);
			this.errPictureBox.Name = "errPictureBox";
			this.errPictureBox.Size = new System.Drawing.Size(11, 11);
			this.errPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.errPictureBox.TabIndex = 0;
			this.errPictureBox.TabStop = false;
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyMenuItem,
            this.collapseMenuItem,
            this.recursiveCollapseMenuItem,
            this.gotoParentFrameMenuItem,
            this.gotoEndOfFrameMenuItem,
            this.gotoNextMessageInTheThreadMenuItem,
            this.gotoPrevMessageInTheThreadMenuItem,
            this.toggleBmkStripMenuItem,
            this.toolStripSeparator1,
            this.showTimeMenuItem,
            this.propertiesMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(275, 230);
			this.contextMenuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.contextMenuStrip1_ItemClicked);
			this.contextMenuStrip1.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip1_Opening);
			// 
			// copyMenuItem
			// 
			this.copyMenuItem.Name = "copyMenuItem";
			this.copyMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.C)));
			this.copyMenuItem.Size = new System.Drawing.Size(274, 22);
			this.copyMenuItem.Text = "Copy";
			// 
			// collapseMenuItem
			// 
			this.collapseMenuItem.Name = "collapseMenuItem";
			this.collapseMenuItem.ShortcutKeyDisplayString = "Arrows";
			this.collapseMenuItem.Size = new System.Drawing.Size(274, 22);
			this.collapseMenuItem.Text = "Collapse/expand";
			// 
			// recursiveCollapseMenuItem
			// 
			this.recursiveCollapseMenuItem.Name = "recursiveCollapseMenuItem";
			this.recursiveCollapseMenuItem.ShortcutKeyDisplayString = "Ctrl + Arrows";
			this.recursiveCollapseMenuItem.Size = new System.Drawing.Size(274, 22);
			this.recursiveCollapseMenuItem.Text = "Recursive collapse/expand";
			// 
			// gotoParentFrameMenuItem
			// 
			this.gotoParentFrameMenuItem.Name = "gotoParentFrameMenuItem";
			this.gotoParentFrameMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)));
			this.gotoParentFrameMenuItem.Size = new System.Drawing.Size(274, 22);
			this.gotoParentFrameMenuItem.Text = "Go to parent frame";
			// 
			// gotoEndOfFrameMenuItem
			// 
			this.gotoEndOfFrameMenuItem.Name = "gotoEndOfFrameMenuItem";
			this.gotoEndOfFrameMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)));
			this.gotoEndOfFrameMenuItem.Size = new System.Drawing.Size(274, 22);
			this.gotoEndOfFrameMenuItem.Text = "Go to the end of frame";
			// 
			// gotoNextMessageInTheThreadMenuItem
			// 
			this.gotoNextMessageInTheThreadMenuItem.Name = "gotoNextMessageInTheThreadMenuItem";
			this.gotoNextMessageInTheThreadMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Down)));
			this.gotoNextMessageInTheThreadMenuItem.Size = new System.Drawing.Size(274, 22);
			this.gotoNextMessageInTheThreadMenuItem.Text = "Go to next message in thread";
			// 
			// gotoPrevMessageInTheThreadMenuItem
			// 
			this.gotoPrevMessageInTheThreadMenuItem.Name = "gotoPrevMessageInTheThreadMenuItem";
			this.gotoPrevMessageInTheThreadMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Up)));
			this.gotoPrevMessageInTheThreadMenuItem.Size = new System.Drawing.Size(274, 22);
			this.gotoPrevMessageInTheThreadMenuItem.Text = "Go to prev message in thread";
			// 
			// toggleBmkStripMenuItem
			// 
			this.toggleBmkStripMenuItem.Name = "toggleBmkStripMenuItem";
			this.toggleBmkStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
			this.toggleBmkStripMenuItem.Size = new System.Drawing.Size(274, 22);
			this.toggleBmkStripMenuItem.Text = "Toggle bookmark";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(271, 6);
			// 
			// showTimeMenuItem
			// 
			this.showTimeMenuItem.Name = "showTimeMenuItem";
			this.showTimeMenuItem.Size = new System.Drawing.Size(274, 22);
			this.showTimeMenuItem.Text = "Show Time";
			// 
			// propertiesMenuItem
			// 
			this.propertiesMenuItem.Name = "propertiesMenuItem";
			this.propertiesMenuItem.Size = new System.Drawing.Size(274, 22);
			this.propertiesMenuItem.Text = "Show properties...";
			// 
			// bookmarkPictureBox
			// 
			this.bookmarkPictureBox.Cursor = System.Windows.Forms.Cursors.Cross;
			this.bookmarkPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("bookmarkPictureBox.Image")));
			this.bookmarkPictureBox.Location = new System.Drawing.Point(0, 0);
			this.bookmarkPictureBox.Name = "bookmarkPictureBox";
			this.bookmarkPictureBox.Size = new System.Drawing.Size(13, 9);
			this.bookmarkPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.bookmarkPictureBox.TabIndex = 0;
			this.bookmarkPictureBox.TabStop = false;
			// 
			// smallBookmarkPictureBox
			// 
			this.smallBookmarkPictureBox.Cursor = System.Windows.Forms.Cursors.Cross;
			this.smallBookmarkPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("smallBookmarkPictureBox.Image")));
			this.smallBookmarkPictureBox.Location = new System.Drawing.Point(0, 0);
			this.smallBookmarkPictureBox.Name = "smallBookmarkPictureBox";
			this.smallBookmarkPictureBox.Size = new System.Drawing.Size(10, 7);
			this.smallBookmarkPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.smallBookmarkPictureBox.TabIndex = 0;
			this.smallBookmarkPictureBox.TabStop = false;
			((System.ComponentModel.ISupportInitialize)(this.warnPictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.errPictureBox)).EndInit();
			this.contextMenuStrip1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.bookmarkPictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.smallBookmarkPictureBox)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox warnPictureBox;
		private System.Windows.Forms.PictureBox errPictureBox;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem copyMenuItem;
		private System.Windows.Forms.ToolStripMenuItem collapseMenuItem;
		private System.Windows.Forms.ToolStripMenuItem recursiveCollapseMenuItem;
		private System.Windows.Forms.ToolStripMenuItem gotoParentFrameMenuItem;
		private System.Windows.Forms.ToolStripMenuItem gotoEndOfFrameMenuItem;
		private System.Windows.Forms.ToolStripMenuItem showTimeMenuItem;
		private System.Windows.Forms.PictureBox bookmarkPictureBox;
		private System.Windows.Forms.PictureBox smallBookmarkPictureBox;
		private System.Windows.Forms.ToolStripMenuItem propertiesMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem toggleBmkStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem gotoNextMessageInTheThreadMenuItem;
		private System.Windows.Forms.ToolStripMenuItem gotoPrevMessageInTheThreadMenuItem;

	}
}