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
    public struct STRowAmountPC
    {
        public DateTime dtime;
        public int appcode;
        public double amount;
    }

    public partial class LoadAmout300 : Form
    {
        public string inpath;

        List<STRowAmountPC> data;

        CConfig config;
        XLog log;

        public LoadAmout300(CConfig cf)
        {
            InitializeComponent();
            config = cf;
        }

        private void LoadAmout300_Load(object sender, EventArgs e)
        {
            log = new XLog();
            log.DirName = config.logpath;
            inpath = Path.GetFullPath(config.inpathamountpc300);

            labelDIR.Text = inpath;
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string msg;
            string[] files = Directory.GetFiles(inpath); // путь к папке
            foreach (string path in files)
            {
                read_file(path, out msg);

                foreach (STRowAmountPC item in data)
                {
                    if (is_row(item.dtime, item.appcode)) update_row(item);
                    else insert_row(item);
                }
            }

            MessageBox.Show("Процесс записи файла завершен.", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

       /* private bool read_file(string FileName, out string msg)
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

                data = new List<STRowAmountPC>();
                STRowAmountPC item = new STRowAmountPC();

                int lastRow = sheet.UsedRange.Rows.Count;
                object o;
                string gdt;
                string stramount;

                for (int i = 1; i <= lastRow; i++)
                {
                    gdt = null; stramount = null;

                    range = sheet.get_Range(string.Format("A{0}", i), Missing.Value);
                    if (range.Value2 != null)
                    {
                        object f = range.Value2;
                        DateTime dt = Convert.ToDateTime(range.Value2).Trim();
                    }

                    range = sheet.get_Range(string.Format("B{0}", i), Missing.Value);
                    if (range.Value2 != null)
                    {
                        stramount = Convert.ToString(range.Value2).Trim();
                    }

                    if (create_row(gdt, stramount, out item, out msg)) data.Add(item);
                    else log.LogLine(string.Format("Ошибка чтения в строке {0}, {1})", i, msg));

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

                string[] arr_lines = File.ReadAllLines(FilePath);
                // если файл пустой  - ошибка
                if (arr_lines.Length <= 0) { msg = "Empty file."; return false; }

                data = new List<STRowAmountPC>();
                STRowAmountPC item = new STRowAmountPC();

                for (i = 0; i < arr_lines.Length; i++)
                {
                    item = new STRowAmountPC();

                    string[] words = arr_lines[i].Split(';');

                    if (create_row(words[0], words[1], out item, out msg)) data.Add(item);
                    else log.LogLine(string.Format("Ошибка чтения в строке {0}, {1})", i, msg));
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

        private bool create_row(string data, string amount, out STRowAmountPC item, out string msg)
        {
            bool ret = true;
            item = new STRowAmountPC();
            item.appcode = 300;
            msg = null;
            try
            {
                if (!get_datetime(data, out item.dtime, out msg)) { return false; }
                if (!double.TryParse(amount, out item.amount)) { return false; }
            }
            catch (Exception ex) { ret = false;  msg = ex.Message; }
            return ret;
        }


        private void insert_row(STRowAmountPC item)
        {
            try
            {
                OracleConnection connection = new OracleConnection(config.connectionstring);
                connection.Open();   

                string query = "INSERT INTO RCD.VALID_AMOUNT_PC ( PDATE, APPCODE, AMOUNT) values (:1, :2, :3)";
                OracleCommand command = new OracleCommand(query, connection);
                command.Parameters.Add(crp(OracleType.DateTime, item.dtime, "1", false));
                command.Parameters.Add(crp(OracleType.Int32, item.appcode, "2", false));
                command.Parameters.Add(crp(OracleType.Number, Math.Round(item.amount, 2), "3", false));
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void update_row(STRowAmountPC item)    
        {
            try
            {
                OracleConnection connection = new OracleConnection(config.connectionstring);
                connection.Open();

                string query = "UPDATE RCD.VALID_AMOUNT_PC SET AMOUNT=:3 WHERE PDATE=:1 and APPCODE=:2";
                OracleCommand command = new OracleCommand(query, connection);
                command.Parameters.Add(crp(OracleType.DateTime, item.dtime, "1", false));
                command.Parameters.Add(crp(OracleType.Int32, item.appcode, "2", false));
                command.Parameters.Add(crp(OracleType.Number, Math.Round(item.amount, 2), "3", false));
                command.ExecuteNonQuery();
                connection.Close();
            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private bool is_row(DateTime dt, int code)
        {
            bool ret = false;
            try
            {
                OracleConnection connection = new OracleConnection(config.connectionstring);
                connection.Open();

                string query = "SELECT * FROM RCD.VALID_AMOUNT_PC WHERE PDATE=:1 and APPCODE=:2";
                OracleCommand command = new OracleCommand(query, connection);
                command.Parameters.Add(crp(OracleType.DateTime, dt, "1", false));
                command.Parameters.Add(crp(OracleType.Int32, code, "2", false));
                OracleDataReader reader = command.ExecuteReader();
                if (reader.HasRows) ret = true;
                reader.Close();
                connection.Close();
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

        private bool get_datetime(string ltime, out DateTime ret, out string msg)
        {
            ret = new DateTime();
            msg = null;

            try
            {
                if (ltime.Length < 10) { msg = "Неверный формат поля даты"; return false;}
                string dt = ltime.Substring(0, 10);
                if (DateTime.TryParse(dt, out ret)) return true;
            }
            catch (Exception ex) { msg = ex.Message; return false; }
            return true;
        }
    }
}
