using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace ValidPay
{
    public partial class LoadWebPayFileProcessing : Form
    {
        public LoadWebPayFileProcessing()
        {
            InitializeComponent();
        }

        private void LoadWebPayFileProcessing_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = "Подготовка к записи файла...";

                backgroundWorker1.RunWorkerAsync();
            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            StartProc(sender, e);
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            
            //AddItemToList(fileinfo);
           
                progressBarData.Value = e.ProgressPercentage;
                this.Text = "Processing.... " + progressBarData.Value.ToString() + "%";
           
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                this.Text = "Запись файла прервана пользователем...";
            }

           // Check to see if an error occurred in the background process.

            else if (e.Error != null)
            {
                this.Text = "Error while performing background operation...";
            }
            else
            {
                // Everything completed normally.
                this.Text = "Запись файла завершена...";
            }

            progressBarData.Value = 0;
            progressBarData.Refresh();
        }

        private void StartProc(object sender, DoWorkEventArgs e)
        {
            
            int pc = 0;
            
            while (pc < 100)
            {
                if (backgroundWorker1.CancellationPending)
                {
                    // Set the e.Cancel flag so that the WorkerCompleted event
                    // knows that the process was cancelled.
                    e.Cancel = true;
                    backgroundWorker1.ReportProgress(0);
                    return;
                }

                Thread.Sleep(100);
                backgroundWorker1.ReportProgress(pc);
                pc++;
            } 
           
        }

        private void LoadWebPayFileProcessing_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker1.IsBusy)
            {
                this.Text = "Внимание! После обработки текущего файла процесс записи будет остановлен...";
                e.Cancel = true;
          //      backgroundWorker1.CancelAsync();
            }
        }
    }
}
