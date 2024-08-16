using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using COMMON;
using ENTITY_LAYER;
using COMMON_LAYER; 


namespace DATA_ACCESS_LAYER
{
    public class DL_WarehouseMaster : DlCommon
    {

        StringBuilder _sbQuery = new StringBuilder();
        DBManager dbManger = null;
        DlCommon dCommon = null;

        public DL_WarehouseMaster()
        {
            this.dCommon = new DlCommon();
            this.dbManger = DBProvider();
        }

        public ObservableCollection<PL_WarehouseMaster> DL_GetWarehouseMaster(PL_WarehouseMaster objPL_WHMaster)
        {
            try
            {
                ObservableCollection<PL_WarehouseMaster> objPL_WH_Master = new ObservableCollection<PL_WarehouseMaster>();
                this.dbManger.Open();
                this.dbManger.CreateParameters(1);
                this.dbManger.AddParameters(0, "@Type", "SELECT");
                IDataReader dataReader = dbManger.ExecuteReader(System.Data.CommandType.StoredProcedure, "USP_WarehouseMaster");
                while (dataReader.Read())
                {
                    objPL_WH_Master.Add(new PL_WarehouseMaster
                    {
                        IsValid = Convert.ToBoolean(dataReader["IsValid"]),
                        WarehouseId = Convert.ToString(dataReader["WHID"]),
                        WarehouseDesc = Convert.ToString(dataReader["WHDescription"]),
                        WarehouseAdd = Convert.ToString(dataReader["WHAddress"]),
                    });
                }
                return objPL_WH_Master;
            }
            catch (Exception ex)
            {
                this.dbManger.Close();
                throw ex;
            }
            finally
            {
                this.dbManger.Close();
            }
        }

        public OperationResult DL_UpdateWarehouseData(PL_WarehouseMaster objPL_WHMaster)
        {
            OperationResult oPeration = OperationResult.UpdateError;
            DataTable DT = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(5);
                this.dbManger.AddParameters(0, "@Type", "UPDATE");
                this.dbManger.AddParameters(1, "@WHId", objPL_WHMaster.WarehouseId);
                this.dbManger.AddParameters(2, "@WHDesc", objPL_WHMaster.WarehouseDesc);
                this.dbManger.AddParameters(3, "@WHAddress", objPL_WHMaster.WarehouseAdd);
                this.dbManger.AddParameters(4, "@CreatedBy", objPL_WHMaster.CreatedBy);
                int Result = dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_WarehouseMaster");
                if (Result > 0)
                {
                    oPeration = OperationResult.UpdateSuccess;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.dbManger.Close();
            }
            return oPeration;
        }

        public OperationResult DL_SaveWarehouseData(PL_WarehouseMaster objPL_WHMaster)
        {
            OperationResult oPeration = OperationResult.SaveError;
            DataTable DT = new DataTable();
            try
            {
                if (!this.CheckDuplicate(objPL_WHMaster))
                {
                    this.dbManger.Open();
                    this.dbManger.CreateParameters(6);
                    this.dbManger.AddParameters(0, "@Type", "INSERT");
                    this.dbManger.AddParameters(1, "@WHId", objPL_WHMaster.WarehouseId);
                    this.dbManger.AddParameters(2, "@WHDesc", objPL_WHMaster.WarehouseDesc);
                    this.dbManger.AddParameters(3, "@WHAddress", objPL_WHMaster.WarehouseAdd);
                    this.dbManger.AddParameters(4, "@PlantCode", VariableInfo.mPlantCode);
                    this.dbManger.AddParameters(5, "@CreatedBy", objPL_WHMaster.CreatedBy);
                    
                    int Result = dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_WarehouseMaster");
                    if (Result > 0)
                    { 
                        oPeration = OperationResult.SaveSuccess; 
                    }
                    else
                    { 
                        oPeration = OperationResult.SaveError; 
                    }
                }
                else
                { 
                    oPeration = OperationResult.Duplicate;
                }
            }
            catch (Exception ex)
            { 
                throw ex; 
            }
            finally
            { 
                this.dbManger.Close();
            }
            return oPeration;
        }

        private bool CheckDuplicate(PL_WarehouseMaster _objPL_WH_Master)
        {
            bool isDuplicate = false;
            DataTable dtUserMaster = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(2);
                this.dbManger.AddParameters(0, "@Type", "CHECKDUP");
                this.dbManger.AddParameters(1, "@WHId", _objPL_WH_Master.WarehouseId);
                dtUserMaster = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_WarehouseMaster").Tables[0];
                if (dtUserMaster.Rows.Count > 0)
                {
                    isDuplicate = true;
                    VariableInfo.sbDuplicateCount.Append(Convert.ToString(_objPL_WH_Master.WarehouseId) + ",");
                }
            }
            catch (Exception ex)
            {
                this.dbManger.Close();
                throw ex;
            }
            finally
            {
                this.dbManger.Close();
            }
            return isDuplicate;
        }

        public OperationResult DL_DeleteWarehouseData(PL_WarehouseMaster _objPL_WH_Master)
        {
            OperationResult oPeration = OperationResult.DeleteError;
            DataTable DT = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(2);
                this.dbManger.AddParameters(0, "@Type", "DELETE");
                this.dbManger.AddParameters(1, "@WHId", _objPL_WH_Master.WarehouseId);
                int Result = dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_WarehouseMaster");
                if (Result > 0)
                {
                    oPeration = OperationResult.DeleteSuccess;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.dbManger.Close();
            }
            return oPeration;
        }
    }
}
