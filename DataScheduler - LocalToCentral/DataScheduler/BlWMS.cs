using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;
using System.Globalization;
using DATA_ACCESS_LAYER;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml;
using Microsoft.Exchange.WebServices;
using Microsoft.Exchange;
using Microsoft.Exchange.WebServices.Data;

namespace DataScheduler
{
    public class BlWMS
    {
        WriteLogFile ObjLog = new WriteLogFile();
        DataTable _dtBindList = new DataTable();
        string sEmailSentStatus = string.Empty;
        string lDataSendToSAP = string.Empty;
        string MaxModelNo = string.Empty;
        string MaxVenderCode = string.Empty;
        string MaxCustCode = string.Empty;
        DataSet theDataSet = new DataSet();
        DataSet ds = new DataSet();
        DataSet dsStockData = new DataSet();
        DataSet dsVendorData = new DataSet();
        DataSet dsDOData = new DataSet();
        DataSet dsSRData = new DataSet();
        DataSet dsMTMData = new DataSet();
        DBManager oDbmCentral = new DBManager();
        DBManager oDbmLocal = new DBManager();
        DBManager oDbmSAP = new DBManager();
        DlCommon oDlcom = new DlCommon();
        int iCount = 0;
        string sLogMsg = "";
        DataTable dtStock = new DataTable();
        DataTable dtVendor = new DataTable();
        DataTable dtSRData = new DataTable();
        DataTable dtDOData = new DataTable();
        DataTable dtMTMData = new DataTable();

        DataTable dtPostLocLabelPrinting = new DataTable();
        DataTable dtPostLocPrintingHistory = new DataTable();
        DataTable dtPostVendorLabelGenerating = new DataTable();
        DataTable dtPostVendorLabelPrinting = new DataTable();
        DataTable dtPostDispatch = new DataTable();
        DataTable dtPostDeliveryCancelled = new DataTable();
        DataTable dtPostMatDamage = new DataTable();
        DataTable dtPostSalesReturn = new DataTable();
        DataTable dtPostPurchaseReturn = new DataTable();
        DataTable dtPostItemSerial = new DataTable();
        DataTable dtPostSAPDeliveryOrderData = new DataTable();
        DataTable dtPostSAPPurchaseOrderData = new DataTable();
        DataTable dtPostSAPSalesReturnData = new DataTable();
        DataTable dtPostReturnPOData = new DataTable();
        DataTable dtProductionReversalData = new DataTable();
        DataTable dtMatDamageData = new DataTable();
        DataTable dtPostQAData = new DataTable();
        DataTable dtPostStackHistoryData = new DataTable();

        public BlWMS()
        {
            oDbmLocal = oDlcom.DBProvider();
            oDbmCentral = oDlcom.ClientDBProvider();
            oDbmSAP = oDlcom.DBProvider();
        }


        #region Local To Central

