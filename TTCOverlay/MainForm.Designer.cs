namespace TTCOverlay
{
	partial class MainForm
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

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.searchRadioButton = new System.Windows.Forms.RadioButton();
			this.priceCheckRadioButton = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.comboBox1 = new System.Windows.Forms.ComboBox();
			this.button1 = new System.Windows.Forms.Button();
			this.contextMenuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenuStrip1
			// 
			this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
			this.contextMenuStrip1.Name = "contextMenuStrip1";
			this.contextMenuStrip1.Size = new System.Drawing.Size(93, 26);
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
			this.exitToolStripMenuItem.Text = "Exit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// searchRadioButton
			// 
			this.searchRadioButton.AutoSize = true;
			this.searchRadioButton.Checked = true;
			this.searchRadioButton.Location = new System.Drawing.Point(291, 34);
			this.searchRadioButton.Name = "searchRadioButton";
			this.searchRadioButton.Size = new System.Drawing.Size(59, 17);
			this.searchRadioButton.TabIndex = 3;
			this.searchRadioButton.TabStop = true;
			this.searchRadioButton.Text = "Search";
			this.searchRadioButton.UseVisualStyleBackColor = true;
			// 
			// priceCheckRadioButton
			// 
			this.priceCheckRadioButton.AutoSize = true;
			this.priceCheckRadioButton.Location = new System.Drawing.Point(356, 34);
			this.priceCheckRadioButton.Name = "priceCheckRadioButton";
			this.priceCheckRadioButton.Size = new System.Drawing.Size(83, 17);
			this.priceCheckRadioButton.TabIndex = 4;
			this.priceCheckRadioButton.Text = "Price Check";
			this.priceCheckRadioButton.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(97, 20);
			this.label1.TabIndex = 0;
			this.label1.Text = "Search TTC:";
			this.label1.Click += new System.EventHandler(this.MainForm_Click);
			// 
			// comboBox1
			// 
			this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.comboBox1.DisplayMember = "Label";
			this.comboBox1.FormattingEnabled = true;
			this.comboBox1.Location = new System.Drawing.Point(115, 8);
			this.comboBox1.Name = "comboBox1";
			this.comboBox1.Size = new System.Drawing.Size(326, 21);
			this.comboBox1.TabIndex = 5;
			this.comboBox1.DropDownClosed += new System.EventHandler(this.comboBox1_DropDownClosed);
			this.comboBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.comboBox1_KeyPress);
			this.comboBox1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.comboBox1_KeyUp2);
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(115, 35);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(62, 23);
			this.button1.TabIndex = 6;
			this.button1.Text = "Research";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.SystemColors.Window;
			this.ClientSize = new System.Drawing.Size(445, 64);
			this.ControlBox = false;
			this.Controls.Add(this.button1);
			this.Controls.Add(this.comboBox1);
			this.Controls.Add(this.priceCheckRadioButton);
			this.Controls.Add(this.searchRadioButton);
			this.Controls.Add(this.label1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "MainForm";
			this.ShowIcon = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Click += new System.EventHandler(this.MainForm_Click);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DragForm_MouseDown);
			this.contextMenuStrip1.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.RadioButton searchRadioButton;
		private System.Windows.Forms.RadioButton priceCheckRadioButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ComboBox comboBox1;
		private System.Windows.Forms.Button button1;
	}
}

