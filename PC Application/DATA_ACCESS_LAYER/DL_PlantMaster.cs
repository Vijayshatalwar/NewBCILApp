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
    public class DL_PlantMaster : DlCommon
    {

        StringBuilder _sbQuery = new StringBuilder();
        DBManager dbManger = null;
        DlCommon dCommon = null;

        public DL_PlantMaster()
        {
            this.dCommon = new DlCommon();
            this.dbManger = DBProvider();
        }

        public ObservableCollection<PL_PlantMaster> DL_GetPlantMastersData(PL_PlantMaster objPLPlantMaster)
        {
            try
            {
                ObservableCollection<PL_PlantMaster> objPL_Plant_Master = new ObservableCollection<PL_PlantMaster>();
                this.dbManger.Open();
                this.dbManger.CreateParameters(1);
                this.dbManger.AddParameters(0, "@Type", "SELECT");
                IDataReader dataReader = dbManger.ExecuteReader(System.Data.CommandType.StoredProcedure, "USP_PlantMaster");
                while (dataReader.Read())
                {
                    objPL_Plant_Master.Add(new PL_PlantMaster
                    {
                        IsValid = Convert.ToBoolean(dataReader["IsValid"]),
                        PlantCode = Convert.ToString(dataReader["PlantCode"]),
                        PlantDesc = Convert.ToString(dataReader["PlantDesc"]),
                        StackPrintRequired = Convert.ToString(dataReader["StackPrintRequired"]),
                    });
                }
                return objPL_Plant_Master;
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

        public OperationResult DL_UpdatePlantData(PL_PlantMaster objPLPlantMaster)
        {
            OperationResult oPeration = OperationResult.UpdateError;
            DataTable DT = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(5);
                this.dbManger.AddParameters(0, "@Type", "UPDATE");
                this.dbManger.AddParameters(1, "@PlantCode", objPLPlantMaster.PlantCode);
                this.dbManger.AddParameters(2, "@PlantDesc", objPLPlantMaster.PlantDesc);
                this.dbManger.AddParameters(3, "@StackPrintRequired", objPLPlantMaster.StackPrintRequired);
                this.dbManger.AddParameters(4, "@CreatedBy", objPLPlantMaster.CreatedBy);
                int Result = dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_PlantMaster");
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

        public OperationResult DL_SavePlantData(PL_PlantMaster objPLPlantMaster)
        {
            OperationResult oPeration = OperationResult.SaveError;
            DataTable DT = new DataTable();
            try
            {
                if (!this.CheckDuplicate(objPLPlantMaster))
                {
                    this.dbManger.Open();
                    this.dbManger.CreateParameters(5);
                    this.dbManger.AddParameters(0, "@Type", "INSERT");
                    this.dbManger.AddParameters(1, "@PlantCode", objPLPlantMaster.PlantCode);
                    this.dbManger.AddParameters(2, "@PlantDesc", objPLPlantMaster.PlantDesc);
                    this.dbManger.AddParameters(3, "@StackPrintRequired", objPLPlantMaster.StackPrintRequired);
                    this.dbManger.AddParameters(4, "@CreatedBy", objPLPlantMaster.CreatedBy);
                    int Result = dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_PlantMaster");
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

        private bool CheckDuplicate(PL_PlantMaster objPLPlantMaster)
        {
            bool isDuplicate = false;
            DataTable dtDepotMaster = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(2);
                this.dbManger.AddParameters(0, "@Type", "CHECKDUP");
                this.dbManger.AddParameters(1, "@PlantCode", objPLPlantMaster.PlantCode);
                dtDepotMaster = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_PlantMaster").Tables[0];
                if (dtDepotMaster.Rows.Count > 0)
                {
                    isDuplicate = true;
                    VariableInfo.sbDuplicateCount.Append(Convert.ToString(objPLPlantMaster.PlantCode) + ",");
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

        public OperationResult DL_DeletePlantData(PL_PlantMaster objPLPlantMaster)
        {
            OperationResult oPeration = OperationResult.DeleteError;
            DataTable DT = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(2);
                this.dbManger.AddParameters(0, "@Type", "DELETE");
                this.dbManger.AddParameters(1, "@PlantCode", objPLPlantMaster.PlantCode);
                int Result = dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_PlantMaster");
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
