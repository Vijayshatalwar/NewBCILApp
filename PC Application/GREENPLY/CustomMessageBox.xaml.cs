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
using System.IO;
using COMMON;
using GREENPLY;
using COMMON_LAYER;
using GREENPLY.Classes;

namespace GREENPLY
{
    /// <summary>
    /// Interaction logic for CustomMessageBox.xaml
    /// </summary>
    public partial class CustomMessageBox : Window
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sMessage"></param>
        /// <param name="sCaption"></param>
        /// <param name="iType">1 - Information, 2 - Exclamation/Warning, 3 - Error</param>
        public CustomMessageBox(string sMessage, string sCaption, int iType)
        {
            InitializeComponent();

            lblCaption.Content = sCaption;
            //lblMessage.Content= sMessage;

            rtbMessage.Document.Blocks.Clear();
            rtbMessage.AppendText(sMessage);
            (rtbMessage.Document.Blocks.FirstBlock as Paragraph).LineHeight = 20;

            ugOK.Visibility = Visibility.Visible;
            ugYesNo.Visibility = Visibility.Collapsed;

            //string root = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            //var files = Directory.GetFiles(System.IO.Path.Combine(root, "Images"), "*.*");
            switch (iType)
            {
                case 1:
                    {
                        BitmapImage ImgTest = new BitmapImage(new Uri(System.Windows.Navigation.BaseUriHelper.GetBaseUri(this), @"/GREENPLY;component/Images/Information.png"));
                        imgIcon.Source = ImgTest;
                        break;
                    }
                case 2:
                    {
                        BitmapImage ImgTest = new BitmapImage(new Uri(System.Windows.Navigation.BaseUriHelper.GetBaseUri(this), @"/GREENPLY;component/Images/Exclamation.png"));
                        imgIcon.Source = ImgTest;
                        break;
                    }
                case 3:
                    {
                        BitmapImage ImgTest = new BitmapImage(new Uri(System.Windows.Navigation.BaseUriHelper.GetBaseUri(this), @"/GREENPLY;component/Images/error.png"));
                        imgIcon.Source = ImgTest;
                        break;
                    }                case 4:
                    {
                        BitmapImage ImgTest = new BitmapImage(new Uri(System.Windows.Navigation.BaseUriHelper.GetBaseUri(this), @"/GREENPLY;component/Images/tick.png"));
                        imgIcon.Source = ImgTest;
                        break;
                    }
            }
        }

        public CustomMessageBox(string sMessage, string sCaption)
        {
            InitializeComponent();

            lblCaption.Content = sCaption;
            //lblMessage.Content = sMessage;
            rtbMessage.Document.Blocks.Clear();
            rtbMessage.AppendText(sMessage);
            (rtbMessage.Document.Blocks.FirstBlock as Paragraph).LineHeight = 20;

            BitmapImage ImgTest = new BitmapImage(new Uri(System.Windows.Navigation.BaseUriHelper.GetBaseUri(this), @"Image\Question-mark-4.png"));
            imgIcon.Source = ImgTest;

            ugOK.Visibility = Visibility.Collapsed;
            ugYesNo.Visibility = Visibility.Visible;
        }



        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                 
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter || e.Key == Key.Escape)
                {
                    btnOk_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                 
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _Result = MessageResult.Yes;
                this.Close();
            }
            catch (Exception ex)
            {
                 
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
            }
        }

        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _Result = MessageResult.No;
                this.Close();
            }
            catch (Exception ex)
            {
                 
                BCommon.setMessageBox(VariableInfo.mApp, ex.Message, 3);
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
