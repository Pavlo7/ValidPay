using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Data.OracleClient;

namespace ValidPay
{
    public struct STValidDataParam
    {
        public DateTime dtbegin;
        public DateTime dtend;
        public int type;            // 1 - дубликаты, 2 - несовпадения сумм
    }

     /* Эмитент */
    public struct STValidData
    {
        public string RRN;
        public int EMTCODEFRM;
        public int AZSCODE;
        public int DOCNUMBER;
        public DateTime DOCDATE;
        public double S_DIIS;
        public string rrn;
        public double amount;
        public string comments;
    };

    class CValidData
    {
        CConfig config;
        XLog log;

        public CValidData(CConfig cf)
        {
            config = cf; 
            log = new XLog();
            log.DirName = config.logpath;
        }

        // Drive&Pay 
        public List<STValidData> GetData(STValidDataParam viewparam)
        {
            List<STValidData> list_data = new List<STValidData>();
            try
            {
                if (viewparam.type == 2) list_data = GetData1(viewparam);
                if (viewparam.type == 1) list_data = GetData2(viewparam);
                if (viewparam.type == 3) list_data = GetData3(viewparam);
            }
            catch (Exception ex) { log.LogLine(ex.Message); }
            return list_data;
        }

        public List<STValidData> GetData1(STValidDataParam viewparam)
        {
            List<STValidData> list_data = new List<STValidData>();

            STValidData item;
            OracleConnection connet = new OracleConnection();

            try
            {
                connet = new OracleConnection(config.connectionstring);
                connet.Open();
                if (connet.State != ConnectionState.Open) { log.LogLine(string.Format("No connection to DB Oracle. CS: {0}", config.connectionstring)); return list_data;}

                string query = string.Format("select RRN, EMTCODEFRM, AZSCODE ,DOCNUMBER, DOCDATE ,S_DIIS,SUMM1,Comments FROM (select T1.RRN, T1.EMTCODEFRM, T1.AZSCODE, T1.DOCNUMBER,  " +
                    "T1.DOCDATE, T1.S_DIIS, case when T2.AMOUNT>0 then ROUND(T2.AMOUNT,2) else 0 end AS SUMM1, " +
                    "case when T2.AMOUNT is null then 'не найден RRN в WebPay' else null end AS Comments FROM ( SELECT RRN, EMTCODEFRM, AZSCODE, DOCNUMBER, DOCDATE, sum(S_DIIS) AS " +
                    "S_DIIS FROM Rcd.VALID_RCPDATA  WHERE EMTCODETO=300 AND SHIFTDATE>=:1 AND SHIFTDATE<=:2 GROUP BY RRN, EMTCODEFRM, AZSCODE, DOCNUMBER,  DOCDATE ) T1 LEFT JOIN " +
                    "Rcd.VALID_WEBPAYDATA T2 ON T1.RRN=T2.RRN ) WHERE S_DIIS<>SUMM1");
                

                OracleCommand cmd = new OracleCommand(query, connet);
                cmd.Parameters.Add(crp(OracleType.DateTime, viewparam.dtbegin, "1", false));
                cmd.Parameters.Add(crp(OracleType.DateTime, viewparam.dtend, "2", false));
                OracleDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new STValidData();

                        if (!reader.IsDBNull(0))
                            item.RRN = reader.GetString(0);
                        if (!reader.IsDBNull(1))
                            item.EMTCODEFRM = reader.GetInt32(1);
                        if (!reader.IsDBNull(2))
                            item.AZSCODE = reader.GetInt32(2);
                        if (!reader.IsDBNull(3))
                            item.DOCNUMBER = reader.GetInt32(3);
                        if (!reader.IsDBNull(4))
                            item.DOCDATE = reader.GetDateTime(4);
                        if (!reader.IsDBNull(5))
                            item.S_DIIS = reader.GetDouble(5);
                        if (!reader.IsDBNull(6))
                            item.amount = reader.GetDouble(6);
                        if (!reader.IsDBNull(7))
                            item.comments = reader.GetString(7);

                        list_data.Add(item);
                    }
                }

                reader.Dispose();
            }
            catch (Exception ex) { log.LogLine(ex.Message); if (connet.State == ConnectionState.Open) connet.Close();  }

