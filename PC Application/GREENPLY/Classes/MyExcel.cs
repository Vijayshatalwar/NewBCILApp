using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using Microsoft.Office.Interop.Excel;
using System.IO;
using System.Data.OleDb;
using Microsoft.Win32;
using System.Drawing;
using System.Data;
using System.Windows.Forms;


namespace GREENPLY.Classes
{
    public class MyExcel
    {
        //#region "variable Declaration"

        //public bool IsProtected;                       // Is the Sheet is Protected
        //public bool IsHeaderLocked;                     // Is the Headers of File should be Locked
        //public bool IsCellLocked;                       // Is the Cells of File should be Locked
        //public string ExcelPassword;                    // If Header or Cell will be Locked then Password for Sheet will be. 
        //public bool IsCellBold;                         // Is the Cells should be Bold.
        //public bool IsHeaderBold;                       // Is the Headers should be Bold.
        //public bool IsBorder;                           // Is Excel file contains Border
        //public System.Drawing.Color HeaderColor;        //Color of Header Background
        //public System.Drawing.Color CellColor;          //Color of Rows Background
        //public System.Data.DataTable DataTableToWrite;  //data table to Write in Excel
        //public string Path;                             //Path of the file

        //#endregion

        //public void WriteInExcel(System.Data.DataTable dt, string xlsFilePath, bool IsProtected, bool IsHeaderLocked, bool IsCellLocked, bool IsCellBold, bool IsHeaderBold, bool IsBorder, string ExcelPassword, System.Drawing.Color HeaderColor, System.Drawing.Color CellColor)
        //{
        //    Microsoft.Office.Interop.Excel.Application exc = null;
        //    try
        //    {
        //        exc = new Microsoft.Office.Interop.Excel.Application();
        //        Microsoft.Office.Interop.Excel.Workbooks workbooks = exc.Workbooks;
        //        Microsoft.Office.Interop.Excel._Workbook workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
        //        _Worksheet worksheet = (_Worksheet)workbook.Worksheets[1];
        //        Range range;
        //        Object[] data;
        //        int i, j;
        //        for (j = 0; j < dt.Columns.Count; j++)
        //        {
        //            range = worksheet.get_Range(Convert.ToChar(65 + j) + 1.ToString(), System.Reflection.Missing.Value);
        //            data = new Object[] { dt.Columns[j].Caption };
        //            if (IsHeaderBold == true)
        //            {
        //                range.Font.Bold = true;
        //            }
        //            if (IsHeaderLocked == true)
        //            {
        //                range.Select();
        //                range.Locked = true;
        //            }
        //            if (!HeaderColor.IsEmpty)
        //            {
        //                range.Select();
        //                range.Interior.Color = HeaderColor.ToArgb();
        //            }
        //            if (IsBorder == true)
        //            {
        //                range.Borders.Color = Color.Black.ToArgb();
        //            }
        //            range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
        //        }
        //        for (i = 0; i < dt.Rows.Count; i++)
        //        {
        //            for (j = 0; j < dt.Columns.Count; j++)
        //            {
        //                range = worksheet.get_Range(Convert.ToChar(65 + j) + Convert.ToString(2 + i), System.Reflection.Missing.Value);
        //                data = new Object[] { dt.Rows[i][j] };
        //                if (!HeaderColor.IsEmpty)
        //                {
        //                    range.Select();
        //                    range.Interior.Color = CellColor.ToArgb();
        //                }
        //                if (IsCellBold == true)
        //                {
        //                    range.Font.Bold = true;
        //                }
        //                if (IsBorder == true)
        //                {
        //                    range.Borders.Color = Color.Black.ToArgb();
        //                }
        //                if (IsCellLocked == true)
        //                {
        //                    range.Select();
        //                    range.Locked = true;
        //                }
        //                else
        //                {
        //                    range.Select();
        //                    range.Locked = false;
        //                }
        //                range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
        //            }
        //        }
        //        range = worksheet.get_Range("A1", "B1");
        //        range.Select();
        //        worksheet.Columns.AutoFit();
        //        if (IsCellLocked == true || IsHeaderLocked == true)
        //        {
        //            worksheet.Protect(ExcelPassword,
        //                worksheet.ProtectDrawingObjects,
        //                true, worksheet.ProtectScenarios,
        //                worksheet.ProtectionMode,
        //                true,
        //                true,
        //                true,
        //                worksheet.Protection.AllowInsertingColumns,
        //                worksheet.Protection.AllowInsertingRows,
        //                worksheet.Protection.AllowInsertingHyperlinks,
        //                worksheet.Protection.AllowDeletingColumns,
        //                worksheet.Protection.AllowDeletingRows,
        //                worksheet.Protection.AllowSorting,
        //                true,
        //                worksheet.Protection.AllowUsingPivotTables);
        //        }
               
