using GreenplyLocalToCentralWebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using GreenplyLocalToCentralWebApi.BI;
using System.Reflection;

namespace GreenplyLocalToCentralWebApi
{
    public class SaveSapData
    {
        DBManager oDbm;
        DCommon oDlcom = null;
        PCommon oPlcom = null;

        WriteLogFile ObjLog = new WriteLogFile();
        public SaveSapData()
        {
            DCommon _DCommon = new DCommon();
            oDbm = _DCommon.SqlDBProvider();
            oDlcom = new DCommon();
           // oPlcom = new PCommon();
        }
        
        public DataSet GetL2CMastersData()
        {
            DataSet Ds = new DataSet();
            try
            {
                oDbm.Open();
                oDbm.CreateParameters(1);
                oDbm.AddParameters(0, "@Type", "mLocalToCentralMasterData");

                Ds = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DataTransferCentralAndLocal");
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, MethodBase.GetCurrentMethod().Name, "RestClient xml Get Sap Posting Data  Error " + ex.Message.ToString());
            }
            finally
            {
                oDbm.Close();
            }
            return Ds;
        }

        public DataSet GetL2CTransactionalData()
        {
            DataSet Ds = new DataSet();
            try
            {
                oDbm.Open();
                ObjLog.WriteLog("Connection string - " + oDbm.ConnectionString.ToString());
                oDbm.CreateParameters(4);
                oDbm.AddParameters(0, "@Type", "mLocalToCentralTransactionalData");
                oDbm.AddParameters(1, "@ProductType", Properties.Settings.Default.PrintMaterialType1.ToString().Trim());
                oDbm.AddParameters(2, "@LocationType", Properties.Settings.Default.PrintingLocationType.ToString().Trim());
                //oDbm.AddParameters(3, "@POLocType", "H");
                oDbm.AddParameters(3, "@LocationCode", Properties.Settings.Default.LocationCode.ToString().Trim());
                Ds = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DataTransferCentralAndLocal");
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, MethodBase.GetCurrentMethod().Name, "RestClient xml Get Sap Posting Data  Error " + ex.Message.ToString());
            }
            finally
            {
                oDbm.Close();
            }
            return Ds;
        }

        public DataSet Get_Data()
        {
            DataSet DT = new DataSet();
            try
            {
                oDbm.Open();
                oDbm.CreateParameters(1);
                oDbm.AddParameters(0, "@Type", "mLocalToCentral");
                DT = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DataTransferCentralAndLocal");
            }
            catch (Exception ex)
            {
                VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, MethodBase.GetCurrentMethod().Name, "RestClient xml Get Sap Posting Data  Error " + ex.Message.ToString());
            }
            finally
            {
                oDbm.Close();
            }
            return DT;
        }
    }
}