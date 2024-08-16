using DATA_ACCESS_LAYER;
using DataScheduler.GreenplyERPPostingService;
using Microsoft.Exchange.WebServices.Data;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;

namespace DataScheduler
{
    public class BlWMS
    {
        DataTable _dtBindList = new DataTable();
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
        DlCommon oDlcom = new DlCommon();
        int iCount = 0;
        string sLogMsg = "";
        DataTable dtStock = new DataTable();
        DataTable dtVendor = new DataTable();
        DataTable dtRejection = new DataTable();
        DataTable dtSRData = new DataTable();
        DataTable dtDOData = new DataTable();
        DataTable dtMTMData = new DataTable();
        string sEmailSentStatus = string.Empty;
        string sLocationCode = string.Empty;

        public BlWMS()
        {
            oDbmCentral = oDlcom.DBProvider();
            oDbmCentral = oDlcom.ClientDBProvider();
        }


        #region Central To SAP

        public void GetCentral2SAPWebAPIProductionData()
        {
            clsLogic oLogic = new clsLogic();
            try
            {
                DataTable dtData = DLGetProductionDetailsToSAPPost();
                if (dtData.Rows.Count > 0)
                {
                    dsStockData = fnPIGetProductionDataSAPPost(dtData);
                    UpdateResponceSAP2CentralStockDataPost(dsStockData);
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection != null)
                {
                    if (oDbmCentral.Connection.State == ConnectionState.Open)
                        oDbmCentral.Close();
                }
            }
        }

        public void GetCentral2SAPWebAPIDoorProductionData()
        {
            clsLogic oLogic = new clsLogic();
            try
            {
                DataTable dtData = DLGetDoorProductionDetailsToSAPPost();
                if (dtData.Rows.Count > 0)
                {
                    dsStockData = fnPIGetProductionDataSAPPost(dtData);
                    UpdateResponceSAP2CentralStockDataPost(dsStockData);
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection != null)
                {
                    if (oDbmCentral.Connection.State == ConnectionState.Open)
                        oDbmCentral.Close();
                }
            }
        }

        public void GetCentral2SAPWebAPIHubDataBeforeProduction()
        {
            clsLogic oLogic = new clsLogic();
            try
            {
                DataTable dtData = DLGetHubStockDetailsToSAPPost();
                if (dtData.Rows.Count > 0)
                {
                    dsStockData = fnPIGetHubDataSAPPost(dtData);
                    UpdateResponceSAP2CentralHubDataPost(dsStockData);
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection != null)
                {
                    if (oDbmCentral.Connection.State == ConnectionState.Open)
                        oDbmCentral.Close();
                }
            }
        }

        public void GetCentral2SAPWebAPIHubQualityData()
        {
            clsLogic oLogic = new clsLogic();
            try
            {
                DataTable dtData = DLGetHubQualityDetailsToSAPPost();
                if (dtData.Rows.Count > 0)
                {
                    dsStockData = fnPIGetHubQualityDataSAPPost(dtData);
                    UpdateResponceSAP2CentralHubQualityDataPost(dsStockData);
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection != null)
                {
                    if (oDbmCentral.Connection.State == ConnectionState.Open)
                        oDbmCentral.Close();
                }
            }
        }

        public void GetCentral2SAPWebAPIHubQualityBOILData()
        {
            clsLogic oLogic = new clsLogic();
            try
            {
                DataTable dtData = DLGetHubQualityBOILDetailsToSAPPost();
                if (dtData.Rows.Count > 0)
                {
                    dsStockData = fnPIGetHubQualityBOILDataSAPPost(dtData);
                    UpdateResponceSAP2CentralHubQualityBOILDataPost(dsStockData);
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection != null)
                {
                    if (oDbmCentral.Connection.State == ConnectionState.Open)
                        oDbmCentral.Close();
                }
            }
        }

        public void GetCentral2SAPWebAPIVendorData()
        {
            clsLogic oLogic = new clsLogic();
            try
            {
                DataTable dtData = DLGetVendorDetailsToSAPPost();
                if (dtData.Rows.Count > 0)
                {
                    dsVendorData = fnPIGetVendorDataSAPPost(dtData);
                    UpdateResponceSAP2CentralVendorDataPost(dsVendorData);
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection != null)
                {
                    if (oDbmCentral.Connection.State == ConnectionState.Open)
                        oDbmCentral.Close();
                }
            }
        }

        public void GetCentral2SAPWebAPIDeliveryOrderData()
        {
            try
            {
                DataTable dtData = DLGetDeliveryOrderDetailsToSAPPost();
                if (dtData.Rows.Count > 0)
                {
                    dsDOData = fnPIGetDeliveryOrderDataSAPPost(dtData);
                    UpdateResponceSAP2CentralDeliveryOrderDataPost(dsDOData);
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection != null)
                {
                    if (oDbmCentral.Connection.State == ConnectionState.Open)
                        oDbmCentral.Close();
                }
            }
        }

        public void GetCentral2SAPWebAPICancelledDeliveryOrderData()
        {
            try
            {
                DataTable dtData = DLGetCancelledDeliveryOrderDetailsToSAPPost();
                if (dtData.Rows.Count > 0)
                {
                    dsDOData = fnPIGetCancelledDeliveryOrderDataSAPPost(dtData);
                    UpdateResponceSAP2CentralCancelledDeliveryOrderDataPost(dsDOData);
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection != null)
                {
                    if (oDbmCentral.Connection.State == ConnectionState.Open)
                        oDbmCentral.Close();
                }
            }
        }

        public void GetCentral2SAPWebAPISalesReturnData()
        {
            try
            {
                DataTable dtData = DLGetSalesReturnDetailsToSAPPost();
                if (dtData.Rows.Count > 0)
                {
                    dsSRData = fnPIGetSalesReturnDataSAPPost(dtData);
                    UpdateResponceSAP2CentralSalesReturnDataPost(dsSRData);
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection != null)
                {
                    if (oDbmCentral.Connection.State == ConnectionState.Open)
                        oDbmCentral.Close();
                }
            }
        }

        public void GetCentral2SAPWebAPIReturnPurchaseOrderData()
        {
            try
            {
                DataTable dtData = DLGetReturnPurchaseOrderDetailsToSAPPost();
                if (dtData.Rows.Count > 0)
                {
                    dsSRData = fnPIGetReturnPurchaseOrderDataSAPPost(dtData);
                    UpdateResponceSAP2CentralPurchaseReturnOrderDataPost(dsSRData);
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection != null)
                {
                    if (oDbmCentral.Connection.State == ConnectionState.Open)
                        oDbmCentral.Close();
                }
            }
        }

        public void GetCentral2SAPWebAPIMTMTransferData()
        {
            try
            {
                DataTable dtData = DLGetMTMTransferDetailsToSAPPost();
                if (dtData.Rows.Count > 0)
                {
                    dsMTMData = fnPIGetMTMTransferDataSAPPost(dtData);
                    UpdateResponceSAP2CentralMTMTransferDataPost(dsMTMData);
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection != null)
                {
                    if (oDbmCentral.Connection.State == ConnectionState.Open)
                        oDbmCentral.Close();
                }
            }
        }

        public void GetCentral2SAPWebAPIProductionReversalData()
        {
            try
            {
                DataTable dtData = DLGetProductionReversalDetailsToSAPPost();
                if (dtData.Rows.Count > 0)
                {
                    dsMTMData = fnPIGetProductionReversalDataSAPPost(dtData);
                    UpdateResponceSAP2CentralProductionReversalDataPost(dsMTMData);
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection != null)
                {
                    if (oDbmCentral.Connection.State == ConnectionState.Open)
                        oDbmCentral.Close();
                }
            }
        }

        public void GetCentral2SAPWebAPIMatDamageData()
        {
            try
            {
                DataTable dtData = DLGetMatDamageDetailsToSAPPost();
                if (dtData.Rows.Count > 0)
                {
                    dsMTMData = fnPIGetMTMTransferDataSAPPost(dtData);
                    UpdateResponceSAP2CentralMatDamageDataPost(dsMTMData);
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection != null)
                {
                    if (oDbmCentral.Connection.State == ConnectionState.Open)
                        oDbmCentral.Close();
                }
            }
        }

        public void GetCentral2SAPWebAPIStackHistoryData()
        {
            try
            {
                DataTable dtData = DLGetStackHistoryDetailsToSAPPost();
                if (dtData.Rows.Count > 0)
                {
                    dsMTMData = fnPIGetStackHistoryDataSAPPost(dtData);
                    UpdateResponceSAP2CentralStackHistoryDataPost(dsMTMData);
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection != null)
                {
                    if (oDbmCentral.Connection.State == ConnectionState.Open)
                        oDbmCentral.Close();
                }
            }
        }

        #endregion


        #region Get Data from Central Server

        public DataTable DLGetProductionDetailsToSAPPost()
        {
            dtStock = new DataTable();
            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                clsGlobal.AppLog.WriteLog("DataScheduler : GetStockDetailsToSAPPost :: ConnectionString :: " + oDbmCentral.ConnectionString);
                oDbmCentral.CreateParameters(1);
                oDbmCentral.AddParameters(0, "@Type", "mGetCentralToSAPProductionData");
                dtStock = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DataTransferCentralToSAP").Tables[0];
                clsGlobal.AppLog.WriteLog("DataScheduler : GetStockDetailsToSAPPost : Total No. of Records Found - " + dtStock.Rows.Count + " of Production Data");
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("DataScheduler : GetStockDetailsToSAPPost :: Error is - " + ex.Message.ToString() + " of Production Data");
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
            return dtStock;
        }

        public DataTable DLGetDoorProductionDetailsToSAPPost()
        {
            dtStock = new DataTable();
            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                clsGlobal.AppLog.WriteLog("DataScheduler : GetStockDetailsToSAPPost :: ConnectionString :: " + oDbmCentral.ConnectionString);
                oDbmCentral.CreateParameters(1);
                oDbmCentral.AddParameters(0, "@Type", "mGetCentralToSAPDoorProductionData");
                dtStock = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DataTransferCentralToSAP").Tables[0];
                clsGlobal.AppLog.WriteLog("DataScheduler : GetStockDetailsToSAPPost : Total No. of Records Found - " + dtStock.Rows.Count + " of Production Data");
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("DataScheduler : GetStockDetailsToSAPPost :: Error is - " + ex.Message.ToString() + " of Production Data");
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
            return dtStock;
        }

        public DataTable DLGetHubStockDetailsToSAPPost()
        {
            dtVendor = new DataTable();
            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                clsGlobal.AppLog.WriteLog("DataScheduler : GetHubDataToSAPPost :: ConnectionString :: " + oDbmCentral.ConnectionString);
                oDbmCentral.CreateParameters(1);
                oDbmCentral.AddParameters(0, "@Type", "mGetCentralToSAPHubStockData");
                dtVendor = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DataTransferCentralToSAP").Tables[0];
                clsGlobal.AppLog.WriteLog("DataScheduler : GetHubDataToSAPPost :: No. of Records Found - " + dtVendor.Rows.Count + " for Central to SAP Posting");
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("DataScheduler : GetHubDataToSAPPost :: Error is - " + ex.Message.ToString() + " for Central To SAP Posting");
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
            return dtVendor;
        }

        public DataTable DLGetHubQualityDetailsToSAPPost()
        {
            dtVendor = new DataTable();
            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                clsGlobal.AppLog.WriteLog("DataScheduler : GetHubQualityDataToSAPPost :: ConnectionString :: " + oDbmCentral.ConnectionString);
                oDbmCentral.CreateParameters(1);
                oDbmCentral.AddParameters(0, "@Type", "mGetCentralToSAPHubQualityData");
                dtVendor = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DataTransferCentralToSAP").Tables[0];
                clsGlobal.AppLog.WriteLog("DataScheduler : GetHubQualityDataToSAPPost :: No. of Records Found - " + dtVendor.Rows.Count + " for Central to SAP Posting");
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("DataScheduler : GetHubQualityDataToSAPPost :: Error is - " + ex.Message.ToString() + " for Central To SAP Posting");
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
            return dtVendor;
        }

        public DataTable DLGetHubQualityBOILDetailsToSAPPost()
        {
            dtVendor = new DataTable();
            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                clsGlobal.AppLog.WriteLog("DataScheduler : GetHubQualityBOILDataToSAPPost :: ConnectionString :: " + oDbmCentral.ConnectionString);
                oDbmCentral.CreateParameters(1);
                oDbmCentral.AddParameters(0, "@Type", "mGetCentralToSAPHubBOILData");
                dtVendor = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DataTransferCentralToSAP").Tables[0];
                clsGlobal.AppLog.WriteLog("DataScheduler : GetHubQualityBOILDataToSAPPost :: No. of Records Found - " + dtVendor.Rows.Count + " for Central to SAP Posting");
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("DataScheduler : GetHubQualityBOILDataToSAPPost :: Error is - " + ex.Message.ToString() + " for Central To SAP Posting");
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
            return dtVendor;
        }

