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
using System.Globalization;
using System.Threading;
//using Microsoft.Reporting.WinForms;

namespace GREENPLY.UserControls.Reports
{
    /// <summary>
    /// Interaction logic for UCStockCountReport.xaml
    /// </summary>
    public partial class UCStockCountReport : UserControl
    {
        Logger objLog = new Logger();
        DataTable dt = new DataTable();
        DataTable _dtBindList = null;
        WriteLogFile ObjLog = new WriteLogFile();
        public UCStockCountReport()
        {
            InitializeComponent();
        }

        #region Private collection
        PL_Reports _objPLPostToSAP = null;
        ObservableCollection<PL_Reports> _PLReports = new ObservableCollection<PL_Reports>();
        public ObservableCollection<PL_Reports> PLReports
        {
            get { return _PLReports; }
            set
            {
                _PLReports = value;
                OnPropertyChanged("PLReports");
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
            CultureInfo ci = CultureInfo.CreateSpecificCulture(CultureInfo.CurrentCulture.Name);
            ci.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            Thread.CurrentThread.CurrentCulture = ci;
            dtpTodate.SelectedDate = DateTime.Now.Date;
            //dtpTodate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private bool ValidateInputs(bool p)
        {
            try
            {
                if (string.IsNullOrEmpty(dtpFromdate.Text))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Please Select From Date", 1);
                    this.dtpFromdate.Focus();
                    return p = false;
                }
                if (string.IsNullOrEmpty(dtpTodate.Text))
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "Please Select To Date", 1);
                    this.dtpTodate.Focus();
                    return p = false;
                }
            }
            catch (Exception ex)
            {
            }
            return p;
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

        private void btngetDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool sInputs = ValidateInputs(true);
                if (sInputs != false)
                {
                    PL_Reports _objlm = new PL_Reports();
                    DateTime fromdate = DateTime.Parse(Convert.ToDateTime(dtpFromdate.Text).ToShortDateString());
                    DateTime todate = DateTime.Parse(Convert.ToDateTime(dtpTodate.Text).ToShortDateString());
                    if (fromdate > todate)
                    {
                        BCommon.setMessageBox(VariableInfo.mApp, "Fromdate is Greater Then Todate, Kindly Select Date Between Range", 1);
                        return;
                    }
                    else
                    {
                        PL_Reports _objPLReport = new PL_Reports();
                        _objPLReport.FromDate = dtpFromdate.SelectedDate.ToString();
                        _objPLReport.ToDate = dtpTodate.SelectedDate.ToString();
                        DisplayData(_objPLReport);
                        //FilteredData();
                    }
                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "StockCountReport => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
                btngetDetails.Cursor = Cursors.Arrow;
            }
        }

        private void DisplayData(PL_Reports _PLPostToSAP)
        {
            try
            {
                PLReports = new BUSSINESS_LAYER.BL_Reports().BLStockCountReportData(_PLPostToSAP);
                //lblTotalnoofrows.Content = PLReorts.Count.ToString();
                dgShowData.ItemsSource = PLReports;
                if (dgShowData.Items.Count == 0)
                {
                    BCommon.setMessageBox(VariableInfo.mApp, "No Records Found to Show", 1);
                    return;
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "StockCountReport => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }

        }

        private void btnExport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                btnExport.Cursor = Cursors.Wait;
                if (dgShowData.Items.Count > 0)
                {
                    MessageBoxResult MessResult1 = MessageBox.Show("Do You Really Want To Export Report?", "Export Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
                    if (MessResult1 == MessageBoxResult.No)
                    { return; }
                    btnExport.Cursor = Cursors.Wait;
                    ReportTypes();
                }
                else
                    BCommon.setMessageBox("Stock Count Report", " No Data available For Export", 2);
            }
            catch (Exception ex)
            {
                btnExport.Cursor = Cursors.Arrow;
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "StockCountReport => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        public void ReportTypes()
        {
            try
            {
                _dtBindList = new DataTable();
                ObservableCollection<PL_Reports> data = (ObservableCollection<PL_Reports>)dgShowData.ItemsSource;
                _dtBindList = VariableInfo.ToDataTable(data);

                #region ReportType2

                _dtBindList.DefaultView.ToTable(true, "MaterialCode", "MaterialDescription", "PlantCode", "Quantity", "SerialNo", "Createdon");
                _dtBindList.Columns.Remove("FromDate");
                _dtBindList.Columns.Remove("ToDate");
                _dtBindList.Columns.Remove("TotalQty");
                _dtBindList.Columns.Remove("IsValid");
                _dtBindList.Columns.Remove("CreatedBy");
                _dtBindList.Columns.Remove("Quantity");
                _dtBindList.Columns.Remove("SerialNo");
                _dtBindList.Columns.Remove("MatStatus");
                _dtBindList.Columns["PlantCode"].SetOrdinal(0);
                _dtBindList.Columns["MaterialCode"].SetOrdinal(1);
                _dtBindList.Columns["MaterialDescription"].SetOrdinal(2);
                _dtBindList.Columns["QRCode"].SetOrdinal(3);
                _dtBindList.Columns["StackQRCode"].SetOrdinal(4);
                _dtBindList.Columns["CreatedOn"].SetOrdinal(5);
                _dtBindList.Columns["MaterialCode"].ColumnName = "Material Code";
                _dtBindList.Columns["MaterialDescription"].ColumnName = "Material Description";
                _dtBindList.Columns["StackQRCode"].ColumnName = "Stack QRCode";
                _dtBindList.Columns["CreatedOn"].ColumnName = "Posting Date";
                if (BCommon.ExportToCSVFromDataTable(_dtBindList, "", "CSV", "StockCountReport"))
                {
                    btnExport.Cursor = Cursors.Arrow;
                    Clear();
                }
                else
                {
                    BCommon.setMessageBox("", " No Data available For Export", 2);
                    btnExport.Cursor = Cursors.Arrow;
                }

                #endregion
            }
            catch (Exception ex)
            {
                btnExport.Cursor = Cursors.Arrow;
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                ObjLog.WriteLog(" (Error) - " + "StockCountReport => " + exDetail.ToString());
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult MessResult = MessageBox.Show("Do You  Want To Clear All Details?", "Clear Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);
            if (MessResult == MessageBoxResult.No)
            { return; }
            Clear();
        }

        private void Clear()
        {
            dgShowData.ItemsSource = null;
            dtpFromdate.SelectedDate = DateTime.Today;
            dtpTodate.SelectedDate = DateTime.Today;
            //FilteredData();
            //lblTotalnoofrows.Content = "***";
        }
    }
}
