using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using COMMON_LAYER;
using COMMON;
using ENTITY_LAYER;
using System.Reflection;
using BUSSINESS_LAYER;
using System.Configuration;
using System.Data.OleDb;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using GREENPLY.UserControls.Transaction;
using GREENPLY.Classes;
using System.Net;
using BCILLogger;
using System.Collections;
using System.Linq;
using GREENPLY.GreenplyERPPostingService;
namespace GREENPLY.UserControls.Masters
{
    /// <summary>
    /// Interaction logic for UcRejectionMaster.xaml
    /// </summary>
    public partial class UcRejectionMaster : UserControl
    {
        Logger objLog = new Logger();
        WriteLogFile ObjLog = new WriteLogFile();
        DataTable DtItems;
        BL_ItemSelection objBLItem;
        DataTable DtSelectedItems;
        int PrintStatuses = 0;
        BcilNetwork _bcilNetwork = new BcilNetwork();

        public UcRejectionMaster()
        {
            InitializeComponent();
        }

        #region Private Collection
        ObservableCollection<PLReprinting> _PLReprinting = new ObservableCollection<PLReprinting>();
        public ObservableCollection<PLReprinting> PLReprinting
        {
            get { return _PLReprinting; }
            set
            {
                _PLReprinting = value;
                OnPropertyChanged("PLReprinting");
            }
        }
        private void OnPropertyChanged(string propertyname)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyname));
        }
        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DisplayData();
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "RejectionMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void DisplayData()
        {
            try
            {
                PLReprinting = new BUSSINESS_LAYER.BL_ItemSelection().BLGetRejectionDetails();
                if (PLReprinting.Count > 0)
                {
                    lv.ItemsSource = PLReprinting;
                    lblCount.Content = lv.Items.Count;
                    DtItems = new DataTable();
                    ObservableCollection<PLReprinting> data = (ObservableCollection<PLReprinting>)lv.ItemsSource;
                    DtItems = VariableInfo.ToDataTable(data);
                }
                else
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "There is No Data Found", 2);
                    btnPrint.IsEnabled = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DataTable objMatData = new DataTable();
                DtSelectedItems = new DataTable();
                int iCount = 0;
                int iPrintCount = 0;
                string PrintStatus = string.Empty;
                //if (PrintStatuses == 0)
                //{
                //    PrintStatuses = 0;
                //    //BCommon.setMessageBox(VariableInfo.mApp, "There is no data found for StackQRCode - " + objSQRCode + " Kindly change", 2);
                //    return;
                //}
                DtSelectedItems = DtItems.Clone();
                foreach (PLReprinting item1 in lv.ItemsSource)
                {
                    if (item1.IsValid == true)
                    {
                        string objRejCode = Convert.ToString(item1.RejCode);
                        string objRejDesc = Convert.ToString(item1.RejDescription);
                        if (DtItems.Rows.Count > 0)
                        {
                            DataRow[] rowsToCopy;
                            rowsToCopy = DtItems.Select("RejCode ='" + objRejCode + "' AND RejDescription = '" + objRejDesc + "'");
                            foreach (DataRow temp in rowsToCopy)
                            {
                                DtSelectedItems.ImportRow(temp);
                            }
                        }
                    }
                    else
                    {
                        iCount++;
                        continue;
                    }
                }
                #region StackQRCode
                if (lv.Items.Count == iCount)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Select atleast one record to print", 1);
                    btnPrint.Cursor = Cursors.Arrow;
                    return;
                }
                if (DtSelectedItems.Rows.Count > 4)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "You have selected " + DtSelectedItems.Rows.Count + " no. of records, Kindly select upto 4 records only", 1);
                    btnPrint.Cursor = Cursors.Arrow;
                    return;
                }
                if (DtSelectedItems.Rows.Count < 4)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "You have selected " + DtSelectedItems.Rows.Count + " no. of records, Kindly select upto 4 records only", 1);
                    btnPrint.Cursor = Cursors.Arrow;
                    return;
                }
                if (DtSelectedItems.Rows.Count == 4)
                {
                    MessageBoxResult MessResult = MessageBox.Show("Are you sure, You want to print the selected rejection codes?", "Print Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                    if (MessResult == MessageBoxResult.No)
                    {
                        return;
                    }
                    if (MessResult == MessageBoxResult.Yes)
                    {
                        if (DtSelectedItems.Rows.Count == 4)
                        {
                            PrintStatus = PrintRejQRCodes(DtSelectedItems);
                            {
                                if (PrintStatus.Contains("SUCCESS"))
                                    iPrintCount++;
                            }
                        }
                        if (PrintStatus.Contains("SUCCESS"))
                        {
                            ObjLog.WriteLog("Rejection QRCodes are printed successfully");
                            BCommon.setMessageBox(VariableInfo.mApp, "Rejection QRCodes are printed successfully", 2);
                            DisplayData();
                        }
                        else
                        {
                            BCommon.setMessageBox(VariableInfo.mApp, "Error in Print - " + PrintStatus.ToString(), 1);
                            return;
                        }
                    }
                }
                btnPrint.Cursor = Cursors.Arrow;
                return;
                #endregion

            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "RejectionMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        public string PrintRejQRCodes(DataTable dtData)
        {
            try
            {
                _bcilNetwork = new BcilNetwork();
                string OutMsg = string.Empty;
                var sReadPrn = string.Empty;

                _bcilNetwork.PrinterIP = Properties.Settings.Default.ExistingStackQRCodePrinterIP;
                _bcilNetwork.PrinterPort = Properties.Settings.Default.ExistingStackQRCodePrinterPort;

                OutMsg = _bcilNetwork.NetworkPrinterStatus();
                if (OutMsg == "PRINTER READY")
                {
                    StringBuilder sb = new StringBuilder();
                    DataTable dt = new DataTable();
                    string sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyRejQRCode.PRN";
                    if (File.Exists(sPrnExist))
                    {
                        StreamReader sr = new StreamReader(sPrnExist);
                        sReadPrn = sr.ReadToEnd();
                        sr.Close();
                        sr.Dispose();
                    }
                    else
                    {
                        ObjLog.WriteLog("PRN File Not Found");
                        throw new Exception("PRN File Not Found");
                    }
                    for (int i = 0; i < dtData.Rows.Count; i++)
                    {
                        string sRejCode = dtData.Rows[i][32].ToString();
                        string sRejDesc = dtData.Rows[i][33].ToString();
                        string sFull = dtData.Rows[i][32].ToString() + " - " + dtData.Rows[i][33].ToString();
                        if (i == 0)
                        {
                            sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(sRejCode.Trim()));
                            sReadPrn = sReadPrn.Replace("{VarBarcode11}", Convert.ToString(sFull.Trim()));
                        }
                        if (i == 1)
                        {
                            sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(sRejCode.Trim()));
                            sReadPrn = sReadPrn.Replace("{VarBarcode22}", Convert.ToString(sFull.Trim()));
                        }
                        if (i == 2)
                        {
                            sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(sRejCode.Trim()));
                            sReadPrn = sReadPrn.Replace("{VarBarcode33}", Convert.ToString(sFull.Trim()));
                        }
                        if (i == 3)
                        {
                            sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(sRejCode.Trim()));
                            sReadPrn = sReadPrn.Replace("{VarBarcode44}", Convert.ToString(sFull.Trim()));
                        }
                    }
                    _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyRejQRCode.PRN";
                    //OutMsg = "SUCCESS";
                    OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                    _bcilNetwork.Dispose();
                    if (OutMsg == "SUCCESS")
                    {
                        return OutMsg = "SUCCESS";
                    }
                }
                else
                {
                    if (OutMsg == "PRINTER NOT IN NETWORK")
                    {
                        ObjLog.WriteLog("RejectionCodes => ERROR ~ Rejection Codes Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly Check the network");
                        OutMsg = "ERROR ~ Rejection Stack Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly Check the network";
                        return OutMsg;
                    }
                    else
                    {
                        ObjLog.WriteLog("RejectionCodes => ERROR ~ Rejection Codes Printer IP : " + _bcilNetwork.PrinterIP + " found error, Error is : " + OutMsg);
                        OutMsg = "ERROR ~ Rejection Codes Printer IP : " + _bcilNetwork.PrinterIP + " found error, Error is : " + OutMsg;
                        return OutMsg;
                    }
                }
                return OutMsg;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "RejectionCodePrinting => " + exDetail.ToString());
                return "ERROR | " + ex.Message;
            }
        }


        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                (VisualTreeHelper.GetParent(this) as StackPanel).Children.Clear();
            }
            catch (Exception ex)
            {

                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                (VisualTreeHelper.GetParent(this) as StackPanel).Children.Clear();
            }
            catch (Exception ex)
            {
            }
        }
    }
}
