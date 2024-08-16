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
using GREENPLY.Classes;
using BUSSINESS_LAYER;
using System.Configuration;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using BCILLogger;

namespace GREENPLY.UserControls.Masters
{
    /// <summary>
    /// Interaction logic for UCStorageLocMaster.xaml
    /// </summary>
    public partial class UCMaterialMaster : UserControl
    {
        Logger ObjLog = new Logger();

        //GREENPLY.Classes.WriteLogFile ObjLog = new WriteLogFile();

        public UCMaterialMaster()
        {
            InitializeComponent();
            ObjLog = new Logger();
        }

        #region Private Collection

        ObservableCollection<PL_MaterialMaster> _PLMaterialMaster = new ObservableCollection<PL_MaterialMaster>();
        public ObservableCollection<PL_MaterialMaster> MaterialMaster
        {
            get { return _PLMaterialMaster; }
            set
            {
                _PLMaterialMaster = value;
                OnPropertyChanged("MaterialMaster");
            }
        }
        private void OnPropertyChanged(string propertyname)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyname));
        }
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                lblSelectAll.Visibility = System.Windows.Visibility.Hidden;
                chkSelectAll.Visibility = System.Windows.Visibility.Hidden;
                btnCancel.Visibility = System.Windows.Visibility.Hidden;
                PL_MaterialMaster __PLMaterialMaster = new PL_MaterialMaster();
                this.DisplayData(__PLMaterialMaster);
                //btnDelete.Visibility = System.Windows.Visibility.Hidden;
                FilteredData();
                txtSearch.Focus();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void lv_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                //EnableDisable(false);
                //btnSave.IsEnabled = false;
                //btnDelete.IsEnabled = false;
                //btnEdit.IsEnabled = true;
                //btnEdit.Content = "Edit";
                //ListViewItem item = sender as ListViewItem;
                //PL_MaterialMaster objPLMaterialMaster = (PL_MaterialMaster)item.Content;
                //this.txtcode.Text = objPLMaterialMaster.MaterialCode;
                //this.txtdesc.Text = objPLMaterialMaster.MaterialDescription;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void chkSelectAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool? bChecked = chkSelectAll.IsChecked;
                foreach (var item in _PLMaterialMaster)
                {
                    item.IsValid = (bool)bChecked;
                }
                lv.ItemsSource = null;
                lv.ItemsSource = _PLMaterialMaster;
                lv.UpdateLayout();
            }
            catch (Exception ex)
            {
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        #region Button Event

        //private void btnSave_Click(object sender, RoutedEventArgs e)
        //{
        //    PL_MaterialMaster _PLMaterialMaster = new PL_MaterialMaster();
        //    try
        //    {
        //        btnSave.Cursor = Cursors.Wait;
        //        string sResult = string.Empty;
        //        OperationResult oResponse = OperationResult.SaveError;

        //        if (lvBrowse.Items.Count > 0)
        //        {
        //            MessageBoxResult MessResult = MessageBox.Show("Do you want to save all details?", "Save Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
        //            if (MessResult == MessageBoxResult.No)
        //            { return; }

        //            int SaveCount = 0;
        //            foreach (DataRowView lvItems in lvBrowse.Items)
        //            {
        //                PL_MaterialMaster _objPLMaterialMaster = new PL_MaterialMaster();
        //                {
        //                    _objPLMaterialMaster.Product = Convert.ToString(lvItems["Product"]);
        //                    _objPLMaterialMaster.Thickness = Convert.ToString(lvItems["Thickness"]);
        //                    _objPLMaterialMaster.Size = Convert.ToString(lvItems["Size"]);
        //                    _objPLMaterialMaster.MaterialCode = Convert.ToString(lvItems["MaterialCode"]);
        //                    _objPLMaterialMaster.MaterialDescription = Convert.ToString(lvItems["MaterialDescription"]);
        //                    _objPLMaterialMaster.CreatedBy = Convert.ToString(VariableInfo.dtVlidateuser) != "" ? Convert.ToString(VariableInfo.dtVlidateuser.Rows[0]["USER_ID"]) : "";
        //                };
        //                oResponse = new BUSSINESS_LAYER.BL_MaterialMaster().Save(_objPLMaterialMaster);
        //                if (oResponse == OperationResult.SaveSuccess)
        //                {
        //                    SaveCount++;
        //                }
        //            }
        //            if (SaveCount > 0)
        //            {
        //                BCommon.setMessageBox(VariableInfo.mApp, SaveCount + " No. of Records save successfully. ", 4);
        //                Clear();
        //                this.DisplayData(_PLMaterialMaster);
        //            }
        //            else if (oResponse == OperationResult.SaveError)
        //            {
        //                BCommon.setMessageBox(VariableInfo.mApp, "Error on save", 3);
        //            }
        //            else if (oResponse == OperationResult.Duplicate)
        //            {
        //                BCommon.setMessageBox(VariableInfo.mApp, "Find duplicate record. Please select other sheet.", 3);
        //            }
        //        }
        //        else
        //        {

        //            bool sInputs = ValidateInputs(true);
        //            if (sInputs != false)
        //            {
        //                MessageBoxResult MessResult = MessageBox.Show("Do you want to save all details?", "Save Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
        //                if (MessResult == MessageBoxResult.No)
        //                { return; }

        //                PL_MaterialMaster _objPLMaterialMaster = new PL_MaterialMaster();
        //                {
        //                    _objPLMaterialMaster.MaterialCode = this.txtcode.Text.Trim();
        //                    _objPLMaterialMaster.MaterialDescription = this.txtdesc.Text.Trim();                          
        //                    _objPLMaterialMaster.CreatedBy = Convert.ToString(VariableInfo.dtVlidateuser) != "" ? Convert.ToString(VariableInfo.dtVlidateuser.Rows[0]["USER_ID"]) : "";
        //                };
        //                oResponse = new BUSSINESS_LAYER.BL_MaterialMaster().Save(_objPLMaterialMaster);
        //                if (oResponse == OperationResult.SaveSuccess)
        //                {
        //                    BCommon.setMessageBox(VariableInfo.mApp, "Records save successfully.", 4);
        //                    Clear();
        //                    DisplayData(_objPLMaterialMaster);
        //                }
        //                else if (oResponse == OperationResult.SaveError)
        //                {
        //                    BCommon.setMessageBox(VariableInfo.mApp, "Error on save", 3);
        //                }
        //                else if (oResponse == OperationResult.Duplicate)
        //                {
        //                    BCommon.setMessageBox(VariableInfo.mApp, "Duplicate record.", 1);
        //                    txtcode.Text = "";
        //                    txtcode.Focus();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
        //        btnSave.Cursor = Cursors.Arrow;
        //    }
        //    finally
        //    {
        //        btnSave.Cursor = Cursors.Arrow;
        //        FilteredData();
        //        chkSelectAll.IsChecked = false;
        //    }
        //}

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //MessageBoxResult MessResult = MessageBox.Show("Do You Want To Clear All Details?", "Clear Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            //if (MessResult == MessageBoxResult.No)
            //{ return; }
            //if (Convert.ToString(btnEdit.Content) == "Update")
            //{ btnEdit.Content = "Edit"; }
            PL_MaterialMaster __PLMaterialMaster = new PL_MaterialMaster();
            this.DisplayData(__PLMaterialMaster);
            //btnDelete.Visibility = System.Windows.Visibility.Hidden;
            FilteredData();
            txtSearch.Focus();
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
       
        //private void btnEdit_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        btnEdit.Cursor = Cursors.Wait;
        //        if (Convert.ToString(btnEdit.Content) == "Edit")
        //        {
        //            btnEdit.Content = "Update";
        //            txtcode.IsEnabled = false;
        //            txtdesc.IsEnabled = true;                    
        //        }
        //        else if (Convert.ToString(btnEdit.Content) == "Update")
        //        {
        //            OperationResult oResponse = OperationResult.UpdateError;
        //            bool sInputs = this.ValidateInputs(true);
        //            if (sInputs != false)
        //            {
        //                PL_MaterialMaster _objPLMaterialMaster = new PL_MaterialMaster();
        //                {
        //                    _objPLMaterialMaster.MaterialDescription = this.txtdesc.Text.Trim();
        //                    _objPLMaterialMaster.MaterialCode = this.txtcode.Text.Trim();
        //                    _objPLMaterialMaster.CreatedBy = Convert.ToString(VariableInfo.dtVlidateuser) != "" ? Convert.ToString(VariableInfo.dtVlidateuser.Rows[0]["USER_ID"]) : "";
        //                };
        //                MessageBoxResult MessResult = MessageBox.Show("Do You Want To Update All Details?", "Update Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
        //                if (MessResult == MessageBoxResult.No)
        //                { return; }
        //                oResponse = new BUSSINESS_LAYER.BL_MaterialMaster().Update(_objPLMaterialMaster);
        //                if (oResponse == OperationResult.UpdateSuccess)
        //                {
        //                    BCommon.setMessageBox(VariableInfo.mApp, "Record Updated Successfully.", 4);
        //                    Clear();
        //                    DisplayData(_objPLMaterialMaster);
        //                }
        //                else if (oResponse == OperationResult.UpdateError)
        //                {
        //                    BCommon.setMessageBox(VariableInfo.mApp, "Error On Update", 3);
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
        //        btnEdit.Cursor = Cursors.Arrow;
        //    }
        //    finally
        //    {
        //        btnEdit.Cursor = Cursors.Arrow;
        //        FilteredData();
        //    }
        //}
        
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
        
        //private void btnbrowse_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        string _ModuleType = string.Empty;
        //        DataTable _dtBindList = null;
        //        btnbrowse.Cursor = Cursors.Wait;
        //        Microsoft.Win32.OpenFileDialog openFileDialog1 = new Microsoft.Win32.OpenFileDialog();
        //        if (openFileDialog1.ShowDialog() == true)
        //        {
        //            openFileDialog1.Filter = "CSV files (*.csv)|*.Csv";
        //            lblPath.Content = openFileDialog1.FileName;
        //            _dtBindList = new DataTable();
        //            if (!openFileDialog1.FileName.Contains(".csv") && !openFileDialog1.FileName.Contains(".Csv"))
        //            {
        //                BCommon.setMessageBox(VariableInfo.mApp, "Invalid sheet!", 2);
        //                lblPath.Content = string.Empty;
        //                return;
        //            }
        //            _dtBindList = ConvertCSVtoDataTable(openFileDialog1.FileName);    //VariableInfo.IMPORT_DATA(openFileDialog1.FileName, _ModuleType);
        //            if (!ValidateExcelColumn(_dtBindList))
        //            {
        //                BCommon.setMessageBox(VariableInfo.mApp, "Invalid sheet!", 2);
        //                lblPath.Content = string.Empty;
        //                lv.Visibility = Visibility.Visible;
        //                lv.MaxHeight = 220;
        //                lvBrowse.Visibility = Visibility.Hidden;
        //                lvBrowse.MaxHeight = 0;
        //                lvBrowse.ItemsSource = _dtBindList.DefaultView;
        //                return;
        //            }
        //            bool CheckNullValues = CheckExcelRowValues(_dtBindList, true);
        //            if (CheckNullValues == false)
        //            {
        //                BCommon.setMessageBox(VariableInfo.mApp, "Invalid sheet!", 2);
        //                lv.Visibility = Visibility.Visible;
        //                lv.MaxHeight = 220;
        //                lvBrowse.Visibility = Visibility.Hidden;
        //                lvBrowse.MaxHeight = 0;
        //                return;
        //            }
        //            if (_dtBindList.Rows.Count > 0)
        //            {
        //                lvBrowse.Visibility = Visibility.Visible;
        //                lvBrowse.MaxHeight = 350;
        //                lv.Visibility = Visibility.Hidden;
        //                lvBrowse.ItemsSource = _dtBindList.DefaultView;
        //            }
        //            else
        //            {
        //                BCommon.setMessageBox(VariableInfo.mApp, " No data available in selected excel file.", 2);
        //                lvBrowse.Visibility = Visibility.Visible;
        //                lvBrowse.MaxHeight = 220;
        //                lvBrowse.ItemsSource = _dtBindList.DefaultView;
        //                return;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.Message.ToUpper().Contains("CHECK NUMBER OF COLUMNS "))
        //        { }
        //        BCommon.setMessageBox(VariableInfo.mApp, "Sheet is open. please close the sheet.", 3);
        //        btnbrowse.Cursor = Cursors.Arrow;
        //    }
        //    finally
        //    {
        //        btnbrowse.Cursor = Cursors.Arrow;
        //    }
        //}

        public static DataTable ConvertCSVtoDataTable(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //btnDelete.Cursor = Cursors.Wait;
                int iCount = 0;
                OperationResult oResponse = OperationResult.DeleteError;
                if (lv.Items.Count <= 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "No Data Found For Delete.", 1);
                    return;
                }
                MessageBoxResult MessResult = MessageBox.Show("Do You  Want To Delete Selected Record ? ", "Delete Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (MessResult == MessageBoxResult.No)
                {
                    return;
                }
                foreach (PL_MaterialMaster item in lv.ItemsSource)
                {
                    PL_MaterialMaster _objPLMaterialMaster = item;
                    {
                        if (item.IsValid == true)
                        {
                            oResponse = new BUSSINESS_LAYER.BL_MaterialMaster().Delete(_objPLMaterialMaster);
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
                    BCommon.setMessageBox(VariableInfo.mApp, "Select At Least One Record To Delete.", 1);
                    return;
                }

                if (oResponse == OperationResult.DeleteSuccess)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Selected Records Deleted Successfully.", 4);
                    PL_MaterialMaster _objPLMaterialMaster = new PL_MaterialMaster();
                    Clear();
                    DisplayData(_objPLMaterialMaster);
                    FilteredData();
                }
                else if (oResponse == OperationResult.DeleteError)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Error On Delete", 3);
                }
                else if (oResponse == OperationResult.DeleteRefference)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Record Used In Transaction, Can't be Delete", 3);
                }
            }
            catch (Exception ex)
            {
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
                //btnDelete.Cursor = Cursors.Arrow;
            }
            finally
            {
                //btnDelete.Cursor = Cursors.Arrow;
            }
        }
        
        //private void btnFileFormat_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        btnFileFormat.Cursor = Cursors.Wait;
        //        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Documents" + "\\MaterialMaster.xls"))
        //        {
        //            Process.Start(AppDomain.CurrentDomain.BaseDirectory + "Documents" + "\\MaterialMaster.xls");
        //        }
        //        else
        //        { BCommon.setMessageBox(VariableInfo.mApp, "File not exist.", 3); }
        //    }
        //    catch (Exception ex)
        //    { btnFileFormat.Cursor = Cursors.Arrow; BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3); }
        //    finally
        //    {
        //        FilteredData();
        //        btnFileFormat.Cursor = Cursors.Arrow;
        //    }
        //}

        #endregion

        #region Private Method

        private void Clear()
        {
            PL_MaterialMaster _PLMaterialMaster = new PL_MaterialMaster();
            txtSearch.Text = "";
            foreach (var items in lv.Items)
            {
                ((PL_MaterialMaster)(items)).IsValid = false;
                DisplayData(_PLMaterialMaster);
            }
            //chkSelectAll.IsChecked = false;
            //txtcode.Text = "";
            //txtdesc.Text = "";
            //FilteredData();
            //btnSave.IsEnabled = true;
            //btnDelete.IsEnabled = true;
            //btnEdit.IsEnabled = false;
            //btnEdit.Content = "Edit";          
            //txtcode.IsEnabled = true;
            //lblPath.Content = "";
            //lvBrowse.Visibility = Visibility.Hidden;
            //lvBrowse.MaxHeight = 0;
            //lvBrowse.ItemsSource = null;
            //lv.Visibility = Visibility.Visible;
            //lv.MaxHeight = 450;
            //txtcode.Focus();

        }
        
        //private bool ValidateInputs(bool p)
        //{
        //    if (String.IsNullOrEmpty(this.txtcode.Text.Trim()))
        //    {
        //        BCommon.setMessageBox(VariableInfo.mApp, "Enter Code first!", 1);
        //        this.txtcode.Focus();
        //        return p = false;
        //    }
        //    else if (String.IsNullOrEmpty(this.txtdesc.Text.Trim()))
        //    {
        //        BCommon.setMessageBox(VariableInfo.mApp, "Enter Description first!", 1);
        //        this.txtdesc.Focus();
        //        return p = false;
        //    }
        //    return p;
        //}
        
        private void DisplayData(PL_MaterialMaster ObjPLMaterialMaster)
        {
            try
            {
                int i = 0;
                string d = "";
                DataTable dt = new DataTable();
                MaterialMaster = new BUSSINESS_LAYER.BL_MaterialMaster().BI_GetMaterialMasterData(ObjPLMaterialMaster);
                //ObservableCollection<PLMaterialMaster> data = (ObservableCollection<PLMaterialMaster>)MaterialMaster;
                //dt = VariableInfo.ToDataTable(data);
                //foreach (DataRow dr in dt.Rows)
                //{
                //    d = dr["CreatedOn"].ToString();
                //    DateTime date = Convert.ToDateTime(d);
                //    d = date.ToString("dd/MM/yyyy");
                //    dt.Rows[i]["CreatedOn"] = d;
                //    i++;
                //}
                lv.ItemsSource = MaterialMaster;
                lblCount.Content = lv.Items.Count;
                ObjLog.WriteLog("Load SAP Material Master -- No of Records Count - " + Convert.ToString(lv.Items.Count) + " at " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }

        }

        private void EnableDisable(bool value)
        {
            //btnSave.IsEnabled = value;
            //txtcode.IsEnabled = value;
            //txtdesc.IsEnabled = value;           
        }

        private void txtSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(lv.ItemsSource).Refresh();
        }

        private void FilteredData()
        {
            if (lv.Items.Count > 0)
            {
                CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(lv.ItemsSource);
                view.Filter = UserFilter;
                lblCount.Content = lv.Items.Count;
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
                return ((item as PL_MaterialMaster).MatDescription.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0);
            }
        }
        
        private bool ValidateExcelColumn(DataTable dt)
        {
            bool isValid = false;
            if (Convert.ToString(dt.Columns[0].ColumnName) != "Product".Trim())
            {
                return isValid = false;
            }
            if (Convert.ToString(dt.Columns[1].ColumnName) != "Thickness".Trim())
            {
                return isValid = false;
            }
            if (Convert.ToString(dt.Columns[2].ColumnName) != "Size".Trim())
            {
                return isValid = false;
            }
            if (Convert.ToString(dt.Columns[3].ColumnName) != "MaterialDescription".Trim())
            {
                return isValid = false;
            }
            if (Convert.ToString(dt.Columns[4].ColumnName) != "MaterialCode".Trim())
            {
                return isValid = false;
            }
            else
            {
                return isValid = true;
            }
            return isValid;
        }
        
        private bool CheckExcelRowValues(DataTable dt, bool isValid)
        {
            isValid = true;
            foreach (DataRow row in dt.Rows)
            {
                if (row.IsNull("MaterialCode") || row["MaterialCode"] == "" )
                    return isValid = false;
               
            }
            return isValid;
        }


        #endregion

        private void txtcode_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            //if (char.IsLetterOrDigit(Convert.ToChar(e.Text)))
            //    e.Handled = false;
            //else
            //    e.Handled = true;
        }

    }
}
