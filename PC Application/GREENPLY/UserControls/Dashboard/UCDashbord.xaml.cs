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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data;
using System.Threading;
using System.Windows.Threading;
using System.Reflection;
using COMMON_LAYER;
using System.Collections;

namespace GREENPLY.UserControls.Dashboard
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UCDashboard : UserControl
    {  
        public UCDashboard()
        {
            InitializeComponent();   
        }
    
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {   
            try
            {
                BindComboBox();
                setTimerforView();      
                Start();
                binddata();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void Start()
        {      
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 5, 0);
            dispatcherTimer.Start();          
        }
       
        private void binddata()
        {
            DataTable dt = new DataTable();
            DataTable dt2 = new DataTable();          
      
            dt = new BUSSINESS_LAYER.BLDashboard().BLPickingStatus();
            dt2 = new BUSSINESS_LAYER.BLDashboard().BLReceivingStatus();

                Xgrid.ItemsSource = dt.DefaultView;
                rsgrid.ItemsSource = dt2.DefaultView;
                rsgrid.CanUserAddRows = false;
                //mcChart.DataContext = dt2.DefaultView;
                //PieCustomerChart.DataContext = dt.DefaultView;
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            binddata();           
        }

        private DispatcherTimer timerView;
        DateTime RefreshTime;
        private void setTimerforView()
        {
            timerView = new DispatcherTimer();
            timerView.Tick += new EventHandler(timerView_Tick);
            timerView.Interval = new TimeSpan(0, 0, 1);
            timerView.Start();
        }

        void timerView_Tick(object sender, EventArgs e)
        {
            try
            {
                TimeSpan ts = (DateTime.Now - RefreshTime);
                lbl_timer.Content  = "Refresh in : " +" " + ts.Minutes + ":" + ts.Seconds;
            }
            catch (Exception ex)
            {
                timerView.Stop();
                lbl_timer.Content = "";        
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

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void cmbtop_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbtop.SelectedIndex!=-1)
            {
                VariableInfo.topitem = Convert.ToInt32(cmbtop.SelectedValue);       
            }
            binddata();
        }
        private void BindComboBox()
        {
            ArrayList list = new ArrayList();
            list.Add("1");
            list.Add("2");
            list.Add("3");
            list.Add("4");
            list.Add("5");
            list.Add("6");
            list.Add("7");
            list.Add("8");
            list.Add("9");
            list.Add("10");
            cmbtop.ItemsSource = list;
        }
    }
}