        //        workbook.Close(true, xlsFilePath, null);

        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (exc != null)
        //            exc.Quit();
        //    }
        //}

        ///// <summary>
        ///// To Write an Excel File.
        ///// </summary>
        ///// <param name="dt"></param>
        ///// <param name="xlsFilePath"></param>
        //public void WriteInExcel(System.Data.DataTable dt, string xlsFilePath, string reporttype)
        //{
        //    Microsoft.Office.Interop.Excel.Application exc = null;
        //    object missing = Type.Missing;
        //    try
        //    {
        //        exc = new Microsoft.Office.Interop.Excel.Application();
        //        Microsoft.Office.Interop.Excel.Workbooks workbooks = exc.Workbooks;
        //        Microsoft.Office.Interop.Excel._Workbook workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
        //        _Worksheet worksheet = (_Worksheet)workbook.Worksheets[1];
        //        worksheet.Name = reporttype;
        //        worksheet.Cells[1, 1].EntireRow.Font.Bold = true;    

        //        Range range;
        //        Object[] data;
        //        int i, j;
        //        for (j = 0; j < dt.Columns.Count; j++)
        //        {
        //            range = worksheet.get_Range(Convert.ToChar(65 + j) + 1.ToString(), System.Reflection.Missing.Value);
        //            data = new Object[] { dt.Columns[j].Caption };
        //            if (IsHeaderBold == true)
        //            {
        //                range.Font.Bold = true;
        //            }
        //            if (IsHeaderLocked == true)
        //            {
        //                range.Select();
        //                range.Locked = true;
        //            }
        //            if (!HeaderColor.IsEmpty)
        //            {
        //                range.Select();
        //                range.Interior.Color = HeaderColor.ToArgb();
        //            }
        //            if (IsBorder == true)
        //            {
        //                range.Borders.Color = Color.Black.ToArgb();
        //            }
        //            range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
        //        }
        //        for (i = 0; i < dt.Rows.Count; i++)
        //        {
        //            for (j = 0; j < dt.Columns.Count; j++)
        //            {
        //                range = worksheet.get_Range(Convert.ToChar(65 + j) + Convert.ToString(2 + i), System.Reflection.Missing.Value);
        //                data = new Object[] { dt.Rows[i][j] };
        //                if (!HeaderColor.IsEmpty)
        //                {
        //                    range.Select();
        //                    range.Interior.Color = CellColor.ToArgb();
        //                }
        //                if (IsCellBold == true)
        //                {
        //                    range.Font.Bold = true;
        //                }
        //                if (IsBorder == true)
        //                {
        //                    range.Borders.Color = Color.Black.ToArgb();
        //                }
        //                if (IsCellLocked == true)
        //                {
        //                    range.Select();
        //                    range.Locked = true;
        //                }
        //                else
        //                {
        //                    range.Select();
        //                    range.Locked = false;
        //                }
        //                range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
        //            }
        //        }
        //        range = worksheet.get_Range("A1", "B1");
        //        range.Select();
        //        worksheet.Columns.AutoFit();
        //        if (IsCellLocked == true || IsHeaderLocked == true)
        //        {
        //            worksheet.Protect(ExcelPassword,
        //                worksheet.ProtectDrawingObjects,
        //                true, worksheet.ProtectScenarios,
        //                worksheet.ProtectionMode,
        //                true,
        //                true,
        //                true,
        //                worksheet.Protection.AllowInsertingColumns,
        //                worksheet.Protection.AllowInsertingRows,
        //                worksheet.Protection.AllowInsertingHyperlinks,
        //                worksheet.Protection.AllowDeletingColumns,
        //                worksheet.Protection.AllowDeletingRows,
        //                worksheet.Protection.AllowSorting,
        //                true,
        //                worksheet.Protection.AllowUsingPivotTables);
        //        }                
        //        workbook.Close(true, xlsFilePath, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (exc != null)
        //            exc.Quit();
        //    }
        //}

       

