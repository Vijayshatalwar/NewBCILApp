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
    /// Interaction logic for UCDecorSegregationStackPrinting.xaml
    /// </summary>
    public partial class UCDecorSegregationStackPrinting : UserControl
    {
        WriteLogFile ObjLog = new WriteLogFile();
        BcilNetwork _bcilNetwork = new BcilNetwork();
        BL_SegStackPrinting objBLItem;
        PL_SegStackPrinting ObjPLprint;
        DataTable DtSelectedItems;
        DataTable DtItems;
        DataTable dtPostData;
        int objPOQty = 0;
        int objPrintedQty = 0;
        int sSaveCount = 0;
        int sDeleteCount = 0;
        string sDeleteStatus = string.Empty;
        string objQRCode = string.Empty;
        string objStackQRCode = string.Empty;
        string oMonth = string.Empty;
        string oDay = string.Empty;
        string oYear = string.Empty;
        string oDateFormat = string.Empty;
        string sStackRunningSerial;
        string objLocationCode = string.Empty;
        string sPrintingSection = string.Empty;
        string sLocationType = string.Empty;
        int PrintNonselect = 0;

        public UCDecorSegregationStackPrinting()
        {
            InitializeComponent();
        }

        #region Private Collection

        ObservableCollection<PL_SegStackPrinting> _PLSegStackPrinting = new ObservableCollection<PL_SegStackPrinting>();
        public ObservableCollection<PL_SegStackPrinting> PLSegStackPrinting
        {
            get { return _PLSegStackPrinting; }
            set
            {
                _PLSegStackPrinting = value;
                OnPropertyChanged("PLSegStackPrinting");
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
                objBLItem = new BL_SegStackPrinting();
                ObjPLprint = new PL_SegStackPrinting();
                btnDelete.Visibility = System.Windows.Visibility.Hidden;
                sPrintingSection = Properties.Settings.Default.PrintingSection.Trim().ToString();
                sLocationType = Properties.Settings.Default.PrintingLocationType.Trim().ToString();
                PLSegStackPrinting = new BUSSINESS_LAYER.BL_SegStackPrinting().BLGetDecorSegregationStackDetails();
                PLSegStackPrinting = new ObservableCollection<PL_SegStackPrinting>(PLSegStackPrinting.Distinct());
                if (PLSegStackPrinting.Count > 0)
                {
                    PrintNonselect = 1;
                    lv.ItemsSource = PLSegStackPrinting;
                    DtItems = new DataTable();
                    ObservableCollection<PL_SegStackPrinting> data = (ObservableCollection<PL_SegStackPrinting>)lv.ItemsSource;
                    DtItems = VariableInfo.ToDataTable(data);
                    lblCount.Content = Convert.ToString(DtItems.Rows.Count);
                    //  btnPrint.IsEnabled = true;
                }
                else
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "There is No Data Found for Segragation Stack QRCode Printing, Kindly Change", 2);
                    // btnPrint.IsEnabled = false;
                    return;
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "SegragationStackPrinting => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void OnSelected(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (PL_SegStackPrinting item1 in lv.ItemsSource)
                {
                    if (item1.IsValid == true)
                    {
                        string objMatCode = Convert.ToString(item1.MatCode);
                        string objUserId = Convert.ToString(item1.UserId);
                        foreach (DataRow dr in DtItems.Rows)
                        {
                            if (dr["MatCode"] == objMatCode)
                            {
                                dr["IsValid"] = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void OnUnSelected(object sender, RoutedEventArgs e)
        {
            try
            {
                foreach (PL_SegStackPrinting item1 in lv.ItemsSource)
                {
                    if (item1.IsValid == true)
                    {
                        string objMatCode = Convert.ToString(item1.MatCode);
                        string objUserId = Convert.ToString(item1.UserId);
                        foreach (DataRow dr in DtItems.Rows)
                        {
                            if (dr["MatCode"] == objMatCode)  //&& dr["UserId"] == objUserId
                            {
                                dr["IsValid"] = false;
                            }
                        }
                    }
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
                objBLItem = new BL_SegStackPrinting();
                DataSet dsData = new DataSet();
                objQRCode = objStackQRCode = string.Empty;
                string objMatcode = string.Empty;
                string objMatDesc = string.Empty;
                string objMatGrade = string.Empty;
                string objMatGroup = string.Empty;
                string objMatThickness = string.Empty;
                string objMatSize = string.Empty;
                int oSAPPostCount = 0;
                sSaveCount = 0;
                DtSelectedItems = new DataTable();
                dtPostData = new DataTable();
                dtPostData.Clear();
                int iCount = 0;
                if (PrintNonselect == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "There is No Data Found for Segragation Stack QRCode Printing, Kindly Change", 2);
                    // btnPrint.IsEnabled = false;
                    PrintNonselect = 0;
                    return;
                }

                DtSelectedItems = DtItems.Clone();
                btnPrint.Cursor = Cursors.Wait;

                foreach (PL_SegStackPrinting item1 in lv.ItemsSource)
                {
                    if (item1.IsValid == true)
                    {
                        string objMatCode = Convert.ToString(item1.MatCode);
                        string objUserId = Convert.ToString(item1.UserId);
                        string BatchNo = Convert.ToString(item1.BatchNo);
                        if (DtSelectedItems.Rows.Count != 0 && DtSelectedItems.Rows.Count <= 7)
                        {
                            if (DtItems.Rows.Count > 0)
                            {
                                DataRow[] rowsToCopy;
                                rowsToCopy = DtItems.Select("MatCode ='" + objMatCode + "' AND BatchNo = '" + BatchNo + "' AND IsValid = '" + true + "'");
                                //rowsToCopy = DtItems.Select("MatCode ='" + objMatCode + "' AND IsValid = '" + true + "'");  // "' AND UserId = '" + objUserId +
                                foreach (DataRow temp in rowsToCopy)
                                {
                                    DtSelectedItems.ImportRow(temp);
                                }
                            }
                        }
                        if (DtSelectedItems.Rows.Count == 0)
                        {
                            if (DtItems.Rows.Count > 0)
                            {
                                DataRow[] rowsToCopy;
                                rowsToCopy = DtItems.Select("MatCode ='" + objMatCode + "' AND BatchNo = '" + BatchNo + "' AND IsValid = '" + true + "'"); //"' AND UserId = '" + objUserId + 
                                foreach (DataRow temp in rowsToCopy)
                                {
                                    DtSelectedItems.ImportRow(temp);
                                }
                            }
                        }
                        if (VariableInfo.mPlantCode == "2000")
                        {
                            DataView view = new DataView(DtSelectedItems);
                            DataTable distinctValues = view.ToTable(true, "MatCode", "BatchNo");
                            if (distinctValues.Rows.Count > 1)
                            {
                                BCommon.setMessageBox(VariableInfo.mApp, "You have selected " + DtSelectedItems.Rows.Count + " no. of records of two diffrent material, Kindly select same material or 1 row at a time", 1);
                                btnPrint.Cursor = Cursors.Arrow;
                                return;
                            }
                        }
                        else
                        {
                            if (DtSelectedItems.Rows.Count > 7)
                            {
                                BCommon.setMessageBox(VariableInfo.mApp, "You have selected " + DtSelectedItems.Rows.Count + " no. of records, Kindly select upto 7 records only", 1);
                                btnPrint.Cursor = Cursors.Arrow;
                                return;
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
                for (int i = 0; i < DtSelectedItems.Rows.Count; i++)
                {
                    DataTable dtData = new DataTable();
                    string sLocCode = VariableInfo.mPlantCode.Trim();
                    string sMatCode = DtSelectedItems.Rows[i][1].ToString().Trim();
                    string sUserId = DtSelectedItems.Rows[i][11].ToString().Trim();
                    if (dtPostData.Columns.Count == 0 && dtPostData.Rows.Count == 0)
                    {
                        dtData = objBLItem.BLDecorSegregationSelectedMatDetails(sLocCode, sMatCode, sUserId);
                        dtPostData = dtData.Clone();
                        dtPostData = dtData.Copy();
                    }
                    else
                    {
                        dtData = objBLItem.BLDecorSegregationSelectedMatDetails(sLocCode, sMatCode, sUserId);
                        for (int j = 0; j < dtData.Rows.Count; j++)
                        {
                            DataRow dr = dtPostData.NewRow();
                            dr[0] = dtData.Rows[j][0].ToString();
                            dr[1] = dtData.Rows[j][1].ToString();
                            dr[2] = dtData.Rows[j][2].ToString();
                            dr[3] = dtData.Rows[j][3].ToString();
                            dr[4] = dtData.Rows[j][4].ToString();
                            dr[5] = dtData.Rows[j][5].ToString();
                            dr[6] = dtData.Rows[j][6].ToString();
                            dr[7] = dtData.Rows[j][7].ToString();
                            dr[8] = dtData.Rows[j][8].ToString();
                            dr[9] = dtData.Rows[j][9].ToString();
                            dr[10] = dtData.Rows[j][10].ToString();
                            dr[11] = dtData.Rows[j][11].ToString();
                            dr[12] = dtData.Rows[j][12].ToString();
                            dr[13] = dtData.Rows[j][13].ToString();
                            dtPostData.Rows.InsertAt(dr, j + 1);
                        }
                    }
                }
                if (dtPostData.Rows.Count > 0)
                {
                    oDay = oMonth = oYear = oDateFormat = string.Empty;
                    oDay = DateTime.Now.ToString("dd");
                    oMonth = DateTime.Now.ToString("MM");
                    oYear = DateTime.Now.ToString("yy");
                    oDateFormat = oDay.Trim() + oMonth.Trim() + oYear.Trim();
                    objLocationCode = VariableInfo.mPlantCode.Trim();
                    sStackRunningSerial = objBLItem.BLGetStackRunningSerialNo(oDateFormat, sPrintingSection, sLocationType);
                    if (sStackRunningSerial == string.Empty || sStackRunningSerial == "")
                    {
                        if (Properties.Settings.Default.PrintingSection.Contains("DECOR"))
                            sStackRunningSerial = "50000";
                        else
                            sStackRunningSerial = "0";
                    }
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
                    string PrintStatus = PrintSegregationStackQRCodeItem(objLocationCode, objStackQRCode, oDateFormat, sPrintingSection, sLocationType, dtPostData);
                    if (PrintStatus.Contains("SUCCESS") && sSaveCount >= 0)
                    {
                        oSAPPostCount = 0;
                        ObjLog.WriteLog("Stack QRCode - " + objStackQRCode + " is Printed Successfully And " + sSaveCount + " - No. of Records Posted to SAP");
                        BCommon.setMessageBox(VariableInfo.mApp, "Stack QRCode - " + objStackQRCode + " is printed successfully and " + sSaveCount + " - no. of records posted to SAP", 2);
                        lv.ItemsSource = null;
                        btnPrint.Cursor = Cursors.Arrow;
                        PLSegStackPrinting = new BUSSINESS_LAYER.BL_SegStackPrinting().BLGetSegregationStackDetails(sLocationType);
                        PLSegStackPrinting = new ObservableCollection<PL_SegStackPrinting>(PLSegStackPrinting.Distinct());
                        if (PLSegStackPrinting.Count > 0)
                        {
                            lv.ItemsSource = PLSegStackPrinting;
                            PrintNonselect = 1;
                            DtItems = new DataTable();
                            ObservableCollection<PL_SegStackPrinting> data = (ObservableCollection<PL_SegStackPrinting>)lv.ItemsSource;
                            DtItems = VariableInfo.ToDataTable(data);
                            lblCount.Content = Convert.ToString(DtItems.Rows.Count);
                        }
                        else
                        {
                            PrintNonselect = 0;
                            BCommon.setMessageBox(VariableInfo.mApp, "There is no more data found for segragation stack QRCode printing", 2);
                            Close_Click(sender, e);
                            return;
                        }
                    }
                    else if (PrintStatus.Contains("ERROR"))
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, PrintStatus.ToString(), 1);
                    }
                }
                btnPrint.Cursor = Cursors.Arrow;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "SegragationStackPrinting => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
                btnPrint.Cursor = Cursors.Arrow;
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                objBLItem = new BL_SegStackPrinting();
                ObjPLprint = new PL_SegStackPrinting();
                objQRCode = objStackQRCode = string.Empty;
                sDeleteStatus = string.Empty;
                DtSelectedItems = new DataTable();
                int iCount = 0;
                sDeleteCount = 0;
                DtSelectedItems = DtItems.Clone();
                btnPrint.Cursor = Cursors.Wait;
                foreach (PL_SegStackPrinting item1 in lv.ItemsSource)
                {
                    if (item1.IsValid == true)
                    {
                        string objMatCode = Convert.ToString(item1.MatCode);
                        //string objUserId = Convert.ToString(item1.UserId);
                        if (DtSelectedItems.Rows.Count != 0)
                        {
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
                    }
                    else
                    {
                        iCount++;
                        continue;
                    }
                }
                if (lv.Items.Count == iCount)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Select Atleast One Record to Delete", 1);
                    btnPrint.Cursor = Cursors.Arrow;
                    return;
                }
                for (int i = 0; i < DtSelectedItems.Rows.Count; i++)
                {
                    string sLocCode = VariableInfo.mPlantCode.Trim();
                    string sMatCode = DtSelectedItems.Rows[i][1].ToString().Trim();
                    string sUserId = DtSelectedItems.Rows[i][11].ToString().Trim();
                    sDeleteStatus = objBLItem.BLSegregationDeleteSelectedMatDetails(sLocCode, sMatCode, sUserId);
                    if (sDeleteStatus == "1")
                        sDeleteCount++;
                }
                if (sDeleteCount == DtSelectedItems.Rows.Count)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, sDeleteCount + " No. of Records Deleted Successfully", 2);
                    PLSegStackPrinting = new BUSSINESS_LAYER.BL_SegStackPrinting().BLGetSegregationStackDetails(sLocationType);
                    PLSegStackPrinting = new ObservableCollection<PL_SegStackPrinting>(PLSegStackPrinting.Distinct());
                    if (PLSegStackPrinting.Count > 0)
                    {
                        lv.ItemsSource = PLSegStackPrinting;
                        DtItems = new DataTable();
                        ObservableCollection<PL_SegStackPrinting> data = (ObservableCollection<PL_SegStackPrinting>)lv.ItemsSource;
                        DtItems = VariableInfo.ToDataTable(data);
                    }
                    else
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "There is No Data Found for Segragation Stack QRCode Printing, Kindly Check", 2);
                        Close_Click(sender, e);
                        return;
                    }
                }
                else if (sDeleteCount != DtSelectedItems.Rows.Count)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, sDeleteCount + " No. of Records Deleted Successfully", 2);
                    PLSegStackPrinting = new BUSSINESS_LAYER.BL_SegStackPrinting().BLGetSegregationStackDetails(sLocationType);
                    PLSegStackPrinting = new ObservableCollection<PL_SegStackPrinting>(PLSegStackPrinting.Distinct());
                    if (PLSegStackPrinting.Count > 0)
                    {
                        lv.ItemsSource = PLSegStackPrinting;
                        DtItems = new DataTable();
                        ObservableCollection<PL_SegStackPrinting> data = (ObservableCollection<PL_SegStackPrinting>)lv.ItemsSource;
                        DtItems = VariableInfo.ToDataTable(data);
                    }
                    else
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "There is No Data Found for Segragation Stack QRCode Printing, Kindly Check", 2);
                        Close_Click(sender, e);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "SegragationPrinting : Delete => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        public string PrintSegregationStackQRCodeItem(string objLocationCode, string objStackQRCode, string sDateFormat, string sPrintingSection, string sLocationType, DataTable dtData)
        {
            try
            {
                _bcilNetwork = new BcilNetwork();
                objBLItem = new BL_SegStackPrinting();
                string OutMsg = string.Empty;
                var sReadPrn = string.Empty;
                string sPrintStatus = string.Empty;
                string sMatCode = string.Empty;
                string sMatStatus = string.Empty;
                string sGradeDesc = string.Empty;
                string sGroup = string.Empty;
                string sGroupDesc = string.Empty;
                string sThicknessDesc = string.Empty;
                string sMatSize = string.Empty;
                string LotSize = string.Empty;
                string sDesignNo = string.Empty;
                string sFinishCode = string.Empty;
                string sBatchNo = string.Empty;
                //
                objQRCode = string.Empty;

                _bcilNetwork.PrinterIP = Properties.Settings.Default.ExistingStackQRCodePrinterIP;
                _bcilNetwork.PrinterPort = Properties.Settings.Default.ExistingStackQRCodePrinterPort;

                OutMsg = _bcilNetwork.NetworkPrinterStatus();
                if (OutMsg == "PRINTER READY")
                {
                    for (int i = 0; i < dtData.Rows.Count; i++)
                    {
                        sMatCode = dtData.Rows[i][1].ToString().Trim();
                        objQRCode = dtData.Rows[i][10].ToString().Trim();
                        sMatStatus = "P"; // dtData.Rows[i][3].ToString().Trim();
                        sGradeDesc = dtData.Rows[i][4].ToString().Trim();
                        sGroup = dtData.Rows[i][5].ToString().Trim();
                        sGroupDesc = dtData.Rows[i][6].ToString().Trim();
                        sThicknessDesc = dtData.Rows[i][8].ToString().Trim();
                        sMatSize = dtData.Rows[i][9].ToString().Trim();
                        sDesignNo = dtData.Rows[i][11].ToString().Trim();
                        sFinishCode = dtData.Rows[i][12].ToString().Trim();
                        sBatchNo = dtData.Rows[i][13].ToString().Trim();

                        string sSaveStatus = objBLItem.BLSegregationSaveStackQRCode(objLocationCode.Trim(), sMatCode.Trim(), objQRCode.Trim(), objStackQRCode.Trim(), sDateFormat.Trim(), sPrintingSection, sLocationType);
                        if (sSaveStatus.Contains("1"))
                        {
                            sSaveCount++;
                        }
                        else if (sSaveStatus.Contains("0"))
                        {
                            ObjLog.WriteLog(DateTime.Now.ToString("(Details) - " + "PrintSegregationStackQRCodeItem => " + "Barcode - " + objStackQRCode + " Not Update"));
                        }
                    }

                    StringBuilder sb = new StringBuilder();
                    DataTable dt = new DataTable();
                    string sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyVSegregationStackQRCode.PRN";
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

                    DataTable distinct = dtData.DefaultView.ToTable(true, "MatCode", "GradeDescription", "MatGroup", "ThicknessDescription", "MatSize", "DesignNo", "FinishCode", "BatchNo");
                    distinct.Columns.Add("TotalQty");
                    int iCount = 0;
                    for (int i = 0; i < distinct.Rows.Count; i++)
                    {
                        iCount = 0;
                        string Finalmatcode = distinct.Rows[i][0].ToString();
                        for (int j = 0; j < dtData.Rows.Count; j++)
                        {
                            string Stackmatcode = dtData.Rows[j][1].ToString();
                            if (Stackmatcode == Finalmatcode)
                            {
                                iCount++;
                                distinct.Rows[i][0] = dtData.Rows[j][1].ToString();
                                distinct.Rows[i][1] = dtData.Rows[j][4].ToString();
                                distinct.Rows[i][2] = dtData.Rows[j][5].ToString();
                                distinct.Rows[i][3] = dtData.Rows[j][8].ToString();
                                distinct.Rows[i][4] = dtData.Rows[j][9].ToString();
                                distinct.Rows[i][5] = dtData.Rows[j][11].ToString();
                                distinct.Rows[i][6] = dtData.Rows[j][12].ToString();
                                distinct.Rows[i][7] = dtData.Rows[j][13].ToString();
                                distinct.Rows[i][8] = Convert.ToString(iCount);
                            }
                        }
                    }
                    sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objStackQRCode.Trim()));
                    sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(objStackQRCode.Trim()));
                    for (int i = 0; i < distinct.Rows.Count; i++)
                    {
                        sMatCode = distinct.Rows[i][0].ToString().Trim();
                        sGradeDesc = distinct.Rows[i][1].ToString().Trim();
                        //sGroupDesc = distinct.Rows[i][2].ToString().Trim();
                        sGroup = distinct.Rows[i][2].ToString().Trim();
                        if (sGroup != "" && sGroup.Length >= 4)
                            sGroup = sGroup.Substring(sGroup.Length - 4);
                        sThicknessDesc = distinct.Rows[i][3].ToString().Trim();
                        sMatSize = distinct.Rows[i][4].ToString().Trim();
                        sDesignNo = distinct.Rows[i][5].ToString().Trim();
                        sFinishCode = distinct.Rows[i][6].ToString().Trim();
                        sBatchNo = distinct.Rows[i][7].ToString().Trim();
                        LotSize = distinct.Rows[i][8].ToString().Trim();
                        if (i == 0)
                        {
                            string ObjCode2 = sGradeDesc.Trim() + "-" + sGroup.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "-" + sDesignNo.Trim() + "-" + sFinishCode.Trim() + "-" + sBatchNo.Trim() + "-" + LotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(ObjCode2.Trim()));
                        }
                        if (i == 1)
                        {
                            string ObjCode3 = sGradeDesc.Trim() + "-" + sGroup.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "-" + sDesignNo.Trim() + "-" + sFinishCode.Trim() + "-" + sBatchNo.Trim() + "-" + LotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(ObjCode3.Trim()));
                        }
                        if (i == 2)
                        {
                            string ObjCode4 = sGradeDesc.Trim() + "-" + sGroup.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "-" + sDesignNo.Trim() + "-" + sFinishCode.Trim() + "-" + sBatchNo.Trim() + "-" + LotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(ObjCode4.Trim()));
                        }
                        if (i == 3)
                        {
                            string ObjCode5 = sGradeDesc.Trim() + "-" + sGroup.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "-" + sDesignNo.Trim() + "-" + sFinishCode.Trim() + "-" + sBatchNo.Trim() + "-" + LotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(ObjCode5.Trim()));
                        }
                        if (i == 4)
                        {
                            string ObjCode6 = sGradeDesc.Trim() + "-" + sGroup.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "-" + sDesignNo.Trim() + "-" + sFinishCode.Trim() + "-" + sBatchNo.Trim() + "-" + LotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode7}", Convert.ToString(ObjCode6.Trim()));
                        }
                        if (i == 5)
                        {
                            string ObjCode7 = sGradeDesc.Trim() + "-" + sGroup.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "-" + sDesignNo.Trim() + "-" + sFinishCode.Trim() + "-" + sBatchNo.Trim() + "-" + LotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode8}", Convert.ToString(ObjCode7.Trim()));
                        }
                        if (i == 6)
                        {
                            string ObjCode8 = sGradeDesc.Trim() + "-" + sGroup.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "-" + sDesignNo.Trim() + "-" + sFinishCode.Trim() + "-" + sBatchNo.Trim() + "-" + LotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode9}", Convert.ToString(ObjCode8.Trim()));
                        }
                    }
                    if (distinct.Rows.Count == 1)
                    {
                        sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode7}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode8}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode9}", Convert.ToString(""));
                    }
                    if (distinct.Rows.Count == 2)
                    {
                        sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode7}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode8}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode9}", Convert.ToString(""));
                    }
                    if (distinct.Rows.Count == 3)
                    {
                        sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode7}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode8}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode9}", Convert.ToString(""));
                    }
                    if (distinct.Rows.Count == 4)
                    {
                        sReadPrn = sReadPrn.Replace("{VarBarcode7}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode8}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode9}", Convert.ToString(""));
                    }
                    if (distinct.Rows.Count == 5)
                    {
                        sReadPrn = sReadPrn.Replace("{VarBarcode8}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode9}", Convert.ToString(""));
                    }
                    if (distinct.Rows.Count == 6)
                    {
                        sReadPrn = sReadPrn.Replace("{VarBarcode9}", Convert.ToString(""));
                    }
                    _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyVSegregationStackQRCode.PRN";
                    for (int i = 0; i < 2; i++)
                    {
                        OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                    }
                    _bcilNetwork.Dispose();
                    //OutMsg = "SUCCESS";
                    if (OutMsg == "SUCCESS")
                    {
                        ObjLog.WriteLog("Decor Stack QRCode - " + objStackQRCode + " Saved And Printed Successfully");
                        return OutMsg = "SUCCESS";
                    }
                }
                else
                {
                    if (OutMsg == "PRINTER NOT IN NETWORK")
                    {
                        ObjLog.WriteLog("ERROR ~ Decor Segregation Stack Printer IP " + _bcilNetwork.PrinterIP + " not in network, Kindly Check the network");
                        OutMsg = "ERROR ~ Decor Segregation Stack Printer IP " + _bcilNetwork.PrinterIP + " not in network, Kindly Check the network";
                        return OutMsg;
                    }
                    else
                    {
                        ObjLog.WriteLog("ERROR ~ Decor Segregation Stack Printer IP " + _bcilNetwork.PrinterIP + " found error. Error : " + OutMsg);
                        OutMsg = "ERROR ~ Decor Segregation Stack Printer IP " + _bcilNetwork.PrinterIP + " found error. Error : " + OutMsg;
                        return OutMsg;
                    }
                }
                return OutMsg;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(DateTime.Now.ToString(" (Error) - " + "DecorSegregationStackPrinting => " + exDetail.ToString()));
                return "ERROR | " + ex.Message;
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
    }
}
