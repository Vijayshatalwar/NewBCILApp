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
using System.IO;

namespace GreenplyScannerCommServer.BI
{
    class B_StackPrinting
    {
        BcilNetwork _bcilNetwork = new BcilNetwork();
        //public static string sMatCode = string.Empty;
        public static int sStackPrintCount = 0;
        public static int sSaveCount = 0;
        string objLocationCode;
        string objQRCode = string.Empty;
        string objStackQRCode = string.Empty;
        string sPrintStatus = string.Empty;
        string sMatCode = string.Empty;
        string sMatStatus = string.Empty;
        string sGradeDesc = string.Empty;
        string sGroupDesc = string.Empty;
        string sThicknessDesc = string.Empty;
        string sMatSize = string.Empty;
        public static DataTable dtStackData = new DataTable();
        DataTable dtQRData = new DataTable();


        #region Segregation Stack Scanning

        internal string GetScannedSegregationQRCodeDetails(string _sLocationCode, string _QRCode, string sStatus)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + _sLocationCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","CHECKSCANNEDQRCODEDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@QRCode", _QRCode),
                                        new SqlParameter("@MatStatus", sStatus),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_StackSegregationPrinting", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    _sResult = "CHECKSCANNEDQRCODEDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "CHECKSCANNEDQRCODEDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("QRCode") && dt.Rows.Count == 1)
                {
                    if (frmMain.dtStackData.Columns.Count > 0)
                    {
                        DataRow _dtRow = frmMain.dtStackData.NewRow();
                        _dtRow["LocationCode"] = dt.Rows[0][0].ToString();
                        _dtRow["MatCode"] = dt.Rows[0][1].ToString();
                        _dtRow["QRCode"] = dt.Rows[0][2].ToString();
                        _dtRow["Status"] = dt.Rows[0][3].ToString();
                        _dtRow["GradeDesc"] = dt.Rows[0][4].ToString();
                        _dtRow["GroupDesc"] = dt.Rows[0][5].ToString();
                        _dtRow["ThicknessDesc"] = dt.Rows[0][6].ToString();
                        _dtRow["MatSize"] = dt.Rows[0][7].ToString();
                        frmMain.dtStackData.Rows.Add(_dtRow);
                    }
                    _sResult = "CHECKSCANNEDQRCODEDETAILS ~ SUCCESS ~ " + dt.Rows[0][2].ToString();
                    return _sResult;
                }
                else
                {
                    _sResult = "CHECKSCANNEDQRCODEDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        //string _strLocationCode, string sMatCode, string sQRCode, string sStackQRCode, string sDateFormat, string sMatStatus, string GradeDesc, string GroupDesc, string ThicknessDesc, string MatSize, string LotSize
        public string PrintSegregationStackQRCode(DataTable dtData, string objStackQRCode, string oDateFormat)  //, string GradeDesc, string GroupDesc, string ThicknessDesc, string MatSize, string LotSize
        {
            try
            {
                string OutMsg = string.Empty;
                var sReadPrn = string.Empty;
                sPrintStatus = string.Empty;
                sMatCode = string.Empty;
                sMatStatus = string.Empty;
                sGradeDesc = string.Empty;
                sGroupDesc = string.Empty;
                sThicknessDesc = string.Empty;
                sMatSize = string.Empty;
                int LotSize = 0;
                sSaveCount = 0;
                objQRCode = string.Empty;

                _bcilNetwork.PrinterIP = Properties.Settings.Default.SegregationStackQRCodePrinterIP;
                _bcilNetwork.PrinterPort = Properties.Settings.Default.SegregationStackQRCodePrinterPort;

                for (int i = 0; i < dtData.Rows.Count; i++)
                {
                    objLocationCode = dtData.Rows[i][0].ToString().Trim();
                    sMatCode = dtData.Rows[i][1].ToString().Trim();
                    objQRCode = dtData.Rows[i][2].ToString().Trim();
                    sMatStatus = dtData.Rows[i][3].ToString().Trim();
                    sGradeDesc = dtData.Rows[i][4].ToString().Trim();
                    sGroupDesc = dtData.Rows[i][5].ToString().Trim();
                    sThicknessDesc = dtData.Rows[i][6].ToString().Trim();
                    sMatSize = dtData.Rows[i][7].ToString().Trim();

                    //int numberOfRecords = 0;
                    //numberOfRecords = dtData.Select("IsActive = " + sMatCode.Trim()).Length;

                    string sSaveStatus = SaveSegregationStackQRCode(objLocationCode.Trim(), sMatCode.Trim(), objQRCode.Trim(), objStackQRCode.Trim(), oDateFormat.Trim(), sMatStatus);
                    if (sSaveStatus.Contains("1"))
                    {
                        sSaveCount++;
                    }
                    else if (sSaveStatus.Contains("0"))
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", "Barcode - " + objStackQRCode + " Not Update At " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                    }
                }
                OutMsg = _bcilNetwork.NetworkPrinterStatus();
                if (OutMsg == "PRINTER READY")
                {
                    StringBuilder sb = new StringBuilder();
                    DataTable dt = new DataTable();
                    string sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplySegregationStackQRCode.PRN";
                    if (File.Exists(sPrnExist))
                    {
                        StreamReader sr = new StreamReader(sPrnExist);
                        sReadPrn = sr.ReadToEnd();
                        sr.Close();
                        sr.Dispose();
                    }
                    else
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", "PRN File Not Found");
                        throw new Exception("PRN File Not Found");
                    }

                    //DataTable distinct = dtData.DefaultView.ToTable(true, "MatCode");
                    DataTable distinct = dtData.DefaultView.ToTable(true, "MatCode", "QRCode");
                    distinct.Columns.Add("GradeDesc");
                    distinct.Columns.Add("GroupDesc");
                    distinct.Columns.Add("ThicknessDesc");
                    distinct.Columns.Add("Size");
                    distinct.Columns.Add("TotalQty");

                    int iCount = 0;
                    for (int i = 0; i < distinct.Rows.Count; i++)
                    {
                        string Finalmatcode = distinct.Rows[i][1].ToString();
                        for (int j = 0; j < dtData.Rows.Count; j++)
                        {
                            string Stackmatcode = dtData.Rows[j][1].ToString();
                            if (Stackmatcode == Finalmatcode)
                            {
                                iCount++;
                                distinct.Rows[i][0] = dtData.Rows[j][0].ToString();
                                distinct.Rows[i][1] = dtData.Rows[j][1].ToString();
                                distinct.Rows[i][2] = dtData.Rows[j][2].ToString();
                                distinct.Rows[i][3] = dtData.Rows[j][3].ToString();
                                distinct.Rows[i][4] = dtData.Rows[j][4].ToString();
                                distinct.Rows[i][5] = dtData.Rows[j][5].ToString();
                                distinct.Rows[i][9] = Convert.ToString(iCount);
                            }
                        }
                    }
                    sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objStackQRCode.Trim()));
                    sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(objStackQRCode.Trim()));
                    for (int i = 0; i < distinct.Rows.Count; i++)
                    {
                        sMatCode = distinct.Rows[i][1].ToString().Trim();
                        sGradeDesc = distinct.Rows[i][4].ToString().Trim();
                        sGroupDesc = distinct.Rows[i][5].ToString().Trim();
                        sThicknessDesc = distinct.Rows[i][6].ToString().Trim();
                        sMatSize = distinct.Rows[i][7].ToString().Trim();
                        LotSize = Convert.ToInt32(dtData.Rows[i][7].ToString().Trim());
                        if (i == 0)
                        {
                            string ObjCode2 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(ObjCode2.Trim()));
                        }
                        if (i == 1)
                        {
                            string ObjCode3 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(ObjCode3.Trim()));
                        }
                        if (i == 2)
                        {
                            string ObjCode4 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(ObjCode4.Trim()));
                        }
                        if (i == 3)
                        {
                            string ObjCode5 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(ObjCode5.Trim()));
                        }
                    }
                    _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplySegregationStackQRCode.PRN";
                    for (int i = 0; i < 2; i++)
                    {
                        OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                        _bcilNetwork.Dispose();
                        //OutMsg = "SUCCESS";
                    }
                    //_bcilNetwork.Dispose();
                    if (OutMsg == "SUCCESS")
                    {
                        sStackPrintCount++;
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", "Stack QRCode - " + objStackQRCode + " Saved And Printed Successfully at " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                        return OutMsg = "SUCCESS~Printed Successfully";
                    }
                }
                else
                {
                    if (OutMsg == "PRINTER NOT IN NETWORK")
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", "ERROR ~ Printer not in network");
                        return OutMsg = "ERROR~Printer not in network";
                    }
                    else
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", "ERROR~Printer error is : " + OutMsg);
                        return OutMsg;
                    }
                }
                return OutMsg;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", exDetail);
                return "ERROR | " + ex.Message;
            }
        }

        internal string SaveSegregationStackQRCode(string objLocationCode, string objMatCode, string objQRCode, string objStackQRCode, string sDateFormat, string sMatStatus)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + objStackQRCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","SAVESEGREGATIONSTACKQRCODE"),
                                        new SqlParameter("@LocationCode", objLocationCode),
                                        new SqlParameter("@MatCode", objMatCode),
                                        new SqlParameter("@StackQRCode", objStackQRCode),
                                        new SqlParameter("@QRCode", objQRCode),
                                        new SqlParameter("@DateFormat", sDateFormat),
                                        new SqlParameter("@MatStatus", sMatStatus),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_StackSegregationPrinting", parma);
                if (dt.Columns.Contains("ERROR"))
                {
                    _sResult = "ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows[0][0].ToString() == "1")
                {
                    _sResult = "SUCCESS ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
                else
                {
                    _sResult = "ERROR ~ " + "NOT FOUND";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //public DataSet fnPIGetSegregationDataSAPPost(DataTable dtdata)
        //{
        //    DataSet ds = new DataSet();
        //    ds.Tables.Add(new DataTable());
        //    try
        //    {
        //        Zbc_tst_wsClient zbcd = new Zbc_tst_wsClient();

        //        zbcd.ClientCredentials.UserName.UserName = Properties.Settings.Default.ServiceUserID;
        //        zbcd.ClientCredentials.UserName.Password = Properties.Settings.Default.ServiceUserPassword;

        //        ZbcPlyqrCodePost master = new ZbcPlyqrCodePost();
        //        ZbcPlyqrCodePostRequest request = new ZbcPlyqrCodePostRequest();
        //        ZbcPlyqrCodePostResponse responce = new ZbcPlyqrCodePostResponse();

        //        ZstrBcMatPost[] post = new ZstrBcMatPost[1];
        //        if (dtdata.Rows.Count > 0)
        //        {
        //            post = new ZstrBcMatPost[dtdata.Rows.Count];
        //            for (int i = 0; i < dtdata.Rows.Count; i++)
        //            {
        //                post[i] = new ZstrBcMatPost();
        //                post[i].Plantcode = dtdata.Rows[i][0].ToString();
        //                post[i].Matcode = dtdata.Rows[i][1].ToString();
        //                post[i].Serial = dtdata.Rows[i][2].ToString();
        //                post[i].Status = dtdata.Rows[i][3].ToString();
        //            }
        //        }
        //        request.ZbcPlyqrCodePost = master;
        //        request.ZbcPlyqrCodePost.DataIn = post;
        //        responce = zbcd.ZbcPlyqrCodePost(request.ZbcPlyqrCodePost);
        //        if (responce.DataOut != null)
        //        {
        //            ds.Tables[0].Columns.Add("LocationCode");
        //            ds.Tables[0].Columns.Add("MatCode");
        //            ds.Tables[0].Columns.Add("QRCode");
        //            ds.Tables[0].Columns.Add("Status");
        //            ds.Tables[0].Columns.Add("SAPStatus");

        //            foreach (var items in responce.DataOut)
        //            {
        //                DataRow dr = ds.Tables[0].NewRow();
        //                dr["LocationCode"] = Convert.ToString(Convert.ToString(items.Plantcode) == null ? "" : Convert.ToString(items.Plantcode));
        //                dr["MatCode"] = Convert.ToString(Convert.ToString(items.Matcode) == null ? "" : Convert.ToString(items.Matcode));
        //                dr["QRCode"] = Convert.ToString(Convert.ToString(items.Serial) == null ? "" : Convert.ToString(items.Serial));
        //                dr["Status"] = Convert.ToString(Convert.ToString(items.Status) == null ? "" : Convert.ToString(items.Status));
        //                dr["SAPStatus"] = Convert.ToString(Convert.ToString(items.Sts) == null ? "" : Convert.ToString(items.Sts));
        //                ds.Tables[0].Rows.Add(dr);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //    return ds;
        //}

        internal string UpdateSegregationQRCodeSAPStatus(string sLocationCode, string sMatCode, string sQRCode, string sStatus, string sPostMsg, string sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + sQRCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","UPDATESEGREGATIONSAPPOSTEDSTATUS"),
                                        new SqlParameter("@LocationCode", sLocationCode),
                                        new SqlParameter("@MatCode", sMatCode),
                                        new SqlParameter("@QRCode", sQRCode),
                                        new SqlParameter("@MatStatus", sStatus),
                                        new SqlParameter("@SAPPostMsg", sPostMsg),
                                        new SqlParameter("@CreatedBy", sUserId),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_StackSegregationPrinting", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    _sResult = "UPDATESEGREGATIONSAPPOSTEDSTATUS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "UPDATESEGREGATIONSAPPOSTEDSTATUS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    _sResult = "UPDATESEGREGATIONSAPPOSTEDSTATUS ~ SUCCESS ~ "; // GlobalVariable.DtToString(dt);
                    return _sResult;
                }
                else
                {
                    _sResult = "UPDATESEGREGATIONSAPPOSTEDSTATUS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        #endregion


        #region Segregation Qrcode Label Scanning

        internal string GetSegregationScannedQRCodeDetails(string _sLocationCode, string _QRCode, string sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSegregationScannedQRCodeDetails - RequestDataFromAndroid => ", "LocationCode : " + _sLocationCode + ", QRCode : " + _QRCode + ", UserId : " + sUserId);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","CHECKSEGREGATIONSCANNEDQRCODEDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode.Trim()),
                                        new SqlParameter("@QRCode", _QRCode.Trim()),
                                        new SqlParameter("@CreatedBy", sUserId.Trim()),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_StackSegregationPrinting", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    _sResult = "CHECKSEGREGATIONSCANNEDQRCODEDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "CHECKSEGREGATIONSCANNEDQRCODEDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows[0][0].ToString() == "1")
                {
                    _sResult = "CHECKSEGREGATIONSCANNEDQRCODEDETAILS ~ SUCCESS ~ QRCode - " + _QRCode + " Scanned Successfully";
                    return _sResult;
                }
                else
                {
                    _sResult = "CHECKSEGREGATIONSCANNEDQRCODEDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        #endregion


        #region Segregation Stack Scanning Against Cancelled Delivery

        internal string GetCancelledDeliverySegregationScannedQRCodeDetails(string _sLocationCode, string _QRCode, string sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "CancelledDeliverySegregationScannedQRCodeDetails - RequestedDataFromAndroid => ", "LocationCode : " + _sLocationCode + ", QRCode : " + _QRCode + ", UserId : " + sUserId);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","CHECKCANCELLEDDELIVERYSCANNEDQRCODEDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode.Trim()),
                                        new SqlParameter("@QRCode", _QRCode.Trim()),
                                        new SqlParameter("@CreatedBy", sUserId.Trim()),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_StackSegregationPrinting", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    _sResult = "CHECKCANCELLEDDELIVERYSCANNEDQRCODEDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "CHECKCANCELLEDDELIVERYSCANNEDQRCODEDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows[0][0].ToString() == "1")
                {
                    _sResult = "CHECKCANCELLEDDELIVERYSCANNEDQRCODEDETAILS ~ SUCCESS ~ QRCode - " + _QRCode + " Scanned Successfully";
                    return _sResult;
                }
                else
                {
                    _sResult = "CHECKCANCELLEDDELIVERYSCANNEDQRCODEDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "CancelledDeliverySegregationScannedQRCodeDetails - Exception => ", ex.ToString());
                throw ex;
            }
            return _sResult;
        }

        #endregion


        #region Stack Merging

        internal string GetStackMergeStackQRCodeDetails(string _sLocationCode, string _sStackQRCode, string _sUserID)
        {
            dtStackData = new DataTable();
            DataTable dtCode = new DataTable();
            DataTable distinctValues = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "RequestDataFromAndroid - StackMerge_UpdateScannedQRCode => ", "LocationCode - " + _sLocationCode + ", StackQRCode - " + _sStackQRCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETSTACKMERGINGSCANNEDSTACKDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@StackQRCode", _sStackQRCode),
                                        new SqlParameter("@CreatedBy", _sUserID),
                                   };
                dtCode = GlobalVariable._clsSql.GetDataUsingProcedure("USP_StackSegregationPrinting", parma);
                if (dtCode.Columns.Contains("ERROR") && dtCode.Rows.Count > 0)
                {
                    _sResult = "GETSTACKMERGINGSCANNEDSTACKDETAILS ~ ERROR ~ " + dtCode.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "RequestDataFromAndroid - StackMerge_UpdateScannedQRCode => ", "Responce - " + _sResult);
                    return _sResult;
                }
                if (dtCode.Columns.Contains("ErrorMessage") && dtCode.Rows.Count > 0)
                {
                    _sResult = "GETSTACKMERGINGSCANNEDSTACKDETAILS ~ ERROR ~ " + dtCode.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "RequestDataFromAndroid - StackMerge_UpdateScannedQRCode => ", "Responce - " + _sResult);
                    return _sResult;
                }
                if (dtCode.Columns.Contains("MatCode") && dtCode.Rows.Count > 0)
                {
                    if (frmMain.dtStackMergeData.Rows.Count > 0)
                    {
                        DataView view = new DataView(dtCode);
                        distinctValues = view.ToTable(true, "MatCode");  //, "StackQRCode", "TotalQty"
                        if (distinctValues.Rows.Count > 1)
                        {
                            _sResult = "GETSTACKMERGINGSCANNEDSTACKDETAILS ~ ERROR ~ " + "The Current Scanned Stack Contains more than one Material Code";  //And There are Already 4 Scanned Material Codes Exists
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "RequestDataFromAndroid - StackMerge_UpdateScannedQRCode => ", "Responce - " + _sResult);
                            return _sResult;
                        }
                        DataView view2 = new DataView(frmMain.dtStackMergeData);
                        distinctValues = view2.ToTable(true, "MatCode");  //, "StackQRCode", "TotalQty"
                        if (distinctValues.Rows.Count > 1)
                        {
                            _sResult = "GETSTACKMERGINGSCANNEDSTACKDETAILS ~ ERROR ~ " + "The Current Scanned Stack Contains one Material Code ";  //And There are Already 4 Scanned Material Codes Exists
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "RequestDataFromAndroid - StackMerge_UpdateScannedQRCode => ", "Responce - " + _sResult);
                            return _sResult;
                        }
                        else
                        {
                            for (int i = 0; i < distinctValues.Rows.Count; i++)
                            {
                                for (int j = 0; j < dtCode.Rows.Count; j++)
                                {
                                    string Finalmatcode = distinctValues.Rows[i][0].ToString();
                                    string Stackmatcode = dtCode.Rows[j][1].ToString();
                                    DataRow dr = frmMain.dtStackMergeData.NewRow();
                                    dr[0] = dtCode.Rows[j][0].ToString();
                                    dr[1] = dtCode.Rows[j][1].ToString();
                                    dr[2] = dtCode.Rows[j][2].ToString();
                                    dr[3] = dtCode.Rows[j][3].ToString();
                                    dr[4] = dtCode.Rows[j][4].ToString();
                                    dr[5] = dtCode.Rows[j][5].ToString();
                                    dr[6] = dtCode.Rows[j][6].ToString();
                                    dr[7] = dtCode.Rows[j][7].ToString();
                                    frmMain.dtStackMergeData.Rows.InsertAt(dr, j + 1);
                                }
                            }
                            DataTable dt1 = frmMain.dtStackMergeData.Copy();
                            dt1.Columns.Remove("LocationCode");
                            dt1.Columns.Remove("GradeDesc");
                            dt1.Columns.Remove("GroupDesc");
                            dt1.Columns.Remove("ThicknessDescription");
                            dt1.Columns.Remove("Size");
                            dt1.Columns.Remove("StackQRCode");
                            DataView view1 = new DataView(dt1);
                            distinctValues = view1.ToTable(true, "MatCode", "TotalQty");
                            _sResult = "GETSTACKMERGINGSCANNEDSTACKDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString(distinctValues);
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "RequestDataFromAndroid - StackMerge_UpdateScannedQRCode => ", "Responce - " + _sResult);
                            return _sResult;
                        }
                    }
                    if (frmMain.dtStackMergeData.Rows.Count == 0)
                    {
                        DataView view = new DataView(dtCode);
                        distinctValues = view.ToTable(true, "MatCode");  //, "StackQRCode", "TotalQty"
                        if (distinctValues.Rows.Count > 1)
                        {
                            _sResult = "GETSTACKMERGINGSCANNEDSTACKDETAILS ~ ERROR ~ " + "The Current Scanned Stack Contains more than one Material Code";  //And There are Already 4 Scanned Material Codes Exists
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "RequestDataFromAndroid - StackMerge_UpdateScannedQRCode => ", "Responce - " + _sResult);
                            return _sResult;
                        }
                        frmMain.dtStackMergeData = dtCode.Copy();
                        DataTable dt2 = dtCode.Copy();
                        dt2.Columns.Remove("LocationCode");
                        dt2.Columns.Remove("GradeDesc");
                        dt2.Columns.Remove("GroupDesc");
                        dt2.Columns.Remove("ThicknessDescription");
                        dt2.Columns.Remove("Size");
                        dt2.Columns.Remove("StackQRCode");
                        DataView view2 = new DataView(dt2);
                        distinctValues = view2.ToTable(true, "MatCode", "TotalQty");
                        _sResult = "GETSTACKMERGINGSCANNEDSTACKDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString(distinctValues);
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "RequestDataFromAndroid - StackMerge_UpdateScannedQRCode => ", "Responce - " + _sResult);
                        return _sResult;
                    }
                }
                else
                {
                    _sResult = "GETSTACKMERGINGSCANNEDSTACKDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "RequestDataFromAndroid - StackMerge_UpdateScannedQRCode => ", "Responce - " + _sResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        public string PrintStackMergingStackQRCode(string objLocationCode, DataTable dtData, string objNewStackQRCode, string oDateFormat, string _sUserID)  //, string GradeDesc, string GroupDesc, string ThicknessDesc, string MatSize, string LotSize
        {
            try
            {
                string OutMsg = string.Empty;
                var sReadPrn = string.Empty;
                sPrintStatus = string.Empty;
                sMatCode = string.Empty;
                sMatStatus = string.Empty;
                sGradeDesc = string.Empty;
                sGroupDesc = string.Empty;
                sThicknessDesc = string.Empty;
                sMatSize = string.Empty;
                int LotSize = 0;
                sSaveCount = 0;

                if (objLocationCode == "2000" && _sUserID.Contains("WH"))
                {
                    _bcilNetwork.PrinterIP = Properties.Settings.Default.WHDispatchStackQRCodePrinterIP;
                    _bcilNetwork.PrinterPort = Properties.Settings.Default.WHDispatchStackQRCodePrinterPort;
                }
                else
                {
                    _bcilNetwork.PrinterIP = Properties.Settings.Default.SegregationStackQRCodePrinterIP;
                    _bcilNetwork.PrinterPort = Properties.Settings.Default.SegregationStackQRCodePrinterPort;
                }
                string sPrintSection = Properties.Settings.Default.PrintingSection;
                string sLocationType = Properties.Settings.Default.PrintingLocationType;

                OutMsg = _bcilNetwork.NetworkPrinterStatus();
                if (OutMsg == "PRINTER READY")
                {
                    for (int i = 0; i < dtData.Rows.Count; i++)
                    {
                        objLocationCode = dtData.Rows[i][0].ToString().Trim();
                        sMatCode = dtData.Rows[i][1].ToString().Trim();
                        sGradeDesc = dtData.Rows[i][2].ToString().Trim();
                        sGroupDesc = dtData.Rows[i][3].ToString().Trim();
                        sThicknessDesc = dtData.Rows[i][4].ToString().Trim();
                        sMatSize = dtData.Rows[i][5].ToString().Trim();
                        objStackQRCode = dtData.Rows[i][6].ToString().Trim();
                        LotSize = Convert.ToInt32(dtData.Rows[i][7].ToString().Trim());
                        string sSaveStatus = SaveStackMergingStackQRCode(objLocationCode.Trim(), objStackQRCode.Trim(), objNewStackQRCode.Trim(), oDateFormat.Trim(), sPrintSection.Trim(), sLocationType.Trim(), _sUserID.Trim());
                        if (sSaveStatus.Contains("1"))
                        {
                            sSaveCount++;
                        }
                        else if (sSaveStatus.Contains("0"))
                        {
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", "Barcode - " + objNewStackQRCode + " Not Update At " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                        }
                    }
                    StringBuilder sb = new StringBuilder();
                    DataTable dt = new DataTable();
                    string sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplySegregationStackQRCode.PRN";
                    if (File.Exists(sPrnExist))
                    {
                        StreamReader sr = new StreamReader(sPrnExist);
                        sReadPrn = sr.ReadToEnd();
                        sr.Close();
                        sr.Dispose();
                    }
                    else
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", "PRN File Not Found");
                        throw new Exception("PRN File Not Found");
                    }
                    DataTable distinctValues11 = dtData.DefaultView.ToTable(true, "MatCode");
                    if (distinctValues11.Rows.Count == dtData.Rows.Count)
                    {
                        sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objNewStackQRCode.Trim()));
                        sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(objNewStackQRCode.Trim()));
                        for (int i = 0; i < dtData.Rows.Count; i++)
                        {
                            sMatCode = dtData.Rows[i][1].ToString().Trim();
                            sGradeDesc = dtData.Rows[i][2].ToString().Trim();
                            sGroupDesc = dtData.Rows[i][3].ToString().Trim();
                            sThicknessDesc = dtData.Rows[i][4].ToString().Trim();
                            sMatSize = dtData.Rows[i][5].ToString().Trim();
                            LotSize = Convert.ToInt32(dtData.Rows[i][7].ToString().Trim());

                            if (i == 0)
                            {
                                string ObjCode2 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                                sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(ObjCode2.Trim()));
                            }
                            if (i == 1)
                            {
                                string ObjCode3 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                                sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(ObjCode3.Trim()));
                            }
                            if (i == 2)
                            {
                                string ObjCode4 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                                sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(ObjCode4.Trim()));
                            }
                            if (i == 3)
                            {
                                string ObjCode5 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                                sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(ObjCode5.Trim()));
                            }
                        }
                        if (dtData.Rows.Count == 1)
                        {
                            sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(""));
                            sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(""));
                            sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                        }
                        if (dtData.Rows.Count == 2)
                        {
                            sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(""));
                            sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                        }
                        if (dtData.Rows.Count == 3)
                        {
                            sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                        }
                    }
                    else
                    {
                        int iCount = 0;
                        distinctValues11.Columns.Add("GradeDesc");
                        distinctValues11.Columns.Add("GroupDesc");
                        distinctValues11.Columns.Add("ThicknessDesc");
                        distinctValues11.Columns.Add("Size");
                        //distinctValues11.Columns["TotalQty"].SetOrdinal(5);
                        distinctValues11.Columns.Add("TotalQty");
                        for (int i = 0; i < distinctValues11.Rows.Count; i++)
                        {
                            string Finalmatcode = distinctValues11.Rows[i][0].ToString();
                            //int oQty = Convert.ToInt32(distinctValues11.Rows[i][5].ToString());
                            for (int j = 0; j < dtData.Rows.Count; j++)
                            {
                                string Stackmatcode = dtData.Rows[j][1].ToString();
                                int oQty = Convert.ToInt32(dtData.Rows[j][7].ToString());
                                if (Stackmatcode != Finalmatcode)
                                {
                                    distinctValues11.Rows[i][0] = dtData.Rows[j][1].ToString();
                                    distinctValues11.Rows[i][1] = dtData.Rows[j][2].ToString();
                                    distinctValues11.Rows[i][2] = dtData.Rows[j][3].ToString();
                                    distinctValues11.Rows[i][3] = dtData.Rows[j][4].ToString();
                                    distinctValues11.Rows[i][4] = dtData.Rows[j][5].ToString();
                                    distinctValues11.Rows[i][5] = dtData.Rows[j][7].ToString(); //Convert.ToString(iCount);
                                }
                                if (Stackmatcode == Finalmatcode)
                                {
                                    int sQty = 0;
                                    if (distinctValues11.Rows[i][5].ToString() != "")
                                    {
                                        sQty = Convert.ToInt32(distinctValues11.Rows[i][5].ToString());
                                    }
                                    distinctValues11.Rows[i][0] = dtData.Rows[j][1].ToString();
                                    distinctValues11.Rows[i][1] = dtData.Rows[j][2].ToString();
                                    distinctValues11.Rows[i][2] = dtData.Rows[j][3].ToString();
                                    distinctValues11.Rows[i][3] = dtData.Rows[j][4].ToString();
                                    distinctValues11.Rows[i][4] = dtData.Rows[j][5].ToString();
                                    distinctValues11.Rows[i][5] = (sQty + oQty); //dtData.Rows[j][7].ToString();
                                    //frmMain.dtStackMergeData.Rows[j][7] = (sQty + oQty);
                                }
                            }
                        }
                        sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objNewStackQRCode.Trim()));
                        sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(objNewStackQRCode.Trim()));
                        for (int i = 0; i < distinctValues11.Rows.Count; i++)
                        {
                            sMatCode = distinctValues11.Rows[i][0].ToString().Trim();
                            sGradeDesc = distinctValues11.Rows[i][1].ToString().Trim();
                            sGroupDesc = distinctValues11.Rows[i][2].ToString().Trim();
                            sThicknessDesc = distinctValues11.Rows[i][3].ToString().Trim();
                            sMatSize = distinctValues11.Rows[i][4].ToString().Trim();
                            LotSize = Convert.ToInt32(distinctValues11.Rows[i][5].ToString().Trim());

                            if (i == 0)
                            {
                                string ObjCode2 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                                sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(ObjCode2.Trim()));
                            }
                            if (i == 1)
                            {
                                string ObjCode3 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                                sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(ObjCode3.Trim()));
                            }
                            if (i == 2)
                            {
                                string ObjCode4 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                                sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(ObjCode4.Trim()));
                            }
                            if (i == 3)
                            {
                                string ObjCode5 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                                sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(ObjCode5.Trim()));
                            }
                        }
                        if (distinctValues11.Rows.Count == 1)
                        {
                            sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(""));
                            sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(""));
                            sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                        }
                        if (distinctValues11.Rows.Count == 2)
                        {
                            sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(""));
                            sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                        }
                        if (distinctValues11.Rows.Count == 3)
                        {
                            sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                        }
                    }
                    _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplySegregationStackQRCode.PRN";
                    for (int i = 0; i < 2; i++)
                    {
                        OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                        //OutMsg = "SUCCESS";
                        //_bcilNetwork.Dispose();
                    }
                    _bcilNetwork.Dispose();
                    if (OutMsg == "SUCCESS")
                    {
                        sStackPrintCount++;
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", "Stack QRCode - " + objNewStackQRCode + " Saved And Printed Successfully at " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                        return OutMsg = "SUCCESS ~ Printed Successfully";
                    }
                }
                else
                {
                    if (OutMsg == "PRINTER NOT IN NETWORK")
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", "ERROR ~ Printer not in network");
                        return OutMsg = "ERROR ~ Printer not in network";
                    }
                    else
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", "ERROR ~ Printer error is : " + OutMsg);
                        return OutMsg;
                    }
                }
                return OutMsg;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", exDetail);
                return "ERROR | " + ex.Message;
            }
        }

        internal string SaveStackMergingStackQRCode(string objLocationCode, string objStackQRCode, string objNewStackQRCode, string sDateFormat, string sPrintSection, string sLocType, string sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + objStackQRCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","SAVEUPDATESTACKMERGSTACKQRCODEDETAILS"),
                                        new SqlParameter("@LocationCode", objLocationCode),
                                        new SqlParameter("@StackQRCode", objStackQRCode),
                                        new SqlParameter("@NewStackQRCode", objNewStackQRCode),
                                        new SqlParameter("@DateFormat", sDateFormat),
                                        new SqlParameter("@PrintSection", sPrintSection),
                                        new SqlParameter("@LocationType", sLocType),
                                        new SqlParameter("@CreatedBy", sUserId),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_StackSegregationPrinting", parma);
                if (dt.Columns.Contains("ERROR"))
                {
                    _sResult = "ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows[0][0].ToString() == "1")
                {
                    _sResult = "SUCCESS ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
                else
                {
                    _sResult = "ERROR ~ " + "NOT FOUND";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region Stack Splitting

        internal string GetStackSplitStackQRCodeDetails(string _sLocationCode, string _sStackQRCode, string _sUserID)
        {
            dtStackData = new DataTable();
            DataTable dtCode = new DataTable();
            DataTable distinctValues = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "RequestDataFromAndroid - StackSplit_StackQRCodeDetails => ", "LocationCode - " + _sLocationCode + ", StackQRCode - " + _sStackQRCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETSTACKSPLITSCANNEDSTACKDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@StackQRCode", _sStackQRCode),
                                        new SqlParameter("@CreatedBy", _sUserID),
                                   };
                dtCode = GlobalVariable._clsSql.GetDataUsingProcedure("USP_StackSegregationPrinting", parma);
                if (dtCode.Columns.Contains("ERROR") && dtCode.Rows.Count > 0)
                {
                    _sResult = "GETSTACKSPLITSCANNEDSTACKDETAILS ~ ERROR ~ " + dtCode.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ResponceSentToAndroid - StackSplit_StackQRCodeDetails => ", "Responce - " + _sResult);
                    return _sResult;
                }
                if (dtCode.Columns.Contains("ErrorMessage") && dtCode.Rows.Count > 0)
                {
                    _sResult = "GETSTACKSPLITSCANNEDSTACKDETAILS ~ ERROR ~ " + dtCode.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ResponceSentToAndroid - StackSplit_StackQRCodeDetails => ", "Responce - " + _sResult);
                    return _sResult;
                }
                if (dtCode.Columns.Contains("MatCode") && dtCode.Rows.Count > 0)
                {
                    //if (frmMain.dtStackSplitData.Rows.Count > 0)
                    //{
                    //    DataView view = new DataView(frmMain.dtStackSplitData);
                    //    distinctValues = view.ToTable(true, "MatCode");
                    //    if (distinctValues.Rows.Count > 4)
                    //    {
                    //        _sResult = "GETSTACKMERGINGSCANNEDSTACKDETAILS ~ ERROR ~ " + "The Current Scanned Stack Contains Diffrent Material Code And There are Already 4 Scanned Material Codes Exists";
                    //        return _sResult;
                    //    }
                    //    else
                    //    {
                    //        DataTable dt1 = frmMain.dtStackSplitData.Copy();
                    //        dt1.Columns.Remove("LocationCode");
                    //        dt1.Columns.Remove("GradeDesc");
                    //        dt1.Columns.Remove("GroupDesc");
                    //        dt1.Columns.Remove("ThicknessDescription");
                    //        dt1.Columns.Remove("Size");
                    //        dt1.Columns.Remove("StackQRCode");
                    //        DataView view1 = new DataView(dt1);
                    //        distinctValues = view1.ToTable(true, "MatCode", "TotalQty");
                    //        _sResult = "GETSTACKSPLITSCANNEDSTACKDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString(distinctValues);
                    //        return _sResult;
                    //    }
                    //}
                    if (frmMain.dtStackSplitData.Rows.Count == 0)
                    {
                        DataView view = new DataView(frmMain.dtStackSplitData);
                        distinctValues = view.ToTable(true, "MatCode");
                        if (distinctValues.Rows.Count > 4)
                        {
                            _sResult = "GETSTACKSPLITSCANNEDSTACKDETAILS ~ ERROR ~ " + "The Current Scanned Stack Contains Diffrent Material Code And There are Already 4 Scanned Material Codes Exists";
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ResponceSentToAndroid - StackSplit_StackQRCodeDetails => ", "Responce - " + _sResult);
                            return _sResult;
                        }
                        frmMain.dtStackSplitData = dtCode.Copy();
                        //DataTable dt2 = dtCode.Copy();
                        //dt2.Columns.Remove("LocationCode");
                        //dt2.Columns.Remove("GradeDesc");
                        //dt2.Columns.Remove("GroupDesc");
                        //dt2.Columns.Remove("ThicknessDescription");
                        //dt2.Columns.Remove("Size");
                        //dt2.Columns.Remove("StackQRCode");
                        DataView view1 = new DataView(dtCode);
                        distinctValues = view1.ToTable(true, "MatCode", "TotalQty");
                        _sResult = "GETSTACKSPLITSCANNEDSTACKDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString(distinctValues);
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ResponceSentToAndroid - StackSplit_StackQRCodeDetails => ", "Responce - " + _sResult);
                        return _sResult;
                    }
                    else
                    {
                        _sResult = "GETSTACKSPLITSCANNEDSTACKDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ResponceSentToAndroid - StackSplit_StackQRCodeDetails => ", "Responce - " + _sResult);
                    }
                }
                else
                {
                    _sResult = "GETSTACKSPLITSCANNEDSTACKDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ResponceSentToAndroid - StackSplit_StackQRCodeDetails => ", "Responce - " + _sResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        public string PrintStackSplittingStackQRCode(string objLocationCode, DataTable dtData, DataTable newStackTable, string objNewStackQRCode, string oDateFormat, string _sUserID)  //, string GradeDesc, string GroupDesc, string ThicknessDesc, string MatSize, string LotSize
        {
            try
            {
                string OutMsg = string.Empty;
                var sReadPrn = string.Empty;
                sPrintStatus = string.Empty;
                sMatCode = string.Empty;
                sMatStatus = string.Empty;
                sGradeDesc = string.Empty;
                sGroupDesc = string.Empty;
                sThicknessDesc = string.Empty;
                sMatSize = string.Empty;
                int LotSize = 0;
                sSaveCount = 0;

                if (objLocationCode == "2000" && _sUserID.Contains("WH"))
                {
                    _bcilNetwork.PrinterIP = Properties.Settings.Default.WHDispatchStackQRCodePrinterIP;
                    _bcilNetwork.PrinterPort = Properties.Settings.Default.WHDispatchStackQRCodePrinterPort;
                }
                else
                {
                    _bcilNetwork.PrinterIP = Properties.Settings.Default.SegregationStackQRCodePrinterIP;
                    _bcilNetwork.PrinterPort = Properties.Settings.Default.SegregationStackQRCodePrinterPort;
                }

                string sPrintSection = Properties.Settings.Default.PrintingSection;
                string sLocationType = Properties.Settings.Default.PrintingLocationType;

                for (int i = 0; i < newStackTable.Rows.Count; i++)
                {
                    objLocationCode = newStackTable.Rows[i][0].ToString().Trim();
                    sMatCode = newStackTable.Rows[i][1].ToString().Trim();
                    sGradeDesc = newStackTable.Rows[i][2].ToString().Trim();
                    sGroupDesc = newStackTable.Rows[i][3].ToString().Trim();
                    sThicknessDesc = newStackTable.Rows[i][4].ToString().Trim();
                    sMatSize = newStackTable.Rows[i][5].ToString().Trim();
                    LotSize = Convert.ToInt32(newStackTable.Rows[i][9].ToString().Trim());
                    objQRCode = newStackTable.Rows[i][6].ToString().Trim();
                    objStackQRCode = newStackTable.Rows[i][7].ToString().Trim();
                    string sScanStatus = newStackTable.Rows[i][8].ToString().Trim();
                    if (sScanStatus == "Y")
                    {
                        string sSaveStatus = SaveStackSplittingStackQRCode(objLocationCode.Trim(), sMatCode.Trim(), objQRCode.Trim(), objStackQRCode.Trim(), objNewStackQRCode.Trim(), oDateFormat.Trim(), sPrintSection.Trim(), sLocationType.Trim(), _sUserID);
                        if (sSaveStatus.Contains("1"))
                        {
                            sSaveCount++;
                            //DataRow drNew = newStackTable.NewRow();
                            //drNew.ItemArray = dtData.Rows[i].ItemArray;
                            //newStackTable.Rows.Add(drNew);
                            //newStackTable.AcceptChanges();
                            //dtData.Rows[i].Delete();
                            //dtData.AcceptChanges();
                        }
                        else if (sSaveStatus.Contains("0"))
                        {
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackSplittingStackQRCode", "Barcode - " + objNewStackQRCode + " Not Update");
                        }
                    }
                }
                OutMsg = _bcilNetwork.NetworkPrinterStatus();
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintStackSplittingStackQRCode - PrintStackQRCode => ", "Printer Status : " + OutMsg + " of IPAddesss : " + _bcilNetwork.PrinterIP);
                if (OutMsg == "PRINTER READY")
                {
                    if (newStackTable.Rows.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        DataTable dt = new DataTable();
                        string sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplySegregationStackQRCode.PRN";
                        if (File.Exists(sPrnExist))
                        {
                            StreamReader sr = new StreamReader(sPrnExist);
                            sReadPrn = sr.ReadToEnd();
                            sr.Close();
                            sr.Dispose();
                        }
                        else
                        {
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackSplittingStackQRCode", "PRN File Not Found");
                            throw new Exception("PRN File Not Found");
                        }
                        //DataView view = new DataView(newStackTable);
                        //DataTable distinctValues1 = view.ToTable(true, "MatCode", "GradeDesc", "GroupDesc", "ThicknessDescription", "Size", "TotalQty");
                        DataTable distinctValues1 = newStackTable.DefaultView.ToTable(true, "MatCode");
                        distinctValues1.Columns.Add("GradeDesc");
                        distinctValues1.Columns.Add("GroupDesc");
                        distinctValues1.Columns.Add("ThicknessDesc");
                        distinctValues1.Columns.Add("Size");
                        distinctValues1.Columns.Add("TotalQty");
                        int iCount = 0;
                        for (int i = 0; i < distinctValues1.Rows.Count; i++)
                        {
                            string Finalmatcode = distinctValues1.Rows[i][0].ToString();
                            for (int j = 0; j < newStackTable.Rows.Count; j++)
                            {
                                string Stackmatcode = newStackTable.Rows[j][1].ToString();
                                if (Stackmatcode == Finalmatcode)
                                {
                                    iCount++;
                                    distinctValues1.Rows[i][0] = newStackTable.Rows[j][1].ToString();
                                    distinctValues1.Rows[i][1] = newStackTable.Rows[j][2].ToString();
                                    distinctValues1.Rows[i][2] = newStackTable.Rows[j][3].ToString();
                                    distinctValues1.Rows[i][3] = newStackTable.Rows[j][4].ToString();
                                    distinctValues1.Rows[i][4] = newStackTable.Rows[j][5].ToString();
                                    distinctValues1.Rows[i][5] = Convert.ToString(iCount);
                                }
                            }
                        }
                        sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objNewStackQRCode.Trim()));
                        sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(objNewStackQRCode.Trim()));
                        for (int i = 0; i < distinctValues1.Rows.Count; i++)
                        {
                            sMatCode = distinctValues1.Rows[i][0].ToString().Trim();
                            sGradeDesc = distinctValues1.Rows[i][1].ToString().Trim();
                            sGroupDesc = distinctValues1.Rows[i][2].ToString().Trim();
                            sThicknessDesc = distinctValues1.Rows[i][3].ToString().Trim();
                            sMatSize = distinctValues1.Rows[i][4].ToString().Trim();
                            LotSize = Convert.ToInt32(distinctValues1.Rows[i][5].ToString().Trim());

                            if (i == 0)
                            {
                                string ObjCode2 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                                sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(ObjCode2.Trim()));
                            }
                            if (i == 1)
                            {
                                string ObjCode3 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                                sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(ObjCode3.Trim()));
                            }
                            if (i == 2)
                            {
                                string ObjCode4 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                                sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(ObjCode4.Trim()));
                            }
                            if (i == 3)
                            {
                                string ObjCode5 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                                sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(ObjCode5.Trim()));
                            }
                        }
                        if (distinctValues1.Rows.Count == 1)
                        {
                            sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(""));
                            sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(""));
                            sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                        }
                        if (distinctValues1.Rows.Count == 2)
                        {
                            sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(""));
                            sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                        }
                        if (distinctValues1.Rows.Count == 3)
                        {
                            sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                        }
                        _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplySegregationStackQRCode.PRN";
                        for (int i = 0; i < 2; i++)
                        {
                            OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                            //OutMsg = "SUCCESS";
                        }
                        //_bcilNetwork.Dispose();
                        if (OutMsg == "SUCCESS")
                        {
                            sStackPrintCount++;
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackSplittingStackQRCode", "New Stack QRCode - " + objNewStackQRCode + " of " + newStackTable.Rows.Count + " Nos. Scanned QRCodes Saved And Printed Successfully");
                            OutMsg = "SUCCESS ~ Printed Successfully";
                        }
                    }
                    if (dtData.Rows.Count > 0)
                    {
                        StringBuilder sb = new StringBuilder();
                        DataTable dt = new DataTable();
                        string sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplySegregationStackQRCode.PRN";
                        if (File.Exists(sPrnExist))
                        {
                            StreamReader sr = new StreamReader(sPrnExist);
                            sReadPrn = sr.ReadToEnd();
                            sr.Close();
                            sr.Dispose();
                        }
                        else
                        {
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackSplittingStackQRCode", "PRN File Not Found");
                            throw new Exception("PRN File Not Found");
                        }

                        objStackQRCode = dtData.Rows[0][7].ToString();//  objLocationCode.Trim() + oDateFormat.Trim() + sStackRunningSerial;

                        //DataView view = new DataView(dtData);
                        //DataTable distinctValues2 = view.ToTable(true, "MatCode", "GradeDesc", "GroupDesc", "ThicknessDescription", "Size", "TotalQty");
                        DataTable distinctValues2 = dtData.DefaultView.ToTable(true, "MatCode");
                        distinctValues2.Columns.Add("GradeDesc");
                        distinctValues2.Columns.Add("GroupDesc");
                        distinctValues2.Columns.Add("ThicknessDesc");
                        distinctValues2.Columns.Add("Size");
                        distinctValues2.Columns.Add("TotalQty");
                        int iCount = 0;
                        for (int i = 0; i < distinctValues2.Rows.Count; i++)
                        {
                            string Finalmatcode = distinctValues2.Rows[i][0].ToString();
                            for (int j = 0; j < dtData.Rows.Count; j++)
                            {
                                string Stackmatcode = dtData.Rows[j][1].ToString();
                                if (Stackmatcode == Finalmatcode)
                                {
                                    iCount++;
                                    distinctValues2.Rows[i][0] = dtData.Rows[j][1].ToString();
                                    distinctValues2.Rows[i][1] = dtData.Rows[j][2].ToString();
                                    distinctValues2.Rows[i][2] = dtData.Rows[j][3].ToString();
                                    distinctValues2.Rows[i][3] = dtData.Rows[j][4].ToString();
                                    distinctValues2.Rows[i][4] = dtData.Rows[j][5].ToString();
                                    distinctValues2.Rows[i][5] = Convert.ToString(iCount);
                                }
                            }
                        }
                        sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objStackQRCode.Trim()));
                        sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(objStackQRCode.Trim()));
                        for (int i = 0; i < distinctValues2.Rows.Count; i++)
                        {
                            sMatCode = distinctValues2.Rows[i][0].ToString().Trim();
                            sGradeDesc = distinctValues2.Rows[i][1].ToString().Trim();
                            sGroupDesc = distinctValues2.Rows[i][2].ToString().Trim();
                            sThicknessDesc = distinctValues2.Rows[i][3].ToString().Trim();
                            sMatSize = distinctValues2.Rows[i][4].ToString().Trim();
                            LotSize = Convert.ToInt32(distinctValues2.Rows[i][5].ToString().Trim());

                            if (i == 0)
                            {
                                string ObjCode2 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                                sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(ObjCode2.Trim()));
                            }
                            if (i == 1)
                            {
                                string ObjCode3 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                                sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(ObjCode3.Trim()));
                            }
                            if (i == 2)
                            {
                                string ObjCode4 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                                sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(ObjCode4.Trim()));
                            }
                            if (i == 3)
                            {
                                string ObjCode5 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                                sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(ObjCode5.Trim()));
                            }
                        }
                        if (distinctValues2.Rows.Count == 1)
                        {
                            sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(""));
                            sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(""));
                            sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                        }
                        if (distinctValues2.Rows.Count == 2)
                        {
                            sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(""));
                            sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                        }
                        if (distinctValues2.Rows.Count == 3)
                        {
                            sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                        }
                        _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplySegregationStackQRCode.PRN";
                        for (int j = 0; j < 2; j++)
                        {
                            OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                            //OutMsg = "SUCCESS";
                        }
                        _bcilNetwork.Dispose();
                        if (OutMsg == "SUCCESS")
                        {
                            sStackPrintCount++;
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackSplittingStackQRCode", "New Stack QRCode - " + objNewStackQRCode + " of remaining " + dtData.Rows.Count + " Nos. QRCodes Saved And Printed Successfully");
                            OutMsg = "SUCCESS ~ Printed Successfully";
                        }
                    }
                }
                else
                {
                    if (OutMsg == "PRINTER NOT IN NETWORK")
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackSplittingStackQRCode", "ERROR ~ Stack Printer IP " + _bcilNetwork.PrinterIP + " not in network, Kindly connect printer to network");
                        return OutMsg = "Stack Printer IP - " + _bcilNetwork.PrinterIP + " not in network, Kindly connect printer to network";
                    }
                    else
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackSplittingStackQRCode", "ERROR ~ Stack Printer IP - " + _bcilNetwork.PrinterIP + ", Error is : " + OutMsg);
                        return OutMsg;
                    }
                }
                return OutMsg;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackSplittingStackQRCode", exDetail);
                return "ERROR | " + ex.Message;
            }
        }

        internal string SaveStackSplittingStackQRCode(string objLocationCode, string objMatCode, string objQRCode, string objStackQRCode, string objNewStackQRCode, string sDateFormat, string sPrintSection, string sLocType, string sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + objStackQRCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","SAVEUPDATESTACKSPLITSTACKQRCODEDETAILS"),
                                        new SqlParameter("@LocationCode", objLocationCode),
                                        new SqlParameter("@MatCode", objMatCode),
                                        new SqlParameter("@QRCode", objQRCode),
                                        new SqlParameter("@StackQRCode", objStackQRCode),
                                        new SqlParameter("@NewStackQRCode", objNewStackQRCode),
                                        new SqlParameter("@DateFormat", sDateFormat),
                                        new SqlParameter("@PrintSection", sPrintSection),
                                        new SqlParameter("@LocationType", sLocType),
                                        new SqlParameter("@CreatedBy", sUserId),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_StackSegregationPrinting", parma);
                if (dt.Columns.Contains("ERROR"))
                {
                    _sResult = "ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows[0][0].ToString() == "1")
                {
                    _sResult = "SUCCESS ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
                else
                {
                    _sResult = "ERROR ~ " + "NOT FOUND";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region MTM Transfer Material Scanned and Stack Printed

        internal string MTMScannedStackQRCodeDetails(string _sLocationCode, string _StackQRCode)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + _sLocationCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","MTMTRANSFERCHECKSCANNEDSTACKQRCODEDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@StackQRCode", _StackQRCode),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_StackSegregationPrinting", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    _sResult = "MTMTRANSFERCHECKSCANNEDSTACKQRCODEDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "MTMTRANSFERCHECKSCANNEDSTACKQRCODEDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("QRCode") && dt.Rows.Count == 1)
                {
                    if (frmMain.dtStackData.Columns.Count > 0)
                    {
                        DataRow _dtRow = frmMain.dtStackData.NewRow();
                        _dtRow["LocationCode"] = dt.Rows[0][0].ToString();
                        _dtRow["MatCode"] = dt.Rows[0][1].ToString();
                        _dtRow["QRCode"] = dt.Rows[0][2].ToString();
                        _dtRow["Status"] = dt.Rows[0][3].ToString();
                        _dtRow["GradeDesc"] = dt.Rows[0][4].ToString();
                        _dtRow["GroupDesc"] = dt.Rows[0][5].ToString();
                        _dtRow["ThicknessDesc"] = dt.Rows[0][6].ToString();
                        _dtRow["MatSize"] = dt.Rows[0][7].ToString();
                        frmMain.dtStackData.Rows.Add(_dtRow);
                    }
                    _sResult = "MTMTRANSFERCHECKSCANNEDSTACKQRCODEDETAILS ~ SUCCESS ~ " + dt.Rows[0][2].ToString();
                    return _sResult;
                }
                else
                {
                    _sResult = "MTMTRANSFERCHECKSCANNEDSTACKQRCODEDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        #endregion


        #region Material Scanned and Stack not Printed

        internal string GetDeleteScannedQRCodeDetails(string _sLocationCode, string _QRCode, string sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + _sLocationCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETDELETESCANNEDQRCODEDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode.Trim()),
                                        new SqlParameter("@QRCode", _QRCode.Trim()),
                                        new SqlParameter("@CreatedBy", sUserId.Trim()),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_StackSegregationPrinting", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    _sResult = "GETDELETESCANNEDQRCODEDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "GETDELETESCANNEDQRCODEDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("MatCode") && dt.Rows.Count == 1)
                {
                    DataRow _dtRow = frmMain.dtDeleteQRData.NewRow();
                    _dtRow["LocationCode"] = _sLocationCode.ToString();
                    _dtRow["MatCode"] = dt.Rows[0][0].ToString();
                    _dtRow["QRCode"] = dt.Rows[0][1].ToString();
                    _dtRow["UserID"] = sUserId.ToString();
                    frmMain.dtDeleteQRData.Rows.Add(_dtRow);
                    _sResult = "GETDELETESCANNEDQRCODEDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString(dt);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETDELETESCANNEDQRCODEDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string DeleteScannedQRCodesData(string _sLocationCode, string _MatCode, string _QRCode, string sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + _sLocationCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","DELETESCANNEDQRCODESDATA"),
                                        new SqlParameter("@LocationCode", _sLocationCode.Trim()),
                                        new SqlParameter("@MatCode", _MatCode.Trim()),
                                        new SqlParameter("@QRCode", _QRCode.Trim()),
                                        new SqlParameter("@CreatedBy", sUserId.Trim()),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_StackSegregationPrinting", parma);
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "DELETESCANNEDQRCODESDATA ~ ERROR ~ " + dt.Rows[0][0].ToString();
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows[0][0].ToString() == "1")
                {
                    _sResult = "DELETESCANNEDQRCODESDATA ~ SUCCESS ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows[0][0].ToString() == "0")
                {
                    _sResult = "DELETESCANNEDQRCODESDATA ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                else
                {
                    _sResult = "DELETESCANNEDQRCODESDATA ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        #endregion


        #region Material Scanned and Stack Printed

        internal string GetDeleteStackDetails(string _sLocationCode, string _sStackQRCode, string _sUserID)
        {
            dtStackData = new DataTable();
            DataTable dtCode = new DataTable();
            DataTable distinctValues = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + _sLocationCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETDELETESCANNEDSTACKDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@StackQRCode", _sStackQRCode),
                                        new SqlParameter("@CreatedBy", _sUserID),
                                   };
                dtCode = GlobalVariable._clsSql.GetDataUsingProcedure("USP_StackSegregationPrinting", parma);
                if (dtCode.Columns.Contains("ERROR") && dtCode.Rows.Count > 0)
                {
                    _sResult = "GETDELETESCANNEDSTACKDETAILS ~ ERROR ~ " + dtCode.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtCode.Columns.Contains("ErrorMessage") && dtCode.Rows.Count > 0)
                {
                    _sResult = "GETDELETESCANNEDSTACKDETAILS ~ ERROR ~ " + dtCode.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtCode.Columns.Contains("MatCode") && dtCode.Rows.Count > 0)
                {
                    //if (frmMain.dtStackDeleteData.Rows.Count == 0)
                    //{
                    //DataView view = new DataView(frmMain.dtStackSplitData);
                    //distinctValues = view.ToTable(true, "MatCode");
                    //if (distinctValues.Rows.Count > 4)
                    //{
                    //    _sResult = "GETDELETESCANNEDSTACKDETAILS ~ ERROR ~ " + "The Current Scanned Stack Contains Diffrent Material Code And There are Already 4 Scanned Material Codes Exists";
                    //    return _sResult;
                    //}
                    frmMain.dtStackDeleteData = dtCode.Copy();
                    DataView view1 = new DataView(dtCode);
                    distinctValues = view1.ToTable(true, "MatCode", "TotalQty");
                    _sResult = "GETDELETESCANNEDSTACKDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString(distinctValues);
                    return _sResult;
                    //}
                    //else
                    //{
                    //    _sResult = "GETDELETESCANNEDSTACKDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                    //}
                }
                else
                {
                    _sResult = "GETDELETESCANNEDSTACKDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string DeleteScannedQRCodes(string _sLocationCode, string _MatCode, string _QRCode, string _StackQRCode, string sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + _sLocationCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","DELETESCANNEDSTACKQRCODESDATA"),
                                        new SqlParameter("@LocationCode", _sLocationCode.Trim()),
                                        new SqlParameter("@MatCode", _MatCode.Trim()),
                                        new SqlParameter("@QRCode", _QRCode.Trim()),
                                        new SqlParameter("@StackQRCode", _StackQRCode.Trim()),
                                        new SqlParameter("@CreatedBy", sUserId.Trim()),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_StackSegregationPrinting", parma);
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "DELETESCANNEDSTACKQRCODESDATA ~ ERROR ~ " + dt.Rows[0][0].ToString();
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows[0][0].ToString() == "1")
                {
                    _sResult = "DELETESCANNEDSTACKQRCODESDATA ~ SUCCESS ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows[0][0].ToString() == "0")
                {
                    _sResult = "DELETESCANNEDSTACKQRCODESDATA ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                else
                {
                    _sResult = "DELETESCANNEDSTACKQRCODESDATA ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal DataTable GetFinalDeleteStackDetails(string _sLocationCode, string _sStackQRCode, string _sUserID)
        {
            DataTable dtCode = new DataTable();
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + _sLocationCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETDELETESCANNEDFINALSTACKDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@StackQRCode", _sStackQRCode),
                                        new SqlParameter("@CreatedBy", _sUserID),
                                   };
                dtCode = GlobalVariable._clsSql.GetDataUsingProcedure("USP_StackSegregationPrinting", parma);
                if (dtCode.Columns.Contains("ERROR") && dtCode.Rows.Count > 0)
                {
                    return dtCode;
                }
                if (dtCode.Columns.Contains("ErrorMessage") && dtCode.Rows.Count > 0)
                {
                    return dtCode;
                }
                if (dtCode.Columns.Contains("MatCode") && dtCode.Rows.Count > 0)
                {
                    return dtCode;
                }
                else
                {
                    return dtCode;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dtCode;
        }

        public string DeletedStackPrintNewStackQRCode(DataTable dtData, string objNewStackQRCode, string oDateFormat, string oLocCode, string _sUserID)  //, string GradeDesc, string GroupDesc, string ThicknessDesc, string MatSize, string LotSize
        {
            try
            {
                string OutMsg = string.Empty;
                var sReadPrn = string.Empty;
                sPrintStatus = string.Empty;
                sMatCode = string.Empty;
                sMatStatus = string.Empty;
                sGradeDesc = string.Empty;
                sGroupDesc = string.Empty;
                sThicknessDesc = string.Empty;
                sMatSize = string.Empty;
                int LotSize = 0;
                sSaveCount = 0;

                _bcilNetwork.PrinterIP = Properties.Settings.Default.SegregationStackQRCodePrinterIP;
                _bcilNetwork.PrinterPort = Properties.Settings.Default.SegregationStackQRCodePrinterPort;
                string sPrintSection = Properties.Settings.Default.PrintingSection;
                string sLocationType = Properties.Settings.Default.PrintingLocationType;

                for (int i = 0; i < dtData.Rows.Count; i++)
                {
                    objLocationCode = oLocCode; // dtData.Rows[i][0].ToString().Trim();
                    sMatCode = dtData.Rows[i][0].ToString().Trim();
                    sGradeDesc = dtData.Rows[i][1].ToString().Trim();
                    sGroupDesc = dtData.Rows[i][2].ToString().Trim();
                    sThicknessDesc = dtData.Rows[i][3].ToString().Trim();
                    sMatSize = dtData.Rows[i][4].ToString().Trim();
                    LotSize = Convert.ToInt32(dtData.Rows[i][6].ToString().Trim());
                    objStackQRCode = dtData.Rows[0][5].ToString().Trim();
                    string sSaveStatus = SaveDeletedStackNewStackQRCode(objLocationCode.Trim(), objStackQRCode.Trim(), objNewStackQRCode.Trim(), oDateFormat.Trim(), sPrintSection.Trim(), sLocationType.Trim(), _sUserID.Trim());
                    if (sSaveStatus.Contains("1"))
                    {
                        sSaveCount++;
                    }
                    else if (sSaveStatus.Contains("0"))
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "MTMTransfer_AfterStack", "Barcode - " + objNewStackQRCode + " Not Update");
                    }
                }
                OutMsg = _bcilNetwork.NetworkPrinterStatus();
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "MTMTransfer_AfterStack", "PrinterIP : " + _bcilNetwork.PrinterIP + ", PrinterStatus : " + OutMsg);
                if (OutMsg == "PRINTER READY")
                {
                    StringBuilder sb = new StringBuilder();
                    DataTable dt = new DataTable();
                    string sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplySegregationStackQRCode.PRN";
                    if (File.Exists(sPrnExist))
                    {
                        StreamReader sr = new StreamReader(sPrnExist);
                        sReadPrn = sr.ReadToEnd();
                        sr.Close();
                        sr.Dispose();
                    }
                    else
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "MTMTransfer_AfterStack", "PRN File Not Found");
                        throw new Exception("PRN File Not Found");
                    }

                    sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objNewStackQRCode.Trim()));
                    sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(objNewStackQRCode.Trim()));
                    for (int i = 0; i < dtData.Rows.Count; i++)
                    {
                        sMatCode = dtData.Rows[i][0].ToString().Trim();
                        sGradeDesc = dtData.Rows[i][1].ToString().Trim();
                        sGroupDesc = dtData.Rows[i][2].ToString().Trim();
                        sThicknessDesc = dtData.Rows[i][3].ToString().Trim();
                        sMatSize = dtData.Rows[i][4].ToString().Trim();
                        LotSize = Convert.ToInt32(dtData.Rows[i][6].ToString().Trim());

                        if (i == 0)
                        {
                            string ObjCode2 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(ObjCode2.Trim()));
                        }
                        if (i == 1)
                        {
                            string ObjCode3 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(ObjCode3.Trim()));
                        }
                        if (i == 2)
                        {
                            string ObjCode4 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(ObjCode4.Trim()));
                        }
                        if (i == 3)
                        {
                            string ObjCode5 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(ObjCode5.Trim()));
                        }
                    }
                    if (dtData.Rows.Count == 1)
                    {
                        sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                    }
                    if (dtData.Rows.Count == 2)
                    {
                        sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                    }
                    if (dtData.Rows.Count == 3)
                    {
                        sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                    }
                    _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplySegregationStackQRCode.PRN";
                    for (int i = 0; i < 2; i++)
                    {
                        OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                        //OutMsg = "SUCCESS";
                    }
                    _bcilNetwork.Dispose();
                    if (OutMsg == "SUCCESS")
                    {
                        sStackPrintCount++;
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "MTMTransfer_AfterStack", "Stack QRCode - " + objNewStackQRCode + " Saved And Printed Successfully at " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                        return OutMsg = "SUCCESS~Printed Successfully";
                    }
                }
                else
                {
                    if (OutMsg == "PRINTER NOT IN NETWORK")
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "MTMTransfer_AfterStack", "ERROR ~ StackQRCode Printer IP : " + _bcilNetwork.PrinterIP + "not in network");
                        return OutMsg = "ERROR~Printer not in network";
                    }
                    else
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "MTMTransfer_AfterStack", "ERROR~ StackQRCode Printer IP : " + _bcilNetwork.PrinterIP + ", Error is : " + OutMsg);
                        return OutMsg;
                    }
                }
                return OutMsg;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "MTMTransfer_AfterStack", exDetail);
                return "ERROR | " + ex.Message;
            }
        }

        internal string SaveDeletedStackNewStackQRCode(string objLocationCode, string objStackQRCode, string objNewStackQRCode, string sDateFormat, string sPrintSection, string sLocType, string sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + objStackQRCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","SAVEUPDATEDELETESTACKSTACKQRCODEDETAILS"),
                                        new SqlParameter("@LocationCode", objLocationCode),
                                        new SqlParameter("@StackQRCode", objStackQRCode),
                                        new SqlParameter("@NewStackQRCode", objNewStackQRCode),
                                        new SqlParameter("@DateFormat", sDateFormat),
                                        new SqlParameter("@PrintSection", sPrintSection),
                                        new SqlParameter("@LocationType", sLocType),
                                        new SqlParameter("@CreatedBy", sUserId),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_StackSegregationPrinting", parma);
                if (dt.Columns.Contains("ERROR"))
                {
                    _sResult = "ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows[0][0].ToString() == "1")
                {
                    _sResult = "SUCCESS ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
                else
                {
                    _sResult = "ERROR ~ " + "NOT FOUND";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

    }
}
