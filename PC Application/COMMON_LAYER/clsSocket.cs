using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Windows;

namespace COMMON_LAYER
{
    public class clsSocket
    {
        TcpClient client;
        public string SendAndReceiveData(string Message)
        {
            string Response = "";
            try
            {
            //    client = new TcpClient();
            //    client.Connect(VariableInfo.mIPAddress, int.Parse(VariableInfo.mPortNo));
            //    byte[] bData = Encoding.ASCII.GetBytes(Message);
            //    client.Client.Send(bData);
            //    byte[] res = new byte[50000];
            //    int x = client.Client.Receive(res);
            //    Response = Encoding.ASCII.GetString(res, 0, x);
            //    client.Client.Send(Encoding.ASCII.GetBytes("quit}"));
            //    client.Close();
            }
            catch (SocketException)
            {
                Response = "NO_CONNECTION";
            }
            return Response;
        }
        public void CloseSocket()
        {
            //client = new TcpClient();
            //client.Connect(oSetting.gstrServerIP.Split('~')[0].ToString(), int.Parse(oSetting.gstrServerIP.Split('~')[1]));
        }
    }
}
