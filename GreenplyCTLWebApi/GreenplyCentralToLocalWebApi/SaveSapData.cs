using GreenplyCentralToLocalWebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using GreenplyCentralToLocalWebApi.BI;
using System.Reflection;

namespace GreenplyCentralToLocalWebApi
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

        public DataSet GetC2LMastersData()
        {
            DataSet DT = new DataSet();
            try
            {
                oDbm.Open();
                oDbm.CreateParameters(1);
                oDbm.AddParameters(0, "@Type", "mCentralToLocalMastersData");
                //oDbm.AddParameters(0, "@ProductType", Properties.Settings.Default.LocalDatabaseName.ToString().Trim());
                //oDbm.AddParameters(0, "@LocationCode", Properties.Settings.Default.LocationCode.ToString().Trim());
                DT = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DataTransferCentralAndLocal");
            }
            catch (Exception ex)
            {
                //ObjLog.WriteLog("Data Scheduler : Error in getting Central Data : ERROR : " + ex.Message.ToString());
                //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, MethodBase.GetCurrentMethod().Name, "RestClient xml Get Sap Posting Data  Error " + ex.Message.ToString());
            }
            finally
            {
                oDbm.Close();
            }
            return DT;
        }

        public DataSet GetC2LTransactionalData(string LocationCode, string LocationType, string POLocType)
        {
            DataSet DT = new DataSet();
            try
            {
                oDbm.Open();
                oDbm.CreateParameters(4);
                oDbm.AddParameters(0, "@Type", "mCentralToLocalTransactionalData");
                oDbm.AddParameters(1, "@LocationCode", LocationCode.ToString().Trim());
                oDbm.AddParameters(2, "@LocationType", LocationType.ToString().Trim());
                oDbm.AddParameters(3, "@POLocType", POLocType.ToString().Trim());
                DT = oDbm.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DataTransferCentralAndLocal");
            }
            catch (Exception ex)
            {
                //ObjLog.WriteLog("Error in getting Central Transaction Data : ERROR : " + ex.Message.ToString());
                //VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, MethodBase.GetCurrentMethod().Name, "RestClient xml Get Sap Posting Data  Error " + ex.Message.ToString());
            }
            finally
            {
                oDbm.Close();
            }
            return DT;
        }

        public DataSet Get_Data()
        {
            DataSet DT = new DataSet();
            try
            {
                oDbm.Open();
                oDbm.CreateParameters(1);
                oDbm.AddParameters(0, "@Type", "mCentralToLocal");
                //oDbm.AddParameters(0, "@ProductType", Properties.Settings.Default.LocalDatabaseName.ToString().Trim());
                //oDbm.AddParameters(0, "@LocationCode", Properties.Settings.Default.LocationCode.ToString().Trim());
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