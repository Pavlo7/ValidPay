namespace ValidPay
{
    partial class Valid
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelRow = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelProbel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelPC = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBarData = new System.Windows.Forms.ToolStripProgressBar();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItemBookTransact = new System.Windows.Forms.ToolStripMenuItem();
            this.экспортВExcelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.backgroundWorkerGrid = new System.ComponentModel.BackgroundWorker();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.checkBoxVO = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxApp = new System.Windows.Forms.ComboBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.buttonPermit = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dateTimePickerDE = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerDB = new System.Windows.Forms.DateTimePicker();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridViewValid = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ExcelToolStripMenuItemExcel = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewValid)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelTime,
            this.toolStripStatusLabelRow,
            this.toolStripStatusLabelProbel,
            this.toolStripStatusLabelPC,
            this.toolStripProgressBarData});
            this.statusStrip1.Location = new System.Drawing.Point(0, 602);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1232, 22);
            this.statusStrip1.TabIndex = 29;
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
            this.toolStripStatusLabelProbel.Size = new System.Drawing.Size(1111, 17);
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
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemBookTransact});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(1232, 24);
            this.menuStrip.TabIndex = 28;
            this.menuStrip.Text = "menuStrip1";
            this.menuStrip.Visible = false;
            // 
            // ToolStripMenuItemBookTransact
            // 
            this.ToolStripMenuItemBookTransact.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.экспортВExcelToolStripMenuItem});
            this.ToolStripMenuItemBookTransact.Name = "ToolStripMenuItemBookTransact";
            this.ToolStripMenuItemBookTransact.Size = new System.Drawing.Size(70, 20);
            this.ToolStripMenuItemBookTransact.Text = "Valid data";
            // 
            // экспортВExcelToolStripMenuItem
            // 
            this.экспортВExcelToolStripMenuItem.Name = "экспортВExcelToolStripMenuItem";
            this.экспортВExcelToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.экспортВExcelToolStripMenuItem.Text = "Export to Excel";
            this.экспортВExcelToolStripMenuItem.Click += new System.EventHandler(this.экспортВExcelToolStripMenuItem_Click);
            // 
            // backgroundWorkerGrid
            // 
            this.backgroundWorkerGrid.WorkerReportsProgress = true;
            this.backgroundWorkerGrid.WorkerSupportsCancellation = true;
            this.backgroundWorkerGrid.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorkerGrid_DoWork);
            this.backgroundWorkerGrid.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorkerGrid_ProgressChanged);
            this.backgroundWorkerGrid.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorkerGrid_RunWorkerCompleted);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(218, 602);
            this.panel1.TabIndex = 30;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Controls.Add(this.buttonPermit);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Location = new System.Drawing.Point(5, 2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(202, 297);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.checkBoxVO);
            this.groupBox4.Controls.Add(this.label1);
            this.groupBox4.Controls.Add(this.comboBoxApp);
            this.groupBox4.Controls.Add(this.radioButton2);
            this.groupBox4.Controls.Add(this.radioButton1);
            this.groupBox4.Location = new System.Drawing.Point(4, 94);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(185, 158);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Options";
            // 
            // checkBoxVO
            // 
            this.checkBoxVO.AutoSize = true;
            this.checkBoxVO.Location = new System.Drawing.Point(14, 126);
            this.checkBoxVO.Name = "checkBoxVO";
            this.checkBoxVO.Size = new System.Drawing.Size(138, 17);
            this.checkBoxVO.TabIndex = 43;
            this.checkBoxVO.Text = "view only discrepancies";
            this.checkBoxVO.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 42;
            this.label1.Text = "Select application";
            // 
            // comboBoxApp
            // 
            this.comboBoxApp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxApp.FormattingEnabled = true;
            this.comboBoxApp.Location = new System.Drawing.Point(14, 88);
            this.comboBoxApp.Name = "comboBoxApp";
            this.comboBoxApp.Size = new System.Drawing.Size(154, 21);
            this.comboBoxApp.TabIndex = 41;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(14, 42);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(145, 17);
            this.radioButton2.TabIndex = 1;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "App Mobele to RCP Data";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(14, 19);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(145, 17);
            this.radioButton1.TabIndex = 0;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "RCP Data to App Mobele";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // buttonPermit
            // 
            this.buttonPermit.Location = new System.Drawing.Point(114, 258);
            this.buttonPermit.Name = "buttonPermit";
            this.buttonPermit.Size = new System.Drawing.Size(75, 23);
            this.buttonPermit.TabIndex = 2;
            this.buttonPermit.Text = "Search";
            this.buttonPermit.UseVisualStyleBackColor = true;
            this.buttonPermit.Click += new System.EventHandler(this.buttonPermit_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dateTimePickerDE);
            this.groupBox2.Controls.Add(this.dateTimePickerDB);
            this.groupBox2.Location = new System.Drawing.Point(7, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(182, 74);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Timespan";
            // 
            // dateTimePickerDE
            // 
            this.dateTimePickerDE.Location = new System.Drawing.Point(11, 45);
            this.dateTimePickerDE.Name = "dateTimePickerDE";
            this.dateTimePickerDE.Size = new System.Drawing.Size(154, 20);
            this.dateTimePickerDE.TabIndex = 3;
            // 
            // dateTimePickerDB
            // 
            this.dateTimePickerDB.Location = new System.Drawing.Point(11, 19);
            this.dateTimePickerDB.Name = "dateTimePickerDB";
            this.dateTimePickerDB.Size = new System.Drawing.Size(154, 20);
            this.dateTimePickerDB.TabIndex = 2;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridViewValid);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(218, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1014, 602);
            this.panel2.TabIndex = 31;
            // 
            // dataGridViewValid
            // 
            this.dataGridViewValid.BackgroundColor = System.Drawing.SystemColors.Control;
            this.dataGridViewValid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewValid.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridViewValid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewValid.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewValid.MultiSelect = false;
            this.dataGridViewValid.Name = "dataGridViewValid";
            this.dataGridViewValid.ReadOnly = true;
            this.dataGridViewValid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewValid.Size = new System.Drawing.Size(1014, 602);
            this.dataGridViewValid.TabIndex = 38;
            this.dataGridViewValid.SelectionChanged += new System.EventHandler(this.dataGridViewValid_SelectionChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ExcelToolStripMenuItemExcel});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(151, 26);
            // 
            // ExcelToolStripMenuItemExcel
            // 
            this.ExcelToolStripMenuItemExcel.Name = "ExcelToolStripMenuItemExcel";
            this.ExcelToolStripMenuItemExcel.Size = new System.Drawing.Size(150, 22);
            this.ExcelToolStripMenuItemExcel.Text = "Export to Excel";
            this.ExcelToolStripMenuItemExcel.Click += new System.EventHandler(this.ExcelToolStripMenuItemExcel_Click);
            // 
            // Valid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1232, 624);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip);
            this.Name = "Valid";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Valid data";
            this.Load += new System.EventHandler(this.Valid_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewValid)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelTime;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelRow;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelProbel;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelPC;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBarData;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemBookTransact;
        private System.ComponentModel.BackgroundWorker backgroundWorkerGrid;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridViewValid;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonPermit;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DateTimePicker dateTimePickerDE;
        private System.Windows.Forms.DateTimePicker dateTimePickerDB;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ToolStripMenuItem экспортВExcelToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ExcelToolStripMenuItemExcel;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.ComboBox comboBoxApp;
        private System.Windows.Forms.CheckBox checkBoxVO;
        private System.Windows.Forms.Label label1;
    }
}