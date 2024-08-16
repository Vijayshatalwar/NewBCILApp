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
using GreenplyLocalToCentralWebApi.BI;

namespace GreenplyLocalToCentralWebApi.Controllers
{
    [AuthenticationFilter]
    //[RoutePrefix("api/Greenply")]
    [RoutePrefix("api/GreenplyLocalToCentral")]
    public class GreenplyController : ApiController
    {
        WriteLogFile ObjLog = new WriteLogFile();
        public void POST()
        {

        }

        [HttpGet]
        [Route("GETL2CMASTERSDATA")]
        public HttpResponseMessage GetL2CMastersData(HttpRequestMessage request)
        {
            string value = string.Empty;
            string strUserMaster = string.Empty;
            DataTable dtPostUserMaster = new DataTable();
            string ResponseString = "";
            StringBuilder stb = new StringBuilder();
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                DataSet dsMData;
                SaveSapData obj = new SaveSapData();
                dsMData = obj.GetL2CMastersData();
                if (dsMData.Tables[0].Rows.Count > 0)
                {
                    dtPostUserMaster = dsMData.Tables[0].Copy();
                }
                dtPostUserMaster.TableName = "mUserMaster";
                if (dtPostUserMaster.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtPostUserMaster.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strUserMaster = xmlStr;
                }

                if (strUserMaster.Length > 0)
                {
                    value = strUserMaster;
                    stb.AppendLine("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:Greenply.in:India:Transactional:PP:LocalToCentral\">");
                    stb.AppendLine("<soapenv:Header/>");
                    stb.AppendLine("<soapenv:Body>");
                    stb.AppendLine("<urn:LocalToCentral>");
                    stb.AppendLine(value);
                    stb.AppendLine("<!--End:LocalToCentral Data:-->");
                    stb.AppendLine("</urn:LocalToCentral>");
                    stb.AppendLine("</soapenv:Body>");
                    stb.AppendLine("</soapenv:Envelope>");
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, MethodBase.GetCurrentMethod().Name, "RestClient xml Send to Sap " + stb.ToString());
                    stb.ToString();
                }
            }
            catch (Exception ex)
            {
                ObjLog.WriteLog("Load LocLabelPrinting => Exception : " + ex.ToString());
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
        [Route("GETL2CTRANSACTIONALDATA")]
        public HttpResponseMessage GetL2CTransactionalData(HttpRequestMessage request)
        {
            string value = string.Empty;
            string strLocLabelPrinting = string.Empty;
            //string strHubLabelPrinting = string.Empty;
            //string strVendorLabelPrinting = string.Empty;
            string strVendorLabelGenerating = string.Empty;
            string strDispatchData = string.Empty;
            string strMatDamageData = string.Empty;
            string strDeliveryCancelledData = string.Empty;
            string strSalesReturn = string.Empty;
            string strPurchaseReturn = string.Empty;
            string strSAPDeliveryOrderData = string.Empty;
            string strSAPPurchaseOrderData = string.Empty;
            string strSAPReturnPOData = string.Empty;
            string strSAPSalesReturnData = string.Empty;
            string strItemSerial = string.Empty;
            //string strStockCount = string.Empty;

            DataTable dtPostLocLabelPrinting = new DataTable();
            //DataTable dtPostHubLabelPrinting = new DataTable();
            DataTable dtPostVendorLabelGenerating = new DataTable();
            //DataTable dtPostVendorLabelPrinting = new DataTable();
            DataTable dtPostDispatch = new DataTable();
            DataTable dtPostDeliveryCancelled = new DataTable();
            DataTable dtPostMatDamage = new DataTable();
            DataTable dtPostSalesReturn = new DataTable();
            DataTable dtPostPurchaseReturn = new DataTable();
            DataTable dtPostSAPDeliveryOrderData = new DataTable();
            DataTable dtPostSAPPurchaseOrderData = new DataTable();
            DataTable dtPostSAPSalesReturnData = new DataTable();
            DataTable dtPostReturnPOData = new DataTable();
            DataTable dtPostItemSerial = new DataTable();
            //DataTable dtPostStockCount = new DataTable();

            string ResponseString = "";
            StringBuilder stb = new StringBuilder();
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                DataSet dsTData;
                SaveSapData obj = new SaveSapData();
                dsTData = obj.GetL2CTransactionalData();

                if (Properties.Settings.Default.PrintingLocationType == "HUB")
                {
                    if (dsTData.Tables["tLocationLabelPrinting"].Rows.Count > 0)   //.TableName == "dtPostLocLabelPrinting"
                    {
                        dtPostLocLabelPrinting = dsTData.Tables["tLocationLabelPrinting"].Copy();
                        ObjLog.WriteLog("LocalToCentral : " + "LocationLabelPriniting => " + dsTData.Tables["tLocationLabelPrinting"].Rows.Count.ToString() + " Nos. of records found");
                    }
                    if (dsTData.Tables["tVendorLabelGenerating"].Rows.Count > 0)   //.TableName == "tItemSerial"
                    {
                        dtPostVendorLabelGenerating = dsTData.Tables["tVendorLabelGenerating"].Copy();
                        ObjLog.WriteLog("LocalToCentral : " + "VendorLabelGenerating => " + dsTData.Tables["tVendorLabelGenerating"].Rows.Count.ToString() + " Nos. of records found");
                    }
                    if (dsTData.Tables["tDispatchData"].Rows.Count > 0)   //.TableName == "tItemSerial"
                    {
                        dtPostDispatch = dsTData.Tables["tDispatchData"].Copy();
                        ObjLog.WriteLog("LocalToCentral : " + "DispatchData => " + dsTData.Tables["tDispatchData"].Rows.Count.ToString() + " Nos. of records found");
                    }
                    if (dsTData.Tables["tDeliveryCancellationData"].Rows.Count > 0)   //.TableName == "tItemSerial"
                    {
                        dtPostDeliveryCancelled = dsTData.Tables["tDeliveryCancellationData"].Copy();
                        ObjLog.WriteLog("LocalToCentral : " + "DeliveryCancellationData => " + dsTData.Tables["tDeliveryCancellationData"].Rows.Count.ToString() + " Nos. of records found");
                    }
                    if (dsTData.Tables["tMatDamageData"].Rows.Count > 0)   //.TableName == "tItemSerial"
                    {
                        dtPostMatDamage = dsTData.Tables["tMatDamageData"].Copy();
                        ObjLog.WriteLog("LocalToCentral : " + "MatDamageData => " + dsTData.Tables["tMatDamageData"].Rows.Count.ToString() + " Nos. of records found");
                    }
                    if (dsTData.Tables["tSalesReturn"].Rows.Count > 0)   //.TableName == "tItemSerial"
                    {
                        dtPostSalesReturn = dsTData.Tables["tSalesReturn"].Copy();
                        ObjLog.WriteLog("LocalToCentral : " + "SalesReturn => " + dsTData.Tables["tSalesReturn"].Rows.Count.ToString() + " Nos. of records found");
                    }
                    if (dsTData.Tables["tPurchaseReturn"].Rows.Count > 0)   //.TableName == "tItemSerial"
                    {
                        dtPostPurchaseReturn = dsTData.Tables["tPurchaseReturn"].Copy();
                        ObjLog.WriteLog("LocalToCentral : " + "PurchaseReturn => " + dsTData.Tables["tPurchaseReturn"].Rows.Count.ToString() + " Nos. of records found");
                    }
                    if (dsTData.Tables["tItemSerial"].Rows.Count > 0)   //.TableName == "tItemSerial"
                    {
                        dtPostItemSerial = dsTData.Tables["tItemSerial"].Copy();
                        ObjLog.WriteLog("LocalToCentral : " + "ItemSerial => " + dsTData.Tables["tItemSerial"].Rows.Count.ToString() + " Nos. of records found");
                    }
                    if (dsTData.Tables["tSAPPurchaseOrderData"].Rows.Count > 0)   //.TableName == "tItemSerial"
                    {
                        dtPostSAPPurchaseOrderData = dsTData.Tables["tSAPPurchaseOrderData"].Copy();
                        ObjLog.WriteLog("LocalToCentral : " + "SAPPurchaseOrderData => " + dsTData.Tables["tSAPPurchaseOrderData"].Rows.Count.ToString() + " Nos. of records found");
                    }
                    if (dsTData.Tables["tSAPDeliveryOrderData"].Rows.Count > 0)   //.TableName == "tItemSerial"
                    {
                        dtPostSAPDeliveryOrderData = dsTData.Tables["tSAPDeliveryOrderData"].Copy();
                        ObjLog.WriteLog("LocalToCentral : " + "VendorLabelGenerating => " + dsTData.Tables["tVendorLabelGenerating"].Rows.Count.ToString() + " Nos. of records found");
                    }
                    if (dsTData.Tables["tSAPSalesReturnData"].Rows.Count > 0)   //.TableName == "tItemSerial"
                    {
                        dtPostSAPSalesReturnData = dsTData.Tables["tSAPSalesReturnData"].Copy();
                        ObjLog.WriteLog("LocalToCentral : " + "SAPSalesReturnData => " + dsTData.Tables["tSAPSalesReturnData"].Rows.Count.ToString() + " Nos. of records found");
                    }
                    if (dsTData.Tables["tSAPPurchaseReturnData"].Rows.Count > 0)   //.TableName == "tItemSerial"
                    {
                        dtPostReturnPOData = dsTData.Tables["tSAPPurchaseReturnData"].Copy();
                        ObjLog.WriteLog("LocalToCentral : " + "SAPPurchaseReturnData => " + dsTData.Tables["tSAPPurchaseReturnData"].Rows.Count.ToString() + " Nos. of records found");
                    }

                    dtPostLocLabelPrinting.TableName = "tLocationLabelPrinting";
                    dtPostVendorLabelGenerating.TableName = "tVendorLabelGenerating";
                    dtPostDispatch.TableName = "tDispatchData";
                    dtPostDeliveryCancelled.TableName = "tDeliveryCancellationData";
                    dtPostMatDamage.TableName = "tMatDamageData";
                    dtPostSalesReturn.TableName = "tSalesReturn";
                    dtPostPurchaseReturn.TableName = "tPurchaseReturn";
                    dtPostSAPDeliveryOrderData.TableName = "tSAPDeliveryOrderData";
                    dtPostSAPPurchaseOrderData.TableName = "tSAPPurchaseOrderData";
                    dtPostSAPSalesReturnData.TableName = "tSAPSalesReturnData";
                    dtPostReturnPOData.TableName = "tSAPPurchaseReturnData";
                    dtPostItemSerial.TableName = "tItemSerial";

                    if (dtPostLocLabelPrinting.Rows.Count > 0)
                    {
                        MemoryStream str = new MemoryStream();
                        dtPostLocLabelPrinting.WriteXml(str, true);
                        str.Seek(0, SeekOrigin.Begin);
                        StreamReader sr = new StreamReader(str);
                        string xmlStr = string.Empty;
                        xmlStr = sr.ReadToEnd();
                        xmlStr = xmlStr.Replace("<NewDataSet>", "");
                        xmlStr = xmlStr.Replace("</NewDataSet>", "");
                        strLocLabelPrinting = xmlStr;
                    }
                    if (dtPostVendorLabelGenerating.Rows.Count > 0)
                    {
                        MemoryStream str = new MemoryStream();
                        dtPostVendorLabelGenerating.WriteXml(str, true);
                        str.Seek(0, SeekOrigin.Begin);
                        StreamReader sr = new StreamReader(str);
                        string xmlStr = string.Empty;
                        xmlStr = sr.ReadToEnd();
                        xmlStr = xmlStr.Replace("<NewDataSet>", "");
                        xmlStr = xmlStr.Replace("</NewDataSet>", "");
                        strVendorLabelGenerating = xmlStr;
                    }
                    if (dtPostDispatch.Rows.Count > 0)
                    {
                        MemoryStream str = new MemoryStream();
                        dtPostDispatch.WriteXml(str, true);
                        str.Seek(0, SeekOrigin.Begin);
                        StreamReader sr = new StreamReader(str);
                        string xmlStr = string.Empty;
                        xmlStr = sr.ReadToEnd();
                        xmlStr = xmlStr.Replace("<NewDataSet>", "");
                        xmlStr = xmlStr.Replace("</NewDataSet>", "");
                        strDispatchData = xmlStr;
                    }
                    if (dtPostDeliveryCancelled.Rows.Count > 0)
                    {
                        MemoryStream str = new MemoryStream();
                        dtPostDeliveryCancelled.WriteXml(str, true);
                        str.Seek(0, SeekOrigin.Begin);
                        StreamReader sr = new StreamReader(str);
                        string xmlStr = string.Empty;
                        xmlStr = sr.ReadToEnd();
                        xmlStr = xmlStr.Replace("<NewDataSet>", "");
                        xmlStr = xmlStr.Replace("</NewDataSet>", "");
                        strDeliveryCancelledData = xmlStr;
                    }
                    if (dtPostMatDamage.Rows.Count > 0)
                    {
                        MemoryStream str = new MemoryStream();
                        dtPostMatDamage.WriteXml(str, true);
                        str.Seek(0, SeekOrigin.Begin);
                        StreamReader sr = new StreamReader(str);
                        string xmlStr = string.Empty;
                        xmlStr = sr.ReadToEnd();
                        xmlStr = xmlStr.Replace("<NewDataSet>", "");
                        xmlStr = xmlStr.Replace("</NewDataSet>", "");
                        strMatDamageData = xmlStr;
                    }
                    if (dtPostSalesReturn.Rows.Count > 0)
                    {
                        MemoryStream str = new MemoryStream();
                        dtPostSalesReturn.WriteXml(str, true);
                        str.Seek(0, SeekOrigin.Begin);
                        StreamReader sr = new StreamReader(str);
                        string xmlStr = string.Empty;
                        xmlStr = sr.ReadToEnd();
                        xmlStr = xmlStr.Replace("<NewDataSet>", "");
                        xmlStr = xmlStr.Replace("</NewDataSet>", "");
                        strSalesReturn = xmlStr;
                    }
                    if (dtPostItemSerial.Rows.Count > 0)
                    {
                        MemoryStream str = new MemoryStream();
                        dtPostItemSerial.WriteXml(str, true);
                        str.Seek(0, SeekOrigin.Begin);
                        StreamReader sr = new StreamReader(str);
                        string xmlStr = string.Empty;
                        xmlStr = sr.ReadToEnd();
                        xmlStr = xmlStr.Replace("<NewDataSet>", "");
                        xmlStr = xmlStr.Replace("</NewDataSet>", "");
                        strItemSerial = xmlStr;
                    }
                    if (dtPostPurchaseReturn.Rows.Count > 0)
                    {
                        MemoryStream str = new MemoryStream();
                        dtPostPurchaseReturn.WriteXml(str, true);
                        str.Seek(0, SeekOrigin.Begin);
                        StreamReader sr = new StreamReader(str);
                        string xmlStr = string.Empty;
                        xmlStr = sr.ReadToEnd();
                        xmlStr = xmlStr.Replace("<NewDataSet>", "");
                        xmlStr = xmlStr.Replace("</NewDataSet>", "");
                        strPurchaseReturn = xmlStr;
                    }
                    if (dtPostSAPDeliveryOrderData.Rows.Count > 0)
                    {
                        MemoryStream str = new MemoryStream();
                        dtPostSAPDeliveryOrderData.WriteXml(str, true);
                        str.Seek(0, SeekOrigin.Begin);
                        StreamReader sr = new StreamReader(str);
                        string xmlStr = string.Empty;
                        xmlStr = sr.ReadToEnd();
                        xmlStr = xmlStr.Replace("<NewDataSet>", "");
                        xmlStr = xmlStr.Replace("</NewDataSet>", "");
                        strSAPDeliveryOrderData = xmlStr;
                    }
                    if (dtPostSAPPurchaseOrderData.Rows.Count > 0)
                    {
                        MemoryStream str = new MemoryStream();
                        dtPostSAPPurchaseOrderData.WriteXml(str, true);
                        str.Seek(0, SeekOrigin.Begin);
                        StreamReader sr = new StreamReader(str);
                        string xmlStr = string.Empty;
                        xmlStr = sr.ReadToEnd();
                        xmlStr = xmlStr.Replace("<NewDataSet>", "");
                        xmlStr = xmlStr.Replace("</NewDataSet>", "");
                        strSAPPurchaseOrderData = xmlStr;
                    }
                    if (dtPostSAPSalesReturnData.Rows.Count > 0)
                    {
                        MemoryStream str = new MemoryStream();
                        dtPostSAPSalesReturnData.WriteXml(str, true);
                        str.Seek(0, SeekOrigin.Begin);
                        StreamReader sr = new StreamReader(str);
                        string xmlStr = string.Empty;
                        xmlStr = sr.ReadToEnd();
                        xmlStr = xmlStr.Replace("<NewDataSet>", "");
                        xmlStr = xmlStr.Replace("</NewDataSet>", "");
                        strSAPSalesReturnData = xmlStr;
                    }
                    if (dtPostReturnPOData.Rows.Count > 0)
                    {
                        MemoryStream str = new MemoryStream();
                        dtPostReturnPOData.WriteXml(str, true);
                        str.Seek(0, SeekOrigin.Begin);
                        StreamReader sr = new StreamReader(str);
                        string xmlStr = string.Empty;
                        xmlStr = sr.ReadToEnd();
                        xmlStr = xmlStr.Replace("<NewDataSet>", "");
                        xmlStr = xmlStr.Replace("</NewDataSet>", "");
                        strSAPReturnPOData = xmlStr;
                    }
                    if ((strLocLabelPrinting.Length > 0 || strVendorLabelGenerating.Length > 0 || strDispatchData.Length > 0)
                        || (strMatDamageData.Length > 0 || strDeliveryCancelledData.Length > 0 || strSalesReturn.Length > 0 || strPurchaseReturn.Length > 0 
                        || strSAPDeliveryOrderData.Length > 0
                        || strSAPPurchaseOrderData.Length > 0 || strSAPReturnPOData.Length > 0 || strSAPSalesReturnData.Length > 0 || strItemSerial.Length > 0))
                    {
                        value = strLocLabelPrinting + Environment.NewLine + strVendorLabelGenerating + Environment.NewLine;
                        value = value + strDispatchData + Environment.NewLine + strMatDamageData + Environment.NewLine + strDeliveryCancelledData + Environment.NewLine;
                        value = value + strSalesReturn + Environment.NewLine + strPurchaseReturn + Environment.NewLine + strSAPDeliveryOrderData + Environment.NewLine;
                        value = value + strSAPPurchaseOrderData + Environment.NewLine + strSAPReturnPOData + Environment.NewLine + strSAPSalesReturnData + Environment.NewLine;
                        value = value + strItemSerial + Environment.NewLine;
                        stb.AppendLine("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:Greenply.in:India:Transactional:PP:LocalToCentral\">");
                        stb.AppendLine("<soapenv:Header/>");
                        stb.AppendLine("<soapenv:Body>");
                        stb.AppendLine("<urn:LocalToCentral>");
                        stb.AppendLine(value);
                        stb.AppendLine("<!--End:LocalToCentral Data:-->");
                        stb.AppendLine("</urn:LocalToCentral>");
                        stb.AppendLine("</soapenv:Body>");
                        stb.AppendLine("</soapenv:Envelope>");
                        //ObjLog.WriteLog("LocalToCentral ==> " + stb.ToString() + " at " + DateTime.Now.ToString());
                        //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, MethodBase.GetCurrentMethod().Name, "RestClient xml Send to Sap " + stb.ToString());
                        stb.ToString();
                    }

                }
                //ObjLog.WriteLog("Load LocationLabelPrinting  ==> " + dsTData.Tables["tLocationLabelPrinting"].TableName.ToString());
                ////ObjLog.WriteLog("Load HubLabelPrinting ==> " + dsTData.Tables[1].TableName.ToString() + " at " + DateTime.Now.ToString());
                //ObjLog.WriteLog("Load VendorLabelGenerating ==> " + dsTData.Tables["tVendorLabelGenerating"].TableName.ToString() );
                ////ObjLog.WriteLog("Load VendorLabelPrinting ==> " + dsTData.Tables[3].TableName.ToString() + " at " + DateTime.Now.ToString());
                //ObjLog.WriteLog("Load DispatchData ==> " + dsTData.Tables["tDispatchData"].TableName.ToString());
                //ObjLog.WriteLog("Load MatDamageData ==> " + dsTData.Tables["tMatDamageData"].TableName.ToString());
                //ObjLog.WriteLog("Load DeliveryCancelledData ==> " + dsTData.Tables["tDeliveryCancellationData"].TableName.ToString());
                //ObjLog.WriteLog("Load SalesReturn ==> " + dsTData.Tables["tSalesReturn"].TableName.ToString());
                //ObjLog.WriteLog("Load PurchaseReturn ==> " + dsTData.Tables["tPurchaseReturn"].TableName.ToString());
                //ObjLog.WriteLog("Load SAPDeliveryOrderData ==> " + dsTData.Tables["tSAPDeliveryOrderData"].TableName.ToString());
                //ObjLog.WriteLog("Load SAPPurchaseOrderData ==> " + dsTData.Tables["tSAPPurchaseOrderData"].TableName.ToString());
                //ObjLog.WriteLog("Load SAPPurchaseReturnOrderData ==> " + dsTData.Tables["tSAPPurchaseReturnData"].TableName.ToString());
                //ObjLog.WriteLog("Load SAPSalesReturnData ==> " + dsTData.Tables["tSAPSalesReturnData"].TableName.ToString());
                //ObjLog.WriteLog("Load ItemSerial ==> " + dsTData.Tables["tItemSerial"].TableName.ToString());
                ////ObjLog.WriteLog("Load StockCount ==> " + dsTData.Tables[14].TableName.ToString() + " at " + DateTime.Now.ToString());
                ////ds.Tables[0].Contains("tLocationLabelPrinting")

                //if (dsTData.Tables[1].Rows.Count > 0)   //.TableName == "dtPostHubLabelPrinting"
                //{
                //    dtPostHubLabelPrinting = dsTData.Tables[1].Copy();
                //    ObjLog.WriteLog("Load Material ==> " + dsTData.Tables[1].TableName.ToString() + " at " + DateTime.Now.ToString());
                //}

                //if (dsTData.Tables[3].Rows.Count > 0)   //.TableName == "tItemSerial"
                //{
                //    dtPostVendorLabelPrinting = dsTData.Tables[3].Copy();
                //}








                //if (dsTData.Tables[14].Rows.Count > 0)   //.TableName == "tItemSerial"
                //{
                //    dtPostStockCount = dsTData.Tables[14].Copy();
                //}
                //dtPostLocLabelPrinting.TableName = "tLocationLabelPrinting";
                ////dtPostHubLabelPrinting.TableName = "tHubLabelPrinting";
                //dtPostVendorLabelGenerating.TableName = "tVendorLabelGenerating";
                ////dtPostVendorLabelPrinting.TableName = "tVendorLabelPrinting";
                //dtPostDispatch.TableName = "tDispatchData";
                //dtPostDeliveryCancelled.TableName = "tDeliveryCancellationData";
                //dtPostMatDamage.TableName = "tMatDamageData";
                //dtPostSalesReturn.TableName = "tSalesReturn";
                //dtPostPurchaseReturn.TableName = "tPurchaseReturn";
                //dtPostSAPDeliveryOrderData.TableName = "tSAPDeliveryOrderData";
                //dtPostSAPPurchaseOrderData.TableName = "tSAPPurchaseOrderData";
                //dtPostSAPSalesReturnData.TableName = "tSAPSalesReturnData";
                //dtPostReturnPOData.TableName = "tSAPPurchaseReturnData";
                //dtPostItemSerial.TableName = "tItemSerial";
                //dtPostStockCount.TableName = "tStockCount";


                //if (dtPostHubLabelPrinting.Rows.Count > 0)
                //{
                //    MemoryStream str = new MemoryStream();
                //    dtPostHubLabelPrinting.WriteXml(str, true);
                //    str.Seek(0, SeekOrigin.Begin);
                //    StreamReader sr = new StreamReader(str);
                //    string xmlStr = string.Empty;
                //    xmlStr = sr.ReadToEnd();
                //    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                //    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                //    strHubLabelPrinting = xmlStr;
                //}
                //if (dtPostVendorLabelPrinting.Rows.Count > 0)
                //{
                //    MemoryStream str = new MemoryStream();
                //    dtPostVendorLabelPrinting.WriteXml(str, true);
                //    str.Seek(0, SeekOrigin.Begin);
                //    StreamReader sr = new StreamReader(str);
                //    string xmlStr = string.Empty;
                //    xmlStr = sr.ReadToEnd();
                //    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                //    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                //    strVendorLabelPrinting = xmlStr;
                //}

                //if (dtPostStockCount.Rows.Count > 0)
                //{
                //    MemoryStream str = new MemoryStream();
                //    dtPostStockCount.WriteXml(str, true);
                //    str.Seek(0, SeekOrigin.Begin);
                //    StreamReader sr = new StreamReader(str);
                //    string xmlStr = string.Empty;
                //    xmlStr = sr.ReadToEnd();
                //    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                //    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                //    strStockCount = xmlStr;
                //}

            }
            catch (Exception ex)
            {
                ObjLog.WriteLog("LocalToCentral : " + "GetL2CTransactionalData => Exception : " + ex.ToString());
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
        [Route("GETDATA")]
        public HttpResponseMessage GetData(HttpRequestMessage request)
        {
            string value = string.Empty;
            string strItemSerial = string.Empty;
            string strLocLabelPrinting = string.Empty;
            string strLoadingOffloading = string.Empty;
            string strSalesReturn = string.Empty;
            string strUserMaster = string.Empty;
            string strStockCount = string.Empty;
            string strSAPDeliveryOrderData = string.Empty;
            string strSAPPurchaseOrderData = string.Empty;
            string strSAPSalesReturnData = string.Empty;
            string strVendorLabelPrinting = string.Empty;

            string ResponseString = "";
            StringBuilder stb = new StringBuilder();
            XmlDocument xmlDoc = new XmlDocument();
            try
            {
                DataSet dt;
                SaveSapData obj = new SaveSapData();
                dt = obj.Get_Data();
                DataTable dtPostItemSerial = dt.Tables[0].Copy();
                DataTable dtPostLocLabelPrinting = dt.Tables[1].Copy();
                DataTable dtPostLoadingOffloading = dt.Tables[2].Copy();
                DataTable dtPostSalesReturn = dt.Tables[3].Copy();
                DataTable dtPostUserMaster = dt.Tables[4].Copy();
                DataTable dtPostStockCount = dt.Tables[5].Copy();
                DataTable dtPostSAPDeliveryOrderData = dt.Tables[6].Copy();
                DataTable dtPostSAPPurchaseOrderData = dt.Tables[7].Copy();
                DataTable dtPostSAPSalesReturnData = dt.Tables[8].Copy();
                DataTable dtPostVendorLabelPrinting = dt.Tables[9].Copy();

                dtPostItemSerial.TableName = "tItemSerial";
                dtPostLocLabelPrinting.TableName = "tLocationLabelPrinting";
                dtPostLoadingOffloading.TableName = "tLoadingOffloading";
                dtPostSalesReturn.TableName = "tSalesReturn";
                dtPostUserMaster.TableName = "mUserMaster";
                dtPostStockCount.TableName = "tStockCount";
                dtPostSAPDeliveryOrderData.TableName = "tSAPDeliveryOrderData";
                dtPostSAPPurchaseOrderData.TableName = "tSAPPurchaseOrderData";
                dtPostSAPSalesReturnData.TableName = "tSAPSalesReturnData";
                dtPostVendorLabelPrinting.TableName = "tVendorLabelPrinting";

                if (dtPostItemSerial.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtPostItemSerial.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strItemSerial = xmlStr;

                }
                if (dtPostLocLabelPrinting.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtPostLocLabelPrinting.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strLocLabelPrinting = xmlStr;
                }
                if (dtPostLoadingOffloading.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtPostLoadingOffloading.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strLoadingOffloading = xmlStr;
                }
                if (dtPostSalesReturn.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtPostSalesReturn.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strSalesReturn = xmlStr;
                }
                if (dtPostUserMaster.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtPostUserMaster.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strUserMaster = xmlStr;
                }
                if (dtPostStockCount.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtPostStockCount.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strStockCount = xmlStr;
                }
                if (dtPostSAPDeliveryOrderData.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtPostSAPDeliveryOrderData.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strSAPDeliveryOrderData = xmlStr;
                }
                if (dtPostSAPPurchaseOrderData.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtPostSAPPurchaseOrderData.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strSAPPurchaseOrderData = xmlStr;
                }
                if (dtPostSAPSalesReturnData.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtPostSAPSalesReturnData.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strSAPSalesReturnData = xmlStr;
                }
                if (dtPostVendorLabelPrinting.Rows.Count > 0)
                {
                    MemoryStream str = new MemoryStream();
                    dtPostVendorLabelPrinting.WriteXml(str, true);
                    str.Seek(0, SeekOrigin.Begin);
                    StreamReader sr = new StreamReader(str);
                    string xmlStr = string.Empty;
                    xmlStr = sr.ReadToEnd();
                    xmlStr = xmlStr.Replace("<NewDataSet>", "");
                    xmlStr = xmlStr.Replace("</NewDataSet>", "");
                    strVendorLabelPrinting = xmlStr;
                }

                if ((strItemSerial.Length > 0 || strLocLabelPrinting.Length > 0 || strLoadingOffloading.Length > 0 || strSalesReturn.Length > 0 || strUserMaster.Length > 0)
                  || (strStockCount.Length > 0 || strSAPDeliveryOrderData.Length > 0 || strSAPPurchaseOrderData.Length > 0 || strSAPSalesReturnData.Length > 0 || strVendorLabelPrinting.Length > 0))
                {
                    value = strItemSerial + Environment.NewLine + strLocLabelPrinting + Environment.NewLine + strLoadingOffloading + Environment.NewLine + strSalesReturn + Environment.NewLine + strUserMaster;
                    value = value + strStockCount + Environment.NewLine + strSAPDeliveryOrderData + Environment.NewLine + strSAPPurchaseOrderData + Environment.NewLine + strSAPSalesReturnData;
                    value = value + strVendorLabelPrinting;
                    stb.AppendLine("<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:urn=\"urn:Greenply.in:India:Transactional:PP:LocalToCentral\">");
                    stb.AppendLine("<soapenv:Header/>");
                    stb.AppendLine("<soapenv:Body>");
                    stb.AppendLine("<urn:LocalToCentral>");
                    stb.AppendLine(value);
                    stb.AppendLine("<!--End:LocalToCentral Data:-->");
                    stb.AppendLine("</urn:LocalToCentral>");
                    stb.AppendLine("</soapenv:Body>");
                    stb.AppendLine("</soapenv:Envelope>");
                    VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, MethodBase.GetCurrentMethod().Name, "RestClient xml Send to Sap " + stb.ToString());
                    stb.ToString();
                }
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, MethodBase.GetCurrentMethod().Name, "Rm Receiving Method Api Error: " + ex.Message.ToString().ToString());
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
