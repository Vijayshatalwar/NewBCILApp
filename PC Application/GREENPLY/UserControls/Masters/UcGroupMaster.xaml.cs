using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Reflection;
using COMMON;
using System.ComponentModel;
using System.Collections.ObjectModel;
using ENTITY_LAYER;
using COMMON_LAYER;
using BUSSINESS_LAYER;
using GREENPLY.Classes;
using BCILLogger;

namespace GREENPLY.UserControls.Masters
{
    /// <summary>
    /// Interaction logic for UcGroupMaster.xaml
    /// </summary>
    public partial class UcGroupMaster : UserControl
    {
        Logger objLog = new Logger();
        WriteLogFile ObjLog = new WriteLogFile();

        public UcGroupMaster()
        {
            InitializeComponent();
        }
        ObservableCollection<PL_Group_Master> _PL_Group_Master = new ObservableCollection<PL_Group_Master>();

        public ObservableCollection<PL_Group_Master> PL_Group_MasterData
        {
            get { return _PL_Group_Master; }
            set
            {
                _PL_Group_Master = value;
                OnPropertyChanged("PL_Group_MasterData");
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
                PL_Group_Master _PL_Group_Master = new PL_Group_Master();
                this.DisplayData(_PL_Group_Master);
                btnEdit.IsEnabled = false;
                txtGroupName.Focus();
                FilteredData();
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog( " (Error) - " + "GroupMaster => " + exDetail.ToString());
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
                return ((item as PL_Group_Master).GroupName.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        void checkPermissions()
        {
          
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

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult MessResult = MessageBox.Show("Clear Confirmation?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (MessResult == MessageBoxResult.No)
            {
                return;
            }
            btnEdit.Content = "Edit";
            Clear();
            ResetForm();
        }

        private bool ValidateInputs(bool p)
        {
            try
            {
                if (String.IsNullOrEmpty(this.txtGroupName.Text.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter group code first", 1);
                    this.txtGroupName.Focus();
                    return p = false;
                }
                else if (String.IsNullOrEmpty(this.txtDescription.Text.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter description", 1);
                    this.txtDescription.Focus();
                    return p = false;
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog( "(Error) - " + "GroupMaster => " + exDetail.ToString());
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
                    BCommon.setMessageBox(VariableInfo.mApp, "No data found to delete", 1);
                    return;
                }
                MessageBoxResult MessResult = MessageBox.Show("Delete Confirmation", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (MessResult == MessageBoxResult.No)
                {
                    return;
                }

                foreach (PL_Group_Master item in lv.ItemsSource)
                {
                    PL_Group_Master _objEntity_RM = item;
                    {
                        if (item.IsValid == true)
                        {
                            if (item.GroupName.ToString() == "ADMIN" || item.GroupName == "admin")
                            {
                                BCommon.setMessageBox(VariableInfo.mApp, "You can not delete ADMIN group", 1);
                                return;
                            }
                            oResponse = new BL_Group_Master().Delete(_objEntity_RM);
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
                    PL_Group_Master _objEntity_RM = new PL_Group_Master();
                    DisplayData(_objEntity_RM);
                }
                else if (oResponse == OperationResult.DeleteError)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Error On Delete, Kindly Try Again", 3);
                }
                else if (oResponse == OperationResult.DeleteRefference)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Record Used in Transaction, Can't Be Deleted", 3);
                }
                else if (oResponse == OperationResult.Invalid)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "You Can Not Delete This Group Because Its Used In User Creation", 1);
                }

            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog( "(Error) - " + "GroupMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
            finally
            {
                chkSelectAll.IsChecked = false;
                FilteredData();
            }
        }


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnSave.Cursor = Cursors.Wait;
                string sResult = string.Empty;

                OperationResult oResponse = OperationResult.SaveError;
                bool sInputs = this.ValidateInputs(true);
                if (sInputs != false)
                {
                    PL_Group_Master _objEntity_RM = new PL_Group_Master();
                    {
                        _objEntity_RM.GroupName = this.txtGroupName.Text.Trim();
                        _objEntity_RM.GroupDesc = this.txtDescription.Text;
                        _objEntity_RM.CreatedBy = Convert.ToString(VariableInfo.dtVlidateuser) != "" ? Convert.ToString(VariableInfo.dtVlidateuser.Rows[0]["USER_ID"]) : "";
                    };
                    MessageBoxResult MessResult = MessageBox.Show("Do You Want To Save All Details?", "Save Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                    if (MessResult == MessageBoxResult.No)
                    { return; }
                    oResponse = new BL_Group_Master().Save(_objEntity_RM);
                    if (oResponse == OperationResult.SaveSuccess)
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Record Saved Successfully", 4);
                        ResetForm();
                        Clear();
                    }
                    else if (oResponse == OperationResult.Duplicate)
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, _objEntity_RM.GroupName + " Already Exists, Kindly Change the Details", 3);
                        Clear();
                    }
                    else
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Error in Saving, Kindly Try Again", 3);
                        Clear();
                    }
                    DisplayData(_objEntity_RM);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog( "(Error) - " + "GroupMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
                btnSave.Cursor = Cursors.Arrow;
            }
            finally
            {
                btnSave.Cursor = Cursors.Arrow;
                FilteredData();
            }
        }

        void Clear()
        {
            PL_Group_Master _PL_Group_Master = new PL_Group_Master();
            foreach (var items in lv.Items)
            {
                ((PL_Group_Master)(items)).IsValid = false;
                DisplayData(_PL_Group_Master);
            }
            chkSelectAll.IsChecked = false;
            txtGroupName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            txtGroupName.Focus();
            txtDescription.IsEnabled = true;
            txtGroupName.IsEnabled = true;
        }
        
        void ResetForm()
        {
            txtGroupName.Text = string.Empty;
            txtDescription.Text = string.Empty;
            btnClear.IsEnabled = true;
            btnSave.IsEnabled = true;
            txtGroupName.Focus();
            btnEdit.IsEnabled = false;
            btnDelete.IsEnabled = true;
            txtGroupName.IsEnabled = true;
            txtDescription.IsEnabled = true;
            txtGroupName.Focus();
        }
       
        private void DisplayData(PL_Group_Master _PL_Group_Master)
        {
            try
            {
                PL_Group_MasterData = new BL_Group_Master().BI_GetUploadData(_PL_Group_Master);
                lv.ItemsSource = PL_Group_MasterData;
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
                btnDelete.IsEnabled = false;
                btnEdit.IsEnabled = true;
                ListViewItem item = sender as ListViewItem;
                PL_Group_Master oPL_Group_Master = (PL_Group_Master)item.Content;
                txtGroupName.Text = oPL_Group_Master.GroupName;
                txtDescription.Text = oPL_Group_Master.GroupDesc;
                lblGroupId.Content = oPL_Group_Master.GroupID;
                txtGroupName.Focus();
            }
            catch (Exception ex)
            {
                 
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void EnableDisable(bool value)
        {
            btnSave.IsEnabled = value;
            txtGroupName.IsEnabled = value;
            txtDescription.IsEnabled = value;
            btnDelete.IsEnabled = value;
            txtGroupName.Focus();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToString(btnEdit.Content) == "Edit")
                {
                    btnEdit.Content = "Update";
                    txtGroupName.IsEnabled = true;
                    txtDescription.IsEnabled = true;

                }
                else if (Convert.ToString(btnEdit.Content) == "Update")
                {
                    OperationResult oResponse = OperationResult.UpdateError;
                    bool sInputs = this.ValidateInputs(true);
                    if (sInputs != false)
                    {
                        PL_Group_Master _objEntity_RM = new PL_Group_Master();
                        {
                            _objEntity_RM.GroupID = this.lblGroupId.Content.ToString();
                            _objEntity_RM.GroupName = this.txtGroupName.Text.Trim();
                            _objEntity_RM.GroupDesc = this.txtDescription.Text.Trim();
                            _objEntity_RM.CreatedBy = Convert.ToString(VariableInfo.dtVlidateuser) != "" ? Convert.ToString(VariableInfo.dtVlidateuser.Rows[0]["USER_ID"]) : "";
                        };
                        MessageBoxResult MessResult = MessageBox.Show("Do You Want To Update All Details?", "Update Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                        if (MessResult == MessageBoxResult.No)
                        { return; }
                        oResponse = new BL_Group_Master().Update(_objEntity_RM);
                        if (oResponse == OperationResult.UpdateSuccess)
                        {
                            BCommon.setMessageBox(VariableInfo.mApp, "Record Updated Successfully", 4);
                            ResetForm();
                            Clear();
                            DisplayData(_objEntity_RM);
                            btnEdit.Content = "Edit";
                            chkSelectAll.IsChecked = false;
                        }
                        else if (oResponse == OperationResult.UpdateError)
                        {
                            BCommon.setMessageBox(VariableInfo.mApp, "Error On Update, Kindly Try Again", 3);
                            Clear();
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog( "(Error) - " + "GroupMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
            finally
            {
                FilteredData();
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
                foreach (var item in _PL_Group_Master)
                {
                    item.IsValid = (bool)bChecked;
                }
                lv.ItemsSource = null;
                lv.ItemsSource = _PL_Group_Master;
                lv.UpdateLayout();
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog( "(Error) - " + "GroupMaster => " + exDetail.ToString());
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
            }
        }

    }
}
