using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data;
using GreenplyCommServer.Common;
using BCILLogger;
using System.IO;
using BcilLib;

/*
 * ========================================================================================
 * Procedure/Module         :   GLOBAL DECLARATION CLASS
 * Purpose                  :   USE FOR DECLARE OBJECT
 * Author                   :   dharmendra rawat
 * Initial Implementation   :   03/07/18
 * Change 1:                :   
 * Change 1: Remarks        :
 * Change 2:                :   
 * Change 2: Remarks        :
 * Copyright (c) Bar Code India Ltd. All rights reserved.
 * ========================================================================================
*/
namespace GreenplyCommServer
{
    public class GlobalVariable
    {
        public static BcilLogger mAppLog = new BcilLogger();
        public static string mSockServer = "SOCK~SERVER";
        public static Logger AppLog;
       public static DCommon _clsSql = new DCommon();
        //public DCommon _clsSql = new DCommon();
        public static string mSqlConString = "";
        public static string mSqlDb = "";
        public static string mSqlServer = "";
        public static string mSqlUser = "";
        public static string mSqlPwd = "";
        internal static Int32 mSockPort = 9100;
        public static string mPrinter = "";
        public static string mPrinterIp = "";
        public static string sMessageBox = "GreenplyCommServer";
        public static string sPrintedQty = "";
        public static string sScannedQty = "0";
        public static string sAppConQty = "0";
        public static Int64 iModuloQty = 0;
        public static string mPlantCode = "";
        public static string mSite_ID = "";
        public static string mAppUser = "";
        public static string mAppUserFullName = "";
        public static string mAppUserType = "";
        public static string mAppProcessType = "";
        //public static string mSapServer = "";
        //public static string mSapClient = "";
        //public static string mSapLng = "";
        //public static string mSapSysNo = "";
        //public static string mSapUser = "";
        //public static string mSapPassword = "";
        public static string mSiteCode = "";
        public static string mUserId = "";
        //public static string SystemUID = "BCIL";
        //public static string SystemPWD = "sal@1234";


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

        public static string DtToString(DataTable dt)
        {
            string sRow = string.Empty;
            string sDTString = string.Empty;
            if (dt.Rows.Count != 0)
            { 
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sCol = string.Empty;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        sCol = sCol + dt.Rows[i][j].ToString() + " $ ";
                    }
                    sRow = sRow + sCol.Substring(0, sCol.Length - 1) + " # ";
                }
                sDTString = sRow.Substring(0, sRow.Length - 2);
            }
            return sDTString;
        }

        public static string DtToString2(DataTable dt)
        {
            string sRow = string.Empty;
            string sDTString = string.Empty;
            if (dt.Rows.Count != 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sCol = string.Empty;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        sCol = sCol + dt.Rows[i][j].ToString() + " $ ";
                    }
                    sRow = sRow + sCol.Substring(0, sCol.Length - 1) + " # ";
                }
                sDTString = sRow.Substring(0, sRow.Length );
            }
            return sDTString;
        }

        public static string DtToString1(DataTable dt)
        {
            string sRow = string.Empty;
            string sDTString = string.Empty;
            if (dt.Rows.Count != 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string sCol = string.Empty;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        sCol = sCol + dt.Rows[i][j].ToString();
                    }
                    sRow = sRow + sCol.Substring(0, sCol.Length) + " # ";
                }
                sDTString = sRow.Substring(0, sRow.Length - 1);
            }
            return sDTString;
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
                throw ex;
            }
            return sPRN;
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

    }

}
