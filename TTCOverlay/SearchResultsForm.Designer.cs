namespace TTCOverlay
{
    partial class SearchResultsForm
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
			this.ResultsGridView = new System.Windows.Forms.DataGridView();
			((System.ComponentModel.ISupportInitialize)(this.ResultsGridView)).BeginInit();
			this.SuspendLayout();
			// 
			// ResultsGridView
			// 
			this.ResultsGridView.AllowUserToAddRows = false;
			this.ResultsGridView.AllowUserToDeleteRows = false;
			this.ResultsGridView.AllowUserToResizeRows = false;
			this.ResultsGridView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.ResultsGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.ResultsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.ResultsGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.ResultsGridView.Location = new System.Drawing.Point(12, 12);
			this.ResultsGridView.Name = "ResultsGridView";
			this.ResultsGridView.RowHeadersVisible = false;
			this.ResultsGridView.Size = new System.Drawing.Size(881, 505);
			this.ResultsGridView.TabIndex = 0;
			this.ResultsGridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView_ColumnHeaderMouseClick);
			this.ResultsGridView.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.ResultsGridView_DataBindingComplete);
			// 
			// SearchResultsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(905, 529);
			this.Controls.Add(this.ResultsGridView);
			this.Name = "SearchResultsForm";
			this.Text = "Search Results";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SearchResultsForm_FormClosing);
			((System.ComponentModel.ISupportInitialize)(this.ResultsGridView)).EndInit();
			this.ResumeLayout(false);

        }

		#endregion

		private System.Windows.Forms.DataGridView ResultsGridView;
	}
}