using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ServerSock;
using System.Net;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using BCILCommServer;
using GreenplyCommServer.Common;
using System.Data.SqlClient;
using System.Data.Sql;
using GreenplyCommServer;
using GreenplyCommServer.BI;
using MOXA_CSharp_MXIO;
using TMSServer;
using TEST;
//using GreenplyCommServer.GreenplyERPPostingService;
using System.Linq;

namespace GreenplyCommServer
{
    public partial class frmMain : Form
    {
        static string strConfigLocaldb = Application.StartupPath + "\\localSetting.ini";
        BCILCommServer.BCILSocketServer _BCILSocketServer;
        ServerSocket m_Socket;
        Hashtable _RunningIP;
        ServerProcess _ServerProcess;
        PCommon clsPCommon = new PCommon();
        BCommon oBCommon = new BCommon();

        public static string sPrintingSection = string.Empty;
        public static string sLocationType = string.Empty;
        //
        bool isRunning;
        bool isNotRunning;

        private System.Threading.Thread myThread;
        B_ClsPrint objclsPrint;
        B_LoadingOffloading objclsload;
        B_StackPrinting objSegPrint;
        ioLogik obj_iologik;
        SendMoxa obj_sendmoxa;
        string objLocationCode;

        string objPrintedMatcode = string.Empty;
        string objPrintedMatGradeDesc = string.Empty;
        string objPrintedMatGroupDesc = string.Empty;
        string objPrintedMatThickness = string.Empty;
        string objPrintedMatSize = string.Empty;
        string objQRCode = string.Empty;
        string objStackQRCode = string.Empty;
        string visualBarcode = string.Empty;
        string oMonth = string.Empty;
        string oDay = string.Empty;
        string oYear = string.Empty;
        string oDateFormat = string.Empty;
        string sQRRunningSerial;
        string sStackRunningSerial;
        string DONo = string.Empty;
        string sMUserId = string.Empty;
        int sMatLotSize;
        int sMatLotPrintedQty = 0;
        int sMatPrintCount = 0;
        int sStackPrintCount = 0;
        int oSAPPostCount = 0;
        int oSaveCount = 0;
        int sSaveCount = 0;
        int oPostCount = 0;
        DataTable dtScannedData;
        DataTable dtLoadOffloadData;
        DataTable dtLoadOffData;
        DataTable dtDLoadOffData;
        DataTable dtDepotData;
        DataTable dtMTMData;
        DataTable dtStockCount;
        DataTable dtProdData;
        DataTable dtsStackData = new DataTable();
        DataTable dtFinalData = new DataTable();
        DataTable dtFinalDispatchData = new DataTable();
        public static DataTable dtStackData;
        DataSet dsData = new DataSet();
        DataSet dsProdData = new DataSet();
        DataTable dtFTPData = null;
        bool IsStackPrint = false;

        delegate void SetTextCallback(string text);

        // delegate void _dlgUpdateClient(string IP, string LogTime, string Message);
        // delegate void _dlgUpdateClient(string IP, string LogTime, string Message);

        delegate void _dlgCloseClient(string IP, string LogTime, string Message);
        public delegate void UpdateRichEditCallback(string msg);
        delegate void _dlgUpdateClient(string IP, string Message);
        LogFile log;
        public static string DbName;

