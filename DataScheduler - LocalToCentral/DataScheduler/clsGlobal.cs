using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BCILLogger;

namespace DataScheduler
{
    public static class clsGlobal
    
    {
        public static string mCServerName = "";
        public static string mCdatabase = "";
        public static string mCDbUser = "";
        public static string mCDbPassword = "";
        public static string mSapSysNo = "";
        public static string mCPlantCode = "";
        public static string mSapRtrStr = "";
        public static string StrClientCon = "";
        public static string StrCon = string.Empty;

        public static string mServerName = "";
        public static string mdatabase = "";
        public static string mDbUser = "";
        public static string mDbPassword = "";     
        public static string mSapLng = "";      
        public static Logger AppLog;
        public static string SystemUID = string.Empty;
        public static string SystemPWD = string.Empty;
        public static string PlantCode = string.Empty;
        public static int TimeInterval = 0;
        public static string SapLastUpdatedDate = string.Empty;
        public static string lmsg = string.Empty;
        public static string sSelectDate = string.Empty;
        public static string sPONumber = string.Empty;
        public static bool isBreak = false;
        public static int iAddCount = 0;
        public static int iUpdateCount = 0;
    }
    
}
