namespace ValidPay
{
    partial class LoadMobileAppData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoadMobileAppData));
            this.listViewData = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.checkBoxMove = new System.Windows.Forms.CheckBox();
            this.checkBoxReload = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonExit = new System.Windows.Forms.Button();
            this.progressBarData = new System.Windows.Forms.ProgressBar();
            this.buttonOK = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.labelInfo = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxApp = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // listViewData
            // 
            this.listViewData.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.listViewData.GridLines = true;
            this.listViewData.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewData.HideSelection = false;
            this.listViewData.Location = new System.Drawing.Point(12, 73);
            this.listViewData.Name = "listViewData";
            this.listViewData.Size = new System.Drawing.Size(532, 351);
            this.listViewData.SmallImageList = this.imageList;
            this.listViewData.TabIndex = 27;
            this.listViewData.UseCompatibleStateImageBehavior = false;
            this.listViewData.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Файл";
            this.columnHeader1.Width = 300;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Инфо";
            this.columnHeader2.Width = 450;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "1485481257-48_78629.png");
            this.imageList.Images.SetKeyName(1, "file_del_22031.png");
            this.imageList.Images.SetKeyName(2, "file-complete256_25223.png");
            this.imageList.Images.SetKeyName(3, "-file-upload_90320.png");
            // 
            // checkBoxMove
            // 
            this.checkBoxMove.AutoSize = true;
            this.checkBoxMove.Location = new System.Drawing.Point(12, 462);
            this.checkBoxMove.Name = "checkBoxMove";
            this.checkBoxMove.Size = new System.Drawing.Size(123, 17);
            this.checkBoxMove.TabIndex = 33;
            this.checkBoxMove.Text = "move files to archive";
            this.checkBoxMove.UseVisualStyleBackColor = true;
            // 
            // checkBoxReload
            // 
            this.checkBoxReload.AutoSize = true;
            this.checkBoxReload.Location = new System.Drawing.Point(12, 439);
            this.checkBoxReload.Name = "checkBoxReload";
            this.checkBoxReload.Size = new System.Drawing.Size(90, 17);
            this.checkBoxReload.TabIndex = 32;
            this.checkBoxReload.Text = "overwrite files";
            this.checkBoxReload.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Location = new System.Drawing.Point(12, 491);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(532, 10);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            // 
            // buttonExit
            // 
            this.buttonExit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonExit.Location = new System.Drawing.Point(469, 558);
            this.buttonExit.Name = "buttonExit";
            this.buttonExit.Size = new System.Drawing.Size(75, 23);
            this.buttonExit.TabIndex = 37;
            this.buttonExit.Text = "Exit";
            this.buttonExit.UseVisualStyleBackColor = true;
            this.buttonExit.Click += new System.EventHandler(this.buttonExit_Click);
            // 
            // progressBarData
            // 
            this.progressBarData.ForeColor = System.Drawing.Color.MediumOrchid;
            this.progressBarData.Location = new System.Drawing.Point(12, 529);
            this.progressBarData.Name = "progressBarData";
            this.progressBarData.Size = new System.Drawing.Size(532, 23);
            this.progressBarData.TabIndex = 36;
            // 
            // buttonOK
            // 
            this.buttonOK.Location = new System.Drawing.Point(12, 558);
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.Size = new System.Drawing.Size(75, 23);
            this.buttonOK.TabIndex = 35;
            this.buttonOK.Text = "Load";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonCancel.Location = new System.Drawing.Point(124, 558);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 23);
            this.buttonCancel.TabIndex = 34;
            this.buttonCancel.Text = "Break";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.WorkerSupportsCancellation = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Location = new System.Drawing.Point(12, 510);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(13, 13);
            this.labelInfo.TabIndex = 38;
            this.labelInfo.Text = "_";
            // 
            // groupBox2
            // 
            this.groupBox2.Location = new System.Drawing.Point(12, 57);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(532, 10);
            this.groupBox2.TabIndex = 32;
            this.groupBox2.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 39;
            this.label1.Text = "Select application";
            // 
            // comboBoxApp
            // 
            this.comboBoxApp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxApp.FormattingEnabled = true;
            this.comboBoxApp.Location = new System.Drawing.Point(15, 25);
            this.comboBoxApp.Name = "comboBoxApp";
            this.comboBoxApp.Size = new System.Drawing.Size(184, 21);
            this.comboBoxApp.TabIndex = 40;
            this.comboBoxApp.SelectedIndexChanged += new System.EventHandler(this.comboBoxApp_SelectedIndexChanged);
            // 
            // LoadMobileAppData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 588);
            this.Controls.Add(this.comboBoxApp);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.buttonExit);
            this.Controls.Add(this.progressBarData);
            this.Controls.Add(this.buttonOK);
            this.Controls.Add(this.buttonCancel);
            this.Controls.Add(this.checkBoxMove);
            this.Controls.Add(this.checkBoxReload);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listViewData);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "LoadMobileAppData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Load AppMobile Data";
            this.Load += new System.EventHandler(this.LoadMobileAppData_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView listViewData;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.CheckBox checkBoxMove;
        private System.Windows.Forms.CheckBox checkBoxReload;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonExit;
        private System.Windows.Forms.ProgressBar progressBarData;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.Label labelInfo;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxApp;
    }
}