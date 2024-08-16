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
using GreenplyScannerCommServer.Common;
using System.Data.SqlClient;
using System.Data.Sql;
using GreenplyScannerCommServer.BI;
using MOXA_CSharp_MXIO;
using TMSServer;
using TEST;
//using GreenplyScannerCommServer.GreenplyERPPostingService;

namespace GreenplyScannerCommServer
{
    public partial class frmMain : Form
    {
        static string strConfigLocaldb = Application.StartupPath + "\\localSetting.ini";
        BCILCommServer.BCILSocketServer _BCILSocketServer;  //
        ServerSocket m_Socket;
        Hashtable _RunningIP;
        ServerProcess _ServerProcess;
        PCommon clsPCommon = new PCommon();
        BCommon oBCommon = new BCommon();
        BcilNetwork _bcilNetwork = new BcilNetwork();
        public static string sPrintingSection = string.Empty;
        public static string sLocationType = string.Empty;

        public static string sHubSelectedMatCode = string.Empty;
        
        /// <summary>
        ///
        /// </summary>

        bool isRunning;
        //bool isRunning;
        bool isNotRunning;
        bool isPrinted;
        private System.Threading.Thread myThread;
        bool SchedularStart = false;
        B_ClsPrint objclsPrint;
        B_LoadingOffloading objclsload;
        B_StackPrinting objSegPrint;
        ioLogik obj_iologik;
        SendMoxa obj_sendmoxa;
        string objLocationCode;

        string objMatcode = string.Empty;
        string objMatDesc = string.Empty;
        string objMatGrade = string.Empty;
        string objMatGradeDesc = string.Empty;
        string objMatGroup = string.Empty;
        string objMatGroupDesc = string.Empty;
        string objMatThickness = string.Empty;
        string objMatThicknessDesc = string.Empty;
        string objMatSize = string.Empty;

        string objPrintedMatcode = string.Empty;
        string objPrintedMatGradeDesc = string.Empty;
        string objPrintedMatGroupDesc = string.Empty;
        string objPrintedMatThickness = string.Empty;
        string objPrintedMatSize = string.Empty;
        string objQRCode = string.Empty;
        string objStackQRCode = string.Empty;
        string objNewStackQRCode = string.Empty;
        string visualBarcode = string.Empty;
        string oMonth = string.Empty;
        string oDay = string.Empty;
        string oYear = string.Empty;
        string oDateFormat = string.Empty;
        string sQRRunningSerial;
        string sStackRunningSerial;
        string DONo = string.Empty;
        string oDispatchDONo = string.Empty;
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
        DataTable dtDispatchData;
        DataTable dtLoadOffData;
        DataTable dtDepotData;
        DataTable dtSRData;
        DataTable dtMTMData;
        DataTable dtStockCount;
        DataTable dtProdData;
        public static DataTable dtDeleteQRData;
        public static DataTable dtStackMergeData;
        public static DataTable dtStackSplitData;
        public static DataTable odtSplitData;
        public static DataTable dtStackDeleteData;
        public static DataTable dtDLoadOffData;
        DataTable dtsStackData = new DataTable();
        DataTable dtFinalData = new DataTable();
        DataTable dtFinalDispatchData = new DataTable();
        public static DataTable dtStackData;
        DataSet dsData = new DataSet();
        DataSet dsProdData = new DataSet();
        DataTable dtFTPData = null;
        bool IsStackPrint = false;

        // delegate void _dlgUpdateClient(string IP, string LogTime, string Message);
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
            dtLoadOffloadData.Columns.Add("GradeDesc");
            dtLoadOffloadData.Columns.Add("GroupDesc");
            dtLoadOffloadData.Columns.Add("ThicknessDesc");
            dtLoadOffloadData.Columns.Add("Size");
            dtLoadOffloadData.Columns.Add("QRCode");
            dtLoadOffloadData.Columns.Add("StackQRCode");
            dtLoadOffloadData.Columns.Add("ScannedStatus");
            dtLoadOffloadData.Columns.Add("ChallanQty");
            dtLoadOffloadData.Columns.Add("ToBeScannedQty");
            dtLoadOffloadData.Columns.Add("TotalScannedQty");
            dtLoadOffloadData.Columns.Add("TotalStackQty");
            dtLoadOffloadData.Columns.Add("TotalMatQty");
            dtLoadOffloadData.Columns.Add("LoadOffloadStatus");
            //dtLoadOffloadData.Columns.Add("sBeScannedQty");
            //dtLoadOffloadData.Columns.Add("sTotalScannedQty");
            //dtLoadOffloadData.Columns.Add("sTotalStackQty");


            dtDeleteQRData = new DataTable();
            dtDeleteQRData.Columns.Add("LocationCode");
            dtDeleteQRData.Columns.Add("MatCode");
            dtDeleteQRData.Columns.Add("QRCode");
            dtDeleteQRData.Columns.Add("UserID");

            dtStackMergeData = new DataTable();
            dtStackMergeData.Columns.Add("LocationCode");
            dtStackMergeData.Columns.Add("MatCode");
            dtStackMergeData.Columns.Add("GradeDesc");
            dtStackMergeData.Columns.Add("GroupDesc");
            dtStackMergeData.Columns.Add("ThicknessDescription");
            dtStackMergeData.Columns.Add("Size");
            dtStackMergeData.Columns.Add("StackQRCode");
            dtStackMergeData.Columns.Add("TotalQty");

            dtStackSplitData = new DataTable();
            dtStackSplitData.Columns.Add("LocationCode");
            dtStackSplitData.Columns.Add("MatCode");
            dtStackSplitData.Columns.Add("GradeDesc");
            dtStackSplitData.Columns.Add("GroupDesc");
            dtStackSplitData.Columns.Add("ThicknessDescription");
            dtStackSplitData.Columns.Add("Size");
            dtStackSplitData.Columns.Add("QRCode");
            dtStackSplitData.Columns.Add("StackQRCode");
            dtStackSplitData.Columns.Add("TotalQty");

            dtStackDeleteData = new DataTable();
            dtStackDeleteData.Columns.Add("LocationCode");
            dtStackDeleteData.Columns.Add("MatCode");
            dtStackDeleteData.Columns.Add("GradeDesc");
            dtStackDeleteData.Columns.Add("GroupDesc");
            dtStackDeleteData.Columns.Add("ThicknessDescription");
            dtStackDeleteData.Columns.Add("Size");
            dtStackDeleteData.Columns.Add("QRCode");
            dtStackDeleteData.Columns.Add("StackQRCode");
            dtStackDeleteData.Columns.Add("TotalQty");

            dtDispatchData = new DataTable();
            dtDispatchData.Columns.Add("LocationCode");
            dtDispatchData.Columns.Add("DONo");
            dtDispatchData.Columns.Add("MatCode");
            dtDispatchData.Columns.Add("QRCode");

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

            //dtDLoadOffData = new DataTable();
            //dtDLoadOffData.Columns.Add("LocationCode");
            //dtDLoadOffData.Columns.Add("DONo");
            //dtDLoadOffData.Columns.Add("CustomerName");
            //dtDLoadOffData.Columns.Add("MatCode");
            //dtDLoadOffData.Columns.Add("DOQty");
            //dtDLoadOffData.Columns.Add("ScannedQty");
            //dtDLoadOffData.Columns.Add("QRCode");
            //dtDLoadOffData.Columns.Add("Status");

