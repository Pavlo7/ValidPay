using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace ValidPay
{
    public partial class Form1 : Form
    {
        CConfig config;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = string.Format("ValidPay.exe v{0}.{1}", Assembly.GetExecutingAssembly().GetName().Version.Major.ToString(), Assembly.GetExecutingAssembly().GetName().Version.Minor.ToString());
                config = new CConfig();
                if (config.bTC) toolStripStatusLabel1.Text = "Соединение с БД установлено";
                else toolStripStatusLabel1.Text = "Соединение с БД отсутствует";
            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void ToolStripMenuItemAbout_Click(object sender, EventArgs e)
        {
            AboutBox1 wnd = new AboutBox1();
            wnd.ShowDialog();
        }

        private void ToolStripMenuItemDataWebPayView_Click(object sender, EventArgs e)
        {

        }

        private void ToolStripMenuItemDataWebPayLoad_Click(object sender, EventArgs e)
        {
            LoadWebPayFile wnd = new LoadWebPayFile(config);
            wnd.ShowDialog();
        }

        private void ToolStripMenuItemLoadRcpData_Click(object sender, EventArgs e)
        {
            LoadRcpData wnd = new LoadRcpData(config);
            wnd.ShowDialog();
        }

        private void ToolStripMenuItemValid_Click(object sender, EventArgs e)
        {
            try
            {
                Valid wnd = new Valid(config);

                wnd.MdiParent = this;
                wnd.Show();
                wnd.Activate();
            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void ToolStripMenuItemLoadMTBankData_Click(object sender, EventArgs e)
        {
            LoadAssistFile wnd = new LoadAssistFile(config);
            wnd.ShowDialog();
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateContract wnd = new UpdateContract(config);
            wnd.ShowDialog();
        }

        private void ToolStripMenuItemLoadAssistCsv_Click(object sender, EventArgs e)
        {
            LoadAssistCsvFile wnd = new LoadAssistCsvFile(config);
            wnd.ShowDialog();
        }

        private void ToolStripMenuItemLoadBelWebBankData_Click(object sender, EventArgs e)
        {
           // LoadReestrBelWeb wnd = new LoadReestrBelWeb(config);
           // wnd.ShowDialog();
        }

        private void ToolStripMenuItemBelWebViewData_Click(object sender, EventArgs e)
        {
            try
            {
                BelWebViewData wnd = new BelWebViewData(config);

                wnd.MdiParent = this;
                wnd.Show();
                wnd.Activate();
            }
            catch (Exception ex) {  }
        }

        private void ToolStripMenuItemAssistViewData_Click(object sender, EventArgs e)
        {
            try
            {
                AssistViewData wnd = new AssistViewData(config);

                wnd.MdiParent = this;
                wnd.Show();
                wnd.Activate();
            }
            catch (Exception ex) { }
        }

        private void ToolStripMenuItemLoadPayment_Click(object sender, EventArgs e)
        {
            LoadPayment wnd = new LoadPayment(config);
            wnd.ShowDialog();
        }

        private void LoadDrivePayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadAmout300 wnd = new LoadAmout300(config);
            wnd.ShowDialog();
        }

        private void ToolStripMenuItemViewDataRCP_Click(object sender, EventArgs e)
        {
            try
            {
                RCPViewData wnd = new RCPViewData(config);

                wnd.MdiParent = this;
                wnd.Show();
                wnd.Activate();
            }
            catch (Exception ex) { }
        }

        private void ToolStripMenuItemOutReestrRCP_Click(object sender, EventArgs e)
        {
            OutReestrRcp wnd = new OutReestrRcp(config);
            wnd.ShowDialog();
        }

        private void ToolStripMenuItemLoadReestr_Click(object sender, EventArgs e)
        {
            LoadMobileAppData wnd = new LoadMobileAppData(config);
            wnd.ShowDialog();
        }

              
    }
}
