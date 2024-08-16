using BCIL;
using Csla.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Csla;
using BCIL.Utility;
using System.Data.SqlClient;
using System.IO;
using System.Data;

namespace GreenplyWebService
{
    [Serializable]
    public class ClsRejectionMaster : BusinessBase<ClsRejectionMaster>
    {

        public static readonly PropertyInfo<string> RejCodeProperty = RegisterProperty<string>(c => c.RejCode);
        [RequiredButNotDefault(ErrorMessage = "Rejection Code is Mandatory.")]
        public string RejCode
        {
            get { return GetProperty(RejCodeProperty); }
            set { SetProperty(RejCodeProperty, value); }
        }

        public static readonly PropertyInfo<string> RejDescProperty = RegisterProperty<string>(c => c.RejDesc);
        [RequiredButNotDefault(ErrorMessage = "Rejection Desc is Mandatory.")]
        public string RejDesc
        {
            get { return GetProperty(RejDescProperty); }
            set { SetProperty(RejDescProperty, value); }
        }

        public static ClsRejectionMaster NewRejMasterOrder()
        {
            var newObj = DataPortal.Create<ClsRejectionMaster>();
            return newObj;
        }

        #region Insert

        string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
        WriteLogFile ObjLog = new WriteLogFile();

        public void InsertRejectionMasterData(SqlConnection con1)
        {
            try
            {
                if (con1.State == System.Data.ConnectionState.Closed)
                    con1.Open();
                SqlCommand cmd = con1.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@MatCode", RejCode);
                cmd.Parameters.AddWithValue("@MatDesc", RejDesc);

                int isExist = CheckExistRejectionMasterData(con1);
                if (isExist == 1)
                {
                    cmd.CommandText = UpdateRejMasterToSQL();
                    cmd.ExecuteNonQuery();
                }
                else if (isExist == 0)
                {
                    cmd.CommandText = InsertRejMasterToSQL();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                con1.Close();
                ObjLog.WriteLog("Load Rejection ==> Error : " + ex.ToString() + " at " + DateTime.Now.ToString());
            }
        }

        private int CheckExistRejectionMasterData(SqlConnection con2)
        {
            if (con2.State == System.Data.ConnectionState.Closed)
                con2.Open();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Count(1) FROM [dbo].[mMaterialMaster] WHERE MatCode = @MatCode");   
            SqlCommand cmd = con2.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@MatCode", RejCode);
            cmd.CommandText = sb.ToString();
            int records = (int)cmd.ExecuteScalar();
            return records;
        }

        private string InsertRejMasterToSQL()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" INSERT INTO [dbo].[mMaterialMaster] ");
            sb.AppendLine(" ([MatCode], [MatDescription])");
            sb.AppendLine(" VALUES ( @Product, @MatCode, @MatDesc, GETDATE(), 'WebService' )");
            return sb.ToString();
        }

        private string UpdateRejMasterToSQL()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" UPDATE [dbo].[mMaterialMaster] ");
            sb.AppendLine(" SET [MatDescription] = @MatDesc, [DownloadOn] = GETDATE(), [DownloadBy] = 'WebService' ");
            sb.AppendLine(" WHERE MatCode = @MatCode "); 
            return sb.ToString();
        }

        #endregion  Insert
    }

    public class ClsGetSAPRejectionMaster
    {
        public string mREJCODE { get; set; }

        public string mREJDESC { get; set; }

    }
}