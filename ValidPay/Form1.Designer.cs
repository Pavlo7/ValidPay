namespace ValidPay
{
    partial class Form1
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ToolStripMenuItemData = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemRCPData = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemLoadRcpData = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemViewDataRCP = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripMenuItemOutReestrRCP = new System.Windows.Forms.ToolStripMenuItem();
            this.PayToolStripMenuItemDataWebPay = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemDataWebPayView = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemDataWebPayLoad = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemMTBankData = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemLoadMTBankData = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemLoadAssistCsv = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemAssistViewData = new System.Windows.Forms.ToolStripMenuItem();
            this.ОтToolStripMenuItemReestrBelWeb = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemLoadBelWebBankData = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemBelWebViewData = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.платежиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemLoadPayment = new System.Windows.Forms.ToolStripMenuItem();
            this.реализацияИзПЦToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LoadDrivePayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemValid = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.updateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripMenuItemLoadReestr = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 806);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1257, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemData,
            this.ToolStripMenuItemValid,
            this.ToolStripMenuItemAbout,
            this.updateToolStripMenuItem,
            this.ToolStripMenuItemLoadReestr});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1257, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ToolStripMenuItemData
            // 
            this.ToolStripMenuItemData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemRCPData,
            this.PayToolStripMenuItemDataWebPay,
            this.ToolStripMenuItemMTBankData,
            this.ОтToolStripMenuItemReestrBelWeb,
            this.toolStripSeparator1,
            this.платежиToolStripMenuItem,
            this.реализацияИзПЦToolStripMenuItem});
            this.ToolStripMenuItemData.Name = "ToolStripMenuItemData";
            this.ToolStripMenuItemData.Size = new System.Drawing.Size(62, 20);
            this.ToolStripMenuItemData.Text = "Данные";
            // 
            // ToolStripMenuItemRCPData
            // 
            this.ToolStripMenuItemRCPData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemLoadRcpData,
            this.ToolStripMenuItemViewDataRCP,
            this.toolStripSeparator2,
            this.ToolStripMenuItemOutReestrRCP});
            this.ToolStripMenuItemRCPData.Name = "ToolStripMenuItemRCPData";
            this.ToolStripMenuItemRCPData.Size = new System.Drawing.Size(178, 22);
            this.ToolStripMenuItemRCPData.Text = "Данные из РЦП";
            // 
            // ToolStripMenuItemLoadRcpData
            // 
            this.ToolStripMenuItemLoadRcpData.Name = "ToolStripMenuItemLoadRcpData";
            this.ToolStripMenuItemLoadRcpData.Size = new System.Drawing.Size(171, 22);
            this.ToolStripMenuItemLoadRcpData.Text = "Загрузить";
            this.ToolStripMenuItemLoadRcpData.Click += new System.EventHandler(this.ToolStripMenuItemLoadRcpData_Click);
            // 
            // ToolStripMenuItemViewDataRCP
            // 
            this.ToolStripMenuItemViewDataRCP.Name = "ToolStripMenuItemViewDataRCP";
            this.ToolStripMenuItemViewDataRCP.Size = new System.Drawing.Size(171, 22);
            this.ToolStripMenuItemViewDataRCP.Text = "Просмотр";
            this.ToolStripMenuItemViewDataRCP.Click += new System.EventHandler(this.ToolStripMenuItemViewDataRCP_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(168, 6);
            // 
            // ToolStripMenuItemOutReestrRCP
            // 
            this.ToolStripMenuItemOutReestrRCP.Name = "ToolStripMenuItemOutReestrRCP";
            this.ToolStripMenuItemOutReestrRCP.Size = new System.Drawing.Size(171, 22);
            this.ToolStripMenuItemOutReestrRCP.Text = "Выгрузить реестр";
            this.ToolStripMenuItemOutReestrRCP.Click += new System.EventHandler(this.ToolStripMenuItemOutReestrRCP_Click);
            // 
            // PayToolStripMenuItemDataWebPay
            // 
            this.PayToolStripMenuItemDataWebPay.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemDataWebPayView,
            this.ToolStripMenuItemDataWebPayLoad});
            this.PayToolStripMenuItemDataWebPay.Name = "PayToolStripMenuItemDataWebPay";
            this.PayToolStripMenuItemDataWebPay.Size = new System.Drawing.Size(178, 22);
            this.PayToolStripMenuItemDataWebPay.Text = "Данные от WebPay";
            this.PayToolStripMenuItemDataWebPay.Visible = false;
            // 
            // ToolStripMenuItemDataWebPayView
            // 
            this.ToolStripMenuItemDataWebPayView.Name = "ToolStripMenuItemDataWebPayView";
            this.ToolStripMenuItemDataWebPayView.Size = new System.Drawing.Size(131, 22);
            this.ToolStripMenuItemDataWebPayView.Text = "Просмотр";
            this.ToolStripMenuItemDataWebPayView.Click += new System.EventHandler(this.ToolStripMenuItemDataWebPayView_Click);
            // 
            // ToolStripMenuItemDataWebPayLoad
            // 
            this.ToolStripMenuItemDataWebPayLoad.Name = "ToolStripMenuItemDataWebPayLoad";
            this.ToolStripMenuItemDataWebPayLoad.Size = new System.Drawing.Size(131, 22);
            this.ToolStripMenuItemDataWebPayLoad.Text = "Загрузить";
            this.ToolStripMenuItemDataWebPayLoad.Click += new System.EventHandler(this.ToolStripMenuItemDataWebPayLoad_Click);
            // 
            // ToolStripMenuItemMTBankData
            // 
            this.ToolStripMenuItemMTBankData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemLoadMTBankData,
            this.ToolStripMenuItemLoadAssistCsv,
            this.ToolStripMenuItemAssistViewData});
            this.ToolStripMenuItemMTBankData.Name = "ToolStripMenuItemMTBankData";
            this.ToolStripMenuItemMTBankData.Size = new System.Drawing.Size(178, 22);
            this.ToolStripMenuItemMTBankData.Text = "Данные от Assist";
            // 
            // ToolStripMenuItemLoadMTBankData
            // 
            this.ToolStripMenuItemLoadMTBankData.Name = "ToolStripMenuItemLoadMTBankData";
            this.ToolStripMenuItemLoadMTBankData.Size = new System.Drawing.Size(174, 22);
            this.ToolStripMenuItemLoadMTBankData.Text = "Загрузить";
            this.ToolStripMenuItemLoadMTBankData.Click += new System.EventHandler(this.ToolStripMenuItemLoadMTBankData_Click);
            // 
            // ToolStripMenuItemLoadAssistCsv
            // 
            this.ToolStripMenuItemLoadAssistCsv.Name = "ToolStripMenuItemLoadAssistCsv";
            this.ToolStripMenuItemLoadAssistCsv.Size = new System.Drawing.Size(174, 22);
            this.ToolStripMenuItemLoadAssistCsv.Text = "Загрузить (csv)";
            this.ToolStripMenuItemLoadAssistCsv.Click += new System.EventHandler(this.ToolStripMenuItemLoadAssistCsv_Click);
            // 
            // ToolStripMenuItemAssistViewData
            // 
            this.ToolStripMenuItemAssistViewData.Name = "ToolStripMenuItemAssistViewData";
            this.ToolStripMenuItemAssistViewData.Size = new System.Drawing.Size(174, 22);
            this.ToolStripMenuItemAssistViewData.Text = "Просмотр данных";
            this.ToolStripMenuItemAssistViewData.Click += new System.EventHandler(this.ToolStripMenuItemAssistViewData_Click);
            // 
            // ОтToolStripMenuItemReestrBelWeb
            // 
            this.ОтToolStripMenuItemReestrBelWeb.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemLoadBelWebBankData,
            this.ToolStripMenuItemBelWebViewData});
            this.ОтToolStripMenuItemReestrBelWeb.Name = "ОтToolStripMenuItemReestrBelWeb";
            this.ОтToolStripMenuItemReestrBelWeb.Size = new System.Drawing.Size(178, 22);
            this.ОтToolStripMenuItemReestrBelWeb.Text = "Реестр БЕЛВЭБ";
            // 
            // ToolStripMenuItemLoadBelWebBankData
            // 
            this.ToolStripMenuItemLoadBelWebBankData.Name = "ToolStripMenuItemLoadBelWebBankData";
            this.ToolStripMenuItemLoadBelWebBankData.Size = new System.Drawing.Size(174, 22);
            this.ToolStripMenuItemLoadBelWebBankData.Text = "Загрузить";
            this.ToolStripMenuItemLoadBelWebBankData.Click += new System.EventHandler(this.ToolStripMenuItemLoadBelWebBankData_Click);
            // 
            // ToolStripMenuItemBelWebViewData
            // 
            this.ToolStripMenuItemBelWebViewData.Name = "ToolStripMenuItemBelWebViewData";
            this.ToolStripMenuItemBelWebViewData.Size = new System.Drawing.Size(174, 22);
            this.ToolStripMenuItemBelWebViewData.Text = "Просмотр данных";
            this.ToolStripMenuItemBelWebViewData.Click += new System.EventHandler(this.ToolStripMenuItemBelWebViewData_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(175, 6);
            // 
            // платежиToolStripMenuItem
            // 
            this.платежиToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripMenuItemLoadPayment});
            this.платежиToolStripMenuItem.Name = "платежиToolStripMenuItem";
            this.платежиToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.платежиToolStripMenuItem.Text = "Платежи";
            // 
            // ToolStripMenuItemLoadPayment
            // 
            this.ToolStripMenuItemLoadPayment.Name = "ToolStripMenuItemLoadPayment";
            this.ToolStripMenuItemLoadPayment.Size = new System.Drawing.Size(128, 22);
            this.ToolStripMenuItemLoadPayment.Text = "Загрузить";
            this.ToolStripMenuItemLoadPayment.Click += new System.EventHandler(this.ToolStripMenuItemLoadPayment_Click);
            // 
            // реализацияИзПЦToolStripMenuItem
            // 
            this.реализацияИзПЦToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.LoadDrivePayToolStripMenuItem});
            this.реализацияИзПЦToolStripMenuItem.Name = "реализацияИзПЦToolStripMenuItem";
            this.реализацияИзПЦToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.реализацияИзПЦToolStripMenuItem.Text = "Реализация из ПЦ";
            // 
            // LoadDrivePayToolStripMenuItem
            // 
            this.LoadDrivePayToolStripMenuItem.Name = "LoadDrivePayToolStripMenuItem";
            this.LoadDrivePayToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.LoadDrivePayToolStripMenuItem.Text = "Загрузить Drive&Pay";
            this.LoadDrivePayToolStripMenuItem.Click += new System.EventHandler(this.LoadDrivePayToolStripMenuItem_Click);
            // 
            // ToolStripMenuItemValid
            // 
            this.ToolStripMenuItemValid.Name = "ToolStripMenuItemValid";
            this.ToolStripMenuItemValid.Size = new System.Drawing.Size(58, 20);
            this.ToolStripMenuItemValid.Text = "Сверка";
            this.ToolStripMenuItemValid.Click += new System.EventHandler(this.ToolStripMenuItemValid_Click);
            // 
            // ToolStripMenuItemAbout
            // 
            this.ToolStripMenuItemAbout.Name = "ToolStripMenuItemAbout";
            this.ToolStripMenuItemAbout.Size = new System.Drawing.Size(94, 20);
            this.ToolStripMenuItemAbout.Text = "О программе";
            this.ToolStripMenuItemAbout.Click += new System.EventHandler(this.ToolStripMenuItemAbout_Click);
            // 
            // updateToolStripMenuItem
            // 
            this.updateToolStripMenuItem.Name = "updateToolStripMenuItem";
            this.updateToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.updateToolStripMenuItem.Text = "Update";
            this.updateToolStripMenuItem.Visible = false;
            this.updateToolStripMenuItem.Click += new System.EventHandler(this.updateToolStripMenuItem_Click);
            // 
            // ToolStripMenuItemLoadReestr
            // 
            this.ToolStripMenuItemLoadReestr.Name = "ToolStripMenuItemLoadReestr";
            this.ToolStripMenuItemLoadReestr.Size = new System.Drawing.Size(122, 20);
            this.ToolStripMenuItemLoadReestr.Text = "Загрузить реестры";
            this.ToolStripMenuItemLoadReestr.Click += new System.EventHandler(this.ToolStripMenuItemLoadReestr_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1257, 828);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAbout;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemData;
        private System.Windows.Forms.ToolStripMenuItem PayToolStripMenuItemDataWebPay;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDataWebPayView;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemDataWebPayLoad;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemRCPData;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemLoadRcpData;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemValid;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemMTBankData;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemLoadMTBankData;
        private System.Windows.Forms.ToolStripMenuItem updateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemLoadAssistCsv;
        private System.Windows.Forms.ToolStripMenuItem ОтToolStripMenuItemReestrBelWeb;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemLoadBelWebBankData;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemBelWebViewData;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemAssistViewData;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem платежиToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemLoadPayment;
        private System.Windows.Forms.ToolStripMenuItem реализацияИзПЦToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem LoadDrivePayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemViewDataRCP;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemOutReestrRCP;
        private System.Windows.Forms.ToolStripMenuItem ToolStripMenuItemLoadReestr;
    }
}

