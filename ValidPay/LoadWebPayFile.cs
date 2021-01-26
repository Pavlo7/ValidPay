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

namespace ValidPay
{
    public struct STRowWebPayData
    {
        public int idsys;
        public string rrn;
        public int idstatus;
        public DateTime ltime;
        public double amount;
        public string filename;
        public string tags;
    }


    public partial class LoadWebPayFile : Form
    {
        public string FilePath;
        public string FileName;
        public string inpath;

        List<STRowWebPayData> data;
        CConfig config;
        XLog log;

        List<string[]> filedata;

        public LoadWebPayFile(CConfig cf)
        {
            InitializeComponent();
            config = cf;
          
        }

        private void LoadWebPayFile_Load(object sender, EventArgs e)
        {
            buttonOK.Enabled = false;
            buttonCancel.Enabled = false;

            log = new XLog();
            log.DirName = config.logpath;
            inpath = config.inpath;
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

//        public string GetData() { return FilePath; }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            //LoadWebPayFileProcessing lwp = new LoadWebPayFileProcessing();
            //lwp.ShowDialog();

            buttonOK.Enabled = false;
            buttonExit.Enabled = false;
            buttonCancel.Enabled = true;
            labelChange.Enabled = false;

            backgroundWorker1.RunWorkerAsync();
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
            
         //   progressBarData.Value = 0;
         //   progressBarData.Refresh();


        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
              //  labelInfo.Text = "Внимание! После обработки текущего файла процесс записи будет остановлен...";
                backgroundWorker1.CancelAsync();
            }
        }

        private void StartProc(object sender, DoWorkEventArgs e)
        {
            string msg = null;
            try
            {
                if (!read_file(out msg)) { log.LogLine(msg); return; }
                if (!load_file(sender, e, out  msg)) log.LogLine(msg);
                
            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); } 
        }

        private bool read_file(out string msg)
        {
            msg = null;

            Excel._Application app = new Excel.Application();
            
            try
            {
                

                Excel._Workbook book = app.Workbooks.Open(FilePath, Missing.Value, Missing.Value,
                       Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                       Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                       Missing.Value, Missing.Value);
                Excel.Sheets sheets = book.Worksheets;
                Excel._Worksheet sheet;

                sheet = (Excel._Worksheet)sheets.get_Item(1);

                Excel.Range range;

            //    int i = 0;
                int row = 0;
                int countcol = 25;
                int pc = 0;

                string[] arr;
                string s;

                filedata = new List<string[]>();

                data = new List<STRowWebPayData>();


                // Получаем значения.
                // Найти последнюю ячейку в столбце.
                range = (Excel.Range)sheet.Columns[1, Type.Missing];
                Excel.Range last_cell =
                    range.get_End(Excel.XlDirection.xlDown);

                // Получить диапазон, содержащий значения.
                Excel.Range first_cell =
                    (Excel.Range)sheet.Cells[row + 2, countcol];
                Excel.Range value_range =
                    (Excel.Range)sheet.get_Range(first_cell, last_cell);

                // Получаем значения.
                object[,] range_values = (object[,])value_range.Value2;

                // Преобразуем это в одномерный массив.
                // Обратите внимание, что массив Range имеет нижние границы 1.
                int num_items = range_values.GetUpperBound(0);
                string[] values1 = new string[countcol];
                for (int i = 0; i < num_items; i++)
                {
                  //  values1 = new string[countcol];
                  //  for (int j = 0; j < countcol; j++)
                  //  {
                  //      values1[j] = range_values[i + 1, j + 1].ToString();
                  //  }
                  // filedata.Add(values1);

                  

                    STRowWebPayData item = new STRowWebPayData();

                    item.filename = FileName;
                    double s1 = (double)range_values[i + 1, 18];
                    double retsumm = (double)range_values[i + 1, 22]; 
                    item.amount = s1 - retsumm;
                    item.idstatus = 0;
                    item.idsys = 1;
                    item.ltime = DateTime.FromOADate((double)range_values[i + 1, 15]);
                    item.rrn = range_values[i + 1, 13].ToString();
                    item.tags = null;
                    item.tags += string.Format("<S1={0}>", s1);
                    item.tags += string.Format("<RS={0}>", retsumm);
                    
                    data.Add(item);
                  
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


        private bool load_file(object sender, DoWorkEventArgs e, out string msg)
        {
            msg = null;

            OracleConnection connection = new OracleConnection(config.connectionstring);

            connection.Open();
            
            
            OracleTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            OracleCommand command = connection.CreateCommand();

            // Start a local transaction
           // transaction = connection.BeginTransaction();
            // Assign transaction object for a pending local transaction
            command.Transaction = transaction;
            
            try
            {
                int pc = 0;
                int i = 0;
                int cnt = data.Count;

          //      command.CommandText = string.Format("DELETE FROM Rcd.CHECKWEBPAYDATA Where FILENAME='{0}'", FileName);
          //      command.ExecuteNonQuery();

                foreach (STRowWebPayData item in data)
                {
                    if (backgroundWorker1.CancellationPending)
                    {
                        transaction.Rollback();
                        e.Cancel = true;
                        backgroundWorker1.ReportProgress(0);
                        return false;
                    }

                    command.CommandText = "INSERT INTO Rcd.VALID_WEBPAYDATA (IDSYS, RRN, IDSTATUS, LTIME, AMOUNT, FILENAME, TAGS) values (:1, :2, :3, :4, :5, :6, :7)";
                    command.Parameters.Clear();
                    command.Parameters.Add(crp(OracleType.Int32, item.idsys, "1", false));
                    command.Parameters.Add(crp(OracleType.Char, item.rrn, "2", false));
                    command.Parameters.Add(crp(OracleType.Int32, item.idstatus, "3", false));
                    command.Parameters.Add(crp(OracleType.DateTime, item.ltime, "4", false));
                    command.Parameters.Add(crp(OracleType.Number, Math.Round(item.amount,2), "5", false));
                    command.Parameters.Add(crp(OracleType.Char, item.filename, "6", false));
                    command.Parameters.Add(crp(OracleType.Clob, item.tags, "7", false));
                    command.ExecuteNonQuery();

                    i+=1;
            //        pc = i / cnt * 100;
                    double d = ((i*1.0) / cnt) * 100;
              //      double f = Math.Round((double)(i / cnt * 100.0));
                    backgroundWorker1.ReportProgress((int)d);
                }

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                msg = ex.Message;
                log.LogLine("Neither record was written to database.");
                return false;
            }
          

            return true;
        }
         

        private void LoadWebPayFile_FormClosing(object sender, FormClosingEventArgs e)
        {

            if (backgroundWorker1.IsBusy)
            {
               // this.Text = "Внимание! После обработки текущего файла процесс записи будет остановлен...";
                e.Cancel = true;
                //      backgroundWorker1.CancelAsync();
            }
        }


        private OracleParameter crp(OracleType type, object val, string name, bool isn)
        {
            OracleParameter param = new OracleParameter();
            param.ParameterName = name;
            param.OracleType = type;
            param.IsNullable = isn;
            if (val == null)
                param.Value = DBNull.Value;
            else param.Value = val;

            return param;
        }
    }
}
