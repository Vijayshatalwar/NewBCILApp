using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Reflection;
using System.Data.SqlClient;
using System.Windows.Input;
using System.Data;
using System.Management;
using COMMON;
using GREENPLY;
using COMMON_LAYER;
using ENTITY_LAYER;
using BUSSINESS_LAYER;
using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Net;
using GREENPLY.GreenplyERPPostingService;

namespace GREENPLY.Classes
{
    class BCommon
    {
        StringBuilder sb = new StringBuilder();
        public static string gPrinterName;
        public static DataTable dtDBDetail = new DataTable();

        public static bool ReadParameter()
        {
            FileInfo ServerFile = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\Setting.ini");
            StreamReader ReadServer = default(StreamReader);

            string strLine = null;
            bool _flage = false;
            string Data = null;
            try
            {
                if (ServerFile.Exists == true)
                {
                    ////BL_UserMaster _objBL_UserMaster = new BL_UserMaster();
                    ////dtDBDetail = _objBL_UserMaster.BLGetDBConnectionDetails();
                    ////if (dtDBDetail.Rows.Count == 1)
                    ////{
                    ////    VariableInfo.mDataSource = dtDBDetail.Rows[0][0].ToString().Trim();
                    ////    VariableInfo.mDatabase = dtDBDetail.Rows[0][1].ToString().Trim();
                    ////    VariableInfo.mDbUser = dtDBDetail.Rows[0][2].ToString().Trim();
                    ////    VariableInfo.mDbPassword = dtDBDetail.Rows[0][3].ToString().Trim();
                    ////    VariableInfo.mPlantCode = dtDBDetail.Rows[0][4].ToString().Trim();
                    ////    VariableInfo.mLocationType = dtDBDetail.Rows[0][5].ToString().Trim();
                    ////}
                    ////else
                    ////{
                    ////    return _flage;
                    ////}

                    ReadServer = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\Setting.ini");
                    string[] strArr = null;
                    Data = ReadServer.ReadToEnd().Trim();
                    if (Data.Contains("<LOCAL_SETTING>") == false)
                    {
                        ReadServer.Close();
                        return false;
                    }
                    ReadServer.Close();
                    ReadServer = null;
                    ReadServer = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\Setting.ini");

                    do
                    {
                        strLine = ReadServer.ReadLine();
                        if (strLine.Trim().ToUpper() == "</LOCAL_SETTING>")
                        {
                            break; // TODO: might not be correct. Was : Exit Do 
                        }
                        strArr = strLine.Split('=');
                        if (strArr[0].Trim().ToUpper() == "DATABASENAME")
                        {
                            VariableInfo.mDatabase = strArr[1].Trim();
                        }
                        if (strArr[0].Trim().ToUpper() == "USERNAME")
                        {
                            VariableInfo.mDbUser = strArr[1].Trim();
                        }
                        if (strArr[0].Trim().ToUpper() == "PASSWORD")
                        {
                            VariableInfo.mDbPassword = strArr[1].Trim();
                        }
                        if (strArr[0].Trim().ToUpper() == "SERVERNAME")
                        {
                            VariableInfo.mDataSource = strArr[1].Trim();
                        }
                        if (strArr[0].Trim().ToUpper() == "PRINTERNAME")
                        {
                            VariableInfo.mPrinterName = strArr[1].Trim();
                        }
                        if (strArr[0].Trim().ToUpper() == "LOCATIONCODE")
                        {
                            VariableInfo.mPlantCode = strArr[1].Trim();
                        }
                        if (strArr[0].Trim().ToUpper() == "LOCATIONTYPE")
                        {
                            VariableInfo.mLocationType = strArr[1].Trim();
                        }
                    }
                    while (!(strLine == null));
                    VariableInfo.mSqlCon = "Data Source =" + VariableInfo.mDataSource + ";" + "Initial Catalog =" + VariableInfo.mDatabase + ";" + "User ID =" + VariableInfo.mDbUser + ";" + "Password =" + VariableInfo.mDbPassword;
                    ReadServer.Close();
                    ReadServer = null;
                    ServerFile = null;
                    SqlConnection con = new SqlConnection(VariableInfo.mSqlCon);
                    try
                    {
                        con.Open();
                        if (con.State == ConnectionState.Open)
                            _flage = true;
                        else
                            _flage = false;

                        return _flage;

                    }
                    catch (Exception)
                    {
                        _flage = false;
                    }
                    return _flage;
                }
                else
                {
                    ServerFile = null;
                    return false;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
            return _flage;
        }

        public static bool SapReadParameter()
        {
            FileInfo ServerFile = new FileInfo(AppDomain.CurrentDomain.BaseDirectory + "\\SapSetting.ini");
            StreamReader ReadServer = default(StreamReader);

            string strLine = null;
            bool _flage = false;
            string Data = null;
            try
            {
                if (ServerFile.Exists == true)
                {
                    ReadServer = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\SapSetting.ini");
                    string[] strArr = null;
                    Data = ReadServer.ReadToEnd().Trim();
                    if (Data.Contains("<LOCAL_SETTING>") == false)
                    {
                        ReadServer.Close();
                        return false;
                    }
                    ReadServer.Close();
                    ReadServer = null;
                    ReadServer = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + "\\SapSetting.ini");

                    do
                    {
                        strLine = ReadServer.ReadLine();
                        if (strLine.Trim().ToUpper() == "</SAP_SETTING>")
                        {
                            break; // TODO: might not be correct. Was : Exit Do 
                        }
                        strArr = strLine.Split('=');
                        if (strArr[0].Trim().ToUpper() == "SAPSERVERNAME")
                        {
                            VariableInfo.mSapServer = strArr[1].Trim();
                        }

                        if (strArr[0].Trim().ToUpper() == "SAPCLIENT")
                        {

                            VariableInfo.mSapClient = strArr[1].Trim();
                        }

                        if (strArr[0].Trim().ToUpper() == "SAPUSERNAME")
                        {

                            VariableInfo.mSapUser = strArr[1].Trim();
                        }

                        if (strArr[0].Trim().ToUpper() == "SAPPASSWORD")
                        {

                            VariableInfo.mSapPassword = strArr[1].Trim();
                        }
                        if (strArr[0].Trim().ToUpper() == "SAPSYSTEM")
                        {

                            VariableInfo.mSapSystemNo = strArr[1].Trim();
                        }
                        if (strArr[0].Trim().ToUpper() == "SAPLANGUAGE")
                        {

                            VariableInfo.mSapLanguage = strArr[1].Trim();
                        }


                    }
                    while (!(strLine == null));

                }
                else
                {
                    ServerFile = null;
                    return false;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
            return _flage;
        }

        public static bool ReadPrn()
        {
            string _sPrnWithMaster = AppDomain.CurrentDomain.BaseDirectory + "\\YAZAKI.PRN";

            string prn = string.Empty;

            if (File.Exists(_sPrnWithMaster))
            {
                StreamReader sr = new StreamReader(_sPrnWithMaster);
                prn = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();

                VariableInfo.mPrn = prn;
            }
            return true;
        }

        public static bool ReadPrnStore()
        {
            string _sPrnWithMaster = AppDomain.CurrentDomain.BaseDirectory + "\\StoreLabel.prn";
            string prn = string.Empty;
            if (File.Exists(_sPrnWithMaster))
            {
                StreamReader sr = new StreamReader(_sPrnWithMaster);
                prn = sr.ReadToEnd();
                sr.Close();
                sr.Dispose();

                VariableInfo.mPrn = prn;
            }
            return true;
        }

        public static void WriteLog(string strLog)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            string logFilePath;
            logFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\LogFiles\\" + "GreenplyPCLogFile_" + System.DateTime.Today.ToString("MM-dd-yyyy") + "." + "txt";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists)
                logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);
            log.WriteLine(strLog);
            log.Close();
        }


        public void AlphaNumericOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAlphaNumeric(e.Text);

        }


