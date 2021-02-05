using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.OracleClient;
//using Oracle.DataAccess.Client;


namespace ValidPay
{
    public struct STRowRcpData
    {
        public DateTime SHIFTDATE;
        public double SALESUM;
        public int EMTCODEFRM;
        public int AZSCODE;
        public int DOCNUMBER;
        public int POSNUMBER;
        public DateTime DOCDATE;
        public int OPERCODE;
        public double REALDOSE;
        public double PRICE;
        public int CURRCODE;
        public int EMTCODETO;
        public int VSTYPE;
        public int VSCODE;
        public int WTCASHTYPE;
        public string DESCRIPT;
        public string CARDBANK;
        public string RRN;
        public double S_DIIS;
        public string NAMEPRODUCT;
    }


    public partial class LoadRcpData : Form
    {
        CConfig config;
        XLog log;
        
        

        DateTime dtBegin;
        DateTime dtEnd;

        List<STEmitent> lst;
        CEmitent clEmt;

        string emtname;

        public LoadRcpData(CConfig cf)
        {
            InitializeComponent();
            config = cf;
            log = new XLog();
            log.DirName = config.logpath;
        }

        [Obsolete]
        private void LoadRcpData_Load(object sender, EventArgs e)
        {
            try
            {
                clEmt = new CEmitent(config);
                init_lst();

             //   DateTime dte = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 0, 0, 0, 0);
             //   dte = dte.AddDays(-1);
             //   DateTime dtb = new DateTime(dte.Year, dte.Month, 1, 0, 0, 0, 0);
             //   dateTimePickerDTB.Value = dtb;
             //   dateTimePickerDTE.Value = dte;

                DateTime dt = GetMaxDate();
                dt = dt.AddDays(1);
                dateTimePickerDTB.Value = dt;
                dateTimePickerDTE.Value = DateTime.Now.AddDays(-1);

            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); } 
        }

        [Obsolete]
        private DateTime GetMaxDate()
        {
            DateTime ret = new DateTime();

            try
            {
                using (OracleConnection connect = new OracleConnection(config.connectionstring))
                {
                    connect.Open();
                    if (connect.State == ConnectionState.Open)
                    {
                        string query = string.Format("SELECT MAX(ShiftDate) FROM Rcd.Valid_rcpdata");

                        OracleCommand cmd = new OracleCommand(query, connect);
                        OracleDataReader reader = cmd.ExecuteReader();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                if (!reader.IsDBNull(0))
                                    ret = reader.GetDateTime(0);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); } 

            return ret;
        }

        private void init_lst()
        {
            try
            {
                
                lst = clEmt.GetData(1);

                checkedListBoxEmt.Items.Clear();

                foreach (STEmitent item in lst)
                    checkedListBoxEmt.Items.Add(item.name, true);
            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); } 
        }

