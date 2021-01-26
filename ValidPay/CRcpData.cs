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
    public struct STVPRCPData
    {
        public DateTime dtbegin;
        public DateTime dtend;
        public string tamplate;
    }

    public class CRcpData
    {
        CConfig config;
        XLog log;

        public CRcpData(CConfig cf)
        {
            config = cf; 
            log = new XLog();
            log.DirName = config.logpath;
        }

        public int GetTable(STVPRCPData param, out DataTable table, out string msg)
        {
            int ret = 0;
            msg = null;
            OracleConnection connet = new OracleConnection();

            table = new DataTable("RCPMAIN");
            
            try
            {
                connet = new OracleConnection(config.connectionstring);
                connet.Open();
                if (connet.State != ConnectionState.Open) { log.LogLine(string.Format("No connection to DB Oracle. CS: {0}", config.connectionstring)); return 1; }

                string query = string.Format(" SELECT T1.dd,T1.AmountRCP, T2.AmountPC, T1.AmountRCP-T2.AmountPC AS Diff FROM ( SELECT TRUNC(DOCDATE) AS DD, sum(S_DIIS) AS AmountRCP " +
                    "FROM Rcd.VALID_RCPDATA  WHERE EMTCODETO=300 AND  DOCDATE>=:1 AND DOCDATE<=:2 group by TRUNC(DOCDATE) ) T1 " +
                    "LEFT JOIN (SELECT PDATE, AMOUNT AS AmountPC FROM Rcd.VALID_AMOUNT_PC WHERE APPCODE=300) T2  ON T1.DD=T2.PDATE order by T1.DD ");

                OracleCommand cmd = new OracleCommand(query, connet);
                cmd.Parameters.Add(crp(OracleType.DateTime, param.dtbegin, "1", false));
                cmd.Parameters.Add(crp(OracleType.DateTime, param.dtend, "2", false));
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(table);
            }
            catch (Exception ex) { log.LogLine("CRcpData.GetTable() " + ex.Message); ret = -1; msg = ex.Message; }
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
