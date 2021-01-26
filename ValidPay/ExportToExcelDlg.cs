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
    public partial class ExportToExcelDlg : Form
    {
        CConfig config; 
        DataTable table;
        List<string> arr_cnames;
        string FN;

        public ExportToExcelDlg(CConfig con, DataTable tb, string fn)
        {
            InitializeComponent();
            config = con;
            table = tb;
            FN = fn;
        }


        private void ExportToExcelDlg_Load(object sender, EventArgs e)
        {
            try
            {
                textBoxPath.Text = config.outpath;
                textBoxFileName.Text = FN + "_" + table.TableName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");
                checkBoxShow.Checked = true;
                textBoxFileName.Focus();
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.Source); }
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            if (!string.IsNullOrEmpty(folderBrowserDialog1.SelectedPath))
                textBoxPath.Text = folderBrowserDialog1.SelectedPath;
        }

        private void ExportTable()
        {
            Excel.Range range;
            string cell;
            Excel._Application app = new Excel.Application();
            

            try
            {


                string dir = textBoxPath.Text.Trim();
                if (!dir.Contains('\\'))
                 dir = Path.Combine(Environment.CurrentDirectory, dir);
                
                if (!Directory.Exists(dir)) Directory.CreateDirectory(textBoxPath.Text.Trim());

                string excelFilePath = Path.Combine(dir, textBoxFileName.Text);
                app = new Excel.Application();
                app.SheetsInNewWorkbook = 1;
                Excel._Workbook book = app.Workbooks.Add(Missing.Value);
                Excel.Sheets sheets = book.Worksheets;
                Excel._Worksheet sheet;
                sheet = (Excel._Worksheet)sheets.get_Item(1);
                sheet.Name = "Data";
               // sheet.Cells.NumberFormat = "@";

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
                        sheet.Cells.NumberFormat = "@";
                        Type t = table.Rows[i][j].GetType();
                        if (t.Name == "Decimal") sheet.Cells.NumberFormat = "#,##0.00";
                        sheet.Cells[i + 2, j + 1] = table.Rows[i][j];
                    }
                }

                book.SaveAs(excelFilePath, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                                     Missing.Value, Excel.XlSaveAsAccessMode.xlExclusive, Missing.Value,
                                     Missing.Value, Missing.Value, Missing.Value, Missing.Value);
                app.UserControl = true;
                if (checkBoxShow.Checked == true)
                    app.Visible = true;
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, ex.Source); }
            finally { GC.Collect(); }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            ExportTable();
        }
    }
}
