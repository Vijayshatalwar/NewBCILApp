using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Diagnostics;

namespace WepApiExample
{
    class Logger
    {

        #region PUBLIC PROPERTY OF CLASS

        private string errFile;

        private int maxFileSize = 1;
        /// <summary>
        /// Get or Set Max log file size in MB default 1
        /// </summary>
        public int MaxFileSize
        {
            get { return maxFileSize; }
            set { maxFileSize = value; }
        }

        private int deleteTime = 10;

        /// <summary>
        /// No of days after delete the previous file default 10 days
        /// </summary>
        public int NoOfDaysToDeleteFile
        {
            get { return deleteTime; }
            set { deleteTime = value; }
        }

        private string appPath;

        /// <summary>
        /// Get or Set Log Path
        /// </summary>
        /// 
        public string AppPath
        {

            get { return appPath; }
            set { appPath = value; }
        }
        private string m_ErrorFileName;
        /// <summary>
        /// Get or set error file name
        /// </summary>
        public string ErrorFileName
        {
            get { return m_ErrorFileName; }
            set { m_ErrorFileName = value; }
        }
        private string m_FileType;
        /// <summary>
        /// Set file type like error, Debug....
        /// </summary>
        public string FileType
        {
            get { return m_FileType; }
            set { m_FileType = value; }
        }
        private bool m_EnableLoging;
        /// <summary>
        /// Enable Loging
        /// </summary>
        public bool EnableLoging
        {
            get { return m_EnableLoging; }
            set { m_EnableLoging = value; }
        }
        #endregion

        TextWriter errorLog;

        public Logger()
        {
            errFileCounter = 0;

        }
        //Close open file on out of scop of object
        ~Logger()
        {
            if (errorLog != null)
            {
                try
                {
                    errorLog.Close();
                    errorLog.Dispose();
                }
                catch (Exception)
                {

                }
            }
        }

        /// <summary>
        /// Delete All 10 days old files
        /// </summary>
        void DeleteOldFiles()
        {
            foreach (string file in Directory.GetFiles(AppPath + "\\Log\\"))
            {
                try
                {
                    TimeSpan ts = new TimeSpan(NoOfDaysToDeleteFile, 0, 0, 0);
                    if (File.GetCreationTime(file).Date < DateTime.Now.Date.Subtract(ts))
                        File.Delete(file);
                }
                catch (Exception)
                {

                }
            }
        }

        int errFileCounter = 0;

        //Create the object of the file
        private TextWriter CreateFile()
        {
            if (errorLog != null)
            {
                errorLog.Close();
                errorLog.Dispose();
            }
            errLogTime = DateTime.Now;
            if (!Directory.Exists(AppPath + "\\Log"))
                Directory.CreateDirectory(AppPath + "\\Log");
            errFile = AppPath + "\\Log\\" + m_ErrorFileName + "_" + m_FileType + "_" + DateTime.Now.ToString("dd_MM_yyyy") + "_" + errFileCounter.ToString() + ".log";

            errorLog = new StreamWriter(errFile, true);
            errorLog = StreamWriter.Synchronized(errorLog);

            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(DeleteOldFiles));
            t.Start();

            FileInfo fp = new FileInfo(errFile);

            errFileSize = fp.Length;

            return errorLog;
        }

        private long errFileSize = 0;
        private DateTime errLogTime;

        /// <summary>
        /// Write String Message
        /// </summary>
        /// <param name="msg"></param>
        public void WriteLog(string msg)
        {
            try
            {
                //if (m_EnableLoging == false)
                //    return;
                if (errorLog == null)
                    errorLog = CreateFile();
                lock (errorLog)
                {
                    string Data = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:fff") + ": " + msg;
                    errFileSize += Data.Length + 1;
                    if (errFileSize > 1024 * 1024 * maxFileSize)
                    {
                        errFileCounter++;
                        errorLog = CreateFile();
                    }
                    if (ValidateFileTime(errLogTime))
                        CreateFile();
                    errorLog.WriteLine(Data);
                    errorLog.Flush();
                }
            }
            catch (Exception)
            {

            }
        }
        /// <summary>
        /// Write Exception
        /// </summary>
        /// <param name="ex"></param>
        public void WriteLog(Exception ex)
        {
            try
            {
                //if (m_EnableLoging == false)
                //    return;
                if (errorLog == null)
                    errorLog = CreateFile();
                lock (errorLog)
                {
                    string Data = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:fff") + ": " + ex.ToString();
                    errFileSize += Data.Length + 1;
                    if (errFileSize > 1024 * 1024 * maxFileSize)
                    {
                        errFileCounter++;
                        errorLog = CreateFile();
                    }
                    if (ValidateFileTime(errLogTime))
                        CreateFile();
                    errorLog.WriteLine(Data);
                    errorLog.Flush();
                }
            }
            catch (Exception)
            {

            }
        }

        /// <summary>
        /// Write Byte Data in Hex Format
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="len"></param>
        public void WriteLog(byte[] msg, int len)
        {
            string str = "";
            try
            {
                if (m_EnableLoging == false)
                    return;
                if (errorLog == null)
                    errorLog = CreateFile();
                lock (errorLog)
                {
                    for (int i = 0; i < len; i++)
                        str = str + msg[i].ToString("X").PadLeft(2, '0');

                    string Data = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss:fff") + ": " + str;
                    errFileSize += Data.Length + 1;
                    if (errFileSize > 1024 * 1024 * maxFileSize)
                    {
                        errFileCounter++;
                        errorLog = CreateFile();
                    }
                    if (ValidateFileTime(errLogTime))
                        CreateFile();
                    errorLog.WriteLine(Data);
                    errorLog.Flush();

                }
            }
            catch (Exception)
            {
            }
        }

        //validate current time 
        private bool ValidateFileTime(DateTime errLogTime)
        {
            if (DateTime.Now.Day != errLogTime.Day)
                return true;
            return false;
        }
    }
}
