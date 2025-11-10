using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Dynamic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TTCOverlay
{
	public partial class SearchResultsForm : Form
	{
		private List<Listing> listingBindingSource;
		private List<PriceInfo> priceInfoBindingSource;

		public SearchResultsForm()
		{
			InitializeComponent();
			this.ResultsGridView.AutoGenerateColumns = true;
			this.ResultsGridView.AutoSize = true;
		}

		private bool sortAscending = false;

		public List<Listing> ListingBindingSource
		{
			get => listingBindingSource;
			set
			{
				listingBindingSource = value;
				this.ResultsGridView.DataSource = listingBindingSource;
				priceInfoBindingSource = null;
			}
		}

		public List<PriceInfo> PriceInfoBindingSource
		{
			get => priceInfoBindingSource;
			set
			{
				priceInfoBindingSource = value;
				this.ResultsGridView.DataSource = priceInfoBindingSource;
				listingBindingSource = null;
			}
		}

		private void dataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
		{
			if (listingBindingSource != null)
			{
				sortRows(listingBindingSource, e.ColumnIndex);
			}
			else
			{
				sortRows(priceInfoBindingSource, e.ColumnIndex);
			}

		}
		private void sortRows<T>(List<T> bindingSource, int column)
		{
			if (sortAscending)
			{
				ResultsGridView.DataSource = bindingSource.OrderBy(ResultsGridView.Columns[column].DataPropertyName).ToList();
			}
			else
			{
				ResultsGridView.DataSource = bindingSource.OrderBy(ResultsGridView.Columns[column].DataPropertyName).Reverse().ToList();
			}

			sortAscending = !sortAscending;
		}

		private void SearchResultsForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			this.Hide();
			e.Cancel = true;
		}

		private void ResultsGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
		{
			this.ResultsGridView.ClearSelection();
			foreach (DataGridViewRow row in this.ResultsGridView.Rows)
			{
				string value = (string)row.Cells[0].Value;
				DataGridViewCellStyle style = new DataGridViewCellStyle(row.Cells[0].Style);
				switch (value)
				{
					case "normal":
						style.BackColor = Color.White;
						style.ForeColor = Color.Black;
						row.Cells[0].Style = style;
						break;
					case "fine":
						style.BackColor = Color.Green;
						style.ForeColor = Color.White;
						row.Cells[0].Style = style;
						break;
					case "superior":
						style.BackColor = Color.Blue;
						style.ForeColor = Color.White;
						row.Cells[0].Style = style;
						break;
					case "epic":
						style.BackColor = Color.Purple;
						style.ForeColor = Color.White;
						row.Cells[0].Style = style;
						break;
					case "legendary":
						style.BackColor = Color.Gold;
						style.ForeColor = Color.White;
						row.Cells[0].Style = style;
						break;
				}
			}
			LibWin.ResizeFormToFitDataGridView(this.ResultsGridView, System.Drawing.Rectangle.Empty, LibWin.ResizeFormOption.None);
		}
	}

	public static partial class LibWin
	{
		/**
		<summary>Options for ResizeFormToFitDataGridView</summary>
		*/
		public enum ResizeFormOption
		{
			/**
			<summary>No options</summary>
			*/
			None = 0
		,
			/**
			<summary>Make the Form fixed size if possible</summary>
			<remarks>Also sets MinimumSize and MaximumSize</remarks>
			*/
			Freeze = 1
		,
			/**
			<summary>Don't make the Form smaller than its MinimumSize</summary>
			*/
			HonorMinimumSize = 2
		,
			/**
			<summary>Don't make the Form larger than its MaximumSize</summary>
			*/
			HonorMaximumSize = 4
		,
			/**
			<summary>Don't swallow ArgumentOutOfRangeExceptions</summary>
			*/
			Throw = 8
		}

		/* Used to avoid scrollbars, adjust as necessary */
		private const int lagniappe = 2;

		/**
		<summary>Attempts to resize the Form to show the entire table of a DataGridView</summary>
		<param name="Grid">The DataGridView that should cause its Form to resize</param>
		<param name="Constraint">A Rectangle to use to limit the size and position of the Form
		  <para>Constraint may be System.Drawing.Rectangle.Empty</para>
		  <para>or consider System.Windows.Forms.Screen.FromControl ( Grid ).Bounds</para>
		</param>
		<param name="Options">See the ResizeFormOption enumeration</param>
		<returns>False if the resize operation swallowed an ArgumentOutOfRangeException; otherwise true</returns>
		<exception cref="System.ArgumentOutOfRangeException">If the Form could not be resized</exception>
		*/
		public static bool
		ResizeFormToFitDataGridView
		(
		  System.Windows.Forms.DataGridView Grid
		,
		  System.Drawing.Rectangle Constraint
		,
		  ResizeFormOption Options
		)
		{
			bool result = true;

			System.Windows.Forms.Form form = Grid.FindForm();

			/* Don't resize fixed-size Forms */
			if
			(
			  (form.FormBorderStyle != System.Windows.Forms.FormBorderStyle.FixedSingle)
			&&
			  (form.FormBorderStyle != System.Windows.Forms.FormBorderStyle.FixedToolWindow)
			)
			{
				int width = lagniappe;
				int height = lagniappe;

				#region Calculate the desired width

				/* Add the current width of the form outside the Grid */
				width += form.Width - Grid.Width;

				/* Add the width of the row headers */
				width += Grid.RowHeadersWidth;

				/* Add each visible column's width */
				for (int i = 0; i < Grid.ColumnCount; i++)
				{
					if (Grid.Columns[i].Visible)
					{
						width += Grid.Columns[i].Width;
					}
				}

				#endregion

				#region Calculate the desired height

				/* Add the current height of the form outside the Grid */
				height += form.Height - Grid.Height;

				/* Add the height of the column headers */
				height += Grid.ColumnHeadersHeight;

				/* Add each visible row's height */
				for (int i = 0; i < Grid.RowCount; i++)
				{
					if (Grid.Rows[i].Visible)
					{
						height += Grid.Rows[i].Height;
					}
				}

				#endregion

				#region Honor the MinimumSize of the Form when requested

				if ((Options & ResizeFormOption.HonorMinimumSize) == ResizeFormOption.HonorMinimumSize)
				{
					if (width < form.MinimumSize.Width)
					{
						width = form.MinimumSize.Width;
					}

					if (height < form.MinimumSize.Height)
					{
						height = form.MinimumSize.Height;
					}
				}

				#endregion

				#region Honor the MaximumSize of the Form when requested

				if ((Options & ResizeFormOption.HonorMaximumSize) == ResizeFormOption.HonorMaximumSize)
				{
					if ((form.MaximumSize.Width > 0) && (width > form.MaximumSize.Width))
					{
						width = form.MaximumSize.Width;
					}

					if ((form.MaximumSize.Height > 0) && (height > form.MaximumSize.Height))
					{
						height = form.MaximumSize.Height;
					}
				}

				#endregion

				/* Preferred size of the Form */
				System.Drawing.Size maxsize = new System.Drawing.Size(width, height);

				/* If the Form is already that size, then we needn't continue */
				if (form.Size != maxsize)
				{

					#region Apply Constraint when requested

					if (Constraint != System.Drawing.Rectangle.Empty)
					{
						/* If the preferred width won't fit the Constraint */
						if (width > Constraint.Width)
						{
							/* slide it all the way to the left */
							form.Left = Constraint.X;

							/* and clip the size to extend only to the right edge */
							width = Constraint.Left + Constraint.Width - form.Left;
						}
						/* If the preferred width would extend beyond the right edge of the Constraint, */
						else if (width > Constraint.Left + Constraint.Width - form.Left)
						{
							/* slide the Form toward the left */
							form.Left -= width - (Constraint.Left + Constraint.Width - form.Left);
						}

						/* If the preferred height won't fit the Constraint */
						if (height > Constraint.Height)
						{
							/* slide it all the way to the top */
							form.Top = Constraint.Y;

							/* and clip the size to extend only to the bottom edge */
							height = Constraint.Y + Constraint.Height - form.Top;
						}
						/* If the preferred height would extend beyond the bottom edge of the Constraint, */
						else if (height > Constraint.Top + Constraint.Height - form.Top)
						{
							/* slide the Form toward the top */
							form.Top -= height - (Constraint.Y + Constraint.Height - form.Top);
						}
					}

					#endregion

					/* Constrained size of the form */
					System.Drawing.Size minsize = new System.Drawing.Size(width, height);

					/* If the Form is already that size, then we needn't continue */
					if (form.Size != minsize)
					{
						/* Try to resize the Form */
						try
						{
							form.Size = minsize;

							#region Freeze the Form when requested

							if ((Options & ResizeFormOption.Freeze) == ResizeFormOption.Freeze)
							{
								form.MaximumSize = maxsize;
								form.MinimumSize = minsize;

								if (minsize == maxsize)
								{
									switch (form.FormBorderStyle)
									{
										case System.Windows.Forms.FormBorderStyle.Sizable:
											{
												form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;

												break;
											}

										case System.Windows.Forms.FormBorderStyle.SizableToolWindow:
											{
												form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;

												break;
											}
									}
								}
							}

							#endregion

						}
						/* Resizing across screen boundaries may cause ArgumentOutOfRangeExceptions */
						catch (System.ArgumentOutOfRangeException err)
						{
							if ((Options & ResizeFormOption.Throw) == ResizeFormOption.Throw)
							{
								err.Data["Top"] = form.Top;
								err.Data["Left"] = form.Left;
								err.Data["Width"] = width;
								err.Data["Height"] = height;

								throw;
							}

							result = false;
						}
					}
				}
			}

			return (result);
		}
	}
}