        public void GetL2CWebAPIMastersData()
        {
            clsLogic oLogic = new clsLogic();
            try
            {
                //ds = WEBAPILocalToCentralMastersData();
                //if (ds.Tables.Contains("mUserMaster"))
                //{
                //    GetL2CUserMaster();
                //}
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Error in Sending Local To Central Masters data :: GetL2CWebAPIMastersData is - " + ex.ToString());
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
        }

        public void GetL2CWebAPITrasactionalData()
        {
            try
            {
                ds = GetL2CTransactionalData();  // WEBAPILocalToCentralTrasactionalData();

                #region HUB
                if (Properties.Settings.Default.LocationType.ToUpper().Trim().ToString() == "HUB")
                {
                    dtPostLocLabelPrinting = ds.Tables[0].Copy();
                    dtPostLocPrintingHistory = ds.Tables[1].Copy();
                    dtPostVendorLabelGenerating = ds.Tables[2].Copy();
                    dtPostDispatch = ds.Tables[3].Copy();
                    dtPostDeliveryCancelled = ds.Tables[4].Copy();
                    dtPostMatDamage = ds.Tables[5].Copy();
                    dtPostSalesReturn = ds.Tables[6].Copy();
                    dtPostPurchaseReturn = ds.Tables[7].Copy();
                    dtPostItemSerial = ds.Tables[8].Copy();
                    dtPostSAPDeliveryOrderData = ds.Tables[9].Copy();
                    dtPostSAPPurchaseOrderData = ds.Tables[10].Copy();
                    dtPostSAPSalesReturnData = ds.Tables[11].Copy();
                    dtPostReturnPOData = ds.Tables[12].Copy();
                    dtPostQAData = ds.Tables[13].Copy();
                    dtProductionReversalData = ds.Tables[14].Copy();
                    dtPostStackHistoryData = ds.Tables[15].Copy();

                    dtPostLocLabelPrinting.TableName = "tLocationLabelPrinting";
                    dtPostLocPrintingHistory.TableName = "tLocationPrintingHistory";
                    dtPostVendorLabelGenerating.TableName = "tVendorLabelGenerating";
                    dtPostDispatch.TableName = "tDispatchData";
                    dtPostDeliveryCancelled.TableName = "tDeliveryCancellationData";
                    dtPostMatDamage.TableName = "tMatDamageData";
                    dtPostSalesReturn.TableName = "tSalesReturn";
                    dtPostPurchaseReturn.TableName = "tPurchaseReturn";
                    dtPostItemSerial.TableName = "tItemSerial";
                    dtPostSAPDeliveryOrderData.TableName = "tSAPDeliveryOrderData";
                    dtPostSAPPurchaseOrderData.TableName = "tSAPPurchaseOrderData";
                    dtPostSAPSalesReturnData.TableName = "tSAPSalesReturnData";
                    dtPostReturnPOData.TableName = "tSAPPurchaseReturnData";
                    dtPostQAData.TableName = "tLocationLabelQAData";
                    dtProductionReversalData.TableName = "tProductionReversalData";
                    dtPostStackHistoryData.TableName = "tStackHistoryData";

                    if (dtPostLocLabelPrinting.Rows.Count > 0)
                    {
                        ObjLog.WriteLog("LocalToCentral : " + "LocationLabelPriniting => " + dtPostLocLabelPrinting.Rows.Count.ToString() + " Nos. of records found");
                        GetL2CLocationLabelPrintingDetails();
                    }
                    if (dtPostLocPrintingHistory.Rows.Count > 0)
                    {
                        ObjLog.WriteLog("LocalToCentral : " + "LocationLabelPriniting => " + dtPostLocPrintingHistory.Rows.Count.ToString() + " Nos. of records found");
                        GetL2CLocationPrintingHistoryDetails();
                    }
                    if (dtPostVendorLabelGenerating.Rows.Count > 0)
                    {
                        ObjLog.WriteLog("LocalToCentral : " + "VendorLabelGenerating => " + dtPostVendorLabelGenerating.Rows.Count.ToString() + " Nos. of records found");
                        GetL2CVendorLabelGeneratingDetails();
                    }
                    if (dtPostDispatch.Rows.Count > 0)
                    {
                        ObjLog.WriteLog("LocalToCentral : " + "DispatchData => " + dtPostDispatch.Rows.Count.ToString() + " Nos. of records found");
                        GetL2CtDispatchData();
                    }
                    if (dtPostDeliveryCancelled.Rows.Count > 0)
                    {
                        ObjLog.WriteLog("LocalToCentral : " + "DeliveryCancellationData => " + dtPostDeliveryCancelled.Rows.Count.ToString() + " Nos. of records found");
                        //GetL2CtDeliveryCancellationData();
                    }
                    if (dtPostMatDamage.Rows.Count > 0)
                    {
                        ObjLog.WriteLog("LocalToCentral : " + "MatDamageData => " + dtPostMatDamage.Rows.Count.ToString() + " Nos. of records found");
                        GetL2CtMatDamageData();
                    }
                    if (dtPostSalesReturn.Rows.Count > 0)
                    {
                        ObjLog.WriteLog("LocalToCentral : " + "SalesReturn => " + dtPostSalesReturn.Rows.Count.ToString() + " Nos. of records found");
                        GetL2CSalesReturn();
                    }
                    if (dtPostPurchaseReturn.Rows.Count > 0)
                    {
                        ObjLog.WriteLog("LocalToCentral : " + "PurchaseReturn => " + dtPostPurchaseReturn.Rows.Count.ToString() + " Nos. of records found");
                        GetL2CPurchaseReturn();
                    }
                    if (dtPostSAPPurchaseOrderData.Rows.Count > 0)
                    {
                        ObjLog.WriteLog("LocalToCentral : " + "DispatchData => " + dtPostSAPPurchaseOrderData.Rows.Count.ToString() + " Nos. of records found");
                        GetL2CtSAPPurchaseOrderData();
                    }
                    if (dtPostItemSerial.Rows.Count > 0)
                    {
                        ObjLog.WriteLog("LocalToCentral : " + "ItemSerial => " + dtPostItemSerial.Rows.Count.ToString() + " Nos. of records found");
                        //GetL2CtItemSerial();
                    }
                    if (dtPostQAData.Rows.Count > 0)
                    {
                        ObjLog.WriteLog("LocalToCentral : " + "HubQAData => " + dtPostQAData.Rows.Count.ToString() + " Nos. of records found");
                        GetL2CLocationPrintingQADetails();
                    }
                    if (dtProductionReversalData.Rows.Count > 0)
                    {
                        ObjLog.WriteLog("LocalToCentral : " + "MTMTransferData => " + dtProductionReversalData.Rows.Count.ToString() + " Nos. of records found");
                        GetL2CProductionReversalData();
                    }
                    if (dtPostStackHistoryData.Rows.Count > 0)
                    {
                        ObjLog.WriteLog("LocalToCentral : " + "StackHistory => " + dtPostStackHistoryData.Rows.Count.ToString() + " Nos. of records found");
                        GetL2CStackHistoryData();
                    }
                }
                #endregion

                #region PLANT
                if (Properties.Settings.Default.LocationType.ToUpper().Trim().ToString() == "PLANT")
                {
                    if (Properties.Settings.Default.LocationCode.ToString().Trim() == "2020"
                        && Properties.Settings.Default.MaterialProductType1.ToString().Trim() == "DOOR")
                    {
                        dtPostLocLabelPrinting = ds.Tables[0].Copy();
                        dtPostLocLabelPrinting.TableName = "tLocationLabelPrinting";
                        if (dtPostLocLabelPrinting.Rows.Count > 0)
                        {
                            ObjLog.WriteLog("LocalToCentral : " + "LocationLabelPriniting => " + dtPostLocLabelPrinting.Rows.Count.ToString() + " Nos. of records found");
                            GetL2CLocationLabelPrintingDetails();
                            return;
                        }
                    }
                    else
                    {
                        dtPostLocLabelPrinting = ds.Tables[0].Copy();
                        dtPostDispatch = ds.Tables[1].Copy();
                        dtPostDeliveryCancelled = ds.Tables[2].Copy();
                        dtPostMatDamage = ds.Tables[3].Copy();
                        dtPostSalesReturn = ds.Tables[4].Copy();
                        dtPostPurchaseReturn = ds.Tables[5].Copy();
                        dtPostItemSerial = ds.Tables[6].Copy();
                        dtPostSAPDeliveryOrderData = ds.Tables[7].Copy();
                        dtPostSAPPurchaseOrderData = ds.Tables[8].Copy();
                        dtPostSAPSalesReturnData = ds.Tables[9].Copy();
                        dtPostReturnPOData = ds.Tables[10].Copy();
                        dtProductionReversalData = ds.Tables[11].Copy();
                        dtPostStackHistoryData = ds.Tables[12].Copy();

                        dtPostLocLabelPrinting.TableName = "tLocationLabelPrinting";
                        dtPostDispatch.TableName = "tDispatchData";
                        dtPostDeliveryCancelled.TableName = "tDeliveryCancellationData";
                        dtPostMatDamage.TableName = "tMatDamageData";
                        dtPostSalesReturn.TableName = "tSalesReturn";
                        dtPostPurchaseReturn.TableName = "tPurchaseReturn";
                        dtPostItemSerial.TableName = "tItemSerial";
                        dtPostSAPDeliveryOrderData.TableName = "tSAPDeliveryOrderData";
                        dtPostSAPPurchaseOrderData.TableName = "tSAPPurchaseOrderData";
                        dtPostSAPSalesReturnData.TableName = "tSAPSalesReturnData";
                        dtPostReturnPOData.TableName = "tSAPPurchaseReturnData";
                        dtProductionReversalData.TableName = "tProductionReversalData";
                        dtPostStackHistoryData.TableName = "tStackHistoryData";

                        if (dtPostLocLabelPrinting.Rows.Count > 0)
                        {
                            ObjLog.WriteLog("LocalToCentral : " + "LocationLabelPriniting => " + dtPostLocLabelPrinting.Rows.Count.ToString() + " Nos. of records found");
                            GetL2CLocationLabelPrintingDetails();
                        }
                        if (dtPostDispatch.Rows.Count > 0)
                        {
                            ObjLog.WriteLog("LocalToCentral : " + "DispatchData => " + dtPostDispatch.Rows.Count.ToString() + " Nos. of records found");
                            GetL2CtDispatchData();
                        }
                        if (dtPostDeliveryCancelled.Rows.Count > 0)
                        {
                            ObjLog.WriteLog("LocalToCentral : " + "DeliveryCancellationData => " + dtPostDeliveryCancelled.Rows.Count.ToString() + " Nos. of records found");
                            //GetL2CtDeliveryCancellationData();
                        }
                        if (dtPostMatDamage.Rows.Count > 0)
                        {
                            ObjLog.WriteLog("LocalToCentral : " + "MatDamageData => " + dtPostMatDamage.Rows.Count.ToString() + " Nos. of records found");
                            GetL2CtMatDamageData();
                        }
                        if (dtPostSalesReturn.Rows.Count > 0)
                        {
                            ObjLog.WriteLog("LocalToCentral : " + "SalesReturn => " + dtPostSalesReturn.Rows.Count.ToString() + " Nos. of records found");
                            GetL2CSalesReturn();
                        }
                        if (dtPostPurchaseReturn.Rows.Count > 0)
                        {
                            ObjLog.WriteLog("LocalToCentral : " + "SalesReturn => " + dtPostPurchaseReturn.Rows.Count.ToString() + " Nos. of records found");
                            //GetL2CSalesReturn();
                        }
                        if (dtPostItemSerial.Rows.Count > 0)
                        {
                            ObjLog.WriteLog("LocalToCentral : " + "ItemSerial => " + dtPostItemSerial.Rows.Count.ToString() + " Nos. of records found");
                            //GetL2CtItemSerial();
                        }
                        if (dtPostSAPPurchaseOrderData.Rows.Count > 0)
                        {
                            ObjLog.WriteLog("LocalToCentral : " + "PurchaseOrderData => " + dtPostSAPPurchaseOrderData.Rows.Count.ToString() + " Nos. of records found");
                            //GetL2CtSAPPurchaseOrderData();
                        }
                        if (dtPostSAPDeliveryOrderData.Rows.Count > 0)
                        {
                            ObjLog.WriteLog("LocalToCentral : " + "PurchaseOrderData => " + dtPostSAPDeliveryOrderData.Rows.Count.ToString() + " Nos. of records found");
                            //GetL2CtSAPPurchaseOrderData();
                        }
                        if (dtProductionReversalData.Rows.Count > 0)
                        {
                            ObjLog.WriteLog("LocalToCentral : " + "MTMTransferData => " + dtProductionReversalData.Rows.Count.ToString() + " Nos. of records found");
                            GetL2CProductionReversalData();
                        }
                        if (dtPostStackHistoryData.Rows.Count > 0)
                        {
                            ObjLog.WriteLog("LocalToCentral : " + "StackHistory => " + dtPostStackHistoryData.Rows.Count.ToString() + " Nos. of records found");
                            GetL2CStackHistoryData();
                        }
                    }

                }
                #endregion

                #region VENDOR
                if (Properties.Settings.Default.LocationType.ToUpper().Trim().ToString() == "VENDOR")
                {
                    dtPostVendorLabelPrinting = ds.Tables[0].Copy();
                    dtPostVendorLabelPrinting.TableName = "tVendorLabelPrinting";
                    if (dtPostVendorLabelPrinting.Rows.Count > 0)
                    {
                        GetL2CVendorLabelPrintingDetails();
                    }
                }
                #endregion

            }
            catch (Exception ex)
            {
                ObjLog.WriteLog("Data Scheduler => GetL2CWebAPITrasactionalData : Error - " + ex.ToString());
            }
        }

        public DataSet GetL2CMastersData()
        {
            DataSet Ds = new DataSet();
            try
            {
                oDbmLocal.Open();
                oDbmLocal.CreateParameters(1);
                oDbmLocal.AddParameters(0, "@Type", "mLocalToCentralMasterData");
                Ds = oDbmLocal.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DataTransferCentralAndLocal");
            }
            catch (Exception ex)
            {
                ObjLog.WriteLog("Data Scheduler => mLocalToCentralMasterData : Error - " + ex.ToString());
            }
            finally
            {
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
            return Ds;
        }

        public DataSet GetL2CTransactionalData()
        {
            DataSet Ds = new DataSet();
            try
            {
                oDbmLocal.Open();
                oDbmLocal.CreateParameters(4);
                oDbmLocal.AddParameters(0, "@Type", "mLocalToCentralTransactionalData");
                oDbmLocal.AddParameters(1, "@ProductType", Properties.Settings.Default.MaterialProductType1.ToString().Trim());
                oDbmLocal.AddParameters(2, "@LocationType", Properties.Settings.Default.LocationType.ToString().Trim());
                //oDbm.AddParameters(3, "@POLocType", "H");
                oDbmLocal.AddParameters(3, "@LocationCode", Properties.Settings.Default.LocationCode.ToString().Trim());
                ObjLog.WriteLog("Data Scheduler => mLocalToCentralTransactionalData : GetDataFromDB => ProductType - " + Properties.Settings.Default.MaterialProductType1.ToString() + ", LocationType - " + Properties.Settings.Default.LocationType.ToString().Trim() + ", LocationCode - " + Properties.Settings.Default.LocationCode.ToString().Trim());
                Ds = oDbmLocal.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DataTransferCentralAndLocal");
            }
            catch (Exception ex)
            {
                ObjLog.WriteLog("Data Scheduler => mLocalToCentralTransactionalData : Error - " + ex.ToString());
            }
            finally
            {
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
            return Ds;
        }


        public void GetL2CLocationLabelPrintingDetails()
        {
            PLLocationLabelPrinting _PLPrint = new PLLocationLabelPrinting();
            StringBuilder sb = new StringBuilder();
            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrClientCon);
                _dtBindList = new DataTable();
                if (dtPostLocLabelPrinting.Rows.Count > 0)
                {
                    ObjLog.WriteLog("Data Scheduler : LocaltoCentral LocationLabelPrinting : Data transfer starting to Central");
                    clsGlobal.iAddCount = 0;
                    clsGlobal.iUpdateCount = 0;
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dtPostLocLabelPrinting.Rows)
                    {
                        sb = new StringBuilder();
                        _PLPrint.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLPrint.PONumber = dr["PONumber"].ToString().Trim();
                        _PLPrint.MatCode = dr["MatCode"].ToString().Trim();
                        _PLPrint.QRCode = dr["QRCode"].ToString().Trim();
                        _PLPrint.StackQRCode = dr["StackQRCode"].ToString().Trim();
                        _PLPrint.MatStatus = dr["MatStatus"].ToString().Trim();
                        _PLPrint.OldMatStatus = dr["OldMatStatus"].ToString().Trim();
                        _PLPrint.VendorInvoice = dr["VendorInvoice"].ToString().Trim();
                        _PLPrint.VendorInvoiceDate = dr["VendorInvoiceDate"].ToString().Trim();
                        _PLPrint.VendorCode = dr["VendorCode"].ToString().Trim();
                        _PLPrint.IsQRCodePrinted = dr["IsQRCodePrinted"].ToString().Trim();
                        _PLPrint.IsQRCodeUsed = dr["IsQRCodeUsed"].ToString().Trim();
                        _PLPrint.IsStackPrinted = dr["IsStackPrinted"].ToString().Trim();
                        _PLPrint.IsStackUsed = dr["IsStackUsed"].ToString().Trim();
                        _PLPrint.OldStackQRCode = dr["OldStackQRCode"].ToString().Trim();
                        _PLPrint.OldMatCode = dr["OldMatCode"].ToString().Trim();
                        _PLPrint.Status = Convert.ToInt32(dr["Status"].ToString().Trim());
                        _PLPrint.ReturnStatus = Convert.ToInt32(dr["ReturnStatus"].ToString().Trim());
                        _PLPrint.CreatedBy = dr["CreatedBy"].ToString().Trim();
                        _PLPrint.CreatedOn = dr["CreatedOn"].ToString().Trim();
                        _PLPrint.PrintedBy = dr["PrintedBy"].ToString().Trim();
                        _PLPrint.PrintedOn = dr["PrintedOn"].ToString().Trim();
                        _PLPrint.QRCodeScanBy = dr["QRCodeScanBy"].ToString().Trim();
                        _PLPrint.QRCodeScanOn = dr["QRCodeScanOn"].ToString().Trim();
                        _PLPrint.MIGONo = dr["MIGONo"].ToString().Trim();
                        _PLPrint.InspectionLotNo = dr["InspectionLotNo"].ToString().Trim();
                        _PLPrint.UpdateOn = dr["UpdateOn"].ToString().Trim();
                        _PLPrint.RejectionCode = dr["RejectionCode"].ToString().Trim();
                        _PLPrint.QCStatus = Convert.ToInt32(dr["QCStatus"].ToString().Trim());
                        _PLPrint.QCBy = dr["QCBy"].ToString().Trim();
                        _PLPrint.QCOn = dr["QCOn"].ToString().Trim();
                        _PLPrint.QCPostedStatus = dr["QCPostedStatus"].ToString().Trim();
                        _PLPrint.QCPostedBy = dr["QCPostedBy"].ToString().Trim();
                        _PLPrint.QCPostedOn = dr["QCPostedOn"].ToString().Trim();
                        _PLPrint.MTMOldQRCode = dr["MtmOldQRCode"].ToString().Trim();
                        _PLPrint.MTMTransferBy = dr["MtmTransferBy"].ToString().Trim();
                        _PLPrint.MTMTransferOn = dr["MtmTransferOn"].ToString().Trim();
                        _PLPrint.BatchNo = ""; // dr["BatchNo"].ToString().Trim();
                        _PLPrint.SentBy = clsGlobal.PlantCode.Trim() + "_DataScheduler";

                        sb.AppendLine("SELECT QRCode from tLocationLabelPrinting WITH (NOLOCK) where LocationCode = '" + _PLPrint.LocationCode + "' AND MatCode = '" + _PLPrint.MatCode + "'");
                        sb.AppendLine(" AND QRCode = '" + _PLPrint.QRCode + "' AND PONumber = '" + _PLPrint.PONumber + "' AND MatStatus = '" + _PLPrint.MatStatus + "'"); //"' AND StackQRCode = '" + _PLPrint.StackQRCode +  AND IsSAPPOsted = 0 
                        DataTable dt = oDbmCentral.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Insert into tLocationLabelPrinting(LocationCode, PONumber, MatCode, QRCode, StackQRCode, MatStatus, OldMatStatus, VendorInvoice, VendorInvoiceDate,");
                            sb.AppendLine("VendorCode, IsQRCodePrinted, IsQRCodeUsed, IsStackPrinted, IsStackUsed, OldStackQRCode, OldMatCode, Status, ReturnStatus, CreatedBy, ");
                            sb.AppendLine("CreatedOn, PrintedBy, PrintedOn, QRCodeScanBy, QRCodeScanOn, MIGONo, InspectionLotNo, UpdateOn, RejectionCode, QCStatus, QCBy, QCOn, ");
                            sb.AppendLine("QCPostedStatus, QCPostedBy, QCPostedOn, MTMOldQRCode,MTMTransferBy, MTMTransferOn, BatchNo, IsSAPPosted, SentOn, SentBy)");
                            sb.AppendLine("Values ");
                            sb.AppendLine("('" + _PLPrint.LocationCode + "','" + _PLPrint.PONumber + "','" + _PLPrint.MatCode + "',");
                            sb.AppendLine("'" + _PLPrint.QRCode + "','" + _PLPrint.StackQRCode + "','" + _PLPrint.MatStatus + "','" + _PLPrint.OldMatStatus + "','" + _PLPrint.VendorInvoice + "','" + _PLPrint.VendorInvoiceDate + "',");
                            sb.AppendLine("'" + _PLPrint.VendorCode + "','" + _PLPrint.IsQRCodePrinted + "','" + _PLPrint.IsQRCodeUsed + "','" + _PLPrint.IsStackPrinted + "',");
                            sb.AppendLine("'" + _PLPrint.IsStackUsed + "','" + _PLPrint.OldStackQRCode + "','" + _PLPrint.OldMatCode + "','" + _PLPrint.Status + "',");
                            sb.AppendLine("'" + _PLPrint.ReturnStatus + "','" + _PLPrint.CreatedBy + "','" + _PLPrint.CreatedOn + "','" + _PLPrint.PrintedBy + "',");
                            sb.AppendLine("'" + _PLPrint.PrintedOn + "','" + _PLPrint.QRCodeScanBy + "','" + _PLPrint.QRCodeScanOn + "','" + _PLPrint.MIGONo + "',");
                            sb.AppendLine("'" + _PLPrint.InspectionLotNo + "','" + _PLPrint.UpdateOn + "','" + _PLPrint.RejectionCode + "','" + _PLPrint.QCStatus + "','" + _PLPrint.QCBy + "','" + _PLPrint.QCOn + "',");
                            sb.AppendLine("'" + _PLPrint.QCPostedStatus + "','" + _PLPrint.QCPostedBy + "','" + _PLPrint.QCPostedOn + "',");
                            sb.AppendLine("'" + _PLPrint.MTMOldQRCode + "','" + _PLPrint.MTMTransferBy + "',");
                            sb.AppendLine("'" + _PLPrint.MTMTransferOn + "','" + _PLPrint.BatchNo + "', 0, GETDATE(), '" + _PLPrint.SentBy + "')");
                            oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                            dr["IsSAPPosted"] = 1;
                            //ObjLog.WriteLog("DataScheduler : LocaltoCentral : LocationLabelPrinting => LocationCode : " + _PLPrint.LocationCode + ", MatCode : " + _PLPrint.MatCode + ", QRCode : " + _PLPrint.QRCode + " posted to Central server successfully");
                        }
                        //else
                        //{
                        //    sb = new StringBuilder();
                        //    sb.AppendLine("Update tLocationLabelPrinting set StackQRCode = '" + _PLPrint.StackQRCode + "'");
                        //    sb.AppendLine(", VendorInvoice = '" + _PLPrint.VendorInvoice + "', VendorInvoiceDate = '" + _PLPrint.VendorInvoiceDate + "'");
                        //    sb.AppendLine(", VendorCode = '" + _PLPrint.VendorCode + "', IsQRCodePrinted = '" + _PLPrint.IsQRCodePrinted + "', IsQRCodeUsed = '" + _PLPrint.IsQRCodeUsed + "'");
                        //    sb.AppendLine(", IsStackPrinted = '" + _PLPrint.IsStackPrinted + "', IsStackUsed = '" + _PLPrint.IsStackUsed + "', OldStackQRCode =  '" + _PLPrint.OldStackQRCode + "'");
                        //    sb.AppendLine(", OldMatCode = '" + _PLPrint.OldMatCode + "', Status = '" + _PLPrint.Status + "', ReturnStatus = '" + _PLPrint.ReturnStatus + "'");
                        //    sb.AppendLine(", PrintedBy = '" + _PLPrint.PrintedBy + "', PrintedOn = '" + _PLPrint.PrintedOn + "', QRCodeScanBy = '" + _PLPrint.QRCodeScanBy + "'");
                        //    sb.AppendLine(", QRCodeScanOn = '" + _PLPrint.QRCodeScanOn + "', MIGONo = '" + _PLPrint.MIGONo + "', InspectionLotNo = '" + _PLPrint.InspectionLotNo + "'");
                        //    sb.AppendLine(", QCStatus = '" + _PLPrint.QCStatus + "', QCBy = '" + _PLPrint.QCBy + "', QCOn = '" + _PLPrint.QCOn + "'");
                        //    sb.AppendLine(", MTMOldQRCode = '" + _PLPrint.MTMOldQRCode + "', MTMTransferBy = '" + _PLPrint.MTMTransferBy + "', MTMTransferOn = '" + _PLPrint.MTMTransferOn + "'");
                        //    sb.AppendLine(", SentOn = GETDATE() ");
                        //    sb.AppendLine(" WHERE LocationCode = '" + _PLPrint.LocationCode + "' AND MatCode = '" + _PLPrint.MatCode + "' AND QRCode = '" + _PLPrint.QRCode + "'");
                        //    sb.AppendLine(" AND PONumber = '" + _PLPrint.PONumber + "' AND MatStatus = '" + _PLPrint.MatStatus + "'");
                        //    oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                        //    clsGlobal.iUpdateCount++;
                        //}
                    }
                    oDbmCentral.CommitTransaction();
                    dtPostLocLabelPrinting.AcceptChanges();
                    oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    DataTable odtScannedData9 = new DataTable();
                    odtScannedData9 = dtPostLocLabelPrinting.Clone();
                    DataRow[] rows1;
                    rows1 = dtPostLocLabelPrinting.Select("IsSAPPosted = True");
                    foreach (DataRow row in rows1)
                    {
                        odtScannedData9.ImportRow(row);
                    }
                    odtScannedData9.AcceptChanges();
                    foreach (DataRow dr in odtScannedData9.Rows)
                    {
                        string sLocCode = dr["LocationCode"].ToString().Trim();
                        string sPONo = dr["PONumber"].ToString().Trim();
                        string sMatcode = dr["MatCode"].ToString().Trim();
                        string sQRCode = dr["QRCode"].ToString().Trim();
                        //string sVendorCode = dr["VendorCode"].ToString().Trim();
                        string sMatStatus = dr["MatStatus"].ToString().Trim();
                        sb = new StringBuilder();
                        sb.AppendLine(" Update tLocationLabelPrinting set IsSAPPosted = 1, PostedOn = GETDATE() ");
                        sb.AppendLine(" where LocationCode = '" + sLocCode + "' AND PONumber = '" + sPONo + "'");
                        sb.AppendLine(" AND MatCode = '" + sMatcode + "' AND MatStatus = '" + sMatStatus + "' AND QRCode = '" + sQRCode + "'");
                        oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                        clsGlobal.iUpdateCount++;
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler : LocaltoCentral : LocationLabelPrinting => Total No. Inserted Records - " + clsGlobal.iAddCount + " posted to central server successfully");
                    clsGlobal.lmsg = "Data Scheduler : LocaltoCentral : LocationLabelPrinting => Total No. of Inserted Records - " + clsGlobal.iAddCount + " and Updated Records - " + clsGlobal.iUpdateCount + " posted to central server successfully at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                }
                else if (dtPostLocLabelPrinting.Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler : LocaltoCentral : LocationLabelPrinting => " + "There is no records found");
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending Production Printing Data from LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " to Central Server of MaterialCode - " + _PLPrint.MatCode + " found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                //}
                oDbmCentral.RollBackTransaction();
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler : LocaltoCentral : LocationLabelPrinting => Error is - " + ex.Message.ToString());
                ObjLog.WriteLog("Data Scheduler : LocaltoCentral : LocationLabelPrinting => Query is - " + sb.ToString());
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public void GetL2CLocationPrintingHistoryDetails()
        {
            PLLocationLabelPrinting _PLPrint = new PLLocationLabelPrinting();
            StringBuilder sb;
            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrClientCon);
                _dtBindList = new DataTable();
                if (dtPostLocPrintingHistory.Rows.Count > 0)
                {
                    clsGlobal.iAddCount = 0;
                    clsGlobal.iUpdateCount = 0;
                    //ObjLog.WriteLog("Data Scheduler : LocaltoCentral LocationLabelPrinting : Total No. of Records Found - " + ds.Tables["tLocationLabelPrinting"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dtPostLocPrintingHistory.Rows)
                    {
                        sb = new StringBuilder();
                        _PLPrint.ID = dr["ID"].ToString().Trim();
                        _PLPrint.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLPrint.PONumber = dr["PONumber"].ToString().Trim();
                        _PLPrint.MatCode = dr["MatCode"].ToString().Trim();
                        _PLPrint.QRCode = dr["QRCode"].ToString().Trim();
                        _PLPrint.StackQRCode = dr["StackQRCode"].ToString().Trim();
                        _PLPrint.MatStatus = dr["MatStatus"].ToString().Trim();
                        _PLPrint.VendorInvoice = dr["VendorInvoice"].ToString().Trim();
                        _PLPrint.VendorInvoiceDate = dr["VendorInvoiceDate"].ToString().Trim();
                        _PLPrint.VendorCode = dr["VendorCode"].ToString().Trim();
                        _PLPrint.IsQRCodePrinted = dr["IsQRCodePrinted"].ToString().Trim();
                        _PLPrint.IsQRCodeUsed = dr["IsQRCodeUsed"].ToString().Trim();
                        _PLPrint.IsStackPrinted = dr["IsStackPrinted"].ToString().Trim();
                        _PLPrint.IsStackUsed = dr["IsStackUsed"].ToString().Trim();
                        _PLPrint.OldStackQRCode = dr["OldStackQRCode"].ToString().Trim();
                        _PLPrint.OldMatCode = dr["OldMatCode"].ToString().Trim();
                        _PLPrint.Status = Convert.ToInt32(dr["Status"].ToString().Trim());
                        _PLPrint.ReturnStatus = Convert.ToInt32(dr["ReturnStatus"].ToString().Trim());
                        _PLPrint.CreatedBy = dr["CreatedBy"].ToString().Trim();
                        _PLPrint.CreatedOn = dr["CreatedOn"].ToString().Trim();
                        _PLPrint.PrintedBy = dr["PrintedBy"].ToString().Trim();
                        _PLPrint.PrintedOn = dr["PrintedOn"].ToString().Trim();
                        _PLPrint.QRCodeScanBy = dr["QRCodeScanBy"].ToString().Trim();
                        _PLPrint.QRCodeScanOn = dr["QRCodeScanOn"].ToString().Trim();
                        _PLPrint.MIGONo = dr["MIGONo"].ToString().Trim();
                        _PLPrint.InspectionLotNo = dr["InspectionLotNo"].ToString().Trim();
                        _PLPrint.UpdateOn = dr["UpdateOn"].ToString().Trim();
                        _PLPrint.RejectionCode = dr["RejectionCode"].ToString().Trim();
                        _PLPrint.QCStatus = Convert.ToInt32(dr["QCStatus"].ToString().Trim());
                        _PLPrint.QCBy = dr["QCBy"].ToString().Trim();
                        _PLPrint.QCOn = dr["QCOn"].ToString().Trim();
                        _PLPrint.QCPostedStatus = dr["QCPostedStatus"].ToString().Trim();
                        _PLPrint.QCPostedBy = dr["QCPostedBy"].ToString().Trim();
                        _PLPrint.QCPostedOn = dr["QCPostedOn"].ToString().Trim();
                        _PLPrint.MTMOldQRCode = dr["MtmOldQRCode"].ToString().Trim();
                        _PLPrint.MTMTransferBy = dr["MtmTransferBy"].ToString().Trim();
                        _PLPrint.MTMTransferOn = dr["MtmTransferOn"].ToString().Trim();
                        _PLPrint.BatchNo = ""; // dr["BatchNo"].ToString().Trim();
                        _PLPrint.SentBy = clsGlobal.PlantCode.Trim() + "_DataScheduler";

                        sb.AppendLine("SELECT QRCode from tLocationPrintingHistory WITH (NOLOCK) where LocationCode = '" + _PLPrint.LocationCode + "' AND MatCode = '" + _PLPrint.MatCode + "'");
                        sb.AppendLine(" AND QRCode = '" + _PLPrint.QRCode + "' AND PONumber = '" + _PLPrint.PONumber + "' AND VendorCode = '" + _PLPrint.VendorCode + "' AND MatStatus = '" + _PLPrint.MatStatus + "'"); //"' AND StackQRCode = '" + _PLPrint.StackQRCode +  AND IsSAPPOsted = 0 
                        DataTable dt = oDbmCentral.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Insert into tLocationPrintingHistory(ID, LocationCode, PONumber, MatCode, QRCode, StackQRCode, MatStatus, OldMatStatus, VendorInvoice, VendorInvoiceDate,");
                            sb.AppendLine("VendorCode, IsQRCodePrinted, IsQRCodeUsed, IsStackPrinted, IsStackUsed, OldStackQRCode, OldMatCode, Status, ReturnStatus, CreatedBy, ");
                            sb.AppendLine("CreatedOn, PrintedBy, PrintedOn, QRCodeScanBy, QRCodeScanOn, MIGONo, InspectionLotNo, UpdateOn, RejectionCode, QCStatus, QCBy, QCOn, ");
                            sb.AppendLine("QCPostedStatus, QCPostedBy, QCPostedOn, MTMOldQRCode,MTMTransferBy, MTMTransferOn, BatchNo, IsSAPPosted, SentOn, SentBy)");
                            sb.AppendLine("Values ");
                            sb.AppendLine("('" + _PLPrint.ID + "','" + _PLPrint.LocationCode + "','" + _PLPrint.PONumber + "','" + _PLPrint.MatCode + "',");
                            sb.AppendLine("'" + _PLPrint.QRCode + "','" + _PLPrint.StackQRCode + "','" + _PLPrint.MatStatus + "','" + _PLPrint.OldMatStatus + "','" + _PLPrint.VendorInvoice + "','" + _PLPrint.VendorInvoiceDate + "',");
                            sb.AppendLine("'" + _PLPrint.VendorCode + "','" + _PLPrint.IsQRCodePrinted + "','" + _PLPrint.IsQRCodeUsed + "','" + _PLPrint.IsStackPrinted + "',");
                            sb.AppendLine("'" + _PLPrint.IsStackUsed + "','" + _PLPrint.OldStackQRCode + "','" + _PLPrint.OldMatCode + "','" + _PLPrint.Status + "',");
                            sb.AppendLine("'" + _PLPrint.ReturnStatus + "','" + _PLPrint.CreatedBy + "','" + _PLPrint.CreatedOn + "','" + _PLPrint.PrintedBy + "',");
                            sb.AppendLine("'" + _PLPrint.PrintedOn + "','" + _PLPrint.QRCodeScanBy + "','" + _PLPrint.QRCodeScanOn + "','" + _PLPrint.MIGONo + "',");
                            sb.AppendLine("'" + _PLPrint.InspectionLotNo + "','" + _PLPrint.UpdateOn + "','" + _PLPrint.RejectionCode + "','" + _PLPrint.QCStatus + "','" + _PLPrint.QCBy + "','" + _PLPrint.QCOn + "',");
                            sb.AppendLine("'" + _PLPrint.QCPostedStatus + "','" + _PLPrint.QCPostedBy + "','" + _PLPrint.QCPostedOn + "',");
                            sb.AppendLine("'" + _PLPrint.MTMOldQRCode + "','" + _PLPrint.MTMTransferBy + "',");
                            sb.AppendLine("'" + _PLPrint.MTMTransferOn + "','" + _PLPrint.BatchNo + "', 0, GETDATE(), '" + _PLPrint.SentBy + "')");
                            oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                            dr["IsSAPPosted"] = 1;
                            //ObjLog.WriteLog("DataScheduler : LocaltoCentral : LocationLabelPrinting => LocationCode : " + _PLPrint.LocationCode + ", MatCode : " + _PLPrint.MatCode + ", QRCode : " + _PLPrint.QRCode + " posted to Central server successfully");
                        }
                        //else
                        //{
                        //    sb = new StringBuilder();
                        //    sb.AppendLine("Update tLocationLabelPrinting set StackQRCode = '" + _PLPrint.StackQRCode + "'");
                        //    sb.AppendLine(", VendorInvoice = '" + _PLPrint.VendorInvoice + "', VendorInvoiceDate = '" + _PLPrint.VendorInvoiceDate + "'");
                        //    sb.AppendLine(", VendorCode = '" + _PLPrint.VendorCode + "', IsQRCodePrinted = '" + _PLPrint.IsQRCodePrinted + "', IsQRCodeUsed = '" + _PLPrint.IsQRCodeUsed + "'");
                        //    sb.AppendLine(", IsStackPrinted = '" + _PLPrint.IsStackPrinted + "', IsStackUsed = '" + _PLPrint.IsStackUsed + "', OldStackQRCode =  '" + _PLPrint.OldStackQRCode + "'");
                        //    sb.AppendLine(", OldMatCode = '" + _PLPrint.OldMatCode + "', Status = '" + _PLPrint.Status + "', ReturnStatus = '" + _PLPrint.ReturnStatus + "'");
                        //    sb.AppendLine(", PrintedBy = '" + _PLPrint.PrintedBy + "', PrintedOn = '" + _PLPrint.PrintedOn + "', QRCodeScanBy = '" + _PLPrint.QRCodeScanBy + "'");
                        //    sb.AppendLine(", QRCodeScanOn = '" + _PLPrint.QRCodeScanOn + "', MIGONo = '" + _PLPrint.MIGONo + "', InspectionLotNo = '" + _PLPrint.InspectionLotNo + "'");
                        //    sb.AppendLine(", QCStatus = '" + _PLPrint.QCStatus + "', QCBy = '" + _PLPrint.QCBy + "', QCOn = '" + _PLPrint.QCOn + "'");
                        //    sb.AppendLine(", MTMOldQRCode = '" + _PLPrint.MTMOldQRCode + "', MTMTransferBy = '" + _PLPrint.MTMTransferBy + "', MTMTransferOn = '" + _PLPrint.MTMTransferOn + "'");
                        //    sb.AppendLine(", SentOn = GETDATE() ");
                        //    sb.AppendLine(" WHERE LocationCode = '" + _PLPrint.LocationCode + "' AND MatCode = '" + _PLPrint.MatCode + "' AND QRCode = '" + _PLPrint.QRCode + "'");
                        //    sb.AppendLine(" AND PONumber = '" + _PLPrint.PONumber + "' AND MatStatus = '" + _PLPrint.MatStatus + "'");
                        //    oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                        //    clsGlobal.iUpdateCount++;
                        //}
                    }
                    oDbmCentral.CommitTransaction();
                    dtPostLocPrintingHistory.AcceptChanges();
                    oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    DataTable odtScannedData8 = new DataTable();
                    odtScannedData8 = dtPostLocPrintingHistory.Clone();
                    DataRow[] rows1;
                    rows1 = dtPostLocPrintingHistory.Select("IsSAPPosted = True");
                    foreach (DataRow row in rows1)
                    {
                        odtScannedData8.ImportRow(row);
                    }
                    odtScannedData8.AcceptChanges();
                    foreach (DataRow dr in odtScannedData8.Rows)
                    {
                        string sLocCode = dr["LocationCode"].ToString().Trim();
                        string sPONo = dr["PONumber"].ToString().Trim();
                        string sMatcode = dr["MatCode"].ToString().Trim();
                        string sQRCode = dr["QRCode"].ToString().Trim();
                        string sVendorCode = dr["VendorCode"].ToString().Trim();
                        string sMatStatus = dr["MatStatus"].ToString().Trim();
                        sb = new StringBuilder();
                        sb.AppendLine(" Update tLocationPrintingHistory set IsSAPPosted = 1, PostedOn = GETDATE() ");
                        sb.AppendLine(" where LocationCode = '" + sLocCode + "' AND PONumber = '" + sPONo + "'");
                        sb.AppendLine(" AND MatCode = '" + sMatcode + "' AND VendorCode = '" + sVendorCode + "' AND MatStatus = '" + sMatStatus + "' AND QRCode = '" + sQRCode + "'");
                        oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                        clsGlobal.iUpdateCount++;
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler : LocaltoCentral : LocationLabelPrinting => Total No. Inserted Records - " + clsGlobal.iAddCount + " And updated - " + clsGlobal.iUpdateCount + " records posted to central server successfully");
                    clsGlobal.lmsg = "Data Scheduler : LocaltoCentral : LocationLabelPrinting => Total No. of Inserted Records - " + clsGlobal.iAddCount + " and Updated Records - " + clsGlobal.iUpdateCount + " posted to central server successfully at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                }
                else if (dtPostLocPrintingHistory.Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler : LocaltoCentral : LocationLabelPrinting => " + "There is no records found");
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending Production Printing History Data from LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " to Central Server of MaterialCode - " + _PLPrint.MatCode + " found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                //}
                oDbmCentral.RollBackTransaction();
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler : LocaltoCentral : LocationLabelPrinting => Error is - " + ex.Message.ToString());
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public void GetL2CLocationPrintingQADetails()
        {
            PLLocationLabelPrinting _PLPrint = new PLLocationLabelPrinting();
            StringBuilder sb;
            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrClientCon);
                _dtBindList = new DataTable();
                if (dtPostQAData.Rows.Count > 0)
                {
                    clsGlobal.iAddCount = 0;
                    clsGlobal.iUpdateCount = 0;
                    //ObjLog.WriteLog("Data Scheduler : LocaltoCentral LocationLabelPrinting : Total No. of Records Found - " + ds.Tables["tLocationLabelPrinting"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dtPostQAData.Rows)
                    {
                        sb = new StringBuilder();
                        //_PLPrint.ID = dr["ID"].ToString().Trim();
                        _PLPrint.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLPrint.PONumber = dr["PONumber"].ToString().Trim();
                        _PLPrint.MatCode = dr["MatCode"].ToString().Trim();
                        _PLPrint.QRCode = dr["QRCode"].ToString().Trim();
                        //_PLPrint.StackQRCode = dr["StackQRCode"].ToString().Trim();
                        //_PLPrint.MatStatus = dr["MatStatus"].ToString().Trim();
                        //_PLPrint.VendorInvoice = dr["VendorInvoice"].ToString().Trim();
                        //_PLPrint.VendorInvoiceDate = dr["VendorInvoiceDate"].ToString().Trim();
                        //_PLPrint.VendorCode = dr["VendorCode"].ToString().Trim();
                        //_PLPrint.IsQRCodePrinted = dr["IsQRCodePrinted"].ToString().Trim();
                        //_PLPrint.IsQRCodeUsed = dr["IsQRCodeUsed"].ToString().Trim();
                        //_PLPrint.IsStackPrinted = dr["IsStackPrinted"].ToString().Trim();
                        //_PLPrint.IsStackUsed = dr["IsStackUsed"].ToString().Trim();
                        //_PLPrint.OldStackQRCode = dr["OldStackQRCode"].ToString().Trim();
                        //_PLPrint.OldMatCode = dr["OldMatCode"].ToString().Trim();
                        //_PLPrint.Status = Convert.ToInt32(dr["Status"].ToString().Trim());
                        //_PLPrint.ReturnStatus = Convert.ToInt32(dr["ReturnStatus"].ToString().Trim());
                        //_PLPrint.CreatedBy = dr["CreatedBy"].ToString().Trim();
                        //_PLPrint.CreatedOn = dr["CreatedOn"].ToString().Trim();
                        //_PLPrint.PrintedBy = dr["PrintedBy"].ToString().Trim();
                        //_PLPrint.PrintedOn = dr["PrintedOn"].ToString().Trim();
                        //_PLPrint.QRCodeScanBy = dr["QRCodeScanBy"].ToString().Trim();
                        //_PLPrint.QRCodeScanOn = dr["QRCodeScanOn"].ToString().Trim();
                        _PLPrint.MIGONo = dr["MIGONo"].ToString().Trim();
                        _PLPrint.InspectionLotNo = dr["InspectionLotNo"].ToString().Trim();
                        _PLPrint.RejectionCode = dr["RejectionCode"].ToString().Trim();
                        _PLPrint.QCStatus = Convert.ToInt32(dr["QCStatus"].ToString().Trim());
                        _PLPrint.QCBy = dr["QCBy"].ToString().Trim();
                        _PLPrint.QCOn = dr["QCOn"].ToString().Trim();
                        //_PLPrint.MTMOldQRCode = dr["MtmOldQRCode"].ToString().Trim();
                        //_PLPrint.MTMTransferBy = dr["MtmTransferBy"].ToString().Trim();
                        //_PLPrint.MTMTransferOn = dr["MtmTransferOn"].ToString().Trim();
                        //_PLPrint.BatchNo = ""; // dr["BatchNo"].ToString().Trim();
                        _PLPrint.SentBy = clsGlobal.PlantCode.Trim() + "_DataScheduler";

                        sb.AppendLine("SELECT QRCode from tLocationPrintingHistory WITH (NOLOCK) where LocationCode = '" + _PLPrint.LocationCode + "' AND MatCode = '" + _PLPrint.MatCode + "'");
                        sb.AppendLine(" AND QRCode = '" + _PLPrint.QRCode + "' AND PONumber = '" + _PLPrint.PONumber + "' AND MIGONo = '" + _PLPrint.MIGONo + "' AND InspectionLotNo = '" + _PLPrint.InspectionLotNo + "'"); //"' AND StackQRCode = '" + _PLPrint.StackQRCode +  AND IsSAPPOsted = 0 
                        DataTable dt = oDbmCentral.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        //if (dt.Rows.Count == 0)
                        //{
                        //    sb = new StringBuilder();
                        //    sb.AppendLine("Insert into tLocationPrintingHistory(ID, LocationCode, PONumber, MatCode, QRCode, StackQRCode, MatStatus, VendorInvoice, VendorInvoiceDate,");
                        //    sb.AppendLine("VendorCode, IsQRCodePrinted, IsQRCodeUsed, IsStackPrinted, IsStackUsed, OldStackQRCode, OldMatCode, Status, ReturnStatus, CreatedBy, ");
                        //    sb.AppendLine("CreatedOn, PrintedBy, PrintedOn, QRCodeScanBy, QRCodeScanOn, MIGONo, InspectionLotNo, QCStatus, QCBy, QCOn, MTMOldQRCode,");
                        //    sb.AppendLine("MTMTransferBy, MTMTransferOn, BatchNo, IsSAPPosted, SentOn, SentBy)");
                        //    sb.AppendLine("Values ");
                        //    sb.AppendLine("('" + _PLPrint.ID + "','" + _PLPrint.LocationCode + "','" + _PLPrint.PONumber + "','" + _PLPrint.MatCode + "',");
                        //    sb.AppendLine("'" + _PLPrint.QRCode + "','" + _PLPrint.StackQRCode + "','" + _PLPrint.MatStatus + "','" + _PLPrint.VendorInvoice + "','" + _PLPrint.VendorInvoiceDate + "',");
                        //    sb.AppendLine("'" + _PLPrint.VendorCode + "','" + _PLPrint.IsQRCodePrinted + "','" + _PLPrint.IsQRCodeUsed + "','" + _PLPrint.IsStackPrinted + "',");
                        //    sb.AppendLine("'" + _PLPrint.IsStackUsed + "','" + _PLPrint.OldStackQRCode + "','" + _PLPrint.OldMatCode + "','" + _PLPrint.Status + "',");
                        //    sb.AppendLine("'" + _PLPrint.ReturnStatus + "','" + _PLPrint.CreatedBy + "','" + _PLPrint.CreatedOn + "','" + _PLPrint.PrintedBy + "',");
                        //    sb.AppendLine("'" + _PLPrint.PrintedOn + "','" + _PLPrint.QRCodeScanBy + "','" + _PLPrint.QRCodeScanOn + "','" + _PLPrint.MIGONo + "',");
                        //    sb.AppendLine("'" + _PLPrint.InspectionLotNo + "','" + _PLPrint.QCStatus + "','" + _PLPrint.QCBy + "','" + _PLPrint.QCOn + "',");
                        //    sb.AppendLine("'" + _PLPrint.MTMOldQRCode + "','" + _PLPrint.MTMTransferBy + "',");
                        //    sb.AppendLine("'" + _PLPrint.MTMTransferOn + "','" + _PLPrint.BatchNo + "', 0, GETDATE(), '" + _PLPrint.SentBy + "')");
                        //    oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                        //    clsGlobal.iAddCount++;
                        //    dr["IsSAPPosted"] = 1;
                        //    //ObjLog.WriteLog("DataScheduler : LocaltoCentral : LocationLabelPrinting => LocationCode : " + _PLPrint.LocationCode + ", MatCode : " + _PLPrint.MatCode + ", QRCode : " + _PLPrint.QRCode + " posted to Central server successfully");
                        //}
                        if (dt.Rows.Count == 1)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Update tLocationPrintingHistory set RejectionCode = '" + _PLPrint.RejectionCode + "'");
                            //sb.AppendLine(", QCStatus = '" + _PLPrint.QCStatus + "', QCBy = '" + _PLPrint.QCBy + "'");
                            //sb.AppendLine(", VendorCode = '" + _PLPrint.VendorCode + "', IsQRCodePrinted = '" + _PLPrint.IsQRCodePrinted + "', IsQRCodeUsed = '" + _PLPrint.IsQRCodeUsed + "'");
                            //sb.AppendLine(", IsStackPrinted = '" + _PLPrint.IsStackPrinted + "', IsStackUsed = '" + _PLPrint.IsStackUsed + "', OldStackQRCode =  '" + _PLPrint.OldStackQRCode + "'");
                            //sb.AppendLine(", OldMatCode = '" + _PLPrint.OldMatCode + "', Status = '" + _PLPrint.Status + "', ReturnStatus = '" + _PLPrint.ReturnStatus + "'");
                            //sb.AppendLine(", PrintedBy = '" + _PLPrint.PrintedBy + "', PrintedOn = '" + _PLPrint.PrintedOn + "', QRCodeScanBy = '" + _PLPrint.QRCodeScanBy + "'");
                            //sb.AppendLine(", QRCodeScanOn = '" + _PLPrint.QRCodeScanOn + "', MIGONo = '" + _PLPrint.MIGONo + "', InspectionLotNo = '" + _PLPrint.InspectionLotNo + "'");
                            sb.AppendLine(", QCStatus = '" + _PLPrint.QCStatus + "', QCBy = '" + _PLPrint.QCBy + "', QCOn = '" + _PLPrint.QCOn + "'");
                            //sb.AppendLine(", QCOn = '" + _PLPrint.QCOn + "'");
                            //sb.AppendLine(", SentOn = GETDATE() ");
                            sb.AppendLine(" WHERE LocationCode = '" + _PLPrint.LocationCode + "' AND MatCode = '" + _PLPrint.MatCode + "' AND QRCode = '" + _PLPrint.QRCode + "'");
                            sb.AppendLine(" AND PONumber = '" + _PLPrint.PONumber + "' AND InspectionLotNo = '" + _PLPrint.InspectionLotNo + "' AND MIGONo = '" + _PLPrint.MIGONo + "'");
                            oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iUpdateCount++;
                            dr["QCPostedStatus"] = 1;
                        }
                    }
                    oDbmCentral.CommitTransaction();
                    dtPostQAData.AcceptChanges();
                    oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    DataTable odtScannedData8 = new DataTable();
                    odtScannedData8 = dtPostQAData.Clone();
                    DataRow[] rows1;
                    rows1 = dtPostQAData.Select("QCPostedStatus = True");
                    foreach (DataRow row in rows1)
                    {
                        odtScannedData8.ImportRow(row);
                    }
                    odtScannedData8.AcceptChanges();
                    foreach (DataRow dr in odtScannedData8.Rows)
                    {
                        string sLocCode = dr["LocationCode"].ToString().Trim();
                        string sPONo = dr["PONumber"].ToString().Trim();
                        string sMatcode = dr["MatCode"].ToString().Trim();
                        string sQRCode = dr["QRCode"].ToString().Trim();
                        string sInspectionLotNo = dr["InspectionLotNo"].ToString().Trim();
                        string sMIGONo = dr["MIGONo"].ToString().Trim();
                        sb = new StringBuilder();
                        sb.AppendLine(" Update tLocationLabelPrinting set QCPostedStatus = 1, QCPostedOn = GETDATE() ");
                        sb.AppendLine(" where LocationCode = '" + sLocCode + "' AND PONumber = '" + sPONo + "'");
                        sb.AppendLine(" AND MatCode = '" + sMatcode + "' AND MIGONo = '" + sMIGONo + "' AND InspectionLotNo = '" + sInspectionLotNo + "' AND QRCode = '" + sQRCode + "'");
                        oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                        clsGlobal.iUpdateCount++;
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler : LocaltoCentral : LocationLabelPrinting => Total No. Inserted Records - " + clsGlobal.iAddCount + " And updated - " + clsGlobal.iUpdateCount + " records posted to central server successfully");
                    clsGlobal.lmsg = "Data Scheduler : LocaltoCentral : LocationLabelPrinting => Total No. of Inserted Records - " + clsGlobal.iAddCount + " and Updated Records - " + clsGlobal.iUpdateCount + " posted to central server successfully at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                }
                else if (dtPostQAData.Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler : LocaltoCentral : LocationLabelPrinting => " + "There is no records found");
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending Production Printing History Data from LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " to Central Server of MaterialCode - " + _PLPrint.MatCode + " found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                //}
                oDbmCentral.RollBackTransaction();
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler : LocaltoCentral : LocationLabelPrinting => Error is - " + ex.Message.ToString());
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public void GetL2CVendorLabelGeneratingDetails()
        {
            clsLogic oLogic = new clsLogic();
            PLtVendorLabelPrinting _PLPrint = new PLtVendorLabelPrinting();
            StringBuilder sb;
            clsGlobal.iUpdateCount = 0;
            clsGlobal.iAddCount = 0;
            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrClientCon);
                _dtBindList = new DataTable();
                if (dtPostVendorLabelGenerating.Rows.Count > 0)
                {
                    //ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral VendorLabelGenerating Data : No. of Records Found - " + ds.Tables["tVendorLabelGenerating"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dtPostVendorLabelGenerating.Rows)
                    {
                        sb = new StringBuilder();
                        _PLPrint.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLPrint.PONumber = dr["PONumber"].ToString().Trim();
                        _PLPrint.MatCode = dr["MatCode"].ToString().Trim();
                        _PLPrint.QRCode = dr["QRCode"].ToString().Trim();
                        _PLPrint.VendorCode = dr["VendorCode"].ToString().Trim();
                        _PLPrint.GeneratedBy = dr["GeneratedBy"].ToString().Trim();
                        _PLPrint.GeneratedOn = dr["GeneratedOn"].ToString().Trim().Substring(0, 10);
                        _PLPrint.IsQRCodePrinted = dr["IsQRCodePrinted"].ToString().Trim();
                        _PLPrint.PrintedBy = dr["PrintedBy"].ToString().Trim();
                        _PLPrint.PrintedOn = dr["PrintedOn"].ToString().Trim();
                        _PLPrint.IsRePrinted = dr["IsRePrinted"].ToString().Trim();
                        _PLPrint.RePrintedBy = dr["RePrintedBy"].ToString().Trim();
                        _PLPrint.RePrintedOn = dr["RePrintedOn"].ToString().Trim();
                        _PLPrint.SentBy = clsGlobal.PlantCode.Trim() + "_DataScheduler";

                        sb.AppendLine("SELECT MatCode from tVendorLabelGenerating where LocationCode = '" + _PLPrint.LocationCode + "' AND PONumber = '" + _PLPrint.PONumber + "'");
                        sb.AppendLine("AND MatCode = '" + _PLPrint.MatCode + "' AND VendorCode = '" + _PLPrint.VendorCode + "' AND QRCode = '" + _PLPrint.QRCode + "'");
                        DataTable dt = oDbmCentral.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Insert into tVendorLabelGenerating(LocationCode, PONumber, MatCode, QRCode,");
                            sb.AppendLine("VendorCode, GeneratedBy, GeneratedOn, IsQRCodePrinted,");
                            sb.AppendLine(" PrintedBy, PrintedOn, IsRePrinted, RePrintedBy, RePrintedOn, IsSAPPosted, SentOn, SentBy)");
                            //sb.AppendLine("SAPPostMsg, PostedBy, PostedOn )");
                            sb.AppendLine("Values ");
                            sb.AppendLine("('" + _PLPrint.LocationCode + "','" + _PLPrint.PONumber + "','" + _PLPrint.MatCode + "',");
                            //sb.AppendLine(" '" + _PLPrint.MatGrade + "','" + _PLPrint.MatGroup + "','" + _PLPrint.MatThickness + "','" + _PLPrint.MatSize + "',");
                            sb.AppendLine(" '" + _PLPrint.QRCode + "','" + _PLPrint.VendorCode + "',");
                            sb.AppendLine(" '" + _PLPrint.GeneratedBy + "','" + _PLPrint.GeneratedOn + "',");
                            sb.AppendLine(" '" + _PLPrint.IsQRCodePrinted + "','" + _PLPrint.PrintedBy + "','" + _PLPrint.PrintedOn + "',");
                            sb.AppendLine(" '" + _PLPrint.IsRePrinted + "',");
                            sb.AppendLine(" '" + _PLPrint.RePrintedBy + "','" + _PLPrint.RePrintedOn + "', 0, GETDATE(), '" + _PLPrint.SentBy + "')");
                            oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                            dr["IsSAPPosted"] = 1;
                            //ObjLog.WriteLog("DataScheduler : LocaltoCentral : VendorLabelGenerating => LocationCode : " + _PLPrint.LocationCode + ", PONumber : " + _PLPrint.PONumber + ", MatCode : " + _PLPrint.MatCode + ", QRCode : " + _PLPrint.QRCode + " posted to Central server successfully");
                        }
                        //else
                        //{
                        //    sb = new StringBuilder();
                        //    sb.AppendLine("Update tVendorLabelPrinting set MatDesc = '" + _PLPrint.MatDesc + "', MatGrade= '" + _PLPrint.MatGrade + "'");
                        //    sb.AppendLine(", MatGroup = '" + _PLPrint.MatGroup + "', MatThickness= '" + _PLPrint.MatThickness + "', MatSize = '" + _PLPrint.MatSize + "'");
                        //    sb.AppendLine(", VendorInvoice = '" + _PLPrint.VendorInvoice + "', VendorInvDate = '" + _PLPrint.VendorInvDate + "', QRCode = '" + _PLPrint.QRCode + "'");
                        //    sb.AppendLine(", StackQRCode = '" + _PLPrint.StackQRCode + "', IsStackGenerated = '" + _PLPrint.IsStackGenerated + "', GeneratedBy = '" + _PLPrint.GeneratedBy + "'");
                        //    sb.AppendLine(", GeneratedOn = '" + _PLPrint.GeneratedOn + "', IsQRCodePrinted = '" + _PLPrint.IsQRCodePrinted + "', IsStackPrinted = '" + _PLPrint.IsStackPrinted + "'");
                        //    sb.AppendLine(", PrintedBy = '" + _PLPrint.PrintedBy + "', PrintedOn = '" + _PLPrint.PrintedOn + "', IsRePrintRequest = '" + _PLPrint.IsRePrintRequest + "'");
                        //    sb.AppendLine(", RequestedBy = '" + _PLPrint.RequestedBy + "', RequestedOn = '" + _PLPrint.RequestedOn + "', IsRePrinted = '" + _PLPrint.IsRePrinted + "'");
                        //    sb.AppendLine(", RePrintedBy = '" + _PLPrint.RePrintedBy + "', IsSAPPosted = '" + _PLPrint.IsSAPPosted + "', SAPPostMsg = '" + _PLPrint.SAPPostMsg + "'");
                        //    sb.AppendLine(", RePrintedBy = '" + _PLPrint.PostedBy + "', SAPPostMsg = '" + _PLPrint.PostedOn + "'");
                        //    sb.AppendLine(" WHERE LocationCode = '" + _PLPrint.LocationCode + "' AND PONumber =  '" + _PLPrint.PONumber + "' AND MatCode =  '" + _PLPrint.MatCode + "' AND VendorCode =  '" + _PLPrint.VendorCode + "'");
                        //    oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                        //    clsGlobal.iUpdateCount++;
                        //}
                    }
                    oDbmCentral.CommitTransaction();
                    dtPostVendorLabelGenerating.AcceptChanges();
                    oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    DataTable odtScannedData7 = new DataTable();
                    odtScannedData7 = dtPostVendorLabelGenerating.Clone();
                    DataRow[] rows1;
                    rows1 = dtPostVendorLabelGenerating.Select("IsSAPPosted = True");
                    foreach (DataRow row in rows1)
                    {
                        odtScannedData7.ImportRow(row);
                    }
                    odtScannedData7.AcceptChanges();
                    foreach (DataRow dr in odtScannedData7.Rows)
                    {
                        string sLocCode = dr["LocationCode"].ToString().Trim();
                        string sPONo = dr["PONumber"].ToString().Trim();
                        string sMatcode = dr["MatCode"].ToString().Trim();
                        string sQRCode = dr["QRCode"].ToString().Trim();
                        string sVendorCode = dr["VendorCode"].ToString().Trim();
                        sb = new StringBuilder();
                        sb.AppendLine(" Update tVendorLabelGenerating set IsSAPPosted = 1, PostedOn = GETDATE() ");
                        sb.AppendLine(" where LocationCode = '" + sLocCode + "' AND PONumber = '" + sPONo + "'");
                        sb.AppendLine(" AND MatCode = '" + sMatcode + "' AND VendorCode = '" + sVendorCode + "' AND QRCode = '" + sQRCode + "'");
                        oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                        clsGlobal.iUpdateCount++;
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler : LocaltoCentral : VendorLabelGenerating => Total No. of Records - " + clsGlobal.iAddCount + " posted to central server successfully");
                    //ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral VendorLabelGenerating Data : No. of Records Updated - " + clsGlobal.iUpdateCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                }
                else if (dtPostVendorLabelGenerating.Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler : LocaltoCentral : VendorLabelGenerating => " + "There is no records found");
                    clsGlobal.lmsg = "Data Scheduler : LocaltoCentral : VendorLabelGenerating => " + "There is no records found";
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending Vendor Label Generation from LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " to Central Server of MaterialCode - " + _PLPrint.MatCode + " found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                //}
                oDbmCentral.RollBackTransaction();
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler : LocaltoCentral : VendorLabelGenerating => Error is - " + ex.Message.ToString());
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public void GetL2CtDispatchData()
        {
            clsLogic oLogic = new clsLogic();
            PLtDispatchData _PLSR = new PLtDispatchData();
            StringBuilder sb;

            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrClientCon);
                _dtBindList = new DataTable();
                if (dtPostDispatch.Rows.Count > 0)
                {
                    clsGlobal.iAddCount = 0;
                    clsGlobal.iUpdateCount = 0;
                    //ObjLog.WriteLog("Data Scheduler : LocaltoCentral Dispatch Data : Total No. of Records Found - " + ds.Tables["tDispatchData"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dtPostDispatch.Rows)
                    {
                        sb = new StringBuilder();
                        _PLSR.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLSR.DONo = dr["DONo"].ToString().Trim();
                        _PLSR.MatCode = dr["MatCode"].ToString().Trim();
                        _PLSR.Qty = Convert.ToInt32(dr["Qty"].ToString().Trim());
                        _PLSR.QRCode = dr["QRCode"].ToString().Trim();
                        _PLSR.StackQRCode = dr["StackQRCode"].ToString().Trim();
                        _PLSR.Status = dr["Status"].ToString().Trim();
                        _PLSR.CreatedBy = dr["CreatedBy"].ToString().Trim();
                        _PLSR.CreatedOn = dr["CreatedOn"].ToString().Trim();
                        _PLSR.UpdatedBy = dr["UpdatedBy"].ToString().Trim();
                        _PLSR.UpdatedOn = dr["UpdatedOn"].ToString().Trim();
                        _PLSR.SentBy = clsGlobal.PlantCode.Trim() + "_DataScheduler";

                        sb.AppendLine("select * from tDispatchData WITH (NOLOCK) where LocationCode = '" + _PLSR.LocationCode + "'");
                        sb.AppendLine(" And DONo = '" + _PLSR.DONo + "' AND MatCode = '" + _PLSR.MatCode + "' AND QRCode = '" + _PLSR.QRCode + "'");

                        DataTable dt = oDbmCentral.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Insert into tDispatchData(LocationCode, DONo, MatCode, Qty, QRCode, StackQRCode, Status,");
                            sb.AppendLine("CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, SentBy, SentOn, IsSAPPosted)");
                            sb.AppendLine("Values ");
                            sb.AppendLine("('" + _PLSR.LocationCode + "','" + _PLSR.DONo + "','" + _PLSR.MatCode + "','" + _PLSR.Qty + "'");
                            sb.AppendLine(", '" + _PLSR.QRCode + "','" + _PLSR.StackQRCode + "','" + _PLSR.Status + "','" + _PLSR.CreatedBy + "'");
                            sb.AppendLine(", '" + _PLSR.CreatedOn + "','" + _PLSR.UpdatedBy + "','" + _PLSR.UpdatedOn + "','" + _PLSR.SentBy + "', GETDATE(), 0 )");
                            oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                            dr["IsSAPPosted"] = 1;
                            //ObjLog.WriteLog("DataScheduler : LocaltoCentral : DispatchData => LocationCode : " + _PLSR.LocationCode + ", DONo : " + _PLSR.DONo + ", MatCode : " + _PLSR.MatCode + ", QRCode : " + _PLSR.QRCode + " posted to Central server successfully");
                        }
                    }
                    oDbmCentral.CommitTransaction();
                    dtPostDispatch.AcceptChanges();
                    oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    DataTable odtScannedData9 = new DataTable();
                    odtScannedData9 = dtPostDispatch.Clone();
                    DataRow[] rows1;
                    rows1 = dtPostDispatch.Select("IsSAPPosted = True");
                    foreach (DataRow row in rows1)
                    {
                        odtScannedData9.ImportRow(row);
                    }
                    odtScannedData9.AcceptChanges();
                    foreach (DataRow dr in odtScannedData9.Rows)
                    {
                        string sLocCode = dr["LocationCode"].ToString().Trim();
                        string sDONo = dr["DONo"].ToString().Trim();
                        string sMatcode = dr["MatCode"].ToString().Trim();
                        string sQRCode = dr["QRCode"].ToString().Trim();
                        sb = new StringBuilder();
                        sb.AppendLine(" Update tDispatchData set IsSAPPosted = 1, PostedOn = GETDATE() ");
                        sb.AppendLine(" where LocationCode = '" + sLocCode + "'");
                        sb.AppendLine(" And DONo = '" + sDONo + "' AND MatCode = '" + sMatcode + "' AND QRCode = '" + sQRCode + "'");
                        oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                        clsGlobal.iUpdateCount++;
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler : LocaltoCentral : DispatchData => Total No. of Records - " + clsGlobal.iAddCount + " posted to central server successfully");
                }
                else if (dtPostDispatch.Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler : LocaltoCentral : DispatchData => " + "There is no records found");
                    clsGlobal.lmsg = "Data Scheduler : LocaltoCentral : DispatchData => " + "There is no records found";
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending DispatchData from LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " to Central Server of MaterialCode - " + _PLSR.MatCode + " found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                //}
                oDbmCentral.RollBackTransaction();
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler : LocaltoCentral : DispatchData => Error is - " + ex.Message.ToString());
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public void GetL2CVendorLabelPrintingDetails()
        {
            clsLogic oLogic = new clsLogic();
            PLtVendorLabelPrinting _PLPrint = new PLtVendorLabelPrinting();
            StringBuilder sb;

            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrClientCon);
                _dtBindList = new DataTable();
                if (dtPostVendorLabelPrinting.Rows.Count > 0)
                {
                    //ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral Vendor Data : No. of Records Found - " + ds.Tables["tVendorLabelPrinting"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dtPostVendorLabelPrinting.Rows)
                    {
                        sb = new StringBuilder();
                        _PLPrint.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLPrint.PONumber = dr["PONumber"].ToString().Trim();
                        _PLPrint.MatCode = dr["MatCode"].ToString().Trim();
                        _PLPrint.QRCode = dr["QRCode"].ToString().Trim();
                        _PLPrint.VendorCode = dr["VendorCode"].ToString().Trim();
                        _PLPrint.VendorInvoice = dr["VendorInvoice"].ToString().Trim();
                        _PLPrint.VendorInvDate = dr["VendorInvDate"].ToString().Trim();
                        _PLPrint.IsQRCodePrinted = dr["IsQRCodePrinted"].ToString().Trim();
                        _PLPrint.Status = dr["Status"].ToString().Trim();
                        _PLPrint.PrintedBy = dr["PrintedBy"].ToString().Trim();
                        _PLPrint.PrintedOn = dr["PrintedOn"].ToString().Trim();
                        _PLPrint.IsRePrinted = dr["IsRePrinted"].ToString().Trim();
                        _PLPrint.RePrintedBy = dr["RePrintedBy"].ToString().Trim();
                        _PLPrint.RePrintedOn = dr["RePrintedOn"].ToString().Trim();
                        _PLPrint.SentBy = clsGlobal.PlantCode.Trim() + "_DataScheduler";

                        sb.AppendLine("SELECT MatCode from tVendorLabelPrinting WITH (NOLOCK) where LocationCode = '" + _PLPrint.LocationCode + "' AND PONumber = '" + _PLPrint.PONumber + "' AND MatCode = '" + _PLPrint.MatCode + "'");
                        sb.AppendLine("AND VendorCode = '" + _PLPrint.VendorCode + "' AND QRCode = '" + _PLPrint.QRCode + "'");
                        DataTable dt = oDbmCentral.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Insert into tVendorLabelPrinting(LocationCode, PONumber, MatCode, QRCode,");
                            sb.AppendLine(" VendorCode, VendorInvoice, VendorInvDate, IsQRCodePrinted,");
                            sb.AppendLine(" Status, PrintedBy, PrintedOn, IsRePrinted, RePrintedBy, IsSAPPosted, SentOn, SentBy)");
                            sb.AppendLine(" Values ");
                            sb.AppendLine("('" + _PLPrint.LocationCode + "','" + _PLPrint.PONumber + "','" + _PLPrint.MatCode + "','" + _PLPrint.QRCode + "',");
                            sb.AppendLine(" '" + _PLPrint.VendorCode + "','" + _PLPrint.VendorInvoice + "','" + _PLPrint.VendorInvDate + "',");
                            sb.AppendLine(" '" + _PLPrint.IsQRCodePrinted + "','" + _PLPrint.Status + "','" + _PLPrint.PrintedBy + "','" + _PLPrint.PrintedOn + "',");
                            sb.AppendLine(" '" + _PLPrint.IsRePrinted + "',");
                            sb.AppendLine(" '" + _PLPrint.RePrintedBy + "', 0, GETDATE(), '" + _PLPrint.SentBy + "')");
                            oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                            dr["IsSAPPosted"] = 1;
                            //ObjLog.WriteLog("DataScheduler : LocaltoCentral : LocationLabelPrinting => LocationCode : " + _PLPrint.LocationCode + ", MatCode : " + _PLPrint.MatCode + ", QRCode : " + _PLPrint.QRCode + " posted to Central server successfully");
                        }
                        //else
                        //{
                        //    sb = new StringBuilder();
                        //    sb.AppendLine("Update tVendorLabelPrinting set MatDesc = '" + _PLPrint.MatDesc + "', MatGrade= '" + _PLPrint.MatGrade + "'");
                        //    sb.AppendLine(", MatGroup = '" + _PLPrint.MatGroup + "', MatThickness= '" + _PLPrint.MatThickness + "', MatSize = '" + _PLPrint.MatSize + "'");
                        //    sb.AppendLine(", VendorInvoice = '" + _PLPrint.VendorInvoice + "', VendorInvDate = '" + _PLPrint.VendorInvDate + "', QRCode = '" + _PLPrint.QRCode + "'");
                        //    sb.AppendLine(", StackQRCode = '" + _PLPrint.StackQRCode + "', IsStackGenerated = '" + _PLPrint.IsStackGenerated + "', GeneratedBy = '" + _PLPrint.GeneratedBy + "'");
                        //    sb.AppendLine(", GeneratedOn = '" + _PLPrint.GeneratedOn + "', IsQRCodePrinted = '" + _PLPrint.IsQRCodePrinted + "', IsStackPrinted = '" + _PLPrint.IsStackPrinted + "'");
                        //    sb.AppendLine(", PrintedBy = '" + _PLPrint.PrintedBy + "', PrintedOn = '" + _PLPrint.PrintedOn + "', IsRePrintRequest = '" + _PLPrint.IsRePrintRequest + "'");
                        //    sb.AppendLine(", RequestedBy = '" + _PLPrint.RequestedBy + "', RequestedOn = '" + _PLPrint.RequestedOn + "', IsRePrinted = '" + _PLPrint.IsRePrinted + "'");
                        //    sb.AppendLine(", RePrintedBy = '" + _PLPrint.RePrintedBy + "', IsSAPPosted = '" + _PLPrint.IsSAPPosted + "', SAPPostMsg = '" + _PLPrint.SAPPostMsg + "'");
                        //    sb.AppendLine(", RePrintedBy = '" + _PLPrint.PostedBy + "', SAPPostMsg = '" + _PLPrint.PostedOn + "'");
                        //    sb.AppendLine(" WHERE LocationCode = '" + _PLPrint.LocationCode + "' AND PONumber =  '" + _PLPrint.PONumber + "' AND MatCode =  '" + _PLPrint.MatCode + "' AND VendorCode =  '" + _PLPrint.VendorCode + "'");
                        //    oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                        //    clsGlobal.iUpdateCount++;
                        //}
                    }
                    oDbmCentral.CommitTransaction();
                    dtPostVendorLabelPrinting.AcceptChanges();
                    oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    DataTable odtScannedData9 = new DataTable();
                    odtScannedData9 = dtPostVendorLabelPrinting.Clone();
                    DataRow[] rows1;
                    rows1 = dtPostVendorLabelPrinting.Select("IsSAPPosted = True");
                    foreach (DataRow row in rows1)
                    {
                        odtScannedData9.ImportRow(row);
                    }
                    odtScannedData9.AcceptChanges();
                    foreach (DataRow dr in odtScannedData9.Rows)
                    {
                        string sLocCode = dr["LocationCode"].ToString().Trim();
                        string sPONumber = dr["PONumber"].ToString().Trim();
                        string sMatcode = dr["MatCode"].ToString().Trim();
                        string sQRCode = dr["QRCode"].ToString().Trim();
                        string sVendorCode = dr["VendorCode"].ToString().Trim();
                        sb = new StringBuilder();
                        sb.AppendLine(" Update tVendorLabelPrinting set IsSAPPosted = 1, PostedOn = GETDATE() ");
                        sb.AppendLine(" where LocationCode = '" + sLocCode + "' AND PONumber = '" + sPONumber + "' AND MatCode = '" + sMatcode + "'");
                        sb.AppendLine("AND VendorCode = '" + sVendorCode + "' AND QRCode = '" + sQRCode + "'");
                        oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                        clsGlobal.iUpdateCount++;
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral Vendor Data : No. of Records Inserted - " + clsGlobal.iAddCount);
                    //ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral Vendor Data : No. of Records Updated - " + clsGlobal.iUpdateCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                }
                else if (dtPostVendorLabelPrinting.Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral Vendor Data : " + "There is no records found");
                    clsGlobal.lmsg = "Saving LocaltoCentral Vendor Data : " + "There is no records found";
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending Vendor Label Printing from LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " to Central Server of MaterialCode - " + _PLPrint.MatCode + " found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                //}
                oDbmCentral.RollBackTransaction();
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral Vendor Data :: Error is - " + ex.Message.ToString());
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public void GetL2CSalesReturn()
        {
            clsLogic oLogic = new clsLogic();
            PLtSalesReturn _PLSR = new PLtSalesReturn();
            StringBuilder sb;

            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrClientCon);
                _dtBindList = new DataTable();
                if (dtPostSalesReturn.Rows.Count > 0)
                {
                    //ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral Sales Return Data : No. of Records Found - " + dtPostSalesReturn.Rows.Count);
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dtPostSalesReturn.Rows)
                    {
                        sb = new StringBuilder();
                        _PLSR.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLSR.SalesReturnNo = dr["SalesReturnNo"].ToString().Trim();
                        _PLSR.MatCode = dr["MatCode"].ToString().Trim();
                        _PLSR.Qty = Convert.ToInt32(dr["Qty"].ToString().Trim());
                        _PLSR.QRCode = dr["QRCode"].ToString().Trim();
                        _PLSR.CreatedBy = dr["CreatedBy"].ToString().Trim();
                        _PLSR.CreatedOn = dr["CreatedOn"].ToString().Trim();
                        //_PLSR.IsQRCodePrinted = dr["IsQRCodePrinted"].ToString().Trim();
                        //_PLSR.PrintedBy = dr["PrintedBy"].ToString().Trim();
                        //_PLSR.PrintedOn = dr["PrintedOn"].ToString().Trim();
                        //_PLSR.IsSAPPosted = dr["IsSAPPosted"].ToString().Trim();
                        //_PLSR.SAPPostMsg = dr["SAPPostMsg"].ToString().Trim();
                        //_PLSR.PostedBy = dr["PostedBy"].ToString().Trim();
                        //_PLSR.PostedOn = dr["PostedOn"].ToString().Trim();
                        _PLSR.SentBy = clsGlobal.PlantCode.Trim() + "_DataScheduler";

                        sb.AppendLine("select QRCode from tSalesReturn WITH (NOLOCK) where LocationCode = '" + _PLSR.LocationCode + "'");
                        sb.AppendLine("and SalesReturnNo = '" + _PLSR.SalesReturnNo + "' AND MatCode = '" + _PLSR.MatCode + "' AND QRCode = '" + _PLSR.QRCode + "'");
                        DataTable dt = oDbmCentral.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Insert into tSalesReturn(LocationCode, SalesReturnNo, MatCode, Qty, QRCode, CreatedBy, CreatedOn,");
                            sb.AppendLine("IsSAPPosted, SentOn, SentBy)");
                            sb.AppendLine("Values ");
                            sb.AppendLine("('" + _PLSR.LocationCode + "','" + _PLSR.SalesReturnNo + "','" + _PLSR.MatCode + "','" + _PLSR.Qty + "'");
                            sb.AppendLine(", '" + _PLSR.QRCode + "','" + _PLSR.CreatedBy + "','" + _PLSR.CreatedOn + "'");
                            sb.AppendLine(", 0, GETDATE(), '" + _PLSR.SentBy + "')");
                            oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                            dr["IsSAPPosted"] = 1;
                        }
                    }
                    oDbmCentral.CommitTransaction();
                    dtPostSalesReturn.AcceptChanges();
                    oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    DataTable odtScannedData9 = new DataTable();
                    odtScannedData9 = dtPostSalesReturn.Clone();
                    DataRow[] rows1;
                    rows1 = dtPostSalesReturn.Select("IsSAPPosted = True");
                    foreach (DataRow row in rows1)
                    {
                        odtScannedData9.ImportRow(row);
                    }
                    odtScannedData9.AcceptChanges();
                    foreach (DataRow dr in odtScannedData9.Rows)
                    {
                        string sLocCode = dr["LocationCode"].ToString().Trim();
                        string sSRNo = dr["SalesReturnNo"].ToString().Trim();
                        string sMatcode = dr["MatCode"].ToString().Trim();
                        string sQRCode = dr["QRCode"].ToString().Trim();
                        sb = new StringBuilder();
                        sb.AppendLine(" Update tSalesReturn set IsSAPPosted = 1, PostedOn = GETDATE() ");
                        sb.AppendLine(" where LocationCode = '" + sLocCode + "'");
                        sb.AppendLine(" And SalesReturnNo = '" + sSRNo + "' AND MatCode = '" + sMatcode + "' AND QRCode = '" + sQRCode + "'");
                        oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                        clsGlobal.iUpdateCount++;
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral Sales Return Data : No. of Records Inserted - " + clsGlobal.iAddCount);
                    //ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral Sales Return Data : No. of Records Inserted - " + clsGlobal.iUpdateCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                }
                else if (dtPostSalesReturn.Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral Sales Return Data : " + "There is no records found");
                    clsGlobal.lmsg = "Saving LocaltoCentral Sales Return Data : " + "There is no records found";
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending Sales Return Data from LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " to Central Server of MaterialCode - " + _PLSR.MatCode + " found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                //}
                oDbmCentral.RollBackTransaction();
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral Sales Return Data :: Error is - " + ex.Message.ToString());
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public void GetL2CProductionReversalData()
        {
            clsLogic oLogic = new clsLogic();
            PLtMTMTransferData _PLSR = new PLtMTMTransferData();
            StringBuilder sb;

            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrClientCon);
                _dtBindList = new DataTable();
                if (dtProductionReversalData.Rows.Count > 0)
                {
                    //ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral Sales Return Data : No. of Records Found - " + dtPostSalesReturn.Rows.Count);
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dtProductionReversalData.Rows)
                    {
                        sb = new StringBuilder();
                        _PLSR.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLSR.NewMatCode = dr["NewMatCode"].ToString().Trim();
                        _PLSR.OldMatCode = dr["OldMatCode"].ToString().Trim();
                        _PLSR.QRCode = dr["QRCode"].ToString().Trim();
                        _PLSR.StackQRCode = dr["StackQRCode"].ToString().Trim();
                        _PLSR.MatStatus = dr["MatStatus"].ToString().Trim();
                        _PLSR.Status = dr["Status"].ToString().Trim();
                        _PLSR.CreatedBy = dr["CreatedBy"].ToString().Trim();
                        _PLSR.CreatedOn = dr["CreatedOn"].ToString().Trim();
                        //_PLSR.IsQRCodePrinted = dr["IsQRCodePrinted"].ToString().Trim();
                        //_PLSR.PrintedBy = dr["PrintedBy"].ToString().Trim();
                        //_PLSR.PrintedOn = dr["PrintedOn"].ToString().Trim();
                        //_PLSR.IsSAPPosted = dr["IsSAPPosted"].ToString().Trim();
                        //_PLSR.SAPPostMsg = dr["SAPPostMsg"].ToString().Trim();
                        //_PLSR.PostedBy = dr["PostedBy"].ToString().Trim();
                        //_PLSR.PostedOn = dr["PostedOn"].ToString().Trim();
                        _PLSR.SentBy = clsGlobal.PlantCode.Trim() + "_DataScheduler";

                        sb.AppendLine("SELECT QRCode from tProductionReversalData WITH (NOLOCK) WHERE LocationCode = '" + _PLSR.LocationCode + "'");
                        sb.AppendLine("AND NewMatCode = '" + _PLSR.NewMatCode + "' AND StackQRCode = '" + _PLSR.StackQRCode + "' AND QRCode = '" + _PLSR.QRCode + "'");
                        DataTable dt = oDbmCentral.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Insert into tProductionReversalData(LocationCode, NewMatCode, OldMatCode, QRCode, StackQRCode, MatStatus, Status, CreatedBy, CreatedOn,");
                            sb.AppendLine("IsSAPPosted, SentOn, SentBy)");
                            sb.AppendLine("Values ");
                            sb.AppendLine("('" + _PLSR.LocationCode + "','" + _PLSR.NewMatCode + "','" + _PLSR.OldMatCode + "','" + _PLSR.QRCode + "'");
                            sb.AppendLine(", '" + _PLSR.StackQRCode + "','" + _PLSR.MatStatus + "'");
                            sb.AppendLine(", '" + _PLSR.Status + "','" + _PLSR.CreatedBy + "','" + _PLSR.CreatedOn + "'");
                            sb.AppendLine(", 0, GETDATE(), '" + _PLSR.SentBy + "')");
                            oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                            dr["IsSAPPosted"] = 1;
                        }
                    }
                    oDbmCentral.CommitTransaction();
                    dtProductionReversalData.AcceptChanges();
                    oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    DataTable odtScannedData9 = new DataTable();
                    odtScannedData9 = dtProductionReversalData.Clone();
                    DataRow[] rows1;
                    rows1 = dtProductionReversalData.Select("IsSAPPosted = True");
                    foreach (DataRow row in rows1)
                    {
                        odtScannedData9.ImportRow(row);
                    }
                    odtScannedData9.AcceptChanges();
                    foreach (DataRow dr in odtScannedData9.Rows)
                    {
                        string sLocCode = dr["LocationCode"].ToString().Trim();
                        string sMatcode = dr["NewMatCode"].ToString().Trim();
                        string sQRCode = dr["QRCode"].ToString().Trim();
                        string sStackQRCode = dr["StackQRCode"].ToString().Trim();
                        sb = new StringBuilder();
                        sb.AppendLine(" Update tProductionReversalData set IsSAPPosted = 1, PostedOn = GETDATE() ");
                        sb.AppendLine(" where LocationCode = '" + sLocCode + "'");
                        sb.AppendLine(" And NewMatCode = '" + sMatcode + "' AND QRCode = '" + sQRCode + "' AND StackQRCode = '" + sStackQRCode + "'");
                        oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                        clsGlobal.iUpdateCount++;
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral tProductionReversalData : No. of Records Inserted - " + clsGlobal.iAddCount);
                    //ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral Sales Return Data : No. of Records Inserted - " + clsGlobal.iUpdateCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                }
                else if (dtProductionReversalData.Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral tProductionReversalData : " + "There is no records found");
                    clsGlobal.lmsg = "Saving LocaltoCentral tProductionReversalData : " + "There is no records found";
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending tProductionReversalData from LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " to Central Server of MaterialCode - " + _PLSR.NewMatCode + " found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                //}
                oDbmCentral.RollBackTransaction();
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral tProductionReversalData :: Error is - " + ex.Message.ToString());
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public void GetL2CStackHistoryData()
        {
            clsLogic oLogic = new clsLogic();
            PLtStackHistoryData _PLSR = new PLtStackHistoryData();
            StringBuilder sb;

            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrClientCon);
                _dtBindList = new DataTable();
                if (dtPostStackHistoryData.Rows.Count > 0)
                {
                    //ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral Sales Return Data : No. of Records Found - " + dtPostSalesReturn.Rows.Count);
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dtPostStackHistoryData.Rows)
                    {
                        sb = new StringBuilder();
                        _PLSR.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLSR.MatCode = dr["MatCode"].ToString().Trim();
                        _PLSR.QRCode = dr["QRCode"].ToString().Trim();
                        _PLSR.OldStack = dr["OldStack"].ToString().Trim();
                        _PLSR.NewStack = dr["NewStack"].ToString().Trim();
                        _PLSR.CreatedBy = dr["CreatedBy"].ToString().Trim();
                        _PLSR.CreatedOn = dr["CreatedOn"].ToString().Trim();
                        _PLSR.SentBy = clsGlobal.PlantCode.Trim() + "_DataScheduler";

                        sb.AppendLine("SELECT QRCode from tStackHistory WITH (NOLOCK) WHERE LocationCode = '" + _PLSR.LocationCode + "'");
                        sb.AppendLine("AND MatCode = '" + _PLSR.MatCode + "' AND OldStack = '" + _PLSR.OldStack + "' AND NewStack = '" + _PLSR.NewStack + "' AND QRCode = '" + _PLSR.QRCode + "'");
                        DataTable dt = oDbmCentral.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Insert into tStackHistory(LocationCode, MatCode, QRCode, NewStack, OldStack, CreatedBy, CreatedOn,");
                            sb.AppendLine("IsSAPPosted, SentOn, SentBy)");
                            sb.AppendLine("Values ");
                            sb.AppendLine("('" + _PLSR.LocationCode + "','" + _PLSR.MatCode + "','" + _PLSR.QRCode + "','" + _PLSR.NewStack + "'");
                            sb.AppendLine(", '" + _PLSR.OldStack + "','" + _PLSR.CreatedBy + "','" + _PLSR.CreatedOn + "'");
                            sb.AppendLine(", 0, GETDATE(), '" + _PLSR.SentBy + "')");
                            oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                            dr["IsSAPPosted"] = 1;
                        }
                    }
                    oDbmCentral.CommitTransaction();
                    dtPostStackHistoryData.AcceptChanges();
                    oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    DataTable odtScannedData9 = new DataTable();
                    odtScannedData9 = dtPostStackHistoryData.Clone();
                    DataRow[] rows1;
                    rows1 = dtPostStackHistoryData.Select("IsSAPPosted = True");
                    foreach (DataRow row in rows1)
                    {
                        odtScannedData9.ImportRow(row);
                    }
                    odtScannedData9.AcceptChanges();
                    foreach (DataRow dr in odtScannedData9.Rows)
                    {
                        string sLocCode = dr["LocationCode"].ToString().Trim();
                        string sMatcode = dr["MatCode"].ToString().Trim();
                        string sQRCode = dr["QRCode"].ToString().Trim();
                        string sNewStack = dr["NewStack"].ToString().Trim();
                        string sOldStack = dr["OldStack"].ToString().Trim();
                        sb = new StringBuilder();
                        sb.AppendLine(" Update tStackHistory set IsSAPPosted = 1, PostedOn = GETDATE() ");
                        sb.AppendLine(" where LocationCode = '" + sLocCode + "'");
                        sb.AppendLine(" And MatCode = '" + sMatcode + "' AND QRCode = '" + sQRCode + "' AND OldStack = '" + sOldStack + "' AND NewStack = '" + sNewStack + "'");
                        oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                        clsGlobal.iUpdateCount++;
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral tStackHistoryData : No. of Records Inserted - " + clsGlobal.iAddCount);
                    //ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral tStackHistoryData : No. of Records Inserted - " + clsGlobal.iUpdateCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                }
                else if (dtProductionReversalData.Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral tStackHistoryData : " + "There is no records found");
                    clsGlobal.lmsg = "Saving LocaltoCentral tStackHistoryData : " + "There is no records found";
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending tStackHistoryData from LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " to Central Server of MaterialCode - " + _PLSR.MatCode + " found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                //}
                oDbmCentral.RollBackTransaction();
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral tStackHistoryData :: Error is - " + ex.Message.ToString());
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public void GetL2CPurchaseReturn()
        {
            clsLogic oLogic = new clsLogic();
            PLtPurchaseReturn _PLSR = new PLtPurchaseReturn();
            StringBuilder sb;

            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrClientCon);
                _dtBindList = new DataTable();
                if (dtPostPurchaseReturn.Rows.Count > 0)
                {
                    //ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral Purchase Return Data : No. of Records Found - " + ds.Tables["tPurchaseReturn"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dtPostPurchaseReturn.Rows)
                    {
                        sb = new StringBuilder();
                        _PLSR.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLSR.POReturnNo = dr["POReturnNo"].ToString().Trim();
                        _PLSR.MatCode = dr["MatCode"].ToString().Trim();
                        _PLSR.Qty = Convert.ToInt32(dr["Qty"].ToString().Trim());
                        _PLSR.QRCode = dr["QRCode"].ToString().Trim();
                        _PLSR.VendorCode = dr["VendorCode"].ToString().Trim();
                        _PLSR.CreatedBy = dr["CreatedBy"].ToString().Trim();
                        _PLSR.CreatedOn = dr["CreatedOn"].ToString().Trim();
                        _PLSR.SentBy = clsGlobal.PlantCode.Trim() + "_DataScheduler";

                        sb.AppendLine("select * from tPurchaseReturn WITH (NOLOCK) where LocationCode = '" + _PLSR.LocationCode + "'");
                        sb.AppendLine("and POReturnNo = '" + _PLSR.POReturnNo + "' AND MatCode = '" + _PLSR.MatCode + "' AND QRCode = '" + _PLSR.QRCode + "'");
                        DataTable dt = oDbmCentral.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Insert into tPurchaseReturn(LocationCode, POReturnNo, MatCode, Qty, QRCode, VendorCode, CreatedBy, CreatedOn,");
                            sb.AppendLine("IsSAPPosted, SentOn, SentBy)");
                            sb.AppendLine("Values ");
                            sb.AppendLine("('" + _PLSR.LocationCode + "','" + _PLSR.POReturnNo + "','" + _PLSR.MatCode + "','" + _PLSR.Qty + "'");
                            sb.AppendLine(", '" + _PLSR.QRCode + "','" + _PLSR.VendorCode + "','" + _PLSR.CreatedBy + "','" + _PLSR.CreatedOn + "'");
                            sb.AppendLine(", 0, GETDATE(), '" + _PLSR.SentBy + "')");
                            //sb.AppendLine(", '" + _PLSR.PostedBy + "', '" + _PLSR.PostedOn + "') ");
                            oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                            dr["IsSAPPosted"] = 1;
                        }
                    }
                    oDbmCentral.CommitTransaction();
                    dtPostPurchaseReturn.AcceptChanges();
                    oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    DataTable odtScannedData9 = new DataTable();
                    odtScannedData9 = dtPostPurchaseReturn.Clone();
                    DataRow[] rows1;
                    rows1 = dtPostPurchaseReturn.Select("IsSAPPosted = True");
                    foreach (DataRow row in rows1)
                    {
                        odtScannedData9.ImportRow(row);
                    }
                    odtScannedData9.AcceptChanges();
                    foreach (DataRow dr in odtScannedData9.Rows)
                    {
                        string sLocCode = dr["LocationCode"].ToString().Trim();
                        string sPONo = dr["POReturnNo"].ToString().Trim();
                        string sMatcode = dr["MatCode"].ToString().Trim();
                        string sQRCode = dr["QRCode"].ToString().Trim();
                        sb = new StringBuilder();
                        sb.AppendLine(" Update tPurchaseReturn set IsSAPPosted = 1, PostedOn = GETDATE() ");
                        sb.AppendLine(" where LocationCode = '" + sLocCode + "'");
                        sb.AppendLine(" And POReturnNo = '" + sPONo + "' AND MatCode = '" + sMatcode + "' AND QRCode = '" + sQRCode + "'");
                        oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                        clsGlobal.iUpdateCount++;
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral PurchaseReturn Data : No. of Records Inserted - " + clsGlobal.iAddCount);
                }
                else if (dtPostPurchaseReturn.Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral PurchaseReturn Data : " + "There is no records found");
                    clsGlobal.lmsg = "Saving LocaltoCentral PurchaseReturn Data : " + "There is no records found";
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending PurchaseReturnData from LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " to Central Server of MaterialCode - " + _PLSR.MatCode + " found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                //}
                oDbmCentral.RollBackTransaction();
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral PurchaseReturn Data :: Error is - " + ex.Message.ToString());
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public void GetL2CtDeliveryCancellationData()
        {
            clsLogic oLogic = new clsLogic();
            PLtDispatchData _PLSR = new PLtDispatchData();
            StringBuilder sb;

            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrClientCon);
                _dtBindList = new DataTable();
                if (dtPostDeliveryCancelled.Rows.Count > 0)
                {
                    clsGlobal.iAddCount = 0;
                    clsGlobal.iUpdateCount = 0;
                    //ObjLog.WriteLog("Data Scheduler : LocaltoCentral Dispatch Data : Total No. of Records Found - " + ds.Tables["tDispatchData"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dtPostDeliveryCancelled.Rows)
                    {
                        sb = new StringBuilder();
                        _PLSR.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLSR.DONo = dr["DONo"].ToString().Trim();
                        _PLSR.MatCode = dr["MatCode"].ToString().Trim();
                        _PLSR.Qty = Convert.ToInt32(dr["Qty"].ToString().Trim());
                        _PLSR.QRCode = dr["QRCode"].ToString().Trim();
                        _PLSR.StackQRCode = dr["StackQRCode"].ToString().Trim();
                        _PLSR.Status = dr["Status"].ToString().Trim();
                        _PLSR.CreatedBy = dr["CreatedBy"].ToString().Trim();
                        _PLSR.CreatedOn = dr["CreatedOn"].ToString().Trim();
                        _PLSR.UpdatedBy = dr["UpdatedBy"].ToString().Trim();
                        _PLSR.UpdatedOn = dr["UpdatedOn"].ToString().Trim();
                        _PLSR.SentBy = clsGlobal.PlantCode.Trim() + "_DataScheduler";

                        sb.AppendLine("select * from tDeliveryCancellationData WITH (NOLOCK) where LocationCode = '" + _PLSR.LocationCode + "'");
                        sb.AppendLine(" And DONo = '" + _PLSR.DONo + "' AND MatCode = '" + _PLSR.MatCode + "' AND QRCode = '" + _PLSR.QRCode + "'");

                        DataTable dt = oDbmCentral.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Insert into tDeliveryCancellationData(LocationCode, DONo, MatCode, Qty, QRCode, StackQRCode, Status,");
                            sb.AppendLine("CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, SentBy, SentOn, IsSAPPosted)");
                            sb.AppendLine("Values ");
                            sb.AppendLine("('" + _PLSR.LocationCode + "','" + _PLSR.DONo + "','" + _PLSR.MatCode + "','" + _PLSR.Qty + "'");
                            sb.AppendLine(", '" + _PLSR.QRCode + "','" + _PLSR.StackQRCode + "','" + _PLSR.Status + "','" + _PLSR.CreatedBy + "'");
                            sb.AppendLine(", '" + _PLSR.CreatedOn + "','" + _PLSR.UpdatedBy + "','" + _PLSR.UpdatedOn + "','" + _PLSR.SentBy + "', GETDATE(), 0 )");
                            oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                            dr["IsSAPPosted"] = 1;
                            //ObjLog.WriteLog("DataScheduler : LocaltoCentral : DispatchData => LocationCode : " + _PLSR.LocationCode + ", DONo : " + _PLSR.DONo + ", MatCode : " + _PLSR.MatCode + ", QRCode : " + _PLSR.QRCode + " posted to Central server successfully");
                        }
                    }
                    oDbmCentral.CommitTransaction();
                    dtPostDeliveryCancelled.AcceptChanges();
                    oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    DataTable odtScannedData9 = new DataTable();
                    odtScannedData9 = dtPostDeliveryCancelled.Clone();
                    DataRow[] rows1;
                    rows1 = dtPostDeliveryCancelled.Select("IsSAPPosted = True");
                    foreach (DataRow row in rows1)
                    {
                        odtScannedData9.ImportRow(row);
                    }
                    odtScannedData9.AcceptChanges();
                    foreach (DataRow dr in odtScannedData9.Rows)
                    {
                        string sLocCode = dr["LocationCode"].ToString().Trim();
                        string sDONo = dr["DONo"].ToString().Trim();
                        string sMatcode = dr["MatCode"].ToString().Trim();
                        string sQRCode = dr["QRCode"].ToString().Trim();
                        sb = new StringBuilder();
                        sb.AppendLine(" Update tDeliveryCancellationData set IsSAPPosted = 1, PostedOn = GETDATE() ");
                        sb.AppendLine(" where LocationCode = '" + sLocCode + "'");
                        sb.AppendLine(" And DONo = '" + sDONo + "' AND MatCode = '" + sMatcode + "' AND QRCode = '" + sQRCode + "'");
                        oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                        clsGlobal.iUpdateCount++;
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler : LocaltoCentral DeliveryCancellationData Data : Total No. of Records Inserted - " + clsGlobal.iAddCount);
                }
                else if (dtPostDeliveryCancelled.Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler : LocaltoCentral DeliveryCancellationData Data : " + "There is no records found");
                    clsGlobal.lmsg = "Data Scheduler : LocaltoCentral DeliveryCancellationData Data : " + "There is no records found";
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending Delivery Cancellation from LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " to Central Server of MaterialCode - " + _PLSR.MatCode + " found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                //}
                oDbmCentral.RollBackTransaction();
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler : LocaltoCentral DeliveryCancellationData Data : Error is - " + ex.Message.ToString());
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public void GetL2CtMatDamageData()
        {
            clsLogic oLogic = new clsLogic();
            PLtDispatchData _PLSR = new PLtDispatchData();
            StringBuilder sb;

            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrClientCon);
                _dtBindList = new DataTable();
                if (dtPostMatDamage.Rows.Count > 0)
                {
                    //ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral ItemSerial Data : No. of Records Found - " + ds.Tables["tMatDamageData"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dtPostMatDamage.Rows)
                    {
                        sb = new StringBuilder();
                        _PLSR.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLSR.DONo = dr["DONo"].ToString().Trim();
                        _PLSR.MatCode = dr["MatCode"].ToString().Trim();
                        _PLSR.QRCode = dr["QRCode"].ToString().Trim();
                        _PLSR.StackQRCode = dr["StackQRCode"].ToString().Trim();
                        _PLSR.Status = dr["Status"].ToString().Trim();
                        _PLSR.CreatedBy = dr["CreatedBy"].ToString().Trim();
                        _PLSR.CreatedOn = dr["CreatedOn"].ToString().Trim();
                        _PLSR.SentBy = clsGlobal.PlantCode.Trim() + "_DataScheduler";

                        sb.Append("SELECT * from tMatDamageData WITH (NOLOCK) where LocationCode = '" + _PLSR.LocationCode + "'");
                        sb.AppendLine("and DONo = '" + _PLSR.DONo + "' AND MatCode = '" + _PLSR.MatCode + "' AND QRCode = '" + _PLSR.QRCode + "'");
                        DataTable dt = oDbmCentral.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Insert into tMatDamageData(LocationCode, DONo, MatCode, QRCode, StackQRCode, Status, CreatedBy, CreatedOn,");
                            sb.AppendLine("IsSAPPosted, SentOn, SentBy)");
                            sb.AppendLine("Values ");
                            sb.AppendLine("('" + _PLSR.LocationCode + "','" + _PLSR.DONo + "','" + _PLSR.MatCode + "','" + _PLSR.QRCode + "'");
                            sb.AppendLine(", '" + _PLSR.StackQRCode + "','" + _PLSR.Status + "'");
                            sb.AppendLine(", '" + _PLSR.CreatedBy + "','" + _PLSR.CreatedOn + "'");
                            sb.AppendLine(", 0, GETDATE(), '" + _PLSR.SentBy + "')");
                            oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                            dr["IsSAPPosted"] = 1;
                        }
                    }
                    oDbmCentral.CommitTransaction();
                    dtPostMatDamage.AcceptChanges();
                    oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    DataTable odtScannedData9 = new DataTable();
                    odtScannedData9 = dtPostMatDamage.Clone();
                    DataRow[] rows1;
                    rows1 = dtPostMatDamage.Select("IsSAPPosted = True");
                    foreach (DataRow row in rows1)
                    {
                        odtScannedData9.ImportRow(row);
                    }
                    odtScannedData9.AcceptChanges();
                    foreach (DataRow dr in odtScannedData9.Rows)
                    {
                        string sLocCode = dr["LocationCode"].ToString().Trim();
                        string sMatcode = dr["MatCode"].ToString().Trim();
                        string sQRCode = dr["QRCode"].ToString().Trim();
                        string sDONo = dr["DONo"].ToString().Trim();
                        sb = new StringBuilder();
                        sb.AppendLine(" Update tMatDamageData set IsSAPPosted = 1, PostedOn = GETDATE() ");
                        sb.AppendLine(" where LocationCode = '" + sLocCode + "'");
                        sb.AppendLine(" And MatCode = '" + sMatcode + "' AND QRCode = '" + sQRCode + "' AND DONo = '" + sDONo + "'");
                        oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                        clsGlobal.iUpdateCount++;
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral tProductionReversalData : No. of Records Inserted - " + clsGlobal.iAddCount);
                    //ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral Sales Return Data : No. of Records Inserted - " + clsGlobal.iUpdateCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                }
                else if (dtPostMatDamage.Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral tProductionReversalData : " + "There is no records found");
                    clsGlobal.lmsg = "Saving LocaltoCentral tProductionReversalData : " + "There is no records found";
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending MatDamagedData from LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " to Central Server of MaterialCode - " + _PLSR.MatCode + " found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                //}
                oDbmCentral.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral MatDamageData :: Error is - " + ex.Message.ToString());
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
        }

        public void GetL2CtItemSerial()
        {
            clsLogic oLogic = new clsLogic();
            PLtItemSerial _PLiSerial = new PLtItemSerial();
            StringBuilder sb;

            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrClientCon);
                _dtBindList = new DataTable();
                if (ds.Tables["tItemSerial"].Rows.Count > 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral ItemSerial Data : No. of Records Found - " + ds.Tables["tItemSerial"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in ds.Tables["tItemSerial"].Rows)
                    {
                        sb = new StringBuilder();
                        _PLiSerial.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLiSerial.PrintSection = dr["PrintSection"].ToString().Trim();
                        _PLiSerial.DateFormat = dr["DateFormat"].ToString().Trim();
                        _PLiSerial.QRCodeSerialNo = dr["QRCodeSerialNo"].ToString().Trim();
                        _PLiSerial.StackSerialNo = dr["StackSerialNo"].ToString().Trim();
                        _PLiSerial.LocationType = dr["LocationType"].ToString().Trim();
                        _PLiSerial.CreatedBy = dr["CreatedBy"].ToString().Trim();
                        _PLiSerial.CreatedOn = dr["CreatedOn"].ToString().Trim();

                        sb.Append(" SELECT * from tItemSerial WITH (NOLOCK) where LocationCode = '" + _PLiSerial.LocationCode + "'");
                        sb.AppendLine("and DateFormat = '" + _PLiSerial.DateFormat + "' AND PrintSection = '" + _PLiSerial.PrintSection + "' AND LocationType = '" + _PLiSerial.LocationType + "'");
                        DataTable dt = oDbmCentral.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Insert into tItemSerial(LocationCode, PrintSection, DateFormat, QRCodeSerialNo, StackSerialNo, LocationType, CreatedBy, CreatedOn) ");
                            sb.AppendLine("Values ");
                            sb.AppendLine("('" + _PLiSerial.LocationCode + "','" + _PLiSerial.PrintSection + "','" + _PLiSerial.DateFormat + "','" + _PLiSerial.QRCodeSerialNo + "','" + _PLiSerial.StackSerialNo + "',");
                            sb.AppendLine("('" + _PLiSerial.LocationType + "','" + _PLiSerial.CreatedBy + "','" + _PLiSerial.CreatedOn + "' ) ");
                            oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                        }
                        //else
                        //{
                        //    sb = new StringBuilder();
                        //    sb.AppendLine("Update tItemSerial SET QRCodeSerialNo = '" + _PLiSerial.QRCodeSerialNo + "',");
                        //    sb.AppendLine(" StackSerialNo = '" + _PLiSerial.StackSerialNo + "', CreatedBy = '" + _PLiSerial.CreatedBy + "', CreatedOn = '" + _PLiSerial.CreatedOn + "',");
                        //    sb.AppendLine(" WHERE LocationCode = '" + _PLiSerial.LocationCode + " AND DateFormat = '" + _PLiSerial.DateFormat + "' and PrintType = '" + _PLiSerial.PrintType + "'");
                        //    oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                        //    clsGlobal.iUpdateCount++;

                        //}
                    }
                    oDbmCentral.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral ItemSerial Data : No. of Records Inserted - " + clsGlobal.iAddCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral ItemSerial Data : No. of Records Updated - " + clsGlobal.iUpdateCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                }
                else if (ds.Tables["tItemSerial"].Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral ItemSerial Data : " + "There is no records found" + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    clsGlobal.lmsg = "Saving LocaltoCentral ItemSerial Data : " + "There is no records found" + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending ItemSerial from LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " to Central Server found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                //}
                oDbmCentral.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral ItemSerial Data :: Error is - " + ex.Message.ToString() + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
        }

        public void GetL2CtSAPDeliveryOrderData()
        {
            clsLogic oLogic = new clsLogic();
            PLSAPDeliveryOrderData _PLDOData = new PLSAPDeliveryOrderData();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (ds.Tables["tSAPDeliveryOrderData"].Rows.Count > 0)
                {
                    clsGlobal.iAddCount = 0;
                    clsGlobal.iUpdateCount = 0;
                    ObjLog.WriteLog("Data Scheduler : CentralToLocal SAPDeliveryOrder Data : Total No. of Records Found - " + ds.Tables["tSAPDeliveryOrderData"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    foreach (DataRow dr in ds.Tables["tSAPDeliveryOrderData"].Rows)
                    {
                        sb = new StringBuilder();
                        if (dr["LocationCode"].ToString().Trim() == clsGlobal.PlantCode.Trim().ToString())
                        {
                            _PLDOData.LocationCode = dr["LocationCode"].ToString().Trim();
                            _PLDOData.DeliveryOrderNo = dr["DeliveryOrderNo"].ToString().Trim();
                            _PLDOData.CustomerCode = dr["CustomerCode"].ToString().Trim();
                            _PLDOData.CustomerName = dr["CustomerName"].ToString().Trim();
                            _PLDOData.MatCode = dr["MatCode"].ToString().Trim();
                            _PLDOData.MatDesc = dr["MatDesc"].ToString().Trim();
                            _PLDOData.ToLocationCode = dr["ToLocationCode"].ToString().Trim();
                            _PLDOData.DOQty = Convert.ToInt32(dr["DOQty"].ToString().Trim());
                            _PLDOData.DODate = dr["DODate"].ToString().Trim();
                            _PLDOData.DOStatus = dr["DOStatus"].ToString().Trim();
                            _PLDOData.DownloadOn = dr["DownloadOn"].ToString().Trim();
                            _PLDOData.DownloadBy = dr["DownloadBy"].ToString().Trim();
                            _PLDOData.IsSAPPosted = dr["IsSAPPosted"].ToString().Trim();
                            _PLDOData.SAPPostedQty = dr["SAPPostedQty"].ToString().Trim();
                            //_PLDOData.PostedBy = dr["PostedBy"].ToString().Trim();
                            //_PLDOData.PostedOn = dr["PostedOn"].ToString().Trim();

                            sb.Append("Select MatCode from tSAPDeliveryOrderData WITH (NOLOCK) WHERE LocationCode = '" + _PLDOData.LocationCode + "' AND DeliveryOrderNo = '" + _PLDOData.DeliveryOrderNo + "'");
                            sb.Append(" AND CustomerCode = '" + _PLDOData.CustomerCode + "' AND MatCode = '" + _PLDOData.MatCode + "'");
                            DataTable dt = oDbmLocal.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                            //if (dt.Rows.Count == 0)
                            //{
                            //    sb = new StringBuilder();
                            //    sb.AppendLine("Insert into tSAPDeliveryOrderData(LocationCode, DeliveryOrderNo, CustomerCode, CustomerName, MatCode, MatDesc,");
                            //    sb.AppendLine("ToLocationCode, DOQty, DODate, DOStatus, DownloadOn, DownloadBy, IsSAPPosted, SAPPostedQty )");  //, PostedBy, PostedOn
                            //    sb.AppendLine("Values ");
                            //    sb.AppendLine("('" + _PLDOData.LocationCode + "','" + _PLDOData.DeliveryOrderNo + "','" + _PLDOData.CustomerCode + "','" + _PLDOData.CustomerName + "',");
                            //    sb.AppendLine("'" + _PLDOData.MatCode + "','" + _PLDOData.MatDesc + "','" + _PLDOData.ToLocationCode + "',");
                            //    sb.AppendLine("'" + _PLDOData.DOQty + "','" + _PLDOData.DODate + "','" + _PLDOData.DOStatus + "', GETDATE(), ");
                            //    sb.AppendLine("'" + _PLDOData.DownloadBy + "','" + _PLDOData.IsSAPPosted + "','" + _PLDOData.SAPPostedQty + "')");
                            //    //sb.AppendLine("'" + _PLDOData.DownloadBy + "','" + _PLDOData.IsSAPPosted + "','" + _PLDOData.SAPPostedQty + "',");
                            //    //sb.AppendLine("'" + _PLDOData.PostedBy + "','" + _PLDOData.PostedOn + "')");
                            //    oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            //    clsGlobal.iAddCount++;
                            //    ObjLog.WriteLog("Data Scheduler : CentralToLocal SAPDeliveryOrder Data : No. of Records Inserted - " + clsGlobal.iAddCount + " of DONo - " + _PLDOData.DeliveryOrderNo + " With MatCode - " + _PLDOData.MatCode + " And Qty - " + _PLDOData.DOQty + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                            //}
                            if (dt.Rows.Count == 0)
                            {
                                sb = new StringBuilder();
                                sb.AppendLine(" Update tSAPDeliveryOrderData set CustomerName = '" + _PLDOData.CustomerName + "', MatDesc = '" + _PLDOData.MatDesc + "'");
                                sb.AppendLine(", ToLocationCode = '" + _PLDOData.ToLocationCode + "', DOQty = '" + _PLDOData.DOQty + "', DODate = '" + _PLDOData.DODate + "'");
                                sb.AppendLine(", DOStatus = '" + _PLDOData.DOStatus + "', DownloadOn =  GETDATE(), DownloadBy = '" + _PLDOData.DownloadBy + "'");
                                sb.AppendLine(", IsSAPPosted = '" + _PLDOData.IsSAPPosted + "', SAPPostedQty = '" + _PLDOData.SAPPostedQty + "'");  //"', PostedBy = '" + _PLDOData.PostedBy + 
                                //sb.AppendLine(", PostedOn = '" + _PLDOData.PostedOn + "'");
                                sb.AppendLine(" WHERE LocationCode = '" + _PLDOData.LocationCode + "' AND DeliveryOrderNo = '" + _PLDOData.DeliveryOrderNo + "' AND CustomerCode = '" + _PLDOData.CustomerCode + "' AND MatCode = '" + _PLDOData.MatCode + "'");
                                oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                                clsGlobal.iUpdateCount++;
                                ObjLog.WriteLog("Data Scheduler : CentralToLocal SAPDeliveryOrder Data : No. of Records Updated - " + clsGlobal.iUpdateCount + " of DONo - " + _PLDOData.DeliveryOrderNo + " With MatCode - " + _PLDOData.MatCode + " And Qty - " + _PLDOData.DOQty + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                            }
                        }
                        //break;
                    }
                    //
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler : CentralToLocal SAPDeliveryOrder Data : Total No. of Records Inserted - " + clsGlobal.iAddCount + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    ObjLog.WriteLog("Data Scheduler : CentralToLocal SAPDeliveryOrder Data : Total No. of Records Updated - " + clsGlobal.iUpdateCount + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                }
                else if (ds.Tables["tSAPDeliveryOrderData"].Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler : CentralToLocal SAPDeliveryOrder Data : " + "There is no records found" + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    clsGlobal.lmsg = "CentralToLocal SAPDeliveryOrder Data : " + "There is no records found" + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                }
            }
            catch (Exception ex)
            {
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPDeliveryOrder Data :: Error is - " + ex.Message.ToString() + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
            }
            finally
            {
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public void GetL2CtSAPPurchaseOrderData()
        {
            clsLogic oLogic = new clsLogic();
            PLSAPPurchaseOrderData _PLPurOrder = new PLSAPPurchaseOrderData();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();

                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrClientCon);
                _dtBindList = new DataTable();
                if (dtPostSAPPurchaseOrderData.Rows.Count > 0)
                {
                    //ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPPurchaseOrder Data : No. of Records Found - " + ds.Tables["tSAPPurchaseOrderData"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dtPostSAPPurchaseOrderData.Rows)
                    {
                        sb = new StringBuilder();
                        _PLPurOrder.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLPurOrder.PONumber = dr["PONumber"].ToString().Trim();
                        _PLPurOrder.VendorCode = dr["VendorCode"].ToString().Trim();
                        _PLPurOrder.MatCode = dr["MatCode"].ToString().Trim();
                        _PLPurOrder.POLocType = Convert.ToString(dr["POLocType"].ToString().Trim());
                        _PLPurOrder.IsQRCodeGenerated = dr["IsQRCodeGenerated"].ToString().Trim();
                        _PLPurOrder.GeneratedQty = dr["GeneratedQty"].ToString().Trim();
                        _PLPurOrder.GeneratedBy = dr["GeneratedBy"].ToString().Trim();
                        _PLPurOrder.GeneratedOn = dr["GeneratedOn"].ToString().Trim();

                        sb.Append("Select MatCode from tSAPPurchaseOrderData WITH (NOLOCK) WHERE LocationCode = '" + _PLPurOrder.LocationCode + "' AND PONumber = '" + _PLPurOrder.PONumber + "'");
                        sb.Append(" AND VendorCode = '" + _PLPurOrder.VendorCode + "' AND MatCode = '" + _PLPurOrder.MatCode + "' AND POLocType = '" + _PLPurOrder.POLocType + "'");
                        DataTable dt = oDbmCentral.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        //if (dt.Rows.Count == 0)
                        //{
                        //    sb = new StringBuilder();
                        //    sb.AppendLine("Insert into tSAPPurchaseOrderData(LocationCode, PONumber, VendorCode, VendorName, MatCode, POQty, POLocType, ");
                        //    sb.AppendLine("PODate, POStatus, IsQRCodeGenerated, GeneratedQty, IsPrinted, IsReprinted, DownloadOn, DownloadBy)");
                        //    sb.AppendLine("Values ");
                        //    sb.AppendLine("('" + _PLPurOrder.LocationCode + "','" + _PLPurOrder.PONumber + "','" + _PLPurOrder.VendorCode + "','" + _PLPurOrder.VendorName + "',");
                        //    sb.AppendLine("'" + _PLPurOrder.MatCode + "','" + _PLPurOrder.POQty + "',");
                        //    //sb.AppendLine("'" + _PLPurOrder.MatGrade + "','" + _PLPurOrder.Category + "','" + _PLPurOrder.MatGroup + "',");
                        //    sb.AppendLine("'" + _PLPurOrder.POLocType + "','" + _PLPurOrder.PODate + "','" + _PLPurOrder.POStatus + "',");
                        //    sb.AppendLine(" 0, 0, 0, 0,  GetDate(), '" + _PLPurOrder.DownloadBy + "')");
                        //    oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                        //    clsGlobal.iAddCount++;
                        //}
                        if (dt.Rows.Count == 1)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine(" Update tSAPPurchaseOrderData set GeneratedQty = '" + _PLPurOrder.GeneratedQty + "'");
                            sb.AppendLine(", IsQRCodeGenerated = '" + _PLPurOrder.IsQRCodeGenerated + "', GeneratedBy = '" + _PLPurOrder.GeneratedBy + "', GeneratedOn = '" + _PLPurOrder.GeneratedOn + "'");
                            sb.AppendLine(" WHERE LocationCode = '" + _PLPurOrder.LocationCode + "' AND PONumber = '" + _PLPurOrder.PONumber + "'");
                            sb.AppendLine(" AND VendorCode = '" + _PLPurOrder.VendorCode + "' AND MatCode = '" + _PLPurOrder.MatCode + "' AND POLocType = '" + _PLPurOrder.POLocType + "'");
                            oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iUpdateCount++;
                        }
                        //break;
                    }
                    oDbmCentral.CommitTransaction();
                    //ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPPurchaseOrder Data : No. of Records Inserted - " + clsGlobal.iAddCount);
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPPurchaseOrder Data : No. of Records Updated - " + clsGlobal.iUpdateCount);
                }
                else if (ds.Tables["tSAPPurchaseOrderData"].Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPPurchaseOrder Data : " + "There is no records found");
                    clsGlobal.lmsg = "Saving CentralToLocal SAPPurchaseOrder Data : " + "There is no records found";
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending Updation of PurchaseOrderData from LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " to Central Server of MaterialCode - " + _PLPurOrder.MatCode + " found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                //}
                oDbmCentral.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPPurchaseOrder Data :: Error is - " + ex.Message.ToString());
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
        }

        public void GetL2CStockCount()
        {
            clsLogic oLogic = new clsLogic();
            PLtStockCount _PLSCount = new PLtStockCount();
            StringBuilder sb;

            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrClientCon);
                _dtBindList = new DataTable();
                if (ds.Tables["tStockCount"].Rows.Count > 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral Stock Count Data : No. of Records Found - " + ds.Tables["tStockCount"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in ds.Tables["tStockCount"].Rows)
                    {
                        sb = new StringBuilder();
                        _PLSCount.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLSCount.MatCode = dr["MatCode"].ToString().Trim();
                        _PLSCount.QRCode = dr["QRCode"].ToString().Trim();
                        _PLSCount.StackQRCode = dr["StackQRCode"].ToString().Trim();
                        _PLSCount.StockBy = dr["StockBy"].ToString().Trim();
                        _PLSCount.StockOn = dr["StockOn"].ToString().Trim();

                        sb.AppendLine("Select * from tStockCount WITH (NOLOCK) where LocationCode = '" + _PLSCount.LocationCode + "' AND MatCode= '" + _PLSCount.MatCode + "'");
                        sb.AppendLine(" AND QRCode = '" + _PLSCount.QRCode + "'");
                        DataTable dt = oDbmCentral.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Insert into tStockCount(LocationCode, MatCode, QRCode, StackQRCode, StockBy, StockOn )");
                            sb.AppendLine("Values ");
                            sb.AppendLine("('" + _PLSCount.LocationCode + "','" + _PLSCount.MatCode + "','" + _PLSCount.QRCode + "','" + _PLSCount.StackQRCode + "'");
                            sb.AppendLine("'" + _PLSCount.StockBy + "', '" + _PLSCount.StockOn + "') ");
                            oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                        }
                        else
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Update tStockCount SET StackQRCode = '" + _PLSCount.StackQRCode + "',");
                            sb.AppendLine(" StockBy = '" + _PLSCount.StockBy + "', StockOn = '" + _PLSCount.StockOn + "'");
                            sb.AppendLine(" WHERE LocationCode = '" + _PLSCount.LocationCode + "' AND MatCode= '" + _PLSCount.MatCode + "'");
                            sb.AppendLine(" AND QRCode = '" + _PLSCount.QRCode + "'");
                            oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iUpdateCount++;
                        }
                    }
                    oDbmCentral.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral Stock Count Data : No. of Records Inserted - " + clsGlobal.iAddCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral Stock Count Data : No. of Records Updated - " + clsGlobal.iUpdateCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                }
                else if (ds.Tables["tStockCount"].Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral Stock Count Data : " + "There is no records found" + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    clsGlobal.lmsg = "Saving LocaltoCentral Stock Count Data : " + "There is no records found" + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral StockCount Data :: Error is - " + ex.Message.ToString() + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
        }

        public void GetL2CGroupRightsData()
        {
            clsLogic oLogic = new clsLogic();
            PLGroupRights _PLGRData = new PLGroupRights();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (ds.Tables["Vendor"].Rows.Count > 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral ItemSerial Data : No. of Records Found - " + ds.Tables["tItemSerial"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in ds.Tables["Vendor"].Rows)
                    {
                        sb = new StringBuilder();
                        _PLGRData.GROUP_ID = dr["GROUP_ID"].ToString().Trim();
                        _PLGRData.MODULE_ID = dr["MODULE_ID"].ToString().Trim();
                        _PLGRData.VIEW_RIGHTS = dr["VIEW_RIGHTS"].ToString().Trim();
                        _PLGRData.SAVE_RIGHTS = dr["SAVE_RIGHTS"].ToString().Trim();
                        _PLGRData.EDIT_RIGHTS = dr["EDIT_RIGHTS"].ToString().Trim();
                        _PLGRData.DELETE_RIGHTS = dr["DELETE_RIGHTS"].ToString().Trim();
                        _PLGRData.DOWNLOAD_RIGHTS = dr["DOWNLOAD_RIGHTS"].ToString().Trim();
                        _PLGRData.lDownloadedBy = "Scheduler";

                        sb.Append("select VIEW_RIGHTS from mGroupRights WITH (NOLOCK) where GROUP_ID = '" + _PLGRData.GROUP_ID + "' AND MODULE_ID = '" + _PLGRData.MODULE_ID + "'");
                        DataTable dt = oDbmCentral.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Insert into mVendor(GROUP_ID, MODULE_ID, VIEW_RIGHTS, SAVE_RIGHTS,");
                            sb.AppendLine(" EDIT_RIGHTS, DELETE_RIGHTS, DOWNLOAD_RIGHTS)");
                            sb.AppendLine("Values ");
                            sb.AppendLine("('" + _PLGRData.GROUP_ID + "','" + _PLGRData.MODULE_ID + "','" + _PLGRData.VIEW_RIGHTS + "','" + _PLGRData.SAVE_RIGHTS + "',");
                            sb.AppendLine("'" + _PLGRData.EDIT_RIGHTS + "','" + _PLGRData.DELETE_RIGHTS + "',");
                            sb.AppendLine("'" + _PLGRData.DOWNLOAD_RIGHTS + "')");
                            oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                        }
                        else
                        {
                            sb = new StringBuilder();
                            sb.AppendLine(" Update mVendor set VIEW_RIGHTS = '" + _PLGRData.VIEW_RIGHTS + "'");
                            sb.AppendLine(", SAVE_RIGHTS = '" + _PLGRData.SAVE_RIGHTS + "', EDIT_RIGHTS = '" + _PLGRData.EDIT_RIGHTS + "', DELETE_RIGHTS = '" + _PLGRData.DELETE_RIGHTS + "'");
                            sb.AppendLine(", DOWNLOAD_RIGHTS" + _PLGRData.DOWNLOAD_RIGHTS + "'");
                            sb.AppendLine(" WHERE GROUP_ID = '" + _PLGRData.GROUP_ID + "' AND MODULE_ID = '" + _PLGRData.MODULE_ID + "'");
                            oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iUpdateCount++;
                        }
                        break;
                    }
                    oDbmCentral.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler => Download Master :: Vendor Master No Of Rows inserted " + clsGlobal.iAddCount++);
                    ObjLog.WriteLog("Data Scheduler => Download Master :: Vendor Master No Of Rows updated" + clsGlobal.iUpdateCount++);
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
        }

        public void GetL2CUserMaster()
        {
            clsLogic oLogic = new clsLogic();
            PLmUserMaster _PLUserMaster = new PLmUserMaster();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (ds.Tables["mUserMaster"].Rows.Count > 0)
                {
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in ds.Tables["mUserMaster"].Rows)
                    {
                        sb = new StringBuilder();
                        _PLUserMaster.LOCATION_TYPE = dr["LOCATION_TYPE"].ToString().Trim();
                        _PLUserMaster.LOCATION_CODE = dr["LOCATION_CODE"].ToString().Trim();
                        _PLUserMaster.USER_ID = dr["USER_ID"].ToString().Trim();
                        _PLUserMaster.USER_NAME = dr["USER_NAME"].ToString().Trim();
                        _PLUserMaster.PASSWORD = dr["PASSWORD"].ToString().Trim();
                        _PLUserMaster.USER_EMAIL = dr["USER_EMAIL"].ToString().Trim();
                        _PLUserMaster.GROUPID = dr["GROUPID"].ToString().Trim();
                        _PLUserMaster.ACTIVE = dr["ACTIVE"].ToString().Trim();
                        _PLUserMaster.USER_TYPE = dr["USER_TYPE"].ToString().Trim();
                        _PLUserMaster.CREATED_BY = dr["CREATED_BY"].ToString().Trim();

                        sb.Append("SELECT USER_ID from mUserMaster WITH (NOLOCK) where LOCATION_CODE = '" + _PLUserMaster.LOCATION_CODE + "' AND USER_ID = '" + _PLUserMaster.USER_ID + "'");
                        DataTable dt = oDbmCentral.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("INSERT into mUserMaster(LOCATION_TYPE, LOCATION_CODE, USER_ID, USER_NAME, PASSWORD, USER_EMAIL,");
                            sb.AppendLine("GROUPID, ACTIVE, USER_TYPE, CREATED_ON, CREATED_BY)");
                            sb.AppendLine("Values ");
                            sb.AppendLine("('" + _PLUserMaster.LOCATION_TYPE + "','" + _PLUserMaster.LOCATION_CODE + "','" + _PLUserMaster.USER_ID + "',");
                            sb.AppendLine("'" + _PLUserMaster.USER_NAME + "','" + _PLUserMaster.PASSWORD + "','" + _PLUserMaster.USER_EMAIL + "',");
                            sb.AppendLine("'" + _PLUserMaster.GROUPID + "','" + _PLUserMaster.ACTIVE + "','" + _PLUserMaster.USER_TYPE + "',");
                            sb.AppendLine("', GetDate(), '" + _PLUserMaster.CREATED_BY + "')");
                            oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                        }
                        else
                        {
                            sb = new StringBuilder();
                            sb.AppendLine(" UPDATE mUserMaster set LOCATION_TYPE = '" + _PLUserMaster.LOCATION_TYPE + "', USER_NAME = '" + _PLUserMaster.USER_NAME + "'");
                            sb.AppendLine(", PASSWORD = '" + _PLUserMaster.PASSWORD + "', USER_EMAIL = '" + _PLUserMaster.USER_EMAIL + "', GROUPID = '" + _PLUserMaster.GROUPID + "'");
                            sb.AppendLine(", ACTIVE = '" + _PLUserMaster.ACTIVE + "', USER_TYPE = '" + _PLUserMaster.USER_TYPE + "'");
                            sb.AppendLine(", CREATED_BY = '" + _PLUserMaster.CREATED_BY + "', CREATED_ON = GETDATE(),");
                            sb.AppendLine(" WHERE LOCATION_CODE = '" + _PLUserMaster.LOCATION_CODE + "' AND USER_ID = '" + _PLUserMaster.USER_ID + "'");
                            oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iUpdateCount++;
                        }
                        break;
                    }
                    oDbmCentral.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler => Download mUserMaster :: User Master No Of Rows inserted " + clsGlobal.iAddCount++);
                    ObjLog.WriteLog("Data Scheduler => Download mUserMaster :: User Master No Of Rows updated" + clsGlobal.iUpdateCount++);
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
        }

        //
        #endregion


        #region Central To Local

        public void GetC2LWebAPIMastersData()
        {
            clsLogic oLogic = new clsLogic();
            try
            {
                ds = WEBAPICentralToLocalMastersData();
                if (ds.Tables.Contains("mMaterialMaster"))
                {
                    GetC2LMaterialMaster();
                }
                //if (ds.Tables.Contains("mUserMaster"))
                //{
                //    GetC2LUserMaster();
                //}
                //if (ds.Tables.Contains("mGroupRights"))
                //{
                //    GetC2LGroupRightsData();
                //}
                //if (ds.Tables.Contains("mVendorMaster"))
                //{
                //    GetC2LVendorMaster();
                //}
                //if (ds.Tables.Contains("mEmailConfigMaster"))
                //{
                //    GetC2LEmailConfigMaster();
                //}
            }
            catch (Exception ex)
            {
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Error in Downloading Central To Local Masters data :: GetC2LWebAPITrasactionalData is - " + ex.ToString() + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                //throw ex;
            }
            finally
            {
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public void GetC2LWebAPITrasactionalData()
        {
            try
            {
                ds = WEBAPICentralToLocalTrasactionalData();
                if (Properties.Settings.Default.LocationType == "PLANT")
                {
                    if (ds.Tables.Contains("tSAPDeliveryOrderData"))
                    {
                        GetC2LtSAPDeliveryOrderData();
                    }
                    if (ds.Tables.Contains("tSAPSalesReturnData"))
                    {
                        GetC2LtSAPSalesReturnData();
                    }
                    if (ds.Tables.Contains("tSAPPurchaseOrderData"))
                    {
                        GetC2LtSAPPurchaseOrderData();
                    }
                    if (ds.Tables.Contains("tSAPReturnPurchaseOrderData"))
                    {
                        GetC2LtSAPReturnPurchaseOrderData();
                    }
                }
                if (Properties.Settings.Default.LocationType == "HUB")
                {
                    if (ds.Tables.Contains("tSAPPurchaseOrderData"))
                    {
                        GetC2LtSAPPurchaseOrderData();
                    }
                    if (ds.Tables.Contains("tSAPDeliveryOrderData"))
                    {
                        GetC2LtSAPDeliveryOrderData();
                    }
                    if (ds.Tables.Contains("tSAPSalesReturnData"))
                    {
                        GetC2LtSAPSalesReturnData();
                    }
                    if (ds.Tables.Contains("tSAPReturnPurchaseOrderData"))
                    {
                        GetC2LtSAPReturnPurchaseOrderData();
                    }
                    if (ds.Tables.Contains("tSAPQAData"))
                    {
                        GetC2LtSAPQualityInspectionData();
                    }
                }
                if (Properties.Settings.Default.LocationType == "VENDOR")
                {
                    if (ds.Tables.Contains("tSAPPurchaseOrderData"))
                    {
                        GetC2LtSAPPurchaseOrderDataForVendor();
                    }
                    if (ds.Tables.Contains("tVendorLabelGenerating"))
                    {
                        GetC2LVendorLabelGeneratingDetails();
                    }
                }
            }
            catch (Exception ex)
            {
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Error in Downloading Central To Local Transactional Data :: GetC2LWebAPITrasactionalData is - " + ex.ToString() + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                //throw ex;
            }
            finally
            {
                if (oDbmLocal.Connection == null)
                {

                }
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public DataSet WEBAPICentralToLocalMastersData()
        {
            string strRespone = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Properties.Settings.Default.APICentralToLocalAddressMasters);

            string UserName = Properties.Settings.Default.APICentralToLocalUserID;
            string Password = Properties.Settings.Default.APICentralToLocalPassword;
            //request.Method = HttpMethod.ToString();
            request.Method = "GET";
            //request.Method = "GETC2LMASTERSDATA";
            string authHeader = System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(UserName + ":" + Password));
            request.Headers.Add("Authorization", "Basic" + " " + authHeader);
            using (HttpWebResponse respone = (HttpWebResponse)request.GetResponse())
            {
                if (respone.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception("Error Code:" + respone.StatusCode.ToString());
                }
                using (System.IO.Stream responestream = respone.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(responestream))
                    {
                        strRespone = reader.ReadToEnd();
                    }
                }
            }
            if (strRespone != string.Empty)
            {
                StringReader theReader = new StringReader(strRespone);
                theDataSet = new DataSet();
                theDataSet.ReadXml(theReader);
            }
            return theDataSet;
        }

        public DataSet WEBAPICentralToLocalTrasactionalData()
        {
            try
            {
                string strRespone = string.Empty;
                string sPOLocType = string.Empty;
                string sLocType = string.Empty;
                if (Properties.Settings.Default.LocationType == "HUB")
                {
                    sPOLocType = "H";
                    sLocType = "HUB";
                }
                else if (Properties.Settings.Default.LocationType.ToUpper() == "VENDOR")
                {
                    sPOLocType = "V";
                    sLocType = "VENDOR";
                }
                else if (Properties.Settings.Default.LocationType.ToUpper() == "PLANT")
                {
                    sPOLocType = "P";
                    sLocType = "PLANT";
                }
                string data = "?LocationCode=" + Properties.Settings.Default.LocationCode + "&LocationType=" + sLocType.Trim().ToString() + "&POLocType=" + sPOLocType.Trim().ToString() + "";
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Properties.Settings.Default.APICentralToLocalAddressTransaction + data);
                ObjLog.WriteLog("Data Scheduler : CentralToLocal WebApi Address - " + Properties.Settings.Default.APICentralToLocalAddressTransaction + data);
                string UserName = Properties.Settings.Default.APICentralToLocalUserID;
                string Password = Properties.Settings.Default.APICentralToLocalPassword;
                request.Method = "GET";
                string authHeader = System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(UserName + ":" + Password));
                request.Headers.Add("Authorization", "Basic" + " " + authHeader);
                request.KeepAlive = true;
                request.UseDefaultCredentials = true;
                request.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;

                using (HttpWebResponse respone = (HttpWebResponse)request.GetResponse())
                {
                    if (respone.StatusCode != HttpStatusCode.OK)
                    {
                        throw new Exception("Error Code:" + respone.StatusCode.ToString());
                    }
                    using (System.IO.Stream responestream = respone.GetResponseStream())
                    {
                        ObjLog.WriteLog("Data Scheduler : CentralToLocal : API Responce -- " + responestream);
                        using (StreamReader reader = new StreamReader(responestream))
                        {
                            strRespone = reader.ReadToEnd();
                        }
                    }
                }
                if (strRespone != string.Empty)
                {
                    StringReader theReader = new StringReader(strRespone);
                    theDataSet = new DataSet();
                    theDataSet.ReadXml(theReader);
                }
                return theDataSet;
            }
            catch (WebException webex)
            {
                WebResponse errResp = webex.Response;
                using (Stream respStream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(respStream);
                    string text = reader.ReadToEnd();
                }
                ObjLog.WriteLog("Data Scheduler : CentralToLocal WebApi Error found - " + webex.Message.ToString());
                return theDataSet;
            }
        }

        public void GetC2LMaterialMaster()
        {
            clsLogic oLogic = new clsLogic();
            PLmMaterialMaster _PLMaterialMaster = new PLmMaterialMaster();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (ds.Tables["mMaterialMaster"].Rows.Count > 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal MaterialMaster Data : No. of Records Found - " + ds.Tables["mMaterialMaster"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    foreach (DataRow dr in ds.Tables["mMaterialMaster"].Rows)
                    {
                        sb = new StringBuilder();
                        string sMatType1 = Properties.Settings.Default.MaterialProductType1.Trim();
                        string sMatType2 = Properties.Settings.Default.MaterialProductType2.Trim();
                        string sMatType3 = Properties.Settings.Default.MaterialProductType3.Trim();
                        string sMatType4 = Properties.Settings.Default.MaterialProductType4.Trim();
                        string sRowMatType = dr["Product"].ToString().Trim();
                        if (sMatType1 == sRowMatType || sMatType2 == sRowMatType || sMatType3 == sRowMatType || sMatType4 == sRowMatType)
                        {
                            _PLMaterialMaster.Product = dr["Product"].ToString().Trim();
                            _PLMaterialMaster.MatCode = dr["MatCode"].ToString().Trim();
                            _PLMaterialMaster.MatDescription = dr["MatDescription"].ToString().Trim();
                            _PLMaterialMaster.Thickness = dr["Thickness"].ToString().Trim();
                            _PLMaterialMaster.ThicknessDescription = dr["ThicknessDescription"].ToString().Trim();
                            _PLMaterialMaster.Size = dr["Size"].ToString().Trim();
                            _PLMaterialMaster.Grade = dr["Grade"].ToString().Trim(); ;
                            _PLMaterialMaster.GradeDescription = dr["GradeDescription"].ToString().Trim();
                            _PLMaterialMaster.Category = dr["Category"].ToString().Trim();
                            _PLMaterialMaster.CategoryDescription = dr["CategoryDescription"].ToString().Trim();
                            _PLMaterialMaster.MatGroup = dr["MatGroup"].ToString().Trim();
                            _PLMaterialMaster.MatGroupDescription = dr["MatGroupDescription"].ToString().Trim();
                            _PLMaterialMaster.DesignNo = dr["DesignNo"].ToString().Trim();
                            _PLMaterialMaster.DesignDescription = dr["DesignDescription"].ToString().Trim();
                            _PLMaterialMaster.FinishCode = dr["FinishCode"].ToString().Trim();
                            _PLMaterialMaster.FinishDescription = dr["FinishDescription"].ToString().Trim();
                            _PLMaterialMaster.VisionPanelCode = dr["VisionPanelCode"].ToString().Trim();
                            _PLMaterialMaster.VisionPanelDescription = dr["VisionPanelDescription"].ToString().Trim();
                            _PLMaterialMaster.LippingCode = dr["LippingCode"].ToString().Trim();
                            _PLMaterialMaster.LippingDescription = dr["LippingDescription"].ToString().Trim();
                            _PLMaterialMaster.UOM = dr["UOM"].ToString().Trim();
                            _PLMaterialMaster.DownloadBy = "Scheduler";

                            sb.Append("select MatCode from mMaterialMaster WITH (NOLOCK) where MatCode = '" + _PLMaterialMaster.MatCode + "' and Product = '" + _PLMaterialMaster.Product + "' ");
                            DataTable dt = oDbmLocal.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                            if (dt.Rows.Count == 0)
                            {
                                sb = new StringBuilder();
                                sb.AppendLine("Insert into mMaterialMaster(Product, MatCode, MatDescription, Thickness,");
                                sb.AppendLine("ThicknessDescription, Size, Grade, GradeDescription,");
                                sb.AppendLine("Category, CategoryDescription, MatGroup, MatGroupDescription,");
                                sb.AppendLine("DesignNo, DesignDescription, FinishCode, FinishDescription,");
                                sb.AppendLine("VisionPanelCode, VisionPanelDescription, LippingCode, LippingDescription,");
                                sb.AppendLine("UOM, DownloadOn, DownloadBy)");
                                sb.AppendLine("Values ");
                                sb.AppendLine("('" + _PLMaterialMaster.Product + "','" + _PLMaterialMaster.MatCode + "','" + _PLMaterialMaster.MatDescription + "','" + _PLMaterialMaster.Thickness + " ',");
                                sb.AppendLine("'" + _PLMaterialMaster.ThicknessDescription + "','" + _PLMaterialMaster.Size + "','" + _PLMaterialMaster.Grade + "','" + _PLMaterialMaster.GradeDescription + " ',");
                                sb.AppendLine("'" + _PLMaterialMaster.Category + "','" + _PLMaterialMaster.CategoryDescription + "','" + _PLMaterialMaster.MatGroup + "','" + _PLMaterialMaster.MatGroupDescription + " ',");
                                sb.AppendLine("'" + _PLMaterialMaster.DesignNo + "','" + _PLMaterialMaster.DesignDescription + "','" + _PLMaterialMaster.FinishCode + "','" + _PLMaterialMaster.FinishDescription + " ',");
                                sb.AppendLine("'" + _PLMaterialMaster.VisionPanelCode + "','" + _PLMaterialMaster.VisionPanelDescription + "','" + _PLMaterialMaster.LippingCode + "','" + _PLMaterialMaster.LippingDescription + " ',");
                                sb.AppendLine("'" + _PLMaterialMaster.UOM + "',");
                                sb.AppendLine(" GETDATE(), '" + _PLMaterialMaster.DownloadBy + "')");
                                oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                                clsGlobal.iAddCount++;
                            }
                            else
                            {
                                sb = new StringBuilder();
                                sb.AppendLine("Update mMaterialMaster set Product = '" + _PLMaterialMaster.Product + "'");
                                sb.AppendLine(", MatDescription = '" + _PLMaterialMaster.MatDescription + "', Thickness = '" + _PLMaterialMaster.Thickness + "'");
                                sb.AppendLine(", ThicknessDescription = '" + _PLMaterialMaster.ThicknessDescription + "', Size = '" + _PLMaterialMaster.Size + "', Grade = '" + _PLMaterialMaster.Grade + "'");
                                sb.AppendLine(", GradeDescription = '" + _PLMaterialMaster.GradeDescription + "', Category = '" + _PLMaterialMaster.Category + "', CategoryDescription = '" + _PLMaterialMaster.CategoryDescription + "'");
                                sb.AppendLine(", MatGroup = '" + _PLMaterialMaster.MatGroup + "', MatGroupDescription = '" + _PLMaterialMaster.MatGroupDescription + "', DesignNo = '" + _PLMaterialMaster.DesignNo + "'");
                                sb.AppendLine(", DesignDescription = '" + _PLMaterialMaster.DesignDescription + "', FinishCode = '" + _PLMaterialMaster.FinishCode + "', FinishDescription = '" + _PLMaterialMaster.FinishDescription + "'");
                                sb.AppendLine(", VisionPanelCode = '" + _PLMaterialMaster.VisionPanelCode + "', VisionPanelDescription = '" + _PLMaterialMaster.VisionPanelDescription + "', LippingCode = '" + _PLMaterialMaster.LippingCode + "'");
                                sb.AppendLine(", LippingDescription = '" + _PLMaterialMaster.LippingDescription + "', UOM = '" + _PLMaterialMaster.UOM + "'");
                                sb.AppendLine(", DownloadBy = '" + _PLMaterialMaster.DownloadBy + "', DownloadOn = GETDATE() where MatCode = '" + _PLMaterialMaster.MatCode + "'");
                                oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                                clsGlobal.iUpdateCount++;
                            }
                        }
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal MaterialMaster Data : No. of Records Inserted - " + clsGlobal.iAddCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal MaterialMaster Data : No. of Records Updated - " + clsGlobal.iUpdateCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                }
                else if (ds.Tables["mMaterialMaster"].Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal MaterialMaster Data : " + "There is no records found" + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    clsGlobal.lmsg = "Saving CentralToLocal MaterialMaster Data : " + "There is no records found" + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                }
            }
            catch (Exception ex)
            {
                ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal MaterialMaster Data :: Error is - " + ex.Message.ToString());
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Receiving of Material Master Data for LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " from Central Server found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                //}
                oDbmLocal.RollBackTransaction();

            }
            finally
            {
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public void GetC2LEmailConfigMaster()
        {
            clsLogic oLogic = new clsLogic();
            PLmEmailConfigMaster _PLEmailConfig = new PLmEmailConfigMaster();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (ds.Tables["mEmailConfigMaster"].Rows.Count > 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal EmailConfigMaster Data : No. of Records Found - " + ds.Tables["mEmailConfigMaster"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    foreach (DataRow dr in ds.Tables["mEmailConfigMaster"].Rows)
                    {
                        sb = new StringBuilder();
                        _PLEmailConfig.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLEmailConfig.SMTPHost = dr["SMTPHost"].ToString().Trim();
                        _PLEmailConfig.EmailID = dr["EmailID"].ToString().Trim();
                        _PLEmailConfig.Name = dr["Name"].ToString().Trim();
                        _PLEmailConfig.Password = dr["Password"].ToString().Trim();
                        _PLEmailConfig.PortNo = dr["PortNo"].ToString().Trim();
                        _PLEmailConfig.Subject = dr["Subject"].ToString().Trim();
                        _PLEmailConfig.CreatedBy = dr["CreatedBy"].ToString().Trim();
                        _PLEmailConfig.CreatedOn = dr["CreatedOn"].ToString().Trim();


                        sb.Append("select EmailId from mEmailConfigMaster WITH (NOLOCK) where LocationCode = '" + _PLEmailConfig.LocationCode + "' AND EmailId = '" + _PLEmailConfig.EmailID + "'");
                        DataTable dt = oDbmLocal.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Insert into mEmailConfigMaster(LocationCode, SMTPHost, EmailID, Name, Password, PortNo,");
                            sb.AppendLine(" Subject, CreatedBy, CreatedOn)");
                            sb.AppendLine("Values ");
                            sb.AppendLine("('" + _PLEmailConfig.LocationCode + "','" + _PLEmailConfig.SMTPHost + "','" + _PLEmailConfig.EmailID + "','" + _PLEmailConfig.Name + "',");
                            sb.AppendLine("'" + _PLEmailConfig.Password + "',");
                            sb.AppendLine("'" + _PLEmailConfig.PortNo + "','" + _PLEmailConfig.Subject + "','" + _PLEmailConfig.CreatedBy + "'");
                            sb.AppendLine("'" + _PLEmailConfig.CreatedOn + "')");
                            oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                        }
                        else
                        {
                            sb = new StringBuilder();
                            sb.AppendLine(" Update mEmailConfigMaster set SMTPHost = '" + _PLEmailConfig.SMTPHost + "', Name = '" + _PLEmailConfig.Name + "'");
                            sb.AppendLine(", Password = '" + _PLEmailConfig.Password + "', PortNo = '" + _PLEmailConfig.PortNo + "'");
                            sb.AppendLine(", Subject = '" + _PLEmailConfig.Subject + "', CreatedBy = '" + _PLEmailConfig.CreatedBy + "'");
                            sb.AppendLine(", CreatedOn = '" + _PLEmailConfig.CreatedOn + "'");
                            sb.AppendLine(" WHERE LocationCode = '" + _PLEmailConfig.LocationCode + "' AND EmailID = '" + _PLEmailConfig.EmailID + "'");
                            oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iUpdateCount++;
                        }
                        break;
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal EmailConfigMaster Data : No. of Records Inserted - " + clsGlobal.iAddCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal EmailConfigMaster Data : No. of Records Updated - " + clsGlobal.iUpdateCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                }
                else if (ds.Tables["mEmailConfigMaster"].Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal EmailConfigMaster Data : " + "There is no records found" + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    clsGlobal.lmsg = "Saving CentralToLocal EmailConfigMaster Data : " + "There is no records found" + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                }
            }
            catch (Exception ex)
            {
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal EmailConfigMaster Data :: Error is - " + ex.Message.ToString() + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
            }
            finally
            {
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public void GetC2LUserMaster()
        {
            clsLogic oLogic = new clsLogic();
            PLmUserMaster _PLUserMaster = new PLmUserMaster();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (ds.Tables["mUserMaster"].Rows.Count > 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal UserMaster Data : No. of Records Found - " + ds.Tables["UserMaster"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    foreach (DataRow dr in ds.Tables["mUserMaster"].Rows)
                    {
                        sb = new StringBuilder();
                        if (Properties.Settings.Default.LocationCode == dr["LOCATION_CODE"].ToString().Trim())
                        {
                            _PLUserMaster.LOCATION_TYPE = dr["LOCATION_TYPE"].ToString().Trim();
                            _PLUserMaster.LOCATION_CODE = dr["LOCATION_CODE"].ToString().Trim();
                            _PLUserMaster.USER_ID = dr["USER_ID"].ToString().Trim();
                            _PLUserMaster.USER_NAME = dr["USER_NAME"].ToString().Trim();
                            _PLUserMaster.PASSWORD = dr["PASSWORD"].ToString().Trim();
                            _PLUserMaster.USER_EMAIL = dr["USER_EMAIL"].ToString().Trim();
                            _PLUserMaster.GROUPID = dr["GROUPID"].ToString().Trim();
                            _PLUserMaster.ACTIVE = dr["ACTIVE"].ToString().Trim();
                            _PLUserMaster.USER_TYPE = dr["USER_TYPE"].ToString().Trim();
                            if (dr["CREATED_BY"].ToString().Trim() == string.Empty || dr["CREATED_BY"].ToString().Trim() == null)
                                _PLUserMaster.CREATED_BY = "";
                            else
                                _PLUserMaster.CREATED_BY = dr["CREATED_BY"].ToString().Trim();
                            if (dr["MODIFIED_BY"].ToString().Trim() == string.Empty || dr["MODIFIED_BY"].ToString().Trim() == null)
                                _PLUserMaster.MODIFIED_BY = "";
                            else
                                _PLUserMaster.MODIFIED_BY = dr["MODIFIED_BY"].ToString().Trim();
                            _PLUserMaster.CREATED_ON = dr["CREATED_ON"].ToString().Trim();
                            _PLUserMaster.MODIFIED_ON = dr["MODIFIED_ON"].ToString().Trim();

                            sb.Append("select USER_ID from mUserMaster WITH (NOLOCK) where USER_ID = '" + _PLUserMaster.USER_ID + "' AND LOCATION_CODE = '" + _PLUserMaster.LOCATION_CODE + "'");
                            DataTable dt = oDbmLocal.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                            if (dt.Rows.Count == 0)
                            {
                                sb = new StringBuilder();
                                sb.AppendLine("Insert into mUserMaster(LOCATION_TYPE, LOCATION_CODE, USER_ID, USER_NAME, PASSWORD, USER_EMAIL,");
                                sb.AppendLine("GROUPID, ACTIVE, USER_TYPE, CREATED_ON, CREATED_BY, MODIFIED_BY, MODIFIED_ON)");
                                sb.AppendLine("Values ");
                                sb.AppendLine("('" + _PLUserMaster.LOCATION_TYPE + "','" + _PLUserMaster.LOCATION_CODE + "','" + _PLUserMaster.USER_ID + "',");
                                sb.AppendLine("'" + _PLUserMaster.USER_NAME + "','" + _PLUserMaster.PASSWORD + "','" + _PLUserMaster.USER_EMAIL + "',");
                                sb.AppendLine("'" + _PLUserMaster.GROUPID + "','" + _PLUserMaster.ACTIVE + "','" + _PLUserMaster.USER_TYPE + "',");
                                sb.AppendLine("'" + _PLUserMaster.CREATED_ON + "','" + _PLUserMaster.CREATED_BY + "','" + _PLUserMaster.MODIFIED_BY + "','" + _PLUserMaster.MODIFIED_ON + "')");
                                oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                                clsGlobal.iAddCount++;
                            }
                            else
                            {
                                sb = new StringBuilder();
                                sb.AppendLine(" Update mUserMaster set LOCATION_TYPE = '" + _PLUserMaster.LOCATION_TYPE + "', USER_NAME = '" + _PLUserMaster.USER_NAME + "'");
                                sb.AppendLine(", PASSWORD = '" + _PLUserMaster.PASSWORD + "', USER_EMAIL = '" + _PLUserMaster.USER_EMAIL + "', GROUPID = '" + _PLUserMaster.GROUPID + "'");
                                sb.AppendLine(", ACTIVE = '" + _PLUserMaster.ACTIVE + "', USER_TYPE = '" + _PLUserMaster.USER_TYPE + "'");
                                sb.AppendLine(", CREATED_BY = '" + _PLUserMaster.CREATED_BY + "', CREATED_ON = '" + _PLUserMaster.CREATED_ON + "'");
                                sb.AppendLine(", MODIFIED_BY = '" + _PLUserMaster.MODIFIED_BY + "', MODIFIED_ON = '" + _PLUserMaster.MODIFIED_ON + "'");
                                sb.AppendLine(" WHERE LOCATION_CODE = '" + _PLUserMaster.LOCATION_CODE + "' AND USER_ID = '" + _PLUserMaster.USER_ID + "'");
                                oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                                clsGlobal.iUpdateCount++;
                            }
                        }
                        //break;
                        //}
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal UserMaster Data : No. of Records Inserted - " + clsGlobal.iAddCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal UserMaster Data : No. of Records Updated - " + clsGlobal.iUpdateCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                }
                else if (ds.Tables["mUserMaster"].Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal UserMaster Data : " + "There is no records found" + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    clsGlobal.lmsg = "Saving CentralToLocal UserMaster Data : " + "There is no records found" + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                }
            }
            catch (Exception ex)
            {
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal UserMaster Data :: Error is - " + ex.Message.ToString() + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
            }
            finally
            {
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public void GetC2LVendorMaster()
        {
            clsLogic oLogic = new clsLogic();
            PLmVendorMaster _PLVendorMaster = new PLmVendorMaster();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (ds.Tables["mVendorMaster"].Rows.Count > 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal VendorMaster Data : No. of Records Found - " + ds.Tables["mVendorMaster"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    foreach (DataRow dr in ds.Tables["mVendorMaster"].Rows)
                    {
                        sb = new StringBuilder();
                        _PLVendorMaster.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLVendorMaster.VendorCode = dr["VendorCode"].ToString().Trim();
                        _PLVendorMaster.VendorDesc = dr["VendorDesc"].ToString().Trim();
                        _PLVendorMaster.VendorEmail = dr["VendorEmail"].ToString().Trim();
                        _PLVendorMaster.VendorAddress = dr["VendorAddress"].ToString().Trim();
                        if (dr["CreatedBy"].ToString().Trim() == string.Empty || dr["CreatedBy"].ToString().Trim() == null)
                            _PLVendorMaster.CreatedBy = "";
                        else
                            _PLVendorMaster.CreatedBy = dr["CreatedBy"].ToString().Trim();
                        if (dr["ModifiedBy"].ToString().Trim() == string.Empty || dr["ModifiedBy"].ToString().Trim() == null)
                            _PLVendorMaster.ModifiedBy = "";
                        else
                            _PLVendorMaster.ModifiedBy = dr["ModifiedBy"].ToString().Trim();
                        _PLVendorMaster.CreatedOn = dr["CreatedOn"].ToString().Trim();
                        _PLVendorMaster.ModifiedOn = dr["ModifiedOn"].ToString().Trim();

                        sb.Append("select SAP_VENDOR_CODE from mVendorMaster WITH (NOLOCK) where SAP_VENDOR_CODE = '" + _PLVendorMaster.VendorCode + "' ");
                        DataTable dt = oDbmLocal.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Insert into mVendorMaster(LocationCode, VendorCode, VendorDesc,");
                            sb.AppendLine("VendorEmail, VendorAddress, CreatedOn, CreatedBy )");
                            sb.AppendLine("Values ");
                            sb.AppendLine("('" + _PLVendorMaster.LocationCode + "','" + _PLVendorMaster.VendorCode + "','" + _PLVendorMaster.VendorDesc + "',");
                            sb.AppendLine("'" + _PLVendorMaster.VendorEmail + "','" + _PLVendorMaster.VendorAddress + "',");
                            sb.AppendLine("'" + _PLVendorMaster.CreatedOn + "','" + _PLVendorMaster.CreatedBy + "')");
                            oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                        }
                        else
                        {
                            sb = new StringBuilder();
                            sb.AppendLine(" Update mVendorMaster set VendorDesc = '" + _PLVendorMaster.VendorDesc + "', VendorEmail = '" + _PLVendorMaster.VendorEmail + "'");
                            sb.AppendLine(", VendorAddress = '" + _PLVendorMaster.VendorAddress + "'");
                            sb.AppendLine(", CreatedOn = '" + _PLVendorMaster.CreatedOn + "'");
                            sb.AppendLine(" CreatedBy = '" + _PLVendorMaster.CreatedBy + "' WHERE VendorCode = '" + _PLVendorMaster.VendorCode + "'");
                            oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iUpdateCount++;
                        }
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal VendorMaster Data : No. of Records Inserted - " + clsGlobal.iAddCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal VendorMaster Data : No. of Records Updated - " + clsGlobal.iUpdateCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                }
                else if (ds.Tables["mVendorMaster"].Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal VendorMaster Data : " + "There is no records found" + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    clsGlobal.lmsg = "Saving CentralToLocal VendorMaster Data : " + "There is no records found" + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Receiving Vendor Master Data for LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " from Central Server found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                //}
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal VendorMaster Data :: Error is - " + ex.Message.ToString() + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
            }
            finally
            {
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public void GetC2LGroupRightsData()
        {
            clsLogic oLogic = new clsLogic();
            PLGroupRights _PLGRData = new PLGroupRights();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (ds.Tables["Vendor"].Rows.Count > 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal GroupRights Data : No. of Records Found - " + ds.Tables["mVendorMaster"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    foreach (DataRow dr in ds.Tables["Vendor"].Rows)
                    {
                        sb = new StringBuilder();
                        _PLGRData.GROUP_ID = dr["GROUP_ID"].ToString().Trim();
                        _PLGRData.MODULE_ID = dr["MODULE_ID"].ToString().Trim();
                        _PLGRData.VIEW_RIGHTS = dr["VIEW_RIGHTS"].ToString().Trim();
                        _PLGRData.SAVE_RIGHTS = dr["SAVE_RIGHTS"].ToString().Trim();
                        _PLGRData.EDIT_RIGHTS = dr["EDIT_RIGHTS"].ToString().Trim();
                        _PLGRData.DELETE_RIGHTS = dr["DELETE_RIGHTS"].ToString().Trim();
                        _PLGRData.DOWNLOAD_RIGHTS = dr["DOWNLOAD_RIGHTS"].ToString().Trim();
                        _PLGRData.lDownloadedBy = "Scheduler";

                        sb.Append("select VIEW_RIGHTS from mGroupRights WITH (NOLOCK) where GROUP_ID = '" + _PLGRData.GROUP_ID + "' AND MODULE_ID = '" + _PLGRData.MODULE_ID + "'");
                        DataTable dt = oDbmLocal.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Insert into mGroupRights(GROUP_ID, MODULE_ID, VIEW_RIGHTS, SAVE_RIGHTS,");
                            sb.AppendLine(" EDIT_RIGHTS, DELETE_RIGHTS, DOWNLOAD_RIGHTS)");
                            sb.AppendLine("Values ");
                            sb.AppendLine("('" + _PLGRData.GROUP_ID + "','" + _PLGRData.MODULE_ID + "','" + _PLGRData.VIEW_RIGHTS + "','" + _PLGRData.SAVE_RIGHTS + "',");
                            sb.AppendLine("'" + _PLGRData.EDIT_RIGHTS + "','" + _PLGRData.DELETE_RIGHTS + "',");
                            sb.AppendLine("'" + _PLGRData.DOWNLOAD_RIGHTS + "')");
                            oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                        }
                        else
                        {
                            sb = new StringBuilder();
                            sb.AppendLine(" Update mGroupRights set VIEW_RIGHTS = '" + _PLGRData.VIEW_RIGHTS + "'");
                            sb.AppendLine(", SAVE_RIGHTS = '" + _PLGRData.SAVE_RIGHTS + "', EDIT_RIGHTS = '" + _PLGRData.EDIT_RIGHTS + "', DELETE_RIGHTS = '" + _PLGRData.DELETE_RIGHTS + "'");
                            sb.AppendLine(", DOWNLOAD_RIGHTS" + _PLGRData.DOWNLOAD_RIGHTS + "'");
                            sb.AppendLine(" WHERE GROUP_ID = '" + _PLGRData.GROUP_ID + "' AND MODULE_ID = '" + _PLGRData.MODULE_ID + "'");
                            oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iUpdateCount++;
                        }
                        break;
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal GroupRights Data : No. of Records Inserted - " + clsGlobal.iAddCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal GroupRights Data : No. of Records Updated - " + clsGlobal.iUpdateCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                }
                else if (ds.Tables["mGroupRights"].Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal GroupRights Data : " + "There is no records found" + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    clsGlobal.lmsg = "Saving CentralToLocal GroupRights Data : " + "There is no records found" + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                }
            }
            catch (Exception ex)
            {
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal GroupRights Data :: Error is - " + ex.Message.ToString() + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
            }
            finally
            {
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }


        public void GetC2LtSAPDeliveryOrderData()
        {
            clsLogic oLogic = new clsLogic();
            PLSAPDeliveryOrderData _PLDOData = new PLSAPDeliveryOrderData();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;
            sb = new StringBuilder();

            try
            {
                oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (ds.Tables["tSAPDeliveryOrderData"].Rows.Count > 0)
                {
                    clsGlobal.iAddCount = 0;
                    clsGlobal.iUpdateCount = 0;
                    ObjLog.WriteLog("Data Scheduler : CentralToLocal SAPDeliveryOrder Data : Total No. of Records Found - " + ds.Tables["tSAPDeliveryOrderData"].Rows.Count);
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    foreach (DataRow dr in ds.Tables["tSAPDeliveryOrderData"].Rows)
                    {
                        sb = new StringBuilder();
                        if (dr["LocationCode"].ToString().Trim() == clsGlobal.PlantCode.Trim().ToString())
                        {
                            _PLDOData.LocationCode = dr["LocationCode"].ToString().Trim();
                            _PLDOData.DeliveryOrderNo = dr["DeliveryOrderNo"].ToString().Trim();
                            _PLDOData.CustomerCode = dr["CustomerCode"].ToString().Trim();
                            _PLDOData.CustomerName = dr["CustomerName"].ToString().Trim();
                            _PLDOData.MatCode = dr["MatCode"].ToString().Trim();
                            _PLDOData.MatDesc = dr["MatDesc"].ToString().Trim();
                            _PLDOData.ToLocationCode = dr["ToLocationCode"].ToString().Trim();
                            _PLDOData.DOQty = Convert.ToInt32(dr["DOQty"].ToString().Trim());
                            _PLDOData.DODate = dr["DODate"].ToString().Trim();
                            _PLDOData.DOStatus = dr["DOStatus"].ToString().Trim();
                            _PLDOData.DOCancelStatus = dr["DOCancelStatus"].ToString().Trim();
                            _PLDOData.DownloadOn = dr["DownloadOn"].ToString().Trim();
                            _PLDOData.DownloadBy = dr["DownloadBy"].ToString().Trim();

                            sb.Append("Select MatCode from tSAPDeliveryOrderData WITH (NOLOCK) WHERE LocationCode = '" + _PLDOData.LocationCode + "' AND DeliveryOrderNo = '" + _PLDOData.DeliveryOrderNo + "'");
                            sb.Append(" AND CustomerCode = '" + _PLDOData.CustomerCode + "' AND MatCode = '" + _PLDOData.MatCode + "'");
                            DataTable dt = oDbmLocal.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                            if (dt.Rows.Count == 0)
                            {
                                sb = new StringBuilder();
                                sb.AppendLine("Insert into tSAPDeliveryOrderData(LocationCode, DeliveryOrderNo, CustomerCode, CustomerName, MatCode, MatDesc,");
                                sb.AppendLine("ToLocationCode, DOQty, ScannedQty, IsSAPPosted, DODate, DOStatus, DOCancelStatus, DownloadOn, DownloadBy )");  //, PostedBy, PostedOn
                                sb.AppendLine("Values ");
                                sb.AppendLine("('" + _PLDOData.LocationCode + "','" + _PLDOData.DeliveryOrderNo + "','" + _PLDOData.CustomerCode + "','" + _PLDOData.CustomerName + "',");
                                sb.AppendLine("'" + _PLDOData.MatCode + "','" + _PLDOData.MatDesc + "','" + _PLDOData.ToLocationCode + "',");
                                sb.AppendLine("'" + _PLDOData.DOQty + "', 0, 0, '" + _PLDOData.DODate + "','" + _PLDOData.DOStatus + "','" + _PLDOData.DOCancelStatus + "', GETDATE(), ");
                                sb.AppendLine("'" + _PLDOData.DownloadBy + "')");
                                oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                                clsGlobal.iAddCount++;
                            }
                            else
                            {
                                sb = new StringBuilder();
                                sb.AppendLine(" Update tSAPDeliveryOrderData set CustomerName = '" + _PLDOData.CustomerName + "', MatDesc = '" + _PLDOData.MatDesc + "'");
                                sb.AppendLine(", ToLocationCode = '" + _PLDOData.ToLocationCode + "', DOQty = '" + _PLDOData.DOQty + "', DODate = '" + _PLDOData.DODate + "'");
                                sb.AppendLine(", DOStatus = '" + _PLDOData.DOStatus + "', DOCancelStatus = '" + _PLDOData.DOCancelStatus + "', DownloadOn =  GETDATE(), DownloadBy = '" + _PLDOData.DownloadBy + "'");
                                sb.AppendLine(" WHERE LocationCode = '" + _PLDOData.LocationCode + "' AND DeliveryOrderNo = '" + _PLDOData.DeliveryOrderNo + "' AND CustomerCode = '" + _PLDOData.CustomerCode + "' AND MatCode = '" + _PLDOData.MatCode + "'");
                                oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                                clsGlobal.iUpdateCount++;
                            }
                        }
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler : CentralToLocal SAPDeliveryOrder Data : Total No. of Records Inserted - " + clsGlobal.iAddCount + " And Updated -  " + clsGlobal.iUpdateCount);
                }
                else if (ds.Tables["tSAPDeliveryOrderData"].Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler : CentralToLocal SAPDeliveryOrder Data : " + "There is no records found");
                    clsGlobal.lmsg = "CentralToLocal SAPDeliveryOrder Data : " + "There is no records found";
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Receiving Delivery Order Data for LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " from Central Server found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                //}
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPDeliveryOrder Data :: Query - " + sb.ToString());
                ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPDeliveryOrder Data :: Error is - " + ex.Message.ToString());
            }
            finally
            {
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public void GetC2LtSAPPurchaseOrderData()
        {
            clsLogic oLogic = new clsLogic();
            PLSAPPurchaseOrderData _PLPurOrder = new PLSAPPurchaseOrderData();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;
            sb = new StringBuilder();
            try
            {
                oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (ds.Tables["tSAPPurchaseOrderData"].Rows.Count > 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPPurchaseOrder Data : No. of Records Found - " + ds.Tables["tSAPPurchaseOrderData"].Rows.Count);
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    foreach (DataRow dr in ds.Tables["tSAPPurchaseOrderData"].Rows)
                    {
                        sb = new StringBuilder();
                        _PLPurOrder.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLPurOrder.PONumber = dr["PONumber"].ToString().Trim();
                        _PLPurOrder.VendorCode = dr["VendorCode"].ToString().Trim();
                        _PLPurOrder.VendorName = dr["VendorName"].ToString().Trim();
                        _PLPurOrder.MatCode = dr["MatCode"].ToString().Trim();
                        _PLPurOrder.MatDescription = dr["MatDescription"].ToString().Trim();
                        _PLPurOrder.POQty = Convert.ToInt32(dr["POQty"].ToString().Trim());
                        _PLPurOrder.POLocType = Convert.ToString(dr["POLocType"].ToString().Trim());
                        _PLPurOrder.PODate = dr["PODate"].ToString().Trim();
                        _PLPurOrder.DownloadBy = "DataScheduler";

                        sb.Append("Select MatCode from tSAPPurchaseOrderData WITH (NOLOCK) WHERE LocationCode = '" + _PLPurOrder.LocationCode + "' AND PONumber = '" + _PLPurOrder.PONumber + "'");
                        sb.Append(" AND VendorCode = '" + _PLPurOrder.VendorCode + "' AND MatCode = '" + _PLPurOrder.MatCode + "'");
                        DataTable dt = oDbmLocal.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Insert into tSAPPurchaseOrderData(LocationCode, PONumber, VendorCode, VendorName, MatCode, MatDescription, POQty, POLocType, ");
                            sb.AppendLine("PODate, POStatus, IsQRCodeGenerated, GeneratedQty, IsPrinted, PrintedQty,  IsReprinted, DownloadOn, DownloadBy)");
                            sb.AppendLine("Values ");
                            sb.AppendLine("('" + _PLPurOrder.LocationCode + "','" + _PLPurOrder.PONumber + "','" + _PLPurOrder.VendorCode + "','" + _PLPurOrder.VendorName + "',");
                            sb.AppendLine("'" + _PLPurOrder.MatCode + "','" + _PLPurOrder.MatDescription + "','" + _PLPurOrder.POQty + "',");
                            sb.AppendLine("'" + _PLPurOrder.POLocType + "','" + _PLPurOrder.PODate + "',");
                            sb.AppendLine("'N', 0, 0, 0, 0, 0,  GetDate(), '" + _PLPurOrder.DownloadBy + "')");
                            oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                        }
                        else
                        {
                            sb = new StringBuilder();
                            sb.AppendLine(" Update tSAPPurchaseOrderData set POQty = '" + _PLPurOrder.POQty + "'");
                            sb.AppendLine(", MatDescription = '" + _PLPurOrder.MatDescription + "'");
                            sb.AppendLine(", POLocType = '" + _PLPurOrder.POLocType + "', PODate = '" + _PLPurOrder.PODate + "'");
                            sb.AppendLine(", DownloadBy = '" + _PLPurOrder.DownloadBy + "', DownloadOn = GETDATE() ");
                            sb.AppendLine(" WHERE LocationCode = '" + _PLPurOrder.LocationCode + "' AND PONumber = '" + _PLPurOrder.PONumber + "'");
                            sb.AppendLine(" AND VendorCode = '" + _PLPurOrder.VendorCode + "' AND MatCode = '" + _PLPurOrder.MatCode + "'");
                            oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iUpdateCount++;
                        }
                        //break;
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPPurchaseOrder Data : No. of Records Inserted - " + clsGlobal.iAddCount++ + " and Updated - " + clsGlobal.iUpdateCount);
                    //ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPPurchaseOrder Data : No. of Records Updated - " + clsGlobal.iUpdateCount + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                }
                else if (ds.Tables["tSAPPurchaseOrderData"].Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPPurchaseOrder Data : " + "There is no records found");
                    clsGlobal.lmsg = "Saving CentralToLocal SAPPurchaseOrder Data : " + "There is no records found";
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Receiving Purchase Order Data for LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " from Central Server found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                //}
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPPurchaseOrder Data :: Query - " + sb.ToString());
                ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPPurchaseOrder Data :: Error is - " + ex.Message.ToString());
            }
            finally
            {
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public void GetC2LtSAPReturnPurchaseOrderData()
        {
            clsLogic oLogic = new clsLogic();
            PLSAPReturnPurchaseOrderData _PLPurOrder = new PLSAPReturnPurchaseOrderData();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (ds.Tables["tSAPReturnPurchaseOrderData"].Rows.Count > 0)
                {
                    //ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPPurchaseOrder Data : No. of Records Found - " + ds.Tables["tSAPReturnPurchaseOrderData"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    foreach (DataRow dr in ds.Tables["tSAPReturnPurchaseOrderData"].Rows)
                    {
                        sb = new StringBuilder();
                        _PLPurOrder.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLPurOrder.POReturnNo = dr["POReturnNo"].ToString().Trim();
                        _PLPurOrder.VendorCode = dr["VendorCode"].ToString().Trim();
                        _PLPurOrder.VendorName = dr["VendorName"].ToString().Trim();
                        _PLPurOrder.MatCode = dr["MatCode"].ToString().Trim();
                        _PLPurOrder.MatDescription = dr["MatDesc"].ToString().Trim();
                        //_PLPurOrder.MatThickness = dr["MatThickness"].ToString().Trim();
                        //_PLPurOrder.MatSize = dr["MatSize"].ToString().Trim();
                        //_PLPurOrder.MatGrade = dr["MatGrade"].ToString().Trim();
                        //_PLPurOrder.Category = dr["Category"].ToString().Trim();
                        //_PLPurOrder.MatGroup = dr["MatGroup"].ToString().Trim();
                        _PLPurOrder.ReturnQty = Convert.ToInt32(dr["ReturnQty"].ToString().Trim());
                        _PLPurOrder.POLocType = Convert.ToString(dr["POLocType"].ToString().Trim());
                        //_PLPurOrder.PODate = dr["ScannedQty"].ToString().Trim();
                        _PLPurOrder.ReturnStatus = dr["ReturnStatus"].ToString().Trim();
                        //_PLPurOrder.IsQRCodeGenerated = dr["IsQRCodeGenerated"].ToString().Trim();
                        //_PLPurOrder.GeneratedQty = dr["GeneratedQty"].ToString().Trim();
                        //_PLPurOrder.GeneratedBy = dr["GeneratedBy"].ToString().Trim();
                        //_PLPurOrder.GeneratedOn = dr["GeneratedOn"].ToString().Trim();
                        //_PLPurOrder.IsPrinted = dr["IsPrinted"].ToString().Trim();
                        //if (dr["PrintedQty"].ToString().Trim() == string.Empty || dr["PrintedQty"].ToString().Trim() == null)
                        //    _PLPurOrder.PrintedQty = 0;
                        //else
                        //    _PLPurOrder.PrintedQty = Convert.ToInt32(dr["PrintedQty"].ToString().Trim());
                        //_PLPurOrder.PrintedBy = dr["PrintedBy"].ToString().Trim();
                        //_PLPurOrder.PrintedOn = dr["PrintedOn"].ToString().Trim();
                        //_PLPurOrder.IsReprinted = dr["IsReprinted"].ToString().Trim();
                        //if (dr["RePrintedQty"].ToString().Trim() == string.Empty || dr["RePrintedQty"].ToString().Trim() == null)
                        //    _PLPurOrder.RePrintedQty = 0;
                        //else
                        //    _PLPurOrder.RePrintedQty = Convert.ToInt32(dr["RePrintedQty"].ToString().Trim());
                        //_PLPurOrder.RePrintedBy = dr["RePrintedBy"].ToString().Trim();
                        //_PLPurOrder.RePrintedOn = dr["RePrintedOn"].ToString().Trim();
                        //_PLPurOrder.DownloadOn = dr["DownloadOn"].ToString().Trim();
                        _PLPurOrder.DownloadBy = "DataScheduler";

                        sb.Append("Select MatCode from tSAPPurchaseReturnData WITH (NOLOCK) WHERE LocationCode = '" + _PLPurOrder.LocationCode + "' AND POReturnNo = '" + _PLPurOrder.POReturnNo + "'");
                        sb.Append(" AND VendorCode = '" + _PLPurOrder.VendorCode + "' AND MatCode = '" + _PLPurOrder.MatCode + "'");
                        DataTable dt = oDbmLocal.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Insert into tSAPPurchaseReturnData(LocationCode, POReturnNo, VendorCode, VendorName, MatCode, MatDesc, ReturnQty, POLocType,");
                            sb.AppendLine(" ReturnStatus, ScannedQty, DownloadOn, DownloadBy)");
                            sb.AppendLine("Values ");
                            sb.AppendLine("('" + _PLPurOrder.LocationCode + "','" + _PLPurOrder.POReturnNo + "','" + _PLPurOrder.VendorCode + "','" + _PLPurOrder.VendorName + "',");
                            sb.AppendLine("'" + _PLPurOrder.MatCode + "','" + _PLPurOrder.MatDescription + "',");
                            //sb.AppendLine("'" + _PLPurOrder.MatGrade + "','" + _PLPurOrder.Category + "','" + _PLPurOrder.MatGroup + "',");
                            sb.AppendLine("'" + _PLPurOrder.ReturnQty + "','" + _PLPurOrder.POLocType + "','" + _PLPurOrder.ReturnStatus + "',");
                            sb.AppendLine(" 0, GetDate(), '" + _PLPurOrder.DownloadBy + "')");
                            oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                        }
                        else
                        {
                            sb = new StringBuilder();
                            sb.AppendLine(" Update tSAPPurchaseReturnData set MatDesc = '" + _PLPurOrder.MatDescription + "'");
                            //sb.AppendLine(", MatSize = '" + _PLPurOrder.MatSize + "', MatGrade = '" + _PLPurOrder.MatGrade + "', Category = '" + _PLPurOrder.Category + "'");
                            sb.AppendLine(", ReturnQty = '" + _PLPurOrder.ReturnQty + "', POLocType = '" + _PLPurOrder.POLocType + "'");
                            sb.AppendLine(", DownloadBy = '" + _PLPurOrder.DownloadBy + "', DownloadOn = GETDATE() ");
                            sb.AppendLine(" WHERE LocationCode = '" + _PLPurOrder.LocationCode + "' AND POReturnNo = '" + _PLPurOrder.POReturnNo + "'");
                            sb.AppendLine(" AND VendorCode = '" + _PLPurOrder.VendorCode + "' AND MatCode = '" + _PLPurOrder.MatCode + "'");
                            oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iUpdateCount++;
                        }
                        //break;
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPPurchaseReturnData Data : No. of Records Inserted - " + clsGlobal.iAddCount + " and Updated Records - " + clsGlobal.iUpdateCount);
                }
                else if (ds.Tables["tSAPReturnPurchaseOrderData"].Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPPurchaseReturnData Data : " + "There is no records found");
                    clsGlobal.lmsg = "Saving CentralToLocal SAPPurchaseOrder Data : " + "There is no records found";
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Receiving Return Purchase Order Data for LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " from Central Server found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                //}
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPPurchaseReturnData Data :: Error is - " + ex.Message.ToString());
            }
            finally
            {
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public void GetC2LtSAPQualityInspectionData()
        {
            clsLogic oLogic = new clsLogic();
            PLLocationLabelPrinting _PLPurOrder = new PLLocationLabelPrinting();
            StringBuilder sb;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (ds.Tables["tSAPQAData"].Rows.Count > 0)
                {
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    foreach (DataRow dr in ds.Tables["tSAPQAData"].Rows)
                    {
                        sb = new StringBuilder();
                        _PLPurOrder.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLPurOrder.PONumber = dr["PONumber"].ToString().Trim();
                        _PLPurOrder.MatCode = dr["MatCode"].ToString().Trim();
                        _PLPurOrder.QRCode = dr["QRCode"].ToString().Trim();
                        _PLPurOrder.MIGONo = dr["MIGONo"].ToString().Trim();
                        _PLPurOrder.InspectionLotNo = dr["InspectionLotNo"].ToString().Trim();
                        //_PLPurOrder.QCStatus = Convert.ToInt32(dr["QCStatus"].ToString().Trim());

                        sb.Append("Select MatCode from tLocationPrintingHistory WITH (NOLOCK) WHERE LocationCode = '" + _PLPurOrder.LocationCode + "' AND PONumber = '" + _PLPurOrder.PONumber + "'");
                        sb.Append(" AND MatCode = '" + _PLPurOrder.MatCode + "' AND QRCode = '" + _PLPurOrder.QRCode + "'");
                        DataTable dt = oDbmLocal.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 1)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine(" Update tLocationPrintingHistory set MIGONo = '" + _PLPurOrder.MIGONo + "'");
                            sb.AppendLine(", InspectionLotNo = '" + _PLPurOrder.InspectionLotNo + "', UpdateOn = GETDATE() ");
                            sb.AppendLine(" WHERE LocationCode = '" + _PLPurOrder.LocationCode + "' AND PONumber = '" + _PLPurOrder.PONumber + "'");
                            sb.AppendLine(" AND MatCode = '" + _PLPurOrder.MatCode + "' AND QRCode = '" + _PLPurOrder.QRCode + "'");
                            oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iUpdateCount++;

                            sb = new StringBuilder();
                            sb.AppendLine(" Update tLocationLabelPrinting set MIGONo = '" + _PLPurOrder.MIGONo + "'");
                            sb.AppendLine(", InspectionLotNo = '" + _PLPurOrder.InspectionLotNo + "', UpdateOn = GETDATE() ");
                            sb.AppendLine(" WHERE LocationCode = '" + _PLPurOrder.LocationCode + "' AND PONumber = '" + _PLPurOrder.PONumber + "'");
                            sb.AppendLine(" AND MatCode = '" + _PLPurOrder.MatCode + "' AND QRCode = '" + _PLPurOrder.QRCode + "'");
                            oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iUpdateCount++;
                        }
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPQualityInspectionData Data : No. of Records Inserted - " + clsGlobal.iAddCount + " and Updated Records - " + clsGlobal.iUpdateCount);
                }
                else if (ds.Tables["tSAPQAData"].Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPQualityInspectionData Data : " + "There is no records found");
                    clsGlobal.lmsg = "Saving CentralToLocal SAPQualityInspectionData Data : " + "There is no records found";
                }
            }
            catch (Exception ex)
            {
                string sResp = "Receiving SAP QualityInspection Data for LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " from Central Server found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPQualityInspectionData Data :: Error is - " + ex.Message.ToString());
            }
            finally
            {
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public void GetC2LtSAPSalesReturnData()
        {
            clsLogic oLogic = new clsLogic();
            PLSAPSalesReturnData _PLSRData = new PLSAPSalesReturnData();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (ds.Tables["tSAPSalesReturnData"].Rows.Count > 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPSalesReturn Data : No. of Records Found - " + ds.Tables["tSAPSalesReturnData"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    foreach (DataRow dr in ds.Tables["tSAPSalesReturnData"].Rows)
                    {
                        sb = new StringBuilder();
                        _PLSRData.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLSRData.SalesReturnNo = dr["SalesReturnNo"].ToString().Trim();
                        _PLSRData.CustomerCode = dr["CustomerCode"].ToString().Trim();
                        _PLSRData.CustomerName = dr["CustomerName"].ToString().Trim();
                        _PLSRData.MatCode = dr["MatCode"].ToString().Trim();
                        _PLSRData.MatDesc = dr["MatDesc"].ToString().Trim();
                        _PLSRData.ReturnQty = Convert.ToInt32(dr["ReturnQty"].ToString().Trim());
                        _PLSRData.DownloadBy = "DataScheduler";

                        sb.Append("SELECT SalesReturnNo from tSAPSalesReturnData WITH (NOLOCK) WHERE LocationCode = '" + _PLSRData.LocationCode + "' AND SalesReturnNo = '" + _PLSRData.SalesReturnNo + "'");
                        sb.Append(" AND CustomerCode = '" + _PLSRData.CustomerCode + "' AND MatCode = '" + _PLSRData.MatCode + "'");
                        DataTable dt = oDbmLocal.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Insert into tSAPSalesReturnData(LocationCode, SalesReturnNo, CustomerCode, CustomerName, MatCode, MatDesc,");
                            sb.AppendLine(" ReturnQty, ScannedQty, ReturnStatus, DownloadOn, DownloadBy)");
                            sb.AppendLine("Values ");
                            sb.AppendLine("('" + _PLSRData.LocationCode + "','" + _PLSRData.SalesReturnNo + "','" + _PLSRData.CustomerCode + "','" + _PLSRData.CustomerName + "',");
                            sb.AppendLine("'" + _PLSRData.MatCode + "','" + _PLSRData.MatDesc + "',");
                            sb.AppendLine("'" + _PLSRData.ReturnQty + "'");
                            sb.AppendLine(", 0, 'N', GETDATE(), '" + _PLSRData.DownloadBy + "')");
                            oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                        }
                        else
                        {
                            sb = new StringBuilder();
                            sb.AppendLine(" Update tSAPSalesReturnData set CustomerName = '" + _PLSRData.CustomerName + "', MatDesc = '" + _PLSRData.MatDesc + "'");
                            sb.AppendLine(", ReturnQty = '" + _PLSRData.ReturnQty + "'");
                            sb.AppendLine(", DownloadOn =  GETDATE(), DownloadBy = '" + _PLSRData.DownloadBy + "'");
                            sb.AppendLine(" WHERE LocationCode = '" + _PLSRData.LocationCode + "' AND SalesReturnNo = '" + _PLSRData.SalesReturnNo + "'");
                            sb.AppendLine(" AND CustomerCode = '" + _PLSRData.CustomerCode + "' AND MatCode = '" + _PLSRData.MatCode + "'");
                            oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iUpdateCount++;
                        }
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPSalesReturn Data : No. of Records Inserted - " + clsGlobal.iAddCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPSalesReturn Data : No. of Records Updated - " + clsGlobal.iUpdateCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                }
                else if (ds.Tables["tSAPSalesReturnData"].Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPSalesReturn Data : " + "There is no records found" + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    clsGlobal.lmsg = "Saving CentralToLocal SAPSalesReturn Data : " + "There is no records found" + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Receiving Sales Return Order Data for LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " from Central Server found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                //}
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Downloading C2L SAP Sales Return Data :: Error is - " + ex.Message.ToString() + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                //throw ex;
            }
            finally
            {
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        public void GetC2LVendorLabelGeneratingDetails()
        {
            clsLogic oLogic = new clsLogic();
            PLtVendorLabelPrinting _PLPrint = new PLtVendorLabelPrinting();
            StringBuilder sb;

            try
            {
                oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (ds.Tables["tVendorLabelGenerating"].Rows.Count > 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral VendorLabelGenerating Data : No. of Records Found - " + ds.Tables["tVendorLabelGenerating"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    foreach (DataRow dr in ds.Tables["tVendorLabelGenerating"].Rows)
                    {
                        sb = new StringBuilder();
                        _PLPrint.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLPrint.PONumber = dr["PONumber"].ToString().Trim();
                        _PLPrint.MatCode = dr["MatCode"].ToString().Trim();
                        _PLPrint.QRCode = dr["QRCode"].ToString().Trim();
                        _PLPrint.VendorCode = dr["VendorCode"].ToString().Trim();
                        _PLPrint.GeneratedBy = dr["GeneratedBy"].ToString().Trim();
                        _PLPrint.GeneratedOn = dr["GeneratedOn"].ToString().Trim().Substring(0, 10);
                        _PLPrint.IsQRCodePrinted = dr["IsQRCodePrinted"].ToString().Trim();
                        _PLPrint.PrintedBy = dr["PrintedBy"].ToString().Trim();
                        _PLPrint.PrintedOn = dr["PrintedOn"].ToString().Trim();
                        _PLPrint.IsRePrinted = dr["IsRePrinted"].ToString().Trim();
                        _PLPrint.RePrintedBy = dr["RePrintedBy"].ToString().Trim();
                        _PLPrint.RePrintedOn = dr["RePrintedOn"].ToString().Trim();
                        _PLPrint.SentBy = clsGlobal.PlantCode.Trim() + "_DataScheduler";

                        sb.AppendLine("SELECT QRCode from tVendorLabelGenerating WITH (NOLOCK) where LocationCode = '" + _PLPrint.LocationCode + "' AND PONumber = '" + _PLPrint.PONumber + "'");
                        sb.AppendLine("AND MatCode = '" + _PLPrint.MatCode + "' AND VendorCode = '" + _PLPrint.VendorCode + "' AND QRCode = '" + _PLPrint.QRCode + "'");
                        DataTable dt = oDbmLocal.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Insert into tVendorLabelGenerating(LocationCode, PONumber, MatCode, QRCode,");
                            sb.AppendLine("VendorCode, GeneratedBy, GeneratedOn, IsQRCodePrinted, Status,");
                            sb.AppendLine(" PrintedBy, PrintedOn, IsRePrinted, RePrintedBy, RePrintedOn, IsSAPPosted, SentOn, SentBy)");
                            sb.AppendLine("Values ");
                            sb.AppendLine("('" + _PLPrint.LocationCode + "','" + _PLPrint.PONumber + "','" + _PLPrint.MatCode + "',");
                            sb.AppendLine(" '" + _PLPrint.QRCode + "','" + _PLPrint.VendorCode + "',");
                            sb.AppendLine(" '" + _PLPrint.GeneratedBy + "','" + _PLPrint.GeneratedOn + "',");
                            sb.AppendLine(" '" + _PLPrint.IsQRCodePrinted + "', 'V', '" + _PLPrint.PrintedBy + "','" + _PLPrint.PrintedOn + "',");
                            sb.AppendLine(" '" + _PLPrint.IsRePrinted + "',");
                            sb.AppendLine(" '" + _PLPrint.RePrintedBy + "','" + _PLPrint.RePrintedOn + "', 0, GETDATE(), '" + _PLPrint.SentBy + "')");
                            oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                            dr["IsSAPPosted"] = "True";
                        }
                    }
                    oDbmLocal.CommitTransaction();
                    ds.Tables["tVendorLabelGenerating"].AcceptChanges();
                    oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrClientCon);
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    DataTable odtScannedData9 = new DataTable();
                    odtScannedData9 = ds.Tables["tVendorLabelGenerating"].Clone();
                    DataRow[] rows1;
                    rows1 = ds.Tables["tVendorLabelGenerating"].Select("IsSAPPosted = True");
                    foreach (DataRow row in rows1)
                    {
                        odtScannedData9.ImportRow(row);
                    }
                    odtScannedData9.AcceptChanges();
                    foreach (DataRow dr in odtScannedData9.Rows)
                    {
                        string sLocCode = dr["LocationCode"].ToString().Trim();
                        string sPONumber = dr["PONumber"].ToString().Trim();
                        string sMatcode = dr["MatCode"].ToString().Trim();
                        string sQRCode = dr["QRCode"].ToString().Trim();
                        string sVendorCode = dr["VendorCode"].ToString().Trim();
                        sb = new StringBuilder();
                        sb.AppendLine(" Update tVendorLabelGenerating set IsSAPPosted = 1, PostedOn = GETDATE() ");
                        sb.AppendLine(" where LocationCode = '" + sLocCode + "' AND VendorCode = '" + sVendorCode + "'");
                        sb.AppendLine(" And PONumber = '" + sPONumber + "' AND MatCode = '" + sMatcode + "' AND QRCode = '" + sQRCode + "'");
                        oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
                        clsGlobal.iUpdateCount++;
                    }
                    oDbmCentral.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral VendorLabelGenerating Data : No. of Records Inserted - " + clsGlobal.iAddCount);
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral VendorLabelGenerating Data : No. of Records Updated - " + clsGlobal.iUpdateCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                }
                else if (ds.Tables["tVendorLabelPrinting"].Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral VendorLabelGenerating Data : " + "There is no records found" + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    clsGlobal.lmsg = "Saving LocaltoCentral Vendor Data : " + "There is no records found" + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Receiving Vendor Label Generating Data for LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " from Central Server found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                //}
                oDbmLocal.RollBackTransaction();
                oDbmCentral.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Saving LocaltoCentral VendorLabelGenerating Data :: Error is - " + ex.Message.ToString() + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
            }
            finally
            {
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
        }

        public void GetC2LtSAPPurchaseOrderDataForVendor()
        {
            clsLogic oLogic = new clsLogic();
            PLSAPPurchaseOrderData _PLPurOrder = new PLSAPPurchaseOrderData();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (ds.Tables["tSAPPurchaseOrderData"].Rows.Count > 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPPurchaseOrder Data : No. of Records Found - " + ds.Tables["tSAPPurchaseOrderData"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmLocal.BeginTransaction(oDbmLocal.Connection);
                    foreach (DataRow dr in ds.Tables["tSAPPurchaseOrderData"].Rows)
                    {
                        sb = new StringBuilder();
                        _PLPurOrder.LocationCode = dr["LocationCode"].ToString().Trim();
                        _PLPurOrder.PONumber = dr["PONumber"].ToString().Trim();
                        _PLPurOrder.VendorCode = dr["VendorCode"].ToString().Trim();
                        _PLPurOrder.VendorName = dr["VendorName"].ToString().Trim();
                        _PLPurOrder.MatCode = dr["MatCode"].ToString().Trim();
                        _PLPurOrder.MatDescription = dr["MatDescription"].ToString().Trim();
                        _PLPurOrder.POQty = Convert.ToInt32(dr["POQty"].ToString().Trim());
                        _PLPurOrder.POLocType = Convert.ToString(dr["POLocType"].ToString().Trim());
                        _PLPurOrder.PODate = Convert.ToString(dr["PODate"].ToString().Trim());
                        _PLPurOrder.IsQRCodeGenerated = Convert.ToString(dr["IsQRCodeGenerated"].ToString().Trim());
                        _PLPurOrder.GeneratedQty = Convert.ToString(dr["GeneratedQty"].ToString().Trim());
                        _PLPurOrder.POStatus = Convert.ToString(dr["POStatus"].ToString().Trim());
                        _PLPurOrder.GeneratedBy = Convert.ToString(dr["GeneratedBy"].ToString().Trim());
                        _PLPurOrder.GeneratedOn = Convert.ToString(dr["GeneratedOn"].ToString().Trim());
                        _PLPurOrder.IsPrinted = Convert.ToString(dr["IsPrinted"].ToString().Trim());
                        _PLPurOrder.PrintedQty = Convert.ToInt32(dr["PrintedQty"].ToString().Trim());
                        _PLPurOrder.PrintedBy = Convert.ToString(dr["PrintedBy"].ToString().Trim());
                        _PLPurOrder.PrintedOn = Convert.ToString(dr["PrintedOn"].ToString().Trim());
                        _PLPurOrder.IsReprinted = Convert.ToString(dr["IsPrinted"].ToString().Trim());
                        _PLPurOrder.RePrintedQty = Convert.ToInt32(dr["RePrintedQty"].ToString().Trim());
                        _PLPurOrder.RePrintedBy = Convert.ToString(dr["RePrintedBy"].ToString().Trim());
                        _PLPurOrder.RePrintedOn = Convert.ToString(dr["RePrintedOn"].ToString().Trim());

                        _PLPurOrder.PODate = dr["PODate"].ToString().Trim();
                        _PLPurOrder.DownloadBy = "DataScheduler";

                        sb.Append("Select MatCode from tSAPPurchaseOrderData WITH (NOLOCK) WHERE LocationCode = '" + _PLPurOrder.LocationCode + "' AND PONumber = '" + _PLPurOrder.PONumber + "'");
                        sb.Append(" AND VendorCode = '" + _PLPurOrder.VendorCode + "' AND MatCode = '" + _PLPurOrder.MatCode + "'");
                        DataTable dt = oDbmLocal.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
                        if (dt.Rows.Count == 0)
                        {
                            sb = new StringBuilder();
                            sb.AppendLine("Insert into tSAPPurchaseOrderData(LocationCode, PONumber, VendorCode, VendorName, MatCode, MatDescription, POQty, POLocType, ");
                            sb.AppendLine("PODate, POStatus, IsQRCodeGenerated, GeneratedQty, GeneratedOn, GeneratedBy, IsPrinted, PrintedQty, PrintedBy, PrintedOn, IsReprinted, ");
                            sb.AppendLine("RePrintedQty, RePrintedBy, RePrintedOn, DownloadOn, DownloadBy)");
                            sb.AppendLine("Values ");
                            sb.AppendLine("('" + _PLPurOrder.LocationCode + "','" + _PLPurOrder.PONumber + "','" + _PLPurOrder.VendorCode + "','" + _PLPurOrder.VendorName + "',");
                            sb.AppendLine("'" + _PLPurOrder.MatCode + "','" + _PLPurOrder.MatDescription + "','" + _PLPurOrder.POQty + "','" + _PLPurOrder.POLocType + "',");
                            sb.AppendLine("'" + _PLPurOrder.PODate + "','" + _PLPurOrder.POStatus + "','" + _PLPurOrder.IsQRCodeGenerated + "','" + _PLPurOrder.GeneratedQty + "',");
                            sb.AppendLine("'" + _PLPurOrder.GeneratedOn + "','" + _PLPurOrder.GeneratedBy + "','" + _PLPurOrder.IsPrinted + "','" + _PLPurOrder.PrintedQty + "',");
                            sb.AppendLine("'" + _PLPurOrder.PrintedBy + "','" + _PLPurOrder.PrintedOn + "','" + _PLPurOrder.IsReprinted + "','" + _PLPurOrder.RePrintedQty + "',");
                            sb.AppendLine("'" + _PLPurOrder.RePrintedBy + "','" + _PLPurOrder.RePrintedOn + "', GetDate(), '" + _PLPurOrder.DownloadBy + "')");
                            oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iAddCount++;
                        }
                        else
                        {
                            sb = new StringBuilder();
                            sb.AppendLine(" Update tSAPPurchaseOrderData set POQty = '" + _PLPurOrder.POQty + "'");
                            sb.AppendLine(", MatDescription = '" + _PLPurOrder.MatDescription + "', POLocType = '" + _PLPurOrder.POLocType + "'");
                            sb.AppendLine(", IsQRCodeGenerated = '" + _PLPurOrder.IsQRCodeGenerated + "', GeneratedQty = '" + _PLPurOrder.GeneratedQty + "'");
                            sb.AppendLine(", GeneratedOn = '" + _PLPurOrder.GeneratedOn + "', GeneratedBy = '" + _PLPurOrder.GeneratedBy + "'");
                            sb.AppendLine(", IsPrinted = '" + _PLPurOrder.IsPrinted + "'");  //"', PrintedQty = '" + _PLPurOrder.PrintedQty +
                            sb.AppendLine(", PrintedBy = '" + _PLPurOrder.PrintedBy + "', PrintedOn = '" + _PLPurOrder.PrintedOn + "'");
                            sb.AppendLine(", IsReprinted = '" + _PLPurOrder.IsReprinted + "'");  //"', RePrintedQty = '" + _PLPurOrder.RePrintedQty + 
                            sb.AppendLine(", RePrintedBy = '" + _PLPurOrder.RePrintedBy + "', RePrintedOn = '" + _PLPurOrder.RePrintedOn + "'");
                            sb.AppendLine(", DownloadBy = '" + _PLPurOrder.DownloadBy + "', DownloadOn = GETDATE() ");
                            sb.AppendLine(" WHERE LocationCode = '" + _PLPurOrder.LocationCode + "' AND PONumber = '" + _PLPurOrder.PONumber + "'");
                            sb.AppendLine(" AND VendorCode = '" + _PLPurOrder.VendorCode + "' AND MatCode = '" + _PLPurOrder.MatCode + "'");
                            oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
                            clsGlobal.iUpdateCount++;
                        }
                        //break;
                    }
                    oDbmLocal.CommitTransaction();
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPPurchaseOrder Data : No. of Records Inserted - " + clsGlobal.iAddCount++ + " and Updated - " + clsGlobal.iUpdateCount);
                    //ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPPurchaseOrder Data : No. of Records Updated - " + clsGlobal.iUpdateCount + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                }
                else if (ds.Tables["tSAPPurchaseOrderData"].Rows.Count == 0)
                {
                    ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPPurchaseOrder Data : " + "There is no records found" + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    clsGlobal.lmsg = "Saving CentralToLocal SAPPurchaseOrder Data : " + "There is no records found" + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss");
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Receiving Purchase Order Data for Vendor for LocationCode - " + Properties.Settings.Default.LocationCode;
                sResp = sResp + " from Central Server found error. Kindly look into the local data scheduler log file for more details";
                SendMail(sResp.ToString());
                //}
                oDbmLocal.RollBackTransaction();
                ObjLog.WriteLog("Data Scheduler => Saving CentralToLocal SAPPurchaseOrder Data :: Error is - " + ex.Message.ToString() + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
            }
            finally
            {
                if (oDbmLocal.Connection.State == ConnectionState.Open)
                    oDbmLocal.Close();
            }
        }

        #endregion


        #region Email

        public bool SendMail(string sResponce)
        {
            try
            {
                string MailUser = Properties.Settings.Default.SMTPUser; // "qrcode@greenply.com";
                string MailPass = Properties.Settings.Default.SMTPPassword; // "Qrc0d@321";
                string MailTo = Properties.Settings.Default.ReceiverEmail; // "ashutosh@barcodeindia.com";
                ExchangeService service = new ExchangeService(ExchangeVersion.Exchange2013_SP1);
                service.Credentials = new NetworkCredential(MailUser, MailPass);
                if (Properties.Settings.Default.UseProxy)
                {
                    WebProxy proxy = new WebProxy(Properties.Settings.Default.ProxyIP.ToString(), Properties.Settings.Default.ProxyPort);
                    service.WebProxy = proxy;
                }
                service.Url = new Uri(Properties.Settings.Default.SMTPServer);  //("https://outlook.office365.com/owa");
                EmailMessage emailMessage = new EmailMessage(service);
                string htmlString = string.Empty;
                emailMessage.Subject = Properties.Settings.Default.SMTPSubject + " - " + DateTime.Now.ToString("ddMMyyyy");
                htmlString = @"<html>
                      <body>
                      <p>Hi,</p>
                      <p>Error has been logged in the data transfer, details of the error is mentioned below:</p>
                      <p>Error - " + sResponce + @"</p>
                      <p>For any queries, feel free to connect with the Greenply Barcode Team.</p>
                      <p>Thanks,<br>Greenply Barcode Team</br></p>
                      </body>
                      </html>
                     ";
                string msgBody = (htmlString.ToString());
                emailMessage.Body = new MessageBody(msgBody);
                emailMessage.ToRecipients.Add(MailTo);
                emailMessage.Send();
                ObjLog.WriteLog("Email sent successfully using SMTP!");
                sEmailSentStatus = "Sent";
                return true;
            }
            catch (Exception ex)
            {
                sEmailSentStatus = "Failed";
                ObjLog.WriteLog("Email sent Failed using SMTP! And Error is : " + ex.Message.ToString());
                if (ex.Message != null)
                    ObjLog.WriteLog(ex.Message);
                return false;
            }

        }

        #endregion

        public static DataTable ConvertCSVtoDataTable2(string strFilePath)
        {
            StreamReader sr = new StreamReader(strFilePath);
            string[] headers = sr.ReadLine().Split(',');
            DataTable dt = new DataTable();
            foreach (string header in headers)
            {
                dt.Columns.Add(header);
            }
            while (!sr.EndOfStream)
            {
                string[] rows = Regex.Split(sr.ReadLine(), ",(?=(?:[^\"]*\"[^\"]*\")*[^\"]*$)");
                DataRow dr = dt.NewRow();
                for (int i = 0; i < headers.Length; i++)
                {
                    dr[i] = rows[i];
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }

        public static DataTable ConvertCSVtoDataTable1(string strFilePath)
        {
            DataTable dt = new DataTable();
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                string[] headers = sr.ReadLine().Split(',');
                foreach (string header in headers)
                {
                    dt.Columns.Add(header);
                }
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Split(',');
                    DataRow dr = dt.NewRow();
                    for (int i = 0; i < headers.Length; i++)
                    {
                        dr[i] = rows[i];
                    }
                    dt.Rows.Add(dr);
                }
            }
            return dt;
        }


        private bool ValidateCSVColumn(DataTable dt)
        {
            bool isValid = false;
            try
            {
                if (Convert.ToString(dt.Columns[0].ColumnName) != "MATERIAL_TYPE".Trim())
                {
                    return isValid = false;
                }
                if (Convert.ToString(dt.Columns[1].ColumnName) != "MATERIAL_GROUP".Trim())
                {
                    return isValid = false;
                }
                if (Convert.ToString(dt.Columns[2].ColumnName) != "MATERIAL_CODE".Trim())
                {
                    return isValid = false;
                }
                if (Convert.ToString(dt.Columns[3].ColumnName) != "MODEL_CODE".Trim())
                {
                    return isValid = false;
                }
                if (Convert.ToString(dt.Columns[4].ColumnName) != "MATERIAL_DESCRIPTION".Trim())
                {
                    return isValid = false;
                }
                else
                {
                    return isValid = true;
                }
            }
            catch (Exception ex)
            {
                return isValid = false;
            }
        }

        public bool IsDate(String date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return true;
            }

            catch
            {
                return false;
            }
        }

        public string ConvertToDate(String date)
        {
            string ItemDate = "";
            try
            {
                string year = date.Substring(0, 4);
                int len = date.Length;
                string Month = date.Substring(len - 10, 2);
                string Day = date.Substring(len - 8, 2);
                string Hr = date.Substring(len - 6, 2);
                string Min = date.Substring(len - 4, 2);
                string sec = date.Substring(len - 2, 2);

                ItemDate = year + "-" + Month + "-" + Day + " " + Hr + ":" + Min + ":" + sec;

            }

            catch (Exception ex)
            {
                throw ex;
            }
            return ItemDate;

        }

        public string ConvertToProdDate(String date)
        {
            string ProdDate = "";
            try
            {
                string year = date.Substring(0, 4);
                int len = date.Length;
                string Month = date.Substring(len - 8, 2);
                string Day = date.Substring(len - 6, 2);

                ProdDate = year + "-" + Month + "-" + Day;

            }

            catch (Exception ex)
            {
                throw ex;
            }
            return ProdDate;

        }

        public void CreateLog(DBManager oDbm, string lModule, string lMethod, string lDescription, string LogFrom, string lPlantCode)
        {
            try
            {
                string lLogfrom = (!string.IsNullOrEmpty(LogFrom)) ? LogFrom : "WMS";
                StringBuilder sb = new StringBuilder();
                sb.Append("Insert into mLog(Module,Method,Description,CreatedBy,CreatedOn,Logfrom,PlantCode) ");
                sb.Append("Values ");
                sb.Append("('" + lModule + "','" + lMethod + "','" + lDescription.Replace("'", "") + "','Scheduler', GetDate(),'" + lLogfrom + "','" + lPlantCode + "') ");
                oDbm.ExecuteNonQuery(CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { oDbm.Close(); }
        }

        public void CreateLog(string lModule, string lMethod, string lDescription, string LogFrom, string lPlantCode)
        {
            try
            {
                string lLogfrom = (!string.IsNullOrEmpty(LogFrom)) ? LogFrom : "WMS";
                StringBuilder sb = new StringBuilder();
                sb.Append("Insert into mLog(Module,Method,Description,CreatedBy,CreatedOn,Logfrom,PlantCode) ");
                sb.Append("Values ");
                sb.Append("('" + lModule + "','" + lMethod + "','" + lDescription.Replace("'", "") + "','Scheduler',GetDate(),'" + lLogfrom + "','" + lPlantCode + "') ");
                oDbmLocal.Open();
                oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { oDbmLocal.Close(); }

        }

        public void saveTimeDtls(string strTime, string strFileFlag, bool bIsActive)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                oDbmLocal.Open();
                oDbmLocal.BeginTransaction(oDbmLocal.Connection);

                sb.Append("Merge into cScheduler S1 ");
                sb.Append("Using ");
                sb.Append("(select '" + strTime + "' as Time) S2 ");
                sb.Append("on (S1.Time = S2.Time) ");
                sb.Append("when matched then ");
                sb.Append("update set S1.FileFlag = '" + strFileFlag + "', S1.isActive = '" + bIsActive + "', S1.ModifiedOn = GetDate() ");
                sb.Append("when not matched then ");
                sb.Append("insert (Time, FileFlag, isActive, CreatedOn, ModifiedOn) ");
                sb.Append("values ('" + strTime + "','" + strFileFlag + "', '" + bIsActive + "',GetDate(),GetDate()); ");
                oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());

                oDbmLocal.CommitTransaction();
            }
            catch (Exception ex)
            {
                oDbmLocal.RollBackTransaction();
            }
            finally
            {
                oDbmLocal.Close();
            }
        }

        public DataTable getTimerDtls()
        {
            try
            {
                oDbmLocal.Open();

                DataTable dtResponse;
                StringBuilder oStr = new StringBuilder();
                oStr.AppendLine(" SELECT Time,FileFlag,isActive ");
                oStr.AppendLine(" FROM cScheduler");
                //oStr.AppendLine(" WHERE isActive=1");
                dtResponse = oDbmLocal.ExecuteDataSet(CommandType.Text, oStr.ToString()).Tables[0];
                oDbmLocal.Close();
                return dtResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable getActiveTimerDtls()
        {
            try
            {
                oDbmLocal.Open();
                DataTable dtResponse;
                StringBuilder oStr = new StringBuilder();
                oStr.AppendLine(" SELECT Time,FileFlag,isActive ");
                oStr.AppendLine(" FROM cScheduler");
                oStr.AppendLine(" WHERE isActive=1");
                dtResponse = oDbmLocal.ExecuteDataSet(CommandType.Text, oStr.ToString()).Tables[0];
                oDbmLocal.Close();
                return dtResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        //public void GetCentral2SAPStockDataPost(DataSet dsData)
        //{
        //    clsLogic oLogic = new clsLogic();
        //    StringBuilder sb;
        //    clsGlobal.iAddCount = 0;
        //    clsGlobal.iUpdateCount = 0;

        //    try
        //    {
        //        sb = new StringBuilder();
        //        oDbmLocal.Open(DataProvider.SqlServer, clsGlobal.StrCon);
        //        _dtBindList = new DataTable();
        //        if (dsData.Tables["tLocationLabelPrinting"].Rows.Count > 0)
        //        {
        //            oDbmLocal.BeginTransaction(oDbmLocal.Connection);
        //            foreach (DataRow dr in dsData.Tables["tLocationLabelPrinting"].Rows)
        //            {
        //                sb = new StringBuilder();
        //                string sLocationCode = dr["LocationCode"].ToString().Trim();
        //                string MatCode = dr["VendorCode"].ToString().Trim();
        //                string QRCode = dr["VendorDesc"].ToString().Trim();
        //                string MatStatus = dr["VendorEmail"].ToString().Trim();
        //                string VendorAddress = dr["VendorAddress"].ToString().Trim();
        //                string CreatedBy = "Scheduler";

        //                sb.Append("select SAP_VENDOR_CODE from mVendor where SAP_VENDOR_CODE = '" + _PLVendorMaster.VendorCode + "' ");
        //                DataTable dt = oDbmLocal.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
        //                if (dt.Rows.Count == 0)
        //                {
        //                    sb = new StringBuilder();
        //                    sb.AppendLine("Insert into mVendor(LocationCode, VendorCode, VendorDesc,");
        //                    sb.AppendLine("VendorEmail, VendorAddress, CreatedOn, CreatedBy)");
        //                    sb.AppendLine("Values ");
        //                    sb.AppendLine("('" + _PLVendorMaster.LocationCode + "','" + _PLVendorMaster.VendorCode + "','" + _PLVendorMaster.VendorDesc + "',");
        //                    sb.AppendLine("'" + _PLVendorMaster.VendorEmail + "','" + _PLVendorMaster.VendorAddress + "',");
        //                    sb.AppendLine("'" + _PLVendorMaster.CreatedOn + "','" + _PLVendorMaster.CreatedBy + "')");
        //                    oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
        //                    clsGlobal.iAddCount++;
        //                }
        //                else
        //                {
        //                    sb = new StringBuilder();
        //                    sb.AppendLine(" Update mVendor set VendorDesc = '" + _PLVendorMaster.VendorDesc + "', VendorEmail = '" + _PLVendorMaster.VendorEmail + "'");
        //                    sb.AppendLine(", VendorAddress = '" + _PLVendorMaster.VendorAddress + "'");
        //                    sb.AppendLine(", CreatedOn = '" + _PLVendorMaster.CreatedOn + "'");
        //                    sb.AppendLine(" CreatedBy = '" + _PLVendorMaster.CreatedBy + "' WHERE VendorCode = '" + _PLVendorMaster.VendorCode + "'");
        //                    oDbmLocal.ExecuteNonQuery(CommandType.Text, sb.ToString());
        //                    clsGlobal.iUpdateCount++;
        //                }
        //                break;
        //                //}
        //            }
        //            oDbmLocal.CommitTransaction();
        //            ObjLog.WriteLog("Data Scheduler => Download Master :: Vendor Master No Of Rows inserted " + clsGlobal.iAddCount++);
        //            ObjLog.WriteLog("Data Scheduler => Download Master :: Vendor Master No Of Rows updated" + clsGlobal.iUpdateCount++);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        oDbmLocal.RollBackTransaction();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        oDbmLocal.Close();
        //    }
        //}

        #region LocalToCentralCommentedCode
        //public DataSet WEBAPILocalToCentralMastersData()
        //{
        //    string strRespone = string.Empty;
        //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Properties.Settings.Default.APILocalToCentralAddressMasters);

        //    string UserName = Properties.Settings.Default.APILocalToCentralUserID;
        //    string Password = Properties.Settings.Default.APILocalToCentralPassword;
        //    //request.Method = HttpMethod.ToString();
        //    request.Method = "GET";
        //    string authHeader = System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(UserName + ":" + Password));
        //    request.Headers.Add("Authorization", "Basic" + " " + authHeader);
        //    using (HttpWebResponse respone = (HttpWebResponse)request.GetResponse())
        //    {
        //        if (respone.StatusCode != HttpStatusCode.OK)
        //        {
        //            throw new Exception("Error Code:" + respone.StatusCode.ToString());
        //        }
        //        using (Stream responestream = respone.GetResponseStream())
        //        {
        //            using (StreamReader reader = new StreamReader(responestream))
        //            {
        //                strRespone = reader.ReadToEnd();
        //            }
        //        }
        //    }
        //    if (strRespone != string.Empty)
        //    {
        //        StringReader theReader = new StringReader(strRespone);
        //        theDataSet = new DataSet();
        //        theDataSet.ReadXml(theReader);
        //    }
        //    return theDataSet;
        //}

        //public DataSet WEBAPILocalToCentralTrasactionalData()
        //{
        //    try
        //    {
        //        string strRespone = string.Empty;
        //        HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Properties.Settings.Default.APILocalToCentralAddressTransaction);
        //        ObjLog.WriteLog("Data Scheduler : LocalToCentral WebApi Address - " + Properties.Settings.Default.APILocalToCentralAddressTransaction);

        //        string UserName = Properties.Settings.Default.APILocalToCentralUserID;
        //        string Password = Properties.Settings.Default.APILocalToCentralPassword;
        //        //request.Method = HttpMethod.ToString();
        //        request.Method = "GET";
        //        string authHeader = System.Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(UserName + ":" + Password));
        //        request.Headers.Add("Authorization", "Basic" + " " + authHeader);
        //        using (HttpWebResponse respone = (HttpWebResponse)request.GetResponse())
        //        {
        //            if (respone.StatusCode != HttpStatusCode.OK)
        //            {
        //                throw new Exception("Error Code:" + respone.StatusCode.ToString());
        //            }
        //            using (Stream responestream = respone.GetResponseStream())
        //            {
        //                ObjLog.WriteLog("Data Scheduler : LocalToCentral : API Responce -- " + responestream);
        //                using (StreamReader reader = new StreamReader(responestream))
        //                {
        //                    strRespone = reader.ReadToEnd();
        //                }
        //            }
        //        }
        //        if (strRespone != string.Empty)
        //        {
        //            StringReader theReader = new StringReader(strRespone);
        //            theDataSet = new DataSet();
        //            theDataSet.ReadXml(theReader);
        //        }
        //        return theDataSet;
        //    }
        //    catch (Exception ex)
        //    {
        //        ObjLog.WriteLog("Data Scheduler : LocalToCentral WebApi Error found - " + ex.Message.ToString());
        //        return theDataSet;
        //    }
        //}
        #endregion

    }
}