        ///// <summary>
        ///// To Write an Excel File.
        ///// </summary>
        //public void WriteInExcel()
        //{
        //    Microsoft.Office.Interop.Excel.Application exc = null;
        //    try
        //    {
        //        exc = new Microsoft.Office.Interop.Excel.Application();
        //        Microsoft.Office.Interop.Excel.Workbooks workbooks = exc.Workbooks;
        //        Microsoft.Office.Interop.Excel._Workbook workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
        //        _Worksheet worksheet = (_Worksheet)workbook.Worksheets[1];
        //        Range range;
        //        Object[] data;
        //        int i, j;
        //        for (j = 0; j < DataTableToWrite.Columns.Count; j++)
        //        {
        //            range = worksheet.get_Range(Convert.ToChar(65 + j) + 1.ToString(), System.Reflection.Missing.Value);
        //            data = new Object[] { DataTableToWrite.Columns[j].Caption };
        //            if (IsHeaderBold == true)
        //            {
        //                range.Font.Bold = true;
        //            }
        //            if (IsHeaderLocked == true)
        //            {
        //                range.Select();
        //                range.Locked = true;
        //            }
        //            if (!HeaderColor.IsEmpty)
        //            {
        //                range.Select();
        //                range.Interior.Color = HeaderColor.ToArgb();
        //            }
        //            if (IsBorder == true)
        //            {
        //                range.Borders.Color = Color.Black.ToArgb();
        //            }
        //            range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
        //        }
        //        for (i = 0; i < DataTableToWrite.Rows.Count; i++)
        //        {
        //            for (j = 0; j < DataTableToWrite.Columns.Count; j++)
        //            {
        //                range = worksheet.get_Range(Convert.ToChar(65 + j) + Convert.ToString(2 + i), System.Reflection.Missing.Value);
        //                data = new Object[] { DataTableToWrite.Rows[i][j] };
        //                if (!HeaderColor.IsEmpty)
        //                {
        //                    range.Select();
        //                    range.Interior.Color = CellColor.ToArgb();
        //                }
        //                if (IsCellBold == true)
        //                {
        //                    range.Font.Bold = true;
        //                }
        //                if (IsBorder == true)
        //                {
        //                    range.Borders.Color = Color.Black.ToArgb();
        //                }
        //                if (IsCellLocked == true)
        //                {
        //                    range.Select();
        //                    range.Locked = true;
        //                }
        //                else
        //                {
        //                    range.Select();
        //                    range.Locked = false;
        //                }
        //                range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
        //            }
        //        }
        //        range = worksheet.get_Range("A1", "B1");
        //        range.Select();
        //        worksheet.Columns.AutoFit();
        //        if (IsCellLocked == true || IsHeaderLocked == true)
        //        {
        //            worksheet.Protect(ExcelPassword,
        //                worksheet.ProtectDrawingObjects,
        //                true, worksheet.ProtectScenarios,
        //                worksheet.ProtectionMode, true, true, true,
        //                worksheet.Protection.AllowInsertingColumns,
        //                worksheet.Protection.AllowInsertingRows,
        //                worksheet.Protection.AllowInsertingHyperlinks,
        //                worksheet.Protection.AllowDeletingColumns,
        //                worksheet.Protection.AllowDeletingRows,
        //                worksheet.Protection.AllowSorting,
        //                true, worksheet.Protection.AllowUsingPivotTables);
        //        }
        //        workbook.Close(true, Path, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (exc != null)
        //            exc.Quit();
        //    }
        //}

        //public bool WriteDataTableToExcel(System.Data.DataTable dataTable, string saveAsLocation)
        //{
        //    Microsoft.Office.Interop.Excel.Application excel;
        //    Microsoft.Office.Interop.Excel.Workbook excelworkBook;
        //    Microsoft.Office.Interop.Excel.Worksheet excelSheet;
        //    Microsoft.Office.Interop.Excel.Range excelCellrange;

        //    try
        //    {
        //        // Start Excel and get Application object.
        //        excel = new Microsoft.Office.Interop.Excel.Application();

        //        // for making Excel visible
        //        excel.Visible = false;
        //        excel.DisplayAlerts = false;

        //        // Creation a new Workbook
        //        excelworkBook = excel.Workbooks.Add(Type.Missing);

        //        // Workk sheet
        //        excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;
               


             
        //        // loop through each row and add values to our sheet
        //        int rowcount = 2;
        //        string hawbValuePrevious = string.Empty;
        //        string hawbValueCurrent = string.Empty;

        //        foreach (DataRow datarow in dataTable.Rows)
        //        {
        //            rowcount += 1;
        //            for (int i = 1; i <= dataTable.Columns.Count-1; i++)
        //            {
        //                // on the first iteration we add the column headers
        //                if (rowcount == 3)
        //                {
        //                    excelSheet.Cells[2, i] = dataTable.Columns[i - 1].ColumnName;
        //                    excelSheet.Cells.Font.Color = System.Drawing.Color.Black;

