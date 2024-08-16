using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using GreenplyLocalToCentralWebApi.BI;

namespace GreenplyLocalToCentralWebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            //string sLogPath = ConfigurationManager.AppSettings["LogFilePath"].ToString();
            //string sPath = HttpContext.Current.Server.MapPath(sLogPath);
            //System.IO.DirectoryInfo _dir = null;
            //_dir = new System.IO.DirectoryInfo(sPath);
            //if (_dir.Exists == false)
            //{
            //    _dir.Create();
            //    System.IO.Directory.CreateDirectory(_dir.ToString() + "LogFiles\\");
            //}
            //BcilLib.BcilLogger _obj = new BcilLib.BcilLogger();
            //_obj.ChangeInterval = BcilLib.BcilLogger.ChangeIntervals.ciDaily;
            //_obj.EnableLogFiles = true;
            //_obj.LogDays = 30;
            //_obj.LogFilesExt = "Log";
            //_obj.LogFilesPath = sPath;
            //_obj.LogFilesPrefix = "GreenplyL2CLog";
            //_obj.StartLogging();
            //_obj.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "BcilAppsInitialize" + "  ::  Main", "Initializing Application....... ");
            //VariableInfo.mAppLog = _obj;
            //_obj.StopLogging();
            //_obj = null;
            //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtData, Assembly.GetExecutingAssembly().GetName() + "::" + MethodBase.GetCurrentMethod().Name + "  ", "AppInitialize");
            //VariableInfo.mAppLog.StartLogging();
        }


    }

    public class WriteLogFile
    {
        public void WriteLog(string strLog)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            string logFilePath;
            logFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\LogFiles\\" + Properties.Settings.Default.PrintingSection + "_SchedulerLogFile_" + System.DateTime.Today.ToString("dd-MM-yyyy") + "." + "txt";
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
            log.WriteLine("(Version: 1.1.0) : " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " " + strLog);
            log.Close();
        }
    }
}
