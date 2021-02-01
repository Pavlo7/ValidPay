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
    public struct STVPBelWebData
    {
        public string tamplate;
    }

    class CBelWebData
    {
        CConfig config;
        XLog log;

        public CBelWebData(CConfig cf)
        {
            config = cf; 
            log = new XLog();
            log.DirName = config.logpath;
        }

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
