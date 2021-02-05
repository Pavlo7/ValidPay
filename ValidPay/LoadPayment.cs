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
    public struct STRowPayment
    {
        public DateTime dtime;
        public double amount;
    }

    public partial class LoadPayment : Form
    {
        public string inpath;

        List<STRowPayment> data;

        CConfig config;
        XLog log;

        public LoadPayment(CConfig cf)
        {
            InitializeComponent();
            config = cf; ;
        }

        private void LoadPayment_Load(object sender, EventArgs e)
        {
            log = new XLog();
            log.DirName = config.logpath;
            inpath = Path.GetFullPath(config.inpathpayment);

            labelDIR.Text = inpath;
        }

        [Obsolete]
        private void buttonOK_Click(object sender, EventArgs e)
        {
            string msg;
            string[] files = Directory.GetFiles(inpath); // путь к папке
            foreach (string path in files)
            {
                read_file(path, out msg);

                foreach(STRowPayment item in data)
                {
                    if (is_row(item.dtime)) update_row(item);
                    else insert_row(item);
                }
            }
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonChangeDir_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                inpath = folderBrowserDialog1.SelectedPath;
                labelDIR.Text = inpath;
            
                //buttonOK.Enabled = true;
            }
        }

        private bool read_file(string FileName, out string msg)
        {
            msg = null;
            int i = 0;
            try
            {
                string FilePath = Path.Combine(inpath, FileName);

                string[] arr_lines = File.ReadAllLines(FilePath);
                // если файл пустой  - ошибка
                if (arr_lines.Length <= 0) { msg = "Empty file."; return false; }

                data = new List<STRowPayment>();
                STRowPayment item = new STRowPayment();

                for (i = 0; i < arr_lines.Length; i++)
                {
                    item = new STRowPayment();

                    string[] words = arr_lines[i].Split(';');

                    if (create_row(words[0], words[1], out item, out msg)) data.Add(item);
                    else log.LogLine(string.Format("Erorr row's reading {0}, {1})", i, msg));
                }
            }
            catch (Exception ex)
            {
                msg = ex.Message;
                log.LogLine("File wasn't read.");
                return false;
            }

            return true;
        }

        private bool create_row(string data, string amount, out STRowPayment item, out string msg)
        {
            bool ret = true;
            item = new STRowPayment();
            
            msg = null;
            try
            {
                if (!get_datetime(data, out item.dtime, out msg)) { return false; }
                if (!double.TryParse(amount, out item.amount)) { return false; }
            }
            catch (Exception ex) { ret = false; msg = ex.Message; }
            return ret;
        }

        private bool get_datetime(string ltime, out DateTime ret, out string msg)
        {
            ret = new DateTime();
            msg = null;

            try
            {
                if (ltime.Length < 10) { msg = "Invalid date field format!"; return false; }
                string dt = ltime.Substring(0, 10);
                if (DateTime.TryParse(dt, out ret)) return true;
            }
            catch (Exception ex) { msg = ex.Message; return false; }
            return true;
        }

        [Obsolete]
        private void insert_row(STRowPayment item)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(config.connectionstring))
                {
                    connection.Open();

                    string query = "INSERT INTO RCD.VALID_PAYMENT ( PDATE, AMOUNT) values (:1, :2)";
                    OracleCommand command = new OracleCommand(query, connection);
                    command.Parameters.Add(crp(OracleType.DateTime, item.dtime, "1", false));
                    command.Parameters.Add(crp(OracleType.Number, Math.Round(item.amount, 2), "2", false));
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        [Obsolete]
        private void update_row(STRowPayment item)
        {
            try
            {
                using (OracleConnection connection = new OracleConnection(config.connectionstring))
                {
                    connection.Open();

                    string query = "UPDATE RCD.VALID_PAYMENT SET AMOUNT=:2 WHERE PDATE=:1";
                    OracleCommand command = new OracleCommand(query, connection);
                    command.Parameters.Add(crp(OracleType.DateTime, item.dtime, "1", false));
                    command.Parameters.Add(crp(OracleType.Number, Math.Round(item.amount, 2), "2", false));
                    command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        [Obsolete]
        private bool is_row(DateTime dt)
        {
            bool ret = false;
            try
            {
                using (OracleConnection connection = new OracleConnection(config.connectionstring))
                {
                    connection.Open();

                    string query = "SELECT * FROM RCD.VALID_PAYMENT WHERE PDATE=:1";
                    OracleCommand command = new OracleCommand(query, connection);
                    command.Parameters.Add(crp(OracleType.DateTime, dt, "1", false));
                    OracleDataReader reader = command.ExecuteReader();
                    if (reader.HasRows) ret = true;
                    reader.Close();
                    connection.Close();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            return ret;
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

        private DateTime get_datetime(string ltime)
        {
            DateTime ret = new DateTime();
            string dt = ltime.Substring(0, 10);
            DateTime.TryParse(dt, out ret);
            return ret;
        }

    }
}
