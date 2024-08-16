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
using System.Collections;
using BCILLogger;
using GREENPLY.GreenplyERPPostingService;
using System.Globalization;
using System.Drawing.Printing;
using System.Linq;

namespace GREENPLY.UserControls.Transaction
{
    /// <summary>
    /// Interaction logic for UCHUBLabelPrinting.xaml
    /// </summary>
    public partial class UCHUBLabelPrinting : UserControl
    {
        Logger objLog = new Logger();
        WriteLogFile ObjLog = new WriteLogFile();
        BcilNetwork _bcilNetwork = new BcilNetwork();
        string _strPlantCode = string.Empty;
        BL_HubPrinting objBL_Hub = new BL_HubPrinting();
        BL_ItemSelection objBLItem = new BL_ItemSelection();
        PL_HubPrinting ObjPLHub;
        DataTable DtPONo;
        DataTable DtPOdata;
        DataTable DtSelectedItems;
        DataTable DtItems;

        int objPOQty = 0;
        int objRemQty = 0;
        int objEnterQty = 0;
        int objPrintedQty = 0;
        int objSelectedIndex;

        string oMonth = string.Empty;
        string oDay = string.Empty;
        string oYear = string.Empty;
        string oDateFormat = string.Empty;
        string sQRRunningSerial;
        string sStackRunningSerial;
        string objQRCode = string.Empty;
        string objStackQRCode = string.Empty;
        string objSMCode = string.Empty;
        private static Random random = new Random();
        public static string sPrintingSection = string.Empty;
        public static string sLocationType = string.Empty;
        string objLocationCode = VariableInfo.mPlantCode.Trim();

        public UCHUBLabelPrinting()
        {
            InitializeComponent();
        }

        #region Private Collection

        ObservableCollection<PL_HubPrinting> _PLHubPOMatData = new ObservableCollection<PL_HubPrinting>();
        public ObservableCollection<PL_HubPrinting> HubPOMatData
        {
            get { return _PLHubPOMatData; }
            set
            {
                _PLHubPOMatData = value;
                OnPropertyChanged("HubPOMatData");
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
                lblSelectAll.Visibility = System.Windows.Visibility.Hidden;
                chkSelectAll.Visibility = System.Windows.Visibility.Hidden;
                sPrintingSection = Properties.Settings.Default.PrintingSection.Trim().ToString();
                sLocationType = Properties.Settings.Default.PrintingLocationType.Trim().ToString();
                this.cmbPONum.SelectionChanged -= new SelectionChangedEventHandler(cmbPONum_SelectionChanged);
                _strPlantCode = VariableInfo.mPlantCode;
                GetSAPHubPONumbers();
                this.cmbPONum.SelectionChanged += new SelectionChangedEventHandler(cmbPONum_SelectionChanged);
                cmbLabelType.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "HUBLabelPrinting : FormLoad => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetSAPHubPONumbers()
        {
            try
            {
                objBL_Hub = new BL_HubPrinting();
                DtPONo = new DataTable();
                DtPONo = objBL_Hub.BLGetSAPHubPONumbers();
                DataRow dr = DtPONo.NewRow();
                dr[0] = "--Select--";
                DtPONo.Rows.InsertAt(dr, 0);
                DataView dataView = new DataView(DtPONo);
                this.cmbPONum.ItemsSource = dataView;
                cmbPONum.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "HUBLabelPrinting : GetSAPHubPONumbers => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void cmbPONum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbPONum.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly select the purchase order no.", 2);
                    cmbPONum.Focus();
                    txtVendorCode.Text = string.Empty;
                    txtbxVendorname.Text = string.Empty;
                    lv.ItemsSource = null;
                    return;
                }
                objBL_Hub = new BL_HubPrinting();
                DtPOdata = new DataTable();
                DtSelectedItems = new DataTable();
                DtPOdata = objBL_Hub.BLPrintGetHubPOVendorDetails(cmbPONum.SelectedValue.ToString());
                if (DtPOdata.Rows.Count > 0)
                {
                    txtVendorCode.Text = DtPOdata.Rows[0][0].ToString();
                    txtbxVendorname.Text = DtPOdata.Rows[0][1].ToString();
                    ObjPLHub = new PL_HubPrinting();
                    ObjPLHub.PONumber = cmbPONum.SelectedValue.ToString();
                    HubPOMatData = new BUSSINESS_LAYER.BL_HubPrinting().BLPrintGetHubPOMatData(ObjPLHub);
                    if (HubPOMatData.Count > 0)
                    {
                        lv.ItemsSource = HubPOMatData;
                        DtItems = new DataTable();
                        ObservableCollection<PL_HubPrinting> data = (ObservableCollection<PL_HubPrinting>)lv.ItemsSource;
                        DtItems = VariableInfo.ToDataTable(data);
                    }
                }
                else
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "There is no items in this selected PO no.: " + cmbPONum.SelectedValue.ToString(), 2);
                    return;
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "HUBLabelPrinting : PONumSelectionChanged => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void chkSelectAll_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    bool? bChecked = chkSelectAll.IsChecked;
            //    foreach (var item in VendorPOMatData)
            //    {
            //        item.IsValid = (bool)bChecked;
            //    }
            //    lv.ItemsSource = null;
            //    lv.ItemsSource = VendorPOMatData;
            //    lv.UpdateLayout();
            //}
            //catch (Exception ex)
            //{
            //    BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            //}
        }

