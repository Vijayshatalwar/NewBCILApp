using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ENTITY_LAYER
{
    public class PLPostToSAP
    {
        public string PostingDate { get; set; }
        public string ToDate { get; set; }
        public string MaterialCode { get; set; }
        public string SerialNo { get; set; }
        public string Barcode { get; set; }
        public string VisualBarcode { get; set; }
        public string MaterialDescription { get; set; }
        public string PlantCode { get; set; }
        public string Createdon { get; set; }
        public string Createdby { get; set; }
        public bool IsValid { get; set; }
        public int Quantity { get; set; }
        public int TotalQty { get; set; }
        
    }
}
