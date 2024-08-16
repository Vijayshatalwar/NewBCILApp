using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Data;

namespace  GreenplyScannerCommServer.Common
{
    public class PCommon
    {
        public string ERROR { get; set; }
        public static string strCnn;
        public static string strCenCnn;

        public static string strCenConfigPath = Application.StartupPath;
        public static string strCenConfigFile = "";
        public static string strLocalConfigPath = Application.StartupPath;
        public static string strLocalConfigFile = "\\localSetting.ini";
        public static string strSiteConfig = "";
        public static string strDemoConfig = "";
        public static string strSiteData;
        public static string strApplicationType;
        public static string SPLITER = "~";


        
        public static string LoginUserID
        {
            get;
            set;
        }

        public static string LoginUserName
        {
            get;
            set;
        }

        public static string LoginSiteCode
        {
            get;
            set;
        }

        public static string LoginSiteName
        {
            get;
            set;
        }

        public static string LoginSiteId
        {
            get;
            set;
        }

        public static string LoginSiteType
        {
            get;
            set;
        }

        public static string LoginDefaultRights
        {
            get;
            set;
        }

        public static string LoginMasterRights
        {
            get;
            set;
        }

        public static string LoginUserType
        {
            get;
            set;
        }

        public static DateTime LoginDate
        {
            get;
            set;
        }

        public DataGridView dgvDetails
        {
            get;
            set;
        }

        public DataGridView dgvErrorDetails
        {
            get;
            set;
        }

        public GroupBox grpDetails
        {
            get;
            set;
        }

        public GroupBox grpSearch
        {
            get;
            set;
        }

        public GroupBox grpActions
        {
            get;
            set;
        }

        public Label Count
        {
            get;
            set;
        }

        public string Event
        {
            get;
            set;
        }

        public string FormName
        {
            get;
            set;
        }

        public string TableName
        {
            get;
            set;
        }

        public ComboBox Combo
        {
            get;
            set;
        }

        public string ComboType
        {
            get;
            set;
        }

        public bool SelectInCombo
        {
            get;
            set;
        }

        public string SqlQuery
        {
            get;
            set;
        }

        public string Filter
        {
            get;
            set;
        }

        public CheckedListBox CheckedListBox
        {
            get;
            set;
        }

        public Hashtable Hashtable
        {
            get;
            set;
        }

        public DataTable Datatable
        {
            get;
            set;
        }

        public DataTable Datatable2
        {
            get;
            set;
        }

        public string SerialNo
        {
            get;
            set;
        }

        public string SerialNoType
        {
            get;
            set;
        }

        public bool IsError
        {
            get;
            set;
        }

        public string SiteCode
        {
            get;
            set;
        }

        public string SiteName
        {
            get;
            set;
        }

        public string SiteAddress
        {
            get;
            set;
        }

        public string CategoryName
        {
            get;
            set;
        }

        public string LocCode
        {
            get;
            set;
        }

        public string FromDate
        {
            get;
            set;
        }

        public string ToDate
        {
            get;
            set;
        }

        public int PrintQty
        {
            get;
            set;
        }

        public string ID
        {
            get;
            set;
        }

        public int StockAge
        {
            get;
            set;
        }

        public string ItemCode
        {
            get;
            set;
        }

        public string Destination
        {
            get;
            set;
        }

        public string SiteType
        {
            get;
            set;
        }

        public string Module
        {
            get;
            set;
        }

        public bool IsModuleExist
        {
            get;
            set;
        }

        public bool ModuleActive
        {
            get;
            set;
        }

        public string Prn
        {
            get;
            set;
        }

        public string Printer
        {
            get;
            set;
        }

        public string Filter1
        {
            get;
            set;
        }
    }
}
