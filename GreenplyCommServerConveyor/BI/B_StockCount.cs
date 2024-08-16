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

namespace GreenplyCommServer.BI
{
    class B_StockCount
    {
        public static DataTable dtSQRCode = new DataTable();
        internal string CheckStackQRCodeDetails(string _sLocCode, string _sBarcode)
        {
            dtSQRCode = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + _sBarcode);
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","CHECKSTACKQRCODEDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocCode.Trim()),
                                        new SqlParameter("@StackQRCode", _sBarcode.Trim()),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_StockCount", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    _sResult = "CHECKSTACKQRCODEDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "CHECKSTACKQRCODEDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("QRCode") && dt.Rows.Count > 0)
                {
                    dtSQRCode = dt.Clone();
                    dtSQRCode = dt.Copy();
                    _sResult = "CHECKSTACKQRCODEDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString1(dt);
                    return _sResult;
                }
                else
                {
                    _sResult = "CHECKSTACKQRCODEDETAILS ~ ERROR ~ " + "STACK QRCODE NOT FOUND";
                }
            }
            catch (Exception ex)
            {
                _sResult = "CHECKSTACKQRCODEDETAILS ~ ERROR ~ " + ex.ToString();
                throw ex;
            }
            return _sResult;
        }

        internal string CheckQRCodeDetails(string _sLocCode, string _sBarcode)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + _sBarcode);
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","CHECKQRCODEDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocCode),
                                        new SqlParameter("@QRCode", _sBarcode),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_StockCount", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    _sResult = "CHECKQRCODEDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "CHECKQRCODEDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("QRCode") && dt.Rows.Count > 0)
                {
                    _sResult = "CHECKQRCODEDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString(dt);
                    return _sResult;
                }
                else
                {
                    _sResult = "CHECKQRCODEDETAILS ~ ERROR ~ " + "QRCODE NOT FOUND";
                }
            }
            catch (Exception ex)
            {
                _sResult = "CHECKQRCODEDETAILS ~ ERROR ~ " + ex.ToString();
                throw ex;
            }
            return _sResult;
        }

        internal string SaveStockCountDetails(string LocCode, string UserId, DataTable dtSData)
        {
            int sSaveCount = 0;
            string _sResult = string.Empty;
            try
            {
                //DataTable objDt = convertStringToDataTable(_string);
                if (dtSData.Rows.Count > 0)
                {
                    for (int i = 0; i < dtSData.Rows.Count; i++)
                    {
                        string oLocationCode = dtSData.Rows[i][0].ToString();
                        string _sBarcode = dtSData.Rows[i][1].ToString();
                        string _sUser = UserId.ToString();
                        SqlParameter[] parma = { 
                                                 new SqlParameter("@Type","SAVEQRCODEDETAILS"),
                                                 new SqlParameter("@LocationCode", oLocationCode),
                                                 new SqlParameter("@QRCode", _sBarcode),
                                                 new SqlParameter("@CreatedBy", _sUser),
                                           };
                        DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_StockCount", parma);
                        if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                        {
                            _sResult = "SAVESTOCKCOUNTDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                            //return _sResult;
                        }
                        if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                        {
                            _sResult = "SAVESTOCKCOUNTDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                            //return _sResult;
                        }
                        if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0)
                        {
                            sSaveCount++;
                            _sResult = "SAVESTOCKCOUNTDETAILS ~ SUCCESS ~ ";
                            //return _sResult;
                        }
                        else
                        {
                            _sResult = "SAVESTOCKCOUNTDETAILS ~ ERROR ~ NOT FOUND";
                        }
                     }
                  }
                _sResult = "SAVESTOCKCOUNTDETAILS ~ SUCCESS ~ " + sSaveCount + " No of Records Saved Successfully Out Of " + dtSData.Rows.Count;
               return _sResult;
            }
            catch (Exception ex)
            {
                _sResult = "SAVESTOCKCOUNTDETAILS ~ ERROR ~ " + ex.ToString();
                throw;
            }
            
        }

        public static DataTable convertStringToDataTable(string data)
        {
            DataTable dataTable = new DataTable();
            bool columnsAdded = false;
            foreach (string row in data.Split('$'))
            {
                DataRow dataRow = dataTable.NewRow();
                foreach (string cell in row.Split('|'))
                {
                    string[] keyValue = cell.Split('~');
                    if (!columnsAdded)
                    {
                        DataColumn dataColumn = new DataColumn(keyValue[0]);
                        dataTable.Columns.Add(dataColumn);
                    }
                    dataRow[keyValue[0]] = keyValue[1];
                }
                columnsAdded = true;
                dataTable.Rows.Add(dataRow);
            }
            return dataTable;
        }

    }
}
