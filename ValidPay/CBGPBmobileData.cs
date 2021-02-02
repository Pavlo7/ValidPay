using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace ValidPay
{
    
    public struct data
    {
        public DateTime date;
        public int emtcodefrom;
        public int azscode;
        public int oilcode;
        public string price;
        public string amount;
        public string summa;
        public string dissumma;
        public int emtcodeto;
        public string cardcode;
        public string discount;
        public string shiftdate;
        public int curcode;
        public int contract_currency;
        public int exrate;
        public string emtprice;
        public string rrn;
        public string paydate;
    }

    public class root
    {
        public data[] lst;
    }

    public struct BGPBmobileRow
    {
        public DateTime DocDate;
        public DateTime ShiftDate;
        public DateTime PayDate;
        public string RRN;
        public double Amount;
        public double AmountDis;
        public string FileName;
        public string Tags;
    }

    class CBGPBmobileData
    {
        CConfig config;
        XLog log;
        MA m_app;

        DateTime dtBegin;
        DateTime dtEnd;

        public CBGPBmobileData(CConfig cf)
        {
            config = cf; 
            log = new XLog();
            log.DirName = config.logpath;
            m_app = get_MA(); 
        }

        private MA get_MA()
        {
            MA ret = new MA();
            try
            {
                foreach (MA s in config.madata)
                {
                    if (s.Code == 305) { ret = s; return ret; };
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
                string query = string.Format("SELECT FILENAME FROM Rcd.VALID_BGPBMobileDATA GROUP BY FILENAME");
                using (OracleConnection connection = new OracleConnection(config.connectionstring))
                {
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
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
            return ret;
        }

        [Obsolete]
        public int DeleteFile(out string msg)
        {
            int ret = 0;
            msg = null;
            try
            {
                string query = string.Format("DELETE FROM Rcd.VALID_BGPBMobileDATA WHERE SHIFTDATE>=:1 AND SHIFTDATE<=:2");

                using (OracleConnection connection = new OracleConnection(config.connectionstring))
                {
                    connection.Open();
                    OracleCommand cmd = new OracleCommand(query, connection);
                    cmd.Parameters.Add(crp(OracleType.DateTime, dtBegin, "1", false));
                    cmd.Parameters.Add(crp(OracleType.DateTime, dtEnd, "2", false));
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex) { msg = ex.Message; return -1; }
            return ret;
        }

        [Obsolete]
        public int InsertRow(OracleConnection connection, BGPBmobileRow item, out string msg)
        {
            int ret = 0;
            msg = null;
            try
            {
                string query = "INSERT INTO RCD.VALID_BGPBMobileDATA (DocDate, ShiftDate, PayDate, RRN, Amount, Amount_Dis, FileName, Tags) " +
                        "values (:1, :2, :3, :4, :5, :6, :7, :8)";
                OracleCommand command = new OracleCommand(query, connection);

                command.Parameters.Add(crp(OracleType.DateTime, item.DocDate, "1", false));
                command.Parameters.Add(crp(OracleType.DateTime, item.ShiftDate, "2", false));
                command.Parameters.Add(crp(OracleType.DateTime, item.PayDate, "3", false));
                command.Parameters.Add(crp(OracleType.Char, item.RRN, "4", false));
                command.Parameters.Add(crp(OracleType.Number, Math.Round(item.Amount, 2), "5", false));
                command.Parameters.Add(crp(OracleType.Number, Math.Round(item.AmountDis, 2), "6", false));
                command.Parameters.Add(crp(OracleType.Char, item.FileName, "7", false));
                command.Parameters.Add(crp(OracleType.Char, item.Tags, "8", false));
                command.ExecuteNonQuery();
            }
            catch (Exception ex) { msg = ex.Message; return -1; }
            return ret;
        }

        public int ReadFile(string filename, out  List<BGPBmobileRow> container, out string msg)
        {
            int ret = 0;
            msg = null;
            container = new List<BGPBmobileRow>();
            BGPBmobileRow item;

            root Fields;

            string text;
            data rowdata;

            try
            {
                string FilePath = Path.Combine(m_app.PathIN, filename);
                if (File.Exists(FilePath))
                {

                    XmlDocument xDoc = new XmlDocument();
                    xDoc.Load(FilePath);

                    // получим корневой элемент
                    XmlElement xRoot = xDoc.DocumentElement;

                    text = null;
                    text = xRoot.GetAttribute("datestart");
                    DateTime.TryParse(text, out dtBegin);

                    text = null;
                    text = xRoot.GetAttribute("dateend");
                    DateTime.TryParse(text, out dtEnd);

                    XmlNodeList nodeList = xRoot.GetElementsByTagName("data");

                    foreach (XmlNode xnode in nodeList)
                    {
                        item = new BGPBmobileRow();
                        rowdata = new data();
                        

                        text = null;
                        text = xnode["date"].InnerText;
                        DateTime.TryParse(text.Replace('T', ' '), out item.DocDate);
                        text = null;
                        text = xnode["shiftdate"].InnerText;
                        DateTime.TryParse(text, out item.ShiftDate);
                        text = null;
                        text = xnode["paydata"].InnerText;
                        DateTime.TryParse(text, out item.PayDate);

                        item.RRN = xnode["rrn"].InnerText;

                        text = null;
                        text = xnode["summa"].InnerText;
                        double.TryParse(text, out item.Amount);
                        text = null;
                        text = xnode["dissumma"].InnerText;
                        double.TryParse(text, out item.AmountDis);

                        item.FileName = filename;

        
                        text = null;
                        text = xnode["emtcodefrom"].InnerText;
                        if (!string.IsNullOrEmpty(text)) item.Tags += string.Format("<EMT={0}>", text);
                        text = null;
                        text = xnode["azscode"].InnerText;
                        if (!string.IsNullOrEmpty(text)) item.Tags += string.Format("<AZS={0}>", text);
                         text = null;
                        text = xnode["oilcode"].InnerText;
                        if (!string.IsNullOrEmpty(text)) item.Tags += string.Format("<OIL={0}>", text);
                         text = null;
                        text = xnode["price"].InnerText;
                        if (!string.IsNullOrEmpty(text)) item.Tags += string.Format("<PRICE={0}>", text);
                         text = null;
                        text = xnode["amount"].InnerText;
                        if (!string.IsNullOrEmpty(text)) item.Tags += string.Format("<QNT={0}>", text);
                         text = null;
                        text = xnode["cardcode"].InnerText;
                        if (!string.IsNullOrEmpty(text)) item.Tags += string.Format("<CARD={0}>", text);
                         text = null;
                        text = xnode["discount"].InnerText;
                        if (!string.IsNullOrEmpty(text)) item.Tags += string.Format("<DIS={0}>", text);
                         text = null;
                        text = xnode["curcode"].InnerText;
                        if (!string.IsNullOrEmpty(text)) item.Tags += string.Format("<CUR={0}>", text);
                         text = null;
                        text = xnode["contract_currency"].InnerText;
                        if (!string.IsNullOrEmpty(text)) item.Tags += string.Format("<CCUR={0}>", text);
                         text = null;
                        text = xnode["exrate"].InnerText;
                        if (!string.IsNullOrEmpty(text)) item.Tags += string.Format("<RATE={0}>", text);
                         text = null;
                        text = xnode["emtprice"].InnerText;
                        if (!string.IsNullOrEmpty(text)) item.Tags += string.Format("<EPRICE={0}>", text);

                        container.Add(item);
                    }

                //    using (FileStream fs = new FileStream(FilePath, FileMode.OpenOrCreate))
                //    {
                //        XmlSerializer formatter = new XmlSerializer(typeof(root));
                //        Fields = (root)formatter.Deserialize(fs);
                //    }

                 //   XmlSerializer ser = new XmlSerializer(typeof(root));
                  //  TextReader reader = new StreamReader(FilePath, Encoding.Default);
                   // Fields = ser.Deserialize(reader) as root;
                  //  reader.Close();
                }
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