        public DataTable DLGetVendorDetailsToSAPPost()
        {
            dtVendor = new DataTable();
            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                clsGlobal.AppLog.WriteLog("DataScheduler : GetVendorDetailsToSAPPost :: ConnectionString :: " + oDbmCentral.ConnectionString);
                oDbmCentral.CreateParameters(1);
                oDbmCentral.AddParameters(0, "@Type", "mGetCentralToSAPVendorDetailsData");
                dtVendor = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DataTransferCentralToSAP").Tables[0];
                clsGlobal.AppLog.WriteLog("DataScheduler : GetVendorDetailsToSAPPost :: No. of Records Found - " + dtVendor.Rows.Count + " for Central to SAP Posting");
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("DataScheduler : GetVendorDetailsToSAPPost :: Error is - " + ex.Message.ToString() + " for Central To SAP Posting");
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
            return dtVendor;
        }

        public DataTable DLGetDeliveryOrderDetailsToSAPPost()
        {
            dtDOData = new DataTable();
            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                clsGlobal.AppLog.WriteLog("DataScheduler : GetDispatchDetailsToSAPPost :: ConnectionString :: " + oDbmCentral.ConnectionString);
                oDbmCentral.CreateParameters(1);
                oDbmCentral.AddParameters(0, "@Type", "mGetCentralToSAPDeliveryOrderData");
                dtDOData = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DataTransferCentralToSAP").Tables[0];
                clsGlobal.AppLog.WriteLog("DataScheduler : GetDispatchDetailsToSAPPost :: No. of Records Found - " + dtDOData.Rows.Count + " for Central to SAP Posting");
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("DataScheduler : GetDispatchDetailsToSAPPost :: Error is - " + ex.Message.ToString() + " for Central To SAP Posting");
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
            return dtDOData;
        }

        public DataTable DLGetCancelledDeliveryOrderDetailsToSAPPost()
        {
            dtDOData = new DataTable();
            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                clsGlobal.AppLog.WriteLog("DataScheduler : GetCancelledDeliveryOrderDetailsToSAPPost :: ConnectionString :: " + oDbmCentral.ConnectionString);
                oDbmCentral.CreateParameters(1);
                oDbmCentral.AddParameters(0, "@Type", "mGetCentralToSAPDeliveryOrderData");
                dtDOData = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DataTransferCentralToSAP").Tables[0];
                clsGlobal.AppLog.WriteLog("DataScheduler : GetCancelledDeliveryOrderDetailsToSAPPost :: No. of Records Found - " + dtDOData.Rows.Count + " for Central to SAP Posting");
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("DataScheduler : GetCancelledDeliveryOrderDetailsToSAPPost :: Error is - " + ex.Message.ToString() + " for Central To SAP Posting");
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
            return dtDOData;
        }

        public DataTable DLGetSalesReturnDetailsToSAPPost()
        {
            dtSRData = new DataTable();
            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                clsGlobal.AppLog.WriteLog("DataScheduler : GetSalesReturnDetailsToSAPPost :: ConnectionString :: " + oDbmCentral.ConnectionString);
                oDbmCentral.CreateParameters(1);
                oDbmCentral.AddParameters(0, "@Type", "mGetCentralToSAPSalesReturnData");
                dtSRData = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DataTransferCentralToSAP").Tables[0];
                clsGlobal.AppLog.WriteLog("DataScheduler : GetSalesReturnDetailsToSAPPost :: No. of Records Found - " + dtSRData.Rows.Count + " for Central to SAP Posting");
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("DataScheduler : GetSalesReturnDetailsToSAPPost :: Error is - " + ex.Message.ToString() + " for Central To SAP Posting");
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
            return dtSRData;
        }

        public DataTable DLGetReturnPurchaseOrderDetailsToSAPPost()
        {
            dtSRData = new DataTable();
            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                clsGlobal.AppLog.WriteLog("DataScheduler : GetReturnPurchaseOrderDetailsToSAPPost :: ConnectionString :: " + oDbmCentral.ConnectionString);
                oDbmCentral.CreateParameters(1);
                oDbmCentral.AddParameters(0, "@Type", "mGetCentralToSAPPurchaseReturnData");
                dtSRData = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DataTransferCentralToSAP").Tables[0];
                clsGlobal.AppLog.WriteLog("DataScheduler : GetReturnPurchaseOrderDetailsToSAPPost :: No. of Records Found - " + dtSRData.Rows.Count + " for Central to SAP Posting");
            }
            catch (Exception ex)
            {
                clsGlobal.AppLog.WriteLog("DataScheduler : GetReturnPurchaseOrderDetailsToSAPPost :: Error is - " + ex.Message.ToString() + " for Central To SAP Posting");
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
            return dtSRData;
        }

        public DataTable DLGetMTMTransferDetailsToSAPPost()
        {
            dtMTMData = new DataTable();
            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                oDbmCentral.CreateParameters(1);
                oDbmCentral.AddParameters(0, "@Type", "mGetCentralToSAPMTMTransferData");
                dtMTMData = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DataTransferCentralToSAP").Tables[0];
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
            return dtMTMData;
        }

        public DataTable DLGetProductionReversalDetailsToSAPPost()
        {
            dtMTMData = new DataTable();
            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                oDbmCentral.CreateParameters(1);
                oDbmCentral.AddParameters(0, "@Type", "mGetCentralToSAPProductionReversalData");
                dtMTMData = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DataTransferCentralToSAP").Tables[0];
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
            return dtMTMData;
        }

        public DataTable DLGetMatDamageDetailsToSAPPost()
        {
            dtMTMData = new DataTable();
            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                oDbmCentral.CreateParameters(1);
                oDbmCentral.AddParameters(0, "@Type", "mGetCentralToSAPMatDamageData");
                dtMTMData = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DataTransferCentralToSAP").Tables[0];
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
            return dtMTMData;
        }

        public DataTable DLGetStackHistoryDetailsToSAPPost()
        {
            dtMTMData = new DataTable();
            try
            {
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                oDbmCentral.CreateParameters(1);
                oDbmCentral.AddParameters(0, "@Type", "mGetCentralToSAPStackHistoryData");
                dtMTMData = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DataTransferCentralToSAP").Tables[0];
            }
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
            return dtMTMData;
        }

        #endregion


        #region Send Data to SAP Server

