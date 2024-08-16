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
//using Microsoft.Exchange.WebServices.Data;

namespace GREENPLY.UserControls.Transaction
{
    /// <summary>
    /// Interaction logic for UCVendorLabelPrinting.xaml
    /// </summary>

    public partial class UCVendorLabelPrinting : UserControl
    {
        Logger objLog = new Logger();
        BL_VendorPrinting objBL_Vendor = new BL_VendorPrinting();
        BL_HubPrinting objBL_Hub = new BL_HubPrinting();
        WriteLogFile ObjLog = new WriteLogFile();
        DataTable DtPONo;
        DataTable DtPOdata;
        DataTable DtSelectedItems;
        DataTable DtItems;
        DataTable dtPostData;
        string _strPlantCode = string.Empty;
        PL_VendorMaster ObjPLVMaster;
        string sEmailSentStatus = string.Empty;
        string sPONumber = string.Empty;
        string sVendorName = string.Empty;

        int objPOQty = 0;
        int objRemQty = 0;
        int objEnterQty = 0;
        int objPrintedQty = 0;

        string objSMCode = string.Empty;
        string objQRCode = string.Empty;
        string objStackQRCode = string.Empty;
        BcilNetwork _bcilNetwork = new BcilNetwork();

        public UCVendorLabelPrinting()
        {
            InitializeComponent();
            //if (VariableInfo.mPlantCode == "VENDOR" || VariableInfo.mPlantCode == "Vendor")
            //{
            lblDescription.Text = " VENDOR LABEL PRINTING";
            //}
            //else
            //{
            //    lblDescription.Text = " HUB LABEL PRINTING";
            //}
        }

        #region Private Collection

