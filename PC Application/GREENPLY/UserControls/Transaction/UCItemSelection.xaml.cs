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
using COMMON_LAYER;
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
using BcilLib;

namespace GREENPLY.UserControls.Transaction
{
    /// <summary>
    /// Interaction logic for UCItemSelection.xaml
    /// </summary>
    public partial class UCItemSelection : UserControl
    {
        Logger objLog = new Logger();
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

        public UCItemSelection()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.cmbMatProduct.SelectionChanged -= new SelectionChangedEventHandler(cmbMatProduct_SelectionChanged);
                _strPlantCode = VariableInfo.mPlantCode;
                GetMatProduct();
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Info) - " + "ConveyorOperation => " + "");
                this.cmbMatProduct.SelectionChanged += new SelectionChangedEventHandler(cmbMatProduct_SelectionChanged);
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetMatProduct()
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatProduct = new DataTable();
                DtMatProduct = objBLItem.BLGetMatProduct();
                if (DtMatProduct.Rows.Count > 0)
                {
                    DataRow dr = DtMatProduct.NewRow();
                    dr[0] = "--Select--";
                    DtMatProduct.Rows.InsertAt(dr, 0);
                    DataView dataView = new DataView(DtMatProduct);
                    this.cmbMatProduct.ItemsSource = dataView;
                    cmbMatProduct.SelectedIndex = 0;
                }
                else
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "There is No Material Product Found", 2);
                    return;
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetMatGroup(string objProduct)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatGroup = new DataTable();
                DtMatGroup = objBLItem.BLGetMatGroup(objProduct);
                if (DtMatGroup.Rows.Count > 0)
                {
                    DataRow dr = DtMatGroup.NewRow();
                    dr[0] = "--Select--";
                    dr[1] = "--Select--";
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
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetMatGrade(string objProduct, string objGroup)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatGrade = new DataTable();
                DtMatGrade = objBLItem.BLGetMatGrade(objProduct, objGroup);
                if (DtMatGrade.Rows.Count > 0)
                {
                    txtGroupDesc.Text = DtMatGrade.Rows[0][0].ToString();
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
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetMatCategory(string objProduct, string objGroup, string objGrade)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatCat = new DataTable();
                DtMatCat = objBLItem.BLGetMatCategory(objProduct, objGroup, objGrade);
                if (DtMatCat.Rows.Count > 0)
                {
                    txtGradeDesc.Text = DtMatCat.Rows[0][0].ToString();
                    DataRow dr = DtMatCat.NewRow();
                    dr[1] = "--Select--";
                    dr[2] = "--Select--";
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
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetMatThickness(string objProduct, string objGroup, string objGrade, string objCat)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatThickness = new DataTable();
                DtMatThickness = objBLItem.BLGetMatThickness(objProduct, objGroup, objGrade, objCat);
                if (DtMatThickness.Rows.Count > 0)
                {
                    txtCatDesc.Text = DtMatThickness.Rows[0][0].ToString();
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
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetMatSize(string objProduct, string objGroup, string objGrade, string objCat, string objThickness)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatSize = new DataTable();
                DtMatSize = objBLItem.BLGetMatSize(objProduct, objGroup, objGrade, objCat, objThickness);
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
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetMatDesignNo(string objProduct, string objGroup, string objGrade, string objCat, string objThickness, string objSize)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatDesign = new DataTable();
                DtMatDesign = objBLItem.BLGetMatDesign(objProduct, objGroup, objGrade, objCat, objThickness, objSize);
                if (DtMatDesign.Rows.Count > 0)
                {
                    string objMd = DtMatDesign.Rows[0][0].ToString();
                    if (objMd == null || objMd == "")
                    {
                        cmbDesignNo.IsEnabled = false;
                        GetMatFinishCode(objProduct, objGroup, objGrade, objCat, objThickness, objSize, objMatDesignNo);
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
                    GetMatFinishCode(objProduct, objGroup, objGrade, objCat, objThickness, objSize, objMatDesignNo);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetMatFinishCode(string objProduct, string objGroup, string objGrade, string objCat, string objThickness, string objSize, string objDesignNo)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatFinishCode = new DataTable();
                DtMatFinishCode = objBLItem.BLGetMatFinishCode(objProduct, objGroup, objGrade, objCat, objThickness, objSize, objDesignNo);
                if (DtMatFinishCode.Rows.Count > 0)
                {
                    string objMFD = DtMatFinishCode.Rows[0][1].ToString();
                    if (objMFD == null || objMFD == "")
                    {
                        cmbFinishCode.IsEnabled = false;
                        GetMatVisionCode(objProduct, objGroup, objGrade, objCat, objThickness, objSize, objDesignNo, objMatFinishCode);
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
                    GetMatVisionCode(objProduct, objGroup, objGrade, objCat, objThickness, objSize, objDesignNo, objMatFinishCode);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetMatVisionCode(string objProduct, string objGroup, string objGrade, string objCat, string objThickness, string objSize, string objDesignNo, string FinishCode)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatVisionCode = new DataTable();
                DtMatVisionCode = objBLItem.BLGetMatVisionCode(objProduct, objGroup, objGrade, objCat, objThickness, objSize, objDesignNo, FinishCode);
                if (DtMatVisionCode.Rows.Count > 0)
                {
                    string objMV = DtMatFinishCode.Rows[0][1].ToString();
                    if (objMV == null || objMV == "")
                    {
                        cmbVisionCode.IsEnabled = false;
                        GetMatLippingCode(objProduct, objGroup, objGrade, objCat, objThickness, objSize, objDesignNo, FinishCode, objMatVisionCode);
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
                    GetMatLippingCode(objProduct, objGroup, objGrade, objCat, objThickness, objSize, objDesignNo, FinishCode, objMatVisionCode);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetMatLippingCode(string objProduct, string objGroup, string objGrade, string objCat, string objThickness, string objSize, string objDesignNo, string FinishCode, string objVisionCode)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatLippingCode = new DataTable();
                DtMatLippingCode = objBLItem.BLGetMatLippingCode(objProduct, objGroup, objGrade, objCat, objThickness, objSize, objDesignNo, FinishCode, objVisionCode);
                if (DtMatLippingCode.Rows.Count > 0)
                {
                    string objLC = DtMatLippingCode.Rows[0][1].ToString();
                    if (objLC == null || objLC == "")
                    {
                        cmbLippingCode.IsEnabled = false;
                        GetSelectedMatCode(objProduct, objGroup, objGrade, objCat, objThickness, objSize, objDesignNo, FinishCode, objVisionCode, objLC);
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
                    GetSelectedMatCode(objProduct, objGroup, objGrade, objCat, objThickness, objSize, objDesignNo, FinishCode, objVisionCode, objMatLippingCode);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetSelectedMatCode(string objProduct, string objGroup, string objGrade, string objCat, string objThickness, string objSize, string objDesignNo, string FinishCode, string objVisionCode, string objLippingCode)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatData = new DataTable();
                DtMatData = objBLItem.BLGetSelectedMatCode(objProduct, objGroup, objGrade, objCat, objThickness, objSize, objDesignNo, FinishCode, objVisionCode, objLippingCode);
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
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void SaveSelectedMatCode(string objMatCode)
        {
            try
            {
                objBLItem = new BL_ItemSelection();
                DtMatData = new DataTable();
                DtMatData = objBLItem.BLSaveSelectedMatCode(objMatCode, txtMatDesc.Text.Trim(), cmbMatSize.SelectedValue.ToString().Trim(), txtThicknessDesc.Text.Trim(), Convert.ToInt32(txtLotSize.Text));
                if (DtMatData.Columns.Contains("STATUS") && DtMatData.Rows[0][0].ToString() == "1" && DtMatData.Rows.Count > 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Material Code - " + objMatCode + " is Selected Successfully", 1);
                    Clear();
                    return;
                }
                else
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "There is An Error in Selection of Material Code - " + objMatCode + ", Kindly Try Again", 2);
                    return;
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
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
                    if (cmbMatCat.ItemsSource != null)
                    {
                        this.cmbMatCat.SelectionChanged -= new SelectionChangedEventHandler(cmbMatCat_SelectionChanged);
                        cmbMatCat.ItemsSource = null;
                        this.cmbMatCat.SelectionChanged += new SelectionChangedEventHandler(cmbMatCat_SelectionChanged);
                    }
                    if (cmbMatGrade.ItemsSource != null)
                    {
                        this.cmbMatGrade.SelectionChanged -= new SelectionChangedEventHandler(cmbMatGrade_SelectionChanged);
                        cmbMatGrade.ItemsSource = null;
                        this.cmbMatGrade.SelectionChanged += new SelectionChangedEventHandler(cmbMatGrade_SelectionChanged);
                    }
                    if (cmbMatGroup.ItemsSource != null)
                    {
                        this.cmbMatGroup.SelectionChanged -= new SelectionChangedEventHandler(cmbMatGroup_SelectionChanged);
                        cmbMatGroup.ItemsSource = null;
                        this.cmbMatGroup.SelectionChanged += new SelectionChangedEventHandler(cmbMatGroup_SelectionChanged);
                    }

                    txtGroupDesc.Text = string.Empty;
                    txtGradeDesc.Text = string.Empty;
                    txtCatDesc.Text = string.Empty;
                    txtThicknessDesc.Text = string.Empty;
                    txtDesignDesc.Text = string.Empty;
                    txtFinishDesc.Text = string.Empty;
                    txtVisionDesc.Text = string.Empty;
                    txtLippingDesc.Text = string.Empty;
                    txtMatCode.Text = string.Empty;
                    txtMatDesc.Text = string.Empty;
                    txtUOM.Text = string.Empty;
                    txtLotSize.Text = string.Empty;

                    objMatProduct = string.Empty;
                    objMatProduct = cmbMatProduct.SelectedValue.ToString().Trim();
                    GetMatGroup(objMatProduct);
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
                        DisableControls(false);
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
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
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
                    if (cmbMatCat.ItemsSource != null)
                    {
                        this.cmbMatCat.SelectionChanged -= new SelectionChangedEventHandler(cmbMatCat_SelectionChanged);
                        cmbMatCat.ItemsSource = null;
                        this.cmbMatCat.SelectionChanged += new SelectionChangedEventHandler(cmbMatCat_SelectionChanged);
                    }
                    if (cmbMatGrade.ItemsSource != null)
                    {
                        this.cmbMatGrade.SelectionChanged -= new SelectionChangedEventHandler(cmbMatGrade_SelectionChanged);
                        cmbMatGrade.ItemsSource = null;
                        this.cmbMatGrade.SelectionChanged += new SelectionChangedEventHandler(cmbMatGrade_SelectionChanged);
                    }

                    txtGroupDesc.Text = string.Empty;
                    txtGradeDesc.Text = string.Empty;
                    txtCatDesc.Text = string.Empty;
                    txtThicknessDesc.Text = string.Empty;
                    txtDesignDesc.Text = string.Empty;
                    txtFinishDesc.Text = string.Empty;
                    txtVisionDesc.Text = string.Empty;
                    txtLippingDesc.Text = string.Empty;
                    txtMatCode.Text = string.Empty;
                    txtMatDesc.Text = string.Empty;
                    txtUOM.Text = string.Empty;
                    txtLotSize.Text = string.Empty;

                    objMatGroup = string.Empty;
                    objMatGroup = cmbMatGroup.SelectedValue.ToString().Trim();
                    GetMatGrade(objMatProduct, objMatGroup);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
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
                if (cmbMatGroup.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Group", 2);
                    cmbMatGroup.Focus();
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
                    if (cmbMatCat.ItemsSource != null)
                    {
                        this.cmbMatCat.SelectionChanged -= new SelectionChangedEventHandler(cmbMatCat_SelectionChanged);
                        cmbMatCat.ItemsSource = null;
                        this.cmbMatCat.SelectionChanged += new SelectionChangedEventHandler(cmbMatCat_SelectionChanged);
                    }

                    txtGradeDesc.Text = string.Empty;
                    txtCatDesc.Text = string.Empty;
                    txtThicknessDesc.Text = string.Empty;
                    txtDesignDesc.Text = string.Empty;
                    txtFinishDesc.Text = string.Empty;
                    txtVisionDesc.Text = string.Empty;
                    txtLippingDesc.Text = string.Empty;
                    txtMatCode.Text = string.Empty;
                    txtMatDesc.Text = string.Empty;
                    txtUOM.Text = string.Empty;
                    txtLotSize.Text = string.Empty;

                    objMatGrade = string.Empty;
                    objMatGrade = cmbMatGrade.SelectedValue.ToString().Trim();
                    GetMatCategory(objMatProduct, objMatGroup, objMatGrade);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
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
                if (cmbMatGroup.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Group", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                if (cmbMatGrade.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Grade", 2);
                    cmbMatGrade.Focus();
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
                    txtCatDesc.Text = string.Empty;
                    txtThicknessDesc.Text = string.Empty;
                    txtDesignDesc.Text = string.Empty;
                    txtFinishDesc.Text = string.Empty;
                    txtVisionDesc.Text = string.Empty;
                    txtLippingDesc.Text = string.Empty;
                    txtMatCode.Text = string.Empty;
                    txtMatDesc.Text = string.Empty;
                    txtUOM.Text = string.Empty;
                    txtLotSize.Text = string.Empty;

                    objMatCat = string.Empty;
                    objMatCat = cmbMatCat.SelectedValue.ToString().Trim();
                    GetMatThickness(objMatProduct, objMatGroup, objMatGrade, objMatCat);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
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
                if (cmbMatGroup.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Group", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                if (cmbMatGrade.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Grade", 2);
                    cmbMatGrade.Focus();
                    return;
                }
                if (cmbMatCat.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Category", 2);
                    cmbMatCat.Focus();
                    return;
                }
                if (cmbMatThickness.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Thickness", 2);
                    cmbMatThickness.Focus();
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

                    txtThicknessDesc.Text = string.Empty;
                    txtDesignDesc.Text = string.Empty;
                    txtFinishDesc.Text = string.Empty;
                    txtVisionDesc.Text = string.Empty;
                    txtLippingDesc.Text = string.Empty;
                    txtMatCode.Text = string.Empty;
                    txtMatDesc.Text = string.Empty;
                    txtUOM.Text = string.Empty;
                    txtLotSize.Text = string.Empty;

                    objMatThickness = string.Empty;
                    objMatThickness = cmbMatThickness.SelectedValue.ToString().Trim();
                    GetMatSize(objMatProduct, objMatGroup, objMatGrade, objMatCat, objMatThickness);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
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
                if (cmbMatGroup.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Group", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                if (cmbMatGrade.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Grade", 2);
                    cmbMatGrade.Focus();
                    return;
                }
                if (cmbMatCat.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Category", 2);
                    cmbMatCat.Focus();
                    return;
                }
                if (cmbMatThickness.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Thickness", 2);
                    cmbMatThickness.Focus();
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

                    txtDesignDesc.Text = string.Empty;
                    txtFinishDesc.Text = string.Empty;
                    txtVisionDesc.Text = string.Empty;
                    txtLippingDesc.Text = string.Empty;
                    txtMatCode.Text = string.Empty;
                    txtMatDesc.Text = string.Empty;
                    txtUOM.Text = string.Empty;
                    txtLotSize.Text = string.Empty;

                    objMatSize = string.Empty;
                    objMatSize = cmbMatSize.SelectedValue.ToString().Trim();
                    if (cmbDesignNo.IsEnabled == true)
                        GetMatDesignNo(objMatProduct, objMatGroup, objMatGrade, objMatCat, objMatThickness, objMatSize);
                    else if (cmbDesignNo.IsEnabled == false)
                        GetMatFinishCode(objMatProduct, objMatGroup, objMatGrade, objMatCat, objMatThickness, objMatSize, objMatDesignNo);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
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
                if (cmbMatGroup.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Group", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                if (cmbMatGrade.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Grade", 2);
                    cmbMatGrade.Focus();
                    return;
                }
                if (cmbMatCat.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Category", 2);
                    cmbMatCat.Focus();
                    return;
                }
                if (cmbMatThickness.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Thickness", 2);
                    cmbMatThickness.Focus();
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

                    txtDesignDesc.Text = string.Empty;
                    txtFinishDesc.Text = string.Empty;
                    txtVisionDesc.Text = string.Empty;
                    txtLippingDesc.Text = string.Empty;
                    txtMatCode.Text = string.Empty;
                    txtMatDesc.Text = string.Empty;
                    txtUOM.Text = string.Empty;
                    txtLotSize.Text = string.Empty;

                    objMatDesignNo = string.Empty;
                    objMatDesignNo = cmbDesignNo.SelectedValue.ToString().Trim();
                    if (cmbFinishCode.IsEnabled == true)
                        GetMatFinishCode(objMatProduct, objMatGroup, objMatGrade, objMatCat, objMatThickness, objMatSize, objMatDesignNo);
                    else if (cmbFinishCode.IsEnabled == false)
                        GetMatVisionCode(objMatProduct, objMatGroup, objMatGrade, objMatCat, objMatThickness, objMatSize, objMatDesignNo, objMatFinishCode);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
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
                if (cmbMatGroup.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Group", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                if (cmbMatGrade.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Grade", 2);
                    cmbMatGrade.Focus();
                    return;
                }
                if (cmbMatCat.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Category", 2);
                    cmbMatCat.Focus();
                    return;
                }
                if (cmbMatThickness.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Thickness", 2);
                    cmbMatThickness.Focus();
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

                    objMatFinishCode = string.Empty;
                    objMatFinishCode = cmbFinishCode.SelectedValue.ToString().Trim();
                    if (cmbVisionCode.IsEnabled == true)
                        GetMatVisionCode(objMatProduct, objMatGroup, objMatGrade, objMatCat, objMatThickness, objMatSize, objMatDesignNo, objMatFinishCode);
                    else if (cmbVisionCode.IsEnabled == false)
                    {
                        if (cmbLippingCode.IsEnabled == true)
                            GetMatLippingCode(objMatProduct, objMatGroup, objMatGrade, objMatCat, objMatThickness, objMatSize, objMatDesignNo, objMatFinishCode, objMatVisionCode);
                        else if (cmbLippingCode.IsEnabled == false)
                        {
                            GetSelectedMatCode(objMatProduct, objMatGroup, objMatGrade, objMatCat, objMatThickness, objMatSize, objMatDesignNo, objMatFinishCode, objMatVisionCode, objMatLippingCode);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
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
                if (cmbMatGroup.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Group", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                if (cmbMatGrade.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Grade", 2);
                    cmbMatGrade.Focus();
                    return;
                }
                if (cmbMatCat.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Category", 2);
                    cmbMatCat.Focus();
                    return;
                }
                if (cmbMatThickness.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Thickness", 2);
                    cmbMatThickness.Focus();
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

                    objMatVisionCode = string.Empty;
                    objMatVisionCode = cmbVisionCode.SelectedValue.ToString().Trim();
                    if (cmbLippingCode.IsEnabled == true)
                        GetMatLippingCode(objMatProduct, objMatGroup, objMatGrade, objMatCat, objMatThickness, objMatSize, objMatDesignNo, objMatFinishCode, objMatVisionCode);
                    else if (cmbLippingCode.IsEnabled == false)
                        GetSelectedMatCode(objMatProduct, objMatGroup, objMatGrade, objMatCat, objMatThickness, objMatSize, objMatDesignNo, objMatFinishCode, objMatVisionCode, objMatLippingCode);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
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
                if (cmbMatGroup.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Group", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                if (cmbMatGrade.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Grade", 2);
                    cmbMatGrade.Focus();
                    return;
                }
                if (cmbMatCat.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Category", 2);
                    cmbMatCat.Focus();
                    return;
                }
                if (cmbMatThickness.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Thickness", 2);
                    cmbMatThickness.Focus();
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

                    objMatLippingCode = string.Empty;
                    objMatLippingCode = cmbLippingCode.SelectedValue.ToString().Trim();
                    if (cmbLippingCode.IsEnabled == true)
                        GetSelectedMatCode(objMatProduct, objMatGroup, objMatGrade, objMatCat, objMatThickness, objMatSize, objMatDesignNo, objMatFinishCode, objMatVisionCode, objMatLippingCode);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }


        private void EnableControls(bool value)
        {
            cmbMatGroup.IsEnabled = value;
            cmbMatGrade.IsEnabled = value;
            cmbMatCat.IsEnabled = value;
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbMatProduct.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Product", 2);
                    cmbMatProduct.Focus();
                    return;
                }
                if (cmbMatGroup.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Group", 2);
                    cmbMatGroup.Focus();
                    return;
                }
                if (cmbMatGrade.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Grade", 2);
                    cmbMatGrade.Focus();
                    return;
                }
                if (cmbMatCat.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Category", 2);
                    cmbMatCat.Focus();
                    return;
                }
                if (cmbMatThickness.SelectedIndex == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Select The Material Thickness", 2);
                    cmbMatThickness.Focus();
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
                if (txtLotSize.Text == string.Empty)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Enter Material Quantity To Print", 2);
                    txtLotSize.Focus();
                    return;
                }
                if (txtLotSize.Text == "0" || txtLotSize.Text == "00" || txtLotSize.Text == "000")
                {
                    BCommon.setMessageBox(VariableInfo.mApp, txtLotSize.Text + " Not Valid Quantity, Kindly Change", 2);
                    txtLotSize.Focus();
                    return;
                }
                else
                {
                    string sMat = txtMatCode.Text.ToString().Trim();
                    SaveSelectedMatCode(sMat);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " (Error) - " + "ConveyorOperation => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
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
