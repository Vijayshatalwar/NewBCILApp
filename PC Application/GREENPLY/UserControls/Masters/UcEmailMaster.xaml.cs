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

namespace GREENPLY.UserControls.Masters
{
    /// <summary>
    /// Interaction logic for UcEmailMaster.xaml
    /// </summary>
    public partial class UcEmailMaster : UserControl
    {
        Logger objLog = new Logger();


        public UcEmailMaster()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                objLog.WriteLog(exDetail);
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }
        
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {

        }

        private void lv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //EnableDisable(false);
                //btnSave.IsEnabled = false;
                //btnEdit.IsEnabled = true;
                //btnDelete.IsEnabled = false;
                //btnEdit.Content = "Edit";
                //ListViewItem item = sender as ListViewItem;
                //PL_UserMaster oPL_ASM_Master = (PL_UserMaster)item.Content;
                //this.txtUserId.Text = oPL_ASM_Master.USER_ID;
                //this.txtUserName.Text = oPL_ASM_Master.USER_NAME;
                //this.cmbgroup.Text = oPL_ASM_Master.GroupName;
                ////this.cmbGroupType.Text = oPL_ASM_Master.USER_TYPE;
                //this.cmbCompanyCode.SelectedValue = oPL_ASM_Master.PlantCode;
                //this.txtEmailID.Text = oPL_ASM_Master.USER_EMAIL;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                //objLog.WriteLog(exDetail);
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lv.ItemsSource).Refresh();
        }

        private void chkSelectAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool? bChecked = chkSelectAll.IsChecked;
                foreach (var item in _PL_UserMaster)
                {
                    item.IsValid = (bool)bChecked;
                }
                lv.ItemsSource = null;
                lv.ItemsSource = _PL_UserMaster;
                lv.UpdateLayout();
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
    }
}
