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
        public int orientation;            // 1 - rpc, 2 - mobile app
        public int app_code;
        public bool b;
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
              //  if (viewparam.type == 2) list_data = GetData1(viewparam);
              //  if (viewparam.type == 1) list_data = GetData2(viewparam);
              //  if (viewparam.type == 3) list_data = GetData3(viewparam);
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

        [Obsolete]
        public int GetTable(STValidDataParam param, out DataTable table, out string msg)
        {
            int ret = 0;
            table = new DataTable();
            msg = null;
            string query = null;
            try
            {
                // if we don't get query - return 
                if (get_query(param.orientation, param.app_code, param.b, out query, out msg) != 0) { return 1; }

                using (OracleConnection connect = new OracleConnection(config.connectionstring))
                {
                    connect.Open();

                    if (connect.State != ConnectionState.Open) { msg = "no connection to DB"; return 2; }
                    using (OracleCommand cmd = new OracleCommand(query, connect))
                    {
                        cmd.Parameters.Add(crp(OracleType.DateTime, param.dtbegin, "1", false));
                        cmd.Parameters.Add(crp(OracleType.DateTime, param.dtend, "2", false));
                        cmd.CommandTimeout = 250;
                        OracleDataReader reader = cmd.ExecuteReader();
                        OracleDataAdapter da = new OracleDataAdapter(cmd);
                        da.Fill(table);
                    }

                }

            }
            catch (Exception ex) { log.LogLine(ex.Message); msg = ex.Message; return -1; }
            return ret;
        }

        private int get_query(int orientation, int app_code , bool b, out string query, out string msg)
        {
            // :1 and :2 - all queries have mandatory parameters date begin and date end

            query = null;
            msg = null;
            int ret = 0;
            try
            {
                // Valid Rcp to Assist all trans
                string query1 = "select Rrn, EmtCodeFrm, AzsCode, DocNumber, DocDate, S_DIIS AS Amount_RCP, SUMM1 AS Amount_Assist, Comments FROM " +
                                " (select T1.RRN, T1.EMTCODEFRM, T1.AZSCODE, T1.DOCNUMBER, T1.DOCDATE, T1.S_DIIS, case when T2.AMOUNT>0 then ROUND(T2.AMOUNT,2) else 0 end AS SUMM1," +
                                " case when T2.AMOUNT is null then 'RRN not found in Assist' else null end AS Comments " +
                                "FROM ( SELECT RRN, EMTCODEFRM, AZSCODE, DOCNUMBER, DOCDATE, sum(S_DIIS) AS S_DIIS FROM Rcd.VALID_RCPDATA  WHERE EMTCODETO=300 AND SHIFTDATE>=:1 " +
                                "AND SHIFTDATE<:2 GROUP BY RRN, EMTCODEFRM, AZSCODE, DOCNUMBER,  DOCDATE ) T1 LEFT JOIN " +
                                "( select OrderNumber, sum(Amount) AS AMOUNT from Rcd.VALID_ASSISTDATA_CSV  group by OrderNumber) T2 ON T1.RRN=T2.OrderNumber)  order by Docdate";

                string query2 = "select Rrn, EmtCodeFrm, AzsCode, DocNumber, DocDate, S_DIIS AS Amount_RCP, SUMM1 AS Amount_Assist, Comments FROM " +
                                "(select T1.RRN, T1.EMTCODEFRM, T1.AZSCODE, T1.DOCNUMBER, T1.DOCDATE, T1.S_DIIS, case when T2.AMOUNT>0 then ROUND(T2.AMOUNT,2) else 0 end AS SUMM1," +
                                " case when T2.AMOUNT is null then 'RRN not found in Assist' else null end AS Comments " +
                                "FROM ( SELECT RRN, EMTCODEFRM, AZSCODE, DOCNUMBER, DOCDATE, sum(S_DIIS) AS S_DIIS FROM Rcd.VALID_RCPDATA  WHERE EMTCODETO=300 AND SHIFTDATE>=:1 AND SHIFTDATE<:2 " +
                                "GROUP BY RRN, EMTCODEFRM, AZSCODE, DOCNUMBER,  DOCDATE ) T1 LEFT JOIN ( select OrderNumber, sum(Amount) AS AMOUNT from Rcd.VALID_ASSISTDATA_CSV  group by OrderNumber) " +
                                "T2 ON T1.RRN=T2.OrderNumber) WHERE S_DIIS<>SUMM1  order by Docdate";

                string query3 = "select Rrn, EmtCodeFrm, AzsCode, DocNumber, DocDate, S_DIIS AS Amount_RCP, SUMM1 AS Amount_BGPBMobile, Comments FROM (select T1.RRN, T1.EMTCODEFRM, T1.AZSCODE," +
                   " T1.DOCNUMBER, T1.DOCDATE, T1.S_DIIS, case when T2.AMOUNT > 0 then ROUND(T2.AMOUNT, 2) else 0 end AS SUMM1, case when T2.AMOUNT is null then 'RRN not found in BGPBMobile' " +
                   "else null end AS Comments FROM (SELECT RRN, EMTCODEFRM, AZSCODE, DOCNUMBER, DOCDATE, sum(S_DIIS) AS S_DIIS FROM Rcd.VALID_RCPDATA WHERE EMTCODETO= 305 " +
                   "AND SHIFTDATE>=:1 AND SHIFTDATE<:2 GROUP BY RRN, EMTCODEFRM, AZSCODE, DOCNUMBER,  DOCDATE ) T1 LEFT JOIN (select Rrn, sum(Amount) AS AMOUNT " +
                   "from Rcd.VALID_BGPBMobileData  group by RRN) T2 ON T1.RRN = T2.RRN) order by Docdate";

                string query4 = "select Rrn, EmtCodeFrm, AzsCode, DocNumber, DocDate, S_DIIS AS Amount_RCP, SUMM1 AS Amount_BGPBMobile, Comments FROM (select T1.RRN, T1.EMTCODEFRM, T1.AZSCODE," +
                    " T1.DOCNUMBER, T1.DOCDATE, T1.S_DIIS, case when T2.AMOUNT > 0 then ROUND(T2.AMOUNT, 2) else 0 end AS SUMM1, case when T2.AMOUNT is null then 'RRN not found in BGPBMobile' " +
                    "else null end AS Comments FROM (SELECT RRN, EMTCODEFRM, AZSCODE, DOCNUMBER, DOCDATE, sum(S_DIIS) AS S_DIIS FROM Rcd.VALID_RCPDATA WHERE EMTCODETO= 305 " +
                    "AND SHIFTDATE>=:1 AND SHIFTDATE<:2 GROUP BY RRN, EMTCODEFRM, AZSCODE, DOCNUMBER,  DOCDATE ) T1 LEFT JOIN (select Rrn, sum(Amount) AS AMOUNT " +
                    "from Rcd.VALID_BGPBMobileData  group by RRN) T2 ON T1.RRN = T2.RRN) WHERE S_DIIS<>SUMM1 order by Docdate";

                string query5 = "select Rrn, EmtCodeFrm, AzsCode, DocNumber, DocDate, S_DIIS AS Amount_RCP, SUMM1 AS Amount_BelWeb, Comments FROM (select T1.RRN, T1.EMTCODEFRM, T1.AZSCODE," +
                  " T1.DOCNUMBER, T1.DOCDATE, T1.S_DIIS, case when T2.AMOUNT > 0 then ROUND(T2.AMOUNT, 2) else 0 end AS SUMM1, case when T2.AMOUNT is null then 'RRN not found in BelWeb' " +
                  "else null end AS Comments FROM (SELECT RRN, EMTCODEFRM, AZSCODE, DOCNUMBER, DOCDATE, sum(S_DIIS) AS S_DIIS FROM Rcd.VALID_RCPDATA WHERE EMTCODETO= 306 " +
                  "AND SHIFTDATE>=:1 AND SHIFTDATE<:2 GROUP BY RRN, EMTCODEFRM, AZSCODE, DOCNUMBER,  DOCDATE ) T1 LEFT JOIN (select Rrn, sum(Amount) AS AMOUNT from Rcd.VALID_BelWEBDATA  group by RRN)" +
                  " T2 ON T1.RRN = T2.RRN) order by Docdate";

                string query6 = "select Rrn, EmtCodeFrm, AzsCode, DocNumber, DocDate, S_DIIS AS Amount_RCP, SUMM1 AS Amount_BelWeb, Comments FROM (select T1.RRN, T1.EMTCODEFRM, T1.AZSCODE," +
                    " T1.DOCNUMBER, T1.DOCDATE, T1.S_DIIS, case when T2.AMOUNT > 0 then ROUND(T2.AMOUNT, 2) else 0 end AS SUMM1, case when T2.AMOUNT is null then 'RRN not found in BelWeb' " +
                    "else null end AS Comments FROM (SELECT RRN, EMTCODEFRM, AZSCODE, DOCNUMBER, DOCDATE, sum(S_DIIS) AS S_DIIS FROM Rcd.VALID_RCPDATA WHERE EMTCODETO= 306 " +
                    "AND SHIFTDATE>=:1 AND SHIFTDATE<:2 GROUP BY RRN, EMTCODEFRM, AZSCODE, DOCNUMBER,  DOCDATE ) T1 LEFT JOIN (select Rrn, sum(Amount) AS AMOUNT from Rcd.VALID_BelWEBDATA  group by RRN)" +
                    " T2 ON T1.RRN = T2.RRN) WHERE S_DIIS<>SUMM1 order by Docdate";

                string query7 = "select T2.OrderNumber, T2.AMOUNT, T1.S_DIIS FROM (select OrderNumber, sum(Amount) AS AMOUNT from Rcd.VALID_ASSISTDATA_CSV where SHIFTDATA >= :1 " +
                    " AND SHIFTDATA <=:2)  group by OrderNumber) T2 LEFT JOIN (select RRN, sum(S_DIIS) AS S_DIIS FROM Rcd.VALID_RCPDATA  WHERE EMTCODETO = 300 GROUP BY RRN) T1" +
                    " ON T2.OrderNumber = T1.RRN   ";

                string query8 = "select T2.OrderNumber, T2.AMOUNT, T1.S_DIIS FROM (select OrderNumber, sum(Amount) AS AMOUNT from Rcd.VALID_ASSISTDATA_CSV where SHIFTDATA >= :1 " +
                    " AND SHIFTDATA <=:2)  group by OrderNumber) T2 LEFT JOIN (select RRN, sum(S_DIIS) AS S_DIIS FROM Rcd.VALID_RCPDATA  WHERE EMTCODETO = 300 GROUP BY RRN) T1" +
                    " ON T2.OrderNumber = T1.RRN where T2.AMOUNT<> T1.S_DIIS  ";

                string query9 = "select T2.RRN, T2.AMOUNT, T1.S_DIIS FROM (select RRN, sum(Amount) AS AMOUNT from Rcd.VALID_BGPBMobileData where SHIFTDATE >= :1 " +
                    " AND SHIFTDATE <=:2)  group by RRN) T2 LEFT JOIN (select RRN, sum(S_DIIS) AS S_DIIS FROM Rcd.VALID_RCPDATA  WHERE EMTCODETO = 305 GROUP BY RRN) T1" +
                    " ON T2.RRN = T1.RRN";

                string query10 = "select T2.RRN, T2.AMOUNT, T1.S_DIIS FROM (select RRN, sum(Amount) AS AMOUNT from Rcd.VALID_BGPBMobileData where SHIFTDATE >= :1 " +
                    " AND SHIFTDATE <=:2)  group by RRN) T2 LEFT JOIN (select RRN, sum(S_DIIS) AS S_DIIS FROM Rcd.VALID_RCPDATA  WHERE EMTCODETO = 305 GROUP BY RRN) T1" +
                    " ON T2.RRN = T1.RRN where T2.AMOUNT<> T1.S_DIIS ";

                string query11 = "select T2.RRN, T2.AMOUNT, T1.S_DIIS FROM (select RRN, sum(Amount) AS AMOUNT from Rcd.Valid_BelWEBDATA where SHIFTDATE >= :1 " +
                    " AND SHIFTDATE <=:2)  group by RRN) T2 LEFT JOIN (select RRN, sum(S_DIIS) AS S_DIIS FROM Rcd.VALID_RCPDATA  WHERE EMTCODETO = 306 GROUP BY RRN) T1" +
                    " ON T2.RRN = T1.RRN";

                string query12 = "select T2.RRN, T2.AMOUNT, T1.S_DIIS FROM (select RRN, sum(Amount) AS AMOUNT from Rcd.Valid_BelWEBDATA where SHIFTDATE >= :1 " +
                    " AND SHIFTDATE <=:2)  group by RRN) T2 LEFT JOIN (select RRN, sum(S_DIIS) AS S_DIIS FROM Rcd.VALID_RCPDATA  WHERE EMTCODETO = 306 GROUP BY RRN) T1" +
                    " ON T2.RRN = T1.RRN where T2.AMOUNT<> T1.S_DIIS";

                switch (orientation)
                {
                    case 1:
                        {
                            switch (app_code)
                            {
                                case 300: { if (b) query = query2; else query = query1; } break;
                                case 305: { if (b) query = query4; else query = query3; } break;
                                case 306: { if (b) query = query6; else query = query5; } break;
                                default: msg = string.Format("Unknown app_code: {0}", orientation); return 2;
                            }
                        
                        }
                        break;
                    case 2:
                        {
                            switch (app_code)
                            {
                                case 300: { if (b) query = query8; else query = query7; } break;
                                case 305: { if (b) query = query10; else query = query9; } break;
                                case 306: { if (b) query = query12; else query = query11; } break;
                                default: msg = string.Format("Unknown app_code: {0}", orientation); return 2;
                            }
                        }
                        break;
                    default: msg = string.Format("Unknown orientation code: {0}", orientation); return 1;
                }
            
            }
            catch (Exception ex) { log.LogLine(ex.Message); msg = ex.Message; return -1; }
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
