using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GreenplyScannerCommServer.Common;
using BCILCommServer;
using GreenplyScannerCommServer;
using System.Net;
using BCILLogger;
using System.IO;
using TEST;

namespace GreenplyScannerCommServer.BI
{
    class B_ClsPrint
    {
        BcilNetwork _bcilNetwork = new BcilNetwork();
        string _StrItmCode = string.Empty;
        string _StrPlantType = string.Empty;
        string _StrBarCode = string.Empty;
        public bool IsPrint;
        public string UserId = string.Empty;
        private static Random random = new Random();
        string objMatDesc = string.Empty;
        string objMatGrade = string.Empty;
        string objMatGroup = string.Empty;
        string objMatThickness = string.Empty;
        string objMatSize = string.Empty;
        DataTable dtFTP = new DataTable();
        DataTable dtMat = new DataTable();

        public string PrintQRCodeItem(string _strLocationCode, string sMatCode, string sMatDesc, string sGrade, string sGroup, string sGroupDesc, string sThickness, string sThicknessDesc, string sSize, string sQRCode, string sMatStatus, string sDateFormat, string PrintSection, string LocationType, int sMatPrintCount)
        {
            try
            {
                string OutMsg = string.Empty;
                var sReadPrn = string.Empty;

                _bcilNetwork.PrinterIP = GreenplyScannerCommServer.Properties.Settings.Default.QRCodePrinterIP;
                _bcilNetwork.PrinterPort = GreenplyScannerCommServer.Properties.Settings.Default.QRCodePrinterPort;

                OutMsg = _bcilNetwork.NetworkPrinterStatus();
                if (OutMsg == "PRINTER READY")
                {
                    string sSaveStatus = SaveQRCode(_strLocationCode.Trim(), sMatCode.Trim(), sMatDesc.Trim(), sGrade.Trim(), sGroup.Trim(), sThickness.Trim(), sSize.Trim(), sQRCode.Trim(), sMatStatus.Trim(), sDateFormat.Trim(), PrintSection.Trim(), LocationType.Trim(), sMatPrintCount);
                    if (sSaveStatus.Contains("SUCCESS"))
                    {
                        StringBuilder sb = new StringBuilder();
                        DataTable dt = new DataTable();
                        string sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyQRCode.PRN";
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

                        string objFull = sQRCode.Trim() + "-" + sGrade.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sSize.Trim();
                        if (sGroup.Trim() != "" && sGroup.Length >= 4)
                            sGroup = sGroup.Substring(sGroup.Length - 4);
                        string objRest = sGrade.Trim() + "-" + sGroup.Trim() + "-" + sThicknessDesc.Trim() + "-" + sSize.Trim();

                        sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objFull.Trim()));
                        sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(sQRCode.Trim()));
                        sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(objRest.Trim()));
                        _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyQRCode.PRN";

                        //OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                        _bcilNetwork.Dispose();
                        OutMsg = "SUCCESS";
                        if (OutMsg == "SUCCESS")
                        {
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", "Barcode - " + sQRCode + " saved and printed successfully at " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                            return OutMsg = "SUCCESS";
                        }
                    }
                    else if (sSaveStatus.Contains("ERROR"))
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", "Barcode - " + sQRCode + " not saved at " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                        return OutMsg;
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

        public string PrintStackQRCodeItem(string _strLocationCode, string sMatCode, string sStackQRCode, string sDateFormat, string PrintSection, string LocationType, string GradeDesc, string GroupDesc, string ThicknessDesc, string MatSize, string LotSize)
        {
            try
            {
                string OutMsg = string.Empty;
                var sReadPrn = string.Empty;

                _bcilNetwork.PrinterIP = GreenplyScannerCommServer.Properties.Settings.Default.StackQRCodePrinterIP;
                _bcilNetwork.PrinterPort = GreenplyScannerCommServer.Properties.Settings.Default.StackQRCodePrinterPort;

                string sSaveStackStatus = SaveStackQRCode(_strLocationCode.Trim(), sMatCode.Trim(), sStackQRCode.Trim(), sDateFormat.Trim(), PrintSection.Trim(), LocationType.Trim());
                if (sSaveStackStatus.Contains("SUCCESS"))
                {
                    OutMsg = _bcilNetwork.NetworkPrinterStatus();
                    if (OutMsg == "PRINTER READY")
                    {
                        StringBuilder sb = new StringBuilder();
                        DataTable dt = new DataTable();
                        string sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyStackQRCode.PRN";
                        if (File.Exists(sPrnExist))
                        {
                            StreamReader sr = new StreamReader(sPrnExist);
                            sReadPrn = sr.ReadToEnd();
                            sr.Close();
                            sr.Dispose();
                        }
                        else
                        {
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackQRCodeItem", "PRN File Not Found");
                            throw new Exception("PRN File Not Found");
                        }
                        string ObjCode2 = GradeDesc.Trim() + "-" + GroupDesc.Trim() + "-" + ThicknessDesc.Trim() + "-" + MatSize.Trim() + "-" + LotSize.Trim() + "Nos";
                        sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(sStackQRCode.Trim()));
                        sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(sStackQRCode.Trim()));
                        sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(ObjCode2.Trim()));

                        _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyStackQRCode.PRN";
                        for (int i = 0; i < 2; i++)
                        {
                            //OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                        }
                        _bcilNetwork.Dispose();
                        OutMsg = "SUCCESS";
                        if (OutMsg == "SUCCESS")
                        {
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackQRCodeItem", "Barcode - " + sStackQRCode + " saved and printed successfully at " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                            return OutMsg = "SUCCESSFULL";
                        }
                        if (OutMsg == "PRINTER NOT IN NETWORK")
                        {
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackQRCodeItem", "Barcode - " + sStackQRCode + " saved and printed successfully at " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                            return OutMsg = "NOT SUCCESSFULL";
                        }
                    }
                    else
                    {
                        if (OutMsg == "PRINTER NOT IN NETWORK")
                        {
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackQRCodeItem", "ERROR ~ Printer not in network");
                            return OutMsg = "ERROR ~ PRINTER NOT IN NETWORK";
                        }
                        else
                        {
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackQRCodeItem", "ERROR~Printer error is : " + OutMsg);
                            return OutMsg;
                        }
                    }
                }
                else if (sSaveStackStatus.Contains("ERROR"))
                {
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackQRCodeItem", "Barcode - " + sStackQRCode + " not saved at " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                    return OutMsg = sSaveStackStatus.ToString();
                }

