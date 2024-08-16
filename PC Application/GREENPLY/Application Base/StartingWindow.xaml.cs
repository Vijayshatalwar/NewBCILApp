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


namespace GREENPLY 
{
    /// <summary>
    /// Interaction logic for StartingWindow.xaml
    /// </summary>
    public partial class StartingWindow : Window
    {
        public StartingWindow()
        {
            this.Cursor = Cursors.Wait;
            InitializeComponent();
            this.Cursor = Cursors.Arrow;
            txtVersion.Text = "Version : 3.0.1"; //+ Convert.ToString(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void Storyboard_Completed(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Wait;
            WinLogin ObjLoginWindow = new WinLogin();
            this.Hide();
            //this.Dispatcher.InvokeShutdown();
            this.Cursor = Cursors.Arrow;
            ObjLoginWindow.ShowDialog();
            ObjLoginWindow = null;
            //MainWindow objMainWindow = new MainWindow();
            //objMainWindow.ShowDialog();
            //objMainWindow = null;
           
        }
    }
}
