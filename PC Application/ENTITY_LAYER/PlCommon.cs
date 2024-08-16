using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ENTITY_LAYER
{
    public class PlCommon
    {

        public static string gDATASOURCE;
        public static string gUSER;
        public static string gDATABASE;
        public static string gPASSWORD;
        public static string gEXCELVER;

        public static string gUser;
        public static string gSysip;
        public static string gPlantCode;
        public static string gPlantName;
        public static int gMatExpiryDays;
        public static string gPrinterName;
        private string _user;
        public string User
        {
            get { return _user; }
            set
            {
                _user = value;
            }
        }

        private string _pwd;
        public string Pwd
        {
            get { return _pwd; }
            set
            {
                _pwd = value;
            }
        }

        private static string _strcon;
        public static string StrCon
        {
            get { return _strcon; }
            set
            {
                _strcon = value;
            }
        }

        private static string _strconCentral;
        public static string StrConCentral
        {
            get { return _strconCentral; }
            set
            {
                _strconCentral = value;
            }
        }

        private static string _strPlant;
        public static string StrPlant
        {
            get { return _strPlant; }
            set
            {
                _strPlant = value;
            }
        }

    }
}
