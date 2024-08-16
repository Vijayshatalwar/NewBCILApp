using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DataScheduler
{
    public class WriteLogFile
    {
        public void WriteLog(string LogMsg)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            string logFilePath;
            logFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\SchedulerLogFiles\\" + Properties.Settings.Default.PrintSection + "_SchedulerLogFile-" + System.DateTime.Today.ToString("dd-MM-yyyy") + "." + "txt";
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
            log.WriteLine("(Version: 1.1.0) : " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " " + LogMsg);
            log.Close();
        }
    }
}
