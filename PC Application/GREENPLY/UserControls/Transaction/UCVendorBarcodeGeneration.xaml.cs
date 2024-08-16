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
using BCILLogger;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Mail;
//using Microsoft.Exchange.WebServices;
//using Microsoft.Exchange;
//using Microsoft.Exchange.WebServices.Data;

namespace GREENPLY.UserControls.Transaction
{
    /// <summary>
    /// Interaction logic for UCVendorBarcodeGeneration.xaml
    /// </summary>
    public partial class UCVendorBarcodeGeneration : UserControl
    {
        Logger objLog = new Logger();
        WriteLogFile ObjLog = new WriteLogFile();
        DataTable DtPONo;
        DataTable DtPOdata;
        DataTable DtItems;
        DataTable DtSelectedItems;
        string objSMCode = string.Empty;
        string objQRCode = string.Empty;
        string objStackQRCode = string.Empty;
        string ftpAddress = string.Empty;
        string ftpUserid = string.Empty;
        string ftppassword = string.Empty;
        string _strPlantCode = string.Empty;
        string oMonth = string.Empty;
        string oDay = string.Empty;
        string oYear = string.Empty;
        string oDateFormat = string.Empty;
        string sQRRunningSerial;
        BL_VendorPrinting objBL_Vendor;
        int objPOQty = 0;
        int objRemQty = 0;
        int objEnterQty = 0;
        int objPrintedQty = 0;
        PL_VendorMaster objBLVendor;
        private static Random random = new Random();
        public static string sPrintingSection = string.Empty;
        public static string sLocationType = string.Empty;
        string sEmailSentStatus = string.Empty;
        string sPONumber = string.Empty;
        string sVendorName = string.Empty;
        public UCVendorBarcodeGeneration()
        {
            InitializeComponent();
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
                if (VariableInfo.mPlantCode == "VENDOR" || VariableInfo.mPlantCode == "Vendor")
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Vendor are not eligible for barcode generation", 2);
                    (VisualTreeHelper.GetParent(this) as StackPanel).Children.Clear();
                    return;
                }
                sPrintingSection = Properties.Settings.Default.PrintingSection.Trim().ToString();
                sLocationType = Properties.Settings.Default.PrintingLocationType.Trim().ToString();
                lblSelectAll.Visibility = System.Windows.Visibility.Hidden;
                chkSelectAll.Visibility = System.Windows.Visibility.Hidden;
                this.cmbPONum.SelectionChanged -= new SelectionChangedEventHandler(cmbPONum_SelectionChanged);
                _strPlantCode = VariableInfo.mPlantCode;
                GetSAPVendorPONumbers();
                this.cmbPONum.SelectionChanged += new SelectionChangedEventHandler(cmbPONum_SelectionChanged);
                DtSelectedItems = new DataTable();
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "VendorBarcodeGeneration => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetSAPVendorPONumbers()
        {
            try
            {
                objBL_Vendor = new BL_VendorPrinting();
                DtPONo = new DataTable();
                DtPONo = objBL_Vendor.BLGetSAPVendorPONumbers();
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
                    lv.ItemsSource = null;
                    lv.Items.Clear();
                    return;
                }
                else
                {
                    lv.ItemsSource = null;
                    txtVendorCode.Text = string.Empty;
                    txtbxVendorname.Text = string.Empty;
                    lblCount.Content = "*";
                    objBL_Vendor = new BL_VendorPrinting();
                    DtPOdata = new DataTable();
                    DtSelectedItems = new DataTable();
                    DtPOdata = objBL_Vendor.BLGetSelectedPOVendorDetails(cmbPONum.SelectedValue.ToString());
                    if (DtPOdata.Rows.Count > 0)
                    {
                        txtVendorCode.Text = DtPOdata.Rows[0][0].ToString();
                        txtbxVendorname.Text = DtPOdata.Rows[0][1].ToString();
                        objBL_Vendor = new BL_VendorPrinting();
                        objBLVendor = new PL_VendorMaster();
                        objBLVendor.PONumber = cmbPONum.SelectedValue.ToString();
                        VendorPOMatData = new BUSSINESS_LAYER.BL_VendorPrinting().BLGetSelectedPOMatData(objBLVendor);
                        if (VendorPOMatData.Count > 0)
                        {
                            lv.ItemsSource = VendorPOMatData;
                            lblCount.Content = lv.Items.Count;
                            DtItems = new DataTable();
                            ObservableCollection<PL_VendorMaster> data = (ObservableCollection<PL_VendorMaster>)lv.ItemsSource;
                            DtItems = VariableInfo.ToDataTable(data);
                        }
                    }
                    else
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "There is no items in this selected PO no.: " + cmbPONum.SelectedValue.ToString(), 2);
                        return;
                    }
                }

            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "VendorBarcodeGeneration => " + exDetail.ToString());
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
                PL_VendorMaster ObjPLHUB = (PL_VendorMaster)item.Content;
                objPOQty = ObjPLHUB.POQty;
                objPrintedQty = ObjPLHUB.PrintedQty;
                objRemQty = ObjPLHUB.RemaningQty;
                objSMCode = ObjPLHUB.MatCode;
                if (ObjPLHUB.IsValid == false)
                    ObjPLHUB.IsValid = true;
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
            //if(Convert.ToInt16(currentTextBox.Text)==0){
            //    BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter printable quantity  greater      than 0 for  for selected material", 2);
            //    currentTextBox.Text = string.Empty;
            //    return;
            //}

        }

        private void txtQty_LostFocus(object sender, RoutedEventArgs e)
        {
            try
            {
                //objRemQty = 0;
                objEnterQty = 0;
                int oTest = 0;
                //int objExpQty = 0;
                TextBox currentTextBox = (TextBox)sender;
                if (currentTextBox.Text == string.Empty)
                {
                    objEnterQty = 0;
                }
                else
                    objEnterQty = Convert.ToInt32(currentTextBox.Text);
                //objExpQty = ((objPOQty * 10) / 100);
                //objExpQty = ((objPOQty * 10) / 100);
                //objExpQty = objPOQty + objExpQty;
                //objExpQty = Convert.ToInt32(System.Math.Round(Convert.ToDecimal(objExpQty)));

                //objRemQty = (objExpQty - objPrintedQty);
                if (currentTextBox.Text != string.Empty)
                {
                    if (Convert.ToInt64(currentTextBox.Text) == 0)
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter printable quantity  greater      than 0 for selected material", 1);
                        btnSave.Cursor = Cursors.Arrow;
                        return;
                    }

                }

                if (objEnterQty == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter printable quantity  greater      than 0 for  for selected material", 1);
                    btnSave.Cursor = Cursors.Arrow;
                    return;
                }
                if (objEnterQty > objRemQty)
                {
                    // BCommon.setMessageBox(VariableInfo.mApp, "Printable quantity - " + objEnterQty + " should be be less      than remaining quantity - " + objRemQty + ", Kindly change", 2);
                    //BCommon.setMessageBox(VariableInfo.mApp, "Entered quantity - " + objEnterQty + " can not be greater      than remaining quantity - " + objRemQty + ", Kindly change", 2);

                    BCommon.setMessageBox(VariableInfo.mApp, "Entered quantity - " + objEnterQty + " can not be greater than remaining quantity - " + objRemQty + ", Kindly change", 2);

                    currentTextBox.Text = string.Empty;
                    return;
                }
                else if (objEnterQty <= objRemQty)
                {
                    objRemQty = (objPOQty - objEnterQty - objPrintedQty);
                    foreach (PL_VendorMaster item in lv.ItemsSource)
                    {
                        //if (item.IsValid == true)
                        //{
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
                        //item.IsValid = false;
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void OnSelected(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    //objPOQty = 0;
            //    //objPrintedQty = 0;
            //    //ListViewItem item = sender as ListViewItem;
            //    //PL_HubPrinting ObjPLHUB = (PL_HubPrinting)item.Content;
            //    //objPOQty = ObjPLHUB.POQty;
            //    //objPrintedQty = ObjPLHUB.PrintedQty;
            //    //if (ObjPLHUB.IsValid == false)
            //    //    ObjPLHUB.IsValid = true;

            //    foreach (PL_HubPrinting item1 in lv.ItemsSource)
            //    {
            //        if (item1.IsValid == true)
            //        {
            //            string objMatCode = Convert.ToString(item1.MatCode);
            //            if (DtSelectedItems.Rows.Count > 0)
            //            {
            //                DataRow[] result = DtSelectedItems.Select("MatCode like '%" + objMatCode + "%'");
            //                if (result.Length > 0)
            //                {
            //                    //PickingData = new BUSSINESS_LAYER.BL_PickingRequistion().BlGetLineDocDetails(objDocNo);
            //                    //dtbrcodeData = VariableInfo.ToDataTable(PickingData);
            //                }
            //            }
            //            else
            //            {
            //                foreach (ListViewItem item2 in lv.Items)
            //                {
            //                    DtSelectedItems.Columns.Add(item2.ToString());
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            //}
        }

        private void OnUnSelected(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
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

        private void chkSelect_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    bool bChecked = false;
            //    CheckBox chkBox = (CheckBox)sender;
            //    if (chkBox.IsChecked == true)
            //        bChecked = true;
            //    for (int i = 0; i < lv.Items.Count; i++)
            //    {
            //        if (bChecked == true)
            //        {
            //            int index = lv.SelectedIndex;
            //        }
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                objQRCode = string.Empty;
                objStackQRCode = string.Empty;
                string objMatcode = string.Empty;
                string objMatDesc = string.Empty;
                string objMatGrade = string.Empty;
                string objMatGroup = string.Empty;
                string objMatThickness = string.Empty;
                string objMatSize = string.Empty;
                int oPOQty = 0;
                int oPrintedQty = 0;
                int oPrintableQty = 0;
                int oTotalGenQty = 0;
                int oLotQty = 0;

                DtSelectedItems = new DataTable();
                int iCount = 0;
                OperationResult oResponse = OperationResult.SaveError;
                DtSelectedItems = DtItems.Clone();
                btnSave.Cursor = Cursors.Wait;
                for (int i = 0; i < DtItems.Rows.Count; i++)
                {
                    string objMatCode = DtItems.Rows[i][11].ToString();   //Convert.ToString(item1.MatCode);
                    if (Convert.ToInt32(DtItems.Rows[i][20]) != 0)
                    {
                        DataRow[] rowsToCopy;
                        rowsToCopy = DtItems.Select("MatCode ='" + objMatCode + "'");
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
                    btnSave.Cursor = Cursors.Arrow;
                    return;
                }
                if (DtSelectedItems.Rows.Count > 0)
                {
                    objBL_Vendor = new BL_VendorPrinting();
                    oDay = oMonth = oYear = oDateFormat = string.Empty;
                    oDay = DateTime.Now.ToString("dd");
                    oMonth = DateTime.Now.ToString("MM");
                    oYear = DateTime.Now.ToString("yy");
                    oDateFormat = oDay.Trim() + oMonth.Trim() + oYear.Trim();
                    sPONumber = this.cmbPONum.SelectedValue.ToString().Trim();
                    sVendorName = this.txtbxVendorname.Text.ToString().Trim();
                    for (int i = 0; i < DtSelectedItems.Rows.Count; i++)
                    {
                        objMatcode = DtSelectedItems.Rows[i][11].ToString();
                        objMatDesc = DtSelectedItems.Rows[i][12].ToString();
                        objMatGrade = DtSelectedItems.Rows[i][17].ToString();
                        objMatGroup = DtSelectedItems.Rows[i][15].ToString();
                        //if (objMatGroup != "" && objMatGroup.Length >= 4)
                        //    objMatGroup = objMatGroup.Substring(objMatGroup.Length - 4);
                        objMatThickness = DtSelectedItems.Rows[i][16].ToString();
                        objMatSize = DtSelectedItems.Rows[i][13].ToString();
                        oPOQty = Convert.ToInt32(DtSelectedItems.Rows[i][18].ToString());
                        oPrintedQty = Convert.ToInt32(DtSelectedItems.Rows[i][19].ToString());
                        oPrintableQty = Convert.ToInt32(DtSelectedItems.Rows[i][20].ToString());
                        if (oPrintableQty == 0)
                            oPrintableQty = Convert.ToInt32(DtSelectedItems.Rows[i][18].ToString());
                        for (int j = 0; j < oPrintableQty; j++)
                        {
                            string objRanNo = RandomString(2);
                            DataTable dtSerial = objBL_Vendor.BLGetQRCodeRunningSerialNo(oDateFormat, sPrintingSection, sLocationType);
                            if (dtSerial.Columns.Contains("SERIALNO") && dtSerial.Rows.Count > 0)
                            {
                                sQRRunningSerial = dtSerial.Rows[0][0].ToString();
                                if (sQRRunningSerial == string.Empty || sQRRunningSerial == "")
                                    sQRRunningSerial = "50000";
                                int objNextNo = Convert.ToInt32(sQRRunningSerial);
                                objNextNo = objNextNo + 1;
                                sQRRunningSerial = Convert.ToString(objNextNo);
                                objQRCode = _strPlantCode.Trim() + oDateFormat.Trim() + objRanNo.Trim() + sQRRunningSerial;
                                PL_VendorMaster _objVPrint = new PL_VendorMaster();
                                {
                                    _objVPrint.PONumber = this.cmbPONum.SelectedValue.ToString().Trim();
                                    _objVPrint.MatCode = objMatcode.Trim();
                                    _objVPrint.MatDesc = objMatDesc.Trim();
                                    _objVPrint.MatGrade = objMatGrade.Trim();
                                    _objVPrint.MatGroup = objMatGroup.Trim();
                                    _objVPrint.MatThickness = objMatThickness.Trim();
                                    _objVPrint.MatSize = objMatSize.Trim();
                                    _objVPrint.VendorId = this.txtVendorCode.Text.Trim();
                                    _objVPrint.QRCode = objQRCode.Trim();
                                    _objVPrint.PrintedQty = Convert.ToInt32((oPrintedQty + 1));
                                    _objVPrint.DateFormat = oDateFormat.Trim();
                                    _objVPrint.PrintingLocationType = sLocationType;
                                    _objVPrint.PrintingSection = sPrintingSection;
                                    _objVPrint.CreatedBy = Convert.ToString(VariableInfo.dtVlidateuser) != "" ? Convert.ToString(VariableInfo.dtVlidateuser.Rows[0]["USER_ID"]) : "";
                                    oResponse = new BL_VendorPrinting().SaveGeneratedQRCode(_objVPrint);
                                    if (oResponse == OperationResult.SaveSuccess)
                                    {
                                        oPrintedQty++;
                                        oTotalGenQty++;
                                    }
                                    else if (oResponse == OperationResult.Duplicate)
                                    {
                                        objQRCode = "";
                                        string objRanNo2 = RandomString(2);
                                        objQRCode = _strPlantCode.Trim() + oDateFormat.Trim() + objRanNo2.Trim() + sQRRunningSerial;
                                        _objVPrint.QRCode = objQRCode.Trim();
                                        oResponse = new BL_VendorPrinting().SaveGeneratedQRCode(_objVPrint);
                                    }
                                };
                            }
                        }
                    }
                    btnSave.Cursor = Cursors.Arrow;
                    BCommon.setMessageBox(VariableInfo.mApp, oTotalGenQty + " No. of QRCodes are generated successfully", 1);
                    lv.ItemsSource = null;
                    this.cmbPONum.SelectionChanged -= new SelectionChangedEventHandler(cmbPONum_SelectionChanged);
                    GetSAPVendorPONumbers();
                    txtVendorCode.Text = string.Empty;
                    txtbxVendorname.Text = string.Empty;
                    lblCount.Content = "*";
                    this.cmbPONum.SelectionChanged += new SelectionChangedEventHandler(cmbPONum_SelectionChanged);
                    //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                    //    SendMail(sVendorName, sPONumber, oTotalGenQty.ToString());
                }
            }
            catch (Exception ex)
            {
                btnSave.Cursor = Cursors.Arrow;
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "VendorBarcodeGeneration => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
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
        //        emailMessage.Subject = Properties.Settings.Default.SMTPSubject + " - " + DateTime.Now.ToString("ddMMyyyy");
        //        htmlString = @"<html>
        //              <body>
        //              <p>Hi,</p>
        //              <p>QR Code for PONumber - " + sPONo + " for Vendor - " + sVendorName + " with total quantity - " + sTotalQty + @" has been generated at hub & its ready for the printing at the vendor point.</p>
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
        //        ObjLog.WriteLog("Email sent Failed using SMTP! And Error is : " + ex.Message.ToString());
        //        if (ex.Message != null)
        //            ObjLog.WriteLog(ex.Message);
        //        return false;
        //    }

        //}


        //public void SendMailToVendor2(string Host)
        //{
        //    MailMessage msg = new MailMessage();
        //    msg.From = new MailAddress("noreply@greenply.com");
        //    msg.To.Add(VendorEmail);
        //    msg.Bcc.Add("abhinab.ply@greenply.com");
        //    msg.Subject = "GPIL PO NO : " + cmbPONum.SelectedValue.ToString();
        //    msg.Body = "Dear Sir,\r\nKindly find the details of the PO NO: " + cmbPONum.SelectedValue.ToString() + " in the application for label printing.\r\n\r\nRegards,\r\nGreenply Industries Limited";
        //    msg.Priority = MailPriority.High;
        //    msg.BodyEncoding = UTF8Encoding.UTF8;

        //    System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
        //    client.Port = Convert.ToInt32(PortNo);
        //    client.Host = Host;
        //    client.EnableSsl = true;
        //    client.Timeout = 10000;
        //    client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
        //    client.Credentials = new System.Net.NetworkCredential("noreply@greenply.com", Password); 
        //    client.Send(msg);
        //    msg.DeliveryNotificationOptions = System.Net.Mail.DeliveryNotificationOptions.OnSuccess;
        //}

        #region Button Event
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
            MessageBoxResult MessResult = MessageBox.Show("Do you want to clear all details?", "Clear confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (MessResult == MessageBoxResult.No)
            {
                return;
            }
            if (MessResult == MessageBoxResult.Yes)
            {
                Clear();
            }
        }

        private void Clear()
        {
            lv.ItemsSource = null;
            this.cmbPONum.SelectionChanged -= new SelectionChangedEventHandler(cmbPONum_SelectionChanged);
            cmbPONum.SelectedIndex = 0;
            this.cmbPONum.SelectionChanged += new SelectionChangedEventHandler(cmbPONum_SelectionChanged);
            txtVendorCode.Text = string.Empty;
            txtbxVendorname.Text = string.Empty;
            lblCount.Content = "*";
        }

        #endregion

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
