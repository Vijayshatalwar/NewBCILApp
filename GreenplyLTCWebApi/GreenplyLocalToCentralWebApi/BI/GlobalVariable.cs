using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using BcilLib;
using GreenplyLocalToCentralWebApi;

using System.Data;
/*
 * ========================================================================================
 * Procedure/Module         :   GLOBAL DECLARATION CLASS
 * Purpose                  :   USE FOR DECLARE OBJECT
 * Author                   :   Dipak Pathak
 * Initial Implementation   :   22/06/11
 * Change 1:                :   
 * Change 1: Remarks        :
 * Change 2:                :   
 * Change 2: Remarks        :
 * Copyright (c) Bar Code India Ltd. All rights reserved.
 * ========================================================================================
*/
namespace GreenplyLocalToCentralWebApi
{
    public class GlobalVariable
    {
        public static string mSockServer = "SOCK~SERVER";
        public static BcilLogger mAppLog = new BcilLib.BcilLogger();
        public static string mSqlConString = "";
        public static string mSqlDb = "";
        public static string mSqlServer = "";
        public static string mSqlUser = "";
        public static string mSqlPwd = "";
        internal static Int32 mSockPort = 0;
        public static string mPrinter = "";
        public static string mPrinterIp = "";
        public static string mPrinterType = "LAN";
        public static string mAppUser = "";
     
      
      
         

       

    }

}