        public frmMain()
        {
            InitializeComponent();
            dtScannedData = new DataTable();
            dtScannedData.Columns.Add("LocationCode");
            dtScannedData.Columns.Add("SalesReturnNo");
            dtScannedData.Columns.Add("MatCode");
            dtScannedData.Columns.Add("QRCode");
            dtScannedData.Columns.Add("Qty");

            dtLoadOffloadData = new DataTable();
            dtLoadOffloadData.Columns.Add("LocationCode");
            dtLoadOffloadData.Columns.Add("DONo");
            dtLoadOffloadData.Columns.Add("MatCode");
            dtLoadOffloadData.Columns.Add("QRCode");
            dtLoadOffloadData.Columns.Add("StackQRCode");

            dtDepotData = new DataTable();
            dtDepotData.Columns.Add("LocationCode");
            dtDepotData.Columns.Add("DONo");
            dtDepotData.Columns.Add("MatCode");
            dtDepotData.Columns.Add("QRCode");
            dtDepotData.Columns.Add("StackQRCode");

            dtFinalDispatchData.Columns.Add("LocationCode");
            dtFinalDispatchData.Columns.Add("MatCode");
            dtFinalDispatchData.Columns.Add("QRCode");
            dtFinalDispatchData.Columns.Add("StackQRCode");
            dtFinalDispatchData.Columns.Add("UserId");
            dtFinalDispatchData.Columns.Add("DONo");

            dtProdData = new DataTable();
            dtProdData.Columns.Add("LocationCode");
            dtProdData.Columns.Add("MatCode");
            dtProdData.Columns.Add("QRCode");
            dtProdData.Columns.Add("Status");

            dtStackData = new DataTable();
            dtStackData.Columns.Add("LocationCode");
            dtStackData.Columns.Add("MatCode");
            dtStackData.Columns.Add("QRCode");
            dtStackData.Columns.Add("Status");
            dtStackData.Columns.Add("GradeDesc");
            dtStackData.Columns.Add("GroupDesc");
            dtStackData.Columns.Add("ThicknessDesc");
            dtStackData.Columns.Add("MatSize");

            dtStockCount = new DataTable();
            dtStockCount.Columns.Add("LocationCode");
            dtStockCount.Columns.Add("QRCode");

            Messagetab.TabPages.Remove(tabPage1);
            Messagetab.TabPages.Remove(tabPage2);
            string _sResult = string.Empty;
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                txtMessage.Visible = false;
                _RunningIP = new Hashtable();
                GetConnect();
                _BCILSocketServer = new BCILSocketServer(VariableInfo.mSockPort, 100);  //VariableInfo.mSockPort
                _BCILSocketServer.OnClientConnect += new BCILSocketServer.NewClientHandler(_BCILSocketServer_OnClientConnect);
                _BCILSocketServer.EOMChar = ">";
                _BCILSocketServer.SessionTimeOut = 1;

                sPrintingSection = Properties.Settings.Default.PrintingSection.Trim().ToString();
                sLocationType = Properties.Settings.Default.PrintingLocationType.Trim().ToString();

                log = _BCILSocketServer.ActiveLog;
                log.EnableLogFiles = true;
                log.ChangeInterval = LogFile.ChangeIntervals.ciDaily;
                //log.LogDays = 5;
                log.LogFilesPath = Application.StartupPath + "\\CommServerLog\\";
                log.LogLevel = EventNotice.EventTypes.evtAll;
                log.StartLogging();
                _BCILSocketServer.StartService();
                obj_iologik = new ioLogik();
                obj_sendmoxa = new SendMoxa();
                cmdStart_Click(sender, e); //
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "Form1_Load", ex.Message);
            }
        }

        private void cmdStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (oBCommon.readConnectionSetting(false) == true)
                {
                    ServerDetails();
                    lblStatus.BackColor = Color.Green;
                    lblStatus.ForeColor = Color.White;
                    lblCount.BackColor = Color.Green;
                    lblCount.ForeColor = Color.White;
                    cmdStart.Enabled = false;
                    cmdStop.Enabled = true;
                    isRunning = true;
                    isNotRunning = false;
                    objLocationCode = GlobalVariable.mSiteCode.Trim();
                    _BCILSocketServer.StartService();
                    if (cmdStart.Enabled == false)
                    {
                        //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "cmdStart_Click", "Connected at");
                        myThread = new Thread(SchedularThreadConnectMode);
                        myThread.Start();
                    }
                }
                else
                {
                    lblStatus.Text = "Communication Server Stopped";
                    lblStatus.BackColor = Color.Red;
                    lblStatus.ForeColor = Color.White;
                    cmdStop.Enabled = false;
                    cmdStart.Enabled = true;
                    if (myThread.ThreadState == System.Threading.ThreadState.Running)
                        myThread.Abort();
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "cmdStart_Click", ex.Message);
                MessageBox.Show(ex.ToString());
            }
        }

        private void cmdStop_Click(object sender, EventArgs e)
        {
            //_BCILSocketServer.StopService();
            lblStatus.BackColor = Color.Red;
            lblStatus.ForeColor = Color.Black;
            cmdStop.Enabled = false;
            cmdStart.Enabled = true;
            lblStatus.Text = "Communication Server Stopped";
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "cmdStop_Click", lblStatus.Text);
            if (cmdStop.Enabled == false)
            {
                isRunning = false;
                myThread = new Thread(SchedularThreadConnectMode);
                myThread.Start();
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                _BCILSocketServer.StopService();
                if (_BCILSocketServer != null)
                    _BCILSocketServer = null;
                if (isRunning == true)
                    isRunning = false;
                if (myThread.ThreadState == System.Threading.ThreadState.Running)
                    myThread.Abort();
                this.Dispose();
                this.Close();
                Application.Exit();
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "btnExit_Click", ex.Message);
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _BCILSocketServer.StopService();
                _BCILSocketServer = null;
                myThread.Abort();
                this.Dispose();
                this.Close();
                Application.Exit();
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "frmMain_FormClosing", ex.Message);
            }
        }

        private void SchedularThreadConnectMode()
        {
            try
            {
                while (isRunning)
                {
                    //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "SchedularThreadConnectMode", "Thread Started");
                    RunProcess();
                    System.Threading.Thread.Sleep(Properties.Settings.Default.ThreadSleepInterval);
                    //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "SchedularThreadConnectMode", "Thread Sleep");
                    //FtpUpload();
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "SchedularThreadConnectMode", ex.Message);
                MessageBox.Show(ex.ToString());
            }
        }

        private void SchedularThreadDisconnectMode()
        {
            try
            {
                isRunning = false;
                while (isNotRunning)
                {
                    //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "SchedularThreadDisconnectMode", "Thread disconnected at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    //FtpUpload();
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "SchedularThreadDisconnectMode", ex.Message.ToString());
            }
        }


        bool isQRPrinted;
        private void RunProcess()
        {
            try
            {
                objQRCode = string.Empty;
                objStackQRCode = string.Empty;
                string objMatcode = string.Empty;
                string objMatDesc = string.Empty;
                string objMatGrade = string.Empty;
                string objMatGradeDesc = string.Empty;
                string objMatGroup = string.Empty;
                string objMatGroupDesc = string.Empty;
                string objMatThickness = string.Empty;
                string objMatThicknessDesc = string.Empty;
                string objMatSize = string.Empty;
                string objMatDesignNo = string.Empty;
                string objMatFinishCode = string.Empty;
                string objMatBatchNo = string.Empty;

                string objMatStatus = "P";
                DataTable objMatData = new DataTable();
                ////VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "RunProcess", "A");
                objclsPrint = new B_ClsPrint();
                obj_iologik = new ioLogik();
                string moxaIp = GreenplyCommServer.Properties.Settings.Default.MoxaDeviceIP;

                #region QRCode

                #region If Stack not printing in plant
                //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ProductionProcess", "Moxa connection start");

                obj_iologik.EthernetDIRead_Main(moxaIp);
                //string obj_itmstus = "ON";  // obj_iologik.Obj_ItmStatus;
                string obj_itmstus = obj_iologik.Obj_ItmStatus;
                //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ProductionProcess", "Moxa Status = " + obj_itmstus + " & QRPrinted = " + isQRPrinted);
                if (obj_itmstus == "ON" && !isQRPrinted)  //   
                {
                    ////VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "RunProcess", "B");
                    objMatData = objclsPrint.GetSelectedMatCode(objLocationCode);
                    ////VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "RunProcess", "C");
                    ////string StackStatus = objclsPrint.GetIsStackPrintStatus(objLocationCode);
                    if (objMatData.Rows.Count == 1)
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ProductionProcess", "Get Selected MatCode = " + objMatData.Rows[0][0].ToString());
                        objMatcode = objMatData.Rows[0][0].ToString();
                        objMatDesc = objMatData.Rows[0][1].ToString();
                        objMatGrade = objMatData.Rows[0][2].ToString();
                        objMatGradeDesc = objMatData.Rows[0][3].ToString();
                        objMatGroup = objMatData.Rows[0][4].ToString();
                        ////if (objMatGroup != "" && objMatGroup.Length >= 4)
                        ////    objMatGroup = objMatGroup.Substring(objMatGroup.Length - 4);//
                        objMatGroupDesc = objMatData.Rows[0][5].ToString();
                        objMatThickness = objMatData.Rows[0][6].ToString();
                        objMatThicknessDesc = objMatData.Rows[0][7].ToString();
                        objMatSize = objMatData.Rows[0][8].ToString();
                        objMatDesignNo = objMatData.Rows[0][9].ToString();
                        objMatFinishCode = objMatData.Rows[0][10].ToString();
                        objMatBatchNo = objMatData.Rows[0][11].ToString();
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ProductionProcess", "Get Selected MatCode Size = " + objMatData.Rows[0][8].ToString());
                    }
                    ////VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "RunProcess", "D");
                    if (objMatcode == "")
                    {
                        MessageBox.Show("There is no matrial selected from PC application to print the QRCode, First select the matrial from PC application to start printing");
                        if (cmdStart.Enabled == false)
                        {
                            lblStatus.BackColor = Color.Red;
                            lblStatus.ForeColor = Color.Black;
                            cmdStop.Enabled = false;
                            cmdStart.Enabled = true;
                            lblStatus.Text = "Communication Server Stopped";
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "cmdStop_Click", lblStatus.Text + " because of no material selected to print on conveyor");
                            if (cmdStop.Enabled == false)
                            {
                                isRunning = false;
                                myThread = new Thread(SchedularThreadDisconnectMode);
                                myThread.Start();
                            }
                        }
                        return;
                    }
                    ////
                    ////VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "RunProcess", "E");
                    ////VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Runprocess", "StackStatus = " + StackStatus);
                    ////if (Convert.ToBoolean(StackStatus) == false)
                    ////{
                    oDay = oMonth = oYear = oDateFormat = string.Empty;
                    oDay = DateTime.Now.ToString("dd");
                    oMonth = DateTime.Now.ToString("MM");
                    oYear = DateTime.Now.ToString("yy");
                    oDateFormat = oDay.Trim() + oMonth.Trim() + oYear.Trim();
                    string objRanNo = objclsPrint.RandomString(2);

                    ////VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "RunProcess", "F");
                    sQRRunningSerial = objclsPrint.GetQRCodeRunningSerial(objLocationCode, oDateFormat, sPrintingSection, sLocationType);
                    if (string.IsNullOrEmpty(sQRRunningSerial))
                    {
                        if (objLocationCode == "2020")
                        {
                            sQRRunningSerial = "0";
                        }
                        else
                        {
                            if (Properties.Settings.Default.PrintingSection.ToUpper().Contains("DOOR"))
                                sQRRunningSerial = "40000";
                            else if (Properties.Settings.Default.PrintingSection.ToUpper().Contains("DECOR"))
                                sQRRunningSerial = "50000";
                            else if (Properties.Settings.Default.PrintingSection.ToUpper().Contains("PLY"))
                                sQRRunningSerial = "0";
                        }
                    }
                    int objNextNo = Convert.ToInt32(sQRRunningSerial);
                    objNextNo = objNextNo + 1;
                    sQRRunningSerial = Convert.ToString(objNextNo);
                    if (sQRRunningSerial.Length == 4)
                        sQRRunningSerial = "0" + sQRRunningSerial;
                    if (sQRRunningSerial.Length == 3)
                        sQRRunningSerial = "00" + sQRRunningSerial;
                    if (sQRRunningSerial.Length == 2)
                        sQRRunningSerial = "000" + sQRRunningSerial;
                    if (sQRRunningSerial.Length == 1)
                        sQRRunningSerial = "0000" + sQRRunningSerial;  //

                    objQRCode = objLocationCode.Trim() + oDateFormat.Trim() + objRanNo.Trim() + sQRRunningSerial;
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ProductionProcess", "objQRCode = " + objQRCode + " sent to print");
                    string PrintStatus = objclsPrint.PrintQRCodeItem(objLocationCode, objMatcode, objMatDesc, objMatGrade, objMatGroup, objMatGroupDesc, objMatThickness, objMatThicknessDesc, objMatSize, objMatDesignNo, objMatFinishCode, objMatBatchNo, objQRCode, objMatStatus, oDateFormat, sPrintingSection, sLocationType, sMatPrintCount);
                    //isQRPrinted = true;
                    if (PrintStatus == "SUCCESS")
                    {
                        isQRPrinted = true;
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Runprocess", "PrintStatus = " + PrintStatus + "");
                        ////VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ProductionProcess", "Sent to Tower light - " + Properties.Settings.Default.MoxaRedColor.Trim().ToString());
                        obj_sendmoxa.SendMoxaIO(moxaIp, Properties.Settings.Default.MoxaGreenColor.Trim().ToString());
                        ////VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ProductionProcess", "Receive from Tower light");
                    }
                    else
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Runprocess", "PrintStatus = " + PrintStatus + "");
                        obj_sendmoxa.SendMoxaIO(moxaIp, Properties.Settings.Default.MoxaRedColor.Trim().ToString());
                        MessageBox.Show("ERROR : " + PrintStatus.ToString().Trim());
                        if (cmdStart.Enabled == false)
                        {
                            lblStatus.BackColor = Color.Red;
                            lblStatus.ForeColor = Color.Black;
                            cmdStop.Enabled = false;
                            cmdStart.Enabled = true;
                            lblStatus.Text = "Communication Server Stopped";
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "cmdStop_Click", lblStatus.Text + " because of no material selected to print on conveyor");
                            if (cmdStop.Enabled == false)
                            {
                                isRunning = false;
                                myThread = new Thread(SchedularThreadDisconnectMode);
                                myThread.Start();
                            }
                        }
                        return;
                        //SetText(PrintStatus.Trim().ToString());
                        ////VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ProductionProcess", "Sent to Tower light - " + Properties.Settings.Default.MoxaRedColor.Trim().ToString());
                        ////VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ProductionProcess", "Receive from Tower light");
                    }
                    ////}///
                }
                else if (obj_itmstus == "OFF")
                {
                    ////VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "cmdStop_Click", "G");
                    ////VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Runprocess", "Moxa Status = " + obj_itmstus + " & isQRPrinted " + isQRPrinted);
                    isQRPrinted = false;
                }

                #endregion


                #region Code not in use as of now

                #region When stack lable is printing on conveyor
                //else if (Convert.ToBoolean(StackStatus) == true)
                //{
                //    sMatLotSize = objclsPrint.GetSelectedMatLotSize(objMatcode);
                //    sMatLotPrintedQty = objclsPrint.GetSelectedMatPrintedLotQty(objMatcode);

                //    #region If LotSize not zero
                //    if (sMatLotSize > 0)
                //    {
                //        #region Is MatCode printed
                //        if (objPrintedMatcode.Trim() != string.Empty || objPrintedMatcode.Trim() != "")
                //        {
                //            if (objMatcode.Trim() != objPrintedMatcode.Trim())
                //            {
                //                sStackRunningSerial = objclsPrint.GetStackRunningSerial(objLocationCode, oDateFormat, sPrintingSection, sLocationType);
                //                if (sStackRunningSerial == string.Empty || sStackRunningSerial == "")
                //                    sStackRunningSerial = "0";
                //                int objNextStackNo = Convert.ToInt32(sStackRunningSerial);
                //                objNextStackNo = objNextStackNo + 1;
                //                sStackRunningSerial = Convert.ToString(objNextStackNo);
                //                if (sStackRunningSerial.Length == 4)
                //                    sStackRunningSerial = "0" + sStackRunningSerial.Trim();
                //                if (sStackRunningSerial.Length == 3)
                //                    sStackRunningSerial = "00" + sStackRunningSerial.Trim();
                //                if (sStackRunningSerial.Length == 2)
                //                    sStackRunningSerial = "000" + sStackRunningSerial.Trim();
                //                if (sStackRunningSerial.Length == 1)
                //                    sStackRunningSerial = "0000" + sStackRunningSerial.Trim();

                //                objStackQRCode = objLocationCode.Trim() + oDateFormat.Trim() + sStackRunningSerial;

                //                string PrintStatus = objclsPrint.PrintStackQRCodeItem(objLocationCode, objPrintedMatcode, objStackQRCode, oDateFormat, sPrintingSection, sLocationType, objPrintedMatGradeDesc.Trim(), objPrintedMatGroupDesc.Trim(), objPrintedMatThickness.Trim(), objPrintedMatSize.Trim(), sMatPrintCount.ToString());
                //                if (PrintStatus == "SUCCESSFULL")
                //                {
                //                    sMatPrintCount = 0;
                //                    oSAPPostCount = 0;
                //                    sMatLotPrintedQty = 0;
                //                    objPrintedMatcode = string.Empty;
                //                    objPrintedMatGradeDesc = string.Empty;
                //                    objPrintedMatGroupDesc = string.Empty;
                //                    objPrintedMatThickness = string.Empty;
                //                    objPrintedMatSize = string.Empty;
                //                    //lblPrintCount.Text = "***";
                //                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtMessage, "ProductionData", dtProdData.Rows.Count + " No of Records Posted In SAP Out of " + dtProdData.Rows.Count + " For Material Code - " + objMatcode + " At " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                //                }
                //            }
                //            if (objMatcode.Trim() == objPrintedMatcode.Trim())
                //            {
                //                if (sMatLotSize > sMatLotPrintedQty)
                //                {
                //                    sQRRunningSerial = objclsPrint.GetQRCodeRunningSerial(objLocationCode, oDateFormat, sPrintingSection, sLocationType);
                //                    if (sQRRunningSerial == string.Empty || sQRRunningSerial == "")
                //                        sQRRunningSerial = "0";
                //                    int objNextNo = Convert.ToInt32(sQRRunningSerial);
                //                    objNextNo = objNextNo + 1;
                //                    sQRRunningSerial = Convert.ToString(objNextNo);
                //                    if (sQRRunningSerial.Length == 4)
                //                        sQRRunningSerial = "0" + sQRRunningSerial;
                //                    if (sQRRunningSerial.Length == 3)
                //                        sQRRunningSerial = "00" + sQRRunningSerial;
                //                    if (sQRRunningSerial.Length == 2)
                //                        sQRRunningSerial = "000" + sQRRunningSerial;
                //                    if (sQRRunningSerial.Length == 1)
                //                        sQRRunningSerial = "0000" + sQRRunningSerial;

                //                    objQRCode = objLocationCode.Trim() + oDateFormat.Trim() + objRanNo.Trim() + sQRRunningSerial;

                //                    sMatPrintCount++;
                //                    string PrintStatus = objclsPrint.PrintQRCodeItem(objLocationCode, objMatcode, objMatDesc, objMatGrade, objMatGroup, objMatGroupDesc, objMatThickness, objMatThicknessDesc, objMatSize, objQRCode, objMatStatus, oDateFormat, sPrintingSection, sLocationType, sMatPrintCount);
                //                    isPrinted = true;
                //                    if (PrintStatus == "SUCCESS")
                //                    {
                //                        objPrintedMatcode = objMatcode.Trim();
                //                        objPrintedMatGradeDesc = objMatGradeDesc.Trim();
                //                        objPrintedMatGroupDesc = objMatGroupDesc.Trim();
                //                        objPrintedMatThickness = objMatThicknessDesc.Trim();
                //                        objPrintedMatSize = objMatSize.Trim();
                //                        obj_sendmoxa.SendMoxaIO(moxaIp, Properties.Settings.Default.MoxaGreenColor.Trim().ToString());
                //                    }
                //                    else
                //                        obj_sendmoxa.SendMoxaIO(moxaIp, Properties.Settings.Default.MoxaRedColor.Trim().ToString());
                //                }
                //                else if (sMatLotSize == sMatLotPrintedQty)
                //                {
                //                    sStackRunningSerial = objclsPrint.GetStackRunningSerial(objLocationCode, oDateFormat, sPrintingSection, sLocationType);
                //                    if (sStackRunningSerial == string.Empty || sStackRunningSerial == "")
                //                        sStackRunningSerial = "0";
                //                    int objNextStackNo = Convert.ToInt32(sStackRunningSerial);
                //                    objNextStackNo = objNextStackNo + 1;
                //                    sStackRunningSerial = Convert.ToString(objNextStackNo);
                //                    if (sStackRunningSerial.Length == 4)
                //                        sStackRunningSerial = "0" + sStackRunningSerial.Trim();
                //                    if (sStackRunningSerial.Length == 3)
                //                        sStackRunningSerial = "00" + sStackRunningSerial.Trim();
                //                    if (sStackRunningSerial.Length == 2)
                //                        sStackRunningSerial = "000" + sStackRunningSerial.Trim();
                //                    if (sStackRunningSerial.Length == 1)
                //                        sStackRunningSerial = "0000" + sStackRunningSerial.Trim();

                //                    objStackQRCode = objLocationCode.Trim() + oDateFormat.Trim() + sStackRunningSerial;

                //                    string PrintStatus = objclsPrint.PrintStackQRCodeItem(objLocationCode, objMatcode, objStackQRCode, oDateFormat, sPrintingSection, sLocationType, objMatGradeDesc.Trim(), objMatGroupDesc.Trim(), objMatThicknessDesc.Trim(), objMatSize.Trim(), sMatLotSize.ToString());
                //                    if (PrintStatus == "SUCCESSFULL")
                //                    {
                //                        sMatPrintCount = 0;
                //                        oSAPPostCount = 0;
                //                        sMatLotPrintedQty = 0;
                //                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtMessage, "ProductionData", dtProdData.Rows.Count + " No of Records Posted In SAP Out of " + dtProdData.Rows.Count + " For Material Code - " + objMatcode + " At " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                //                    }
                //                }
                //            }
                //        }
                //        else if (sMatLotSize == sMatLotPrintedQty)
                //        {
                //            sStackRunningSerial = objclsPrint.GetStackRunningSerial(objLocationCode, oDateFormat, sPrintingSection, sLocationType);
                //            if (sStackRunningSerial == string.Empty || sStackRunningSerial == "")
                //                sStackRunningSerial = "0";
                //            int objNextStackNo = Convert.ToInt32(sStackRunningSerial);
                //            objNextStackNo = objNextStackNo + 1;
                //            sStackRunningSerial = Convert.ToString(objNextStackNo);
                //            if (sStackRunningSerial.Length == 4)
                //                sStackRunningSerial = "0" + sStackRunningSerial.Trim();
                //            if (sStackRunningSerial.Length == 3)
                //                sStackRunningSerial = "00" + sStackRunningSerial.Trim();
                //            if (sStackRunningSerial.Length == 2)
                //                sStackRunningSerial = "000" + sStackRunningSerial.Trim();
                //            if (sStackRunningSerial.Length == 1)
                //                sStackRunningSerial = "0000" + sStackRunningSerial.Trim();

                //            objStackQRCode = objLocationCode.Trim() + oDateFormat.Trim() + sStackRunningSerial;

                //            string PrintStatus = objclsPrint.PrintStackQRCodeItem(objLocationCode, objMatcode, objStackQRCode, oDateFormat, sPrintingSection, sLocationType, objMatGradeDesc.Trim(), objMatGroupDesc.Trim(), objMatThicknessDesc.Trim(), objMatSize.Trim(), sMatLotSize.ToString());
                //            if (PrintStatus == "SUCCESSFULL")
                //            {
                //                sMatPrintCount = 0;
                //                oSAPPostCount = 0;
                //                sMatLotPrintedQty = 0;
                //                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtMessage, "ProductionData", dtProdData.Rows.Count + " No of Records Posted In SAP Out of " + dtProdData.Rows.Count + " For Material Code - " + objMatcode + " At " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                //            }
                //        }
                //        #endregion

                //        #region If Matcode is printing first time
                //        if (objPrintedMatcode.Trim() == string.Empty || objPrintedMatcode.Trim() == "")
                //        {
                //            sQRRunningSerial = objclsPrint.GetQRCodeRunningSerial(objLocationCode, oDateFormat, sPrintingSection, sLocationType);
                //            if (sQRRunningSerial == string.Empty || sQRRunningSerial == "")
                //                sQRRunningSerial = "0";
                //            int objNextNo = Convert.ToInt32(sQRRunningSerial);
                //            objNextNo = objNextNo + 1;
                //            sQRRunningSerial = Convert.ToString(objNextNo);
                //            if (sQRRunningSerial.Length == 4)
                //                sQRRunningSerial = "0" + sQRRunningSerial;
                //            if (sQRRunningSerial.Length == 3)
                //                sQRRunningSerial = "00" + sQRRunningSerial;
                //            if (sQRRunningSerial.Length == 2)
                //                sQRRunningSerial = "000" + sQRRunningSerial;
                //            if (sQRRunningSerial.Length == 1)
                //                sQRRunningSerial = "0000" + sQRRunningSerial;

                //            objQRCode = objLocationCode.Trim() + oDateFormat.Trim() + objRanNo.Trim() + sQRRunningSerial;

                //            sMatPrintCount++;
                //            string PrintStatus = objclsPrint.PrintQRCodeItem(objLocationCode, objMatcode, objMatDesc, objMatGrade, objMatGroup, objMatGroupDesc, objMatThickness, objMatThicknessDesc, objMatSize, objQRCode, objMatStatus, oDateFormat, sPrintingSection, sLocationType, sMatPrintCount);
                //            isPrinted = true;
                //            if (PrintStatus == "SUCCESS")
                //            {
                //                objPrintedMatcode = objMatcode.Trim();
                //                objPrintedMatGradeDesc = objMatGradeDesc.Trim();
                //                objPrintedMatGroupDesc = objMatGroupDesc.Trim();
                //                objPrintedMatThickness = objMatThicknessDesc.Trim();
                //                objPrintedMatSize = objMatSize.Trim();
                //                obj_sendmoxa.SendMoxaIO(moxaIp, Properties.Settings.Default.MoxaGreenColor.Trim().ToString());
                //            }
                //            else
                //                obj_sendmoxa.SendMoxaIO(moxaIp, Properties.Settings.Default.MoxaRedColor.Trim().ToString());
                //        }
                //        #endregion

                //        //else if (sMatLotSize == sMatLotPrintedQty)
                //        //{
                //        //    sStackRunningSerial = objclsPrint.GetStackRunningSerial(objLocationCode, oDateFormat, sPrintingSection, sLocationType);
                //        //    if (sStackRunningSerial == string.Empty || sStackRunningSerial == "")
                //        //        sStackRunningSerial = "0";
                //        //    int objNextStackNo = Convert.ToInt32(sStackRunningSerial);
                //        //    objNextStackNo = objNextStackNo + 1;
                //        //    sStackRunningSerial = Convert.ToString(objNextStackNo);
                //        //    if (sStackRunningSerial.Length == 4)
                //        //        sStackRunningSerial = "0" + sStackRunningSerial.Trim();
                //        //    if (sStackRunningSerial.Length == 3)
                //        //        sStackRunningSerial = "00" + sStackRunningSerial.Trim();
                //        //    if (sStackRunningSerial.Length == 2)
                //        //        sStackRunningSerial = "000" + sStackRunningSerial.Trim();
                //        //    if (sStackRunningSerial.Length == 1)
                //        //        sStackRunningSerial = "0000" + sStackRunningSerial.Trim();

                //        //    objStackQRCode = objLocationCode.Trim() + oDateFormat.Trim() + sStackRunningSerial;

                //        //    string PrintStatus = objclsPrint.PrintStackQRCodeItem(objLocationCode, objMatcode, objStackQRCode, oDateFormat, sPrintingSection, sLocationType, objMatGradeDesc.Trim(), objMatGroupDesc.Trim(), objMatThicknessDesc.Trim(), objMatSize.Trim(), sMatLotSize.ToString());
                //        //    if (PrintStatus == "SUCCESSFULL")
                //        //    {
                //        //        sMatPrintCount = 0;
                //        //        oSAPPostCount = 0;
                //        //        sMatLotPrintedQty = 0;
                //        //        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtMessage, "ProductionData", dtProdData.Rows.Count + " No of Records Posted In SAP Out of " + dtProdData.Rows.Count + " For Material Code - " + objMatcode + " At " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                //        //    }
                //        //}
                //    }

                //    #endregion

                //    #region If Lotsize is Zero
                //    else if (sMatLotSize == 0)
                //    {
                //        sQRRunningSerial = objclsPrint.GetQRCodeRunningSerial(objLocationCode, oDateFormat, sPrintingSection, sLocationType);
                //        if (sQRRunningSerial == string.Empty || sQRRunningSerial == "")
                //            sQRRunningSerial = "0";
                //        int objNextNo = Convert.ToInt32(sQRRunningSerial);
                //        objNextNo = objNextNo + 1;
                //        sQRRunningSerial = Convert.ToString(objNextNo);
                //        if (sQRRunningSerial.Length == 4)
                //            sQRRunningSerial = "0" + sQRRunningSerial;
                //        if (sQRRunningSerial.Length == 3)
                //            sQRRunningSerial = "00" + sQRRunningSerial;
                //        if (sQRRunningSerial.Length == 2)
                //            sQRRunningSerial = "000" + sQRRunningSerial;
                //        if (sQRRunningSerial.Length == 1)
                //            sQRRunningSerial = "0000" + sQRRunningSerial;

                //        objQRCode = objLocationCode.Trim() + oDateFormat.Trim() + objRanNo.Trim() + sQRRunningSerial;

                //        sMatPrintCount++;
                //        string PrintStatus = objclsPrint.PrintQRCodeItem(objLocationCode, objMatcode, objMatDesc, objMatGrade, objMatGroup, objMatGroupDesc, objMatThickness, objMatThicknessDesc, objMatSize, objQRCode, objMatStatus, oDateFormat, sPrintingSection, sLocationType, sMatPrintCount);
                //        isPrinted = true;
                //        if (PrintStatus == "SUCCESS")
                //            obj_sendmoxa.SendMoxaIO(moxaIp, Properties.Settings.Default.MoxaGreenColor.Trim().ToString());
                //        else
                //            obj_sendmoxa.SendMoxaIO(moxaIp, Properties.Settings.Default.MoxaRedColor.Trim().ToString());
                //    }
                //    #endregion

                //}
                #endregion

                #endregion

                #endregion

            }
            catch (ThreadAbortException ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "RunProcess", ex.ExceptionState.ToString());
                //MessageBox.Show(ex.ExceptionState.ToString());
            }
        }

        //private void SetText(string text)
        //{
        //    if (this.txtbxMessage.InvokeRequired)
        //    {
        //        SetTextCallback d = new SetTextCallback(SetText);
        //        this.Invoke(d, new object[] { text });
        //    }
        //    else
        //    {
        //        if (!text.Contains("0"))
        //            this.txtbxMessage.Text = "Error : " + text.ToString().Trim();
        //        else
        //            this.txtbxMessage.Text = "Error : Markem Imaje printer didn't return proper responce. See the log for more details.";
        //        this.txtbxMessage.BackColor = Color.Red;
        //        this.txtbxMessage.ForeColor = Color.Black;

        //        lblDBStatus.BackColor = Color.Red;
        //        lblDBStatus.ForeColor = Color.Black;
        //        cmdStop.Enabled = false;
        //        cmdStart.Enabled = true;
        //        lblDBStatus.Text = "Communication Server Stopped";
        //        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "cmdStop_Click", lblDBStatus.Text + " because of Markem-Imaje Printer Issue. See the log for more details.");
        //        if (cmdStop.Enabled == false)
        //        {
        //            isRunning = false;
        //            myThread = new Thread(SchedularThreadDisconnectMode);
        //            myThread.Start();
        //        }
        //        return;
        //    }
        //}



        public void FtpUpload()
        {
            objclsPrint = new B_ClsPrint();
            int aHour = Properties.Settings.Default.ScheduledTimeHR;
            int aMinute = Properties.Settings.Default.ScheduledTimeMM;
            if (DateTime.Now.Hour == aHour && DateTime.Now.Minute == aMinute)
            {
                string path = "";  //Properties.Settings.Default.LocalFolderPath;
                string folderPath = path;
                string dbname = ""; //lblDatabase1.Text.Trim();
                string filename = string.Empty;
                if (dbname.Contains("QRCODE-RKT-PLY"))
                {
                    filename = "2010_PL_" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                }
                if (dbname.Contains("QRCODE-RKT-DOOR"))
                {
                    filename = "2010_DC_" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                }
                if (dbname.Contains("QRCODE-RKT-DECO"))
                {
                    filename = "2010_DR0_" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                }
                if (dbname.Contains("QRCODE-TIZIT-PLY"))
                {
                    filename = "2020_PL_" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                }
                if (dbname.Contains("QRCODE-KRP-PLY"))
                {
                    filename = "2000_PL_" + DateTime.Now.ToString("yyyyMMdd") + ".csv";     //HHmm
                }
                string file_browse_path = folderPath + filename;
                string path11 = @"" + file_browse_path;
                if (!File.Exists(path11))
                {
                    string fromdate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                    string todate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                    objclsPrint.GetFTPReportDetails(objLocationCode, fromdate, todate);
                    if (dtFTPData.Rows.Count == 0)
                        return;
                    else if (dtFTPData.Rows.Count > 0)
                    {
                        dtFTPData.DefaultView.ToTable(true, "ItemCode", "ItemDescription", "PlantCode", "Quantity", "SerialNo", "Createdon");
                        dtFTPData.Columns.Remove("CreatedBy");
                        dtFTPData.Columns.Remove("TotalQty");
                        //dt.Columns.Remove("Barcode");
                        dtFTPData.Columns["PlantCode"].SetOrdinal(0);
                        dtFTPData.Columns["ItemCode"].SetOrdinal(1);
                        dtFTPData.Columns["ItemDescription"].SetOrdinal(2);
                        dtFTPData.Columns["Quantity"].SetOrdinal(3);
                        dtFTPData.Columns["SerialNo"].SetOrdinal(4);
                        dtFTPData.Columns["Createdon"].SetOrdinal(5);
                        dtFTPData.Columns["ItemCode"].ColumnName = "Material Code";
                        dtFTPData.Columns["ItemDescription"].ColumnName = "Material Description";
                        dtFTPData.Columns["SerialNo"].ColumnName = "QR Code Serial Number";
                        dtFTPData.Columns["Createdon"].ColumnName = "Posting Date";
                        objclsPrint.ExportToCSVFromDataTable(dtFTPData, "PostToFTP", "CSV");
                    }
                }
                else
                    return;
            }
        }

        void _RemoteClient_OnClientClose(ClientHandler _RemoteClient)
        {
            try
            {
                ListViewItem _lItem;
                if (lstData.InvokeRequired)
                {
                    _dlgCloseClient _dNew = new _dlgCloseClient(_CloseClient);
                    this.Invoke(_dNew, new object[] { _RemoteClient.ClientIP, DateTime.Now.ToString(), _RemoteClient.WelcomeMsg });
                    //lv.Invoke(new _dlgUpdateClient(_UpdateClientCount));
                }
                else
                {
                    //lv.Items.Clear();
                    _RunningIP.Remove(_RemoteClient.ClientIP);
                    foreach (DictionaryEntry _de in _RunningIP)
                    {
                        _lItem = new ListViewItem();
                        _lItem.Text = _de.Value.ToString().Trim();
                        _lItem.SubItems.Add(DateTime.Now.ToString());
                        _lItem.SubItems.Add("Close");
                        lstData.Items.Add(_lItem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "_RemoteClient_OnClientClose", ex.Message);
            }
        }

        void _CloseClient(string IP, string LogTime, string Message)
        {
            try
            {
                ListViewItem _lItem;
                if (lstData.InvokeRequired)
                {
                    _dlgCloseClient _dNew = new _dlgCloseClient(_CloseClient);
                    this.Invoke(_dNew, new object[] { IP, LogTime, Message });
                }
                else
                {
                    //lv.Items.Clear();
                    _RunningIP.Remove(IP);
                    foreach (DictionaryEntry _de in _RunningIP)
                    {
                        _lItem = new ListViewItem();
                        _lItem.Text = _de.Value.ToString().Trim();
                        _lItem.SubItems.Add(LogTime);
                        _lItem.SubItems.Add(Message);
                        lstData.Items.Add(_lItem);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "_CloseClient", ex.Message);
            }
        }

        void _BCILSocketServer_OnClientConnect(ClientHandler _RemoteClient)
        {
            try
            {
                _RemoteClient.OnDataArrival += new ClientHandler.DataArrivalHandler(_RemoteClient_OnDataArrival);
                _RemoteClient.BusinessProcess = _ServerProcess;
                if (_RunningIP.ContainsKey(_RemoteClient.ClientIP) == false)
                    _RunningIP.Add(_RemoteClient.ClientIP, _RemoteClient.ClientIP);
                _RemoteClient.OnClientClose += new ClientHandler.ClientCloseHandler(_RemoteClient_OnClientClose);
                _UpdateClientCount(_RemoteClient.ClientIP, DateTime.Now.ToString(), _RemoteClient.WelcomeMsg);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "_BCILSocketServer_OnClientConnect", _RemoteClient.ClientIP + ": " + ex.Message);
            }
        }

        void _UpdateClientCount(string IP, string LogTime, string Message)
        {
            try
            {
                ListViewItem _lItem;
                if (lstData.InvokeRequired)
                {
                    _dlgUpdateClient _dNew = new _dlgUpdateClient(_UpdateClientCount);
                    this.Invoke(_dNew, new object[] { IP, Message });
                }
                else
                {
                    if (lstData.Items.Count >= 300000)
                        lstData.Items.Clear();
                    if (_RunningIP.Count == 0)
                    {
                        _lItem = new ListViewItem();
                        _lItem.Text = IP;
                        _lItem.SubItems.Add(LogTime);
                        _lItem.SubItems.Add(Message);
                        lstData.Items.Add(_lItem);
                    }
                    else
                    {
                        foreach (DictionaryEntry _de in _RunningIP)
                        {
                            if (Convert.ToString(_de.Key) == IP)
                            {
                                _lItem = new ListViewItem();
                                _lItem.Text = _de.Value.ToString().Trim();
                                _lItem.SubItems.Add(LogTime);
                                _lItem.SubItems.Add(Message);
                                lstData.Items.Add(_lItem);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "_UpdateClientCount", ex.Message.ToString());
            }
        }



        void _RemoteClient_OnDataArrival(ClientHandler RemoteClient)
        {
            _UpdateClientCount(RemoteClient.ClientIP, RemoteClient.Message);

            string Prefix = "";
            string[] Data = null;
            if (RemoteClient.Message.Contains("~") || RemoteClient.Message.Contains("quit"))
            {
                Data = RemoteClient.Message.Split('~');
                Prefix = Data[0].ToString();
            }
            string Response = string.Empty;
            try
            {
                _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                switch (Prefix)
                {

                    #region Login

                    case "LOGIN":
                        _BClsLogin ClsBI1 = new _BClsLogin();
                        Response = ClsBI1.CheckValidUser(Data[1], Data[2]);
                        sMUserId = string.Empty;
                        sMUserId = Data[1].ToString();
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI1 = null;
                        break;

                    case "GETANDROIDUSERRIGHTS":
                        _BClsLogin ClsBI2 = new _BClsLogin();
                        Response = ClsBI2.GetUserRights(Data[1]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI2 = null;
                        break;

                    #endregion

                    #region Stock Count

                    case "CHECKSTACKQRCODEDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StockCount ClsBI3 = new B_StockCount();
                        Response = ClsBI3.CheckStackQRCodeDetails(Data[1], Data[2]);
                        if (Response.Contains("SUCCESS"))
                        {
                            if (dtStockCount.Columns.Count > 0)
                            {
                                for (int i = 0; i < B_StockCount.dtSQRCode.Rows.Count; i++)
                                {
                                    DataRow _dtRow = dtStockCount.NewRow();
                                    _dtRow["LocationCode"] = Data[1].ToString();
                                    _dtRow["QRCode"] = B_StockCount.dtSQRCode.Rows[i][0].ToString();
                                    dtStockCount.Rows.Add(_dtRow);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < B_StockCount.dtSQRCode.Rows.Count; i++)
                                {
                                    dtStockCount.Columns.Add("LocationCode");
                                    dtStockCount.Columns.Add("QRCode");
                                    DataRow _dtRow = dtStockCount.NewRow();
                                    _dtRow["LocationCode"] = Data[1].ToString();
                                    _dtRow["QRCode"] = B_StockCount.dtSQRCode.Rows[i][0].ToString();
                                    dtStockCount.Rows.Add(_dtRow);
                                }
                            }
                        }
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI3 = null;
                        break;

                    case "CHECKQRCODEDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StockCount ClsBI4 = new B_StockCount();
                        Response = ClsBI4.CheckQRCodeDetails(Data[1], Data[2]);
                        if (Response.Contains("SUCCESS"))
                        {
                            if (dtStockCount.Columns.Count > 0)
                            {
                                DataRow _dtRow = dtStockCount.NewRow();
                                _dtRow["LocationCode"] = Data[1].ToString();
                                _dtRow["QRCode"] = Data[2].ToString();
                                dtStockCount.Rows.Add(_dtRow);
                            }
                            else
                            {
                                dtStockCount.Columns.Add("LocationCode");
                                dtStockCount.Columns.Add("QRCode");
                                DataRow _dtRow = dtStockCount.NewRow();
                                _dtRow["LocationCode"] = Data[1].ToString();
                                _dtRow["QRCode"] = Data[2].ToString();
                                dtStockCount.Rows.Add(_dtRow);
                            }
                        }
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI4 = null;
                        break;

                    case "CLEARSTOCKCOUNTTABLE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI42 = new B_StackPrinting();
                        dtStockCount.Rows.Clear();
                        Response = "CLEARSTOCKCOUNTTABLE ~ SUCCESS";
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI42 = null;
                        break;

                    case "SAVESTOCKCOUNTDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StockCount ClsBI5 = new B_StockCount();
                        if (dtStockCount.Rows.Count > 0)
                        {
                            Response = ClsBI5.SaveStockCountDetails(Data[1], Data[2], dtStockCount);
                        }
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI5 = null;
                        break;

                    #endregion

                    #region LoadingOffloading

                    case "GETDELIVERYORDERDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI7 = new B_LoadingOffloading();
                        Response = ClsBI7.GetDeliveryOrderNumbersDetails(Data[1], Data[2]);
                        DONo = string.Empty;
                        DONo = Data[2].Trim().ToString();
                        if (dtLoadOffloadData.Rows.Count > 0)
                            dtLoadOffloadData.Rows.Clear();
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI7 = null;
                        break;

                    case "GETSTACKQRCODEDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI8 = new B_LoadingOffloading();
                        if (dtLoadOffloadData.Rows.Count > 0)
                        {
                            if (dtLoadOffloadData.Rows[0][4].ToString() != Data[2].ToString())
                            {
                                Response = ClsBI8.GetStackQRCodeDetails(Data[1], Data[2]);
                                if (Response.Contains("SUCCESS"))
                                {
                                    if (B_LoadingOffloading.dtStackQRCode.Rows.Count > 0)
                                    {
                                        for (int i = 0; i < B_LoadingOffloading.dtStackQRCode.Rows.Count; i++)
                                        {
                                            if (dtLoadOffloadData.Columns.Count > 0)
                                            {
                                                DataRow _dtRow = dtLoadOffloadData.NewRow();
                                                _dtRow["LocationCode"] = Data[1].ToString();
                                                _dtRow["DONo"] = Data[3].ToString();
                                                _dtRow["MatCode"] = B_LoadingOffloading.dtStackQRCode.Rows[i][1].ToString(); //B_LoadingOffloading.sMatCode.ToString();
                                                _dtRow["QRCode"] = B_LoadingOffloading.dtStackQRCode.Rows[i][0].ToString();
                                                _dtRow["StackQRCode"] = Data[2].ToString();
                                                dtLoadOffloadData.Rows.Add(_dtRow);
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                Response = "GETSTACKQRCODEDETAILS ~ ERROR ~ STACK QRCode is Already Scanned, Kindly Scan Another";
                            }
                        }
                        else if (dtLoadOffloadData.Rows.Count == 0)
                        {
                            Response = ClsBI8.GetStackQRCodeDetails(Data[1], Data[2]);
                            if (Response.Contains("SUCCESS"))
                            {
                                if (B_LoadingOffloading.dtStackQRCode.Rows.Count > 0)
                                {
                                    for (int i = 0; i < B_LoadingOffloading.dtStackQRCode.Rows.Count; i++)
                                    {
                                        if (dtLoadOffloadData.Columns.Count > 0)
                                        {
                                            DataRow _dtRow = dtLoadOffloadData.NewRow();
                                            _dtRow["LocationCode"] = Data[1].ToString();
                                            _dtRow["DONo"] = Data[3].ToString();
                                            _dtRow["MatCode"] = B_LoadingOffloading.dtStackQRCode.Rows[i][1].ToString(); //B_LoadingOffloading.sMatCode.ToString();
                                            _dtRow["QRCode"] = B_LoadingOffloading.dtStackQRCode.Rows[i][0].ToString();
                                            _dtRow["StackQRCode"] = Data[2].ToString();
                                            dtLoadOffloadData.Rows.Add(_dtRow);
                                        }
                                    }
                                }
                            }
                        }
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI8 = null;
                        break;

                    case "GETQRCODEDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI9 = new B_LoadingOffloading();
                        Response = ClsBI9.GetQRCodeDetails(Data[1], Data[2]);
                        if (Response.Contains("SUCCESS"))
                        {
                            if (dtLoadOffloadData.Columns.Count > 0)
                            {
                                DataRow _dtRow = dtLoadOffloadData.NewRow();
                                _dtRow["LocationCode"] = Data[1].ToString();
                                _dtRow["MatCode"] = B_LoadingOffloading.sMatCode.ToString();
                                _dtRow["QRCode"] = B_LoadingOffloading.dtStackQRCode.Rows[0][0].ToString();
                                dtLoadOffloadData.Rows.Add(_dtRow);
                            }
                            else
                            {
                                dtLoadOffloadData.Columns.Add("LocationCode");
                                dtLoadOffloadData.Columns.Add("MatCode");
                                dtLoadOffloadData.Columns.Add("QRCode");
                                DataRow _dtRow = dtLoadOffloadData.NewRow();
                                _dtRow["LocationCode"] = Data[1].ToString();
                                _dtRow["MatCode"] = B_LoadingOffloading.sMatCode.ToString();
                                _dtRow["QRCode"] = B_LoadingOffloading.dtStackQRCode.Rows[0][0].ToString();
                                dtLoadOffloadData.Rows.Add(_dtRow);
                            }
                        }
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI9 = null;
                        break;

                    case "CLEARLOADOFFLOADTABLE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI35 = new B_StackPrinting();
                        if (dtLoadOffloadData.Rows.Count > 0)
                            dtLoadOffloadData.Rows.Clear();
                        Response = "CLEARLOADOFFLOADTABLE ~ SUCCESS";
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI35 = null;
                        break;

                    case "PRINTLOADOFFLOADSTACKQRCODE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI336 = new B_LoadingOffloading();
                        DataTable dtData = ConvertStringToDataTable(Data[1].ToString(), '#', '$');
                        Response = PrintLoadOffloadStackQRCode(dtData, DONo);
                        if (sSaveCount == dtData.Rows.Count && sStackPrintCount == 1)
                        {
                            FinalLoadOffloadData(dtData, sMUserId.Trim().ToString());
                            Response = "PRINTLOADOFFLOADSTACKQRCODE ~ SUCCESS ~ " + sSaveCount + " No. of Records Saved And Printed Successfully";
                        }
                        if (sSaveCount < dtData.Rows.Count && sStackPrintCount == 0)
                        {
                            FinalLoadOffloadData(dtData, sMUserId.Trim().ToString());
                            string sResponce = Response.Replace(@"ERROR", "");
                            sResponce = sResponce.Replace(@"~", "");
                            Response = "PRINTLOADOFFLOADSTACKQRCODE ~ SUCCESS ~ " + sSaveCount + " No. of Records Saved Successfully And" + Response.ToString();
                        }
                        else if (sSaveCount == 0 && sStackPrintCount == 0)
                            Response = "PRINTLOADOFFLOADSTACKQRCODE ~ ERROR ~ " + Response.ToString();
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI336 = null;
                        break;

                    case "CLEARSTACKDATAFROMLOADOFFLOADTABLE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI339 = new B_LoadingOffloading();
                        RemoveStackDataFromLoadOffloadTable(Data[1].ToString());
                        Response = "CLEARSTACKDATAFROMLOADOFFLOADTABLE ~ SUCCESS";
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI339 = null;
                        break;

                    case "SAVELOADOFFLOADDATA":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI36 = new B_LoadingOffloading();
                        //dtLoadOffData = new DataTable();
                        //dtLoadOffData = ConvertLoadOffloadStringToDataTable(Data[1].ToString(), '#', '$');
                        if (dtLoadOffloadData.Rows.Count > 0)
                        {
                            string mDeliveryNo = Data[1].Trim().ToString();
                            string mUserID = Data[2].Trim().ToString();
                            oSaveCount = 0;
                            foreach (DataRow dr in dtLoadOffloadData.Rows)
                            {
                                string mLocCode = "";
                                string mMatcode = "";
                                string mQRCode = "";
                                string mStackQRCode = "";
                                mLocCode = dr["LocationCode"].ToString().Trim();
                                //mDeliveryNo = dr["DONo"].ToString().Trim();
                                mMatcode = dr["MatCode"].ToString().Trim();
                                mQRCode = dr["QRCode"].ToString().Trim();
                                mStackQRCode = dr["StackQRCode"].ToString().Trim();
                                string lSaveStatus = "";
                                lSaveStatus = ClsBI36.SaveLoadOffloadQRCodeData(mLocCode, mDeliveryNo, mMatcode, mQRCode, mStackQRCode, sMUserId.Trim().ToString());
                                if (lSaveStatus.Contains("SUCCESS"))
                                    oSaveCount++;
                            }
                        }
                        if (oSaveCount > 0)
                        {
                            if (dtLoadOffloadData.Rows.Count > 0)
                                dtLoadOffloadData.Rows.Clear();
                            Response = "SAVELOADOFFLOADDATA ~ SUCCESS ~ " + oSaveCount + " - No of Records Saved Successfully"; //Out Of " + dtLoadOffData.Rows.Count
                        }
                        if (oSaveCount == 0)
                            Response = "SAVELOADOFFLOADDATA ~ ERROR ~ " + oSaveCount + " - No Record Saved, Kindly try Again"; // Out Of " + dtLoadOffData.Rows.Count;
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI36 = null;
                        break;

                    case "UPDATELOADOFFLOADSAPSTATUS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI28 = new B_LoadingOffloading();
                        if (dtLoadOffloadData.Rows.Count > 0)
                            dtLoadOffloadData.Rows.Clear();
                        //dsData = new DataSet();
                        //dsData = ClsBI28.fnPIGetLoadOffloadDataSAPPost(dtLoadOffData);
                        //string PostStatus = string.Empty;
                        //if (dsData.Tables[0].Rows.Count > 0)
                        //{
                        //foreach (DataRow dr in dsData.Tables[0].Rows)
                        //{
                        //    string oLocCode = dr["LocationCode"].ToString().Trim();
                        //    string oDelNo = dr["DONo"].ToString().Trim();
                        //    string oMatcode = dr["MatCode"].ToString().Trim();
                        //    string oQRCode = dr["QRCode"].ToString().Trim();
                        //    string oUserId = Data[1]; // dr["UserId"].ToString().Trim();
                        //    string oSAPPostMsg = dr["SAPStatus"].ToString().Trim();
                        //    PostStatus = ClsBI28.UpdateLoadOffloadQRCodeSAPStatus(oLocCode, oDelNo, oMatcode, oQRCode, oSAPPostMsg, oUserId);
                        //    if (PostStatus.Contains("SUCCESS"))
                        //        oSAPPostCount++;
                        //}
                        //}
                        //if (oSAPPostCount == dtLoadOffData.Rows.Count)
                        //    Response = "UPDATELOADOFFLOADSAPSTATUS ~ SUCCESS ~ " + oSAPPostCount + " - No of Records Posted To SAP Successfully Out Of " + dtLoadOffData.Rows.Count;
                        //if (oSAPPostCount < dtLoadOffData.Rows.Count)
                        //    Response = "UPDATELOADOFFLOADSAPSTATUS ~ SUCCESS ~ " + oSAPPostCount + " - No of Records Posted To SAP Successfully Out Of " + dtLoadOffData.Rows.Count;
                        //if (oSAPPostCount == 0)
                        //    Response = "UPDATELOADOFFLOADSAPSTATUS ~ ERROR ~ " + PostStatus;
                        Response = "UPDATELOADOFFLOADSAPSTATUS ~ SUCCESS ~ " + oSaveCount + " - No of Records Posted To SAP Successfully"; // Out Of " + dtLoadOffData.Rows.Count;
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI28 = null;
                        break;

                    #endregion

                    #region Offload Against Delivery

                    case "GETDELIVERYORDERDETAILSAGAINSTOFFLOAD":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI40 = new B_LoadingOffloading();
                        Response = ClsBI40.GetDODetailsAgainstDelivery(Data[1], Data[2]);
                        DONo = string.Empty;
                        DONo = Data[2].ToString();
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI40 = null;
                        break;

                    case "SAVEUPDATEDAMAGEDLOADOFFLOADDATA":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI55 = new B_LoadingOffloading();
                        dtDLoadOffData = new DataTable();
                        int oDSaveCount = 0;
                        int dStatus = 0;
                        dtDLoadOffData = ConvertDamagedLoadOffloadStringToDataTable(Data[1].ToString(), '#', '$');
                        if (dtDLoadOffData.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtDLoadOffData.Rows)
                            {
                                string sLocCode = dr["LocationCode"].ToString().Trim();
                                string sDelNo = dr["DONo"].ToString().Trim();
                                string sMatcode = dr["MatCode"].ToString().Trim();
                                string sQRCode = dr["QRCode"].ToString().Trim();
                                string sStatus = dr["Status"].ToString().Trim();
                                string sUserId = dr["UserId"].ToString().Trim();
                                if (sStatus == "D")
                                {
                                    dStatus++;
                                    string UpdateStatus = ClsBI55.SaveUpdateDamagedQRCode(sLocCode, sDelNo, sMatcode, sQRCode, sUserId);
                                    if (UpdateStatus.Contains("SUCCESS"))
                                        oDSaveCount++;
                                }
                            }
                        }
                        if (oDSaveCount == 0)
                            Response = "SAVEUPDATEDAMAGEDLOADOFFLOADDATA ~ ERROR ~ No QRCode is Updated As Damaged, Kindly Try Again";
                        if (oDSaveCount > 0 && oDSaveCount <= dtDLoadOffData.Rows.Count)
                        {
                            //DataSet odsData = new DataSet();
                            //odsData = ClsBI55.fnPIGetLoadOffloadDataSAPPost(dtDLoadOffData);
                            //if (odsData.Tables[0].Rows.Count > 0)
                            //    Response = "SAVEUPDATEDAMAGEDLOADOFFLOADDATA ~ SUCCESS ~ " + oDSaveCount + " - No of Records Saved and Posted To SAP Successfully Out Of " + dtDLoadOffData.Rows.Count;
                            //if (odsData.Tables[0].Rows.Count == 0)
                            Response = "SAVEUPDATEDAMAGEDLOADOFFLOADDATA ~ SUCCESS ~ " + oDSaveCount + " - No of Records Saved as Damaged and Posted To SAP Successfully";
                        }
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI36 = null;
                        break;

                    #endregion

                    #region Depot Dispatch

                    case "GETDEPOTDELIVERYORDERDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsB77 = new B_LoadingOffloading();
                        Response = ClsB77.GetDeliveryOrderNumbersDetails(Data[1], Data[2]);
                        DONo = string.Empty;
                        DONo = Data[2].Trim().ToString();
                        if (dtDepotData.Rows.Count > 0)
                            dtDepotData.Rows.Clear();
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsB77 = null;
                        break;

                    case "GETDEPOTQRCODEDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsB99 = new B_LoadingOffloading();
                        Response = ClsB99.GetQRCodeDetails(Data[1], Data[2]);
                        if (Response.Contains("SUCCESS"))
                        {
                            if (dtDepotData.Columns.Count > 0)
                            {
                                DataRow _dtRow = dtDepotData.NewRow();
                                _dtRow["LocationCode"] = Data[1].ToString();
                                _dtRow["MatCode"] = B_LoadingOffloading.sMatCode.ToString();
                                _dtRow["QRCode"] = B_LoadingOffloading.dtStackQRCode.Rows[0][0].ToString();
                                dtDepotData.Rows.Add(_dtRow);
                            }
                            else
                            {
                                dtDepotData.Columns.Add("LocationCode");
                                dtDepotData.Columns.Add("MatCode");
                                dtDepotData.Columns.Add("QRCode");
                                DataRow _dtRow = dtDepotData.NewRow();
                                _dtRow["LocationCode"] = Data[1].ToString();
                                _dtRow["MatCode"] = B_LoadingOffloading.sMatCode.ToString();
                                _dtRow["QRCode"] = B_LoadingOffloading.dtStackQRCode.Rows[0][0].ToString();
                                dtDepotData.Rows.Add(_dtRow);
                            }
                        }
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsB99 = null;
                        break;

                    case "CLEARDEPOTDATATABLE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI95 = new B_StackPrinting();
                        if (dtDepotData.Rows.Count > 0)
                            dtDepotData.Rows.Clear();
                        Response = "CLEARLOADOFFLOADTABLE ~ SUCCESS";
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI95 = null;
                        break;

                    case "SAVEDEPOTDISPATCHDATA":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI69 = new B_LoadingOffloading();
                        //dtLoadOffData = new DataTable();
                        //dtLoadOffData = ConvertLoadOffloadStringToDataTable(Data[1].ToString(), '#', '$');
                        if (dtDepotData.Rows.Count > 0)
                        {
                            oSaveCount = 0;
                            foreach (DataRow dr in dtDepotData.Rows)
                            {
                                string mLocCode = "";
                                string mDeliveryNo = "";
                                string mMatcode = "";
                                string mQRCode = "";
                                string mStackQRCode = "";

                                mLocCode = dr["LocationCode"].ToString().Trim();
                                mDeliveryNo = dr["DONo"].ToString().Trim();
                                mMatcode = dr["MatCode"].ToString().Trim();
                                mQRCode = dr["QRCode"].ToString().Trim();
                                mStackQRCode = dr["StackQRCode"].ToString().Trim();
                                string lSaveStatus = "";
                                lSaveStatus = ClsBI69.SaveLoadOffloadQRCodeData(mLocCode, mDeliveryNo, mMatcode, mQRCode, mStackQRCode, sMUserId.Trim().ToString());
                                if (lSaveStatus.Contains("SUCCESS"))
                                    oSaveCount++;
                            }
                        }
                        if (oSaveCount > 0)
                        {
                            if (dtDepotData.Rows.Count > 0)
                                dtDepotData.Rows.Clear();
                            Response = "SAVELOADOFFLOADDATA ~ SUCCESS ~ " + oSaveCount + " - No of Records Saved Successfully"; //Out Of " + dtLoadOffData.Rows.Count
                        }
                        if (oSaveCount == 0)
                            Response = "SAVELOADOFFLOADDATA ~ ERROR ~ " + oSaveCount + " - No Record Saved, Kindly try Again"; // Out Of " + dtLoadOffData.Rows.Count;
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI69 = null;
                        break;

                    #endregion

                    #region Segregation Stack Label Printing

                    case "CHECKSCANNEDQRCODEDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI13 = new B_StackPrinting();
                        string QRCode = Data[2].Substring(0, 17);
                        Response = ClsBI13.GetScannedSegregationQRCodeDetails(Data[1], QRCode, Data[3]);
                        //if (Response.Contains("SUCCESS"))
                        //{
                        //    if (dtStackData.Columns.Count > 0)
                        //    {
                        //        DataRow _dtRow = dtStackData.NewRow();
                        //        _dtRow["LocationCode"] = Data[1].ToString();
                        //        _dtRow["MatCode"] = B_StackPrinting.sMatCode.Trim();
                        //        _dtRow["QRCode"] = QRCode.Trim();
                        //        _dtRow["Status"] = Data[3].ToString();
                        //        dtStackData.Rows.Add(_dtRow);
                        //    }
                        //    else
                        //    {
                        //        dtStackData.Columns.Add("LocationCode");
                        //        dtStackData.Columns.Add("MatCode");
                        //        dtStackData.Columns.Add("QRCode");
                        //        dtStackData.Columns.Add("Status");
                        //        DataRow _dtRow = dtStackData.NewRow();
                        //        _dtRow["LocationCode"] = Data[1].ToString();
                        //        _dtRow["MatCode"] = B_StackPrinting.sMatCode.Trim();
                        //        _dtRow["QRCode"] = QRCode.Trim();
                        //        _dtRow["Status"] = Data[3].ToString();
                        //        dtStackData.Rows.Add(_dtRow);
                        //    }
                        //}
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI13 = null;
                        break;

                    case "PRINTSEGREGATIONSTACKQRCODE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI31 = new B_StackPrinting();
                        Response = PrintSegregationStackQRCode(Data[1], Data[2]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI31 = null;
                        break;

                    case "PRINTSEGREGATIONCLEARTABLE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI32 = new B_StackPrinting();
                        dtStackData.Rows.Clear();
                        Response = "PRINTSEGREGATIONCLEARTABLE ~ SUCCESS";
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI32 = null;
                        break;

                    #endregion

                    #region Segregation Qrcode Label Scanning

                    case "CHECKSEGREGATIONSCANNEDQRCODEDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI555 = new B_StackPrinting();
                        string objQRCode = Data[2].Substring(0, 17);
                        Response = ClsBI555.GetSegregationScannedQRCodeDetails(Data[1].Trim(), objQRCode.Trim().ToString(), Data[3].Trim());
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI555 = null;
                        break;

                    #endregion

                    #region Sales Return

                    case "GETSALESRETURNNUMBERDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_SalesReturn ClsBI20 = new B_SalesReturn();
                        Response = ClsBI20.GetSalesReturnNumberDetails(Data[1], Data[2]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI20 = null;
                        break;

                    case "GETSALESRETURNSTATUS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_SalesReturn ClsBI30 = new B_SalesReturn();
                        Response = ClsBI30.GetSalesReturnScannnedStatus(Data[1], Data[2], Data[3]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI30 = null;
                        break;

                    case "GETSALESRETURNQRCODEDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_SalesReturn ClsBI22 = new B_SalesReturn();
                        Response = ClsBI22.GetSalesReturnQRCodeDetails(Data[1], Data[2], Data[3], Data[4], Convert.ToInt32(1), Data[5]);
                        if (Response.Contains("SUCCESS"))
                        {
                            if (dtScannedData.Columns.Count > 0)
                            {
                                DataRow _dtRow = dtScannedData.NewRow();
                                _dtRow["LocationCode"] = Data[1].ToString();
                                _dtRow["SalesReturnNo"] = Data[2].ToString();
                                _dtRow["MatCode"] = Data[3].ToString();
                                _dtRow["QRCode"] = Data[4].ToString();
                                _dtRow["Qty"] = "1";
                                dtScannedData.Rows.Add(_dtRow);
                            }
                            else
                            {
                                dtScannedData.Columns.Add("LocationCode");
                                dtScannedData.Columns.Add("SalesReturnNo");
                                dtScannedData.Columns.Add("MatCode");
                                dtScannedData.Columns.Add("QRCode");
                                dtScannedData.Columns.Add("Qty");
                                DataRow _dtRow = dtScannedData.NewRow();
                                _dtRow["LocationCode"] = Data[1].ToString();
                                _dtRow["SalesReturnNo"] = Data[2].ToString();
                                _dtRow["MatCode"] = Data[3].ToString();
                                _dtRow["QRCode"] = Data[4].ToString();
                                _dtRow["Qty"] = "1";
                                dtScannedData.Rows.Add(_dtRow);
                            }
                        }
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI22 = null;
                        break;

                    case "CLEARSALESRETURNTABLE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI45 = new B_StackPrinting();
                        dtScannedData.Rows.Clear();
                        Response = "CLEARSALESRETURNTABLE ~ SUCCESS";
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI45 = null;
                        break;

                    case "PRINTSALESRETURNQRCODE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_SalesReturn ClsBI29 = new B_SalesReturn();
                        Response = PrintSalesReturnQRCode(Data[1], Data[2]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI29 = null;
                        break;

                    case "UPDATESALESRETURNSAPSTATUS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_SalesReturn ClsBI24 = new B_SalesReturn();
                        //dsData = new DataSet();
                        //dsData = ClsBI24.fnPIGetSalesReturnDataSAPPost(dtScannedData);
                        //if (dsData.Tables[0].Rows.Count > 0)
                        //{
                        //    foreach (DataRow dr in dsData.Tables[0].Rows)
                        //    {
                        //        string LocCode = dr["LocationCode"].ToString().Trim();
                        //        string SRNo = dr["SalesReturnNo"].ToString().Trim();
                        //        string objMatcode = dr["MatCode"].ToString().Trim();
                        //        objQRCode = dr["QRCode"].ToString().Trim();
                        //        string SAPPostMsg = dr["SAPStatus"].ToString().Trim();
                        //        string sPostStatus = ClsBI24.UpdateSalesReturnQRCodeSAPStatus(LocCode, SRNo, objMatcode, objQRCode, SAPPostMsg, Data[1]);
                        //        if (sPostStatus.Contains("SUCCESS"))
                        //            oSAPPostCount++;
                        //        if (sPostStatus.Contains("ERROR"))
                        //        { }
                        //    }
                        //}
                        //if (oSAPPostCount == dtScannedData.Rows.Count)
                        //    Response = "UPDATESALESRETURNSAPSTATUS ~ SUCCESS ~ " + oSAPPostCount + " - No of Records Posted To SAP Successfully Out Of " + dtScannedData.Rows.Count;
                        //if (oSAPPostCount < dtScannedData.Rows.Count)
                        //    Response = "UPDATESALESRETURNSAPSTATUS ~ SUCCESS ~ " + oSAPPostCount + " - No of Records Posted To SAP Successfully Out Of " + dtScannedData.Rows.Count;
                        //if (oSAPPostCount == 0)
                        //    Response = "UPDATESALESRETURNSAPSTATUS ~ ERROR ~  No Record Posted to SAP Out Of " + dtScannedData.Rows.Count;
                        Response = "UPDATESALESRETURNSAPSTATUS ~ SUCCESS ~ " + dtScannedData.Rows.Count + " - No of Records Posted Out Of " + dtScannedData.Rows.Count + " To SAP Successfully";
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI24 = null;
                        break;



                    #endregion

                    #region View Item Details

                    case "VIEWITEMDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        ViewDetails ClsBI3255 = new ViewDetails();
                        Response = ClsBI3255.ViewItemDetails(Data[1]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI3255 = null;
                        break;

                    #endregion

                    #region Get Item Details

                    case "VIEWRACKDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        ViewDetails ClsBI33 = new ViewDetails();
                        Response = ClsBI33.ViewRackDetails(Data[1]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI33 = null;
                        break;

                    #endregion

                    #region Material To Material Transfer

                    case "GETMTMMATPRODUCT":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_M2MTransfer ClsBI60 = new B_M2MTransfer();
                        Response = ClsBI60.GetM2MMatProducts();
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI60 = null;
                        break;

                    case "GETMTMMATGROUPS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_M2MTransfer ClsBI61 = new B_M2MTransfer();
                        Response = ClsBI61.GetM2MMatGroups(Data[1]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI61 = null;
                        break;

                    case "GETMTMMATGRADES":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_M2MTransfer ClsBI62 = new B_M2MTransfer();
                        Response = ClsBI62.GetM2MMatGrades(Data[1], Data[2]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI62 = null;
                        break;

                    case "GETMTMMATCATEGORIES":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_M2MTransfer ClsBI63 = new B_M2MTransfer();
                        Response = ClsBI63.GetM2MMatCategory(Data[1], Data[2], Data[3]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI63 = null;
                        break;

                    case "GETMTMMATTHICKNESS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_M2MTransfer ClsBI64 = new B_M2MTransfer();
                        Response = ClsBI64.GetM2MMatThickness(Data[1], Data[2], Data[3], Data[4]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI64 = null;
                        break;

                    case "GETMTMMATSIZE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_M2MTransfer ClsBI65 = new B_M2MTransfer();
                        Response = ClsBI65.GetM2MMatSize(Data[1], Data[2], Data[3], Data[4], Data[5]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI65 = null;
                        break;

                    case "GETMTMMATDESIGNNO":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_M2MTransfer ClsBI66 = new B_M2MTransfer();
                        Response = ClsBI66.GetM2MMatDesignNo(Data[1], Data[2], Data[3], Data[4], Data[5], Data[6]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI66 = null;
                        break;

                    case "GETMTMMATFINISHCODE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_M2MTransfer ClsBI67 = new B_M2MTransfer();
                        Response = ClsBI67.GetM2MMatFinishCodes(Data[1], Data[2], Data[3], Data[4], Data[5], Data[6], Data[7]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI67 = null;
                        break;

                    case "GETMTMMATVISIONPANELCODE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_M2MTransfer ClsBI68 = new B_M2MTransfer();
                        Response = ClsBI68.GetM2MMatVisionCodes(Data[1], Data[2], Data[3], Data[4], Data[5], Data[6], Data[7], Data[8]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI68 = null;
                        break;

                    case "GETMTMMATLIPPINGCODE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_M2MTransfer ClsBI699 = new B_M2MTransfer();
                        Response = ClsBI699.GetM2MMatLippingCodes(Data[1], Data[2], Data[3], Data[4], Data[5], Data[6], Data[7], Data[8], Data[9]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI699 = null;
                        break;

                    case "GETMTMSELECTEDMATERIALDATA":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_M2MTransfer ClsBI70 = new B_M2MTransfer();
                        Response = ClsBI70.GetM2MMatGetSelectedMatDetails(Data[1], Data[2], Data[3], Data[4], Data[5], Data[6], Data[7], Data[8], Data[9], Data[10]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI70 = null;
                        break;

                    case "GETMTMSCANNEDQRCODESTATUS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_M2MTransfer ClsBI71 = new B_M2MTransfer();
                        Response = ClsBI71.GetM2MScannedQRStatus(Data[1], Data[2]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI71 = null;
                        break;

                    case "SAVEMTMQRCODEDATA":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI72 = new B_LoadingOffloading();
                        dtMTMData = new DataTable();
                        int mSaveCount = 0;
                        int mSaveStatus = 0;
                        dtMTMData = ConvertMTMStringToDataTable(Data[1].ToString(), '#', '$');
                        if (dtMTMData.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dtMTMData.Rows)
                            {
                                string mLocCode = dr["LocationCode"].ToString().Trim();
                                string mNewMatcode = dr["MatCode"].ToString().Trim();
                                string mOldMatcode = dr["MatCode"].ToString().Trim();
                                string mMatDesc = dr["MatCode"].ToString().Trim();
                                string mMatGrade = dr["MatCode"].ToString().Trim();
                                string mMatGroup = dr["MatCode"].ToString().Trim();
                                string mThickness = dr["MatCode"].ToString().Trim();
                                string mSize = dr["MatCode"].ToString().Trim();
                                string mOldQRCode = dr["QRCode"].ToString().Trim();
                                string mStatus = dr["Status"].ToString().Trim();
                                string mUserId = dr["UserId"].ToString().Trim();
                                if (mStatus == "D")
                                {
                                    mSaveCount++;
                                    string UpdateStatus = ClsBI72.SaveUpdateDamagedQRCode(mLocCode, mNewMatcode, mOldMatcode, mOldQRCode, mUserId);
                                    if (UpdateStatus.Contains("SUCCESS"))
                                        mSaveCount++;
                                }
                            }
                        }
                        if (mSaveCount == 0)
                            Response = "SAVEMTMQRCODEDATA ~ ERROR ~ No Material Transferred, Kindly Try Again";
                        if (mSaveCount > 0)
                            Response = "SAVEMTMQRCODEDATA ~ SUCCESS ~ " + mSaveCount + " No. of Material Transfered Successfully";
                        //if (mSaveStatus >= mSaveCount)
                        //{
                        //    DataSet odsData = new DataSet();
                        //    odsData = ClsBI72.fnPIGetLoadOffloadDataSAPPost(dtDLoadOffData);
                        //    if (odsData.Tables[0].Rows.Count > 0)
                        //        Response = "SAVEMTMQRCODEDATA ~ SUCCESS ~ " + mSaveCount + " - No of Records Saved and Posted To SAP Successfully Out Of " + dtDLoadOffData.Rows.Count;
                        //    if (odsData.Tables[0].Rows.Count == 0)
                        //        Response = "SAVEMTMQRCODEDATA ~ SUCCESS ~ " + odsData.Tables[0].Rows.Count + " Transfered Successfully And Posted To SAP Successfully Out Of " + dtDLoadOffData.Rows.Count;
                        //}
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI36 = null;
                        break;

                        #endregion

                }

            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "_RemoteClient_OnDataArrival", exDetail);
                RemoteClient.Response = "ERROR ~ " + ex.Message;
                Response = "ERROR ~ " + ex.Message;
            }
            finally
            {
                RemoteClient.Response = Response + ">";
            }
        }



        public DataTable ConvertStringToDataTable(string strResponse, char chRowDel, char chColDel)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("LocationCode");
            dt.Columns.Add("MatCode");
            dt.Columns.Add("QRCode");
            dt.Columns.Add("StackQRCode");
            dt.Columns.Add("UserId");
            dt.Columns.Add("GradeDesc");
            dt.Columns.Add("GroupDesc");
            dt.Columns.Add("ThicknessDesc");
            dt.Columns.Add("Size");

            if (strResponse != string.Empty)
            {
                string[] strRow = strResponse.Split(chRowDel);

                for (int i = 0; i < strRow.Length; i++)
                {
                    string[] strCol = strRow[i].Split(chColDel);
                    DataRow dr = dt.NewRow();
                    dr[0] = strCol[0].Trim().ToString();
                    dr[1] = strCol[1].Trim().ToString();
                    dr[2] = strCol[2].Trim().ToString();
                    dr[3] = strCol[3].Trim().ToString();
                    dr[4] = strCol[4].Trim().ToString();
                    sMUserId = strCol[4].Trim().ToString();
                    dr[5] = strCol[5].Trim().ToString();
                    dr[6] = strCol[6].Trim().ToString();
                    dr[7] = strCol[7].Trim().ToString();
                    dr[8] = strCol[8].Trim().ToString();
                    dt.Rows.InsertAt(dr, i + 1);
                }
            }
            return dt;
        }

        public DataTable ConvertLoadOffloadStringToDataTable(string strResponse, char chRowDel, char chColDel)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("LocationCode");
            dt.Columns.Add("MatCode");
            dt.Columns.Add("QRCode");
            dt.Columns.Add("StackQRCode");
            dt.Columns.Add("UserId");
            dt.Columns.Add("DONo");
            if (strResponse != string.Empty)
            {
                string[] strRow = strResponse.Split(chRowDel);
                for (int i = 0; i < strRow.Length; i++)
                {
                    string[] strCol = strRow[i].Split(chColDel);
                    DataRow dr = dt.NewRow();
                    dr[0] = strCol[0].Trim().ToString();
                    dr[1] = strCol[1].Trim().ToString();
                    dr[2] = strCol[2].Trim().ToString();
                    dr[3] = strCol[3].Trim().ToString();
                    dr[4] = strCol[4].Trim().ToString();
                    dr[5] = strCol[9].Trim().ToString();
                    dt.Rows.InsertAt(dr, i + 1);
                }
            }
            dt.Columns["DONo"].SetOrdinal(1);
            return dt;
        }

        public DataTable FinalLoadOffloadData(DataTable dtTable, string tUserId)
        {
            for (int i = 0; i < dtTable.Rows.Count; i++)
            {
                string tMatcode = dtTable.Rows[i][1].ToString();
                string tQRcode = dtTable.Rows[i][2].ToString();
                for (int j = 0; j < dtLoadOffloadData.Rows.Count; j++)
                {
                    //string Stackmatcode = dtLoadOffloadData.Rows[j][1].ToString();
                    DataRow dr = dtLoadOffloadData.Rows[j];
                    if (dr[2].ToString() == tMatcode.Trim().ToString() && dr[3].ToString() == tQRcode.Trim().ToString())  //
                    {
                        //dr.Delete();
                        dtLoadOffloadData.Rows.Remove(dr);
                    }
                }
            }
            dtLoadOffloadData.AcceptChanges();
            return dtLoadOffloadData;
        }

        public DataTable RemoveStackDataFromLoadOffloadTable(string StackQRCode)
        {
            DataRow[] rows;
            rows = dtLoadOffloadData.Select("StackQRCode =" + StackQRCode);
            foreach (DataRow row in rows)
                dtLoadOffloadData.Rows.Remove(row);
            dtLoadOffloadData.AcceptChanges();
            return dtLoadOffloadData;
        }

        public DataTable ConvertDamagedLoadOffloadStringToDataTable(string strResponse, char chRowDel, char chColDel)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("LocationCode");
            dt.Columns.Add("DONo");
            dt.Columns.Add("MatCode");
            dt.Columns.Add("QRCode");
            dt.Columns.Add("Status");
            dt.Columns.Add("UserId");
            if (strResponse != string.Empty)
            {
                string[] strRow = strResponse.Split(chRowDel);
                for (int i = 0; i < strRow.Length; i++)
                {
                    string[] strCol = strRow[i].Split(chColDel);
                    DataRow dr = dt.NewRow();
                    dr[0] = strCol[5].Trim().ToString();
                    dr[1] = strCol[7].Trim().ToString();
                    dr[2] = strCol[0].Trim().ToString();
                    dr[3] = strCol[3].Trim().ToString();
                    dr[4] = strCol[4].Trim().ToString();
                    dr[5] = strCol[6].Trim().ToString();
                    dt.Rows.InsertAt(dr, i + 1);
                }
            }
            return dt;
        }

        public DataTable ConvertMTMStringToDataTable(string strResponse, char chRowDel, char chColDel)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("LocationCode");
            dt.Columns.Add("NewMatCode");
            dt.Columns.Add("MatDesc");
            dt.Columns.Add("MatGrade");
            dt.Columns.Add("MatGroup");
            dt.Columns.Add("Thickness");
            dt.Columns.Add("Size");
            dt.Columns.Add("OldQRCode");
            dt.Columns.Add("OldMatCode");
            dt.Columns.Add("Status");
            dt.Columns.Add("UserId");
            if (strResponse != string.Empty)
            {
                string[] strRow = strResponse.Split(chRowDel);
                for (int i = 0; i < strRow.Length; i++)
                {
                    string[] strCol = strRow[i].Split(chColDel);
                    DataRow dr = dt.NewRow();
                    dr[0] = strCol[0].Trim().ToString();
                    dr[1] = strCol[1].Trim().ToString();
                    dr[2] = strCol[2].Trim().ToString();
                    dr[3] = strCol[3].Trim().ToString();
                    dr[4] = strCol[4].Trim().ToString();
                    dr[5] = strCol[5].Trim().ToString();
                    dr[6] = strCol[6].Trim().ToString();
                    dr[7] = strCol[7].Trim().ToString();
                    dr[8] = strCol[8].Trim().ToString();
                    dr[9] = strCol[9].Trim().ToString();
                    dr[10] = strCol[10].Trim().ToString();
                    dt.Rows.InsertAt(dr, i + 1);
                }
            }
            return dt;
        }


        bool isPrinted;
        public string PrintSalesReturnQRCode(string objLocationCode, string objMatcode)
        {
            objclsPrint = new B_ClsPrint();
            string sPrintStatus = string.Empty;
            oDay = oMonth = oYear = oDateFormat = string.Empty;
            oDay = DateTime.Now.ToString("dd");
            oMonth = DateTime.Now.ToString("MM");
            oYear = DateTime.Now.ToString("yy");
            oDateFormat = oDay.Trim() + oMonth.Trim() + oYear.Trim();
            string objRanNo = objclsPrint.RandomString(2);

            sQRRunningSerial = objclsPrint.GetQRCodeRunningSerial(objLocationCode, oDateFormat, sPrintingSection, sLocationType);
            if (sQRRunningSerial == string.Empty || sQRRunningSerial == "")
                sQRRunningSerial = "0";
            int objNextNo = Convert.ToInt32(sQRRunningSerial);
            objNextNo = objNextNo + 1;
            sQRRunningSerial = Convert.ToString(objNextNo);
            if (sQRRunningSerial.Length == 4)
                sQRRunningSerial = "0" + sQRRunningSerial;
            if (sQRRunningSerial.Length == 3)
                sQRRunningSerial = "00" + sQRRunningSerial;
            if (sQRRunningSerial.Length == 2)
                sQRRunningSerial = "000" + sQRRunningSerial;
            if (sQRRunningSerial.Length == 1)
                sQRRunningSerial = "0000" + sQRRunningSerial;

            objQRCode = objLocationCode.Trim() + oDateFormat.Trim() + objRanNo.Trim() + sQRRunningSerial;

            string PrintStatus = objclsPrint.PrintSalesReturnQRCode(objLocationCode, objMatcode, objQRCode, oDateFormat);
            isPrinted = true;
            if (PrintStatus.Contains("SUCCESS"))
            {
                //DataTable dt = new DataTable();
                //dt = new DataTable();
                //dt.Columns.Add("LocationCode");
                //dt.Columns.Add("MatCode");
                //dt.Columns.Add("QRCode");
                //dt.Columns.Add("Status");
                //DataRow dr = dt.NewRow();
                //dr[0] = objLocationCode;
                //dr[1] = objMatcode;
                //dr[2] = objQRCode;
                //dr[3] = "S";
                //dt.Rows.Add(dr);
                //DataSet ds = fnPIGetProductionDataSAPPost(dt);
                //if (ds.Tables[0].Rows.Count > 0)
                //{
                //    foreach (DataRow dr1 in ds.Tables[0].Rows)
                //    {
                //        string objLoc = dr1["LocationCode"].ToString().Trim();
                //        string objMat = dr1["MatCode"].ToString().Trim();
                //        string objQR = dr1["QRCode"].ToString().Trim();
                //        string objStatus = dr1["Status"].ToString().Trim();
                //        string SAPPostMsg = dr1["SAPStatus"].ToString().Trim();
                //        string PostStatus = objclsPrint.UpdateQRCodeSAPStatus(objLoc, objMat, objQR, objStatus, SAPPostMsg);
                //        if (PostStatus.Contains("SUCCESS"))
                //            oSAPPostCount++;
                //    }
                //    if (oSAPPostCount > 0)
                //    {
                //        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtMessage, "SalesReturnData", oSAPPostCount + " No of Records Posted In SAP Out of " + dsProdData.Tables[0].Rows.Count + " For Materail Code - " + objMatcode + " At " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                //    }
                //}
                sPrintStatus = "PRINTSALESRETURNQRCODE ~ SUCCESS ~ QRCODE - " + objQRCode + " Is Generated And Printed Successfully";
            }
            else if (PrintStatus.Contains("ERROR"))
            {
                sPrintStatus = "PRINTSALESRETURNQRCODE ~ ERROR ~ ERROR In Save/Posted ";
            }
            return sPrintStatus;
        }

        public string PrintLoadOffloadStackQRCode(DataTable dt, string DONo)
        {
            dtsStackData = new DataTable();
            dtFinalData = new DataTable();
            objclsPrint = new B_ClsPrint();
            objclsload = new B_LoadingOffloading();
            string objLocCode = string.Empty;
            string objMatCode = string.Empty;
            string objQRCode = string.Empty;
            string objStackQRCode = string.Empty;
            string objNewStackQRCode = string.Empty;
            string objUserId = string.Empty;
            string sSaveStatus = string.Empty;
            string sPrintStatus = string.Empty;
            sSaveCount = 0;
            sStackPrintCount = 0;
            oDay = oMonth = oYear = oDateFormat = string.Empty;
            oDay = DateTime.Now.ToString("dd");
            oMonth = DateTime.Now.ToString("MM");
            oYear = DateTime.Now.ToString("yy");
            oDateFormat = oDay.Trim() + oMonth.Trim() + oYear.Trim();

            sStackRunningSerial = objclsPrint.GetStackRunningSerial(objLocationCode, oDateFormat, sPrintingSection, sLocationType);
            if (sStackRunningSerial == string.Empty || sStackRunningSerial == "")
                sStackRunningSerial = "0";
            int objNextStackNo = Convert.ToInt32(sStackRunningSerial);
            objNextStackNo = objNextStackNo + 1;
            sStackRunningSerial = Convert.ToString(objNextStackNo);
            if (sStackRunningSerial.Length == 4)
                sStackRunningSerial = "0" + sStackRunningSerial.Trim();
            if (sStackRunningSerial.Length == 3)
                sStackRunningSerial = "00" + sStackRunningSerial.Trim();
            if (sStackRunningSerial.Length == 2)
                sStackRunningSerial = "000" + sStackRunningSerial.Trim();
            if (sStackRunningSerial.Length == 1)
                sStackRunningSerial = "0000" + sStackRunningSerial.Trim();

            objNewStackQRCode = objLocationCode.Trim() + oDateFormat.Trim() + sStackRunningSerial;
            dtsStackData = dt.Clone();
            dtsStackData = dt.Copy();
            //dtSplit.ImportRow(dtLoadOffloadData.Rows[dtLoadOffloadData.Rows.Count - Convert.ToInt32(SplitQty)]);

            //for (int i = 0; i < dtSplit.Rows.Count; i++)
            //{
            //    string Dqrcode = dtSplit.Rows[i][3].ToString();
            //    for (int j = 0; j < dtLoadOffloadData.Rows.Count; j++)
            //    {
            //        DataRow dr = dtLoadOffloadData.Rows[j];
            //        if (dr["QRCode"].ToString() == Dqrcode.ToString())
            //            dr.Delete();
            //    }
            //}
            //dtLoadOffloadData.AcceptChanges();

            if (dtsStackData.Rows.Count > 0)
            {
                for (int i = 0; i < dtsStackData.Rows.Count; i++)
                {
                    objLocCode = dtsStackData.Rows[i][0].ToString().Trim();
                    objMatCode = dtsStackData.Rows[i][1].ToString().Trim();
                    objQRCode = dtsStackData.Rows[i][2].ToString().Trim();
                    objStackQRCode = dtsStackData.Rows[i][3].ToString().Trim();
                    objUserId = dtsStackData.Rows[i][4].ToString().Trim();
                    sSaveStatus = objclsload.SaveUpdateStackQRCode(objLocCode, DONo.Trim(), objMatCode, objQRCode, objStackQRCode, objNewStackQRCode, oDateFormat, objUserId);
                    if (sSaveStatus.Contains("SUCCESS"))
                    {
                        sSaveCount++;
                        isPrinted = true;
                    }
                }
                if (sSaveCount == dtsStackData.Rows.Count)
                {
                    sSaveStatus = objclsload.UpdateStackItemSerial(objLocCode, objNewStackQRCode, oDateFormat, sPrintingSection, sLocationType);
                    if (sSaveStatus.Contains("SUCCESS"))
                    {
                        int iCount = 0;
                        dtFinalData = dtsStackData.Clone();
                        dtFinalData = dtsStackData.DefaultView.ToTable(true, "MatCode");  //"QRCode"
                        dtFinalData.Columns.Add("LocationCode");
                        dtFinalData.Columns.Add("QRCode");
                        dtFinalData.Columns.Add("StackQRCode");
                        dtFinalData.Columns.Add("UserId");
                        dtFinalData.Columns.Add("GradeDesc");
                        dtFinalData.Columns.Add("GroupDesc");
                        dtFinalData.Columns.Add("ThicknessDesc");
                        dtFinalData.Columns.Add("Size");
                        dtFinalData.Columns.Add("TotalQty");
                        dtFinalData.Columns["LocationCode"].SetOrdinal(0);
                        dtsStackData.Columns.Add("TotalQty");

                        for (int i = 0; i < dtFinalData.Rows.Count; i++)
                        {
                            string Finalmatcode = dtFinalData.Rows[i][1].ToString();
                            for (int j = 0; j < dtsStackData.Rows.Count; j++)
                            {
                                string Stackmatcode = dtsStackData.Rows[j][1].ToString();
                                if (Stackmatcode == Finalmatcode)
                                {
                                    iCount++;
                                    dtFinalData.Rows[i][0] = dtsStackData.Rows[j][0].ToString();
                                    dtFinalData.Rows[i][1] = dtsStackData.Rows[j][1].ToString();
                                    dtFinalData.Rows[i][2] = dtsStackData.Rows[j][2].ToString();
                                    dtFinalData.Rows[i][3] = dtsStackData.Rows[j][3].ToString();
                                    dtFinalData.Rows[i][4] = dtsStackData.Rows[j][4].ToString();
                                    dtFinalData.Rows[i][5] = dtsStackData.Rows[j][5].ToString();
                                    dtFinalData.Rows[i][6] = dtsStackData.Rows[j][6].ToString();
                                    dtFinalData.Rows[i][7] = dtsStackData.Rows[j][7].ToString();
                                    dtFinalData.Rows[i][8] = dtsStackData.Rows[j][8].ToString();
                                    dtFinalData.Rows[i][9] = Convert.ToString(iCount);
                                }
                            }
                        }

                        sPrintStatus = objclsload.PrintStackQRCodeItem(dtFinalData, objNewStackQRCode);
                        if (sPrintStatus.Contains("SUCCESS"))
                        {
                            isPrinted = true;
                            sStackPrintCount++;
                        }
                    }
                }
            }
            else
            {
                sPrintStatus = "PRINTLOADOFFLOADSTACKQRCODE ~ ERROR ~ ERROR In Create";
            }
            return sPrintStatus;
        }

        public string PrintSegregationStackQRCode(string sLocCode, string sUserID)
        {
            objclsPrint = new B_ClsPrint();
            objSegPrint = new B_StackPrinting();
            string sPrintStatus = string.Empty;
            string sMatCode = string.Empty;
            string sMatStatus = string.Empty;
            string sGradeDesc = string.Empty;
            string sGroupDesc = string.Empty;
            string sThicknessDesc = string.Empty;
            string sMatSize = string.Empty;
            oDay = oMonth = oYear = oDateFormat = string.Empty;
            oDay = DateTime.Now.ToString("dd");
            oMonth = DateTime.Now.ToString("MM");
            oYear = DateTime.Now.ToString("yy");
            oDateFormat = oDay.Trim() + oMonth.Trim() + oYear.Trim();

            sStackRunningSerial = objclsPrint.GetStackRunningSerial(objLocationCode, oDateFormat, sPrintingSection, sLocationType);
            if (sStackRunningSerial == string.Empty || sStackRunningSerial == "")
                sStackRunningSerial = "0";
            int objNextStackNo = Convert.ToInt32(sStackRunningSerial);
            objNextStackNo = objNextStackNo + 1;
            sStackRunningSerial = Convert.ToString(objNextStackNo);
            if (sStackRunningSerial.Length == 4)
                sStackRunningSerial = "0" + sStackRunningSerial.Trim();
            if (sStackRunningSerial.Length == 3)
                sStackRunningSerial = "00" + sStackRunningSerial.Trim();
            if (sStackRunningSerial.Length == 2)
                sStackRunningSerial = "000" + sStackRunningSerial.Trim();
            if (sStackRunningSerial.Length == 1)
                sStackRunningSerial = "0000" + sStackRunningSerial.Trim();

            objStackQRCode = objLocationCode.Trim() + oDateFormat.Trim() + sStackRunningSerial;

            if (dtStackData.Rows.Count > 0)
            {
                //for (int i = 0; i < dtStackData.Rows.Count; i++)
                //{
                //objLocationCode = dtStackData.Rows[i][0].ToString().Trim();
                //sMatCode = dtStackData.Rows[i][1].ToString().Trim();
                //objQRCode = dtStackData.Rows[i][2].ToString().Trim();
                //sMatStatus = dtStackData.Rows[i][3].ToString().Trim();
                //sGradeDesc = dtStackData.Rows[i][4].ToString().Trim();
                //sGroupDesc = dtStackData.Rows[i][5].ToString().Trim();
                //sThicknessDesc = dtStackData.Rows[i][6].ToString().Trim();
                //sMatSize = dtStackData.Rows[i][7].ToString().Trim();
                if (B_StackPrinting.sStackPrintCount == 0)
                {
                    //objLocationCode, sMatCode, objQRCode, objStackQRCode, oDateFormat, sMatStatus, sGradeDesc, sGroupDesc, sThicknessDesc, sMatSize, Convert.ToString(dtStackData.Rows.Count)
                    sPrintStatus = objSegPrint.PrintSegregationStackQRCode(dtStackData, objStackQRCode, oDateFormat);
                    if (sPrintStatus.Contains("SUCCESS"))
                    {
                        isPrinted = true;
                        sStackPrintCount++;
                    }
                }
                else if (B_StackPrinting.sStackPrintCount == 1)
                {
                    sPrintStatus = objSegPrint.SaveSegregationStackQRCode(objLocationCode, sMatCode, objQRCode, objStackQRCode, oDateFormat, sMatStatus);
                    if (sPrintStatus.Contains("SUCCESS"))
                    {
                        isPrinted = true;
                        //sStackPrintCount++;
                    }
                }
                //}
            }
            if (sStackPrintCount == 1)
            {
                //dsData = new DataSet();
                //dtStackData.Columns.Remove("GradeDesc");
                //dtStackData.Columns.Remove("GroupDesc");
                //dtStackData.Columns.Remove("ThicknessDesc");
                //dtStackData.Columns.Remove("MatSize");
                //dsData = objSegPrint.fnPIGetSegregationDataSAPPost(dtStackData);
                //if (dsData.Tables[0].Rows.Count > 0)
                //{
                //    foreach (DataRow dr in dsData.Tables[0].Rows)
                //    {
                //        string LocCode = dr["LocationCode"].ToString().Trim();
                //        string objMatcode = dr["MatCode"].ToString().Trim();
                //        objQRCode = dr["QRCode"].ToString().Trim();
                //        string objMatStatus = dr["Status"].ToString().Trim();
                //        string SAPPostMsg = dr["SAPStatus"].ToString().Trim();
                //        string PostStatus = objSegPrint.UpdateSegregationQRCodeSAPStatus(LocCode, objMatcode, objQRCode, objMatStatus, SAPPostMsg, sUserID);
                //        if (PostStatus.Contains("SUCCESS"))
                //            oSAPPostCount++;
                //    }
                //}
                //if (oSAPPostCount > 0)
                sPrintStatus = "PRINTSEGREGATIONSTACKQRCODE ~ SUCCESS ~ STACK QRCODE - " + objStackQRCode + " Is Generated, Printed And " + oSAPPostCount + " No fo Records Posted To SAP Successfully Out Of " + dtStackData.Rows.Count;
            }
            else
            {
                sPrintStatus = "PRINTSEGREGATIONSTACKQRCODE ~ ERROR ~ ERROR In Printing of Stack QRCode, Kindly Try Again For Printing";
            }
            return sPrintStatus;
        }


        public static string GetLoaclIP()
        {
            string ip = "127.0.0.1";

            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();
            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);
            IPAddress[] addr = ipEntry.AddressList;
            foreach (IPAddress adr in addr)
            {
                // clsGlobal.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "_RemoteClient_OnDataArrival", adr);
                if (adr.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                {
                    if (adr.ToString().StartsWith("192.") || adr.ToString().StartsWith("10.") || adr.ToString().StartsWith("172."))
                    {
                        ip = adr.ToString();
                        break;
                    }
                }
            }
            return ip;
        }

        delegate void AddClientList(string IP);
        delegate void RemoveClientList(string IP);

        //void RemoveList(string ip)
        //{
        //    for (int i = 0; i < lvClientIP.Items.Count; i++)
        //    {
        //        if (lvClientIP.Items[i].Text.CompareTo(ip) == 0)
        //        {
        //            lvClientIP.Items[i].Remove();
        //            break;
        //        }
        //    }
        //    UpdateClientCount();
        //}

        //void AddList(string ip)
        //{
        //    lvClientIP.Items.Add(ip);
        //    UpdateClientCount();

        //}

        //void m_Socket_OnClientConnected(string IP)
        //{
        //    lvClientIP.Invoke(new AddClientList(AddList), IP);
        //}

        void m_Socket_OnClientDisconnected(string IP)
        {
            //lvClientIP.Invoke(new RemoveClientList(RemoveList), IP);
            //MessageBox.Show("Client Disconnected : " + IP);
            //throw new Exception("The method or operation is not implemented.");
        }

        // This method could be called by either the main thread or any of the
        // worker threads

        void _UpdateClientCount(string IP, string Message)
        {
            ListViewItem _lItem;
            if (lstData.InvokeRequired)
            {
                _dlgUpdateClient _dNew = new _dlgUpdateClient(_UpdateClientCount);
                this.Invoke(_dNew, new object[] { IP, Message });
            }
            else
            {
                if (lstData.Items.Count >= 100)
                    lstData.Items.Clear();
                _lItem = new ListViewItem();
                _lItem.Text = Convert.ToString(lstData.Items.Count);
                //_lItem.SubItems.Add(LogTime);
                _lItem.SubItems.Add(Message);
                _lItem.SubItems.Add(IP);
                lstData.Items.Add(_lItem);
            }
        }

        private void ServerDetails()
        {
            DbName = string.Empty;
            string strDatabase = string.Empty;
            string strServer = string.Empty;
            try
            {
                string hostName = Dns.GetHostName();
                string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
                strDatabase = GlobalVariable.mSqlDb;
                DbName = GlobalVariable.mSqlDb;
                lblserverandport.Text = string.Format("{0}:{1}", myIP, 9191);
                lblStatus.Text = string.Format("Database : {0}", strDatabase);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "ServerDetails", ex.Message.ToString());
            }
        }

        private void GetConnect()
        {
            try
            {
                if (oBCommon.readConnectionSetting(false))
                {
                    ServerDetails();
                    cmdStop.Enabled = true;
                    cmdSave1.Enabled = false;
                    //cmdConnect_Click(null, null);
                    //clearAllValues();
                }
                else
                {
                    Messagetab.SelectedTab = tabPage2;
                    //SetDbAppSetting();
                }
                //lblPort.Text = "PORT : - " + clsGlobal.mSockPort;
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "GetConnect", ex.Message.ToString());
                //throw ex;
                MessageBox.Show(ex.Message);
            }
        }

        ////void UpdateClientCount()
        ////{
        ////    lblCount.Text = lvClientIP.Items.Count.ToString();
        ////    lblCount.Text = "Version 1.2.1";
        ////}

        private void button1_Click_1(object sender, EventArgs e)
        {
            // m_Socket.SendMsgToClient("EXIT", "192.168.0.222");
        }

        private void cmdSend_Click(object sender, EventArgs e)
        {
            try
            {
                //m_Socket.SendMsgToClient("<" + txtMessage.Text + ">", lvClientIP.Items[0].Text);
                //   txtMessage.Text = ""; 
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "cmdSend_Click", ex.Message.ToString());
            }
        }

        private void txtMessage_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                cmdSend_Click(sender, e);
        }

        delegate void delUpdateTagData(string data);

        private void btnclear_Click(object sender, EventArgs e)
        {
            if (lstData.Items.Count > 0)
            {
                lstData.Items.Clear();
            }
        }

        private void lstData_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmdTestCon1_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbServer1.Text.Trim() != string.Empty)
                {
                    string strCon = "Data Source=" + cmbServer1.Text.Trim() + ";Initial Catalog=" + cmbSchema1.SelectedValue.ToString() + ";";
                    strCon = strCon + " User ID=" + txtUserID1.Text.Trim() + ";";
                    txtConString.Text = strCon + "Password = ******;";
                    strCon = strCon + "Password = " + txtPwd1.Text.Trim() + ";";
                    SqlConnection oCon = new SqlConnection(strCon);
                    oCon.Open();
                    oCon.Close();
                    txtConString.Text = strCon;
                    MessageBox.Show("Test Connection Sucessfully!");
                }
            }
            catch (SqlException oSql)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "cmdTestCon1_Click", oSql.Message.ToString());
                MessageBox.Show(oSql.Message);
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "cmdTestCon1_Click", ex.Message.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbServer1_Enter(object sender, EventArgs e)
        {
            if (cmbServer1.Items.Count == 0)
            {
                FillComboBox(cmbServer1, getDBServer(), true);
            }
        }

        private void FillComboBox(ComboBox cbo, DataTable dt, bool isSelect)
        {
            if (isSelect)
            {
                DataRow dr = dt.NewRow();
                dr[0] = "--Select--";
                dr[1] = "";
                dt.Rows.InsertAt(dr, 0);
            }
            cbo.DisplayMember = dt.Columns[0].ToString();
            cbo.ValueMember = dt.Columns[1].ToString();
            cbo.DataSource = dt;
        }

        private DataTable getDBServer()
        {
            DataTable dtDbServer = new DataTable();
            dtDbServer.Columns.Add("Display");
            dtDbServer.Columns.Add("Value");
            DataTable dtResults = SqlDataSourceEnumerator.Instance.GetDataSources();
            string strInstance;
            foreach (DataRow dr in dtResults.Rows)
            {
                if (dr["InstanceName"].ToString() != string.Empty)
                {
                    strInstance = "\\" + dr["InstanceName"].ToString();
                }
                else
                {
                    strInstance = string.Empty;
                }

                DataRow drRow = dtDbServer.NewRow();
                drRow["Display"] = dr["ServerName"].ToString() + strInstance;
                drRow["Value"] = dr["ServerName"].ToString() + strInstance;
                dtDbServer.Rows.Add(drRow);
            }
            return dtDbServer;
        }

        private void cmbSchema1_Enter(object sender, EventArgs e)
        {
            try
            {
                if (cmbSchema1.Items.Count == 0)
                {
                    FillComboBox(cmbSchema1, getDBSchema(cmbServer1.Text.ToString(), txtUserID1.Text.Trim(), txtPwd1.Text.Trim()), true);
                }
            }
            catch (SqlException ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "cmbSchema1_Enter", ex.Message.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private DataTable getDBSchema(string strSource, string strUser, string strPwd)
        {
            DataTable dtSchema = new DataTable();
            try
            {
                dtSchema.Columns.Add("Display");
                dtSchema.Columns.Add("Value");
                string strCon = "Data Source=" + strSource + ";";
                strCon = strCon + " User ID=" + strUser + "; Password=" + strPwd + ";";

                SqlConnection oCon = new SqlConnection(strCon);
                oCon.Open();
                DataTable dtResults = oCon.GetSchema("Databases"); ;
                oCon.Close();
                foreach (DataRow dr in dtResults.Rows)
                {
                    DataRow drRow = dtSchema.NewRow();
                    drRow["Display"] = dr["database_name"].ToString();
                    drRow["Value"] = dr["database_name"].ToString();
                    dtSchema.Rows.Add(drRow);
                }
                return dtSchema;
            }
            catch (SqlException ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "getDBSchema", ex.Message.ToString());
                MessageBox.Show(ex.Message);
                return dtSchema;
            }
        }

        private void cmdSave1_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSAPId.Text == "" && txtSAPPassword.Text == "" && txtUserID1.Text == "" && txtPwd1.Text == "" && cmbSchema1.SelectedIndex == 0 && cmbServer1.SelectedIndex == 0)
                {
                    MessageBox.Show("Please fill all the details.");
                    return;
                }


                StreamWriter _wr = new StreamWriter(strConfigLocaldb);
                _wr.WriteLine("<LOCAL_SETTING>");
                _wr.WriteLine("SERVERNAME=" + "180.151.246.50, 4222");// cmbServer1.Text.ToString().Trim());
                _wr.WriteLine("DATABASENAME=" + cmbSchema1.Text.ToString().Trim());
                _wr.WriteLine("USERNAME=" + txtUserID1.Text.Trim());
                _wr.WriteLine("PASSWORD=" + txtPwd1.Text.Trim());
                _wr.WriteLine("SAPID=" + txtSAPId.Text.Trim());
                _wr.WriteLine("SAPPASSWORD=" + txtSAPPassword.Text.Trim());
                _wr.WriteLine("</LOCAL_SETTING>");
                _wr.Close();
                _wr.Dispose();

                if (oBCommon.readConnectionSetting(false))
                {
                    ServerDetails();
                    //cmdStart(null, null);
                    //clearAllValues();
                }
                else
                {
                    Messagetab.SelectedTab = tabPage2;
                    //SetDbAppSetting();
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "cmdSave1_Click", ex.Message.ToString());
                MessageBox.Show(ex.Message);
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {
            //Messagetab.TabPages.Remove(tabPage2);
        }

       
    }
}