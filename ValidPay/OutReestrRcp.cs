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


namespace ValidPay
{
    public struct STParam
    {
        public int app_code;
        public DateTime dtBegin;
        public DateTime dtEnd;
        public bool sh;
        public string fullpath;
    }

    public struct STDataReest
    {
        public string RRN;
        public DateTime ShiftDate;
        public DateTime DocDate;
        public double Amount;
        public double Price;
        public double Quantity;
        public string NameProduct;
        public int EmtCodeFrm;
        public int AzsCode;
        public int DocNumber;
        public int PosNumber;
        public int Currency;
        public string CardBank;
    }

    public partial class OutReestrRcp : Form
    {
        public string outpath;

        List<STRowAmountPC> data;

        CConfig config;
        XLog log;

        public OutReestrRcp(CConfig cf)
        {
            InitializeComponent();
            config = cf;
        }

        private void OutReestrRcp_Load(object sender, EventArgs e)
        {
            log = new XLog();
            log.DirName = config.logpath;
            outpath = Path.GetFullPath(config.outpath);

            init_combo();

            DateTime dtn = new DateTime(DateTime.Now.Year, DateTime.Now.Month,1,0,0,0);
            DateTime dtEnd = dtn.AddDays(-1);
            DateTime dtBegin = new DateTime(dtEnd.Year, dtEnd.Month, 1, 0, 0, 0, 0);
            dateTimePickerDB.Value = dtBegin;
            dateTimePickerDE.Value = dtEnd;

            radioButtonShiftDate.Checked = true;
            labelDIR.Text = outpath;
            textBoxFileName.Text = string.Format("RCPxDP_{0}.csv", DateTime.Now.ToString("yyyyMMddHHssmm"));
        }