        //                }

        //                excelSheet.Cells[rowcount, i] = datarow[i - 1].ToString();
        //                hawbValueCurrent = datarow[dataTable.Columns.Count-1].ToString();

        //                //for alternate rows
        //                if (rowcount > 3)
        //                {
        //                    if (i == dataTable.Columns.Count)
        //                    {
        //                        if (rowcount % 2 == 0)
        //                        {
        //                            excelCellrange = excelSheet.Range[excelSheet.Cells[rowcount, 1], excelSheet.Cells[rowcount, dataTable.Columns.Count]];
        //                          //  FormattingExcelCells(excelCellrange, "#CCCCFF", System.Drawing.Color.Black, false);
        //                        }

        //                    }
        //                }
        //            }

        //            if (hawbValueCurrent == hawbValuePrevious)
        //            {
                       
                       
        //                excelSheet.Range[excelSheet.Cells[rowcount, 6], excelSheet.Cells[rowcount - 1, 6]].Merge();
        //                excelSheet.Range[excelSheet.Cells[rowcount, 7], excelSheet.Cells[rowcount - 1, 7]].Merge();
        //                excelSheet.Range[excelSheet.Cells[rowcount, 8], excelSheet.Cells[rowcount - 1, 8]].Merge();
        //                excelSheet.Range[excelSheet.Cells[rowcount, 9], excelSheet.Cells[rowcount - 1, 9]].Merge();
        //                excelSheet.Range[excelSheet.Cells[rowcount, 10], excelSheet.Cells[rowcount - 1, 10]].Merge();
        //                excelSheet.Range[excelSheet.Cells[rowcount, 11], excelSheet.Cells[rowcount - 1, 11]].Merge();
        //                excelSheet.Range[excelSheet.Cells[rowcount, 12], excelSheet.Cells[rowcount - 1, 12]].Merge();
        //                excelSheet.Range[excelSheet.Cells[rowcount, 13], excelSheet.Cells[rowcount - 1, 13]].Merge();
        //                excelSheet.Range[excelSheet.Cells[rowcount, 5], excelSheet.Cells[rowcount - 1, 14]].Merge();

                       
                       
        //                excelSheet.Cells[rowcount, 6].Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
        //                excelSheet.Cells[rowcount, 7].Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
        //                excelSheet.Cells[rowcount, 8].Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
        //                excelSheet.Cells[rowcount, 9].Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
        //                excelSheet.Cells[rowcount, 10].Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
        //                excelSheet.Cells[rowcount, 11].Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
        //                excelSheet.Cells[rowcount, 12].Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
        //                excelSheet.Cells[rowcount, 13].Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
        //                excelSheet.Cells[rowcount, 14].Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;

        //                //excelSheet.Cells[rowcount, 22].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
        //            }
        //            hawbValuePrevious = hawbValueCurrent;
        //            hawbValueCurrent = string.Empty;

        //        }
        //        //excelSheet.Range[excelSheet.Cells[10, 22], excelSheet.Cells[12, 22]].Merge();
        //        // now we resize the columns
        //        excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[rowcount, dataTable.Columns.Count-1]];
        //        excelCellrange.EntireColumn.AutoFit();
        //        Microsoft.Office.Interop.Excel.Borders border = excelCellrange.Borders;
        //        border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //        border.Weight = 2d;


        //        excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[2, dataTable.Columns.Count-1]];
        //        FormattingExcelCells(excelCellrange, "#000099", System.Drawing.Color.White, true);


        //        //now save the workbook and exit Excel


        //        excelworkBook.SaveAs(saveAsLocation); ;
        //        excelworkBook.Close();
        //        excel.Quit();
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //        return false;
        //    }
        //    finally
        //    {
        //        excelSheet = null;
        //        excelCellrange = null;
        //        excelworkBook = null;
        //    }

