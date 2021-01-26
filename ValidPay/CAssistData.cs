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
    public struct STVPAssistData
    {
        public string tamplate;
    }

    class CAssistData
    {
        CConfig config;
        XLog log;

        public CAssistData(CConfig cf)
        {
            config = cf; 
            log = new XLog();
            log.DirName = config.logpath;
        }

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
    }
}
