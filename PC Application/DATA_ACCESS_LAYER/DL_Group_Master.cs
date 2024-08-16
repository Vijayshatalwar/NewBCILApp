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
    public class DL_Group_Master : DlCommon
    {
        StringBuilder _sbQuery = new StringBuilder();
        DBManager dbManger = null;
        DlCommon dCommon = null;

        public DL_Group_Master()
        {
            this.dCommon = new DlCommon();
            this.dbManger = DBProvider();
        }
        public OperationResult Save(PL_Group_Master _objPL_Group_Master)
        {
            OperationResult oPeration = OperationResult.SaveError;
            DataTable DT = new DataTable();
            try
            {
                if (!CheckDuplicate(_objPL_Group_Master))
                {
                    this.dbManger.Open();
                    this.dbManger.CreateParameters(5);
                    this.dbManger.AddParameters(0, "@Type", "INSERT");
                    this.dbManger.AddParameters(1, "@GroupID", _objPL_Group_Master.GroupID);
                    this.dbManger.AddParameters(2, "@GroupName", _objPL_Group_Master.GroupName);
                    this.dbManger.AddParameters(3, "@GroupDesc", _objPL_Group_Master.GroupDesc);
                    this.dbManger.AddParameters(4, "@CreatedBy", _objPL_Group_Master.CreatedBy);
                    int result = this.dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "[USP_MGroups]");
                    if (result > 0)
                    {
                        oPeration = OperationResult.SaveSuccess;
                        VariableInfo.sbSaveCount.Append(Convert.ToString(_objPL_Group_Master.GroupID) + ",");
                    }
                    //}
                    else
                    {
                        oPeration = OperationResult.SaveSuccess;
                    }
                }
                else
                {
                    oPeration = OperationResult.Duplicate;
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
            return oPeration;
        }

        public ObservableCollection<PL_Group_Master> DL_GetGroupMaster(PL_Group_Master _objPL_Group_Master)
        {
            try
            {
                ObservableCollection<PL_Group_Master> __objPL_Group_Master = new ObservableCollection<PL_Group_Master>();
                this.dbManger.Open();
                this.dbManger.CreateParameters(1);
                this.dbManger.AddParameters(0, "@Type", "SELECT");
                IDataReader dataReader = dbManger.ExecuteReader(System.Data.CommandType.StoredProcedure, "[USP_MGroups]");
                while (dataReader.Read())
                {
                    __objPL_Group_Master.Add(new PL_Group_Master
                    {
                        IsValid = Convert.ToBoolean(dataReader["IsValid"]),
                        GroupID = Convert.ToString(dataReader["GroupID"]),
                        GroupName = Convert.ToString(dataReader["GroupName"]),
                        GroupDesc = Convert.ToString(dataReader["GroupDesc"]),
                        CreatedOn = Convert.ToString(dataReader["CreatedOn"]),
                        CreatedBy = Convert.ToString(dataReader["CreatedBy"]),
                    });
                }
                return __objPL_Group_Master;
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

        public OperationResult Delete(PL_Group_Master _objPL_Group_Master)
        {
            OperationResult oPeration = OperationResult.DeleteError;
            DataTable DT = new DataTable();
            StringBuilder _sb = new StringBuilder();
            try
            {
                this.dbManger.Open();
                _sb.AppendLine(" select * from mUserMaster where USER_TYPE = '" + _objPL_Group_Master.GroupName + "'");
                DT = this.dbManger.ExecuteDataSet(System.Data.CommandType.Text, _sb.ToString()).Tables[0];
                if (DT.Rows.Count > 0)
                {
                    oPeration = OperationResult.Invalid;
                    return oPeration;
                }
                this.dbManger.Open();
                this.dbManger.CreateParameters(2);
                this.dbManger.AddParameters(0, "@Type", "DELETE");
                this.dbManger.AddParameters(1, "@GroupID", _objPL_Group_Master.GroupID);
                int result = this.dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "[USP_MGroups]");
                if (result > 0)
                {
                    oPeration = OperationResult.DeleteSuccess;
                }
            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("REFERENCE"))
                {
                    oPeration = OperationResult.DeleteRefference;
                }
                else
                {
                    this.dbManger.Close();
                    throw ex;
                }
            }
            finally
            {
                this.dbManger.Close();
            }
            return oPeration;
        }

        public OperationResult Update(PL_Group_Master _objPL_Group_Master)
        {
            OperationResult oPeration = OperationResult.UpdateError;
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(5);
                this.dbManger.AddParameters(0, "@Type", "UPDATE");
                this.dbManger.AddParameters(1, "@GroupID", _objPL_Group_Master.GroupID);
                this.dbManger.AddParameters(2, "@GroupName", _objPL_Group_Master.GroupName);
                this.dbManger.AddParameters(3, "@GroupDesc", _objPL_Group_Master.GroupDesc);
                this.dbManger.AddParameters(4, "@CreatedBy", _objPL_Group_Master.CreatedBy);
                // this.dbManger.AddParameters(4, "@CreatedOn", _objPL_Group_Master.CreatedOn);
                int result = this.dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "[USP_MGroups]");
                if (result > 0)
                {
                    oPeration = OperationResult.UpdateSuccess;
                }
                else
                {
                    oPeration = OperationResult.Duplicate;
                }
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return oPeration;
        }

        private bool CheckDuplicate(PL_Group_Master _objPL_UserMaster)
        {
            bool isDuplicate = false;
            DataTable dtGroupMaster = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(2);
                this.dbManger.AddParameters(0, "@Type", "CHECKDUP");
                this.dbManger.AddParameters(1, "@GroupName", _objPL_UserMaster.GroupName);
                dtGroupMaster = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "[USP_MGroups]").Tables[0];
                if (dtGroupMaster.Rows.Count > 0)
                {
                    isDuplicate = true;
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
    }
}