        ObservableCollection<PL_VendorMaster> _PLVendorPOMatData = new ObservableCollection<PL_VendorMaster>();
        public ObservableCollection<PL_VendorMaster> VendorPOMatData
        {
            get { return _PLVendorPOMatData; }
            set
            {
                _PLVendorPOMatData = value;
                OnPropertyChanged("VendorPOMatData");
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
                this.cmbPONum.SelectionChanged -= new SelectionChangedEventHandler(cmbPONum_SelectionChanged);
                _strPlantCode = VariableInfo.mPlantCode;
                GetGeneratedPONumbersPrintingAtVendor();
                cmbPrinter.SelectedIndex = 0;
                this.cmbPONum.SelectionChanged += new SelectionChangedEventHandler(cmbPONum_SelectionChanged);
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "VendorLabelPrinting => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetGeneratedPONumbersPrintingAtVendor()
        {
            try
            {
                objBL_Vendor = new BL_VendorPrinting();
                DtPONo = new DataTable();
                DtPONo = objBL_Vendor.BLGetGeneratedPONumbersPrintingAtVendor();
                DataRow dr = DtPONo.NewRow();
                dr[0] = "--Select--";
                DtPONo.Rows.InsertAt(dr, 0);
                DataView dataView = new DataView(DtPONo);
                this.cmbPONum.ItemsSource = dataView;
                cmbPONum.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
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
                    txtInvNo.Text = string.Empty;
                    dtpInvdate.Text = string.Empty;
                    lv.ItemsSource = null;
                    return;
                }
                //if (VariableInfo.mPlantCode == "VENDOR" || VariableInfo.mPlantCode == "Vendor")
                //{
                objBL_Vendor = new BL_VendorPrinting();
                DtPOdata = new DataTable();
                DtSelectedItems = new DataTable();
                DtPOdata = objBL_Vendor.BLPrintGetVendorPODetails(cmbPONum.SelectedValue.ToString());
                if (DtPOdata.Rows.Count > 0)
                {
                    txtVendorCode.Text = DtPOdata.Rows[0][0].ToString();
                    txtbxVendorname.Text = DtPOdata.Rows[0][1].ToString();
                    //txtInvNo.Text = DtPOdata.Rows[0][2].ToString();
                    //dtpInvdate.Text = DtPOdata.Rows[0][3].ToString();
                    ObjPLVMaster = new PL_VendorMaster();
                    ObjPLVMaster.PONumber = cmbPONum.SelectedValue.ToString();
                    VendorPOMatData = new BUSSINESS_LAYER.BL_VendorPrinting().BLPrintGetVendorPOMatData(ObjPLVMaster);
                    if (VendorPOMatData.Count > 0)
                    {
                        lv.ItemsSource = VendorPOMatData;
                        DtItems = new DataTable();
                        ObservableCollection<PL_VendorMaster> data = (ObservableCollection<PL_VendorMaster>)lv.ItemsSource;
                        DtItems = VariableInfo.ToDataTable(data);
                    }
                }
                else
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "There is no items in this selected PO no. : " + cmbPONum.SelectedValue.ToString(), 2);
                    return;
                }
                //}
                //else
                //{
                //    objBL_Hub = new BL_HubPrinting();
                //    DtPOdata = new DataTable();
                //    DtSelectedItems = new DataTable();
                //    DtPOdata = objBL_Hub.BLPrintGetHubPODetails(cmbPONum.SelectedValue.ToString());
                //    if (DtPOdata.Rows.Count > 0)
                //    {
                //        txtVendorCode.Text = DtPOdata.Rows[0][0].ToString();
                //        txtbxVendorname.Text = DtPOdata.Rows[0][1].ToString();
                //        ObjPLVMaster = new PL_VendorMaster();
                //        ObjPLVMaster.PONumber = cmbPONum.SelectedValue.ToString();
                //        VendorPOMatData = new BUSSINESS_LAYER.BL_VendorPrinting().BLPrintGetVendorPOMatData(ObjPLVMaster);
                //        if (VendorPOMatData.Count > 0)
                //        {
                //            lv.ItemsSource = VendorPOMatData;
                //            //lblPrintCount.Content = lv.Items.Count;
                //            DtItems = new DataTable();
                //            ObservableCollection<PL_VendorMaster> data = (ObservableCollection<PL_VendorMaster>)lv.ItemsSource;
                //            DtItems = VariableInfo.ToDataTable(data);
                //        }
                //    }
                //    else
                //    {
                //        BCommon.setMessageBox(VariableInfo.mApp, "There Is No Items In This Selected PO No.: " + cmbPONum.SelectedValue.ToString(), 2);
                //        return;
                //    }
                //}
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "VendorLabelPrinting => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
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
                PL_VendorMaster objPLVMaster = (PL_VendorMaster)item.Content;
                objPOQty = objPLVMaster.POQty;
                objPrintedQty = objPLVMaster.PrintedQty;
                if (objPLVMaster.IsValid == false)
                    objPLVMaster.IsValid = true;
            }
            catch (Exception ex)
            {
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void txtInvNo_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox currentTextBox = (TextBox)sender;
            if (currentTextBox.IsReadOnly)
                currentTextBox.IsReadOnly = false;
            else
                currentTextBox.IsReadOnly = true;
        }

        private void txtInvNo_TextChanged(object sender, TextChangedEventArgs e)
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

        private void txtInvNo_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox currentTextBox = (TextBox)sender;
            if (currentTextBox.Text == string.Empty)
            {
                BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter the invoice number", 2);
                return;
            }
            if (System.Text.RegularExpressions.Regex.IsMatch(currentTextBox.Text, @"[^a-zA-Z0-9\s]"))
            {
                BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter numbers/characters only", 2);
                currentTextBox.Text = string.Empty;
                return;
            }
            foreach (PL_VendorMaster item in lv.ItemsSource)
            {
                if (item.IsValid == true)
                {
                    string objMatCode = Convert.ToString(item.MatCode);
                    foreach (DataRow row in DtItems.Rows)
                    {
                        if (row["MatCode"].ToString() == objMatCode)
                        {
                            row["InvoiceNo"] = currentTextBox.Text;
                            currentTextBox.IsReadOnly = true;
                        }
                    }
                    item.IsValid = false;
                }
            }
        }

