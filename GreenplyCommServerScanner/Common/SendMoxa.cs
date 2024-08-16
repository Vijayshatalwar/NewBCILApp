using System;
using System.Collections.Generic;
using BCILLogger;
using System.Linq;
using System.Text;
using System.Threading;
using MOXA_CSharp_MXIO;
using System.Windows.Forms;

namespace TMSServer
{
    class SendMoxa
    {
        Logger objLog1 = new Logger();

        void MoxaThread(object ob)
        {
            try
            {
                objLog1 = new Logger();
                objLog1.AppPath = Application.StartupPath + "\\MOXA2\\";
                objLog1.EnableLogging = true;
                objLog1.ErrorFileName = "MOXA2";
                objLog1.MaxFileSize = 5;
                objLog1.NoOfDaysToDeleteFile = 10;
                objLog1.WriteLog("Moxa2 communication started..");
                string[] d = ob.ToString().Split('~');
                string IP = d[0];
                string port = "0"; //d[1];
                byte bytCount = 1; // 3 channels 
                byte bytStartChannel = byte.Parse(d[1]); //
                Int32[] hConnection = new Int32[1];
                string IPAddr = IP;
                UInt32 Timeout = 5000;
                string Password = "";
                UInt16 Port = UInt16.Parse("502");
                MXIO_CS.MXEIO_Init();
                MXIO_CS.MXEIO_E1K_Connect(System.Text.Encoding.UTF8.GetBytes(IPAddr), 502, Timeout, hConnection, System.Text.Encoding.UTF8.GetBytes(Password));
                UInt32 dwSetDOValue = 1;
                int ret = MXIO_CS.E1K_DO_Writes(hConnection[0], bytStartChannel, bytCount, dwSetDOValue);
                System.Threading.Thread.Sleep(3000);
                ret = MXIO_CS.E1K_DO_Writes(hConnection[0], bytStartChannel, bytCount,0);
              
                MXIO_CS.MXEIO_Disconnect(hConnection[0]);
            }
            catch (Exception ex)
            {
                objLog1.WriteLog(ex);
            }

        }

        public void SendMoxaIO(string IP, string port)
        {
            ParameterizedThreadStart ps = new ParameterizedThreadStart(MoxaThread);
            Thread td = new Thread(ps);
            td.Start(IP + "~" + port);
        }

    }
}
