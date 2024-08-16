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
    /// Interaction logic for UCDoorPlyExistingStockPrinting.xaml
    /// </summary>
    public partial class UCDoorPlyExistingStockPrinting : UserControl
    {
        BcilNetwork _bcilNetwork = new BcilNetwork();
        //Logger objLog = new Logger();
        BCILLogger.Logger objLog1 = new Logger();
        //BCILLogger objLog = new BCILLogger();
        WriteLogFile ObjLog = new WriteLogFile();
        string _strPlantCode = string.Empty;
        BL_ItemSelection objBLItem;
        DataTable DtMatProduct;
        DataTable DtMatGroup;
        DataTable DtMatGrade;
        DataTable DtMatCat;
        DataTable DtMatThickness;
        DataTable DtMatSize;
        DataTable DtMatDesign;
        DataTable DtMatFinishCode;
        DataTable DtMatVisionCode;
        DataTable DtMatLippingCode;
        DataTable DtMatData;

        string objMatProduct = string.Empty;
        string objMatGroup = string.Empty;
        string objMatGrade = string.Empty;
        string objMatCat = string.Empty;
        string objMatThickness = string.Empty;
        string objMatSize = string.Empty;
        string objMatDesignNo = string.Empty;
        string objMatFinishCode = string.Empty;
        string objMatVisionCode = string.Empty;
        string objMatLippingCode = string.Empty;

        public static string sPrintingSection = string.Empty;
        public static string sLocationType = string.Empty;

        DataTable dtProdData;

        string objQRCode = string.Empty;
        string objStackQRCode = string.Empty;
        string oMonth = string.Empty;
        string oDay = string.Empty;
        string oYear = string.Empty;
        string oDateFormat = string.Empty;
        string oDateFormatEX = string.Empty;
        private static Random random = new Random();
        string objMatDesc = string.Empty;
        string sQRRunningSerial;
        string sStackRunningSerial;
        int sMatLotSize;
        int sMatLotPrintedQty;
        int sMatPrintCount = 0;
        int sStackPrintCount = 0;
        int oSAPPostCount = 0;
        int oPostCount = 0;

        public UCDoorPlyExistingStockPrinting()
        {
            InitializeComponent();
            //cmbLabelType.SelectedIndex = 0;
            dtProdData = new DataTable();
            dtProdData.Columns.Add("LocationCode");
            dtProdData.Columns.Add("MatCode");
            dtProdData.Columns.Add("QRCode");
            dtProdData.Columns.Add("Status");
            sPrintingSection = Properties.Settings.Default.PrintingSection.Trim().ToString();
            sLocationType = Properties.Settings.Default.PrintingLocationType.Trim().ToString();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Properties.Settings.Default.PrintMaterialType1 == "PLY" || Properties.Settings.Default.PrintMaterialType1 == "Ply")
                    lblDescription.Text = "EXISTING STOCK LABEL PRINTING FOR PLY";
                else if (Properties.Settings.Default.PrintMaterialType1 == "DOOR" || Properties.Settings.Default.PrintMaterialType1 == "Door")
                    lblDescription.Text = "EXISTING STOCK LABEL PRINTING FOR DOOR";
                this.cmbMatProduct.SelectionChanged -= new SelectionChangedEventHandler(cmbMatProduct_SelectionChanged);
                _strPlantCode = VariableInfo.mPlantCode;
                GetMatProduct();
                if (Properties.Settings.Default.PrintingLocationType == "HUB")
                {
                    lblPONo.Visibility = System.Windows.Visibility.Visible;
                    txtbxPONo.Visibility = System.Windows.Visibility.Visible;
                    lblVCode.Visibility = System.Windows.Visibility.Visible;
                    txtVendorCode.Visibility = System.Windows.Visibility.Visible;
                    cmbLabelType.SelectedIndex = 1;
                    cmbLabelType.IsEnabled = true;
                    txtVendorCode.Text = string.Empty;
                    txtbxPONo.Text = string.Empty;
                }
                if (Properties.Settings.Default.PrintingLocationType == "PLANT")
                {
                    lblPONo.Visibility = System.Windows.Visibility.Hidden;
                    txtbxPONo.Visibility = System.Windows.Visibility.Hidden;
                    lblVCode.Visibility = System.Windows.Visibility.Hidden;
                    txtVendorCode.Visibility = System.Windows.Visibility.Hidden;
                    cmbLabelType.SelectedIndex = 0;
                    cmbLabelType.IsEnabled = false;
                    txtVendorCode.Text = "0";
                    txtbxPONo.Text = "0";
                }
                this.cmbMatProduct.SelectionChanged += new SelectionChangedEventHandler(cmbMatProduct_SelectionChanged);
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "DoorPlyExistingStockPrinting => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetMatProduct()
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatProduct = new DataTable();
                DtMatProduct.Columns.Add("Product");
                string ProductType1 = Properties.Settings.Default.PrintMaterialType1.ToString().Trim();
                string ProductType2 = Properties.Settings.Default.PrintMaterialType2.ToString().Trim();
                //DtMatProduct = objBLItem.BLGetMatProduct(ProductType);
                //if (DtMatProduct.Rows.Count > 0)
                //{
                //DataRow dr = DtMatProduct.NewRow();
                DtMatProduct.Rows.Add(new Object[] { "--Select--" });
                //DtMatProduct.Rows[0]["Product"] = "--Select--";
                if (ProductType1 != string.Empty && ProductType2 != string.Empty)
                {
                    DtMatProduct.Rows.Add(new Object[] { ProductType1.Trim() });
                    DtMatProduct.Rows.Add(new Object[] { ProductType2.Trim() });
                    //DtMatProduct.Rows[1]["Product"] = ProductType1.Trim();
                    //DtMatProduct.Rows[2]["Product"] = ProductType2.Trim();
                }
                if (ProductType1 == string.Empty && ProductType2 != string.Empty)
                {
                    DtMatProduct.Rows.Add(new Object[] { ProductType2.Trim() });
                    //DtMatProduct.Rows[1]["Product"] = ProductType1.Trim();
                }
                if (ProductType1 != string.Empty && ProductType2 == string.Empty)
                {
                    DtMatProduct.Rows.Add(new Object[] { ProductType1.Trim() });
                    //DtMatProduct.Rows[1]["Product"] = ProductType1.Trim();
                }
                if (ProductType1 == string.Empty && ProductType2 == string.Empty)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "There is No Material Product Found, Kindly Check", 2);
                    return;
                }
                //DtMatProduct.Rows.InsertAt(dr, 0);
                DataView dataView = new DataView(DtMatProduct);
                this.cmbMatProduct.ItemsSource = dataView;
                cmbMatProduct.SelectedIndex = 0;
                //}
                //else
                //{
                //    BCommon.setMessageBox(VariableInfo.mApp, "There is No Material Product Found", 2);
                //    return;
                //}
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "DoorPlyExistingStockPrinting : GetMatProduct => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void cmbMatProduct_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbMatProduct.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Product", 2);
                    cmbMatProduct.Focus();
                    return;
                }
                else
                {
                    if (cmbLippingCode.ItemsSource != null)
                    {
                        this.cmbLippingCode.SelectionChanged -= new SelectionChangedEventHandler(cmbLippingCode_SelectionChanged);
                        cmbLippingCode.ItemsSource = null;
                        this.cmbLippingCode.SelectionChanged += new SelectionChangedEventHandler(cmbLippingCode_SelectionChanged);
                    }
                    if (cmbVisionCode.ItemsSource != null)
                    {
                        this.cmbVisionCode.SelectionChanged -= new SelectionChangedEventHandler(cmbVisionCode_SelectionChanged);
                        cmbVisionCode.ItemsSource = null;
                        this.cmbVisionCode.SelectionChanged += new SelectionChangedEventHandler(cmbVisionCode_SelectionChanged);
                    }
                    if (cmbFinishCode.ItemsSource != null)
                    {
                        this.cmbFinishCode.SelectionChanged -= new SelectionChangedEventHandler(cmbFinishCode_SelectionChanged);
                        cmbFinishCode.ItemsSource = null;
                        this.cmbFinishCode.SelectionChanged += new SelectionChangedEventHandler(cmbFinishCode_SelectionChanged);
                    }
                    if (cmbDesignNo.ItemsSource != null)
                    {
                        this.cmbDesignNo.SelectionChanged -= new SelectionChangedEventHandler(cmbDesignNo_SelectionChanged);
                        cmbDesignNo.ItemsSource = null;
                        this.cmbDesignNo.SelectionChanged += new SelectionChangedEventHandler(cmbDesignNo_SelectionChanged);
                    }
                    if (cmbMatSize.ItemsSource != null)
                    {
                        this.cmbMatSize.SelectionChanged -= new SelectionChangedEventHandler(cmbMatSize_SelectionChanged);
                        cmbMatSize.ItemsSource = null;
                        this.cmbMatSize.SelectionChanged += new SelectionChangedEventHandler(cmbMatSize_SelectionChanged);
                    }
                    if (cmbMatThickness.ItemsSource != null)
                    {
                        this.cmbMatThickness.SelectionChanged -= new SelectionChangedEventHandler(cmbMatThickness_SelectionChanged);
                        cmbMatThickness.ItemsSource = null;
                        this.cmbMatThickness.SelectionChanged += new SelectionChangedEventHandler(cmbMatThickness_SelectionChanged);
                    }
                    if (cmbMatGroup.ItemsSource != null)
                    {
                        this.cmbMatGroup.SelectionChanged -= new SelectionChangedEventHandler(cmbMatGroup_SelectionChanged);
                        cmbMatGroup.ItemsSource = null;
                        this.cmbMatGroup.SelectionChanged += new SelectionChangedEventHandler(cmbMatGroup_SelectionChanged);
                    }
                    if (cmbMatGrade.ItemsSource != null)
                    {
                        this.cmbMatGrade.SelectionChanged -= new SelectionChangedEventHandler(cmbMatGrade_SelectionChanged);
                        cmbMatGrade.ItemsSource = null;
                        this.cmbMatGrade.SelectionChanged += new SelectionChangedEventHandler(cmbMatGrade_SelectionChanged);
                    }
                    if (cmbMatCat.ItemsSource != null)
                    {
                        this.cmbMatCat.SelectionChanged -= new SelectionChangedEventHandler(cmbMatCat_SelectionChanged);
                        cmbMatCat.ItemsSource = null;
                        this.cmbMatCat.SelectionChanged += new SelectionChangedEventHandler(cmbMatCat_SelectionChanged);
                    }
                    txtCatDesc.Text = string.Empty;
                    txtGradeDesc.Text = string.Empty;
                    txtGroupDesc.Text = string.Empty;
                    txtThicknessDesc.Text = string.Empty;
                    txtDesignDesc.Text = string.Empty;
                    txtFinishDesc.Text = string.Empty;
                    txtVisionDesc.Text = string.Empty;
                    txtLippingDesc.Text = string.Empty;
                    txtMatCode.Text = string.Empty;
                    txtMatDesc.Text = string.Empty;
                    txtUOM.Text = string.Empty;
                    txtLotSize.Text = string.Empty;
                    txtbxPONo.Text = string.Empty;
                    txtVendorCode.Text = string.Empty;

                    objMatProduct = string.Empty;
                    objMatProduct = cmbMatProduct.SelectedValue.ToString().Trim();
                    GetMatCategory(objMatProduct);
                    //GetMatGroup(objMatProduct);
                    if (objMatProduct == "DOOR")
                    {
                        EnableControls(true);
                    }
                    if (objMatProduct == "TRADING DOOR")
                    {
                        EnableControls(true);
                    }
                    else if (objMatProduct == "PLY")
                    {
                        EnableControls(true);
                        DisableControls(false);
                    }
                    else if (objMatProduct == "TRADING PLY")
                    {
                        EnableControls(true);
                        //DisableControls(false);
                    }
                    else if (objMatProduct == "VENEER")
                    {
                        EnableControls(true);
                    }
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "DoorPlyExistingStockPrinting : MatProductSelectionChanged => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetMatCategory(string objProduct)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatCat = new DataTable();
                DtMatCat = objBLItem.BLGetMatCategory(objProduct);
                if (DtMatCat.Rows.Count > 0)
                {
                    //txtGradeDesc.Text = DtMatCat.Rows[0][0].ToString();
                    DataRow dr = DtMatCat.NewRow();
                    dr[0] = "--Select--";
                    dr[1] = "--Select--";
                    DtMatCat.Rows.InsertAt(dr, 0);
                    DataView dataView = new DataView(DtMatCat);
                    this.cmbMatCat.ItemsSource = dataView;
                    this.cmbMatCat.SelectionChanged -= new SelectionChangedEventHandler(cmbMatCat_SelectionChanged);
                    cmbMatCat.SelectedIndex = 0;
                    this.cmbMatCat.SelectionChanged += new SelectionChangedEventHandler(cmbMatCat_SelectionChanged);
                }
                else
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "There is No Material Category Found", 2);
                    return;
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "DoorPlyExistingStockPrinting : GetMatCategory => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void cmbMatCat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbMatProduct.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Product", 2);
                    cmbMatProduct.Focus();
                    return;
                }
                if (cmbMatCat.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Category", 2);
                    cmbMatCat.Focus();
                    return;
                }
                else
                {
                    if (cmbLippingCode.ItemsSource != null)
                    {
                        this.cmbLippingCode.SelectionChanged -= new SelectionChangedEventHandler(cmbLippingCode_SelectionChanged);
                        cmbLippingCode.ItemsSource = null;
                        this.cmbLippingCode.SelectionChanged += new SelectionChangedEventHandler(cmbLippingCode_SelectionChanged);
                    }
                    if (cmbVisionCode.ItemsSource != null)
                    {
                        this.cmbVisionCode.SelectionChanged -= new SelectionChangedEventHandler(cmbVisionCode_SelectionChanged);
                        cmbVisionCode.ItemsSource = null;
                        this.cmbVisionCode.SelectionChanged += new SelectionChangedEventHandler(cmbVisionCode_SelectionChanged);
                    }
                    if (cmbFinishCode.ItemsSource != null)
                    {
                        this.cmbFinishCode.SelectionChanged -= new SelectionChangedEventHandler(cmbFinishCode_SelectionChanged);
                        cmbFinishCode.ItemsSource = null;
                        this.cmbFinishCode.SelectionChanged += new SelectionChangedEventHandler(cmbFinishCode_SelectionChanged);
                    }
                    if (cmbDesignNo.ItemsSource != null)
                    {
                        this.cmbDesignNo.SelectionChanged -= new SelectionChangedEventHandler(cmbDesignNo_SelectionChanged);
                        cmbDesignNo.ItemsSource = null;
                        this.cmbDesignNo.SelectionChanged += new SelectionChangedEventHandler(cmbDesignNo_SelectionChanged);
                    }
                    if (cmbMatSize.ItemsSource != null)
                    {
                        this.cmbMatSize.SelectionChanged -= new SelectionChangedEventHandler(cmbMatSize_SelectionChanged);
                        cmbMatSize.ItemsSource = null;
                        this.cmbMatSize.SelectionChanged += new SelectionChangedEventHandler(cmbMatSize_SelectionChanged);
                    }
                    if (cmbMatThickness.ItemsSource != null)
                    {
                        this.cmbMatThickness.SelectionChanged -= new SelectionChangedEventHandler(cmbMatThickness_SelectionChanged);
                        cmbMatThickness.ItemsSource = null;
                        this.cmbMatThickness.SelectionChanged += new SelectionChangedEventHandler(cmbMatThickness_SelectionChanged);
                    }
                    if (cmbMatGroup.ItemsSource != null)
                    {
                        this.cmbMatGroup.SelectionChanged -= new SelectionChangedEventHandler(cmbMatGroup_SelectionChanged);
                        cmbMatGroup.ItemsSource = null;
                        this.cmbMatGroup.SelectionChanged += new SelectionChangedEventHandler(cmbMatGroup_SelectionChanged);
                    }
                    if (cmbMatGrade.ItemsSource != null)
                    {
                        this.cmbMatGrade.SelectionChanged -= new SelectionChangedEventHandler(cmbMatGrade_SelectionChanged);
                        cmbMatGrade.ItemsSource = null;
                        this.cmbMatGrade.SelectionChanged += new SelectionChangedEventHandler(cmbMatGrade_SelectionChanged);
                    }
                    txtGradeDesc.Text = string.Empty;
                    txtGroupDesc.Text = string.Empty;
                    txtThicknessDesc.Text = string.Empty;
                    txtDesignDesc.Text = string.Empty;
                    txtFinishDesc.Text = string.Empty;
                    txtVisionDesc.Text = string.Empty;
                    txtLippingDesc.Text = string.Empty;
                    txtMatCode.Text = string.Empty;
                    txtMatDesc.Text = string.Empty;
                    txtUOM.Text = string.Empty;
                    txtLotSize.Text = string.Empty;
                    txtbxPONo.Text = string.Empty;
                    txtVendorCode.Text = string.Empty;

                    objMatCat = string.Empty;
                    objMatCat = cmbMatCat.SelectedValue.ToString().Trim();
                    GetMatGrade(objMatProduct, objMatCat);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "DoorPlyExistingStockPrinting : MatCatSelectionChanged => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetMatGrade(string objProduct, string objCat)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatGrade = new DataTable();
                DtMatGrade = objBLItem.BLGetMatGrade(objProduct, objCat);
                if (DtMatGrade.Rows.Count > 0)
                {
                    txtCatDesc.Text = DtMatGrade.Rows[0][0].ToString();
                    DataRow dr = DtMatGrade.NewRow();
                    dr[1] = "--Select--";
                    dr[2] = "--Select--";
                    DtMatGrade.Rows.InsertAt(dr, 0);
                    DataView dataView = new DataView(DtMatGrade);
                    this.cmbMatGrade.ItemsSource = dataView;
                    this.cmbMatGrade.SelectionChanged -= new SelectionChangedEventHandler(cmbMatGrade_SelectionChanged);
                    cmbMatGrade.SelectedIndex = 0;
                    this.cmbMatGrade.SelectionChanged += new SelectionChangedEventHandler(cmbMatGrade_SelectionChanged);
                }
                else
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "There is No Material Grade Found", 2);
                    return;
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "DoorPlyExistingStockPrinting : GetMatGrade => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void cmbMatGrade_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbMatProduct.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Product", 2);
                    cmbMatProduct.Focus();
                    return;
                }
                if (cmbMatCat.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Category", 2);
                    cmbMatCat.Focus();
                    return;
                }
                if (cmbMatGrade.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Grade", 2);
                    cmbMatGrade.Focus();
                    return;
                }
                else
                {
                    if (cmbLippingCode.ItemsSource != null)
                    {
                        this.cmbLippingCode.SelectionChanged -= new SelectionChangedEventHandler(cmbLippingCode_SelectionChanged);
                        cmbLippingCode.ItemsSource = null;
                        this.cmbLippingCode.SelectionChanged += new SelectionChangedEventHandler(cmbLippingCode_SelectionChanged);
                    }
                    if (cmbVisionCode.ItemsSource != null)
                    {
                        this.cmbVisionCode.SelectionChanged -= new SelectionChangedEventHandler(cmbVisionCode_SelectionChanged);
                        cmbVisionCode.ItemsSource = null;
                        this.cmbVisionCode.SelectionChanged += new SelectionChangedEventHandler(cmbVisionCode_SelectionChanged);
                    }
                    if (cmbFinishCode.ItemsSource != null)
                    {
                        this.cmbFinishCode.SelectionChanged -= new SelectionChangedEventHandler(cmbFinishCode_SelectionChanged);
                        cmbFinishCode.ItemsSource = null;
                        this.cmbFinishCode.SelectionChanged += new SelectionChangedEventHandler(cmbFinishCode_SelectionChanged);
                    }
                    if (cmbDesignNo.ItemsSource != null)
                    {
                        this.cmbDesignNo.SelectionChanged -= new SelectionChangedEventHandler(cmbDesignNo_SelectionChanged);
                        cmbDesignNo.ItemsSource = null;
                        this.cmbDesignNo.SelectionChanged += new SelectionChangedEventHandler(cmbDesignNo_SelectionChanged);
                    }
                    if (cmbMatSize.ItemsSource != null)
                    {
                        this.cmbMatSize.SelectionChanged -= new SelectionChangedEventHandler(cmbMatSize_SelectionChanged);
                        cmbMatSize.ItemsSource = null;
                        this.cmbMatSize.SelectionChanged += new SelectionChangedEventHandler(cmbMatSize_SelectionChanged);
                    }
                    if (cmbMatThickness.ItemsSource != null)
                    {
                        this.cmbMatThickness.SelectionChanged -= new SelectionChangedEventHandler(cmbMatThickness_SelectionChanged);
                        cmbMatThickness.ItemsSource = null;
                        this.cmbMatThickness.SelectionChanged += new SelectionChangedEventHandler(cmbMatThickness_SelectionChanged);
                    }
                    if (cmbMatGroup.ItemsSource != null)
                    {
                        this.cmbMatGroup.SelectionChanged -= new SelectionChangedEventHandler(cmbMatGroup_SelectionChanged);
                        cmbMatGroup.ItemsSource = null;
                        this.cmbMatGroup.SelectionChanged += new SelectionChangedEventHandler(cmbMatGroup_SelectionChanged);
                    }
                    txtGroupDesc.Text = string.Empty;
                    txtThicknessDesc.Text = string.Empty;
                    txtDesignDesc.Text = string.Empty;
                    txtFinishDesc.Text = string.Empty;
                    txtVisionDesc.Text = string.Empty;
                    txtLippingDesc.Text = string.Empty;
                    txtMatCode.Text = string.Empty;
                    txtMatDesc.Text = string.Empty;
                    txtUOM.Text = string.Empty;
                    txtLotSize.Text = string.Empty;
                    txtbxPONo.Text = string.Empty;
                    txtVendorCode.Text = string.Empty;

                    objMatGrade = string.Empty;
                    objMatGrade = cmbMatGrade.SelectedValue.ToString().Trim();
                    GetMatGroup(objMatProduct, objMatCat, objMatGrade);
                    //GetMatCategory(objMatProduct, objMatGroup, objMatGrade);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "DoorPlyExistingStockPrinting : MatGradeSelectionChanged => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetMatGroup(string objProduct, string objCat, string objGrade)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatGroup = new DataTable();
                DtMatGroup = objBLItem.BLGetMatGroup(objProduct, objCat, objGrade);
                if (DtMatGroup.Rows.Count > 0)
                {
                    txtGradeDesc.Text = DtMatGroup.Rows[0][0].ToString();
                    DataRow dr = DtMatGroup.NewRow();
                    dr[1] = "--Select--";
                    dr[2] = "--Select--";
                    DtMatGroup.Rows.InsertAt(dr, 0);
                    DataView dataView = new DataView(DtMatGroup);
                    this.cmbMatGroup.ItemsSource = dataView;
                    this.cmbMatGroup.SelectionChanged -= new SelectionChangedEventHandler(cmbMatGroup_SelectionChanged);
                    cmbMatGroup.SelectedIndex = 0;
                    this.cmbMatGroup.SelectionChanged += new SelectionChangedEventHandler(cmbMatGroup_SelectionChanged);
                }
                else
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "There is No Material Group Found", 2);
                    return;
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "DoorPlyExistingStockPrinting : GetMatGroup => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void cmbMatGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbMatProduct.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Product", 2);
                    cmbMatProduct.Focus();
                    return;
                }
                if (cmbMatCat.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Category", 2);
                    cmbMatCat.Focus();
                    return;
                }
                if (cmbMatGrade.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Grade", 2);
                    cmbMatGrade.Focus();
                    return;
                }
                if (cmbMatGroup.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Group", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                else
                {
                    if (cmbLippingCode.ItemsSource != null)
                    {
                        this.cmbLippingCode.SelectionChanged -= new SelectionChangedEventHandler(cmbLippingCode_SelectionChanged);
                        cmbLippingCode.ItemsSource = null;
                        this.cmbLippingCode.SelectionChanged += new SelectionChangedEventHandler(cmbLippingCode_SelectionChanged);
                    }
                    if (cmbVisionCode.ItemsSource != null)
                    {
                        this.cmbVisionCode.SelectionChanged -= new SelectionChangedEventHandler(cmbVisionCode_SelectionChanged);
                        cmbVisionCode.ItemsSource = null;
                        this.cmbVisionCode.SelectionChanged += new SelectionChangedEventHandler(cmbVisionCode_SelectionChanged);
                    }
                    if (cmbFinishCode.ItemsSource != null)
                    {
                        this.cmbFinishCode.SelectionChanged -= new SelectionChangedEventHandler(cmbFinishCode_SelectionChanged);
                        cmbFinishCode.ItemsSource = null;
                        this.cmbFinishCode.SelectionChanged += new SelectionChangedEventHandler(cmbFinishCode_SelectionChanged);
                    }
                    if (cmbDesignNo.ItemsSource != null)
                    {
                        this.cmbDesignNo.SelectionChanged -= new SelectionChangedEventHandler(cmbDesignNo_SelectionChanged);
                        cmbDesignNo.ItemsSource = null;
                        this.cmbDesignNo.SelectionChanged += new SelectionChangedEventHandler(cmbDesignNo_SelectionChanged);
                    }
                    if (cmbMatSize.ItemsSource != null)
                    {
                        this.cmbMatSize.SelectionChanged -= new SelectionChangedEventHandler(cmbMatSize_SelectionChanged);
                        cmbMatSize.ItemsSource = null;
                        this.cmbMatSize.SelectionChanged += new SelectionChangedEventHandler(cmbMatSize_SelectionChanged);
                    }
                    if (cmbMatThickness.ItemsSource != null)
                    {
                        this.cmbMatThickness.SelectionChanged -= new SelectionChangedEventHandler(cmbMatThickness_SelectionChanged);
                        cmbMatThickness.ItemsSource = null;
                        this.cmbMatThickness.SelectionChanged += new SelectionChangedEventHandler(cmbMatThickness_SelectionChanged);
                    }
                    txtThicknessDesc.Text = string.Empty;
                    txtDesignDesc.Text = string.Empty;
                    txtFinishDesc.Text = string.Empty;
                    txtVisionDesc.Text = string.Empty;
                    txtLippingDesc.Text = string.Empty;
                    txtMatCode.Text = string.Empty;
                    txtMatDesc.Text = string.Empty;
                    txtUOM.Text = string.Empty;
                    txtLotSize.Text = string.Empty;
                    txtbxPONo.Text = string.Empty;
                    txtVendorCode.Text = string.Empty;

                    objMatGroup = string.Empty;
                    objMatGroup = cmbMatGroup.SelectedValue.ToString().Trim();
                    GetMatThickness(objMatProduct, objMatCat, objMatGrade, objMatGroup);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "DoorPlyExistingStockPrinting : MatGroupSelectionChanged => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetMatThickness(string objProduct, string objCat, string objGrade, string objGroup)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatThickness = new DataTable();
                DtMatThickness = objBLItem.BLGetMatThickness(objProduct, objCat, objGrade, objGroup);
                if (DtMatThickness.Rows.Count > 0)
                {
                    txtGroupDesc.Text = DtMatThickness.Rows[0][0].ToString();
                    DataRow dr = DtMatThickness.NewRow();
                    dr[1] = "--Select--";
                    dr[2] = "--Select--";
                    DtMatThickness.Rows.InsertAt(dr, 0);
                    DataView dataView = new DataView(DtMatThickness);
                    this.cmbMatThickness.ItemsSource = dataView;
                    this.cmbMatThickness.SelectionChanged -= new SelectionChangedEventHandler(cmbMatThickness_SelectionChanged);
                    cmbMatThickness.SelectedIndex = 0;
                    this.cmbMatThickness.SelectionChanged += new SelectionChangedEventHandler(cmbMatThickness_SelectionChanged);
                }
                else
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "There is No Material Thickness Found", 2);
                    return;
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "DoorPlyExistingStockPrinting : GetMatThickness => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void cmbMatThickness_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbMatProduct.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Product", 2);
                    cmbMatProduct.Focus();
                    return;
                }
                if (cmbMatCat.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Category", 2);
                    cmbMatCat.Focus();
                    return;
                }
                if (cmbMatGrade.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Grade", 2);
                    cmbMatGrade.Focus();
                    return;
                }
                if (cmbMatGroup.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Group", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                if (cmbMatThickness.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Thickness", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                else
                {
                    if (cmbLippingCode.ItemsSource != null)
                    {
                        this.cmbLippingCode.SelectionChanged -= new SelectionChangedEventHandler(cmbLippingCode_SelectionChanged);
                        cmbLippingCode.ItemsSource = null;
                        this.cmbLippingCode.SelectionChanged += new SelectionChangedEventHandler(cmbLippingCode_SelectionChanged);
                    }
                    if (cmbVisionCode.ItemsSource != null)
                    {
                        this.cmbVisionCode.SelectionChanged -= new SelectionChangedEventHandler(cmbVisionCode_SelectionChanged);
                        cmbVisionCode.ItemsSource = null;
                        this.cmbVisionCode.SelectionChanged += new SelectionChangedEventHandler(cmbVisionCode_SelectionChanged);
                    }
                    if (cmbFinishCode.ItemsSource != null)
                    {
                        this.cmbFinishCode.SelectionChanged -= new SelectionChangedEventHandler(cmbFinishCode_SelectionChanged);
                        cmbFinishCode.ItemsSource = null;
                        this.cmbFinishCode.SelectionChanged += new SelectionChangedEventHandler(cmbFinishCode_SelectionChanged);
                    }
                    if (cmbDesignNo.ItemsSource != null)
                    {
                        this.cmbDesignNo.SelectionChanged -= new SelectionChangedEventHandler(cmbDesignNo_SelectionChanged);
                        cmbDesignNo.ItemsSource = null;
                        this.cmbDesignNo.SelectionChanged += new SelectionChangedEventHandler(cmbDesignNo_SelectionChanged);
                    }
                    if (cmbMatSize.ItemsSource != null)
                    {
                        this.cmbMatSize.SelectionChanged -= new SelectionChangedEventHandler(cmbMatSize_SelectionChanged);
                        cmbMatSize.ItemsSource = null;
                        this.cmbMatSize.SelectionChanged += new SelectionChangedEventHandler(cmbMatSize_SelectionChanged);
                    }
                    txtDesignDesc.Text = string.Empty;
                    txtFinishDesc.Text = string.Empty;
                    txtVisionDesc.Text = string.Empty;
                    txtLippingDesc.Text = string.Empty;
                    txtMatCode.Text = string.Empty;
                    txtMatDesc.Text = string.Empty;
                    txtUOM.Text = string.Empty;
                    txtLotSize.Text = string.Empty;
                    txtbxPONo.Text = string.Empty;
                    txtVendorCode.Text = string.Empty;

                    objMatThickness = string.Empty;
                    objMatThickness = cmbMatThickness.SelectedValue.ToString().Trim();
                    GetMatSize(objMatProduct, objMatCat, objMatGrade, objMatGroup, objMatThickness);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "DoorPlyExistingStockPrinting : MatThicknessSelectionChanged => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetMatSize(string objProduct, string objCat, string objGrade, string objGroup, string objThickness)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatSize = new DataTable();
                DtMatSize = objBLItem.BLGetMatSize(objProduct, objCat, objGrade, objGroup, objThickness);
                if (DtMatSize.Rows.Count > 0)
                {
                    txtThicknessDesc.Text = DtMatSize.Rows[0][0].ToString();
                    DataRow dr = DtMatSize.NewRow();
                    dr[1] = "--Select--";
                    DtMatSize.Rows.InsertAt(dr, 0);
                    DataView dataView = new DataView(DtMatSize);
                    this.cmbMatSize.ItemsSource = dataView;
                    this.cmbMatSize.SelectionChanged -= new SelectionChangedEventHandler(cmbMatSize_SelectionChanged);
                    cmbMatSize.SelectedIndex = 0;
                    this.cmbMatSize.SelectionChanged += new SelectionChangedEventHandler(cmbMatSize_SelectionChanged);
                }
                else
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "There is No Material Size Found", 2);
                    return;
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "DoorPlyExistingStockPrinting : GetMatSize => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void cmbMatSize_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbMatProduct.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Product", 2);
                    cmbMatProduct.Focus();
                    return;
                }
                if (cmbMatCat.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Category", 2);
                    cmbMatCat.Focus();
                    return;
                }
                if (cmbMatGrade.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Grade", 2);
                    cmbMatGrade.Focus();
                    return;
                }
                if (cmbMatGroup.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Group", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                if (cmbMatThickness.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Thickness", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                if (cmbMatSize.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Size", 2);
                    cmbMatSize.Focus();
                    return;
                }
                else
                {
                    if (cmbLippingCode.ItemsSource != null)
                    {
                        this.cmbLippingCode.SelectionChanged -= new SelectionChangedEventHandler(cmbLippingCode_SelectionChanged);
                        cmbLippingCode.ItemsSource = null;
                        this.cmbLippingCode.SelectionChanged += new SelectionChangedEventHandler(cmbLippingCode_SelectionChanged);
                    }
                    if (cmbVisionCode.ItemsSource != null)
                    {
                        this.cmbVisionCode.SelectionChanged -= new SelectionChangedEventHandler(cmbVisionCode_SelectionChanged);
                        cmbVisionCode.ItemsSource = null;
                        this.cmbVisionCode.SelectionChanged += new SelectionChangedEventHandler(cmbVisionCode_SelectionChanged);
                    }
                    if (cmbFinishCode.ItemsSource != null)
                    {
                        this.cmbFinishCode.SelectionChanged -= new SelectionChangedEventHandler(cmbFinishCode_SelectionChanged);
                        cmbFinishCode.ItemsSource = null;
                        this.cmbFinishCode.SelectionChanged += new SelectionChangedEventHandler(cmbFinishCode_SelectionChanged);
                    }
                    if (cmbDesignNo.ItemsSource != null)
                    {
                        this.cmbDesignNo.SelectionChanged -= new SelectionChangedEventHandler(cmbDesignNo_SelectionChanged);
                        cmbDesignNo.ItemsSource = null;
                        this.cmbDesignNo.SelectionChanged += new SelectionChangedEventHandler(cmbDesignNo_SelectionChanged);
                    }
                    txtFinishDesc.Text = string.Empty;
                    txtVisionDesc.Text = string.Empty;
                    txtLippingDesc.Text = string.Empty;
                    txtMatCode.Text = string.Empty;
                    txtMatDesc.Text = string.Empty;
                    txtUOM.Text = string.Empty;
                    txtLotSize.Text = string.Empty;
                    txtbxPONo.Text = string.Empty;
                    txtVendorCode.Text = string.Empty;

                    objMatSize = string.Empty;
                    objMatSize = cmbMatSize.SelectedValue.ToString().Trim();
                    if (cmbDesignNo.IsEnabled == true)
                        GetMatDesignNo(objMatProduct, objMatCat, objMatGrade, objMatGroup, objMatThickness, objMatSize);
                    else if (cmbDesignNo.IsEnabled == false)
                        GetMatFinishCode(objMatProduct, objMatCat, objMatGrade, objMatGroup, objMatThickness, objMatSize, objMatDesignNo);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "DoorPlyExistingStockPrinting : MatSizeSelectionChanged => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetMatDesignNo(string objProduct, string objCat, string objGrade, string objGroup, string objThickness, string objSize)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatDesign = new DataTable();
                DtMatDesign = objBLItem.BLGetMatDesign(objProduct, objCat, objGrade, objGroup, objThickness, objSize);
                if (DtMatDesign.Rows.Count > 0)
                {
                    string objMd = DtMatDesign.Rows[0][0].ToString();
                    if (objMd == null || objMd == "")
                    {
                        cmbDesignNo.IsEnabled = false;
                        GetMatFinishCode(objProduct, objCat, objGrade, objGroup, objThickness, objSize, objMatDesignNo);
                    }
                    else
                    {
                        DataRow dr = DtMatDesign.NewRow();
                        dr[0] = "--Select--";
                        dr[1] = "--Select--";
                        DtMatDesign.Rows.InsertAt(dr, 0);
                        DataView dataView = new DataView(DtMatDesign);
                        this.cmbDesignNo.ItemsSource = dataView;
                        this.cmbDesignNo.SelectionChanged -= new SelectionChangedEventHandler(cmbDesignNo_SelectionChanged);
                        cmbDesignNo.SelectedIndex = 0;
                        this.cmbDesignNo.SelectionChanged += new SelectionChangedEventHandler(cmbDesignNo_SelectionChanged);
                    }
                }
                else if (DtMatDesign.Rows.Count == 0)
                {
                    cmbDesignNo.IsEnabled = false;
                    GetMatFinishCode(objProduct, objCat, objGrade, objGroup, objThickness, objSize, objMatDesignNo);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "DoorPlyExistingStockPrinting : GetMatDesignNo => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void cmbDesignNo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbMatProduct.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Product", 2);
                    cmbMatProduct.Focus();
                    return;
                }
                if (cmbMatCat.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Category", 2);
                    cmbMatCat.Focus();
                    return;
                }
                if (cmbMatGrade.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Grade", 2);
                    cmbMatGrade.Focus();
                    return;
                }
                if (cmbMatGroup.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Group", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                if (cmbMatThickness.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Thickness", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                if (cmbMatSize.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Size", 2);
                    cmbMatSize.Focus();
                    return;
                }
                if (cmbDesignNo.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Design Code", 2);
                    cmbDesignNo.Focus();
                    return;
                }
                else
                {
                    if (cmbLippingCode.ItemsSource != null)
                    {
                        this.cmbLippingCode.SelectionChanged -= new SelectionChangedEventHandler(cmbLippingCode_SelectionChanged);
                        cmbLippingCode.ItemsSource = null;
                        this.cmbLippingCode.SelectionChanged += new SelectionChangedEventHandler(cmbLippingCode_SelectionChanged);
                    }
                    if (cmbVisionCode.ItemsSource != null)
                    {
                        this.cmbVisionCode.SelectionChanged -= new SelectionChangedEventHandler(cmbVisionCode_SelectionChanged);
                        cmbVisionCode.ItemsSource = null;
                        this.cmbVisionCode.SelectionChanged += new SelectionChangedEventHandler(cmbVisionCode_SelectionChanged);
                    }
                    if (cmbFinishCode.ItemsSource != null)
                    {
                        this.cmbFinishCode.SelectionChanged -= new SelectionChangedEventHandler(cmbFinishCode_SelectionChanged);
                        cmbFinishCode.ItemsSource = null;
                        this.cmbFinishCode.SelectionChanged += new SelectionChangedEventHandler(cmbFinishCode_SelectionChanged);
                    }
                    txtVisionDesc.Text = string.Empty;
                    txtLippingDesc.Text = string.Empty;
                    txtMatCode.Text = string.Empty;
                    txtMatDesc.Text = string.Empty;
                    txtUOM.Text = string.Empty;
                    txtLotSize.Text = string.Empty;
                    txtbxPONo.Text = string.Empty;
                    txtVendorCode.Text = string.Empty;

                    objMatDesignNo = string.Empty;
                    objMatDesignNo = cmbDesignNo.SelectedValue.ToString().Trim();
                    if (cmbFinishCode.IsEnabled == true)
                        GetMatFinishCode(objMatProduct, objMatCat, objMatGrade, objMatGroup, objMatThickness, objMatSize, objMatDesignNo);
                    else if (cmbFinishCode.IsEnabled == false)
                        GetMatVisionCode(objMatProduct, objMatCat, objMatGrade, objMatGroup, objMatThickness, objMatSize, objMatDesignNo, objMatFinishCode);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "DoorPlyExistingStockPrinting : DesignNoSelectionChanged => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetMatFinishCode(string objProduct, string objCat, string objGrade, string objGroup, string objThickness, string objSize, string objDesignNo)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatFinishCode = new DataTable();
                DtMatFinishCode = objBLItem.BLGetMatFinishCode(objProduct, objCat, objGrade, objGroup, objThickness, objSize, objDesignNo);
                if (DtMatFinishCode.Rows.Count > 0)
                {
                    string objMFD = DtMatFinishCode.Rows[0][1].ToString();
                    if (objMFD == null || objMFD == "")
                    {
                        cmbFinishCode.IsEnabled = false;
                        GetMatVisionCode(objProduct, objCat, objGrade, objGroup, objThickness, objSize, objDesignNo, objMatFinishCode);
                    }
                    else
                    {
                        txtDesignDesc.Text = DtMatFinishCode.Rows[0][0].ToString();
                        DataRow dr = DtMatFinishCode.NewRow();
                        dr[1] = "--Select--";
                        dr[2] = "--Select--";
                        DtMatFinishCode.Rows.InsertAt(dr, 0);
                        DataView dataView = new DataView(DtMatFinishCode);
                        this.cmbFinishCode.ItemsSource = dataView;
                        this.cmbFinishCode.SelectionChanged -= new SelectionChangedEventHandler(cmbFinishCode_SelectionChanged);
                        cmbFinishCode.SelectedIndex = 0;
                        this.cmbFinishCode.SelectionChanged += new SelectionChangedEventHandler(cmbFinishCode_SelectionChanged);
                    }
                }
                else if (DtMatFinishCode.Rows.Count == 0)
                {
                    cmbFinishCode.IsEnabled = false;
                    GetMatVisionCode(objProduct, objCat, objGrade, objGroup, objThickness, objSize, objDesignNo, objMatFinishCode);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "DoorPlyExistingStockPrinting : GetMatFinishCode => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void cmbFinishCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbMatProduct.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Product", 2);
                    cmbMatProduct.Focus();
                    return;
                }
                if (cmbMatCat.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Category", 2);
                    cmbMatCat.Focus();
                    return;
                }
                if (cmbMatGrade.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Grade", 2);
                    cmbMatGrade.Focus();
                    return;
                }
                if (cmbMatGroup.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Group", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                if (cmbMatThickness.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Thickness", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                if (cmbMatSize.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Size", 2);
                    cmbMatSize.Focus();
                    return;
                }
                if (cmbDesignNo.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Design Code", 2);
                    cmbDesignNo.Focus();
                    return;
                }
                if (cmbFinishCode.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Finish Code", 2);
                    cmbFinishCode.Focus();
                    return;
                }
                else
                {
                    if (cmbLippingCode.ItemsSource != null)
                    {
                        this.cmbLippingCode.SelectionChanged -= new SelectionChangedEventHandler(cmbLippingCode_SelectionChanged);
                        cmbLippingCode.ItemsSource = null;
                        this.cmbLippingCode.SelectionChanged += new SelectionChangedEventHandler(cmbLippingCode_SelectionChanged);
                    }
                    if (cmbVisionCode.ItemsSource != null)
                    {
                        this.cmbVisionCode.SelectionChanged -= new SelectionChangedEventHandler(cmbVisionCode_SelectionChanged);
                        cmbVisionCode.ItemsSource = null;
                        this.cmbVisionCode.SelectionChanged += new SelectionChangedEventHandler(cmbVisionCode_SelectionChanged);
                    }
                    txtFinishDesc.Text = string.Empty;
                    txtVisionDesc.Text = string.Empty;
                    txtLippingDesc.Text = string.Empty;
                    txtMatCode.Text = string.Empty;
                    txtMatDesc.Text = string.Empty;
                    txtUOM.Text = string.Empty;
                    txtLotSize.Text = string.Empty;
                    txtbxPONo.Text = string.Empty;
                    txtVendorCode.Text = string.Empty;

                    objMatFinishCode = string.Empty;
                    objMatFinishCode = cmbFinishCode.SelectedValue.ToString().Trim();
                    if (cmbVisionCode.IsEnabled == true)
                        GetMatVisionCode(objMatProduct, objMatCat, objMatGrade, objMatGroup, objMatThickness, objMatSize, objMatDesignNo, objMatFinishCode);
                    else if (cmbVisionCode.IsEnabled == false)
                    {
                        if (cmbLippingCode.IsEnabled == true)
                            GetMatLippingCode(objMatProduct, objMatCat, objMatGrade, objMatGroup, objMatThickness, objMatSize, objMatDesignNo, objMatFinishCode, objMatVisionCode);
                        else if (cmbLippingCode.IsEnabled == false)
                        {
                            GetSelectedMatCode(objMatProduct, objMatCat, objMatGrade, objMatGroup, objMatThickness, objMatSize, objMatDesignNo, objMatFinishCode, objMatVisionCode, objMatLippingCode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "DoorPlyExistingStockPrinting : FinishCodeSelectionChanged => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetMatVisionCode(string objProduct, string objCat, string objGrade, string objGroup, string objThickness, string objSize, string objDesignNo, string FinishCode)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatVisionCode = new DataTable();
                DtMatVisionCode = objBLItem.BLGetMatVisionCode(objProduct, objCat, objGrade, objGroup, objThickness, objSize, objDesignNo, FinishCode);
                if (DtMatVisionCode.Rows.Count > 0)
                {
                    string objMV = DtMatFinishCode.Rows[0][1].ToString();
                    if (objMV == null || objMV == "")
                    {
                        cmbVisionCode.IsEnabled = false;
                        GetMatLippingCode(objProduct, objCat, objGrade, objGroup, objThickness, objSize, objDesignNo, FinishCode, objMatVisionCode);
                    }
                    else
                    {
                        txtFinishDesc.Text = DtMatVisionCode.Rows[0][0].ToString();
                        DataRow dr = DtMatVisionCode.NewRow();
                        dr[1] = "--Select--";
                        dr[2] = "--Select--";
                        DtMatVisionCode.Rows.InsertAt(dr, 0);
                        DataView dataView = new DataView(DtMatVisionCode);
                        this.cmbVisionCode.ItemsSource = dataView;
                        this.cmbVisionCode.SelectionChanged -= new SelectionChangedEventHandler(cmbVisionCode_SelectionChanged);
                        cmbVisionCode.SelectedIndex = 0;
                        this.cmbVisionCode.SelectionChanged += new SelectionChangedEventHandler(cmbVisionCode_SelectionChanged);
                        txtDesignDesc.Text = DtMatVisionCode.Rows[0][0].ToString();
                    }
                }
                else if (DtMatVisionCode.Rows.Count == 0)
                {
                    cmbVisionCode.IsEnabled = false;
                    GetMatLippingCode(objProduct, objCat, objGrade, objGroup, objThickness, objSize, objDesignNo, FinishCode, objMatVisionCode);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "DoorPlyExistingStockPrinting : GetMatVisionCode => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void cmbVisionCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbMatProduct.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Product", 2);
                    cmbMatProduct.Focus();
                    return;
                }
                if (cmbMatCat.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Category", 2);
                    cmbMatCat.Focus();
                    return;
                }
                if (cmbMatGrade.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Grade", 2);
                    cmbMatGrade.Focus();
                    return;
                }
                if (cmbMatGroup.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Group", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                if (cmbMatThickness.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Thickness", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                if (cmbMatSize.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Size", 2);
                    cmbMatSize.Focus();
                    return;
                }
                if (cmbDesignNo.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Design Code", 2);
                    cmbDesignNo.Focus();
                    return;
                }
                if (cmbFinishCode.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Finish Code", 2);
                    cmbFinishCode.Focus();
                    return;
                }
                if (cmbVisionCode.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Vision Code", 2);
                    cmbVisionCode.Focus();
                    return;
                }
                else
                {
                    if (cmbLippingCode.ItemsSource != null)
                    {
                        this.cmbLippingCode.SelectionChanged -= new SelectionChangedEventHandler(cmbLippingCode_SelectionChanged);
                        cmbLippingCode.ItemsSource = null;
                        this.cmbLippingCode.SelectionChanged += new SelectionChangedEventHandler(cmbLippingCode_SelectionChanged);
                    }
                    txtVisionDesc.Text = string.Empty;
                    txtLippingDesc.Text = string.Empty;
                    txtMatCode.Text = string.Empty;
                    txtMatDesc.Text = string.Empty;
                    txtUOM.Text = string.Empty;
                    txtLotSize.Text = string.Empty;
                    txtbxPONo.Text = string.Empty;
                    txtVendorCode.Text = string.Empty;

                    objMatVisionCode = string.Empty;
                    objMatVisionCode = cmbVisionCode.SelectedValue.ToString().Trim();
                    if (cmbLippingCode.IsEnabled == true)
                        GetMatLippingCode(objMatProduct, objMatCat, objMatGrade, objMatGroup, objMatThickness, objMatSize, objMatDesignNo, objMatFinishCode, objMatVisionCode);
                    else if (cmbLippingCode.IsEnabled == false)
                        GetSelectedMatCode(objMatProduct, objMatCat, objMatGrade, objMatGroup, objMatThickness, objMatSize, objMatDesignNo, objMatFinishCode, objMatVisionCode, objMatLippingCode);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "DoorPlyExistingStockPrinting : VisionCodeSelectionChanged => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetMatLippingCode(string objProduct, string objCat, string objGrade, string objGroup, string objThickness, string objSize, string objDesignNo, string FinishCode, string objVisionCode)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatLippingCode = new DataTable();
                DtMatLippingCode = objBLItem.BLGetMatLippingCode(objProduct, objCat, objGrade, objGroup, objThickness, objSize, objDesignNo, FinishCode, objVisionCode);
                if (DtMatLippingCode.Rows.Count > 0)
                {
                    string objLC = DtMatLippingCode.Rows[0][1].ToString();
                    if (objLC == null || objLC == "")
                    {
                        cmbLippingCode.IsEnabled = false;
                        GetSelectedMatCode(objProduct, objCat, objGrade, objGroup, objThickness, objSize, objDesignNo, FinishCode, objVisionCode, objLC);
                    }
                    else
                    {
                        txtVisionDesc.Text = DtMatLippingCode.Rows[0][0].ToString();
                        DataRow dr = DtMatLippingCode.NewRow();
                        dr[1] = "--Select--";
                        dr[2] = "--Select--";
                        DtMatLippingCode.Rows.InsertAt(dr, 0);
                        DataView dataView = new DataView(DtMatLippingCode);
                        this.cmbLippingCode.ItemsSource = dataView;
                        this.cmbLippingCode.SelectionChanged -= new SelectionChangedEventHandler(cmbLippingCode_SelectionChanged);
                        cmbLippingCode.SelectedIndex = 0;
                        this.cmbLippingCode.SelectionChanged += new SelectionChangedEventHandler(cmbLippingCode_SelectionChanged);
                        txtVisionDesc.Text = DtMatLippingCode.Rows[0][0].ToString();
                    }
                }
                else if (DtMatLippingCode.Rows.Count == 0)
                {
                    cmbLippingCode.IsEnabled = false;
                    GetSelectedMatCode(objProduct, objCat, objGrade, objGroup, objThickness, objSize, objDesignNo, FinishCode, objVisionCode, objMatLippingCode);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "DoorPlyExistingStockPrinting : GetMatLippingCode => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void cmbLippingCode_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbMatProduct.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Product", 2);
                    cmbMatProduct.Focus();
                    return;
                }
                if (cmbMatCat.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Category", 2);
                    cmbMatCat.Focus();
                    return;
                }
                if (cmbMatGrade.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Grade", 2);
                    cmbMatGrade.Focus();
                    return;
                }
                if (cmbMatGroup.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Group", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                if (cmbMatThickness.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Thickness", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                if (cmbMatSize.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Size", 2);
                    cmbMatSize.Focus();
                    return;
                }
                if (cmbDesignNo.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Design Code", 2);
                    cmbDesignNo.Focus();
                    return;
                }
                if (cmbFinishCode.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Finish Code", 2);
                    cmbFinishCode.Focus();
                    return;
                }
                if (cmbVisionCode.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Vision Code", 2);
                    cmbVisionCode.Focus();
                    return;
                }
                if (cmbLippingCode.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Lipping Code", 2);
                    cmbLippingCode.Focus();
                    return;
                }
                else
                {
                    txtLippingDesc.Text = string.Empty;
                    txtMatCode.Text = string.Empty;
                    txtMatDesc.Text = string.Empty;
                    txtUOM.Text = string.Empty;
                    txtLotSize.Text = string.Empty;
                    txtbxPONo.Text = string.Empty;
                    txtVendorCode.Text = string.Empty;

                    objMatLippingCode = string.Empty;
                    objMatLippingCode = cmbLippingCode.SelectedValue.ToString().Trim();
                    if (cmbLippingCode.IsEnabled == true)
                        GetSelectedMatCode(objMatProduct, objMatCat, objMatGrade, objMatGroup, objMatThickness, objMatSize, objMatDesignNo, objMatFinishCode, objMatVisionCode, objMatLippingCode);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "DoorPlyExistingStockPrinting : LippingCodeSelectionChanged => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetSelectedMatCode(string objProduct, string objCat, string objGrade, string objGroup, string objThickness, string objSize, string objDesignNo, string FinishCode, string objVisionCode, string objLippingCode)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatData = new DataTable();
                DtMatData = objBLItem.BLGetSelectedMatCode(objProduct, objCat, objGrade, objGroup, objThickness, objSize, objDesignNo, FinishCode, objVisionCode, objLippingCode);
                if (DtMatData.Rows.Count > 0)
                {
                    txtThicknessDesc.Text = DtMatData.Rows[0][0].ToString();
                    txtDesignDesc.Text = DtMatData.Rows[0][1].ToString();
                    txtFinishDesc.Text = DtMatData.Rows[0][2].ToString();
                    txtVisionDesc.Text = DtMatData.Rows[0][3].ToString();
                    txtLippingDesc.Text = DtMatData.Rows[0][4].ToString();
                    txtMatCode.Text = DtMatData.Rows[0][5].ToString();
                    txtMatDesc.Text = DtMatData.Rows[0][6].ToString();
                    txtUOM.Text = DtMatData.Rows[0][7].ToString();
                    txtLotSize.Text = DtMatData.Rows[0][8].ToString();
                    if (txtLotSize.Text != string.Empty || txtLotSize.Text == "0")
                        txtLotSize.IsEnabled = true;
                }
                else
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Invalid Selection, There Is No Material Code And Details Exists For The Selected Material Properties, Kindly Change", 2);
                    cmbLippingCode.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "DoorPlyExistingStockPrinting : GetSelectedMatCode => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbMatProduct.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Product", 2);
                    cmbMatProduct.Focus();
                    return;
                }
                if (cmbMatCat.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Category", 2);
                    cmbMatCat.Focus();
                    return;
                }
                if (cmbMatGrade.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Grade", 2);
                    cmbMatGrade.Focus();
                    return;
                }
                if (cmbMatGroup.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Group", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                if (cmbMatThickness.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Thickness", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                if (cmbMatSize.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Size", 2);
                    cmbMatSize.Focus();
                    return;
                }
                if (txtLotSize.Text == string.Empty)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Enter Material Quantity to Print", 2);
                    txtLotSize.Focus();
                    return;
                }
                if (txtLotSize.Text == "0" || txtLotSize.Text == "00" || txtLotSize.Text == "000")
                {
                    BCommon.setMessageBox(VariableInfo.mApp, txtLotSize.Text + " Not Valid Quantity, Kindly Change", 2);
                    txtLotSize.Focus();
                    return;
                }
                if (Properties.Settings.Default.PrintingLocationType == "HUB")
                {
                    if (txtbxPONo.Text == string.Empty)
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter PO No. to print existing stock", 2);
                        txtbxPONo.Focus();
                        return;
                    }
                    string sPO = txtbxPONo.Text;
                    int sLen = sPO.Length;
                    if (sLen > 10)
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter only 10 character PO no. to print existing stock", 2);
                        txtbxPONo.Focus();
                        return;
                    }
                    if (sLen < 10)
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter 10 character PO no. to print existing stock", 2);
                        txtbxPONo.Focus();
                        return;
                    }

                    if (txtVendorCode.Text == string.Empty)
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter vendor code to print existing stock", 2);
                        txtVendorCode.Focus();
                        return;
                    }
                    string sVCode = txtVendorCode.Text;
                    sLen = sVCode.Length;
                    if (sLen > 10)
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter only 10 character vendor code to print existing stock", 2);
                        txtVendorCode.Focus();
                        return;
                    }
                    if (sLen < 10)
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter 10 character vendor code to print existing stock", 2);
                        txtVendorCode.Focus();
                        return;
                    }
                }
                if (Properties.Settings.Default.PrintingLocationType == "PLANT")
                {
                    txtVendorCode.Text = "0";
                    txtbxPONo.Text = "0";
                }
                objBLItem = new BL_ItemSelection();
                objQRCode = string.Empty;
                objStackQRCode = string.Empty;
                sMatPrintCount = 0;
                sMatLotPrintedQty = 0;
                string objMatcode = string.Empty;
                string objMatDesc = string.Empty;
                string objMatGrade = string.Empty;
                string objMatGroup = string.Empty;
                string objMatThickness = string.Empty;
                string objMatSize = string.Empty;
                string objMatStatus = "E";
                DataTable objMatData = new DataTable();
                dtProdData.Rows.Clear();
                string objLocationCode = VariableInfo.mPlantCode.Trim();
                objMatcode = txtMatCode.Text.Trim();
                objMatDesc = txtMatDesc.Text.Trim();
                objMatGrade = cmbMatGrade.SelectedValue.ToString().Trim();
                objMatGroup = cmbMatGroup.SelectedValue.ToString().Trim();
                //if (objMatGroup != "" && objMatGroup.Length >= 4)
                //    objMatGroup = objMatGroup.Substring(objMatGroup.Length - 4);
                objMatThickness = cmbMatThickness.SelectedValue.ToString().Trim();
                objMatSize = cmbMatSize.SelectedValue.ToString().Trim();

                oDay = oMonth = oYear = oDateFormat = oDateFormatEX = string.Empty;
                oDay = DateTime.Now.ToString("dd");
                oMonth = DateTime.Now.ToString("MM");
                oYear = "EX";  // DateTime.Now.ToString("yy");
                oDateFormatEX = oDay.Trim() + oMonth.Trim() + oYear.Trim();
                oDateFormat = oDay.Trim() + oMonth.Trim() + DateTime.Now.ToString("yy").Trim();

                string sMatchGroup = string.Empty;
                sMatLotSize = Convert.ToInt32(txtLotSize.Text);
                string oLabelType = cmbLabelType.Text.ToString();

                sPrintingSection = Properties.Settings.Default.PrintingSection.Trim().ToString();
                DataTable dtMGroupData = new DataTable();
                dtMGroupData = objBLItem.BLGetUnbrandedMatGroups(VariableInfo.mPlantCode.Trim().ToString());
                if (dtMGroupData.Rows.Count > 0)
                {
                    for (int i = 0; i < dtMGroupData.Rows.Count; i++)
                    {
                        string sMGroup = dtMGroupData.Rows[i][0].ToString().Trim();
                        if (sMGroup == objMatGroup)
                        {
                            sMatchGroup = sMGroup;
                        }
                    }
                }
                if (sMatchGroup != string.Empty)
                {
                    if (oLabelType == "2X2 Label")
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "You can not print 2X2 labels for Selected material group " + objMatGroup + ", Kindly change the Material group", 3);
                        return;
                    }
                }
                if (sMatchGroup == string.Empty)
                {
                    if (oLabelType == "2X1 Label")
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "You can not print 2X1 labels for Selected material group " + objMatGroup + ", Kindly change the Material group", 3);
                        return;
                    }
                }

                #region Door/TradingDoor
                if (cmbMatProduct.SelectedValue.ToString().Trim() == "DOOR" || cmbMatProduct.SelectedValue.ToString().Trim() == "TRADING DOOR")
                {
                    for (int i = 0; i < sMatLotSize; i++)
                    {
                        sPrintingSection = Properties.Settings.Default.PrintingSection.Trim().ToString();
                        string objRanNo = RandomString(2);
                        sQRRunningSerial = objBLItem.BLVGetQRCodeRunningSerialNo(oDateFormat, sPrintingSection, sLocationType);
                        if (sQRRunningSerial == string.Empty || sQRRunningSerial == "")
                        {
                            if (Properties.Settings.Default.PrintingSection.Contains("DOOR"))
                                sQRRunningSerial = "40000";
                            if (Properties.Settings.Default.PrintingSection.Contains("DECOR"))
                                sQRRunningSerial = "50000";
                            else
                                sQRRunningSerial = "0";
                        }
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


                        objQRCode = objLocationCode.Trim() + oDateFormatEX.Trim() + objRanNo.Trim() + sQRRunningSerial;
                        string PrintStatus = PrintHubQRCodeItem(objLocationCode, objMatcode, objMatDesc, objMatGrade, objMatGroup, txtGroupDesc.Text.Trim(), objMatThickness, txtThicknessDesc.Text.Trim(), objMatSize, objQRCode, objMatStatus, oDateFormat, sPrintingSection, sLocationType, oLabelType, txtbxPONo.Text.ToString(), txtVendorCode.Text.Trim().ToString());
                        if (PrintStatus.Contains("SUCCESS"))
                        {
                            sMatPrintCount++;
                        }
                        else if (PrintStatus.Contains("ERROR ~"))
                        {
                            BCommon.setMessageBox(VariableInfo.mApp, "Existing QRCode Printing " + PrintStatus.ToString(), 2);
                        }
                    }
                    if (sMatPrintCount > 0)
                    {
                        ObjLog.WriteLog(sMatPrintCount + " nos. of QRCodes printed Successfully");
                        BCommon.setMessageBox(VariableInfo.mApp, sMatPrintCount + " nos. of QRCodes printed successfully", 2);
                        Clear();
                    }
                }
                #endregion

                #region Ply/TradingPly
                else if (cmbMatProduct.SelectedValue.ToString().Trim() == "PLY" || cmbMatProduct.SelectedValue.ToString().Trim() == "TRADING PLY")
                {
                    for (int i = 0; i <= sMatLotSize; i++)
                    {
                        string objRanNo = RandomString(2);
                        sMatLotPrintedQty = sMatPrintCount;

                        if (sMatLotSize > sMatLotPrintedQty)
                        {
                            sPrintingSection = Properties.Settings.Default.PrintingSection.Trim().ToString();
                            sQRRunningSerial = objBLItem.BLVGetQRCodeRunningSerialNo(oDateFormat, sPrintingSection, sLocationType);
                            if (sQRRunningSerial == string.Empty || sQRRunningSerial == "")
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

                            oLabelType = cmbLabelType.Text.ToString();
                            objQRCode = objLocationCode.Trim() + oDateFormatEX.Trim() + objRanNo.Trim() + sQRRunningSerial;
                            string PrintStatus = PrintHubQRCodeItem(objLocationCode, objMatcode, objMatDesc, objMatGrade, objMatGroup, txtGroupDesc.Text.Trim(), objMatThickness, txtThicknessDesc.Text.Trim(), objMatSize, objQRCode, objMatStatus, oDateFormat, sPrintingSection, sLocationType, oLabelType, txtbxPONo.Text.Trim().ToString(), txtVendorCode.Text.Trim().ToString());
                            if (PrintStatus.Contains("SUCCESS"))
                            {
                                sMatPrintCount++;
                            }
                            else if (PrintStatus.Contains("ERROR ~"))
                            {
                                BCommon.setMessageBox(VariableInfo.mApp, "Existing QRCode Printing => " + PrintStatus.ToString(), 2);
                            }
                        }
                        if (oLabelType == "2X2 Label")
                        {
                            if (sMatLotSize == sMatLotPrintedQty)
                            {
                                sPrintingSection = Properties.Settings.Default.PrintingSection.Trim().ToString();
                                sStackRunningSerial = objBLItem.BLVGetStackRunningSerialNo(oDateFormat, sPrintingSection, sLocationType);
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

                                string PrintStatus = PrintStackQRCodeItem(objLocationCode, objMatcode, objStackQRCode, oDateFormat, sPrintingSection, sLocationType, txtGradeDesc.Text.Trim(), txtGroupDesc.Text.Trim(), txtThicknessDesc.Text.Trim(), cmbMatSize.SelectedValue.ToString(), sMatLotSize.ToString(), txtbxPONo.Text.Trim());
                                if (PrintStatus.Contains("SUCCESS"))
                                {
                                    oSAPPostCount = 0;
                                    sMatLotPrintedQty = 0;
                                    ObjLog.WriteLog("Stack QRCode - " + objStackQRCode + " is Printed  And " + sMatLotSize + " - No. of Records Posted to SAP for Material Code - " + objMatcode + " Successfully");
                                    BCommon.setMessageBox(VariableInfo.mApp, "Stack QRCode - " + objStackQRCode + " is Printed  And " + sMatLotSize + " - No. of Records Posted to SAP for Material Code - " + objMatcode + " Successfully ", 2);
                                }
                                else if (PrintStatus.Contains("ERROR ~"))
                                {
                                    BCommon.setMessageBox(VariableInfo.mApp, "Existing Stack QRCode Printing => " + PrintStatus, 2);
                                }
                            }
                        }
                        if (oLabelType == "2X1 Label")
                        {
                            if (sMatLotSize == sMatLotPrintedQty)
                            {
                                sPrintingSection = Properties.Settings.Default.PrintingSection.Trim().ToString();
                                sPrintingSection = sPrintingSection + "_PR";
                                sStackRunningSerial = objBLItem.BLVGetStackRunningSerialNo(oDateFormat, sPrintingSection, sLocationType);
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

                                string PrintStatus = PrintPRDStackQRCodeItem(objLocationCode, objMatcode, objStackQRCode, oDateFormat, sPrintingSection, sLocationType, txtGradeDesc.Text.Trim(), txtGroupDesc.Text.Trim(), txtThicknessDesc.Text.Trim(), cmbMatSize.SelectedValue.ToString(), sMatLotSize.ToString(), txtbxPONo.Text.Trim());
                                if (PrintStatus.Contains("SUCCESS"))
                                {
                                    oSAPPostCount = 0;
                                    sMatLotPrintedQty = 0;
                                    ObjLog.WriteLog("Stack QRCode - " + objStackQRCode + " is Printed  And " + sMatLotSize + " - No. of Records Posted to SAP for Material Code - " + objMatcode + " Successfully");
                                    BCommon.setMessageBox(VariableInfo.mApp, "Stack QRCode - " + objStackQRCode + " is Printed  And " + sMatLotSize + " - No. of Records Posted to SAP for Material Code - " + objMatcode + " Successfully ", 2);
                                }
                                else if (PrintStatus.Contains("ERROR ~"))
                                {
                                    BCommon.setMessageBox(VariableInfo.mApp, "Existing Stack QRCode Printing " + PrintStatus, 2);
                                }
                            }
                        }
                    }
                }
                #endregion

                else
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Selected material print type is not valid, Kindly change and try again", 3);
                    return;
                }
                Clear();//
            }
            catch (Exception ex)
            {
                dtProdData.Rows.Clear();
                Clear();
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "ExistingStockPrinting : PrintClick => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }//
        }

        public string PrintHubQRCodeItem(string objLocationCode, string objMatCode, string sMatDesc, string sGrade, string sGroup, string sGroupDesc, string sThickness, string sThicknessDesc, string sSize, string objQRCode, string sMatStatus, string sDateFormat, string sPrintSection, string sLocType, string oLabelType, string oPONo, string oVendorCode)
        {
            try
            {
                _bcilNetwork = new BcilNetwork();
                objBLItem = new BL_ItemSelection();
                string OutMsg = string.Empty;
                var sReadPrn = string.Empty;

                _bcilNetwork.PrinterIP = Properties.Settings.Default.ExistingQRCodePrinterIP;
                _bcilNetwork.PrinterPort = Properties.Settings.Default.ExistingQRCodePrinterPort;

                if (Properties.Settings.Default.PrintingLocationType == "HUB")
                {
                    OutMsg = _bcilNetwork.NetworkPrinterStatus();
                }
                if (Properties.Settings.Default.PrintingLocationType == "PLANT")
                {
                    OutMsg = "PRINTER READY";
                }
                if (OutMsg == "PRINTER READY")
                {
                    string sSaveStatus = objBLItem.BLSaveHubQRCode(objLocationCode.Trim(), objMatCode.Trim(), sMatDesc.Trim(), sGrade.Trim(), sGroup.Trim(), sThickness.Trim(), sSize.Trim(), objQRCode.Trim(), sMatStatus.Trim(), sDateFormat.Trim(), sPrintSection.Trim(), sLocType.Trim(), oPONo.Trim(), oVendorCode, oLabelType);
                    if (sSaveStatus == "SUCCESS")
                    {
                        StringBuilder sb = new StringBuilder();
                        DataTable dt = new DataTable();
                        string sPrnExist = string.Empty;
                        if (oLabelType == "2X2 Label")
                        {
                            sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyQRCode.PRN";
                        }
                        else if (oLabelType == "2X1 Label")
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
                        if (oLabelType == "2X2 Label")
                        {
                            string objFull = objQRCode.Trim() + "-" + sGrade.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sSize.Trim();
                            if (sGroup.Trim() != "" && sGroup.Length >= 4)
                                sGroup = sGroup.Substring(objMatGroup.Length - 4);
                            string objRest = sGrade.Trim() + "-" + sGroup.Trim() + "-" + sThicknessDesc.Trim() + "-" + sSize.Trim();
                            sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objFull.Trim()));
                            sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(objQRCode.Trim()));
                            sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(objRest.Trim()));
                            _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyQRCode.PRN";
                        }
                        else if (oLabelType == "2X1 Label")
                        {
                            string objRest = sGrade + "-" + sGroupDesc + "-" + sThicknessDesc + "-" + sSize;
                            string objFull = objQRCode + "-" + objRest;
                            sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objFull));
                            sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(objQRCode));
                            sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(sGrade + "-" + sGroup));
                            sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(sThicknessDesc + "-" + sSize));
                            _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyHubQRCode.PRN";
                        }
                        if (Properties.Settings.Default.PrintingLocationType == "HUB")
                        {
                            //OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);//
                            OutMsg = "SUCCESS";
                        }
                        if (Properties.Settings.Default.PrintingLocationType == "PLANT")
                        {
                            //OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);//
                            OutMsg = "SUCCESS";
                        }
                        _bcilNetwork.Dispose();
                        if (OutMsg == "SUCCESS")
                        {
                            ObjLog.WriteLog("(Info) - ExistingStockPrinting : PrintingQRCode => " + "QRCode - " + objQRCode + " saved and printed successfully");
                            return OutMsg = "SUCCESS";
                        }
                    }
                    else if (sSaveStatus == "ERROR")
                    {
                        ObjLog.WriteLog("(Error) - " + "ExistingStockPrinting : PrintingQRCode => " + "QRCode - " + objQRCode + " not updated");
                        return OutMsg;
                    }
                }
                else
                {
                    if (OutMsg == "PRINTER NOT IN NETWORK")
                    {
                        ObjLog.WriteLog("(Error) - " + "ExistingStockPrinting : PrintingQRCode => " + "ERROR ~ Existing QRCode Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network");
                        OutMsg = "ERROR ~ Existing QRCode Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network";
                        return OutMsg;
                    }
                    else
                    {
                        ObjLog.WriteLog("(Error) - " + "ExistingStockPrinting : PrintingQRCode => " + "ERROR ~ Existing QRCode Printer IP : " + _bcilNetwork.PrinterIP + ", Error is : " + OutMsg);
                        OutMsg = "ERROR ~ Existing QRCode Printer IP : " + _bcilNetwork.PrinterIP + ", Error is : " + OutMsg;
                        return OutMsg;
                    }
                }
                return OutMsg;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "ExistingStockPrinting : PrintingQRCode => " + exDetail.ToString());
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
                string _sMatStatus = "E";
                OutMsg = _bcilNetwork.NetworkPrinterStatus();
                if (OutMsg == "PRINTER READY")
                {
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
                            OutMsg = "SUCCESS";
                        }
                        _bcilNetwork.Dispose();
                        
                        if (OutMsg == "SUCCESS")
                        {
                            ObjLog.WriteLog("(Info) - ExistingStockPrinting : PrintingStackQRCode => " + "StackQRCode - " + objStackQRCode + " saved and printed successfully");
                            return OutMsg = "SUCCESS";
                        }
                    }
                    else if (sSaveStatus == "ERROR")
                    {
                        ObjLog.WriteLog("(Error) - " + "ExistingStockPrinting : PrintingStackQRCode => " + "StackQRCode - " + objStackQRCode + " Not Updated");
                        return OutMsg;
                    }
                }
                else
                {
                    if (OutMsg == "PRINTER NOT IN NETWORK")
                    {
                        ObjLog.WriteLog("(Error) - " + "ExistingStockPrinting : PrintingStackQRCode => " + "ERROR ~ Existing Stack Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network");
                        OutMsg = "ERROR ~ Existing Stack Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network";
                        return OutMsg;
                    }
                    else
                    {
                        ObjLog.WriteLog("(Error) - " + "ExistingStockPrinting : PrintingStackQRCode => " + "ERROR ~ Existing Stack Printer IP : " + _bcilNetwork.PrinterIP + ", Error is : " + OutMsg);
                        OutMsg = "ERROR ~ Existing Stack Printer IP : " + _bcilNetwork.PrinterIP + " found error, Error : " + OutMsg;
                        return OutMsg;
                    }
                }
                return OutMsg;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "ExistingStockPrinting : PrintingStackQRCode => " + exDetail.ToString());
                return "ERROR | " + ex.Message;
            }
        }

        public string PrintPRDStackQRCodeItem(string objLocationCode, string objMatCode, string objStackQRCode, string sDateFormat, string sPrintingSection, string sLocationType, string GradeDesc, string GroupDesc, string ThicknessDesc, string MatSize, string LotSize, string sPONo)
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
                    string sSaveStatus = objBLItem.BLSavePRDStackQRCode(objLocationCode.Trim(), objStackQRCode.Trim(), sDateFormat.Trim(), sPrintingSection, sLocationType);
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
                        objStackQRCode = "PR-" + objStackQRCode;
                        string ObjCode2 = GradeDesc.Trim() + "-" + GroupDesc.Trim() + "-" + ThicknessDesc.Trim() + "-" + MatSize.Trim() + "-" + LotSize.Trim() + "Nos";
                        sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objStackQRCode.Trim()));
                        sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(objStackQRCode.Trim()));
                        sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(ObjCode2.Trim()));
                        _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyStackQRCode.PRN";
                        for (int i = 0; i < 2; i++)
                        {
                            //OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                            OutMsg = "SUCCESS";
                        }
                        _bcilNetwork.Dispose();
                        if (OutMsg == "SUCCESS")
                        {
                            ObjLog.WriteLog("(Info) - ExistingStockPrinting : PrintingStackQRCode => " + "StackQRCode - " + objStackQRCode + " for Production purpose is printed successfully");
                            return OutMsg = "SUCCESS";
                        }
                    }
                    else if (sSaveStatus == "ERROR")
                    {
                        ObjLog.WriteLog("(Error) - " + "ExistingStockPrinting : PrintingStackQRCode => " + "StackQRCode - " + objStackQRCode + " Not Updated");
                        return OutMsg;
                    }
                }
                else
                {
                    if (OutMsg == "PRINTER NOT IN NETWORK")
                    {
                        ObjLog.WriteLog("(Error) - " + "ExistingStockPrinting : PrintingStackQRCode => " + "ERROR ~ Existing Stack Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network");
                        OutMsg = "ERROR ~ Existing Stack Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network";
                        return OutMsg;
                    }
                    else
                    {
                        ObjLog.WriteLog("(Error) - " + "ExistingStockPrinting : PrintingStackQRCode => " + "ERROR ~ Existing Stack Printer IP : " + _bcilNetwork.PrinterIP + ", Error is : " + OutMsg);
                        OutMsg = "ERROR ~ Existing Stack Printer IP : " + _bcilNetwork.PrinterIP + ", Error is : " + OutMsg;
                        return OutMsg;
                    }
                }
                return OutMsg;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog("(Error) - " + "ExistingStockPrinting : PrintingStackQRCode => " + exDetail.ToString());
                return "ERROR | " + ex.Message;
            }
        }

        internal string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        private void EnableControls(bool value)
        {
            cmbMatCat.IsEnabled = value;
            cmbMatGrade.IsEnabled = value;
            cmbMatGroup.IsEnabled = value;
            cmbMatThickness.IsEnabled = value;
            cmbMatSize.IsEnabled = value;
            cmbDesignNo.IsEnabled = value;
            cmbFinishCode.IsEnabled = value;
            cmbVisionCode.IsEnabled = value;
            cmbLippingCode.IsEnabled = value;
        }

        private void DisableControls(bool value)
        {
            cmbVisionCode.IsEnabled = value;
            cmbLippingCode.IsEnabled = value;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult MessResult = MessageBox.Show("Do You Want To Clear All Details?", "Clear Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (MessResult == MessageBoxResult.No)
            {
                return;
            }
            if (MessResult == MessageBoxResult.Yes)
            {
                Clear();
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

        private void Clear()
        {
            if (cmbLippingCode.ItemsSource != null)
            {
                this.cmbLippingCode.SelectionChanged -= new SelectionChangedEventHandler(cmbLippingCode_SelectionChanged);
                cmbLippingCode.ItemsSource = null;
                this.cmbLippingCode.SelectionChanged += new SelectionChangedEventHandler(cmbLippingCode_SelectionChanged);
            }
            if (cmbVisionCode.ItemsSource != null)
            {
                this.cmbVisionCode.SelectionChanged -= new SelectionChangedEventHandler(cmbVisionCode_SelectionChanged);
                cmbVisionCode.ItemsSource = null;
                this.cmbVisionCode.SelectionChanged += new SelectionChangedEventHandler(cmbVisionCode_SelectionChanged);
            }
            if (cmbFinishCode.ItemsSource != null)
            {
                this.cmbFinishCode.SelectionChanged -= new SelectionChangedEventHandler(cmbFinishCode_SelectionChanged);
                cmbFinishCode.ItemsSource = null;
                this.cmbFinishCode.SelectionChanged += new SelectionChangedEventHandler(cmbFinishCode_SelectionChanged);
            }
            if (cmbDesignNo.ItemsSource != null)
            {
                this.cmbDesignNo.SelectionChanged -= new SelectionChangedEventHandler(cmbDesignNo_SelectionChanged);
                cmbDesignNo.ItemsSource = null;
                this.cmbDesignNo.SelectionChanged += new SelectionChangedEventHandler(cmbDesignNo_SelectionChanged);
            }
            //if (cmbMatSize.ItemsSource != null)
            //{
            //    this.cmbMatSize.SelectionChanged -= new SelectionChangedEventHandler(cmbMatSize_SelectionChanged);
            //    cmbMatSize.ItemsSource = null;
            //    this.cmbMatSize.SelectionChanged += new SelectionChangedEventHandler(cmbMatSize_SelectionChanged);
            //}
            //this.cmbMatSize.SelectionChanged -= new SelectionChangedEventHandler(cmbMatSize_SelectionChanged);
            //cmbMatSize.SelectedIndex = 0;
            //txtThicknessDesc.Text = string.Empty;
            //this.cmbMatSize.SelectionChanged += new SelectionChangedEventHandler(cmbMatSize_SelectionChanged);
            //if (cmbMatThickness.ItemsSource != null)
            //{
            //    this.cmbMatThickness.SelectionChanged -= new SelectionChangedEventHandler(cmbMatThickness_SelectionChanged);
            //    cmbMatThickness.ItemsSource = null;
            //    this.cmbMatThickness.SelectionChanged += new SelectionChangedEventHandler(cmbMatThickness_SelectionChanged);
            //}
            //if (cmbMatCat.ItemsSource != null)
            //{
            //    this.cmbMatCat.SelectionChanged -= new SelectionChangedEventHandler(cmbMatCat_SelectionChanged);
            //    cmbMatCat.ItemsSource = null;
            //    this.cmbMatCat.SelectionChanged += new SelectionChangedEventHandler(cmbMatCat_SelectionChanged);
            //}
            //if (cmbMatGrade.ItemsSource != null)
            //{
            //    this.cmbMatGrade.SelectionChanged -= new SelectionChangedEventHandler(cmbMatGrade_SelectionChanged);
            //    cmbMatGrade.ItemsSource = null;
            //    this.cmbMatGrade.SelectionChanged += new SelectionChangedEventHandler(cmbMatGrade_SelectionChanged);
            //}
            //if (cmbMatGroup.ItemsSource != null)
            //{
            //    this.cmbMatGroup.SelectionChanged -= new SelectionChangedEventHandler(cmbMatGroup_SelectionChanged);
            //    cmbMatGroup.ItemsSource = null;
            //    this.cmbMatGroup.SelectionChanged += new SelectionChangedEventHandler(cmbMatGroup_SelectionChanged);
            //}

            //txtGroupDesc.Text = string.Empty;
            //txtGradeDesc.Text = string.Empty;
            //txtCatDesc.Text = string.Empty;
            //txtThicknessDesc.Text = string.Empty;
            txtDesignDesc.Text = string.Empty;
            txtFinishDesc.Text = string.Empty;
            txtVisionDesc.Text = string.Empty;
            txtLippingDesc.Text = string.Empty;
            txtMatCode.Text = string.Empty;
            txtMatDesc.Text = string.Empty;
            txtUOM.Text = string.Empty;
            txtLotSize.Text = string.Empty;
            txtLotSize.IsEnabled = false;
            txtbxPONo.Text = string.Empty;
            txtVendorCode.Text = string.Empty;

            //this.cmbMatProduct.SelectionChanged -= new SelectionChangedEventHandler(cmbMatProduct_SelectionChanged);
            //cmbMatProduct.SelectedIndex = 0;
            //this.cmbMatProduct.SelectionChanged += new SelectionChangedEventHandler(cmbMatProduct_SelectionChanged);
        }

        private void txtLotSize_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtLotSize.Text == string.Empty || txtLotSize.Text == "")
                return;
            else if (!System.Text.RegularExpressions.Regex.IsMatch(txtLotSize.Text, "[0-9]"))
            {
                BCommon.setMessageBox(VariableInfo.mApp, "Kindly enter numbers only", 2);
                return;
            }
        }
    }
}
