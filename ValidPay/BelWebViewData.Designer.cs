namespace ValidPay
{
    partial class BelWebViewData
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ExcelToolStripMenuItemExportToExcelMenu = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelRow = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelProbel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelPC = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBarData = new System.Windows.Forms.ToolStripProgressBar();
            this.backgroundWorkerGrid = new System.ComponentModel.BackgroundWorker();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonApplay = new System.Windows.Forms.Button();
            this.dataGridViewBT = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ExcelToolStripMenuItemExportToExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBT)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExcelToolStripMenuItemExportToExcelMenu});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1023, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // ExcelToolStripMenuItemExportToExcelMenu
            // 
            this.ExcelToolStripMenuItemExportToExcelMenu.Name = "ExcelToolStripMenuItemExportToExcelMenu";
            this.ExcelToolStripMenuItemExportToExcelMenu.Size = new System.Drawing.Size(102, 20);
            this.ExcelToolStripMenuItemExportToExcelMenu.Text = "Экспорт в Excel";
            this.ExcelToolStripMenuItemExportToExcelMenu.Click += new System.EventHandler(this.ExcelToolStripMenuItemExportToExcelMenu_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelTime,
            this.toolStripStatusLabelRow,
            this.toolStripStatusLabelProbel,
            this.toolStripStatusLabelPC,
            this.toolStripProgressBarData});
            this.statusStrip1.Location = new System.Drawing.Point(0, 487);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1023, 22);
            this.statusStrip1.TabIndex = 28;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelTime
            // 
            this.toolStripStatusLabelTime.BorderSides = System.Windows.Forms.ToolStripStatusLabelBorderSides.Right;
            this.toolStripStatusLabelTime.Name = "toolStripStatusLabelTime";
            this.toolStripStatusLabelTime.Size = new System.Drawing.Size(4, 17);
            // 
            // toolStripStatusLabelRow
            // 
            this.toolStripStatusLabelRow.Name = "toolStripStatusLabelRow";
            this.toolStripStatusLabelRow.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabelProbel
            // 
            this.toolStripStatusLabelProbel.BorderSides = ((System.Windows.Forms.ToolStripStatusLabelBorderSides)((System.Windows.Forms.ToolStripStatusLabelBorderSides.Left | System.Windows.Forms.ToolStripStatusLabelBorderSides.Right)));
            this.toolStripStatusLabelProbel.Name = "toolStripStatusLabelProbel";
            this.toolStripStatusLabelProbel.Size = new System.Drawing.Size(902, 17);
            this.toolStripStatusLabelProbel.Spring = true;
            // 
            // toolStripStatusLabelPC
            // 
            this.toolStripStatusLabelPC.Name = "toolStripStatusLabelPC";
            this.toolStripStatusLabelPC.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripProgressBarData
            // 
            this.toolStripProgressBarData.Name = "toolStripProgressBarData";
            this.toolStripProgressBarData.Size = new System.Drawing.Size(100, 16);
            // 
            // backgroundWorkerGrid
            // 
            this.backgroundWorkerGrid.WorkerReportsProgress = true;
            this.backgroundWorkerGrid.WorkerSupportsCancellation = true;
            this.backgroundWorkerGrid.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerGrid_DoWork);
            this.backgroundWorkerGrid.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerGrid_ProgressChanged);
            this.backgroundWorkerGrid.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerGrid_RunWorkerCompleted);
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.buttonApplay);
            this.splitContainer1.Panel1MinSize = 30;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridViewBT);
            this.splitContainer1.Size = new System.Drawing.Size(1023, 487);
            this.splitContainer1.SplitterDistance = 224;
            this.splitContainer1.SplitterWidth = 2;
            this.splitContainer1.TabIndex = 29;
            // 
            // buttonApplay
            // 
            this.buttonApplay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonApplay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonApplay.Location = new System.Drawing.Point(130, 446);
            this.buttonApplay.Name = "buttonApplay";
            this.buttonApplay.Size = new System.Drawing.Size(78, 23);
            this.buttonApplay.TabIndex = 9;
            this.buttonApplay.Text = "&Apply";
            this.buttonApplay.UseVisualStyleBackColor = true;
            // 
            // dataGridViewBT
            // 
            this.dataGridViewBT.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridViewBT.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataGridViewBT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBT.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridViewBT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewBT.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewBT.MultiSelect = false;
            this.dataGridViewBT.Name = "dataGridViewBT";
            this.dataGridViewBT.ReadOnly = true;
            this.dataGridViewBT.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewBT.Size = new System.Drawing.Size(795, 485);
            this.dataGridViewBT.TabIndex = 10;
            this.dataGridViewBT.SelectionChanged += new System.EventHandler(this.dataGridViewBT_SelectionChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExcelToolStripMenuItemExportToExcel});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(158, 26);
            // 
            // ExcelToolStripMenuItemExportToExcel
            // 
            this.ExcelToolStripMenuItemExportToExcel.Name = "ExcelToolStripMenuItemExportToExcel";
            this.ExcelToolStripMenuItemExportToExcel.Size = new System.Drawing.Size(157, 22);
            this.ExcelToolStripMenuItemExportToExcel.Text = "Экспорт в Excel";
            this.ExcelToolStripMenuItemExportToExcel.Click += new System.EventHandler(this.ExcelToolStripMenuItemExportToExcel_Click);
            // 
            // BelWebViewData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1023, 509);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "BelWebViewData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "View BelWeb Data";
            this.Load += new System.EventHandler(this.BelWebViewData_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBT)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelTime;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelRow;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelProbel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelPC;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBarData;
        private System.ComponentModel.BackgroundWorker backgroundWorkerGrid;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Button buttonApplay;
        private System.Windows.Forms.DataGridView dataGridViewBT;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ExcelToolStripMenuItemExportToExcelMenu;
        private System.Windows.Forms.ToolStripMenuItem ExcelToolStripMenuItemExportToExcel;
    }
}