using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.Linq;
using System.Text;
using System.Windows.Forms;
using BCILCommServer;
using System.Data.SqlClient;
using System.IO;
using System.Diagnostics;
using System.Data.OracleClient;

namespace GreenplyCommServer
{
    class ServerProcess
    {
        LogFile log;
        public ServerProcess(LogFile _log)
        {
            this.log = _log;
        }
    }
}
