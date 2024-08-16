using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using GreenplyCommServer;

namespace TEST
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                if (!Directory.Exists(Application.StartupPath + "\\Log"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\Log");
                }
                if (!Directory.Exists(Application.StartupPath + "\\CommServerLog"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\CommServerLog");
                }
                bool createdNew;
                System.Threading.Mutex m = new System.Threading.Mutex(true, Application.ProductName, out createdNew);
                if (!createdNew)
                {
                    MessageBox.Show("Communication server is already running", "Communication Server", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    return;
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                VariableInfo.mAppLog = new BcilLib.BcilLogger();
                VariableInfo.mAppLog.ChangeInterval = BcilLib.BcilLogger.ChangeIntervals.ciDaily;
                VariableInfo.mAppLog.EnableLogFiles = true;
                //VariableInfo.mAppLog.LogDays = 5;
                VariableInfo.mAppLog.LogFilesExt = "log";
                VariableInfo.mAppLog.LogFilesPath = Application.StartupPath;
                VariableInfo.mAppLog.LogFilesPrefix = "Greenply_Android";
                VariableInfo.mAppLog.StartLogging();
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "BcilAppsInitialize" + "  ::  Main", "Initializing Application.......");
                //GlobalVariable.mAppLog = _obj;
                //_obj = null;
                Application.Run(new frmMain());
            }
            catch (Exception ex)
            {
                GlobalVariable.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "_RemoteClient_OnDataArrival", "Communication Server : " + ex.Message);
            }

            //Application.EnableVisualStyles();
            //Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new frmMain());





        }
    }
}