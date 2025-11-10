using Gma.System.MouseKeyHook;
using System;
using System.Drawing;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using System.IO;

namespace TTCOverlay
{
	public partial class MainForm : Form
	{
		private string searchURL = "https://us.tamrieltradecentre.com/pc/Trade/SearchResult?ItemID={0}&SearchType={1}&ItemNamePattern={2}&ItemCategory1ID=&ItemCategory2ID=&ItemCategory3ID=&ItemTraitID=&ItemQualityID=&IsChampionPoint=false&LevelMin=&LevelMax=&MasterWritVoucherMin=&MasterWritVoucherMax=&AmountMin=&AmountMax=&PriceMin=&PriceMax=";
		private string suggestionURL = "https://us.tamrieltradecentre.com/api/pc/Trade/GetItemAutoComplete?term={0}";
		private Regex beginCharacterLine = new Regex("^ {20}([^}{]+)$");
		private Regex endCharacterLine = new Regex(@"^ {20}\}");
		private Regex beginResearchLine = new Regex("^ {24}([^}{]+)$");
		private Regex endResearchLine = new Regex(@"^ {24}\}");
		private Regex beginTraitLine = new Regex("^ {28}(.*)$");
		private AutoCompleteStringCollection autoCompleteStrings = new AutoCompleteStringCollection();
		private SearchResultsForm searchResultsForm = null;
		private SearchResultsForm priceCheckResultsForm = null;
		private IKeyboardMouseEvents m_Events;

		public MainForm()
		{
			InitializeComponent();
			this.DoubleBuffered = true;
			this.SetStyle(ControlStyles.ResizeRedraw, true);
			this.AddDrag(this.label1);
			SubscribeGlobal();
		}

		// This adds the event handler for the control
		private void AddDrag(Control Control)
		{
			Control.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragForm_MouseDown);
		}

		public const int WM_NCLBUTTONDOWN = 0xA1;
		public const int HT_CAPTION = 0x2;
		[System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
		public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
		[System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
		public static extern bool ReleaseCapture();

		private void DragForm_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				ReleaseCapture();
				SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
			}
		}

		private const int cGrip = 16;      // Grip size
										   //private const int cCaption = 32;   // Caption bar height;
		private const int cCaption = 0;   // Caption bar height;
		protected override void WndProc(ref Message m)
		{
			// flag values at https://docs.microsoft.com/en-us/windows/desktop/inputdev/wm-nchittest
			if (m.Msg == 0x84) // Trap WM_NCHITTEST
			{
				Point pos = new Point(m.LParam.ToInt32());
				pos = this.PointToClient(pos);
				if (pos.Y < cCaption)
				{
					m.Result = (IntPtr)2;  // HTCAPTION
					return;
				}
				if (pos.X >= this.ClientSize.Width && pos.Y >= cCaption) // are at the right edge
				{
					m.Result = (IntPtr)11; // HTRIGHT
					return;
				}
			}
			base.WndProc(ref m);
		}

		private void SubscribeApplication()
		{
			Unsubscribe();
			Subscribe(Hook.AppEvents());
		}

		private void SubscribeGlobal()
		{
			Unsubscribe();
			Subscribe(Hook.GlobalEvents());
		}

		private void Subscribe(IKeyboardMouseEvents events)
		{
			m_Events = events;
			m_Events.KeyDown += OnKeyDown;
			m_Events.KeyUp += OnKeyUp;
			m_Events.KeyPress += OnKeyPress;
		}

		private void Unsubscribe()
		{
			if (m_Events == null) return;

			m_Events.KeyDown -= OnKeyDown;
			m_Events.KeyUp -= OnKeyUp;
			m_Events.KeyPress -= OnKeyPress;

			m_Events.Dispose();
			m_Events = null;
		}


		private void OnKeyDown(object sender, KeyEventArgs e)
		{
			if (e.Control && e.Shift && e.KeyCode == Keys.T)
			{
				if (this.WindowState == FormWindowState.Minimized)
				{
					this.WindowState = FormWindowState.Normal;
				}
				else
				{
					this.WindowState = FormWindowState.Minimized;
				}
				this.Focus();
				e.Handled = true;
			}
		}

		private void OnKeyUp(object sender, KeyEventArgs e)
		{
		}

