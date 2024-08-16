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
    public class ClsGetQualityInspectionData
    {
        public string tLOCATIONCODE { get; set; }

        public string tPONO { get; set; }

        public string tMATCODE { get; set; }

        public string tQRCODE { get; set; }

        public string tMIGONO { get; set; }

        public string tINSPECTIONLOTNO { get; set; }

        public string tQCSTATUS { get; set; }

        public string tQCBY { get; set; }

        public string tQCON { get; set; }
    }


}