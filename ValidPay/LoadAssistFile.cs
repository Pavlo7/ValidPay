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
using System.IO.Compression;


namespace ValidPay
{
    public struct STRowAssistData
    {
        public string ordernumber;
        public int orderstate;
        public DateTime ltime;
        public double amount;
        public string cardnumber;
        public string bank;
        public string filename;
        public string tags;
        public double orderamount;
        public DateTime orderdate;
        public int type;
    }

    public partial class LoadAssistFile : Form
    {
     //   public string FilePath;
     //   public string FileName;
        public string inpath;

        List<STRowAssistData> data;
        CConfig config;
        XLog log;

        List<string> lst_loadedfile;

        List<string[]> filedata;
        string[] files;

        List<STFileData> lst_filedata;

        STFileData curr_file;

        bool reloadflag;
        bool moveflag;

        public LoadAssistFile(CConfig cf)
        {
            InitializeComponent();
            config = cf;
        }

        private void LoadMTBankFile_Load(object sender, EventArgs e)
        {
            buttonOK.Enabled = false;
            buttonCancel.Enabled = false;

            log = new XLog();
            log.DirName = config.logpath;
            inpath = Path.GetFullPath(config.inpathassist);

            labelDIR.Text = inpath;
            
            lst_loadedfile = get_loaded_file();
            init_filedata();

            if (files.Length > 0)
                buttonOK.Enabled = true;

            if (config.reloadflag == 1) checkBoxReload.Checked = true;
            else checkBoxReload.Checked = false;

            if (config.moveflag == 1) checkBoxMove.Checked = true;
            else checkBoxMove.Checked = false;
        }