		private void OnKeyPress(object sender, KeyPressEventArgs e)
		{
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams createParams = base.CreateParams;
				createParams.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
				return createParams;
			}
		}

		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Unsubscribe();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Environment.Exit(0);
		}

		private void MainForm_Click(object sender, EventArgs e)
		{
			MouseEventArgs me = (MouseEventArgs)e;
			if (me.Button == MouseButtons.Right)
			{
				contextMenuStrip1.Show(this, me.Location);
			}
		}

		private async void comboBox1_KeyUp2(object sender, KeyEventArgs e)
		{
			Regex recordPattern = new Regex("<img(.*?)<div class=\"item-quality.*?row-separator");
			Regex categoryPattern = new Regex("(?:data-category2)|(?:data-master-writ-description)=\"([^\"]+)\"");
			Regex traitPattern = new Regex("data-trait=\"([^\"]+)\"");
			Regex rarityPattern = new Regex("item-quality-([^\"]+)\"");

			if (e.KeyCode == Keys.Enter)
			{
				string itemID = "";
				string term = WebUtility.UrlEncode(this.comboBox1.Text);
				if (this.comboBox1.SelectedIndex != -1
				&& this.comboBox1.Text.Equals(((Suggestion)this.comboBox1.SelectedItem).Label))
				{
					itemID = ((Suggestion)this.comboBox1.SelectedItem).ID;
					if (itemID.Length > 0)
					{
						term = "";
					}
				}
				e.SuppressKeyPress = true;
				HttpClient client = new HttpClient();
				string searchHref = string.Format(searchURL, itemID, searchRadioButton.Checked ? "Sell" : "PriceCheck", term);
				string result = await client.GetStringAsync(searchHref);
				//Console.WriteLine(searchHref);
				result += await client.GetStringAsync(searchHref + "&page=2");
				result = Regex.Replace(result, @"[\r\n]+", "");
				result = WebUtility.HtmlDecode(result);

				if (searchRadioButton.Checked)
				{
					Regex pat = new Regex("<img(.*?)<div class=\"item-quality.*?row-separator");
					Regex pat2 = new Regex("<img.*?<div class=\"" + @"item-quality[^>]+[^A-Za-z0-0]+([A-Za-z0-9 #;&,':\-]+)[^0-9]+([0-9]+)([^0-9]+([0-9]+)){0,1}<[^<]+[^>]+[^<]+[^>]+[^<]+[^>]+[^<]+[^>]+[^<]+[^>]+[^<]+[^>]+[^<]+[^>]+[^A-Za-z0-9]+((?:[^\s]+ )+)[^<]+[^>]+[^<]+[^>]+[^A-Za-z0-9]+((?:[^\s]+ )+)[^0-9]+([0-9,]+)[^0-9]+([0-9]+)[^0-9]+([0-9,]+)[^0-9]+([0-9]+)");
					Regex praxisNoLocationPattern = new Regex(@"\s*X\s*");

					List<Listing> resultsBindingSource = new List<Listing>();
					foreach (Match match in pat.Matches(result))
					{
						Match match2 = pat2.Match(match.Value);
						if (praxisNoLocationPattern.IsMatch(match2.Groups[6].Value))
						{
							continue;
						}

						string itemDetails = match.Groups[1].Value;
						Match categoryMatch = categoryPattern.Match(itemDetails);
						Match traitMatch = traitPattern.Match(itemDetails);
						Match rarityMatch = rarityPattern.Match(itemDetails);

						Listing listing = new Listing();

						string optionalGroup = rarityMatch.Groups[1].Value;
						if (!string.IsNullOrEmpty(optionalGroup)) // record has a rarity
						{
							listing.Rarity = optionalGroup;
						}
						else
						{
							listing.Rarity = "N/A";
						}

						optionalGroup = categoryMatch.Groups[1].Value;
						if (!string.IsNullOrEmpty(optionalGroup)) // record has a category
						{
							listing.Description = optionalGroup;
						}
						else
						{
							listing.Description = "N/A";
						}

						optionalGroup = traitMatch.Groups[1].Value;
						if (!string.IsNullOrEmpty(optionalGroup)) // record has a category
						{
							listing.Trait = optionalGroup;
						}
						else
						{
							listing.Trait = "N/A";
						}

						listing.Name = match2.Groups[1].Value;
						optionalGroup = match2.Groups[4].Value;
						if (!string.IsNullOrEmpty(optionalGroup))
						{
							listing.Vouchers = int.Parse(match2.Groups[2].Value, NumberStyles.AllowThousands);
							listing.Level = int.Parse(match2.Groups[4].Value);
						}
						else
						{
							listing.Vouchers = 0;
							listing.Level = int.Parse(match2.Groups[2].Value);
						}
						listing.Location = match2.Groups[5].Value;
						listing.Guild = match2.Groups[6].Value;
						listing.UnitPrice = int.Parse(match2.Groups[7].Value, NumberStyles.AllowThousands);
						listing.Quantity = int.Parse(match2.Groups[8].Value, NumberStyles.AllowThousands);
						listing.TotalPrice = int.Parse(match2.Groups[9].Value, NumberStyles.AllowThousands);
						listing.MinutesOld = int.Parse(match2.Groups[10].Value);
						resultsBindingSource.Add(listing);
					}
					if (this.searchResultsForm == null)
					{
						this.searchResultsForm = new SearchResultsForm();
					}
					this.searchResultsForm.ListingBindingSource = resultsBindingSource;
					this.searchResultsForm.Visible = false;
					this.searchResultsForm.Show(this);
				}
				else if (priceCheckRadioButton.Checked)
				{
					Regex parsedRecordPattern = new Regex("<img.*?<div class=\"" + @"item-quality[^>]+[^A-Za-z0-0]+([A-Za-z0-9 #;&,':\-]+)[^0-9]+([0-9]+)(?:[^0-9]+([0-9,.]+)){0,1}<.*?Min[^0-9]+([0-9,.]+).*?Avg[^0-9]+([0-9,.]+).*?Max[^0-9]+([0-9.,]+).*?<td>((Not enough data)|([^0-9]+([0-9,.]+)[^~]+~[^0-9]+([0-9.,]+)))[^0-9]+([0-9]+).*?Listings(?:[^0-9]+([0-9.,]+) ){0,1}");

					List<PriceInfo> resultsBindingSource = new List<PriceInfo>();
					foreach (Match match in recordPattern.Matches(result))
					{
						string itemDetails = match.Groups[1].Value;
						Match categoryMatch = categoryPattern.Match(itemDetails);
						Match traitMatch = traitPattern.Match(itemDetails);
						Match rarityMatch = rarityPattern.Match(itemDetails);
						Match match2 = parsedRecordPattern.Match(match.Value);

						PriceInfo priceInfo = new PriceInfo();

						string optionalGroup = rarityMatch.Groups[1].Value;
						if (!string.IsNullOrEmpty(optionalGroup)) // record has a rarity
						{
							priceInfo.Rarity = optionalGroup;
						}
						else
						{
							priceInfo.Rarity = "N/A";
						}

						optionalGroup = categoryMatch.Groups[1].Value;
						if (!string.IsNullOrEmpty(optionalGroup)) // record has a category
						{
							priceInfo.Description = optionalGroup;
						}
						else
						{
							priceInfo.Description = "N/A";
						}

						optionalGroup = traitMatch.Groups[1].Value;
						if (!string.IsNullOrEmpty(optionalGroup)) // record has a category
						{
							priceInfo.Trait = optionalGroup;
						}
						else
						{
							priceInfo.Trait = "N/A";
						}

						priceInfo.Name = match2.Groups[1].Value;
						optionalGroup = match2.Groups[3].Value;
						if (!string.IsNullOrEmpty(optionalGroup)) // voucher info
						{
							priceInfo.Vouchers = int.Parse(match2.Groups[2].Value, NumberStyles.AllowThousands);
							priceInfo.Level = int.Parse(match2.Groups[3].Value);
						}
						else
						{
							priceInfo.Vouchers = 0;
							priceInfo.Level = int.Parse(match2.Groups[2].Value);
						}
						priceInfo.Min = decimal.Parse(match2.Groups[4].Value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands);
						priceInfo.Avg = decimal.Parse(match2.Groups[5].Value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands);
						priceInfo.Max = decimal.Parse(match2.Groups[6].Value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands);
						optionalGroup = match2.Groups[8].Value;
						if (string.IsNullOrEmpty(optionalGroup)) // suggested price info
						{
							priceInfo.Low = decimal.Parse(match2.Groups[10].Value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands);
							priceInfo.High = decimal.Parse(match2.Groups[11].Value, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands);
						}
						else // "Not enough data"
						{
							priceInfo.Low = 0;
							priceInfo.High = 0;
						}
						priceInfo.Listings = int.Parse(match2.Groups[12].Value, NumberStyles.AllowThousands);
						optionalGroup = match2.Groups[13].Value;
						if (!string.IsNullOrEmpty(optionalGroup)) // # of items 
						{
							priceInfo.Items = int.Parse(match2.Groups[13].Value, NumberStyles.AllowThousands);
						}
						else
						{
							priceInfo.Items = 0;
						}
						resultsBindingSource.Add(priceInfo);
					}
					if (this.priceCheckResultsForm == null)
					{
						this.priceCheckResultsForm = new SearchResultsForm();
					}
					this.priceCheckResultsForm.PriceInfoBindingSource = resultsBindingSource;
					this.priceCheckResultsForm.Visible = false;
					this.priceCheckResultsForm.Show(this);
				}
			}
			else if (e.KeyCode == Keys.Back)
			{
				suggestionItems(this.comboBox1.Text);
			}
		}

		private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
		{
			if ( !char.IsLetterOrDigit(e.KeyChar) )
			{
				return;
			}
			suggestionItems(this.comboBox1.Text + e.KeyChar);
			e.Handled = true;
		}

		private void suggestionItems(string term)
		{
			//Console.WriteLine(term);
			if (term.Length >= 3)
			{
				HttpClient client = new HttpClient();
				string searchHref = string.Format(suggestionURL, WebUtility.UrlEncode(term));
				//Console.WriteLine(searchHref);
				try
				{
					Task<string> resultTask = client.GetStringAsync(searchHref);
					resultTask.Wait();
					string result = resultTask.Result;
					JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
					object[] items = jsonSerializer.Deserialize<dynamic>(result);
					List<Suggestion> suggestions = new List<Suggestion>();
					Dictionary<string, object> suggestion = new Dictionary<string, object>();
					suggestion["label"] = term;
					suggestion["ItemID"] = "";
					suggestions.Add(new Suggestion(suggestion));
					foreach (object item in items)
					{
						suggestions.Add(new Suggestion((Dictionary<string, object>)item));
					}
					this.comboBox1.DataSource = suggestions;
					this.comboBox1.DroppedDown = true;
					this.comboBox1.SelectedIndex = -1;
					this.comboBox1.Text = term;
					this.comboBox1.SelectionStart = term.Length;
					//Console.WriteLine(result);
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.Message);
				}
			}
			else
			{
				this.comboBox1.DataSource = new List<Suggestion>();
				this.comboBox1.Text = term;
				this.comboBox1.SelectionStart = term.Length;
			}
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
		}

		private void comboBox1_SelectionChangeCommitted(object sender, EventArgs e)
		{

		}

		private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
		{

		}

		private void comboBox1_DropDownClosed(object sender, EventArgs e)
		{
		}

		private void button1_Click(object sender, EventArgs e)
		{
			string myDocuments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
			string savedVariablesFile = myDocuments + @"\Elder Scrolls Online\live\SavedVariables\SaveResearch.lua";
			List<string> lines = File.ReadAllLines(savedVariablesFile).ToList<string>();
			for ( int i = 0; i < lines.Count; i++ )
			{
				Match cMatch = beginCharacterLine.Match(lines[i]);
				if ( cMatch.Success )
				{
					string cName = cMatch.Groups[1].Value;
					for (i+=2; i < lines.Count; i++ )
					{
						Console.WriteLine("begin char: " + cName);
						Match researchMatch = beginResearchLine.Match(lines[i]);
						if ( researchMatch.Success )
						{
							string researchName = researchMatch.Groups[1].Value;
							Console.WriteLine("begin research: " + researchName);
							for ( i+=2; i < lines.Count; i++ )
							{
								Match traitMatch = beginTraitLine.Match(lines[i]);
								if ( traitMatch.Success )
								{
									string traitName = traitMatch.Groups[1].Value;
									Console.WriteLine("trait: " + traitName);
								}
								else
								{
									Match end = endResearchLine.Match(lines[i]);
									if (end.Success)
									{
										Console.WriteLine("end research: " + cName);
										break;
									}
								}
							}
						}
						else
						{
							Match end = endCharacterLine.Match(lines[i]);
							if (end.Success )
							{
								Console.WriteLine("end char: " + cName);
								break;
							}
						}
					}
				}
			}
		}
	}
}
