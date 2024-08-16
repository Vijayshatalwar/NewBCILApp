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
    public class ClsSalesReturn : BusinessBase<ClsSalesReturn>
    {

        public static readonly PropertyInfo<string> SalesReturnNoProperty = RegisterProperty<string>(c => c.SalesReturnNo);
        [RequiredButNotDefault(ErrorMessage = "Sales Return Number is Mandatory.")]
        public string SalesReturnNo
        {
            get { return GetProperty(SalesReturnNoProperty); }
            set { SetProperty(SalesReturnNoProperty, value); }
        }

        public static readonly PropertyInfo<string> LocationCodeProperty = RegisterProperty<string>(c => c.LocationCode);
        [RequiredButNotDefault(ErrorMessage = "Location Code is Mandatory.")]
        public string LocationCode
        {
            get { return GetProperty(LocationCodeProperty); }
            set { SetProperty(LocationCodeProperty, value); }
        }

        public static readonly PropertyInfo<string> CustomerCodeProperty = RegisterProperty<string>(c => c.CustomerCode);
        public string CustomerCode
        {
            get { return GetProperty(CustomerCodeProperty); }
            set { SetProperty(CustomerCodeProperty, value); }
        }

        public static readonly PropertyInfo<string> MatCodeProperty = RegisterProperty<string>(c => c.MatCode);
        public string MatCode
        {
            get { return GetProperty(MatCodeProperty); }
            set { SetProperty(MatCodeProperty, value); }
        }

        public static readonly PropertyInfo<string> MatDescProperty = RegisterProperty<string>(c => c.MatDesc);
        public string MatDesc
        {
            get { return GetProperty(MatDescProperty); }
            set { SetProperty(MatDescProperty, value); }
        }

        public static readonly PropertyInfo<string> CustomerNameProperty = RegisterProperty<string>(c => c.CustomerName);
        public string CustomerName
        {
            get { return GetProperty(CustomerNameProperty); }
            set { SetProperty(CustomerNameProperty, value); }
        }

        public static readonly PropertyInfo<int> ReturnQtyProperty = RegisterProperty<int>(c => c.ReturnQty);
        public int ReturnQty
        {
            get { return GetProperty(ReturnQtyProperty); }
            set { SetProperty(ReturnQtyProperty, value); }
        }




        public static ClsSalesReturn NewSalesReturn()
        {
            var newObj = DataPortal.Create<ClsSalesReturn>();
            return newObj;
        }

        WriteLogFile ObjLog = new WriteLogFile();
        string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

        public void InsertSalesReturnData(SqlConnection con1)
        {
            try
            {
                if (con1.State == System.Data.ConnectionState.Closed)
                    con1.Open();
                SqlCommand cmd = con1.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@LocationCode", LocationCode);
                cmd.Parameters.AddWithValue("@SalesReturnNo", SalesReturnNo);
                cmd.Parameters.AddWithValue("@MatCode", MatCode);
                cmd.Parameters.AddWithValue("@MatDesc", MatDesc);
                cmd.Parameters.AddWithValue("@CustomerCode", CustomerCode);
                cmd.Parameters.AddWithValue("@CustomerName", CustomerName);
                cmd.Parameters.AddWithValue("@ReturnQty", ReturnQty);

                int isExist = CheckExistSalesReturnDetail(con1);
                if (isExist == 1)
                {
                    cmd.CommandText = UpdateSalesReturnDataToSQL();
                    cmd.ExecuteNonQuery();
                }
                else if (isExist == 0)
                {
                    cmd.CommandText = InsertSalesReturnDataToSQL();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                con1.Close();
                ObjLog.WriteLog("Load Sales Return ==> " + ex.ToString() + " at " + DateTime.Now.ToString());
            }
        }

        private int CheckExistSalesReturnDetail(SqlConnection con2)
        {
            if (con2.State == System.Data.ConnectionState.Closed)
                con2.Open();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Count(1) FROM [dbo].[tSAPSalesReturnData] WHERE LocationCode = @LocationCode AND SalesReturnNo = @SalesReturnNo AND MatCode = @MatCode");
            //ObjLog.WriteLog("Is Sales Return Number exist query => " + sb.ToString() + " at " + DateTime.Now.ToString());
            SqlCommand cmd = con2.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@LocationCode", LocationCode);
            cmd.Parameters.AddWithValue("@SalesReturnNo", SalesReturnNo);
            cmd.Parameters.AddWithValue("@MatCode", MatCode);
            cmd.CommandText = sb.ToString();
            int records = (int)cmd.ExecuteScalar();
            return records;
        }


        private string InsertSalesReturnDataToSQL()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" INSERT INTO [dbo].[tSAPSalesReturnData] ");
            sb.AppendLine(" ([LocationCode], [SalesReturnNo], [CustomerCode], [CustomerName], [MatCode], [MatDesc]");
            sb.AppendLine(", [ReturnQty], [ReturnStatus], [ScannedQty], [DownloadOn], [DownloadBy] )");
            sb.AppendLine(" VALUES ( @LocationCode, @SalesReturnNo, @CustomerCode, @CustomerName, @MatCode, @MatDesc,");
            sb.AppendLine(" @ReturnQty, 'N', 0, GETDATE(), 'WebService')");
            //ObjLog.WriteLog("Insert SAP Sales Return Number Query=> " + sb.ToString() + " at " + DateTime.Now.ToString());
            return sb.ToString();
        }

        private string UpdateSalesReturnDataToSQL()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" UPDATE [dbo].[tSAPSalesReturnData] ");
            sb.AppendLine(" SET [CustomerCode] = @CustomerCode, [CustomerName] = @CustomerName, [MatCode] = @MatCode, [MatDesc] =  @MatDesc,");
            sb.AppendLine(" [ReturnQty] = @ReturnQty, ");
            sb.AppendLine(" [DownloadOn] = GETDATE(), [DownloadBy] = 'WebService'");
            sb.AppendLine(" WHERE LocationCode = @LocationCode AND SalesReturnNo = @SalesReturnNo ");
            //ObjLog.WriteLog("Update SAP Sales Return Number Query => " + sb.ToString() + " at " + DateTime.Now.ToString());
            return sb.ToString();
        }


    }
}