        private void txtInvDate_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            TextBox currentTextBox = (TextBox)sender;
            if (currentTextBox.IsReadOnly)
                currentTextBox.IsReadOnly = false;
            else
                currentTextBox.IsReadOnly = true;
        }

        private void txtInvDate_TextChanged(object sender, TextChangedEventArgs e)
        {
            //TextBox currentTextBox = (TextBox)sender;
            //DateTime Test;
            //if (DateTime.TryParseExact(currentTextBox.Text, "dd-MM-yyyy", null, DateTimeStyles.None, out Test) == true)
            //{
            //    //return;
            //}
            //else
            //{
            //    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Enter Valid Date Format Means dd-MM-yyyy Format", 2);
            //    currentTextBox.Text = string.Empty;
            //    return;
            //}
            //if (!System.Text.RegularExpressions.Regex.IsMatch(currentTextBox.Text, "[0-9-]"))
            //{
            //    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Enter Numbers And - Only for Date Format", 2);
            //    currentTextBox.Text = string.Empty;
            //    return;
            //}
        }

        private void txtInvDate_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBox currentTextBox = (TextBox)sender;
            if (currentTextBox.Text == string.Empty)
            {
                BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter the invoice date", 2);
                return;
            }
            DateTime Test;
            if (DateTime.TryParseExact(currentTextBox.Text, "dd-MM-yyyy", null, DateTimeStyles.None, out Test) == true)
            {
                //return;
            }
            else
            {
                BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter valid date format means dd-MM-yyyy format", 2);
                currentTextBox.Text = string.Empty;
                return;
            }
            foreach (PL_VendorMaster item in lv.ItemsSource)
            {
                if (item.IsValid == true)
                {
                    string objMatCode = Convert.ToString(item.MatCode);
                    foreach (DataRow row in DtItems.Rows)
                    {
                        if (row["MatCode"].ToString() == objMatCode)
                        {
                            row["InvoiceDate"] = currentTextBox.Text;
                            currentTextBox.IsReadOnly = true;
                        }
                    }
                    item.IsValid = false;
                }
            }
        }

        private void chkSelectAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool? bChecked = chkSelectAll.IsChecked;
                foreach (var item in VendorPOMatData)
                {
                    item.IsValid = (bool)bChecked;
                }
                lv.ItemsSource = null;
                lv.ItemsSource = VendorPOMatData;
                lv.UpdateLayout();
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
                if (cmbPONum.Text == string.Empty)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly select the vendor code", 1);
                    return;
                }
                if (txtInvNo.Text == string.Empty || txtInvNo.Text == "")
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter the invoice no. to proceed", 1);
                    return;
                }
                if (dtpInvdate.Text == string.Empty || dtpInvdate.Text == "NULL")
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter the invoice date to proceed", 1);
                    return;
                }
                DataSet dsData = new DataSet();
                objQRCode = string.Empty;
                objStackQRCode = string.Empty;
                string objMatcode = string.Empty;
                string objMatDesc = string.Empty;
                string objMatGrade = string.Empty;
                string objMatGroupDesc = string.Empty;
                string objMatGroup = string.Empty;
                string objMatThickness = string.Empty;
                string objMatSize = string.Empty;
                string objInvNo = string.Empty;
                string objInvDate = string.Empty;
                int oPrintCount = 0;
                int oTotalPrintedQty = 0;
                DtSelectedItems = new DataTable();
                dtPostData = new DataTable();
                dtPostData.Clear();
                dtPostData.Columns.Add("LocationCode");
                dtPostData.Columns.Add("PONo");
                dtPostData.Columns.Add("VendorInvNo");
                dtPostData.Columns.Add("VendorInvDate");
                dtPostData.Columns.Add("VendorCode");
                dtPostData.Columns.Add("MatCode");
                dtPostData.Columns.Add("POQty");
                dtPostData.Columns.Add("QRCode");
                int iCount = 0;
                OperationResult oResponse = OperationResult.SaveError;
                DtSelectedItems = DtItems.Clone();
                btnPrint.Cursor = Cursors.Wait;
                foreach (PL_VendorMaster item1 in lv.ItemsSource)
                {
                    if (item1.IsValid == true)
                    {
                        string objMatCode = Convert.ToString(item1.MatCode);
                        if (DtItems.Rows.Count > 0)
                        {
                            DataRow[] rowsToCopy;
                            rowsToCopy = DtItems.Select("MatCode ='" + objMatCode + "'");
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
                if (lv.Items.Count == iCount)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Select atleast one record to print", 1);
                    btnPrint.Cursor = Cursors.Arrow;
                    return;
                }
                if (DtSelectedItems.Rows.Count > 0)
                {
                    sPONumber = this.cmbPONum.SelectedValue.ToString().Trim();
                    sVendorName = this.txtbxVendorname.Text.ToString().Trim();
                    objBL_Vendor = new BL_VendorPrinting();
                    for (int i = 0; i < DtSelectedItems.Rows.Count; i++)
                    {
                        objMatcode = string.Empty;
                        objMatDesc = string.Empty;
                        objMatGrade = string.Empty;
                        objMatGroupDesc = string.Empty;
                        objMatGroup = string.Empty;
                        objMatThickness = string.Empty;
                        objMatSize = string.Empty;
                        objInvNo = string.Empty;
                        objInvDate = string.Empty;

                        objInvNo = txtInvNo.Text;
                        objInvDate = dtpInvdate.Text;
                        DateTime date = Convert.ToDateTime(objInvDate);
                        string tempdate = date.ToString("dd-MM-yyyy");
                        if (tempdate != objInvDate)
                        {
                            BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter invoice date in format of dd-MM-yyyy only", 1);
                            btnPrint.Cursor = Cursors.Arrow;
                            return;
                        }
                        objMatcode = DtSelectedItems.Rows[i][11].ToString();
                        objMatGrade = DtSelectedItems.Rows[i][17].ToString();
                        objMatGroup = DtSelectedItems.Rows[i][15].ToString();
                        objMatGroupDesc = DtSelectedItems.Rows[i][30].ToString();
                        objMatThickness = DtSelectedItems.Rows[i][16].ToString();
                        objMatSize = DtSelectedItems.Rows[i][13].ToString();
                        ObjLog.WriteLog("(Info) - " + "VendorLabelPrinting => Data sent to DB : LocationCode - " + VariableInfo.mPlantCode.Trim() + ", PONum - " + cmbPONum.SelectedValue.ToString() + ", Matcode : " + objMatcode.ToString() + ", VendorCode - " + txtVendorCode.Text.Trim());
                        DataTable dtData = objBL_Vendor.BLPrintGetPrintMatQRCodeDetails(cmbPONum.SelectedValue.ToString(), objMatcode, txtVendorCode.Text.Trim());
                        ObjLog.WriteLog("(Info) - " + "VendorLabelPrinting => No. of records found : " + dtData.Rows.Count + " for Matcode : " + objMatcode.ToString());
                        if (dtData.Rows.Count > 0)
                        {
                            for (int j = 0; j < dtData.Rows.Count; j++)
                            {
                                string sPrintStatus = string.Empty;
                                objQRCode = dtData.Rows[j][0].ToString();
                                DateTime oDate = Convert.ToDateTime(objInvDate);
                                objInvDate = oDate.ToString("yyyy-MM-dd");
                                sPrintStatus = PrintQRCodeItem(cmbPONum.SelectedValue.ToString(), objMatcode, txtVendorCode.Text.Trim(), objInvNo.Trim(), objInvDate.Trim(), objQRCode.Trim(), objMatGrade.Trim(), objMatGroup.Trim(), objMatGroupDesc.Trim(), objMatThickness.Trim(), objMatSize.Trim());
                                if (sPrintStatus.Contains("SUCCESS"))
                                {
                                    oPrintCount++;
                                    oTotalPrintedQty++;
                                }
                                else if (sPrintStatus.Contains("ERROR"))
                                {
                                    BCommon.setMessageBox(VariableInfo.mApp, sPrintStatus, 1);
                                    btnPrint.Cursor = Cursors.Arrow;
                                    return;
                                }
                                else if (sPrintStatus.Contains("Printer not connected"))
                                {
                                    BCommon.setMessageBox(VariableInfo.mApp, "Printer is not connected, Kindly connect the printer and try again", 1);
                                    btnPrint.Cursor = Cursors.Arrow;
                                    return;
                                }
                            }
                        }
                        else
                        {
                            BCommon.setMessageBox(VariableInfo.mApp, "There is no data found for selected PO number to print, Kindly change", 1);
                            btnPrint.Cursor = Cursors.Arrow;
                            return;
                        }
                    }
                    lv.ItemsSource = null;
                    txtVendorCode.Text = string.Empty;
                    txtbxVendorname.Text = string.Empty;
                    txtInvNo.Text = string.Empty;
                    dtpInvdate.Text = string.Empty;
                    BCommon.setMessageBox(VariableInfo.mApp, oPrintCount + " no. of QRCode printed successfully", 1);
                    btnPrint.Cursor = Cursors.Arrow;
                    this.cmbPONum.SelectionChanged -= new SelectionChangedEventHandler(cmbPONum_SelectionChanged);
                    GetGeneratedPONumbersPrintingAtVendor();
                    this.cmbPONum.SelectionChanged += new SelectionChangedEventHandler(cmbPONum_SelectionChanged);
                    //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                    //    SendMail(sVendorName, sPONumber, oPrintCount.ToString());
                    btnPrint.Cursor = Cursors.Arrow;
                    return;
                }
                btnPrint.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "VendorLabelPrinting => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
                btnPrint.Cursor = Cursors.Arrow;
            }
        }

        public string PrintQRCodeItem(string _strPONo, string sMatCode, string sVendorCode, string sInvNo, string sInvDate, string sQRCode, string sGrade, string sGroup, string sGroupDesc, string sThicknessDesc, string sSize)
        {
            try
            {
                _bcilNetwork = new BcilNetwork();
                objBL_Vendor = new BL_VendorPrinting();
                string OutMsg = string.Empty;
                var sReadPrn = string.Empty;
                bool IsPrinterConnected = false;
                bool IsPrinted = false;
                if (cmbPrinter.SelectedIndex == 0)  //(VariableInfo.mLocationType == "VENDOR" || VariableInfo.mLocationType == "Vendor")
                {
                    PrinterSettings printerName = new PrinterSettings();
                    string printer = printerName.PrinterName;
                    IsPrinterConnected = COMMON.PrintBarcode.IsPrinterAvailable(printer);
                    ObjLog.WriteLog("(Info) - " + "VendorLabelPrinting => Printer : " + printer + " connected status : " + IsPrinterConnected);
                    if (IsPrinterConnected == true)
                    {
                        string sSaveStatus = objBL_Vendor.BLUpdateQRCode(_strPONo.Trim(), sMatCode.Trim(), sVendorCode.Trim(), sInvNo, sInvDate, sQRCode.Trim(), sVendorCode.Trim());
                        if (sSaveStatus == "1")
                        {
                            ObjLog.WriteLog("PONumber - " + sPONumber + " and QRCode - " + sQRCode + " for VendorCode - " + sVendorCode + " and MatCode - " + sMatCode + " saved successfully");
                            StringBuilder sb = new StringBuilder();
                            DataTable dt = new DataTable();
                            string sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyQRCode.PRN";
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
                            if (sGroup != "" && sGroup.Length >= 4)
                                sGroup = sGroup.Substring(sGroup.Length - 4);
                            string objFull = sQRCode.Trim() + "-" + sGrade.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sSize.Trim();
                            string objRest = sGrade.Trim() + "-" + sGroup.Trim() + "-" + sThicknessDesc.Trim() + "-" + sSize.Trim();
                            sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objFull));
                            sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(sQRCode));
                            sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(objRest));
                            _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyQRCode.PRN";

                            IsPrinted = COMMON.PrintBarcode.PrintCommand(sReadPrn, printer);
                            _bcilNetwork.Dispose();
                            if (IsPrinted == true)
                            {
                                ObjLog.WriteLog("PONumber - " + sPONumber + " and QRCode - " + sQRCode + " for VendorCode - " + sVendorCode + " and MatCode - " + sMatCode + " printed successfully");
                                return OutMsg = "SUCCESS~Printed Successfully";
                            }
                        }//
                        else if (sSaveStatus == "0")
                        {
                            ObjLog.WriteLog("PONumber - " + sPONumber + " and QRCode - " + sQRCode + " for VendorCode - " + sVendorCode + " and MatCode - " + sMatCode + " not saved or already printed");
                            return OutMsg = "ERROR ~ " + sSaveStatus;
                        }
                        else
                        {
                            ObjLog.WriteLog("PONumber - " + sPONumber + " and QRCode - " + sQRCode + " for VendorCode - " + sVendorCode + " and MatCode - " + sMatCode + " not updated, Error found - " + sSaveStatus);
                            return OutMsg = "ERROR ~ " + sSaveStatus;
                        }
                    }
                    else
                    {
                        if (OutMsg == "Printer not connected")
                        {
                            ObjLog.WriteLog("ERROR ~ Default Printer : " + printer + " not connected");
                            return OutMsg = "ERROR~Default Printer not connected";
                        }
                        else
                        {
                            ObjLog.WriteLog("ERROR~Printer error is : " + OutMsg);
                            return OutMsg;
                        }
                    }
                }
                else
                {
                    _bcilNetwork.PrinterIP = Properties.Settings.Default.VendorQRCodePrinterIP;
                    _bcilNetwork.PrinterPort = Properties.Settings.Default.VendorQRCodePrinterPort;

                    OutMsg = _bcilNetwork.NetworkPrinterStatus();
                    ObjLog.WriteLog("(Info) - " + "VendorLabelPrinting => Printer IP : " + _bcilNetwork.PrinterIP + ", Status : " + OutMsg);
                    if (OutMsg == "PRINTER READY")
                    {
                        string sSaveStatus = objBL_Vendor.BLUpdateQRCode(_strPONo.Trim(), sMatCode.Trim(), sVendorCode.Trim(), sInvNo, sInvDate, sQRCode.Trim(), sVendorCode.Trim());
                        if (sSaveStatus == "1")
                        {
                            ObjLog.WriteLog("PONumber - " + sPONumber + " and QRCode - " + sQRCode + " for VendorCode - " + sVendorCode + " and MatCode - " + sMatCode + " saved successfully");
                            StringBuilder sb = new StringBuilder();
                            DataTable dt = new DataTable();
                            string sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyQRCode.PRN";
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
                            if (sGroup != "" && sGroup.Length >= 4)
                                sGroup = sGroup.Substring(sGroup.Length - 4);
                            string objFull = sQRCode.Trim() + "-" + sGrade.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sSize.Trim();
                            string objRest = sGrade.Trim() + "-" + sGroup.Trim() + "-" + sThicknessDesc.Trim() + "-" + sSize.Trim();
                            sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objFull));
                            sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(sQRCode));
                            sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(objRest));
                            _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyQRCode.PRN";

                            OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                            OutMsg = "SUCCESS";
                            _bcilNetwork.Dispose();
                            if (OutMsg == "SUCCESS")
                            {
                                ObjLog.WriteLog("PONumber - " + sPONumber + " and QRCode - " + sQRCode + " for VendorCode - " + sVendorCode + " and MatCode - " + sMatCode + " printed successfully");
                                return OutMsg = "SUCCESS~Printed Successfully";
                            }
                        }
                        else if (sSaveStatus == "0")
                        {
                            ObjLog.WriteLog("PONumber - " + sPONumber + " and QRCode - " + sQRCode + " for VendorCode - " + sVendorCode + " and MatCode - " + sMatCode + " not saved or already printed");
                            return OutMsg = "ERROR ~ " + sSaveStatus;
                        }
                        else
                        {
                            ObjLog.WriteLog("PONumber - " + sPONumber + " and QRCode - " + sQRCode + " for VendorCode - " + sVendorCode + " and MatCode - " + sMatCode + " not updated, Error found - " + sSaveStatus);
                            return OutMsg = "ERROR ~ " + sSaveStatus;
                        }
                    }
                    else
                    {
                        if (OutMsg == "Printer not connected")
                        {
                            ObjLog.WriteLog("ERROR ~ Printer IP : " + _bcilNetwork.PrinterIP + " not connected");
                            return OutMsg = "ERROR~Default Printer not connected";
                        }
                        else
                        {
                            ObjLog.WriteLog("ERROR~Printer error is : " + OutMsg);
                            return OutMsg;
                        }
                    }
                }
                return OutMsg;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "VendorLabelPrinting => " + exDetail.ToString());
                return "ERROR | " + ex.Message;
            }
        }

        //public bool SendMail(string sVendorName, string sPONo, string sTotalQty)
        //{
        //    try
        //    {
        //        string MailUser = Properties.Settings.Default.SMTPUser; // "qrcode@greenply.com";
        //        string MailPass = Properties.Settings.Default.SMTPPassword; // "Qrc0d@321";
        //        string MailTo = Properties.Settings.Default.ReceiverEmail; // "ashutosh@barcodeindia.com";
        //        ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2010_SP1);
        //        service.Credentials = new NetworkCredential(MailUser, MailPass);
        //        if (Properties.Settings.Default.UseProxy)
        //        {
        //            WebProxy proxy = new WebProxy(Properties.Settings.Default.ProxyIP.ToString(), Properties.Settings.Default.ProxyPort);
        //            service.WebProxy = proxy;
        //        }
        //        service.Url = new Uri(Properties.Settings.Default.SMTPServer);  //("https://outlook.office365.com/owa");
        //        EmailMessage emailMessage = new EmailMessage(service);
        //        string htmlString = string.Empty;
        //        emailMessage.Subject = Properties.Settings.Default.VendorSMTPSubject + " - " + DateTime.Now.ToString("ddMMyyyy");
        //        htmlString = @"<html>
        //              <body>
        //              <p>Hi,</p>
        //              <p>QR Codes for PONumber - " + sPONo + " for Vendor - " + sVendorName + " with total quantity - " + sTotalQty + @" are printed successfully at the vendor point. .</p>
        //              <p>For any queries, feel free to connect with the Greenply Barcode Team.</p>
        //              <p>Thanks,<br>Greenply Barcode Team</br></p>
        //              </body>
        //              </html>
        //             ";
        //        string msgBody = (htmlString.ToString());
        //        emailMessage.Body = new MessageBody(msgBody);
        //        emailMessage.ToRecipients.Add(MailTo);
        //        emailMessage.Send();
        //        ObjLog.WriteLog("Email sent successfully using SMTP!");
        //        sEmailSentStatus = "Sent";
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        sEmailSentStatus = "Failed";
        //        ObjLog.WriteLog("Email sent Failed at Vendor using SMTP! And Error is : " + ex.Message.ToString());
        //        if (ex.Message != null)
        //            ObjLog.WriteLog(ex.Message);
        //        return false;
        //    }

        //}

        #region Button Clicks

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            lv.ItemsSource = null;
            cmbPONum.SelectedIndex = 0;
            txtVendorCode.Text = string.Empty;
            txtbxVendorname.Text = string.Empty;
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

        #endregion

        private void txtInvDate_SourceUpdated(object sender, DataTransferEventArgs e)
        {

        }

        private void txtInvDate_SourceUpdated_1(object sender, DataTransferEventArgs e)
        {

        }

    }
}
