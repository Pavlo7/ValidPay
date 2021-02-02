using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;
using System.IO;

namespace ValidPay
{
    public struct STVPAssistData
    {
        public string tamplate;
    }

    class CAssistData
    {
        CConfig config;
        XLog log;
        MA m_app;

        public CAssistData(CConfig cf)
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
                    if (s.Code == 300) { ret = s; return ret; };
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
                string query = string.Format("SELECT FILENAME FROM Rcd.VALID_ASSISTDATA_CSV GROUP BY FILENAME");
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
        public int GetTable(STVPAssistData param, out DataTable table, out string msg)
        {
            int ret = 0;
            msg = null;
            OracleConnection connet = new OracleConnection();

            table = new DataTable("MAIN");
            
            try
            {
                connet = new OracleConnection(config.connectionstring);
                connet.Open();
                if (connet.State != ConnectionState.Open) { log.LogLine(string.Format("No connection to DB Oracle. CS: {0}", config.connectionstring)); return 1; }

                string query = string.Format(" SELECT T.FileDate, T.FileName, T.AMOUNT, T.COMISSION , T1.AMOUNT AS Payment, T1.AMOUNT-T.AMOUNT AS NEG FROM " +
                    " (SELECT FileDate, FileName, SUM(AMOUNT) AS AMOUNT, SUM(COMMISSION) AS COMISSION from Rcd.VALID_ASSISTDATA_CSV  group by FileDate, FileName) T  " +
                    "LEFT JOIN Rcd.Valid_Payment T1 ON T.FileDate = T1.PDATE order by T.FileDate");

                OracleCommand cmd = new OracleCommand(query, connet);
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(table);
            }
            catch (Exception ex) { log.LogLine("CAssistData.GetTable() " + ex.Message); ret = -1; msg = ex.Message; }
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

        public int ReadFile(string filename, out List<STRowAssistDataCsv> container, out string msg)
        {
            int ret = 0;
            msg = null;
            container = new List<STRowAssistDataCsv>();
            STRowAssistDataCsv item;
            int i = 0;
            string text;

            try
            {
                string FilePath = Path.Combine(m_app.PathIN, filename);
                if (File.Exists(FilePath))
                {
                    DateTime c_date = getfiledate(filename);
                    string[] arr_lines = File.ReadAllLines(FilePath);
                    // если файл пустой  - ошибка
                    if (arr_lines.Length <= 0) { msg = "Empty file."; return 1; }

                    for (i = 1; i < arr_lines.Length; i++)
                    {
                        item = new STRowAssistDataCsv();

                        string[] words = arr_lines[i].Split(';');

                        item.filename = filename;
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

                        double.TryParse(words[2].Replace('.', ','), out item.amount);
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

                        container.Add(item);
                    }
                }
            }
            catch (Exception ex) { msg = ex.Message; return -1; }
            return ret;
        }


        [Obsolete]
        public int InsertRow(OracleConnection connection, STRowAssistDataCsv item, out string msg)
        {
            int ret = 0;
            msg = null;
            try
            {
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
            }
            catch (Exception ex) { msg = ex.Message; return -1; }
            return ret;
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

        public int DeleteFile(string filename, out string msg)
        {
            int ret = 0;
            msg = null;
            try
            {
                string query = string.Format("DELETE FROM Rcd.VALID_ASSISTDATA_CSV WHERE FILENAME=:1");

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

    }
}
