using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ENTITY_LAYER;
using System.Data;
using COMMON;
using COMMON_LAYER;


namespace DATA_ACCESS_LAYER
{
    public class DL_ExistingStockPrint : DlCommon
    {
        DBManager dbManger = null;
        DlCommon dCommon = null;
        DataTable dt = null;
        StringBuilder sb = null;

        public DL_ExistingStockPrint()
        {
            this.dCommon = new DlCommon();
            this.dbManger = DBProvider();
        }

        public DataTable DLGetMatGroupDesc()
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(1);
                dbManger.AddParameters(0, "@Type", "GetPONumber");
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_M_TrandingDetails").Tables[0];
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

        public DataTable DLGetMatProduct()
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(2);
                dbManger.AddParameters(0, "@Type", "GETPRODUCT");
                dbManger.AddParameters(1, "@PlantCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
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

        public DataTable DLGetMatSize(string objProduct)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(3);
                dbManger.AddParameters(0, "@Type", "GETMATSIZE");
                dbManger.AddParameters(1, "@Product", objProduct);
                dbManger.AddParameters(1, "@PlantCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
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

        public DataTable DLGetMatThickness(string objProduct)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(3);
                dbManger.AddParameters(0, "@Type", "GETMATSIZE");
                dbManger.AddParameters(1, "@Product", objProduct);
                dbManger.AddParameters(1, "@PlantCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
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

        public DataTable DLGetMatGrade(string objProduct)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(3);
                dbManger.AddParameters(0, "@Type", "GETMATSIZE");
                dbManger.AddParameters(1, "@Product", objProduct);
                dbManger.AddParameters(1, "@PlantCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
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

        public DataTable DLGetMatCategory(string objProduct)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(3);
                dbManger.AddParameters(0, "@Type", "GETMATSIZE");
                dbManger.AddParameters(1, "@Product", objProduct);
                dbManger.AddParameters(1, "@PlantCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
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

        public DataTable DLGetMatData(string objProduct)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(3);
                dbManger.AddParameters(0, "@Type", "GETMATSIZE");
                dbManger.AddParameters(1, "@Product", objProduct);
                dbManger.AddParameters(1, "@PlantCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
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
