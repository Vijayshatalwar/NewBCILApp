using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataScheduler
{

    #region Masters

    public class PLmMaterialMaster
    {
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
        public string DownloadOn { get; set; }
        public string DownloadBy { get; set; }

        public string lDownloadedBy { get; set; }
        public string lstatus { get; set; }
        public string Lmsg { get; set; }
        public string sTime { get; set; }
        public string TimeStamp { get; set; }
        public string LastUpadteddate { get; set; }
    }

    public class PLmUserMaster
    {
        public string LOCATION_TYPE { get; set; }
        public string LOCATION_CODE { get; set; }
        public string USER_ID { get; set; }
        public string USER_NAME { get; set; }
        public string PASSWORD { get; set; }
        public string USER_EMAIL { get; set; }
        public string GROUPID { get; set; }
        public string ACTIVE { get; set; }
        public string USER_TYPE { get; set; }
        public string CREATED_BY { get; set; }
        public string CREATED_ON { get; set; }
        public string MODIFIED_BY { get; set; }
        public string MODIFIED_ON { get; set; }
        public string lDownloadedBy { get; set; }
        public string lstatus { get; set; }
        public string Lmsg { get; set; }
        public string sTime { get; set; }
        public string TimeStamp { get; set; }
        public string LastUpadteddate { get; set; }
    }

    public class PLGroupRights
    {
        public string GROUP_ID { get; set; }
        public string MODULE_ID { get; set; }
        public string VIEW_RIGHTS { get; set; }
        public string SAVE_RIGHTS { get; set; }
        public string EDIT_RIGHTS { get; set; }
        public string DELETE_RIGHTS { get; set; }
        public string DOWNLOAD_RIGHTS { get; set; }

        public string lDownloadedBy { get; set; }
        public string lstatus { get; set; }
        public string Lmsg { get; set; }
        public string sTime { get; set; }
        public string TimeStamp { get; set; }
        public string LastUpadteddate { get; set; }
    }

    public class PLmVendorMaster
    {
        public string LocationCode { get; set; }
        public string VendorCode { get; set; }
        public string VendorDesc { get; set; }
        public string VendorEmail { get; set; }
        public string VendorAddress { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
        public string lDownloadedBy { get; set; }
        public string lstatus { get; set; }
        public string Lmsg { get; set; }
        public string sTime { get; set; }
        public string TimeStamp { get; set; }
        public string LastUpadteddate { get; set; }
    }

    public class PLmEmailConfigMaster
    {
        public string LocationCode { get; set; }
        public string SMTPHost { get; set; }
        public string EmailID { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string PortNo { get; set; }
        public string Subject { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }

        public string Lmsg { get; set; }
        public string sTime { get; set; }
        public string TimeStamp { get; set; }
        public string LastUpadteddate { get; set; }
    }

    public class PLGroupMaster
    {
        public string GroupId { get; set; }
        public string GroupName { get; set; }
        public string GroupType { get; set; }

        public string lDownloadedBy { get; set; }
        public string lstatus { get; set; }
        public string Lmsg { get; set; }
        public string sTime { get; set; }
        public string TimeStamp { get; set; }
        public string LastUpadteddate { get; set; }
    }

    public class PLModuleMaster
    {
        public string ModuleID { get; set; }
        public string ModuleName { get; set; }

        public string lDownloadedBy { get; set; }
        public string lstatus { get; set; }
        public string Lmsg { get; set; }
        public string sTime { get; set; }
        public string TimeStamp { get; set; }
        public string LastUpadteddate { get; set; }
    }

    #endregion



    #region Transaction

    public class PLLocationLabelPrinting
    {
        public string LocationCode { get; set; }
        public string MatCode { get; set; }
        public string MatDesc { get; set; }
        public string MatGrade { get; set; }
        public string MatGroup { get; set; }
        public string MatThickness { get; set; }
        public string MatSize { get; set; }
        public string QRCode { get; set; }
        public string StackQRCode { get; set; }
        public string IsStackPrinted { get; set; }
        public string MatStatus { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string QRCodeScanBy { get; set; }
        public string QRCodeScanOn { get; set; }
        public string MTMOldQRCode { get; set; }
        public string MTMTransferBy { get; set; }
        public string MTMTransferOn { get; set; }
        public bool IsStockTake { get; set; }
        public string StockTakeBy { get; set; }
        public string StockTakeOn { get; set; }
        public string SentBy { get; set; }
        public string SentOn { get; set; }

        public string lDownloadedBy { get; set; }
        public string lstatus { get; set; }
        public string Lmsg { get; set; }
        public string sTime { get; set; }
        public string TimeStamp { get; set; }
        public string LastUpadteddate { get; set; }
    }

    public class PLSAPPurchaseOrderData
    {
        public string LocationCode { get; set; }
        public string PONumber { get; set; }
        public string VendorCode { get; set; }
        public string MatCode { get; set; }
        public string MatDescription { get; set; }
        public string MatThickness { get; set; }
        public string MatSize { get; set; }
        public string MatGrade { get; set; }
        public string Category { get; set; }
        public string MatGroup { get; set; }
        public int POQty { get; set; }
        public string PODate { get; set; }
        public string IsQRCodeGenerated { get; set; }
        public string GeneratedQty { get; set; }
        public string POStatus { get; set; }
        public string GeneratedBy { get; set; }
        public string GeneratedOn { get; set; }
        public string IsPrinted { get; set; }
        public int PrintedQty { get; set; }
        public string PrintedBy { get; set; }
        public string PrintedOn { get; set; }
        public string IsReprinted { get; set; }
        public int RePrintedQty { get; set; }
        public string RePrintedBy { get; set; }
        public string RePrintedOn { get; set; }
        public string DownloadBy { get; set; }
        public string DownloadOn { get; set; }

        public string lDownloadedBy { get; set; }
        public string lstatus { get; set; }
        public string Lmsg { get; set; }
        public string sTime { get; set; }
        public string TimeStamp { get; set; }
        public string LastUpadteddate { get; set; }
    }

    public class PLSAPDeliveryOrderData
    {
        public string LocationCode { get; set; }
        public string DeliveryOrderNo { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string MatCode { get; set; }
        public string MatDesc { get; set; }
        public string ToLocationCode { get; set; }
        public int DOQty { get; set; }
        public string DODate { get; set; }
        public string DOStatus { get; set; }
        public string DownloadOn { get; set; }
        public string DownloadBy { get; set; }
        public string IsSAPPosted { get; set; }
        public string SAPPostedQty { get; set; }
        public string PostedBy { get; set; }
        public string PostedOn { get; set; }

        public string lDownloadedBy { get; set; }
        public string lstatus { get; set; }
        public string Lmsg { get; set; }
        public string sTime { get; set; }
        public string TimeStamp { get; set; }
        public string LastUpadteddate { get; set; }
    }

    public class PLSAPSalesReturnData
    {
        public string LocationCode { get; set; }
        public string SalesReturnNo { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string MatCode { get; set; }
        public string MatDesc { get; set; }
        public int ReturnQty { get; set; }
        public int ScannedQty { get; set; }
        public string ReturnStatus { get; set; }
        public string DownloadOn { get; set; }
        public string DownloadBy { get; set; }

        public string lDownloadedBy { get; set; }
        public string lstatus { get; set; }
        public string Lmsg { get; set; }
        public string sTime { get; set; }
        public string TimeStamp { get; set; }
        public string LastUpadteddate { get; set; }
    }

    public class PLtVendorLabelPrinting
    {
        public string LocationCode { get; set; }
        public string PONumber { get; set; }
        public string MatCode { get; set; }
        public string MatDesc { get; set; }
        public string MatGrade { get; set; }
        public string MatGroup { get; set; }
        public string MatThickness { get; set; }
        public string MatSize { get; set; }
        public string VendorCode { get; set; }
        public string VendorInvoice { get; set; }
        public string VendorInvDate { get; set; }
        public string QRCode { get; set; }
        public string StackQRCode { get; set; }
        public string IsStackGenerated { get; set; }
        public string IsStackGeneratedBy { get; set; }
        public string GeneratedBy { get; set; }
        public string GeneratedOn { get; set; }
        public string IsQRCodePrinted { get; set; }
        public string IsStackPrinted { get; set; }
        public string PrintedBy { get; set; }
        public string PrintedOn { get; set; }
        public string IsRePrintRequest { get; set; }
        public string RequestedBy { get; set; }
        public string RequestedOn { get; set; }
        public string IsRePrinted { get; set; }
        public string RePrintedBy { get; set; }
        public string RePrintedOn { get; set; }
        public string IsSAPPosted { get; set; }
        public string SAPPostMsg { get; set; }
        public string PostedBy { get; set; }
        public string PostedOn { get; set; }

        public string lDownloadedBy { get; set; }
        public string lstatus { get; set; }
        public string Lmsg { get; set; }
        public string sTime { get; set; }
        public string TimeStamp { get; set; }
        public string LastUpadteddate { get; set; }
    }

    public class PLtStockCount
    {
        public string LocationCode { get; set; }
        public string MatCode { get; set; }
        public string QRCode { get; set; }
        public string StackQRCode { get; set; }
        public string StockOn { get; set; }
        public string StockBy { get; set; }
        public string IsSAPPosted { get; set; }
        public string PostedBy { get; set; }
        public string PostedOn { get; set; }

        public string lDownloadedBy { get; set; }
        public string lstatus { get; set; }
        public string Lmsg { get; set; }
        public string sTime { get; set; }
        public string TimeStamp { get; set; }
        public string LastUpadteddate { get; set; }
    }

    public class PLtSalesReturn
    {
        public string LocationCode { get; set; }
        public string SalesReturnNo { get; set; }
        public string MatCode { get; set; }
        public string MatDesc { get; set; }
        public int Qty { get; set; }
        public string QRCode { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string IsQRCodePrinted { get; set; }
        public string PrintedOn { get; set; }
        public string PrintedBy { get; set; }
        public string IsSAPPosted { get; set; }
        public string SAPPostMsg { get; set; }
        public string PostedOn { get; set; }
        public string PostedBy { get; set; }

        public string lDownloadedBy { get; set; }
        public string lstatus { get; set; }
        public string Lmsg { get; set; }
        public string sTime { get; set; }
        public string TimeStamp { get; set; }
        public string LastUpadteddate { get; set; }
    }

    public class PLtDispatchData
    {
        public string LocationCode { get; set; }
        public string DONo { get; set; }
        public string MatCode { get; set; }
        public int Qty { get; set; }
        public string QRCode { get; set; }
        public string StackQRCode { get; set; }
        public string Status { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public string IsSAPPosted { get; set; }
        public string SAPPostMsg { get; set; }
        public string PostedOn { get; set; }
        public string PostedBy { get; set; }
        public string SentOn { get; set; }
        public string SentBy { get; set; }

        public string lDownloadedBy { get; set; }
        public string lstatus { get; set; }
        public string Lmsg { get; set; }
        public string sTime { get; set; }
        public string TimeStamp { get; set; }
        public string LastUpadteddate { get; set; }
    }

    public class PLtItemSerial
    {
        public string LocationCode { get; set; }
        public string DateFormat { get; set; }
        public string QRCodeSerialNo { get; set; }
        public string StackSerialNo { get; set; }
        public string PrintType { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }

        public string lDownloadedBy { get; set; }
        public string lstatus { get; set; }
        public string Lmsg { get; set; }
        public string sTime { get; set; }
        public string TimeStamp { get; set; }
        public string LastUpadteddate { get; set; }
    }
    
    #endregion
}
