using System;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Controls;
using System.Data.OleDb;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Net.Mail;
using System.Configuration;
using System.IO;
using System.Net;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

//using BcilLib;
using System.Security.Cryptography;

namespace COMMON_LAYER
{
    /// <summary>
    /// Common Class 
    /// 
    /// </summary>
    public static class VariableInfo
    {
        //public static BcilCustomFunction mBcilCustomFunction = new BcilCustomFunction();
        //public static BcilLogger mAppLog = new BcilLogger();
        //internal static BcilLib.BcilLogger mAppLog = null;
        public static string mPlantType = "";
        public static string gEXCELVER = "12.0";
        public static string mSqlCon = string.Empty;
        public static string mBlockStatus = string.Empty;
        public static string mPrn = string.Empty;
        public static string mDataSource = string.Empty;
        public static string mDbUser = string.Empty;
        public static string mDatabase = string.Empty;
        public static string mDbPassword = string.Empty;
        public static string mAppUserName = string.Empty;
        public static string mAppUserType = string.Empty;
        public static string mAppUserID = string.Empty;
        public static string mApp = "GREENPLY";
        public static string mComPortName = string.Empty;
        // public static string mPrinterName = "ZDesigner ZM400 200 dpi (ZPL)";
        public static string mPrinterName = "";
        public static string mLabelSize = string.Empty;
        public static DateTime mSysDate;
        public static string mSrNo = string.Empty;
        public static string mBarcode = string.Empty;
        public static bool start = false;
        public static bool dbsetting = false;
        public static bool usermaster = false;
        public static bool reprint = false;
        public static string _STATUS = string.Empty;
        public static string _Barcode = string.Empty;
        public static string SuccessPrint = "Printed successfully";
        public static string SuccessUnlock = "Unlock successfully";
        public static string SuccessAdd = "Data successfully added";
        public static string SuccessDelete = "Data successfully deleted";
        public static string SuccessEdit = "Data successfully updated";
        public static string DeleteConfirmation = "Are you sure?";
        public static string SuccessSave = "Data successfully saved";
        public static string printingConfirmation = "Are you sure to print?";
        public static string RollBackConfirmation = "Are you sure to rollback scan data ?";
        //SAP
        public static string mSapUser = "";
        public static string mSapPassword = "";
        public static string mSapClient = "";
        public static string mSapServer = "";
        public static string mSapSystemNo = "";
        public static string mSapLanguage = "";
        public static string mBatchNumber = "";
        public static string mItemCode = "";
        public static string mQCStatus = "";
        public static string mCarbon = "";
        public static string mCromium = "";
        public static string mJominy = "";
        public static string mMag = "";
        public static string mMn = "";
        public static int mQCPrintQty = 0;
        public static string sLabelType = "";
        //End 


        //added by SANTOSH: for User Rights:
        public static string mUserCategory = string.Empty;
        public static string mUserStatus = string.Empty;
        public static string mUserTypeAdmin = string.Empty;
        public static string mUserPassword = string.Empty;
        public static string mUserRollID = string.Empty;
        public static int mUserIDint = 0;
        public static DataSet dsrights = new DataSet();
        public static string sModelNo = " ";
        public static string mPlantCode = "";
        public static string mLocationType = string.Empty;
        public static int EnteredQty { get; set; }
        public static string _PartialPrint = string.Empty;
        public static StringBuilder sbSaveCount = new StringBuilder();
        public static StringBuilder sbDuplicateCount = new StringBuilder();
        public static bool sCreditDetails = false;
        public static bool sPartWorking = false;
        public static bool sSaleWorking = false;
        public static DataTable _dtCDExcel = new DataTable();
        public static DataTable _dtPWExcel = new DataTable();
        public static DataTable _dtSWExcel = new DataTable();
        public static DataTable dtGroupRights = new DataTable();
        public static DataTable dtVlidateuser = new DataTable();
        public static string[] _strRights;
        public static string SelectedItem = string.Empty;
        public static DataTable PlantCode = null;
        public static string sMailStatus = string.Empty;
        public static string sFileName = string.Empty;
        public static string sReportName = string.Empty;
        public static string sRGPREPORT = string.Empty;
        //  public static string GetMonth = string.Empty;

