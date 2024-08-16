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
using BcilLib;
using BCILLogger;

namespace GREENPLY.UserControls.Masters
{
    /// <summary>
    /// Interaction logic for UcDepotMaster.xaml
    /// </summary>
    public partial class UcDepotMaster : UserControl
    {
        Logger objLog = new Logger();
        WriteLogFile ObjLog = new WriteLogFile();
        public UcDepotMaster()
        {
            InitializeComponent();
        }

        ObservableCollection<PL_DepotMaster> _PL_DepotMaster = new ObservableCollection<PL_DepotMaster>();
        public ObservableCollection<PL_DepotMaster> PL_DepotMasterData
        {
            get { return _PL_DepotMaster; }
            set
            {
                _PL_DepotMaster = value;
                OnPropertyChanged("PL_DepotMasterData");
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
                PL_DepotMaster _PlDepoMaster = new PL_DepotMaster();
                this.DisplayData(_PlDepoMaster);
                btnEdit.IsEnabled = false;
                FilteredData();
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog( " (Error) - " + "DepotMaster => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void DisplayData(PL_DepotMaster _PL_DepotMaster)
        {
            try
            {
                PL_DepotMasterData = new BL_DepotMaster().BL_GetDepotData(_PL_DepotMaster);
                lv.ItemsSource = PL_DepotMasterData;
                lblCount.Content = lv.Items.Count;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "DepotMaster => " + exDetail.ToString());
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
                return ((item as PL_DepotMaster).DepotDesc.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }

        private bool ValidateInputs(bool p, bool bDelete)
        {
            try
            {
                if (String.IsNullOrEmpty(this.txtDepotId.Text.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter Depot ID", 1);
                    this.txtDepotId.Focus();
                    return p = false;

                }
                else if (String.IsNullOrEmpty(this.txtDepotName.Text.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter Depot Description", 1);
                    this.txtDepotName.Focus();
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
                ObjLog.WriteLog( "(Error) - " + "DepotMaster => " + exDetail.ToString());
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
                    PL_DepotMaster _objEntity_ASM = new PL_DepotMaster();
                    {
                        _objEntity_ASM.LocationCode = VariableInfo.mPlantCode;
                        _objEntity_ASM.DepotId = this.txtDepotId.Text.Trim();
                        _objEntity_ASM.DepotDesc = this.txtDepotName.Text.Trim();
                        _objEntity_ASM.CreatedBy = Convert.ToString(VariableInfo.dtVlidateuser) != "" ? Convert.ToString(VariableInfo.dtVlidateuser.Rows[0]["USER_ID"]) : "";
                    };
                    MessageBoxResult MessResult = MessageBox.Show("Do You Want To Save All Details?", "Save Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                    if (MessResult == MessageBoxResult.No)
                        return; 
                    oResponse = new BL_DepotMaster().BL_SaveDepotData(_objEntity_ASM);
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
                        BCommon.setMessageBox(VariableInfo.mApp, _objEntity_ASM.DepotId + " Already Exists, Kindly Change", 2);
                    }
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog( "(Error) - " + "DepotMaster => " + exDetail.ToString());
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
                    txtDepotId.IsEnabled = false;
                    txtDepotName.IsEnabled = true;
                }
                else if (Convert.ToString(btnEdit.Content) == "Update")
                {
                    OperationResult oResponse = OperationResult.UpdateError;
                    bool bDelete = false;
                    bool sInputs = this.ValidateInputs(true, bDelete);
                    if (sInputs != false)
                    {
                        PL_DepotMaster _objEntity_ASM = new PL_DepotMaster();
                        {
                            _objEntity_ASM.LocationCode = VariableInfo.mPlantCode;
                            _objEntity_ASM.DepotId = this.txtDepotId.Text.Trim();
                            _objEntity_ASM.DepotDesc = this.txtDepotName.Text.Trim();
                            _objEntity_ASM.CreatedBy = Convert.ToString(VariableInfo.dtVlidateuser) != "" ? Convert.ToString(VariableInfo.dtVlidateuser.Rows[0]["USER_ID"]) : "";
                        };
                        oResponse = new BL_DepotMaster().BL_UpdateDepotData(_objEntity_ASM);
                        if (oResponse == OperationResult.UpdateSuccess)
                        {
                            BCommon.setMessageBox(VariableInfo.mApp, "Record Updated Successfully.", 4);
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
                ObjLog.WriteLog( "(Error) - " + "DepotMaster => " + exDetail.ToString());
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
                foreach (PL_DepotMaster item in lv.ItemsSource)
                {
                    PL_DepotMaster _objEntity_RM = item;
                    {
                        if (item.IsValid == true)
                        {
                            oResponse = new BL_DepotMaster().BL_DeleteDepot(_objEntity_RM);
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
                    PL_DepotMaster _objEntity_RM = new PL_DepotMaster();
                    DisplayData(_objEntity_RM);
                }
                else if (oResponse == OperationResult.DeleteError)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Error On Delete, Kindly Try Again", 3);
                }
                else if (oResponse == OperationResult.Error)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Depot Id is Used in Other Process", 3);
                }
                else if (oResponse == OperationResult.DeleteRefference)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Record Used In Transaction, Can't be Delete", 3);
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog( "(Error) - " + "DepotMaster => " + exDetail.ToString());
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
            PL_DepotMaster _PL_DepoMaster = new PL_DepotMaster();
            foreach (var items in lv.Items)
            {
                ((PL_DepotMaster)(items)).IsValid = false;
                DisplayData(_PL_DepoMaster);
            }
            chkSelectAll.IsChecked = false;
            txtDepotId.Text = string.Empty;
            txtDepotName.Text = string.Empty;
            txtSearch.Text = string.Empty;
            txtDepotId.IsEnabled = true;
            txtDepotName.IsEnabled = true;
            txtSearch.IsEnabled = true;
        }

        void ResetForm()
        {
            txtDepotId.Text = string.Empty;
            txtDepotName.Text = string.Empty;
            btnReset.IsEnabled = true;
            btnSave.IsEnabled = true;
            btnDelete.IsEnabled = true;
            btnEdit.IsEnabled = false;
            EnableDisableGB(true);
        }

        private void EnableDisable(bool value)
        {
            btnSave.IsEnabled = value;
            txtDepotId.IsEnabled = value;
            txtDepotName.IsEnabled = value;
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
                PL_DepotMaster oPL_Depot_Master = (PL_DepotMaster)item.Content;
                this.txtDepotId.Text = oPL_Depot_Master.DepotId;
                this.txtDepotName.Text = oPL_Depot_Master.DepotDesc;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog( "(Error) - " + "DepotMaster => " + exDetail.ToString());
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
                foreach (var item in _PL_DepotMaster)
                {
                    item.IsValid = (bool)bChecked;
                }
                lv.ItemsSource = null;
                lv.ItemsSource = _PL_DepotMaster;
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
