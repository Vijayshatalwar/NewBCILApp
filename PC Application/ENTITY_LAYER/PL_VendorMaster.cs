using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ENTITY_LAYER
{
    public class PL_VendorMaster
    {
        public bool IsValid { get; set; }
        public string VendorId { get; set; }
        public string VendorDesc { get; set; }
        public string VendorEmail { get; set; }
        public string VendorAdd { get; set; }
        public string VendorPwd { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }

        public string PONumber { get; set; }
        public string MatCode { get; set; }
        public string MatDesc { get; set; }
        public string MatSize { get; set; }
        public string Category { get; set; }
        public string MatGroup { get; set; }
        public string MatThickness { get; set; }
        public string MatGrade { get; set; }
        public int POQty { get; set; }
        public int PrintedQty { get; set; }
        public int PrintableQty { get; set; }
        public int RemaningQty { get; set; }
        public string QRCode { get; set; }
        public string StackQRCCode { get; set; }
        public string InvoiceNo { get; set; }
        public string InvoiceDate { get; set; }
        public string DateFormat { get; set; }
        public int GeneratedQty { get; set; }
        public string PrintingLocationType { get; set; }
        public string PrintingSection { get; set; }
        public string MatGroupDesc { get; set; }
    }
}
