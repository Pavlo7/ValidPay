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
    /* Эмитент */
    public struct STEmitent
    {
        public int code;                /* код эмитента */
        public string name;             /* наименование эмитента */
        public string address;          /* адрес */
        public string phone;            /* телефон */
        public string postindex;        /* почтовый индекс */
        public string email;            /* эл. почта */
        public int? bankcode;           /* код банка */
        public string account;          /* расчётный счёт */
        public int? deptcode;           /* код министерства */
        public string OKPO;             /* шифр ОКПО */
        public string UNN;              /* шифр УНН */
        public string OKED;             /* шифр ОКЭД */
        public int? transcode;          /* код для кодирования эмитента */
        public DateTime? periodbegin;   /* начало рабочего периода */
        public DateTime? periodend;     /* окончание рабочего периода */
        public string directname;       /* фио директора */
        public string buchname;         /* фио глав. гухгалтера */
        public int? CBScode;            /* номер ЦБУ */
        public string c_string;         /* сторка подключения к БД РЦП данного эмитента */
        public string shortname;        /* краткое наименование эмитента */
    };
    

    public class CEmitent
    {
        CConfig config;
        XLog log;

        public CEmitent(CConfig cf)
        {
            config = cf; 
            log = new XLog();
            log.DirName = config.logpath;
        }

        public List<STEmitent> GetData(int trcode)
        {
            List<STEmitent> list_data = new List<STEmitent>();

            STEmitent item;
            OracleConnection connet = new OracleConnection();

            try
            {
                connet = new OracleConnection(config.connectionstring);
                connet.Open();
                if (connet.State != ConnectionState.Open) { log.LogLine(string.Format("No connection to DB Oracle. CS: {0}", config.connectionstring)); return list_data;}

                string query = string.Format("SELECT EmtCode,EmtName,Address,Phone,PostIndex,E_mail,BankCode,Account, " +
                    "DeptCode,Shifr_OKPO,Shifr_UNN,Shifr_OKED,TransCode,PeriodBeg,PeriodEnd,DirectName," +
                    "BuchName,CBSCode,ConnectionString,EmtShortName FROM {0}.Emitent WHERE TransCode={1} ORDER BY EmtCode",
                    "RCD", trcode);

                OracleCommand cmd = new OracleCommand(query, connet);
                OracleDataReader reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        item = new STEmitent();

                        item.code = reader.GetInt32(0);
                        item.name = reader["EmtName"].ToString().Trim();
                        item.address = reader["Address"].ToString().Trim();
                        item.phone = reader["Phone"].ToString().Trim();
                        item.postindex = reader["PostIndex"].ToString().Trim();
                        item.email = reader["E_mail"].ToString().Trim();
                        if (!reader.IsDBNull(6))
                            item.bankcode = reader.GetInt32(6);
                        item.account = reader["Account"].ToString().Trim();
                        if (!reader.IsDBNull(8))
                            item.deptcode = reader.GetInt32(8);
                        item.OKPO = reader["Shifr_OKPO"].ToString().Trim();
                        item.UNN = reader["Shifr_UNN"].ToString().Trim();
                        item.OKED = reader["Shifr_OKED"].ToString().Trim();
                        if (!reader.IsDBNull(12))
                            item.transcode = reader.GetInt32(12);
                        if (!reader.IsDBNull(13))
                            item.periodbegin = reader.GetDateTime(13);
                        if (!reader.IsDBNull(14))
                            item.periodend = reader.GetDateTime(14);
                        item.directname = reader["DirectName"].ToString().Trim();
                        item.buchname = reader["BuchName"].ToString().Trim();
                        if (!reader.IsDBNull(17))
                            item.CBScode = reader.GetInt32(17);
                        if (!reader.IsDBNull(18))
                            item.c_string = reader.GetString(18);
                        else item.c_string = null;
                        if (!reader.IsDBNull(19))
                            item.shortname = reader.GetString(19);
                        else item.shortname = null;

                        list_data.Add(item);
                    }
                }

                reader.Dispose();
            }
            catch (Exception ex) { log.LogLine(ex.Message); if (connet.State == ConnectionState.Open) connet.Close();  }

            return list_data;
        }
    }
}
