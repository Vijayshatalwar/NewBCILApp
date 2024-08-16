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
    /// Interaction logic for UcEmailConfigMaster.xaml
    /// </summary>
    public partial class UcEmailConfigMaster : UserControl
    {
        Logger objLog = new Logger();
        WriteLogFile ObjLog = new WriteLogFile();

        public UcEmailConfigMaster()
        {
            InitializeComponent();
        }

        ObservableCollection<PL_EmailConfigMaster> _PL_EmailConfigMaster = new ObservableCollection<PL_EmailConfigMaster>();
        public ObservableCollection<PL_EmailConfigMaster> PL_EmailConfigMasterData
        {
            get { return _PL_EmailConfigMaster; }
            set
            {
                _PL_EmailConfigMaster = value;
                OnPropertyChanged("PL_EmailConfigMasterData");
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
                PL_EmailConfigMaster _PlEconfig_Master = new PL_EmailConfigMaster();
                this.DisplayData(_PlEconfig_Master);
                btnEdit.IsEnabled = false;
                FilteredData();
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "EmailConfigMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void DisplayData(PL_EmailConfigMaster _PL_ConfigMaster)
        {
            try
            {
                PL_EmailConfigMasterData = new BL_EmailConfigMaster().BL_GetEmailConfigMasterData(_PL_ConfigMaster);
                lv.ItemsSource = PL_EmailConfigMasterData;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog( "(Error) - " + "EmailConfigMaster => " + exDetail.ToString());
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
                return ((item as PL_EmailConfigMaster).Name.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private void ValidateEmail()
        {
            string email = txtEmailID.Text;
            Regex regex = new Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");
            Match match = regex.Match(email);
            if (match.Success)
            {
                //BCommon.setMessageBox(VariableInfo.mApp, email + " is Valid Email Address", 3);
            }
            else
            {
                BCommon.setMessageBox(VariableInfo.mApp, email + " is Invalid Email Address", 3);
                return;
            }
        }

        private bool ValidateInputs(bool p, bool bDelete)
        {
            try
            {
                if (String.IsNullOrEmpty(this.txtHost.Text.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Enter SMTP Host", 1);
                    this.txtHost.Focus();
                    return p = false;
                }
                else if (String.IsNullOrEmpty(this.txtName.Text.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Enter Name", 1);
                    this.txtName.Focus();
                    return p = false;
                }
                if (String.IsNullOrEmpty(this.txtEmailID.Text.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Enter Email ID", 1);
                    this.txtEmailID.Focus();
                    return p = false;
                }
                
                else if (String.IsNullOrEmpty(this.txtPort.Text.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Enter Port", 1);
                    this.txtPort.Focus();
                    return p = false;

                }
                else if (String.IsNullOrEmpty(this.txtPwd.Text.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Enter Password", 1);
                    this.txtPwd.Focus();
                    return p = false;
                }
                else if (String.IsNullOrEmpty(this.txtSubject.Text.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Kindly Enter Email Subject", 1);
                    this.txtSubject.Focus();
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
                ObjLog.WriteLog( "(Error) - " + "EmailConfigMaster => " + exDetail.ToString());
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
                    PL_EmailConfigMaster _objEntity_ASM = new PL_EmailConfigMaster();
                    {
                        _objEntity_ASM.EmailId = this.txtEmailID.Text.Trim();
                        _objEntity_ASM.SmtpHost = this.txtHost.Text.Trim();
                        _objEntity_ASM.Name = this.txtName.Text.Trim();
                        _objEntity_ASM.PortNo = this.txtPort.Text.Trim();
                        _objEntity_ASM.Password = this.txtPwd.Text.Trim();
                        _objEntity_ASM.Subject = this.txtSubject.Text.Trim();
                        _objEntity_ASM.CreatedBy = Convert.ToString(VariableInfo.dtVlidateuser) != "" ? Convert.ToString(VariableInfo.dtVlidateuser.Rows[0]["USER_ID"]) : "";
                    };
                    MessageBoxResult MessResult = MessageBox.Show("Do You Want To Save All Details?", "Save Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                    if (MessResult == MessageBoxResult.No)
                    { return; }
                    oResponse = new BL_EmailConfigMaster().BL_SaveEmailConfigMasterData(_objEntity_ASM);
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
                        BCommon.setMessageBox(VariableInfo.mApp, "Error in Saving The Record, Kindly Try Again", 3);
                    }
                    else if (oResponse == OperationResult.Duplicate)
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, _objEntity_ASM.EmailId + " is Already Exists, Kindly Change", 2);
                    }
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog( "(Error) - " + "EmailConfigMaster => " + exDetail.ToString());
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
                    txtEmailID.IsEnabled = false;
                    txtHost.IsEnabled = true;
                    txtName.IsEnabled = true;
                    txtPort.IsEnabled = true;
                    txtPwd.IsEnabled = true;
                    txtSubject.IsEnabled = true;
                }
                else if (Convert.ToString(btnEdit.Content) == "Update")
                {
                    OperationResult oResponse = OperationResult.UpdateError;
                    bool bDelete = false;
                    bool sInputs = this.ValidateInputs(true, bDelete);
                    if (sInputs != false)
                    {
                        PL_EmailConfigMaster _objEntity_ASM = new PL_EmailConfigMaster();
                        {
                            _objEntity_ASM.EmailId = this.txtEmailID.Text.Trim();
                            _objEntity_ASM.SmtpHost = this.txtHost.Text.Trim();
                            _objEntity_ASM.Name = this.txtName.Text.Trim();
                            _objEntity_ASM.PortNo = this.txtPort.Text.Trim();
                            _objEntity_ASM.Password = this.txtPwd.Text.Trim();
                            _objEntity_ASM.Subject = this.txtSubject.Text.Trim();
                            _objEntity_ASM.CreatedBy = Convert.ToString(VariableInfo.dtVlidateuser) != "" ? Convert.ToString(VariableInfo.dtVlidateuser.Rows[0]["USER_ID"]) : "";
                        };
                        oResponse = new BL_EmailConfigMaster().BL_UpdateEmailConfigMasterData(_objEntity_ASM);
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
                            BCommon.setMessageBox(VariableInfo.mApp, "Error On Update, Kindly Try Again", 3);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog( "(Error) - " + "EmailConfigMaster => " + exDetail.ToString());
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
                foreach (PL_EmailConfigMaster item in lv.ItemsSource)
                {
                    PL_EmailConfigMaster _objEntity_RM = item;
                    {
                        if (item.IsValid == true)
                        {
                            oResponse = new BL_EmailConfigMaster().BL_DeleteEmailConfigMasterData(_objEntity_RM);
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
                    PL_EmailConfigMaster _objEntity_RM = new PL_EmailConfigMaster();
                    DisplayData(_objEntity_RM);
                }
                else if (oResponse == OperationResult.DeleteError)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Error On Delete, Kindly Try Again", 3);
                }
                else if (oResponse == OperationResult.Error)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Email Id is used in other table", 3);
                }
                else if (oResponse == OperationResult.DeleteRefference)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Record Used In Transaction, Can't be Delete", 3);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog( "(Error) - " + "EmailConfigMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
            finally
            {
                chkSelectAll.IsChecked = false;
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

        void Clear()
        {
            PL_EmailConfigMaster _PL_EMaster = new PL_EmailConfigMaster();
            foreach (var items in lv.Items)
            {
                ((PL_EmailConfigMaster)(items)).IsValid = false;
                DisplayData(_PL_EMaster);
            }
            chkSelectAll.IsChecked = false;
            txtSubject.Text = string.Empty;
            txtPwd.Text = string.Empty;
            txtPort.Text = string.Empty;
            txtName.Text = string.Empty;
            txtHost.Text = string.Empty;
            txtEmailID.Text = string.Empty;
            txtEmailID.IsEnabled = true;
            txtHost.IsEnabled = true;
            txtName.IsEnabled = true;
            txtPort.IsEnabled = true;
            txtPwd.IsEnabled = true;
            txtSearch.IsEnabled = true;
            txtSubject.IsEnabled = true;
        }

        void ResetForm()
        {
            txtSubject.Text = string.Empty;
            txtPwd.Text = string.Empty;
            txtPort.Text = string.Empty;
            txtName.Text = string.Empty;
            txtHost.Text = string.Empty;
            txtEmailID.Text = string.Empty;
            btnReset.IsEnabled = true;
            btnSave.IsEnabled = true;
            btnDelete.IsEnabled = true;
            btnEdit.IsEnabled = false;
            EnableDisableGB(true);
        }

        private void EnableDisable(bool value)
        {
            btnSave.IsEnabled = value;
            txtSubject.IsEnabled = value;
            txtPwd.IsEnabled = value;
            txtPort.IsEnabled = value;
            txtName.IsEnabled = value;
            txtHost.IsEnabled = value;
            txtEmailID.IsEnabled = value;
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
                PL_EmailConfigMaster oPL_EConfig_Master = (PL_EmailConfigMaster)item.Content;
                this.txtEmailID.Text = oPL_EConfig_Master.EmailId;
                this.txtHost.Text = oPL_EConfig_Master.SmtpHost;
                this.txtName.Text = oPL_EConfig_Master.Name;
                this.txtPort.Text = oPL_EConfig_Master.PortNo;
                this.txtPwd.Text = oPL_EConfig_Master.Password;
                this.txtSubject.Text = oPL_EConfig_Master.Subject;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog( "(Error) - " + "EmailConfigMaster => " + exDetail.ToString());
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
                foreach (var item in _PL_EmailConfigMaster)
                {
                    item.IsValid = (bool)bChecked;
                }
                lv.ItemsSource = null;
                lv.ItemsSource = _PL_EmailConfigMaster;
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

        private void txtPort_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(txtPort.Text, "[^0-9]"))
            {
                BCommon.setMessageBox(VariableInfo.mApp, "Please Enter Only Numbers", 2);
                txtPort.Text = txtPort.Text.Remove(txtPort.Text.Length - 1);
                return;
            }
        }

        private void txtEmailID_LostFocus(object sender, RoutedEventArgs e)
        {
            ValidateEmail();
        }
    }
}
