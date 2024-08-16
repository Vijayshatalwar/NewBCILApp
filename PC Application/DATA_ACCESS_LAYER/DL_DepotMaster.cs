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
    public class DL_DepotMaster : DlCommon
    {
        StringBuilder _sbQuery = new StringBuilder();
        DBManager dbManger = null;
        DlCommon dCommon = null;

        public DL_DepotMaster()
        {
            this.dCommon = new DlCommon();
            this.dbManger = DBProvider();
        }

        public ObservableCollection<PL_DepotMaster> DL_GetDepotMaster(PL_DepotMaster objPLDepotMaster)
        {
            try
            {
                ObservableCollection<PL_DepotMaster> objPL_Depot_Master = new ObservableCollection<PL_DepotMaster>();
                this.dbManger.Open();
                this.dbManger.CreateParameters(1);
                this.dbManger.AddParameters(0, "@Type", "SELECT");
                IDataReader dataReader = dbManger.ExecuteReader(System.Data.CommandType.StoredProcedure, "USP_DepotMaster");
                while (dataReader.Read())
                {
                    objPL_Depot_Master.Add(new PL_DepotMaster
                    {
                        IsValid = Convert.ToBoolean(dataReader["IsValid"]),
                        LocationCode = Convert.ToString(dataReader["LocationCode"]),
                        DepotId = Convert.ToString(dataReader["DepotID"]),
                        DepotDesc = Convert.ToString(dataReader["DepotDesc"]),
                    });
                }
                return objPL_Depot_Master;
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

        public OperationResult DL_UpdateDepotData(PL_DepotMaster objPLDepotMaster)
        {
            OperationResult oPeration = OperationResult.UpdateError;
            DataTable DT = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(5);
                this.dbManger.AddParameters(0, "@Type", "UPDATE");
                this.dbManger.AddParameters(1, "@DepotID", objPLDepotMaster.DepotId);
                this.dbManger.AddParameters(2, "@DepotDesc", objPLDepotMaster.DepotDesc);
                this.dbManger.AddParameters(3, "@LocationCode", VariableInfo.mPlantCode);
                this.dbManger.AddParameters(4, "@CreatedBy", objPLDepotMaster.CreatedBy);
                int Result = dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_DepotMaster");
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

        public OperationResult DL_SaveDepotData(PL_DepotMaster objDepotMaster)
        {
            OperationResult oPeration = OperationResult.SaveError;
            DataTable DT = new DataTable();
            try
            {
                if (!this.CheckDuplicate(objDepotMaster))
                {
                    this.dbManger.Open();
                    this.dbManger.CreateParameters(5);
                    this.dbManger.AddParameters(0, "@Type", "SAVE");
                    this.dbManger.AddParameters(1, "@DepotID", objDepotMaster.DepotId);
                    this.dbManger.AddParameters(2, "@DepotDesc", objDepotMaster.DepotDesc);
                    this.dbManger.AddParameters(3, "@LocationCode", VariableInfo.mPlantCode);
                    this.dbManger.AddParameters(4, "@CreatedBy", objDepotMaster.CreatedBy);
                    int Result = dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_DepotMaster");
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

        private bool CheckDuplicate(PL_DepotMaster objDepotMaster)
        {
            bool isDuplicate = false;
            DataTable dtDepotMaster = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(2);
                this.dbManger.AddParameters(0, "@Type", "CHECKDUP");
                this.dbManger.AddParameters(1, "@DepotID", objDepotMaster.DepotId);
                dtDepotMaster = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_DepotMaster").Tables[0];
                if (dtDepotMaster.Rows.Count > 0)
                {
                    isDuplicate = true;
                    VariableInfo.sbDuplicateCount.Append(Convert.ToString(objDepotMaster.DepotId) + ",");
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

        public OperationResult DL_DeleteDepotData(PL_DepotMaster objDepotMaster)
        {
            OperationResult oPeration = OperationResult.DeleteError;
            DataTable DT = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(2);
                this.dbManger.AddParameters(0, "@Type", "DELETE");
                this.dbManger.AddParameters(1, "@DepotID", objDepotMaster.DepotId);
                int Result = dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_DepotMaster");
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
