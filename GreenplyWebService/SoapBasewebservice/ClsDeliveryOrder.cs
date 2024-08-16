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
    public class ClsDeliveryOrder : BusinessBase<ClsDeliveryOrder>
    {
        public static readonly PropertyInfo<string> DeliveryOrderNoProperty = RegisterProperty<string>(c => c.DeliveryOrderNo);
        [RequiredButNotDefault(ErrorMessage = "Delivery Order Number is Mandatory.")]
        public string DeliveryOrderNo
        {
            get { return GetProperty(DeliveryOrderNoProperty); }
            set { SetProperty(DeliveryOrderNoProperty, value); }
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

        public static readonly PropertyInfo<string> CustomerNameProperty = RegisterProperty<string>(c => c.CustomerName);
        public string CustomerName
        {
            get { return GetProperty(CustomerNameProperty); }
            set { SetProperty(CustomerNameProperty, value); }
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

        public static readonly PropertyInfo<string> ToLocationCodeProperty = RegisterProperty<string>(c => c.ToLocationCode);
        public string ToLocationCode
        {
            get { return GetProperty(ToLocationCodeProperty); }
            set { SetProperty(ToLocationCodeProperty, value); }
        }

        public static readonly PropertyInfo<int> DeliveryOrderQtyProperty = RegisterProperty<int>(c => c.DeliveryOrderQty);
        public int DeliveryOrderQty
        {
            get { return GetProperty(DeliveryOrderQtyProperty); }
            set { SetProperty(DeliveryOrderQtyProperty, value); }
        }


        public static readonly PropertyInfo<DateTime> DeliveryOrderDateProperty = RegisterProperty<DateTime>(c => c.DeliveryOrderDate);
        public DateTime DeliveryOrderDate
        {
            get { return GetProperty(DeliveryOrderDateProperty); }
            set { SetProperty(DeliveryOrderDateProperty, value); }
        }




        public static ClsDeliveryOrder NewDeliveryOrder()
        {
            var newObj = DataPortal.Create<ClsDeliveryOrder>();
            return newObj;
        }

        WriteLogFile ObjLog = new WriteLogFile();
        string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

        public void InsertDeliveryOrderData(SqlConnection con1)
        {
            try
            {
                if (con1.State == System.Data.ConnectionState.Closed)
                    con1.Open();
                SqlCommand cmd = con1.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@LocationCode", LocationCode.Trim());
                cmd.Parameters.AddWithValue("@DeliveryOrderNo", DeliveryOrderNo.Trim());
                cmd.Parameters.AddWithValue("@CustomerCode", CustomerCode.Trim());
                cmd.Parameters.AddWithValue("@CustomerName", CustomerName.Trim());
                cmd.Parameters.AddWithValue("@MatCode", MatCode.Trim());
                cmd.Parameters.AddWithValue("@MatDesc", MatDesc.Trim());
                cmd.Parameters.AddWithValue("@ToLocationCode", ToLocationCode.Trim());
                cmd.Parameters.AddWithValue("@DOQty", DeliveryOrderQty);
                cmd.Parameters.AddWithValue("@DODate", DeliveryOrderDate);

                int isExist = CheckExistDODetail(con1);
                if (isExist == 1)
                {
                    cmd.CommandText = UpdateDODataToSQL();
                    cmd.ExecuteNonQuery();
                }
                else if (isExist == 0)
                {
                    cmd.CommandText = InsertDODataToSQL();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                con1.Close();
                ObjLog.WriteLog("Load Delivery Order ==> " + ex.ToString());
            }
        }


        private int CheckExistDODetail(SqlConnection con2)
        {
            if (con2.State == System.Data.ConnectionState.Closed)
                con2.Open();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Count(1) FROM [dbo].[tSAPDeliveryOrderData] WHERE LocationCode = @LocationCode AND DeliveryOrderNo = @DeliveryOrderNo AND MatCode = @MatCode");
            //ObjLog.WriteLog("Is Delivery Order No exist query => " + sb.ToString() + " at " + DateTime.Now.ToString());
            SqlCommand cmd = con2.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@LocationCode", LocationCode);
            cmd.Parameters.AddWithValue("@DeliveryOrderNo", DeliveryOrderNo);
            cmd.Parameters.AddWithValue("@MatCode", MatCode);
            cmd.CommandText = sb.ToString();
            int records = (int)cmd.ExecuteScalar();
            return records;
        }

        private string InsertDODataToSQL()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" INSERT INTO [dbo].[tSAPDeliveryOrderData] ");
            sb.AppendLine(" ( [LocationCode], [DeliveryOrderNo], [CustomerCode], [CustomerName], [MatCode], [MatDesc], [ToLocationCode]");
            sb.AppendLine(", [DOQty], [DODate], [DOStatus], [DOCancelStatus], [IsSAPPosted], [DownloadOn], [DownloadBy] )");
            sb.AppendLine(" VALUES ( @LocationCode, @DeliveryOrderNo, @CustomerCode, @CustomerName, @MatCode, @MatDesc, @ToLocationCode, ");
            sb.AppendLine(" @DOQty, @DODate, 'N', 'N', 0, GETDATE(), 'WebService' )");
            //ObjLog.WriteLog("Insert SAP Delivery Order No Query=> " + sb.ToString() + " at " + DateTime.Now.ToString());
            return sb.ToString();

            //convert(varchar, getdate(), 105)  dd-MM-yyyy
        }

        private string UpdateDODataToSQL()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" UPDATE [dbo].[tSAPDeliveryOrderData] ");
            sb.AppendLine(" SET [CustomerCode] = @CustomerCode, [CustomerName] = @CustomerName, [MatDesc] =  @MatDesc,");
            sb.AppendLine(" [ToLocationCode] = @ToLocationCode, [DOQty] = @DOQty, ");
            sb.AppendLine(" [DODate] = @DODate, [DownloadOn] = GETDATE(), [DownloadBy] = 'WebService' ");
            sb.AppendLine(" WHERE LocationCode = @LocationCode AND DeliveryOrderNo = @DeliveryOrderNo AND MatCode = @MatCode");
            //ObjLog.WriteLog("Update SAP Delivery Order No Query => " + sb.ToString() + " at " + DateTime.Now.ToString());
            return sb.ToString();
        }

    }
}