        private bool GetData(string connectionstring, out List<STRowRcpData> data, out string msg)
        {
            SqlConnection connect;
            data = new List<STRowRcpData>();
            msg = null;
            STRowRcpData item;
            bool ret = true;
            try
            {
                connect = new SqlConnection(connectionstring);
                connect.Open();
                if (connect.State != ConnectionState.Open) { msg = string.Format("No connection to DB. CS: {0}", connectionstring); return false; }

                string query = string.Format("SELECT Shiftdate,SaleSum,EmtCodeFrm,AZSCode,DocNumber,PosNumber,DocDate,OperCode,RealDose,Price,CurrCode,EmtCodeTo,VSType,VsCode,WtCashType,Descript," +
                    "CardBank,RRN, S_DIIS, NameProduct  FROM View_JOIN_RCPvWEBPAY WHERE Shiftdate>=@1 AND Shiftdate<=@2");
                SqlCommand cmd = new SqlCommand(query, connect);
                cmd.Parameters.Add(crp(SqlDbType.DateTime, "@1", dtBegin));
                cmd.Parameters.Add(crp(SqlDbType.DateTime, "@2", dtEnd));
                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if (!read(reader, out item, out msg))
                        {
                            msg = "Warning! Perhaps not all data is received from the database!";
                            ret = false;
                        }
                        else data.Add(item);
                    }
                }
                reader.Dispose();
                connect.Close();
            }
            catch (Exception ex) { msg = ex.Message; return false; }
            return ret;
        }

        private void delete_data(int emtcode, OracleConnection connection)
        {
            if (connection is null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            string query = string.Format("DELETE FROM Rcd.VALID_RCPDATA WHERE Shiftdate>=:1 AND Shiftdate<=:2 AND EmtCodeFrm=:3");

            OracleCommand cmd = new OracleCommand(query, connection);

            try
            {
                cmd.Parameters.Add(crp(OracleType.DateTime,  dtBegin, "1", false));
                cmd.Parameters.Add(crp(OracleType.DateTime,  dtEnd, "2", false));
                cmd.Parameters.Add(crp(OracleType.Int32, emtcode, "3", false));

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private bool read(SqlDataReader reader, out STRowRcpData data, out string msg)
        {
            bool ret = true;
            data = new STRowRcpData();
            msg = null;
            try
            {
                if (!reader.IsDBNull(0))
                    data.SHIFTDATE = reader.GetDateTime(0);
                if (!reader.IsDBNull(1))
                    data.SALESUM = reader.GetDouble(1);
                if (!reader.IsDBNull(2))
                    data.EMTCODEFRM = reader.GetInt32(2);
                if (!reader.IsDBNull(3))
                    data.AZSCODE = reader.GetInt32(3);
                if (!reader.IsDBNull(4))
                    data.DOCNUMBER = reader.GetInt32(4);
                if (!reader.IsDBNull(5))
                    data.POSNUMBER = reader.GetInt32(5);
                if (!reader.IsDBNull(6))
                    data.DOCDATE = reader.GetDateTime(6);
                if (!reader.IsDBNull(7))
                    data.OPERCODE = reader.GetInt32(7);
                if (!reader.IsDBNull(8))
                    data.REALDOSE = reader.GetDouble(8);
                if (!reader.IsDBNull(9))
                    data.PRICE = reader.GetDouble(9);
                if (!reader.IsDBNull(10))
                    data.CURRCODE = reader.GetInt32(10);
                if (!reader.IsDBNull(11))
                    data.EMTCODETO = reader.GetInt32(11);
                if (!reader.IsDBNull(12))
                    data.VSTYPE = reader.GetInt32(12);
                if (!reader.IsDBNull(13))
                    data.VSCODE = reader.GetInt32(13);
                if (!reader.IsDBNull(14))
                    data.WTCASHTYPE = reader.GetInt32(14);
                if (!reader.IsDBNull(15))
                    data.DESCRIPT = reader.GetString(15);
                if (!reader.IsDBNull(16))
                    data.CARDBANK = reader.GetString(16);
                if (!reader.IsDBNull(17))
                    data.RRN = reader.GetString(17);
                if (!reader.IsDBNull(18))
                    data.S_DIIS = reader.GetDouble(18);
                if (!reader.IsDBNull(19))
                    data.NAMEPRODUCT = reader.GetString(19);
                
            }
            catch (Exception ex) { log.LogLine("CBookOwner.read() " + ex.Message); msg = ex.Message; ret = false; }
            return ret;
        }

        [Obsolete]
        private bool SetData(STRowRcpData item, out string msg, OracleConnection connection)
        {
            if (connection is null)
            {
                throw new ArgumentNullException(nameof(connection));
            }

            msg = null;

            try
            {

                string query = "INSERT INTO Rcd.VALID_RCPDATA (SHIFTDATE, SALESUM, EMTCODEFRM, AZSCODE, DOCNUMBER, POSNUMBER, DOCDATE, OPERCODE, REALDOSE, PRICE, CURRCODE, EMTCODETO, VSTYPE, " +
                    "VSCODE, WTCASHTYPE, DESCRIPT, CARDBANK, RRN, S_DIIS, NAMEPRODUCT) values (:1, :2, :3, :4, :5, :6, :7, :8, :9, :10, :11, :12, :13, :14, :15, :16, :17, :18, :19, :20)";
                OracleCommand command = new OracleCommand(query, connection);

                command.Parameters.Add(crp(OracleType.DateTime, item.SHIFTDATE, "1", false));
                command.Parameters.Add(crp(OracleType.Number, item.SALESUM, "2", false));
                command.Parameters.Add(crp(OracleType.Int32, item.EMTCODEFRM, "3", false));
                command.Parameters.Add(crp(OracleType.Int32, item.AZSCODE, "4", false));
                command.Parameters.Add(crp(OracleType.Int32, item.DOCNUMBER, "5", false));
                command.Parameters.Add(crp(OracleType.Int32, item.POSNUMBER, "6", false));
                command.Parameters.Add(crp(OracleType.DateTime, item.DOCDATE, "7", false));
                command.Parameters.Add(crp(OracleType.Int32, item.OPERCODE, "8", false));
                command.Parameters.Add(crp(OracleType.Number, item.REALDOSE, "9", false));
                command.Parameters.Add(crp(OracleType.Number, item.PRICE, "10", false));
                command.Parameters.Add(crp(OracleType.Int32, item.CURRCODE, "11", false));
                command.Parameters.Add(crp(OracleType.Int32, item.EMTCODETO, "12", false));
                command.Parameters.Add(crp(OracleType.Int32, item.VSTYPE, "13", false));
                command.Parameters.Add(crp(OracleType.Int32, item.VSCODE, "14", false));
                command.Parameters.Add(crp(OracleType.Int32, item.WTCASHTYPE, "15", false));
                command.Parameters.Add(crp(OracleType.Char, item.DESCRIPT, "16", false));
                command.Parameters.Add(crp(OracleType.Char, item.CARDBANK, "17", false));
                command.Parameters.Add(crp(OracleType.Char, item.RRN, "18", false));
                command.Parameters.Add(crp(OracleType.Number, item.S_DIIS, "19", false));
                command.Parameters.Add(crp(OracleType.Char, item.NAMEPRODUCT, "20", false));

                command.ExecuteNonQuery();
            }
            catch (Exception ex) { msg = ex.Message; return false; }
            return true;
        }

        [Obsolete]
        private void StartProc(object sender, DoWorkEventArgs e)
        {
            using (OracleConnection connection = new OracleConnection(config.connectionstring))
            {
                string msg = null;
                List<STEmitent> lst_emt = new List<STEmitent>();
                List<STRowRcpData> data;
                int cnt_r, cnt_b;
                try
                {
                    connection.Open();

                    dtBegin = new DateTime(dateTimePickerDTB.Value.Year, dateTimePickerDTB.Value.Month, dateTimePickerDTB.Value.Day, 0, 0, 0, 0);
                    dtEnd = new DateTime(dateTimePickerDTE.Value.Year, dateTimePickerDTE.Value.Month, dateTimePickerDTE.Value.Day, 23, 59, 59, 0);

                    for (int i = 0; i <= (checkedListBoxEmt.Items.Count - 1); i++)
                    {
                        if (checkedListBoxEmt.GetItemChecked(i))
                        {
                            string emtname = checkedListBoxEmt.Items[i].ToString();
                            STEmitent st = get_emt(emtname);
                            if (st.code > 0) lst_emt.Add(st);
                        }
                    }


                    if (lst_emt.Count > 0)
                    {
                        log.LogLine(string.Format("-------------------------", emtname));
                        log.LogLine(string.Format("Timespan: {0}-{1} ", dtBegin.ToString("dd-MM-yyyy"), dtEnd.ToString("dd-MM-yyyy")));
                        log.LogLine(string.Format("-------------------------", emtname));

                        foreach (STEmitent emt in lst_emt)
                        {
                            emtname = emt.name;
                            log.LogLine(string.Format("{0} start processing...", emtname));

                            if (backgroundWorker1.CancellationPending)
                            {
                                e.Cancel = true;
                                backgroundWorker1.ReportProgress(0);
                                return;
                            }

                            backgroundWorker1.ReportProgress(0);

                            data = new List<STRowRcpData>();
                            if (!GetData(emt.c_string, out data, out msg)) { log.LogLine(msg); continue; }

                            log.LogLine(string.Format("{0} rows prepare to writing", data.Count));

                            cnt_r = 0;
                            cnt_b = 0;

                            if (data.Count > 0)
                            {
                                // удаляем период
                                delete_data(emt.code, connection);

                                int cnt = data.Count;
                                int pc = 0;
                                int i = 0;

                                foreach (STRowRcpData item in data)
                                {
                                    if (backgroundWorker1.CancellationPending)
                                    {
                                        e.Cancel = true;
                                        backgroundWorker1.ReportProgress(0);
                                        return;
                                    }

                                    if (!SetData(item, out msg, connection)) { cnt_b++; log.LogLine(msg); }
                                    else cnt_r++;

                                    i += 1;
                                    double d = ((i * 1.0) / cnt) * 100;
                                    backgroundWorker1.ReportProgress((int)d);
                                }
                            }

                            log.LogLine(string.Format("{0}/{1} rows added/missed", cnt_r, cnt_b));
                            log.LogLine(string.Format("{0} end processing...", emtname));
                        }
                    }


                    connection.Close();
                }
                catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }

        private STEmitent get_emt(string emtname)
        {
            STEmitent ret = new STEmitent();

            if (string.IsNullOrEmpty(emtname)) return ret;
            if (lst.Count <= 0) return ret;

            var results = from myRow in lst.AsEnumerable()
                          where myRow.name == emtname
                          select myRow; 

            foreach (var vr in results)
                ret = vr;
            return ret;
        
        }

        private SqlParameter crp(SqlDbType type, string pname, object val)
        {
            SqlParameter param = new SqlParameter();
            param.ParameterName = pname;
            param.SqlDbType = type;
            param.IsNullable = true;
            if (val != null)
                param.Value = val;
            else
                param.Value = DBNull.Value;

            return param;
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

        private void buttonOK_Click(object sender, EventArgs e)
        {
            buttonOK.Enabled = false;
            buttonExit.Enabled = false;
            buttonCancel.Enabled = true;
            
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            StartProc(sender, e);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            labelEmtCode.Text = emtname;
            progressBarData.Value = e.ProgressPercentage;
            labelInfo.Text = "Processing.... " + progressBarData.Value.ToString() + "%";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                labelInfo.Text = "File recording interrupted by user...";
            }

         // Check to see if an error occurred in the background process.

            else if (e.Error != null)
            {
                labelInfo.Text = "Error while performing background operation...";
            }
            else
            {
                // Everything completed normally.
                labelInfo.Text = "File recording completed...";
            }

            buttonCancel.Enabled = false;
            buttonExit.Enabled = true;
            buttonOK.Enabled = true;
                    
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

    }
}
