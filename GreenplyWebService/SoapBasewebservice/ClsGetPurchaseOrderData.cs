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
    public class ClsGetPurchaseOrderData
    {
        public string tLOCATIONCODE { get; set; }

        public string tPONUMBER { get; set; }

        public string tVENDORCODE { get; set; }

        public string tVENDORNAME { get; set; }

        public string tMATCODE { get; set; }

        public string tMATDESC { get; set; }

        public string tPOQTY { get; set; }

        public string tPODATE { get; set; }

        public string tPOLocType { get; set; }

    }

    public class ClsGetPurchaseOrderReturnData
    {
        public string tLOCATIONCODE { get; set; }

        public string tRETURNPONUMBER { get; set; }

        public string tVENDORCODE { get; set; }

        public string tVENDORNAME { get; set; }

        public string tMATCODE { get; set; }

        public string tMATDESC { get; set; }

        public string tPOReturnQTY { get; set; }

        public string tPOLocType { get; set; }

    }
}