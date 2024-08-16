using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data.OleDb;
using System.Drawing;
using System.Data;

namespace GreenplyCentralToLocalWebApi.BI
{
    internal static class VariableInfo
    {
        #region "Public Variable"


        public static string mAppFilePath = "\\Application";
        public static string mDeviceRootDir = "\\Application\\DESPATCH\\";
        public static string mFile = "DESPATCH.TXT";        
        public static bool mliveflag = false;
        public static string mSerialPortName = "Com1";
        public static Hashtable mHtInput = new Hashtable();
        public static string merrorMsg = string.Empty;
        public static bool IsManual = false;
        //public static string mUdCodeFile = "UD-CODE.TXT";
       // public static TcpClient mTcpClient;
        internal static string mSockIp = "192.168.1.103";
        // internal static string mSockIp = "192.168.1.173";
        internal static string mPrinterIp = "192.168.1.238";
        internal static Int32 mSockPort = 9100;
        internal static string mAppVersion = "1.0.0.2";
        internal static string mApps = "HUL";
       // public static SerialPort mSerialPort = null;
        public static string _sType = string.Empty;
        public static string UserID = string.Empty;
        public static string errSoundfile = "Application\\Track\\Error.wav";
        public static string beepsoundfile = "Application\\Track\\beep1.wav";
        public static string LastBinScan = string.Empty;

        internal static string mMaterilaPlanner = "L001";

        internal static string mSockServer = "SOCK~SERVER";
        internal static string mSapServer = "";
        internal static string mSapClient = "";
        internal static string mSapLng = "";
        internal static string mSapSysNo = "";
        internal static string mSapUser = "";
        internal static string mSapPassword = "";
        internal static string mSqlDb = "";
        internal static string mSqlServer = "";
        internal static string mSqlUser = "";
        internal static string mSqlPwd = "";
        internal static string mVersion = "";
         
        internal static string mScedulerAutoStartTime = "";
       // internal static string mClientSettingFile = Application.StartupPath + @"\ClientSetting.ini";
       
       // public static Logger _log = new Logger();
        internal static Hashtable mHtServerReboot = new Hashtable();
        internal static BcilLib.BcilLogger mAppLog = null;

        internal static string sSCHEMA = "";


        #endregion

    }
}
