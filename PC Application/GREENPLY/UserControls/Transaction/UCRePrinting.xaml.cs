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
    /// Interaction logic for UCRePrinting.xaml
    /// </summary>
    public partial class UCRePrinting : UserControl
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

        public UCRePrinting()
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
                btnDelete.Visibility = System.Windows.Visibility.Hidden;
                //dtpTodate.SelectedDate = DateTime.Now.Date;
                dtpFromdate.SelectedDate = DateTime.Now.Date; //AddDays(-1);
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "RePrinting => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void btnGetStackNos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtpFromdate.Text == "")
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Please select from date", 1);
                    return;
                }
                //if (dtpTodate.Text == "")
                //{
                //    BCommon.setMessageBox(VariableInfo.mApp, "Please select to date", 1);
                //    return;
                //}
                DateTime FromDate = Convert.ToDateTime(dtpFromdate.Text);
                //DateTime ToDate = Convert.ToDateTime(dtpTodate.Text);
                //if (FromDate > ToDate)
                //{
                //    BCommon.setMessageBox(VariableInfo.mApp, "From date is greater than To date, Please select date between range", 1);
                //    return;
                //}
                //else
                //{
                //Fromdate = Convert.ToDateTime(dtpFromdate.Text).ToString("yyyy-MM-dd");
                //Todate = Convert.ToDateTime(dtpTodate.Text).ToString("yyyy-MM-dd");
                PLReprinting _PlReprinting = new ENTITY_LAYER.PLReprinting();
                _PlReprinting.LocationCode = VariableInfo.mPlantCode.Trim();
                _PlReprinting.Fromdate = Convert.ToDateTime(dtpFromdate.Text).ToString("yyyy-MM-dd");
                //_PlReprinting.Todate = Convert.ToDateTime(dtpTodate.Text).ToString("yyyy-MM-dd");
                GetPrintedStackQRCodes(_PlReprinting);
                //}
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "RePrinting => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetPrintedStackQRCodes(PLReprinting _ACDetails)
        {
            try
            {
                DtSQRCode = new DataTable();
                DtSQRCode = new BUSSINESS_LAYER.BL_ItemSelection().BLGetPrintedStackQRCodesData(_ACDetails);
                if (DtSQRCode.Rows.Count > 0)
                {
                    DataRow dr = DtSQRCode.NewRow();
                    dr[0] = "--Select--";
                    DtSQRCode.Rows.InsertAt(dr, 0);
                    DataView dataView = new DataView(DtSQRCode);
                    this.cmbStackQRCode.ItemsSource = dataView;
                    this.cmbStackQRCode.SelectionChanged -= new SelectionChangedEventHandler(cmbStackQRCode_SelectionChanged);
                    cmbStackQRCode.SelectedIndex = 0;
                    this.cmbStackQRCode.SelectionChanged += new SelectionChangedEventHandler(cmbStackQRCode_SelectionChanged);
                }
                else
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "There is No Stack QRCodes Found in Selected Date Range, Kindly Change", 2);
                    return;
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "RePrinting => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void cmbStackQRCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbStackQRCode.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Stack QRCode", 2);
                    cmbStackQRCode.Focus();
                    btnPrint.IsEnabled = false;
                    return;
                }
                else
                {
                    lv.ItemsSource = null;
                    objBLItem = new BL_ItemSelection();
                    ObjPLReprint = new PLReprinting();
                    ObjPLReprint.StackQRCode = cmbStackQRCode.SelectedValue.ToString();
                    PLReprinting = new BUSSINESS_LAYER.BL_ItemSelection().BLGetSelectedStackDetails(ObjPLReprint);
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
                        BCommon.setMessageBox(VariableInfo.mApp, "There is No Data Found for Stack QRCode - " + objSQRCode + " Kindly Change", 2);
                        cmbStackQRCode.Focus();
                        btnPrint.IsEnabled = false;
                        return;
                    }
                }
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
                if (PrintStatuses == 0)
                {
                    PrintStatuses = 0;
                    BCommon.setMessageBox(VariableInfo.mApp, "There is no data found for StackQRCode - " + objSQRCode + " Kindly change", 2);
                    cmbStackQRCode.Focus();
                    return;
                }

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

                #region StackQRCode
                if (lv.Items.Count == iCount)
                {
                    MessageBoxResult MessResult = MessageBox.Show("Are you sure, You want to print only StackQRCode?", "Save Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                    if (MessResult == MessageBoxResult.No)
                    {
                        return;
                    }
                    if (MessResult == MessageBoxResult.Yes)
                    {
                        objStackQRCode = cmbStackQRCode.SelectedValue.ToString();
                        if (DtSelectedItems.Rows.Count == 0)
                        {
                            PrintStatus = PrintStackQRCodeItem(objStackQRCode, DtItems);
                            if (PrintStatus.Contains("SUCCESS"))
                                iPrintCount++;
                        }
                        if (PrintStatus.Contains("SUCCESS"))
                        {
                            oSAPPostCount = 0;
                            BCommon.setMessageBox(VariableInfo.mApp, "Stack QRCode - " + objStackQRCode + " is reprinted successfully", 2);
                        }
                        else if (PrintStatus.Contains("ERROR ~"))
                        {
                            BCommon.setMessageBox(VariableInfo.mApp, "Error in Print - " + PrintStatus.ToString(), 1);
                        }
                    }
                    btnPrint.Cursor = Cursors.Arrow;
                    return;
                }
                #endregion

                #region QRCodes without stack qrcodes
                else
                {
                    MessageBoxResult MessResult = MessageBox.Show("Are you sure, You want to only print selected QRCodes?", "Save Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                    if (MessResult == MessageBoxResult.No)
                    {
                        return;
                    }
                    if (MessResult == MessageBoxResult.Yes)
                    {
                        objStackQRCode = cmbStackQRCode.SelectedValue.ToString();
                        if (DtSelectedItems.Rows.Count > 0)
                        {
                            for (int i = 0; i < DtSelectedItems.Rows.Count; i++)
                            {
                                string sGrade = DtSelectedItems.Rows[i][7].ToString();
                                string sGroup = DtSelectedItems.Rows[i][11].ToString();
                                string sGroupDesc = DtSelectedItems.Rows[i][12].ToString();
                                string sThicknessDesc = DtSelectedItems.Rows[i][5].ToString();
                                string sSize = DtSelectedItems.Rows[i][6].ToString();
                                string objoQRCode = DtSelectedItems.Rows[i][22].ToString();
                                PrintStatus = PrintQRCodeItem(sGrade, sGroup, sGroupDesc, sThicknessDesc, sSize, objoQRCode);
                                if (PrintStatus.Contains("SUCCESS"))
                                {
                                    iPrintCount++;
                                    BCommon.setMessageBox(VariableInfo.mApp, DtSelectedItems.Rows.Count + " no. of QRCode reprinted successfully", 2);
                                }
                                else if (PrintStatus.Contains("ERROR ~"))
                                {
                                    BCommon.setMessageBox(VariableInfo.mApp, "Error in Print - " + PrintStatus.ToString(), 1);
                                }
                            }
                        }
                    }
                    btnPrint.Cursor = Cursors.Arrow;
                    Clear();
                    return;
                }
                #endregion

            }
            catch (Exception ex)
            {
                Clear();
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "RePrinting => " + exDetail.ToString());
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

                _bcilNetwork.PrinterIP = Properties.Settings.Default.QRCodePrinterIP;
                _bcilNetwork.PrinterPort = Properties.Settings.Default.QRCodePrinterPort;

                OutMsg = _bcilNetwork.NetworkPrinterStatus();
                if (OutMsg == "PRINTER READY")
                {
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
                    string objFull = objQRCode.Trim() + "-" + sGrade.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sSize.Trim();
                    if (sGroup.Trim() != "" && sGroup.Length >= 4)
                        sGroup = sGroup.Substring(sGroup.Length - 4);
                    string objRest = sGrade.Trim() + "-" + sGroup.Trim() + "-" + sThicknessDesc.Trim() + "-" + sSize.Trim();
                    sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objFull.Trim()));
                    sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(objQRCode.Trim()));
                    sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(objRest.Trim()));
                    _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyQRCode.PRN";

                    OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                    //OutMsg = "SUCCESS";
                    _bcilNetwork.Dispose();
                    if (OutMsg == "SUCCESS")
                    {
                        ObjLog.WriteLog("Reprinting QRCode - " + objQRCode + " Saved And Printed Successfully");
                        return OutMsg = "SUCCESS";
                    }

                }
                else
                {
                    if (OutMsg == "PRINTER NOT IN NETWORK")
                    {
                        ObjLog.WriteLog("ERROR ~ Reprinting QRCode Printer IP " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network");
                        OutMsg = "ERROR ~ Reprinting QRCode Printer IP " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network";
                        return OutMsg;
                    }
                    else
                    {
                        ObjLog.WriteLog("ERROR ~ Reprinting QRCode Printer IP " + _bcilNetwork.PrinterIP + " found error. Error - " + OutMsg);
                        OutMsg = "ERROR ~ Reprinting QRCode Printer IP " + _bcilNetwork.PrinterIP + " found error. Error - " + OutMsg;
                        return OutMsg;
                    }
                }
                return OutMsg;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "Reprinting QRCode => " + exDetail.ToString());
                return "ERROR | " + ex.Message;
            }
        }

        public string PrintStackQRCodeItem(string objStackQRCode, DataTable dtData)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                string OutMsg = string.Empty;
                var sReadPrn = string.Empty;
                string sPrintStatus = string.Empty;
                string sMatCode = string.Empty;
                string sMatStatus = string.Empty;
                string sGradeDesc = string.Empty;
                string sGroupDesc = string.Empty;
                string sThicknessDesc = string.Empty;
                string sMatSize = string.Empty;
                string LotSize = string.Empty;

                _bcilNetwork.PrinterIP = Properties.Settings.Default.ExistingStackQRCodePrinterIP;
                _bcilNetwork.PrinterPort = Properties.Settings.Default.ExistingStackQRCodePrinterPort;

                OutMsg = _bcilNetwork.NetworkPrinterStatus();
                if (OutMsg == "PRINTER READY")
                {
                    StringBuilder sb = new StringBuilder();
                    string sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplySegregationStackQRCode.PRN";
                    if (File.Exists(sPrnExist))
                    {
                        StreamReader sr = new StreamReader(sPrnExist);
                        sReadPrn = sr.ReadToEnd();
                        sr.Close();
                        sr.Dispose();
                    }
                    else
                    {
                        ObjLog.WriteLog("GreenplySegregationStackQRCode.PRN File Not Found");
                        throw new Exception("GreenplySegregationStackQRCode.PRN File Not Found");
                    }

                    DataTable distinct = dtData.DefaultView.ToTable(true, "MatCode", "GradeDescription", "MatGroupDescription", "ThicknessDescription", "Size", "QRCodeCount");
                    sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objStackQRCode.Trim()));
                    sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(objStackQRCode.Trim()));
                    for (int i = 0; i < distinct.Rows.Count; i++)
                    {
                        sMatCode = distinct.Rows[i][0].ToString().Trim();
                        sGradeDesc = distinct.Rows[i][1].ToString().Trim();
                        sGroupDesc = distinct.Rows[i][2].ToString().Trim();
                        sThicknessDesc = distinct.Rows[i][3].ToString().Trim();
                        sMatSize = distinct.Rows[i][4].ToString().Trim();
                        LotSize = distinct.Rows[i][5].ToString().Trim();
                        if (i == 0)
                        {
                            string ObjCode2 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(ObjCode2.Trim()));
                        }
                        if (i == 1)
                        {
                            string ObjCode3 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(ObjCode3.Trim()));
                        }
                        if (i == 2)
                        {
                            string ObjCode4 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(ObjCode4.Trim()));
                        }
                        if (i == 3)
                        {
                            string ObjCode5 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(ObjCode5.Trim()));
                        }
                    }
                    if (distinct.Rows.Count == 1)
                    {
                        sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                    }
                    if (distinct.Rows.Count == 2)
                    {
                        sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                    }
                    if (distinct.Rows.Count == 3)
                    {
                        sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                    }
                    _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplySegregationStackQRCode.PRN";
                    for (int i = 0; i < 2; i++)
                    {
                        OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                    }
                    _bcilNetwork.Dispose();
                    //OutMsg = "SUCCESS";
                    if (OutMsg == "SUCCESS")
                    {
                        ObjLog.WriteLog("Reprinting Stack QRCode - " + objStackQRCode + " Saved And Reprinted Successfully");
                        return OutMsg = "SUCCESS";
                    }
                }
                else
                {
                    if (OutMsg == "PRINTER NOT IN NETWORK")
                    {
                        ObjLog.WriteLog("ERROR ~ Reprinting Stack QRCode Printer IP " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network");
                        OutMsg = "ERROR ~ Reprinting Stack QRCode Printer IP " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network";
                        return OutMsg;
                    }
                    else
                    {
                        ObjLog.WriteLog("ERROR ~ Reprinting StackQRCode Printer IP " + _bcilNetwork.PrinterIP + " found error. Error - " + OutMsg);
                        OutMsg = "ERROR ~ Reprinting Stack QRCode Printer IP " + _bcilNetwork.PrinterIP + " found error. Error - " + OutMsg;
                        return OutMsg;
                    }
                }
                return OutMsg;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "RePrinting Stack QRCode Error => " + exDetail.ToString());
                return "ERROR | " + ex.Message;
            }
        }

        private void Clear()
        {
            try
            {
                this.cmbStackQRCode.SelectionChanged -= new SelectionChangedEventHandler(cmbStackQRCode_SelectionChanged);
                cmbStackQRCode.SelectedIndex = 0;
                this.cmbStackQRCode.SelectionChanged += new SelectionChangedEventHandler(cmbStackQRCode_SelectionChanged);
                //cmbStackQRCode.ItemsSource = null;
                lv.ItemsSource = null;
                //lv.Items.Clear();
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

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (dtpFromdate.Text == "")
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Please select from date", 1);
                    return;
                }
                //if (dtpTodate.Text == "")
                //{
                //    BCommon.setMessageBox(VariableInfo.mApp, "Please select to date", 1);
                //    return;
                //}
                DateTime FromDate = Convert.ToDateTime(dtpFromdate.Text);
                //DateTime ToDate = Convert.ToDateTime(dtpTodate.Text);
                //if (FromDate > ToDate)
                //{
                //    BCommon.setMessageBox(VariableInfo.mApp, "From date is greater than To date, Please select date between range", 1);
                //    return;
                //}
                //else
                //{
                Fromdate = Convert.ToDateTime(dtpFromdate.Text).ToString("yyyy-MM-dd");
                //Todate = Convert.ToDateTime(dtpTodate.Text).ToString("yyyy-MM-dd");
                //GetPrintedStackQRCodes(Fromdate, Todate);
                //}
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "RePrinting => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void cmbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

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
                //if (dtpTodate.Text == "")
                //{
                //    BCommon.setMessageBox(VariableInfo.mApp, "Please select to date", 1);
                //    return;
                //}
                DateTime FromDate = Convert.ToDateTime(dtpFromdate.Text);
                //DateTime ToDate = Convert.ToDateTime(dtpTodate.Text);
                //if (FromDate > ToDate)
                //{
                //    BCommon.setMessageBox(VariableInfo.mApp, "From date is greater than To date, Please select date between range", 1);
                //    return;
                //}
                //else
                //{
                ////Fromdate = Convert.ToDateTime(dtpFromdate.Text).ToString("yyyy-MM-dd");
                ////Todate = Convert.ToDateTime(dtpTodate.Text).ToString("yyyy-MM-dd");
                PLReprinting _PlReprinting = new ENTITY_LAYER.PLReprinting();
                _PlReprinting.LocationCode = VariableInfo.mPlantCode.Trim();
                _PlReprinting.Fromdate = Convert.ToDateTime(dtpFromdate.Text).ToString("ddMMyy"); //("yyyy-MM-dd");   //"ddMMyy"
                //_PlReprinting.Todate = Convert.ToDateTime(dtpTodate.Text).ToString("yyyy-MM-dd"); //("yyyy-MM-dd");  //"ddMMyy"
                GetPrintedStackQRCodes(_PlReprinting);
                //}
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "RePrinting => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

    }
}
