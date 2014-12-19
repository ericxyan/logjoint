﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LogJoint.UI.Presenters.BookmarksList;

namespace LogJoint.UI
{
	public partial class BookmarksView : UserControl, IView
	{
		public BookmarksView()
		{
			InitializeComponent();

			linkDisplayFont = new Font(listBox.Font, FontStyle.Underline);
			timeDeltaDisplayFont = listBox.Font;
			
			displayStringFormat = new StringFormat();
			displayStringFormat.Alignment = StringAlignment.Near;
			displayStringFormat.LineAlignment = StringAlignment.Near;
			displayStringFormat.Trimming = StringTrimming.EllipsisCharacter;
			displayStringFormat.FormatFlags = StringFormatFlags.NoWrap;

			using (Graphics g = this.CreateGraphics())
				listBox.ItemHeight = (int)(15.0 * g.DpiY / 96.0);
		}

		void IView.SetPresenter(IViewEvents presenter)
		{
			this.presenter = presenter;
		}

		void IView.UpdateItems(IEnumerable<KeyValuePair<IBookmark, TimeSpan?>> items)
		{
			metrics = null;
			listBox.Items.Clear();
			foreach (var i in items)
				listBox.Items.Add(new BookmarkItem(i.Key, i.Value));
		}

		IBookmark IView.SelectedBookmark { get { return Get(listBox.SelectedIndex); } }

		void IView.RefreshFocusedMessageMark()
		{
			var focusedItemMarkBounds = UIUtils.FocusedItemMarkBounds;
			listBox.Invalidate(new Rectangle(
				GetMetrics().FocusedMessageMarkX + focusedItemMarkBounds.Left,
				0,
				focusedItemMarkBounds.Width,
				ClientSize.Height));
		}

		int? GetLinkFromPoint(int x, int y, bool fullRowMode)
		{
			if (listBox.Items.Count == 0)
				return null;
			y -= listBox.GetItemRectangle(0).Top;
			int idx = y / listBox.ItemHeight;
			if (idx < 0 || idx >= listBox.Items.Count)
				return null;
			if (!fullRowMode)
			{
				var txt = listBox.Items[idx].ToString();
				using (var g = listBox.CreateGraphics())
				{
					var m = GetMetrics();
					if (x < m.TextX || x > m.TextX + g.MeasureString(txt, linkDisplayFont, listBox.ClientSize.Width - m.TextX, displayStringFormat).Width)
						return null;
				}
			}
			return idx;
		}

		private void listBox1_DoubleClick(object sender, EventArgs e)
		{
			var pt = listBox.PointToClient(Control.MousePosition);
			if (GetLinkFromPoint(pt.X, pt.Y, true) != null)
				presenter.OnViewDoubleClicked();
		}

		class Metrics
		{
			public int DeltaStringX;
			public int DeltaStringWith;
			public int IconX;
			public int FocusedMessageMarkX;
			public int TextX;
		};

		Metrics CreateMetrics()
		{
			using (var g = this.CreateGraphics())
			{
				var m = new Metrics();
				m.DeltaStringX = 1;

				m.DeltaStringWith = (int)EnumItems()
					.Select(i => i.DeltaStr)
					.Select(s => g.MeasureString(s, timeDeltaDisplayFont, new PointF(), displayStringFormat).Width)
					.Union(Enumerable.Repeat(0f, 1))
					.Max() + 2;

				m.IconX = m.DeltaStringX + m.DeltaStringWith;
				m.FocusedMessageMarkX = m.IconX + imageList1.ImageSize.Width + 1;
				m.TextX = m.FocusedMessageMarkX + 4;
				
				return m;
			}
		}

		private void listBox1_DrawItem(object sender, DrawItemEventArgs e)
		{
			var item = GetItem(e.Index);
			if (item == null)
			{
				e.Graphics.FillRectangle(Brushes.White, e.Bounds);
				return; // DrawItem sometimes called even when no item in the list :(
			}

			if ((e.State & DrawItemState.Selected) != 0)
				e.Graphics.FillRectangle(selectedBkBrush, e.Bounds);
			else
				e.Graphics.FillRectangle(Brushes.White, e.Bounds);

			var m = GetMetrics();

			Rectangle r = e.Bounds;
			r.X = m.DeltaStringX;
			r.Width = m.DeltaStringWith;

			string deltaStr = null;
			if (listBox.SelectedIndices.Count >= 2)
			{
				if (listBox.SelectedIndices[0] != e.Index && listBox.SelectedIndices.Contains(e.Index))
				{
					var prevSelected = listBox.SelectedIndices[listBox.SelectedIndices.IndexOf(e.Index) - 1];
					var delta = new TimeSpan();
					for (int i = prevSelected + 1; i <= e.Index; ++i)
						delta += GetItem(i).Delta.GetValueOrDefault();
					deltaStr = BookmarkItem.DeltaToStr(delta);
				}
			}
			else
			{
				deltaStr = item.DeltaStr;
			}
			if (deltaStr != null)
			{
				e.Graphics.DrawString(
					deltaStr,
					timeDeltaDisplayFont,
					Brushes.Black,
					r,
					displayStringFormat);
			}

			var imgSize = imageList1.ImageSize;
			imageList1.Draw(e.Graphics,
				e.Bounds.X + m.IconX,
				e.Bounds.Y + (e.Bounds.Height - imgSize.Height) / 2, 
				0);

			r.X = m.TextX;
			r.Width = ClientSize.Width - m.TextX;
			e.Graphics.DrawString(item.Bookmark.ToString(), linkDisplayFont, Brushes.Blue, r, displayStringFormat);
			if ((e.State & DrawItemState.Selected) != 0 && (e.State & DrawItemState.Focus) != 0)
			{
				ControlPaint.DrawFocusRectangle(e.Graphics, r, Color.Black, Color.White);
			}
			Tuple<int, int> focused;
			presenter.OnFocusedMessagePositionRequired(out focused);
			if (focused != null)
			{
				int y;
				if (focused.Item1 != focused.Item2)
					y = listBox.ItemHeight * focused.Item1 + listBox.ItemHeight / 2;
				else
					y = listBox.ItemHeight * focused.Item1;
				if (y == 0)
					y = UIUtils.FocusedItemMarkBounds.Height / 2;
				UIUtils.DrawFocusedItemMark(e.Graphics, metrics.FocusedMessageMarkX, y);
			}
		}