                return OutMsg;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "SchedularThreadConnectMode", exDetail);
                return "ERROR | " + ex.Message;
            }
        }

        public string PrintHUBStackQRCodeItem(string _strLocationCode, string sMatCode, string sStackQRCode, string GradeDesc, string GroupDesc, string ThicknessDesc, string MatSize, string LotSize)
        {
            try
            {
                string OutMsg = string.Empty;
                var sReadPrn = string.Empty;

                _bcilNetwork.PrinterIP = GreenplyScannerCommServer.Properties.Settings.Default.StackQRCodePrinterIP;
                _bcilNetwork.PrinterPort = GreenplyScannerCommServer.Properties.Settings.Default.StackQRCodePrinterPort;
                OutMsg = _bcilNetwork.NetworkPrinterStatus();
                //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackQRCodeItem", "StackQRCode Printer IP : " + _bcilNetwork.PrinterIP + ", Printer Status : " + OutMsg);
                if (OutMsg == "PRINTER READY")
                {
                    StringBuilder sb = new StringBuilder();
                    DataTable dt = new DataTable();
                    string sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyStackQRCode.PRN";
                    if (File.Exists(sPrnExist))
                    {
                        StreamReader sr = new StreamReader(sPrnExist);
                        sReadPrn = sr.ReadToEnd();
                        sr.Close();
                        sr.Dispose();
                    }
                    else
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackQRCodeItem", "PRN File Not Found");
                        throw new Exception("PRN File Not Found");
                    }
                    string ObjCode2 = GradeDesc.Trim() + "-" + GroupDesc.Trim() + "-" + ThicknessDesc.Trim() + "-" + MatSize.Trim() + "-" + LotSize.Trim() + "Nos";
                    sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(sStackQRCode.Trim()));
                    sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(sStackQRCode.Trim()));
                    sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(ObjCode2.Trim()));

                    _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyStackQRCode.PRN";
                    for (int i = 0; i < 2; i++)
                    {
                        OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                        //OutMsg = "SUCCESS";
                    }
                    _bcilNetwork.Dispose();
                    if (OutMsg == "SUCCESS")
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackQRCodeItem", "Production at Hub StackQRCode - " + sStackQRCode + " saved and printed successfully");
                        return OutMsg;
                    }
                }
                else
                {
                    if (OutMsg == "PRINTER NOT IN NETWORK")
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackQRCodeItem", "ERROR ~ Production at Hub StackQRCode Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network");
                        OutMsg = "ERROR ~ Production at Hub StackQRCode Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network";
                        return OutMsg;
                    }
                    else
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackQRCodeItem", "ERROR ~ Production at Hub StackQRCode Printer IP : " + _bcilNetwork.PrinterIP + " found error, Error - " + OutMsg);
                        OutMsg = "ERROR ~ Production at Hub StackQRCode Printer IP : " + _bcilNetwork.PrinterIP + " found error, Error - " + OutMsg;
                        return OutMsg;
                    }
                }
                return OutMsg;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "SchedularThreadConnectMode", exDetail);
                return "ERROR | " + ex.Message;
            }
        }



        internal string GetIsStackPrintStatus(string objLocationCode)
        {
            string _sResult = "";
            //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + objLocationCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETISSTACKPRINTSTATUS"),
                                        new SqlParameter("@LocationCode", objLocationCode),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialMaster", parma);
                if (dt.Columns.Count == 1 && dt.Rows.Count == 1)
                {
                    _sResult = dt.Rows[0][0].ToString();
                    return _sResult;
                }
                else
                {
                    _sResult = dt.Rows[0][0].ToString();
                    return _sResult;
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "GetSelectedMatCode", ex.Message.ToString());
                throw ex;
            }
        }

        internal int GetSelectedMatLotSize(string objMatCode)
        {
            int _sResult = 0;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + objMatCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETSELECTEDMATERIALCODELOTSIZE"),
                                        new SqlParameter("@MatCode", objMatCode),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialMaster", parma);
                if (dt.Columns.Contains("LotSize") && dt.Rows.Count == 1)
                {
                    _sResult = Convert.ToInt32(dt.Rows[0][0].ToString().Trim());
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
                else
                {
                    _sResult = 0;
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        internal int GetSelectedMatPrintedLotQty(string objMatCode)
        {
            int _sResult = 0;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + objMatCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETSELECTEDMATERIALCODEPRINTEDLOTQTY"),
                                        new SqlParameter("@MatCode", objMatCode),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialMaster", parma);
                if (dt.Columns.Contains("PrintedLotQty") && dt.Rows.Count == 1)
                {
                    _sResult = Convert.ToInt32(dt.Rows[0][0].ToString().Trim());
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
                else
                {
                    _sResult = 0;
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "GetSelectedMatPrintedLotQty", ex.Message.ToString());
                throw ex;
            }
        }

        internal string GetQRCodeRunningSerial(string objLocationCode, string DateFormat, string sPrintingSection, string sLocationType)
        {
            string _sResult = "";
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetQRCodeRunningSerial - RequestData => ", "LocationCode - " + objLocationCode + ", DateFormat - " + DateFormat + ", PrintingSection - " + sPrintingSection + ", LocationType - " + sLocationType);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETQRCODERUNNINGSERIALNO"),
                                        new SqlParameter("@LocationCode", objLocationCode.Trim()),
                                        new SqlParameter("@DateFormat", DateFormat.Trim()),
                                        new SqlParameter("@PrintSection", sPrintingSection.Trim().ToString()),
                                        new SqlParameter("@LocationType", sLocationType.Trim().ToString()),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialMaster", parma);
                if (dt.Columns.Contains("SERIALNO") && dt.Rows.Count == 1)
                {
                    _sResult = dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
                else
                {
                    _sResult = "0";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "GetQRCodeRunningSerial", ex.Message.ToString());
                throw ex;
            }
        }

        internal string GetStackRunningSerial(string objLocationCode, string DateFormat, string sPrintingSection, string sLocationType)
        {
            string _sResult = "";
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetStackRunningSerial : RequestedData => ", "LocationCode : " + objLocationCode + ", DateFormat : " + DateFormat + ", PrintingSection : " + sPrintingSection + ", LocationType : " + sLocationType);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETSTACKRUNNINGSERIALNO"),
                                        new SqlParameter("@LocationCode", objLocationCode.Trim().ToString()),
                                        new SqlParameter("@DateFormat", DateFormat.Trim().ToString()),
                                        new SqlParameter("@PrintSection", sPrintingSection.Trim().ToString()),
                                        new SqlParameter("@LocationType", sLocationType.Trim().ToString()),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialMaster", parma);
                if (dt.Columns.Contains("SERIALNO") && dt.Rows.Count == 1)
                {
                    _sResult = dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetStackRunningSerial : RequestedData => ", "ReceivedNewStackSerialNo : " + _sResult);
                    return _sResult;
                }
                else
                {
                    _sResult = "0";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetStackRunningSerial : RequestedData => ", "ReceivedNewStackSerialNo : " + _sResult);
                    return _sResult;
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetStackRunningSerial : RequestedData => ", "Error : " + ex.ToString());
                throw ex;
            }
        }

        internal string SaveQRCode(string objLocationCode, string objMatCode, string sMatDesc, string sGrade, string sGroup, string sThickness, string sSize, string objQRCode, string sMatStatus, string sDateFormat, string PrintSection, string LocationType, int sMatPrintCount)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetQRCodeRunningSerial - RequestData => ", "LocationCode - " + objLocationCode + ", MatCode - " + objMatCode + ", QRCode - " + objQRCode + ", DateFormat - " + sDateFormat + ", LocationType - " + LocationType);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","SAVEQRCODE"),
                                        new SqlParameter("@LocationCode", objLocationCode),
                                        new SqlParameter("@MatCode", objMatCode),
                                        new SqlParameter("@MatDesc", sMatDesc),
                                        new SqlParameter("@Grade", sGrade),
                                        new SqlParameter("@Group", sGroup),
                                        new SqlParameter("@Thickness", sThickness),
                                        new SqlParameter("@Size", sSize),
                                        new SqlParameter("@QRCode", objQRCode),
                                        new SqlParameter("@MatStatus", sMatStatus),
                                        new SqlParameter("@DateFormat", sDateFormat),
                                        new SqlParameter("@PrintSection", PrintSection),
                                        new SqlParameter("@LocationType", LocationType),
                                        new SqlParameter("@PrintedQty", sMatPrintCount),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialMaster", parma);
                if (dt.Columns.Contains("ERROR"))
                {
                    _sResult = "ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0)
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
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "SaveQRCode", ex.Message.ToString());
                throw ex;
            }
        }

        internal string SaveStackQRCode(string objLocationCode, string objMatCode, string objStackQRCode, string sDateFormat, string PrintSection, string LocationType)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "SaveStackQRCode => ", "LocationCode - " + objLocationCode + ", MatCode - " + objMatCode + ", StackQRCode - " + objStackQRCode + ", DateFormat - " + sDateFormat);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","SAVESTACKQRCODE"),
                                        new SqlParameter("@LocationCode", objLocationCode),
                                        new SqlParameter("@MatCode", objMatCode.Trim()),
                                        new SqlParameter("@StackQRCode", objStackQRCode.Trim()),
                                        new SqlParameter("@DateFormat", sDateFormat.Trim()),
                                        new SqlParameter("@PrintSection", PrintSection.Trim()),
                                        new SqlParameter("@LocationType", LocationType.Trim()),
                                        new SqlParameter("@MatStatus", "P"),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialMaster", parma);
                if (dt.Columns.Contains("ERROR"))
                {
                    _sResult = "ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0)
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
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "SaveStackQRCode", ex.Message.ToString());
                throw ex;
            }
        }


        internal string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public string PrintSalesReturnQRCode(string _strLocationCode, string sMatCode, string sQRCode, string sDateFormat)
        {
            try
            {
                string OutMsg = string.Empty;
                var sReadPrn = string.Empty;

                _bcilNetwork.PrinterIP = GreenplyScannerCommServer.Properties.Settings.Default.QRCodePrinterIP;
                _bcilNetwork.PrinterPort = GreenplyScannerCommServer.Properties.Settings.Default.QRCodePrinterPort;

                OutMsg = _bcilNetwork.NetworkPrinterStatus();
                if (OutMsg == "PRINTER READY")
                {
                    DataTable dtMat = GetMatDeatils(_strLocationCode.Trim(), sMatCode.Trim());
                    if (dtMat.Rows.Count > 0)
                    {
                        objMatDesc = dtMat.Rows[0][0].ToString();
                        objMatGrade = dtMat.Rows[0][1].ToString();
                        objMatGroup = dtMat.Rows[0][2].ToString();
                        if (objMatGroup != "" && objMatGroup.Length >= 4)
                            objMatGroup = objMatGroup.Substring(objMatGroup.Length - 4);
                        objMatThickness = dtMat.Rows[0][3].ToString();
                        objMatSize = dtMat.Rows[0][4].ToString();
                        string sSaveStatus = SaveSalesReturnQRCode(_strLocationCode.Trim(), sMatCode.Trim(), objMatDesc.Trim(), objMatGrade.Trim(), objMatGroup.Trim(), objMatThickness.Trim(), objMatSize.Trim(), sQRCode.Trim(), sDateFormat.Trim());
                        if (sSaveStatus.Contains("SUCCESS"))
                        {
                            StringBuilder sb = new StringBuilder();
                            DataTable dt = new DataTable();
                            string sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyQRCode.PRN";
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
                            string objRest = objMatGrade + " - " + objMatGroup + " - " + objMatThickness + " - " + objMatSize;
                            string objFull = sQRCode + " - " + objRest;
                            sReadPrn = sReadPrn.Replace("{VarBarcode}", Convert.ToString(objFull));
                            sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(sQRCode));
                            sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(objRest));
                            _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyQRCode.PRN";

                            //OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                            OutMsg = "SUCCESS";
                            _bcilNetwork.Dispose();
                            if (OutMsg == "SUCCESS")
                            {
                                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", "Barcode - " + sQRCode + " saved and printed successfully at " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                                return OutMsg = "Printed Successfully";
                            }
                        }
                        else if (sSaveStatus.Contains("ERROR"))
                        {
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", "Barcode - " + sQRCode + " not saved at " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss"));
                        }
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

        internal string SaveSalesReturnQRCode(string objLocationCode, string objMatCode, string sMatDesc, string sGrade, string sGroup, string sThickness, string sSize, string objQRCode, string sDateFormat)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "SaveSalesReturnQRCode - RequestDataFromAndroid => ", "LocationCode - " + objLocationCode + ", MatCode - " + objMatCode + ", QRCode - " + objQRCode + ", DateFormat - " + sDateFormat);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","SAVESALESRETURNNEWQRCODE"),
                                        new SqlParameter("@LocationCode", objLocationCode),
                                        new SqlParameter("@MatCode", objMatCode),
                                        new SqlParameter("@MatDesc", sMatDesc),
                                        new SqlParameter("@Grade", sGrade),
                                        new SqlParameter("@Group", sGroup),
                                        new SqlParameter("@Thickness", sThickness),
                                        new SqlParameter("@Size", sSize),
                                        new SqlParameter("@QRCode", objQRCode),
                                        new SqlParameter("@MatStatus", "S"),
                                        new SqlParameter("@DateFormat", sDateFormat),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_SalesReturn", parma);
                if (dt.Columns.Contains("ERROR"))
                {
                    _sResult = "ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "SaveSalesReturnQRCode => ", "Responce - " + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0)
                {
                    _sResult = "SUCCESS ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "SaveSalesReturnQRCode => ", "Responce - " + _sResult);
                    return _sResult;
                }
                else
                {
                    _sResult = "ERROR ~ " + "NOT FOUND";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "SaveSalesReturnQRCode => ", "Responce - " + _sResult);
                    return _sResult;
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "SaveSalesReturnQRCode => Exception - ", ex.Message.ToString());
                throw ex;
            }
        }

        internal DataTable GetMatDeatils(string objLocationCode, string objMatCode)
        {
            DataTable dtData = new DataTable();
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + objLocationCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETMATERIALDETAILS"),
                                        new SqlParameter("@MatCode", objMatCode),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_SalesReturn", parma);
                if (dt.Columns.Count > 1 && dt.Rows.Count == 1)
                {
                    dtData = dt;
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + dt.Rows[0][0].ToString());
                    return dtData;
                }
                else
                {
                    dtData = dt;
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + dt.Rows[0][0].ToString());
                    return dtData;
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "GetMatDeatils", ex.Message.ToString());
                throw ex;
            }
        }

        internal string UpdateQRCodeSAPStatus(string sLocationCode, string sMatCode, string sQRCode, string sStatus, string sPostMsg)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + sQRCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","UPDATESAPPOSTEDSTATUS"),
                                        new SqlParameter("@LocationCode", sLocationCode),
                                        new SqlParameter("@MatCode", sMatCode),
                                        new SqlParameter("@QRCode", sQRCode),
                                        new SqlParameter("@MatStatus", sStatus),
                                        new SqlParameter("@SAPPostMsg", sPostMsg),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialMaster", parma);
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
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "UpdateQRCodeSAPStatus", ex.Message.ToString());
                throw ex;
            }
            return _sResult;
        }

        public DataTable GetFTPReportDetails(string _strPlantCode, string fromdate, string todate)
        {
            dtFTP = new DataTable();
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETFTPDATA"),
                                        new SqlParameter("@LocationCode",_strPlantCode),
                                        new SqlParameter("@FromDate",fromdate),
                                        new SqlParameter("@ToDate",todate),
                                   };
                dtFTP = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialMaster", parma);
                if (dtFTP.Rows.Count > 0)
                {
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + "There are " + dtFTP.Rows.Count + " records untill " + DateTime.Now.ToString());
                }
                else
                {
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + "There are " + dtFTP.Rows.Count + " records at " + DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + ex);
            }
            return dtFTP;
        }

        public void ExportToCSVFromDataTable(DataTable dataTable, string sReportName, string fileformat)
        {
            try
            {
                string ftpaddress = ""; // Properties.Settings.Default.FTPAddress;
                string username = ""; //Properties.Settings.Default.FTPUsername;
                string password = ""; //Properties.Settings.Default.FTPPassword;

                string csv = string.Empty;
                string filename = string.Empty;
                foreach (DataColumn column in dataTable.Columns)
                {
                    csv += column.ColumnName + ',';
                }
                csv += "\r\n";
                foreach (DataRow row in dataTable.Rows)
                {
                    foreach (DataColumn column in dataTable.Columns)
                    {
                        csv += row[column.ColumnName].ToString().Replace(",", ";") + ',';
                    }
                    csv += "\r\n";
                }
                string path = ""; //Properties.Settings.Default.LocalFolderPath;
                string folderPath = path;
                string dbname = ""; // lblDatabase1.Text.Trim();
                if (dbname.Contains("QRCODE-RKT-PLY"))
                {
                    filename = "2010_PL_" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                }
                if (dbname.Contains("QRCODE-RKT-DOOR"))
                {
                    filename = "2010_DC_" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                }
                if (dbname.Contains("QRCODE-RKT-DECO"))
                {
                    filename = "2010_DR_" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                }
                if (dbname.Contains("QRCODE-TIZIT-PLY"))
                {
                    filename = "2020_PL_" + DateTime.Now.ToString("yyyyMMdd") + ".csv";
                }
                if (dbname.Contains("QRCODE-KRP-PLY"))
                {
                    filename = "2000_PL_" + DateTime.Now.ToString("yyyyMMdd") + ".csv";     //HHmm
                }
                string file_browse_path = folderPath + filename;
                string path11 = @"" + file_browse_path;
                if (!File.Exists(path11))
                {
                    File.WriteAllText(file_browse_path, csv);   //folderPath + "Report2_" + DateTime.Now.ToString("yyyyMMddHHmm") + ".csv"
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + filename + " created Successfully at " + DateTime.Now.ToString());
                }
                else
                {

                }
                if (File.Exists(path11))
                {
                    uploadFile(ftpaddress, folderPath + filename, username, password, filename);
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + ex);
            }
        }

        private void uploadFile(string FTPAddress, string filePath, string username, string password, string filename)
        {
            try
            {
                bool errCatch = false;
                string ss = filePath.Replace(@"\\", @"\");
                FtpWebRequest request = (FtpWebRequest)FtpWebRequest.Create(FTPAddress + "/" + Path.GetFileName(filePath));
                request.Method = WebRequestMethods.Ftp.UploadFile;
                request.Credentials = new NetworkCredential(username.Trim(), password.Trim());
                request.UsePassive = true;
                request.UseBinary = true;
                request.KeepAlive = false;
                try
                {
                    FtpWebResponse response = (FtpWebResponse)request.GetResponse();
                    errCatch = true;
                }
                catch (WebException e)
                {
                    if (e.Status == WebExceptionStatus.ProtocolError)
                    {
                        errCatch = false;
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + e + "at " + DateTime.Now.ToString());
                    }
                    else
                    {
                        errCatch = false;
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + e + "at " + DateTime.Now.ToString());
                    }
                }
                if (errCatch == true)
                {
                    Stream reqStream = request.GetRequestStream();
                    FileStream stream = File.OpenRead(filePath);
                    byte[] buffer = new byte[stream.Length];
                    stream.Read(buffer, 0, buffer.Length);
                    stream.Close();
                    reqStream.Write(buffer, 0, buffer.Length);
                    reqStream.Close();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + filename + " Uploaded Successfully at " + DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + ex);
            }
        }


        #region QRCode Scanning and Printing At Hub

        internal DataTable GetSelectedMatCode(string objLocationCode)
        {
            DataTable dtData = new DataTable();
            //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + objLocationCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETSELECTEDMATERIALCODE"),
                                        new SqlParameter("@LocationCode", objLocationCode),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LocationPrinting", parma);
                if (dt.Columns.Count > 1 && dt.Rows.Count == 1)
                {
                    dtData = dt;
                    //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + dt.Rows[0][0].ToString());
                    return dtData;
                }
                else
                {
                    dtData = dt.Clone();
                    //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data => There is no data found");
                    return dtData;
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "GetSelectedMatCode", ex.Message.ToString());
                throw ex;
            }
        }

        public string GetHUBSelectedMatCode()
        {
            string _sResult = "";
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetHUBSelectedMatCode - RequestDataFromAndroid => ", "");
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETSELECTEDMATCODEDETAIL"),
                                       };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LocationPrinting", parma);
                if (dt.Columns.Contains("MatCode") && dt.Rows.Count == 1)
                {
                    frmMain.sHubSelectedMatCode = dt.Rows[0][0].ToString();
                    _sResult = "SUCCESS ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetHUBSelectedMatCode - ResponceSentToAndroid => SelectedMatcode : ", _sResult.ToString());
                    return _sResult;
                }
                if (dt.Columns.Contains("MatCode") && dt.Rows.Count == 0)
                {
                    _sResult = "ERROR ~ There is no material selected for printing, Kindly select the material first";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetHUBSelectedMatCode - ResponceSentToAndroid => SelectedMatcode : ", _sResult.ToString());
                    return _sResult;
                }
                else if ((dt.Columns.Contains("ErrorMessage")))
                {
                    _sResult = dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetHUBSelectedMatCode - ResponceSentToAndroid => SelectedMatcode : ", _sResult.ToString());
                    return _sResult;
                }
                return _sResult;
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "GetHUBSelectedMatCode - ResponceSentToAndroid => SelectedMatcode : ", ex.ToString());
                _sResult = ex.Message;
                return _sResult;
            }
        }

        public string GetHUBScannedQRCodeDetails(string objLocationCode, string sQRCode, string sUserId, string sMatCode, string sMatGrade, string sMatGroup, string sMatGroupDesc, string sMatThicknessDesc, string sMatSize, string _sPrintingSection, string sLocationType)
        {
            string _sResult = "";
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintScannedQRCodeAtHUB - RequestDataFromAndroid => ", "LocationCode : " + objLocationCode + ", QRCode : " + sQRCode + ", MatCode : " + sMatCode + ", MatGrade : " + sMatGrade + ", MatGroup : " + sMatGroup + ", MatGroupDesc : " + sMatGroupDesc + ", MatThicknessDesc : " + sMatThicknessDesc + ", MatSize : " + sMatSize);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETSCANNEDQRCODEDETAILS"),
                                        new SqlParameter("@LocationCode", objLocationCode.Trim()),
                                        new SqlParameter("@QRCode", sQRCode.Trim()),
                                        new SqlParameter("@CreatedBy", sUserId.Trim()),
                                        new SqlParameter("@PrintSection", _sPrintingSection.Trim()),
                                        new SqlParameter("@LocationType", sLocationType.Trim()),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LocationPrinting", parma);
                if (dt.Columns.Contains("MatCode") && dt.Rows.Count == 1)
                {
                    string OutMsg = string.Empty;
                    _bcilNetwork.PrinterIP = GreenplyScannerCommServer.Properties.Settings.Default.QRCodePrinterIP;
                    _bcilNetwork.PrinterPort = GreenplyScannerCommServer.Properties.Settings.Default.QRCodePrinterPort;

                    OutMsg = _bcilNetwork.NetworkPrinterStatus();

                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", "Production at Hub QRCode Printer IP : " + _bcilNetwork.PrinterIP + ", Printer Status : " + OutMsg);
                    if (OutMsg == "PRINTER READY")
                    {
                        _sResult = HubPrintingScannedQRCodeItem(objLocationCode.Trim(), sMatCode.Trim(), sMatGrade.Trim(), sMatGroup.Trim(), sMatGroupDesc.Trim(), sMatThicknessDesc.Trim(), sMatSize.Trim(), sQRCode.Trim(), sUserId.Trim());
                        if (_sResult.Contains("SUCCESS"))
                        {
                            _sResult = "SUCCESS ~ " + GlobalVariable.DtToString(dt);
                        }
                    }
                    else
                    {
                        if (OutMsg == "PRINTER NOT IN NETWORK")
                        {
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "ProductionAtHub", "ERROR ~ Production at Hub QRCode Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network");
                            OutMsg = "ERROR ~ Production at Hub QRCode Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network";
                            return OutMsg;
                        }
                        else
                        {
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "ProductionAtHub", "ERROR ~ Production at Hub QRCode Printer IP : " + _bcilNetwork.PrinterIP + " found error, Error - " + OutMsg);
                            OutMsg = "ERROR ~ Production at Hub QRCode Printer IP : " + _bcilNetwork.PrinterIP + " found error, Error - " + OutMsg;
                            return OutMsg;
                        }
                    }
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintScannedQRCodeAtHUB - ResponceSentToAndroid => ", _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("Completed") && dt.Rows.Count == 1)
                {
                    string OutMsg = string.Empty;
                    _bcilNetwork.PrinterIP = GreenplyScannerCommServer.Properties.Settings.Default.QRCodePrinterIP;
                    _bcilNetwork.PrinterPort = GreenplyScannerCommServer.Properties.Settings.Default.QRCodePrinterPort;

                    OutMsg = _bcilNetwork.NetworkPrinterStatus();

                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", "Production at Hub QRCode Printer IP : " + _bcilNetwork.PrinterIP + ", Printer Status : " + OutMsg);
                    if (OutMsg == "PRINTER READY")
                    {
                        _sResult = HubPrintingScannedQRCodeItem(objLocationCode.Trim(), sMatCode.Trim(), sMatGrade.Trim(), sMatGroup.Trim(), sMatGroupDesc.Trim(), sMatThicknessDesc.Trim(), sMatSize.Trim(), sQRCode.Trim(), sUserId.Trim());
                        if (_sResult.Contains("SUCCESS"))
                        {
                            string OutMsg1 = string.Empty;
                            _bcilNetwork.PrinterIP = GreenplyScannerCommServer.Properties.Settings.Default.StackQRCodePrinterIP;
                            _bcilNetwork.PrinterPort = GreenplyScannerCommServer.Properties.Settings.Default.StackQRCodePrinterPort;
                            OutMsg1 = _bcilNetwork.NetworkPrinterStatus();
                            //OutMsg1 = "SUCCESS";
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackQRCodeItem", "StackQRCode Printer IP : " + _bcilNetwork.PrinterIP + ", Printer Status : " + OutMsg1);
                            if (OutMsg1 == "PRINTER READY")
                            {
                                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackQRCodeItem", "Generated Stack QRCode : " + dt.Rows[0][1].ToString());
                                _sResult = PrintHUBStackQRCodeItem(objLocationCode.Trim(), sMatCode.Trim(), dt.Rows[0][1].ToString(), sMatGrade.Trim(), sMatGroupDesc.Trim(), sMatThicknessDesc.Trim(), sMatSize.Trim(), dt.Rows[0][2].ToString());
                                frmMain.sHubSelectedMatCode = string.Empty;
                                if (_sResult.Contains("SUCCESS"))
                                {
                                    _sResult = "COMPLETE ~ Production at Hub StackQRCode - " + dt.Rows[0][0].ToString() + " is generated and printed successfully";
                                    frmMain.sHubSelectedMatCode = string.Empty;
                                }
                                else if (_sResult.Contains("ERROR"))
                                {
                                    _sResult = "COMPLETE ~ StackQRCode - " + dt.Rows[0][0].ToString() + " is generated and " + _sResult.ToString();
                                    frmMain.sHubSelectedMatCode = string.Empty;
                                }
                            }
                            else
                            {
                                if (OutMsg1 == "PRINTER NOT IN NETWORK")
                                {
                                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackQRCodeItem", "ERROR ~ Production at Hub StackQRCode Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network");
                                    OutMsg1 = "ERROR ~ Production at Hub StackQRCode Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network";
                                    return OutMsg1;
                                }
                                else
                                {
                                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintStackQRCodeItem", "ERROR ~ Production at Hub StackQRCode Printer IP : " + _bcilNetwork.PrinterIP + " found error, Error - " + OutMsg1);
                                    OutMsg1 = "ERROR ~ Production at Hub StackQRCode Printer IP : " + _bcilNetwork.PrinterIP + " found error, Error - " + OutMsg1;
                                    return OutMsg1;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (OutMsg == "PRINTER NOT IN NETWORK")
                        {
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "ProductionAtHub", "ERROR ~ Production at Hub QRCode Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network");
                            OutMsg = "ERROR ~ Production at Hub QRCode Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network";
                            return OutMsg;
                        }
                        else
                        {
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "ProductionAtHub", "ERROR ~ Production at Hub QRCode Printer IP : " + _bcilNetwork.PrinterIP + " found error, Error - " + OutMsg);
                            OutMsg = "ERROR ~ Production at Hub QRCode Printer IP : " + _bcilNetwork.PrinterIP + " found error, Error - " + OutMsg;
                            return OutMsg;
                        }
                    }
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintScannedQRCodeAtHUB - ResponceSentToAndroid => ", _sResult);
                    return _sResult;
                }
                else if ((dt.Columns.Contains("ERROR")) || (dt.Columns.Contains("ErrorMessage")))
                {
                    _sResult = "ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintScannedQRCodeAtHUB - ResponceSentToAndroid => ", _sResult);
                    return _sResult;
                }
                else
                {
                    return _sResult; // = "ERROR ~ " + "There is ";
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintScannedQRCodeAtHUB - Exception => ", ex.Message.ToString());
                _sResult = ex.Message;
                return _sResult;
            }
        }

        internal string HubPrintingScannedQRCodeItem(string _strLocationCode, string sMatCode, string sGrade, string sGroup, string sGroupDesc, string sThicknessDesc, string sSize, string sQRCode, string sUserID)
        {
            try
            {
                string OutMsg = string.Empty;
                var sReadPrn = string.Empty;

                _bcilNetwork.PrinterIP = GreenplyScannerCommServer.Properties.Settings.Default.QRCodePrinterIP;
                _bcilNetwork.PrinterPort = GreenplyScannerCommServer.Properties.Settings.Default.QRCodePrinterPort;
                OutMsg = _bcilNetwork.NetworkPrinterStatus();
                //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", "QRCode Printer IP : " + _bcilNetwork.PrinterIP + ", OutMsg : " + OutMsg);
                if (OutMsg == "PRINTER READY")
                {
                    StringBuilder sb = new StringBuilder();
                    DataTable dt = new DataTable();
                    string sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyQRCode.PRN";
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

                    string objFull = sQRCode.Trim() + "-" + sGrade.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sSize.Trim();
                    if (sGroup.Trim() != "" && sGroup.Length >= 4)
                        sGroup = sGroup.Substring(sGroup.Length - 4);
                    string objRest = sGrade.Trim() + "-" + sGroup.Trim() + "-" + sThicknessDesc.Trim() + "-" + sSize.Trim();

                    sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objFull.Trim()));
                    sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(sQRCode.Trim()));
                    sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(objRest.Trim()));
                    _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyQRCode.PRN";

                    OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                    //OutMsg = "SUCCESS";
                    _bcilNetwork.Dispose();
                    if (OutMsg == "SUCCESS")
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "ProductionAtHub", "Printing Scanned QRCode - " + sQRCode + " saved and printed successfully");
                        return OutMsg = "SUCCESS";
                    }
                }
                else
                {
                    if (OutMsg == "PRINTER NOT IN NETWORK")
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "ProductionAtHub", "ERROR ~ Production at Hub QRCode Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network");
                        OutMsg = "ERROR ~ Production at Hub QRCode Printer IP : " + _bcilNetwork.PrinterIP + " not in network, Kindly check the network";
                        return OutMsg;
                    }
                    else
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "ProductionAtHub", "ERROR ~ Production at Hub QRCode Printer IP : " + _bcilNetwork.PrinterIP + " found error, Error - " + OutMsg);
                        OutMsg = "ERROR ~ Production at Hub QRCode Printer IP : " + _bcilNetwork.PrinterIP + " found error, Error - " + OutMsg;
                        return OutMsg;
                    }
                }
                return OutMsg;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "ProductionAtHub", exDetail);
                return "ERROR | " + ex.Message;
            }
        }

        internal string HubPrintingUpdateQRCodeStatus(string objLocationCode, string objMatCode, string objQRCode, string objUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + objQRCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","UPDATEQRCODEPRINTSTATUS"),
                                        new SqlParameter("@LocationCode", objLocationCode),
                                        new SqlParameter("@MatCode", objMatCode),
                                        new SqlParameter("@QRCode", objQRCode),
                                        new SqlParameter("@CreatedBy", objUserId),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LocationPrinting", parma);
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
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "SaveQRCode", ex.Message.ToString());
                throw ex;
            }
        }

        internal string ClearTempHubLabelPrinting(string _sLocationCode, string sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ClearTempHubLabelPrinting - RequestDataFromAndroid => ", "LocationCode : " + _sLocationCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","CLEARTEMPHUBLABELPRINTING"),
                                        new SqlParameter("@LocationCode", _sLocationCode.Trim()),
                                        new SqlParameter("@CreatedBy", sUserId.Trim()),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LocationPrinting", parma);
                if (dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage"))
                {
                    _sResult = "CLEARTEMPHUBLABELPRINTING ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "ClearTempHubLabelPrinting - ResponceSentToAndroid => ", _sResult);
                    return _sResult;
                }
                else
                {
                    _sResult = "CLEARTEMPHUBLABELPRINTING ~ SUCCESS ";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "ClearTempHubLabelPrinting - ResponceSentToAndroid => ", _sResult);
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "ClearTempHubLabelPrinting - ResponceSentToAndroid => ", ex.ToString());
                throw ex;
            }
            return _sResult;
        }

        public string CompletedStackQRCodeData(string objLocationCode, string sUserId, string sMatCode, string sMatGrade, string sMatGroup, string sMatGroupDesc, string sMatThicknessDesc, string sMatSize, string _sPrintingSection, string sLocationType)
        {
            string _sResult = "";
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintScannedStackQRCodeAtHUB - RequestDataFromAndroid => ", "LocationCode : " + objLocationCode + ", MatCode : " + sMatCode + ", MatGrade : " + sMatGrade + ", MatGroup : " + sMatGroup + ", MatGroupDesc : " + sMatGroupDesc + ", MatThicknessDesc : " + sMatThicknessDesc + ", MatSize : " + sMatSize);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","COMPLETEDSTACKQRCODE"),
                                        new SqlParameter("@LocationCode", objLocationCode.Trim()),
                                        new SqlParameter("@CreatedBy", sUserId.Trim()),
                                        new SqlParameter("@PrintSection", _sPrintingSection.Trim()),
                                        new SqlParameter("@LocationType", sLocationType.Trim()),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LocationPrinting", parma);
                if (dt.Columns.Contains("Completed") && dt.Rows.Count == 1)
                {
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "ProductionAtHub => PrintStackQRCodeItem", "Generated Stack QRCode : " + dt.Rows[0][1].ToString());
                    _sResult = PrintHUBStackQRCodeItem(objLocationCode.Trim(), sMatCode.Trim(), dt.Rows[0][1].ToString(), sMatGrade.Trim(), sMatGroupDesc.Trim(), sMatThicknessDesc.Trim(), sMatSize.Trim(), dt.Rows[0][2].ToString());
                    if (_sResult.Contains("SUCCESS"))
                    {
                        _sResult = "COMPLETE ~ StackQRCode - " + dt.Rows[0][0].ToString() + " is generated and printed successfully";
                    }
                    else if (_sResult.Contains("ERROR"))
                    {
                        _sResult = "COMPLETE ~ StackQRCode - " + dt.Rows[0][0].ToString() + " is generated and " + _sResult.ToString();
                    }
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "CompletedStackQRCodeData - ResponceSentToAndroid => ", _sResult);
                    return _sResult;
                }
                else if ((dt.Columns.Contains("ERROR")) || (dt.Columns.Contains("ErrorMessage")))
                {
                    _sResult = "ERROR ~" + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "CompletedStackQRCodeData - ResponceSentToAndroid => ", _sResult);
                    return _sResult;
                }
                return _sResult;
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "CompletedStackQRCodeData - Exception => ", ex.Message.ToString());
                _sResult = "ERROR ~" + ex.Message;
                return _sResult;
            }
        }


        #endregion


        #region QA Process at HUB

        internal string GetQAScannedPODetails(string _sLocationCode, string _sPONo, string _sUserID)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetQAScannedPODetails - RequestDataFromAndroid => ", "LocationCode : " + _sLocationCode + ", PONo. : " + _sPONo + ", UserID : " + _sUserID);
            try
            {
                SqlParameter[] parma = {
                                          new SqlParameter("@Type","GETQASCANNEDPODETAILS"),
                                          new SqlParameter("@LocationCode", _sLocationCode),
                                          new SqlParameter("@PONo", _sPONo),
                                          new SqlParameter("@CreatedBy", _sUserID),
                                       };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LocationPrinting", parma);
                if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                {
                    _sResult = "GETQASCANNEDPODETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("InspectionLotNo") && dt.Rows.Count > 1)
                {
                    _sResult = "GETQASCANNEDPODETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString(dt);
                    return _sResult;
                }
                if (dt.Columns.Contains("InspectionLotNo") && dt.Rows.Count == 1)
                {
                    _sResult = "GETQASCANNEDPODETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString2(dt);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETQASCANNEDPODETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetQAScannedPODetails - ResponceSentToAndroid => ", _sResult);
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetQAScannedPODetails - Exception =>", ex.ToString());
                throw ex;
            }
            return _sResult;
        }

        internal string GetQASelectedPOInspMatDetails(string _sLocationCode, string _sPONo, string _sInpsLotNo, string _sUserID)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetQASelectedPOInspMatDetails - RequestDataFromAndroid => ", "LocationCode : " + _sLocationCode + ", PONo. : " + _sPONo + ", InspectionLotNo. : " + _sInpsLotNo + ", UserID : " + _sUserID);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETQAINSPLOTMATDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@PONo", _sPONo),
                                        new SqlParameter("@InspLotNo", _sInpsLotNo),
                                        new SqlParameter("@CreatedBy", _sUserID),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LocationPrinting", parma);
                if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                {
                    _sResult = "GETQAINSPLOTMATDETAILS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("MatCode") && dt.Rows.Count > 0)
                {
                    _sResult = "GETQAINSPLOTMATDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString2(dt);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETQAINSPLOTMATDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetQAScannedPODetails - ResponceSentToAndroid => ", _sResult);
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetQAScannedPODetails - Exception =>", ex.ToString());
                throw ex;
            }
            return _sResult;
        }

        internal string UpdatedQAPOInspLotMatRejectedStatus(string _sLocationCode, string _sPONo, string _sInpsLotNo, string _sMatCode, string _sUserID)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdatedQAPOInspLotMatRejectedStatus - RequestDataFromAndroid => ", "LocationCode : " + _sLocationCode + ", PONo. : " + _sPONo + ", InspectionLotNo. : " + _sInpsLotNo + ", MatCode : " + _sMatCode + ", UserID : " + _sUserID);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","UPDATEQAINSPLOTMATREJECTED"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@PONo", _sPONo),
                                        new SqlParameter("@InspLotNo", _sInpsLotNo),
                                        new SqlParameter("@MatCode", _sMatCode),
                                        new SqlParameter("@CreatedBy", _sUserID),
                                        new SqlParameter("@RejCode", "LREJ"),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LocationPrinting", parma);
                if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                {
                    _sResult = "UPDATEQAINSPLOTMATREJECTED ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows.Count == 1)
                {
                    _sResult = "UPDATEQAINSPLOTMATREJECTED ~ SUCCESS ~ Selected InspectionLotNo - " + _sInpsLotNo + " is rejected successfully";
                    return _sResult;
                }
                else
                {
                    _sResult = "UPDATEQAINSPLOTMATREJECTED ~ ERROR ~ " + "NO DETAILS FOUND";
                }
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdatedQAPOInspLotMatRejectedStatus - ResponceSentToAndroid => ", _sResult);
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdatedQAPOInspLotMatRejectedStatus - Exception =>", ex.ToString());
                throw ex;
            }
            return _sResult;
        }

        internal string UpdatedQAPOInspLotMatAcceptedStatus(string _sLocationCode, string _sPONo, string _sInpsLotNo, string _sMatCode, string _sUserID)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdatedQAPOInspLotMatAcceptedStatus - RequestDataFromAndroid => ", "LocationCode : " + _sLocationCode + ", PONo. : " + _sPONo + ", InspectionLotNo. : " + _sInpsLotNo + ", MatCode : " + _sMatCode + ", UserID : " + _sUserID);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","UPDATEQAINSPLOTMATACCEPTED"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@PONo", _sPONo),
                                        new SqlParameter("@InspLotNo", _sInpsLotNo),
                                        new SqlParameter("@MatCode", _sMatCode),
                                        new SqlParameter("@CreatedBy", _sUserID),
                                        new SqlParameter("@RejCode", "OK"),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LocationPrinting", parma);
                if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                {
                    _sResult = "UPDATEQAINSPLOTMATACCEPTED ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows.Count == 1)
                {
                    _sResult = "UPDATEQAINSPLOTMATACCEPTED ~ SUCCESS ~ Selected InspectionLotNo - " + _sInpsLotNo + " is accepted successfully";
                    return _sResult;
                }
                else
                {
                    _sResult = "UPDATEQAINSPLOTMATACCEPTED ~ ERROR ~ " + "NO DETAILS FOUND";
                }
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdatedQAPOInspLotMatAcceptedStatus - ResponceSentToAndroid => ", _sResult);
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdatedQAPOInspLotMatAcceptedStatus - Exception =>", ex.ToString());
                throw ex;
            }
            return _sResult;
        }

        internal string GetQASelectedMatDetails(string _sLocationCode)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetQASelectedPOInspMatDetails - RequestDataFromAndroid => ", "LocationCode : " + _sLocationCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETQASELECTEDMATDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                   };
                dtMat = new DataTable();
                dtMat = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LocationPrinting", parma);
                if ((dtMat.Columns.Contains("ERROR") || dtMat.Columns.Contains("ErrorMessage")) && dtMat.Rows.Count > 0)
                {
                    _sResult = "GETQASELECTEDMATDETAILS ~ ERROR ~ " + dtMat.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtMat.Columns.Contains("MatCode") && dtMat.Rows.Count == 1)
                {
                    DataTable dt1 = new DataTable();
                    dt1 = dtMat.Copy();
                    dt1.Columns.Remove("Thickness");
                    dt1.Columns.Remove("ThicknessDescription");
                    dt1.Columns.Remove("Grade");
                    dt1.Columns.Remove("GradeDescription");
                    dt1.Columns.Remove("Category");
                    dt1.Columns.Remove("CategoryDescription");
                    dt1.Columns.Remove("MatGroup");
                    dt1.Columns.Remove("MatGroupDescription");
                    dt1.Columns.Remove("Size");
                    _sResult = "GETQASELECTEDMATDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString2(dt1);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETQASELECTEDMATDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetQAScannedPODetails - ResponceSentToAndroid => ", _sResult);
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetQAScannedPODetails - Exception =>", ex.ToString());
                throw ex;
            }
            return _sResult;
        }

        internal string UpdateBoilingScannedStatus(string _sLocationCode, string _sPONo, string _sInpsLotNo, string _sOldMatCode, string _sNewMatCode, string _sQRCode, string _sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateBoilingScannedStatus - RequestDataFromAndroid => ", "LocationCode : " + _sLocationCode + ", PONo. : " + _sPONo + ", InspectionLotNo. : " + _sInpsLotNo + ", OldMatCode : " + _sOldMatCode + ", NewMatCode : " + _sNewMatCode + ", QRCode : " + _sQRCode + ", UserID : " + _sUserId);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type", "INSERTQABOILEDTESTQRCODESTATUS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@PONo", _sPONo),
                                        new SqlParameter("@InspLotNo", _sInpsLotNo),
                                        new SqlParameter("@MatCode", _sOldMatCode),
                                        new SqlParameter("@NewMatCode", _sNewMatCode),
                                        new SqlParameter("@CreatedBy", _sUserId),
                                        new SqlParameter("@QRCode", _sQRCode),
                                        new SqlParameter("@RejCode", "BOIL"),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LocationPrinting", parma);
                if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                {
                    _sResult = "INSERTQABOILEDTESTQRCODESTATUS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("Matcode") && dt.Rows.Count == 1)
                {
                    string _Matcode = dt.Rows[0][0].ToString();
                    string _Grade = dt.Rows[0][1].ToString();
                    string _MatGroup = dt.Rows[0][2].ToString();
                    if (_MatGroup != "" && _MatGroup.Length >= 4)
                        _MatGroup = _MatGroup.Substring(_MatGroup.Length - 4);
                    string _MatGroupDescription = dt.Rows[0][3].ToString();
                    string _ThicknessDescription = dt.Rows[0][4].ToString();
                    string _Size = dt.Rows[0][5].ToString();
                    _sResult = PrintBoilingTestQRCodeItem(_sQRCode, _Grade, _MatGroup, _MatGroupDescription, _ThicknessDescription, _Size);
                    if (_sResult.Contains("SUCCESS"))
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateBoilingScannedStatus - ResponceSentToAndroid => ", _sResult);
                        _sResult = "INSERTQABOILEDTESTQRCODESTATUS ~ SUCCESS ~ Boiling test successful and QRCode - " + _sQRCode + " printed successfully";
                    }
                    else if (_sResult.Contains("Not in Network"))
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateBoilingScannedStatus - ResponceSentToAndroid => ", _sResult);
                        _sResult = "INSERTQABOILEDTESTQRCODESTATUS ~ SUCCESS ~ Boiling test successful but QRCode - " + _sQRCode + " Printing Error - " + _sResult.ToString() + ", Kindly Reprint scanned QRCode";
                    }
                    else //if (_sResult.Contains("Not in Network"))
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateBoilingScannedStatus - ResponceSentToAndroid => ", _sResult);
                        _sResult = "INSERTQABOILEDTESTQRCODESTATUS ~ SUCCESS ~ Boiling test successful but QRCode - " + _sQRCode + " Printing Error - " + _sResult.ToString();
                    }
                    return _sResult;
                }
                else
                {
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateBoilingScannedStatus - ResponceSentToAndroid => ", _sResult);
                    _sResult = "INSERTQABOILEDTESTQRCODESTATUS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "UpdateBoilingScannedStatus - Exception =>", ex.ToString());
                throw ex;
            }
            return _sResult;
        }

        public string PrintBoilingTestQRCodeItem(string sQRCode, string sGrade, string sGroup, string sGroupDesc, string sThicknessDesc, string sSize)
        {
            try
            {
                _bcilNetwork = new BcilNetwork();
                string OutMsg = string.Empty;
                var sReadPrn = string.Empty;
                string sPrnExist = string.Empty;
                _bcilNetwork.PrinterIP = Properties.Settings.Default.ExistingQRCodePrinterIP;
                _bcilNetwork.PrinterPort = Properties.Settings.Default.ExistingQRCodePrinterPort;

                OutMsg = _bcilNetwork.NetworkPrinterStatus();
                if (OutMsg == "PRINTER READY")
                {
                    StringBuilder sb = new StringBuilder();
                    DataTable dt = new DataTable();
                    sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyHubQRCode.PRN";
                    if (File.Exists(sPrnExist))
                    {
                        StreamReader sr = new StreamReader(sPrnExist);
                        sReadPrn = sr.ReadToEnd();
                        sr.Close();
                        sr.Dispose();
                    }
                    else
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintBoilingTestQRCode - Error =>", "GreenplyHubQRCode.PRN File Not Found");
                        throw new Exception("PRN File Not Found");
                    }
                    string objRest = sGrade + "-" + sGroupDesc + "-" + sThicknessDesc + "-" + sSize;
                    string objFull = sQRCode + "-" + objRest;
                    sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objFull));
                    sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(sQRCode));
                    sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(sGrade + "-" + sGroup));
                    sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(sThicknessDesc + "-" + sSize));
                    _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyHubQRCode.PRN";
                    OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                    //OutMsg = "SUCCESS";
                    _bcilNetwork.Dispose();
                    if (OutMsg == "SUCCESS")
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintBoilingTestQRCode -", " PrintQRCodeItem => " + "QRCode - " + sQRCode + " printed successfully");
                        return OutMsg = "SUCCESS~Printed Successfully";
                    }
                }
                else
                {
                    if (OutMsg == "PRINTER NOT IN NETWORK")
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintBoilingTestQRCode - Error =>", " Printer IP :" + _bcilNetwork.PrinterIP + " not in network");
                        return OutMsg = "PrintBoilingTestQRCode Printer IP :" + _bcilNetwork.PrinterIP + " Not in Network";
                    }
                    else
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintBoilingTestQRCode - Error =>", " Printer error is : " + OutMsg);
                        return OutMsg;
                    }
                }
                return OutMsg;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintBoilingTestQRCode - Error =>", " PrintBoilingTestQRCode : PrintQRCodeItem => " + ex.ToString());
                return "ERROR | " + ex.Message;
            }
        }

        internal string UpdatedQAPOInspLotBoilTestRejected(string _sLocationCode, string _sPONo, string _sInpsLotNo, string _sUserID)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdatedQAPOInspLotBoilTestRejected - RequestDataFromAndroid => ", "LocationCode : " + _sLocationCode + ", PONo. : " + _sPONo + ", InspectionLotNo. : " + _sInpsLotNo + ", UserID : " + _sUserID);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","UPDATEQAINSPLOTBOILTESTREJECTED"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@PONo", _sPONo),
                                        new SqlParameter("@InspLotNo", _sInpsLotNo),
                                        //new SqlParameter("@MatCode", _sMatCode),
                                        new SqlParameter("@CreatedBy", _sUserID),
                                        new SqlParameter("@RejCode", "LREJ"),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LocationPrinting", parma);
                if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                {
                    _sResult = "UPDATEQAINSPLOTBOILTESTREJECTED ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdatedQAPOInspLotBoilTestRejected - ResponceSentToAndroid => ", _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows.Count == 1)
                {
                    _sResult = "UPDATEQAINSPLOTBOILTESTREJECTED ~ SUCCESS ~ InspectionLotNo - " + _sInpsLotNo + " is rejected on basis of Boiled test";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdatedQAPOInspLotBoilTestRejected - ResponceSentToAndroid => ", _sResult);
                    return _sResult;
                }
                else
                {
                    _sResult = "UPDATEQAINSPLOTBOILTESTREJECTED ~ ERROR ~ " + "NO DETAILS FOUND";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdatedQAPOInspLotBoilTestRejected - ResponceSentToAndroid => ", _sResult);
                    return _sResult;
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdatedQAPOInspLotBoilTestRejected - Exception =>", ex.ToString());
                throw ex;
            }
            return _sResult;
        }


        internal string UpdateQAPartialRejectionQRCodeStatus(string _sLocationCode, string _sPONo, string _sInpsLotNo, string _sOldMatCode, string _sNewMatCode, string _sQRCode, string _sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdatePartialRejectionQRCodeStatus - RequestDataFromAndroid => ", "LocationCode : " + _sLocationCode + ", PONo. : " + _sPONo + ", InspectionLotNo. : " + _sInpsLotNo + ", UserID : " + _sUserId);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type", "UPDATEQAPARTIALREJECTIONQRCODESTATUS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@PONo", _sPONo),
                                        new SqlParameter("@InspLotNo", _sInpsLotNo),
                                        new SqlParameter("@MatCode", _sOldMatCode),
                                        new SqlParameter("@NewMatCode", _sNewMatCode),
                                        new SqlParameter("@CreatedBy", _sUserId),
                                        new SqlParameter("@QRCode", _sQRCode),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LocationPrinting", parma);
                if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                {
                    _sResult = "UPDATEQAPARTIALREJECTIONQRCODESTATUS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows[0][0].ToString() == "SUCCESS")
                {
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdatePartialRejectionQRCodeStatus - ResponceSentToAndroid => ", _sResult);
                    _sResult = "UPDATEQAPARTIALREJECTIONQRCODESTATUS ~ SUCCESS ~ Boiled test and QRCode - " + _sQRCode + " printed successfully";
                    return _sResult;
                }
                else
                {
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdatePartialRejectionQRCodeStatus - ResponceSentToAndroid => ", _sResult);
                    _sResult = "UPDATEQAPARTIALREJECTIONQRCODESTATUS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "UpdatePartialRejectionQRCodeStatus - Exception =>", ex.ToString());
                throw ex;
            }
            return _sResult;
        }

        internal string GetQARejectionCode(string _sLocationCode, string _sRejQRCode, string _sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetQARejectionCode - RequestDataFromAndroid => ", "LocationCode : " + _sLocationCode + ", RejCode : " + _sRejQRCode + ", UserID : " + _sUserId);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type", "GETQAREJECTIONCODE"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@RejCode", _sRejQRCode),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LocationPrinting", parma);
                if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                {
                    _sResult = "GETQAREJECTIONCODE ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetQARejectionCode - ResponceSentToAndroid => ", _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("RejDescription") && dt.Rows.Count == 1)
                {
                    _sResult = "GETQAREJECTIONCODE ~ SUCCESS ~ " + GlobalVariable.DtToString1(dt);
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetQARejectionCode - ResponceSentToAndroid => ", _sResult);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETQAREJECTIONCODE ~ ERROR ~ " + "NO DETAILS FOUND";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetQARejectionCode - ResponceSentToAndroid => ", _sResult);
                    return _sResult;
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "GetQARejectionCode - Exception =>", ex.ToString());
                throw ex;
            }
            return _sResult;
        }

        internal string UpdatedQAPartialRejectionScannedQRCodeStatus(string _sLocationCode, string _sPONo, string _sInpsLotNo, string _sMatCode, string _RejCode, string _QRCode, string _sUserID)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdatedQASelectedPOInspLotMatStatus - RequestDataFromAndroid => ", "LocationCode : " + _sLocationCode + ", PONo. : " + _sPONo + ", InspectionLotNo. : " + _sInpsLotNo + ", MatCode : " + _sMatCode + ", QRCode : " + _QRCode + ", RejCode : " + _RejCode + ", UserID : " + _sUserID);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","UPDATEQAPARTIALREJECTIONSCANNEDQRCODEDETAIL"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@PONo", _sPONo),
                                        new SqlParameter("@InspLotNo", _sInpsLotNo),
                                        new SqlParameter("@MatCode", _sMatCode),
                                        new SqlParameter("@QRCode", _QRCode),
                                        new SqlParameter("@RejCode", _RejCode),
                                        new SqlParameter("@CreatedBy", _sUserID),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LocationPrinting", parma);
                if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                {
                    _sResult = "UPDATEQAPARTIALREJECTIONSCANNEDQRCODEDETAIL ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows.Count == 1)
                {
                    _sResult = "UPDATEQAPARTIALREJECTIONSCANNEDQRCODEDETAIL ~ SUCCESS ~ Scanned QRCode - " + _QRCode + " is rejected successfully";
                    return _sResult;
                }
                else
                {
                    _sResult = "UPDATEQAPARTIALREJECTIONSCANNEDQRCODEDETAIL ~ ERROR ~ " + "NO DETAILS FOUND";
                }
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdatedQASelectedPOInspLotMatStatus - ResponceSentToAndroid => ", _sResult);
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdatedQASelectedPOInspLotMatStatus - Exception =>", ex.ToString());
                throw ex;
            }
            return _sResult;
        }

        internal string UpdatedQAInspectionLotStatus(string _sLocationCode, string _sPONo, string _sInpsLotNo, string _sMatCode, string _sUserID)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdatedQAInspectionLotStatus - RequestDataFromAndroid => ", "LocationCode : " + _sLocationCode + ", PONo. : " + _sPONo + ", InspectionLotNo. : " + _sInpsLotNo + ", MatCode : " + _sMatCode + ", UserID : " + _sUserID);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","UPDATEQAINSPECTIONLOT"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@PONo", _sPONo),
                                        new SqlParameter("@InspLotNo", _sInpsLotNo),
                                        new SqlParameter("@MatCode", _sMatCode),
                                        new SqlParameter("@CreatedBy", _sUserID),
                                        new SqlParameter("@RejCode", "OK"),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LocationPrinting", parma);
                if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                {
                    _sResult = "UPDATEQAINSPECTIONLOT ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows.Count == 1)
                {
                    _sResult = "UPDATEQAINSPECTIONLOT ~ SUCCESS ~ Remaining Quantity of Selected InspectionLotNo. - " + _sInpsLotNo + " is updated as OK successfully";
                    return _sResult;
                }
                else
                {
                    _sResult = "UPDATEQAINSPECTIONLOT ~ ERROR ~ " + "NO DETAILS FOUND";
                }
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdatedQAInspectionLotStatus - ResponceSentToAndroid => ", _sResult);
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdatedQAInspectionLotStatus - Exception =>", ex.ToString());
                throw ex;
            }
            return _sResult;
        }


        #endregion


        #region MTM For Rejected Material
        internal string GetQAMTMSelectedMatDetails(string _sLocationCode)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GETQAMTMSELECTEDMATDETAILS - RequestDataFromAndroid => ", "LocationCode : " + _sLocationCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETQASELECTEDMATDETAILS"),
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                   };
                dtMat = new DataTable();
                dtMat = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LocationPrinting", parma);
                if ((dtMat.Columns.Contains("ERROR") || dtMat.Columns.Contains("ErrorMessage")) && dtMat.Rows.Count > 0)
                {
                    _sResult = "GETQAMTMSELECTEDMATDETAILS ~ ERROR ~ " + dtMat.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtMat.Columns.Contains("MatCode") && dtMat.Rows.Count == 1)
                {
                    DataTable dt1 = new DataTable();
                    dt1 = dtMat.Copy();
                    dt1.Columns.Remove("Thickness");
                    dt1.Columns.Remove("ThicknessDescription");
                    dt1.Columns.Remove("Grade");
                    dt1.Columns.Remove("GradeDescription");
                    dt1.Columns.Remove("Category");
                    dt1.Columns.Remove("CategoryDescription");
                    dt1.Columns.Remove("MatGroup");
                    dt1.Columns.Remove("MatGroupDescription");
                    dt1.Columns.Remove("Size");
                    _sResult = "GETQAMTMSELECTEDMATDETAILS ~ SUCCESS ~ " + GlobalVariable.DtToString2(dt1);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETQAMTMSELECTEDMATDETAILS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GETQAMTMSELECTEDMATDETAILS - ResponceSentToAndroid => ", _sResult);
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GETQAMTMSELECTEDMATDETAILS - Exception =>", ex.ToString());
                throw ex;
            }
            return _sResult;
        }

        //internal string UpdateQAMTMRejectedScannedQRCodeStatus(string _sLocationCode, string _sQRCode, string _sUserId)
        //{
        //    string _sResult = string.Empty;
        //    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateQAMTMRejectedQRCodeScannedStatus - RequestDataFromAndroid => ", "LocationCode : " + _sLocationCode + ", QRCode : " + _sQRCode + ", UserID : " + _sUserId);
        //    try
        //    {
        //        SqlParameter[] parma = {
        //                                new SqlParameter("@Type", "UPDATEMTMREJECTEDQRCODESCANNEDSTATUS"),//
        //                                new SqlParameter("@LocationCode", _sLocationCode),
        //                                new SqlParameter("@CreatedBy", _sUserId),
        //                                new SqlParameter("@QRCode", _sQRCode),
        //                           };
        //        DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LocationPrinting", parma);
        //        if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
        //        {
        //            _sResult = "UPDATEMTMREJECTEDQRCODESCANNEDSTATUS ~ ERROR ~ " + dt.Rows[0][0].ToString();
        //            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateQAMTMRejectedQRCodeScannedStatus - ResponceSentToAndroid => ", _sResult);
        //            return _sResult;
        //        }
        //        if (dt.Columns.Contains("STATUS") && dt.Rows[0][0].ToString() == "SUCCESS")
        //        {
        //            _sResult = "UPDATEMTMREJECTEDQRCODESCANNEDSTATUS ~ SUCCESS ~ QRCode - " + _sQRCode + " scanned successfully";
        //            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateQAMTMRejectedQRCodeScannedStatus - ResponceSentToAndroid => ", _sResult);
        //            return _sResult;
        //        }
        //        else
        //        {
        //            _sResult = "UPDATEMTMREJECTEDQRCODESCANNEDSTATUS ~ ERROR ~ " + "NO DETAILS FOUND";
        //            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateQAMTMRejectedQRCodeScannedStatus - ResponceSentToAndroid => ", _sResult);
        //            return _sResult;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "UpdateQAMTMRejectedQRCodeScannedStatus - Exception =>", ex.ToString());
        //        throw ex;
        //    }
        //    return _sResult;
        //}

        internal string UpdateQAMTMRejectedScannedQRCodeStatus(string _sLocationCode, string _sNewMatCode, string _sQRCode, string _sUserId)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateQAMTMRejectedQRCodeScannedStatus - RequestDataFromAndroid => ", "LocationCode : " + _sLocationCode + ", QRCode : " + _sQRCode + ", UserID : " + _sUserId);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type", "UPDATEMTMREJECTEDQRCODESCANNEDSTATUS"),//
                                        new SqlParameter("@LocationCode", _sLocationCode),
                                        new SqlParameter("@NewMatCode", _sNewMatCode),
                                        new SqlParameter("@CreatedBy", _sUserId),
                                        new SqlParameter("@QRCode", _sQRCode),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_LocationPrinting", parma);
                if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                {
                    _sResult = "UPDATEMTMREJECTEDQRCODESCANNEDSTATUS ~ ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateQAMTMRejectedQRCodeScannedStatus - ResponceSentToAndroid => ", _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("Matcode") && dt.Rows.Count == 1)
                {
                    string _Matcode = dt.Rows[0][0].ToString();
                    string _Grade = dt.Rows[0][1].ToString();
                    string _MatGroup = dt.Rows[0][2].ToString();
                    if (_MatGroup != "" && _MatGroup.Length >= 4)
                        _MatGroup = _MatGroup.Substring(_MatGroup.Length - 4);
                    string _MatGroupDescription = dt.Rows[0][3].ToString();
                    string _ThicknessDescription = dt.Rows[0][4].ToString();
                    string _Size = dt.Rows[0][5].ToString();
                    _sResult = PrintMTMRejectedQRCodes(_sQRCode, _Grade, _MatGroup, _MatGroupDescription, _ThicknessDescription, _Size);
                    if (_sResult.Contains("SUCCESS"))
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateQAMTMRejectedQRCodeScannedStatus - ResponceSentToAndroid => ", _sResult);
                        _sResult = "UPDATEMTMREJECTEDQRCODESCANNEDSTATUS ~ SUCCESS ~ MTM Transfer successful and QRCode - " + _sQRCode + " printed successfully";
                    }
                    else if (_sResult.Contains("Not in Network"))
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateQAMTMRejectedQRCodeScannedStatus - ResponceSentToAndroid => ", _sResult);
                        _sResult = "UPDATEMTMREJECTEDQRCODESCANNEDSTATUS ~ SUCCESS ~ MTM Transfer successful but QRCode - " + _sQRCode + " Printing Error - " + _sResult.ToString() + ", Kindly Reprint scanned QRCode";
                    }
                    else //if (_sResult.Contains("Not in Network"))
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateQAMTMRejectedQRCodeScannedStatus - ResponceSentToAndroid => ", _sResult);
                        _sResult = "UPDATEMTMREJECTEDQRCODESCANNEDSTATUS ~ SUCCESS ~ MTM Transfer successful but QRCode - " + _sQRCode + " Printing Error - " + _sResult.ToString();
                    }
                    return _sResult;
                }
                else
                {
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateQAMTMRejectedQRCodeScannedStatus - ResponceSentToAndroid => ", _sResult);
                    _sResult = "UPDATEMTMREJECTEDQRCODESCANNEDSTATUS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
                //if (dt.Columns.Contains("STATUS") && dt.Rows[0][0].ToString() == "SUCCESS")
                //{
                //    _sResult = "UPDATEMTMREJECTEDQRCODESCANNEDSTATUS ~ SUCCESS ~ QRCode - " + _sQRCode + " scanned successfully";
                //    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateQAMTMRejectedQRCodeScannedStatus - ResponceSentToAndroid => ", _sResult);
                //    return _sResult;
                //}
                //else
                //{
                //    _sResult = "UPDATEMTMREJECTEDQRCODESCANNEDSTATUS ~ ERROR ~ " + "NO DETAILS FOUND";
                //    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "UpdateQAMTMRejectedQRCodeScannedStatus - ResponceSentToAndroid => ", _sResult);
                //    return _sResult;
                //}
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "UpdateQAMTMRejectedQRCodeScannedStatus - Exception =>", ex.ToString());
                throw ex;
            }
            return _sResult;
        }

        public string PrintMTMRejectedQRCodes(string sQRCode, string sGrade, string sGroup, string sGroupDesc, string sThicknessDesc, string sSize)
        {
            try
            {
                _bcilNetwork = new BcilNetwork();
                string OutMsg = string.Empty;
                var sReadPrn = string.Empty;
                string sPrnExist = string.Empty;
                _bcilNetwork.PrinterIP = Properties.Settings.Default.ExistingQRCodePrinterIP;
                _bcilNetwork.PrinterPort = Properties.Settings.Default.ExistingQRCodePrinterPort;

                OutMsg = _bcilNetwork.NetworkPrinterStatus();
                if (OutMsg == "PRINTER READY")
                {
                    StringBuilder sb = new StringBuilder();
                    DataTable dt = new DataTable();
                    sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyHubQRCode.PRN";
                    if (File.Exists(sPrnExist))
                    {
                        StreamReader sr = new StreamReader(sPrnExist);
                        sReadPrn = sr.ReadToEnd();
                        sr.Close();
                        sr.Dispose();
                    }
                    else
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintMTMRejectedQRCodes - Error =>", "GreenplyHubQRCode.PRN File Not Found");
                        throw new Exception("PRN File Not Found");
                    }
                    string objRest = sGrade + "-" + sGroupDesc + "-" + sThicknessDesc + "-" + sSize;
                    string objFull = sQRCode + "-" + objRest;
                    sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objFull));
                    sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(sQRCode));
                    sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(sGrade + "-" + sGroup));
                    sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(sThicknessDesc + "-" + sSize));
                    _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "GreenplyHubQRCode.PRN";
                    OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                    //OutMsg = "SUCCESS";
                    _bcilNetwork.Dispose();
                    if (OutMsg == "SUCCESS")
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintMTMRejectedQRCodes -", " PrintQRCodeItem => " + "QRCode - " + sQRCode + " printed successfully");
                        return OutMsg = "SUCCESS~Printed Successfully";
                    }
                }
                else
                {
                    if (OutMsg == "PRINTER NOT IN NETWORK")
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintMTMRejectedQRCodes - Error =>", " Printer IP :" + _bcilNetwork.PrinterIP + " not in network");
                        return OutMsg = "PrintMTMRejectedQRCodes Printer IP :" + _bcilNetwork.PrinterIP + " Not in Network";
                    }
                    else
                    {
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintMTMRejectedQRCodes - Error =>", " Printer error is : " + OutMsg);
                        return OutMsg;
                    }
                }
                return OutMsg;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintMTMRejectedQRCodes - Error =>", " PrintQRCodeItem => " + ex.ToString());
                return "ERROR | " + ex.Message;
            }
        }

        #endregion
    }
}
