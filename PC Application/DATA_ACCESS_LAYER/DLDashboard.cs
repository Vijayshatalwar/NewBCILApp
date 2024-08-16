using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ENTITY_LAYER;
using System.Collections.ObjectModel;
using COMMON;
using COMMON_LAYER;

namespace DATA_ACCESS_LAYER
{
  public class DLDashboard : DlCommon
    {
        DBManager dbManger = null;
        DlCommon dCommon = null;
        DataTable dt = null;
        StringBuilder sb = null;

        public DLDashboard()
        {
            this.dCommon = new DlCommon();
            this.dbManger = DBProvider();
        }

        public DataTable DLPickingStatus()
        {
            dt = new DataTable();
            string strOutParm = string.Empty;
            try
            {
                //dbManger = new DBManager();    
                dbManger.Open();
                dbManger.CreateParameters(3);
                dbManger.AddParameters(0, "@Type", "PickingStatus");
                dbManger.AddParameters(1, "@PlantCode", VariableInfo.mPlantCode);
                dbManger.AddParameters(2, "@topitem", VariableInfo.topitem);
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_Dashboard").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLReceivingStatus()
        {
            dt = new DataTable();
            string strOutParm = string.Empty;
            try
            {
                //dbManger = new DBManager();
                dbManger.Open();
                dbManger.CreateParameters(3);
                dbManger.AddParameters(0, "@Type", "ReceivingStatus");
                dbManger.AddParameters(1, "@PlantCode", VariableInfo.mPlantCode);
                dbManger.AddParameters(2, "@topitem", VariableInfo.topitem);
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_Dashboard").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }
    }
}
