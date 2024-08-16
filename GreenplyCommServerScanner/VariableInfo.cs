using System;
using System.Collections.Generic;
using System.Collections;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Text;
using GreenplyScannerCommServer;

namespace GreenplyScannerCommServer
{
    internal static class VariableInfo
    {
        #region "Public Variable"
        internal static string mSockServer = "SOCK~SERVER";
        internal static string mSapServer = string.Empty;
        internal static string mSapClient = string.Empty;
        internal static string mSapLng = string.Empty;
        internal static string mSapSysNo = string.Empty;
        internal static string mSapUser = string.Empty;
        internal static string mSapPassword = string.Empty;
        internal static string mSqlDb = string.Empty;
        internal static string mSqlServer = string.Empty;
        internal static string mSqlUser = string.Empty;
        internal static string mSqlPwd = string.Empty;
        internal static string mVersion = string.Empty;
        internal static Int32 mSockPort = 6161;
        internal static string mScedulerAutoStartTime = string.Empty;
        internal static string mDbPath = Application.StartupPath + @"\Db_LotTracking.mdb";
        internal static string mClientSettingFile = Application.StartupPath + @"\ClientSetting.ini";
       // public static SQL Odb = new SQL();
       //
      //  public static BcilLib.Logger _log = new Logger();
        internal static Hashtable mHtServerReboot = new Hashtable();
        internal static OleDbConnection mOleDbConnection;
        internal static BcilLib.BcilLogger mAppLog = null;
        internal static string AppPlantCode = string.Empty;
        internal static string AppUserCode = string.Empty;



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

        #endregion


    }
}
