using BCIL;
using Csla.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Csla;
using BCIL.Utility;
using System.Data.SqlClient;
using System.IO;

namespace GreenplyWebService
{
    public class ClsGetSalesReturnData
    {
        public string tLOCATIONCODE { get; set; }

        public string tSALESRETURNNO { get; set; }

        public string tCUSTOMERCODE { get; set; }

        public string tCUSTOMERNAME { get; set; }

        public string tMATCODE { get; set; }

        public string tMATDESC { get; set; }

        public string tRETURNQTY { get; set; }

    }
}