        private void lv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                objPOQty = 0;
                objPrintedQty = 0;
                objRemQty = 0;
                objSMCode = string.Empty;
                ListViewItem item = sender as ListViewItem;
                PL_HubPrinting objPLVMaster = (PL_HubPrinting)item.Content;
                objPOQty = objPLVMaster.POQty;
                objPrintedQty = objPLVMaster.PrintedQty;
                objRemQty = objPLVMaster.RemaningQty;
                objSMCode = objPLVMaster.MatCode;
                if (objPLVMaster.IsValid == false)
                    objPLVMaster.IsValid = true;
            }
            catch (Exception ex)
            {
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void txtQty_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox currentTextBox = (TextBox)sender;
            if (currentTextBox.IsReadOnly)
                currentTextBox.IsReadOnly = false;
            else
                currentTextBox.IsReadOnly = true;
        }

        private void txtQty_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox currentTextBox = (TextBox)sender;
            if (currentTextBox.Text == string.Empty)
                return;
            else if (!System.Text.RegularExpressions.Regex.IsMatch(currentTextBox.Text, "[0-9]"))
            {
                BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter numbers only", 2);
                currentTextBox.Text = string.Empty;
                return;
            }
        }

        private void txtQty_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                //objRemQty = 0;
                objEnterQty = 0;
                //int objExpQty = 0;
                TextBox currentTextBox = (TextBox)sender;
                if (currentTextBox.Text == string.Empty)
                {
                    objEnterQty = 0;
                }
                else
                    objEnterQty = Convert.ToInt32(currentTextBox.Text);
                //objExpQty = ((objPOQty * 10) / 100);
                //objExpQty = objPOQty + objExpQty;
                //objExpQty = Convert.ToInt32(System.Math.Round(Convert.ToDecimal(objExpQty)));

                //objRemQty = (objExpQty - objPrintedQty);
                if (objEnterQty == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter printable quantity for selected material", 1);
                    btnPrint.Cursor = Cursors.Arrow;
                    return;
                }
                if (objEnterQty > objRemQty)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Entered quantity - " + objEnterQty + " can not be greater than remaining quantity - " + objRemQty + ", Kindly change", 2);
                    currentTextBox.Text = string.Empty;
                    return;
                }
                else if (objEnterQty <= objRemQty)
                {
                    objRemQty = (objPOQty - objEnterQty - objPrintedQty);
                    foreach (PL_HubPrinting item in lv.ItemsSource)
                    {
                        string objMatCode = objSMCode.Trim();   //Convert.ToString(item.MatCode);
                        foreach (DataRow row in DtItems.Rows)
                        {
                            if (row["MatCode"].ToString() == objMatCode)
                            {
                                if (currentTextBox.IsReadOnly == false)
                                {
                                    row["RemaningQty"] = objRemQty;
                                    row["PrintableQty"] = objEnterQty;
                                    currentTextBox.IsReadOnly = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ObjLog.WriteLog("(Error) - " + "HUBLabelPrinting : txtQtyLostFocus => " + ex.ToString());
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

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            lv.ItemsSource = null;
            this.cmbPONum.SelectionChanged -= new SelectionChangedEventHandler(cmbPONum_SelectionChanged);
            cmbPONum.SelectedIndex = 0;
            this.cmbPONum.SelectionChanged += new SelectionChangedEventHandler(cmbPONum_SelectionChanged);
            txtVendorCode.Text = string.Empty;
            txtbxVendorname.Text = string.Empty;
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
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
                string objSMTPHost = string.Empty;
                string objVendorEMail = string.Empty;
                string objVendorName = string.Empty;
                string objVPassword = string.Empty;
                string objPortNo = string.Empty;
                string objSubject = string.Empty;
                string objInvNo = string.Empty;
                string objInvDate = string.Empty;
                int oPOQty = 0;
                int oPrintedQty = 0;
                int oPrintableQty = 0;
                int oTotalGenQty = 0;
                int oLotQty = 0;

                DtSelectedItems = new DataTable();
                int iCount = 0;
                OperationResult oResponse = OperationResult.SaveError;
                DtSelectedItems = DtItems.Clone();
                btnPrint.Cursor = Cursors.Wait;
                for (int i = 0; i < DtItems.Rows.Count; i++)
                {
                    string objMatCode = DtItems.Rows[i]["MatCode"].ToString();   //Convert.ToString(item1.MatCode);
                    if (Convert.ToInt32(DtItems.Rows[i]["PrintableQty"]) != 0)
                    {
                        DataRow[] rowsToCopy;
                        rowsToCopy = DtItems.Select("MatCode = '" + objMatCode + "'");
                        foreach (DataRow temp in rowsToCopy)
                        {
                            DtSelectedItems.ImportRow(temp);
                        }
                    }
                    else
                    {
                        iCount++;
                        continue;
                    }
                }
                if (lv.Items.Count == iCount)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter printable quantity to atleast against one material to proceed", 1);
                    btnPrint.Cursor = Cursors.Arrow;
                    return;
                }
                if (DtSelectedItems.Rows.Count > 0)
                {
                    if ((txtInvNo.Text == string.Empty || txtInvNo.Text == "") && (dtpInvdate.Text == string.Empty || dtpInvdate.Text == ""))
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter the vendor invoice number and invoice date", 2);
                        btnPrint.Cursor = Cursors.Arrow;
                        return;
                    }
                    else if ((txtInvNo.Text == string.Empty || txtInvNo.Text == "") && (dtpInvdate.Text != string.Empty || dtpInvdate.Text != ""))
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter the vendor invoice number", 2);
                        btnPrint.Cursor = Cursors.Arrow;
                        return;
                    }
                    else if ((txtInvNo.Text != string.Empty || txtInvNo.Text != "") && (dtpInvdate.Text == string.Empty || dtpInvdate.Text == ""))
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter the vendor invoice date", 2);
                        btnPrint.Cursor = Cursors.Arrow;
                        return;
                    }
                    else if ((txtInvNo.Text != string.Empty || txtInvNo.Text != "") && (dtpInvdate.Text != string.Empty || dtpInvdate.Text != ""))
                    {
                        objInvNo = "";
                        objInvDate = "";
                        objInvNo = txtInvNo.Text;
                        objInvDate = dtpInvdate.Text;
                        //if (dtpInvdate.Text != string.Empty || dtpInvdate.Text != "")
                        //{
                        DateTime date = Convert.ToDateTime(objInvDate);
                        string tempdate = date.ToString("dd-MM-yyyy");
                        if (tempdate != objInvDate)
                        {
                            BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter invoice date in format of dd-MM-yyyy only", 1);
                            btnPrint.Cursor = Cursors.Arrow;
                            return;
                        }
                        objBL_Hub = new BL_HubPrinting();
                        oDay = oMonth = oYear = oDateFormat = string.Empty;
                        oDay = DateTime.Now.ToString("dd");
                        oMonth = DateTime.Now.ToString("MM");
                        oYear = DateTime.Now.ToString("yy");
                        oDateFormat = oDay.Trim() + oMonth.Trim() + oYear.Trim();
                        _bcilNetwork = new BcilNetwork();

                        string sMatchGroup = string.Empty;
                        string oLabelType = cmbLabelType.Text.ToString();

                        DataTable dtMGroupData = new DataTable();
                        dtMGroupData = objBL_Hub.BLGetUnbrandedMatGroups(VariableInfo.mPlantCode.Trim().ToString());
                        if (dtMGroupData.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtMGroupData.Rows.Count; i++)
                            {
                                string sMGroup = dtMGroupData.Rows[i][0].ToString().Trim();
                                for (int j = 0; j < DtSelectedItems.Rows.Count; j++)
                                {
                                    objMatGroup = DtSelectedItems.Rows[j]["MatGroup"].ToString();
                                    if (sMGroup == objMatGroup)
                                    {
                                        sMatchGroup = sMGroup;
                                    }
                                }
                            }
                        }
                        if (sMatchGroup != string.Empty)
                        {
                            if (oLabelType == "2X2 Label")
                            {
                                BCommon.setMessageBox(VariableInfo.mApp, "You can not print 2X2 labels for material group " + objMatGroup + ", Kindly check again with PO Matcode", 3);
                                return;
                            }
                        }
                        if (sMatchGroup == string.Empty)
                        {
                            if (oLabelType == "2X1 Label")
                            {
                                BCommon.setMessageBox(VariableInfo.mApp, "You can not print 2X1 labels for material group " + objMatGroup + ", Kindly change again with PO Matcode", 3);
                                return;
                            }
                        }
                        string OutMsg = string.Empty;
                        _bcilNetwork.PrinterIP = Properties.Settings.Default.ExistingQRCodePrinterIP;
                        _bcilNetwork.PrinterPort = Properties.Settings.Default.ExistingQRCodePrinterPort;
                        OutMsg = _bcilNetwork.NetworkPrinterStatus();
                        if (OutMsg == "PRINTER READY")
                        {
                            for (int i = 0; i < DtSelectedItems.Rows.Count; i++)
                            {
                                objMatcode = DtSelectedItems.Rows[i]["MatCode"].ToString();
                                objMatDesc = DtSelectedItems.Rows[i]["MatDesc"].ToString();
                                objMatGrade = DtSelectedItems.Rows[i]["MatGrade"].ToString();
                                objMatGradeDesc = DtSelectedItems.Rows[i]["MatGradeDesc"].ToString();
                                objMatGroup = DtSelectedItems.Rows[i]["MatGroup"].ToString();
                                if (objMatGroup != "" && objMatGroup.Length >= 4)
                                    objMatGroup = objMatGroup.Substring(objMatGroup.Length - 4);
                                objMatGroupDesc = DtSelectedItems.Rows[i]["MatGroupDesc"].ToString();
                                objMatThickness = DtSelectedItems.Rows[i]["MatThickness"].ToString();
                                objMatThicknessDesc = DtSelectedItems.Rows[i]["MatThicknessDesc"].ToString();
                                objMatSize = DtSelectedItems.Rows[i]["MatSize"].ToString();
                                oPOQty = Convert.ToInt32(DtSelectedItems.Rows[i]["POQty"].ToString());
                                oPrintedQty = Convert.ToInt32(DtSelectedItems.Rows[i]["PrintedQty"].ToString());
                                oPrintableQty = Convert.ToInt32(DtSelectedItems.Rows[i]["PrintableQty"].ToString());
                                if (oPrintableQty == 0)
                                    oPrintableQty = Convert.ToInt32(DtSelectedItems.Rows[i]["POQty"].ToString());
                                DateTime oDate = Convert.ToDateTime(objInvDate);
                                objInvDate = oDate.ToString("yyyy-MM-dd");

                                for (int j = 0; j < oPrintableQty; j++)
                                {
                                    string objRanNo = RandomString(2);
                                    DataTable dtSerial = objBL_Hub.BLGetQRCodeRunningSerialNo(oDateFormat, sPrintingSection, sLocationType);
                                    if (dtSerial.Columns.Contains("SERIALNO") && dtSerial.Rows.Count > 0)
                                    {
                                        sQRRunningSerial = dtSerial.Rows[0][0].ToString();
                                        if (sQRRunningSerial == string.Empty || sQRRunningSerial == "" || sQRRunningSerial == "0")
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

                                        objQRCode = _strPlantCode.Trim() + oDateFormat.Trim() + objRanNo.Trim() + sQRRunningSerial;

                                        PL_HubPrinting _objVPrint = new PL_HubPrinting();
                                        {
                                            _objVPrint.PONumber = this.cmbPONum.SelectedValue.ToString().Trim();
                                            _objVPrint.MatCode = objMatcode.Trim();
                                            _objVPrint.MatStatus = "H";
                                            _objVPrint.QRCode = objQRCode.Trim();
                                            _objVPrint.PrintedQty = Convert.ToInt32((oPrintedQty + 1));
                                            _objVPrint.DateFormat = oDateFormat.Trim();
                                            _objVPrint.PrintingLocationType = sLocationType;
                                            _objVPrint.PrintingSection = sPrintingSection;
                                            _objVPrint.VendorInvoiceNo = objInvNo;
                                            _objVPrint.VendorInvoiceDate = objInvDate;
                                            _objVPrint.VendorCode = txtVendorCode.Text.Trim().ToString();
                                            _objVPrint.CreatedBy = Convert.ToString(VariableInfo.dtVlidateuser) != "" ? Convert.ToString(VariableInfo.dtVlidateuser.Rows[0]["USER_ID"]) : "";
                                            oResponse = new BL_HubPrinting().SaveHUBQRCode(_objVPrint);
                                            if (oResponse == OperationResult.SaveSuccess)
                                            {
                                                oPrintedQty++;
                                                string sPrintStatus = PrintQRCodeItem(objQRCode, objMatGrade, objMatGroup, objMatGroupDesc, objMatThicknessDesc, objMatSize, oLabelType);
                                                if (sPrintStatus.Contains("SUCCESS"))
                                                {
                                                    oTotalGenQty++;
                                                    ObjLog.WriteLog("Total Qty- " + oTotalGenQty);
                                                }
                                                else if (sPrintStatus.Contains("ERROR"))
                                                {
                                                    BCommon.setMessageBox(VariableInfo.mApp, "Hub Label QRCode Printing Error - " + sPrintStatus + ", Kindly check ", 2);
                                                }
                                            }
                                            else if (oResponse == OperationResult.Duplicate)
                                            {
                                                objQRCode = "";
                                                string objRanNo2 = RandomString(2);
                                                objQRCode = _strPlantCode.Trim() + oDateFormat.Trim() + objRanNo2.Trim() + sQRRunningSerial;
                                                _objVPrint.QRCode = objQRCode.Trim();
                                                oResponse = new BL_HubPrinting().SaveHUBQRCode(_objVPrint);
                                            }
                                        };
                                    }
                                }
                                if (oLabelType == "2X2 Label")
                                {
                                    if (oTotalGenQty == oPrintableQty)
                                    {
                                        sStackRunningSerial = objBL_Hub.BLGetStackRunningSerialNo(oDateFormat, sPrintingSection, sLocationType);
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

                                        objStackQRCode = objLocationCode.Trim() + oDateFormat.Trim() + sStackRunningSerial;
                                        ObjLog.WriteLog("Printed Quantity = " + oTotalGenQty);
                                        string PrintStatus = PrintStackQRCodeItem(objLocationCode, objMatcode, objStackQRCode, oDateFormat, sPrintingSection, sLocationType, objMatGradeDesc.Trim(), objMatGroupDesc.Trim(), objMatThicknessDesc.Trim(), objMatSize.Trim().ToString(), oPrintableQty.ToString(), cmbPONum.SelectedValue.ToString().Trim());
                                        if (PrintStatus.Contains("SUCCESS"))
                                        {
                                            ObjLog.WriteLog("Stack QRCode - " + objStackQRCode + " is Printed with total quantity " + oTotalGenQty + " - No. of Records Posted to SAP for Material Code - " + objMatcode + " Successfully");
                                            BCommon.setMessageBox(VariableInfo.mApp, "Stack QRCode - " + objStackQRCode + " is Printed  And " + oTotalGenQty + " - No. of Records Posted to SAP for Material Code - " + objMatcode + " Successfully ", 2);
                                        }
                                        else if (PrintStatus.Contains("ERROR"))
                                        {
                                            BCommon.setMessageBox(VariableInfo.mApp, "Hub Label Stack QRCode Printing Error - " + PrintStatus + ", Kindly check", 2);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (OutMsg == "PRINTER NOT IN NETWORK")
                            {
                                ObjLog.WriteLog("(Error) - " + "HUBLabelPrinting : PrintQRCodeItem => " + "ERROR ~ HUBLabel QRCode Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network");
                                BCommon.setMessageBox(VariableInfo.mApp, "HUBLabel QRCode Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network", 3);
                            }
                            else
                            {
                                ObjLog.WriteLog("(Error) - " + "HUBLabelPrinting : PrintQRCodeItem => " + "ERROR ~ HUBLabel QRCode Printer IP : " + _bcilNetwork.PrinterIP + "found error, Error is - " + OutMsg);
                                BCommon.setMessageBox(VariableInfo.mApp, "HUBLabel QRCode Printer IP : " + _bcilNetwork.PrinterIP + " found error, Error is - " + OutMsg, 3);
                            }
                        }
                    }
                    _bcilNetwork.Dispose();
                    ObjLog.WriteLog("(Info) - " + "HUBLabelPrinting : PrintQRCodeItem => Total " + oTotalGenQty + " No. of QRCodes are printed successfully");
                    BCommon.setMessageBox(VariableInfo.mApp, "Total " + oTotalGenQty + " No. of QRCodes are printed successfully", 1);
                    //oPrintableQty = 0;
                    oPrintedQty = 0;
                    oTotalGenQty = 0;
                    cmbLabelType.SelectedIndex = 0;
                    lv.ItemsSource = null;
                    this.cmbPONum.SelectionChanged -= new SelectionChangedEventHandler(cmbPONum_SelectionChanged);
                    GetSAPHubPONumbers();
                    txtVendorCode.Text = string.Empty;
                    txtbxVendorname.Text = string.Empty;
                    txtInvNo.Text = string.Empty;
                    dtpInvdate.Text = string.Empty;
                    this.cmbPONum.SelectionChanged += new SelectionChangedEventHandler(cmbPONum_SelectionChanged);
                    btnPrint.Cursor = Cursors.Arrow;  //
                }
            }
            catch (Exception ex)
            {
                btnPrint.Cursor = Cursors.Arrow;
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "HUBLabelPrinting : PrintQRCodeItem => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string PrintQRCodeItem(string sQRCode, string sGrade, string sGroup, string sGroupDesc, string sThicknessDesc, string sSize, string LabelType)
        {
            try
            {
                //_bcilNetwork = new BcilNetwork();
                string OutMsg = string.Empty;
                var sReadPrn = string.Empty;
                string sPrnExist = string.Empty;
                //_bcilNetwork.PrinterIP = Properties.Settings.Default.ExistingQRCodePrinterIP;
                //_bcilNetwork.PrinterPort = Properties.Settings.Default.ExistingQRCodePrinterPort;

                //OutMsg = _bcilNetwork.NetworkPrinterStatus();
                //if (OutMsg == "PRINTER READY")
                //{
                StringBuilder sb = new StringBuilder();
                DataTable dt = new DataTable();
                if (LabelType == "2X2 Label")
                {
                    sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyQRCode.PRN";
                }
                if (LabelType == "2X1 Label")
                {
                    sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyHubQRCode.PRN";
                }
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
                if (LabelType == "2X2 Label")
                {
                    string objRest = sGrade.Trim() + "-" + sGroup.Trim() + "-" + sThicknessDesc.Trim() + "-" + sSize.Trim();
                    string objFull = sQRCode.Trim() + "-" + sGrade.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sSize.Trim();
                    sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objFull.Trim()));
                    sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(sQRCode.Trim()));
                    sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(objRest.Trim()));
                    _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyQRCode.PRN";
                }
                else if (LabelType == "2X1 Label")
                {
                    string objRest = sGrade + "-" + sGroupDesc + "-" + sThicknessDesc + "-" + sSize;
                    string objFull = sQRCode + "-" + objRest;
                    sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objFull));
                    sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(sQRCode));
                    sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(sGrade + "-" + sGroup));
                    sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(sThicknessDesc + "-" + sSize));
                    _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyHubQRCode.PRN";
                }
                OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                //OutMsg = "SUCCESS";
                //_bcilNetwork.Dispose();
                if (OutMsg == "SUCCESS")
                {
                    ObjLog.WriteLog("(Info) - " + "HUBLabelPrinting : PrintQRCodeItem => " + "QRCode - " + sQRCode + " saved and printed successfully");
                    return OutMsg = "SUCCESS ~ Printed Successfully";
                }
                else
                {
                    ObjLog.WriteLog("ERROR ~ Printing error is : " + OutMsg);
                    OutMsg = "ERROR ~ Printing error is : " + OutMsg;
                    return OutMsg;
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "HUBLabelPrinting : PrintQRCodeItem => " + ex.ToString());
                return "ERROR | " + ex.Message;
            }
        }

        public string PrintStackQRCodeItem(string objLocationCode, string objMatCode, string objStackQRCode, string sDateFormat, string sPrintingSection, string sLocationType, string GradeDesc, string GroupDesc, string ThicknessDesc, string MatSize, string LotSize, string sPONo)
        {
            try
            {
                _bcilNetwork = new BcilNetwork();
                objBLItem = new BL_ItemSelection();
                string OutMsg = string.Empty;
                var sReadPrn = string.Empty;

                _bcilNetwork.PrinterIP = Properties.Settings.Default.ExistingStackQRCodePrinterIP;
                _bcilNetwork.PrinterPort = Properties.Settings.Default.ExistingStackQRCodePrinterPort;
                OutMsg = _bcilNetwork.NetworkPrinterStatus();
                if (OutMsg == "PRINTER READY")
                {
                    string _sMatStatus = "H";
                    string sSaveStatus = objBLItem.BLSaveStackQRCode(objLocationCode.Trim(), objMatCode.Trim(), objStackQRCode.Trim(), sDateFormat.Trim(), sPrintingSection, sLocationType, _sMatStatus);
                    if (sSaveStatus == "SUCCESS")
                    {

                        StringBuilder sb = new StringBuilder();
                        DataTable dt = new DataTable();
                        string sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyStackQRCode.PRN";
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
                        string ObjCode2 = GradeDesc.Trim() + "-" + GroupDesc.Trim() + "-" + ThicknessDesc.Trim() + "-" + MatSize.Trim() + "-" + LotSize.Trim() + "Nos";
                        sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objStackQRCode.Trim()));
                        sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(objStackQRCode.Trim()));
                        sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(ObjCode2.Trim()));
                        _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyStackQRCode.PRN";
                        for (int i = 0; i < 2; i++)
                        {
                            //OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                        }
                        _bcilNetwork.Dispose();
                        OutMsg = "SUCCESS";
                        if (OutMsg == "SUCCESS")
                        {
                            ObjLog.WriteLog("(Info) - HubLabelPrinting : PrintingStackQRCode => " + "StackQRCode - " + objStackQRCode + " saved and printed successfully");
                            return OutMsg = "SUCCESS";
                        }
                    }
                    else if (sSaveStatus == "ERROR")
                    {
                        ObjLog.WriteLog("(Error) - " + "HubLabelPrinting : PrintingStackQRCode => " + "StackQRCode - " + objStackQRCode + " Not Updated");
                        return OutMsg;
                    }
                }
                else
                {
                    if (OutMsg == "PRINTER NOT IN NETWORK")
                    {
                        ObjLog.WriteLog("(Error) - " + "HubLabelPrinting : PrintingStackQRCode => " + "ERROR ~ Stack QRCode Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network");
                        OutMsg = "ERROR ~ Stack QRCode Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network";
                        return OutMsg;
                    }
                    else
                    {
                        ObjLog.WriteLog("(Error) - " + "HubLabelPrinting : PrintingStackQRCode => " + "ERROR ~ Stack QRCode Printer IP : " + _bcilNetwork.PrinterIP + " found error, Error - " + OutMsg);
                        OutMsg = "ERROR ~ Stack QRCode Printer IP : " + _bcilNetwork.PrinterIP + " found error, Error - " + OutMsg;
                        return OutMsg;
                    }
                }
                return OutMsg;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "HubLabelPrinting : PrintingStackQRCode => " + exDetail.ToString());
                return "ERROR | " + ex.Message;
            }
        }

    }
}
