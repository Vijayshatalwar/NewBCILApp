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

namespace GREENPLY.UserControls.Transaction
{
    /// <summary>
    /// Interaction logic for UCDepotBranchDispatch.xaml
    /// </summary>
    public partial class UCDepotBranchDispatch : UserControl
    {
        Logger objLog = new Logger();
        WriteLogFile ObjLog = new WriteLogFile();

        public UCDepotBranchDispatch()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //sPrintingSection = Properties.Settings.Default.PrintingSection.Trim().ToString();
                //sLocationType = Properties.Settings.Default.PrintingLocationType.Trim().ToString();
                //lblSelectAll.Visibility = System.Windows.Visibility.Hidden;
                //chkSelectAll.Visibility = System.Windows.Visibility.Hidden;
                //this.cmbPONum.SelectionChanged -= new SelectionChangedEventHandler(cmbPONum_SelectionChanged);
                //_strPlantCode = VariableInfo.mPlantCode;
                //GetSAPVendorPONumbers();
                //this.cmbPONum.SelectionChanged += new SelectionChangedEventHandler(cmbPONum_SelectionChanged);
                //DtSelectedItems = new DataTable();
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "VendorBarcodeGeneration => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void cmbPONum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void cmbDONumber_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

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

        private void Clear()
        {
            lv.ItemsSource = null;
            cmbPONum.SelectedIndex = 0;
        }

        #endregion
    }
}
