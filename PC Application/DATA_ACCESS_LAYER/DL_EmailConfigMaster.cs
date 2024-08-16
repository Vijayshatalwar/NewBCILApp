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
    public class DL_EmailConfigMaster : DlCommon
    {
        StringBuilder _sbQuery = new StringBuilder();
        DBManager dbManger = null;
        DlCommon dCommon = null;

        public DL_EmailConfigMaster()
        {
            this.dCommon = new DlCommon();
            this.dbManger = DBProvider();
        }

        public ObservableCollection<PL_EmailConfigMaster> DL_GetEmailConfigMasterData(PL_EmailConfigMaster obj_EConfigMaster)
        {
            try
            {
                ObservableCollection<PL_EmailConfigMaster> objPL_EConfigMaster = new ObservableCollection<PL_EmailConfigMaster>();
                this.dbManger.Open();
                this.dbManger.CreateParameters(1);
                this.dbManger.AddParameters(0, "@Type", "SELECT");
                IDataReader dataReader = dbManger.ExecuteReader(System.Data.CommandType.StoredProcedure, "USP_EmailConfigMaster");
                while (dataReader.Read())
                {
                    objPL_EConfigMaster.Add(new PL_EmailConfigMaster
                    {
                        IsValid = Convert.ToBoolean(dataReader["IsValid"]),
                        LocationCode = Convert.ToString(dataReader["LocationCode"]),
                        SmtpHost = Convert.ToString(dataReader["SMTPHost"]),
                        EmailId = Convert.ToString(dataReader["EmailID"]),
                        Name = Convert.ToString(dataReader["Name"]),
                        Password = Convert.ToString(dataReader["Password"]),
                        PortNo = Convert.ToString(dataReader["PortNo"]),
                        Subject = Convert.ToString(dataReader["Subject"]),
                    });
                }
                return objPL_EConfigMaster;
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

        public OperationResult DL_UpdateEmailConfigMaster(PL_EmailConfigMaster obj_EConfigMaster)
        {
            OperationResult oPeration = OperationResult.UpdateError;
            DataTable DT = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(9);
                this.dbManger.AddParameters(0, "@Type", "UPDATE");
                this.dbManger.AddParameters(1, "@SMTPHost", obj_EConfigMaster.SmtpHost);
                this.dbManger.AddParameters(2, "@PortNo", obj_EConfigMaster.PortNo);
                this.dbManger.AddParameters(3, "@EmailID", obj_EConfigMaster.EmailId);
                this.dbManger.AddParameters(4, "@Name", obj_EConfigMaster.Name);
                this.dbManger.AddParameters(5, "@Password", obj_EConfigMaster.Password);
                this.dbManger.AddParameters(6, "@Subject", obj_EConfigMaster.Subject);
                this.dbManger.AddParameters(7, "@LocationCode", VariableInfo.mPlantCode);
                this.dbManger.AddParameters(8, "@CreatedBy", obj_EConfigMaster.CreatedBy);
                int Result = dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_EmailConfigMaster");
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

        public OperationResult DL_SaveEmailConfigMaster(PL_EmailConfigMaster obj_EConfigMaster)
        {
            OperationResult oPeration = OperationResult.SaveError;
            DataTable DT = new DataTable();
            try
            {
                if (!this.CheckDuplicate(obj_EConfigMaster))
                {
                    this.dbManger.Open();
                    this.dbManger.CreateParameters(9);
                    this.dbManger.AddParameters(0, "@Type", "INSERT");
                    this.dbManger.AddParameters(1, "@SMTPHost", obj_EConfigMaster.SmtpHost);
                    this.dbManger.AddParameters(2, "@PortNo", obj_EConfigMaster.PortNo);
                    this.dbManger.AddParameters(3, "@Name", obj_EConfigMaster.Name);
                    this.dbManger.AddParameters(4, "@EmailID", obj_EConfigMaster.EmailId);
                    this.dbManger.AddParameters(5, "@Password", obj_EConfigMaster.Password);
                    this.dbManger.AddParameters(6, "@Subject", obj_EConfigMaster.Subject);
                    this.dbManger.AddParameters(7, "@LocationCode", VariableInfo.mPlantCode);
                    this.dbManger.AddParameters(8, "@CreatedBy", obj_EConfigMaster.CreatedBy);
                    int Result = dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_EmailConfigMaster");
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

        private bool CheckDuplicate(PL_EmailConfigMaster obj_EConfigMaster)
        {
            bool isDuplicate = false;
            DataTable dtUserMaster = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(2);
                this.dbManger.AddParameters(0, "@Type", "CHECKDUP");
                this.dbManger.AddParameters(1, "@EmailID", obj_EConfigMaster.EmailId);
                dtUserMaster = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_EmailConfigMaster").Tables[0];
                if (dtUserMaster.Rows.Count > 0)
                {
                    isDuplicate = true;
                    VariableInfo.sbDuplicateCount.Append(Convert.ToString(obj_EConfigMaster.EmailId) + ",");
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

        public OperationResult DL_DeleteEmailConfigMaster(PL_EmailConfigMaster obj_EConfigMaster)
        {
            OperationResult oPeration = OperationResult.DeleteError;
            DataTable DT = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(2);
                this.dbManger.AddParameters(0, "@Type", "DELETE");
                this.dbManger.AddParameters(1, "@EmailID", obj_EConfigMaster.EmailId);
                int Result = dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_EmailConfigMaster");
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
