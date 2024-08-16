using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Threading;
using GREENPLY.Classes;

namespace GREENPLY
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    { // give the mutex a  unique name
        private const string MutexName = "GreenPly Application";

        // declare the mutex
        private readonly Mutex _mutex;

        bool createdNew;

        // overload the constructor
        public App()
        {
            _mutex = new Mutex(true, MutexName, out createdNew);

            if (!createdNew)
            {
                MessageBox.Show("This program is already running");
                Application.Current.Shutdown(0);
            }

        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {
                if (!createdNew) return;

                if (BCommon.ReadParameter())
                {
                    StartingWindow _StartingWindow = new StartingWindow();
                    _StartingWindow.Show();
                }
                else
                {
                    WinDbSetting UIDBConfig = new WinDbSetting();
                    UIDBConfig.ShowDialog();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
