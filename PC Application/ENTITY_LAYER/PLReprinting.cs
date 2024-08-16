using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ENTITY_LAYER
{
    public class PLReprinting
    {
        public bool IsValid { get; set; }
        public string Product { get; set; }
        public string MatCode { get; set; }
        public string MatDescription { get; set; }
        public string Thickness { get; set; }
        public string ThicknessDescription { get; set; }
        public string Size { get; set; }
        public string Grade { get; set; }
        public string GradeDescription { get; set; }
        public string Category { get; set; }
        public string CategoryDescription { get; set; }
        public string MatGroup { get; set; }
        public string MatGroupDescription { get; set; }
        public string DesignNo { get; set; }
        public string DesignDescription { get; set; }
        public string FinishCode { get; set; }
        public string FinishDescription { get; set; }
        public string VisionPanelCode { get; set; }
        public string VisionPanelDescription { get; set; }
        public string LippingCode { get; set; }
        public string LippingDescription { get; set; }
        public string UOM { get; set; }
        public string QRCode { get; set; }
        public string StackQRCode { get; set; }
        public string IsSAPPosted { get; set; }
        public string QRCodeCount { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string Fromdate { get; set; }
        public string Todate { get; set; }
        public string LocationCode { get; set; }
        public string PONumber { get; set; }

        public string RejCode { get; set; }
        public string RejDescription { get; set; }
        public string BatchNo { get; set; }
    }
}
