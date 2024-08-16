using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.ObjectModel;
using System.Reflection;
using System.ComponentModel;
using COMMON;
using System.Data;
using System.Text.RegularExpressions;
using ENTITY_LAYER;
using BUSSINESS_LAYER;
using COMMON_LAYER;
using GREENPLY.Classes;
using BCILLogger;
using System.Net.Mail;
using System.Net;
using System.IO;
//using Microsoft.Exchange.WebServices.Data;

namespace GREENPLY.UserControls.Transaction
{
    /// <summary>
    /// Interaction logic for UCVendorReprintApproval.xaml
    /// </summary>
    public partial class UCVendorReprintApproval : UserControl
    {
        BcilNetwork _bcilNetwork = new BcilNetwork();
        Logger objLog = new Logger();
        WriteLogFile ObjLog = new WriteLogFile();
        string _strPlantCode = string.Empty;
        BL_VendorPrinting objBL_Vendor = new BL_VendorPrinting();
        DataTable DtPONo;
        DataTable DtPOdata;
        DataTable DtMatData;
        DataTable DtItems;
        DataTable DtSelectedItems;
        PL_VendorMaster ObjPLVMaster;
        string objQRCode = string.Empty;
        string objStackQRCode = string.Empty;
        string objMatcode = string.Empty;
        string objMatGrade = string.Empty;
        string objMatGroup = string.Empty;
        string objMatThickness = string.Empty;
        string objMatSize = string.Empty;
        int RepReqCount = 0;
        string sEmailSentStatus = string.Empty;


        public UCVendorReprintApproval()
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
                this.cmbPONum.SelectionChanged -= new SelectionChangedEventHandler(cmbPONum_SelectionChanged);
                _strPlantCode = VariableInfo.mPlantCode;
                GetSAPVendorPONumbers();
                this.cmbPONum.SelectionChanged += new SelectionChangedEventHandler(cmbPONum_SelectionChanged);
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "VendorLabelReprinting => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }
      
        private void GetSAPVendorPONumbers()
        {
            try
            {
                objBL_Vendor = new BL_VendorPrinting();
                DtPONo = new DataTable();
                DtPONo = objBL_Vendor.BLRePrintedGetSAPVendorPOs();
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
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Purchase Order No.", 2);
                    cmbPONum.Focus();
                    lv.Items.Clear();
                    return;
                }
                objBL_Vendor = new BL_VendorPrinting();
                ObjPLVMaster = new PL_VendorMaster();

                ObjPLVMaster.PONumber = cmbPONum.SelectedValue.ToString();
                VendorPOMatData = new BUSSINESS_LAYER.BL_VendorPrinting().BLRePrintGetSelectedMatQRCodeData(ObjPLVMaster);
                if (VendorPOMatData.Count > 0)
                {
                    lv.ItemsSource = VendorPOMatData;
                    lblCount.Content = lv.Items.Count;
                    DtItems = new DataTable();
                    ObservableCollection<PL_VendorMaster> data = (ObservableCollection<PL_VendorMaster>)lv.ItemsSource;
                    DtItems = VariableInfo.ToDataTable(data);
                }
                else if (DtMatData.Rows.Count > 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "There is No Data for Selected PO No., Kindly Try With Another", 2);
                    lv.Items.Clear();
                    return;
                }

            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "VendorLabelReprinting => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void btnEmail_Click(object sender, RoutedEventArgs e)
        {
            int iCount = 0;
            DtSelectedItems = new DataTable();
            objMatcode = string.Empty;
            objQRCode = string.Empty;
            objStackQRCode = string.Empty;
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
                BCommon.setMessageBox(VariableInfo.mApp, "Select Atleast One Record To EMail The Details", 1);
                return;
            }
            if (DtSelectedItems.Rows.Count > 0)
            {
                objBL_Vendor = new BL_VendorPrinting();
                for (int i = 0; i < DtSelectedItems.Rows.Count; i++)
                {
                    objMatcode = DtSelectedItems.Rows[i][10].ToString();
                    objQRCode = DtSelectedItems.Rows[i][10].ToString();
                    objStackQRCode = DtSelectedItems.Rows[i][10].ToString();
                    string sReqStatus = objBL_Vendor.BLUpdateReprintRequest(cmbPONum.SelectedValue.ToString(), objMatcode, objQRCode, objStackQRCode);
                    if (sReqStatus == "1")
                        RepReqCount++;
                }
                if (RepReqCount > 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, RepReqCount + " - No of Reprint Request Has Been Generated Out of Selected QRCode - " + DtSelectedItems.Rows.Count + " Successfully", 1);
                    lv.ItemsSource = null;
                    GetSAPVendorPONumbers();
                    return;
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

        private void lv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ListViewItem item = sender as ListViewItem;
                PL_VendorMaster objPLVMaster = (PL_VendorMaster)item.Content;
                if (objPLVMaster.IsValid == false)
                    objPLVMaster.IsValid = true;
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
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            cmbPONum.SelectedIndex = 0;
        }

        //public bool SendMail(string sResponce)
        //{
        //    try
        //    {
        //        string MailUser = Properties.Settings.Default.SMTPUser; // "qrcode@greenply.com";
        //        string MailPass = Properties.Settings.Default.SMTPPassword; // "Qrc0d@321";
        //        string MailTo = Properties.Settings.Default.ReceiverEmail; // "ashutosh@barcodeindia.com";
        //        ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2013_SP1);
        //        service.Credentials = new NetworkCredential(MailUser, MailPass);
        //        if (Properties.Settings.Default.UseProxy)
        //        {
        //            WebProxy proxy = new WebProxy(Properties.Settings.Default.ProxyIP.ToString(), Properties.Settings.Default.ProxyPort);
        //            service.WebProxy = proxy;
        //        }
        //        service.Url = new Uri(Properties.Settings.Default.SMTPServer);  //("https://outlook.office365.com/owa");
        //        EmailMessage emailMessage = new EmailMessage(service);
        //        emailMessage.Subject = Properties.Settings.Default.SMTPSubject + " - " + DateTime.Now.ToString("ddMMyyyy");
        //        string htmlString = @"<html>
        //              <body>
        //              <p>Hi,</p>
        //              <p>Error has been logged in the data transfer, details of the error is mentioned below:</p>
        //              <p>Error - " + sResponce + @"</p>
        //              <p>For any queries, feel free to connect with the Greenply Barcode Team.</p>
        //              <p>Thanks,<br>IT Team</br></p>
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
    }
}
