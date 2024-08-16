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
using System.IO;

namespace GREENPLY.UserControls.Transaction
{
    /// <summary>
    /// Interaction logic for UCVendorLabelReprinting.xaml
    /// </summary>
    public partial class UCVendorLabelReprinting : UserControl
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

        public UCVendorLabelReprinting()
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
                    BCommon.setMessageBox(VariableInfo.mApp, "There is No Data for Selected Material Code, Kindly Change", 2);
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

        private void cmbMatCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                //if (cmbMatCode.SelectedIndex == 0)
                //{
                //    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Code", 2);
                //    cmbMatCode.Focus();
                //    lv.Items.Clear();
                //    return;
                //}
                //objBL_Vendor = new BL_VendorPrinting();
                //ObjPLVMaster = new PL_VendorMaster();
                //string matcode = cmbMatCode.SelectedValue.ToString().Trim();
                //DtMatData = objBL_Vendor.BLPrintGetSelectedMatDetails(cmbPONum.SelectedValue.ToString().Trim(), matcode);
                //if (DtMatData.Rows.Count > 0)
                //{
                //    txtMatSize.Text = DtMatData.Rows[0][2].ToString();
                //    txtMatGrade.Text = DtMatData.Rows[0][3].ToString();
                //    txtMatThickness.Text = DtMatData.Rows[0][1].ToString();
                //    txtGroupDesc.Text = DtMatData.Rows[0][0].ToString();
                //    txtGroup.Text = DtMatData.Rows[0][4].ToString();
                //    ObjPLVMaster.PONumber = cmbPONum.SelectedValue.ToString();
                //    ObjPLVMaster.MatCode = cmbMatCode.SelectedValue.ToString();
                //    VendorPOMatData = new BUSSINESS_LAYER.BL_VendorPrinting().BLRePrintGetSelectedMatQRCodeData(ObjPLVMaster);
                //    if (VendorPOMatData.Count > 0)
                //    {
                //        lv.ItemsSource = VendorPOMatData;
                //        lblCount.Content = lv.Items.Count;
                //        DtItems = new DataTable();
                //        ObservableCollection<PL_VendorMaster> data = (ObservableCollection<PL_VendorMaster>)lv.ItemsSource;
                //        DtItems = VariableInfo.ToDataTable(data);
                //    }
                //}
                //else if (DtMatData.Rows.Count > 0)
                //{
                //    BCommon.setMessageBox(VariableInfo.mApp, "There is No Data for Selected Material Code, Kindly Change", 2);
                //    cmbMatCode.Focus();
                //    lv.Items.Clear();
                //    return;
                //}

            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "VendorLabelReprinting => " + exDetail.ToString());
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

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            int iCount = 0;
            int oPrintCount = 0;
            objMatcode = string.Empty;
            objMatGrade = string.Empty;
            objMatGroup = string.Empty;
            objMatThickness = string.Empty;
            objMatSize = string.Empty;
            objQRCode = string.Empty;
            objStackQRCode = string.Empty;
            DtSelectedItems = new DataTable();
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
                BCommon.setMessageBox(VariableInfo.mApp, "Select Atleast One Record To Print", 1);
                btnPrint.Cursor = Cursors.Arrow;
                return;
            }
            if (DtSelectedItems.Rows.Count > 0)
            {
                objBL_Vendor = new BL_VendorPrinting();
                for (int i = 0; i < DtSelectedItems.Rows.Count; i++)
                {
                    //objMatcode = DtSelectedItems.Rows[i][10].ToString();
                    //objMatGroup = txtGroup.Text.Trim();
                    //if (objMatGroup != "" && objMatGroup.Length >= 4)
                    //    objMatGroup = objMatGroup.Substring(objMatGroup.Length - 4);
                    //objQRCode = DtSelectedItems.Rows[i][10].ToString();
                    //objStackQRCode = DtSelectedItems.Rows[i][10].ToString();
                    //string sPrintStatus = RePrintQRCodeItem(cmbPONum.SelectedValue.ToString(), objMatcode, objQRCode.Trim(), objMatGrade.Trim(), objMatGroup.Trim(), objMatThickness.Trim(), objMatSize.Trim());
                    //if (sPrintStatus.Contains("SUCCESS"))
                    //{
                    //    oPrintCount++;
                    //}
                }
                if (oPrintCount > 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, oPrintCount + " No of QRCodes Are Printed Out of Selected No of " + DtSelectedItems.Rows.Count +  " Successfully", 1);
                    btnPrint.Cursor = Cursors.Arrow;
                    lv.ItemsSource = null;
                    //txtGroup.Text = string.Empty;
                    //txtGroupDesc.Text = string.Empty;
                    //txtMatGrade.Text = string.Empty;
                    //txtMatSize.Text = string.Empty;
                    //txtMatThickness.Text = string.Empty;
                    //GetSAPVendorPONumbers();
                    return;
                }
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
                btnPrint.Cursor = Cursors.Arrow;
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
                    btnPrint.Cursor = Cursors.Arrow;
                    lv.ItemsSource = null;
                    GetSAPVendorPONumbers();
                    return;
                }
            }
        }

        public void SendMailToVendor2(string Host, string VendorEmail, string Name, string Password, string PortNo, string Subject)
        {
            MailMessage msg = new MailMessage();
            msg.From = new MailAddress("krish.kashyap@barcodindia.com");
            msg.To.Add("krishkashyap1991@gmail.com");
            msg.Subject = "GPIL PO NO : " + cmbPONum.SelectedValue.ToString();
            msg.Body = "Dear Sir,\r\nKindly find the details of the PO NO: " + cmbPONum.SelectedValue.ToString() + " in the application for label printing.\r\n\r\nRegards,\r\nGreenply Industries Limited";
            msg.Priority = MailPriority.High;
            msg.BodyEncoding = UTF8Encoding.UTF8;

            System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
            client.Port = Convert.ToInt32("587");      // 587;
            client.Host = "smtp.gmail.com";
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = true;  //false
            client.Credentials = new System.Net.NetworkCredential("krish.kashyap@barcodindia.com", "bcil@123"); //("mymailid", "mypassword", "smtp.gmail.com");
            //System.Net.Mail.MailMessage mm = new System.Net.Mail.MailMessage("Krish.kashyap@barcodindia.com", VendorEmail, "GPIL PO NO : " + cmbPONum.SelectedValue.ToString(), Body);   //("donotreply@domain.com", "sendtomyemail@domain.co.uk", "subject", "Body");
            //mm.BodyEncoding = UTF8Encoding.UTF8;
            //mm.DeliveryNotificationOptions = System.Net.Mail.DeliveryNotificationOptions.OnFailure;
            client.Send(msg);
            msg.DeliveryNotificationOptions = System.Net.Mail.DeliveryNotificationOptions.OnSuccess;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            cmbPONum.SelectedIndex = 0;
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

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lv.ItemsSource).Refresh();
        }

        private void FilteredData()
        {
            if (lv.Items.Count > 0)
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lv.ItemsSource);
                view.Filter = UserFilter;
            }
        }

        private bool UserFilter(object item)
        {
            if (String.IsNullOrEmpty(""))
            {
                return true;
            }
            else
            {
                return ((item as PL_VendorMaster).MatCode.IndexOf("", StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        public string RePrintQRCodeItem(string _strPONo, string sMatCode, string sQRCode, string sGrade, string sGroup, string sThickness, string sSize)
        {
            try
            {
                _bcilNetwork = new BcilNetwork();
                objBL_Vendor = new BL_VendorPrinting();
                string OutMsg = string.Empty;
                var sReadPrn = string.Empty;

                _bcilNetwork.PrinterIP = Properties.Settings.Default.ExistingQRCodePrinterIP;
                _bcilNetwork.PrinterPort = Properties.Settings.Default.ExistingQRCodePrinterPort;

                OutMsg = _bcilNetwork.NetworkPrinterStatus();
                if (OutMsg == "PRINTER READY")
                {
                    string sSaveStatus = objBL_Vendor.BLUpdateRePrintQRCode(_strPONo.Trim(), sMatCode.Trim(), sQRCode.Trim());
                    if (sSaveStatus == "1")
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
                        string objRest = sGrade + " - " + sGroup + " - " + sThickness + " - " + sSize;
                        string objFull = sQRCode + " - " + objRest;
                        sReadPrn = sReadPrn.Replace("{VarBarcode}", Convert.ToString(objFull));
                        sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(sQRCode));
                        sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(objRest));
                        _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyStackQRCode.PRN";

                        //OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                        OutMsg = "SUCCESS";
                        _bcilNetwork.Dispose();
                        if (OutMsg == "SUCCESS")
                        {
                            ObjLog.WriteLog("QRCode - " + sQRCode + " Saved And Printed Successfully at " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                            return OutMsg = "SUCCESS~Printed Successfully";
                        }
                    }
                    else if (sSaveStatus == "0")
                    {
                        ObjLog.WriteLog("Barcode - " + sQRCode + " Not Update At " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                    }
                }
                else
                {
                    if (OutMsg == "PRINTER NOT IN NETWORK")
                    {
                        ObjLog.WriteLog("ERROR ~ Printer not in network");
                        return OutMsg = "ERROR~Printer not in network";
                    }
                    else
                    {
                        ObjLog.WriteLog("ERROR~Printer error is : " + OutMsg);
                        return OutMsg;
                    }
                }
                return OutMsg;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "VendorLabelReprinting => " + exDetail.ToString());
                return "ERROR | " + ex.Message;
            }
        }
    }
}
