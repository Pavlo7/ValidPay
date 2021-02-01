using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.IO;
using System.Data;
using System.Data.OracleClient;

namespace ValidPay
{
    public struct MA
    {
        public string Name;
        public int Code;
        public string PathIN;
        public string PathArch;
    }

    public class ConfigFields
    {
        public string ConnectionString;
        public string LogPath;
        public string InPath;
        public string OutPath;
        public string InPathAssistsFile;
        public string InPathAssistsCsvFile;
        public string InPathBelWebFile;
        public string ArchPathAssistsFile;
        public string ArchPathAssistsCsvFile;
        public string ArchPathBelWebFile;
        public string InPathPayment;
        public string InPathAmountPC300;
        public int ReloadFile;
        public int MoveFile;
        public List<MA> MADATA;
    }

    public class CConfig
    {
        public string connectionstring;
        public string logpath;
        public string inpath;
        public string outpath;
        public string inpathassist;
        public string inpathassistcsv;
        public string inpathbelweb;
        public string archpathassist;
        public string archpathassistcsv;
        public string archpathbelweb;
        public string inpathpayment;
        public string inpathamountpc300;
        public int reloadflag;
        public int moveflag;
        public List<MA> madata;

        public bool bTC;

        public ConfigFields Fields;
       
        // XLog log;

        public CConfig() 
        {
            try
            {
                bTC = false;
                if (read_xml())
                {
                    connectionstring = Fields.ConnectionString;
                    logpath = Fields.LogPath;
                    inpath = Fields.InPath;
                    outpath = Fields.OutPath;
                    inpathassist = Fields.InPathAssistsFile;
                    inpathbelweb = Fields.InPathBelWebFile;
                    inpathassistcsv = Fields.InPathAssistsCsvFile;
                    archpathassist = Fields.ArchPathAssistsFile;
                    archpathbelweb = Fields.ArchPathBelWebFile;
                    archpathassistcsv = Fields.ArchPathAssistsCsvFile;
                    inpathpayment = Fields.InPathPayment;
                    inpathamountpc300 = Fields.InPathAmountPC300;
                    reloadflag = Fields.ReloadFile;
                    moveflag = Fields.MoveFile;
                    madata = Fields.MADATA;

                    bTC = try_connect(connectionstring);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            
        }
        
        public bool read_xml()
        {
            try
            {
                string PathSettings = Path.Combine(Environment.CurrentDirectory, "settings.cfg");
                if (File.Exists(PathSettings))
                {
                    XmlSerializer ser = new XmlSerializer(typeof(ConfigFields));
                    TextReader reader = new StreamReader(PathSettings, Encoding.Default);
                    Fields = ser.Deserialize(reader) as ConfigFields;
                    reader.Close();
                }
                else { MessageBox.Show(string.Format("Отсутствует файл конфигурации: {0}", PathSettings), "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); return false; }
            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            return true;
        }

        private bool try_connect(string cs)
        {
            bool ret = false;
            try
            {
                using (OracleConnection connet = new OracleConnection(cs))
                {
                    connet.Open();
                    if (connet.State == ConnectionState.Open)
                    {
                        ret = true;
                        connet.Close();
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            return ret;
        }
    }
}