        private void buttonChangeDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                inpath = folderBrowserDialog1.SelectedPath;
                labelDIR.Text = inpath;
                init_list();
                //buttonOK.Enabled = true;
            }
        }

        private void init_filedata()
        {
            try
            {
                lst_filedata = new List<STFileData>();
                STFileData item;
                files = Directory.GetFiles(inpath); // путь к папке
                foreach (string path in files)
                {
                    item = new STFileData();
                    item.filename = Path.GetFileName(path);
                    item.status = 0;
                    if (is_loaded(item.filename)) item.status = 2;

                    lst_filedata.Add(item);
                    // checkedListBoxFiles.Items.Add(filename, true);
                }

                init_list();
            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private bool is_loaded(string filename)
        {
            foreach (string file in lst_loadedfile)
                if (file == filename) return true;

            return false;
        }

        private void init_list()
        {
            try
            {
                listViewData.Items.Clear();
                foreach (STFileData item in lst_filedata)
                {
                    AddItemToList(item);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void AddItemToList(STFileData data)
        {
            ListViewItem item;
            item = new ListViewItem(data.filename, data.status);
            item.SubItems.Add(data.text);
            listViewData.Items.Add(item);
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (checkBoxReload.Checked == true) reloadflag = true;
            else reloadflag = false;

            if (checkBoxMove.Checked == true) moveflag = true;
            else moveflag = false;

            buttonOK.Enabled = false;
            buttonExit.Enabled = false;
            buttonCancel.Enabled = true;
            buttonChangeDir.Enabled = false;

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
            buttonChangeDir.Enabled = true;

            //   progressBarData.Value = 0;
            //   progressBarData.Refresh();

        }

        private List<STFileData> change_status_file(string filename, int status, string text)
        {
            List<STFileData> ret = new List<STFileData>();
            STFileData item;
            foreach (STFileData file in lst_filedata)
            {
                if (file.filename != filename) ret.Add(file);
                else
                {
                    item = new STFileData();
                    item.filename = file.filename;
                    item.status = status;
                    item.text = text;
                    ret.Add(item);
                }

            }
            return ret;
        }

        private void StartProc(object sender, DoWorkEventArgs e)
        {
            string msg = null;
            try
            {
                foreach (STFileData file in lst_filedata)
                {
                    if (file.status == 0 || (file.status == 2 && reloadflag))
                    {
                        curr_file = file;

                        if (!read_file(curr_file.filename, out msg)) { log.LogLine(msg); lst_filedata = change_status_file(file.filename, 1, msg); }
                        else if (!load_file(sender, e, out  msg)) log.LogLine(msg);
                    }
                }

                if (moveflag) MoveFiles();

            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void MoveFiles()
        {
            try
            {
                string zipname = string.Format("{0}.zip", DateTime.Now.ToString("yyyy-mm-dd_HH-mm-ss"));
                string pathzip = Path.Combine(config.archpathassistcsv, zipname);
             //   ZipFile.CreateFromDirectory(config.inpathassistcsv, pathzip);

                DirectoryInfo myDirInfo = new DirectoryInfo(config.inpathassistcsv);

                foreach (FileInfo file in myDirInfo.GetFiles())
                {
                    file.Delete();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void LoadMTBankFile_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                // this.Text = "Внимание! После обработки текущего файла процесс записи будет остановлен...";
                e.Cancel = true;
                //      backgroundWorker1.CancelAsync();
            }
        }

        /*private bool read_file(string FileName, out string msg)
        {
            msg = null;

            Excel._Application app = new Excel.Application();

            try
            {
                string FilePath = Path.Combine(inpath, FileName);

                Excel._Workbook book = app.Workbooks.Open(FilePath, Missing.Value, Missing.Value,
                       Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                       Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                       Missing.Value, Missing.Value);
                Excel.Sheets sheets = book.Worksheets;
                Excel._Worksheet sheet;

                sheet = (Excel._Worksheet)sheets.get_Item(1);

                Excel.Range range;

                //    int i = 0;
               // int row = 0;
               // int countcol = 12;
               // int pc = 0;

               // string[] arr;
               // string s;

                filedata = new List<string[]>();

                data = new List<STRowAssistData>();

                int lastRow = sheet.UsedRange.Rows.Count;
                object o;

                for (int i = 2; i <= lastRow; i++)
                {
                   // range = sheet.get_Range(string.Format("A{0}", i), Missing.Value);
                   // if (range.Value2 == null) continue;
                   // string s = Convert.ToString(range.Value2).Trim();

                  //  if (!string.IsNullOrEmpty(s)) continue;
                  //  else
                  //  {
                        STRowAssistData item = new STRowAssistData();
                        item.filename = FileName;
                        item.tags = null;

                        range = sheet.get_Range(string.Format("C{0}", i), Missing.Value);
                        if (range.Value2 != null)
                        {
                            item.ordernumber = Convert.ToString(range.Value2).Trim();
                        }
                        else item.ordernumber = null;

                        range = sheet.get_Range(string.Format("D{0}", i), Missing.Value);
                        if (range.Value2 != null)
                        {
                            string namecode = Convert.ToString(range.Value2).Trim();
                            item.orderstate = get_opercode(namecode);
                        }
                        else item.orderstate = 0;

                        string sa = "Оплата";
                        range = sheet.get_Range(string.Format("K{0}", i), Missing.Value);
                        if (range.Value2 != null)
                        {
                            sa = Convert.ToString(range.Value2).Trim();
                        }

                        range = sheet.get_Range(string.Format("N{0}", i), Missing.Value);
                        if (range.Value2 != null)
                        {
                            double dbl = Convert.ToDouble(range.Value2);
                            if (sa == "Отмена") dbl = -dbl;
                            item.amount = dbl;
                        }

                        range = sheet.get_Range(string.Format("P{0}", i), Missing.Value);
                        if (range.Value2 != null)
                        {
                            string gdt  = Convert.ToString(range.Value2).Trim();
                            item.ltime = get_datetime(gdt);
                        }

                        range = sheet.get_Range(string.Format("S{0}", i), Missing.Value);
                        if (range.Value2 != null)
                        {
                            item.cardnumber = Convert.ToString(range.Value2).Trim();
                        }

                        range = sheet.get_Range(string.Format("AE{0}", i), Missing.Value);
                        if (range.Value2 != null)
                        {
                            item.bank = Convert.ToString(range.Value2).Trim();
                        }

                        range = sheet.get_Range(string.Format("J{0}", i), Missing.Value);
                        if (range.Value2 != null)
                        {
                            item.tags = string.Format("<CH={0}>",Convert.ToString(range.Value2).Trim());
                        }
                        else item.tags = null;

                        data.Add(item);
                 //   }
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
        }*/
        private bool read_file(string FileName, out string msg)
        {
            msg = null;
            int i = 0;
            try
            {
                string FilePath = Path.Combine(inpath, FileName);


                data = new List<STRowAssistData>();
                STRowAssistData item;

                string[] arr_lines = File.ReadAllLines(FilePath);
                // если файл пустой  - ошибка
                if (arr_lines.Length <= 0) { msg = "Empty file."; return false; }

                for (i = 1; i < arr_lines.Length; i++)
                {
                    item = new STRowAssistData();

                    string[] words = arr_lines[i].Split(';');

                    item.filename = FileName;

                    item.ordernumber = words[2].Trim();
                    item.orderstate = get_opercode(words[3].Trim());
                    item.cardnumber = words[18].Trim();
                    item.bank = words[30].Trim();

                    double.TryParse(words[13].Replace('.', ','), out item.amount);
                    double.TryParse(words[5].Replace('.', ','), out item.orderamount);

                    
                    
                    DateTime.TryParse(words[15].Substring(0,19), out item.ltime);
                    DateTime.TryParse(words[7].Substring(0, 19), out item.orderdate);

                    string sa = words[10].Trim();
                    item.type = 1;
                    if (sa == "Отмена") { item.amount = -item.amount; item.type = 2; }
                    if (sa == "Подтверждение оплаты") item.type = 3;

                    if (!string.IsNullOrEmpty(words[9])) item.tags += string.Format("<CN={0}>", words[9].Trim());
                    if (!string.IsNullOrEmpty(words[19])) item.tags += string.Format("<CH={0}>", words[19].Trim());
                    if (!string.IsNullOrEmpty(words[0])) item.tags += string.Format("<BN={0}>", words[0]);
                    if (!string.IsNullOrEmpty(words[14])) item.tags += string.Format("<CUR={0}>", words[14]);
                    
                    data.Add(item);
                }

            }
            catch (Exception ex)
            {
                msg += string.Format("Not the correct number of fields in a line {0}.", i + 1);
                msg = ex.Message;
                log.LogLine("File wasn't read.");
                return false;
            }


            return true;
        }

        private DateTime get_datetime(string ltime)
        {
            DateTime ret = new DateTime();
            string dt = ltime.Substring(0, 19);
            DateTime.TryParse(dt, out ret);
            return ret;
        }

        private int get_opercode(string name)
        {
            int ret = 0;

            switch (name)
            {
                case "Approved": ret = 1; break;
                case "PartialCanceled": ret = 2; break;
                case "Canceled": ret = 3; break;
                case "Declined": ret = 4; break;
                case "Timeout": ret = 5; break;
                default: ret = 0; break;
            }

            return ret;
        }

        private bool load_file(object sender, DoWorkEventArgs e, out string msg)
        {
            msg = null;

            OracleConnection connection = new OracleConnection(config.connectionstring);

            backgroundWorker1.ReportProgress(0);

            connection.Open();


         //   OracleTransaction transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted);
      //      OracleCommand command = connection.CreateCommand();

            // Start a local transaction
            // transaction = connection.BeginTransaction();
            // Assign transaction object for a pending local transaction
         //   command.Transaction = transaction;

            try
            {
                int pc = 0;
                int i = 0;
                int cnt = data.Count;

                //      command.CommandText = string.Format("DELETE FROM Rcd.CHECKWEBPAYDATA Where FILENAME='{0}'", FileName);
                //      command.ExecuteNonQuery();

                delete_file(data[0].filename);

                foreach (STRowAssistData item in data)
                {
                    if (backgroundWorker1.CancellationPending)
                    {
                      //  transaction.Rollback();
                        e.Cancel = true;
                        backgroundWorker1.ReportProgress(0);
                        return false;
                    }

                    string query = "INSERT INTO RCD.VALID_ASSISTDATA ( ORDERNUMBER, ORDERSTATE, LTIME, AMOUNT, CARDNUMBER, BANK, FILENAME, TAGS, ORDERAMOUNT, ORDERDATE, TYPE) " +
                        "values (:1, :2, :3, :4, :5, :6, :7, :8, :9, :10, :11)";
                    OracleCommand command = new OracleCommand(query, connection);
                //    command.CommandText = "INSERT INTO RCD.VALID_ASSISTDATA ( ORDERNUMBER, ORDERSTATE, LTIME, AMOUNT, CARDNUMBER, BANK, FILENAME, TAGS) values (:1, :2, :3, :4, :5, :6, :7, :8)";
                //    command.Parameters.Clear();
                    command.Parameters.Add(crp(OracleType.Char, item.ordernumber, "1", false));
                    command.Parameters.Add(crp(OracleType.Int32, item.orderstate, "2", false));
                    command.Parameters.Add(crp(OracleType.DateTime, item.ltime, "3", false));
                    command.Parameters.Add(crp(OracleType.Number, Math.Round(item.amount, 2), "4", false));
                    command.Parameters.Add(crp(OracleType.Char, item.cardnumber, "5", false));
                    command.Parameters.Add(crp(OracleType.Char, item.bank, "6", false));
                    command.Parameters.Add(crp(OracleType.Char, item.filename, "7", false));
                    command.Parameters.Add(crp(OracleType.Clob, item.tags, "8", false));
                    command.Parameters.Add(crp(OracleType.Number, Math.Round(item.orderamount, 2), "9", false));
                    command.Parameters.Add(crp(OracleType.DateTime, item.orderdate, "10", false));
                    command.Parameters.Add(crp(OracleType.Int32, item.type, "11", false));
                    command.ExecuteNonQuery();

                    i += 1;
                    //        pc = i / cnt * 100;
                    double d = ((i * 1.0) / cnt) * 100;
                    //      double f = Math.Round((double)(i / cnt * 100.0));
                    backgroundWorker1.ReportProgress((int)d);
                }

            //    transaction.Commit();
            }
            catch (Exception ex)
            {
            //    transaction.Rollback();
                msg = ex.Message;
                log.LogLine("Neither record was written to database.");
                return false;
            }


            return true;
        }

        private void delete_file(string filename)
        {
            string query = string.Format("DELETE FROM Rcd.VALID_ASSISTDATA WHERE FILENAME=:1");

            OracleConnection connection = new OracleConnection(config.connectionstring);
            connection.Open();

            OracleCommand cmd = new OracleCommand(query, connection);

            try
            {
                cmd.Parameters.Add(crp(OracleType.Char, filename, "1", false));

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message);  }
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

        private bool is_d(string s)
        {
            bool ret = false;
            int x;
            if (int.TryParse(s, out x)) return true;

            return ret;
        }

        private List<string> get_loaded_file()
        {
            List<string> ret = new List<string>();
            try
            {
                string query = string.Format("SELECT FILENAME FROM Rcd.VALID_ASSISTDATA GROUP BY FILENAME");
                OracleConnection connection = new OracleConnection(config.connectionstring);
                connection.Open();

                OracleCommand cmd = new OracleCommand(query, connection);
                OracleDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                        ret.Add(reader.GetString(0));
                }

                reader.Dispose();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            return ret;
        }
    }


   
}
