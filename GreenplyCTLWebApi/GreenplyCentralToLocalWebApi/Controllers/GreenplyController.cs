using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Web.Http;
using System.Xml;
using BcilLib;
using GreenplyCentralToLocalWebApi.BI;

namespace GreenplyCentralToLocalWebApi.Controllers
{
    [AuthenticationFilter]
    //[AllowAnonymous]
    //[RoutePrefix("api/Greenply")]
    [RoutePrefix("api/GreenplyCentralToLocal")]
    public class GreenplyController : ApiController
    {
        WriteLogFile ObjLog = new WriteLogFile();
        public void POST()
        {

        }

        [HttpGet]
        [Route("GETC2LMASTERSDATA")]
        public HttpResponseMessage GetC2LMastersData(HttpRequestMessage request)
        {
            string value = string.Empty;
            string strMaterialMaster = string.Empty;
            string strEmailConfigMaster = string.Empty;
            string strGroupRights = string.Empty;
            string strUserMaster = string.Empty;
            string strVendorMaster = string.Empty;
            string strWarehouseMaster = string.Empty;
            string strPlantMaster = string.Empty;

            string ResponseString = "";
            StringBuilder stb = new StringBuilder();
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                DataSet dt;
                SaveSapData obj = new SaveSapData();
                dt = obj.GetC2LMastersData();
                DataTable dtMaterialMaster = dt.Tables[0].Copy();
                DataTable dtEmailConfigMaster = dt.Tables[1].Copy();
                DataTable dtGroupRights = dt.Tables[2].Copy();
                DataTable dtUserMaster = dt.Tables[3].Copy();
                DataTable dtVendorMaster = dt.Tables[4].Copy();
                DataTable dtWarehouseMaster = dt.Tables[5].Copy();
                DataTable dtPlantMaster = dt.Tables[6].Copy();

                dtMaterialMaster.TableName = "mMaterialMaster";
                dtEmailConfigMaster.TableName = "mEmailConfigMaster";
                dtGroupRights.TableName = "mGroupRights";
                dtUserMaster.TableName = "mUserMaster";
                dtVendorMaster.TableName = "mVendorMaster";
                dtWarehouseMaster.TableName = "mWarehouseMaster";
                dtPlantMaster.TableName = "mPlantMaster";
                
                if (dtMaterialMaster.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtMaterialMaster.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strMaterialMaster = xmlStr;

                }
                if (dtEmailConfigMaster.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtEmailConfigMaster.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strEmailConfigMaster = xmlStr;
                }
                if (dtGroupRights.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtGroupRights.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strGroupRights = xmlStr;
                }
                if (dtUserMaster.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtUserMaster.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strUserMaster = xmlStr;
                }
                if (dtVendorMaster.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtVendorMaster.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strVendorMaster = xmlStr;
                }
                if (dtWarehouseMaster.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtWarehouseMaster.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strWarehouseMaster = xmlStr;
                }
                if (dtPlantMaster.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtPlantMaster.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strPlantMaster = xmlStr;
                }

                if ((strMaterialMaster.Length > 0 || strEmailConfigMaster.Length > 0)
                  || (strGroupRights.Length > 0 || strUserMaster.Length > 0 || strVendorMaster.Length > 0 || strWarehouseMaster.Length > 0 || strPlantMaster.Length > 0))
                {
                    value = strMaterialMaster + Environment.NewLine + strEmailConfigMaster;
                    value = value + strGroupRights + Environment.NewLine + strUserMaster + Environment.NewLine + strVendorMaster + Environment.NewLine + strWarehouseMaster;
                    value = value + strPlantMaster;
                    stb.AppendLine("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:Greenply.in:India:Transactional:PP:CentralToLocal\">");
                    stb.AppendLine("<soapenv:Header/>");
                    stb.AppendLine("<soapenv:Body>");
                    stb.AppendLine("<urn:CentralToLocal>");
                    stb.AppendLine(value);
                    stb.AppendLine("<!--End:CentralToLocal Data:-->");
                    stb.AppendLine("</urn:CentralToLocal>");
                    stb.AppendLine("</soapenv:Body>");
                    stb.AppendLine("</soapenv:Envelope>");
                    //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, MethodBase.GetCurrentMethod().Name, "RestClient xml Send to Sap " + stb.ToString());
                    stb.ToString();
                }
            }
            catch (Exception ex)
            {
                //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, MethodBase.GetCurrentMethod().Name, "Rm Receiving Method Api Error: " + ex.Message.ToString().ToString());
                return new HttpResponseMessage()
                {
                    Content = new StringContent(ex.Message.ToString(), Encoding.UTF8, "application/xml")
                };
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(stb.ToString(), Encoding.UTF8, "application/xml")
            };
        }

