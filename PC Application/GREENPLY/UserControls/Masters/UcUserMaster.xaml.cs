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
    /// Interaction logic for UcUserMaster.xaml
    /// </summary>
    /// 
    public partial class UcUserMaster : UserControl
    {
        Logger objLog = new Logger();
        WriteLogFile ObjLog = new WriteLogFile();
        public UcUserMaster()
        {
            InitializeComponent();
        }

        string _ModuleType = string.Empty;

        ObservableCollection<PL_UserMaster> _PL_UserMaster = new ObservableCollection<PL_UserMaster>();

        public ObservableCollection<PL_UserMaster> PL_UserMasterData
        {
            get { return _PL_UserMaster; }
            set
            {
                _PL_UserMaster = value;
                OnPropertyChanged("PL_UserMasterData");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyname)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyname));
        }

        private void DisplayData(PL_UserMaster _PL_ASM_Master)
        {
            try
            {
                PL_UserMasterData = new BL_UserMaster().BI_GetUploadData(_PL_ASM_Master);
                lv.ItemsSource = PL_UserMasterData;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "UserMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                // checkPermissions();
                PL_UserMaster _PlASM_Master = new PL_UserMaster();
                this.DisplayData(_PlASM_Master);
                GetGroupName();
                GetLocationCode();
                btnEdit.IsEnabled = false;
                cmbgroup.Focus();
                FilteredData();
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "UserMaster => " + exDetail.ToString());
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
                return ((item as PL_UserMaster).USER_NAME.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
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
                    PL_UserMaster _objEntity_ASM = new PL_UserMaster();
                    {
                        _objEntity_ASM.LOCATION_TYPE = cmbLocType.Text.ToString();
                        _objEntity_ASM.LOCATION_CODE = cmbLocCode.Text.ToString();
                        _objEntity_ASM.USER_ID = this.txtUserId.Text.Trim();
                        _objEntity_ASM.USER_NAME = this.txtUserName.Text.Trim();
                        _objEntity_ASM.Password = VariableInfo.EncryptPassword(this.txtPassword.Password.Trim(), "E");
                        _objEntity_ASM.USER_EMAIL = this.txtEmailID.Text.Trim();
                        _objEntity_ASM.GroupName = this.cmbgroup.Text.ToString();
                        //_objEntity_ASM.USER_TYPE = this.cmbGroupType.Text.Trim();
                        _objEntity_ASM.CREATED_BY = Convert.ToString(VariableInfo.dtVlidateuser) != "" ? Convert.ToString(VariableInfo.dtVlidateuser.Rows[0]["USER_ID"]) : "";
                    };
                    MessageBoxResult MessResult = MessageBox.Show("Do You  Want To Save All Details?", "Save Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                    if (MessResult == MessageBoxResult.No)
                    { return; }
                    oResponse = new BL_UserMaster().Save(_objEntity_ASM);
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
                        BCommon.setMessageBox(VariableInfo.mApp, "Error in Saving The Record, Kindly Try Again", 3);
                    }
                    else if (oResponse == OperationResult.Duplicate)
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Duplicate Record, Kindly Change Details", 2);
                    }
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "UserMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
                btnSave.Cursor = Cursors.Arrow;
            }
            finally
            {
                btnSave.Cursor = Cursors.Arrow;
                FilteredData();
            }
        }

        private bool ValidateInputs(bool p, bool bDelete)
        {
            try
            {
                if (cmbgroup.SelectedIndex <= -1)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Select Group", 1);
                    this.cmbgroup.Focus();
                    return p = false;
                }
                if (cmbLocType.SelectedIndex <= -1)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Select Location Type", 1);
                    this.cmbgroup.Focus();
                    return p = false;
                }
                if (String.IsNullOrEmpty(this.txtUserId.Text.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter User Id", 1);
                    this.txtUserId.Focus();
                    return p = false;

                }
                if (String.IsNullOrEmpty(this.txtPassword.Password.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter Password", 1);
                    this.txtPassword.Focus();
                    return p = false;
                }
                if (String.IsNullOrEmpty(this.txtConfirmPassword.Password.Trim()))
                {
                        BCommon.setMessageBox(VariableInfo.mApp, "Enter Confirm Password", 1);
                        this.txtConfirmPassword.Focus();
                        return p = false;
                }
                if (String.IsNullOrEmpty(this.txtUserName.Text.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter User Name", 1);
                    this.txtUserName.Focus();
                    return p = false;
                }
                if (cmbLocCode.SelectedIndex <= -1)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Select Location Code", 1);
                    this.cmbgroup.Focus();
                    return p = false;
                }
                if (String.IsNullOrEmpty(this.txtEmailID.Text.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter Email Id", 1);
                    this.txtEmailID.Focus();
                    return p = false;
                }
                if (!Regex.IsMatch(txtEmailID.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter valid Email Id", 1);
                    this.txtEmailID.Focus();
                    return p = false;
                }
                if (bDelete != true)
                {
                    if (String.IsNullOrEmpty(this.txtPassword.Password.Trim()))
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Enter Password", 1);
                        this.txtPassword.Focus();
                        return p = false;
                    }
                    else if (String.IsNullOrEmpty(this.txtConfirmPassword.Password.Trim()))
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Enter Confirm Password", 1);
                        this.txtConfirmPassword.Focus();
                        return p = false;
                    }
                    else if (txtConfirmPassword.Password != txtPassword.Password)
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Password and Confirm Password are Mismatch, Kindly Change", 1);
                        this.txtConfirmPassword.Focus();
                        this.txtPassword.Password = "";
                        this.txtConfirmPassword.Password = "";
                        txtPassword.Focus();
                        return p = false;
                    }
                    bDelete = false;
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "UserMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
            return p;
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
                PL_UserMaster oPL_ASM_Master = (PL_UserMaster)item.Content;

                this.cmbgroup.Text = oPL_ASM_Master.GroupName;
                this.cmbLocType.Text = oPL_ASM_Master.LOCATION_TYPE;
                this.cmbLocCode.Text = oPL_ASM_Master.LOCATION_CODE;
                this.txtEmailID.Text = oPL_ASM_Master.USER_EMAIL;
                this.txtUserId.Text = oPL_ASM_Master.USER_ID;
                this.txtUserName.Text = oPL_ASM_Master.USER_NAME;
                //this.cmbCompanyCode.SelectedValue = oPL_ASM_Master.PlantCode;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "UserMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void EnableDisable(bool value)
        {
            btnSave.IsEnabled = value;
            txtUserId.IsEnabled = value;
            txtUserName.IsEnabled = value;
            txtPassword.IsEnabled = value;
            txtConfirmPassword.IsEnabled = value;
            txtEmailID.IsEnabled = value;
            cmbgroup.IsEnabled = value;
            //cmbGroupType.IsEnabled = value;
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Convert.ToString(btnEdit.Content) == "Edit")
                {
                    btnEdit.Content = "Update";
                    txtUserId.IsEnabled = false;
                    txtUserName.IsEnabled = true;
                    txtPassword.IsEnabled = true;
                    txtConfirmPassword.IsEnabled = true;
                    txtEmailID.IsEnabled = true;
                    cmbgroup.IsEnabled = true;
                    cmbLocType.IsEnabled = true;
                    cmbLocCode.IsEnabled = true;
                    txtPassword.Focus();
                }
                else if (Convert.ToString(btnEdit.Content) == "Update")
                {
                    OperationResult oResponse = OperationResult.UpdateError;
                    bool bDelete = false;
                    bool sInputs = this.ValidateInputs(true, bDelete);
                    if (sInputs != false)
                    {
                        PL_UserMaster _objEntity_ASM = new PL_UserMaster();
                        {
                            _objEntity_ASM.LOCATION_CODE = cmbLocCode.Text.ToString();
                            _objEntity_ASM.LOCATION_TYPE = cmbLocType.Text.ToString();
                            _objEntity_ASM.USER_NAME = this.txtUserName.Text.Trim();
                            _objEntity_ASM.Password = VariableInfo.EncryptPassword(this.txtPassword.Password.Trim(), "E");
                            _objEntity_ASM.USER_EMAIL = this.txtEmailID.Text.Trim();
                            _objEntity_ASM.GroupName = this.cmbgroup.Text.ToString();
                            //_objEntity_ASM.USER_TYPE = this.cmbGroupType.SelectedValue.ToString();
                            _objEntity_ASM.USER_ID = this.txtUserId.Text.Trim();
                            _objEntity_ASM.CREATED_BY = Convert.ToString(VariableInfo.dtVlidateuser) != "" ? Convert.ToString(VariableInfo.dtVlidateuser.Rows[0]["USER_ID"]) : "";
                        };
                        oResponse = new BL_UserMaster().Update(_objEntity_ASM);
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
                            BCommon.setMessageBox(VariableInfo.mApp, "Error In Updating The Record, Kindly Try Again", 3);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "UserMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
            finally
            {
                FilteredData();
            }
        }

        void Clear()
        {
            PL_UserMaster _PL_UserMaster = new PL_UserMaster();
            foreach (var items in lv.Items)
            {
                ((PL_UserMaster)(items)).IsValid = false;
                DisplayData(_PL_UserMaster);
            }
            chkSelectAll.IsChecked = false;
            cmbgroup.Text = string.Empty;
            cmbLocType.Text = string.Empty;
            cmbLocCode.Text = string.Empty;
            txtSearch.Text = string.Empty;
            //cmbGroupType.Text = string.Empty;
            cmbCompanyCode.Text = string.Empty;
            txtUserId.Text = string.Empty;
            txtUserName.Text = string.Empty;
            txtPassword.Password = string.Empty;
            txtConfirmPassword.Password = string.Empty;
            txtEmailID.Text = string.Empty;
            cmbgroup.Focus();
        }

        void ResetForm()
        {
            txtUserId.Text = string.Empty;
            txtUserName.Text = string.Empty;
            txtPassword.Password = string.Empty;
            txtConfirmPassword.Password = string.Empty;
            txtEmailID.Text = string.Empty;
            cmbgroup.Text = string.Empty;
            // cmbGroupType.Text = string.Empty;
            cmbCompanyCode.Text = string.Empty;
            btnReset.IsEnabled = true;
            btnSave.IsEnabled = true;
            btnDelete.IsEnabled = true;
            btnEdit.IsEnabled = false;
            txtUserId.IsEnabled = true;
            txtUserName.IsEnabled = true;
            txtPassword.IsEnabled = true;
            txtConfirmPassword.IsEnabled = true;
            cmbgroup.IsEnabled = true;
            // cmbGroupType.IsEnabled = true;
            txtEmailID.IsEnabled = true;
            EnableDisableGB(true);
            cmbgroup.Focus();
        }

        private void GetGroupName()
        {
            try
            {
                BL_UserMaster objASM = new BL_UserMaster();
                PL_UserMaster objPLASM = new PL_UserMaster();
                { }
                DataSet dsDDData = objASM.GetDropDownData(objPLASM, "GROUPNAME");
                VariableInfo.BindDropDown(cmbgroup, dsDDData.Tables[0], "GroupName", "GroupID", "GroupName");
            }
            catch (Exception ex)
            {

                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetLocationCode()
        {
            try
            {
                BL_UserMaster objASM = new BL_UserMaster();
                DataSet dsDDData = objASM.GetLocationCodeData();
                VariableInfo.BindDropDown(cmbLocCode, dsDDData.Tables[0], "PlantCode", "PlantCode", "PlantCode");
            }
            catch (Exception ex)
            {

                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void EnableDisableGB(bool value)
        {
            gbEntry.IsEnabled = value;
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult MessResult = MessageBox.Show("Do You  Want To Clear All Details?", "Clear Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (MessResult == MessageBoxResult.No)
            {
                return;
            }
            btnEdit.Content = "Edit";
            Clear();
            ResetForm();
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

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int iCount = 0;
                OperationResult oResponse = OperationResult.DeleteError;
                if (lv.Items.Count <= 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "No Data Found For Delete.", 1);
                    return;
                }
                MessageBoxResult MessResult = MessageBox.Show("Delete Confirmation", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (MessResult == MessageBoxResult.No)
                {
                    return;
                }
                foreach (PL_UserMaster item in lv.ItemsSource)
                {
                    PL_UserMaster _objEntity_RM = item;
                    {
                        string ValidateUser = Convert.ToString(VariableInfo.dtVlidateuser.Rows[0]["USER_ID"]);
                        if (item.IsValid == true)
                        {
                            if (item.USER_ID.ToString() == "ADMIN" && item.GroupName.ToString() == "ADMIN" && item.USER_TYPE.ToString() == "ADMIN")
                            {
                                BCommon.setMessageBox(VariableInfo.mApp, "You Can Not Delete ADMIN User.", 1);
                                return;
                            }
                            if (item.USER_ID.ToString() == ValidateUser.Trim())
                            {
                                BCommon.setMessageBox(VariableInfo.mApp, "You Can Not Delete Login User.", 1);
                                return;
                            }
                            oResponse = new BL_UserMaster().Delete(_objEntity_RM);
                        }
                        else
                        {
                            iCount++;
                            continue;
                        }
                    }
                }
                if (lv.Items.Count == iCount)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Select At Least One Record To Delete.", 1);
                    return;
                }
                else if (oResponse == OperationResult.DeleteSuccess)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Selected Records Deleted Successfully.", 4);
                    ResetForm();
                    Clear();
                    PL_UserMaster _objEntity_RM = new PL_UserMaster();
                    DisplayData(_objEntity_RM);
                }
                else if (oResponse == OperationResult.DeleteError)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Error On Delete", 3);
                }
                else if (oResponse == OperationResult.Error)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Group Id is used in other table", 3);
                }
                else if (oResponse == OperationResult.DeleteRefference)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Record Used In Transaction,Can't be Delete", 3);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "UserMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
            finally
            {
                chkSelectAll.IsChecked = false;
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
