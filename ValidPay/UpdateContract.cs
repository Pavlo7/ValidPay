using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Data.OracleClient;
using System.Reflection;                // For Missing.Value and BindingFlags
using System.Runtime.InteropServices;   // For COMException
using Excel = Microsoft.Office.Interop.Excel;
using System.Data.SqlClient;

namespace ValidPay
{
    public struct STRowUpdateContract
    {
        public int contrcode;
        public string mdmcode;
    }

    public partial class UpdateContract : Form
    {
        public string FilePath;
        public string FileName;
        public string inpath;

        List<STRowUpdateContract> data;
        CConfig config;
        XLog log;

        CEmitent clEmt;
        List<STEmitent> lst;

        public UpdateContract(CConfig cf)
        {
            InitializeComponent();
            config = cf;
        }

        private void UpdateContract_Load(object sender, EventArgs e)
        {
            clEmt = new CEmitent(config);
            lst = clEmt.GetData(1);

            buttonOK.Enabled = false;
            buttonCancel.Enabled = false;

            log = new XLog();
            log.DirName = config.logpath;
            inpath = config.inpath;
        }


        private STEmitent get_emt(int emt)
        {
            STEmitent ret = new STEmitent();

            if (lst.Count <= 0) return ret;

            var results = from myRow in lst.AsEnumerable()
                          where myRow.code == emt
                          select myRow;

            foreach (var vr in results)
                ret = vr;
            return ret;

        }

        private bool read_file( out string msg)
        {
            msg = null;

            Excel._Application app = new Excel.Application();

            try
            {
              //  string FilePath = Path.Combine(inpath, FileName);

                Excel._Workbook book = app.Workbooks.Open(FilePath, Missing.Value, Missing.Value,
                       Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                       Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                       Missing.Value, Missing.Value);
                Excel.Sheets sheets = book.Worksheets;
                Excel._Worksheet sheet;

                sheet = (Excel._Worksheet)sheets.get_Item(1);

                Excel.Range range;

                data = new List<STRowUpdateContract>();

                int lastRow = sheet.UsedRange.Rows.Count;
                
                for (int i = 1; i <= lastRow; i++)
                {
                    range = sheet.get_Range(string.Format("A{0}", i), Missing.Value);
                    if (range.Value2 == null) continue;
                    string s = Convert.ToString(range.Value2).Trim();

                    if (!is_d(s)) continue;
                    else
                    {
                        STRowUpdateContract item = new STRowUpdateContract();
                       
                        range = sheet.get_Range(string.Format("A{0}", i), Missing.Value);
                        if (range.Value2 != null)
                        {
                            item.contrcode = Convert.ToInt32(range.Value2);
                        }
                        else item.contrcode = -1;

                        range = sheet.get_Range(string.Format("B{0}", i), Missing.Value);
                        if (range.Value2 != null)
                        {
                            item.mdmcode = Convert.ToString(range.Value2).Trim();
                        }
                        else item.mdmcode = null;

                        if ( item.contrcode > 0 && !string.IsNullOrEmpty(item.mdmcode))
                            data.Add(item);
                    }
                }
                app.Quit();
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                log.LogLine("File wasn't read.");
                return false;
            }
            finally { app.Quit(); }

            return true;
        }


        private bool load_file(object sender, DoWorkEventArgs e, out string msg, int emt)
        {
            msg = null;

            try
            {
                int i = 0;
                int cnt = data.Count;

                STEmitent st = get_emt(emt);
                string connectionstring = st.c_string;

                SqlConnection connect = new SqlConnection(connectionstring);
                connect.Open();
                if (connect.State != ConnectionState.Open) { msg = string.Format("No connection to DB. CS: {0}", connectionstring); return false; }

                backgroundWorker1.ReportProgress(0);

                string query = "UPDATE Contract SET CNSIPartCode=@1 WHERE ContrCode=@2";
                SqlCommand cmd;

                foreach (STRowUpdateContract item in data)
                {
                    cmd = new SqlCommand(query, connect);

                    cmd.Parameters.Add(crp(SqlDbType.Text, "@1", item.mdmcode, false));
                    cmd.Parameters.Add(crp(SqlDbType.Int, "@2", item.contrcode, false));

                    cmd.ExecuteNonQuery();

                    i += 1;
                    double d = ((i * 1.0) / cnt) * 100;
                    backgroundWorker1.ReportProgress((int)d);
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                log.LogLine("Neither record was written to database.");
                return false;
            }


            return true;
        }

        /* параметр */
        private SqlParameter crp(SqlDbType type, string pname, object val, bool isn)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = pname;
            param.SqlDbType = type;
            param.IsNullable = isn;
            if (val != null)
                param.Value = val;
            else
                param.Value = DBNull.Value;

            return param;
        }

        private bool is_d(string s)
        {
            bool ret = false;
            int x;
            if (int.TryParse(s, out x)) return true;

            return ret;
        }

        private void labelChange_Click(object sender, EventArgs e)
        {
            openFileDialog1.InitialDirectory = Path.GetFullPath(inpath);
            openFileDialog1.Filter = "All files (*.*)|*.*|Excel files (*.xls)|*.xls|Excel files (*.xlsx)|*.xlsx";

            if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return;

            DirectoryInfo DI = new DirectoryInfo(openFileDialog1.FileName);
            FilePath = DI.FullName;
            FileName = DI.Name;
            if (!string.IsNullOrEmpty(FilePath) && File.Exists(FilePath))
            {
                textBoxFile.Text = FilePath;
                buttonOK.Enabled = true;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            buttonOK.Enabled = false;
            buttonExit.Enabled = false;
            buttonCancel.Enabled = true;
            labelChange.Enabled = false;

            backgroundWorker1.RunWorkerAsync();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                //  labelInfo.Text = "Внимание! После обработки текущего файла процесс записи будет остановлен...";
                backgroundWorker1.CancelAsync();
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            StartProc(sender, e);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBarData.Value = e.ProgressPercentage;
            labelInfo.Text = "Processing.... " + progressBarData.Value.ToString() + "%";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                labelInfo.Text = "Запись файла прервана пользователем...";
            }

           // Check to see if an error occurred in the background process.

            else if (e.Error != null)
            {
                labelInfo.Text = "Error while performing background operation...";
            }
            else
            {
                // Everything completed normally.
                labelInfo.Text = "Запись файла завершена...";
            }

            buttonCancel.Enabled = false;
            buttonExit.Enabled = true;
            buttonOK.Enabled = true;
            labelChange.Enabled = true;
            
        }

        private void StartProc(object sender, DoWorkEventArgs e)
        {
            string msg = null;
            try
            {
                int emt;
                if (int.TryParse(textBoxEmt.Text.Trim(), out emt))
                {
                    if (!read_file(out msg)) { log.LogLine(msg); return; }
                    if (!load_file(sender, e, out  msg, emt)) log.LogLine(msg);
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }
    }
}
