using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ENTITY_LAYER
{
    public class PL_TradingData
    {
        public string PONo { get; set; }
        public DateTime PODate { get; set; }
        public string VendorName { get; set; }
        public string VerticalName { get; set; }
        public string Barcode { get; set; }
        public string VisualBarcode { get; set; }
        public string MatName { get; set; }
        public string MatCode { get; set; }
        public string MatDesc { get; set; }
        public string PlantCode { get; set; }
        public string Createdon { get; set; }
        public string Createdby { get; set; }
        public int Quantity { get; set; }
        public int TotalQty { get; set; }
    }
}