        //}
        //public void FormattingExcelCells(Microsoft.Office.Interop.Excel.Range range, string HTMLcolorCode, System.Drawing.Color fontColor, bool IsFontbool)
        //{
        //    range.Interior.Color = System.Drawing.ColorTranslator.FromHtml(HTMLcolorCode);
        //    range.Font.Color = System.Drawing.ColorTranslator.ToOle(fontColor);
        //    if (IsFontbool == true)
        //    {
        //        range.Font.Bold = IsFontbool;
        //    }
        //}
        ///// <summary>
        ///// Read an excel file into datatable.
        ///// </summary>
        ///// <param name="FilePath"></param>
        ///// <returns></returns>
        //public System.Data.DataSet ReadExcel(string FilePath)
        //{
        //    //string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Extended Properties=Excel 12.0; Data Source=" + FilePath;
        //    string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Extended Properties='Excel 12.0;IMEX=1;TypeGuessRows=0;ImportMixedTypes=Text;'; Data Source=" + FilePath;
        //    Console.WriteLine("Connecting to Excel File.");
        //    DataSet dataset = new DataSet();
        //    OleDbConnection conn = new OleDbConnection(excelConnectionString);
        //    try
        //    {
        //        OleDbDataAdapter adapter = new OleDbDataAdapter();
        //        conn.Open();
        //        System.Data.DataTable dtSheets = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //        if (dtSheets == null)
        //        {
        //            return null;
        //        }
        //        String[] excelSheetNames = new String[dtSheets.Rows.Count];
        //        int i = 0;
        //        foreach (DataRow row in dtSheets.Rows)
        //        {
        //            excelSheetNames[i] = row["TABLE_NAME"].ToString();
        //            i++;
        //        }
        //        adapter.SelectCommand = new OleDbCommand("Select * from [Data$]", conn);
        //        adapter.Fill(dataset);

        //        conn.Close();
        //        conn.Dispose();
        //        //DataSet dss = new DataSet();
        //        //dss.Tables.Add(dt);
        //        //dss.Tables.Add(dt1);
        //        //dss.Tables.Add(dt2);
        //        return dataset;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (conn.State == ConnectionState.Open)
        //            conn.Close();
        //    }
        //}

        //public System.Data.DataTable ReadExcel1(string FilePath)
        //{
        //    try
        //    {
        //        string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Extended Properties=Excel 12.0; Data Source=" + FilePath;
        //        Console.WriteLine("Connecting to Excel File.");
        //        DataSet dataset = new DataSet();
        //        OleDbConnection conn = new OleDbConnection(excelConnectionString);
        //        OleDbDataAdapter adapter = new OleDbDataAdapter();
        //        conn.Open();
        //        System.Data.DataTable dtSheets = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
        //        if (dtSheets == null)
        //        {
        //            return null;
        //        }
        //        String[] excelSheetNames = new String[dtSheets.Rows.Count];
        //        int i = 0;
        //        foreach (DataRow row in dtSheets.Rows)
        //        {
        //            excelSheetNames[i] = row["TABLE_NAME"].ToString();
        //            i++;
        //        }
        //        adapter.SelectCommand = new OleDbCommand("Select * from [" + excelSheetNames[0] + "]", conn);
        //        adapter.Fill(dataset);
        //        System.Data.DataTable dt = dataset.Tables[0];

        //        conn.Close();
        //        conn.Dispose();
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        ///// <summary>
        ///// To Read an Excel File.
        ///// </summary>
        ///// <returns></returns>
        //public System.Data.DataTable ReadExcelFile(string Path)
        //{
        //    try
        //    {
        //        string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Extended Properties=Excel 12.0; Data Source=" + Path;
        //        OleDbConnection connection = new OleDbConnection(excelConnectionString);
        //        Console.WriteLine("Connecting to Excel File.");
        //        OleDbCommand command = new OleDbCommand("Select * FROM [Sheet1$]", connection);
        //        OleDbCommand count = new OleDbCommand("Select count(*) FROM [Sheet1$]", connection);
        //        DataSet dataset = new DataSet();
        //        OleDbConnection conn = new OleDbConnection(excelConnectionString);
        //        OleDbDataAdapter adapter = new OleDbDataAdapter();
        //        adapter.SelectCommand = new OleDbCommand("Select * from [Sheet1$]", connection);
        //        adapter.Fill(dataset);
        //        System.Data.DataTable dt = dataset.Tables[0];
        //        return dt;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //public void WriteInExcelWithFormat(System.Data.DataTable dt, string xlsFilePath, bool IsProtected, bool IsHeaderLocked, bool IsCellLocked, bool IsCellBold, bool IsHeaderBold, bool IsBorder, string ExcelPassword, System.Drawing.Color HeaderColor, System.Drawing.Color CellColor, string ReportName, string UserID)
        //{
        //    Microsoft.Office.Interop.Excel.Application exc = null;
        //    try
        //    {
        //        exc = new Microsoft.Office.Interop.Excel.Application();
        //        Microsoft.Office.Interop.Excel.Workbooks workbooks = exc.Workbooks;
        //        Microsoft.Office.Interop.Excel._Workbook workbook = workbooks.Add(XlWBATemplate.xlWBATWorksheet);
        //        _Worksheet worksheet = (_Worksheet)workbook.Worksheets[1];
        //        Range range;
        //        Object[] data;

