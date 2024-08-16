using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GreenplyScannerCommServer.Common;
using BCILCommServer;
using GreenplyScannerCommServer;
using TEST;

namespace GreenplyScannerCommServer.BI
{
    class ViewDetails
    {
        internal string ViewItemDetails(string _ItemBarcode)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + _ItemBarcode);
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","VIEWITEMDETAILS"),
                                        new SqlParameter("@ItemCode", _ItemBarcode),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_M_GetDetails", parma);
                if (dt.Columns.Contains("ERROR"))
                {
                    _sResult = "VIEWITEMDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                }
                if (dt.Columns.Contains("ErrorMessage"))
                {
                    _sResult = "VIEWITEMDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                }
                else if (dt.Columns.Count > 1 && dt.Rows.Count > 0)
                {
                    _sResult = "VIEWITEMDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString(dt);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
            return _sResult;
        }

        internal string ViewRackDetails(string _Rackcode)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + _Rackcode);
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","VIEWRACKDETAILS"),
                                        new SqlParameter("@RackCode", _Rackcode),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_M_GetDetails", parma);
                if (dt.Columns.Contains("ERROR"))
                {
                    _sResult = "VIEWRACKDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                }
                if (dt.Columns.Contains("ErrorMessage"))
                {
                    _sResult = "VIEWRACKDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                }
                else if (dt.Columns.Count > 1 && dt.Rows.Count > 0)
                {
                    _sResult = "VIEWRACKDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString(dt);
                }
                else
                {
                    _sResult = "VIEWRACKDETAILS ~ ERROR ~ There are no items available in this rack.";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
            return _sResult;
        }
    }
}
