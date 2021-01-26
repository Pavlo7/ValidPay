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
using System.IO.Compression;
using Excel = Microsoft.Office.Interop.Excel;

namespace ValidPay
{
    public struct STRowAssistDataCsv
    {
        public string bilnumber;
        public DateTime ltime;
        public double amount;
        public double amount_wc;
        public double commision;
        public string ordernumber;
        public DateTime orderdate;
        public string tags;
        public string filename;
        public DateTime filedate;
    }

    public partial class LoadAssistCsvFile : Form
    {
        public string inpath;

        List<STRowAssistDataCsv> data;
        CConfig config;
        XLog log;

        string[] files;

        List<STFileData> lst_filedata;
        List<string> lst_loadedfile;

        STFileData curr_file;

        bool reloadflag;
        bool moveflag;

        DateTime c_date;

        public LoadAssistCsvFile(CConfig cf)
        {
            InitializeComponent();
            config = cf;
        }

        private void LoadAssistCsvFile_Load(object sender, EventArgs e)
        {
            buttonOK.Enabled = false;
            buttonCancel.Enabled = false;

            log = new XLog();
            log.DirName = config.logpath;
            inpath = Path.GetFullPath(config.inpathassistcsv);

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
            if (e.ProgressPercentage == 0 || e.ProgressPercentage == 100) init_list();

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
                        c_date = getfiledate(curr_file.filename);  
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
            //    ZipFile.CreateFromDirectory(config.inpathassistcsv, pathzip);

                DirectoryInfo myDirInfo = new DirectoryInfo(config.inpathassistcsv);

                foreach (FileInfo file in myDirInfo.GetFiles())
                {
                    file.Delete();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
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

        private bool load_file(object sender, DoWorkEventArgs e, out string msg)
        {
            msg = null;

            try
            {
                OracleConnection connection = new OracleConnection(config.connectionstring);
                connection.Open();

                lst_filedata = change_status_file(curr_file.filename, 3, "запись файла ...");
                backgroundWorker1.ReportProgress(0);
            

                int pc = 0;
                int i = 0;
                int cnt = data.Count;

                delete_file(data[0].filename);
                
                int cntrows = 0;

                foreach (STRowAssistDataCsv item in data)
                {
                    if (backgroundWorker1.CancellationPending)
                    {
                        e.Cancel = true;
                        backgroundWorker1.ReportProgress(0);
                        return false;
                    }

                    string query = "INSERT INTO RCD.VALID_ASSISTDATA_CSV (SHIFTDATA, BILNUMBER, AMOUNT, AMOUNTWC, COMMISSION, FILENAME, ORDERNUMBER, ORDERDATE, TAGS, FILEDATE) " +
                        "values (:1, :2, :3, :4, :5, :6, :7, :8, :9, :10)";
                    OracleCommand command = new OracleCommand(query, connection);

                    command.Parameters.Add(crp(OracleType.DateTime, item.ltime, "1", false));
                    command.Parameters.Add(crp(OracleType.Char, item.bilnumber, "2", false));
                    command.Parameters.Add(crp(OracleType.Number, Math.Round(item.amount, 2), "3", false));
                    command.Parameters.Add(crp(OracleType.Number, Math.Round(item.amount_wc, 2), "4", false));
                    command.Parameters.Add(crp(OracleType.Number, Math.Round(item.commision, 2), "5", false));
                    command.Parameters.Add(crp(OracleType.Char, item.filename, "6", false));
                    command.Parameters.Add(crp(OracleType.Char, item.ordernumber, "7", false));
                    command.Parameters.Add(crp(OracleType.DateTime, item.orderdate, "8", false));
                    command.Parameters.Add(crp(OracleType.Char, item.tags, "9", false));
                    command.Parameters.Add(crp(OracleType.DateTime, item.filedate, "10", false));
                    command.ExecuteNonQuery();

                    i += 1;
                    double d = ((i * 1.0) / cnt) * 100;

                    if (d < 100)  backgroundWorker1.ReportProgress((int)d);
                    
                    cntrows++;

                }

                lst_filedata = change_status_file(curr_file.filename, 2, string.Format("ок ({0} rows)", cntrows));
                backgroundWorker1.ReportProgress(100);
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

        private List<string> get_loaded_file()
        {
            List<string> ret = new List<string>();
            try
            {
                string query = string.Format("SELECT FILENAME FROM Rcd.VALID_ASSISTDATA_CSV GROUP BY FILENAME");
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


        private void delete_file(string filename)
        {
            string query = string.Format("DELETE FROM Rcd.VALID_ASSISTDATA_CSV WHERE FILENAME=:1");

            OracleConnection connection = new OracleConnection(config.connectionstring);
            connection.Open();

            OracleCommand cmd = new OracleCommand(query, connection);

            try
            {
                cmd.Parameters.Add(crp(OracleType.Char, filename, "1", false));

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
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

        private DateTime getfiledate(string filename)
        {
            DateTime ret = new DateTime();
            try
            {
                if (filename.Length != 25) return ret;

              //  string year = filename.Substring(13, 2);
             //   string month = filename.Substring(15, 2);
             //   string day = filename.Substring(17, 2);

                int day = 0;
                int.TryParse(filename.Substring(13, 2), out day);
                int month;
                int.TryParse(filename.Substring(15, 2), out month);
                int year;
                int.TryParse(filename.Substring(17, 4), out year);

                ret = new DateTime(year, month, day);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            return ret;
        }

        private bool read_file(string FileName, out string msg)
        {
            msg = null;
            int i=0;
            try
            {
                string FilePath = Path.Combine(inpath, FileName);


                data = new List<STRowAssistDataCsv>();
                STRowAssistDataCsv item;

                string[] arr_lines = File.ReadAllLines(FilePath);
                // если файл пустой  - ошибка
                if (arr_lines.Length <= 0) { msg = "Empty file."; return false; }

                for (i = 1; i < arr_lines.Length; i++)
                {
                    item = new STRowAssistDataCsv();

                    string[] words = arr_lines[i].Split(';');

                    item.filename = FileName;
                    item.filedate = c_date;

                    int day = 0;
                    int month = 0;
                    int year = 0;
                    DateTime dt;
                    //дата
                    if (words[0].Length == 6) 
                    {
                        int.TryParse(words[0].Substring(0, 2), out day);
                        int.TryParse(words[0].Substring(2, 2), out month);
                        int.TryParse(words[0].Substring(4, 2), out year);
                    }
                    else if (words[0].Length == 5)
                    {
                        int.TryParse(words[0].Substring(0, 1), out day);
                        int.TryParse(words[0].Substring(1, 2), out month);
                        int.TryParse(words[0].Substring(3, 2), out year);
                    }
                    else { msg = string.Format("Not the correct date in a line {0}.\r\n", i + 1); continue; }

                    dt = new DateTime(2000 + year, month, day, 0, 0, 0, 0);
                    item.ltime = dt;

                    // bilnumber
                    item.bilnumber = words[1].Trim();

                    double.TryParse(words[2].Replace('.',','), out item.amount);
                    double.TryParse(words[3].Replace('.', ','), out item.amount_wc);
                    double.TryParse(words[4].Replace('.', ','), out item.commision);

                    item.ordernumber = words[5].Trim();

                    DateTime.TryParse(words[7].Trim(), out item.orderdate);

                    if (!string.IsNullOrEmpty(words[6])) item.tags += string.Format("<ORS={0}>", words[6]);
                    if (!string.IsNullOrEmpty(words[8])) item.tags += string.Format("<CN={0}>", words[8]);
                    if (!string.IsNullOrEmpty(words[9])) item.tags += string.Format("<CARD={0}>", words[9]);
                    if (!string.IsNullOrEmpty(words[10])) item.tags += string.Format("<CH={0}>", words[10]);
                    if (!string.IsNullOrEmpty(words[11])) item.tags += string.Format("<BC={0}>", words[11]);
                    if (!string.IsNullOrEmpty(words[12])) item.tags += string.Format("<BANK={0}>", words[12]);
                    if (!string.IsNullOrEmpty(words[13])) item.tags += string.Format("<RRN={0}>", words[13]);

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
    }
}
