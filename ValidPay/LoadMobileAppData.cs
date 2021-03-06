﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Data.OracleClient;
using System.IO.Compression;


namespace ValidPay
{
    public struct STFileData
    {
        public string filename;
        public int status;
        public string text;
    }


    public partial class LoadMobileAppData : Form
    {
        CConfig config;
        XLog log;

        List<STFileData> lst_filedata;
        List<string> lst_loadedfile;
        string[] files;

        STFileData curr_file;

        bool reloadflag;
        bool moveflag;

       // DateTime c_date;
        MA m_app;

        object clWork;

        int cr;

        CBGPBmobileData clBGPBmobile;
        CBelWebData clUP;
        CAssistData clAssist;

        public LoadMobileAppData(CConfig cf)
        {
            InitializeComponent();
            config = cf;
        }

        private void LoadMobileAppData_Load(object sender, EventArgs e)
        {
            buttonOK.Enabled = false;
            buttonCancel.Enabled = false;

            log = new XLog();
            log.DirName = config.logpath;

            clBGPBmobile = new CBGPBmobileData(config);
            clUP = new CBelWebData(config);
            clAssist = new CAssistData(config);

            cr = 1;
            init_combo();

            if (config.reloadflag == 1) checkBoxReload.Checked = true;
            else checkBoxReload.Checked = false;

            if (config.moveflag == 1) checkBoxMove.Checked = true;
            else checkBoxMove.Checked = false;
        }

        private void init_combo()
        {
            try
            {
                foreach(MA m_app in config.madata)
                    comboBoxApp.Items.Add(m_app.Name);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message);}
        }

        [Obsolete]
        private void comboBoxApp_SelectedIndexChanged(object sender, EventArgs e)
        {
            string msg = null;
            
            if (comboBoxApp.Text.Length > 0)
            {
                if (get_MA(comboBoxApp.Text.Trim(), out m_app, out msg) != 0) MessageBox.Show(msg);
                else init_proc(m_app);
            }
        }

        private int get_MA(string name, out MA app, out string msg)
        {
            msg = null;
            app = new MA();
            int ret = 0;
            try
            {
                foreach (MA s in config.madata)
                {
                    if (s.Name == name) { app = s; return 0; };
                } 
            }
            catch (Exception ex) { msg = ex.Message; return -1; }
            return ret;
        }

        [Obsolete]
        private void init_proc(MA m_app)
        {
            switch (m_app.Code)
            {
                case 300:
                    lst_loadedfile = clAssist.get_loaded_file();
                    break;
                case 306:
                    lst_loadedfile = clUP.get_loaded_file();
                    break;
                case 305:
                    lst_loadedfile = clBGPBmobile.get_loaded_file();
                    break;

            }

            init_filedata(m_app.PathIN);

            if (files.Length > 0)
                buttonOK.Enabled = true;
        }

