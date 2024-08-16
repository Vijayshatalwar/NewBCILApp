using System;
using System.Windows;
using System.Reflection;
using System.IO;
using System.Windows.Navigation;
using System.Diagnostics;
using GREENPLY.Application_Base;
using COMMON_LAYER;
using ENTITY_LAYER;
using System.Windows.Input;
using GREENPLY.Classes;
using BUSSINESS_LAYER;
using System.Data;
using System.Windows.Forms;

namespace GREENPLY
{
    /// <summary>
    /// Interaction logic for WinLogin.xaml
    /// </summary>
    public partial class WinLogin : Window
    {
        public WinLogin()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DirectoryInfo _dir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory + "\\" + "LogFiles");
            if (_dir.Exists == false)
            {
                _dir.Create();
            }
            //LblLicInfo.Content = "Version 1.0.0";
            LblLicInfo.Content = Convert.ToString(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
            GetPlantCode();
            txtUserId.Focus();
           // txtPlantCode.Text = "3020";
            
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Login();
            }
            catch (Exception ex)
            {
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            WinChangePassword _WinChangePassword = new WinChangePassword();
            _WinChangePassword.ShowDialog();
            this.Hide();
        }

        private void txtPassword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                //if(
            }
            catch (Exception ex)
            { BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3); }
        }

        private void txtPassword_PreviewKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                {
                    Login();
                }
            }
            catch (Exception ex)
            { BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3); }

        }

        private void Login()
        {
            //VariableInfo.PlantCode =new BUSSINESS_LAYER.BlReceiving().BlGetPlantCode();
            PL_UserMaster _objPL_UserMaster = new PL_UserMaster();
            BL_UserMaster _objBL_UserMaster = new BL_UserMaster();
            try
            {
                if (string.IsNullOrEmpty(txtUserId.Text))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter User Id/Code", 1);
                    txtUserId.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtPassword.Password))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter User Password", 1);
                    txtPassword.Focus();
                    return;
                }
                else if (string.IsNullOrEmpty(txtPlantCode.Text))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Enter Location Code/Type", 1);
                    txtPlantCode.Focus();
                    return;
                }

                #region If No user exists then user name is ADMIN


                int iCdate = Convert.ToInt32(DateTime.Today.Date.ToString("dd"));
                iCdate = iCdate * 2;
                int iCmonth = Convert.ToInt32(DateTime.Today.Month);
                iCmonth = iCmonth * 2;
                if (_objBL_UserMaster.AppStartFirstTime())
                {
                    if (txtUserId.Text.ToUpper() == "ADMIN".ToUpper() && txtPassword.Password == (Convert.ToString(iCdate) + Convert.ToString(iCmonth)))
                    {
                        VariableInfo.mAppUserID = txtUserId.Text.Trim();
                        //VariableInfo.mPlantCode = cmbPlantCode.SelectedValue.ToString();
                        VariableInfo.mAppUserType = "ADMIN";
                        MainWindow _window = new MainWindow();
                        _window.Show();
                        this.Hide();
                    }
                    else
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Invalid User Id/Password", 2);
                        txtUserId.Focus();
                        return;
                    }
                }
                #endregion

                string strUser = txtUserId.Text.ToUpper();
                string str1 = strUser.Substring(0, 1);
                if (str1 == "D")
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "This user is authenticate to GreenPly Device Application, Login with PC User", 2);
                    return;
                }
                else
                {
                    string Pwd = string.Empty;
                    _objPL_UserMaster.USER_ID = txtUserId.Text.Trim();
                    _objPL_UserMaster.Password = txtPassword.Password.Trim();
                    _objPL_UserMaster.Password = VariableInfo.EncryptPassword(txtPassword.Password.Trim(), "E");
                    _objPL_UserMaster.LOCATION_CODE =  txtPlantCode.Text.Trim();
                    if (txtPlantCode.Text.Trim() == "Vendor" || txtPlantCode.Text.Trim() == "VENDOR")
                        VariableInfo.dtVlidateuser = _objBL_UserMaster.ValidateVendorUser(_objPL_UserMaster);
                    else
                        VariableInfo.dtVlidateuser = _objBL_UserMaster.ValidateUser(_objPL_UserMaster);

                    if (VariableInfo.dtVlidateuser.Columns.Contains("STATUS"))
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, VariableInfo.dtVlidateuser.Rows[0][0].ToString().Trim(), 2);
                        //txtUserId.Text = "";
                        //txtPassword.Password = "";
                        txtUserId.Focus();
                        return;
                    }
                    else
                    {
                        if (VariableInfo.dtVlidateuser.Rows.Count > 0)
                        {
                            if (txtPlantCode.Text.Trim() == "Vendor" || txtPlantCode.Text.Trim() == "VENDOR")
                            {
                                VariableInfo.mAppUserID = txtUserId.Text.Trim();
                                MainWindow _window = new MainWindow();
                                _window.Show();
                                this.Hide();
                            }
                            else
                            {
                                VariableInfo.mPlantCode = txtPlantCode.Text.Trim();
                                //VariableInfo.mPlantType = _objBL_UserMaster.GetPlantType();
                                VariableInfo.mAppUserID = txtUserId.Text.Trim();
                                VariableInfo.dtGroupRights = _objBL_UserMaster.GetGroupRights(txtUserId.Text.Trim());
                                MainWindow _window = new MainWindow();
                                _window.Show();
                                this.Hide();
                            }
                        }
                        else
                        {
                            BCommon.setMessageBox(VariableInfo.mApp, "Invalid User Id/Password", 2);
                            txtUserId.Text = "";
                            txtPassword.Password = "";
                            txtUserId.Focus();
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void GetPlantCode()
        {
            try
            {
                BL_UserMaster objBL_UserMaster = new BL_UserMaster();
                { }
                //VariableInfo.mPlantCode = objBL_UserMaster.GetPlantCode();
                txtPlantCode.Text = ""; // VariableInfo.mPlantCode;
                //VariableInfo.mPlantCode = txtPlantCode.Text.Trim();
            }
            catch (Exception ex)
            {
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

    }
}