        private void init_combo()
        {
            string[] arr = new string[] {"Drive&Pay","A1","BelWeb","BGPmobile"};
            foreach (string s in arr)
                comboBoxMobileApp.Items.Add(s);

            comboBoxMobileApp.Text = comboBoxMobileApp.Items[0].ToString();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            string msg;
            List<STDataReest> container;

            try
            {
                STParam param = new STParam();
                if (read_param(out param, out msg) != 0) { MessageBox.Show(msg); return; }

                if (GetContainer(param, out container, out msg) != 0) { MessageBox.Show(msg); return; }

                if (create_file(container, param.fullpath, out msg) != 0) MessageBox.Show(msg, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else MessageBox.Show("Файл создан.", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
               
        }

        private int read_param(out STParam param, out string msg)
        {
            int ret = 0;
            param = new STParam();
            msg = null;
            try
            {
                param.dtBegin = new DateTime(dateTimePickerDB.Value.Year, dateTimePickerDB.Value.Month, dateTimePickerDB.Value.Day,0,0,0,0);
                param.dtEnd = new DateTime(dateTimePickerDE.Value.Year, dateTimePickerDE.Value.Month, dateTimePickerDE.Value.Day,23,59,59,0);
                string app = comboBoxMobileApp.Text.Trim();
                switch(app)
                {
                    case "Drive&Pay": { param.app_code = 300; break;}
                    case "A1": { param.app_code = 303; break;}
                    case "BelWeb": { param.app_code = 306; break;}
                    case "BGPmobile": { param.app_code = 305; break;}
                    default : { msg = "неизвестное приложение"; return 1; }
                }
                param.sh=true;
                if (radioButtonDocDate.Checked == true) param.sh = false;
                param.fullpath = Path.Combine(outpath, textBoxFileName.Text.Trim());
            }
            catch(Exception ex) { msg = ex.Message; return -1; }
            return ret;
        }

        private int create_file(List<STDataReest> container, string fullpath, out string msg)
        {
            int ret = 0;
            msg = null; 
            try
            {
                if (container.Count <=0) return 1;

                string[] c_file = new string[container.Count+1];

                string row = null;
                c_file[0] = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12}", "RRN", "ShiftDate", "DocDate", "Amount", "Price", "Quantity", "Product", "EmtCodeFrm",
                        "AzsCode", "DocNumber", "PosNumber", "Currency", "CardBank");

                int i=1;
                foreach(STDataReest item in container)
                {
                    row = null;
                    row = string.Format("{0};{1};{2};{3};{4};{5};{6};{7};{8};{9};{10};{11};{12}", item.RRN, item.ShiftDate.ToShortDateString(), item.DocDate.ToString("dd.MM.yyyy HH:mm:ss"), 
                        item.Amount, item.Price, item.Quantity, item.NameProduct, item.EmtCodeFrm, item.AzsCode, item.DocNumber, item.PosNumber, item.Currency, item.CardBank);

                    c_file[i] = row;
                    i++;
                }

              //  File.Create(fullpath);
                File.WriteAllLines(fullpath, c_file, Encoding.UTF8);

              
            }
            catch (Exception ex) { msg = ex.Message; return -1; }
            return ret;
        }


        private int GetContainer(STParam param, out List<STDataReest> container, out string msg)
        {
            int ret = 0;
            container = new List<STDataReest>();
            msg = null;
            OracleConnection connect = new OracleConnection();
            STDataReest item;
            try
            {
                connect = new OracleConnection(config.connectionstring);
                connect.Open();
                if (connect.State != ConnectionState.Open) { log.LogLine(string.Format("No connection to DB Oracle. CS: {0}", config.connectionstring)); return 1; }

                string strdate = "ShiftDate";
                if (!param.sh) strdate = "DocDate";

                string query = string.Format("Select RRN, SHIFTDATE, DOCDATE, S_DIIS, PRICE, REALDOSE, NAMEPRODUCT, EMTCODEFRM, AZSCODE ,DOCNUMBER,POSNUMBER, CURRCODE,CARDBANK  FROM Rcd.VALID_RCPDATA " +
                    "WHERE {0}>=:1 AND {0}<=:2 AND EmtCodeTo=:3 ORDER BY RRN", strdate);

                OracleCommand cmd = new OracleCommand(query, connect);
                cmd.Parameters.Add(crp(OracleType.DateTime, param.dtBegin, "1", false));
                cmd.Parameters.Add(crp(OracleType.DateTime, param.dtEnd, "2", false));
                cmd.Parameters.Add(crp(OracleType.Int32, param.app_code, "3", false));
                OracleDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new STDataReest();


                        if (!reader.IsDBNull(0))
                            item.RRN = reader.GetString(0);
                        if (!reader.IsDBNull(1))
                            item.ShiftDate = reader.GetDateTime(1);
                        if (!reader.IsDBNull(2))
                            item.DocDate = reader.GetDateTime(2);
                        if (!reader.IsDBNull(3))
                            item.Amount = reader.GetDouble(3);
                        if (!reader.IsDBNull(4))
                            item.Price = reader.GetDouble(4);
                        if (!reader.IsDBNull(5))
                            item.Quantity = reader.GetDouble(5);
                        if (!reader.IsDBNull(6))
                            item.NameProduct = reader.GetString(6);
                        if (!reader.IsDBNull(7))
                            item.EmtCodeFrm = reader.GetInt32(7);
                         if (!reader.IsDBNull(8))
                            item.AzsCode = reader.GetInt32(8);
                        if (!reader.IsDBNull(9))
                            item.DocNumber = reader.GetInt32(9);
                        if (!reader.IsDBNull(10))
                            item.PosNumber = reader.GetInt32(10);
                        if (!reader.IsDBNull(11))
                            item.Currency = reader.GetInt32(11);
                        if (!reader.IsDBNull(12))
                            item.CardBank = reader.GetString(12);
                        
                        container.Add(item);
                    }
                }

                reader.Dispose();
            }
            catch (Exception ex) { msg = ex.Message; return -1; }
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
    }
}
