using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace ValidPay
{
    public partial class StatmetProc : Form
    {
        int iMsec;
        public string sTime;
        bool bClose;

        object oParam;
        object oClass;
        public object oData;
        bool bParam;
        object oParam1;

        bool bOper;

        public StatmetProc(object Class, object Param, bool bp)
        {
            InitializeComponent();
            iMsec = 1;
            sTime = string.Format("{0:00} мсек", iMsec);

            oClass = Class;
            oParam = Param;
            bParam = bp;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                iMsec++;
                print_time();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.Source); }
        }

        [Obsolete]
        private void StatmetProc_Load(object sender, EventArgs e)
        {
            try
            {
                this.ControlBox = false;

                bOper = false;

                timer1.Start();

                Thread mThread = new Thread(start: Execute);
                mThread.Start();

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.Source); }
        }

        private void print_time()
        {
            string sPrint;

            int iminute = 0;
            int isecond = 0;

            try
            {
                iminute = iMsec / 600;
                if (iminute <= 0)
                    isecond = iMsec / 10;
                else isecond = (iMsec % 600) / 10;

                if (iMsec < 10)
                    sTime = string.Format("{0:00} msec", iMsec);
                else if (iminute > 0)
                    sTime = string.Format("{0:00}:{1:00} min", iminute, isecond);
                else sTime = string.Format("{0:00} sec", isecond);

                sPrint = string.Format("Runtime: {0}", sTime);
                labelLabel.Text = sPrint;

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.Source); }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            try
            {
                timer1.Stop();
                bClose = false;
                DialogResult = DialogResult.Cancel;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.Source); }
        }

        private void StatmetProc_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = bClose;
        }

        [Obsolete]
        private void Execute()
        {
            string msg;
            int retcode;

            try
            {
                if (oClass is CValidData)
                {
                    CValidData g_clBook = (CValidData)oClass;
                    //      List<STValidData> g_listResult;

                    //  oData =  g_clBook.GetData((STValidDataParam)oParam);
                    DataTable table;
                    g_clBook.GetTable((STValidDataParam)oParam, out table, out msg);

                    oData = table;
                      bOper = true;
                    timer1.Stop();
                    bClose = false;
                    DialogResult = DialogResult.OK;
                }

                if (oClass is CBelWebData)
                {
                    DataTable table;
                    CBelWebData g_clBook = (CBelWebData)oClass;
                    //      List<STValidData> g_listResult;

                    retcode = g_clBook.GetTable((STVPBelWebData)oParam, out table, out msg);

                    if (retcode == 0)
                    {
                        oData = table;
                    }
                    else
                        MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    bOper = true;
                    timer1.Stop();
                    bClose = false;
                    DialogResult = DialogResult.OK;
                }

                if (oClass is CAssistData)
                {
                    DataTable table;
                    CAssistData g_clBook = (CAssistData)oClass;
                    //      List<STValidData> g_listResult;

                    retcode = g_clBook.GetTable((STVPAssistData)oParam, out table, out msg);

                    if (retcode == 0)
                    {
                        oData = table;
                    }
                    else
                        MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    bOper = true;
                    timer1.Stop();
                    bClose = false;
                    DialogResult = DialogResult.OK;
                }

                if (oClass is CRcpData)
                {
                    DataTable table;
                    CRcpData g_clBook = (CRcpData)oClass;
                    //      List<STValidData> g_listResult;

                    retcode = g_clBook.GetTable((STVPRCPData)oParam, out table, out msg);

                    if (retcode == 0)
                    {
                        oData = table;
                    }
                    else
                        MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    bOper = true;
                    timer1.Stop();
                    bClose = false;
                    DialogResult = DialogResult.OK;
                }
             
             

                timer1.Stop();
                bClose = false;
                DialogResult = DialogResult.OK;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.Source); timer1.Stop(); }
        }
    }
}
