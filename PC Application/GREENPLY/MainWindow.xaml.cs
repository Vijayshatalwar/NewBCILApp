using System;
using System.Windows;
using System.Windows.Controls;
using GREENPLY.UserControls.Masters;
using System.Windows.Media.Animation;
using ENTITY_LAYER;
using COMMON_LAYER;
using GREENPLY.UserControls.Transaction;


namespace GREENPLY
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string User_Type = string.Empty;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnContract_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnExpand_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            PL_UserMaster obj = new PL_UserMaster();
            try
            {
                if (VariableInfo.mLocationType == "VENDOR" || VariableInfo.mLocationType == "Vendor")
                {
                    LblLicInfo.Content = "Welcome - " + "User Name : "
                    + VariableInfo.dtVlidateuser.Rows[0][1]
                    + " | Location Code/Type : " + VariableInfo.mPlantCode
                    + " | Connected to Database : " + VariableInfo.mDatabase
                    + " | Version : 3.0.1"
                    + " | © 2021-22 Bar Code India";
                }
                else
                {
                    LblLicInfo.Content = "Welcome - " + "User Name : "
                    + VariableInfo.dtVlidateuser.Rows[0]["USER_NAME"]
                    + " | Location Code/Type : " + VariableInfo.mPlantCode
                    + " | Connected to Database : " + VariableInfo.mDatabase
                    + " | Version : 3.0.1"
                    + " | © 2021-22 Bar Code India";
                    User_Type = Convert.ToString(VariableInfo.dtVlidateuser.Rows[0]["USER_TYPE"]);
                }

                if (VariableInfo.dtGroupRights == null || User_Type.Equals("ADMIN"))
                {
                    TrueAllModules();    // in case of admin user
                }
                else if (VariableInfo.mLocationType != "VENDOR")
                {
                    #region User Rights
                    if (VariableInfo.dtGroupRights.Rows.Count > 0)
                    {
                        int no = VariableInfo.dtGroupRights.Rows.Count;
                        for (int i = 0; i < no; i++)
                        {
                            string module_name = Convert.ToString(VariableInfo.dtGroupRights.Rows[i]["Module_Name"]);
                            bool View_Rights = Convert.ToBoolean(VariableInfo.dtGroupRights.Rows[i]["VIEW_RIGHTS"]);

                            if (MaterialMaster.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                MaterialMaster.IsEnabled = true;
                            }
                            if (UserMaster.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                UserMaster.IsEnabled = true;
                            }
                            if (WarehouseMaster.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                WarehouseMaster.IsEnabled = true;
                            }
                            if (DepotMaster.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                DepotMaster.IsEnabled = true;
                            }
                            if (PlantMaster.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                PlantMaster.IsEnabled = true;
                            }
                            if (VendorMaster.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                VendorMaster.IsEnabled = true;
                            }
                            if (EMailConfigurationMaster.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                EMailConfigurationMaster.IsEnabled = true;
                            }
                            if (GroupMaster.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                GroupMaster.IsEnabled = true;
                            }
                            if (GroupRights.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                GroupRights.IsEnabled = true;
                            }
                            if (ItemSelection.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                ItemSelection.IsEnabled = true;
                            }
                            if (ItemSelectionRejMaterial.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                ItemSelectionRejMaterial.IsEnabled = true;
                            }
                            if (SegregationStackPrinting.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                SegregationStackPrinting.IsEnabled = true;
                            }
                            if (VendorBarcodeGeneration.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                VendorBarcodeGeneration.IsEnabled = true;
                            }
                            if (VendorLabelPrinting.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                VendorLabelPrinting.IsEnabled = true;
                            }
                            if (VendorLabelRePrinting.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                VendorLabelRePrinting.IsEnabled = true;
                            }
                            if (ExistingStockLabelPrinting.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                ExistingStockLabelPrinting.IsEnabled = true;
                            }
                            if (SerialNoGenReport.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                SerialNoGenReport.IsEnabled = true;
                            }
                            if (StockCountReport.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                StockCountReport.IsEnabled = true;
                            }
                            if (StackLabelReprinting.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                StackLabelReprinting.IsEnabled = true;
                            }
                            if (QualityInspection.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                QualityInspection.IsEnabled = true;
                            }
                            if (RejectionMaster.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                RejectionMaster.IsEnabled = true;
                            }
                            if (HubLabelReprinting.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                HubLabelReprinting.IsEnabled = true;
                            }
                            if (HubLabelPrinting.Name.ToUpper() == module_name.ToUpper() && View_Rights == true)
                            {
                                HubLabelPrinting.IsEnabled = true;
                            }
                            if (Properties.Settings.Default.PrintingLocationType == "PLANT")
                            {
                                ItemSelectionRejMaterial.Visibility = System.Windows.Visibility.Hidden;
                            }
                            if (Properties.Settings.Default.PrintingLocationType == "HUB")
                            {
                                ItemSelectionRejMaterial.Visibility = System.Windows.Visibility.Visible;
                            }
                        }
                    }
                    #endregion
                }
                else if (VariableInfo.mLocationType == "VENDOR" || VariableInfo.mLocationType == "Vendor")
                {
                    #region User Rights
                    Masters.Visibility = System.Windows.Visibility.Hidden;
                    UserManagement.Visibility = System.Windows.Visibility.Hidden;
                    ItemSelection.Visibility = System.Windows.Visibility.Hidden;
                    SegregationPrinting.Visibility = System.Windows.Visibility.Hidden;
                    WarehouseOperation.Visibility = System.Windows.Visibility.Hidden;
                    Reports.Visibility = System.Windows.Visibility.Hidden;
                    CONVEYOROPERATION.Visibility = System.Windows.Visibility.Hidden;
                    QUALITYINSPECTION.Visibility = System.Windows.Visibility.Hidden;
                    //VendorPortal.Margin = new Thickness(0, 10, 0, 10);
                    VendorBarcodeGeneration.IsEnabled = false;
                    VendorBarcodeGeneration.Visibility = System.Windows.Visibility.Hidden;
                    VendorLabelPrinting.IsEnabled = true;
                    VendorLabelRePrinting.IsEnabled = true;
                    #endregion
                }

            }
            catch (Exception ex)
            {
                ex.ToString();
            }
        }

        //
        void TrueAllModules()
        {
            GroupMaster.IsEnabled = true;
            GroupRights.IsEnabled = true;
            UserMaster.IsEnabled = true;
            MaterialMaster.IsEnabled = true;
            WarehouseMaster.IsEnabled = true;
            DepotMaster.IsEnabled = true;
            PlantMaster.IsEnabled = true;
            VendorMaster.IsEnabled = true;
            EMailConfigurationMaster.IsEnabled = true;
            VendorLabelPrinting.IsEnabled = true;
            VendorLabelRePrinting.IsEnabled = true;
            VendorBarcodeGeneration.IsEnabled = true;
            SegregationStackPrinting.IsEnabled = true;
            ItemSelectionRejMaterial.IsEnabled = true;
            ExistingStockLabelPrinting.IsEnabled = true;
            SerialNoGenReport.IsEnabled = true;
            StockCountReport.IsEnabled = true;
            ItemSelection.IsEnabled = true;
            StackLabelReprinting.IsEnabled = true;
            HubLabelPrinting.IsEnabled = true;
            HubLabelReprinting.IsEnabled = true;
            DeliveryCancellation.IsEnabled = true;
            DepotBranchDispatch.IsEnabled = true;
            QualityInspection.IsEnabled = true;
            RejectionMaster.IsEnabled = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            Application.Current.Shutdown();
            try
            {
                this.Close();
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                //  VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "TackCommServer_FormClosing", ex.Message);
            }
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            WinLogin oLogin = new WinLogin();
            oLogin.Show();
            this.Hide();
        }

        private void HandleExpanderExpanded(object sender, RoutedEventArgs e)
        {
            ExpandExculsively(sender as Expander);
        }

        private void ExpandExculsively(Expander expander)
        {
            foreach (var child in spButtons.Children)
            {
                if (child is Expander && child != expander)
                    ((Expander)child).IsExpanded = false;
            }
        }

        #region Masters

        private void btnGroupMaster_Click(object sender, RoutedEventArgs e)
        {
            UcGroupMaster _UcGroupMaster = new UcGroupMaster();
            if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
            { return; }
            stcPnlCntr.Children.Clear();
            stcPnlCntr.Children.Add(_UcGroupMaster);
            Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
            s.Begin();
        }

        private void GroupRights_Click(object sender, RoutedEventArgs e)
        {
            UcGroupRights _UcGroupRights = new UcGroupRights();
            if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
            { return; }
            stcPnlCntr.Children.Clear();
            stcPnlCntr.Children.Add(_UcGroupRights);
            Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
            s.Begin();
        }

        private void btnUserMaster_Click(object sender, RoutedEventArgs e)
        {
            UcUserMaster _UcUserMaster = new UcUserMaster();
            if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
            { return; }
            stcPnlCntr.Children.Clear();
            stcPnlCntr.Children.Add(_UcUserMaster);
            Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
            s.Begin();
        }

        private void ITEMMASTER_Click(object sender, RoutedEventArgs e)
        {
            UCMaterialMaster _StkUCUCMaterialMaster = new UCMaterialMaster();
            if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
            { return; }
            stcPnlCntr.Children.Clear();
            stcPnlCntr.Children.Add(_StkUCUCMaterialMaster);
            Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
            s.Begin();
        }

        private void WAREHOUSEMASTER_Click(object sender, RoutedEventArgs e)
        {
            UcWarehouseMaster _UcWHMaster = new UcWarehouseMaster();
            if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
            { return; }
            stcPnlCntr.Children.Clear();
            stcPnlCntr.Children.Add(_UcWHMaster);
            Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
            s.Begin();
        }

        private void DEPOTMASTER_Click(object sender, RoutedEventArgs e)
        {
            UcDepotMaster _UcDepotMaster = new UcDepotMaster();
            if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
            { return; }
            stcPnlCntr.Children.Clear();
            stcPnlCntr.Children.Add(_UcDepotMaster);
            Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
            s.Begin();
        }

        private void PLANTMASTER_Click(object sender, RoutedEventArgs e)
        {
            UcPlantMaster _UcPlantMaster = new UcPlantMaster();
            if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
            { return; }
            stcPnlCntr.Children.Clear();
            stcPnlCntr.Children.Add(_UcPlantMaster);
            Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
            s.Begin();
        }

        private void VENDORMASTER_Click(object sender, RoutedEventArgs e)
        {
            UcVendorMaster _UcVMaster = new UcVendorMaster();
            if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
            { return; }
            stcPnlCntr.Children.Clear();
            stcPnlCntr.Children.Add(_UcVMaster);
            Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
            s.Begin();
        }

        private void EMAILCONFIGMASTER_Click(object sender, RoutedEventArgs e)
        {
            UcEmailConfigMaster _UcEmailConfigMaster = new UcEmailConfigMaster();
            if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
            { return; }
            stcPnlCntr.Children.Clear();
            stcPnlCntr.Children.Add(_UcEmailConfigMaster);
            Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
            s.Begin();
        }

        private void RejectionMaster_Click(object sender, RoutedEventArgs e)
        {
            UcRejectionMaster _UcRejMaster = new UcRejectionMaster();
            if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
            { return; }
            stcPnlCntr.Children.Clear();
            stcPnlCntr.Children.Add(_UcRejMaster);
            Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
            s.Begin();
        }

        #endregion

        #region Item Selection

        private void ITEMSELECTION_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.PrintMaterialType1 == "PLY" || Properties.Settings.Default.PrintMaterialType1 == "DOOR"
                || Properties.Settings.Default.PrintMaterialType2 == "PLY" || Properties.Settings.Default.PrintMaterialType2 == "DOOR"
                || Properties.Settings.Default.PrintMaterialType1 == "TRADING PLY" || Properties.Settings.Default.PrintMaterialType2 == "TRADING DOOR"
                || Properties.Settings.Default.PrintMaterialType2 == "TRADING PLY" || Properties.Settings.Default.PrintMaterialType2 == "TRADING DOOR")
            {
                UCDoorPlyItemSelection _UcMatSelect = new UCDoorPlyItemSelection(Properties.Settings.Default.PrintMaterialType1, Properties.Settings.Default.PrintMaterialType2);
                if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
                { return; }
                stcPnlCntr.Children.Clear();
                stcPnlCntr.Children.Add(_UcMatSelect);
                Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
                s.Begin();
            }
            else if (Properties.Settings.Default.PrintMaterialType1 == "VENEER" || Properties.Settings.Default.PrintMaterialType1 == "TRADING VENEER"
                     || Properties.Settings.Default.PrintMaterialType2 == "VENEER" || Properties.Settings.Default.PrintMaterialType2 == "TRADING VENEER")
            {
                UCDecorItemSelection _UcMatSelect = new UCDecorItemSelection();
                if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
                { return; }
                stcPnlCntr.Children.Clear();
                stcPnlCntr.Children.Add(_UcMatSelect);
                Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
                s.Begin();
            }
        }

        private void ExistingStockLabelPrinting_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.PrintMaterialType1 == "PLY" || Properties.Settings.Default.PrintMaterialType2 == "DOOR"
                || Properties.Settings.Default.PrintMaterialType1 == "TRADING PLY" || Properties.Settings.Default.PrintMaterialType2 == "TRADING DOOR")
            {
                UCDoorPlyExistingStockPrinting _UcMatSelect = new UCDoorPlyExistingStockPrinting();
                if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
                { return; }
                stcPnlCntr.Children.Clear();
                stcPnlCntr.Children.Add(_UcMatSelect);
                Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
                s.Begin();
            }
            else if (Properties.Settings.Default.PrintMaterialType1 == "VENEER" || Properties.Settings.Default.PrintMaterialType1 == "TRADING VENEER"
                     || Properties.Settings.Default.PrintMaterialType2 == "VENEER" || Properties.Settings.Default.PrintMaterialType2 == "TRADING VENEER")
            {
                UCDecorExistingStockPrinting _Ucprinting = new UCDecorExistingStockPrinting();
                if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
                { return; }
                stcPnlCntr.Children.Clear();
                stcPnlCntr.Children.Add(_Ucprinting);
                Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
                s.Begin();
            }
        }

        private void ItemSelectionRejMaterial_Click(object sender, RoutedEventArgs e)
        {
            UCDoorPlyItemSelection _UcMatSelect = new UCDoorPlyItemSelection("REJECTION", "");
            if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
            { return; }
            stcPnlCntr.Children.Clear();
            stcPnlCntr.Children.Add(_UcMatSelect);
            Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
            s.Begin();
        }

        #endregion


        private void StackLabelReprinting_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.PrintMaterialType1 == "PLY" || Properties.Settings.Default.PrintMaterialType2 == "DOOR"
                || Properties.Settings.Default.PrintMaterialType1 == "TRADING PLY" || Properties.Settings.Default.PrintMaterialType2 == "TRADING DOOR")
            {
                UCRePrinting _Ucprinting = new UCRePrinting();
                if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
                { return; }
                stcPnlCntr.Children.Clear();
                stcPnlCntr.Children.Add(_Ucprinting);
                Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
                s.Begin();
            }
            else if (Properties.Settings.Default.PrintMaterialType1 == "VENEER" || Properties.Settings.Default.PrintMaterialType1 == "TRADING VENEER"
                     || Properties.Settings.Default.PrintMaterialType2 == "VENEER" || Properties.Settings.Default.PrintMaterialType2 == "TRADING VENEER")
            {
                UCDecorReprinting _Ucprinting = new UCDecorReprinting();
                if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
                { return; }
                stcPnlCntr.Children.Clear();
                stcPnlCntr.Children.Add(_Ucprinting);
                Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
                s.Begin();
            }
        }

        #region Manufacturing Plant

        #endregion

        #region Reports

        private void SerialNoGenReport_Click(object sender, RoutedEventArgs e)
        {
            UserControls.Reports.UCSerialNoGenReports _UcReport = new UserControls.Reports.UCSerialNoGenReports();
            if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
            { return; }
            stcPnlCntr.Children.Clear();
            stcPnlCntr.Children.Add(_UcReport);
            Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
            s.Begin();
        }

        private void StockCountReport_Click(object sender, RoutedEventArgs e)
        {
            UserControls.Reports.UCStockCountReport _UcReport = new UserControls.Reports.UCStockCountReport();
            if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
            { return; }
            stcPnlCntr.Children.Clear();
            stcPnlCntr.Children.Add(_UcReport);
            Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
            s.Begin();
        }

        #endregion

        #region Vendor Portal

        private void LabelPrinting_Click(object sender, RoutedEventArgs e)
        {
            UCVendorLabelPrinting _Ucprinting = new UCVendorLabelPrinting();
            if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
            { return; }
            stcPnlCntr.Children.Clear();
            stcPnlCntr.Children.Add(_Ucprinting);
            Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
            s.Begin();
        }

        private void LabelRePrinting_Click(object sender, RoutedEventArgs e)
        {
            UCVendorLabelReprinting _Ucprinting = new UCVendorLabelReprinting();
            if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
            { return; }
            stcPnlCntr.Children.Clear();
            stcPnlCntr.Children.Add(_Ucprinting);
            Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
            s.Begin();
        }

        private void POTransferToVendor_Click(object sender, RoutedEventArgs e)
        {
            UCVendorBarcodeGeneration _UcTransfer = new UCVendorBarcodeGeneration();
            if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
            { return; }
            stcPnlCntr.Children.Clear();
            stcPnlCntr.Children.Add(_UcTransfer);
            Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
            s.Begin();
        }

        #endregion

        private void SegragationStackPrinting_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.PrintMaterialType1 == "PLY" || Properties.Settings.Default.PrintMaterialType2 == "DOOR"
                || Properties.Settings.Default.PrintMaterialType1 == "TRADING PLY" || Properties.Settings.Default.PrintMaterialType2 == "TRADING DOOR")
            {
                UCSegragationStackPrinting _Ucprinting = new UCSegragationStackPrinting();
                if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
                { return; }
                stcPnlCntr.Children.Clear();
                stcPnlCntr.Children.Add(_Ucprinting);
                Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
                s.Begin();
            }
            else if (Properties.Settings.Default.PrintMaterialType1 == "VENEER" || Properties.Settings.Default.PrintMaterialType1 == "TRADING VENEER"
                     || Properties.Settings.Default.PrintMaterialType2 == "VENEER" || Properties.Settings.Default.PrintMaterialType2 == "TRADING VENEER")
            {
                UCDecorSegregationStackPrinting _Ucprinting = new UCDecorSegregationStackPrinting();
                if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
                { return; }
                stcPnlCntr.Children.Clear();
                stcPnlCntr.Children.Add(_Ucprinting);
                Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
                s.Begin();
            }


            
        }

        private void HubLabelPrinting_Click(object sender, RoutedEventArgs e)
        {
            UCHUBLabelPrinting _Ucprinting = new UCHUBLabelPrinting();
            if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
            { return; }
            stcPnlCntr.Children.Clear();
            stcPnlCntr.Children.Add(_Ucprinting);
            Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
            s.Begin();
        }

        private void HubLabelReprinting_Click(object sender, RoutedEventArgs e)
        {
            UCHUBLabelReprinting _Ucprinting = new UCHUBLabelReprinting();
            if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
            { return; }
            stcPnlCntr.Children.Clear();
            stcPnlCntr.Children.Add(_Ucprinting);
            Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
            s.Begin();
        }

        private void DeliveryCancellation_Click(object sender, RoutedEventArgs e)
        {
            UCDeliveryCancelledStack _Ucprinting = new UCDeliveryCancelledStack();
            if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
            { return; }
            stcPnlCntr.Children.Clear();
            stcPnlCntr.Children.Add(_Ucprinting);
            Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
            s.Begin();
        }

        private void DepotBranchDispatch_Click(object sender, RoutedEventArgs e)
        {
            UCDepotBranchDispatch _UcMatSelect = new UCDepotBranchDispatch();
            if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
            { return; }
            stcPnlCntr.Children.Clear();
            stcPnlCntr.Children.Add(_UcMatSelect);
            Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
            s.Begin();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
            try
            {
                this.Close();
                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                //  VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "TackCommServer_FormClosing", ex.Message);
            }
        }

        private void QualityInspection_Click(object sender, RoutedEventArgs e)
        {
            UCDoorPlyItemSelection _UcMatSelect = new UCDoorPlyItemSelection("", "");
            if (stcPnlCntr.Children.Count > 0 && stcPnlCntr.Children[0].GetType().Equals(typeof(Image)))
            { return; }
            stcPnlCntr.Children.Clear();
            stcPnlCntr.Children.Add(_UcMatSelect);
            Storyboard s = (Storyboard)TryFindResource("ContractingStoryboard");
            s.Begin();
        }

    }
}
