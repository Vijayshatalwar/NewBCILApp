using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Reflection; 
using System.Data;
using System.Collections.ObjectModel;
using System.ComponentModel;
using COMMON;
using ENTITY_LAYER;
using COMMON_LAYER;
using BUSSINESS_LAYER;
using GREENPLY.Classes;

namespace GREENPLY.UserControls.Masters
{
    /// <summary>
    /// Interaction logic for UcGroupRights.xaml
    /// </summary>
    public partial class UcGroupRights : UserControl
    {
        public UcGroupRights()
        {
            InitializeComponent();
        } 
        ObservableCollection<PL_Group_Master> _PL_Group_Master = new ObservableCollection<PL_Group_Master>();
        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string propertyname)
        {
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyname));
        }
        public ObservableCollection<PL_Group_Master> PL_Group_MasterData
        {
            get { return _PL_Group_Master; }
            set
            {
                _PL_Group_Master = value;
                OnPropertyChanged("PL_Group_MasterData");
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult MessResult = MessageBox.Show("Do You Want To Clear All Details?", "Clear Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (MessResult == MessageBoxResult.No)
            {
                return;
            }
            lv.ItemsSource = null;
            this.cmbgroup.SelectionChanged -= new SelectionChangedEventHandler(cmbgroup_SelectionChanged);
            cmbgroup.SelectedIndex = 0;
            this.cmbgroup.SelectionChanged += new SelectionChangedEventHandler(cmbgroup_SelectionChanged);
            cmbgroup.Focus();
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

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BL_Group_Rights blobj = new BL_Group_Rights();
                PL_Group_Master plobj = new PL_Group_Master();
                OperationResult oResutl = OperationResult.Error;
                if (cmbgroup.SelectedIndex < 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Select Group Name", 1);
                    return;
                }
                int iRowCnt = lv.Items.Count;
                if (iRowCnt == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "No Data Found For Update", 1);
                    return;
                }
                MessageBoxResult MessResult = MessageBox.Show("Update Confirmation", "Update", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                if (MessResult == MessageBoxResult.No)
                {
                    return;
                }
                plobj.GroupID = cmbgroup.SelectedValue.ToString();
                foreach (PL_Group_Master gvRow in lv.ItemsSource)
                {
                    PL_Group_Master obj = gvRow;
                    obj.GroupID = cmbgroup.SelectedValue.ToString();
                    oResutl = blobj.SaveUpdateGroupRights(obj);
                }
                if (oResutl == OperationResult.UpdateSuccess)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Record Updated Successfully", 4);
                    lv.ItemsSource = null;
                    cmbgroup.SelectedIndex = 0;
                }
                else if (oResutl == OperationResult.UpdateError)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Error On Update, Kindly Try Again", 3);
                }
                //GetGroupRights();
            }
            catch (Exception ex)
            {
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
            finally
            {
                //checkPermissions();
            }
        } 
        void checkPermissions()
        {
            if (!VariableInfo.dtVlidateuser.Rows[0]["USER_TYPE"].ToString().Equals("ADMIN"))
            {
                string _strRights = VariableInfo.GetRights("GROUPRIGHTS", VariableInfo.dtGroupRights);
                VariableInfo._strRights = _strRights.Split('^');
                if (VariableInfo._strRights[0] == "0")  //Check view rights
                {

                } 
                if (VariableInfo._strRights[2] == "0") //Check edit/update rights
                {
                    btnEdit.IsEnabled = false;
                }

            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //checkPermissions();
                GetGroups();
                cmbgroup.Focus();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }  
        private void GetGroups()
        {
            try
            {
                BL_Group_Rights objBL_Group_Rights = new BL_Group_Rights();
                PL_Group_Master objPL_Group_Master = new PL_Group_Master();
                { }
                DataSet dsDDData = objBL_Group_Rights.GetDropDownData(objPL_Group_Master, "GROUP");
                VariableInfo.BindDropDown(cmbgroup, dsDDData.Tables[0], "GroupName", "GroupID", "GROUP");
            }
            catch (Exception ex)
            {

            }
        }
        private void DisplayData()
        {
            try
            {
                PL_Group_Master objPL_Group_Master = new PL_Group_Master();
                {
                    objPL_Group_Master.GroupID = Convert.ToString(this.cmbgroup.SelectedValue); 
                }
                PL_Group_MasterData = new BL_Group_Rights().BI_GetUploadData(objPL_Group_Master);
                lv.ItemsSource = PL_Group_MasterData;
                //lblModuleId.Content = objPL_Group_Master.MODULE_ID; 
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
            }
        }

        private void cmbgroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbgroup.SelectedIndex > -1)
                {
                    DisplayData();
                }
                else
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "", 1);
                }
            }
            catch (Exception ex)
            {
                 
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            } 
        }

      
    }
}
