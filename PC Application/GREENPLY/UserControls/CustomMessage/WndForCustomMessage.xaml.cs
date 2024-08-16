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
using System.Reflection;
using COMMON_LAYER;
using COMMON;

namespace GREENPLY
{
    /// <summary>
    /// Interaction logic for WndForCustomMessage.xaml
    /// </summary>
    public partial class WndForCustomMessage : Window
    {
        string sMessage = string.Empty;
        string sAppName = string.Empty;
        int iType = 0;

        public WndForCustomMessage(string sMessage, string sCaption, int iType)
        {
            InitializeComponent();

            this.sMessage = sMessage;
            this.sAppName = sCaption;
            this.iType = iType;
        }

        public WndForCustomMessage(string sMessage, string sCaption)
        {
            InitializeComponent();
            this.sMessage = sMessage;
            this.sAppName = sCaption;

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (iType == 0)
                {
                    CustomMessageBox message = new CustomMessageBox(sMessage, sAppName);
                    message.ShowDialog();
                    _Result = message.Result;
                }
                else
                {
                    CustomMessageBox message = new CustomMessageBox(sMessage, sAppName, iType);
                    message.ShowDialog();
                }


                this.Close();
            }
            catch (Exception ex)
            {
              
            }

        }

        public MessageResult Result
        {
            get { return _Result; }
            set { _Result = value; }
        }
        private MessageResult _Result;

    }
}
