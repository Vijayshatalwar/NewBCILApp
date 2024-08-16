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
    /// Interaction logic for UcWarehouseMaster.xaml
    /// </summary>
    public partial class UcWarehouseMaster : UserControl
    {
        Logger objLog = new Logger();
        WriteLogFile ObjLog = new WriteLogFile();
        public UcWarehouseMaster()
        {
            InitializeComponent();
        }

        ObservableCollection<PL_WarehouseMaster> _PL_WarehouseMaster = new ObservableCollection<PL_WarehouseMaster>();
        public ObservableCollection<PL_WarehouseMaster> PL_WarehouseMasterData
        {
            get { return _PL_WarehouseMaster; }
            set
            {
                _PL_WarehouseMaster = value;
                OnPropertyChanged("PL_WarehouseMasterData");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyname)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyname));
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                PL_WarehouseMaster _PlWH_Master = new PL_WarehouseMaster();
                this.DisplayData(_PlWH_Master);
                btnEdit.IsEnabled = false;
                FilteredData();
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "WarehouseMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void DisplayData(PL_WarehouseMaster _PL_WHMaster)
        {
            try
            {
                PL_WarehouseMasterData = new BL_WarehouseMaster().BL_GetWarehouseData(_PL_WHMaster);
                lv.ItemsSource = PL_WarehouseMasterData;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "WarehouseMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
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

            if (String.IsNullOrEmpty(txtSearch.Text))
            {
                return true;
            }
            else
            {
                return ((item as PL_WarehouseMaster).WarehouseDesc.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private bool ValidateInputs(bool p, bool bDelete)
        {
            try
            {
                if (String.IsNullOrEmpty(this.txtWarehouseId.Text.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter Warehouse ID", 1);
                    this.txtWarehouseId.Focus();
                    return p = false;

                }
                else if (String.IsNullOrEmpty(this.txtWarehouseDesc.Text.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter Warehouse Description", 1);
                    this.txtWarehouseDesc.Focus();
                    return p = false;
                }
                else if (String.IsNullOrEmpty(this.txtWarehouseAdd.Text.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter Warehouse Address", 1);
                    this.txtWarehouseAdd.Focus();
                    return p = false;
                }
                //if (bDelete != true)
                //{
                //    if (String.IsNullOrEmpty(this.txtWarehouseId.ToString().Trim()))
                //    {
                //        BCommon.setMessageBox(VariableInfo.mApp, "Enter Password first", 1);
                //        this.txtWarehouseId.Focus();
                //        return p = false;
                //    }
                //    else if (String.IsNullOrEmpty(this.txtWarehouseDesc.ToString().Trim()))
                //    {
                //        BCommon.setMessageBox(VariableInfo.mApp, "Enter Confirm Password first!!", 1);
                //        this.txtWarehouseDesc.Focus();
                //        return p = false;
                //    }
                //    else if (String.IsNullOrEmpty(this.txtWarehouseAdd.ToString().Trim()))
                //    {
                //        BCommon.setMessageBox(VariableInfo.mApp, "Enter Confirm Password first!!", 1);
                //        this.txtWarehouseAdd.Focus();
                //        return p = false;
                //    }
                //    bDelete = false;
                //}
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "WarehouseMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
            return p;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int iCount = 0;
                OperationResult oResponse = OperationResult.DeleteError;
                if (lv.Items.Count <= 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "No Data Found For Delete", 1);
                    return;
                }
                MessageBoxResult MessResult = MessageBox.Show("Delete Confirmation", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (MessResult == MessageBoxResult.No)
                {
                    return;
                }
                foreach (PL_WarehouseMaster item in lv.ItemsSource)
                {
                    PL_WarehouseMaster _objEntity_RM = item;
                    {
                        if (item.IsValid == true)
                        {
                            oResponse = new BL_WarehouseMaster().BL_DeleteWarehose(_objEntity_RM);
                        }
                        else
                        {
                            iCount++;
                            continue;
                        }
                    };
                }
                if (lv.Items.Count == iCount)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Select At Least One Record To Delete", 1);
                    return;
                }
                
                else if (oResponse == OperationResult.DeleteSuccess)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Selected Record Deleted Successfully", 4);
                    ResetForm();
                    Clear();
                    PL_WarehouseMaster _objEntity_RM = new PL_WarehouseMaster();
                    DisplayData(_objEntity_RM);
                }
                else if (oResponse == OperationResult.DeleteError)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Error On Delete, Kindly Try Again", 3);
                }
                else if (oResponse == OperationResult.Error)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Warehouse Id is Used in Another Process, Can Not be Deleted", 3);
                }
                else if (oResponse == OperationResult.DeleteRefference)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Record Used In Transaction, Can Not be Deleted", 3);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "WarehouseMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
            finally
            {
                chkSelectAll.IsChecked = false;
                FilteredData();
            }
        }

        void Clear()
        {
            PL_WarehouseMaster _PL_WHMaster = new PL_WarehouseMaster();
            foreach (var items in lv.Items)
            {
                ((PL_WarehouseMaster)(items)).IsValid = false;
                DisplayData(_PL_WHMaster);
            }
            chkSelectAll.IsChecked = false;
            txtWarehouseId.Text = string.Empty;
            txtWarehouseDesc.Text = string.Empty;
            txtWarehouseAdd.Text = string.Empty;
            txtSearch.Text = string.Empty;
            txtSearch.IsEnabled = true;
            txtWarehouseAdd.IsEnabled = true;
            txtWarehouseDesc.IsEnabled = true;
            txtWarehouseId.IsEnabled = true;
        }

        void ResetForm()
        {
            txtWarehouseId.Text = string.Empty;
            txtWarehouseDesc.Text = string.Empty;
            txtWarehouseAdd.Text = string.Empty;
            btnReset.IsEnabled = true;
            btnSave.IsEnabled = true;
            btnDelete.IsEnabled = true;
            btnEdit.IsEnabled = false;
            EnableDisableGB(true);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToString(btnEdit.Content) == "Edit")
                {
                    btnEdit.Content = "Update";
                    txtWarehouseId.IsEnabled = false;
                    txtWarehouseDesc.IsEnabled = true;
                    txtWarehouseAdd.IsEnabled = true;
                }
                else if (Convert.ToString(btnEdit.Content) == "Update")
                {
                    OperationResult oResponse = OperationResult.UpdateError;
                    bool bDelete = false;
                    bool sInputs = this.ValidateInputs(true, bDelete);
                    if (sInputs != false)
                    {
                        PL_WarehouseMaster _objEntity_ASM = new PL_WarehouseMaster();
                        {
                            _objEntity_ASM.WarehouseId = this.txtWarehouseId.Text.Trim();
                            _objEntity_ASM.WarehouseDesc = this.txtWarehouseDesc.Text.Trim();
                            _objEntity_ASM.WarehouseAdd = this.txtWarehouseAdd.Text.Trim();
                            _objEntity_ASM.CreatedBy = Convert.ToString(VariableInfo.dtVlidateuser) != "" ? Convert.ToString(VariableInfo.dtVlidateuser.Rows[0]["USER_ID"]) : "";
                        };
                        oResponse = new BL_WarehouseMaster().BL_UpdateWarehouseData(_objEntity_ASM);
                        if (oResponse == OperationResult.UpdateSuccess)
                        {
                            BCommon.setMessageBox(VariableInfo.mApp, "Record Updated Successfully", 4);
                            ResetForm();
                            Clear();
                            DisplayData(_objEntity_ASM);
                            btnEdit.Content = "Edit";
                            chkSelectAll.IsChecked = false;
                        }
                        else if (oResponse == OperationResult.UpdateError)
                        {
                            BCommon.setMessageBox(VariableInfo.mApp, "Error On Update", 3);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "WarehouseMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
            finally
            {
                FilteredData();
            }
        }
            
        void btnReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult MessResult = MessageBox.Show("Do You Want To Clear All Details?", "Clear Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (MessResult == MessageBoxResult.No)
            {
                return;
            }
            btnEdit.Content = "Edit";
            Clear();
            ResetForm();

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.Cursor = Cursors.Wait;
                string sResult = string.Empty;
                OperationResult oResponse = OperationResult.SaveError;
                bool bDelete = false;
                bool sInputs = ValidateInputs(true, bDelete);
                if (sInputs != false)
                {
                    PL_WarehouseMaster _objEntity_ASM = new PL_WarehouseMaster();
                    {
                        _objEntity_ASM.WarehouseId = this.txtWarehouseId.Text.Trim();
                        _objEntity_ASM.WarehouseDesc = this.txtWarehouseDesc.Text.Trim();
                        _objEntity_ASM.WarehouseAdd = this.txtWarehouseAdd.Text.Trim();
                        _objEntity_ASM.CreatedBy = Convert.ToString(VariableInfo.dtVlidateuser) != "" ? Convert.ToString(VariableInfo.dtVlidateuser.Rows[0]["USER_ID"]) : "";    
                    };
                    MessageBoxResult MessResult = MessageBox.Show("Do You Want To Save All Details?", "Save Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                    if (MessResult == MessageBoxResult.No)
                    { return; }
                    oResponse = new BL_WarehouseMaster().BL_SaveWarehouseData(_objEntity_ASM);
                    if (oResponse == OperationResult.SaveSuccess)
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Record Save Successfully", 4);
                        VariableInfo.sbSaveCount = new StringBuilder();
                        VariableInfo.sbDuplicateCount = new StringBuilder();
                        ResetForm();
                        Clear();
                        DisplayData(_objEntity_ASM);
                    }
                    else if (oResponse == OperationResult.SaveError)
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Error in Saving Record, Kindly Try Again", 3);
                    }
                    else if (oResponse == OperationResult.Duplicate)
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, _objEntity_ASM.WarehouseId + " Already Exists, Kindly Change", 2);
                    }
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "WarehouseMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
                btnSave.Cursor = Cursors.Arrow;
            }
            finally
            {
                btnSave.Cursor = Cursors.Arrow;
                FilteredData(); 
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

                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void lv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                EnableDisable(false);
                btnSave.IsEnabled = false;
                btnEdit.IsEnabled = true;
                btnDelete.IsEnabled = false;
                btnEdit.Content = "Edit";
                ListViewItem item = sender as ListViewItem;
                PL_WarehouseMaster oPL_ASM_Master = (PL_WarehouseMaster)item.Content;
                this.txtWarehouseId.Text = oPL_ASM_Master.WarehouseId;
                this.txtWarehouseDesc.Text = oPL_ASM_Master.WarehouseDesc;
                this.txtWarehouseAdd.Text = oPL_ASM_Master.WarehouseAdd;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "WarehouseMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void EnableDisable(bool value)
        {
            btnSave.IsEnabled = value;
            txtWarehouseId.IsEnabled = value;
            txtWarehouseDesc.IsEnabled = value;
            txtWarehouseAdd.IsEnabled = value;
        }

        private void EnableDisableGB(bool value)
        {
            gbEntry.IsEnabled = value;
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
                foreach (var item in _PL_WarehouseMaster)
                {
                    item.IsValid = (bool)bChecked;
                }
                lv.ItemsSource = null;
                lv.ItemsSource = _PL_WarehouseMaster;
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