        private void init_filedata(string inpath)
        {
            try
            {
                lst_filedata = new List<STFileData>();
                STFileData item;
                files = Directory.GetFiles(inpath); // путь к папке
                foreach (string path in files)
                {
                    item = new STFileData();
                    item.filename = Path.GetFileName(path);
                    item.status = 0;
                    if (is_loaded(item.filename)) item.status = 2;

                    lst_filedata.Add(item);
                    // checkedListBoxFiles.Items.Add(filename, true);
                }

                if (files.Length> 0) init_list();
            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private bool is_loaded(string filename)
        {
            foreach (string file in lst_loadedfile)
                if (file == filename) return true;

            return false;
        }

        private void init_list()
        {
            try
            {
                listViewData.Items.Clear();
                foreach (STFileData item in lst_filedata)
                {
                    AddItemToList(item);
                }
                if(cr>0) listViewData.Items[cr-1].EnsureVisible();
            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void AddItemToList(STFileData data)
        {
            ListViewItem item;
            item = new ListViewItem(data.filename, data.status);
            item.SubItems.Add(data.text);
            listViewData.Items.Add(item);
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                //  labelInfo.Text = "Внимание! После обработки текущего файла процесс записи будет остановлен...";
                backgroundWorker1.CancelAsync();
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            if (checkBoxReload.Checked == true) reloadflag = true;
            else reloadflag = false;

            if (checkBoxMove.Checked == true) moveflag = true;
            else moveflag = false;

            buttonOK.Enabled = false;
            buttonExit.Enabled = false;
            buttonCancel.Enabled = true;

            backgroundWorker1.RunWorkerAsync();
        }

        [Obsolete]
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            
            StartProc(sender, e);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 0 || e.ProgressPercentage == 100) init_list();

            progressBarData.Value = e.ProgressPercentage;
            labelInfo.Text = "Processing.... " + progressBarData.Value.ToString() + "%";
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                labelInfo.Text = "Запись файла прервана пользователем...";
            }

          // Check to see if an error occurred in the background process.

            else if (e.Error != null)
            {
                labelInfo.Text = "Error while performing background operation...";
            }
            else
            {
                // Everything completed normally.
                labelInfo.Text = "Запись файла завершена...";
            }

            buttonCancel.Enabled = false;
            buttonExit.Enabled = true;
            buttonOK.Enabled = true;
        }

        [Obsolete]
        private void StartProc(object sender, DoWorkEventArgs e)
        {
            string msg = null;
            try
            {
                cr = 0;
                foreach (STFileData file in lst_filedata)
                {
                    
                    if (file.status == 0 || (file.status == 2 && reloadflag))
                    {
                        curr_file = file;
                        //c_date = getfiledate(curr_file.filename);
                        switch(m_app.Code)
                        {
                            case 300:
                                {
                                    List<STRowAssistDataCsv> container1 = new List<STRowAssistDataCsv>();
                                    if (clAssist.ReadFile(file.filename, out container1, out msg) != 0) { log.LogLine(msg); lst_filedata = change_status_file(file.filename, 1, msg); }
                                    else if (load_file1(sender, e, container1, file.filename, out msg) != 0) log.LogLine(msg);
                                }
                                break;
                            case 305:
                                {
                                    List<BGPBmobileRow> container3 = new List<BGPBmobileRow>();
                                    if (clBGPBmobile.ReadFile(file.filename, out container3, out msg) != 0) { log.LogLine(msg); lst_filedata = change_status_file(file.filename, 1, msg); }
                                    else if (load_file3(sender, e, container3, out msg) != 0) log.LogLine(msg);
                                }
                                break;
                            case 306:
                                {
                                    JFile jfile = new JFile();
                                    if (clUP.ReadFile(file.filename, out jfile, out msg) != 0) { log.LogLine(msg); lst_filedata = change_status_file(file.filename, 1, msg); }
                                    else if (load_file2(sender, e, jfile, file.filename, out msg) != 0) log.LogLine(msg);
                                }
                                break;
                        }

                        
                    }

                    cr++;
                }

                if (moveflag) MoveFiles(m_app.PathIN, m_app.PathArch);
            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        [Obsolete]
        private int load_file1(object sender, DoWorkEventArgs e, List<STRowAssistDataCsv> container, string filename, out string msg)
        {
            msg = null;

            try
            {
                using (OracleConnection connection = new OracleConnection(config.connectionstring))
                {
                    connection.Open();

                    lst_filedata = change_status_file(curr_file.filename, 3, "запись файла ...");
                    backgroundWorker1.ReportProgress(0);

                    int pc = 0;
                    int i = 0;
                    int cnt = container.Count;

                    if (container.Count <= 0) { msg = "Пустой файл"; return 1; }

                    clAssist.DeleteFile(filename, out msg);

                    int cntrows = 0;

                    foreach (STRowAssistDataCsv item in container)
                    {
                        if (backgroundWorker1.CancellationPending)
                        {
                            e.Cancel = true;
                            backgroundWorker1.ReportProgress(0);
                            return 3;
                        }

                        clAssist.InsertRow(connection, item, out msg);

                        i += 1;
                        double d = ((i * 1.0) / cnt) * 100;

                        if (d < 100) backgroundWorker1.ReportProgress((int)d);

                        cntrows++;

                    }

                    lst_filedata = change_status_file(curr_file.filename, 2, string.Format("ок ({0} rows)", cntrows));
                }
                backgroundWorker1.ReportProgress(100);
            }
            catch (Exception ex) { msg = ex.Message; return -1; }
            return 0;
        }

        [Obsolete]
        private int load_file2(object sender, DoWorkEventArgs e, JFile jfile, string filename, out string msg)
        {
            msg = null;

            try
            {
                OracleConnection connection = new OracleConnection(config.connectionstring);
                connection.Open();

                lst_filedata = change_status_file(curr_file.filename, 3, "запись файла ...");
                backgroundWorker1.ReportProgress(0);


                int pc = 0;
                int i = 0;
                int cnt = jfile.rows.Count;

                if (cnt <= 0) { msg = "Пустой файл"; return 1; }

                clUP.DeleteFile(filename, out msg);

                int cntrows = 0;

                foreach (UPRow item in jfile.rows)
                {
                    if (backgroundWorker1.CancellationPending)
                    {
                        e.Cancel = true;
                        backgroundWorker1.ReportProgress(0);
                        return 3;
                    }

                    clUP.InsertRow(connection, item, jfile.orderNumber, filename, out msg);

                    i += 1;
                    double d = ((i * 1.0) / cnt) * 100;

                    if (d < 100) backgroundWorker1.ReportProgress((int)d);

                    cntrows++;

                }

                lst_filedata = change_status_file(curr_file.filename, 2, string.Format("ок ({0} rows)", cntrows));
                backgroundWorker1.ReportProgress(100);
            }
            catch (Exception ex) { msg = ex.Message; return -1; }
            return 0;
        }

        [Obsolete]
        private int load_file3(object sender, DoWorkEventArgs e, List<BGPBmobileRow> container, out string msg)
        {
            msg = null;

            try
            {
                using (OracleConnection connection = new OracleConnection(config.connectionstring))
                {
                    connection.Open();

                    lst_filedata = change_status_file(curr_file.filename, 3, "запись файла ...");
                    backgroundWorker1.ReportProgress(0);


                    int pc = 0;
                    int i = 0;
                    int cnt = container.Count;

                    if (container.Count <= 0) { msg = "Пустой файл"; return 1; }

                    clBGPBmobile.DeleteFile(out msg);

                    int cntrows = 0;

                    foreach (BGPBmobileRow item in container)
                    {
                        if (backgroundWorker1.CancellationPending)
                        {
                            e.Cancel = true;
                            backgroundWorker1.ReportProgress(0);
                            return 3;
                        }

                        clBGPBmobile.InsertRow(connection, item, out msg);

                        i += 1;
                        double d = ((i * 1.0) / cnt) * 100;

                        if (d < 100) backgroundWorker1.ReportProgress((int)d);

                        cntrows++;

                    }

                    lst_filedata = change_status_file(curr_file.filename, 2, string.Format("ок ({0} rows)", cntrows));
                }
                backgroundWorker1.ReportProgress(100);
            }
            catch (Exception ex)   { msg = ex.Message; return -1;}
            return 0;
        }


        private void MoveFiles(string pathin, string pathout)
        {
            try
            {
                string zipname = string.Format("{0}_{1}.zip", m_app.Name, DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
                string pathzip = Path.Combine(pathout, zipname);
                ZipFile.CreateFromDirectory(pathin, pathzip);

                DirectoryInfo myDirInfo = new DirectoryInfo(pathin);

                foreach (FileInfo file in myDirInfo.GetFiles())
                {
                    file.Delete();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private List<STFileData> change_status_file(string filename, int status, string text)
        {
            List<STFileData> ret = new List<STFileData>();
            STFileData item;
            foreach (STFileData file in lst_filedata)
            {
                if (file.filename != filename) ret.Add(file);
                else
                {
                    item = new STFileData();
                    item.filename = file.filename;
                    item.status = status;
                    item.text = text;
                    ret.Add(item);
                }

            }
            return ret;
        }
    }
}
