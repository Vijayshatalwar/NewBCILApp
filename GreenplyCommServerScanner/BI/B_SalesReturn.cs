using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using GreenplyScannerCommServer.Common;
using TEST;
using BCILCommServer;
using GreenplyScannerCommServer;
//using GreenplyScannerCommServer.GreenplyERPPostingService;

namespace GreenplyScannerCommServer.BI
{
    class B_SalesReturn
    {

        #region Sales Return

        internal string GetSalesReturnNumberDetails(string _sLocationCode, string _sSRNo)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSalesReturnNumberDetails - RequestDataFromAndroid => ", "LocationCode - " + _sLocationCode + ", SRNo - " + _sSRNo);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETSALESRETURNNUMBERDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@SalesReturnNo", _sSRNo),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_SalesReturn", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    _sResult = "GETSALESRETURNNUMBERDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSalesReturnNumberDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "GETSALESRETURNNUMBERDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSalesReturnNumberDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("CustomerName") && dt.Rows.Count > 1)
                {
                    _sResult = "GETSALESRETURNNUMBERDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString(dt);
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSalesReturnNumberDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("CustomerName") && dt.Rows.Count == 1)
                {
                    _sResult = "GETSALESRETURNNUMBERDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString2(dt);
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSalesReturnNumberDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETSALESRETURNNUMBERDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSalesReturnNumberDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string GetSalesReturnQRCodeDetails(string sLocationCode, string sSalesReturnNo, string sQRCode, string sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSalesReturnQRCodeDetails - RequestDataFromAndroid => ", "LocationCode - " + sLocationCode + ", SRNo - " + sSalesReturnNo + ", QRCode - " + sQRCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETSAVESALESRETURNQRCODEDETAILS"),
                                        new SqlParameter("@LocationCode", sLocationCode),
                                        new SqlParameter("@SalesReturnNo", sSalesReturnNo),
                                        new SqlParameter("@QRCode", sQRCode),
                                        new SqlParameter("@CreatedBy", sUserId),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_SalesReturn", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    _sResult = "GETSAVESALESRETURNQRCODEDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSalesReturnQRCodeDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "GETSAVESALESRETURNQRCODEDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSalesReturnQRCodeDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows[0][1].ToString() == "1")
                {
                    string oMatCode = dt.Rows[0][0].ToString();
                    _sResult = "GETSAVESALESRETURNQRCODEDETAILS ~ SUCCESS ~ " + oMatCode; // GlobalVariable.DtToString(dt);
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSalesReturnQRCodeDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows[0][1].ToString() == "0")
                {
                    _sResult = "GETSAVESALESRETURNQRCODEDETAILS ~ ERROR ~ " + "The scanned QRCoode - " + sQRCode + " is not linked with the material of scanned sales return, Kindly scan valid QRCode";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSalesReturnQRCodeDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETSAVESALESRETURNQRCODEDETAILS ~ ERROR ~ " + "NO DETAILS FOUND, KINDLY TRY AGAIN";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSalesReturnQRCodeDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }


        #region Not in use
        internal string GetSalesReturnScannnedStatus(string sLocationCode, string sSalesReturnNo, string sMatCode)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSalesReturnScannnedStatus - RequestDataFromAndroid => ", "LocationCode - " + sLocationCode + ", SRNo - " + sSalesReturnNo + ", MatCode - " + sMatCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETSALESRETURNSTATUS"),
                                        new SqlParameter("@LocationCode", sLocationCode),
                                        new SqlParameter("@SalesReturnNo", sSalesReturnNo),
                                        new SqlParameter("@MatCode", sMatCode),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_SalesReturn", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    _sResult = "GETSALESRETURNSTATUS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSalesReturnQRCodeDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "GETSALESRETURNSTATUS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSalesReturnQRCodeDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("RemainingQty") && dt.Rows.Count > 0)
                {
                    _sResult = "GETSALESRETURNSTATUS ~ SUCCESS ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSalesReturnQRCodeDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETSALESRETURNSTATUS ~ ERROR ~ " + "NO DETAILS FOUND";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSalesReturnQRCodeDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string UpdateSalesReturnQRCodeSAPStatus(string sLocationCode, string sSalesReturnNo, string sMatCode, string sQRCode, string sPostMsg, string sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + sQRCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","UPDATESAPPOSTEDSTATUS"),
                                        new SqlParameter("@LocationCode", sLocationCode),
                                        new SqlParameter("@SalesReturnNo", sSalesReturnNo),
                                        new SqlParameter("@MatCode", sMatCode),
                                        new SqlParameter("@QRCode", sQRCode),
                                        new SqlParameter("@SAPPostMsg", sPostMsg),
                                        new SqlParameter("@CreatedBy", sUserId),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_SalesReturn", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    _sResult = "UPDATESAPPOSTEDSTATUS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "UPDATESAPPOSTEDSTATUS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    _sResult = "UPDATESAPPOSTEDSTATUS ~ SUCCESS ~ "; // GlobalVariable.DtToString(dt);
                    return _sResult;
                }
                else
                {
                    _sResult = "UPDATESAPPOSTEDSTATUS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }
        #endregion

        #endregion


        #region Purchase Return

        internal string GetPurchaseReturnNumberDetails(string _sLocationCode, string _sPOReturnNo)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetPurchaseReturnNumberDetails - RequestDataFromAndroid => ", "LocationCode - " + _sLocationCode + ", POReturnNo - " + _sPOReturnNo);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETPURCHASERETURNNUMBERDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@POReturnNo", _sPOReturnNo),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_PurchaseReturn", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    _sResult = "GETPURCHASERETURNNUMBERDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetPurchaseReturnNumberDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "GETPURCHASERETURNNUMBERDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetPurchaseReturnNumberDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("VendorCode") && dt.Rows.Count > 1)
                {
                    _sResult = "GETPURCHASERETURNNUMBERDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString(dt);
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetPurchaseReturnNumberDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("VendorCode") && dt.Rows.Count == 1)
                {
                    _sResult = "GETPURCHASERETURNNUMBERDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString2(dt);
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetPurchaseReturnNumberDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETPURCHASERETURNNUMBERDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetPurchaseReturnNumberDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string GetPurchaseReturnQRCodeDetails(string sLocationCode, string sPOReturnNo, string sQRCode, string VendorCode, string sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetPurchaseReturnQRCodeDetails - RequestDataFromAndroid => ", "LocationCode - " + sLocationCode + ", POReturnNo - " + sPOReturnNo + ", QRCode - " + sQRCode + ", VendorCode - " + VendorCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETSAVEPURCHASERETURNQRCODEDETAILS"),
                                        new SqlParameter("@LocationCode", sLocationCode),
                                        new SqlParameter("@POReturnNo", sPOReturnNo),
                                        new SqlParameter("@QRCode", sQRCode),
                                        new SqlParameter("@Vendorcode", VendorCode),
                                        new SqlParameter("@CreatedBy", sUserId),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_PurchaseReturn", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    if (dt.Columns.Contains("ERROR") && dt.Columns.Contains("MatCode"))
                    {
                        _sResult = "The material code " + dt.Rows[0][1].ToString() + " of scanned QRCode - "+ sQRCode + " is not exists in scanned POReturn materials, Kindly scan valid QRCode";
                        _sResult = "GETSAVEPURCHASERETURNQRCODEDETAILS ~ ERROR ~ " + _sResult;
                    }
                    else
                        _sResult = "GETSAVEPURCHASERETURNQRCODEDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetPurchaseReturnQRCodeDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "GETSAVEPURCHASERETURNQRCODEDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetPurchaseReturnQRCodeDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows[0][1].ToString() == "1")
                {
                    string oMatCode = dt.Rows[0][0].ToString();
                    _sResult = "GETSAVEPURCHASERETURNQRCODEDETAILS ~ SUCCESS ~ " + oMatCode; // GlobalVariable.DtToString(dt);
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetPurchaseReturnQRCodeDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows[0][1].ToString() == "0")
                {
                    _sResult = "GETSAVEPURCHASERETURNQRCODEDETAILS ~ ERROR ~ " + "NO DETAILS SAVED, KINDLY TRY AGAIN";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetPurchaseReturnQRCodeDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETSAVEPURCHASERETURNQRCODEDETAILS ~ ERROR ~ " + "NO DETAILS FOUND, KINDLY TRY AGAIN";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetPurchaseReturnQRCodeDetails - ResponceSentToAndroid => ", "Responce - " + _sResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string GetPurchaseReturnScannnedStatus(string sLocationCode, string sSalesReturnNo, string sMatCode)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetPurchaseReturnScannnedStatus - RequestDataFromAndroid => ", "LocationCode - " + sLocationCode + ", POReturnNo - " + sSalesReturnNo + ", MatCode - " + sMatCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETSALESRETURNSTATUS"),
                                        new SqlParameter("@LocationCode", sLocationCode),
                                        new SqlParameter("@SalesReturnNo", sSalesReturnNo),
                                        new SqlParameter("@MatCode", sMatCode),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_SalesReturn", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    _sResult = "GETSALESRETURNSTATUS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "GETSALESRETURNSTATUS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("RemainingQty") && dt.Rows.Count > 0)
                {
                    _sResult = "GETSALESRETURNSTATUS ~ SUCCESS ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                else
                {
                    _sResult = "GETSALESRETURNSTATUS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string UpdatePurchaseReturnQRCodeSAPStatus(string sLocationCode, string sSalesReturnNo, string sMatCode, string sQRCode, string sPostMsg, string sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + sQRCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","UPDATESAPPOSTEDSTATUS"),
                                        new SqlParameter("@LocationCode", sLocationCode),
                                        new SqlParameter("@SalesReturnNo", sSalesReturnNo),
                                        new SqlParameter("@MatCode", sMatCode),
                                        new SqlParameter("@QRCode", sQRCode),
                                        new SqlParameter("@SAPPostMsg", sPostMsg),
                                        new SqlParameter("@CreatedBy", sUserId),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_SalesReturn", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    _sResult = "UPDATESAPPOSTEDSTATUS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "UPDATESAPPOSTEDSTATUS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    _sResult = "UPDATESAPPOSTEDSTATUS ~ SUCCESS ~ "; // GlobalVariable.DtToString(dt);
                    return _sResult;
                }
                else
                {
                    _sResult = "UPDATESAPPOSTEDSTATUS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        #endregion
    }
}
