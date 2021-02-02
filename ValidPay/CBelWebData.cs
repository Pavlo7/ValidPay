using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.IO;
using Newtonsoft.Json.Linq;
//using System.Text.Json;
//using System.Text.Json.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace ValidPay
{
    public struct STVPBelWebData
    {
        public string tamplate;
    }

    public struct UPRow
    {
        public string targetChannel;
        public double transAmount;
        public double feeAmount;
        public int transCurrency;
        public int targetNumber;
        public string authCode;
        public string transDate;
        public string postingDate;
        public string settlmDate;
        public double settlmAmount;
        public string sourceNumber;
        public string rrn;
        public int mcc;
        public int transType;
        public string requestCategory;
        public string transDetails;
        public int externalId;
    }

    public struct STTotal
    {
        public int rows;
        public double transAmount;
        public double feeAmount;
        public double settlmAmount;
    }


    [Serializable]
    public class JFile
    {
        public string bank;
        public List<UPRow> rows;
        public string orderNumber;
        public STTotal total;
    }


    class CBelWebData
    {
        CConfig config;
        XLog log;
        MA m_app;

        DateTime dtBegin;
        DateTime dtEnd;

        public CBelWebData(CConfig cf)
        {
            config = cf; 
            log = new XLog();
            log.DirName = config.logpath;
            m_app = get_MA();
        }

        [Obsolete]
        public int GetTable(STVPBelWebData param, out DataTable table, out string msg)
        {
            int ret = 0;
            msg = null;
            table = new DataTable("MAIN");
            try
            {
                using (OracleConnection connet = new OracleConnection(config.connectionstring))
                {
                    connet.Open();
                    if (connet.State != ConnectionState.Open) { log.LogLine(string.Format("No connection to DB Oracle. CS: {0}", config.connectionstring)); return 1; }

                    string query = string.Format("SELECT FileName, SUM(AMOUNT) AS AMOUNT, SUM(FEEAMOUNT) AS COMISSION from Rcd.VALID_BELWEBDATA  group by FileName order by FileName ");

                    OracleCommand cmd = new OracleCommand(query, connet);
                    OracleDataAdapter da = new OracleDataAdapter(cmd);
                    da.Fill(table);
                }
            }
            catch (Exception ex) { log.LogLine("CBelWebData.GetTable() " + ex.Message); ret = -1; msg = ex.Message; }
            return ret;
        }

        [Obsolete]
        public int InsertRow(OracleConnection connection, UPRow item, string ordernumber, string filename, out string msg)
        {
            int ret = 0;
            msg = null;
            DateTime dt;
            try
            {
                string query = "INSERT INTO RCD.VALID_BELWEBDATA (RRN, Amount, FeeAmount, CardNumber, TransDate, Postingdate, SetLmDate, OperType, PayDocNumber, FileName, Tags) " +
                        "values (:1, :2, :3, :4, :5, :6, :7, :8, :9, :10, :11)";
                OracleCommand command = new OracleCommand(query, connection);

                command.Parameters.Add(crp(OracleType.Char, item.rrn, "1", false));
                command.Parameters.Add(crp(OracleType.Number, Math.Round(item.transAmount, 2), "2", false));
                command.Parameters.Add(crp(OracleType.Number, Math.Round(item.feeAmount, 2), "3", false));
                command.Parameters.Add(crp(OracleType.Char, item.targetNumber, "4", false));
                DateTime.TryParse(item.transDate.Replace('T', ' '), out dt);
                command.Parameters.Add(crp(OracleType.DateTime, dt, "5", false));
                DateTime.TryParse(item.postingDate, out dt);
                command.Parameters.Add(crp(OracleType.DateTime, dt, "6", false));
                DateTime.TryParse(item.settlmDate, out dt);
                command.Parameters.Add(crp(OracleType.DateTime, dt, "7", false));
                int opercode = get_opercode(item.requestCategory);
                command.Parameters.Add(crp(OracleType.Int32, opercode, "8", false));
                command.Parameters.Add(crp(OracleType.Char, ordernumber, "9", false));
                command.Parameters.Add(crp(OracleType.Char, filename, "10", false));
                command.Parameters.Add(crp(OracleType.Char, get_tags(item), "11", false));
                command.ExecuteNonQuery();
            }
            catch (Exception ex) { msg = ex.Message; return -1; }
            return ret;
        }
        private int get_opercode(string oc)
        {
            int ret = 0;
            try
            {
                if (oc.ToUpper().StartsWith("P")) ret = 1;
                if (oc.ToUpper().StartsWith("R")) ret = 2;
                if (oc.ToUpper().StartsWith("J")) ret = 3;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            return ret;
        }

        private string get_tags(UPRow item)
        {
            string ret = null;
            try
            {
                if (!string.IsNullOrEmpty(item.transDetails)) ret += string.Format("<TDT={0}>", item.transDetails);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            return ret;
        }

        public int ReadFile(string filename, out JFile jfile, out string msg)
        {
            int ret = 0;
            msg = null;
            jfile = new JFile();

         //   container = new List<UPRow>();
         //   UPRow item;

            string text;

            try
            {
                string FilePath = Path.Combine(m_app.PathIN, filename);
                if (File.Exists(FilePath))
                {
                   // using (StreamReader r = new StreamReader(FilePath))
                   // {
                   //     string json = r.ReadToEnd();
                   //     List<UPRow> items = JsonConvert.DeserializeObject<List<UPRow>>(json);
                   // }

                    DataContractJsonSerializer formatter = new DataContractJsonSerializer(typeof(JFile));
                    // открываем поток (json файл)
                    using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
                    {
                        // десериализация (создание объекта из потока)
                        jfile = (JFile)formatter.ReadObject(fs);
                    }
                }
            }
            catch (Exception ex) { msg = ex.Message; return -1; }
            return ret;
        }

        [Obsolete]
        public int DeleteFile(string filename, out string msg)
        {
            int ret = 0;
            msg = null;
            try
            {
                string query = string.Format("DELETE FROM Rcd.VALID_BELWEBDATA WHERE FILENAME=:1");

                using (OracleConnection connection = new OracleConnection(config.connectionstring))
                {
                    connection.Open();
                    OracleCommand cmd = new OracleCommand(query, connection);
                    cmd.Parameters.Add(crp(OracleType.Char, filename, "1", false));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { msg = ex.Message; return -1; }
            return ret;
        }

        private MA get_MA()
        {
            MA ret = new MA();
            try
            {
                foreach (MA s in config.madata)
                {
                    if (s.Code == 306) { ret = s; return ret; };
                }
            }
            catch (Exception) { }
            return ret;
        }

        [Obsolete]
        public List<string> get_loaded_file()
        {
            List<string> ret = new List<string>();
            try
            {
                string query = string.Format("SELECT FILENAME FROM Rcd.VALID_BelWebData GROUP BY FILENAME");
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
