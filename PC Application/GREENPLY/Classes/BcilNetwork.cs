using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace GREENPLY.Classes
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
        private string _MoxaIp = "";
        private string _MoxaPort = "";
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
        public string MoxaIP
        {
            get { return _MoxaIp; }
            set { _MoxaIp = value; }
        }
        public string MoxaPort
        {
            get { return _MoxaPort; }
            set { _MoxaPort = value; }
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
                    //_Logger.LogMessage(EventNotice.EventTypes.evtInfo,"_InitializeSockClient", "Initialze Socket Connection Failed........."  );                            
                    return false;
                }
            }
            catch (Exception ex)
            {
                //_Logger.LogMessage(EventNotice.EventTypes.evtInfo, "_InitializeSockClient", ex.Message);                                                    
                throw ex;
            }
        }

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

        public string _Response(Socket _client)
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

        public string NetworkPrint(string _Prn)
        {
            byte[] _dBuffer = System.Text.Encoding.ASCII.GetBytes("");
            try
            {
                if (_IsSockConnected() == false)
                {
                    if (_InitializeSockClient() != false)
                        return "PRINTER NOT INITIALIZE";
                }
                _dBuffer = System.Text.Encoding.ASCII.GetBytes(_Prn);
                _Sock.Send(_dBuffer);
                return "SUCCESS";
            }
            catch (Exception ex)
            {
                _SocklientTerminate();
                return "PRINTER NOT IN NETWORK";
            }
        }

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
                    else if (Convert.ToInt64(_Arr[4].Trim()) > 2000)
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
                // _Logger.LogMessage(EventNotice.EventTypes.evtError, "NetworkPrinterStatus", ex.StackTrace);                        
                _sReturn = "PRINTER NOT IN NETWORK";
            }
            return _sReturn;
        }

        #endregion

        #region "Dispose"

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



        public void Dispose()
        {
            _SocklientTerminate();
            Dispose(true);
            GC.SuppressFinalize(true);
        }

        #endregion
    

    }
}
