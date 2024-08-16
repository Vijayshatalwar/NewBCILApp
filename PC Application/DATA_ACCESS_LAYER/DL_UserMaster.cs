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
    public class DL_UserMaster : DlCommon
    {
        StringBuilder _sbQuery = new StringBuilder();
        DBManager dbManger = null;
        DlCommon dCommon = null;
        public DL_UserMaster()
        {
            this.dCommon = new DlCommon();
            this.dbManger = DBProvider();
        }

        public ObservableCollection<PL_UserMaster> DL_GetUserMasterData(PL_UserMaster _objPL_UserMaster)
        {
            try
            {
                ObservableCollection<PL_UserMaster> __objPL_UserMaster = new ObservableCollection<PL_UserMaster>();
                this.dbManger.Open();
                this.dbManger.CreateParameters(1);
                this.dbManger.AddParameters(0, "@Type", "SELECT");
                IDataReader dataReader = dbManger.ExecuteReader(System.Data.CommandType.StoredProcedure, "USP_UserMaster");
                while (dataReader.Read())
                {
                    __objPL_UserMaster.Add(new PL_UserMaster
                    {
                        IsValid = Convert.ToBoolean(dataReader["IsValid"]),
                        GroupName = Convert.ToString(dataReader["GroupName"]),
                        GroupID = Convert.ToString(dataReader["GroupID"]),
                        USER_TYPE = Convert.ToString(dataReader["USER_TYPE"]),
                        PlantCode = VariableInfo.mPlantCode,
                        USER_ID = Convert.ToString(dataReader["USER_ID"]),
                        USER_NAME = Convert.ToString(dataReader["USER_NAME"]),
                        USER_EMAIL = Convert.ToString(dataReader["USER_EMAIL"]),
                        LOCATION_TYPE = Convert.ToString(dataReader["LOCATION_TYPE"]),
                        LOCATION_CODE = Convert.ToString(dataReader["LOCATION_CODE"]),
                        //CREATED_ON = Convert.ToString(dataReader["CREATED_ON"]),
                        //CREATED_BY = Convert.ToString(dataReader["CREATED_BY"]),
                        //MODIFIED_ON = Convert.ToString(dataReader["MODIFIED_ON"]),
                        //MODIFIED_BY = Convert.ToString(dataReader["MODIFIED_BY"]),
                    });
                }
                return __objPL_UserMaster;
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

        public DataTable GetGroupRights(string _UserID)
        {
            DataTable DT = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(3);
                this.dbManger.AddParameters(0, "@Type", "GETAPPLICATIONUSERRIGHTS");
                this.dbManger.AddParameters(1, "@UserID", _UserID);
                this.dbManger.AddParameters(2, "@LocationCode", VariableInfo.mPlantCode.Trim().ToString());
                DT = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_UserMaster").Tables[0];

                //this.dbManger.Open();
                //StringBuilder sbQuery = new StringBuilder();
                //sbQuery.AppendLine("SELECT * FROM vw_UserGroupRights WHERE USER_ID = '" + _UserID.Trim() + "'");
                //DT = dbManger.ExecuteDataSet(CommandType.Text, sbQuery.ToString()).Tables[0];
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                this.dbManger.Close();
            }
            return DT;
        }

        public DataTable ValidateUser(PL_UserMaster objPL_UserMaster)
        {
            DataTable DT = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(4);
                this.dbManger.AddParameters(0, "@Type", "VALIDATEUSER");
                this.dbManger.AddParameters(1, "@UserID", objPL_UserMaster.USER_ID);
                this.dbManger.AddParameters(2, "@Password", objPL_UserMaster.Password);
                this.dbManger.AddParameters(3, "@LocationCode", objPL_UserMaster.LOCATION_CODE);
                DT = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_UserMaster").Tables[0];
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
            return DT;
        }

        public DataTable ValidateVendorUser(PL_UserMaster objPL_UserMaster)
        {
            DataTable DT = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(3);
                this.dbManger.AddParameters(0, "@Type", "VALIDATEVENDORUSER");
                this.dbManger.AddParameters(1, "@UserID", objPL_UserMaster.USER_ID);
                this.dbManger.AddParameters(2, "@Password", objPL_UserMaster.Password);
                DT = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_UserMaster").Tables[0];
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
            return DT;
        }

        public string GetPlantType()
        {
            string PlantType = "";
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(2);
                this.dbManger.AddParameters(0, "@Type", "GETPLANTTYPE");
                this.dbManger.AddParameters(1, "@PlantCode", VariableInfo.mPlantCode);
                PlantType = Convert.ToString(this.dbManger.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USP_UserMaster"));
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
            return PlantType;
        }

        public DataTable ShowDetails(PL_UserMaster objPL_UserMaster)
        {
            DataTable DT = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(12);
                this.dbManger.AddParameters(0, "@Type", "SELECT");
                this.dbManger.AddParameters(1, "@UserID", null);
                this.dbManger.AddParameters(2, "@GroupID", null);
                this.dbManger.AddParameters(4, "@PlantCode", null);
                this.dbManger.AddParameters(6, "@UserName", null);
                this.dbManger.AddParameters(7, "@Password", null);
                this.dbManger.AddParameters(8, "@EmailID", null);
                this.dbManger.AddParameters(9, "@CreatedBy", null);
                this.dbManger.AddParameters(10, "@ModifieddBy", null);
                this.dbManger.AddParameters(11, "@UserType", null);
                DT = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_UserMaster").Tables[0];
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                this.dbManger.Close();
            }
            return DT;
        }

        public DataTable ShowDetailsByID(PL_UserMaster objPL_UserMaster)
        {
            DataTable DT = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(12);
                this.dbManger.AddParameters(0, "@Type", "SELECTBYID");
                this.dbManger.AddParameters(1, "@UserID", objPL_UserMaster.USER_ID);
                this.dbManger.AddParameters(2, "@GroupID", null);
                this.dbManger.AddParameters(4, "@PlantCode", null);
                this.dbManger.AddParameters(6, "@UserName", null);
                this.dbManger.AddParameters(7, "@Password", null);
                this.dbManger.AddParameters(8, "@EmailID", null);
                this.dbManger.AddParameters(9, "@CreatedBy", null);
                this.dbManger.AddParameters(10, "@ModifieddBy", null);
                this.dbManger.AddParameters(11, "@UserType", null);
                DT = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_UserMaster").Tables[0];
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                this.dbManger.Close();
            }
            return DT;
        }

        public OperationResult Save(PL_UserMaster objPL_UserMaster)
        {
            OperationResult oPeration = OperationResult.SaveError; 
            DataTable DT = new DataTable();
            try
            {
                if (!this.CheckDuplicate(objPL_UserMaster))
                {
                    this.dbManger.Open(); 
                    this.dbManger.CreateParameters(10);
                    this.dbManger.AddParameters(0, "@Type", "INSERT");
                    this.dbManger.AddParameters(1, "@LocationType", objPL_UserMaster.LOCATION_TYPE);
                    this.dbManger.AddParameters(2, "@LocationCode", objPL_UserMaster.LOCATION_CODE);
                    this.dbManger.AddParameters(3, "@UserID", objPL_UserMaster.USER_ID);
                    this.dbManger.AddParameters(4, "@UserName", objPL_UserMaster.USER_NAME);
                    this.dbManger.AddParameters(5, "@Password", objPL_UserMaster.Password);
                    this.dbManger.AddParameters(6, "@EmailID", objPL_UserMaster.USER_EMAIL);
                    this.dbManger.AddParameters(7, "@GroupID", objPL_UserMaster.GroupName);
                    this.dbManger.AddParameters(8, "@UserType", objPL_UserMaster.GroupName);
                    this.dbManger.AddParameters(9, "@CreatedBy", objPL_UserMaster.CREATED_BY);
                    int Result = dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_UserMaster");
                    if (Result > 0)
                    { oPeration = OperationResult.SaveSuccess; }
                    else
                    { oPeration = OperationResult.SaveError; }
                }
                else
                { oPeration = OperationResult.Duplicate; }
            }
            catch (Exception ex)
            { throw ex; }
            finally
            { this.dbManger.Close(); }
            return oPeration;
        }

        private bool CheckDuplicate(PL_UserMaster _objPL_UserMaster)
        {
            bool isDuplicate = false;
            DataTable dtUserMaster = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(2);
                this.dbManger.AddParameters(0, "@Type", "CHECKDUP");
                this.dbManger.AddParameters(1, "@UserID", _objPL_UserMaster.USER_ID);
                dtUserMaster = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "[USP_UserMaster]").Tables[0];
                if (dtUserMaster.Rows.Count > 0)
                {
                    isDuplicate = true;
                    VariableInfo.sbDuplicateCount.Append(Convert.ToString(_objPL_UserMaster.USER_ID) + ",");
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

        private bool CheckGroupId(PL_UserMaster _objPL_UserMaster)
        {
            bool isDuplicate = false;
            try
            {
                this.dbManger.Open();
                StringBuilder _sb = new StringBuilder();
                _sb.AppendLine("select * from [dbo].[mGroupMaster] where GroupID='" + _objPL_UserMaster.GroupID + "'");
                DataSet result = this.dbManger.ExecuteDataSet(System.Data.CommandType.Text, _sb.ToString());
                if (result.Tables[0].Rows.Count > 0)
                {
                    isDuplicate = true;
                    isDuplicate = true;
                    VariableInfo.sbDuplicateCount.Append(Convert.ToString(_objPL_UserMaster.USER_ID) + ",");
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

        public OperationResult Update(PL_UserMaster objPL_UserMaster)
        {
            OperationResult oPeration = OperationResult.UpdateError;
            DataTable DT = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(10);
                this.dbManger.AddParameters(0, "@Type", "UPDATE");
                this.dbManger.AddParameters(1, "@LocationType", objPL_UserMaster.LOCATION_TYPE);
                this.dbManger.AddParameters(2, "@LocationCode", objPL_UserMaster.LOCATION_CODE);
                this.dbManger.AddParameters(3, "@UserName", objPL_UserMaster.USER_NAME);
                this.dbManger.AddParameters(4, "@Password", objPL_UserMaster.Password);
                this.dbManger.AddParameters(5, "@EmailID", objPL_UserMaster.USER_EMAIL);
                this.dbManger.AddParameters(6, "@GroupID", objPL_UserMaster.GroupName);
                this.dbManger.AddParameters(7, "@UserType", objPL_UserMaster.GroupName);
                this.dbManger.AddParameters(8, "@CreatedBy", objPL_UserMaster.CREATED_BY);
                this.dbManger.AddParameters(9, "@UserID", objPL_UserMaster.USER_ID);
                
                int Result = dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_UserMaster");
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

        public OperationResult Delete(PL_UserMaster objPL_UserMaster)
        {
            OperationResult oPeration = OperationResult.DeleteError;
            DataTable DT = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(2);
                this.dbManger.AddParameters(0, "@Type", "DELETE");
                this.dbManger.AddParameters(1, "@UserID", objPL_UserMaster.USER_ID);
                int Result = dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_UserMaster");
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

        public OperationResult UpdatePassword(PL_UserMaster objPL_UserMaster)
        {
            OperationResult oPeration = OperationResult.UpdateError;
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(4);
                this.dbManger.AddParameters(0, "@Type", "UPDATEPASSWORD");
                this.dbManger.AddParameters(1, "@UserID", objPL_UserMaster.USER_ID);
                this.dbManger.AddParameters(2, "@Password", objPL_UserMaster.Password);
                this.dbManger.AddParameters(3, "@NewPassword", objPL_UserMaster.NewPswd);
                int Result = dbManger.ExecuteNonQuery(CommandType.StoredProcedure, "USP_UserMaster");
                if (Result > 0)
                {
                    oPeration = OperationResult.UpdateSuccess;
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

        public DataSet GetDropDownData(PL_UserMaster obj, string sType)
        {
            DataSet dsdata = new DataSet();
            try
            {
                this.dbManger.Open();
                StringBuilder _sb = new StringBuilder();
                if (sType == "GROUPNAME")
                {
                    _sb.AppendLine(" SELECT GroupName, GroupID FROM [dbo].[mGroupMaster] ");
                }
                dsdata = this.dbManger.ExecuteDataSet(System.Data.CommandType.Text, _sb.ToString());
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
            return dsdata;
        }


        public DataSet GetLocationCodeData()
        {
            DataSet dsdata = new DataSet();
            try
            {
                this.dbManger.Open();
                StringBuilder _sb = new StringBuilder();
                _sb.AppendLine(" SELECT DISTINCT PlantCode, PlantDesc FROM [dbo].[mPlantMaster] ");
                dsdata = this.dbManger.ExecuteDataSet(System.Data.CommandType.Text, _sb.ToString());
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
            return dsdata;
        }
       

        public bool AppStartFirstTime()
        {
            try
            {
                _sbQuery.Length = 0;
                this.dbManger.Open();
                _sbQuery.Append("SELECT USER_ID FROM mUserMaster");
                IDataReader dataReader = this.dbManger.ExecuteReader(System.Data.CommandType.Text, _sbQuery.ToString());
                if (dataReader.Read())
                {
                    return false;
                }
                else
                {
                    return true;
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
        }

        //public DataTable GetPlantCode()
        //{

        //    DataTable dsdata = new DataTable();
        //    try
        //    {
        //        this.dbManger.Open();
        //        this.dbManger.CreateParameters(1);
        //        this.dbManger.AddParameters(0, "@Type", "GETPLANTCODE");
        //        dsdata = this.dbManger.ExecuteDataSet(CommandType.StoredProcedure, "USP_M_UserMaster").Tables[0];
        //    }
        //    catch (Exception ex)
        //    {
        //        this.dbManger.Close();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        this.dbManger.Close();
        //    }
        //    return dsdata;
        //}
        public string GetPlantCode()
        {
            string sPlantcode = "";
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(1);
                this.dbManger.AddParameters(0, "@Type", "GETPLANTCODE");
                sPlantcode = Convert.ToString(this.dbManger.ExecuteScalar(System.Data.CommandType.StoredProcedure, "[USP_UserMaster]"));
                sPlantcode = sPlantcode.Replace ("\r\n", "");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                this.dbManger.Close();
            }
            return sPlantcode;
        }


        public DataTable DLGetDBConnectionDetails()
        {
            DataTable DT = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(4);
                this.dbManger.AddParameters(0, "@Type", "GETDATABASEDETAILS");
                DT = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_UserMaster").Tables[0];
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
            return DT;
        }

    }
}