        //        range = worksheet.get_Range(Convert.ToChar(65 + 0) + Convert.ToString(1 + 0), System.Reflection.Missing.Value);
        //        data = new Object[] { "MAKE MY TRIP" };
        //        range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
        //        range.WrapText = false;
        //        range.Borders.Color = Color.Black.ToArgb();
        //        range.VerticalAlignment = XlVAlign.xlVAlignTop;
        //        range.Select();
        //        //------------------------------------------------------------------------------------------------------------------
        //        range = worksheet.get_Range(Convert.ToChar(65 + 0) + Convert.ToString(2 + 0), System.Reflection.Missing.Value);
        //        data = new Object[] { "REPORT NAME" };
        //        range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
        //        range.VerticalAlignment = XlVAlign.xlVAlignTop;
        //        range.WrapText = false;
        //        range.Borders.Color = Color.Black.ToArgb();

        //        range = worksheet.get_Range(Convert.ToChar(65 + 1) + Convert.ToString(2 + 0), System.Reflection.Missing.Value);
        //        data = new Object[] { ReportName };
        //        range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
        //        range.VerticalAlignment = XlVAlign.xlVAlignTop;
        //        range.WrapText = false;
        //        range.Borders.Color = Color.Black.ToArgb();
        //        //------------------------------------------------------------------------------------------------------------------

        //        range = worksheet.get_Range(Convert.ToChar(65 + 0) + Convert.ToString(3 + 0), System.Reflection.Missing.Value);
        //        data = new Object[] { "GENERATED ON" };
        //        range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
        //        range.Borders.Color = Color.Black.ToArgb();
        //        range.WrapText = false;
        //        range.VerticalAlignment = XlVAlign.xlVAlignTop;

        //        string strDate = DateTime.Today.ToString("dd/MM/yyyy");
        //        range = worksheet.get_Range(Convert.ToChar(65 + 1) + Convert.ToString(3 + 0), System.Reflection.Missing.Value);
        //        data = new Object[] { "'" + strDate };
        //        range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
        //        range.WrapText = false;
        //        range.Borders.Color = Color.Black.ToArgb();
        //        range.VerticalAlignment = XlVAlign.xlVAlignTop;
        //        //------------------------------------------------------------------------------------------------------------------

        //        range = worksheet.get_Range(Convert.ToChar(65 + 0) + Convert.ToString(4 + 0), System.Reflection.Missing.Value);
        //        data = new Object[] { "GENERATED BY" };
        //        range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
        //        range.Borders.Color = Color.Black.ToArgb();
        //        range.WrapText = false;
        //        range.VerticalAlignment = XlVAlign.xlVAlignTop;


        //        range = worksheet.get_Range(Convert.ToChar(65 + 1) + Convert.ToString(4 + 0), System.Reflection.Missing.Value);
        //        data = new Object[] { UserID };
        //        range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
        //        range.VerticalAlignment = XlVAlign.xlVAlignTop;
        //        range.WrapText = false;
        //        range.Borders.Color = Color.Black.ToArgb();

