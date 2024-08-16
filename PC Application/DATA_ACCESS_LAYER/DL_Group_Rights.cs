using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel; 
using System.Data.SqlClient;
using System.Data;
using COMMON;
using ENTITY_LAYER; 

namespace DATA_ACCESS_LAYER
{
   public class DL_Group_Rights :DlCommon
    {

        StringBuilder _sbQuery = new StringBuilder();
        DBManager dbManger = null;
        DlCommon dCommon = null;
        public DL_Group_Rights()
        {
            this.dCommon = new DlCommon();
            this.dbManger = DBProvider();
        }
        public DataSet GetDropDownData(PL_Group_Master obj, string sType)
        {

            DataSet dsdata = new DataSet();
            StringBuilder _sb = new StringBuilder();
            try
            {

                this.dbManger.Open();
                if (sType == "GROUP")
                {
                    _sb.AppendLine(" SELECT DISTINCT GroupName,GroupID FROM [dbo].[mGroupMaster] ");
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

        public ObservableCollection<PL_Group_Master> DL_GetGroupRoghts(PL_Group_Master _objPL_Group_Master)
        {
            try
            {
                ObservableCollection<PL_Group_Master> __objPL_Group_Master = new ObservableCollection<PL_Group_Master>();
                this.dbManger.Open();
                this.dbManger.CreateParameters(2);
                this.dbManger.AddParameters(0, "@Type", "GROUPRIGHTS");
                this.dbManger.AddParameters(1, "@GroupID", _objPL_Group_Master.GroupID);
                IDataReader dataReader = dbManger.ExecuteReader(System.Data.CommandType.StoredProcedure, "[USP_MGroups]");
                while (dataReader.Read())
                {
                    __objPL_Group_Master.Add(new PL_Group_Master
                    {
                        GroupID = Convert.ToString(dataReader["GroupID"]),
                        MODULE_ID = Convert.ToString(dataReader["MODULE_ID"]),
                        MODULE_DESC = Convert.ToString(dataReader["MODULE_DESC"]),
                        VIEW_RIGHTS = Convert.ToBoolean(dataReader["VIEW_RIGHTS"]),
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
        public OperationResult SaveUpdateGroupRights(PL_Group_Master objPL_Group_Master)
        {
            int Result = 0;
            OperationResult oPeration = OperationResult.UpdateSuccess;
            DataTable DT = new DataTable();
            try
            {
                this.dbManger.Open();
               {
                    this.dbManger.CreateParameters(7);
                    this.dbManger.AddParameters(0, "@GroupID", int.Parse(objPL_Group_Master.GroupID));
                    this.dbManger.AddParameters(1, "@ModuleID", int.Parse(objPL_Group_Master.MODULE_ID));
                    this.dbManger.AddParameters(2, "@ViewRights", objPL_Group_Master.VIEW_RIGHTS);
                    this.dbManger.AddParameters(3, "@SaveRights", objPL_Group_Master.SAVE_RIGHTS);
                    this.dbManger.AddParameters(4, "@EditRights", objPL_Group_Master.EDIT_RIGHTS);
                    this.dbManger.AddParameters(5, "@DeleteRights", objPL_Group_Master.DELETE_RIGHTS);
                    this.dbManger.AddParameters(6, "@DownloadRights", objPL_Group_Master.DOWNLOAD_RIGHTS);
                    Result = dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_M_GroupRights");
                
                }
                if (Result == -1)
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
        
    }
}
