using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ENTITY_LAYER
{
    public class PL_Reports
    {
        
        public string PlantCode { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialDescription { get; set; }
        public string QRCode { get; set; }
        public string StackQRCode { get; set; }
        public string SerialNo { get; set; }
        public string MatStatus { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public bool IsValid { get; set; }
        public int Quantity { get; set; }
        public int TotalQty { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        //public DateTime FromDate { get; set; }
        //public DateTime ToDate { get; set; }

       
    }
}
