using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GreenplyCommServer;
using BcilLib;

namespace GreenplyCommServer.Common
{
    public class BcilNetwork : IDisposable
    {
        #region "Distructor"               
        ~BcilNetwork()
        {
            Dispose(true);
        }
        #endregion

        #region Private Variables
        private string _PrinterIp = "";
        private int _PrinterPort = 9100;
        private string _Prn = "";
        private string _LogPath = @"C:\";
        private bool _LogWriter = false;
        private Socket _Sock = null;
        private IPEndPoint serverEndPoint;
        private IPAddress IPAddressServer;
        private bool _IsDisposed = false;
        #endregion

        #region Public variables
        public string PrinterIP
        {
            get { return _PrinterIp; }
            set { _PrinterIp = value; }
        }
        public int PrinterPort
        {
            get { return _PrinterPort; }
            set { _PrinterPort = value; }
        }
        public string Prn
        {
            get { return _Prn; }
            set { _Prn = value; }
        }
        public bool LogWriter
        {
            get { return _LogWriter; }
            set { _LogWriter = value; }
        }
        public string LogPath
        {
            get { return _LogPath; }
            set { _LogPath = value; }
        }
        #endregion

        #region "NetworkPrinting"
        /// <summary>
        /// INITIALISE SOCKET
        /// </summary>
        /// <returns></returns>
        bool _InitializeSockClient()
        {
            try
            {
                _Sock = null;
                IPAddressServer = IPAddress.Parse(_PrinterIp);
                serverEndPoint = new IPEndPoint(IPAddressServer, _PrinterPort);
                _Sock = new Socket(serverEndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                _Sock.Connect(serverEndPoint);
                if (_Sock.Connected)
                    return true;
                else
                {
                    //VariableInfo.mAppLog.LogMessage(EventNotice.EventTypes.evtInfo,"_InitializeSockClient", "Initialze Socket Connection Failed........."  );                            
                    return false;
                }
            }
            catch (Exception ex)
            {
                //_Logger.LogMessage(EventNotice.EventTypes.evtInfo, "_InitializeSockClient", ex.Message);                                                    
                throw ex;
            }
        }


        /// <summary>
        /// CHECK FOR SOCKET CONNECTED OR NOT
        /// </summary>
        /// <returns></returns>
        bool _IsSockConnected()
        {
            try
            {
                if (_Sock == null)
                    return false;
                if (_Sock.Connected == false)
                    return false;
                return true;
            }
            catch (System.Exception ex)
            {
                _SocklientTerminate();
                //_Logger.LogMessage(EventNotice.EventTypes.evtError, "IsSockConnected", ex.Message);                                                                            
                throw ex;
            }
        }


        /// <summary>
        /// SOCKET TERMINATE
        /// </summary>
        void _SocklientTerminate()
        {
            try
            {
                if (_Sock != null && _Sock.Connected)
                    _Sock.Close();
                _Sock = null;
            }
            catch (Exception ex)
            {
                //_Logger.LogMessage(EventNotice.EventTypes.evtError, "SocklientTerminate", ex.StackTrace);                                                                                                    
            }
        }


        /// <summary>
        /// DATA RECEIVE FROM PRINTER VIA SOCKET
        /// </summary>
        /// <param name="_client"></param>
        /// <returns></returns>
        string _Response(Socket _client)
        {
            try
            {
                Byte[] _data = new Byte[8025];
                SocketAsyncEventArgs _ar = new SocketAsyncEventArgs();
                _client.ReceiveTimeout = 5000;
                Int32 byteCount = _client.Receive(_data);
                return System.Text.Encoding.ASCII.GetString(_data, 0, byteCount);
            }
            catch (Exception ex)
            {
                _SocklientTerminate();
                throw ex;
            }
        }


        /// <summary>
        /// SEND DATA FROM SOCKET
        /// </summary>
        /// <param name="_sChkPrinterStatusFlag"></param>
        /// <returns></returns>
        public string NetworkPrint(string _Prn)
        {
            //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "NetworkPrint", _Prn); 
            string _sReturn = "";
            byte[] _dBuffer = System.Text.Encoding.ASCII.GetBytes("");
            try
            {
                //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "NetworkPrint", _IsSockConnected().ToString());  
                if (_IsSockConnected() == false)
                {
                    if (_InitializeSockClient() != true)
                        return "PRINTER NOT INITIALIZE";
                }
                _dBuffer = System.Text.Encoding.ASCII.GetBytes(_Prn);
                _Sock.Send(_dBuffer);
                //_sReturn = _Response(_Sock).ToString();
                return "SUCCESS";
            }
            catch (Exception ex)
            {
                _SocklientTerminate();
                return "PRINTER NOT IN NETWORK";
            }
        }


        //AddedByKrishKashyap for MarkemImage Printer
        public string CheckIMNetworkPrinterStatus()
        {
            string[] _Arr = null;
            string _sReturn = "";
            byte[] _dBuffer = System.Text.Encoding.ASCII.GetBytes("");
            try
            {
                if (_IsSockConnected() == false)
                {
                    if (_InitializeSockClient() != true)
                    {
                        _sReturn = "PRINTER NOT INITIALIZE";
                        VariableInfo.mAppLog.LogMessage(EventNotice.EventTypes.evtInfo, "CheckIMNetworkPrinterStatus", "InitializeSockClient : " + _sReturn);
                        return _sReturn;
                    }
                }

                string _Code = "!S100" + "\n" + "!!m.a.i.6?" + "\n";

                //!S100  -- To check all details about printer
                //!!m.a.i.6? -- Printer Ready

                _dBuffer = System.Text.Encoding.ASCII.GetBytes(_Code);
                VariableInfo.mAppLog.LogMessage(EventNotice.EventTypes.evtInfo, "CheckIMNetworkPrinterStatus", "Send Code to IMPrinter : " + "!S100\n!!m.a.i.6?\n");
                _Sock.Send(_dBuffer);
                _sReturn = _Response(_Sock);
                //_SocklientTerminate();
                //Dispose(true);
                _Arr = _sReturn.Split('\n');
                VariableInfo.mAppLog.LogMessage(EventNotice.EventTypes.evtInfo, "CheckIMNetworkPrinterStatus", "Responce Code from IMPrinter Position 0 : " + _Arr[0].ToString());
                VariableInfo.mAppLog.LogMessage(EventNotice.EventTypes.evtInfo, "CheckIMNetworkPrinterStatus", "Responce Code from IMPrinter Position 1 : " + _Arr[1].ToString());
                if (_Arr[0].Contains("1") == true)
                {
                    #region MarkemImajeReturnStstus
                    //if (_sReturn.Contains("1") == true)
                    //{
                    int index = _Arr[0].IndexOf("1");
                    if (index == 0)
                    {
                        _sReturn = "The temperature around the power unit is too hot.";
                        return _sReturn;
                    }
                    if (index == 1)
                    {
                        _sReturn = "One of the PIC processors, on the I/O board, has an erroneous behavior.";
                        return _sReturn;
                    }
                    if (index == 2)
                    {
                        _sReturn = "There is not enough RFS (Remote File System) for rendering the scalable fonts.";
                        return _sReturn;
                    }
                    if (index == 3)
                    {
                        _sReturn = "The building of the page, or rendering of a bitmap, failed.";
                        return _sReturn;
                    }
                    if (index == 4)
                    {
                        _sReturn = "The machine could not save the parameters. Concerns both the parameters in the fixed RFS area and the parameters stored in file.";
                        return _sReturn;
                    }
                    if (index == 5)
                    {
                        _sReturn = "The parameter file(s) could not be found in the sys-folder in the RFS (Remote File System).";
                        return _sReturn;
                    }
                    if (index == 6)
                    {
                        _sReturn = "No air pressure.";
                        return _sReturn;
                    }
                    if (index == 7)
                    {
                        _sReturn = "No paper.";
                        return _sReturn;
                    }
                    if (index == 8)
                    {
                        _sReturn = "No ribbon.";
                        return _sReturn;
                    }
                    if (index == 9)
                    {
                        _sReturn = "The printhead is not in print position.";
                        return _sReturn;
                    }
                    if (index == 10)
                    {
                        _sReturn = "Press the Start button in order to reset.";
                        return _sReturn;
                    }
                    if (index == 11)
                    {
                        _sReturn = "No return sensor signal when expected. The return sensor brings the applicator arm back to its home position.";
                        return _sReturn;
                    }
                    if (index == 12)
                    {
                        _sReturn = "The arm has not returned to its home position.";
                        return _sReturn;
                    }
                    if (index == 13)
                    {
                        _sReturn = "The arm is not in home position when expected.";
                        return _sReturn;
                    }
                    if (index == 14)
                    {
                        _sReturn = "The barcode scan failed. The barcode scan did not succeed before the barcode reader timeout, or a new print and apply cycle was started before the barcode reader timeout.";
                        return _sReturn;
                    }
                    if (index == 15)
                    {
                        _sReturn = "Label on grid when not expected.";
                        return _sReturn;
                    }
                    if (index == 16)
                    {
                        _sReturn = "No label on grid when expected.";
                        return _sReturn;
                    }
                    if (index == 17)
                    {
                        _sReturn = "The ribbon cover is open.";
                        return _sReturn;
                    }
                    if (index == 18)
                    {
                        _sReturn = "The system is paused.";
                        return _sReturn;
                    }
                    if (index == 19)
                    {
                        _sReturn = "Running out of paper.";
                        return _sReturn;
                    }
                    if (index == 20)
                    {
                        _sReturn = "Running out of ribbon.";
                        return _sReturn;
                    }
                    if (index == 21)
                    {
                        _sReturn = "The barcode scan failed, but the number of no reads before stop is set to only activate a system warning.";
                        return _sReturn;
                    }
                    if (index == 22)
                    {
                        _sReturn = "The printout is not finished when the apply cycle was supposed to start. That is, printout finished before the ApplyDelay timeout.";
                        return _sReturn;
                    }
                    if (index == 23)
                    {
                        _sReturn = "The date has been updated. This warning has to be acknowledged in order to update the printed date information.";
                        return _sReturn;
                    }
                    if (index == 24)
                    {
                        _sReturn = "The matchcode function failed. The printed barcode does not match the scanned barcode.";
                        return _sReturn;
                    }
                    if (index == 25)
                    {
                        _sReturn = "The barcode quality function failed. The printed barcode is of poor quality.";
                        return _sReturn;
                    }
                    if (index == 26)
                    {
                        _sReturn = "The machine has received the pre trig and is waiting for the main trig.Information message when sequence two is enabled.";
                        return _sReturn;
                    }
                    if (index == 30)
                    {
                        _sReturn = "Running out of paper.";
                        return _sReturn;
                    }
                    else
                    {
                        _sReturn = "The responce is in between 26 to 32 index";
                        return _sReturn;
                    }
                    //}
                    #endregion
                }
                //byte[] _dBuffer1 = System.Text.Encoding.ASCII.GetBytes("");
                //string Code = "!!m.a.i.6?";
                //_dBuffer1 = System.Text.Encoding.ASCII.GetBytes(Code);
                //VariableInfo.mAppLog.LogMessage(EventNotice.EventTypes.evtInfo, "CheckIMNetworkPrinterStatus", "Send Code to IMPrinter : " + Code);

                ////if (_Sock.Blocking == true)
                ////{
                ////    _Sock.Blocking = false;
                ////    _Sock.Send(_dBuffer);
                ////    _Sock.Blocking = true;
                ////}

                //_Sock.Send(_dBuffer1);
                //_sReturn = _Response(_Sock);
                //VariableInfo.mAppLog.LogMessage(EventNotice.EventTypes.evtInfo, "CheckIMNetworkPrinterStatus", "Responce Code from IMPrinter : " + _sReturn);
                else if ((_Arr[1].Contains("m.a.i.6!0") == true ) || (_Arr[1].Contains("m.a.i.6!1") == true))   //if (_sReturn.Contains("m.a.i.6!0"))
                {
                    _sReturn = "PRINTER READY";
                    return _sReturn;
                }
                //else //if (_sReturn.Contains("m.a.i.3!1"))
                //{
                //    _sReturn = "PRINTER READY";
                //    return _sReturn;
                //}
                else
                {
                    _sReturn = "PRINTER READY";
                    return _sReturn;
                }
                //return _sReturn;
            }
            catch (Exception ex)
            {
                _SocklientTerminate();
                VariableInfo.mAppLog.LogMessage(EventNotice.EventTypes.evtInfo, "CheckIMNetworkPrinterStatus", "Exception Received from IMPrinter : " + ex.Message.ToString());
                _sReturn = "PRINTER NOT IN AVAILABLE MODE/ OR PRINTER IS IN READY MODE/ OR PLEASE CHECK THE PRINTER IS ON & NETWORK/ PLEASE PRINT ONE LABEL";
            }
            return _sReturn;
        }

        //Ended


        /// <summary>
        /// Get Network Printer Status
        /// </summary>
        /// <returns></returns>
        public string NetworkPrinterStatus()
        {
            string[] _Arr = null;
            string _sReturn = "";
            byte[] _dBuffer = System.Text.Encoding.ASCII.GetBytes("");
            try
            {
                if (_IsSockConnected() == false)
                {
                    if (_InitializeSockClient() != true)
                    {
                        _sReturn = "PRINTER NOT INITIALIZE";
                        return _sReturn;
                    }
                }
                _dBuffer = System.Text.Encoding.ASCII.GetBytes("~HS");
                _Sock.Send(_dBuffer);
                _sReturn = _Response(_Sock);
                _dBuffer = System.Text.Encoding.ASCII.GetBytes("");
                _Arr = _sReturn.Split(',');
                if (_Arr.Length > 14)
                {
                    if (_Arr[2].Trim() == "1")
                    {
                        if (_Arr[13].Trim() == "1" && _Arr[14].Trim() == "0")
                            _sReturn = "Printer in pause status with Head open. Please close Head then press the pause button in printer to Printer Ready.";
                        else if (_Arr[13].Trim() == "0" && _Arr[14].Trim() == "0")
                            _sReturn = "Printer in pause status with Label out. Please set label then press the pause button in printer to Printer Ready.";
                        else if (_Arr[13].Trim() == "0" && _Arr[14].Trim() == "1")
                            _sReturn = "Printer in pause status with Ribben Out. Please set Ribben then press the pause button in printer to Printer Ready.";
                        else if (_Arr[13].Trim() == "0" && _Arr[14].Trim() == "0" && _Arr[15].Trim() == "1")
                            _sReturn = "Printer in pause status with Label out and Ribben Out. Please set Ribben And/Or Label then press the pause button in printer to Printer Ready.";
                        else
                            _sReturn = "Printer Pause, Please press the pause button in printer to Printer Ready.";
                    }
                    else if (_Arr[5].Trim() == "1")
                        _sReturn = "PRINTER BUFFER FULL";
                    else if (Convert.ToInt64(_Arr[4].Trim()) > 50)
                        _sReturn = "UNUSED BIT GREATER THAN 50";
                    else
                        _sReturn = "PRINTER READY";
                }
                else
                    _sReturn = "UNKNOWN ERROR";
                //_sReturn = "PRINTER READY";
                return _sReturn;
            }
            catch (Exception ex)
            {
                _SocklientTerminate();
                _sReturn = "PRINTER NOT IN NETWORK";
            }
            return _sReturn;
        }
        #endregion

        #region "Dispose"
        /// <summary>
        /// Dispose
        /// </summary>
        /// <param name="IsDisposing"></param>
        protected virtual void Dispose(bool IsDisposing)
        {
            if (_IsDisposed)
                return;
            if (IsDisposing)
            {
                // Free any managed resources in this section
            }
            _IsDisposed = true;
        }
        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            _SocklientTerminate();
            Dispose(true);
            // Tell the garbage collector not to call the finalizer
            // since all the cleanup will already be done.
            GC.SuppressFinalize(true);
        }
        #endregion
    }
}
