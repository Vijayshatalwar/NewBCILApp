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
using System.Windows.Navigation;
using System.Diagnostics;
using System.Windows.Media.Animation;
using System.Reflection;
using COMMON_LAYER;
using GREENPLY.Classes;
using COMMON; 
using ENTITY_LAYER;
using GREENPLY;
using BUSSINESS_LAYER;
namespace GREENPLY.Application_Base
{
    /// <summary>
    /// Interaction logic for WinChangePassword.xaml
    /// </summary>
    public partial class WinChangePassword : Window
    {
        public WinChangePassword()
        {
            InitializeComponent();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            OperationResult _oResponse = OperationResult.UpdateError;
            try
            {
                bool sInput = ValidateInputs(true);
                if (sInput != false)
                {
                    PL_UserMaster _objPL_UserMaster = new PL_UserMaster();
                    {
                        _objPL_UserMaster.USER_ID = txtUserId.Text.Trim();
                        _objPL_UserMaster.Password = VariableInfo.EncryptPassword(txtCurrentPassword.Password.Trim(), "E");
                        _objPL_UserMaster.NewPswd = VariableInfo.EncryptPassword(txtNewPassword.Password.Trim(), "E");
                    };
                    _oResponse = new BL_UserMaster().UpdatePassword(_objPL_UserMaster);
                    if (_oResponse == OperationResult.UpdateSuccess)
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Password Changed Successfully!!", 4);
                        txtCurrentPassword.Password = string.Empty;
                        txtNewPassword.Password = string.Empty;
                        txtConfirmPassword.Password = string.Empty;
                        txtUserId.Text = string.Empty;
                        txtUserId.Focus();
                        WinLogin _oLogin = new WinLogin();
                        _oLogin.Show();
                        this.Close();
                        return;
                    }
                    else if (_oResponse == OperationResult.UpdateError)
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Either UserID/Current Password wrong!!", 3);
                        txtCurrentPassword.Password = string.Empty;
                        txtNewPassword.Password = string.Empty;
                        txtConfirmPassword.Password = string.Empty;
                        txtCurrentPassword.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
                 
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
        }

        private bool ValidateInputs(bool p)
        {
            try
            {
                if (string.IsNullOrEmpty(txtUserId.Text.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter User Id first!!", 1);
                    txtUserId.Focus();
                    return p = false;
                }
                else if (string.IsNullOrEmpty(txtCurrentPassword.Password.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter Current Password first!!", 1);
                    txtCurrentPassword.Focus();
                    return p = false;
                }
                else if (string.IsNullOrEmpty(txtNewPassword.Password.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter New Password first!!", 1);
                    txtNewPassword.Focus();
                    return p = false;
                }
                else if (string.IsNullOrEmpty(txtConfirmPassword.Password.Trim()))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter Confirm Password first!!", 1);
                    txtConfirmPassword.Focus();
                    return p = false;
                }
                else if (txtNewPassword.Password.Trim() != txtConfirmPassword.Password.Trim())
                {
                    BCommon.setMessageBox(VariableInfo.mApp, " New and Confirm Password Mismatch!!", 1);
                    txtConfirmPassword.Focus();
                    return p = false;
                }
            }
            catch (Exception ex)
            {
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
                 
            }
            return p;
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                WinLogin _WinLogin = new WinLogin();
                _WinLogin.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
                 
            }
        }
    }
}