        public static bool IsTextAlphaNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^a-zA-Z0-9 -]*");
            return reg.IsMatch(str);

        }
       
        public bool IsTextAlphaNumericForBulk(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("^[a-zA-Z0-9-]*$");
            return reg.IsMatch(str);

        }
       
        public void AlphaOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = IsTextAlpha(e.Text);

        }


        public bool IsTextAlpha(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("^[a-zA-Z_]*$");
            return reg.IsMatch(str);

        }

        public void NumericOnly(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // e.Handled = e.Text.IsTextNumeric();

        }


        public bool IsTextNumeric(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[^0-9]");
            return reg.IsMatch(str);

        }

        public bool IsDecimalNumeric(System.Object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            bool _flag = false;
            decimal d;
            if (decimal.TryParse(e.Text.Trim(), out d))
            {
                _flag = true;

            }
            return _flag;
        }

        public bool IsDecimalNumeric(string str)
        {
            bool _flag = false;
            decimal d;
            if (decimal.TryParse(str.Trim(), out d))
            {
                _flag = true;
            }
            return _flag;

        }

        public bool IsTextNumericForBulk(string str)
        {
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("^[0-9_]*$");
            return reg.IsMatch(str);

        }

        public static string splittimefromdate(string textboxvalue)
        {
            string[] firstsplist = textboxvalue.Split(' ');
            string date = firstsplist[0].ToString();
            string[] secondsplit = date.Split('/');
            string mm = secondsplit[0].ToString();
            if (Convert.ToInt32(mm) < 10)
            {
                mm = '0' + mm;
            }
            string dd = secondsplit[1].ToString();
            if (Convert.ToInt32(dd) < 10)
            {
                dd = '0' + dd;
            }
            string yyyy = secondsplit[2].ToString();


            return (dd + '/' + mm + '/' + yyyy);
        }

        public static void setMessageBox(string sAppName, string sMessage, int iWhich)
        {
            WndForCustomMessage message = new WndForCustomMessage(sMessage, sAppName, iWhich);
            message.ShowDialog();
        }

        public static MessageResult setMessageBox(string sAppName, string sMessage)
        {
            WndForCustomMessage message = new WndForCustomMessage(sMessage, sAppName);
            message.ShowDialog();
            return message.Result;
        }

        public static string EncryptPassword(string lPass, string Ltype)
        {
            string str1 = string.Empty;
            string str2 = string.Empty;
            int k = lPass.Length;
            str2 = "BCILBCILBCILBCILBCILBCIL";
            for (int i = 0; i < k; i++)
            {
                char ch1 = Convert.ToChar(lPass.Substring(i, 1));
                char ch2 = Convert.ToChar(str2.Substring(i, 1));
                if (Ltype == "E")
                {
                    Encoding encoding1 = Encoding.GetEncoding(1252);
                    int j = ch1 + ch2 + i;
                    string str3 = encoding1.GetString(new byte[] { Convert.ToByte(j) });
                    str1 = string.Concat(str1, str3);
                }
                else
                {
                    int j = Encoding.GetEncoding(1252).GetBytes(new char[] { Convert.ToChar(ch1) })[0] - ch2 - i;
                    str1 = string.Concat(str1, (ushort)j);
                }
            }
            return str1;
        }

        public static string EncryptPasswordNew(string lPass, string Ltype)
        {
            string encName = string.Empty;
            string LCompname = string.Empty;
            int i;
            int j;
            int LLen;
            char c1;
            char c2;

            LLen = lPass.Length;
            LCompname = "BCILBCILBCILBCILBCILBCIL";
            for (i = 0; i < LLen; i++)
            {
                c1 = Convert.ToChar(lPass.Substring(i, 1));
                c2 = Convert.ToChar(LCompname.Substring(i, 1));

                if (Ltype == "E")
                {
                    var e1 = Encoding.GetEncoding(1252);
                    j = (int)c1 + (int)c2 + i;
                    var s = e1.GetString(new byte[] { Convert.ToByte(j) });

                    encName = encName + s;
                }
                else
                {
                    var e1 = Encoding.GetEncoding(1252);
                    var r = e1.GetBytes(new char[] { Convert.ToChar(c1) });
                    j = (int)r[0] - (int)c2 - i;

                    encName = encName + (char)j;
                }
            }
            return encName;
        }

        public string EncryptDcrypt(string strVal, string strType)
        {
            try
            {
                string encName = string.Empty;
                string LCompname = string.Empty;
                int i;
                int j;
                int LLen;
                char c1;
                char c2;

                LLen = strVal.Length;
                LCompname = "BCILBCILBCILBCILBCILBCIL";
                for (i = 0; i < LLen; i++)
                {
                    c1 = Convert.ToChar(strVal.Substring(i, 1));
                    c2 = Convert.ToChar(strVal.Substring(i, 1));
                    c2 = 'B';

                    if (strType.ToUpper() == "E")
                    {
                        var e1 = Encoding.GetEncoding(1252);
                        j = (int)c1 + (int)c2 + i;
                        //j = (int)c1 + i;
                        var s = e1.GetString(new byte[] { Convert.ToByte(j) });

                        encName = encName + s;
                    }
                    else if (strType.ToUpper() == "D")
                    {
                        var e1 = Encoding.GetEncoding(1252);
                        var r = e1.GetBytes(new char[] { Convert.ToChar(c1) });
                        j = (int)r[0] - (int)c2 - i;
                        //j = (int)r[0] - i;
                        encName = encName + (char)j;
                    }
                }
                return encName;
            }
            catch
            {

                throw;
            }
        }

        public static void FillCombobox(System.Windows.Controls.ComboBox cb, DataTable dtSource, string DisplayMember, string SelectedValue)
        {
            try
            {
                cb.DataContext = null;
                cb.DataContext = dtSource;
                cb.DisplayMemberPath = DisplayMember;
                cb.SelectedValuePath = SelectedValue;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region "Fill Combobox From DataTable
        public static void FillCombobox(System.Windows.Controls.ComboBox cb, Dictionary<string, int> Source)
        {
            try
            {
                cb.ItemsSource = null;
                cb.ItemsSource = Source.Keys;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public static void FillCombobox(System.Windows.Controls.ComboBox cb, DataTable dtSource, string DisplayMember)
        {
            try
            {
                cb.DataContext = null;
                cb.DataContext = dtSource;
                cb.DisplayMemberPath = DisplayMember;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        /// GetDateFormat
        /// </summary>
        /// <param name="sModuleType"></param>
        /// <param name="sPrifix"></param>
        /// 
        public string _GetDateFormat()
        {
            string _sDateFormat = string.Empty;
            try
            {

            }
            catch (Exception ex)
            {

                throw ex;
            }
            return _sDateFormat;
        }

        #region Command

        public ICommand SaveCommand { get; set; }
        public void SaveCommand_Execute()
        {
            //MessageBox.Show("Save Executed", "Message Save", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion

        public static void FillComboBoxGRN(ComboBox cbo, DataTable dt, bool isSelect)
        {
            if (isSelect)
            {
                if (dt.Rows.Count != 0)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = "--Select--";
                    dt.Rows.InsertAt(dr, 0);
                    cbo.ItemsSource = dt.DefaultView;
                    cbo.DisplayMemberPath = dt.Columns[0].ToString();
                    cbo.SelectedValuePath = dt.Columns[0].ToString();
                }
                else
                {
                    cbo.ItemsSource = dt.DefaultView;
                }
            }
        }

        public static bool ExportToExcelFromDataTable(DataTable dataTable, string sReportName, string fileformat, string reporttype)
        {
            bool allowExport = false;
            if (dataTable == null || dataTable.Rows.Count == 0)
                allowExport = false;
            else
                allowExport = true;
            if (allowExport)
            {
                MyExcel exl = new MyExcel();
                //System.Windows.Forms.SaveFileDialog sfg = new System.Windows.Forms.SaveFileDialog();
                //if (fileformat == "CSV")
                //    sfg.Filter = "CSV Format |*.csv";
                //else if (fileformat == "EXCEL")
                //    sfg.Filter = "Excel Format |*.xlsx";
                //if (sfg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //{
                //sfg.FileName = reporttype + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string FileName = reporttype + DateTime.Now.ToString("_yyyyMMddHHmmss");
                string path = Properties.Settings.Default.LocalFolderPath;
                string folderPath = path + FileName; //sfg.FileName;
                //exl.WriteInExcel(dataTable, folderPath, reporttype);
                BCommon.setMessageBox(VariableInfo.mApp, sReportName + " Imported Succesfully At This Location :" + folderPath, 1);
                //}

            }
            return allowExport;
        }

        public static bool ExportToCSVFromDataTable(DataTable dataTable, string sReportName, string fileformat, string reportType)
        {
            bool allowExport = false;
            if (dataTable == null || dataTable.Rows.Count == 0)
                allowExport = false;
            else
                allowExport = true;
            if (allowExport)
            {
                //System.Windows.Forms.SaveFileDialog sfg = new System.Windows.Forms.SaveFileDialog();
                //if (sfg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                //{
                string csv = string.Empty;
                foreach (DataColumn column in dataTable.Columns)
                {
                    csv += column.ColumnName + ',';
                }
                csv += "\r\n";
                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                    }
                    csv += "\r\n";
                }
                //sfg.Title = "Save Reports";
                //sfg.DefaultExt = "CSV";
                //sfg.Filter = "CSV files (*.csv)|*.csv|All files (*.*)|*.*";
                //sfg.FileName = "Report2_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string FileName = reportType + DateTime.Now.ToString("_yyyyMMddHHmmss");
                string path = Properties.Settings.Default.LocalFolderPath;

                string folderPath = path;
                File.WriteAllText(folderPath + FileName + ".csv", csv);
                BCommon.setMessageBox(VariableInfo.mApp, FileName + " Saved Succesfully At This Location : " + folderPath + FileName, 1);


                //}
            }
            return allowExport;
        }

        public static bool ExportToExcelDataTable(DataTable dataTable)
        {
            bool allowExport = false;
            if (dataTable == null || dataTable.Rows.Count == 0)
                allowExport = false;
            else
                allowExport = true;
            if (allowExport)
            {
                MyExcel exl = new MyExcel();
                System.Windows.Forms.SaveFileDialog sfg = new System.Windows.Forms.SaveFileDialog();
                sfg.Filter = "Excel |*.xlsx";
                if (sfg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //exl.WriteDataTableToExcel(dataTable, sfg.FileName);
                    BCommon.setMessageBox(VariableInfo.mApp, "Data Imported Succesfully At This Location :" + sfg.FileName, 1);
                }
            }
            return allowExport;
        }

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
        //            for (int i = 1; i <= dataTable.Columns.Count - 1; i++)
        //            {
        //                // on the first iteration we add the column headers
        //                if (rowcount == 3)
        //                {
        //                    excelSheet.Cells[2, i] = dataTable.Columns[i - 1].ColumnName;
        //                    excelSheet.Cells.Font.Color = System.Drawing.Color.Black;

        //                }

        //                excelSheet.Cells[rowcount, i] = datarow[i - 1].ToString();
        //                hawbValueCurrent = datarow[dataTable.Columns.Count - 1].ToString();

        //                //for alternate rows
        //                if (rowcount > 3)
        //                {
        //                    if (i == dataTable.Columns.Count)
        //                    {
        //                        if (rowcount % 2 == 0)
        //                        {
        //                            excelCellrange = excelSheet.Range[excelSheet.Cells[rowcount, 1], excelSheet.Cells[rowcount, dataTable.Columns.Count]];
        //                            //  FormattingExcelCells(excelCellrange, "#CCCCFF", System.Drawing.Color.Black, false);
        //                        }

        //                    }
        //                }
        //            }

        //            if (hawbValueCurrent == hawbValuePrevious)
        //            {

        //                excelSheet.Range[excelSheet.Cells[rowcount, 5], excelSheet.Cells[rowcount - 1, 5]].Merge();
        //                excelSheet.Range[excelSheet.Cells[rowcount, 6], excelSheet.Cells[rowcount - 1, 6]].Merge();
        //                excelSheet.Range[excelSheet.Cells[rowcount, 7], excelSheet.Cells[rowcount - 1, 7]].Merge();
        //                excelSheet.Range[excelSheet.Cells[rowcount, 8], excelSheet.Cells[rowcount - 1, 8]].Merge();
        //                excelSheet.Range[excelSheet.Cells[rowcount, 9], excelSheet.Cells[rowcount - 1, 9]].Merge();
        //                excelSheet.Range[excelSheet.Cells[rowcount, 10], excelSheet.Cells[rowcount - 1, 10]].Merge();
        //                excelSheet.Range[excelSheet.Cells[rowcount, 11], excelSheet.Cells[rowcount - 1, 11]].Merge();
        //                excelSheet.Range[excelSheet.Cells[rowcount, 12], excelSheet.Cells[rowcount - 1, 12]].Merge();
        //                excelSheet.Range[excelSheet.Cells[rowcount, 13], excelSheet.Cells[rowcount - 1, 13]].Merge();


        //                excelSheet.Cells[rowcount, 5].Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
        //                excelSheet.Cells[rowcount, 6].Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
        //                excelSheet.Cells[rowcount, 7].Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
        //                excelSheet.Cells[rowcount, 8].Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
        //                excelSheet.Cells[rowcount, 9].Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
        //                excelSheet.Cells[rowcount, 10].Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
        //                excelSheet.Cells[rowcount, 11].Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
        //                excelSheet.Cells[rowcount, 12].Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
        //                excelSheet.Cells[rowcount, 13].Style.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;

        //                //excelSheet.Cells[rowcount, 22].Style.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
        //            }
        //            hawbValuePrevious = hawbValueCurrent;
        //            hawbValueCurrent = string.Empty;

        //        }
        //        //excelSheet.Range[excelSheet.Cells[10, 22], excelSheet.Cells[12, 22]].Merge();
        //        // now we resize the columns
        //        excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[rowcount, dataTable.Columns.Count - 1]];
        //        excelCellrange.EntireColumn.AutoFit();
        //        Microsoft.Office.Interop.Excel.Borders border = excelCellrange.Borders;
        //        border.LineStyle = Microsoft.Office.Interop.Excel.XlLineStyle.xlContinuous;
        //        border.Weight = 2d;


        //        excelCellrange = excelSheet.Range[excelSheet.Cells[1, 1], excelSheet.Cells[2, dataTable.Columns.Count - 1]];
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


    }

}
