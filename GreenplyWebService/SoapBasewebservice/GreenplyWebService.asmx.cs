using BCIL;
using BCIL.Utility;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Web;
using System.Web.Services;
using System.Globalization;

namespace GreenplyWebService
{

    //[WebService(Namespace = "http://103.106.192.226/")]  // for BCIL Amul Server
    [WebService(Namespace = "http://localhost:64646/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]


    //[WebService(Namespace = "http://tempuri.org/")]
    //[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    //[System.ComponentModel.ToolboxItem(false)]


    public class SoapServices : System.Web.Services.WebService
    {
        [WebGet]
        public bool Login(string UserName, string Pswd)
        {

            if (UserName.Equals("BCIL") && Pswd.Equals("bcil@123"))
            {

                return true;
            }
            else
            {
                return false;
            }
        }


        [WebMethod(MessageName = "GetSAPMatMasterData")]
        public clsResponseMatList GetSAPMatMasterData(ListMatMasterDetails objMat, string UserName, string Pswd)
        {
            WriteLogFile objLog = new WriteLogFile();
            clsResponseMatList clsob = new clsResponseMatList();
            List<ClsMatMasterResponse> lstResponse = new List<ClsMatMasterResponse>();

            if (!Login(UserName, Pswd))
            {
                throw new BCILException("Authantication", new Exception("Not Valid user"));
            }
            string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

            SqlConnection conMain = new SqlConnection(conString);

            if (conMain.State == ConnectionState.Closed)
                conMain.Open();

            try
            {
                objLog.WriteLog("Load SAP Material Master -- No of Records Count - " + Convert.ToString(objMat.LoadMatMaster.Count));
                for (int i = 0; i < objMat.LoadMatMaster.Count; i++)
                {
                    try
                    {
                        string _strLog = objMat.LoadMatMaster[i].mPRODUCT + ',' + objMat.LoadMatMaster[i].mMATCODE + ',' + objMat.LoadMatMaster[i].mMATDESC + ',' +
                                         objMat.LoadMatMaster[i].mTHICKNESS + ',' + objMat.LoadMatMaster[i].mTHICKNESSDESC + ',' + objMat.LoadMatMaster[i].mSIZE + ',' + objMat.LoadMatMaster[i].mGRADE + ',' + objMat.LoadMatMaster[i].mGRADEDESC + ',' +
                                         objMat.LoadMatMaster[i].mCATEGORY + ',' + objMat.LoadMatMaster[i].mCATEGORYDESC + ',' + objMat.LoadMatMaster[i].mMATGROUP + ',' +
                                         objMat.LoadMatMaster[i].mMATGROUPDESC + ',' + objMat.LoadMatMaster[i].mDESIGNNO + ',' + objMat.LoadMatMaster[i].mDESIGNDESC + ',' + objMat.LoadMatMaster[i].mFINISHCODE + ',' +
                                         objMat.LoadMatMaster[i].mFINISHDESC + ',' + objMat.LoadMatMaster[i].mVISIONPANELCODE + ',' + objMat.LoadMatMaster[i].mVISIONPANELDESC + ',' +
                                         objMat.LoadMatMaster[i].mLIPPINGCODE + ',' + objMat.LoadMatMaster[i].mLIPPINGDESC + ',' + objMat.LoadMatMaster[i].mUOM;

                        objLog.WriteLog("Load Material Master Record No. - " + Convert.ToString(i+1) + "==>" + _strLog);
                        if (objMat.LoadMatMaster[i].IsNull())
                        {
                            lstResponse.Add(new ClsMatMasterResponse("", "Bad request"));
                        }

                        if (objMat.LoadMatMaster[i].mMATCODE.IsNotNullOrWhiteSpace() && objMat.LoadMatMaster[i].mMATCODE.Length < 0)
                        {
                            lstResponse.Add(new ClsMatMasterResponse(objMat.LoadMatMaster[i].mMATCODE, "Invalid Material Code"));
                            clsob.ResopseMatMaster = lstResponse;
                            return clsob;
                        }
                        BCIL.Utility.BcilLogger.WriteMessage(LogLevel.Info, "Material Master :" + Serializer.Json.Serialize(objMat.LoadMatMaster[i].ToString()) + " saved.");
                        ClsMatMaster LP = new ClsMatMaster();
                        LP = ClsMatMaster.NewMatMasterOrder();
                        if (!LP.IsValid)
                        {
                            throw new WebFaultException<string>(string.Join(Environment.NewLine, LP.BrokenRulesCollection.Select(x => x.Description)), HttpStatusCode.BadRequest);
                        }
                        LP.Product = objMat.LoadMatMaster[i].mPRODUCT;
                        LP.MatCode = objMat.LoadMatMaster[i].mMATCODE;
                        LP.MatDesc = objMat.LoadMatMaster[i].mMATDESC;
                        LP.MatThickness = objMat.LoadMatMaster[i].mTHICKNESS;
                        LP.MatThicknessDesc = objMat.LoadMatMaster[i].mTHICKNESSDESC;
                        LP.MatSize = objMat.LoadMatMaster[i].mSIZE;
                        LP.MatGrade = objMat.LoadMatMaster[i].mGRADE;
                        LP.GradeDesc = objMat.LoadMatMaster[i].mGRADEDESC;
                        LP.Category = objMat.LoadMatMaster[i].mCATEGORY;
                        LP.CategoryDesc = objMat.LoadMatMaster[i].mCATEGORYDESC;
                        LP.MatGroup = objMat.LoadMatMaster[i].mMATGROUP;
                        LP.MatGroupDesc = objMat.LoadMatMaster[i].mMATGROUPDESC;
                        LP.DesignNo = objMat.LoadMatMaster[i].mDESIGNNO;
                        LP.DesignDesc = objMat.LoadMatMaster[i].mDESIGNDESC;
                        LP.FinishCode = objMat.LoadMatMaster[i].mFINISHCODE;
                        LP.FinishDesc = objMat.LoadMatMaster[i].mFINISHDESC;
                        LP.VisionPanelCode = objMat.LoadMatMaster[i].mVISIONPANELCODE;
                        LP.VisionPanelDesc = objMat.LoadMatMaster[i].mVISIONPANELDESC;
                        LP.LippingCode = objMat.LoadMatMaster[i].mLIPPINGCODE;
                        LP.LippingDesc = objMat.LoadMatMaster[i].mLIPPINGDESC;
                        LP.UOM = objMat.LoadMatMaster[i].mUOM;

                        LP.InsertMaterialMasterData(conMain);
                        objLog.WriteLog("Load SAP Material Master Data - Record " + Convert.ToString(i) + " ==> " + objMat.LoadMatMaster[i].mMATCODE + " Saved successfully at " + DateTime.Now.ToString());
                        lstResponse.Add(new ClsMatMasterResponse(objMat.LoadMatMaster[i].mMATCODE, 
                            "Load SAP Material Master Data - Record " + Convert.ToString(i) + " ==> " + objMat.LoadMatMaster[i].mMATCODE + " Saved successfully at " + DateTime.Now.ToString()));
                        clsob.ResopseMatMaster = lstResponse;
                    }
                    catch (Exception ex)
                    {
                        objLog.WriteLog("Load Material " + Convert.ToString(i) + " ==> " + ex.ToString() + " at " + DateTime.Now.ToString());
                        lstResponse.Add(new ClsMatMasterResponse(objMat.LoadMatMaster[i].mMATCODE, " - Downloading Failed in Material Code" + " at " + DateTime.Now.ToString()));
                        clsob.ResopseMatMaster = lstResponse;
                        return clsob;
                    }
                    clsob.ResopseMatMaster = lstResponse;
                }
                conMain.Close();
            }
            catch (Exception ex)
            {
                conMain.Close();
                lstResponse.Add(new ClsMatMasterResponse("", "Downloading Failed"));
            }
            return clsob;
        }


        [WebMethod(MessageName = "GetSAPRejMasterData")]
        public clsResponseRejList GetSAPRejMasterData(ListRejMasterDetails objRej, string UserName, string Pswd)
        {
            WriteLogFile objLog = new WriteLogFile();
            clsResponseRejList clsob = new clsResponseRejList();
            List<ClsRejMasterResponse> lstResponse = new List<ClsRejMasterResponse>();

            if (!Login(UserName, Pswd))
            {
                throw new BCILException("Authantication", new Exception("Not Valid user"));
            }
            string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

            SqlConnection conMain = new SqlConnection(conString);

            if (conMain.State == ConnectionState.Closed)
                conMain.Open();

            try
            {
                objLog.WriteLog("Load SAP Rejection Master -- No of Records Count - " + Convert.ToString(objRej.LoadRejMaster.Count));
                for (int i = 0; i < objRej.LoadRejMaster.Count; i++)
                {
                    try
                    {
                        string _strLog = objRej.LoadRejMaster[i].mREJCODE + ',' + objRej.LoadRejMaster[i].mREJDESC;

                        objLog.WriteLog("Load Rejection Master Record No. - " + Convert.ToString(i + 1) + "==>" + _strLog);
                        if (objRej.LoadRejMaster[i].IsNull())
                        {
                            lstResponse.Add(new ClsRejMasterResponse("", "Bad request"));
                        }

                        if (objRej.LoadRejMaster[i].mREJCODE.IsNotNullOrWhiteSpace() && objRej.LoadRejMaster[i].mREJCODE.Length < 0)
                        {
                            lstResponse.Add(new ClsRejMasterResponse(objRej.LoadRejMaster[i].mREJCODE, "Invalid Material Code"));
                            clsob.ResopseRejMaster = lstResponse;
                            return clsob;
                        }
                        BCIL.Utility.BcilLogger.WriteMessage(LogLevel.Info, "Material Master :" + Serializer.Json.Serialize(objRej.LoadRejMaster[i].ToString()) + " saved.");
                        ClsMatMaster LP = new ClsMatMaster();
                        LP = ClsMatMaster.NewMatMasterOrder();
                        if (!LP.IsValid)
                        {
                            throw new WebFaultException<string>(string.Join(Environment.NewLine, LP.BrokenRulesCollection.Select(x => x.Description)), HttpStatusCode.BadRequest);
                        }
                        LP.MatCode = objRej.LoadRejMaster[i].mREJCODE;
                        LP.MatDesc = objRej.LoadRejMaster[i].mREJDESC;

                        LP.InsertMaterialMasterData(conMain);
                        objLog.WriteLog("Load SAP Material Master Data - Record " + Convert.ToString(i) + " ==> " + objRej.LoadRejMaster[i].mREJCODE + " Saved successfully at " + DateTime.Now.ToString());
                    }
                    catch (Exception ex)
                    {
                        objLog.WriteLog("Load Material " + Convert.ToString(i) + " ==> " + ex.ToString() + " at " + DateTime.Now.ToString());
                        lstResponse.Add(new ClsRejMasterResponse(objRej.LoadRejMaster[i].mREJCODE, " - Downloading Failed in Rejection Code" + " at " + DateTime.Now.ToString()));
                        clsob.ResopseRejMaster = lstResponse;
                        return clsob;
                    }
                    clsob.ResopseRejMaster = lstResponse;
                }
                conMain.Close();
            }
            catch (Exception ex)
            {
                conMain.Close();
                lstResponse.Add(new ClsRejMasterResponse("", "Downloading Failed"));
            }
            return clsob;
        }


        [WebMethod(MessageName = "GetSAPPurchaseOrderData")]
        public clsResponsePurchaseOrderList GetSAPPurchaseOrderData(ListPurchaseOrderDetails objPO, string UserName, string Pswd)
        {
            WriteLogFile objLog = new WriteLogFile();
            clsResponsePurchaseOrderList clsob = new clsResponsePurchaseOrderList();
            List<ClsPurchaseOrderResponse> lstResponse = new List<ClsPurchaseOrderResponse>();

            if (!Login(UserName, Pswd))
            {
                throw new BCILException("Authantication", new Exception("Not Valid user"));
            }
            string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

            SqlConnection conMain = new SqlConnection(conString);

            if (conMain.State == ConnectionState.Closed)
                conMain.Open();

            try
            {
                objLog.WriteLog("Load SAP Purchase Order -- No of Records Count - " + Convert.ToString(objPO.LoadPurchaseOrderData.Count));
                for (int i = 0; i < objPO.LoadPurchaseOrderData.Count; i++)
                {
                    try
                    {
                        string _strLog = objPO.LoadPurchaseOrderData[i].tLOCATIONCODE + ',' + objPO.LoadPurchaseOrderData[i].tPONUMBER + ',' + objPO.LoadPurchaseOrderData[i].tVENDORCODE + ',' +
                                         objPO.LoadPurchaseOrderData[i].tMATCODE + ',' + objPO.LoadPurchaseOrderData[i].tMATDESC + ',' +  //+ objPO.LoadPurchaseOrderData[i].tMATTHICKNESS + ',' + objPO.LoadPurchaseOrderData[i].tMATSIZE + ',' +
                                         //objPO.LoadPurchaseOrderData[i].tMATGRADE + ',' + objPO.LoadPurchaseOrderData[i].tMATCATEGORY + ',' + objPO.LoadPurchaseOrderData[i].tMATGROUP + ',' +
                                         objPO.LoadPurchaseOrderData[i].tPOQTY + ',' + objPO.LoadPurchaseOrderData[i].tPOLocType + ',' + objPO.LoadPurchaseOrderData[i].tPODATE;

                        objLog.WriteLog("Load Purchase Order Record No. - " + Convert.ToString(i) + "==>" + _strLog);
                        if (objPO.LoadPurchaseOrderData[i].IsNull())
                        {
                            lstResponse.Add(new ClsPurchaseOrderResponse("", "Bad request"));
                        }

                        if (objPO.LoadPurchaseOrderData[i].tPONUMBER.IsNotNullOrWhiteSpace() && objPO.LoadPurchaseOrderData[i].tPONUMBER.Length < 0)
                        {
                            lstResponse.Add(new ClsPurchaseOrderResponse(objPO.LoadPurchaseOrderData[i].tPONUMBER, "Invalid Material Code"));
                            clsob.ResponsePurchaseOrderData = lstResponse;
                            return clsob;
                        }

                        if (objPO.LoadPurchaseOrderData[i].tLOCATIONCODE.IsNotNullOrWhiteSpace() && objPO.LoadPurchaseOrderData[i].tLOCATIONCODE.Length < 0)
                        {

                            lstResponse.Add(new ClsPurchaseOrderResponse(objPO.LoadPurchaseOrderData[i].tLOCATIONCODE, "Plant Code mandatry"));
                            clsob.ResponsePurchaseOrderData = lstResponse;
                            return clsob;
                        }

                        BCIL.Utility.BcilLogger.WriteMessage(LogLevel.Info, "Material Master :" + Serializer.Json.Serialize(objPO.LoadPurchaseOrderData[i].ToString()) + " saved.");
                        ClsPurchaseOrder LP = new ClsPurchaseOrder();
                        LP = ClsPurchaseOrder.NewPurchaseOrder();
                        if (!LP.IsValid)
                        {
                            throw new WebFaultException<string>(string.Join(Environment.NewLine, LP.BrokenRulesCollection.Select(x => x.Description)), HttpStatusCode.BadRequest);
                        }

                        LP.LocationCode = objPO.LoadPurchaseOrderData[i].tLOCATIONCODE;
                        LP.PONumber = objPO.LoadPurchaseOrderData[i].tPONUMBER;
                        LP.VendorCode = objPO.LoadPurchaseOrderData[i].tVENDORCODE;
                        LP.VendorName = objPO.LoadPurchaseOrderData[i].tVENDORNAME;
                        LP.MatCode = objPO.LoadPurchaseOrderData[i].tMATCODE;
                        LP.MatDesc = objPO.LoadPurchaseOrderData[i].tMATDESC;
                        LP.POLocType = objPO.LoadPurchaseOrderData[i].tPOLocType;
                        LP.POQty = Convert.ToInt32(objPO.LoadPurchaseOrderData[i].tPOQTY);
                        string sDate = objPO.LoadPurchaseOrderData[i].tPODATE.ToString();
                        LP.PODate = DateTime.ParseExact(sDate, "dd-MM-yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(objPO.LoadPurchaseOrderData[i].tPODATE).ToString("yyyy-MM-dd"); //Convert.ToDateTime(objPO.LoadPurchaseOrderData[i].tPODATE);

                        LP.InsertPurchaseOrderData(conMain);
                        objLog.WriteLog("Load SAP Purchase Order Data - Record " + Convert.ToString(i) + " ==> " + objPO.LoadPurchaseOrderData[i].tPONUMBER + " Saved successfully at " + DateTime.Now.ToString());
                    }
                    catch (Exception ex)
                    {
                        objLog.WriteLog("Load Purchase Order " + Convert.ToString(i) + " ==> " + ex.ToString() + " at " + DateTime.Now.ToString());
                        lstResponse.Add(new ClsPurchaseOrderResponse(objPO.LoadPurchaseOrderData[i].tPONUMBER, " - Downloading Failed in Purchase Order" + " at " + DateTime.Now.ToString()));
                        clsob.ResponsePurchaseOrderData = lstResponse;
                        return clsob;
                    }
                    clsob.ResponsePurchaseOrderData = lstResponse;
                }
                conMain.Close();
            }
            catch (Exception ex)
            {
                conMain.Close();
                lstResponse.Add(new ClsPurchaseOrderResponse("", "Downloading Failed"));
            }
            return clsob;
        }


        [WebMethod(MessageName = "GetSAPPurchaseOrderReturnData")]
        public clsResponsePurchaseOrderReturnList GetSAPPurchaseOrderReturnData(ListPurchaseOrderReturnDetails objPO, string UserName, string Pswd)
        {
            WriteLogFile objLog = new WriteLogFile();
            clsResponsePurchaseOrderReturnList clReturnsob = new clsResponsePurchaseOrderReturnList();
            List<ClsPurchaseOrderReturnResponse> lstReturnResponse = new List<ClsPurchaseOrderReturnResponse>();

            if (!Login(UserName, Pswd))
            {
                throw new BCILException("Authantication", new Exception("Not Valid user"));
            }
            string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

            SqlConnection conMain = new SqlConnection(conString);

            if (conMain.State == ConnectionState.Closed)
                conMain.Open();

            try
            {
                objLog.WriteLog("Load SAP Purchase Order Return -- No of Records Count - " + Convert.ToString(objPO.LoadPurchaseOrderReturnData.Count));
                for (int i = 0; i < objPO.LoadPurchaseOrderReturnData.Count; i++)
                {
                    try
                    {
                        string _strLog = objPO.LoadPurchaseOrderReturnData[i].tLOCATIONCODE + ',' + objPO.LoadPurchaseOrderReturnData[i].tRETURNPONUMBER + ',' + objPO.LoadPurchaseOrderReturnData[i].tVENDORCODE + ',' +
                                         objPO.LoadPurchaseOrderReturnData[i].tVENDORNAME + ',' + objPO.LoadPurchaseOrderReturnData[i].tMATCODE + ',' + objPO.LoadPurchaseOrderReturnData[i].tMATDESC + ',' +
                                         objPO.LoadPurchaseOrderReturnData[i].tPOReturnQTY + ',' + objPO.LoadPurchaseOrderReturnData[i].tPOLocType;

                        objLog.WriteLog("Load Purchase Order Return Record No. - " + Convert.ToString(i) + "==>" + _strLog);
                        if (objPO.LoadPurchaseOrderReturnData[i].IsNull())
                        {
                            lstReturnResponse.Add(new ClsPurchaseOrderReturnResponse("", "Bad request"));
                        }

                        if (objPO.LoadPurchaseOrderReturnData[i].tRETURNPONUMBER.IsNotNullOrWhiteSpace() && objPO.LoadPurchaseOrderReturnData[i].tRETURNPONUMBER.Length < 0)
                        {
                            lstReturnResponse.Add(new ClsPurchaseOrderReturnResponse(objPO.LoadPurchaseOrderReturnData[i].tRETURNPONUMBER, "Invalid Material Code"));
                            clReturnsob.ResponsePurchaseOrderReturnData = lstReturnResponse;
                            return clReturnsob;
                        }

                        if (objPO.LoadPurchaseOrderReturnData[i].tLOCATIONCODE.IsNotNullOrWhiteSpace() && objPO.LoadPurchaseOrderReturnData[i].tLOCATIONCODE.Length < 0)
                        {

                            lstReturnResponse.Add(new ClsPurchaseOrderReturnResponse(objPO.LoadPurchaseOrderReturnData[i].tLOCATIONCODE, "Plant Code mandatry"));
                            clReturnsob.ResponsePurchaseOrderReturnData = lstReturnResponse;
                            return clReturnsob;
                        }

                        BCIL.Utility.BcilLogger.WriteMessage(LogLevel.Info, "Material Master :" + Serializer.Json.Serialize(objPO.LoadPurchaseOrderReturnData[i].ToString()) + " saved.");
                        ClsPurchaseOrderReturn LP = new ClsPurchaseOrderReturn();
                        LP = ClsPurchaseOrderReturn.NewPurchaseOrderReturn();
                        if (!LP.IsValid)
                        {
                            throw new WebFaultException<string>(string.Join(Environment.NewLine, LP.BrokenRulesCollection.Select(x => x.Description)), HttpStatusCode.BadRequest);
                        }

                        LP.LocationCode = objPO.LoadPurchaseOrderReturnData[i].tLOCATIONCODE;
                        LP.POReturnNumber = objPO.LoadPurchaseOrderReturnData[i].tRETURNPONUMBER;
                        LP.VendorCode = objPO.LoadPurchaseOrderReturnData[i].tVENDORCODE;
                        LP.VendorName = objPO.LoadPurchaseOrderReturnData[i].tVENDORNAME;
                        LP.MatCode = objPO.LoadPurchaseOrderReturnData[i].tMATCODE;
                        LP.MatDesc = objPO.LoadPurchaseOrderReturnData[i].tMATDESC;
                        LP.ReturnQty = Convert.ToInt32(objPO.LoadPurchaseOrderReturnData[i].tPOReturnQTY);
                        LP.POLocType = objPO.LoadPurchaseOrderReturnData[i].tPOLocType;
                        //string sDate = objPO.LoadPurchaseOrderReturnData[i].tPODATE.ToString();
                        //LP.PODate = DateTime.ParseExact(sDate, "dd-MM-yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(objPO.LoadPurchaseOrderData[i].tPODATE).ToString("yyyy-MM-dd"); //Convert.ToDateTime(objPO.LoadPurchaseOrderData[i].tPODATE);

                        LP.InsertPurchaseOrderReturnData(conMain);
                        objLog.WriteLog("Load SAP Purchase Order Return Data - Record " + Convert.ToString(i) + " ==> " + objPO.LoadPurchaseOrderReturnData[i].tRETURNPONUMBER + " Saved successfully at " + DateTime.Now.ToString());
                    }
                    catch (Exception ex)
                    {
                        objLog.WriteLog("Load Purchase Order " + Convert.ToString(i) + " ==> " + ex.ToString() + " at " + DateTime.Now.ToString());
                        lstReturnResponse.Add(new ClsPurchaseOrderReturnResponse(objPO.LoadPurchaseOrderReturnData[i].tRETURNPONUMBER, " - Downloading Failed in Purchase Order Return" + " at " + DateTime.Now.ToString()));
                        clReturnsob.ResponsePurchaseOrderReturnData = lstReturnResponse;
                        return clReturnsob;
                    }
                    clReturnsob.ResponsePurchaseOrderReturnData = lstReturnResponse;
                }
                conMain.Close();
            }
            catch (Exception ex)
            {
                conMain.Close();
                lstReturnResponse.Add(new ClsPurchaseOrderReturnResponse("", "Downloading Failed"));
            }
            return clReturnsob;
        }


        [WebMethod(MessageName = "GetSAPDeliveryOrderData")]
        public clsResponseDeliveryOrderList GetSAPDeliveryOrderData(ListDeliveryOrderDetails objPO, string UserName, string Pswd)
        {
            WriteLogFile objLog = new WriteLogFile();
            clsResponseDeliveryOrderList clsob = new clsResponseDeliveryOrderList();
            List<ClsDeliveryOrderResponse> lstResponse = new List<ClsDeliveryOrderResponse>();

            if (!Login(UserName, Pswd))
            {
                throw new BCILException("Authantication", new Exception("Not Valid user"));
            }
            string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

            SqlConnection conMain = new SqlConnection(conString);

            if (conMain.State == ConnectionState.Closed)
                conMain.Open();

            try
            {
                objLog.WriteLog("Load SAP Delivery Order -- No of Records Count - " + Convert.ToString(objPO.LoadDeliveryOrderData.Count));
                for (int i = 0; i < objPO.LoadDeliveryOrderData.Count; i++)
                {
                    try
                    {
                        string _strLog = objPO.LoadDeliveryOrderData[i].tLOCATIONCODE + ',' + objPO.LoadDeliveryOrderData[i].tDELIVERYORDERNO + ',' + objPO.LoadDeliveryOrderData[i].tCUSTOMERCODE + ',' +
                                         objPO.LoadDeliveryOrderData[i].tCUSTOMERNAME + ',' + objPO.LoadDeliveryOrderData[i].tMATCODE + ',' + objPO.LoadDeliveryOrderData[i].tMATDESC + ',' + objPO.LoadDeliveryOrderData[i].tTOLOCATIONCODE + ',' +
                                         objPO.LoadDeliveryOrderData[i].tDOQTY + ',' + objPO.LoadDeliveryOrderData[i].tDODATE;

                        objLog.WriteLog("Load Delivery Order Record No. - " + Convert.ToString(i) + "==>" + _strLog);
                        if (objPO.LoadDeliveryOrderData[i].IsNull())
                        {
                            lstResponse.Add(new ClsDeliveryOrderResponse("", "Bad request"));
                        }

                        if (objPO.LoadDeliveryOrderData[i].tDELIVERYORDERNO.IsNotNullOrWhiteSpace() && objPO.LoadDeliveryOrderData[i].tDELIVERYORDERNO.Length < 0)
                        {
                            lstResponse.Add(new ClsDeliveryOrderResponse(objPO.LoadDeliveryOrderData[i].tDELIVERYORDERNO, "Invalid Material Code"));
                            clsob.ResponseDeliveryOrderData = lstResponse;
                            return clsob;
                        }

                        if (objPO.LoadDeliveryOrderData[i].tLOCATIONCODE.IsNotNullOrWhiteSpace() && objPO.LoadDeliveryOrderData[i].tLOCATIONCODE.Length < 0)
                        {

                            lstResponse.Add(new ClsDeliveryOrderResponse(objPO.LoadDeliveryOrderData[i].tLOCATIONCODE, "Plant Code mandatry"));
                            clsob.ResponseDeliveryOrderData = lstResponse;
                            return clsob;
                        }

                        BCIL.Utility.BcilLogger.WriteMessage(LogLevel.Info, "Material Master :" + Serializer.Json.Serialize(objPO.LoadDeliveryOrderData[i].ToString()) + " saved.");
                        ClsDeliveryOrder LP = new ClsDeliveryOrder();
                        LP = ClsDeliveryOrder.NewDeliveryOrder();
                        if (!LP.IsValid)
                        {
                            throw new WebFaultException<string>(string.Join(Environment.NewLine, LP.BrokenRulesCollection.Select(x => x.Description)), HttpStatusCode.BadRequest);
                        }

                        LP.LocationCode = objPO.LoadDeliveryOrderData[i].tLOCATIONCODE;
                        LP.DeliveryOrderNo = objPO.LoadDeliveryOrderData[i].tDELIVERYORDERNO;
                        LP.CustomerCode = objPO.LoadDeliveryOrderData[i].tCUSTOMERCODE;
                        LP.CustomerName = objPO.LoadDeliveryOrderData[i].tCUSTOMERNAME;
                        LP.MatCode = objPO.LoadDeliveryOrderData[i].tMATCODE;
                        LP.MatDesc = objPO.LoadDeliveryOrderData[i].tMATDESC;
                        LP.ToLocationCode = objPO.LoadDeliveryOrderData[i].tTOLOCATIONCODE;
                        LP.DeliveryOrderQty = Convert.ToInt32(objPO.LoadDeliveryOrderData[i].tDOQTY);
                        string sDate = objPO.LoadDeliveryOrderData[i].tDODATE.ToString();
                        LP.DeliveryOrderDate = DateTime.ParseExact(sDate, "dd-MM-yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(sDate).ToString("yyyy-MM-dd"); //Convert.ToDateTime(objPO.LoadDeliveryOrderData[i].tDODATE);

                        LP.InsertDeliveryOrderData(conMain);
                        objLog.WriteLog("Load SAP Delivery Order Data - Record " + Convert.ToString(i) + " ==> " + objPO.LoadDeliveryOrderData[i].tDELIVERYORDERNO + " Saved successfully at " + DateTime.Now.ToString());
                    }
                    catch (Exception ex)
                    {
                        objLog.WriteLog("Load Delivery Order " + Convert.ToString(i) + " ==> " + ex.ToString() + " at " + DateTime.Now.ToString());
                        lstResponse.Add(new ClsDeliveryOrderResponse(objPO.LoadDeliveryOrderData[i].tDELIVERYORDERNO, " - Downloading Failed in Delivery Order" + " at " + DateTime.Now.ToString()));
                        clsob.ResponseDeliveryOrderData = lstResponse;
                        return clsob;
                    }
                    clsob.ResponseDeliveryOrderData = lstResponse;
                }
                conMain.Close();
            }
            catch (Exception ex)
            {
                conMain.Close();
                lstResponse.Add(new ClsDeliveryOrderResponse("", "Downloading Failed"));
            }
            return clsob;
        }


        [WebMethod(MessageName = "GetSAPSalesReturnData")]
        public clsResponseSalesReturnList GetSAPSalesReturnData(ListSalesReturnDetails objPO, string UserName, string Pswd)
        {
            WriteLogFile objLog = new WriteLogFile();
            clsResponseSalesReturnList clsob = new clsResponseSalesReturnList();
            List<ClsSalesReturnResponse> lstResponse = new List<ClsSalesReturnResponse>();

            if (!Login(UserName, Pswd))
            {
                throw new BCILException("Authantication", new Exception("Not Valid user"));
            }
            string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

            SqlConnection conMain = new SqlConnection(conString);

            if (conMain.State == ConnectionState.Closed)
                conMain.Open();

            try
            {
                objLog.WriteLog("Load SAP Sales Return -- No of Records Count - " + Convert.ToString(objPO.LoadSalesReturnData.Count));
                for (int i = 0; i < objPO.LoadSalesReturnData.Count; i++)
                {
                    try
                    {
                        string _strLog = objPO.LoadSalesReturnData[i].tLOCATIONCODE + ',' + objPO.LoadSalesReturnData[i].tSALESRETURNNO + ',' + objPO.LoadSalesReturnData[i].tCUSTOMERCODE + ',' +
                                         objPO.LoadSalesReturnData[i].tCUSTOMERNAME + ',' + objPO.LoadSalesReturnData[i].tMATCODE + ',' + objPO.LoadSalesReturnData[i].tMATDESC + ',' +
                                         objPO.LoadSalesReturnData[i].tRETURNQTY;  

                        objLog.WriteLog("Load Sales Return Record No. - " + Convert.ToString(i) + "==>" + _strLog);
                        if (objPO.LoadSalesReturnData[i].IsNull())
                        {
                            lstResponse.Add(new ClsSalesReturnResponse("", "Bad request"));
                        }

                        if (objPO.LoadSalesReturnData[i].tSALESRETURNNO.IsNotNullOrWhiteSpace() && objPO.LoadSalesReturnData[i].tSALESRETURNNO.Length < 0)
                        {
                            lstResponse.Add(new ClsSalesReturnResponse(objPO.LoadSalesReturnData[i].tSALESRETURNNO, "Invalid Material Code"));
                            clsob.ResponseSalesReturnData = lstResponse;
                            return clsob;
                        }

                        if (objPO.LoadSalesReturnData[i].tLOCATIONCODE.IsNotNullOrWhiteSpace() && objPO.LoadSalesReturnData[i].tLOCATIONCODE.Length < 0)
                        {

                            lstResponse.Add(new ClsSalesReturnResponse(objPO.LoadSalesReturnData[i].tLOCATIONCODE, "Plant Code mandatry"));
                            clsob.ResponseSalesReturnData = lstResponse;
                            return clsob;
                        }

                        BCIL.Utility.BcilLogger.WriteMessage(LogLevel.Info, "Sales Return :" + Serializer.Json.Serialize(objPO.LoadSalesReturnData[i].ToString()) + " saved.");
                        ClsSalesReturn LP = new ClsSalesReturn();
                        LP = ClsSalesReturn.NewSalesReturn();
                        if (!LP.IsValid)
                        {
                            throw new WebFaultException<string>(string.Join(Environment.NewLine, LP.BrokenRulesCollection.Select(x => x.Description)), HttpStatusCode.BadRequest);
                        }

                        LP.LocationCode = objPO.LoadSalesReturnData[i].tLOCATIONCODE;
                        LP.SalesReturnNo = objPO.LoadSalesReturnData[i].tSALESRETURNNO;
                        LP.CustomerCode = objPO.LoadSalesReturnData[i].tCUSTOMERCODE;
                        LP.CustomerName = objPO.LoadSalesReturnData[i].tCUSTOMERNAME;
                        LP.MatCode = objPO.LoadSalesReturnData[i].tMATCODE;
                        LP.MatDesc = objPO.LoadSalesReturnData[i].tMATDESC;
                        LP.ReturnQty = Convert.ToInt32(objPO.LoadSalesReturnData[i].tRETURNQTY);

                        LP.InsertSalesReturnData(conMain);
                        objLog.WriteLog("Load SAP Sales Return Data - Record " + Convert.ToString(i) + " ==> " + objPO.LoadSalesReturnData[i].tSALESRETURNNO + " Saved successfully at " + DateTime.Now.ToString());
                    }
                    catch (Exception ex)
                    {
                        objLog.WriteLog("Load Sales Return " + Convert.ToString(i) + " ==> " + ex.ToString() + " at " + DateTime.Now.ToString());
                        lstResponse.Add(new ClsSalesReturnResponse(objPO.LoadSalesReturnData[i].tSALESRETURNNO, " - Downloading Failed in Sales Return" + " at " + DateTime.Now.ToString()));
                        clsob.ResponseSalesReturnData = lstResponse;
                        return clsob;
                    }
                    clsob.ResponseSalesReturnData = lstResponse;
                }
                conMain.Close();
            }
            catch (Exception ex)
            {
                conMain.Close();
                lstResponse.Add(new ClsSalesReturnResponse("", "Downloading Failed"));
            }
            return clsob;
        }


        [WebMethod(MessageName = "GetSAPQualityInspectionData")]
        public clsResponseQuanlityInspectionList GetSAPQualityInspectionData(ListQualityInspectionDetails objPO, string UserName, string Pswd)
        {
            WriteLogFile objLog = new WriteLogFile();
            clsResponseQuanlityInspectionList clsob = new clsResponseQuanlityInspectionList();
            List<ClsQuanlityInspectionResponse> lstResponse = new List<ClsQuanlityInspectionResponse>();

            if (!Login(UserName, Pswd))
            {
                throw new BCILException("Authantication", new Exception("Not Valid user"));
            }
            string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

            SqlConnection conMain = new SqlConnection(conString);

            if (conMain.State == ConnectionState.Closed)
                conMain.Open();

            try
            {
                //objLog.WriteLog("Load SAP QualityInspection -- No of Records Count - " + Convert.ToString(objPO.LoadQualityInspectionData.Count));
                for (int i = 0; i < objPO.LoadQualityInspectionData.Count; i++)
                {
                    try
                    {
                        string _strLog = objPO.LoadQualityInspectionData[i].tLOCATIONCODE + ',' + objPO.LoadQualityInspectionData[i].tPONO + ',' + objPO.LoadQualityInspectionData[i].tMATCODE + ',' +
                                         objPO.LoadQualityInspectionData[i].tQRCODE + ',' + objPO.LoadQualityInspectionData[i].tMIGONO + ',' + objPO.LoadQualityInspectionData[i].tINSPECTIONLOTNO;

                        //objLog.WriteLog("Load SAP Quality Inspection Record No. - " + Convert.ToString(i) + "=>" + _strLog);
                        if (objPO.LoadQualityInspectionData[i].IsNull())
                        {
                            lstResponse.Add(new ClsQuanlityInspectionResponse("", "Bad request"));
                        }
                        if (objPO.LoadQualityInspectionData[i].tMIGONO.IsNotNullOrWhiteSpace() && objPO.LoadQualityInspectionData[i].tMIGONO.Length < 0)
                        {
                            lstResponse.Add(new ClsQuanlityInspectionResponse(objPO.LoadQualityInspectionData[i].tMIGONO, "Invalid Material Code"));
                            clsob.ResponseQuanlityInspectionData = lstResponse;
                            return clsob;
                        }
                        if (objPO.LoadQualityInspectionData[i].tLOCATIONCODE.IsNotNullOrWhiteSpace() && objPO.LoadQualityInspectionData[i].tLOCATIONCODE.Length < 0)
                        {
                            lstResponse.Add(new ClsQuanlityInspectionResponse(objPO.LoadQualityInspectionData[i].tLOCATIONCODE, "Plant Code mandatry"));
                            clsob.ResponseQuanlityInspectionData = lstResponse;
                            return clsob;
                        }
                        //BCIL.Utility.BcilLogger.WriteMessage(LogLevel.Info, "QualityInspectionData :" + Serializer.Json.Serialize(objPO.LoadQualityInspectionData[i].ToString()) + " saved.");
                        ClsQualityInspection LP = new ClsQualityInspection();
                        LP = ClsQualityInspection.UpdateQuanlityInspection();
                        if (!LP.IsValid)
                        {
                            throw new WebFaultException<string>(string.Join(Environment.NewLine, LP.BrokenRulesCollection.Select(x => x.Description)), HttpStatusCode.BadRequest);
                        }

                        LP.LocationCode = objPO.LoadQualityInspectionData[i].tLOCATIONCODE;
                        LP.PurchaseOrderNo = objPO.LoadQualityInspectionData[i].tPONO;
                        LP.MatCode = objPO.LoadQualityInspectionData[i].tMATCODE;
                        LP.QRCode = objPO.LoadQualityInspectionData[i].tQRCODE;
                        LP.MIGONo = objPO.LoadQualityInspectionData[i].tMIGONO;
                        LP.InspLotNo = objPO.LoadQualityInspectionData[i].tINSPECTIONLOTNO;
                        //string sDate = objPO.LoadQualityInspectionData[i].tDODATE.ToString();
                        //LP.DeliveryOrderDate = DateTime.ParseExact(sDate, "dd-MM-yyyy", CultureInfo.InvariantCulture); //Convert.ToDateTime(sDate).ToString("yyyy-MM-dd"); //Convert.ToDateTime(objPO.LoadDeliveryOrderData[i].tDODATE);

                        LP.InsertQualityInspData(conMain);
                        objLog.WriteLog("SAP Quality Inspection ==> LocationCode - " + LP.LocationCode + ", PONumber - " + LP.PurchaseOrderNo + ", Matcode - "+ LP.MatCode + ", QRCode - " + LP.QRCode + ", MIGONo - "+ LP.MIGONo + ", InspectionLotNo - " + LP.InspLotNo + " Updated successfully");
                    }
                    catch (Exception ex)
                    {
                        objLog.WriteLog("SAP Quality Inspection PONumber ==> Error - " + ex.ToString());
                        lstResponse.Add(new ClsQuanlityInspectionResponse(objPO.LoadQualityInspectionData[i].tLOCATIONCODE, " - Downloading Failed in Quanlity InspData"));
                        clsob.ResponseQuanlityInspectionData = lstResponse;
                        return clsob;
                    }
                    clsob.ResponseQuanlityInspectionData = lstResponse;
                }
                conMain.Close();
            }
            catch (Exception ex)
            {
                conMain.Close();
                lstResponse.Add(new ClsQuanlityInspectionResponse("", "Downloading Failed"));
            }
            return clsob;
        }


    }

    public class ListMatMasterDetails
    {
        public List<ClsGetSAPMaterialMaster> LoadMatMaster { get; set; }
    }

    public class ListRejMasterDetails
    {
        public List<ClsGetSAPRejectionMaster> LoadRejMaster { get; set; }
    }

    public class ListPurchaseOrderDetails
    {
        public List<ClsGetPurchaseOrderData> LoadPurchaseOrderData { get; set; }
    }

    public class ListPurchaseOrderReturnDetails
    {
        public List<ClsGetPurchaseOrderReturnData> LoadPurchaseOrderReturnData { get; set; }
    }

    public class ListDeliveryOrderDetails
    {
        public List<ClsGetDeliveryOrderData> LoadDeliveryOrderData { get; set; }
    }

    public class ListQualityInspectionDetails
    {
        public List<ClsGetQualityInspectionData> LoadQualityInspectionData { get; set; }
    }

    public class ListSalesReturnDetails
    {
        public List<ClsGetSalesReturnData> LoadSalesReturnData { get; set; }
    }

    public class ListLocationLabelPrinting
    {
        public List<ClsGetSalesReturnData> LoadSalesReturnData { get; set; }
    }

    public class WriteLogFile
    {
        public void WriteLog(string strLog)
        {
            StreamWriter log;
            FileStream fileStream = null;
            DirectoryInfo logDirInfo = null;
            FileInfo logFileInfo;

            string logFilePath;
            logFilePath = AppDomain.CurrentDomain.BaseDirectory + "\\LogFiles\\" + "LogFile-" + System.DateTime.Today.ToString("dd-MM-yyyy") + "." + "txt";
            logFileInfo = new FileInfo(logFilePath);
            logDirInfo = new DirectoryInfo(logFileInfo.DirectoryName);
            if (!logDirInfo.Exists)
                logDirInfo.Create();
            if (!logFileInfo.Exists)
            {
                fileStream = logFileInfo.Create();
            }
            else
            {
                fileStream = new FileStream(logFilePath, FileMode.Append);
            }
            log = new StreamWriter(fileStream);
            log.WriteLine("(Version: 1.1.0) : " + DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss") + " " + strLog);
            log.Close();
        }
    }

}


