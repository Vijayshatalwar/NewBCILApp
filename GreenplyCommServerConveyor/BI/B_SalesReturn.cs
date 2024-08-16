using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using GreenplyCommServer.Common;
using TEST;
using BCILCommServer;
using GreenplyCommServer;
//using GreenplyCommServer.GreenplyERPPostingService;

namespace GreenplyCommServer.BI
{
    class B_SalesReturn
    {
        DataTable dtScannedData;
        internal string GetSalesReturnNumberDetails(string _sLocationCode, string _sSRNo)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + _sLocationCode);
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
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "GETSALESRETURNNUMBERDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("CustomerCode") && dt.Rows.Count > 1)
                {
                    _sResult = "GETSALESRETURNNUMBERDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString(dt);
                    return _sResult;
                }
                if (dt.Columns.Contains("CustomerCode") && dt.Rows.Count == 1)
                {
                    _sResult = "GETSALESRETURNNUMBERDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString2(dt);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETSALESRETURNNUMBERDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string GetSalesReturnScannnedStatus(string sLocationCode, string sSalesReturnNo, string sMatCode)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + sSalesReturnNo);
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
                if (dt.Columns.Contains("RemainingQty") && dt.Rows.Count > 0 )
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

        internal string GetSalesReturnQRCodeDetails(string sLocationCode, string sSalesReturnNo, string sMatCode, string sQRCode, int ScannedQty, string sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + sQRCode);
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETSALESRETURNQRCODEDETAILS"),
                                        new SqlParameter("@LocationCode", sLocationCode),
                                        new SqlParameter("@SalesReturnNo", sSalesReturnNo),
                                        new SqlParameter("@MatCode", sMatCode),
                                        new SqlParameter("@QRCode", sQRCode),
                                        new SqlParameter("@ScannedQty", ScannedQty),
                                        new SqlParameter("@CreatedBy", sUserId),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_SalesReturn", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    _sResult = "GETSALESRETURNQRCODEDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "GETSALESRETURNQRCODEDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    _sResult = "GETSALESRETURNQRCODEDETAILS ~ SUCCESS ~ QRCode - " + sQRCode + " Is Scanned And Saved Successfully"; // GlobalVariable.DtToString(dt);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETSALESRETURNQRCODEDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
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

    }
}