        public static DateTime fdate1 { get; set; }
        public static DateTime tdate2 { get; set; }
        public static string fdate { get; set; }
        public static string tdate { get; set; }
        public static string GRNNo { get; set; }
        public static string mLocation { get; set; }
        public static int topitem { get; set; }
        public static string RESERVATION_NO { get; set; }
        

        public static string sTargetPath = AppDomain.CurrentDomain.BaseDirectory + "\\PROCESSED\\";
        public static void KillUnusedExcelProcess()
        {
            //Process[] oXlProcess = Process.GetProcessesByName("EXCEL");
            //foreach (Process oXLP in oXlProcess)
            //{
            //    oXLP.Kill();
            //}


        }

        public static void BindDropDown(ComboBox cmb, DataTable DT, string text, string value, string sSelect)
        {
            DataView dv = new DataView();
            if (DT.Rows.Count > 0)
            {
                cmb.DataContext = null;
                cmb.DisplayMemberPath = text;
                cmb.SelectedValuePath = value;
                cmb.ItemsSource = DT.DefaultView;
                //dv = DT.DefaultView;
                //cmb.ItemsSource = dv;
                //dv.Sort = DT.Columns[0].ToString() + " ASC";
                cmb.SelectedIndex = -1;
            }
            else
            {
                cmb.ItemsSource = DT.DefaultView;
            }
        }

        //public static DataTable IMPORT_DATA(string strPath, string ProcessType)
        //{
        //    string conn = "";
        //    DataTable dt = null;
        //    try
        //    {
        //        Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
        //        Microsoft.Office.Interop.Excel.Worksheet wsheet = new Microsoft.Office.Interop.Excel.Worksheet();
        //        Microsoft.Office.Interop.Excel.Workbook wbook = excelApp.Workbooks.Open(strPath, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        //        wsheet = (Microsoft.Office.Interop.Excel.Worksheet)excelApp.ActiveSheet;
        //        if (VariableInfo.gEXCELVER == "12.0")
        //        {
        //            conn = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + strPath + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\";";
        //        }
        //        else if (VariableInfo.gEXCELVER == "4.0")
        //        {
        //            conn = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + strPath + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\";";
        //        }
        //        else
        //        {
        //            return dt;
        //        }
        //        OleDbDataAdapter adp = new OleDbDataAdapter("select   * from [" + wsheet.Name + "$]  ", conn);
        //        dt = new DataTable();
        //        adp.Fill(dt);
        //        adp.Dispose();
        //        Marshal.ReleaseComObject(wsheet);
        //        Marshal.ReleaseComObject(wbook);
        //        wsheet = null;
        //        excelApp.Quit();
        //        Marshal.ReleaseComObject(excelApp);
        //        wbook = null;
        //        excelApp = null;
        //        GC.GetTotalMemory(false);
        //        GC.Collect();
        //        GC.WaitForPendingFinalizers();
        //        GC.Collect();
        //        GC.GetTotalMemory(true);
        //        DataTable filteredRows = dt.Rows.Cast<DataRow>().Where(row => row.ItemArray.Any(field => !(field is System.DBNull))).CopyToDataTable(); //use to remove excel's blank rows(Added by SANTOSH)
        //        return filteredRows;
        //    }
        //    catch
        //    {
        //        throw;
        //    }
        //}

