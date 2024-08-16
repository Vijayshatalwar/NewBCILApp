using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BCILLogger;
using System.IO;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Threading;
using System.Timers;
using System.Threading.Tasks;
using System.Net;

namespace DataScheduler
{
    public partial class frmMain : Form
    {
        bool SchedularStart = false;
        bool isRunning;
        //public System.Windows.Forms.Label lblMessage;
        #region variable declaration
        BlWMS oWMS;
        DlCommon oDlcom = new DlCommon();
        public string DATASOURCE;
        public string DATABASE;
        public string dbUSER;
        public string PASSWORD;
        public string PLANTCODE;
        public string SystemUID;
        public string SystemPWD;
        public int TimeInterval = 0;
        string strOldTime = string.Empty;
        string strFlag = string.Empty;
        DataTable dtTime = new DataTable();
        private System.Threading.Thread myThread;
        bool isStart = false;
        string sTime = string.Empty;
        //Thread t = null;
        //Thread tMaster;
        //Thread tProcess;
        public delegate void IVoidDelegate();

        #endregion

        public frmMain()
        {
            InitializeComponent();
            Logger _obj = new Logger();
            _obj.NoOfDaysToDeleteFile = 30;
            _obj.ErrorFileName = "log";
            _obj.AppPath = Application.StartupPath;
            _obj.EnableLogging = true;
            _obj.WriteLog("Scheduler :: Initializing .......");
            clsGlobal.AppLog = _obj;
            //this.Height = 508;
            //this.Width = 562;
            oWMS = new BlWMS();
        }

        private void defaultDisplay()
        {
            //gbDMaster.SetBounds(335, 124, 220, 310);
            //gbDTrans.SetBounds(115, 484, 220, 298);
            //gbUTrans.SetBounds(337, 484, 220, 298);
            //gbDMaster.Visible = false;
            //gbDTrans.Visible = false;
            //gbUTrans.Visible = false;
            lblMessage.Text = string.Empty;
        }

        #region Radio button events

        private void optDMaster_Click(object sender, EventArgs e)
        {
            defaultDisplay();
            //gbDTrans.SetBounds(337, 124, 220, 298);
            //gbDMaster.Visible = true;
        }

        private void optDTrans_Click(object sender, EventArgs e)
        {
            defaultDisplay();
            //gbDTrans.SetBounds(335, 124, 220, 296);
            //gbDTrans.Visible = true;
        }

        private void optUpload_Click(object sender, EventArgs e)
        {
            defaultDisplay();
            // gbUTrans.SetBounds(335, 124, 220, 298);
            //gbUTrans.Visible = true;
        }

        #endregion

        private void frmMain_Load(object sender, EventArgs e)
        {
            try
            {
                //pnlOperation.Visible = false;

                //For Central Location
                if (Properties.Settings.Default.RunningLocation == "CENTRAL")
                {
                    timer_Central.Enabled = true;
                    timer_Central.Interval = Convert.ToInt32(Properties.Settings.Default.CentralDataInterval.Trim());
                }

                string s = Application.OpenForms[0].Name.ToString();
                string str = System.Reflection.MethodBase.GetCurrentMethod().Name;
                defaultDisplay();
                AddFlaginList();
                //optDMaster.Checked = true;
                //dudSession.SelectedIndex = 0;
                lblVersion.Text = "Version: 3.0.0";
                try
                {
                    if (!File.Exists(Application.StartupPath + "\\SAPConfig.ini"))
                    {
                        clsGlobal.AppLog.WriteLog("Data Scheduler :: Loading  " + "SAP Configuration file not found !!!");
                        MessageBox.Show("SAP config file does not exist");
                        this.Dispose();
                        this.Close();
                        return;
                    }
                    if (checkConStr() == true)
                    {
                        clsGlobal.AppLog.WriteLog("Data Scheduler :: Loading  " + "Database setting file not found !!!");
                        MessageBox.Show("Database setting file not found !!!");
                        this.Dispose();
                        this.Close();
                        return;
                    }
                    if (checkConClientStr() == true)
                    {
                        clsGlobal.AppLog.WriteLog("Data Schedular :: Loading  " + "Database setting file not found !!!");
                        MessageBox.Show("Database setting file not found !!!");
                        this.Dispose();
                        this.Close();
                        return;
                    }
                    ReadSAPSettings();
                    clsGlobal.AppLog.WriteLog("Scheduler :: Loading  " + " Successfull...");
                    isRunning = true;
                    myThread = new Thread(SchedularThreadConnectMode);
                    myThread.Start();
                }
                catch (Exception ex)
                {
                    clsGlobal.AppLog.WriteLog("Data Scheduler :: Loading  " + "Loading Failed..." + ex.Message.ToString());
                }
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Loading  " + "Loading Failed..." + ex.Message.ToString());
            }
        }

