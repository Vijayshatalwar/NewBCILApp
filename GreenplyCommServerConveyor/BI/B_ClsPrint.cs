using System;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GreenplyCommServer.Common;
using GreenplyCommServer;
using System.Net;
using BCILLogger;
using System.IO;
using TEST;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace GreenplyCommServer.BI
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
        ClientSocket objBin = new ClientSocket();
        bool bHardwareDisconnected = false;
        public string ServerIP { get; set; }
        public int Port { get; set; }

        public string PrintQRCodeItem(string _strLocationCode, string sMatCode, string sMatDesc, string sGrade, string sGroup, string sGroupDesc, string sThickness, string sThicknessDesc, string sSize, string objMatDesignNo, string objMatFinishCode, string objMatBatchNo, string sQRCode, string sMatStatus, string sDateFormat, string PrintSection, string LocationType, int sMatPrintCount)
        {
            string OutMsg = string.Empty;
            var sReadPrn = string.Empty;
            _bcilNetwork = new BcilNetwork();

            try
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", "DefaultPrinter : " + GreenplyCommServer.Properties.Settings.Default.DefaultPrinter.ToString());
                
                #region Zebra printer
                if (GreenplyCommServer.Properties.Settings.Default.DefaultPrinter.ToUpper().ToString() == "ZEBRA")
                {
                    _bcilNetwork.PrinterIP = GreenplyCommServer.Properties.Settings.Default.ZebraQRCodePrinterIP;
                    _bcilNetwork.PrinterPort = GreenplyCommServer.Properties.Settings.Default.ZebraQRCodePrinterPort;

                    OutMsg = _bcilNetwork.NetworkPrinterStatus();
                    if (OutMsg == "PRINTER READY")
                    {
                        #region Decorative
                        if (Properties.Settings.Default.PrintingSection.ToUpper().Contains("DECOR"))
                        {
                            string sSaveStatus = SaveQRCode(_strLocationCode.Trim(), sMatCode.Trim(), objMatBatchNo.Trim(), sQRCode.Trim(), sMatStatus.Trim(), sDateFormat.Trim(), PrintSection.Trim(), LocationType.Trim(), sMatPrintCount);
                            if (sSaveStatus.Contains("SUCCESS"))
                            {
                                StringBuilder sb = new StringBuilder();
                                DataTable dt = new DataTable();
                                string sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "ZebraGreenplyVQRCode.PRN";
                                if (File.Exists(sPrnExist))
                                {
                                    StreamReader sr = new StreamReader(sPrnExist);
                                    sReadPrn = sr.ReadToEnd();
                                    sr.Close();
                                    sr.Dispose();
                                }
                                else
                                {
                                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintDecorQRCodeItem_Zebra", "ZebraGreenplyVQRCode.PRN PRN File Not Found");
                                    throw new Exception("PRN File Not Found");
                                }
                                if (sGroup.Trim() != "" && sGroup.Length >= 4)
                                    sGroup = sGroup.Substring(sGroup.Length - 4); //
                                string objRest1 = sGrade.Trim() + "-" + sGroup.Trim() + "-" + sThicknessDesc.Trim() + "-" + sSize.Trim();
                                string objRest2 = objMatDesignNo.Trim() + "-" + objMatFinishCode.Trim() + "-" + objMatBatchNo.Trim();
                                string objFull = sQRCode.Trim() + "-" + sGrade.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sSize.Trim() + "-" + objRest2.Trim();
                                sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objFull.Trim()));
                                sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(sQRCode.Trim()));
                                sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(objRest1.Trim()));
                                sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(objRest2.Trim()));
                                _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "ZebraGreenplyVQRCode.PRN";

                                OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                                _bcilNetwork.Dispose();
                                if (OutMsg == "SUCCESS")
                                {
                                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintDecorQRCodeItem_Zebra", "Barcode - " + sQRCode + " saved and printed successfully");
                                    return OutMsg = "SUCCESS";
                                }
                            }
                            else if (sSaveStatus.Contains("ERROR"))
                            {
                                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintDecorQRCodeItem_Zebra", "Barcode - " + sQRCode + " not saved");
                                return OutMsg;
                            }
                        }
                        #endregion

                        #region Door/Ply
                        else
                        {
                            string sSaveStatus = SaveQRCode(_strLocationCode.Trim(), sMatCode.Trim(), objMatBatchNo.Trim(), sQRCode.Trim(), sMatStatus.Trim(), sDateFormat.Trim(), PrintSection.Trim(), LocationType.Trim(), sMatPrintCount);
                            if (sSaveStatus.Contains("SUCCESS"))
                            {
                                StringBuilder sb = new StringBuilder();
                                DataTable dt = new DataTable();
                                string sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "ZebraGreenplyQRCode.PRN";
                                if (File.Exists(sPrnExist))
                                {
                                    StreamReader sr = new StreamReader(sPrnExist);
                                    sReadPrn = sr.ReadToEnd();
                                    sr.Close();
                                    sr.Dispose();
                                }
                                else
                                {
                                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintPlyDoorQRCodeItem_Zebra", "ZebraGreenplyQRCode.PRN PRN File Not Found");
                                    throw new Exception("PRN File Not Found");
                                }

                                string objFull = sQRCode.Trim() + "-" + sGrade.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sSize.Trim();
                                if (sGroup.Trim() != "" && sGroup.Length >= 4)
                                    sGroup = sGroup.Substring(sGroup.Length - 4);
                                string objRest = sGrade.Trim() + "-" + sGroup.Trim() + "-" + sThicknessDesc.Trim() + "-" + sSize.Trim();

                                sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objFull.Trim()));
                                sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(sQRCode.Trim()));
                                sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(objRest.Trim()));
                                _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "ZebraGreenplyQRCode.PRN";

                                OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                                _bcilNetwork.Dispose();
                                if (OutMsg == "SUCCESS")
                                {
                                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintPlyDoorQRCodeItem_Zebra", "Barcode - " + sQRCode + " saved and printed successfully");
                                    return OutMsg = "SUCCESS";
                                }
                            }
                            else if (sSaveStatus.Contains("ERROR"))
                            {
                                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintPlyDoorQRCodeItem_Zebra", "Barcode - " + sQRCode + " not saved ");
                                return OutMsg;
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        if (OutMsg == "PRINTER NOT IN NETWORK")
                        {
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem_Zebra", "ERROR ~ Zebra QRCode Printer IP - " + _bcilNetwork.PrinterIP + " not in network, Kindly check your network");
                            return OutMsg = "Printer not in network";
                        }
                        else
                        {
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem_Zebra", "ERROR ~ Zebra QRCode Printer IP - " + _bcilNetwork.PrinterIP + ", Error : " + OutMsg);
                            return OutMsg;
                        }
                    }
                }
                #endregion

                #region MarkemImage Printer
                else
                {
                    _bcilNetwork.PrinterIP = GreenplyCommServer.Properties.Settings.Default.IMQRCodePrinterIP;
                    _bcilNetwork.PrinterPort = GreenplyCommServer.Properties.Settings.Default.IMQRCodePrinterPort;

                    string sIP = GreenplyCommServer.Properties.Settings.Default.IMQRCodePrinterIP.Trim();
                    if (objBin.HardwareConnected(sIP) == true && bHardwareDisconnected == false)  //
                    {
                        bHardwareDisconnected = true;
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintDecorQRCodeItem", "Markem Image Printer Status - Connected ");
                    }
                    else if (objBin.HardwareConnected(sIP) == true && bHardwareDisconnected == true)  //
                    {
                        bHardwareDisconnected = false;
                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintDecorQRCodeItem", "Markem Image Printer Status - Not Connected ");
                        objBin.Connect();
                    }
                    if (bHardwareDisconnected == true)
                    {
                        OutMsg = _bcilNetwork.CheckIMNetworkPrinterStatus();  //"!!m.a.i.6?"
                        if (OutMsg == "PRINTER READY")
                        {

                            #region Decorative
                            if (Properties.Settings.Default.PrintingSection.ToUpper().Contains("DECOR"))
                            {
                                string sSaveStatus = SaveQRCode(_strLocationCode.Trim(), sMatCode.Trim(), objMatBatchNo.Trim(), sQRCode.Trim(), sMatStatus.Trim(), sDateFormat.Trim(), PrintSection.Trim(), LocationType.Trim(), sMatPrintCount);
                                if (sSaveStatus.Contains("SUCCESS"))
                                {
                                    StringBuilder sb = new StringBuilder();
                                    DataTable dt = new DataTable();
                                    string sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "MarkemImageGreenplyVQRCode.PRN";
                                    if (File.Exists(sPrnExist))
                                    {
                                        StreamReader sr = new StreamReader(sPrnExist);
                                        sReadPrn = sr.ReadToEnd();
                                        sr.Close();
                                        sr.Dispose();
                                    }
                                    else
                                    {
                                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintDecorQRCodeItemMarkemImage", "MarkemImageGreenplyVQRCode.PRN PRN File Not Found");
                                        throw new Exception("PRN File Not Found");
                                    }
                                    if (sGroup.Trim() != "" && sGroup.Length >= 4)
                                        sGroup = sGroup.Substring(sGroup.Length - 4); //
                                    string objRest1 = sGrade.Trim() + "-" + sGroup.Trim() + "-" + sThicknessDesc.Trim() + "-" + sSize.Trim();
                                    string objRest2 = objMatDesignNo.Trim() + "-" + objMatFinishCode.Trim() + "-" + objMatBatchNo.Trim();
                                    string objFull = sQRCode.Trim() + "-" + sGrade.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sSize.Trim() + "-" + objRest2.Trim();
                                    sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objFull.Trim()));
                                    sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(sQRCode.Trim()));
                                    sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(objRest1.Trim()));
                                    sReadPrn = sReadPrn.Replace("{VarBarcode4}", Convert.ToString(objRest2.Trim()));
                                    _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "MarkemImageGreenplyVQRCode.PRN";
                                    OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                                    _bcilNetwork.Dispose();
                                    if (OutMsg == "SUCCESS")
                                    {
                                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintDecorQRCodeItemMarkemImage", "Barcode - " + sQRCode + " saved and printed successfully");
                                        return OutMsg = "SUCCESS";
                                    }
                                }
                                else if (sSaveStatus.Contains("ERROR"))
                                {
                                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintDecorQRCodeItemMarkemImage", "Barcode - " + sQRCode + " not saved");
                                    return OutMsg;
                                }
                            }
                            #endregion

                            #region Door/Ply
                            else
                            {
                                string sSaveStatus = SaveQRCode(_strLocationCode.Trim(), sMatCode.Trim(), objMatBatchNo.Trim(), sQRCode.Trim(), sMatStatus.Trim(), sDateFormat.Trim(), PrintSection.Trim(), LocationType.Trim(), sMatPrintCount);
                                if (sSaveStatus.Contains("SUCCESS"))
                                {
                                    StringBuilder sb = new StringBuilder();
                                    DataTable dt = new DataTable();
                                    string sPrnExist = System.Windows.Forms.Application.StartupPath + "\\" + "MarkemImageGreenplyQRCode.PRN";
                                    if (File.Exists(sPrnExist))
                                    {
                                        StreamReader sr = new StreamReader(sPrnExist);
                                        sReadPrn = sr.ReadToEnd();
                                        sr.Close();
                                        sr.Dispose();
                                    }
                                    else
                                    {
                                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintPlyDoorQRCodeItem_MarkemImage", "MarkemImageGreenplyQRCode.PRN PRN File Not Found");
                                        throw new Exception("PRN File Not Found");
                                    }

                                    string objFull = sQRCode.Trim() + "-" + sGrade.Trim() + "-" + sGroupDesc.Trim() + "-" + sThicknessDesc.Trim() + "-" + sSize.Trim();
                                    if (sGroup.Trim() != "" && sGroup.Length >= 4)
                                        sGroup = sGroup.Substring(sGroup.Length - 4);
                                    string objRest = sGrade.Trim() + "-" + sGroup.Trim() + "-" + sThicknessDesc.Trim() + "-" + sSize.Trim();

                                    sReadPrn = sReadPrn.Replace("{VarBarcode1}", Convert.ToString(objFull.Trim()));
                                    sReadPrn = sReadPrn.Replace("{VarBarcode2}", Convert.ToString(sQRCode.Trim()));
                                    sReadPrn = sReadPrn.Replace("{VarBarcode3}", Convert.ToString(objRest.Trim()));
                                    _bcilNetwork.Prn = System.Windows.Forms.Application.StartupPath + "\\" + "MarkemImageGreenplyQRCode.PRN";
                                    OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                                    _bcilNetwork.Dispose();
                                    if (OutMsg == "SUCCESS")
                                    {
                                        VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "PrintPlyDoorQRCodeItem_MarkemImage", "Barcode - " + sQRCode + " saved and printed successfully");
                                        return OutMsg = "SUCCESS";
                                    }
                                }
                                else if (sSaveStatus.Contains("ERROR"))
                                {
                                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem_MarkemImage", "Barcode - " + sQRCode + " not saved ");
                                    return OutMsg;
                                }
                            }
                            #endregion
                        }
                        else
                        {
                            if (OutMsg == "PRINTER NOT IN NETWORK")
                            {
                                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem_MarkemImage", "ERROR ~ MarkemImage QRCode Printer IP - " + _bcilNetwork.PrinterIP + " not in network, Kindly check your network");
                                return OutMsg = "Printer not in network";  //ERROR~
                            }
                            else
                            {
                                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem_MarkemImage", "ERROR ~ MarkemImage QRCode Printer IP - " + _bcilNetwork.PrinterIP + ", Error : " + OutMsg);
                                return OutMsg;
                            }
                        }
                    }
                    else
                    {
                        if (bHardwareDisconnected == false)
                        {
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", "ERROR ~ MarkemImage QRCode Printer IP - " + _bcilNetwork.PrinterIP + " not in network, Kindly check your network");
                            return OutMsg = "Printer not in network";
                        }
                        else
                        {
                            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", "ERROR ~ MarkemImage QRCode Printer IP - " + _bcilNetwork.PrinterIP + ", Error : " + OutMsg);
                            return OutMsg;
                        }
                    }
                }
                #endregion

                return OutMsg;
            }
            catch (Exception ex)
            {
                String exDetail = String.Format(ex.Message, Environment.NewLine, ex.Source, ex.StackTrace);
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "PrintQRCodeItem", exDetail);
                OutMsg = "ERROR | " + ex.Message;
                return OutMsg;
            }
        }

        public string PrintStackQRCodeItem(string _strLocationCode, string sMatCode, string sStackQRCode, string sDateFormat, string PrintSection, string LocationType, string GradeDesc, string GroupDesc, string ThicknessDesc, string MatSize, string LotSize)
        {
            try
            {
                string OutMsg = string.Empty;
                var sReadPrn = string.Empty;

                _bcilNetwork.PrinterIP = GreenplyCommServer.Properties.Settings.Default.StackQRCodePrinterIP;
                _bcilNetwork.PrinterPort = GreenplyCommServer.Properties.Settings.Default.StackQRCodePrinterPort;

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

                        OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
                        _bcilNetwork.Dispose();
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

        internal DataTable GetSelectedMatCode(string objLocationCode)
        {
            DataTable dtData = new DataTable();
            //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSelectedMatCode", "Request started to get selected material details");
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","GETSELECTEDMATERIALCODE"),
                                        //new SqlParameter("@LocationCode", objLocationCode),
                                   };
                //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSelectedMatCode", "Before running procedure");
                dtData = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialMaster", parma);
                //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSelectedMatCode", "After running procedure");
                if (dtData.Columns.Count > 1 && dtData.Rows.Count == 1)
                {
                    //dtData = dt;
                    //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSelectedMatCode", "Received selected material details");
                    //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Responce data =>" + dt.Rows[0][0].ToString());
                    return dtData;
                }
                else
                {
                    //dtData = dt.Clone();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetSelectedMatCode", "There is no data found for selected material for production");
                    return dtData;
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "GetSelectedMatCode", ex.Message.ToString());
                throw ex;
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
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetQRCodeRunningSerial", "Request LocationCode =>" + objLocationCode);
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
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetQRCodeRunningSerial", "Datatable =>" + dt.Rows[0][0].ToString());
                if (dt.Columns.Contains("SERIALNO") && dt.Rows.Count == 1)
                {
                    _sResult = dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetQRCodeRunningSerial", "Responce data =>" + _sResult);
                    return _sResult;
                }
                else
                {
                    _sResult = "0";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "GetQRCodeRunningSerial", "Responce data =>" + _sResult);
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
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + objLocationCode);
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
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "GetStackRunningSerial", ex.Message.ToString());
                throw ex;
            }
        }

        internal string SaveQRCode(string objLocationCode, string objMatCode, string objMatBatchNo, string objQRCode, string sMatStatus, string sDateFormat, string PrintSection, string LocationType, int sMatPrintCount)
        {
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "SaveProductionQRCode", "Request QRCode to Save =>" + objQRCode);
            try
            {
                SqlParameter[] parma = {
                                        new SqlParameter("@Type","SAVEQRCODE"),
                                        new SqlParameter("@LocationCode", objLocationCode),
                                        new SqlParameter("@MatCode", objMatCode),
                                        new SqlParameter("@QRCode", objQRCode),
                                        new SqlParameter("@MatStatus", sMatStatus),
                                        new SqlParameter("@DateFormat", sDateFormat),
                                        new SqlParameter("@PrintSection", PrintSection),
                                        new SqlParameter("@LocationType", LocationType),
                                        new SqlParameter("@CreatedBy", ""),
                                        new SqlParameter("@PONumber", "0"),
                                        new SqlParameter("@VendorCode", "0"),
                                        new SqlParameter("@LabelType", "2X2 Label"),
                                        new SqlParameter("@VBatchNo", objMatBatchNo),
                                   };
                DataTable dt = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialMaster", parma);
                if (dt.Columns.Contains("ERROR"))
                {
                    _sResult = "ERROR ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "SaveProductionQRCode", "Responce data =>" + _sResult);
                    return _sResult;
                }
                if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0)
                {
                    _sResult = "SUCCESS ~ " + dt.Rows[0][0].ToString();
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "SaveProductionQRCode", "Responce data =>" + _sResult);
                    return _sResult;
                }
                else
                {
                    _sResult = "ERROR ~ " + "NOT FOUND";
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "SaveProductionQRCode", "Responce data =>" + _sResult);
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
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + objStackQRCode);
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

                _bcilNetwork.PrinterIP = GreenplyCommServer.Properties.Settings.Default.ZebraQRCodePrinterIP;
                _bcilNetwork.PrinterPort = GreenplyCommServer.Properties.Settings.Default.ZebraQRCodePrinterPort;

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

                            OutMsg = _bcilNetwork.NetworkPrint(sReadPrn);
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
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>" + objQRCode);
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
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtError, "SaveSalesReturnQRCode", ex.Message.ToString());
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
                string ftpaddress = "";  //Properties.Settings.Default.FTPAddress;
                string username = "";  //Properties.Settings.Default.FTPUsername;
                string password = "";  //Properties.Settings.Default.FTPPassword;

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
                string path = "";  //Properties.Settings.Default.LocalFolderPath;
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
    }
}