        //        //------------------------------------------------------------------------------------------------------------------
        //        int i, j;
        //        for (j = 0; j < dt.Columns.Count; j++)
        //        {
        //            range = worksheet.get_Range(Convert.ToChar(65 + j) + 6.ToString(), System.Reflection.Missing.Value);
        //            data = new Object[] { dt.Columns[j].Caption };
        //            if (IsHeaderBold == true)
        //            {
        //                range.Font.Bold = true;
        //            }
        //            if (IsHeaderLocked == true)
        //            {
        //                range.Select();
        //                range.Locked = true;
        //            }
        //            if (!HeaderColor.IsEmpty)
        //            {
        //                range.Select();
        //                range.Interior.Color = HeaderColor.ToArgb();
        //            }
        //            if (IsBorder == true)
        //            {
        //                range.Borders.Color = Color.Black.ToArgb();
        //            }
        //            range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
        //        }
        //        for (i = 0; i < dt.Rows.Count; i++)
        //        {
        //            for (j = 0; j < dt.Columns.Count; j++)
        //            {
        //                range = worksheet.get_Range(Convert.ToChar(65 + j) + Convert.ToString(7 + i), System.Reflection.Missing.Value);
        //                data = new Object[] { dt.Rows[i][j] };
        //                if (!HeaderColor.IsEmpty)
        //                {
        //                    range.Select();
        //                    range.Interior.Color = CellColor.ToArgb();
        //                }
        //                if (IsCellBold == true)
        //                {
        //                    range.Font.Bold = true;
        //                }
        //                if (IsBorder == true)
        //                {
        //                    range.Borders.Color = Color.Black.ToArgb();
        //                }
        //                if (IsCellLocked == true)
        //                {
        //                    range.Select();
        //                    range.Locked = true;
        //                }
        //                else
        //                {
        //                    range.Select();
        //                    range.Locked = false;
        //                }
        //                range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
        //            }
        //        }
        //        range = worksheet.get_Range("A1", "B1");
        //        range.Select();
        //        worksheet.Columns.AutoFit();
        //        if (IsCellLocked == true || IsHeaderLocked == true)
        //        {
        //            worksheet.Protect(ExcelPassword,
        //                worksheet.ProtectDrawingObjects,
        //                true, worksheet.ProtectScenarios,
        //                worksheet.ProtectionMode,
        //                true,
        //                true,
        //                true,
        //                 worksheet.Protection.AllowInsertingColumns,
        //                worksheet.Protection.AllowInsertingRows,
        //                worksheet.Protection.AllowInsertingHyperlinks,
        //                worksheet.Protection.AllowDeletingColumns,
        //                worksheet.Protection.AllowDeletingRows,
        //                worksheet.Protection.AllowSorting,
        //                true,
        //                worksheet.Protection.AllowUsingPivotTables);
        //        }
        //        workbook.Close(true, xlsFilePath, null);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    finally
        //    {
        //        if (exc != null)
        //            exc.Quit();
        //    }
        //}

        ////Added by: Gourav
        //public void WriteInExcel(System.Data.DataSet ds, string xlsFilePath)
        //{

        //    Microsoft.Office.Interop.Excel.Application oXL = new Microsoft.Office.Interop.Excel.Application();
        //    oXL.Visible = false;
        //    Microsoft.Office.Interop.Excel.Workbook oWB = oXL.Workbooks.Add(System.Reflection.Missing.Value);


        //    Microsoft.Office.Interop.Excel.Worksheet oSheet
        //        = oWB.ActiveSheet as Microsoft.Office.Interop.Excel.Worksheet;


        //    for (int x = ds.Tables.Count - 1; x >= 0; x--)
        //    {
        //        if (x < ds.Tables.Count - 1)
        //        {
        //            oSheet =
        //               oWB.Sheets.Add(System.Reflection.Missing.Value, System.Reflection.Missing.Value,
        //               1, System.Reflection.Missing.Value)
        //                           as Microsoft.Office.Interop.Excel.Worksheet;
        //        }

        //        System.Data.DataTable dt = ds.Tables[x];
        //        for (int j = 0; j < dt.Columns.Count; j++)
        //        {
        //            //Microsoft.Office.Interop.Excel.Range c1 = oSheet.Cells[1, j + 1];
        //            //Microsoft.Office.Interop.Excel.Range c2 = oSheet.Cells[1, j + 1];
        //            //var range = (Microsoft.Office.Interop.Excel.Range)oSheet.get_Range(c1, c2);
        //            ////= oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[1, dt.Columns.Count]);
        //            //range.BorderAround(Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous,
        //            //    Microsoft.Office.Interop.Excel.XlBorderWeight.xlThin);

        //            oSheet.Name = dt.TableName;
        //            oSheet.Cells[1, j + 1] = dt.Columns[j].ColumnName;
        //            //  oSheet.Cells[1, j + 1].Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.Yellow);
        //        }
        //        for (int i = 0; i < dt.Rows.Count; i++)
        //        {
        //            for (int j = 0; j < dt.Columns.Count; j++)
        //            {
        //                oSheet.Cells[i + 2, j + 1] = dt.Rows[i][j].ToString();
        //            }
        //        }
        //        oSheet.Columns.AutoFit();
        //    }

        //    oWB.SaveAs(xlsFilePath, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal,
        //        System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value,
        //        System.Reflection.Missing.Value,
        //        Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
        //        System.Reflection.Missing.Value, System.Reflection.Missing.Value,
        //        System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
        //    oWB.Close(System.Reflection.Missing.Value, System.Reflection.Missing.Value, System.Reflection.Missing.Value);
        //    oXL.UserControl = true;
        //    oXL.Quit();
        //}

