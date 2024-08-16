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

                _bcilNetwork.PrinterIP = "";  //Properties.Settings.Default.SegregationStackQRCodePrinterIP;
                _bcilNetwork.PrinterPort = 0;  //Properties.Settings.Default.SegregationStackQRCodePrinterPort;

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

                    OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                    _bcilNetwork.Dispose();
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


        #region Segregation Stack Scanning

        internal string GetSegregationScannedQRCodeDetails(string _sLocationCode, string _QRCode, string sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + _sLocationCode);
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


    }
}
