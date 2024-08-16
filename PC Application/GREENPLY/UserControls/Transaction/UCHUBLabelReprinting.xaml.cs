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

namespace GREENPLY.UserControls.Transaction
{
    /// <summary>
    /// Interaction logic for UCHUBLabelReprinting.xaml
    /// </summary>
    public partial class UCHUBLabelReprinting : UserControl
    {
        BcilNetwork _bcilNetwork = new BcilNetwork();
        WriteLogFile ObjLog = new WriteLogFile();
        BL_ItemSelection objBLItem;
        PLReprinting ObjPLReprint;
        DataTable DtSQRCode;
        DataTable dtStackData;
        DataTable DtItems;
        DataTable DtSelectedItems;
        Logger objLog = new Logger();
        string objSQRCode = string.Empty;
        string objQRCode = string.Empty;
        string objStackQRCode = string.Empty;
        DataSet dsProdData = new DataSet();
        int oSAPPostCount = 0;
        string Fromdate = string.Empty;
        string Todate = string.Empty;
        int PrintStatuses = 0;

        public UCHUBLabelReprinting()
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
                dtpTodate.SelectedDate = DateTime.Now.Date;
                dtpFromdate.SelectedDate = DateTime.Now.Date; //DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "RePrinting => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                objQRCode = string.Empty;
                objStackQRCode = string.Empty;
                DataTable objMatData = new DataTable();
                DtSelectedItems = new DataTable();
                int iCount = 0;
                int iPrintCount = 0;
                string PrintStatus = string.Empty;
                DtSelectedItems = DtItems.Clone();
                foreach (PLReprinting item1 in lv.ItemsSource)
                {
                    if (item1.IsValid == true)
                    {
                        string objMatCode = Convert.ToString(item1.MatCode);
                        string objMQRCode = Convert.ToString(item1.QRCode);
                        if (DtItems.Rows.Count > 0)
                        {
                            DataRow[] rowsToCopy;
                            rowsToCopy = DtItems.Select("MatCode ='" + objMatCode + "' AND QRCode = '" + objMQRCode + "'");
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
                MessageBoxResult MessResult = MessageBox.Show("Are you sure, you want to print selected QRCodes?", "Save Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (MessResult == MessageBoxResult.No)
                {
                    return;
                }
                if (MessResult == MessageBoxResult.Yes)
                {
                    if (DtSelectedItems.Rows.Count > 0)
                    {
                        for (int i = 0; i < DtSelectedItems.Rows.Count; i++)
                        {
                            string sGrade = DtSelectedItems.Rows[i][7].ToString();
                            string sGroup = DtSelectedItems.Rows[i][11].ToString();
                            if (sGroup != "" && sGroup.Length >= 4)
                                sGroup = sGroup.Substring(sGroup.Length - 4);
                            string sGroupDesc = DtSelectedItems.Rows[i][12].ToString();
                            string sThicknessDesc = DtSelectedItems.Rows[i][5].ToString();
                            string sSize = DtSelectedItems.Rows[i][6].ToString();
                            string objoQRCode = DtSelectedItems.Rows[i][22].ToString();
                            PrintStatus = PrintQRCodeItem(sGrade, sGroup, sGroupDesc, sThicknessDesc, sSize, objoQRCode);
                            if (PrintStatus.Contains("SUCCESS"))
                            {
                                iPrintCount++;
                                ObjLog.WriteLog("HubLabelReprinting => QRCode - " + objQRCode + " reprinted successfully");
                            }
                            else if (PrintStatus.Contains("ERROR"))
                            {
                                BCommon.setMessageBox(VariableInfo.mApp, PrintStatus.ToString(), 2);
                            }
                        }
                    }
                    BCommon.setMessageBox(VariableInfo.mApp, "Total - " + iPrintCount + " no. of QRCode reprinted successfully", 2);
                }
                btnPrint.Cursor = Cursors.Arrow;
                Clear();
                return;
            }
            catch (Exception ex)
            {
                Clear();
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "HubLabelReprinting => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        public string PrintQRCodeItem(string sGrade, string sGroup, string sGroupDesc, string sThicknessDesc, string sSize, string objQRCode)
        {
            try
            {
                _bcilNetwork = new BcilNetwork();
                objBLItem = new BL_ItemSelection();
                string OutMsg = string.Empty;
                var sReadPrn = string.Empty;

                _bcilNetwork.PrinterIP = Properties.Settings.Default.ExistingQRCodePrinterIP;
                _bcilNetwork.PrinterPort = Properties.Settings.Default.ExistingQRCodePrinterPort;

                OutMsg = _bcilNetwork.NetworkPrinterStatus();
                if (OutMsg == "PRINTER READY")
                {
                    StringBuilder sb = new StringBuilder();
                    DataTable dt = new DataTable();
                    string sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyHubQRCode.PRN";
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
                    string objRest = sGrade + "-" + sGroupDesc + "-" + sThicknessDesc + "-" + sSize;
                    string objFull = objQRCode + "-" + objRest;
                    sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objFull));
                    sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(objQRCode));
                    sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(sGrade + "-" + sGroup));
                    sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(sThicknessDesc + "-" + sSize));
                    _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyHubQRCode.PRN";

                    OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                    //OutMsg = "SUCCESS";
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
                        ObjLog.WriteLog("ERROR ~ HubLabelReprinting => ERROR ~ Hub Label Reprinting QRCode Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network");
                        OutMsg = "ERROR ~ Hub Label Reprinting QRCode Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network";
                        return OutMsg;
                    }
                    else
                    {
                        ObjLog.WriteLog("ERROR ~ HubLabelReprinting => ERROR ~ Hub Label Reprinting QRCode Printer IP : " + _bcilNetwork.PrinterIP + " found error, Error - " + OutMsg);
                        OutMsg = "ERROR ~ Hub Label Reprinting QRCode Printer IP : " + _bcilNetwork.PrinterIP + " found error, Error - " + OutMsg;
                        return OutMsg;
                    }
                }
                return OutMsg;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "HubReprinting => " + exDetail.ToString());
                return "ERROR | " + ex.Message;
            }
        }

        private void Clear()
        {
            try
            {
                this.cmbPONo.SelectionChanged -= new SelectionChangedEventHandler(cmbPONo_SelectionChanged);
                cmbPONo.SelectedIndex = 0;
                this.cmbPONo.SelectionChanged += new SelectionChangedEventHandler(cmbPONo_SelectionChanged);
                lv.ItemsSource = null;
            }
            catch (Exception ex)
            {

            }

        }

        private void chkSelectAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool? bChecked = chkSelectAll.IsChecked;
                foreach (var item in _PLReprinting)
                {
                    item.IsValid = (bool)bChecked;
                }
                lv.ItemsSource = null;
                lv.ItemsSource = _PLReprinting;
                lv.UpdateLayout();
            }
            catch (Exception ex)
            {
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clear();
                //cmbStackQRCode.ItemsSource = null;
            }
            catch (Exception ex)
            {

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

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                (VisualTreeHelper.GetParent(this) as StackPanel).Children.Clear();
            }
            catch (Exception ex)
            {
            }
        }

        private void btnGetDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtpFromdate.Text == "")
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Please select from date", 1);
                    return;
                }
                if (dtpTodate.Text == "")
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Please select to date", 1);
                    return;
                }
                DateTime FromDate = Convert.ToDateTime(dtpFromdate.Text);
                DateTime ToDate = Convert.ToDateTime(dtpTodate.Text);
                if (FromDate > ToDate)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "From date is greater than To date, Please select date between range", 1);
                    return;
                }
                else
                {
                    PLReprinting _PlReprinting = new ENTITY_LAYER.PLReprinting();
                    _PlReprinting.LocationCode = VariableInfo.mPlantCode.Trim();
                    _PlReprinting.Fromdate = Convert.ToDateTime(dtpFromdate.Text).ToString("yyyy-MM-dd");
                    _PlReprinting.Todate = Convert.ToDateTime(dtpTodate.Text).ToString("yyyy-MM-dd");
                    GetPrintedHUBPOs(_PlReprinting);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "RePrinting => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetPrintedHUBPOs(PLReprinting _ACDetails)
        {
            try
            {
                DtSQRCode = new DataTable();
                DtSQRCode = new BUSSINESS_LAYER.BL_ItemSelection().BLGetPrintedHUBPOData(_ACDetails);
                if (DtSQRCode.Rows.Count > 0)
                {
                    DataRow dr = DtSQRCode.NewRow();
                    dr[0] = "--Select--";
                    DtSQRCode.Rows.InsertAt(dr, 0);
                    DataView dataView = new DataView(DtSQRCode);
                    this.cmbPONo.ItemsSource = null;
                    this.cmbPONo.ItemsSource = dataView;
                    this.cmbPONo.SelectionChanged -= new SelectionChangedEventHandler(cmbPONo_SelectionChanged);
                    cmbPONo.SelectedIndex = 0;
                    this.cmbPONo.SelectionChanged += new SelectionChangedEventHandler(cmbPONo_SelectionChanged);
                }
                else
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "There is no PO numbers found in selected date range, Kindly change", 2);
                    return;
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "HubRePrinting => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void cmbPONo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                DtSQRCode = new DataTable();
                string sPONO = cmbPONo.SelectedValue.ToString();
                DtSQRCode = new BUSSINESS_LAYER.BL_ItemSelection().BLGetSelectedPOMatDetails(sPONO);
                if (DtSQRCode.Rows.Count > 0)
                {
                    DataRow dr = DtSQRCode.NewRow();
                    dr[0] = "--Select--";
                    DtSQRCode.Rows.InsertAt(dr, 0);
                    DataView dataView = new DataView(DtSQRCode);
                    this.cmbMatcode.ItemsSource = null;
                    this.cmbMatcode.ItemsSource = dataView;
                    this.cmbMatcode.SelectionChanged -= new SelectionChangedEventHandler(cmbMatcode_SelectionChanged);
                    cmbMatcode.SelectedIndex = 0;
                    this.cmbMatcode.SelectionChanged += new SelectionChangedEventHandler(cmbMatcode_SelectionChanged);

                }
                else
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "There is no material codes found in selected PO number - " + sPONO + ", Kindly change", 2);
                    return;
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "HUBRePrinting => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void cmbMatcode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbPONo.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly select the PO number", 2);
                    cmbPONo.Focus();
                    btnPrint.IsEnabled = false;
                    return;
                }
                if (cmbMatcode.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly select the material code", 2);
                    cmbMatcode.Focus();
                    btnPrint.IsEnabled = false;
                    return;
                }
                else
                {
                    lv.ItemsSource = null;
                    objBLItem = new BL_ItemSelection();
                    ObjPLReprint = new PLReprinting();
                    ObjPLReprint.PONumber = cmbPONo.SelectedValue.ToString();
                    ObjPLReprint.MatCode = cmbMatcode.SelectedValue.ToString();
                    PLReprinting = new BUSSINESS_LAYER.BL_ItemSelection().BLGetSelectedMatDetails(ObjPLReprint);
                    if (PLReprinting.Count > 0)
                    {
                        PrintStatuses = 1;
                        lv.ItemsSource = PLReprinting;
                        lblCount.Content = lv.Items.Count;
                        DtItems = new DataTable();
                        ObservableCollection<PLReprinting> data = (ObservableCollection<PLReprinting>)lv.ItemsSource;
                        DtItems = VariableInfo.ToDataTable(data);
                    }
                    else
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "There is no data found for selected matcode - " + ObjPLReprint.MatCode + " Kindly change", 2);
                        cmbMatcode.Focus();
                        btnPrint.IsEnabled = false;
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "HubRePrinting => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }


    }
}
