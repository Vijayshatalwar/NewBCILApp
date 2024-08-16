using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using GreenplyScannerCommServer;

namespace GreenplyScannerCommServer.Common
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
                    // _Logger.LogMessage(EventNotice.EventTypes.evtInfo,"_InitializeSockClient", "Initialze Socket Connection Failed........."  );                            
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
                _sReturn = "PRINTER READY";
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
