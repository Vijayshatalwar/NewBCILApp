using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Net.Sockets;

namespace GreenplyCommServer
{
    public class ClientSocket
    {
        private Socket _Sock = null;

        private bool isConnected;

        private TcpClient client = new TcpClient();

        private byte[] readBuffer = new byte[3024];

        public string ServerIP { get; set; }

        public int Port { get; set; }

        public bool IsConnected => this.client != null && this.client.Client.Connected;

        public event ClientSocket.OnConnected OnConnect;

        public event ClientSocket.OnDisConnected OnDisConnect;

        public event ClientSocket.Recieved OnRecieved;

        public event ClientSocket.SocketError OnSocketError;

        public delegate void OnConnected(int err);

        public delegate void OnDisConnected(int err);

        public delegate void Recieved(byte[] data, int len);

        public delegate void SocketError(string Msg);

        public bool Connect()
        {
            try
            {
                client = new TcpClient();
                client.Connect(this.ServerIP, this.Port);
                isConnected = this.client.Connected;
                readBuffer = new byte[client.ReceiveBufferSize];
                OnConnect(1);
            }
            catch (Exception ex)
            {
                isConnected = this.client.Connected;
                OnConnect(-1);
                return false;
            }
            client.GetStream().BeginRead(readBuffer, 0, client.ReceiveBufferSize, new AsyncCallback(StreamReceiver), (object)null);
            return true;
        }

        private void StreamReceiver(IAsyncResult ar)
        {
            byte[] numArray = new byte[client.ReceiveBufferSize];
            try
            {
                int len;
                lock (this.client.GetStream())
                    len = this.client.GetStream().EndRead(ar);
                if (len > 0)
                    this.OnRecieved(this.readBuffer, len);
                lock (this.client.GetStream())
                    this.client.GetStream().BeginRead(this.readBuffer, 0, client.ReceiveBufferSize, new AsyncCallback(this.StreamReceiver), (object)null);
            }
            catch (Exception ex)
            {
                //PCommon.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "BcilAppsInitialize" + "  ::  Data", ex.Message);
                this.OnSocketError(ex.Message);
            }
        }

        public void SendData(byte[] Data, int count)
        {
            try
            {
                lock (this.client.GetStream())
                {
                    BinaryWriter binaryWriter = new BinaryWriter((Stream)this.client.GetStream());
                    binaryWriter.Write(Data, 0, Data.Length);
                    binaryWriter.Flush();
                }
            }
            catch (SocketException ex)
            {
                this.OnSocketError(ex.Message);
            }
            catch (Exception ex)
            {
                this.OnSocketError(ex.Message);
            }
        }

        public void SendDataToPrinter(string Data)
        {
            try
            {
                lock (client.GetStream())
                {
                    StreamWriter streamWriter = new StreamWriter((Stream)client.GetStream());
                    streamWriter.Write(Data);
                    streamWriter.Flush();
                }
            }
            catch (SocketException ex)
            {
                OnSocketError(ex.Message);
            }
            catch (Exception ex)
            {
                OnSocketError(ex.Message);
            }
        }

        public bool HardwareConnected(string sIP)
        {
            bool bConnected = true;
            try
            {
                Ping ping = new Ping();
                PingReply pingresult = ping.Send(sIP);
                if (pingresult.Status.ToString() == "Success")
                {
                    bConnected = true;
                }
                else
                {
                    bConnected = false;
                }
            }
            catch (Exception)
            {
                bConnected = false;
                throw;
            }
            return bConnected;
        }

        public string CheckPrinterStatus(string _Prn)
        {
            string _sReturn = "";
            //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "NetworkPrint", _Prn);                
            byte[] _dBuffer = System.Text.Encoding.ASCII.GetBytes("");
            try
            {
                _dBuffer = System.Text.Encoding.ASCII.GetBytes(_Prn);
                _Sock.Send(_dBuffer);
                _sReturn = _Response(_Sock);
                return _sReturn;
            }
            catch (Exception ex)
            {
                _SocklientTerminate();
                return "PRINTER NOT IN NETWORK";
            }
        }

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

    }
}