		private void listBox1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Enter)
				presenter.OnEnterKeyPressed();
		}

		private void listBox1_MouseDown(object sender, MouseEventArgs e)
		{
			int? linkUnderMouse = GetLinkFromPoint(e.X, e.Y, false);
			if (linkUnderMouse != null)
			{
				if (e.Button == MouseButtons.Left)
				{
					presenter.OnBookmarkLeftClicked(Get(linkUnderMouse.Value));
				}
				else if (e.Button == MouseButtons.Right)
				{
					listBox.SelectedIndex = linkUnderMouse.Value;
				}
			}
			InvailidateDeltasIfMultiselectChanged();
		}

		private void listBox1_MouseMove(object sender, MouseEventArgs e)
		{
			int? linkUnderMouse = GetLinkFromPoint(e.X, e.Y, false);
			listBox.Cursor = linkUnderMouse.HasValue ? Cursors.Hand : Cursors.Default;
		}

		IEnumerable<BookmarkItem> EnumItems()
		{
			return Enumerable.Range(0, listBox.Items.Count).Select(GetItem);
		}

		BookmarkItem GetItem(int index)
		{
			if (index >= 0 && index < listBox.Items.Count)
				return listBox.Items[index] as BookmarkItem;
			return null;
		}

		IBookmark Get(int index)
		{
			var item = GetItem(index);
			if (item != null)
				return item.Bookmark;
			return null;
		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{
			presenter.OnDeleteMenuItemClicked();
		}

		private void contextMenu_Opening(object sender, CancelEventArgs e)
		{
			bool cancel = e.Cancel;
			presenter.OnContextMenu(ref cancel);
			e.Cancel = cancel;
		}

		private void listBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			InvailidateDeltasIfMultiselectChanged();
		}

		void InvailidateDeltasIfMultiselectChanged()
		{
			bool wasMultiselect = lastSelectedCount >= 2;
			bool isMultiselect = listBox.SelectedIndices.Count >= 2;
			lastSelectedCount = listBox.SelectedIndices.Count;
			if ((wasMultiselect != isMultiselect) || (wasMultiselect && isMultiselect))
				listBox.Invalidate(new Rectangle(0, 0, GetMetrics().IconX, Height));
		}

		class BookmarkItem
		{
			readonly public IBookmark Bookmark;
			readonly public string DeltaStr;
			readonly public TimeSpan? Delta;

			public BookmarkItem(IBookmark bookmark, TimeSpan? delta)
			{
				Bookmark = bookmark;
				Delta = delta;
				DeltaStr = DeltaToStr(delta);
			}

			public static string DeltaToStr(TimeSpan? delta)
			{
				if (delta != null)
				{
					if (delta.Value.Ticks <= 0)
						return "+0ms";
					else if (delta.Value >= TimeSpan.FromMilliseconds(1))
						return string.Concat(
							"+",
							string.Join(" ",
								EnumTimeSpanComponents(delta.Value)
								.Where(c => c.Value != 0)
								.Take(2)
								.Select(c => string.Format("{0}{1}", c.Value, c.Key))
							)
						);
					else
						return "+ <1ms";
				}
				else
				{
					return "";
				}
			}

			static IEnumerable<KeyValuePair<string, int>> EnumTimeSpanComponents(TimeSpan ts)
			{
				yield return new KeyValuePair<string, int>("d", ts.Days);
				yield return new KeyValuePair<string, int>("h", ts.Hours);
				yield return new KeyValuePair<string, int>("m", ts.Minutes);
				yield return new KeyValuePair<string, int>("s", ts.Seconds);
				yield return new KeyValuePair<string, int>("ms", ts.Milliseconds);
			}

			public override string ToString()
			{
				return Bookmark.ToString();
			}
		};

		Metrics GetMetrics()
		{
			if (metrics == null)
				metrics = CreateMetrics();
			return metrics;
		}

		private IViewEvents presenter;
		private Font timeDeltaDisplayFont;
		private Font linkDisplayFont;
		private StringFormat displayStringFormat;
		private Metrics metrics;
		private Brush selectedBkBrush = new SolidBrush(Color.FromArgb(197, 206, 231));
		private int lastSelectedCount = -1;
	}

}