            //Messagetab.TabPages.Remove(tabPage1);
            Messagetab.TabPages.Remove(tabPage2);
            Messagetab.TabPages.Remove(tabPage1);
            string _sResult = string.Empty;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                txtMessage.Visible = false;
                _RunningIP = new Hashtable();
                GetConnect();
                _BCILSocketServer = new BCILSocketServer(VariableInfo.mSockPort, 100);
                _BCILSocketServer.OnClientConnect += new BCILSocketServer.NewClientHandler(_BCILSocketServer_OnClientConnect); //
                _BCILSocketServer.EOMChar = ">";
                _BCILSocketServer.SessionTimeOut = 2;

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
                button1_Click(sender, e);//
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "FormLoad", ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
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
                    objLocationCode = GlobalVariable.mSiteCode.Trim();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GreenplyScannerCommServer => ", "Communication Server Started and Connected to " + lblStatus.Text + " at IP Address and Port : " + lblserverandport.Text + " successfully");
                }
                else
                {
                    lblStatus.Text = "Communication Server Disconnected";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GreenplyScannerCommServer => ", "Communication Server Disconnected Because of Connection File Error");
                    lblStatus.BackColor = Color.Green;
                    cmdStop.Enabled = false;
                    cmdStart.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "ConnectClick", ex.Message.ToString());
                MessageBox.Show(ex.ToString());
            }
        }

        private void cmdStop_Click(object sender, EventArgs e)
        {
            try
            {
                lblStatus.BackColor = Color.Red;
                lblStatus.ForeColor = Color.White;
                cmdStop.Enabled = false;
                cmdStart.Enabled = true;
                lblStatus.Text = "Communication Server Disconnected";
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GreenplyScannerCommServer => ", "Communication Server Disconnected");
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "DisconnectClick", ex.Message);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            try
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GreenplyScannerCommServer => ", "Communication Server Stoped");
                _BCILSocketServer.StopService();
                if (_BCILSocketServer != null)
                    _BCILSocketServer = null;
                if (isRunning == true)
                    isRunning = false;
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
                string objMatStatus = "P";
                //int sMatPrintCount = 0;
                DataTable objMatData = new DataTable();

                //isPrinted = false;
                objclsPrint = new B_ClsPrint();
                obj_iologik = new ioLogik();
                string moxaIp = GreenplyScannerCommServer.Properties.Settings.Default.MoxaDeviceIP;
                objMatData = objclsPrint.GetSelectedMatCode(objLocationCode);
                string StackStatus = objclsPrint.GetIsStackPrintStatus(objLocationCode);
                if (objMatData.Rows.Count == 1)
                {
                    objMatcode = objMatData.Rows[0][0].ToString();
                    objMatDesc = objMatData.Rows[0][1].ToString();
                    objMatGrade = objMatData.Rows[0][2].ToString();
                    objMatGradeDesc = objMatData.Rows[0][3].ToString();
                    objMatGroup = objMatData.Rows[0][4].ToString();
                    //if (objMatGroup != "" && objMatGroup.Length >= 4)
                    //    objMatGroup = objMatGroup.Substring(objMatGroup.Length - 4);
                    objMatGroupDesc = objMatData.Rows[0][5].ToString();
                    objMatThickness = objMatData.Rows[0][6].ToString();
                    objMatThicknessDesc = objMatData.Rows[0][7].ToString();
                    objMatSize = objMatData.Rows[0][8].ToString();
                }
                if (objMatcode == "")
                {
                    MessageBox.Show("There is No Matrial Selected From PC Application to Print the QRCode, First Select the Matrial From PC Application to Start Printing");
                    if (cmdStart.Enabled == false)
                    {
                        lblStatus.BackColor = Color.Red;
                        lblStatus.ForeColor = Color.Black;
                        cmdStop.Enabled = false;
                        cmdStart.Enabled = true;
                        lblStatus.Text = "Communication Server Stopped";
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "cmdStop_Click", lblStatus.Text);
                    }
                    //_BCILSocketServer.StopService();
                    //_BCILSocketServer = null;
                    return;
                }

                #region QRCode
                oDay = oMonth = oYear = oDateFormat = string.Empty;
                oDay = DateTime.Now.ToString("dd");
                oMonth = DateTime.Now.ToString("MM");
                oYear = DateTime.Now.ToString("yy");
                oDateFormat = oDay.Trim() + oMonth.Trim() + oYear.Trim();
                string objRanNo = objclsPrint.RandomString(2);

                #region If Stack not printing in plant

                obj_iologik.EthernetDIRead_Main(moxaIp);
                string obj_itmstus = obj_iologik.Obj_ItmStatus;
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "RunProcess", "MOXA Status " + obj_iologik.Obj_ItmStatus + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                if (obj_itmstus == "ON" && !isPrinted)
                {
                    if (Convert.ToBoolean(StackStatus) == false)
                    {
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
                        //sMatPrintCount++;
                        string PrintStatus = objclsPrint.PrintQRCodeItem(objLocationCode, objMatcode, objMatDesc, objMatGrade, objMatGroup, objMatGroupDesc, objMatThickness, objMatThicknessDesc, objMatSize, objQRCode, objMatStatus, oDateFormat, sPrintingSection, sLocationType, sMatPrintCount);
                        //isPrinted = true;
                        if (PrintStatus == "SUCCESS")
                        {
                            obj_sendmoxa.SendMoxaIO(moxaIp, "1");
                            //return;
                        }
                        else
                        {
                            obj_sendmoxa.SendMoxaIO(moxaIp, "0");
                            //return;
                        }
                    }
                }
                else if (obj_itmstus == "OFF")
                {
                    isPrinted = false;
                }

                #endregion

                #region When stack lable is printing on conveyor
                else if (Convert.ToBoolean(StackStatus) == true)
                {
                    sMatLotSize = objclsPrint.GetSelectedMatLotSize(objMatcode);
                    sMatLotPrintedQty = objclsPrint.GetSelectedMatPrintedLotQty(objMatcode);

                    #region If LotSize not zero
                    if (sMatLotSize > 0)
                    {
                        #region Is MatCode printed
                        if (objPrintedMatcode.Trim() != string.Empty || objPrintedMatcode.Trim() != "")
                        {
                            if (objMatcode.Trim() != objPrintedMatcode.Trim())
                            {
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

                                string PrintStatus = objclsPrint.PrintStackQRCodeItem(objLocationCode, objPrintedMatcode, objStackQRCode, oDateFormat, sPrintingSection, sLocationType, objPrintedMatGradeDesc.Trim(), objPrintedMatGroupDesc.Trim(), objPrintedMatThickness.Trim(), objPrintedMatSize.Trim(), sMatPrintCount.ToString());
                                if (PrintStatus == "SUCCESSFULL")
                                {
                                    sMatPrintCount = 0;
                                    oSAPPostCount = 0;
                                    sMatLotPrintedQty = 0;
                                    objPrintedMatcode = string.Empty;
                                    objPrintedMatGradeDesc = string.Empty;
                                    objPrintedMatGroupDesc = string.Empty;
                                    objPrintedMatThickness = string.Empty;
                                    objPrintedMatSize = string.Empty;
                                    //lblPrintCount.Text = "***";
                                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtMessage, "ProductionData", dtProdData.Rows.Count + " No of Records Posted In SAP Out of " + dtProdData.Rows.Count + " For Material Code - " + objMatcode + " At " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                                }
                            }
                            if (objMatcode.Trim() == objPrintedMatcode.Trim())
                            {
                                if (sMatLotSize > sMatLotPrintedQty)
                                {
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

                                    sMatPrintCount++;
                                    string PrintStatus = objclsPrint.PrintQRCodeItem(objLocationCode, objMatcode, objMatDesc, objMatGrade, objMatGroup, objMatGroupDesc, objMatThickness, objMatThicknessDesc, objMatSize, objQRCode, objMatStatus, oDateFormat, sPrintingSection, sLocationType, sMatPrintCount);
                                    isPrinted = true;
                                    if (PrintStatus == "SUCCESS")
                                    {
                                        objPrintedMatcode = objMatcode.Trim();
                                        objPrintedMatGradeDesc = objMatGradeDesc.Trim();
                                        objPrintedMatGroupDesc = objMatGroupDesc.Trim();
                                        objPrintedMatThickness = objMatThicknessDesc.Trim();
                                        objPrintedMatSize = objMatSize.Trim();
                                        obj_sendmoxa.SendMoxaIO(moxaIp, "1");
                                    }
                                    else
                                        obj_sendmoxa.SendMoxaIO(moxaIp, "0");
                                }
                                else if (sMatLotSize == sMatLotPrintedQty)
                                {
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

                                    string PrintStatus = objclsPrint.PrintStackQRCodeItem(objLocationCode, objMatcode, objStackQRCode, oDateFormat, sPrintingSection, sLocationType, objMatGradeDesc.Trim(), objMatGroupDesc.Trim(), objMatThicknessDesc.Trim(), objMatSize.Trim(), sMatLotSize.ToString());
                                    if (PrintStatus == "SUCCESSFULL")
                                    {
                                        sMatPrintCount = 0;
                                        oSAPPostCount = 0;
                                        sMatLotPrintedQty = 0;
                                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtMessage, "ProductionData", dtProdData.Rows.Count + " No of Records Posted In SAP Out of " + dtProdData.Rows.Count + " For Material Code - " + objMatcode + " At " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                                    }
                                }
                            }
                        }
                        else if (sMatLotSize == sMatLotPrintedQty)
                        {
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

                            string PrintStatus = objclsPrint.PrintStackQRCodeItem(objLocationCode, objMatcode, objStackQRCode, oDateFormat, sPrintingSection, sLocationType, objMatGradeDesc.Trim(), objMatGroupDesc.Trim(), objMatThicknessDesc.Trim(), objMatSize.Trim(), sMatLotSize.ToString());
                            if (PrintStatus == "SUCCESSFULL")
                            {
                                sMatPrintCount = 0;
                                oSAPPostCount = 0;
                                sMatLotPrintedQty = 0;
                                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtMessage, "ProductionData", dtProdData.Rows.Count + " No of Records Posted In SAP Out of " + dtProdData.Rows.Count + " For Material Code - " + objMatcode + " At " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                            }
                        }
                        #endregion

                        #region If Matcode is printing first time
                        if (objPrintedMatcode.Trim() == string.Empty || objPrintedMatcode.Trim() == "")
                        {
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

                            sMatPrintCount++;
                            string PrintStatus = objclsPrint.PrintQRCodeItem(objLocationCode, objMatcode, objMatDesc, objMatGrade, objMatGroup, objMatGroupDesc, objMatThickness, objMatThicknessDesc, objMatSize, objQRCode, objMatStatus, oDateFormat, sPrintingSection, sLocationType, sMatPrintCount);
                            isPrinted = true;
                            if (PrintStatus == "SUCCESS")
                            {
                                objPrintedMatcode = objMatcode.Trim();
                                objPrintedMatGradeDesc = objMatGradeDesc.Trim();
                                objPrintedMatGroupDesc = objMatGroupDesc.Trim();
                                objPrintedMatThickness = objMatThicknessDesc.Trim();
                                objPrintedMatSize = objMatSize.Trim();
                                obj_sendmoxa.SendMoxaIO(moxaIp, "1");
                            }
                            else
                                obj_sendmoxa.SendMoxaIO(moxaIp, "0");
                        }
                        #endregion

                        //else if (sMatLotSize == sMatLotPrintedQty)
                        //{
                        //    sStackRunningSerial = objclsPrint.GetStackRunningSerial(objLocationCode, oDateFormat, sPrintingSection, sLocationType);
                        //    if (sStackRunningSerial == string.Empty || sStackRunningSerial == "")
                        //        sStackRunningSerial = "0";
                        //    int objNextStackNo = Convert.ToInt32(sStackRunningSerial);
                        //    objNextStackNo = objNextStackNo + 1;
                        //    sStackRunningSerial = Convert.ToString(objNextStackNo);
                        //    if (sStackRunningSerial.Length == 4)
                        //        sStackRunningSerial = "0" + sStackRunningSerial.Trim();
                        //    if (sStackRunningSerial.Length == 3)
                        //        sStackRunningSerial = "00" + sStackRunningSerial.Trim();
                        //    if (sStackRunningSerial.Length == 2)
                        //        sStackRunningSerial = "000" + sStackRunningSerial.Trim();
                        //    if (sStackRunningSerial.Length == 1)
                        //        sStackRunningSerial = "0000" + sStackRunningSerial.Trim();

                        //    objStackQRCode = objLocationCode.Trim() + oDateFormat.Trim() + sStackRunningSerial;

                        //    string PrintStatus = objclsPrint.PrintStackQRCodeItem(objLocationCode, objMatcode, objStackQRCode, oDateFormat, sPrintingSection, sLocationType, objMatGradeDesc.Trim(), objMatGroupDesc.Trim(), objMatThicknessDesc.Trim(), objMatSize.Trim(), sMatLotSize.ToString());
                        //    if (PrintStatus == "SUCCESSFULL")
                        //    {
                        //        sMatPrintCount = 0;
                        //        oSAPPostCount = 0;
                        //        sMatLotPrintedQty = 0;
                        //        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtMessage, "ProductionData", dtProdData.Rows.Count + " No of Records Posted In SAP Out of " + dtProdData.Rows.Count + " For Material Code - " + objMatcode + " At " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                        //    }
                        //}
                    }

                    #endregion

                    #region If Lotsize is Zero
                    else if (sMatLotSize == 0)
                    {
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

                        sMatPrintCount++;
                        string PrintStatus = objclsPrint.PrintQRCodeItem(objLocationCode, objMatcode, objMatDesc, objMatGrade, objMatGroup, objMatGroupDesc, objMatThickness, objMatThicknessDesc, objMatSize, objQRCode, objMatStatus, oDateFormat, sPrintingSection, sLocationType, sMatPrintCount);
                        isPrinted = true;
                        if (PrintStatus == "SUCCESS")
                            obj_sendmoxa.SendMoxaIO(moxaIp, "1");
                        else
                            obj_sendmoxa.SendMoxaIO(moxaIp, "0");
                    }
                    #endregion

                }
                #endregion

                #endregion

            }
            catch (ThreadAbortException ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "RunProcess", ex.ExceptionState.ToString());
                MessageBox.Show(ex.ExceptionState.ToString());
            }
        }



        public void FtpUpload()
        {
            objclsPrint = new B_ClsPrint();
            int aHour = Properties.Settings.Default.ScheduledTimeHR;
            int aMinute = Properties.Settings.Default.ScheduledTimeMM;
            if (DateTime.Now.Hour == aHour && DateTime.Now.Minute == aMinute)
            {
                string path = ""; //Properties.Settings.Default.LocalFolderPath;
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
                    filename = "2010_DR_" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
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
                }
                else
                {
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
                    // lv.Items.Clear();
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
                MessageBox.Show(ex.Message);
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "_UpdateClientCount", ex.Message);
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
                _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG"); //
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
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GETANDROIDUSERRIGHTS - ResponceSentToAndroid => ", Response);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI2 = null;
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
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetDispatchDeliveryOrderDetails - ResponceSentToAndroid => ", Response);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI7 = null;
                        break;

                    case "GETSTACKQRCODEDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI567 = new B_LoadingOffloading();
                        DataTable objDT2 = ClsBI567.GetDTStackQRCodeDetails(Data[1], Data[2], Data[3], Data[4]);
                        if (!objDT2.Columns.Contains("ERROR"))
                        {
                            DataRow[] dtrow = objDT2.Select("STATUS =  'EQ' OR STATUS = 'EI'");
                            DataRow[] dtrow1 = objDT2.Select("STATUS =  'OK'");
                            if (dtrow.Length > 0)
                            {
                                Response = "GETSTACKQRCODEDETAILS ~ STACK ~ " + GlobalVariable.DtToString(objDT2);
                            }
                            else if (objDT2.Columns.Count > 0 && objDT2.Rows.Count > 0)
                            {
                                B_LoadingOffloading obj = new B_LoadingOffloading();
                                string data = obj.GetDeliveryOrderNumbersDetails(Data[1], Data[2]);
                                Response = data;
                                obj = null;
                            }
                        }
                        else
                            Response = "GETSTACKQRCODEDETAILS ~ ERROR ~ " + objDT2.Rows[0][0].ToString();//   Material of scanned stack : " + Data[3].ToString() + " are not available in scanned challan : " + Data[2].ToString() + ", Kindly scan valid stack";
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetDispatchStackQRCodeDetails - ResponceSentToAndroid => ", Response);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI567 = null;
                        break;

                    case "GETLOADOFFLOADINITQUANTITY":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI55 = new B_LoadingOffloading();
                        DataTable objDT21 = ClsBI55.GetDTStackQRCodeInitData(Data[1], Data[2], Data[3], "", Data[4].ToString());
                        Response = "GETLOADOFFLOADINITQUANTITY ~ SUCCESS ~ " + GlobalVariable.DtToString(objDT21);
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetDispatchLoadOffloadInitQty - ResponceSentToAndroid => ", Response);
                        //GETLOADOFFLOADINITQUANTITY ~3020 ~2300001557 ~302004062100001 ~Offload
                        break;

                    case "UPDATELOADOFFLOADSCANNEDQRCODESTATUS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI56 = new B_LoadingOffloading();
                        DataTable objDT22 = ClsBI56.UPDATEDTScannedQRCodeData(Data[1], Data[2], Data[3], Data[4].ToString(), Data[5].ToString(), Data[6].ToString());
                        if (!(objDT22.Columns.Contains("ERROR") || objDT22.Columns.Contains("ErrorMessage")) && objDT22.Rows.Count > 0)
                            Response = "UPDATELOADOFFLOADSCANNEDQRCODESTATUS ~ SUCCESS ~ " + GlobalVariable.DtToString(objDT22);
                        if (!(objDT22.Columns.Contains("ERROR") || objDT22.Columns.Contains("ErrorMessage")) && objDT22.Rows.Count == 0)
                            Response = "UPDATELOADOFFLOADSCANNEDQRCODESTATUS ~ COMPLETE";
                        else if (objDT22.Columns.Contains("ERROR") || objDT22.Columns.Contains("ErrorMessage"))
                            Response = "UPDATELOADOFFLOADSCANNEDQRCODESTATUS ~ ERROR ~ " + GlobalVariable.DtToString(objDT22);
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateLoadOffloadScannedQRCodeStatus - ResponceSentToAndroid => ", Response);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        break;

                    case "PRINTLOADOFFLOADSTACKQRCODE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI66 = new B_LoadingOffloading();
                        DataTable odtScannedData3 = new DataTable();
                        odtScannedData3 = ClsBI66.GetOffloadPrintStackData(Data[1], Data[2], Data[3], Data[4].ToString(), Data[5].ToString());
                        if (odtScannedData3.Rows.Count == 0)
                        {
                            odtScannedData3 = ClsBI66.GetOffloadPrintStackData(Data[1], Data[2], Data[3], Data[4].ToString(), Data[5].ToString());
                        }
                        if (odtScannedData3.Rows.Count > 0)
                            odtScannedData3.AcceptChanges();
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintLoadOffloadStackQRCode - ResponceSentToAndroid => ", odtScannedData3.Rows.Count + " No. of Records received to print Stack QRCode");
                        Response = PrintLoadOffloadStackQRCode(odtScannedData3, Data[4].ToString(), Data[5].ToString());
                        if (Response.Contains("SUCCESS"))
                        {
                            Response = "PRINTLOADOFFLOADSTACKQRCODE ~ SUCCESS ~ Records saved and StackQrcode - " + objNewStackQRCode + " printed successfully";
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintLoadOffloadStackQRCode - ResponceSentToAndroid => ", " Records Saved and StackQrcode - " + objNewStackQRCode + " Printed Successfully");
                        }
                        else //if (Response.Contains("PRINTER NOT IN NETWORK"))
                        {
                            Response = "PRINTLOADOFFLOADSTACKQRCODE ~ ERROR ~ Error in Printing of New Stack QRCode is " + Response.ToString() + ", Kindly check network and Try again";
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintLoadOffloadStackQRCode - ResponceSentToAndroid => Responce : ", Response.ToString());
                        }
                        //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintLoadOffloadStackQRCode - ResponceSentToAndroid => Responce : ", Response);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI66 = null;
                        break;

                    case "CLEARLOADOFFLOADTEMPTABLE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI10 = new B_StackPrinting();
                        B_LoadingOffloading ClsBI57 = new B_LoadingOffloading();
                        DataTable objDT23 = ClsBI57.ClearLoadOffloadTempData(Data[1], Data[2], Data[3], "");
                        Response = "CLEARLOADOFFLOADTEMPTABLE ~ SUCCESS";
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ClearLoadOffloadTempTable - ResponceSentToAndroid : Responce : ", Response);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI57 = null;//
                        break;

                    #endregion

                    #region Offload Against Delivery

                    case "GETDELIVERYORDERDETAILSAGAINSTOFFLOAD":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI15 = new B_LoadingOffloading();
                        Response = ClsBI15.GetDODetailsAgainstDelivery(Data[1], Data[2]);
                        DONo = string.Empty;
                        DONo = Data[2].ToString();
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "DODetailsAgainstMatOffload - ResponceSentToAndroid => ", Response);
                        ClsBI15 = null;
                        break;

                    case "SAVEUPDATEDAMAGEDLOADOFFLOADDATA":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI16 = new B_LoadingOffloading();
                        Response = ClsBI16.SaveUpdateDamagedQRCode(Data[1].ToString(), Data[2].ToString(), Data[3].ToString(), Data[4].ToString());
                        Response = "SAVEUPDATEDAMAGEDLOADOFFLOADDATA ~ " + Response;
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "OffloadAgainstDelivery - ResponceSentToAndroid => ", Response);
                        ClsBI16 = null;
                        break;

                    #endregion

                    #region Depot Dispatch

                    case "GETDEPOTDELIVERYORDERDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI17 = new B_LoadingOffloading();
                        Response = ClsBI17.GetDeliveryOrderNumbersDetails(Data[1], Data[2]);
                        DONo = string.Empty;
                        DONo = Data[2].Trim().ToString();
                        if (dtDepotData.Rows.Count > 0)
                            dtDepotData.Rows.Clear();
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "DepotDispatchDODetails - ResponceSentToAndroid => ", Response);
                        ClsBI17 = null;
                        break;

                    case "GETDEPOTQRCODEDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI18 = new B_LoadingOffloading();
                        Response = ClsBI18.GetQRCodeDetails(Data[1], Data[2]);
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
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "DepotDispatchScannedQRCodeDetails - ResponceSentToAndroid => ", Response);
                        ClsBI18 = null;
                        break;

                    case "CLEARDEPOTDATATABLE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI19 = new B_StackPrinting();
                        if (dtDepotData.Rows.Count > 0)
                            dtDepotData.Rows.Clear();
                        Response = "CLEARLOADOFFLOADTABLE ~ SUCCESS";
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "DepotDispatchClearTable - ResponceSentToAndroid => ", Response);
                        ClsBI19 = null;
                        break;

                    case "SAVEDEPOTDISPATCHDATA":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI20 = new B_LoadingOffloading();
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
                                lSaveStatus = ClsBI20.SaveLoadOffloadQRCodeData(mLocCode, mDeliveryNo, mMatcode, mQRCode, mStackQRCode, sMUserId.Trim().ToString());
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
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "DepotDispatchSaveScannedData - ResponceSentToAndroid => ", Response);
                        ClsBI20 = null;
                        break;

                    #endregion

                    #region Delivery Cancellation

                    case "GETDELIVERYCANCELLEDDODETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI151 = new B_LoadingOffloading();
                        Response = ClsBI151.GetDeliveryCancellationDODetails(Data[1], Data[2], Data[3].ToString());
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI151 = null;
                        break;

                    case "UPDATEDELIVERYCANCELLEDDOSTATUS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI166 = new B_LoadingOffloading();
                        int oCSaveCount = 0;
                        string oCUserId = Data[3].ToString();
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateDeliveryCancellationDODetails - RequestDataFromAndroid => ", "LocationCode : " + Data[1].ToString() + ", DONo : " + Data[2].ToString() + ", UserId : " + Data[3].ToString()); ;
                        DataTable odtScannedData7 = new DataTable();
                        if (dtDLoadOffData.Rows.Count > 0)
                        {
                            odtScannedData7 = dtDLoadOffData.Clone();
                            DataRow[] rows1;
                            rows1 = dtDLoadOffData.Select("LocationCode =  '" + Data[1].ToString() + "' AND DeliveryOrderNo = '" + Data[2].ToString() + "'");
                            foreach (DataRow row in rows1)
                            {
                                odtScannedData7.ImportRow(row);
                            }
                            odtScannedData7.AcceptChanges();
                            foreach (DataRow dr in odtScannedData7.Rows)
                            {
                                string sLocCode = dr["LocationCode"].ToString().Trim();
                                string sDelNo = dr["DeliveryOrderNo"].ToString().Trim();
                                string sMatcode = dr["MatCode"].ToString().Trim();
                                string sQRCode = dr["QRCode"].ToString().Trim();
                                string UpdateStatus = ClsBI166.UpdateDeliveryCancellationDODetails(sLocCode, sDelNo, sMatcode, sQRCode, oCUserId);
                                if (UpdateStatus.Contains("SUCCESS"))
                                    oCSaveCount++;
                            }
                        }
                        if (oCSaveCount == 0)
                        {
                            Response = "UPDATEDELIVERYCANCELLEDDOSTATUS ~ ERROR ~ Delivery Order No. - " + Data[2].ToString() + " is not Cancelled, Kindly Try Again";
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateDeliveryCancellationDODetails - ResponceDataToAndroid => ", "Responce : " + Response.ToString()); ;
                        }
                        if (oCSaveCount == odtScannedData7.Rows.Count)
                        {
                            if (dtDLoadOffData.Rows.Count > 0)
                                dtDLoadOffData.Rows.Clear();
                            Response = "UPDATEDELIVERYCANCELLEDDOSTATUS ~ SUCCESS ~ Delivery Order No. - " + Data[2].ToString() + " is Cancelled Successfully";
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateDeliveryCancellationDODetails - ResponceDataToAndroid => ", "Responce : " + Response.ToString()); ;
                        }
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI16 = null;
                        break;

                    case "CLEARCANCELLEDLOADOFFLOADTABLE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        if (dtDLoadOffData != null)
                        {
                            if (dtDLoadOffData.Columns.Count > 0)
                            {
                                if (dtDLoadOffData.Rows.Count > 0)
                                    dtDLoadOffData.Rows.Clear();
                            }
                        }
                        Response = "CLEARCANCELLEDLOADOFFLOADTABLE ~ SUCCESS";
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI10 = null;
                        break;

                    #endregion

                    #region Segregation Qrcode Label Scanning Against Cancelled Delivery

                    case "CHECKCANCELLEDDELIVERYSCANNEDQRCODEDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI244 = new B_StackPrinting();
                        string objQRCode = Data[2].Substring(0, 17);
                        Response = ClsBI244.GetCancelledDeliverySegregationScannedQRCodeDetails(Data[1].Trim(), objQRCode.Trim().ToString(), Data[3].Trim());
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "CancelledDeliverySegregationScannedQRCodeDetails - ResponceSentToAndroid => ", Response);
                        ClsBI244 = null;
                        break;

                    #endregion

                    #region Segregation Stack Label Printing

                    case "CHECKSCANNEDQRCODEDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI21 = new B_StackPrinting();
                        string QRCode = Data[2].Substring(0, 17);
                        Response = ClsBI21.GetScannedSegregationQRCodeDetails(Data[1], QRCode, Data[3]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI21 = null;
                        break;

                    case "PRINTSEGREGATIONSTACKQRCODE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI22 = new B_StackPrinting();
                        Response = PrintSegregationStackQRCode(Data[1], Data[2]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI22 = null;
                        break;

                    case "PRINTSEGREGATIONCLEARTABLE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI23 = new B_StackPrinting();
                        dtStackData.Rows.Clear();
                        Response = "PRINTSEGREGATIONCLEARTABLE ~ SUCCESS";
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI23 = null;
                        break;

                    #endregion

                    #region Segregation Qrcode Label Scanning

                    case "CHECKSEGREGATIONSCANNEDQRCODEDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI24 = new B_StackPrinting();
                        int sLength = Data[2].Length;
                        if (sLength > 17 || sLength < 17)
                        {
                            Response = "CHECKSEGREGATIONSCANNEDQRCODEDETAILS ~ ERROR ~ Scanned QRCode - " + Data[2].ToString() + " not valid, Kindly scan valid QRCode";
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSegregationScannedQRCodeDetails - ResponceSentToAndroid => ", Response);
                            _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                            ClsBI24 = null;
                            break;
                        }
                        string objsQRCode = Data[2].Substring(0, 17);
                        Response = ClsBI24.GetSegregationScannedQRCodeDetails(Data[1].Trim(), objsQRCode.Trim().ToString(), Data[3].Trim());
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSegregationScannedQRCodeDetails - ResponceSentToAndroid => ", Response);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI24 = null;
                        break;

                    #endregion

                    #region Sales Return

                    case "GETSALESRETURNNUMBERDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_SalesReturn ClsBI25 = new B_SalesReturn();
                        Response = ClsBI25.GetSalesReturnNumberDetails(Data[1], Data[2]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI25 = null;
                        break;

                    case "GETSAVESALESRETURNQRCODEDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_SalesReturn ClsBI27 = new B_SalesReturn();
                        Response = ClsBI27.GetSalesReturnQRCodeDetails(Data[1], Data[2], Data[3], Data[4]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI27 = null;
                        break;

                    #region Not in Use As of Now

                    case "GETSALESRETURNSTATUS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_SalesReturn ClsBI26 = new B_SalesReturn();
                        Response = ClsBI26.GetSalesReturnScannnedStatus(Data[1], Data[2], Data[3]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI26 = null;
                        break;

                    case "CLEARSALESRETURNTABLE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI28 = new B_StackPrinting();
                        dtScannedData.Rows.Clear();
                        Response = "CLEARSALESRETURNTABLE ~ SUCCESS";
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI28 = null;
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
                        B_SalesReturn ClsBI30 = new B_SalesReturn();
                        Response = "UPDATESALESRETURNSAPSTATUS ~ SUCCESS ~ " + dtScannedData.Rows.Count + " - No of Records Posted Out Of " + dtScannedData.Rows.Count + " To SAP Successfully";
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI30 = null;
                        break;

                    #endregion


                    #endregion

                    #region Purchase Return

                    case "GETPURCHASERETURNNUMBERDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_SalesReturn ClsBI225 = new B_SalesReturn();
                        Response = ClsBI225.GetPurchaseReturnNumberDetails(Data[1], Data[2]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI225 = null;
                        break;

                    case "GETSAVEPURCHASERETURNQRCODEDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_SalesReturn ClsBI277 = new B_SalesReturn();
                        Response = ClsBI277.GetPurchaseReturnQRCodeDetails(Data[1].Trim().ToString(), Data[2].Trim().ToString(), Data[3].Trim().ToString(), Data[4].Trim().ToString(), Data[5].Trim().ToString());
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI277 = null;
                        break;

                    #region Not in Use
                    case "GETPURCHASERETURNSTATUS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_SalesReturn ClsBI226 = new B_SalesReturn();
                        Response = ClsBI226.GetPurchaseReturnScannnedStatus(Data[1], Data[2], Data[3]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI226 = null;
                        break;

                    case "GETPURCHASERETURNQRCODEDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_SalesReturn ClsBI227 = new B_SalesReturn();
                        Response = "";// ClsBI227.GetPurchaseReturnQRCodeDetails(Data[1], Data[2], Data[3], Data[4], Convert.ToInt32(1), Data[5]);
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
                        ClsBI227 = null;
                        break;

                    case "CLEARPURCHASERETURNTABLE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI228 = new B_StackPrinting();
                        dtScannedData.Rows.Clear();
                        Response = "CLEARSALESRETURNTABLE ~ SUCCESS";
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI228 = null;
                        break;

                    case "PRINTPURCHASERETURNQRCODE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_SalesReturn ClsBI229 = new B_SalesReturn();
                        Response = PrintSalesReturnQRCode(Data[1], Data[2]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI229 = null;
                        break;

                    case "UPDATEPURCHASERETURNSAPSTATUS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_SalesReturn ClsBI310 = new B_SalesReturn();
                        Response = "UPDATESALESRETURNSAPSTATUS ~ SUCCESS ~ " + dtScannedData.Rows.Count + " - No of Records Posted Out Of " + dtScannedData.Rows.Count + " To SAP Successfully";
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI310 = null;
                        break;
                    #endregion

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

                    #region MTM Transfer
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
                        B_M2MTransfer ClsBI667 = new B_M2MTransfer();
                        Response = ClsBI667.GetM2MMatDesignNo(Data[1], Data[2], Data[3], Data[4], Data[5], Data[6]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI667 = null;
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
                                    string UpdateStatus = "";// ClsBI72.SaveUpdateDamagedQRCode(mLocCode, mNewMatcode, mOldMatcode, mOldQRCode, mUserId);
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
                        ClsBI72 = null;
                        break;

                    #endregion

                    #region Material Scanned and Stack Printed

                    case "MTMTRANSFERCHECKSCANNEDSTACKQRCODEDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI890 = new B_StackPrinting();
                        Response = ClsBI890.MTMScannedStackQRCodeDetails(Data[1], Data[2]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI890 = null;
                        break;

                    #endregion

                    #endregion

                    #region Dispatch Depot And Doors

                    case "GETDISPATCHDELIVERYORDERDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI77 = new B_LoadingOffloading();
                        Response = ClsBI77.GetDispatchDeliveryOrderNoDetails(Data[1], Data[2]);
                        oDispatchDONo = string.Empty;
                        oDispatchDONo = Data[2].Trim().ToString();
                        if (dtDispatchData.Rows.Count > 0)
                            dtDispatchData.Rows.Clear();
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI77 = null;
                        break;

                    case "GETSAVEDISPATCHQRCODEDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI99 = new B_LoadingOffloading();
                        if (dtDispatchData.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtDispatchData.Rows)
                            {
                                if (row["QRCode"].ToString() == Data[2].ToString())
                                {
                                    Response = "GETSAVEDISPATCHQRCODEDETAILS ~ ERROR ~ QRCode is Already Scanned, Kindly Scan Another QRCode";
                                    return;
                                }
                            }
                        }
                        Response = ClsBI99.GetSaveDispatchQRCodeDetails(Data[1], Data[2], Data[3], Data[4]);
                        if (Response.Contains("SUCCESS"))
                        {
                            if (dtDispatchData.Columns.Count > 0)
                            {
                                DataRow _dtRow = dtDispatchData.NewRow();
                                _dtRow["LocationCode"] = Data[1].ToString();
                                _dtRow["DONo"] = Data[3].ToString();
                                _dtRow["MatCode"] = B_LoadingOffloading.oMatCode.ToString();
                                _dtRow["QRCode"] = Data[2].ToString();
                                dtDispatchData.Rows.Add(_dtRow);
                            }
                        }
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI99 = null;
                        break;

                    case "CLEARDISPATCHDELIVERYORDERTABLE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        if (dtDispatchData.Rows.Count > 0)
                            dtDispatchData.Rows.Clear();
                        Response = "CLEARDISPATCHDELIVERYORDERTABLE ~ SUCCESS";
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        break;

                    #endregion

                    #region Vendor Dispatch

                    case "GETVENDORPODETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI78 = new B_LoadingOffloading();
                        Response = ClsBI78.GetVendorPODetails(Data[1], Data[2]);
                        oDispatchDONo = string.Empty;
                        oDispatchDONo = Data[2].Trim().ToString();
                        if (dtDispatchData.Rows.Count > 0)
                            dtDispatchData.Rows.Clear();
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI78 = null;
                        break;

                    case "GETSAVEVENDORQRCODEDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_LoadingOffloading ClsBI100 = new B_LoadingOffloading();
                        if (dtDispatchData.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtDispatchData.Rows)
                            {
                                if (row["QRCode"].ToString() == Data[2].ToString())
                                {
                                    Response = "GETSAVEDISPATCHQRCODEDETAILS ~ ERROR ~ QRCode is Already Scanned, Kindly Scan Another QRCode";
                                    return;
                                }
                            }
                        }
                        Response = ClsBI100.GetSaveDispatchQRCodeDetails(Data[1], Data[2], Data[3], Data[4]);
                        if (Response.Contains("SUCCESS"))
                        {
                            if (dtDispatchData.Columns.Count > 0)
                            {
                                DataRow _dtRow = dtDispatchData.NewRow();
                                _dtRow["LocationCode"] = Data[1].ToString();
                                _dtRow["DONo"] = Data[3].ToString();
                                _dtRow["MatCode"] = B_LoadingOffloading.oMatCode.ToString();
                                _dtRow["QRCode"] = Data[2].ToString();
                                dtDispatchData.Rows.Add(_dtRow);
                            }
                        }
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI100 = null;
                        break;

                    case "CLEARVENDORPOORDERTABLE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        if (dtDispatchData.Rows.Count > 0)
                            dtDispatchData.Rows.Clear();
                        Response = "CLEARDISPATCHDELIVERYORDERTABLE ~ SUCCESS";
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        break;

                    #endregion

                    #region Stack Merging

                    case "GETSTACKMERGINGSCANNEDSTACKDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI88 = new B_StackPrinting();
                        if (dtStackMergeData.Rows.Count > 0)
                        {
                            foreach (DataRow row in dtStackMergeData.Rows)
                            {
                                if (row["StackQRCode"].ToString() == Data[2].ToString())
                                {
                                    Response = "GETSTACKMERGINGSCANNEDSTACKDETAILS ~ ERROR ~ Stack QRCode is Already Scanned, Kindly Scan Another Stack";
                                    return;
                                }
                            }
                        }
                        Response = ClsBI88.GetStackMergeStackQRCodeDetails(Data[1], Data[2], Data[3]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI88 = null;
                        break;

                    case "PRINTSTACKMERGINGSTACKQRCODE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        Response = PrintStackMergingStackQRCode(Data[1], Data[2]);
                        if (Response.Contains("SUCCESS"))
                            Response = "PRINTSTACKMERGINGSTACKQRCODE ~ SUCCESS ~ " + "Scanned stacks are merged into StackQRCode - " + objStackQRCode + " successfully";
                        else if (Response.Contains("SUCCESS"))
                            Response = "PRINTSTACKMERGINGSTACKQRCODE ~ " + Response.ToString();
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ResponceSentToAndroid - StackMerge_UpdateScannedQRCode => ", "Responce - " + Response);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        break;

                    case "CLEARSTACKMERGINGTABLE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        if (dtStackMergeData.Rows.Count > 0)
                            dtStackMergeData.Rows.Clear();
                        Response = "CLEARSTACKMERGINGTABLE ~ SUCCESS";
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI28 = null;
                        break;

                    #endregion

                    #region Stack Splitting

                    case "GETSTACKSPLITSCANNEDSTACKDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI888 = new B_StackPrinting();
                        if (dtStackSplitData.Rows.Count > 0)
                            dtStackSplitData.Rows.Clear();
                        Response = ClsBI888.GetStackSplitStackQRCodeDetails(Data[1], Data[2], Data[3]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI888 = null;
                        break;

                    case "UPDATESTACKSPLITSCANNEDQRCODESTATUS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "RequestDataFromAndroid - StackSplit_UpdateScannedQRCode =>", "LocationCode - " + Data[1] + ", QRCode - " + Data[2]);
                        B_StackPrinting ClsBI891 = new B_StackPrinting();
                        string sMatCode = string.Empty;
                        foreach (DataRow dr in dtStackSplitData.Rows)
                        {
                            if (dr[6].ToString() == Data[2].ToString() && dr[8].ToString() == "N")
                            {
                                dr[8] = "Y";
                                sMatCode = dr[1].ToString();
                                //sQty = Convert.ToInt32(dr[9].ToString());
                                //sQty = sQty - 1;
                                //dr[9] = Convert.ToString(sQty);
                                Response = "UPDATESTACKSPLITSCANNEDQRCODESTATUS ~ SUCCESS ~ " + sMatCode.ToString().Trim();  //+ " $ 0 $ "
                                dtStackSplitData.AcceptChanges();
                                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ResponceSentToAndroid - StackSplit_UpdateScannedQRCode =>", "Responce - " + Response);
                                _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                                ClsBI891 = null;
                                return;
                            }
                            else if (dr[6].ToString() == Data[2].ToString() && dr[8].ToString() == "Y")
                            {
                                Response = "UPDATESTACKSPLITSCANNEDQRCODESTATUS ~ ERROR ~ QRCode - " + Data[2].ToString() + " is Already Scanned, Kindly Scan Another";
                                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ResponceSentToAndroid - StackSplit_UpdateScannedQRCode =>", "Responce - " + Response);
                                _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                                ClsBI891 = null;
                                return;
                            }
                            else if (dr[6].ToString() != Data[2].ToString())
                            {
                                Response = "UPDATESTACKSPLITSCANNEDQRCODESTATUS ~ ERROR ~ QRCode - " + Data[2].ToString() + " Not Exists in Scanned Stack, Kindly Scan Another";
                                //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ResponceSentToAndroid - StackSplit_UpdateScannedQRCode =>", "Responce - " + Response);
                                //_UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                                //ClsBI891 = null;
                                //return;
                            }
                            else
                            {
                                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "StackSplit_UpdateScannedQRCode =>", "Datarow 0 - " + dr[0].ToString() + "Datarow 0 - " + dr[0].ToString() + "Datarow 1 - " + dr[1].ToString() + "Datarow 2 - " + dr[2].ToString() + "Datarow 3 - " + dr[3].ToString() + "Datarow 4 - " + dr[4].ToString() + "Datarow 5 - " + dr[5].ToString() + "Datarow 6 - " + dr[6].ToString() + "Datarow 7 - " + dr[7].ToString() + "Datarow 8 - " + dr[8].ToString());
                                Response = "UPDATESTACKSPLITSCANNEDQRCODESTATUS ~ ERROR ~ QRCode - " + Data[2].ToString() + " Not valid or some scanning issue, Kindly try again";
                            }
                        }
                        dtStackSplitData.AcceptChanges();
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ResponceSentToAndroid - StackSplit_UpdateScannedQRCode =>", "Responce - " + Response);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI891 = null;
                        break;

                    case "PRINTSTACKSPLITTINGSTACKQRCODE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");

                        odtSplitData = new DataTable();
                        odtSplitData = dtStackSplitData.Clone();
                        DataRow[] row4;
                        row4 = dtStackSplitData.Select("ScannedStatus = 'Y'");
                        foreach (DataRow row in row4)
                        {
                            odtSplitData.ImportRow(row);
                            dtStackSplitData.Rows.Remove(row);
                            dtStackSplitData.AcceptChanges();
                            odtSplitData.AcceptChanges();
                        }
                        Response = PrintStackSplittingStackQRCode(Data[1], Data[2]);
                        if (Response.Contains("SUCCESS"))
                        {
                            Response = "PRINTSTACKSPLITTINGSTACKQRCODE ~ SUCCESS ~ Scanned Stack is splitted in Stack - " + Data[3].ToString() + " and Stack - " + objStackQRCode + " successfully";
                            if (dtStackSplitData.Rows.Count > 0)
                                dtStackSplitData.Rows.Clear();
                        }
                        else
                        {
                            Response = "PRINTSTACKSPLITTINGSTACKQRCODE ~ ERROR ~ " + Response.ToString() + "";
                        }
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ResponceSentToAndroid - StackSplit_PrintNewStacks => ", "Responce - " + Response);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        break;

                    case "CLEARSTACKSPLITDATATABLE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        if (dtStackSplitData.Rows.Count > 0)
                            dtStackSplitData.Rows.Clear();
                        Response = "CLEARSTACKSPLITDATATABLE ~ SUCCESS";
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI10 = null;
                        break;

                    #endregion

                    #region Quality Inspection at HUB

                    // QCStatus = 0 Means Just Printed
                    // QCStatus = 1 Means Rejected
                    // QCStatus = 2 Means OK
                    // QCStatus = 3 Means BoiledTest

                    case "GETQASCANNEDPODETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_ClsPrint ClsBI788 = new B_ClsPrint();
                        Response = ClsBI788.GetQAScannedPODetails(Data[1].Trim().ToString(), Data[2].Trim().ToString(), Data[3].Trim().ToString());
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI788 = null;
                        break;

                    case "GETQAINSPLOTMATDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_ClsPrint ClsBI789 = new B_ClsPrint();
                        Response = ClsBI789.GetQASelectedPOInspMatDetails(Data[1].Trim().ToString(), Data[2].Trim().ToString(), Data[3].Trim().ToString(), Data[4].Trim().ToString());
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI789 = null;
                        break;

                    case "UPDATEQAINSPLOTMATREJECTED":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_ClsPrint ClsBI790 = new B_ClsPrint();
                        Response = ClsBI790.UpdatedQAPOInspLotMatRejectedStatus(Data[1].Trim().ToString(), Data[2].Trim().ToString(), Data[3].Trim().ToString(), Data[4].Trim().ToString(), Data[5].Trim().ToString());
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI790 = null;
                        break;

                    case "UPDATEQAINSPLOTMATACCEPTED":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_ClsPrint ClsBI794 = new B_ClsPrint();
                        Response = ClsBI794.UpdatedQAPOInspLotMatAcceptedStatus(Data[1].Trim().ToString(), Data[2].Trim().ToString(), Data[3].Trim().ToString(), Data[4].Trim().ToString(), Data[5].Trim().ToString());
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI794 = null;
                        break;

                    case "GETQASELECTEDMATDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_ClsPrint ClsBI791 = new B_ClsPrint();
                        Response = ClsBI791.GetQASelectedMatDetails(Data[1].Trim().ToString());
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI791 = null;
                        break;

                    case "INSERTQABOILEDTESTQRCODESTATUS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_ClsPrint ClsBI792 = new B_ClsPrint();
                        Response = ClsBI792.UpdateBoilingScannedStatus(Data[1].Trim().ToString(), Data[2].Trim().ToString(), Data[3].Trim().ToString(), Data[4].Trim().ToString(), Data[5].Trim().ToString(), Data[6].Trim().ToString(), Data[7].Trim().ToString());
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI792 = null;
                        break;

                    case "UPDATEQAINSPLOTBOILTESTREJECTED":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_ClsPrint ClsBI892 = new B_ClsPrint();
                        Response = ClsBI892.UpdatedQAPOInspLotBoilTestRejected(Data[1].Trim().ToString(), Data[2].Trim().ToString(), Data[3].Trim().ToString(), Data[4].Trim().ToString());
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI892 = null;
                        break;

                    case "UPDATEQAPARTIALREJECTIONQRCODESTATUS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_ClsPrint ClsBI793 = new B_ClsPrint();
                        Response = ClsBI793.UpdateQAPartialRejectionQRCodeStatus(Data[1].Trim().ToString(), Data[2].Trim().ToString(), Data[3].Trim().ToString(), Data[4].Trim().ToString(), Data[5].Trim().ToString(), Data[6].Trim().ToString(), Data[7].Trim().ToString());
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI793 = null;
                        break;

                    #region Partial Rejection

                    case "GETQAREJECTIONCODE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_ClsPrint ClsBI894 = new B_ClsPrint();
                        Response = ClsBI894.GetQARejectionCode(Data[1].Trim().ToString(), Data[2].Trim().ToString(), Data[3].Trim().ToString());
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI894 = null;
                        break;

                    case "UPDATEQAPARTIALREJECTIONSCANNEDQRCODEDETAIL":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_ClsPrint ClsBI795 = new B_ClsPrint();
                        Response = ClsBI795.UpdatedQAPartialRejectionScannedQRCodeStatus(Data[1].Trim().ToString(), Data[2].Trim().ToString(), Data[3].Trim().ToString(), Data[4].Trim().ToString(), Data[5].Trim().ToString(), Data[6].Trim().ToString(), Data[7].Trim().ToString());
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI795 = null;
                        break;

                    case "UPDATEQAINSPECTIONLOT":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_ClsPrint ClsBI796 = new B_ClsPrint();
                        Response = ClsBI796.UpdatedQAInspectionLotStatus(Data[1].Trim().ToString(), Data[2].Trim().ToString(), Data[3].Trim().ToString(), Data[4].Trim().ToString(), Data[5].Trim().ToString());
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI795 = null;
                        break;

                    #endregion

                    #endregion

                    #region MTM For Rejected Material

                    case "GETQAMTMSELECTEDMATDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_ClsPrint ClsBI995 = new B_ClsPrint();
                        Response = ClsBI995.GetQAMTMSelectedMatDetails(Data[1].Trim().ToString());
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI791 = null;
                        break;

                    case "UPDATEMTMREJECTEDQRCODESCANNEDSTATUS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_ClsPrint ClsBI797 = new B_ClsPrint();
                        //Response = ClsBI797.UpdateQAMTMRejectedScannedQRCodeStatus(Data[1].Trim().ToString(), Data[2].Trim().ToString(), Data[3].Trim().ToString());
                        Response = ClsBI797.UpdateQAMTMRejectedScannedQRCodeStatus(Data[1].Trim().ToString(), Data[2].Trim().ToString(), Data[3].Trim().ToString(), Data[4].Trim().ToString());
                        //Response = ClsBI797.UpdateBoilingScannedStatus(Data[1].Trim().ToString(), Data[2].Trim().ToString(), Data[3].Trim().ToString(), Data[4].Trim().ToString(), Data[5].Trim().ToString(), Data[6].Trim().ToString(), Data[7].Trim().ToString());
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI797 = null;
                        break;

                    #endregion

                    #region QRCode Production At Hub

                    case "GETHUBSCANNEDQRCODEDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_ClsPrint ClsBI889 = new B_ClsPrint();
                        //sHubSelectedMatCode = string.Empty;
                        DataTable objMatData = new DataTable();
                        if (sHubSelectedMatCode == string.Empty)
                        {
                            Response = ClsBI889.GetHUBSelectedMatCode();
                        }
                        if (sHubSelectedMatCode != string.Empty) // (Response.Contains("SUCCESS"))
                        {
                            objMatData = ClsBI889.GetSelectedMatCode(objLocationCode);
                            //string StackStatus = objclsPrint.GetIsStackPrintStatus(objLocationCode);
                            if (objMatData.Rows.Count == 1)
                            {
                                if (sHubSelectedMatCode.Trim().ToString() != objMatData.Rows[0][0].ToString().Trim())
                                {
                                    Response = "Selected MaterialCode is - " + objMatData.Rows[0][0].ToString() + " and you are scanning of MaterialCode - " + sHubSelectedMatCode + ", You have to complete the stack or Change the selected material";
                                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "HubScannedQRCodeDetails : Responce => ", Response);
                                    Response = "GETHUBSCANNEDQRCODEDETAILS ~ ERROR ~ " + Response;
                                    _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                                    ClsBI889 = null;
                                    return;
                                }
                                objMatcode = objMatData.Rows[0][0].ToString();
                                objMatDesc = objMatData.Rows[0][1].ToString();
                                objMatGrade = objMatData.Rows[0][2].ToString();
                                objMatGradeDesc = objMatData.Rows[0][3].ToString();
                                objMatGroup = objMatData.Rows[0][4].ToString();
                                objMatGroupDesc = objMatData.Rows[0][5].ToString();
                                objMatThickness = objMatData.Rows[0][6].ToString();
                                objMatThicknessDesc = objMatData.Rows[0][7].ToString();
                                objMatSize = objMatData.Rows[0][8].ToString();
                            }
                            Response = ClsBI889.GetHUBScannedQRCodeDetails(Data[1], Data[2], Data[3], objMatcode, objMatGrade, objMatGroup, objMatGroupDesc, objMatThicknessDesc, objMatSize, sPrintingSection, sLocationType);
                            if (Response.Contains("SUCCESS"))
                            {
                                Response = "GETHUBSCANNEDQRCODEDETAILS ~ " + Response;
                            }
                            else if (Response.Contains("COMPLETE"))
                            {
                                Response = "GETHUBSCANNEDQRCODEDETAILS ~ " + Response;
                            }
                            else
                            {
                                Response = "GETHUBSCANNEDQRCODEDETAILS ~ " + Response;
                            }
                        }
                        else if (Response.Contains("ERROR") || Response.Contains("ErrorMessage"))
                        {
                            Response = "GETHUBSCANNEDQRCODEDETAILS ~ " + Response;
                        }
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI889 = null;
                        break;

                    case "CLEARTEMPHUBLABELPRINTING":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_ClsPrint ClsBI893 = new B_ClsPrint();
                        Response = ClsBI893.ClearTempHubLabelPrinting(Data[1].Trim().ToString(), Data[2].Trim().ToString());
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI893 = null;
                        break;

                    case "COMPLETEDSTACKQRCODE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_ClsPrint ClsBI895 = new B_ClsPrint();
                        DataTable objMatData1 = new DataTable();
                        if (sHubSelectedMatCode == string.Empty)
                        {
                            Response = ClsBI895.GetHUBSelectedMatCode();
                        }
                        if (sHubSelectedMatCode != string.Empty) //if (Response.Contains("SUCCESS"))
                        {
                            objMatData1 = ClsBI895.GetSelectedMatCode(objLocationCode);
                            if (objMatData1.Rows.Count == 1)
                            {
                                if (sHubSelectedMatCode.Trim().ToString() != objMatData1.Rows[0][0].ToString().Trim())
                                {
                                    Response = "Selected MaterialCode is - " + objMatData1.Rows[0][0].ToString() + " and you are completing the stack of MaterialCode - " + sHubSelectedMatCode + ", Change the selected material in PC application";
                                    Response = "GETHUBSCANNEDQRCODEDETAILS ~ ERROR ~ " + Response;
                                    _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                                    ClsBI889 = null;
                                    return;
                                }
                                objMatcode = objMatData1.Rows[0][0].ToString();
                                objMatDesc = objMatData1.Rows[0][1].ToString();
                                objMatGrade = objMatData1.Rows[0][2].ToString();
                                objMatGradeDesc = objMatData1.Rows[0][3].ToString();
                                objMatGroup = objMatData1.Rows[0][4].ToString();
                                objMatGroupDesc = objMatData1.Rows[0][5].ToString();
                                objMatThickness = objMatData1.Rows[0][6].ToString();
                                objMatThicknessDesc = objMatData1.Rows[0][7].ToString();
                                objMatSize = objMatData1.Rows[0][8].ToString();
                            }
                            Response = ClsBI895.CompletedStackQRCodeData(Data[1], Data[2], objMatcode, objMatGrade, objMatGroup, objMatGroupDesc, objMatThicknessDesc, objMatSize, sPrintingSection, sLocationType);
                            if (Response.Contains("COMPLETE"))
                            {
                                sHubSelectedMatCode = string.Empty;
                                Response = "COMPLETEDSTACKQRCODE ~ " + Response;
                            }
                            else
                            {
                                Response = "COMPLETEDSTACKQRCODE ~ ERROR ~ " + Response;
                            }
                        }
                        else if (Response.Contains("ERROR") || Response.Contains("ErrorMessage"))
                        {
                            Response = "COMPLETEDSTACKQRCODE ~ ERROR ~ " + Response;
                        }
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI889 = null;
                        break;

                    #endregion

                    #region Material Scanned But Stack Not Printed

                    case "GETDELETESCANNEDQRCODEDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI989 = new B_StackPrinting();
                        Response = ClsBI989.GetDeleteScannedQRCodeDetails(Data[1], Data[2], Data[3]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI989 = null;
                        break;

                    case "DELETESCANNEDQRCODESDATA":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI990 = new B_StackPrinting();
                        int sDCount = 0;
                        for (int i = 0; i < dtDeleteQRData.Rows.Count; i++)
                        {
                            string sLocCode = dtDeleteQRData.Rows[i][0].ToString();
                            string sMCode = dtDeleteQRData.Rows[i][1].ToString();
                            string sQRCode = dtDeleteQRData.Rows[i][2].ToString();
                            string sUserId = dtDeleteQRData.Rows[i][3].ToString();
                            Response = ClsBI990.DeleteScannedQRCodesData(sLocCode, sMCode, sQRCode, sUserId);
                            if (Response.Contains("SUCCESS"))
                                sDCount++;
                        }
                        if (sDCount > 0)
                            Response = "DELETESCANNEDQRCODESDATA ~ SUCCESS ~ " + sDCount + " QRCodes out of total No. of " + dtDeleteQRData.Rows.Count + " Deleted Successfully";
                        if (sDCount == 0)
                            Response = "DELETESCANNEDQRCODESDATA ~ ERROR ~ No QRCode Deleted, Kindly Try Again";
                        if (dtDeleteQRData.Rows.Count > 0)
                            dtDeleteQRData.Rows.Clear();
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI990 = null;
                        break;

                    case "CLEARDELETEQRCODESDATATABLE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        if (dtDeleteQRData.Rows.Count > 0)
                            dtDeleteQRData.Rows.Clear();
                        Response = "CLEARDELETEQRCODESDATATABLE ~ SUCCESS";
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI10 = null;
                        break;

                    #endregion

                    #region Material Scanned And Stack Printed

                    case "GETDELETESCANNEDSTACKDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI555 = new B_StackPrinting();
                        Response = ClsBI555.GetDeleteStackDetails(Data[1], Data[2], Data[3]);
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI555 = null;
                        break;

                    case "UPDATEDELETESTACKSCANNEDQRCODESTATUS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        foreach (DataRow dr in dtStackDeleteData.Rows)
                        {
                            if (dr[0].ToString() == Data[1].ToString() && dr[7].ToString() == Data[2].ToString() && dr[6].ToString() == Data[3].ToString()
                                && dr[8].ToString() == "N")
                            {
                                dr[8] = "Y";
                                Response = "UPDATEDELETESTACKSCANNEDQRCODESTATUS ~ SUCCESS ~ ";
                                dtStackDeleteData.AcceptChanges();
                                _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                                return;
                            }
                            else if (dr[0].ToString() == Data[1].ToString() && dr[7].ToString() == Data[2].ToString() && dr[6].ToString() == Data[3].ToString()
                                && dr[8].ToString() == "Y")
                            {
                                Response = "UPDATEDELETESTACKSCANNEDQRCODESTATUS ~ ERROR ~ QRCode - " + Data[2].ToString() + " is Already Scanned, Kindly Scan Another";
                                _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                                ClsBI891 = null;
                                return;
                            }
                            else if (dr[0].ToString() == Data[1].ToString() && dr[7].ToString() == Data[2].ToString() && dr[6].ToString() != Data[3].ToString())
                            {
                                Response = "UPDATEDELETESTACKSCANNEDQRCODESTATUS ~ ERROR ~ QRCode - " + Data[2].ToString() + " Not Exists in Scanned Stack, Kindly Scan Another";
                            }
                        }
                        dtStackDeleteData.AcceptChanges();
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        break;


                    case "DELETESCANNEDSTACKQRCODESDATA":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StackPrinting ClsBI991 = new B_StackPrinting();
                        int sDeleteCount = 0;
                        DataTable odtDeleteData = new DataTable();
                        odtDeleteData = dtStackDeleteData.Clone();
                        DataRow[] rows4;
                        rows4 = dtStackDeleteData.Select("ScannedStatus = 'Y'");
                        foreach (DataRow row in rows4)
                        {
                            odtDeleteData.ImportRow(row);
                            //dtStackDeleteData.Rows.Remove(row);
                            dtStackDeleteData.AcceptChanges();
                        }
                        dtStackDeleteData.AcceptChanges();
                        odtDeleteData.AcceptChanges();
                        if (odtDeleteData.Rows.Count > 0)
                        {
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "DELETESCANNEDSTACKQRCODESDATA => ", "Total No. of Records to Be Deleted - " + odtDeleteData.Rows.Count);
                            for (int i = 0; i < odtDeleteData.Rows.Count; i++)
                            {
                                string sLocCode = odtDeleteData.Rows[i][0].ToString();
                                string sMCode = odtDeleteData.Rows[i][1].ToString();
                                string sQRCode = odtDeleteData.Rows[i][6].ToString();
                                string sUserId = Data[3].ToString(); // odtDeleteData.Rows[i][3].ToString();
                                string sStackQRCode = odtDeleteData.Rows[i][7].ToString();
                                Response = ClsBI991.DeleteScannedQRCodes(sLocCode, sMCode, sQRCode, sStackQRCode, sUserId);
                                if (Response.Contains("SUCCESS"))
                                    sDeleteCount++;
                            }
                            if (sDeleteCount == odtDeleteData.Rows.Count)
                            {
                                string sLocCode = Data[1].ToString();
                                string sStackQRCode = Data[2].ToString();
                                string sUserId = Data[3].ToString();
                                DataTable dtFinalData = ClsBI991.GetFinalDeleteStackDetails(sLocCode, sStackQRCode, sUserId);
                                if (dtFinalData.Rows.Count > 0)
                                {
                                    if (dtFinalData.Columns.Contains("MatCode") && dtFinalData.Rows.Count == 1)
                                    {
                                        //if (sDeleteCount == Convert.ToInt32(dtFinalData.Rows[0][6].ToString()))
                                        //{
                                        //    Response = "DELETESCANNEDSTACKQRCODESDATA ~ SUCCESS ~ Stack Deleted Successfully";
                                        //    _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                                        //    ClsBI991 = null;
                                        //    return;
                                        //}
                                        //else if (sDeleteCount < Convert.ToInt32(dtFinalData.Rows[0][6].ToString()))
                                        //{
                                            Response = PrintStackDeletedNewStackQRCode(dtFinalData, sLocCode, sUserId);
                                        //}
                                    }
                                    //if (dtFinalData.Columns.Contains("MatCode") && dtFinalData.Rows.Count > 1)
                                    //{
                                    //    Response = PrintStackDeletedNewStackQRCode(dtFinalData, sLocCode, sUserId);
                                    //}
                                    else
                                    {
                                        int iCount = 0;
                                        dtFinalData = new DataTable();
                                        //dtFinalData = dtStackDeleteData.Clone();
                                        dtFinalData = dtStackDeleteData.DefaultView.ToTable(true, "MatCode");
                                        dtFinalData.Columns.Add("GradeDesc");
                                        dtFinalData.Columns.Add("GroupDesc");
                                        dtFinalData.Columns.Add("ThicknessDesc");
                                        dtFinalData.Columns.Add("Size");
                                        dtFinalData.Columns.Add("TotalQty");
                                        for (int i = 0; i < dtFinalData.Rows.Count; i++)
                                        {
                                            string Finalmatcode = dtFinalData.Rows[i][1].ToString();
                                            for (int j = 0; j < dtStackDeleteData.Rows.Count; j++)
                                            {
                                                string Stackmatcode = dtStackDeleteData.Rows[j][1].ToString();
                                                if (Stackmatcode == Finalmatcode)
                                                {
                                                    iCount++;
                                                    dtFinalData.Rows[i][0] = dtStackDeleteData.Rows[j][1].ToString();
                                                    dtFinalData.Rows[i][1] = dtStackDeleteData.Rows[j][2].ToString();
                                                    dtFinalData.Rows[i][2] = dtStackDeleteData.Rows[j][3].ToString();
                                                    dtFinalData.Rows[i][3] = dtStackDeleteData.Rows[j][4].ToString();
                                                    dtFinalData.Rows[i][4] = dtStackDeleteData.Rows[j][5].ToString();
                                                    dtFinalData.Rows[i][5] = Convert.ToString(iCount);
                                                }
                                            }
                                        }
                                        Response = PrintStackDeletedNewStackQRCode(dtFinalData, sLocCode, sUserId);
                                    }
                                }
                                else if (dtFinalData.Rows.Count == 0)
                                {
                                    Response = "DELETESCANNEDSTACKQRCODESDATA ~ SUCCESS ~ Stack Deleted Successfully";
                                    _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                                    ClsBI991 = null;
                                    return;
                                }
                            }
                        }
                        else
                        {
                            Response = "DELETESCANNEDSTACKQRCODESDATA ~ ERROR ~ There is no QRCode scanned for Delete yet, Kindly scan QRCode first";
                            _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                            ClsBI991 = null;
                            return;
                        }
                        if (Response.Contains("SUCCESS"))
                        {
                            Response = "DELETESCANNEDSTACKQRCODESDATA ~ SUCCESS ~ New StackQRCode Printed Successfully";
                            if (dtStackDeleteData.Rows.Count > 0)
                                dtStackDeleteData.Rows.Clear();
                        }
                        else
                        {
                            Response = "DELETESCANNEDSTACKQRCODESDATA ~ ERROR ~ " + Response;
                        }
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI991 = null;
                        break;

                    case "CLEARSCANNEDSTACKDATATABLE":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        if (dtStackDeleteData.Rows.Count > 0)
                            dtStackDeleteData.Rows.Clear();
                        Response = "CLEARSCANNEDSTACKDATATABLE ~ SUCCESS";
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI10 = null;
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
                        B_StackPrinting ClsBI5 = new B_StackPrinting();
                        dtStockCount.Rows.Clear();
                        Response = "CLEARSTOCKCOUNTTABLE ~ SUCCESS";
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI5 = null;
                        break;

                    case "SAVESTOCKCOUNTDETAILS":
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + RemoteClient.Message, "LOG");
                        B_StockCount ClsBI6 = new B_StockCount();
                        if (dtStockCount.Rows.Count > 0)
                        {
                            Response = ClsBI6.SaveStockCountDetails(Data[1], Data[2], dtStockCount);
                        }
                        _UpdateClientCount(RemoteClient.ClientIP + ";" + Response, "LOG");
                        ClsBI6 = null;
                        break;

                        #endregion

                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.ToString(), Environment.NewLine, ex.Source, ex.StackTrace);
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
                string tLocCode = dtTable.Rows[i][0].ToString();
                string tDONo = dtTable.Rows[i][1].ToString();
                string tMatcode = dtTable.Rows[i][2].ToString();
                string tQRcode = dtTable.Rows[i][7].ToString();
                string tStackQRCode = dtTable.Rows[i][8].ToString();
                for (int j = 0; j < dtLoadOffloadData.Rows.Count; j++)
                {
                    DataRow dr = dtLoadOffloadData.Rows[j];
                    if (dr["LocationCode"].ToString() == tLocCode.Trim().ToString() && dr["DONo"].ToString() == tDONo.Trim().ToString()
                        && dr["MatCode"].ToString() == tMatcode.Trim().ToString() && dr["QRCode"].ToString() == tQRcode.Trim().ToString()
                        && dr["StackQRCode"].ToString() == tStackQRCode.Trim().ToString())
                    {
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
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "RemoveStackDataFromLoadOffloadTable", "No. of Records removed of stack QRCode - " + StackQRCode + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
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
                sPrintStatus = "PRINTSALESRETURNQRCODE ~ SUCCESS ~ QRCODE - " + objQRCode + " is Generated And Printed Successfully";
            }
            else if (PrintStatus.Contains("ERROR"))
            {
                sPrintStatus = "PRINTSALESRETURNQRCODE ~ ERROR ~ ERROR in Save/Posted ";
            }
            return sPrintStatus;
        }

        public string PrintLoadOffloadStackQRCode(DataTable dtsStackData, string _sType, string _sUserId)
        {
            _bcilNetwork = new BcilNetwork();
            string OutMsg = string.Empty;
            dtFinalData = new DataTable();
            objclsPrint = new B_ClsPrint();
            objclsload = new B_LoadingOffloading();
            string objLocCode = string.Empty;
            string objDONo = string.Empty;
            string objMatCode = string.Empty;
            string objQRCode = string.Empty;
            string objStackQRCode = string.Empty;
            objNewStackQRCode = string.Empty;
            string objUserId = string.Empty;
            string sSaveStatus = string.Empty;
            string sPrintStatus = string.Empty;
            sSaveCount = 0;
            sStackPrintCount = 0;

            if (objLocationCode == "2000" && _sUserId.Contains("WH"))
            {
                _bcilNetwork.PrinterIP = Properties.Settings.Default.WHDispatchStackQRCodePrinterIP;
                _bcilNetwork.PrinterPort = Properties.Settings.Default.WHDispatchStackQRCodePrinterPort;
            }
            else
            {
                _bcilNetwork.PrinterIP = Properties.Settings.Default.DispatchStackQRCodePrinterIP;
                _bcilNetwork.PrinterPort = Properties.Settings.Default.DispatchStackQRCodePrinterPort;
            }
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintLoadOffloadStackQRCode - PrintStackQRCode => ", "LocationCode : " + objLocationCode + " & UserId : " + _sUserId);
            OutMsg = _bcilNetwork.NetworkPrinterStatus();
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintLoadOffloadStackQRCode - PrintStackQRCode => ", "Printer Status : " + OutMsg + " of IPAddesss : " + _bcilNetwork.PrinterIP);
            if (OutMsg == "PRINTER READY")
            {
                oDay = oMonth = oYear = oDateFormat = string.Empty;
                oDay = DateTime.Now.ToString("dd");
                oMonth = DateTime.Now.ToString("MM");
                oYear = DateTime.Now.ToString("yy");
                oDateFormat = oDay.Trim() + oMonth.Trim() + oYear.Trim();

                sStackRunningSerial = objclsPrint.GetStackRunningSerial(objLocationCode, oDateFormat, sPrintingSection, sLocationType);
                if (sStackRunningSerial == string.Empty || sStackRunningSerial == "" || sStackRunningSerial == "0")
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
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintLoadOffloadStackQRCode - GeneratedNewStackQRCode => ", objNewStackQRCode);

                if (dtsStackData.Rows.Count > 0)
                {
                    for (int i = 0; i < dtsStackData.Rows.Count; i++)
                    {
                        objLocCode = dtsStackData.Rows[i]["LocationCode"].ToString().Trim();
                        objDONo = dtsStackData.Rows[i]["DONo"].ToString().Trim();
                        objStackQRCode = dtsStackData.Rows[i]["StackQRCode"].ToString().Trim();
                        objUserId = _sUserId;
                        sSaveStatus = objclsload.SaveUpdateStackQRCode(objLocCode, objDONo.Trim(), objMatCode, objQRCode, objStackQRCode, objNewStackQRCode, oDateFormat, objUserId, _sType, sPrintingSection, sLocationType);
                        if (sSaveStatus.Contains("SUCCESS"))
                        {
                            sSaveCount++;
                        }
                    }
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintLoadOffloadStackQRCode - PrintStackQRCode => ", "No. of Records Updated - " + sSaveCount + " for New StackQRCode " + objNewStackQRCode);
                    if (sSaveStatus.Contains("SUCCESS"))
                    {
                        int iCount = 0;
                        _bcilNetwork.Dispose();
                        sPrintStatus = objclsload.PrintStackQRCodeItem(dtsStackData, objNewStackQRCode, _sUserId);
                        if (sPrintStatus.Contains("SUCCESS"))
                        {
                            isPrinted = true;
                            sStackPrintCount++;
                        }
                        else if (sPrintStatus.Contains("ERROR"))
                        {
                            return sPrintStatus;
                        }
                    }
                }
                else
                {
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintLoadOffloadStackQRCode - PrintStackQRCode => ", "No. of Records received to print stack : " + dtsStackData.Rows.Count + " for New StackQRCode : " + objNewStackQRCode);
                    sPrintStatus = "ERROR in creation of stack";
                }
            }
            else
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintLoadOffloadStackQRCode - PrintStackQRCode => ", "Printer Status : " + OutMsg + " of IPAddesss : " + _bcilNetwork.PrinterIP);
                sPrintStatus = "Printer IP : " + _bcilNetwork.PrinterIP + " not in network";
                _bcilNetwork.Dispose();
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
            //if (sStackPrintCount == 1)
            //{
            //    dsData = new DataSet();
            //    dtStackData.Columns.Remove("GradeDesc");
            //    dtStackData.Columns.Remove("GroupDesc");
            //    dtStackData.Columns.Remove("ThicknessDesc");
            //    dtStackData.Columns.Remove("MatSize");
            //    dsData = objSegPrint.fnPIGetSegregationDataSAPPost(dtStackData);
            //    if (dsData.Tables[0].Rows.Count > 0)
            //    {
            //        foreach (DataRow dr in dsData.Tables[0].Rows)
            //        {
            //            string LocCode = dr["LocationCode"].ToString().Trim();
            //            string objMatcode = dr["MatCode"].ToString().Trim();
            //            objQRCode = dr["QRCode"].ToString().Trim();
            //            string objMatStatus = dr["Status"].ToString().Trim();
            //            string SAPPostMsg = dr["SAPStatus"].ToString().Trim();
            //            string PostStatus = objSegPrint.UpdateSegregationQRCodeSAPStatus(LocCode, objMatcode, objQRCode, objMatStatus, SAPPostMsg, sUserID);
            //            if (PostStatus.Contains("SUCCESS"))
            //                oSAPPostCount++;
            //        }
            //    }
            //    if (oSAPPostCount > 0)
            //        sPrintStatus = "PRINTSEGREGATIONSTACKQRCODE ~ SUCCESS ~ STACK QRCODE - " + objStackQRCode + " Is Generated, Printed And " + oSAPPostCount + " No fo Records Posted To SAP Successfully Out Of " + dtStackData.Rows.Count;
            //}
            //else
            //{
            //    sPrintStatus = "PRINTSEGREGATIONSTACKQRCODE ~ ERROR ~ ERROR In Printing of Stack QRCode, Kindly Try Again For Printing";
            //}
            return sPrintStatus;
        }

        public string PrintStackMergingStackQRCode(string sLocCode, string sUserID)
        {
            objclsPrint = new B_ClsPrint();
            objSegPrint = new B_StackPrinting();
            string sPrintStatus = string.Empty;
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

            if (dtStackMergeData.Rows.Count > 0)
            {
                sPrintStatus = objSegPrint.PrintStackMergingStackQRCode(objLocationCode, dtStackMergeData, objStackQRCode, oDateFormat, sUserID);
                if (sPrintStatus.Contains("SUCCESS"))
                {
                    isPrinted = true;
                    sStackPrintCount++;
                }
            }
            return sPrintStatus;
        }

        public string PrintStackSplittingStackQRCode(string sLocCode, string sUserID)
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

            if (dtStackSplitData.Rows.Count > 0)
            {
                sPrintStatus = objSegPrint.PrintStackSplittingStackQRCode(objLocationCode, dtStackSplitData, odtSplitData, objStackQRCode, oDateFormat, sUserID);
                if (sPrintStatus.Contains("SUCCESS"))
                {
                    isPrinted = true;
                    sStackPrintCount++;
                }
                else
                {

                }
            }
            return sPrintStatus;
        }

        public string PrintStackDeletedNewStackQRCode(DataTable dt, string sLocCode, string sUserID)
        {
            objclsPrint = new B_ClsPrint();
            objSegPrint = new B_StackPrinting();
            string sPrintStatus = string.Empty;
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

            if (dt.Rows.Count > 0)
            {
                sPrintStatus = objSegPrint.DeletedStackPrintNewStackQRCode(dt, objStackQRCode, oDateFormat, sLocCode, sUserID);
                if (sPrintStatus.Contains("SUCCESS"))
                {
                    isPrinted = true;
                    sStackPrintCount++;
                }
            }
            return sPrintStatus;
        }


        /// <summary>
        /// //
        /// </summary>
        /// <returns></returns>

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


        //
        void RemoveList(string ip)
        {
            for (int i = 0; i < lvClientIP.Items.Count; i++)
            {
                if (lvClientIP.Items[i].Text.CompareTo(ip) == 0)
                {
                    lvClientIP.Items[i].Remove();
                    break;
                }
            }
            UpdateClientCount();
        }

        void AddList(string ip)
        {
            lvClientIP.Items.Add(ip);
            UpdateClientCount();

        }

        void m_Socket_OnClientConnected(string IP)
        {
            lvClientIP.Invoke(new AddClientList(AddList), IP);
        }

        void m_Socket_OnClientDisconnected(string IP)
        {
            lvClientIP.Invoke(new RemoveClientList(RemoveList), IP);
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
                lblserverandport.Text = string.Format("{0}:{1}", myIP, 6161);
                lblStatus.Text = string.Format("Database : {0}", strDatabase);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                throw ex;
            }
        }

        void UpdateClientCount()
        {
            lblCount.Text = lvClientIP.Items.Count.ToString();
            lblCount.Text = "Version 1.0.1";
        }

        //
        private void cmdSend_Click(object sender, EventArgs e)
        {
            try
            {
                m_Socket.SendMsgToClient("<" + txtMessage.Text + ">", lvClientIP.Items[0].Text);
                //   txtMessage.Text = ""; 
            }
            catch (Exception ex) { }
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

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                _BCILSocketServer.StopService();
                if (_BCILSocketServer != null)
                    _BCILSocketServer = null;
                if (isRunning == true)
                    isRunning = false;
                this.Dispose();
                this.Close();
                Application.Exit();
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                //  VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "TackCommServer_FormClosing", ex.Message);
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
                MessageBox.Show(oSql.Message);
            }
            catch (Exception ex)
            {
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
                MessageBox.Show(ex.Message);
            }
        }

        private DataTable getDBSchema(string strSource, string strUser, string strPwd)
        {
            try
            {
                DataTable dtSchema = new DataTable();
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
                throw ex;
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
                    // cmdStart(null, null);
                    // clearAllValues();
                }
                else
                {
                    Messagetab.SelectedTab = tabPage2;
                    //SetDbAppSetting();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

    }
}