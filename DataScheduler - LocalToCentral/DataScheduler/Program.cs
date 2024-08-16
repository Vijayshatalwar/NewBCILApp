using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.IO;

namespace DataScheduler
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
                if (!Directory.Exists(Application.StartupPath + "\\SchedulerLogFiles"))
                {
                    Directory.CreateDirectory(Application.StartupPath + "\\SchedulerLogFiles");
                }
                bool createdNew;
                System.Threading.Mutex m = new System.Threading.Mutex(true, Application.ProductName, out createdNew);
                if (!createdNew)
                {
                    MessageBox.Show("Local Data Scheduler is already running", "Local Data Scheduler", MessageBoxButtons.OK, MessageBoxIcon.Asterisk, MessageBoxDefaultButton.Button1);
                    return;
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmMain());

            }
            catch (Exception)
            {
                throw;
            }
           
        }
    }
}