        public DataSet fnPIGetProductionDataSAPPost(DataTable dtdata)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(new DataTable());
            sLocationCode = string.Empty;
            try
            {

                ZBC_SAP_2_BCClient zbcd = new ZBC_SAP_2_BCClient();

                zbcd.ClientCredentials.UserName.UserName = Properties.Settings.Default.CentralToSAPWebServiceUserID;
                zbcd.ClientCredentials.UserName.Password = Properties.Settings.Default.CentralToSAPWebServiceUserPassword;

                ZbcPlyqrCodePost master = new ZbcPlyqrCodePost();
                ZbcPlyqrCodePostRequest request = new ZbcPlyqrCodePostRequest();
                ZbcPlyqrCodePostResponse responce = new ZbcPlyqrCodePostResponse();

                ZstrBcMatPost[] post = new ZstrBcMatPost[1];
                if (dtdata.Rows.Count > 0)
                {
                    post = new ZstrBcMatPost[dtdata.Rows.Count];
                    for (int i = 0; i < dtdata.Rows.Count; i++)
                    {
                        post[i] = new ZstrBcMatPost();
                        post[i].Plantcode = dtdata.Rows[i][0].ToString();
                        sLocationCode = dtdata.Rows[i][0].ToString();
                        post[i].Tomatcode = dtdata.Rows[i][1].ToString();
                        post[i].Frommatcode = dtdata.Rows[i][2].ToString();
                        post[i].Serial = dtdata.Rows[i][3].ToString();
                        post[i].Status = dtdata.Rows[i][4].ToString();
                        post[i].Stackno = dtdata.Rows[i][5].ToString();
                    }
                }
                request.ZbcPlyqrCodePost = master;
                request.ZbcPlyqrCodePost.DataIn = post;
                responce = zbcd.ZbcPlyqrCodePost(request.ZbcPlyqrCodePost);
                clsGlobal.AppLog.WriteLog("Data Scheduler : Sending Central To SAP Stock Data : " + responce.DataOut.Count() + " No. of Records Found");
                if (responce.DataOut != null)
                {
                    ds.Tables[0].Columns.Add("LocationCode");
                    ds.Tables[0].Columns.Add("ToMatCode");
                    ds.Tables[0].Columns.Add("FromMatCode");
                    ds.Tables[0].Columns.Add("QRCode");
                    ds.Tables[0].Columns.Add("Status");
                    ds.Tables[0].Columns.Add("StackQRCode");
                    ds.Tables[0].Columns.Add("SAPStatus");

                    foreach (var items in responce.DataOut)
                    {
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["LocationCode"] = Convert.ToString(Convert.ToString(items.Plantcode) == null ? "" : Convert.ToString(items.Plantcode));
                        dr["ToMatCode"] = Convert.ToString(Convert.ToString(items.Tomatcode) == null ? "" : Convert.ToString(items.Tomatcode));
                        dr["FromMatCode"] = Convert.ToString(Convert.ToString(items.Frommatcode) == null ? "" : Convert.ToString(items.Frommatcode));
                        dr["QRCode"] = Convert.ToString(Convert.ToString(items.Serial) == null ? "" : Convert.ToString(items.Serial));
                        dr["Status"] = Convert.ToString(Convert.ToString(items.Status) == null ? "" : Convert.ToString(items.Status));
                        dr["StackQRCode"] = Convert.ToString(Convert.ToString(items.Stackno) == null ? "" : Convert.ToString(items.Stackno));
                        dr["SAPStatus"] = Convert.ToString(Convert.ToString(items.Sts) == null ? "" : Convert.ToString(items.Sts));
                        ds.Tables[0].Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending Production Data for LocationCode - " + sLocationCode;
                sResp = sResp + " from Central Server to SAP found error. Kindly look into the central data scheduler log file of respective date for more details";
                clsGlobal.AppLog.WriteLog("Data Scheduler : Error in Web Service Sending Central To SAP Stock Data is : " + ex.Message.ToString());
                SendMail(sResp.ToString());
                //}
                
                //throw ex;
            }
            return ds;
        }

        public DataSet fnPIGetHubDataSAPPost(DataTable dtdata)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(new DataTable());
            sLocationCode = string.Empty;
            try
            {
                ZBC_SAP_2_BCClient zbcd = new ZBC_SAP_2_BCClient();

                zbcd.ClientCredentials.UserName.UserName = Properties.Settings.Default.CentralToSAPWebServiceUserID;
                zbcd.ClientCredentials.UserName.Password = Properties.Settings.Default.CentralToSAPWebServiceUserPassword;

                ZbcVendorPrintPost master = new ZbcVendorPrintPost();
                ZbcVendorPrintPostRequest request = new ZbcVendorPrintPostRequest();
                ZbcVendorPrintPostResponse responce = new ZbcVendorPrintPostResponse();

                ZstrBcVenPrintPost[] post = new ZstrBcVenPrintPost[1];
                if (dtdata.Rows.Count > 0)
                {
                    post = new ZstrBcVenPrintPost[dtdata.Rows.Count];
                    for (int i = 0; i < dtdata.Rows.Count; i++)
                    {
                        post[i] = new ZstrBcVenPrintPost();
                        post[i].Plantcode = dtdata.Rows[i][0].ToString();
                        sLocationCode = dtdata.Rows[i][0].ToString();
                        post[i].Ponum = dtdata.Rows[i][1].ToString();
                        post[i].Vendinvno = dtdata.Rows[i][2].ToString();
                        post[i].Vendinvdt = dtdata.Rows[i][3].ToString();
                        post[i].Vendcode = dtdata.Rows[i][4].ToString();
                        post[i].Matcode = dtdata.Rows[i][5].ToString();
                        post[i].Poqty = dtdata.Rows[i][6].ToString();
                        post[i].Serial = dtdata.Rows[i][7].ToString();
                        post[i].Poloctype = dtdata.Rows[i][8].ToString();
                    }
                }
                request.ZbcVendorPrintPost = master;
                request.ZbcVendorPrintPost.DataIn = post;
                responce = zbcd.ZbcVendorPrintPost(request.ZbcVendorPrintPost);
                clsGlobal.AppLog.WriteLog("Data Scheduler : Sending Central To SAP Hub Printing Data : " + responce.DataOut.Count() + " No. of Records Found");
                if (responce.DataOut != null)
                {
                    ds.Tables[0].Columns.Add("LocationCode");
                    ds.Tables[0].Columns.Add("PONo");
                    ds.Tables[0].Columns.Add("VendorInvNo");
                    ds.Tables[0].Columns.Add("VendorInvDate");
                    ds.Tables[0].Columns.Add("VendorCode");
                    ds.Tables[0].Columns.Add("MatCode");
                    ds.Tables[0].Columns.Add("QRCode");
                    ds.Tables[0].Columns.Add("LocType");
                    ds.Tables[0].Columns.Add("SAPStatus");

                    foreach (var items in responce.DataOut)
                    {
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["LocationCode"] = Convert.ToString(Convert.ToString(items.Plantcode) == null ? "" : Convert.ToString(items.Plantcode));
                        dr["PONo"] = Convert.ToString(Convert.ToString(items.Ponum) == null ? "" : Convert.ToString(items.Ponum));
                        dr["VendorInvNo"] = Convert.ToString(Convert.ToString(items.Vendinvno) == null ? "" : Convert.ToString(items.Vendinvno));
                        dr["VendorInvDate"] = Convert.ToString(Convert.ToString(items.Vendinvdt) == null ? "" : Convert.ToString(items.Vendinvdt));
                        dr["VendorCode"] = Convert.ToString(Convert.ToString(items.Vendcode) == null ? "" : Convert.ToString(items.Vendcode));
                        dr["MatCode"] = Convert.ToString(Convert.ToString(items.Matcode) == null ? "" : Convert.ToString(items.Matcode));
                        dr["QRCode"] = Convert.ToString(Convert.ToString(items.Serial) == null ? "" : Convert.ToString(items.Serial));
                        dr["LocType"] = Convert.ToString(Convert.ToString(items.Poloctype) == null ? "" : Convert.ToString(items.Poloctype));
                        dr["SAPStatus"] = Convert.ToString(Convert.ToString(items.Sts) == null ? "" : Convert.ToString(items.Sts));
                        ds.Tables[0].Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending Production History Data for LocationCode - " + sLocationCode;
                sResp = sResp + " from Central Server to SAP found error. Kindly look into the central data scheduler log file of respective date for more details";
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Web Service Sending Hub Printing Data : " + ex.Message.ToString());
                SendMail(sResp.ToString());
                //}
               
            }
            return ds;
        }

        public DataSet fnPIGetHubQualityDataSAPPost(DataTable dtdata)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(new DataTable());
            sLocationCode = string.Empty;
            try
            {
                ZBC_SAP_2_BCClient zbcd = new ZBC_SAP_2_BCClient();

                zbcd.ClientCredentials.UserName.UserName = Properties.Settings.Default.CentralToSAPWebServiceUserID;
                zbcd.ClientCredentials.UserName.Password = Properties.Settings.Default.CentralToSAPWebServiceUserPassword;
                
                ZbcQcPost master = new ZbcQcPost();
                ZbcQcPostRequest request = new ZbcQcPostRequest();
                ZbcQcPostResponse responce = new ZbcQcPostResponse();

                ZstrBcQcPost[] post = new ZstrBcQcPost[1];
                if (dtdata.Rows.Count > 0)
                {
                    post = new ZstrBcQcPost[dtdata.Rows.Count];
                    for (int i = 0; i < dtdata.Rows.Count; i++)
                    {
                        post[i] = new ZstrBcQcPost();
                        post[i].Plantcode = dtdata.Rows[i][0].ToString();
                        sLocationCode = dtdata.Rows[i][0].ToString();
                        post[i].Pono = dtdata.Rows[i][1].ToString();
                        post[i].Matcode = dtdata.Rows[i][2].ToString();
                        post[i].NewMatcode = dtdata.Rows[i][3].ToString();
                        post[i].Serial = dtdata.Rows[i][4].ToString();
                        post[i].Vendcode = dtdata.Rows[i][5].ToString();
                        post[i].Migono = dtdata.Rows[i][6].ToString();
                        post[i].Inslotno = dtdata.Rows[i][7].ToString();
                        post[i].Defectcode = dtdata.Rows[i][8].ToString();
                        post[i].Qcstatus = dtdata.Rows[i][9].ToString();
                    }
                }
                request.ZbcQcPost = master;
                request.ZbcQcPost.DataIn = post;
                responce = zbcd.ZbcQcPost(request.ZbcQcPost);
                clsGlobal.AppLog.WriteLog("Data Scheduler : Sending Central To SAP HubQuality Data : " + responce.DataOut.Count() + " No. of Records Found");
                if (responce.DataOut != null)
                {
                    ds.Tables[0].Columns.Add("LocationCode");
                    ds.Tables[0].Columns.Add("PONo");
                    ds.Tables[0].Columns.Add("MatCode");
                    ds.Tables[0].Columns.Add("NewMatCode");
                    ds.Tables[0].Columns.Add("QRCode");
                    ds.Tables[0].Columns.Add("VendorCode");
                    ds.Tables[0].Columns.Add("MIGONo");
                    ds.Tables[0].Columns.Add("InspLotNo");
                    ds.Tables[0].Columns.Add("RejectionCode");
                    ds.Tables[0].Columns.Add("QCStatus");
                    ds.Tables[0].Columns.Add("SAPStatus");

                    foreach (var items in responce.DataOut)
                    {
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["LocationCode"] = Convert.ToString(Convert.ToString(items.Plantcode) == null ? "" : Convert.ToString(items.Plantcode));
                        dr["PONo"] = Convert.ToString(Convert.ToString(items.Pono) == null ? "" : Convert.ToString(items.Pono));
                        dr["MatCode"] = Convert.ToString(Convert.ToString(items.Matcode) == null ? "" : Convert.ToString(items.Matcode));
                        dr["NewMatCode"] = Convert.ToString(Convert.ToString(items.NewMatcode) == null ? "" : Convert.ToString(items.NewMatcode));
                        dr["QRCode"] = Convert.ToString(Convert.ToString(items.Serial) == null ? "" : Convert.ToString(items.Serial));
                        dr["VendorCode"] = Convert.ToString(Convert.ToString(items.Vendcode) == null ? "" : Convert.ToString(items.Vendcode));
                        dr["MIGONo"] = Convert.ToString(Convert.ToString(items.Migono) == null ? "" : Convert.ToString(items.Migono));
                        dr["InspLotNo"] = Convert.ToString(Convert.ToString(items.Inslotno) == null ? "" : Convert.ToString(items.Inslotno));
                        dr["RejectionCode"] = Convert.ToString(Convert.ToString(items.Defectcode) == null ? "" : Convert.ToString(items.Defectcode));
                        dr["QCStatus"] = Convert.ToString(Convert.ToString(items.Qcstatus) == null ? "" : Convert.ToString(items.Qcstatus));
                        dr["SAPStatus"] = Convert.ToString(Convert.ToString(items.Sts) == null ? "" : Convert.ToString(items.Sts));
                        ds.Tables[0].Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                string sResp = "Sending HubQuality Data for LocationCode - " + sLocationCode;
                sResp = sResp + " from Central Server to SAP found error. Kindly look into the central data scheduler log file of respective date for more details";
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Web Service Sending HubQuality Data : " + ex.Message.ToString());
                SendMail(sResp.ToString());
            }
            return ds;
        }

        public DataSet fnPIGetHubQualityBOILDataSAPPost(DataTable dtdata)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(new DataTable());
            sLocationCode = string.Empty;
            try
            {
                ZBC_SAP_2_BCClient zbcd = new ZBC_SAP_2_BCClient();

                zbcd.ClientCredentials.UserName.UserName = Properties.Settings.Default.CentralToSAPWebServiceUserID;
                zbcd.ClientCredentials.UserName.Password = Properties.Settings.Default.CentralToSAPWebServiceUserPassword;

                ZbcQcPostBoilingtest master = new ZbcQcPostBoilingtest();
                ZbcQcPostBoilingtestRequest request = new ZbcQcPostBoilingtestRequest();
                ZbcQcPostBoilingtestResponse responce = new ZbcQcPostBoilingtestResponse();

                ZstrBcQcPost1[] post = new ZstrBcQcPost1[1];
                if (dtdata.Rows.Count > 0)
                {
                    post = new ZstrBcQcPost1[dtdata.Rows.Count];
                    for (int i = 0; i < dtdata.Rows.Count; i++)
                    {
                        post[i] = new ZstrBcQcPost1();
                        post[i].Plantcode = dtdata.Rows[i][0].ToString();
                        sLocationCode = dtdata.Rows[i][0].ToString();
                        post[i].Pono = dtdata.Rows[i][1].ToString();
                        post[i].Matcode = dtdata.Rows[i][2].ToString();
                        post[i].NewMatcode = dtdata.Rows[i][2].ToString();
                        post[i].Serial = dtdata.Rows[i][3].ToString();
                        post[i].Vendcode = dtdata.Rows[i][4].ToString();
                        post[i].Migono = dtdata.Rows[i][5].ToString();
                        post[i].Inslotno = dtdata.Rows[i][6].ToString();
                        //post[i].Defectcode = dtdata.Rows[i][7].ToString();
                        post[i].Qcstatus = dtdata.Rows[i][8].ToString();
                    }
                }
                request.ZbcQcPostBoilingtest = master;
                request.ZbcQcPostBoilingtest.DataIn = post;
                responce = zbcd.ZbcQcPostBoilingtest(request.ZbcQcPostBoilingtest);
                clsGlobal.AppLog.WriteLog("Data Scheduler : Sending Central To SAP Hub Printing Data : " + responce.DataOut.Count() + " No. of Records Found");
                if (responce.DataOut != null)
                {
                    ds.Tables[0].Columns.Add("LocationCode");
                    ds.Tables[0].Columns.Add("PONo");
                    ds.Tables[0].Columns.Add("MatCode");
                    ds.Tables[0].Columns.Add("NewMatCode");
                    ds.Tables[0].Columns.Add("QRCode");
                    ds.Tables[0].Columns.Add("VendorCode");
                    ds.Tables[0].Columns.Add("MIGONo");
                    ds.Tables[0].Columns.Add("InspLotNo");
                    ds.Tables[0].Columns.Add("RejectionCode");
                    ds.Tables[0].Columns.Add("SAPStatus");

                    foreach (var items in responce.DataOut)
                    {
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["LocationCode"] = Convert.ToString(Convert.ToString(items.Plantcode) == null ? "" : Convert.ToString(items.Plantcode));
                        dr["PONo"] = Convert.ToString(Convert.ToString(items.Pono) == null ? "" : Convert.ToString(items.Pono));
                        dr["MatCode"] = Convert.ToString(Convert.ToString(items.Matcode) == null ? "" : Convert.ToString(items.Matcode));
                        dr["NewMatCode"] = Convert.ToString(Convert.ToString(items.NewMatcode) == null ? "" : Convert.ToString(items.NewMatcode));
                        dr["QRCode"] = Convert.ToString(Convert.ToString(items.Serial) == null ? "" : Convert.ToString(items.Serial));
                        dr["VendorCode"] = Convert.ToString(Convert.ToString(items.Vendcode) == null ? "" : Convert.ToString(items.Vendcode));
                        dr["MIGONo"] = Convert.ToString(Convert.ToString(items.Migono) == null ? "" : Convert.ToString(items.Migono));
                        dr["InspLotNo"] = Convert.ToString(Convert.ToString(items.Inslotno) == null ? "" : Convert.ToString(items.Inslotno));
                        //dr["RejectionCode"] = Convert.ToString(Convert.ToString(items.Defectcode) == null ? "" : Convert.ToString(items.Defectcode));
                        dr["SAPStatus"] = Convert.ToString(Convert.ToString(items.Sts) == null ? "" : Convert.ToString(items.Sts));
                        ds.Tables[0].Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                string sResp = "Sending LotRejection Data for LocationCode - " + sLocationCode;
                sResp = sResp + " from Central Server to SAP found error. Kindly look into the central data scheduler log file of respective date for more details";
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Web Service Sending LotRejection Data : " + ex.Message.ToString());
                SendMail(sResp.ToString());
            }
            return ds;
        }

        public DataSet fnPIGetVendorDataSAPPost(DataTable dtdata)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(new DataTable());
            sLocationCode = string.Empty;
            try
            {
                ZBC_SAP_2_BCClient zbcd = new ZBC_SAP_2_BCClient();

                zbcd.ClientCredentials.UserName.UserName = Properties.Settings.Default.CentralToSAPWebServiceUserID;
                zbcd.ClientCredentials.UserName.Password = Properties.Settings.Default.CentralToSAPWebServiceUserPassword;

                ZbcVendorPrintPost master = new ZbcVendorPrintPost();
                ZbcVendorPrintPostRequest request = new ZbcVendorPrintPostRequest();
                ZbcVendorPrintPostResponse responce = new ZbcVendorPrintPostResponse();

                ZstrBcVenPrintPost[] post = new ZstrBcVenPrintPost[1];
                if (dtdata.Rows.Count > 0)
                {
                    post = new ZstrBcVenPrintPost[dtdata.Rows.Count];
                    for (int i = 0; i < dtdata.Rows.Count; i++)
                    {
                        post[i] = new ZstrBcVenPrintPost();
                        post[i].Plantcode = dtdata.Rows[i][0].ToString();
                        sLocationCode = dtdata.Rows[i][0].ToString();
                        post[i].Ponum = dtdata.Rows[i][1].ToString();
                        post[i].Vendinvno = dtdata.Rows[i][2].ToString();
                        post[i].Vendinvdt = dtdata.Rows[i][3].ToString();
                        post[i].Vendcode = dtdata.Rows[i][4].ToString();
                        post[i].Matcode = dtdata.Rows[i][5].ToString();
                        post[i].Poqty = dtdata.Rows[i][6].ToString();
                        post[i].Serial = dtdata.Rows[i][7].ToString();
                        post[i].Poloctype = dtdata.Rows[i][8].ToString();
                    }
                }
                request.ZbcVendorPrintPost = master;
                request.ZbcVendorPrintPost.DataIn = post;
                responce = zbcd.ZbcVendorPrintPost(request.ZbcVendorPrintPost);
                clsGlobal.AppLog.WriteLog("Data Scheduler : Sending Central To SAP Vendor Printing Data : " + responce.DataOut.Count() + " No. of Records Found");
                if (responce.DataOut != null)
                {
                    ds.Tables[0].Columns.Add("LocationCode");
                    ds.Tables[0].Columns.Add("PONo");
                    ds.Tables[0].Columns.Add("VendorInvNo");
                    ds.Tables[0].Columns.Add("VendorInvDate");
                    ds.Tables[0].Columns.Add("VendorCode");
                    ds.Tables[0].Columns.Add("MatCode");
                    ds.Tables[0].Columns.Add("Qty");
                    ds.Tables[0].Columns.Add("QRCode");
                    ds.Tables[0].Columns.Add("LocType");
                    ds.Tables[0].Columns.Add("SAPStatus");

                    foreach (var items in responce.DataOut)
                    {
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["LocationCode"] = Convert.ToString(Convert.ToString(items.Plantcode) == null ? "" : Convert.ToString(items.Plantcode));
                        dr["PONo"] = Convert.ToString(Convert.ToString(items.Ponum) == null ? "" : Convert.ToString(items.Ponum));
                        dr["VendorInvNo"] = Convert.ToString(Convert.ToString(items.Vendinvno) == null ? "" : Convert.ToString(items.Vendinvno));
                        dr["VendorInvDate"] = Convert.ToString(Convert.ToString(items.Vendinvdt) == null ? "" : Convert.ToString(items.Vendinvdt));
                        dr["VendorCode"] = Convert.ToString(Convert.ToString(items.Vendcode) == null ? "" : Convert.ToString(items.Vendcode));
                        dr["MatCode"] = Convert.ToString(Convert.ToString(items.Matcode) == null ? "" : Convert.ToString(items.Matcode));
                        dr["Qty"] = Convert.ToString(Convert.ToString(items.Poqty) == null ? "" : Convert.ToString(items.Poqty));
                        dr["QRCode"] = Convert.ToString(Convert.ToString(items.Serial) == null ? "" : Convert.ToString(items.Serial));
                        dr["LocType"] = Convert.ToString(Convert.ToString(items.Poloctype) == null ? "" : Convert.ToString(items.Poloctype));
                        dr["SAPStatus"] = Convert.ToString(Convert.ToString(items.Sts) == null ? "" : Convert.ToString(items.Sts));
                        ds.Tables[0].Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending Vendor Printing Data for LocationCode - " + sLocationCode;
                sResp = sResp + " from Central Server to SAP found error. Kindly look into the central data scheduler log file of respective date for more details";
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Web Service Sending Vendor Printing Data : " + ex.Message.ToString());
                SendMail(sResp.ToString());
                //}
            }
            return ds;
        }

        public DataSet fnPIGetDeliveryOrderDataSAPPost(DataTable dtdata)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(new DataTable());
            sLocationCode = string.Empty;
            try
            {
                ZBC_SAP_2_BCClient zbcd = new ZBC_SAP_2_BCClient();

                zbcd.ClientCredentials.UserName.UserName = Properties.Settings.Default.CentralToSAPWebServiceUserID;
                zbcd.ClientCredentials.UserName.Password = Properties.Settings.Default.CentralToSAPWebServiceUserPassword;

                ZbcDeliveryOrderPost master = new ZbcDeliveryOrderPost();
                ZbcDeliveryOrderPostRequest request = new ZbcDeliveryOrderPostRequest();
                ZbcDeliveryOrderPostResponse responce = new ZbcDeliveryOrderPostResponse();

                ZstrSdBcDelvPost[] post = new ZstrSdBcDelvPost[1];
                if (dtdata.Rows.Count > 0)
                {
                    post = new ZstrSdBcDelvPost[dtdata.Rows.Count];
                    for (int i = 0; i < dtdata.Rows.Count; i++)
                    {
                        post[i] = new ZstrSdBcDelvPost();
                        post[i].Plantcode = dtdata.Rows[i][0].ToString();
                        post[i].Deliveryno = dtdata.Rows[i][1].ToString();
                        post[i].Matcode = dtdata.Rows[i][2].ToString();
                        post[i].Qty = "1";
                        post[i].Serial = dtdata.Rows[i][4].ToString();
                    }
                }
                request.ZbcDeliveryOrderPost = master;
                request.ZbcDeliveryOrderPost.DataIn = post;
                responce = zbcd.ZbcDeliveryOrderPost(request.ZbcDeliveryOrderPost);
                clsGlobal.AppLog.WriteLog("Data Scheduler : Sending Central To SAP Dispatch Data : " + responce.DataOut.Count() + " No. of Records Found");
                if (responce.DataOut != null)
                {
                    ds.Tables[0].Columns.Add("LocationCode");
                    ds.Tables[0].Columns.Add("DONo");
                    ds.Tables[0].Columns.Add("MatCode");
                    ds.Tables[0].Columns.Add("QRCode");
                    ds.Tables[0].Columns.Add("SAPStatus");

                    foreach (var items in responce.DataOut)
                    {
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["LocationCode"] = Convert.ToString(Convert.ToString(items.Plantcode) == null ? "" : Convert.ToString(items.Plantcode));
                        dr["DONo"] = Convert.ToString(Convert.ToString(items.Deliveryno) == null ? "" : Convert.ToString(items.Deliveryno));
                        dr["MatCode"] = Convert.ToString(Convert.ToString(items.Matcode) == null ? "" : Convert.ToString(items.Matcode));
                        dr["QRCode"] = Convert.ToString(Convert.ToString(items.Serial) == null ? "" : Convert.ToString(items.Serial));
                        dr["SAPStatus"] = Convert.ToString(Convert.ToString(items.Sts) == null ? "" : Convert.ToString(items.Sts));
                        ds.Tables[0].Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending Dispatch Data for LocationCode - " + sLocationCode;
                sResp = sResp + " from Central Server to SAP found error. Kindly look into the central data scheduler log file of respective date for more details";
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Web Service Sending Dispatch Data : " + ex.Message.ToString());
                SendMail(sResp.ToString());
                //}
            }
            return ds;
        }

        public DataSet fnPIGetCancelledDeliveryOrderDataSAPPost(DataTable dtdata)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(new DataTable());
            try
            {
                ZBC_SAP_2_BCClient zbcd = new ZBC_SAP_2_BCClient();

                zbcd.ClientCredentials.UserName.UserName = Properties.Settings.Default.CentralToSAPWebServiceUserID;
                zbcd.ClientCredentials.UserName.Password = Properties.Settings.Default.CentralToSAPWebServiceUserPassword;

                ZbcDeliveryCancellPost master = new ZbcDeliveryCancellPost();
                ZbcDeliveryCancellPostRequest request = new ZbcDeliveryCancellPostRequest();
                ZbcDeliveryCancellPostResponse responce = new ZbcDeliveryCancellPostResponse();

                ZstrSdBcDelvPost[] post = new ZstrSdBcDelvPost[1];
                if (dtdata.Rows.Count > 0)
                {
                    post = new ZstrSdBcDelvPost[dtdata.Rows.Count];
                    for (int i = 0; i < dtdata.Rows.Count; i++)
                    {
                        post[i] = new ZstrSdBcDelvPost();
                        post[i].Plantcode = dtdata.Rows[i][0].ToString();
                        post[i].Deliveryno = dtdata.Rows[i][1].ToString();
                        post[i].Matcode = dtdata.Rows[i][2].ToString();
                        post[i].Qty = "1";
                        post[i].Serial = dtdata.Rows[i][4].ToString();
                    }
                }
                request.ZbcDeliveryCancellPost = master;
                request.ZbcDeliveryCancellPost.DataIn = post;
                responce = zbcd.ZbcDeliveryCancellPost(request.ZbcDeliveryCancellPost);
                clsGlobal.AppLog.WriteLog("Data Scheduler : Sending Central To SAP Dispatch Data : " + responce.DataOut.Count() + " No. of Records Found");
                if (responce.DataOut != null)
                {
                    ds.Tables[0].Columns.Add("LocationCode");
                    ds.Tables[0].Columns.Add("DONo");
                    ds.Tables[0].Columns.Add("MatCode");
                    ds.Tables[0].Columns.Add("QRCode");
                    ds.Tables[0].Columns.Add("SAPStatus");

                    foreach (var items in responce.DataOut)
                    {
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["LocationCode"] = Convert.ToString(Convert.ToString(items.Plantcode) == null ? "" : Convert.ToString(items.Plantcode));
                        dr["DONo"] = Convert.ToString(Convert.ToString(items.Deliveryno) == null ? "" : Convert.ToString(items.Deliveryno));
                        dr["MatCode"] = Convert.ToString(Convert.ToString(items.Matcode) == null ? "" : Convert.ToString(items.Matcode));
                        dr["QRCode"] = Convert.ToString(Convert.ToString(items.Serial) == null ? "" : Convert.ToString(items.Serial));
                        dr["SAPStatus"] = Convert.ToString(Convert.ToString(items.Sts) == null ? "" : Convert.ToString(items.Sts));
                        ds.Tables[0].Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending Cancelled Dispatch Data for LocationCode - " + sLocationCode;
                sResp = sResp + " from Central Server to SAP found error. Kindly look into the central data scheduler log file of respective date for more details";
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Web Service Sending Dispatch Data : " + ex.Message.ToString());
                SendMail(sResp.ToString());
                //}
            }
            return ds;
        }

        public DataSet fnPIGetReturnPurchaseOrderDataSAPPost(DataTable dtdata)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(new DataTable());
            try
            {
                ZBC_SAP_2_BCClient zbcd = new ZBC_SAP_2_BCClient();

                zbcd.ClientCredentials.UserName.UserName = Properties.Settings.Default.CentralToSAPWebServiceUserID;
                zbcd.ClientCredentials.UserName.Password = Properties.Settings.Default.CentralToSAPWebServiceUserPassword;

                ZbcPoPostRet master = new ZbcPoPostRet();
                ZbcPoPostRetRequest request = new ZbcPoPostRetRequest();
                ZbcPoPostRetResponse responce = new ZbcPoPostRetResponse();

                ZstrBcPoPost[] post = new ZstrBcPoPost[1];
                if (dtdata.Rows.Count > 0)
                {
                    post = new ZstrBcPoPost[dtdata.Rows.Count];
                    for (int i = 0; i < dtdata.Rows.Count; i++)
                    {
                        post[i] = new ZstrBcPoPost();
                        post[i].Plantcode = dtdata.Rows[i][0].ToString();
                        post[i].Ponum = dtdata.Rows[i][1].ToString();
                        post[i].Matcode = dtdata.Rows[i][2].ToString();
                        post[i].Vendcode = dtdata.Rows[i][3].ToString();
                        post[i].Serial = dtdata.Rows[i][4].ToString();
                    }
                }
                request.ZbcPoPostRet = master;
                request.ZbcPoPostRet.DataIn = post;
                responce = zbcd.ZbcPoPostRet(request.ZbcPoPostRet);
                clsGlobal.AppLog.WriteLog("Data Scheduler : Sending Central To SAP Purchase Return Data : " + responce.DataOut.Count() + " No. of Records Found");
                if (responce.DataOut != null)
                {
                    ds.Tables[0].Columns.Add("LocationCode");
                    ds.Tables[0].Columns.Add("PONo");
                    ds.Tables[0].Columns.Add("MatCode");
                    ds.Tables[0].Columns.Add("QRCode");
                    ds.Tables[0].Columns.Add("VendorCode");
                    ds.Tables[0].Columns.Add("SAPStatus");

                    foreach (var items in responce.DataOut)
                    {
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["LocationCode"] = Convert.ToString(Convert.ToString(items.Plantcode) == null ? "" : Convert.ToString(items.Plantcode));
                        dr["PONo"] = Convert.ToString(Convert.ToString(items.Ponum) == null ? "" : Convert.ToString(items.Ponum));
                        dr["MatCode"] = Convert.ToString(Convert.ToString(items.Matcode) == null ? "" : Convert.ToString(items.Matcode));
                        dr["QRCode"] = Convert.ToString(Convert.ToString(items.Serial) == null ? "" : Convert.ToString(items.Serial));
                        dr["VendorCode"] = Convert.ToString(Convert.ToString(items.Vendcode) == null ? "" : Convert.ToString(items.Vendcode));
                        dr["SAPStatus"] = Convert.ToString(Convert.ToString(items.Sts) == null ? "" : Convert.ToString(items.Sts));
                        ds.Tables[0].Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending Return Purchase Order Data for LocationCode - " + sLocationCode;
                sResp = sResp + " from Central Server to SAP found error. Kindly look into the central data scheduler log file of respective date for more details";
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Web Service Sending Purchase Return Data : " + ex.Message.ToString());
                SendMail(sResp.ToString());
                //}
            }
            return ds;
        }

        public DataSet fnPIGetSalesReturnDataSAPPost(DataTable dtdata)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(new DataTable());
            try
            {
                ZBC_SAP_2_BCClient zbcd = new ZBC_SAP_2_BCClient();

                zbcd.ClientCredentials.UserName.UserName = Properties.Settings.Default.CentralToSAPWebServiceUserID;
                zbcd.ClientCredentials.UserName.Password = Properties.Settings.Default.CentralToSAPWebServiceUserPassword;

                ZbcDelvOrderPostRet master = new ZbcDelvOrderPostRet();
                ZbcDelvOrderPostRetRequest request = new ZbcDelvOrderPostRetRequest();
                ZbcDelvOrderPostRetResponse responce = new ZbcDelvOrderPostRetResponse();

                ZstrSdBcDelvPost[] post = new ZstrSdBcDelvPost[1];
                if (dtdata.Rows.Count > 0)
                {
                    post = new ZstrSdBcDelvPost[dtdata.Rows.Count];
                    for (int i = 0; i < dtdata.Rows.Count; i++)
                    {
                        post[i] = new ZstrSdBcDelvPost();
                        post[i].Plantcode = dtdata.Rows[i][0].ToString();
                        post[i].Deliveryno = dtdata.Rows[i][1].ToString();
                        post[i].Matcode = dtdata.Rows[i][2].ToString();
                        post[i].Qty = dtdata.Rows[i][3].ToString();
                        post[i].Serial = dtdata.Rows[i][4].ToString();
                    }
                }
                request.ZbcDelvOrderPostRet = master;
                request.ZbcDelvOrderPostRet.DataIn = post;
                responce = zbcd.ZbcDelvOrderPostRet(request.ZbcDelvOrderPostRet);
                clsGlobal.AppLog.WriteLog("Data Scheduler : Sending Central To SAP Sales Return Data : " + responce.DataOut.Count() + " No. of Records Found");
                if (responce.DataOut != null)
                {
                    ds.Tables[0].Columns.Add("LocationCode");
                    ds.Tables[0].Columns.Add("SalesReturnNo");
                    ds.Tables[0].Columns.Add("MatCode");
                    ds.Tables[0].Columns.Add("QRCode");
                    ds.Tables[0].Columns.Add("Qty");
                    ds.Tables[0].Columns.Add("SAPStatus");

                    foreach (var items in responce.DataOut)
                    {
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["LocationCode"] = Convert.ToString(Convert.ToString(items.Plantcode) == null ? "" : Convert.ToString(items.Plantcode));
                        dr["SalesReturnNo"] = Convert.ToString(Convert.ToString(items.Deliveryno) == null ? "" : Convert.ToString(items.Deliveryno));
                        dr["MatCode"] = Convert.ToString(Convert.ToString(items.Matcode) == null ? "" : Convert.ToString(items.Matcode));
                        dr["QRCode"] = Convert.ToString(Convert.ToString(items.Serial) == null ? "" : Convert.ToString(items.Serial));
                        dr["Qty"] = Convert.ToString(Convert.ToString(items.Qty) == null ? "" : Convert.ToString(items.Qty));
                        dr["SAPStatus"] = Convert.ToString(Convert.ToString(items.Sts) == null ? "" : Convert.ToString(items.Sts));
                        ds.Tables[0].Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending Sales Return Order Data for LocationCode - " + sLocationCode;
                sResp = sResp + " from Central Server to SAP found error. Kindly look into the central data scheduler log file of respective date for more details";
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Web Service Sending Sales Return Data : " + ex.Message.ToString());
                SendMail(sResp.ToString());
                //}
            }
            return ds;
        }

        public DataSet fnPIGetProductionReversalDataSAPPost(DataTable dtdata)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(new DataTable());
            try
            {
                ZBC_SAP_2_BCClient zbcd = new ZBC_SAP_2_BCClient();
                //ZBC_SAP_2_BCClient zbcd = new ZBC_SAP_2_BCClient();

                zbcd.ClientCredentials.UserName.UserName = Properties.Settings.Default.CentralToSAPWebServiceUserID;
                zbcd.ClientCredentials.UserName.Password = Properties.Settings.Default.CentralToSAPWebServiceUserPassword;

                ZbcPlyqrCodeRevPost master = new ZbcPlyqrCodeRevPost();
                ZbcPlyqrCodeRevPostRequest request = new ZbcPlyqrCodeRevPostRequest();
                ZbcPlyqrCodeRevPostResponse responce = new ZbcPlyqrCodeRevPostResponse();

                ZstrBcMatRev[] post = new ZstrBcMatRev[1];
                if (dtdata.Rows.Count > 0)
                {
                    post = new ZstrBcMatRev[dtdata.Rows.Count];
                    for (int i = 0; i < dtdata.Rows.Count; i++)
                    {
                        post[i] = new ZstrBcMatRev();
                        post[i].Plantcode = dtdata.Rows[i][0].ToString();
                        post[i].Frommatcode = dtdata.Rows[i][1].ToString();
                        post[i].Tomatcode = dtdata.Rows[i][2].ToString();
                        post[i].Serial = dtdata.Rows[i][3].ToString();
                        post[i].Stackno = dtdata.Rows[i][4].ToString();
                        post[i].Status = dtdata.Rows[i][5].ToString();
                    }
                }
                request.ZbcPlyqrCodeRevPost = master;
                request.ZbcPlyqrCodeRevPost.DataIn = post;
                responce = zbcd.ZbcPlyqrCodeRevPost(request.ZbcPlyqrCodeRevPost);
                clsGlobal.AppLog.WriteLog("Data Scheduler : Sending Central To SAP ProductionReversalData : " + responce.DataOut.Count() + " No. of Records Found");
                if (responce.DataOut != null)
                {
                    ds.Tables[0].Columns.Add("LocationCode");
                    ds.Tables[0].Columns.Add("NewMatCode");
                    ds.Tables[0].Columns.Add("OldMatCode");
                    ds.Tables[0].Columns.Add("QRCode");
                    ds.Tables[0].Columns.Add("Status");
                    ds.Tables[0].Columns.Add("SAPStatus");

                    foreach (var items in responce.DataOut)
                    {
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["LocationCode"] = Convert.ToString(Convert.ToString(items.Plantcode) == null ? "" : Convert.ToString(items.Plantcode));
                        dr["NewMatCode"] = Convert.ToString(Convert.ToString(items.Tomatcode) == null ? "" : Convert.ToString(items.Tomatcode));
                        dr["OldMatCode"] = Convert.ToString(Convert.ToString(items.Frommatcode) == null ? "" : Convert.ToString(items.Frommatcode));
                        dr["QRCode"] = Convert.ToString(Convert.ToString(items.Serial) == null ? "" : Convert.ToString(items.Serial));
                        dr["Status"] = Convert.ToString(Convert.ToString(items.Status) == null ? "" : Convert.ToString(items.Status));
                        dr["SAPStatus"] = Convert.ToString(Convert.ToString(items.Sts) == null ? "" : Convert.ToString(items.Sts));
                        ds.Tables[0].Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending Production Reversal Data for LocationCode - " + sLocationCode;
                sResp = sResp + " from Central Server to SAP found error. Kindly look into the central data scheduler log file of respective date for more details";
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Web Service Sending ProductionReversalData : " + ex.Message.ToString());
                SendMail(sResp.ToString());
                //}
            }
            return ds;
        }

        public DataSet fnPIGetStackHistoryDataSAPPost(DataTable dtdata)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(new DataTable());
            try
            {
                ZBC_SAP_2_BCClient zbcd = new ZBC_SAP_2_BCClient();
                //ZBC_SAP_2_BCClient zbcd = new ZBC_SAP_2_BCClient();

                zbcd.ClientCredentials.UserName.UserName = Properties.Settings.Default.CentralToSAPWebServiceUserID;
                zbcd.ClientCredentials.UserName.Password = Properties.Settings.Default.CentralToSAPWebServiceUserPassword;

                ZbcGetStackDet master = new ZbcGetStackDet();
                ZbcGetStackDetRequest request = new ZbcGetStackDetRequest();
                ZbcGetStackDetResponse responce = new ZbcGetStackDetResponse();

                ZstrBcStckDet[] post = new ZstrBcStckDet[1];
                if (dtdata.Rows.Count > 0)
                {
                    post = new ZstrBcStckDet[dtdata.Rows.Count];
                    for (int i = 0; i < dtdata.Rows.Count; i++)
                    {
                        post[i] = new ZstrBcStckDet();
                        post[i].Plantcode = dtdata.Rows[i][0].ToString();
                        post[i].Matcode = dtdata.Rows[i][1].ToString();
                        post[i].Qrcode = dtdata.Rows[i][2].ToString();
                        post[i].Newstackno = dtdata.Rows[i][3].ToString();
                        post[i].Oldstackno = dtdata.Rows[i][4].ToString();
                        post[i].Stacksts = dtdata.Rows[i][5].ToString();
                    }
                }
                request.ZbcGetStackDet = master;
                request.ZbcGetStackDet.DataIn = post;
                responce = zbcd.ZbcGetStackDet(request.ZbcGetStackDet);
                clsGlobal.AppLog.WriteLog("Data Scheduler : Sending Central To SAP StackHistoryData : " + responce.DataOut.Count() + " No. of Records Found");
                if (responce.DataOut != null)
                {
                    ds.Tables[0].Columns.Add("LocationCode");
                    ds.Tables[0].Columns.Add("MatCode");
                    ds.Tables[0].Columns.Add("QRCode");
                    ds.Tables[0].Columns.Add("OldStack");
                    ds.Tables[0].Columns.Add("NewStack");
                    ds.Tables[0].Columns.Add("Status");
                    ds.Tables[0].Columns.Add("SAPStatus");

                    foreach (var items in responce.DataOut)
                    {
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["LocationCode"] = Convert.ToString(Convert.ToString(items.Plantcode) == null ? "" : Convert.ToString(items.Plantcode));
                        dr["MatCode"] = Convert.ToString(Convert.ToString(items.Matcode) == null ? "" : Convert.ToString(items.Matcode));
                        dr["QRCode"] = Convert.ToString(Convert.ToString(items.Qrcode) == null ? "" : Convert.ToString(items.Qrcode));
                        dr["OldStack"] = Convert.ToString(Convert.ToString(items.Oldstackno) == null ? "" : Convert.ToString(items.Oldstackno));
                        dr["NewStack"] = Convert.ToString(Convert.ToString(items.Newstackno) == null ? "" : Convert.ToString(items.Newstackno));
                        dr["Status"] = Convert.ToString(Convert.ToString(items.Stacksts) == null ? "" : Convert.ToString(items.Stacksts));
                        dr["SAPStatus"] = Convert.ToString(Convert.ToString(items.Status) == null ? "" : Convert.ToString(items.Status));
                        ds.Tables[0].Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending Stack History Data for LocationCode - " + sLocationCode;
                sResp = sResp + " from Central Server to SAP found error. Kindly look into the central data scheduler log file of respective date for more details";
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Web Service Sending Stack History Data : " + ex.Message.ToString());
                SendMail(sResp.ToString());
                //}
            }
            return ds;
        }

        public DataSet fnPIGetMTMTransferDataSAPPost(DataTable dtdata)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(new DataTable());
            try
            {
                ZBC_SAP_2_BCClient zbcd = new ZBC_SAP_2_BCClient();

                zbcd.ClientCredentials.UserName.UserName = Properties.Settings.Default.CentralToSAPWebServiceUserID;
                zbcd.ClientCredentials.UserName.Password = Properties.Settings.Default.CentralToSAPWebServiceUserPassword;

                ZbcMatToMatTrf master = new ZbcMatToMatTrf();
                ZbcMatToMatTrfRequest request = new ZbcMatToMatTrfRequest();
                ZbcMatToMatTrfResponse responce = new ZbcMatToMatTrfResponse();

                ZstrBcMatToMat[] post = new ZstrBcMatToMat[1];
                if (dtdata.Rows.Count > 0)
                {
                    post = new ZstrBcMatToMat[dtdata.Rows.Count];
                    for (int i = 0; i < dtdata.Rows.Count; i++)
                    {
                        post[i] = new ZstrBcMatToMat();
                        post[i].Plantcode = dtdata.Rows[i][0].ToString();
                        post[i].MatcodeFrom = dtdata.Rows[i][1].ToString();
                        post[i].SerialFrom = dtdata.Rows[i][2].ToString();
                        post[i].MatcodeTo = dtdata.Rows[i][4].ToString();
                        post[i].SerialTo = dtdata.Rows[i][3].ToString();
                    }
                }
                request.ZbcMatToMatTrf = master;
                request.ZbcMatToMatTrf.DataIn = post;
                responce = zbcd.ZbcMatToMatTrf(request.ZbcMatToMatTrf);
                clsGlobal.AppLog.WriteLog("Data Scheduler : Sending Central To SAP MTMTransfer Data : " + responce.DataOut.Count() + " No. of Records Found");
                if (responce.DataOut != null)
                {
                    ds.Tables[0].Columns.Add("LocationCode");
                    ds.Tables[0].Columns.Add("MatCodeFrom");
                    ds.Tables[0].Columns.Add("QRCodeFrom");
                    ds.Tables[0].Columns.Add("MatCodeTo");
                    ds.Tables[0].Columns.Add("QRCodeTo");
                    ds.Tables[0].Columns.Add("SAPStatus");

                    foreach (var items in responce.DataOut)
                    {
                        DataRow dr = ds.Tables[0].NewRow();
                        dr["LocationCode"] = Convert.ToString(Convert.ToString(items.Plantcode) == null ? "" : Convert.ToString(items.Plantcode));
                        dr["MatCodeFrom"] = Convert.ToString(Convert.ToString(items.MatcodeFrom) == null ? "" : Convert.ToString(items.MatcodeFrom));
                        dr["QRCodeFrom"] = Convert.ToString(Convert.ToString(items.SerialFrom) == null ? "" : Convert.ToString(items.SerialFrom));
                        dr["MatCodeTo"] = Convert.ToString(Convert.ToString(items.MatcodeTo) == null ? "" : Convert.ToString(items.MatcodeTo));
                        dr["QRCodeTo"] = Convert.ToString(Convert.ToString(items.SerialTo) == null ? "" : Convert.ToString(items.SerialTo));
                        dr["SAPStatus"] = Convert.ToString(Convert.ToString(items.Sts) == null ? "" : Convert.ToString(items.Sts));
                        ds.Tables[0].Rows.Add(dr);
                    }
                }
            }
            catch (Exception ex)
            {
                //if (sEmailSentStatus == "Failed" || sEmailSentStatus == string.Empty || sEmailSentStatus == "")
                //{
                string sResp = "Sending Material To Material Transfer Data for LocationCode - " + sLocationCode;
                sResp = sResp + " from Central Server to SAP found error. Kindly look into the central data scheduler log file of respective date for more details";
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Web Service Sending MTMTransfer Data : " + ex.Message.ToString());
                SendMail(sResp.ToString());
                //}
            }
            return ds;
        }

        #endregion


        #region Update SAP Responce to Central Server

        public void UpdateResponceSAP2CentralStockDataPost(DataSet dsData)
        {
            clsLogic oLogic = new clsLogic();
            StringBuilder sb;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (dsData.Tables["Table1"].Rows.Count > 0)
                {
                    clsGlobal.AppLog.WriteLog("Central Data Scheduler : Responce Production Data from SAP : Total No. of Records Found - " + dsData.Tables["Table1"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dsData.Tables["Table1"].Rows)
                    {
                        sb = new StringBuilder();
                        string CreatedBy = "CentralScheduler";

                        oDbmCentral.CreateParameters(7);
                        oDbmCentral.AddParameters(0, "@Type", "UPDATESTOCKSAPPOSTEDSTATUS");
                        oDbmCentral.AddParameters(1, "@LocationCode", dr["LocationCode"].ToString().Trim());
                        oDbmCentral.AddParameters(2, "@MatCode", dr["ToMatCode"].ToString().Trim());
                        oDbmCentral.AddParameters(3, "@QRCode", dr["QRCode"].ToString().Trim());
                        oDbmCentral.AddParameters(4, "@MatStatus", dr["Status"].ToString().Trim());
                        oDbmCentral.AddParameters(5, "@SAPPostMsg", dr["SAPStatus"].ToString().Trim());
                        oDbmCentral.AddParameters(6, "@CreatedBy", CreatedBy);
                        DataTable dt = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_UpdateSAPStatus").Tables[0];
                        if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                        {
                            clsGlobal.AppLog.WriteLog("Central Data Scheduler : Error in Saving QRCode SAP Responce : ERROR : " + dt.Rows[0][0].ToString());
                        }
                        if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                        {
                            clsGlobal.iUpdateCount++;
                            //clsGlobal.AppLog.WriteLog("Central Data Scheduler : Responce Production Data from SAP : No. Of Rows updated - " + clsGlobal.iUpdateCount + " of MatCode - " + dr["ToMatCode"].ToString().Trim() + " with QRCode - " + dr["QRCode"].ToString().Trim());
                        }
                    }
                    oDbmCentral.CommitTransaction();
                    clsGlobal.AppLog.WriteLog("Central Data Scheduler : Responce QRCode Data from SAP : Total No. Of Rows updated - " + clsGlobal.iUpdateCount);
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Saving QRCode SAP Responce :: ERROR : " + ex.Message.ToString());
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
        }

        public void UpdateResponceSAP2CentralHubDataPost(DataSet dsData)
        {
            clsLogic oLogic = new clsLogic();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (dsData.Tables["Table1"].Rows.Count > 0)
                {
                    clsGlobal.AppLog.WriteLog("Data Scheduler => Saving Responce HubStockData from SAP : No. of Records Found - " + dsData.Tables["Table1"].Rows.Count);
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dsData.Tables["Table1"].Rows)
                    {
                        sb = new StringBuilder();
                        string CreatedBy = "Scheduler";

                        oDbmCentral.CreateParameters(8);
                        oDbmCentral.AddParameters(0, "@Type", "UPDATEHUBSTOCKSAPPOSTEDSTATUS");
                        oDbmCentral.AddParameters(1, "@LocationCode", dr["LocationCode"].ToString().Trim());
                        oDbmCentral.AddParameters(2, "@PONo", dr["PONo"].ToString().Trim());
                        oDbmCentral.AddParameters(3, "@MatCode", dr["MatCode"].ToString().Trim());
                        oDbmCentral.AddParameters(4, "@VendorCode", dr["VendorCode"].ToString().Trim());
                        oDbmCentral.AddParameters(5, "@QRCode", dr["QRCode"].ToString().Trim());
                        oDbmCentral.AddParameters(6, "@SAPPostMsg", dr["SAPStatus"].ToString().Trim());
                        oDbmCentral.AddParameters(7, "@CreatedBy", CreatedBy);
                        DataTable dt = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_UpdateSAPStatus").Tables[0];
                        if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                        {
                            clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Saving HubStock SAP Responce :: ERROR : " + dt.Rows[0][0].ToString());
                        }
                        if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                        {
                            clsGlobal.iUpdateCount++;
                        }
                    }
                    oDbmCentral.CommitTransaction();
                    clsGlobal.AppLog.WriteLog("Data Scheduler :: Saving Responce HubStock from SAP :: No. Of Rows updated - " + clsGlobal.iUpdateCount);
                }

            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Saving HubStock SAP Responce :: ERROR : " + ex.Message.ToString());
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
        }

        public void UpdateResponceSAP2CentralHubQualityDataPost(DataSet dsData)
        {
            clsLogic oLogic = new clsLogic();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (dsData.Tables["Table1"].Rows.Count > 0)
                {
                    clsGlobal.AppLog.WriteLog("Data Scheduler => Saving Responce HubQuality from SAP : No. of Records Found - " + dsData.Tables["Table1"].Rows.Count);
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dsData.Tables["Table1"].Rows)
                    {
                        sb = new StringBuilder();
                        string CreatedBy = "Scheduler";

                        oDbmCentral.CreateParameters(8);
                        oDbmCentral.AddParameters(0, "@Type", "UPDATEHUBQCSTOCKSAPPOSTEDSTATUS");
                        oDbmCentral.AddParameters(1, "@LocationCode", dr["LocationCode"].ToString().Trim());
                        oDbmCentral.AddParameters(2, "@PONo", dr["PONo"].ToString().Trim());
                        oDbmCentral.AddParameters(3, "@MatCode", dr["MatCode"].ToString().Trim());
                        oDbmCentral.AddParameters(4, "@VendorCode", dr["VendorCode"].ToString().Trim());
                        oDbmCentral.AddParameters(5, "@QRCode", dr["QRCode"].ToString().Trim());
                        oDbmCentral.AddParameters(6, "@SAPPostMsg", dr["SAPStatus"].ToString().Trim());
                        oDbmCentral.AddParameters(7, "@CreatedBy", CreatedBy);
                        DataTable dt = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_UpdateSAPStatus").Tables[0];
                        if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                        {
                            clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Saving HubRejectionData SAP Responce :: ERROR : " + dt.Rows[0][0].ToString());
                        }
                        if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                        {
                            clsGlobal.iUpdateCount++;
                        }
                    }
                    oDbmCentral.CommitTransaction();
                    clsGlobal.AppLog.WriteLog("Data Scheduler :: Saving Responce HubQuality from SAP :: No. Of Rows updated - " + clsGlobal.iUpdateCount);
                }

            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Saving HubQuality SAP Responce :: ERROR : " + ex.Message.ToString());
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
        }

        public void UpdateResponceSAP2CentralHubQualityBOILDataPost(DataSet dsData)
        {
            clsLogic oLogic = new clsLogic();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (dsData.Tables["Table1"].Rows.Count > 0)
                {
                    clsGlobal.AppLog.WriteLog("Data Scheduler => Saving Responce HubRejectionData from SAP : No. of Records Found - " + dsData.Tables["Table1"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dsData.Tables["Table1"].Rows)
                    {
                        sb = new StringBuilder();
                        string CreatedBy = "Scheduler";

                        oDbmCentral.CreateParameters(8);
                        oDbmCentral.AddParameters(0, "@Type", "UPDATEHUBQCSTOCKSAPPOSTEDSTATUS");
                        oDbmCentral.AddParameters(1, "@LocationCode", dr["LocationCode"].ToString().Trim());
                        oDbmCentral.AddParameters(2, "@PONo", dr["PONo"].ToString().Trim());
                        oDbmCentral.AddParameters(3, "@MatCode", dr["MatCode"].ToString().Trim());
                        oDbmCentral.AddParameters(4, "@VendorCode", dr["VendorCode"].ToString().Trim());
                        oDbmCentral.AddParameters(5, "@QRCode", dr["QRCode"].ToString().Trim());
                        oDbmCentral.AddParameters(6, "@SAPPostMsg", dr["SAPStatus"].ToString().Trim());
                        oDbmCentral.AddParameters(7, "@CreatedBy", CreatedBy);
                        DataTable dt = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_UpdateSAPStatus").Tables[0];
                        if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                        {
                            clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Saving HubRejectionData SAP Responce :: ERROR : " + dt.Rows[0][0].ToString());
                        }
                        if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                        {
                            clsGlobal.iUpdateCount++;
                        }
                    }
                    oDbmCentral.CommitTransaction();
                    clsGlobal.AppLog.WriteLog("Data Scheduler :: Saving Responce HubRejectionData from SAP :: No. Of Rows updated - " + clsGlobal.iUpdateCount);
                }

            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Saving HubRejectionData SAP Responce :: ERROR : " + ex.Message.ToString());
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
        }

        public void UpdateResponceSAP2CentralVendorDataPost(DataSet dsData)
        {
            clsLogic oLogic = new clsLogic();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (dsData.Tables["Table1"].Rows.Count > 0)
                {
                    clsGlobal.AppLog.WriteLog("Data Scheduler :: Saving Responce Vendor Printing Data from SAP : No. of Records Found - " + dsData.Tables["Table1"].Rows.Count);
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dsData.Tables["Table1"].Rows)
                    {
                        sb = new StringBuilder();
                        string CreatedBy = "Scheduler";

                        oDbmCentral.CreateParameters(8);
                        oDbmCentral.AddParameters(0, "@Type", "UPDATEVENDORSAPPOSTEDSTATUS");
                        oDbmCentral.AddParameters(1, "@LocationCode", dr["LocationCode"].ToString().Trim());
                        oDbmCentral.AddParameters(2, "@PONo", dr["PONo"].ToString().Trim());
                        oDbmCentral.AddParameters(3, "@MatCode", dr["MatCode"].ToString().Trim());
                        oDbmCentral.AddParameters(4, "@VendorCode", dr["VendorCode"].ToString().Trim());
                        oDbmCentral.AddParameters(5, "@QRCode", dr["QRCode"].ToString().Trim());
                        oDbmCentral.AddParameters(6, "@SAPPostMsg", dr["SAPStatus"].ToString().Trim());
                        oDbmCentral.AddParameters(7, "@CreatedBy", CreatedBy);
                        DataTable dt = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_UpdateSAPStatus").Tables[0];
                        if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                        {
                            clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Saving Vendor Printing Data SAP Responce :: ERROR : " + dt.Rows[0][0].ToString());
                        }
                        if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                        {
                            clsGlobal.iUpdateCount++;
                        }
                    }
                    oDbmCentral.CommitTransaction();
                    clsGlobal.AppLog.WriteLog("Data Scheduler :: Saving Responce Vendor Printing Data from SAP :: No. Of Rows updated - " + clsGlobal.iUpdateCount);
                }

            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Saving Vendor Printing Data SAP Responce :: ERROR : " + ex.Message.ToString());
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
        }

        public void UpdateResponceSAP2CentralDeliveryOrderDataPost(DataSet dsData)
        {
            clsLogic oLogic = new clsLogic();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (dsData.Tables["Table1"].Rows.Count > 0)
                {
                    clsGlobal.AppLog.WriteLog("Central Data Scheduler : Saving Responce Dispatch Data from SAP : Total No. of Records Found - " + dsData.Tables["Table1"].Rows.Count);
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dsData.Tables["Table1"].Rows)
                    {
                        sb = new StringBuilder();
                        string CreatedBy = "CentralScheduler";

                        oDbmCentral.CreateParameters(7);
                        oDbmCentral.AddParameters(0, "@Type", "UPDATEDISPATCHSAPPOSTEDSTATUS");
                        oDbmCentral.AddParameters(1, "@LocationCode", dr["LocationCode"].ToString().Trim());
                        oDbmCentral.AddParameters(2, "@DONo", dr["DONo"].ToString().Trim());
                        oDbmCentral.AddParameters(3, "@MatCode", dr["MatCode"].ToString().Trim());
                        oDbmCentral.AddParameters(4, "@QRCode", dr["QRCode"].ToString().Trim());
                        oDbmCentral.AddParameters(5, "@SAPPostMsg", dr["SAPStatus"].ToString().Trim());
                        oDbmCentral.AddParameters(6, "@CreatedBy", CreatedBy);
                        DataTable dt = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_UpdateSAPStatus").Tables[0];
                        if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                        {
                            clsGlobal.AppLog.WriteLog("Central Data Scheduler : Error in Saving Dispatch Data SAP Responce : ERROR : " + dt.Rows[0][0].ToString());
                        }
                        if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                        {
                            clsGlobal.iUpdateCount++;
                            //clsGlobal.AppLog.WriteLog("Data Scheduler : Responce Dispatch Data from SAP : No. Of Rows updated - " + clsGlobal.iUpdateCount + " of DONo - " + dr["DONo"].ToString().Trim() + " having MatCode - " + dr["MatCode"].ToString().Trim() + " with QRCode - " + dr["QRCode"].ToString().Trim());
                        }
                    }
                    oDbmCentral.CommitTransaction();
                    clsGlobal.AppLog.WriteLog("Data Scheduler : Saving Responce Dispatch Data from SAP : Total No. Of Rows updated - " + clsGlobal.iUpdateCount);
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                clsGlobal.AppLog.WriteLog("Data Scheduler : Error in Saving Dispatch Data SAP Responce : ERROR : " + ex.Message.ToString());
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
        }

        public void UpdateResponceSAP2CentralCancelledDeliveryOrderDataPost(DataSet dsData)
        {
            clsLogic oLogic = new clsLogic();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (dsData.Tables["Table1"].Rows.Count > 0)
                {
                    clsGlobal.AppLog.WriteLog("Central Data Scheduler : Saving Responce Dispatch Data from SAP : Total No. of Records Found - " + dsData.Tables["Table1"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dsData.Tables["Table1"].Rows)
                    {
                        sb = new StringBuilder();
                        string CreatedBy = "CentralScheduler";

                        oDbmCentral.CreateParameters(7);
                        oDbmCentral.AddParameters(0, "@Type", "UPDATECANCELLEDDELIVERYSAPPOSTEDSTATUS");
                        oDbmCentral.AddParameters(1, "@LocationCode", dr["LocationCode"].ToString().Trim());
                        oDbmCentral.AddParameters(2, "@DONo", dr["DONo"].ToString().Trim());
                        oDbmCentral.AddParameters(3, "@MatCode", dr["MatCode"].ToString().Trim());
                        oDbmCentral.AddParameters(4, "@QRCode", dr["QRCode"].ToString().Trim());
                        oDbmCentral.AddParameters(5, "@SAPPostMsg", dr["SAPStatus"].ToString().Trim());
                        oDbmCentral.AddParameters(6, "@CreatedBy", CreatedBy);
                        DataTable dt = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_UpdateSAPStatus").Tables[0];
                        if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                        {
                            clsGlobal.AppLog.WriteLog("Central Data Scheduler : Error in Saving Dispatch Data SAP Responce : ERROR : " + dt.Rows[0][0].ToString() + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                        }
                        if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                        {
                            clsGlobal.iUpdateCount++;
                            clsGlobal.AppLog.WriteLog("Data Scheduler : Responce Dispatch Data from SAP : No. Of Rows updated - " + clsGlobal.iUpdateCount + " of DONo - " + dr["DONo"].ToString().Trim() + " having MatCode - " + dr["MatCode"].ToString().Trim() + " with QRCode - " + dr["QRCode"].ToString().Trim() + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                        }
                    }
                    oDbmCentral.CommitTransaction();
                    clsGlobal.AppLog.WriteLog("Data Scheduler : Saving Responce Dispatch Data from SAP : Total No. Of Rows updated - " + clsGlobal.iUpdateCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                clsGlobal.AppLog.WriteLog("Data Scheduler : Error in Saving Dispatch Data SAP Responce : ERROR : " + ex.Message.ToString() + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
        }

        public void UpdateResponceSAP2CentralSalesReturnDataPost(DataSet dsData)
        {
            clsLogic oLogic = new clsLogic();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (dsData.Tables["Table1"].Rows.Count > 0)
                {
                    clsGlobal.AppLog.WriteLog("Data Scheduler :: Saving Responce SalesReturn Data from SAP : No. of Records Found - " + dsData.Tables["Table1"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dsData.Tables["Table1"].Rows)
                    {
                        sb = new StringBuilder();
                        string CreatedBy = "CentralScheduler";

                        oDbmCentral.CreateParameters(7);
                        oDbmCentral.AddParameters(0, "@Type", "UPDATESALESRETURNSAPPOSTEDSTATUS");
                        oDbmCentral.AddParameters(1, "@LocationCode", dr["LocationCode"].ToString().Trim());
                        oDbmCentral.AddParameters(2, "@SalesReturnNo", dr["SalesReturnNo"].ToString().Trim());
                        oDbmCentral.AddParameters(3, "@MatCode", dr["MatCode"].ToString().Trim());
                        oDbmCentral.AddParameters(4, "@QRCode", dr["QRCode"].ToString().Trim());
                        oDbmCentral.AddParameters(5, "@SAPPostMsg", dr["SAPStatus"].ToString().Trim());
                        oDbmCentral.AddParameters(6, "@CreatedBy", CreatedBy);
                        DataTable dt = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_UpdateSAPStatus").Tables[0];
                        if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                        {
                            clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Saving SalesReturn Data SAP Responce :: ERROR : " + dt.Rows[0][0].ToString() + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                        }
                        if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                        {
                            clsGlobal.iUpdateCount++;
                        }
                    }
                    oDbmCentral.CommitTransaction();
                    clsGlobal.AppLog.WriteLog("Data Scheduler :: Saving Responce SalesReturn Data from SAP :: No. Of Rows updated - " + clsGlobal.iUpdateCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                }

            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Saving SalesReturn Data SAP Responce :: ERROR : " + ex.Message.ToString() + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
        }

        public void UpdateResponceSAP2CentralPurchaseReturnOrderDataPost(DataSet dsData)
        {
            clsLogic oLogic = new clsLogic();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (dsData.Tables["Table1"].Rows.Count > 0)
                {
                    clsGlobal.AppLog.WriteLog("Data Scheduler :: Saving Responce PurchaseReturn Data from SAP : No. of Records Found - " + dsData.Tables["Table1"].Rows.Count);
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dsData.Tables["Table1"].Rows)
                    {
                        sb = new StringBuilder();
                        string CreatedBy = "Scheduler";

                        oDbmCentral.CreateParameters(8);
                        oDbmCentral.AddParameters(0, "@Type", "UPDATEPURCHASERETURNSAPPOSTEDSTATUS");
                        oDbmCentral.AddParameters(1, "@LocationCode", dr["LocationCode"].ToString().Trim());
                        oDbmCentral.AddParameters(2, "@PONo", dr["PONo"].ToString().Trim());
                        oDbmCentral.AddParameters(3, "@MatCode", dr["MatCode"].ToString().Trim());
                        oDbmCentral.AddParameters(4, "@QRCode", dr["QRCode"].ToString().Trim());
                        oDbmCentral.AddParameters(5, "@VendorCode", dr["VendorCode"].ToString().Trim());
                        oDbmCentral.AddParameters(6, "@SAPPostMsg", dr["SAPStatus"].ToString().Trim());
                        oDbmCentral.AddParameters(7, "@CreatedBy", CreatedBy);
                        DataTable dt = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_UpdateSAPStatus").Tables[0];
                        if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                        {
                            clsGlobal.AppLog.WriteLog("Central Data Scheduler : Error in Saving PurchaseReturn SAP Responce : ERROR : " + dt.Rows[0][0].ToString());
                        }
                        if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                        {
                            clsGlobal.iUpdateCount++;
                            //clsGlobal.AppLog.WriteLog("Central Data Scheduler : Responce Production Data from SAP : No. Of Rows updated - " + clsGlobal.iUpdateCount + " of MatCode - " + dr["ToMatCode"].ToString().Trim() + " with QRCode - " + dr["QRCode"].ToString().Trim());
                        }
                    }
                    oDbmCentral.CommitTransaction();
                    clsGlobal.AppLog.WriteLog("Data Scheduler :: Saving Responce PurchaseReturn Data from SAP :: No. Of Rows updated - " + clsGlobal.iUpdateCount);
                }

            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                clsGlobal.AppLog.WriteLog("Data Scheduler :: Error in Saving PurchaseReturn Data SAP Responce :: ERROR : " + ex.Message.ToString());
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
        }

        public void UpdateResponceSAP2CentralMTMTransferDataPost(DataSet dsData)
        {
            clsLogic oLogic = new clsLogic();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (dsData.Tables["Table1"].Rows.Count > 0)
                {
                    clsGlobal.AppLog.WriteLog("Data Scheduler : Saving Responce MTM Transfer Data from SAP : No. of Records Found - " + dsData.Tables["Table1"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dsData.Tables["Table1"].Rows)
                    {
                        sb = new StringBuilder();
                        string CreatedBy = "Scheduler";

                        oDbmCentral.CreateParameters(7);
                        oDbmCentral.AddParameters(0, "@Type", "UPDATEMTMTRANSFERSAPPOSTEDSTATUS");
                        oDbmCentral.AddParameters(1, "@LocationCode", dr["LocationCode"].ToString().Trim());
                        oDbmCentral.AddParameters(2, "@MatCode", dr["MatCode"].ToString().Trim());
                        oDbmCentral.AddParameters(3, "@QRCode", dr["QRCode"].ToString().Trim());
                        oDbmCentral.AddParameters(4, "@MatStatus", dr["MatStatus"].ToString().Trim());
                        oDbmCentral.AddParameters(5, "@SAPPostMsg", dr["SAPPostMsg"].ToString().Trim());
                        oDbmCentral.AddParameters(6, "@SAPPostMsg", CreatedBy);
                        DataTable dt = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_UpdateSAPStatus").Tables[0];
                        if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                        {
                            clsGlobal.AppLog.WriteLog("Data Scheduler : Error in Saving MTM Transfer Data SAP Responce : ERROR : " + dt.Rows[0][0].ToString() + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                        }
                        if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                        {
                            clsGlobal.iUpdateCount++;
                        }
                    }
                    oDbmCentral.CommitTransaction();
                    clsGlobal.AppLog.WriteLog("Data Scheduler : Saving Responce MTM Transfer Data from SAP : No. Of Rows updated - " + clsGlobal.iUpdateCount++ + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                clsGlobal.AppLog.WriteLog("Data Scheduler : Error in Saving MTM Transfer Data SAP Responce : ERROR : " + ex.Message.ToString() + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
        }

        public void UpdateResponceSAP2CentralProductionReversalDataPost(DataSet dsData)
        {
            clsLogic oLogic = new clsLogic();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (dsData.Tables["Table1"].Rows.Count > 0)
                {
                    clsGlobal.AppLog.WriteLog("Data Scheduler : Saving Responce ProductionReversalData from SAP : No. of Records Found - " + dsData.Tables["Table1"].Rows.Count + " at " + DateTime.Now.ToString("dd-MM-yyyy hh:mm:ss"));
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dsData.Tables["Table1"].Rows)
                    {
                        sb = new StringBuilder();
                        string CreatedBy = "Scheduler";

                        oDbmCentral.CreateParameters(7);
                        oDbmCentral.AddParameters(0, "@Type", "UPDATESAPPOSTEDSTATUS");
                        oDbmCentral.AddParameters(1, "@LocationCode", dr["LocationCode"].ToString().Trim());
                        oDbmCentral.AddParameters(2, "@MatCode", dr["NewMatCode"].ToString().Trim());
                        oDbmCentral.AddParameters(3, "@QRCode", dr["QRCode"].ToString().Trim());
                        oDbmCentral.AddParameters(4, "@MatStatus", dr["Status"].ToString().Trim());
                        oDbmCentral.AddParameters(5, "@SAPPostMsg", dr["SAPPostMsg"].ToString().Trim());
                        oDbmCentral.AddParameters(6, "@SAPPostMsg", CreatedBy);
                        DataTable dt = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
                        if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                        {
                            clsGlobal.AppLog.WriteLog("Data Scheduler : Error in Saving ProductionReversalData SAP Responce : ERROR : " + dt.Rows[0][0].ToString());
                        }
                        if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                        {
                            clsGlobal.iUpdateCount++;
                        }
                    }
                    oDbmCentral.CommitTransaction();
                    clsGlobal.AppLog.WriteLog("Data Scheduler : Saving Responce ProductionReversalData from SAP : No. Of Rows updated - " + clsGlobal.iUpdateCount);
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                clsGlobal.AppLog.WriteLog("Data Scheduler : Error in Saving ProductionReversalData SAP Responce : ERROR : " + ex.Message.ToString());
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
        }

        public void UpdateResponceSAP2CentralMatDamageDataPost(DataSet dsData)
        {
            clsLogic oLogic = new clsLogic();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (dsData.Tables["Table1"].Rows.Count > 0)
                {
                    clsGlobal.AppLog.WriteLog("Data Scheduler : Saving Responce ProductionReversalData from SAP : No. of Records Found - " + dsData.Tables["Table1"].Rows.Count);
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dsData.Tables["Table1"].Rows)
                    {
                        sb = new StringBuilder();
                        string CreatedBy = "Scheduler";

                        oDbmCentral.CreateParameters(7);
                        oDbmCentral.AddParameters(0, "@Type", "UPDATEMATDAMAGESAPPOSTEDSTATUS");
                        oDbmCentral.AddParameters(1, "@LocationCode", dr["LocationCode"].ToString().Trim());
                        oDbmCentral.AddParameters(2, "@MatCode", dr["NewMatCode"].ToString().Trim());
                        oDbmCentral.AddParameters(3, "@QRCode", dr["QRCode"].ToString().Trim());
                        oDbmCentral.AddParameters(4, "@MatStatus", dr["Status"].ToString().Trim());
                        oDbmCentral.AddParameters(5, "@SAPPostMsg", dr["SAPPostMsg"].ToString().Trim());
                        oDbmCentral.AddParameters(6, "@SAPPostMsg", CreatedBy);
                        DataTable dt = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_UpdateSAPStatus").Tables[0];
                        if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                        {
                            clsGlobal.AppLog.WriteLog("Data Scheduler : Error in Saving ProductionReversalData SAP Responce : ERROR : " + dt.Rows[0][0].ToString());
                        }
                        if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                        {
                            clsGlobal.iUpdateCount++;
                        }
                    }
                    oDbmCentral.CommitTransaction();
                    clsGlobal.AppLog.WriteLog("Data Scheduler : Saving Responce ProductionReversalData from SAP : No. Of Rows updated - " + clsGlobal.iUpdateCount);
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                clsGlobal.AppLog.WriteLog("Data Scheduler : Error in Saving ProductionReversalData SAP Responce : ERROR : " + ex.Message.ToString());
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
        }

        public void UpdateResponceSAP2CentralStackHistoryDataPost(DataSet dsData)
        {
            clsLogic oLogic = new clsLogic();
            StringBuilder sb;
            clsGlobal.iAddCount = 0;
            clsGlobal.iUpdateCount = 0;

            try
            {
                sb = new StringBuilder();
                oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
                _dtBindList = new DataTable();
                if (dsData.Tables["Table1"].Rows.Count > 0)
                {
                    clsGlobal.AppLog.WriteLog("Data Scheduler : Saving Responce Stack History Data from SAP : No. of Records Found - " + dsData.Tables["Table1"].Rows.Count);
                    oDbmCentral.BeginTransaction(oDbmCentral.Connection);
                    foreach (DataRow dr in dsData.Tables["Table1"].Rows)
                    {
                        sb = new StringBuilder();
                        string CreatedBy = "Scheduler";

                        oDbmCentral.CreateParameters(8);
                        oDbmCentral.AddParameters(0, "@Type", "UPDATESTACKHISTORYSAPPOSTEDSTATUS");
                        oDbmCentral.AddParameters(1, "@LocationCode", dr["LocationCode"].ToString().Trim());
                        oDbmCentral.AddParameters(2, "@MatCode", dr["NewMatCode"].ToString().Trim());
                        oDbmCentral.AddParameters(3, "@QRCode", dr["QRCode"].ToString().Trim());
                        oDbmCentral.AddParameters(4, "@OldStackQRCode", dr["OldStack"].ToString().Trim());
                        oDbmCentral.AddParameters(5, "@StackQRCode", dr["NewStack"].ToString().Trim());
                        oDbmCentral.AddParameters(6, "@SAPPostMsg", dr["SAPPostMsg"].ToString().Trim());
                        oDbmCentral.AddParameters(7, "@SAPPostMsg", CreatedBy);
                        DataTable dt = oDbmCentral.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_UpdateSAPStatus").Tables[0];
                        if ((dt.Columns.Contains("ERROR") || dt.Columns.Contains("ErrorMessage")) && dt.Rows.Count > 0)
                        {
                            clsGlobal.AppLog.WriteLog("Data Scheduler : Error in Saving Stack History Data SAP Responce : ERROR : " + dt.Rows[0][0].ToString());
                        }
                        if (dt.Columns.Contains("STATUS") && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                        {
                            clsGlobal.iUpdateCount++;
                        }
                    }
                    oDbmCentral.CommitTransaction();
                    clsGlobal.AppLog.WriteLog("Data Scheduler : Saving Responce Stack History Data from SAP : No. Of Rows updated - " + clsGlobal.iUpdateCount);
                }
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
                clsGlobal.AppLog.WriteLog("Data Scheduler : Error in Saving Stack History Data SAP Responce : ERROR : " + ex.Message.ToString());
            }
            finally
            {
                if (oDbmCentral.Connection.State == ConnectionState.Open)
                    oDbmCentral.Close();
            }
        }

        #endregion


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
                clsGlobal.AppLog.WriteLog("Data Scheduler => EMail sent successfully");
                sEmailSentStatus = "Sent";
                return true;
            }
            catch (Exception ex)
            {
                sEmailSentStatus = "Failed";
                clsGlobal.AppLog.WriteLog("Data Scheduler => Email sent Failed using SMTP! And Error is : " + ex.Message.ToString());
                if (ex.Message != null)
                    clsGlobal.AppLog.WriteLog("Data Scheduler => Email sent Failed using SMTP! And Error is : " + ex.Message.ToString());
                return false;
            }

        }

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
                oDbmCentral.Open();
                oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { oDbmCentral.Close(); }

        }

        public void saveTimeDtls(string strTime, string strFileFlag, bool bIsActive)
        {
            StringBuilder sb = new StringBuilder();
            try
            {
                oDbmCentral.Open();
                oDbmCentral.BeginTransaction(oDbmCentral.Connection);

                sb.Append("Merge into cScheduler S1 ");
                sb.Append("Using ");
                sb.Append("(select '" + strTime + "' as Time) S2 ");
                sb.Append("on (S1.Time = S2.Time) ");
                sb.Append("when matched then ");
                sb.Append("update set S1.FileFlag = '" + strFileFlag + "', S1.isActive = '" + bIsActive + "', S1.ModifiedOn = GetDate() ");
                sb.Append("when not matched then ");
                sb.Append("insert (Time, FileFlag, isActive, CreatedOn, ModifiedOn) ");
                sb.Append("values ('" + strTime + "','" + strFileFlag + "', '" + bIsActive + "',GetDate(),GetDate()); ");
                oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());

                oDbmCentral.CommitTransaction();
            }
            catch (Exception ex)
            {
                oDbmCentral.RollBackTransaction();
            }
            finally
            {
                oDbmCentral.Close();
            }
        }

        public DataTable getTimerDtls()
        {
            try
            {
                oDbmCentral.Open();

                DataTable dtResponse;
                StringBuilder oStr = new StringBuilder();
                oStr.AppendLine(" SELECT Time,FileFlag,isActive ");
                oStr.AppendLine(" FROM cScheduler");
                //oStr.AppendLine(" WHERE isActive=1");
                dtResponse = oDbmCentral.ExecuteDataSet(CommandType.Text, oStr.ToString()).Tables[0];
                oDbmCentral.Close();
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
                oDbmCentral.Open();
                DataTable dtResponse;
                StringBuilder oStr = new StringBuilder();
                oStr.AppendLine(" SELECT Time,FileFlag,isActive ");
                oStr.AppendLine(" FROM cScheduler");
                oStr.AppendLine(" WHERE isActive=1");
                dtResponse = oDbmCentral.ExecuteDataSet(CommandType.Text, oStr.ToString()).Tables[0];
                oDbmCentral.Close();
                return dtResponse;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //
        //public void GetCentral2SAPStockDataPost(DataSet dsData)
        //{
        //    clsLogic oLogic = new clsLogic();
        //    StringBuilder sb;
        //    clsGlobal.iAddCount = 0;
        //    clsGlobal.iUpdateCount = 0;

        //    try
        //    {
        //        sb = new StringBuilder();
        //        oDbmCentral.Open(DataProvider.SqlServer, clsGlobal.StrCon);
        //        _dtBindList = new DataTable();
        //        if (dsData.Tables["tLocationLabelPrinting"].Rows.Count > 0)
        //        {
        //            oDbmCentral.BeginTransaction(oDbmCentral.Connection);
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
        //                DataTable dt = oDbmCentral.ExecuteDataSet(CommandType.Text, sb.ToString()).Tables[0];
        //                if (dt.Rows.Count == 0)
        //                {
        //                    sb = new StringBuilder();
        //                    sb.AppendLine("Insert into mVendor(LocationCode, VendorCode, VendorDesc,");
        //                    sb.AppendLine("VendorEmail, VendorAddress, CreatedOn, CreatedBy)");
        //                    sb.AppendLine("Values ");
        //                    sb.AppendLine("('" + _PLVendorMaster.LocationCode + "','" + _PLVendorMaster.VendorCode + "','" + _PLVendorMaster.VendorDesc + "',");
        //                    sb.AppendLine("'" + _PLVendorMaster.VendorEmail + "','" + _PLVendorMaster.VendorAddress + "',");
        //                    sb.AppendLine("'" + _PLVendorMaster.CreatedOn + "','" + _PLVendorMaster.CreatedBy + "')");
        //                    oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
        //                    clsGlobal.iAddCount++;
        //                }
        //                else
        //                {
        //                    sb = new StringBuilder();
        //                    sb.AppendLine(" Update mVendor set VendorDesc = '" + _PLVendorMaster.VendorDesc + "', VendorEmail = '" + _PLVendorMaster.VendorEmail + "'");
        //                    sb.AppendLine(", VendorAddress = '" + _PLVendorMaster.VendorAddress + "'");
        //                    sb.AppendLine(", CreatedOn = '" + _PLVendorMaster.CreatedOn + "'");
        //                    sb.AppendLine(" CreatedBy = '" + _PLVendorMaster.CreatedBy + "' WHERE VendorCode = '" + _PLVendorMaster.VendorCode + "'");
        //                    oDbmCentral.ExecuteNonQuery(CommandType.Text, sb.ToString());
        //                    clsGlobal.iUpdateCount++;
        //                }
        //                break;
        //                //}
        //            }
        //            oDbmCentral.CommitTransaction();
        //            clsGlobal.AppLog.WriteLog("Data Scheduler :: Download Master :: Vendor Master No Of Rows inserted " + clsGlobal.iAddCount++);
        //            clsGlobal.AppLog.WriteLog("Data Scheduler :: Download Master :: Vendor Master No Of Rows updated" + clsGlobal.iUpdateCount++);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        oDbmCentral.RollBackTransaction();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        oDbmCentral.Close();
        //    }
        //}

    }
}


