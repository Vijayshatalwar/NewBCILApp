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
    public class DL_VendorMaster : DlCommon
    {
        StringBuilder _sbQuery = new StringBuilder();
        DBManager dbManger = null;
        DlCommon dCommon = null;

        public DL_VendorMaster()
        {
            this.dCommon = new DlCommon();
            this.dbManger = DBProvider();
        }

        public ObservableCollection<PL_VendorMaster> DL_GetVendorMasterData(PL_VendorMaster objPL_VendorMaster)
        {
            try
            {
                ObservableCollection<PL_VendorMaster> objPL_Vendor_Master = new ObservableCollection<PL_VendorMaster>();
                this.dbManger.Open();
                this.dbManger.CreateParameters(1);
                this.dbManger.AddParameters(0, "@Type", "SELECT");
                IDataReader dataReader = dbManger.ExecuteReader(System.Data.CommandType.StoredProcedure, "[USP_VendorMaster]");
                while (dataReader.Read())
                {
                    objPL_Vendor_Master.Add(new PL_VendorMaster
                    {
                        IsValid = Convert.ToBoolean(dataReader["IsValid"]),
                        VendorId = Convert.ToString(dataReader["VendorCode"]),
                        VendorDesc = Convert.ToString(dataReader["VendorDesc"]),
                        VendorEmail = Convert.ToString(dataReader["VendorEmail"]),
                        VendorAdd = Convert.ToString(dataReader["VendorAddress"]),
                    });
                }
                return objPL_Vendor_Master;
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

        public OperationResult DL_UpdateVendorData(PL_VendorMaster objPL_VendorMaster)
        {
            OperationResult oPeration = OperationResult.UpdateError;
            DataTable DT = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(7);
                this.dbManger.AddParameters(0, "@Type", "UPDATE");
                this.dbManger.AddParameters(1, "@VendorCode", objPL_VendorMaster.VendorId);
                this.dbManger.AddParameters(2, "@VendorDesc", objPL_VendorMaster.VendorDesc);
                this.dbManger.AddParameters(3, "@VendorAddress", objPL_VendorMaster.VendorAdd);
                this.dbManger.AddParameters(4, "@VendorPassword", objPL_VendorMaster.VendorPwd);
                this.dbManger.AddParameters(5, "@VendorEmail", objPL_VendorMaster.VendorEmail);
                this.dbManger.AddParameters(6, "@CreatedBy", objPL_VendorMaster.CreatedBy);
                int Result = dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_VendorMaster");
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

        public OperationResult DL_SaveVendorData(PL_VendorMaster objPL_VendorMaster)
        {
            OperationResult oPeration = OperationResult.SaveError;
            DataTable DT = new DataTable();
            try
            {
                if (!this.CheckDuplicate(objPL_VendorMaster))
                {
                    this.dbManger.Open();
                    this.dbManger.CreateParameters(7);
                    this.dbManger.AddParameters(0, "@Type", "INSERT");
                    this.dbManger.AddParameters(1, "@VendorCode", objPL_VendorMaster.VendorId);
                    this.dbManger.AddParameters(2, "@VendorDesc", objPL_VendorMaster.VendorDesc);
                    this.dbManger.AddParameters(3, "@VendorAddress", objPL_VendorMaster.VendorAdd);
                    this.dbManger.AddParameters(4, "@VendorPassword", objPL_VendorMaster.VendorPwd);
                    this.dbManger.AddParameters(5, "@CreatedBy", objPL_VendorMaster.CreatedBy);
                    this.dbManger.AddParameters(6, "@VendorEmail", objPL_VendorMaster.VendorEmail);
                    int Result = dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_VendorMaster");
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

        private bool CheckDuplicate(PL_VendorMaster objPL_VendorMaster)
        {
            bool isDuplicate = false;
            DataTable dtUserMaster = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(2);
                this.dbManger.AddParameters(0, "@Type", "CHECKDUP");
                this.dbManger.AddParameters(1, "@VendorCode", objPL_VendorMaster.VendorId);
                dtUserMaster = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_VendorMaster").Tables[0];
                if (dtUserMaster.Rows.Count > 0)
                {
                    isDuplicate = true;
                    VariableInfo.sbDuplicateCount.Append(Convert.ToString(objPL_VendorMaster.VendorId) + ",");
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

        public OperationResult DL_DeleteVendorData(PL_VendorMaster objPL_VendorMaster)
        {
            OperationResult oPeration = OperationResult.DeleteError;
            DataTable DT = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(2);
                this.dbManger.AddParameters(0, "@Type", "DELETE");
                this.dbManger.AddParameters(1, "@VendorCode", objPL_VendorMaster.VendorId);
                int Result = dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_VendorMaster");
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
