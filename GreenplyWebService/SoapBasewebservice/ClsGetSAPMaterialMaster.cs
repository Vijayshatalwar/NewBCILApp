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
    public class ClsGetSAPMaterialMaster
    {
        //public string mPLANT { get; set; }

        public string mPRODUCT { get; set; }

        public string mMATCODE { get; set; }

        public string mMATDESC { get; set; }

        public string mTHICKNESS { get; set; }

        public string mTHICKNESSDESC { get; set; }

        public string mSIZE { get; set; }

        public string mGRADE { get; set; }

        public string mGRADEDESC { get; set; }

        public string mCATEGORY { get; set; }

        public string mCATEGORYDESC { get; set; }

        public string mMATGROUP { get; set; }

        public string mMATGROUPDESC { get; set; }

        public string mDESIGNNO { get; set; }

        public string mDESIGNDESC { get; set; }

        public string mFINISHCODE { get; set; }

        public string mFINISHDESC { get; set; }

        public string mVISIONPANELCODE { get; set; }

        public string mVISIONPANELDESC { get; set; }

        public string mLIPPINGCODE { get; set; }

        public string mLIPPINGDESC { get; set; }

        public string mUOM { get; set; }
    }
}