        public static string NumberToText(int number, bool isUK)
        {
            if (number == 0) return "Zero";
            string and = isUK ? "and " : ""; // deals with UK or US numbering
            if (number == -2147483648) return "Minus Two Billion One Hundred " + and +
            "Forty Seven Million Four Hundred " + and + "Eighty Three Thousand " +
            "Six Hundred " + and + "Forty Eight";
            int[] num = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (number < 0)
            {
                sb.Append("Minus ");
                number = -number;
            }
            string[] words0 = { "", "One ", "Two ", "Three ", "Four ", "Five ", "Six ", "Seven ", "Eight ", "Nine " };
            string[] words1 = { "Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ", "Fifteen ", "Sixteen ", "Seventeen ", "Eighteen ", "Nineteen " };
            string[] words2 = { "Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ", "Seventy ", "Eighty ", "Ninety " };
            string[] words3 = { "Thousand ", "Million ", "Billion " };
            num[0] = number % 1000;           // units
            num[1] = number / 1000;
            num[2] = number / 1000000;
            num[1] = num[1] - 1000 * num[2];  // thousands
            num[3] = number / 1000000000;     // billions
            num[2] = num[2] - 1000 * num[3];  // millions
            for (int i = 3; i > 0; i--)
            {
                if (num[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (num[i] == 0) continue;
                u = num[i] % 10;              // ones
                t = num[i] / 10;
                h = num[i] / 100;             // hundreds
                t = t - 10 * h;               // tens
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i < first) sb.Append(and);
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd() + " Only";
        }

        public static string GetRights(string _PageName, DataTable dtRights)
        {
            char _RtVw = '0';
            char _RtSave = '0';
            char _RtEdit = '0';
            char _RtDlt = '0';
            char _RtExp = '0'; ;
            var vrCountry = (from country in dtRights.AsEnumerable()
                             where country.Field<string>("MODULE_NAME") == _PageName
                             select country);
            var rows = vrCountry.ToList();
            if (rows.Count > 0)
            {
                _RtVw = rows[0][7].ToString() == "True" ? '1' : '0';
                _RtSave = rows[0][8].ToString() == "True" ? '1' : '0';
                _RtEdit = rows[0][9].ToString() == "True" ? '1' : '0';
                _RtDlt = rows[0][10].ToString() == "True" ? '1' : '0';
                _RtExp = rows[0][11].ToString() == "True" ? '1' : '0';
            }
            return (_RtVw + "^" + _RtSave + "^" + _RtEdit + "^" + _RtDlt + "^" + _RtExp);
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

        public static string DecryptData(string EncryptedText, string Encryptionkey)
        {
            RijndaelManaged objrij = new RijndaelManaged();
            objrij.Mode = CipherMode.CBC;
            objrij.Padding = PaddingMode.PKCS7;
            objrij.KeySize = 0x80;
            objrij.BlockSize = 0x80;
            byte[] encryptedTextByte = Convert.FromBase64String(EncryptedText);
            byte[] passBytes = Encoding.UTF8.GetBytes(Encryptionkey);
            byte[] EncryptionkeyBytes = new byte[0x10];
            int len = passBytes.Length;
            if (len > EncryptionkeyBytes.Length)
            {
                len = EncryptionkeyBytes.Length;
            }
            Array.Copy(passBytes, EncryptionkeyBytes, len);
            objrij.Key = EncryptionkeyBytes;
            objrij.IV = EncryptionkeyBytes;
            byte[] TextByte = objrij.CreateDecryptor().TransformFinalBlock(encryptedTextByte, 0, encryptedTextByte.Length);
            return Encoding.UTF8.GetString(TextByte);
        }

        public static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        public static DataTable getDBServer()
        {
            DataTable dtDbServer = new DataTable();
            dtDbServer.Columns.Add("Display");
            dtDbServer.Columns.Add("Value");
            DataTable dtResults = SqlDataSourceEnumerator.Instance.GetDataSources();

            string strInstance;
            foreach (DataRow dr in dtResults.Rows)
            {
                if (dr["InstanceName"].ToString() != string.Empty)
                {
                    strInstance = "\\" + dr["InstanceName"].ToString();
                }
                else
                {
                    strInstance = string.Empty;
                }

                DataRow drRow = dtDbServer.NewRow();
                drRow["Display"] = dr["ServerName"].ToString() + strInstance;
                drRow["Value"] = dr["ServerName"].ToString() + strInstance;
                dtDbServer.Rows.Add(drRow);
            }

            return dtDbServer;
        }

        public static DataTable getdbserver1()
        {
            DataTable dtdbserver = new DataTable();
            dtdbserver.Columns.Add("Display");
            dtdbserver.Columns.Add("Value");
            DataTable dtresult1 = SqlDataSourceEnumerator.Instance.GetDataSources();

            string strInstance1;
            foreach (DataRow dr in dtresult1.Rows)
            {
                if (dr["InstanceName"].ToString() != string.Empty)
                {
                    strInstance1 = "\\" + dr["InstanceName"].ToString();
                }
                else
                {
                    strInstance1 = string.Empty;

                }

                DataRow drRow1 = dtdbserver.NewRow();
                drRow1["Display"] = dr["ServerName"].ToString() + strInstance1;
                drRow1["Value"] = dr["ServerName"].ToString() + strInstance1;

            }

            return dtdbserver;

        }

        public static DataTable getDBSchema(string strSource, string strUser, string strPwd)
        {
            try
            {
                DataTable dtSchema = new DataTable();
                dtSchema.Columns.Add("Display");
                dtSchema.Columns.Add("Value");
                string strCon = "Data Source=" + strSource + ";";
                strCon = strCon + " User ID=" + strUser + "; Password=" + strPwd + ";";

                SqlConnection oCon = new SqlConnection(strCon);
                oCon.Open();
                DataTable dtResults = oCon.GetSchema("Databases"); ;
                oCon.Close();
                foreach (DataRow dr in dtResults.Rows)
                {
                    DataRow drRow = dtSchema.NewRow();
                    drRow["Display"] = dr["database_name"].ToString();
                    drRow["Value"] = dr["database_name"].ToString();
                    dtSchema.Rows.Add(drRow);
                }
                return dtSchema;
            }
            catch (SqlException ex)
            {
                throw;
            }
        }

        public static string getPRN(string sPRNName)
        {
            string sPRN = string.Empty;
            try
            {
                if (File.Exists(System.Windows.Forms.Application.StartupPath + "\\" + sPRNName + ".prn"))
                {
                    StreamReader str = new StreamReader(System.Windows.Forms.Application.StartupPath + "\\" + sPRNName + ".prn");
                    sPRN = str.ReadToEnd();
                    str.Close();
                    str.Dispose();
                }
                else
                    return sPRN;
            }
            catch (Exception ex)
            {

                throw;
            }
            return sPRN;
        }
       
        //
        //public static string SendEmail1(string FilePath, BCILLogger.Logger oLog, string sTargetPath)
        //{

        //    string result = "SENT";
        //    string senderID = Convert.ToString(ConfigurationManager.AppSettings["UserId"]);
        //    string senderPassword = Convert.ToString(ConfigurationManager.AppSettings["Password"]);
        //    try
        //    {
        //        SmtpClient smtp = new SmtpClient
        //        {
        //            //EnableSsl = true,
        //            //DeliveryMethod = SmtpDeliveryMethod.Network,
        //            Host = Convert.ToString(ConfigurationManager.AppSettings["Host"]),
        //            Port = Convert.ToInt32(ConfigurationManager.AppSettings["PORT"]),
        //            Credentials = new NetworkCredential(senderID, senderPassword),
        //        };
        //        if (!Directory.Exists(sTargetPath))
        //        {
        //            Directory.CreateDirectory(sTargetPath);
        //        }
        //        //foreach (DataRow row in dtEmail.Rows)
        //        //{
        //        MailMessage message = new MailMessage();
        //        message.From = new MailAddress(senderID);
        //        message.To.Add(Convert.ToString(ConfigurationManager.AppSettings["TO"]));
        //        message.CC.Add(Convert.ToString(ConfigurationManager.AppSettings["CC"]));
        //        message.Bcc.Add(Convert.ToString(ConfigurationManager.AppSettings["BCC"]));
        //        message.Subject = (Convert.ToString(ConfigurationManager.AppSettings["SUBJECT"]));
        //        message.Body = (Convert.ToString(ConfigurationManager.AppSettings["BODY"]));
        //        message.IsBodyHtml = true;
        //        Attachment attachment;
        //        attachment = new Attachment(FilePath);
        //        message.Attachments.Add(attachment);
        //        oLog.WriteLog("File Attached: " + FilePath);
        //        try
        //        {
        //            smtp.Send(message);
        //            oLog.WriteLog("Mail Send: " + FilePath);
        //            message.Dispose();
        //            FileInfo fs = new FileInfo(FilePath);
        //            File.Copy(FilePath, sTargetPath + fs.Name, true);
        //            File.Delete(FilePath);
        //        }
        //        catch (Exception ex1)
        //        {
        //            message.Dispose();
        //            oLog.WriteLog(ex1);
        //        }
        //        //}
        //    }
        //    catch (Exception ex)
        //    {
        //        result = "ERROR";
        //        oLog.WriteLog("Error sending email. : " + ex.Message.ToString());
        //        throw ex;
        //    }
        //    return result;
        //}

        public static DataTable ToDataTable<T>(ObservableCollection<T> data)
        {
            PropertyDescriptorCollection props = TypeDescriptor.GetProperties(typeof(T));
            object[] values = new object[props.Count];
            using (DataTable table = new DataTable())
            {
                long _pCt = props.Count;
                for (int i = 0; i < _pCt; ++i)
                {
                    PropertyDescriptor prop = props[i];
                    table.Columns.Add(prop.Name, prop.PropertyType);
                }
                foreach (T item in data)
                {
                    long _vCt = values.Length;
                    for (int i = 0; i < _vCt; ++i)
                    {
                        values[i] = props[i].GetValue(item);
                    }
                    table.Rows.Add(values);
                }
                return table;
            }
        }

        public static bool ValidateInteger(string key)
        {
            try
            {
                Regex regex = new Regex("[^0-9]+");
                bool a = regex.IsMatch(key);
                return a;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetMonthDetails()
        {
            string GetMonth = DateTime.Now.ToString("MM");
            switch (GetMonth)
            {
                case "01":
                    GetMonth = "A";
                    break;
                case "02":
                    GetMonth = "B";
                    break;
                case "03":
                    GetMonth = "C";
                    break;
                case "04":
                    GetMonth = "D";
                    break;
                case "05":
                    GetMonth = "E";
                    break;
                case "06":
                    GetMonth = "F";
                    break;
                case "07":
                    GetMonth = "G";
                    break;
                case "08":
                    GetMonth = "H";
                    break;
                case "09":
                    GetMonth = "I";
                    break;
                case "10":
                    GetMonth = "J";
                    break;
                case "11":
                    GetMonth = "K";
                    break;
                case "12":
                    GetMonth = "L";
                    break;
                default:
                    break;
            }
            return GetMonth;
        }

        public static string GetYearDetails()
        {
            string GetYear = DateTime.Now.ToString("yy");
            switch (GetYear)
            {
                case "18":
                    GetYear = "A";
                    break;
                case "19":
                    GetYear = "B";
                    break;
                case "20":
                    GetYear = "C";
                    break;
                case "21":
                    GetYear = "D";
                    break;
                case "22":
                    GetYear = "E";
                    break;
                case "23":
                    GetYear = "F";
                    break;
                case "24":
                    GetYear = "G";
                    break;
                case "25":
                    GetYear = "H";
                    break;
                case "26":
                    GetYear = "I";
                    break;
                case "27":
                    GetYear = "J";
                    break;
                case "28":
                    GetYear = "K";
                    break;
                case "29":
                    GetYear = "L";
                    break;
                case "30":
                    GetYear = "M";
                    break;
                default:
                    break;
            }
            return GetYear;
        }

        public static string GetDayDetails()
        {
            string GetDay = DateTime.Now.ToString("dd");
            switch (GetDay)
            {
                case "01":
                    GetDay = "1";
                    break;
                case "02":
                    GetDay = "2";
                    break;
                case "03":
                    GetDay = "3";
                    break;
                case "04":
                    GetDay = "4";
                    break;
                case "05":
                    GetDay = "5";
                    break;
                case "06":
                    GetDay = "6";
                    break;
                case "07":
                    GetDay = "7";
                    break;
                case "08":
                    GetDay = "8";
                    break;
                case "09":
                    GetDay = "9";
                    break;
                case "10":
                    GetDay = "A";
                    break;
                case "11":
                    GetDay = "B";
                    break;
                case "12":
                    GetDay = "C";
                    break;
                case "13":
                    GetDay = "D";
                    break;
                case "14":
                    GetDay = "E";
                    break;
                case "15":
                    GetDay = "F";
                    break;
                case "16":
                    GetDay = "G";
                    break;
                case "17":
                    GetDay = "H";
                    break;
                case "18":
                    GetDay = "I";
                    break;
                case "19":
                    GetDay = "J";
                    break;
                case "20":
                    GetDay = "K";
                    break;
                case "21":
                    GetDay = "L";
                    break;
                case "22":
                    GetDay = "M";
                    break;
                case "23":
                    GetDay = "N";
                    break;
                case "24":
                    GetDay = "O";
                    break;
                case "25":
                    GetDay = "P";
                    break;
                case "26":
                    GetDay = "Q";
                    break;
                case "27":
                    GetDay = "R";
                    break;
                case "28":
                    GetDay = "S";
                    break;
                case "29":
                    GetDay = "T";
                    break;
                case "30":
                    GetDay = "U";
                    break;
                case "31":
                    GetDay = "V";
                    break;
                default:
                    break;
            }
            return GetDay;
        }

        public static bool ValidateSpecialCharacters(string key)
        {
            try
            {
                Regex regex = new Regex(@"^*'(?:\.*)?$");
                bool a = regex.IsMatch(key);
                return a;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public static bool ExportInExcel(System.Data.DataTable dt)
        //{
        //    Microsoft.Office.Interop.Excel._Application excel = new Microsoft.Office.Interop.Excel.Application();
        //    Microsoft.Office.Interop.Excel.Workbooks workbooks = excel.Workbooks;
        //    Microsoft.Office.Interop.Excel._Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
        //    Microsoft.Office.Interop.Excel._Worksheet worksheet = (Microsoft.Office.Interop.Excel._Worksheet)workbook.Worksheets[1];
        //    Microsoft.Office.Interop.Excel.Range range;
        //    Object[] data;

        //    for (int j = 0; j < dt.Columns.Count; j++)
        //    {
        //        if (j <= 25)
        //            range = (Microsoft.Office.Interop.Excel.Range)worksheet.get_Range(Convert.ToChar(65 + j) + 1.ToString(), System.Reflection.Missing.Value);
        //        else
        //            range = (Microsoft.Office.Interop.Excel.Range)worksheet.get_Range("A" + Convert.ToChar(65 + j - 26) + 1.ToString(), System.Reflection.Missing.Value);
        //        data = new object[] { dt.Columns[j].Caption };
        //        range.NumberFormat = "@";
        //        range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
        //    }
        //    for (int i = 0; i < dt.Rows.Count; i++)
        //    {
        //        for (int j = 0; j < dt.Columns.Count; j++)
        //        {
        //            if (j <= 25)
        //                range = (Microsoft.Office.Interop.Excel.Range)worksheet.get_Range(Convert.ToChar(65 + j) + Convert.ToString(2 + i), System.Reflection.Missing.Value);
        //            else
        //                range = (Microsoft.Office.Interop.Excel.Range)worksheet.get_Range("A" + Convert.ToChar(65 + j - 26) + Convert.ToString(2 + i), System.Reflection.Missing.Value);
        //            data = new object[] { dt.Rows[i][j] };
        //            range.NumberFormat = "@";
        //            range.GetType().InvokeMember("Value", System.Reflection.BindingFlags.SetProperty, null, range, data);
        //        }
        //    }
        //    worksheet.Columns.AutoFit();
        //    bool Iscancelled = true;
        //    System.Windows.Forms.FileDialog oDialog = new System.Windows.Forms.SaveFileDialog();
           
        //    if (oDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //    {
        //        oDialog.DefaultExt = "xls";
        //        string sFilename = oDialog.FileName;
        //        if (File.Exists(sFilename) == true)
        //            File.Delete(sFilename);
        //        workbook.Close(true, sFilename, null);

        //    }
        //    else
        //    {
        //        Iscancelled = false;
        //    }
        //    return Iscancelled;

        //}

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }

            }

            return dt;
        }

        #region  Alpha Numeric Validation

        public static bool ValidateSystemKeys(System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Back || e.Key == Key.Delete || e.Key == Key.Tab || e.Key == Key.Right || e.Key == Key.Left || e.Key == Key.Up || e.Key == Key.Down || e.Key == Key.Home || e.Key == Key.End)
                { return true; }
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static void AlphaNumberic(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (ValidateSystemKeys(e))
                { }
                else if ((e.Key >= Key.A && e.Key <= Key.Z) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key >= Key.D0 && e.Key <= Key.D9))
                {

                }
                else
                    e.Handled = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void NumericValidation(System.Windows.Input.KeyEventArgs e)
        {
            if (ValidateSystemKeys(e))
            { }
            else if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Back || e.Key == Key.Delete)
            {

            }
            else
            {
                e.Handled = true;
            }
        }

        #endregion


    }
}
//                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                             