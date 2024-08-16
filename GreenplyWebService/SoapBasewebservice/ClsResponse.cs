using BCIL.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GreenplyWebService
{
    
    public class ClsMatMasterResponse
    {
        public ClsMatMasterResponse() { }
        public ClsMatMasterResponse(string matcode, string msg)
        {
            MatCode = matcode;
            Message = msg;
        }
        public string MatCode { get; set; }
        public string Message { get; set; }

    }

    public class clsResponseMatList
    {
        public List<ClsMatMasterResponse> ResopseMatMaster { get; set; }
    }


    public class ClsRejMasterResponse
    {
        public ClsRejMasterResponse() { }
        public ClsRejMasterResponse(string matcode, string msg)
        {
            RejCode = matcode;
            Message = msg;
        }
        public string RejCode { get; set; }
        public string Message { get; set; }

    }

    public class clsResponseRejList
    {
        public List<ClsRejMasterResponse> ResopseRejMaster { get; set; }
    }


    public class ClsPurchaseOrderResponse
    {
        public ClsPurchaseOrderResponse() { }
        public ClsPurchaseOrderResponse(string PurchaseOrderNo, string msg)
        {
            PurchaseOrderNumber = PurchaseOrderNo;
            Message = msg;
        }
        public string PurchaseOrderNumber { get; set; }
        public string Message { get; set; }

    }

    public class clsResponsePurchaseOrderList
    {
        public List<ClsPurchaseOrderResponse> ResponsePurchaseOrderData { get; set; }
    }


    public class ClsPurchaseOrderReturnResponse
    {
        public ClsPurchaseOrderReturnResponse() { }
        public ClsPurchaseOrderReturnResponse(string PurchaseOrderNo, string msg)
        {
            PurchaseOrderNumber = PurchaseOrderNo;
            Message = msg;
        }
        public string PurchaseOrderNumber { get; set; }
        public string Message { get; set; }

    }

    public class clsResponsePurchaseOrderReturnList
    {
        public List<ClsPurchaseOrderReturnResponse> ResponsePurchaseOrderReturnData { get; set; }
    }


    public class ClsDeliveryOrderResponse
    {
        public ClsDeliveryOrderResponse() { }
        public ClsDeliveryOrderResponse(string DeliveryOrderNo, string msg)
        {
            DeliveryOrderNumber = DeliveryOrderNo;
            Message = msg;
        }
        public string DeliveryOrderNumber { get; set; }
        public string Message { get; set; }

    }

    public class ClsQuanlityInspectionResponse
    {
        public ClsQuanlityInspectionResponse() { }
        public ClsQuanlityInspectionResponse(string PONo, string msg)
        {
            PONumber = PONo;
            Message = msg;
        }
        public string PONumber { get; set; }
        public string Message { get; set; }

    }

    public class clsResponseDeliveryOrderList
    {
        public List<ClsDeliveryOrderResponse> ResponseDeliveryOrderData { get; set; }
    }

    public class clsResponseQuanlityInspectionList
    {
        public List<ClsQuanlityInspectionResponse> ResponseQuanlityInspectionData { get; set; }
    }

    public class ClsSalesReturnResponse
    {
        public ClsSalesReturnResponse() { }
        public ClsSalesReturnResponse(string SalesReturnNo, string msg)
        {
            SalesReturnNumber = SalesReturnNo;
            Message = msg;
        }
        public string SalesReturnNumber { get; set; }
        public string Message { get; set; }

    }

    public class clsResponseSalesReturnList
    {
        public List<ClsSalesReturnResponse> ResponseSalesReturnData { get; set; }
    }

}