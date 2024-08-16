using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using System.Reflection;
using System.Data.SqlClient;
using System.Data;
using COMMON_LAYER;
using GREENPLY;
using BUSSINESS_LAYER;
using GREENPLY.Classes;

namespace GREENPLY
{
    /// <summary>
    /// Interaction logic for WinDbSetting.xaml
    /// </summary>
    public partial class WinDbSetting : Window
    {
        public WinDbSetting()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            cmbServerName.SelectedValue = VariableInfo.mDataSource;
            cmbDbName.SelectedValue = VariableInfo.mDatabase;
            txtUserName.Text = VariableInfo.mDbUser;
            txtPwd.Password = VariableInfo.mDbPassword;
            cmbServerName.Focus();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidatePage();
                VariableInfo.mDataSource = cmbServerName.Text.Trim();
                VariableInfo.mDatabase = cmbDbName.Text.Trim();
                VariableInfo.mDbUser = txtUserName.Text.Trim();
                VariableInfo.mDbPassword = txtPwd.Password.Trim();
                VariableInfo.mPrinterName = txtPrinter.Text.Trim();
                StreamWriter _wr = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + "\\Setting.ini", false);
                _wr.WriteLine("<LOCAL_SETTING>");
                _wr.WriteLine("SERVERNAME=" + cmbServerName.Text.ToString().Trim());
                _wr.WriteLine("DATABASENAME=" + cmbDbName.Text.ToString().Trim());
                _wr.WriteLine("USERNAME=" + txtUserName.Text.Trim());
                _wr.WriteLine("PASSWORD=" + txtPwd.Password.Trim());
                _wr.WriteLine("PRINTERNAME=" + VariableInfo.mPrinterName);
                _wr.WriteLine("</LOCAL_SETTING>");
                _wr.Close();
                _wr.Dispose();
                if (BCommon.ReadParameter() == true)
                {
                    MessageBox.Show("Database Connected Successfully");
                    WinLogin _uiLogin = new WinLogin();
                    _uiLogin.Show();
                    this.Hide();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ValidatePage()
        {
            try
            {
                if (cmbServerName.SelectedIndex == 0)
                { BCommon.setMessageBox(VariableInfo.mApp, "Select Server Name", 2); cmbServerName.Focus(); return; }
                if (cmbDbName.SelectedIndex == 0)
                { BCommon.setMessageBox(VariableInfo.mApp, "Select Database Name", 2); cmbDbName.Focus(); return; }
                if (txtUserName.Text == "")
                { BCommon.setMessageBox(VariableInfo.mApp, "Enter user name", 2); txtUserName.Focus(); return; }
                if (txtPwd.Password == "")
                { BCommon.setMessageBox(VariableInfo.mApp, "Enter password", 2); txtPwd.Focus(); return; }
                if (txtPrinter.Text == "")
                { BCommon.setMessageBox(VariableInfo.mApp, "Enter Printer Name", 2); txtPrinter.Focus(); return; }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult mResult = MessageBox.Show("Do You Really Want To Clear ?", "GreenPly", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (mResult == MessageBoxResult.No)
                { return; }
                if (cmbDbName.SelectedValue.ToString().Trim() != null)
                { cmbDbName.SelectedIndex = 0; }
                if (cmbServerName.SelectedValue.ToString().Trim() != null)
                { cmbServerName.SelectedIndex = 0; }
                if (txtUserName != null)
                { txtUserName.Text = ""; }
                if (txtPwd != null)
                { txtPwd.Password = ""; }
                txtPrinter.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btn1Exit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cmbServer_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (cmbServerName.Items.Count == 0)
                {
                    BCommon.FillCombobox(cmbServerName, VariableInfo.getDBServer(), "Display");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Sever Not Found! :  " + ex.ToString());
            }
        }

        private void cmbDbName_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (cmbDbName.Items.Count == 0 && !String.IsNullOrEmpty(txtUserName.Text.Trim()) && !String.IsNullOrEmpty(txtPwd.Password.Trim()) && cmbServerName.SelectedIndex > 0)
                {
                    BCommon.FillCombobox(cmbDbName, VariableInfo.getDBSchema(cmbServerName.Text.ToString(), txtUserName.Text.Trim(), txtPwd.Password.Trim()), "Display");
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Sever Not Found! :  " + ex.ToString());
            }
        }

    }
}
