using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Management;
using System.Reflection;
using System.IO;
using System.Printing;

namespace COMMON
{
    [StructLayout(LayoutKind.Sequential)]
    public struct DOCINFO
    {
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pDocName;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pOutputFile;
        [MarshalAs(UnmanagedType.LPWStr)]
        public string pDataType;
    }

    public class PrintBarcode
    {
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern long OpenPrinter(string pPrinterName, ref IntPtr phPrinter, int pDefault);
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern long StartDocPrinter(IntPtr hPrinter, int Level, ref DOCINFO pDocInfo);
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern long StartPagePrinter(IntPtr hPrinter);
        [DllImport("winspool.drv", CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern long WritePrinter(IntPtr hPrinter, string data, int buf, ref int pcWritten);
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern long EndPagePrinter(IntPtr hPrinter);
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern long EndDocPrinter(IntPtr hPrinter);
        [DllImport("winspool.drv", CharSet = CharSet.Unicode, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern long ClosePrinter(IntPtr hPrinter);

        public static bool PrintCommand(string PrintData, string PrinterName)
        {
            try
            {
            PrintAgain:
                System.Windows.Forms.Application.DoEvents();
                if (GetNumberOfPrintJobs(PrinterName) < 10)
                {
                    System.IntPtr lhPrinter = new System.IntPtr();
                    DOCINFO di = new DOCINFO();
                    di.pDocName = "Bcil";
                    int pcWritten = 0;
                    int iprinter = 0;
                    for (int i = 0; i <= System.Drawing.Printing.PrinterSettings.InstalledPrinters.Count - 1; i++)
                    {
                        if (Convert.ToString(System.Drawing.Printing.PrinterSettings.InstalledPrinters[i]).ToUpper() == PrinterName.ToUpper())
                        {
                            iprinter = 1;
                            break; // TODO: might not be correct. Was : Exit For 
                        }
                    }
                    if (iprinter == 1)
                    {
                        Console.WriteLine(PrinterName);
                        PrintBarcode.OpenPrinter(PrinterName, ref lhPrinter, 0);
                        if (lhPrinter == IntPtr.Zero)
                        {
                            Console.WriteLine("Printer not found");
                            return false;
                        }
                        PrintBarcode.StartDocPrinter(lhPrinter, 1, ref di);
                        PrintBarcode.StartPagePrinter(lhPrinter);
                        PrintBarcode.WritePrinter(lhPrinter, PrintData, PrintData.Length, ref pcWritten);
                        PrintBarcode.EndPagePrinter(lhPrinter);
                        PrintBarcode.EndDocPrinter(lhPrinter);
                        PrintBarcode.ClosePrinter(lhPrinter);
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                { goto PrintAgain; }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static bool IsPrinterAvailable(string sPrinterName)
        {
            try
            {

                // Select Printers from WMI Object Collections  
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_Printer WHERE Name='" + sPrinterName + "'");
                ManagementObjectCollection collection = searcher.Get();

                foreach (ManagementObject printer in collection)
                {
                    Console.WriteLine("********** Printer = " + printer["Name"] + " **********");
                    if (Convert.ToString(printer["WorkOffline"]).ToLower().Equals("true"))
                    {
                        // printer is offline by user
                        Console.WriteLine("Your Plug-N-Play printer is not connected.");
                        printer.Dispose();
                        collection.Dispose();
                        searcher.Dispose();
                        return false;
                    }
                    else
                    {
                        // printer is not offline
                        Console.WriteLine("Your Plug-N-Play printer is connected.");
                        printer.Dispose();
                        collection.Dispose();
                        searcher.Dispose();
                        //clsSettings.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, Assembly.GetExecutingAssembly().GetName() + "::" + MethodBase.GetCurrentMethod().Name, "Ready");

                        return true;

                    }
                }
                collection.Dispose();
                searcher.Dispose();
                return false;
            }
            catch (Exception ex)
            {
                //clsSettings.mBcilLogger.LogMessage(BcilLib.EventNotice.EventTypes.evtError, Assembly.GetExecutingAssembly().GetName() + "::" + MethodBase.GetCurrentMethod().Name, ex.Message);
                throw ex;

            }
        }

        public static int GetNumberOfPrintJobs(string sPrinterName)
        {
            LocalPrintServer server = new LocalPrintServer();
            PrintQueueCollection queueCollection = server.GetPrintQueues();
            PrintQueue printQueue = null;

            foreach (PrintQueue pq in queueCollection)
            {
                if (pq.FullName == sPrinterName)
                    printQueue = pq;
            }

            int numberOfJobs = 0;
            if (printQueue != null)
                numberOfJobs = printQueue.NumberOfJobs;

            return numberOfJobs;
        }



    }
}