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
using System.IO;

namespace GreenplyCommServer.BI
{
    class B_LoadingOffloading
    {
        BcilNetwork _bcilNetwork = new BcilNetwork();
        public static int sCount = 0;
        public static string sMatCode = string.Empty;
        public static DataTable dtStackQRCode = new DataTable();
        public static int sStackPrintCount = 0;
        public static int sSaveCount = 0;
        string objLocationCode;
        string objQRCode = string.Empty;
        string objStackQRCode = string.Empty;
        string sPrintStatus = string.Empty;
        string sobjMatCode = string.Empty;
        string sMatStatus = string.Empty;
        string sGradeDesc = string.Empty;
        string sGroupDesc = string.Empty;
        string sThicknessDesc = string.Empty;
        string sMatSize = string.Empty;

        internal string GetDeliveryOrderNumbersDetails(string _sLocationCode, string _sDONo)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + _sLocationCode);
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETDELIVERYORDERDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@DONo", _sDONo),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LoadingOffloading", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    _sResult = "GETDELIVERYORDERDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "GETDELIVERYORDERDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("CustomerName") && dt.Rows.Count > 1)
                {
                    _sResult = "GETDELIVERYORDERDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString(dt);
                    return _sResult;
                }
                if (dt.Columns.Contains("CustomerName") && dt.Rows.Count == 1)
                {
                    _sResult = "GETDELIVERYORDERDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString2(dt);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETDELIVERYORDERDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string GetStackQRCodeDetails(string _sLocationCode, string _sStackQRCode)
        {
            dtStackQRCode = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + _sLocationCode);
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETSTACKQRCODEDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@StackQRCode", _sStackQRCode),
                                   };
                dtStackQRCode = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LoadingOffloading", parma);
                if (dtStackQRCode.Columns.Contains("ERROR") && dtStackQRCode.Rows.Count > 0)
                {
                    _sResult = "GETSTACKQRCODEDETAILS ~ ERROR ~ " + dtStackQRCode.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtStackQRCode.Columns.Contains("ErrorMessage") && dtStackQRCode.Rows.Count > 0)
                {
                    _sResult = "GETSTACKQRCODEDETAILS ~ ERROR ~ " + dtStackQRCode.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtStackQRCode.Columns.Contains("QRCode") && dtStackQRCode.Rows.Count > 0)
                {
                    //sMatCode = dtStackQRCode.Rows[0][1].ToString();
                    //dtStackQRCode.Columns.Remove("MatCode");
                    _sResult = "GETSTACKQRCODEDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString(dtStackQRCode);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETSTACKQRCODEDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string GetQRCodeDetails(string _sLocationCode, string _sQRCode)
        {
            dtStackQRCode = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + _sLocationCode);
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETQRCODEDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@QRCode", _sQRCode),
                                   };
                dtStackQRCode = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LoadingOffloading", parma);
                if (dtStackQRCode.Columns.Contains("ERROR") && dtStackQRCode.Rows.Count > 0)
                {
                    _sResult = "GETQRCODEDETAILS ~ ERROR ~ " + dtStackQRCode.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtStackQRCode.Columns.Contains("ErrorMessage") && dtStackQRCode.Rows.Count > 0)
                {
                    _sResult = "GETQRCODEDETAILS ~ ERROR ~ " + dtStackQRCode.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtStackQRCode.Columns.Contains("QRCode") && dtStackQRCode.Rows.Count > 1)
                {
                    sMatCode = dtStackQRCode.Rows[0][1].ToString();
                    dtStackQRCode.Columns.Remove("MatCode");
                    _sResult = "GETQRCODEDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString(dtStackQRCode);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETQRCODEDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        public string PrintStackQRCodeItem(DataTable dtData, string sStackQRCode)  //string _strLocationCode, string sMatCode, string sQRCode, string sStackQRCode, string sDateFormat, string sUserId
        {
            try
            {
                string OutMsg = string.Empty;
                var sReadPrn = string.Empty;
                sPrintStatus = string.Empty;
                sobjMatCode = string.Empty;
                sMatStatus = string.Empty;
                sGradeDesc = string.Empty;
                sGroupDesc = string.Empty;
                sThicknessDesc = string.Empty;
                sMatSize = string.Empty;
                int LotSize = 0;
                sSaveCount = 0;
                objQRCode = string.Empty;
                objStackQRCode = string.Empty;
                objStackQRCode = sStackQRCode.Trim();
                _bcilNetwork.PrinterIP = Properties.Settings.Default.StackQRCodePrinterIP;
                _bcilNetwork.PrinterPort = Properties.Settings.Default.StackQRCodePrinterPort;

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
                    DataTable distinct = dtData.DefaultView.ToTable(true, "MatCode", "GradeDesc", "GroupDesc", "ThicknessDesc", "Size", "TotalQty");
                    //distinct.Columns.Add("TotalQty");
                    //int iCount = 0;
                    //for (int i = 0; i < distinct.Rows.Count; i++)
                    //{
                    //    iCount = 0;
                    //    string Finalmatcode = distinct.Rows[i][0].ToString();
                    //    for (int j = 0; j < dtData.Rows.Count; j++)
                    //    {
                    //        string Stackmatcode = dtData.Rows[j][1].ToString();
                    //        if (Stackmatcode == Finalmatcode)
                    //        {
                    //            iCount++;
                    //            distinct.Rows[i][0] = dtData.Rows[j][1].ToString();
                    //            distinct.Rows[i][1] = dtData.Rows[j][5].ToString();
                    //            distinct.Rows[i][2] = dtData.Rows[j][6].ToString();
                    //            distinct.Rows[i][3] = dtData.Rows[j][7].ToString();
                    //            distinct.Rows[i][4] = dtData.Rows[j][8].ToString();
                    //            //distinct.Rows[i][5] = dtData.Rows[j][5].ToString();
                    //            distinct.Rows[i][5] = Convert.ToString(iCount);
                    //        }
                    //    }
                    //}

                    sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objStackQRCode.Trim()));
                    sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(objStackQRCode.Trim()));

                    for (int i = 0; i < distinct.Rows.Count; i++)
                    {
                        sMatCode = distinct.Rows[i][0].ToString().Trim();
                        sGradeDesc = distinct.Rows[i][1].ToString().Trim();
                        sGroupDesc = distinct.Rows[i][2].ToString().Trim();
                        sThicknessDesc = distinct.Rows[i][3].ToString().Trim();
                        sMatSize = distinct.Rows[i][4].ToString().Trim();
                        string sLotSize = distinct.Rows[i][5].ToString().Trim();
                        if (i == 0)
                        {
                            string ObjCode2 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + sLotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(ObjCode2.Trim()));
                        }
                        if (i == 1)
                        {
                            string ObjCode3 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + sLotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(ObjCode3.Trim()));
                        }
                        if (i == 2)
                        {
                            string ObjCode4 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + sLotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(ObjCode4.Trim()));
                        }
                        if (i == 3)
                        {
                            string ObjCode5 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + sLotSize + " Nos.";
                            sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(ObjCode5.Trim()));
                        }
                    }
                    if (distinct.Rows.Count == 1)
                    {
                        sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                    }
                    if (distinct.Rows.Count == 2)
                    {
                        sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(""));
                        sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                    }
                    if (distinct.Rows.Count == 3)
                    {
                        sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(""));
                    }

                    //for (int i = 0; i < dtData.Rows.Count; i++)
                    //{
                    //    sMatCode = dtData.Rows[i][1].ToString().Trim();
                    //    sGradeDesc = dtData.Rows[i][5].ToString().Trim();
                    //    sGroupDesc = dtData.Rows[i][6].ToString().Trim();
                    //    sThicknessDesc = dtData.Rows[i][7].ToString().Trim();
                    //    sMatSize = dtData.Rows[i][8].ToString().Trim();
                    //    LotSize = Convert.ToInt32(dtData.Rows[i][9].ToString().Trim());
                    //    if (i == 0)
                    //    {
                    //        string ObjCode2 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                    //        sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(ObjCode2.Trim()));
                    //    }
                    //    if (i == 1)
                    //    {
                    //        string ObjCode3 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                    //        sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(ObjCode3.Trim()));
                    //    }
                    //    if (i == 2)
                    //    {
                    //        string ObjCode4 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                    //        sReadPrn = sReadPrn.Replace("{VarBarcode5}", Convert.ToString(ObjCode4.Trim()));
                    //    }
                    //    if (i == 3)
                    //    {
                    //        string ObjCode5 = sGradeDesc.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sMatSize.Trim() + "- " + LotSize + " Nos.";
                    //        sReadPrn = sReadPrn.Replace("{VarBarcode6}", Convert.ToString(ObjCode5.Trim()));
                    //    }
                    //}

                    _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplySegregationStackQRCode.PRN";
                    for (int i = 0; i < 2; i++)
                    {
                        OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                    }
                    _bcilNetwork.Dispose();
                    if (OutMsg == "SUCCESS")
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", "Stack QRCode - " + objStackQRCode + " Saved And Printed Successfully at " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                        return OutMsg;// = "SUCCESS~Printed Successfully";
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

        internal string SaveUpdateStackQRCode(string objLocationCode, string DONo, string objMatCode, string objQRCode, string objStackQRCode, string objNewStackQRCode, string sDateFormat, string sUserID)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + objStackQRCode);
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","SAVEUPDATESTACKQRCODE"),
                                        new SqlParameter("@LocationCode", objLocationCode),
                                        new SqlParameter("@DONo", DONo.Trim().ToString()),
                                        new SqlParameter("@MatCode", objMatCode),
                                        new SqlParameter("@QRCode", objQRCode),
                                        new SqlParameter("@StackQRCode", objStackQRCode),
                                        new SqlParameter("@NewStackQRCode", objNewStackQRCode),
                                        new SqlParameter("@DateFormat", sDateFormat),
                                        new SqlParameter("@CreatedBy", sUserID),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LoadingOffloading", parma);
                if (dt.Columns.Contains("ERROR"))
                {
                    _sResult = "ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows[0][0].ToString() == "1")
                {
                    sCount++;
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

        internal string UpdateStackItemSerial(string objLocationCode, string objNewStackQRCode, string sDateFormat, string sPrintingSection, string sLocationType)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + objNewStackQRCode);
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","UPDATESTACKSERIAL"),
                                        new SqlParameter("@LocationCode", objLocationCode.Trim().ToString()),
                                        new SqlParameter("@NewStackQRCode", objNewStackQRCode.Trim().ToString()),
                                        new SqlParameter("@DateFormat", sDateFormat.Trim().ToString()),
                                        new SqlParameter("@PrintSection", sPrintingSection.Trim().ToString()),
                                        new SqlParameter("@LocationType", sLocationType.Trim().ToString()),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LoadingOffloading", parma);
                if (dt.Columns.Contains("ERROR"))
                {
                    _sResult = "ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows[0][0].ToString() == "1")
                {
                    sCount++;
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

        internal string SaveLoadOffloadQRCodeData(string sLocationCode, string sDONo, string sMatCode, string sQRCode, string sStackQRCode, string sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + sQRCode);
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","SAVELOADOFFLOADDATA"),
                                        new SqlParameter("@LocationCode", sLocationCode),
                                        new SqlParameter("@DONo", sDONo),
                                        new SqlParameter("@MatCode", sMatCode),
                                        new SqlParameter("@QRCode", sQRCode),
                                        new SqlParameter("@StackQRCode", sStackQRCode),
                                        new SqlParameter("@CreatedBy", sUserId),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LoadingOffloading", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    _sResult = "SAVELOADOFFLOADDATA ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "SAVELOADOFFLOADDATA ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    _sResult = "SAVELOADOFFLOADDATA ~ SUCCESS ~ "; // GlobalVariable.DtToString(dt);
                    return _sResult;
                }
                else
                {
                    _sResult = "SAVELOADOFFLOADDATA ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string UpdateLoadOffloadQRCodeSAPStatus(string sLocationCode, string sDONo, string sMatCode, string sQRCode, string sPostMsg, string sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + sQRCode);
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","UPDATESAPPOSTEDSTATUS"),
                                        new SqlParameter("@LocationCode", sLocationCode),
                                        new SqlParameter("@DONo", sDONo),
                                        new SqlParameter("@MatCode", sMatCode),
                                        new SqlParameter("@QRCode", sQRCode),
                                        new SqlParameter("@SAPPostMsg", sPostMsg),
                                        new SqlParameter("@CreatedBy", sUserId),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LoadingOffloading", parma);
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

        internal string GetDODetailsAgainstDelivery(string _sLocationCode, string _sDONo)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + _sLocationCode);
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETDELIVERYORDERDETAILSAGAINSTOFFLOAD"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@DONo", _sDONo),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LoadingOffloading", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    _sResult = "GETDELIVERYORDERDETAILSAGAINSTOFFLOAD ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "GETDELIVERYORDERDETAILSAGAINSTOFFLOAD ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("CustomerName") && dt.Rows.Count > 1)
                {
                    _sResult = "GETDELIVERYORDERDETAILSAGAINSTOFFLOAD ~ SUCCESS ~ " + GlobalVariable.DtToString(dt);
                    return _sResult;
                }
                if (dt.Columns.Contains("CustomerName") && dt.Rows.Count == 1)
                {
                    _sResult = "GETDELIVERYORDERDETAILSAGAINSTOFFLOAD ~ SUCCESS ~ " + GlobalVariable.DtToString2(dt);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETDELIVERYORDERDETAILSAGAINSTOFFLOAD ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string SaveUpdateDamagedQRCode(string sLocCode, string sDONo, string sMatCode, string sQRCode, string sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + objStackQRCode);
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","SAVEUPDATEDAMAGEDSTATUS"),
                                        new SqlParameter("@LocationCode", sLocCode),
                                        new SqlParameter("@DONo", sDONo),
                                        new SqlParameter("@MatCode", sMatCode),
                                        new SqlParameter("@QRCode", sQRCode),
                                        new SqlParameter("@CreatedBy", sUserId),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LoadingOffloading", parma);
                if (dt.Columns.Contains("ERROR"))
                {
                    _sResult = "ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows[0][0].ToString() == "1")
                {
                    sCount++;
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

    }
}