        #region Connection part

        public bool ReadClientSettings()
        {

            FileInfo serverfile = new FileInfo(Application.StartupPath + "\\CentralConfig.ini");
            StreamReader readServer;
            string strline = string.Empty;
            try
            {
                if (serverfile.Exists)
                {
                    string[] strArr;
                    readServer = new StreamReader(Application.StartupPath + "\\CentralConfig.ini");
                    do
                    {
                        strline = readServer.ReadLine();
                        if (strline.ToUpper() == "</SQL-LOCAL_SETTING>")
                        {
                            break;
                        }
                        strArr = strline.Split('~');

                        if (strArr[0].ToString().Trim() == "SERVERNAME")
                        {
                            clsGlobal.mCServerName = strArr[1].ToString().Trim();
                        }
                        if (strArr[0].ToString().Trim() == "DATABASENAME")
                        {
                            clsGlobal.mCdatabase = strArr[1].ToString().Trim();
                        }
                        if (strArr[0].ToString().Trim() == "USERNAME")
                        {
                            clsGlobal.mCDbUser = strArr[1].ToString().Trim();
                        }
                        if (strArr[0].ToString().Trim() == "PASSWORD")
                        {
                            clsGlobal.mCDbPassword = strArr[1].ToString().Trim();
                        }
                        if (strArr[0].ToString().Trim() == "PLANTCODE")
                        {
                            clsGlobal.mCPlantCode = strArr[1].ToString().Trim();

                        }

                    } while (strline != null);
                    readServer.Close();
                    readServer = null;
                    return true;
                }
                else
                {
                    serverfile = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                readServer = null;
                serverfile = null;
            }
        }

        public void ReadSAPSettings()
        {
            try
            {
                string strLine = "";
                string[] strArr = null;
                StreamReader sr = new StreamReader(Application.StartupPath + "\\SAPConfig.ini");
                while (!sr.EndOfStream)
                {
                    strLine = sr.ReadLine();
                    strArr = strLine.Split('=');
                    switch (strArr[0])
                    {
                        case "SAP_SERVER":
                            //clsGlobal.mSapServer = strArr[1].ToString().Trim();
                            break;
                        case "SAP_CLIENT":
                            // clsGlobal.mSapClient = strArr[1].ToString().Trim();
                            break;
                        case "SAP_LANGUAGE":
                            //clsGlobal.mSapLng = strArr[1].ToString().Trim();
                            break;
                        case "SAP_SYSTEM_NO":
                            clsGlobal.mSapSysNo = strArr[1].ToString().Trim();
                            break;
                        case "SAP_USER":
                            //clsGlobal.mSapUser = strArr[1].ToString().Trim();
                            break;
                        case "SAP_PASSWORD":
                            //clsGlobal.mSapPassword = strArr[1].ToString().Trim();
                            break;
                        //case "SAP_ROUTER_STRING":
                        //    clsGlobal.mSapRtrStr = strArr[1].ToString().Trim();
                        //    break;
                    }
                }
                sr.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool ReadSqlParameter()
        {
            FileInfo serverfile = new FileInfo(Application.StartupPath + "\\DBSetting.ini");
            StreamReader readServer;
            string strline = string.Empty;
            try
            {
                if (serverfile.Exists)
                {
                    string[] strArr;
                    readServer = new StreamReader(Application.StartupPath + "\\DBSetting.ini");
                    do
                    {
                        strline = readServer.ReadLine();
                        if (strline.ToUpper() == "</SQL-LOCAL_SETTING>")
                        {
                            break;
                        }
                        strArr = strline.Split('~');

                        if (strArr[0].ToString().Trim() == "DATABASENAME")
                        {
                            DATABASE = strArr[1].ToString().Trim();
                        }
                        if (strArr[0].ToString().Trim() == "USERNAME")
                        {
                            dbUSER = strArr[1].ToString().Trim();
                        }
                        if (strArr[0].ToString().Trim() == "PASSWORD")
                        {
                            PASSWORD = strArr[1].ToString().Trim();
                        }
                        if (strArr[0].ToString().Trim() == "SERVERNAME")
                        {
                            DATASOURCE = strArr[1].ToString().Trim();
                        }
                        if (strArr[0].ToString().Trim() == "PLANTCODE")
                        {
                            PLANTCODE = strArr[1].ToString().Trim();
                            clsGlobal.PlantCode = PLANTCODE;
                        }
                        if (strArr[0].ToString().Trim() == "SystemUID")
                        {
                            SystemUID = strArr[1].ToString().Trim();
                            clsGlobal.SystemUID = SystemUID;
                        }
                        if (strArr[0].ToString().Trim() == "SystemPWD")
                        {
                            SystemPWD = strArr[1].ToString().Trim();
                            clsGlobal.SystemPWD = SystemPWD;
                        }
                        if (strArr[0].ToString().Trim() == "TimeInteravl")
                        {
                            TimeInterval = Convert.ToInt32(strArr[1].ToString().Trim());
                            clsGlobal.TimeInterval = TimeInterval;
                        }
                    }
                    while (strline != null);
                    readServer.Close();
                    readServer = null;
                    return true;
                }
                else
                {
                    serverfile = null;
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                readServer = null;
                serverfile = null;
            }

        }

        private bool checkConStr()
        {
            bool ldbsetting = false;
            if (ReadSqlParameter())
            {
                clsGlobal.StrCon = "Data Source=" + DATASOURCE + ";Initial Catalog=" + DATABASE + ";User ID=" + dbUSER + ";Password=" + PASSWORD + ";Connect Timeout=0";
                ldbsetting = false;
            }
            else
            {
                ldbsetting = true;
            }
            return ldbsetting;
        }

        private bool checkConClientStr()
        {
            bool lClientdbsetting = false;
            if (ReadClientSettings())
            {
                clsGlobal.StrClientCon = "Data Source=" + clsGlobal.mCServerName + ";Initial Catalog=" + clsGlobal.mCdatabase + ";User ID=" + clsGlobal.mCDbUser + ";Password=" + clsGlobal.mCDbPassword + ";connection timeout=0";
                clsGlobal.AppLog.WriteLog("Data Schedular :: Central connection " + clsGlobal.StrClientCon);
                lClientdbsetting = false;
            }
            else
            {
                lClientdbsetting = true;
            }
            return lClientdbsetting;
        }

        #endregion


        #region Context menu strip

        private void tmiClose_Click(object sender, EventArgs e)
        {

            CloseMsg("Transactions Pending for process.. Please wait for application to be closed.");
            clsGlobal.isBreak = true;
            //  while(clsGlobal.isBreak) 
            //  t.Abort();
            //Application.ExitThread();
            //Application.Exit();
        }

        private void tmiShow_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void tmiHide_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        #endregion


        //private void btnClose_Click(object sender, EventArgs e)
        //{
        //    this.Close();
        //}

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            clsGlobal.AppLog.WriteLog("Central Data Scheduler ::  Stop - " + " Stopped Successfully at" + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
            if (myThread.ThreadState == System.Threading.ThreadState.Running)
                myThread.Abort();
            SchedularStart = false;
            isRunning = false;
            timer_Central.Stop();
            this.Dispose();
            this.Close();
            Application.Exit();
            Environment.Exit(1);
        }

        private void optConfig_Click(object sender, EventArgs e)
        {
            defaultDisplay();
            //pnlTimer.SetBounds(0, 70, 556, 400);
            //pnlTimer.Visible = true;
            oWMS = new BlWMS();
            setTimeGrid(oWMS);
        }

        private void cmdTimerBack_Click(object sender, EventArgs e)
        {
            //pnlTimer.Visible = false;
            clearTimerControls();
            //optDMaster.PerformClick();
        }

        private void AddFlaginList()
        {
            //lbFlags.Items.Clear();
            //lbFlags.Items.Add("Download Master");
            //lbFlags.Items.Add("Download Process");
        }

        private void clearTimerControls()
        {
            //nudHours.Value = nudHours.Minimum;
            //nudMin.Value = nudMin.Minimum;
            //dudSession.SelectedIndex = 0;
            //chkActive.Checked = false;
            //for (int i = 0; i < lbFlags.Items.Count; i++)
            //{
            //    lbFlags.SetItemCheckState(i, CheckState.Unchecked);
            //}
            strOldTime = string.Empty;
        }

        private void cmdClearTime_Click(object sender, EventArgs e)
        {
            try
            {
                clearTimerControls();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            oWMS = new BlWMS();
            string SchTime = string.Empty;
            string strFlags = string.Empty;
            bool IsActive = false;

            //SchTime = nudHours.Value.ToString() + ":" + nudMin.Value.ToString().PadLeft(2, '0') + " " + dudSession.Text.Trim();
            //CheckedListBox.CheckedItemCollection oItems = lbFlags.CheckedItems;

            //if (oItems.Count > 0)
            //{
            //    for (int i = 0; i < oItems.Count; i++)
            //    {
            //        if (strFlags == string.Empty)
            //        {
            //            strFlags = oItems[i].ToString();
            //        }
            //        else
            //        {
            //            strFlags = strFlags + "," + oItems[i].ToString();
            //        }
            //    }
            //    IsActive = chkActive.Checked;

            //    oWMS.saveTimeDtls(SchTime, strFlags, IsActive);
            //    clearTimerControls();
            //    setTimeGrid(oWMS);
            //    dtTime = oWMS.getActiveTimerDtls();
            //    chkActive.Checked = true;
                //Application.Exit();
            //}
        }

        private void setTimeGrid(BlWMS oWMS)
        {
            DataTable dt = oWMS.getTimerDtls();

            //this.dgvTimeDtls.RowsDefaultCellStyle.BackColor = Color.Bisque;
            //this.dgvTimeDtls.AlternatingRowsDefaultCellStyle.BackColor = Color.Beige;

            //dgvTimeDtls.Rows.Clear();

            //foreach (DataRow dr in dt.Rows)
            //{
            //    dgvTimeDtls.Rows.Add(dr["Time"].ToString(), dr["FileFlag"].ToString(), dr["isActive"].ToString());
            //}
        }

        private void dgvTimeDtls_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    //strOldTime = dgvTimeDtls.Rows[e.RowIndex].Cells[0].Value.ToString();

                    //nudHours.Value = Convert.ToInt32(strOldTime.Split(':')[0].ToString());
                    //nudMin.Value = Convert.ToInt32(strOldTime.Split(':')[1].ToString().Split(' ')[0]);
                    //dudSession.Text = strOldTime.Split(':')[1].ToString().Split(' ')[1];

                    //chkActive.Checked = Convert.ToBoolean(dgvTimeDtls.Rows[e.RowIndex].Cells[2].Value.ToString());

                    //string[] strFlags = dgvTimeDtls.Rows[e.RowIndex].Cells[1].Value.ToString().Split(',');

                    //for (int j = 0; j < lbFlags.Items.Count; j++)
                    //{
                    //    lbFlags.SetItemCheckState(j, CheckState.Unchecked);
                    //}

                    //for (int i = 0; i < strFlags.Length; i++)
                    //{
                    //    for (int j = 0; j < lbFlags.Items.Count; j++)
                    //    {
                    //        if (strFlags[i] == lbFlags.Items[j].ToString())
                    //        {
                    //            lbFlags.SetItemCheckState(j, CheckState.Checked);
                    //            break;
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SchedularThreadConnectMode()
        {
            try
            {
                SchedularStart = true;
                while (isRunning)
                {
                    TimerThreadCentalToSAPData();
                }
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("Data Scheduler : Error in Sending Central Data : ERROR : " + ex.Message.ToString() + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                MessageBox.Show(ex.ToString());
            }
        }

        private void timer_Central_Tick(object sender, EventArgs e)
        {
            //myThread = new Thread(TimerThreadCentalToSAPData);
            //myThread.Start();
            //TimerThreadCentalToSAPData();
        }



        private void TimerThreadCentalToSAPData()
        {
            try
            {
                DwldC2SAPWebAPILabelPrintingData();
                DwldC2SAPWebAPIDoorLabelPrintingData();
                DwldC2SAPWebAPIHubPrintingDataBeforeProduction();
                DwldC2SAPWebAPIHubQualityData();
                DwldC2SAPWebAPIVendorPrintData();
                DwldC2SAPWebAPIDeliveryOrderData();
                //DwldC2SAPWebAPIDeliveryCencellationOrderData();
                DwldC2SAPWebAPISalesReturnData();
                //DwldC2SAPWebAPIPurchaseOrderData();
                DwldC2SAPWebAPIPurchaseReturnData();
                DwldC2SAPWebAPIProductionReversalData();
                DwldC2SAPWebAPIMatDamageData();
                DwldC2SAPWebAPIMTMTransferData();
                DwldC2SAPWebAPIStackHistoryData();
                System.Threading.Thread.Sleep(Convert.ToInt32(Properties.Settings.Default.CentralDataInterval.Trim()));
            }
            catch (Exception ex)
            {
                System.Threading.Thread.Sleep(Convert.ToInt32(Properties.Settings.Default.CentralDataInterval.Trim()));
                clsGlobal.AppLog.WriteLog("Data Scheduler : Error in Sending Central Data : ERROR : " + ex.Message.ToString());
                MessageBox.Show(ex.ToString());
            }
        }



        #region Central To SAP

        private void DwldC2SAPWebAPILabelPrintingData()
        {
            try
            {
                oWMS = new BlWMS();
                clsGlobal.lmsg = string.Empty;
                clsGlobal.AppLog.WriteLog("Central Data Scheduler : Sending Central To SAP Label Printing Data : " + "Sending started " );
                oWMS.GetCentral2SAPWebAPIProductionData();
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("Central Data Scheduler : Error in Sending Central To SAP Label Printing Data : " + ex.Message.ToString());
            }
            finally
            {
                oWMS = null;
            }
        }

        private void DwldC2SAPWebAPIDoorLabelPrintingData()
        {
            try
            {
                oWMS = new BlWMS();
                clsGlobal.lmsg = string.Empty;
                clsGlobal.AppLog.WriteLog("Central Data Scheduler : Sending Central To SAP Label Door Printing Data : " + "Sending started" );
                oWMS.GetCentral2SAPWebAPIDoorProductionData();
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("Central Data Scheduler : Error in Sending Central To SAP Label Printing Data : " + ex.Message.ToString());
            }
            finally
            {
                oWMS = null;
            }
        }

        private void DwldC2SAPWebAPIHubPrintingDataBeforeProduction()
        {
            try
            {
                oWMS = new BlWMS();
                clsGlobal.lmsg = string.Empty;
                clsGlobal.AppLog.WriteLog("Central Data Scheduler : Sending Central To SAP Label Printing Data : " + "Sending started" );
                oWMS.GetCentral2SAPWebAPIHubDataBeforeProduction();
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("Central Data Scheduler : Error in Sending Central To SAP Label Printing Data : " + ex.Message.ToString());
            }
            finally
            {
                oWMS = null;
            }
        }

        private void DwldC2SAPWebAPIHubQualityData()
        {
            try
            {
                oWMS = new BlWMS();
                clsGlobal.lmsg = string.Empty;
                clsGlobal.AppLog.WriteLog("Central Data Scheduler : Sending Central To SAP HubQuality Data : " + "Sending started");
                oWMS.GetCentral2SAPWebAPIHubQualityData();
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("Central Data Scheduler : Error in Sending Central To SAP HubQuality Data : " + ex.Message.ToString());
            }
            finally
            {
                oWMS = null;
            }
        }

        private void DwldC2SAPWebAPIHubQualityBOILData()
        {
            try
            {
                oWMS = new BlWMS();
                clsGlobal.lmsg = string.Empty;
                clsGlobal.AppLog.WriteLog("Central Data Scheduler : Sending Central To SAP Label Printing Data : " + "Sending started" );
                oWMS.GetCentral2SAPWebAPIHubQualityBOILData();
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("Central Data Scheduler : Error in Sending Central To SAP Label Printing Data : " + ex.Message.ToString());
            }
            finally
            {
                oWMS = null;
            }
        }

        private void DwldC2SAPWebAPIVendorPrintData()
        {
            try
            {
                oWMS = new BlWMS();
                sTime = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                clsGlobal.lmsg = string.Empty;
                clsGlobal.AppLog.WriteLog("Central Data Scheduler :: Sending Central To SAP Vendor Printing Data " + " Sending started ");
                oWMS.GetCentral2SAPWebAPIVendorData();
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("Central Data Scheduler : Error in Sending Central To SAP Vendor Printing Data : " + ex.Message.ToString());
            }
            finally
            {
                oWMS = null;
            }
        }

        private void DwldC2SAPWebAPIDeliveryOrderData()
        {
            try
            {
                oWMS = new BlWMS();
                sTime = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                clsGlobal.lmsg = string.Empty;
                clsGlobal.AppLog.WriteLog("Central Data Scheduler : Sending Central To SAP Dispatch Data :" + " Sending started");
                oWMS.GetCentral2SAPWebAPIDeliveryOrderData();
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("Central Data Scheduler : Error in Sending Central To SAP Dispatch Data : " + ex.Message.ToString());
            }
            finally
            {
                oWMS = null;
            }
        }

        private void DwldC2SAPWebAPIDeliveryCencellationOrderData()
        {
            try
            {
                oWMS = new BlWMS();
                sTime = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                clsGlobal.lmsg = string.Empty;
                clsGlobal.AppLog.WriteLog("Central Data Scheduler : Sending Central To SAP Dispatch Data :" + " Sending started" );
                oWMS.GetCentral2SAPWebAPICancelledDeliveryOrderData();
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("Central Data Scheduler : Error in Sending Central To SAP Dispatch Data : " + ex.Message.ToString());
            }
            finally
            {
                oWMS = null;
            }
        }

        private void DwldC2SAPWebAPISalesReturnData()
        {
            try
            {
                oWMS = new BlWMS();
                sTime = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                clsGlobal.lmsg = string.Empty;
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Sending Central To SAP Sales Return Data : " + "Sending started");
                oWMS.GetCentral2SAPWebAPISalesReturnData();
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("Data Scheduler : Error in Sending Central To SAP Sales Return Data : " + ex.Message.ToString());
            }
            finally
            {
                oWMS = null;
            }
        }

        private void DwldC2SAPWebAPIPurchaseOrderData()
        {
            try
            {
                oWMS = new BlWMS();
                sTime = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                clsGlobal.lmsg = string.Empty;
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Sending Central To SAP Sales Return Data : " + "Sending started " );
                oWMS.GetCentral2SAPWebAPIReturnPurchaseOrderData();
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("Data Scheduler : Error in Sending Central To SAP Sales Return Data : " + ex.Message.ToString());
            }
            finally
            {
                oWMS = null;
            }
        }

        private void DwldC2SAPWebAPIPurchaseReturnData()
        {
            try
            {
                oWMS = new BlWMS();
                sTime = DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                clsGlobal.lmsg = string.Empty;
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Sending Central To SAP Purchase Return Data : " + "Sending started " );
                oWMS.GetCentral2SAPWebAPIReturnPurchaseOrderData();
                lblMessage.Text = "Data Scheduler : Sending Central To SAP Purchase Return Data : " + "Total No. of Records Updated at Central Server";
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("Data Scheduler : Error in Sending Central To SAP Sales Return Data : " + ex.Message.ToString());
            }
            finally
            {
                oWMS = null;
            }
        }

        private void DwldC2SAPWebAPIProductionReversalData()
        {
            try
            {
                oWMS = new BlWMS();
                clsGlobal.lmsg = string.Empty;
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Download CentralToLocal :: Central To Local " + " Downloading started");
                oWMS.GetCentral2SAPWebAPIProductionReversalData();
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("Data Scheduler : Error in Sending Central To SAP MTMTransfer Data : " + ex.Message.ToString());
            }
            finally
            {
                oWMS = null;
            }
        }

        private void DwldC2SAPWebAPIMatDamageData()
        {
            try
            {
                oWMS = new BlWMS();
                clsGlobal.lmsg = string.Empty;
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Download CentralToLocal :: Central To Local " + " Downloading started");
                oWMS.GetCentral2SAPWebAPIMatDamageData();
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("Data Scheduler : Error in Sending Central To SAP MTMTransfer Data : " + ex.Message.ToString());
            }
            finally
            {
                oWMS = null;
            }
        }

        private void DwldC2SAPWebAPIMTMTransferData()
        {
            try
            {
                oWMS = new BlWMS();
                clsGlobal.lmsg = string.Empty;
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Download CentralToLocal :: Central To Local " + " Downloading started");
                oWMS.GetCentral2SAPWebAPIMTMTransferData();
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("Data Scheduler : Error in Sending Central To SAP MTMTransfer Data : " + ex.Message.ToString());
            }
            finally
            {
                oWMS = null;
            }
        }

        private void DwldC2SAPWebAPIStackHistoryData()
        {
            try
            {
                oWMS = new BlWMS();
                clsGlobal.lmsg = string.Empty;
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Download CentralToLocal :: Central To Local " + " Downloading started");
                oWMS.GetCentral2SAPWebAPIStackHistoryData();
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("Data Scheduler : Error in Sending Central To SAP Stack History Data : " + ex.Message.ToString());
            }
            finally
            {
                oWMS = null;
            }
        }

        #endregion


        delegate void ChangeMsg(string sMsg);
        delegate void delCloseForm();

        private void UpdateTime(string sMsg)
        {
            //if (this.InvokeRequired)
            //{
            //    this.Invoke(new ChangeMsg(UpdateTime), sMsg);
            //}
            //else
            //{
            //    lblMessage.Text = sMsg; // +"..." + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString(); 
            //}
        }

        private void CloseMsg(string sMsg)
        {
            if (this.InvokeRequired) { this.Invoke(new ChangeMsg(CloseMsg), sMsg); }
            else
            { //lblCloseMsg.Text = sMsg;
            }
        }

        private void CloseForm()
        {
            if (this.InvokeRequired) { this.Invoke(new delCloseForm(CloseForm)); }
            else
            { this.Close(); }
        }

        private void btnPODownloads_Click(object sender, EventArgs e)
        {
            try
            {
                //if (dtpSelectDate.CustomFormat == " " && String.IsNullOrEmpty(txtPONumber.Text))
                //{
                //    MessageBox.Show("Choose Atleast One Option!!", "DATA SCHEDULER", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    dtpSelectDate.Focus();
                //    return;
                //}
                //clsGlobal.sSelectDate = Convert.ToString(dtpSelectDate.Value.Date);
                //clsGlobal.sPONumber = Convert.ToString(txtPONumber.Text.Trim());
                //DwldGRNDetails();
                clsGlobal.sSelectDate = "";
                clsGlobal.sPONumber = "";
            }
            catch (Exception ex)
            {
                try
                {
                    oWMS.CreateLog("Error in downloading Mannual Dowloads PO", "Mannual Dowloads", ex.ToString(), "WMS", "");
                }
                catch (Exception exLog)
                {
                    clsGlobal.AppLog.WriteLog("ebatchTrace Scheduler :: Mannual Downloads   ::  Mannual Downloads :: " + "Error while inserting log" + exLog.Message.ToString());
                }
                clsGlobal.AppLog.WriteLog("ebatchTrace Scheduler :: Mannual Downloads    ::  Mannual Downloads :: " + "Error in downloading " + ex.Message.ToString());
                //lblMessage.Text = "Mannual Downloads :: Error in downloading  Mannual Downloads " + sTime + " Error " + ex.Message.ToString();
            }
        }

        private void dtpSelectDate_ValueChanged(object sender, EventArgs e)
        {
            //dtpSelectDate.CustomFormat = "dd/MM/yyyy";
        }

        private void btnBackManual_Click(object sender, EventArgs e)
        {
            try
            {
                //pnlMannualPODownload.Visible = false;
                //pnlMannualPODownload.SetBounds(0, 0, 0, 0);
                //pnlMannualPODownload.SendToBack();
                pnlHeader.Visible = true;
                pnlHeader.BringToFront();
                pnlImage.Visible = true;
                pnlImage.BringToFront();
                //pnlOperation.Visible = true;
                //pnlOperation.BringToFront();
                //gbDMaster.Visible = true;
            }
            catch (Exception ex)
            { MessageBox.Show(ex.Message.ToString()); }
        }

        private void lbFlags_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //for (int ix = 0; ix < lbFlags.Items.Count; ++ix)
            //    //lbFlags.SetItemCheckState(ix, CheckState.Checked);
            //    if (ix != e.Index) lbFlags.SetItemChecked(ix, false);
        }

        private void optDatabaseConfig_Click(object sender, EventArgs e)
        {
            defaultDisplay();
            //pnlDataConfig.SetBounds(0, 70, 556, 400);
            //pnlDataConfig.Visible = true;
            //oWMS = new BlWMS();
            setTimeGrid(oWMS);
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            //pnlDataConfig.Visible = false;
            clearTimerControls();
            //optDMaster.PerformClick();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            //MessageBoxResult mResult = MessageBox.Show("Do You Really Want To Clear ?", "Hellmann WMS", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //if (mResult == MessageBoxResult.No)
            //{ return; }
            //if (cmbServerName.SelectedValue.ToString().Trim() != null)
            //{ cmbServerName.SelectedIndex = 0; }
            //if (cmbdatabase.SelectedValue.ToString().Trim() != null)
            //{ cmbdatabase.SelectedIndex = 0; }
            //if (txtUsername.Text.ToString() != null)
            //{ txtUsername.Text = ""; }
            //if (txtPswd.Text.ToString() != null)
            //{ txtPswd.Text = ""; }
            //if (txtSystemPWD.Text.ToString() != null)
            //{ txtSystemPWD.Text = ""; }
            //if (txtSystemUID.Text.ToString() != null)
            //{ txtSystemUID.Text = ""; }
            //if (txtPlantCode.Text.ToString() != null)
            //{ txtPlantCode.Text = ""; }
        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            try
            {
                // ValidatePage();
                //clsGlobal.mServerName = cmbServerName.Text.Trim();
                //clsGlobal.mdatabase = cmbdatabase.Text.Trim();
                //clsGlobal.mDbUser = txtUsername.Text.Trim();
                //clsGlobal.mDbPassword = txtPswd.Text.Trim();
                //clsGlobal.SystemUID = txtSystemUID.Text.Trim();
                //clsGlobal.SystemPWD = txtSystemPWD.Text.Trim();
                //clsGlobal.PlantCode = txtPlantCode.Text.Trim();
                //StreamWriter _wr = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Setting.ini", false);
                //_wr.WriteLine("<LOCAL_SETTING>");
                //_wr.WriteLine("SERVERNAME=" + cmbServerName.Text.ToString().Trim());
                //_wr.WriteLine("DATABASENAME=" + cmbdatabase.Text.ToString().Trim());
                //_wr.WriteLine("USERNAME=" + txtUsername.Text.Trim());
                //_wr.WriteLine("PASSWORD=" + txtPswd.Text.Trim());
                //// _wr.WriteLine("SYSTEM"
                //_wr.WriteLine("PLANTCODE=" + txtPlantCode.Text.Trim());
                //_wr.WriteLine("</LOCAL_SETTING>");
                //_wr.Close();
                //_wr.Dispose();
                //if (BCommon.ReadParameter() == true)
                //{
                //    MessageBox.Show("Database Connected Successfully");
                //    pnlDataConfig.Visible = false;
                //    clearTimerControls();
                //    optDMaster.PerformClick();
                //}
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Download Process :: GetPODetails" + "Error in download in GRN Details " + ex.Message.ToString());
                throw ex;
            }
        }

    }
}
