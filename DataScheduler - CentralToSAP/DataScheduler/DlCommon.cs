using System;
using System.Collections.Generic;
using System.Text;
using DATA_ACCESS_LAYER;


namespace DataScheduler
{
    public class DlCommon
    {
        public static string StrCon = string.Empty;
        public static string StrClientCon = string.Empty;
        public DBManager ClientDBProvider()
        {
            try
            {
                DBManager oManager = new DBManager(DataProvider.SqlServer, clsGlobal.StrClientCon);
                return oManager;
            }

            catch (Exception ex)
            { throw ex; }
        }

        public DBManager DBProvider()
        {
            try
            {
                DBManager oManager1 = new DBManager(DataProvider.SqlServer, clsGlobal.StrCon);
                return oManager1;
            }

            catch (Exception ex)
            { throw ex; }
        }

    }
}