        //public class ExcelUtlity
        //{
        //    /// <summary>
        //    /// FUNCTION FOR EXPORT TO EXCEL
        //    /// </summary>
        //    /// <param name="dataTable"></param>
        //    /// <param name="worksheetName"></param>
        //    /// <param name="saveAsLocation"></param>
        //    /// <returns></returns>
        //    public bool WriteDataTableToExcel(System.Data.DataTable dataTable, string worksheetName, string saveAsLocation, string ReporType)
        //    {
        //        Microsoft.Office.Interop.Excel.Application excel = null;
        //        Microsoft.Office.Interop.Excel.Workbook excelworkBook;
        //        Microsoft.Office.Interop.Excel.Worksheet excelSheet;
        //        Microsoft.Office.Interop.Excel.Range excelCellrange;

        //        try
        //        {
        //            Cursor.Current = Cursors.WaitCursor;
        //            // Start Excel and get Application object.
        //            excel = new Microsoft.Office.Interop.Excel.Application();

        //            // for making Excel visible
        //            excel.Visible = false;
        //            excel.DisplayAlerts = false;

        //            // Creation a new Workbook
        //            excelworkBook = excel.Workbooks.Add(Type.Missing);

        //            // Workk sheet
        //            excelSheet = (Microsoft.Office.Interop.Excel.Worksheet)excelworkBook.ActiveSheet;
        //            //excelSheet.Name = worksheetName;


        //            excelSheet.Cells[1, 1] = ReporType;
        //            excelSheet.Cells[1, 2] = "Date : " + DateTime.Now.ToShortDateString();

        //            // loop through each row and add values to our sheet
        //            int rowcount = 2;

        //            foreach (DataRow datarow in dataTable.Rows)
        //            {
        //                rowcount += 1;
        //                for (int i = 1; i <= dataTable.Columns.Count; i++)
        //                {
        //                    // on the first iteration we add the column headers
        //                    if (rowcount == 3)
        //                    {
        //                        excelSheet.Cells[2, i] = dataTable.Columns[i - 1].ColumnName;
        //                        excelSheet.Cells.Font.Color = System.Drawing.Color.Black;

        //                    }

        //                    excelSheet.Cells[rowcount, i] = datarow[i - 1].ToString();

        //                    //for alternate rows
        //                    if (rowcount > 3)
        //                    {
        //                        if (i == dataTable.Columns.Count)
        //                        {
        //                            if (rowcount % 2 == 0)
        //                            {
        //                                excelCellrange = excelSheet.Range[excelSheet.Cells[rowcount, 1], excelSheet.Cells[rowcount, dataTable.Columns.Count]];
        //                                FormattingExcelCells(excelCellrange, "#CCCCFF", System.Drawing.Color.Black, false);
        //                            }

        //                        }
        //                    }

        //                }

        //            }

        //            // now we resize the columns
        //            excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[rowcount, dataTable.Columns.Count]];
        //            excelCellrange.EntireColumn.AutoFit();
        //            Microsoft.Office.Interop.Excel.Borders border = excelCellrange.Borders;
        //            border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //            border.Weight = 2d;


        //            excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[2, dataTable.Columns.Count]];
        //            FormattingExcelCells(excelCellrange, "#000099", System.Drawing.Color.White, true);


        //            //now save the workbook and exit Excel


        //            excelworkBook.SaveAs(saveAsLocation); ;
        //            excelworkBook.Close();
        //            excel.Quit();
        //            return true;
        //        }
        //        catch (Exception ex)
        //        {
        //            Cursor.Current = Cursors.Default;
        //            MessageBox.Show(ex.Message);
        //            return false;
        //        }
        //        finally
        //        {
        //            excelSheet = null;
        //            excelCellrange = null;
        //            excelworkBook = null;
        //            if (excel != null)
        //            {
        //                excel.Quit();
        //            }
        //            Cursor.Current = Cursors.Default;
        //        }


        //    }

        //    /// <summary>
        //    /// FUNCTION FOR FORMATTING EXCEL CELLS
        //    /// </summary>
        //    /// <param name="range"></param>
        //    /// <param name="HTMLcolorCode"></param>
        //    /// <param name="fontColor"></param>
        //    /// <param name="IsFontbool"></param>
        //    public void FormattingExcelCells(Microsoft.Office.Interop.Excel.Range range, string HTMLcolorCode, System.Drawing.Color fontColor, bool IsFontbool)
        //    {
        //        range.Interior.Color = System.Drawing.ColorTranslator.FromHtml(HTMLcolorCode);
        //        range.Font.Color = System.Drawing.ColorTranslator.ToOle(fontColor);
        //        if (IsFontbool == true)
        //        {
        //            range.Font.Bold = IsFontbool;
        //        }
        //    }

        //}
    }
}