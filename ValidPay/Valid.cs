using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;                // For Missing.Value and BindingFlags
using System.Runtime.InteropServices;   // For COMException
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;


namespace ValidPay
{
    public partial class Valid : Form
    {
        CConfig config;
        XLog log;

        CValidData clWork;
        STValidDataParam param;
        int s_index;
        int iRow;

        StatmetProc process;

        List<STValidData> list;

        public Valid(CConfig cf)
        {
            InitializeComponent();
            config = cf;
            log = new XLog();
            log.DirName = config.logpath;
        }

        private void Valid_Load(object sender, EventArgs e)
        {
            try
            {
                clWork = new CValidData(config);

                this.WindowState = FormWindowState.Maximized;

                DateTime dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1, 23, 59, 59, 0);
                DateTime dtend = dt.AddDays(-1);
                DateTime dtbegin = new DateTime(dtend.Year, dtend.Month, 1, 0, 0, 0, 0);

                dateTimePickerDB.Value = dtbegin;
                dateTimePickerDE.Value = dtend;

                radioButton1.Checked = true;
                radioButtonDuble.Checked = true;

                s_index = -1;
                dataGridViewValid.AllowUserToAddRows = false;

                iRow = 0;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.Source); }
        }

        private void read_param()
        {
            try
            {
                param = new STValidDataParam();

                param.dtbegin = new DateTime(dateTimePickerDB.Value.Year, dateTimePickerDB.Value.Month, dateTimePickerDB.Value.Day, 0, 0, 0, 0);
                param.dtend = new DateTime(dateTimePickerDE.Value.Year, dateTimePickerDE.Value.Month, dateTimePickerDE.Value.Day, 23, 59, 59, 0);


                if (radioButtonDuble.Checked == true) param.type = 1;
                if (radioButtonNotEq.Checked == true) param.type = 2;
                if (radioButtonRCPAssist.Checked == true) param.type = 3;

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.Source); }
        }

        private void init_data(object sender, DoWorkEventArgs e)
        {
            try
            {
                double pc = 0;
                int i = 0;

                for (i = 0; i < iRow; i++)
                {
                    if (backgroundWorkerGrid.CancellationPending)
                    {
                        e.Cancel = true;
                        return;
                    }

                    print_row(i, list[i]);

                    pc = ((i + 1) * 1.0 / iRow) * 100;

                    backgroundWorkerGrid.ReportProgress((int)pc);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.Source); }
        }

        private void print_row(int i, STValidData data)
        {
            try
            {
                if (dateTimePickerDB.InvokeRequired)
                {
                    dataGridViewValid.Invoke(new Action<string>((s) => dataGridViewValid.Rows[i].Cells[0].Value = data.RRN.ToString()), data.RRN.ToString());
                    dataGridViewValid.Invoke(new Action<string>((s) => dataGridViewValid.Rows[i].Cells[1].Value = data.DOCDATE.ToString("yyyy-MM-dd HH:mm:ss")), data.DOCDATE.ToString("yyyy-MM-dd HH:mm:ss"));
                    dataGridViewValid.Invoke(new Action<string>((s) => dataGridViewValid.Rows[i].Cells[2].Value = data.EMTCODEFRM.ToString()), data.EMTCODEFRM.ToString());
                    dataGridViewValid.Invoke(new Action<string>((s) => dataGridViewValid.Rows[i].Cells[3].Value = data.AZSCODE.ToString()), data.AZSCODE.ToString());
                    dataGridViewValid.Invoke(new Action<string>((s) => dataGridViewValid.Rows[i].Cells[4].Value = data.S_DIIS.ToString("f2")), data.S_DIIS.ToString("f2"));
                    dataGridViewValid.Invoke(new Action<string>((s) => dataGridViewValid.Rows[i].Cells[5].Value = data.amount.ToString("f2")), data.amount.ToString("f2"));
                    dataGridViewValid.Invoke(new Action<string>((s) => dataGridViewValid.Rows[i].Cells[6].Value = data.comments), data.comments);
                }
                else
                {
                    dataGridViewValid.Rows[i].Cells[0].Value = data.RRN;
                    dataGridViewValid.Rows[i].Cells[1].Value = data.DOCDATE.ToString("yyyy-MM-dd HH:mm:ss");
                    dataGridViewValid.Rows[i].Cells[2].Value = data.EMTCODEFRM.ToString();
                    dataGridViewValid.Rows[i].Cells[3].Value = data.AZSCODE.ToString();
                    dataGridViewValid.Rows[i].Cells[4].Value = data.S_DIIS.ToString("f2");
                    dataGridViewValid.Rows[i].Cells[5].Value = data.amount.ToString("f2");
                    dataGridViewValid.Rows[i].Cells[6].Value = data.comments;
                }
            }

            catch (Exception ex) { MessageBox.Show(ex.Message, ex.Source); }
        }

        private void complete()
        {
            try
            {
                dataGridViewValid.AutoResizeColumns();
                dataGridViewValid.AllowUserToAddRows = false;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.Source); }
        }

        private void on_print()
        {
            try
            {
                if (iRow > 0)
                {
                    int index = dataGridViewValid.CurrentCell.RowIndex;
                    toolStripStatusLabelRow.Text = string.Format("cтрока {0} из {1}", index + 1, iRow);
                }
                else toolStripStatusLabelRow.Text = string.Format("нет данных");

                statusStrip1.Refresh();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.Source); }
        }

        private void buttonPermit_Click(object sender, EventArgs e)
        {
            try
            {
                read_param();

                dataGridViewValid.Rows.Clear();

                iRow = 0;

                toolStripProgressBarData.Value = 0;
                toolStripStatusLabelPC.Text = "";
                toolStripStatusLabelRow.Text = "";
                toolStripStatusLabelTime.Text = "";

                panel1.Enabled = false;

                process = new StatmetProc(clWork, param, true);
                if (process.ShowDialog() == DialogResult.Cancel)
                {
                    panel1.Enabled = true;
                    toolStripStatusLabelTime.Text = "Отмена";
                }
                else
                {
                    string s = process.sTime;
                    toolStripStatusLabelTime.Text = s;
                    list = process.oData as List<STValidData>;
                    iRow = list.Count;

                    ListCompareBPRound_Averpcall clC = new ListCompareBPRound_Averpcall();
                    list.Sort(clC);

                    if (iRow > 0)
                    {
                        dataGridViewValid.Rows.Add(iRow);
                        backgroundWorkerGrid.RunWorkerAsync();
                    }
                 //   else
                 //   {
                 //       on_print();
                        //            complete();
                //    }

                    on_print();
                    panel1.Enabled = true;
                }

            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.Source); }
        }

        private void backgroundWorkerGrid_DoWork(object sender, DoWorkEventArgs e)
        {
            init_data(sender, e);
        }

        private void backgroundWorkerGrid_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage > 0)
            {
                toolStripProgressBarData.Value = e.ProgressPercentage;
                toolStripStatusLabelPC.Text = toolStripProgressBarData.Value.ToString() + "%";
            }
        }

        private void backgroundWorkerGrid_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            complete();
        }

        private void экспортВExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnExcel();
        }

        private void OnExcel()
        {
            DataTable table = new DataTable();
            for (int iCol = 0; iCol < dataGridViewValid.Columns.Count; iCol++)
            {
                table.Columns.Add(dataGridViewValid.Columns[iCol].Name);
            }

            foreach (DataGridViewRow row in dataGridViewValid.Rows)
            {

                DataRow datarw = table.NewRow();

                for (int iCol = 0; iCol < dataGridViewValid.Columns.Count; iCol++)
                {
                    datarw[iCol] = row.Cells[iCol].Value;
                }

                table.Rows.Add(datarw);
            }

            if (table.Rows.Count > 0) ExportTable(table);
        }

        private void ExportTable(DataTable table)
        {
            Excel.Range range;
            string cell;
            Excel._Application app = new Excel.Application();


            try
            {
                string dir = Path.Combine(Environment.CurrentDirectory, "Report");
                if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
                
                string excelFilePath = Path.Combine(dir, string.Format("Valid_RCP_to_Assist_{0}", DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss")));
                app = new Excel.Application();
                app.SheetsInNewWorkbook = 1;
                Excel._Workbook book = app.Workbooks.Add(Missing.Value);
                Excel.Sheets sheets = book.Worksheets;
                Excel._Worksheet sheet;
                sheet = (Excel._Worksheet)sheets.get_Item(1);
                sheet.Name = "Data";
                sheet.Cells.NumberFormat = "@";

                app.ActiveWindow.SplitRow = 1;
                app.ActiveWindow.FreezePanes = true;

                for (var i = 0; i < table.Columns.Count; i++)
                {
                    // sheet.Cells[1, i + 1] = arr_cnames[i];
                    sheet.Cells[1, i + 1] = table.Columns[i].ColumnName;
                }


                for (var i = 0; i < table.Rows.Count; i++)
                {
                    // to do: format datetime values before printing
                    for (var j = 0; j < table.Columns.Count; j++)
                    {
                        sheet.Cells[i + 2, j + 1] = table.Rows[i][j].ToString();
                    }
                }

                book.SaveAs(excelFilePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                     Missing.Value, Excel.XlSaveAsAccessMode.xlExclusive, Missing.Value,
                                     Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                app.UserControl = true;
                
                app.Visible = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.Source); }
            finally { GC.Collect(); }
        }

        private void ExcelToolStripMenuItemExcel_Click(object sender, EventArgs e)
        {
            OnExcel();
        }
       
    }




    public class ListCompareBPRound_Averpcall : IComparer<STValidData>
    {
        public int Compare(STValidData x, STValidData y)
        {
            if (x.DOCDATE > y.DOCDATE) return -1;
            if (x.DOCDATE < y.DOCDATE) return 1;
            if (x.DOCDATE == y.DOCDATE)   return 0;
            
            return 0;
        }

    };
}
