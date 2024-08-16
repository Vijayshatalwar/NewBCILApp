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
    /// Interaction logic for UcVendorMaster.xaml
    /// </summary>
    public partial class UcVendorMaster : UserControl
    {
        Logger objLog = new Logger();
        WriteLogFile ObjLog = new WriteLogFile();

        public UcVendorMaster()
        {
            InitializeComponent();
        }

        ObservableCollection<PL_VendorMaster> _PL_VendorMaster = new ObservableCollection<PL_VendorMaster>();
        public ObservableCollection<PL_VendorMaster> PL_VendorMasterData
        {
            get { return _PL_VendorMaster; }
            set
            {
                _PL_VendorMaster = value;
                OnPropertyChanged("PL_VendorMasterData");
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
                PL_VendorMaster _PlVendorMaster = new PL_VendorMaster();
                this.DisplayData(_PlVendorMaster);
                btnEdit.IsEnabled = false;
                FilteredData();
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "VendorMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void DisplayData(PL_VendorMaster _PlVendorMaster)
        {
            try
            {
                PL_VendorMasterData = new BL_VendorMaster().BL_GetVendorMasterData(_PlVendorMaster);
                lv.ItemsSource = PL_VendorMasterData;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "VendorMaster => " + exDetail.ToString());
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
                return ((item as PL_VendorMaster).VendorDesc.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private bool ValidateInputs(bool p, bool bDelete)
        {
            try
            {
                if (String.IsNullOrEmpty(this.txtVendorId.Text.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter Vendor ID", 1);
                    this.txtVendorId.Focus();
                    return p = false;
                }
                if (String.IsNullOrEmpty(this.txtPassword.Password.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter Vendor Password", 1);
                    this.txtVendorId.Focus();
                    return p = false;
                }
                else if (String.IsNullOrEmpty(this.txtVendorName.Text.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter Vendor Description", 1);
                    this.txtVendorName.Focus();
                    return p = false;
                }
                else if (String.IsNullOrEmpty(this.txtVendorEmail.Text.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter Vendor EMail Id", 1);
                    this.txtVendorEmail.Focus();
                    return p = false;
                }
                else if (!Regex.IsMatch(txtVendorEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter Valid Email Id", 1);
                    this.txtVendorEmail.Focus();
                    return p = false;
                }
                else if (String.IsNullOrEmpty(this.txtVendorAddress.Text.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter Vendor Address", 1);
                    this.txtVendorAddress.Focus();
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
                ObjLog.WriteLog(" (Error) - " + "VendorMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
            return p;
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
                    PL_VendorMaster _objEntity_ASM = new PL_VendorMaster();
                    {
                        _objEntity_ASM.VendorId = this.txtVendorId.Text.Trim();
                        _objEntity_ASM.VendorDesc = this.txtVendorName.Text.Trim();
                        _objEntity_ASM.VendorAdd = this.txtVendorAddress.Text.Trim();
                        _objEntity_ASM.VendorEmail = this.txtVendorEmail.Text.Trim();
                        _objEntity_ASM.VendorPwd = VariableInfo.EncryptPassword(txtPassword.Password, "E");
                        _objEntity_ASM.CreatedBy = Convert.ToString(VariableInfo.dtVlidateuser) != "" ? Convert.ToString(VariableInfo.dtVlidateuser.Rows[0]["USER_ID"]) : "";
                    };
                    MessageBoxResult MessResult = MessageBox.Show("Do You Want To Save All Details?", "Save Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                    if (MessResult == MessageBoxResult.No)
                    { 
                        return;
                    }
                    oResponse = new BL_VendorMaster().BL_SaveVendorData(_objEntity_ASM);
                    if (oResponse == OperationResult.SaveSuccess)
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Record Saved Successfully", 4);
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
                        BCommon.setMessageBox(VariableInfo.mApp, _objEntity_ASM.VendorId + " Already Exists, Kindly Change", 2);
                    }
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "VendorMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
                btnSave.Cursor = Cursors.Arrow;
            }
            finally
            {
                btnSave.Cursor = Cursors.Arrow;
                FilteredData();
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToString(btnEdit.Content) == "Edit")
                {
                    btnEdit.Content = "Update";
                    txtVendorId.IsEnabled = false;
                    txtVendorName.IsEnabled = true;
                    txtVendorAddress.IsEnabled = true;
                    txtVendorEmail.IsEnabled = true;
                    txtPassword.IsEnabled = true;
                }
                else if (Convert.ToString(btnEdit.Content) == "Update")
                {
                    OperationResult oResponse = OperationResult.UpdateError;
                    bool bDelete = false;
                    bool sInputs = this.ValidateInputs(true, bDelete);
                    if (sInputs != false)
                    {
                        PL_VendorMaster _objEntity_ASM = new PL_VendorMaster();
                        {
                            _objEntity_ASM.VendorId = this.txtVendorId.Text.Trim();
                            _objEntity_ASM.VendorDesc = this.txtVendorName.Text.Trim();
                            _objEntity_ASM.VendorAdd = this.txtVendorAddress.Text.Trim();
                            _objEntity_ASM.VendorEmail = this.txtVendorEmail.Text.Trim();
                            _objEntity_ASM.VendorPwd = VariableInfo.EncryptPassword(txtPassword.Password, "E");
                            _objEntity_ASM.CreatedBy = Convert.ToString(VariableInfo.dtVlidateuser) != "" ? Convert.ToString(VariableInfo.dtVlidateuser.Rows[0]["USER_ID"]) : "";
                        };
                        oResponse = new BL_VendorMaster().BL_UpdateVendorData(_objEntity_ASM);
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
                            BCommon.setMessageBox(VariableInfo.mApp, "Error in Updating The Record, Kindly Try Again", 3);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "VendorMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
            finally
            {
                FilteredData();
            }
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
                foreach (PL_VendorMaster item in lv.ItemsSource)
                {
                    PL_VendorMaster _objEntity_RM = item;
                    {
                        if (item.IsValid == true)
                        {
                            oResponse = new BL_VendorMaster().BL_DeleteVendor(_objEntity_RM);
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
                    PL_VendorMaster _objEntity_RM = new PL_VendorMaster();
                    DisplayData(_objEntity_RM);
                }
                else if (oResponse == OperationResult.DeleteError)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Error On Delete, Kindly Try Again", 3);
                }
                else if (oResponse == OperationResult.Error)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Vendor is Used in Another Process, Can Not be Deleted", 3);
                }
                else if (oResponse == OperationResult.DeleteRefference)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Record Used In Transaction,Can't be Delete", 3);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "VendorMaster => " + exDetail.ToString());
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
            PL_VendorMaster _PL_VendorMaster = new PL_VendorMaster();
            foreach (var items in lv.Items)
            {
                ((PL_VendorMaster)(items)).IsValid = false;
                DisplayData(_PL_VendorMaster);
            }
            chkSelectAll.IsChecked = false;
            txtVendorId.Text = string.Empty;
            txtVendorName.Text = string.Empty;
            txtVendorAddress.Text = string.Empty;
            txtVendorEmail.Text = string.Empty;
            txtSearch.Text = string.Empty;
            txtPassword.Password = string.Empty;
            txtSearch.IsEnabled = true;
            txtVendorAddress.IsEnabled = true;
            txtVendorEmail.IsEnabled = true;
            txtVendorId.IsEnabled = true;
            txtVendorName.IsEnabled = true;
            txtPassword.IsEnabled = true;
        }

        void ResetForm()
        {
            txtVendorId.Text = string.Empty;
            txtVendorName.Text = string.Empty;
            txtVendorAddress.Text = string.Empty;
            txtVendorEmail.Text = string.Empty;
            txtPassword.Password = string.Empty;
            btnReset.IsEnabled = true;
            btnSave.IsEnabled = true;
            btnDelete.IsEnabled = true;
            btnEdit.IsEnabled = false;
            EnableDisableGB(true);
        }

        private void EnableDisable(bool value)
        {
            btnSave.IsEnabled = value;
            txtVendorId.IsEnabled = value;
            txtVendorName.IsEnabled = value;
            txtVendorAddress.IsEnabled = value;
            txtVendorEmail.IsEnabled = value;
            txtPassword.IsEnabled = value;
        }

        private void EnableDisableGB(bool value)
        {
            gbEntry.IsEnabled = value;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
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
                PL_VendorMaster oPL_ASM_Master = (PL_VendorMaster)item.Content;
                this.txtVendorId.Text = oPL_ASM_Master.VendorId;
                this.txtVendorName.Text = oPL_ASM_Master.VendorDesc;
                this.txtVendorAddress.Text = oPL_ASM_Master.VendorAdd;
                this.txtVendorEmail.Text = oPL_ASM_Master.VendorEmail;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "VendorMaster => " + exDetail.ToString());
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
                foreach (var item in _PL_VendorMaster)
                {
                    item.IsValid = (bool)bChecked;
                }
                lv.ItemsSource = null;
                lv.ItemsSource = _PL_VendorMaster;
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
    }
}
