using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ValidPay
{
    public partial class RCPViewData : Form
    {
        CConfig config;
        XLog log;
        STVPRCPData param;
        DataTable table;
        StatmetProc process;
        TamplateGrid tamplategrid;

        CRcpData clWork;

        int s_index;
        int iRow;

        public RCPViewData(CConfig cf)
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            config = cf;
        }

        private void RCPViewData_Load(object sender, EventArgs e)
        {
            try
            {
                log = new XLog();
                log.DirName = config.logpath;
                clWork = new CRcpData(config);

                param = new STVPRCPData();

                param.dtbegin = new DateTime(2020,07,11,1,0,0,0);
                param.dtend = DateTime.Now;
                OnPermit("MAINRCP");


            }
            catch (Exception ex) { MessageBox.Show(ex.TargetSite + " " + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void OnPermit(string tamplate)
        {
            string msg;
            try
            {
                tamplategrid = new TamplateGrid(tamplate);
                CreateGrid(tamplategrid.arr_columns);

                splitContainer1.Panel1.Enabled = false;

                iRow = 0;

                toolStripProgressBarData.Value = 0;
                toolStripStatusLabelPC.Text = "";
                toolStripStatusLabelRow.Text = "";
                toolStripStatusLabelTime.Text = "";

                splitContainer1.Panel1.Enabled = false;

    //            read_param();

                process = new StatmetProc(clWork, param, true);
                if (process.ShowDialog() == DialogResult.Cancel)
                {
                    splitContainer1.Panel1.Enabled = true;
                    toolStripStatusLabelTime.Text = "Отмена";
                }
                else
                {
                    string s = process.sTime;
                    toolStripStatusLabelTime.Text = s;

                    table = process.oData as DataTable;

                    iRow = table.Rows.Count;

                    if (iRow > 0)
                    {
                        dataGridViewBT.Rows.Add(iRow);
                        backgroundWorkerGrid.RunWorkerAsync();
                    }
                    else
                    {
                        splitContainer1.Panel1.Enabled = true;
                        on_print();
                    }
                    //splitContainer1.Panel1.Enabled = true;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.Source); }
        }

        private void read_param()
        {
            param = new STVPRCPData();
        }

        private void CreateGrid(List<ColumnData> tamplate)
        {
            DataGridViewColumn dcol;
            DataGridViewCell cell;
            try
            {
                dataGridViewBT.Columns.Clear();

                foreach (ColumnData col in tamplate)
                {
                    dcol = new DataGridViewColumn();
                    dcol.Name = col.columnname;
                    dcol.HeaderText = col.headername;
                    dcol.Width = col.size;
                    cell = new DataGridViewTextBoxCell();
                    dcol.CellTemplate = cell;
                    if (col.sortmode == "a")
                        dcol.SortMode = DataGridViewColumnSortMode.Automatic;
                    else dcol.SortMode = DataGridViewColumnSortMode.Programmatic;
                    switch (col.alignment)
                    {
                        case "r": dcol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight; break;
                        case "l": dcol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft; break;
                        case "c": dcol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter; break;
                        default: dcol.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft; break;
                    }

                    dataGridViewBT.Columns.Add(dcol);
                }
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

                    // print_row(i, table.Rows[i]);
                    string st = null;
                    for (int rw = 0; rw < table.Columns.Count; rw++)
                    {

                        // if (curTid == table.Rows[i][0].ToString()) s_index = i;

                        st = null;

                        if (dataGridViewBT.InvokeRequired)
                        {
                            Type t = table.Rows[i][rw].GetType();
                            switch (t.Name)
                            {
                                case "String":
                                    {
                                        st = table.Rows[i][rw].ToString();

                                        string format = get_format(table.Columns[rw].ColumnName);
                                        if (!string.IsNullOrEmpty(format))
                                        {
                                            if (format.StartsWith("f"))
                                            {
                                                decimal dc;
                                                decimal.TryParse(st.Replace(".", ","), out dc);
                                                st = dc.ToString(format);
                                            }
                                        }

                                        break;
                                    }
                                case "DateTime":
                                    {
                                        DateTime dt = (DateTime)table.Rows[i][rw];
                                        st = dt.ToString(get_format(table.Columns[rw].ColumnName));
                                        break;
                                    }
                                case "Decimal":
                                    {
                                        Decimal dc = (Decimal)table.Rows[i][rw];
                                        st = dc.ToString(get_format(table.Columns[rw].ColumnName));
                                        break;
                                    }
                                default: st = table.Rows[i][rw].ToString(); break;

                            }
                            //   if (t.Equals())

                            string colname = table.Columns[rw].ColumnName;
                            dataGridViewBT.Invoke(new Action<string>((s) =>
                                dataGridViewBT.Rows[i].Cells[colname].Value = st), st);
                        }
                    }

                    pc = ((i + 1) * 1.0 / iRow) * 100;

                    backgroundWorkerGrid.ReportProgress((int)pc);
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.Source); }
        }

        private string get_format(string columnname)
        {
            string ret = null;
            try
            {
                foreach (ColumnData cd in tamplategrid.arr_columns)
                {
                    if (cd.columnname == columnname) return cd.format;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.Source); }
            return ret;
        }

        private void on_print()
        {
            try
            {
                if (iRow > 0)
                {
                    int index = dataGridViewBT.CurrentCell.RowIndex;
                    toolStripStatusLabelRow.Text = string.Format("cтрока {0} из {1}", index + 1, iRow);
                }
                else toolStripStatusLabelRow.Text = string.Format("нет данных");

                statusStrip1.Refresh();
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

        private void complete()
        {
            int rowIndex = -1;

            try
            {

                splitContainer1.Panel1.Enabled = true;
                //        dataGridViewBER.AutoResizeColumns();

                // выделяем нужную строку
                // foreach (DataGridViewRow row in dataGridViewBT.Rows)
                // {

                //    string rc = row.Cells["TID"].Value.ToString();
                //    if (row.Cells["TID"].Value.ToString().Equals(curTid))
                //    {
                //        rowIndex = row.Index;
                //        break;
                //    }
                // }

                if (rowIndex >= 0)
                {
                    dataGridViewBT.Rows[rowIndex].Selected = true;
                    dataGridViewBT.FirstDisplayedScrollingRowIndex = rowIndex;
                }


                dataGridViewBT.AllowUserToAddRows = false;

                on_print();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.Source); }
        }

        private void to_excel()
        {
            try
            {
                if (iRow > 0)
                {
                    ExportToExcelDlg dlg = new ExportToExcelDlg(config, table, "AssistMain");
                    dlg.ShowDialog();
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.Source); }
        }

        private void ExcelToolStripMenuItemExportToExcelMenu_Click(object sender, EventArgs e)
        {
            to_excel();
        }

        private void ExcelToolStripMenuItemExportToExcel_Click(object sender, EventArgs e)
        {
            to_excel();
        }

        private void dataGridViewBT_SelectionChanged(object sender, EventArgs e)
        {
            on_print();
        }

    }
}