            return list_data;
        }

        // Drive&Pay 
        public List<STValidData> GetData2(STValidDataParam viewparam)
        {
            List<STValidData> list_data = new List<STValidData>();

            STValidData item;
            OracleConnection connet = new OracleConnection();

            try
            {
                connet = new OracleConnection(config.connectionstring);
                connet.Open();
                if (connet.State != ConnectionState.Open) { log.LogLine(string.Format("No connection to DB Oracle. CS: {0}", config.connectionstring)); return list_data; }

                string query = string.Format("select T3.RRN, T3.EMTCODEFRM, T3.AZSCODE, T3.DOCNUMBER,  T3.DOCDATE, T3.S_DIIS, T4.RRN, ROUND(T4.AMOUNT,2) FROM Rcd.VALID_RCPDATA T3 " +
                    "LEFT JOIN Rcd.VALID_WEBPAYDATA T4 ON T3.RRN=T4.RRN where T3.RRN in ( select RRN FROM ( select RRN, count(*) AS CNT FROM " +
                    "( SELECT RRN, EMTCODEFRM, AZSCODE, DOCNUMBER, DOCDATE, sum(S_DIIS) AS  S_DIIS FROM Rcd.VALID_RCPDATA WHERE EMTCODETO=300 AND SHIFTDATE>=:1 AND SHIFTDATE<=:2 " +
                    "GROUP BY RRN, EMTCODEFRM, AZSCODE, DOCNUMBER,  DOCDATE) T1 GROUP BY RRN ) T2 WHERE CNT>1 ) order by T3.RRN");

                OracleCommand cmd = new OracleCommand(query, connet);
                cmd.Parameters.Add(crp(OracleType.DateTime, viewparam.dtbegin, "1", false));
                cmd.Parameters.Add(crp(OracleType.DateTime, viewparam.dtend, "2", false));
                OracleDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new STValidData();

                        if (!reader.IsDBNull(0))
                            item.RRN = reader.GetString(0);
                        if (!reader.IsDBNull(1))
                            item.EMTCODEFRM = reader.GetInt32(1);
                        if (!reader.IsDBNull(2))
                            item.AZSCODE = reader.GetInt32(2);
                        if (!reader.IsDBNull(3))
                            item.DOCNUMBER = reader.GetInt32(3);
                        if (!reader.IsDBNull(4))
                            item.DOCDATE = reader.GetDateTime(4);
                        if (!reader.IsDBNull(5))
                            item.S_DIIS = reader.GetDouble(5);
                        if (!reader.IsDBNull(6))
                            item.rrn = reader.GetString(6);
                        if (!reader.IsDBNull(7))
                            item.amount = reader.GetDouble(7);

                        list_data.Add(item);
                    }
                }

                reader.Dispose();
            }
            catch (Exception ex) { log.LogLine(ex.Message); if (connet.State == ConnectionState.Open) connet.Close(); }

            return list_data;
        }

        // Drive&Pay Assist
        /*public List<STValidData> GetData3(STValidDataParam viewparam)
        {
            List<STValidData> list_data = new List<STValidData>();

            STValidData item;
            OracleConnection connet = new OracleConnection();

            try
            {
                connet = new OracleConnection(config.connectionstring);
                connet.Open();
                if (connet.State != ConnectionState.Open) { log.LogLine(string.Format("No connection to DB Oracle. CS: {0}", config.connectionstring)); return list_data; }

                string query = string.Format("select RRN, EMTCODEFRM, AZSCODE ,DOCNUMBER, DOCDATE ,S_DIIS,SUMM1,Comments FROM ( select T1.RRN, T1.EMTCODEFRM, T1.AZSCODE, T1.DOCNUMBER, " +
                    "T1.DOCDATE, T1.S_DIIS, case when T2.AMOUNT>0 then ROUND(T2.AMOUNT,2) else 0 end AS SUMM1, case when T2.AMOUNT is null then 'Не найден номер заказа в Assist' else null end AS Comments " +
                    "FROM ( SELECT RRN, EMTCODEFRM, AZSCODE, DOCNUMBER, DOCDATE, sum(S_DIIS) AS S_DIIS FROM Rcd.VALID_RCPDATA  WHERE EMTCODETO=300 AND SHIFTDATE>=:1 AND SHIFTDATE<:2 GROUP BY RRN, EMTCODEFRM, " +
                    "AZSCODE, DOCNUMBER,  DOCDATE ) T1 LEFT JOIN ( select OrderNumber, sum(Amount) AS AMOUNT from Rcd.VALID_ASSISTDATA where Type in (1,2) group by OrderNumber ) T2 ON T1.RRN=T2.OrderNumber " +
                    ") WHERE S_DIIS<>SUMM1 ");
                //WHERE LTime>=:1 AND LTime<:2

                OracleCommand cmd = new OracleCommand(query, connet);
                cmd.Parameters.Add(crp(OracleType.DateTime, viewparam.dtbegin, "1", false));
                cmd.Parameters.Add(crp(OracleType.DateTime, viewparam.dtend, "2", false));
                OracleDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new STValidData();

                        if (!reader.IsDBNull(0))
                            item.RRN = reader.GetString(0);
                        if (!reader.IsDBNull(1))
                            item.EMTCODEFRM = reader.GetInt32(1);
                        if (!reader.IsDBNull(2))
                            item.AZSCODE = reader.GetInt32(2);
                        if (!reader.IsDBNull(3))
                            item.DOCNUMBER = reader.GetInt32(3);
                        if (!reader.IsDBNull(4))
                            item.DOCDATE = reader.GetDateTime(4);
                        if (!reader.IsDBNull(5))
                            item.S_DIIS = reader.GetDouble(5);
                        if (!reader.IsDBNull(6))
                            item.amount = reader.GetDouble(6);
                        if (!reader.IsDBNull(7))
                            item.comments = reader.GetString(7);

                        list_data.Add(item);
                    }
                }

                reader.Dispose();
            }
            catch (Exception ex) { log.LogLine(ex.Message); if (connet.State == ConnectionState.Open) connet.Close(); }

            return list_data;
        }*/
         
        public List<STValidData> GetData3(STValidDataParam viewparam)
        {
            List<STValidData> list_data = new List<STValidData>();

            STValidData item;
            OracleConnection connet = new OracleConnection();

            try
            {
                connet = new OracleConnection(config.connectionstring);
                connet.Open();
                if (connet.State != ConnectionState.Open) { log.LogLine(string.Format("No connection to DB Oracle. CS: {0}", config.connectionstring)); return list_data; }

                string query = string.Format("select RRN, EMTCODEFRM, AZSCODE ,DOCNUMBER, DOCDATE ,S_DIIS,SUMM1,Comments FROM ( select T1.RRN, T1.EMTCODEFRM, T1.AZSCODE, T1.DOCNUMBER, " +
                    "T1.DOCDATE, T1.S_DIIS, case when T2.AMOUNT>0 then ROUND(T2.AMOUNT,2) else 0 end AS SUMM1, case when T2.AMOUNT is null then 'Не найден номер заказа в Assist' else null end AS Comments " +
                    "FROM ( SELECT RRN, EMTCODEFRM, AZSCODE, DOCNUMBER, DOCDATE, sum(S_DIIS) AS S_DIIS FROM Rcd.VALID_RCPDATA  WHERE EMTCODETO=300 AND SHIFTDATE>=:1 AND SHIFTDATE<:2 GROUP BY RRN, EMTCODEFRM, " +
                    "AZSCODE, DOCNUMBER,  DOCDATE ) T1 LEFT JOIN ( select OrderNumber, sum(Amount) AS AMOUNT from Rcd.VALID_ASSISTDATA_CSV  group by OrderNumber) T2 ON T1.RRN=T2.OrderNumber " +
                    ") WHERE S_DIIS<>SUMM1 ");
                //WHERE LTime>=:1 AND LTime<:2

                OracleCommand cmd = new OracleCommand(query, connet);
                cmd.Parameters.Add(crp(OracleType.DateTime, viewparam.dtbegin, "1", false));
                cmd.Parameters.Add(crp(OracleType.DateTime, viewparam.dtend, "2", false));
                OracleDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new STValidData();

                        if (!reader.IsDBNull(0))
                            item.RRN = reader.GetString(0);
                        if (!reader.IsDBNull(1))
                            item.EMTCODEFRM = reader.GetInt32(1);
                        if (!reader.IsDBNull(2))
                            item.AZSCODE = reader.GetInt32(2);
                        if (!reader.IsDBNull(3))
                            item.DOCNUMBER = reader.GetInt32(3);
                        if (!reader.IsDBNull(4))
                            item.DOCDATE = reader.GetDateTime(4);
                        if (!reader.IsDBNull(5))
                            item.S_DIIS = reader.GetDouble(5);
                        if (!reader.IsDBNull(6))
                            item.amount = reader.GetDouble(6);
                        if (!reader.IsDBNull(7))
                            item.comments = reader.GetString(7);

                        list_data.Add(item);
                    }
                }

                reader.Dispose();
            }
            catch (Exception ex) { log.LogLine(ex.Message); if (connet.State == ConnectionState.Open) connet.Close(); }

            return list_data;
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