        [HttpGet]
        [Route("GETC2LTRANSACTIONALDATA")]
        
        public HttpResponseMessage GetC2LTransactionalData(HttpRequestMessage request, string LocationCode, string LocationType, string POLocType)  //, string LocationCode, string LocationType, string POLocType
        {
            ObjLog.WriteLog("GetC2LTransactionalData => Data Receiving Request : Data receive for LocationCode - " + LocationCode + ", LocationType - " + LocationType + " and POLocType - " + POLocType);
            string value = string.Empty;
            string strSAPDeliveryOrderData = string.Empty;
            string strSAPPurchaseOrderData = string.Empty;
            string strSAPSalesReturnData = string.Empty;
            string strSAPReturnPurchaseOrderData = string.Empty;
            string strSAPVendorGenData = string.Empty;
            string strSAPQAData = string.Empty;

            DataTable dtSAPDeliveryOrderData = new DataTable();
            DataTable dtSAPPurchaseOrderData = new DataTable();
            DataTable dtSAPSalesReturnData = new DataTable();
            DataTable dtSAPReturnPurchaseOrderData = new DataTable();
            DataTable dtSAPVendorGenData = new DataTable();
            DataTable dtSAPQAData = new DataTable();

            string ResponseString = "";
            StringBuilder stb = new StringBuilder();
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                DataSet dt;
                SaveSapData obj = new SaveSapData();
                dt = obj.GetC2LTransactionalData(LocationCode, LocationType, POLocType);
                if (LocationType == "PLANT")
                {
                    dtSAPDeliveryOrderData = dt.Tables[0].Copy();
                    dtSAPSalesReturnData = dt.Tables[1].Copy();
                    dtSAPPurchaseOrderData = dt.Tables[2].Copy();
                    dtSAPReturnPurchaseOrderData = dt.Tables[3].Copy();
                    //ObjLog.WriteLog("GetC2LTransactionalData => Data Receiving Request : LocationCode - " + LocationCode + ", Data receive DeliveryOrder - " + dtSAPDeliveryOrderData.Rows.Count + " and SalesReturn - " + dtSAPSalesReturnData.Rows.Count + " and PurchaseOrder - " + dtSAPPurchaseOrderData.Rows.Count + " and ReturnPurchaseOrder - " + dtSAPReturnPurchaseOrderData.Rows.Count);
                }
                if (LocationType == "HUB")
                {
                    dtSAPDeliveryOrderData = dt.Tables[0].Copy();
                    dtSAPSalesReturnData = dt.Tables[1].Copy();
                    dtSAPPurchaseOrderData = dt.Tables[2].Copy();
                    dtSAPReturnPurchaseOrderData = dt.Tables[3].Copy();
                    dtSAPQAData = dt.Tables[4].Copy();
                    ObjLog.WriteLog("GetC2LTransactionalData => Data Receiving Request : LocationCode - " + LocationCode + ", Data receive DeliveryOrder - " + dtSAPDeliveryOrderData.Rows.Count + " and SalesReturn - " + dtSAPSalesReturnData.Rows.Count + " and PurchaseOrder - " + dtSAPPurchaseOrderData.Rows.Count + " and ReturnPurchaseOrder - " + dtSAPReturnPurchaseOrderData.Rows.Count + " and QualityInspection - " + dtSAPQAData.Rows.Count);
                }
                if (LocationType == "VENDOR")
                {
                    dtSAPPurchaseOrderData = dt.Tables[0].Copy();
                    dtSAPVendorGenData = dt.Tables[1].Copy();
                    //ObjLog.WriteLog("GetC2LTransactionalData => Data Receiving Request : Data receive DeliveryOrder - " + dtSAPVendorGenData.Rows.Count);
                }

                dtSAPDeliveryOrderData.TableName = "tSAPDeliveryOrderData";
                dtSAPSalesReturnData.TableName = "tSAPSalesReturnData";
                dtSAPPurchaseOrderData.TableName = "tSAPPurchaseOrderData";
                dtSAPReturnPurchaseOrderData.TableName = "tSAPReturnPurchaseOrderData";
                dtSAPVendorGenData.TableName = "tVendorLabelGenerating";
                dtSAPQAData.TableName = "tSAPQAData";

                if (dtSAPDeliveryOrderData.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtSAPDeliveryOrderData.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strSAPDeliveryOrderData = xmlStr;
                }
                if (dtSAPSalesReturnData.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtSAPSalesReturnData.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strSAPSalesReturnData = xmlStr;
                }
                if (dtSAPPurchaseOrderData.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtSAPPurchaseOrderData.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strSAPPurchaseOrderData = xmlStr;
                }
                if (dtSAPReturnPurchaseOrderData.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtSAPReturnPurchaseOrderData.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strSAPReturnPurchaseOrderData = xmlStr;
                }
                if (dtSAPQAData.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtSAPQAData.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strSAPQAData = xmlStr;
                }
                if (dtSAPVendorGenData.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtSAPVendorGenData.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strSAPVendorGenData = xmlStr;
                }
                if (strSAPDeliveryOrderData.Length > 0 || strSAPPurchaseOrderData.Length > 0 || strSAPSalesReturnData.Length > 0 || strSAPReturnPurchaseOrderData.Length > 0 || strSAPVendorGenData.Length > 0 || strSAPQAData.Length > 0)
                {
                    value = strSAPDeliveryOrderData + Environment.NewLine + strSAPPurchaseOrderData + Environment.NewLine + strSAPSalesReturnData + Environment.NewLine + strSAPReturnPurchaseOrderData + Environment.NewLine + strSAPVendorGenData + Environment.NewLine + strSAPQAData;
                    
                    stb.AppendLine("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:Greenply.in:India:Transactional:PP:CentralToLocal\">");
                    stb.AppendLine("<soapenv:Header/>");
                    stb.AppendLine("<soapenv:Body>");
                    stb.AppendLine("<urn:CentralToLocal>");
                    stb.AppendLine(value);
                    stb.AppendLine("<!--End:CentralToLocal Data:-->");
                    stb.AppendLine("</urn:CentralToLocal>");
                    stb.AppendLine("</soapenv:Body>");
                    stb.AppendLine("</soapenv:Envelope>");
                    //ObjLog.WriteLog("GetC2LTransactionalData => RestClient xml Send to Sap " + stb.ToString());
                    //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, MethodBase.GetCurrentMethod().Name, "RestClient xml Send to Sap " + stb.ToString());
                    stb.ToString();
                }
            }
            catch (Exception ex)
            {
                ObjLog.WriteLog("GetC2LTransactionalData => RestClient xml Received From SAP ERROR :  " + ex.ToString());
                return new HttpResponseMessage()
                {
                    Content = new StringContent(ex.Message.ToString(), Encoding.UTF8, "application/xml")
                };
            }
            return new HttpResponseMessage()
            {
                Content = new StringContent(stb.ToString(), Encoding.UTF8, "application/xml")//
            };
        }

        [HttpGet]
        [Route("GETDATA")]
        public HttpResponseMessage GetData(HttpRequestMessage request)
        {
            string value = string.Empty;
            string strMaterialMaster = string.Empty;
            string strSAPDeliveryOrderData = string.Empty;
            string strSAPPurchaseOrderData = string.Empty;
            string strSAPSalesReturnData = string.Empty;
            string strEmailConfigMaster = string.Empty;
            string strGroupRights = string.Empty;
            string strUserMaster = string.Empty;
            string strVendorMaster = string.Empty;
            string strWarehouseMaster = string.Empty;
            string strPlantMaster = string.Empty;

            string ResponseString = "";
            StringBuilder stb = new StringBuilder();
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                DataSet dt;
                SaveSapData obj = new SaveSapData();
                dt = obj.Get_Data();
                DataTable dtMaterialMaster = dt.Tables[0].Copy();
                DataTable dtSAPDeliveryOrderData = dt.Tables[1].Copy();
                DataTable dtSAPPurchaseOrderData = dt.Tables[2].Copy();
                DataTable dtSAPSalesReturnData = dt.Tables[3].Copy();
                DataTable dtEmailConfigMaster = dt.Tables[4].Copy();
                DataTable dtGroupRights = dt.Tables[5].Copy();
                DataTable dtUserMaster = dt.Tables[6].Copy();
                DataTable dtVendorMaster = dt.Tables[7].Copy();
                DataTable dtWarehouseMaster = dt.Tables[8].Copy();
                DataTable dtPlantMaster = dt.Tables[9].Copy();

                dtMaterialMaster.TableName = "mMaterialMaster";
                dtSAPDeliveryOrderData.TableName = "tSAPDeliveryOrderData";
                dtSAPPurchaseOrderData.TableName = "tSAPPurchaseOrderData";
                dtSAPSalesReturnData.TableName = "tSAPSalesReturnData";
                dtEmailConfigMaster.TableName = "mEmailConfigMaster";
                dtGroupRights.TableName = "mGroupRights";
                dtUserMaster.TableName = "mUserMaster";
                dtVendorMaster.TableName = "mVendorMaster";
                dtWarehouseMaster.TableName = "mWarehouseMaster";
                dtPlantMaster.TableName = "mPlantMaster";

                if (dtMaterialMaster.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtMaterialMaster.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strMaterialMaster = xmlStr;

                }
                if (dtSAPDeliveryOrderData.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtSAPDeliveryOrderData.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strSAPDeliveryOrderData = xmlStr;
                }
                if (dtSAPPurchaseOrderData.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtSAPPurchaseOrderData.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strSAPPurchaseOrderData = xmlStr;
                }
                if (dtSAPSalesReturnData.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtSAPSalesReturnData.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strSAPSalesReturnData = xmlStr;
                }
                if (dtEmailConfigMaster.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtEmailConfigMaster.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strEmailConfigMaster = xmlStr;
                }
                if (dtGroupRights.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtGroupRights.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strGroupRights = xmlStr;
                }
                if (dtUserMaster.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtUserMaster.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strUserMaster = xmlStr;
                }
                if (dtVendorMaster.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtVendorMaster.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strVendorMaster = xmlStr;
                }
                if (dtWarehouseMaster.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtWarehouseMaster.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strWarehouseMaster = xmlStr;
                }
                if (dtPlantMaster.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtPlantMaster.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strPlantMaster = xmlStr;
                }

                if ((strMaterialMaster.Length > 0 || strSAPDeliveryOrderData.Length > 0 || strSAPPurchaseOrderData.Length > 0 || strSAPSalesReturnData.Length > 0 || strEmailConfigMaster.Length > 0)
                  || (strGroupRights.Length > 0 || strUserMaster.Length > 0 || strVendorMaster.Length > 0 || strWarehouseMaster.Length > 0 || strPlantMaster.Length > 0))
                {
                    value = strMaterialMaster + Environment.NewLine + strSAPDeliveryOrderData + Environment.NewLine + strSAPPurchaseOrderData + Environment.NewLine + strSAPSalesReturnData + Environment.NewLine + strEmailConfigMaster;
                    value = value + strGroupRights + Environment.NewLine + strUserMaster + Environment.NewLine + strVendorMaster + Environment.NewLine + strWarehouseMaster;
                    value = value + strPlantMaster;
                    stb.AppendLine("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:Greenply.in:India:Transactional:PP:CentralToLocal\">");
                    stb.AppendLine("<soapenv:Header/>");
                    stb.AppendLine("<soapenv:Body>");
                    stb.AppendLine("<urn:CentralToLocal>");
                    stb.AppendLine(value);
                    stb.AppendLine("<!--End:CentralToLocal Data:-->");
                    stb.AppendLine("</urn:CentralToLocal>");
                    stb.AppendLine("</soapenv:Body>");
                    stb.AppendLine("</soapenv:Envelope>");
                    //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, MethodBase.GetCurrentMethod().Name, "RestClient xml Send to Sap " + stb.ToString());
                    stb.ToString();
                }
            }
            catch (Exception ex)
            {
                //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, MethodBase.GetCurrentMethod().Name, "Rm Receiving Method Api Error: " + ex.Message.ToString().ToString());
                return new HttpResponseMessage()
                {
                    Content = new StringContent(ex.Message.ToString(), Encoding.UTF8, "application/xml")
                };

            }
            return new HttpResponseMessage()
            {

                Content = new StringContent(stb.ToString(), Encoding.UTF8, "application/xml")
            };
        }



    }
}
