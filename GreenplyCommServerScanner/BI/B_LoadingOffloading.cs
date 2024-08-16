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
using System.Data;

namespace GreenplyScannerCommServer.BI
{
    class B_LoadingOffloading
    {
        BcilNetwork _bcilNetwork = new BcilNetwork();
        public static int sCount = 0;
        public static string sMatCode = string.Empty;
        public static string oMatCode = string.Empty;
        public static DataTable dtStackQRCode = new DataTable();
        public static DataTable dtdisQRCode = new DataTable();
        DataTable dtQRCode = new DataTable();
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
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetDispatchDeliveryOrderDetails - RequestDataFromAndroid => ", "LocationCode : " + _sLocationCode + ", DONo. : " + _sDONo);
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
                DataRow[]rw = dt.Select("RemainingQty > 0");
                if(rw.Length == 0)
                {
                    _sResult = "GETDELIVERYORDERDETAILS ~ COMPLETE";
                    return _sResult;
                }
                else if (dt.Columns.Contains("CustomerName") && dt.Rows.Count > 0)
                {
                    _sResult = "GETDELIVERYORDERDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString(dt);
                    return _sResult;
                }
                //if (dt.Columns.Contains("CustomerName") && dt.Rows.Count == 1)
                //{
                //    _sResult = "GETDELIVERYORDERDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString2(dt);
                //    return _sResult;
                //}
                else
                {
                    _sResult = "GETDELIVERYORDERDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetDispatchDeliveryOrderDetails - Exception =>", ex.ToString());
                throw ex;
            }
            return _sResult;
        }

        internal DataTable GetDTStackQRCodeDetails(string _sLocationCode, string _sDONo, string _sStackQRCode, string _sUserID)
        {
            dtStackQRCode = new DataTable();
            dtQRCode = new DataTable();
            string _sResult = string.Empty;
            //GlobalVariable._clsSql.TranStartTransaction.GetDTStackQRCodeDetails();
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetDispatchStackQRCodeDetails - RequestDataFromAndroid => ", "LocationCode : " + _sLocationCode + ", DONo : " + _sDONo + ", StackQRCode : " + _sStackQRCode + ", UserId : " + _sUserID);
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETSTACKQRCODEDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@DONo", _sDONo),
                                        new SqlParameter("@StackQRCode", _sStackQRCode),
                                        new SqlParameter("@CreatedBy", _sUserID),
                                   };
                dtStackQRCode = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LoadingOffloading", parma);
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetDispatchStackQRCodeDetails - Exception => ", ex.Message.ToString());
                throw ex;
            }
            return dtStackQRCode;;
        }

        internal DataTable GetDTStackQRCodeInitData(string _sLocationCode, string _sDONo, string _sStackQRCode, string _sUserID, string _sType)
        {
            dtStackQRCode = new DataTable();
            dtQRCode = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetDispatchLoadOffloadInitQty - RequestDataFromAndroid => ", "LocationCode : " + _sLocationCode + ", DONo : " + _sDONo + ", StackQRCode : " + _sStackQRCode + ", Type : " + _sType + ", UserId : " + _sUserID);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETOFFLOADUPLOADDATA"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@DONo", _sDONo),
                                        new SqlParameter("@StackQRCode", _sStackQRCode),
                                        new SqlParameter("@CreatedBy", _sUserID),
                                        new SqlParameter("@SMergeType", _sType),
                                   };
                dtStackQRCode = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LoadingOffloading", parma);
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetDispatchLoadOffloadInitQty - Exception => ", ex.Message.ToString());
                throw ex;
            }
            return dtStackQRCode; ;
        }

        internal DataTable UPDATEDTScannedQRCodeData(string _sLocationCode, string _sDONo, string _sQRCode, string _sStackQRCode, string _sType, string _sUserID )
        {
            //
            dtStackQRCode = new DataTable();
            dtQRCode = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateLoadOffloadScannedQRCodeStatus - RequestedDataFromAndroid => ", "LocationCode : " + _sLocationCode + ", DONo : " + _sDONo + ", StackQRCode : " + _sStackQRCode +  ", QRCode : " + _sQRCode + ", Type : " + _sType + ", UserId : " + _sUserID);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETSCANNEDQRCODEDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@DONo", _sDONo),
                                        new SqlParameter("@StackQRCode", _sStackQRCode),
                                        new SqlParameter("@CreatedBy", _sUserID),
                                        new SqlParameter("@QRCode", _sQRCode),
                                        new SqlParameter("@SMergeType", _sType),
                                   };
                dtStackQRCode = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LoadingOffloading", parma);
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateLoadOffloadScannedQRCodeStatus - Exception => ", ex.Message.ToString());
                throw ex;
            }
            return dtStackQRCode; ;
        }

        internal DataTable ClearLoadOffloadTempData(string _sLocationCode, string _sDONo, string _sStackQRCode, string _sUserID)
        {
            dtStackQRCode = new DataTable();
            dtQRCode = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ClearLoadOffloadTempData", "RequestedDataFromAndroid => LocationCode : " + _sLocationCode + ", DONo : " + _sDONo + ", StackQRCode : " + _sStackQRCode + ", UserId : " + _sUserID);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","CLEARLOADOFFLOADTEMPTABLE"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@DONo", _sDONo),
                                        new SqlParameter("@StackQRCode", _sStackQRCode),
                                        new SqlParameter("@CreatedBy", _sUserID),
                                   };
                dtStackQRCode = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LoadingOffloading", parma);
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ClearLoadOffloadTempData", "Exception => " + ex.Message.ToString());
                throw ex;
            }
            return dtStackQRCode; ;
        }

        internal DataTable GetOffloadPrintStackData(string _sLocationCode, string _sDONo, string _sStackQRCode, string _sType, string _sUserID)
        {

            dtStackQRCode = new DataTable();
            dtQRCode = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintOffloadStackData", "RequestedDataFromAndroid => LocationCode : " + _sLocationCode + ", DONo : " + _sDONo + ", StackQRCode : " + _sStackQRCode + ", Type : " + _sType + ", UserId : " + _sUserID);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETOFFLOADPRINTSTACKDATA"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@DONo", _sDONo),
                                        new SqlParameter("@StackQRCode", _sStackQRCode),
                                        new SqlParameter("@CreatedBy", _sUserID),
                                        new SqlParameter("@SMergeType", _sType),
                                   };
                dtStackQRCode = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LoadingOffloading", parma);
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintOffloadStackData", "Exception => " + ex.ToString());
                throw ex;
            }
            return dtStackQRCode; ;
        }




        internal string GetQRCodeDetails(string _sLocationCode, string _sQRCode)
        {
            dtStackQRCode = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "DepotDispatchScannedQRCodeDetails - RequestedDataFromAndroid => ", "LocationCode : " + _sLocationCode + " QRCode : " + _sQRCode);
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
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "DepotDispatchScannedQRCodeDetails - Exception => ", ex.ToString());
                throw ex;
            }
            return _sResult;
        }

        public string PrintStackQRCodeItem(DataTable dtData, string sStackQRCode, string _sUserId)  //string _strLocationCode, string sMatCode, string sQRCode, string sStackQRCode, string sDateFormat, string sUserId
        {
            try
            {
                _bcilNetwork = new BcilNetwork();
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

                if (GlobalVariable.mSiteCode == "2000" && _sUserId.Contains("WH"))
                {
                    _bcilNetwork.PrinterIP = Properties.Settings.Default.WHDispatchStackQRCodePrinterIP;
                    _bcilNetwork.PrinterPort = Properties.Settings.Default.WHDispatchStackQRCodePrinterPort;
                }
                else
                {
                    _bcilNetwork.PrinterIP = Properties.Settings.Default.DispatchStackQRCodePrinterIP;
                    _bcilNetwork.PrinterPort = Properties.Settings.Default.DispatchStackQRCodePrinterPort;
                }

                OutMsg = _bcilNetwork.NetworkPrinterStatus();
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintLoadOffloadStackQRCode - PrintStackQRCode => ", "Printer Status : " + OutMsg + " of IPAddesss : " + _bcilNetwork.PrinterIP);
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
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintLoadOffloadStackQRCode - PrintStackQRCodeItem => ", "GreenplySegregationStackQRCode PRN File Not Found");
                        throw new Exception("PRN File Not Found");
                    }
                    DataTable distinct = dtData.DefaultView.ToTable(true, "MatCode", "GradeDesc", "GroupDesc", "ThicknessDesc", "Size", "TotalQty");

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

                    _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplySegregationStackQRCode.PRN";
                    for (int j = 0; j < 2; j++)
                    {
                        OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                        //OutMsg = "SUCCESS";
                    }
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintLoadOffloadStackQRCode - PrintStackQRCodeItem => ", "Stack " + objStackQRCode + " Print Status => " + OutMsg + " of IPAddesss : " + _bcilNetwork.PrinterIP);
                    _bcilNetwork.Dispose();
                    if (OutMsg == "SUCCESS")
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintLoadOffloadStackQRCode - PrintStackQRCodeItem => ", "Stack QRCode - " + objStackQRCode + " Saved And Printed Successfully");
                        return OutMsg;
                    }
                }
                else
                {
                    if (OutMsg == "PRINTER NOT IN NETWORK")
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintLoadOffloadStackQRCode - PrintStackQRCodeItem => ", "ERROR ~ Stack QRCode Printer IP : "+ _bcilNetwork.PrinterIP + " not in network, Kindly check the network");
                        OutMsg = "ERROR ~ Print LoadOffload Stack QRCode Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network";
                        return OutMsg;
                    }
                    else
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintLoadOffloadStackQRCode - PrintStackQRCodeItem => ", "ERROR ~ Stack QRCode Printer IP : " + _bcilNetwork.PrinterIP + " found error, Error - " + OutMsg);
                        OutMsg = "ERROR ~ Stack QRCode Printer IP : " + _bcilNetwork.PrinterIP + " found error, Error - " + OutMsg;
                        return OutMsg;
                    }
                }
                return OutMsg;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintLoadOffloadStackQRCode - PrintStackQRCodeItem => ", "Exception : " + ex.ToString());
                return "ERROR | " + ex.Message;
            }
        }

        internal string SaveUpdateStackQRCode(string objLocationCode, string DONo, string objMatCode, string objQRCode, string objStackQRCode, string objNewStackQRCode, string sDateFormat, string sUserID, string _sType, string sPrintingSection, string sLocationType)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintLoadOffloadStackQRCode - SaveUpdateStackQRCode => ", "LocationCode : " + objLocationCode + ", DONo : " + DONo + ", OldStackQRCode : " + objStackQRCode + ", NewStackQRCode : " + objNewStackQRCode + ", Type : " + _sType + ", UserId : " + sUserID);
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","SAVEUPDATESTACKQRCODE"),
                                        new SqlParameter("@LocationCode", objLocationCode),
                                        new SqlParameter("@DONo", DONo.Trim().ToString()),
                                        //new SqlParameter("@MatCode", objMatCode),
                                        //new SqlParameter("@QRCode", objQRCode),
                                        new SqlParameter("@StackQRCode", objStackQRCode),
                                        new SqlParameter("@NewStackQRCode", objNewStackQRCode),
                                        new SqlParameter("@DateFormat", sDateFormat),
                                        new SqlParameter("@CreatedBy", sUserID),
                                        new SqlParameter("@SMergeType", _sType),
                                        new SqlParameter("@PrintSection", sPrintingSection.Trim().ToString()),
                                        new SqlParameter("@LocationType", sLocationType.Trim().ToString()),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LoadingOffloading", parma);
                if (dt.Columns.Contains("ERROR"))
                {
                    _sResult = "ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintLoadOffloadStackQRCode - SaveUpdateNewStackQRCode => ", "ERROR of ScannedStackQrcode : " + _sResult);
                    return _sResult;
                }
                else if(dt.Columns.Contains("STATUS"))  //&& dt.Rows[0][0].ToString() == "1"
                { 
                    sCount++;
                    _sResult = "SUCCESS ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintLoadOffloadStackQRCode - SaveUpdateNewStackQRCode => ", "Update completed of ScannedStackQrcode - " + objStackQRCode + " With New StackQRcode - " + objNewStackQRCode + " of DONo - " + DONo);
                    return _sResult;
                }
                else if (dt.Columns.Contains("ErrorMessage"))
                {
                    _sResult = "ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintLoadOffloadStackQRCode - SaveUpdateNewStackQRCode => ", "ErrorMessage of ScannedStackQrcode : " + _sResult);
                    return _sResult;
                }
                return _sResult;
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintLoadOffloadStackQRCode - SaveUpdateNewStackQRCode => ", "Exception => " + ex.ToString());
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
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "DepotDispatchSaveScannedData - RequestedDataFromAndroid => ", "LocationCode : " + sLocationCode + " DONo : " + sDONo + " MatCode : " + sMatCode + " QRCode : " + sQRCode + " StackQRCode : " + sStackQRCode + " Userid : " + sUserId);
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
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "DepotDispatchSaveScannedData => ", "ERROR of - " + sStackQRCode + " And DONo - " + sDONo + " And Matcode - " + sMatCode + " And at QRCode - " + sQRCode);
                    _sResult = "SAVELOADOFFLOADDATA ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "DepotDispatchSaveScannedData => ", "ErrorMessage of - " + sStackQRCode + " And DONo - " + sDONo + " And Matcode - " + sMatCode + " And at QRCode - " + sQRCode);
                    _sResult = "SAVELOADOFFLOADDATA ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "DepotDispatchSaveScannedData => ", "DataSaved of - " + sStackQRCode + " And DONo - " + sDONo + " And Matcode - " + sMatCode + " And at QRCode - " + sQRCode);
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
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "DepotDispatchSaveScannedData => Exception : ", ex.ToString());
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



        // Material Offload against delivery start
        internal string GetDODetailsAgainstDelivery(string _sLocationCode, string _sDONo)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "DODetailsAgainstMatOffload - RequestDataFromAndroid => ", "LocationCode : " + _sLocationCode + ", DONo : " + _sDONo); ;
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETDELIVERYORDERDETAILSAGAINSTOFFLOAD"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@DONo", _sDONo),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_DeliveryCancellation", parma);
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "DODetailsAgainstMatOffload => ", "No. Of Record count => " + dt.Rows.Count.ToString());
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
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "DODetailsAgainstMatOffload => ", "Exception => " + ex.Message.ToString());
                throw ex;
            }
            return _sResult;
        }

        internal string SaveUpdateDamagedQRCode(string sLocCode, string sDONo, string sQRCode, string sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "OffloadAgainstDelivery - RequestDataFromAndroid => ", "LocationCode : " + sLocCode + ", DONo : " + sDONo + ", MatCode : " + sMatCode + ", QRCode : " + sQRCode + ", UserId : " + sUserId); ;
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
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_DeliveryCancellation", parma);
                if (dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage"))
                {
                    _sResult = "ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "OffloadAgainstDelivery => ", "ERROR : DONo - " + sDONo + ", QRCode - " + sQRCode + "  ");
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows[0][0].ToString() == "1")
                {
                    sCount++;
                    _sResult = "SUCCESS ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "OffloadAgainstDelivery => ", "DataSaved As Damaged of  DONo - " + sDONo + ", Matcode - " + sMatCode + ", QRCode - " + sQRCode);
                    return _sResult;
                }
                else
                {
                    _sResult = "ERROR ~ " + "NOT FOUND";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "OffloadAgainstDelivery => ", "Error As Damaged of DONo - " + sDONo + ", Matcode - " + sMatCode + ", QRCode - " + sQRCode);
                    return _sResult;
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "OffloadAgainstDelivery => ", "Exception : " + ex.Message.ToString());
                throw ex;
            }
        }


        // Material Offload against delivery end 



        #region Deport And Door Dispatch

        internal string GetDispatchDeliveryOrderNoDetails(string _sLocationCode, string _sDONo)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetDispatchDeliveryOrderNoDetails - RequestDataFromAndroid => ", "LocationCode - " + _sLocationCode + ", DONo. - " + _sDONo);
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETDISPATCHDELIVERYORDERDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@DONo", _sDONo),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LoadingOffloading", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    _sResult = "GETDISPATCHDELIVERYORDERDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetDispatchDeliveryOrderNoDetails - ResponceSentToAndroid => ", "Responce -" + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "GETDISPATCHDELIVERYORDERDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetDispatchDeliveryOrderNoDetails - ResponceSentToAndroid => ", "Responce -" + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("CustomerName") && dt.Rows.Count > 1)
                {
                    _sResult = "GETDISPATCHDELIVERYORDERDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString(dt);
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetDispatchDeliveryOrderNoDetails - ResponceSentToAndroid => ", "Responce -" + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("CustomerName") && dt.Rows.Count == 1)
                {
                    _sResult = "GETDISPATCHDELIVERYORDERDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString2(dt);
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetDispatchDeliveryOrderNoDetails - ResponceSentToAndroid => ", "Responce -" + _sResult);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETDISPATCHDELIVERYORDERDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetDispatchDeliveryOrderNoDetails - ResponceSentToAndroid => ", "Responce -" + _sResult);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string GetSaveDispatchQRCodeDetails(string _sLocationCode, string _sQRCode, string _sDONo, string _sUserId)
        {
            dtdisQRCode = new DataTable();
            string _sResult = string.Empty;
            DataTable dt1 = new System.Data.DataTable();
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSaveDispatchQRCodeDetails - RequestDataFromAndroid => ", "LocationCode - " + _sLocationCode + ", QRCode - " + _sQRCode + ", DONo. - " + _sDONo);
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETSAVEDISPATCHQRCODEDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@QRCode", _sQRCode),
                                        new SqlParameter("@DONo", _sDONo),
                                        new SqlParameter("@CreatedBy", _sUserId),
                                   };
                dtdisQRCode = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LoadingOffloading", parma);
                if (dtdisQRCode.Columns.Contains("ERROR") && dtdisQRCode.Rows.Count > 0)
                {
                    _sResult = "GETSAVEDISPATCHQRCODEDETAILS ~ ERROR ~ " + dtdisQRCode.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtdisQRCode.Columns.Contains("ErrorMessage") && dtdisQRCode.Rows.Count > 0)
                {
                    _sResult = "GETSAVEDISPATCHQRCODEDETAILS ~ ERROR ~ " + dtdisQRCode.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtdisQRCode.Columns.Contains("STATUS") && dtdisQRCode.Rows[0][1].ToString() == "1")
                {
                    oMatCode = dtdisQRCode.Rows[0][0].ToString();
                    _sResult = "GETSAVEDISPATCHQRCODEDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString(dtdisQRCode);
                    return _sResult;
                }
                if (dtdisQRCode.Columns.Contains("STATUS") && dtdisQRCode.Rows[0][1].ToString() == "0")
                {
                    _sResult = "GETSAVEDISPATCHQRCODEDETAILS ~ ERROR ~ " + " NO DETAILS SAVED";
                    return _sResult;
                }
                else
                {
                    _sResult = "GETSAVEDISPATCHQRCODEDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;



            
        }

        #endregion


        #region Vendor Dispatch

        internal string GetVendorPODetails(string _sLocationCode, string _sDONo)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + _sLocationCode);
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETDISPATCHDELIVERYORDERDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@DONo", _sDONo),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LoadingOffloading", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    _sResult = "GETDISPATCHDELIVERYORDERDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "GETDISPATCHDELIVERYORDERDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("CustomerName") && dt.Rows.Count > 1)
                {
                    _sResult = "GETDISPATCHDELIVERYORDERDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString(dt);
                    return _sResult;
                }
                if (dt.Columns.Contains("CustomerName") && dt.Rows.Count == 1)
                {
                    _sResult = "GETDISPATCHDELIVERYORDERDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString2(dt);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETDISPATCHDELIVERYORDERDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string GetSaveVendorQRCodeDetails(string _sLocationCode, string _sQRCode, string _sDONo, string _sUserId)
        {
            dtdisQRCode = new DataTable();
            string _sResult = string.Empty;
            DataTable dt1 = new System.Data.DataTable();
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + _sLocationCode);
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETSAVEDISPATCHQRCODEDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@QRCode", _sQRCode),
                                        new SqlParameter("@DONo", _sDONo),
                                        new SqlParameter("@CreatedBy", _sUserId),
                                   };
                dtdisQRCode = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LoadingOffloading", parma);
                if (dtdisQRCode.Columns.Contains("ERROR") && dtdisQRCode.Rows.Count > 0)
                {
                    _sResult = "GETSAVEDISPATCHQRCODEDETAILS ~ ERROR ~ " + dtdisQRCode.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtdisQRCode.Columns.Contains("ErrorMessage") && dtdisQRCode.Rows.Count > 0)
                {
                    _sResult = "GETSAVEDISPATCHQRCODEDETAILS ~ ERROR ~ " + dtdisQRCode.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtdisQRCode.Columns.Contains("STATUS") && dtdisQRCode.Rows[0][1].ToString() == "1")
                {
                    oMatCode = dtdisQRCode.Rows[0][0].ToString();
                    _sResult = "GETSAVEDISPATCHQRCODEDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString(dtdisQRCode);
                    return _sResult;
                }
                if (dtdisQRCode.Columns.Contains("STATUS") && dtdisQRCode.Rows[0][1].ToString() == "0")
                {
                    _sResult = "GETSAVEDISPATCHQRCODEDETAILS ~ ERROR ~ " + " NO DETAILS SAVED";
                    return _sResult;
                }
                else
                {
                    _sResult = "GETSAVEDISPATCHQRCODEDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;




        }

        #endregion


        #region Delivery Cancellation

        internal string GetDeliveryCancellationDODetails(string _sLocationCode, string _sDONo, string _sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "DeliveryCancellationDODetails - RequestDataFromAndroid => ", "LocationCode : " + _sLocationCode + ", DONo : " + _sDONo + ", UserId : " + _sUserId); ;
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETDELIVERYCANCELLEDDODETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@DONo", _sDONo),
                                        new SqlParameter("@CreatedBy", _sUserId),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_DeliveryCancellation", parma);
                if (dt.Columns.Contains("ERROR") && dt.Rows.Count > 0)
                {
                    _sResult = "GETDELIVERYCANCELLEDDODETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("ErrorMessage") && dt.Rows.Count > 0)
                {
                    _sResult = "GETDELIVERYCANCELLEDDODETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("CustomerName") && dt.Rows.Count > 1)
                {
                    frmMain.dtDLoadOffData = dt.Copy();
                    dt.Columns.Remove("LocationCode");
                    dt.Columns.Remove("DeliveryOrderNo");
                    dt.Columns.Remove("QRCode");
                    dt.Columns.Remove("Status");
                    DataView view3 = new DataView(dt);
                    DataTable distinctValues2 = view3.ToTable(true, "CustomerName", "MatCode", "DOQty", "ScannedQty");
                    _sResult = "GETDELIVERYCANCELLEDDODETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString(distinctValues2);
                    return _sResult;
                }
                if (dt.Columns.Contains("CustomerName") && dt.Rows.Count == 1)
                {
                    frmMain.dtDLoadOffData = dt.Copy();
                    dt.Columns.Remove("LocationCode");
                    dt.Columns.Remove("DeliveryOrderNo");
                    dt.Columns.Remove("QRCode");
                    dt.Columns.Remove("Status");
                    DataView view3 = new DataView(dt);
                    DataTable distinctValues3 = view3.ToTable(true, "CustomerName", "MatCode", "DOQty", "ScannedQty");
                    _sResult = "GETDELIVERYCANCELLEDDODETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString2(distinctValues3);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETDELIVERYCANCELLEDDODETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "DeliveryCancellationDODetails - ResponceDataToAndroid =>", " Responce : " + _sResult.ToString()); ;
            return _sResult;
        }

        internal string UpdateDeliveryCancellationDODetails(string sLocCode, string sDONo, string sMatCode, string sQRCode, string sUserId)
        {
            string _sResult = string.Empty;
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","UPDATEDELIVERYCANCELLEDDOSTATUS"),
                                        new SqlParameter("@LocationCode", sLocCode),
                                        new SqlParameter("@DONo", sDONo),
                                        new SqlParameter("@MatCode", sMatCode),
                                        new SqlParameter("@QRCode", sQRCode),
                                        new SqlParameter("@CreatedBy", sUserId),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_DeliveryCancellation", parma);
                if (dt.Columns.Contains("ERROR"))
                {
                    _sResult = "ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "UpdateDeliveryCancellationDODetails => ", "Error : DONo - " + sDONo + ", Matcode - " + sMatCode + ", QRCode - " + sQRCode + "," + dt.Rows[0][0].ToString());
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows[0][0].ToString() == "1")
                {
                    sCount++;
                    _sResult = "SUCCESS ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateDeliveryCancellationDODetails => ", "Success : DONo - " + sDONo + ", Matcode - " + sMatCode + ", QRCode - " + sQRCode);
                    return _sResult;
                }
                else
                {
                    _sResult = "ERROR ~ " + "NOT FOUND";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "UpdateDeliveryCancellationDODetails => ", "Error : DONo - " + sDONo + ", Matcode - " + sMatCode + ", QRCode - " + sQRCode + ", " + "NOT FOUND");
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
