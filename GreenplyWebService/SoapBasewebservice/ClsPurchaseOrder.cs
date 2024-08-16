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
    public class ClsPurchaseOrder : BusinessBase<ClsPurchaseOrder>
    {
        public static readonly PropertyInfo<string> PONumberProperty = RegisterProperty<string>(c => c.PONumber);
        [RequiredButNotDefault(ErrorMessage = "Purchase Order Number is Mandatory.")]
        public string PONumber
        {
            get { return GetProperty(PONumberProperty); }
            set { SetProperty(PONumberProperty, value); }
        }

        public static readonly PropertyInfo<string> LocationCodeProperty = RegisterProperty<string>(c => c.LocationCode);
        [RequiredButNotDefault(ErrorMessage = "Location Code is Mandatory.")]
        public string LocationCode
        {
            get { return GetProperty(LocationCodeProperty); }
            set { SetProperty(LocationCodeProperty, value); }
        }

        public static readonly PropertyInfo<string> VendorCodeProperty = RegisterProperty<string>(c => c.VendorCode);
        public string VendorCode
        {
            get { return GetProperty(VendorCodeProperty); }
            set { SetProperty(VendorCodeProperty, value); }
        }

        public static readonly PropertyInfo<string> VendorNameProperty = RegisterProperty<string>(c => c.VendorName);
        public string VendorName
        {
            get { return GetProperty(VendorNameProperty); }
            set { SetProperty(VendorNameProperty, value); }
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

        public static readonly PropertyInfo<int> POQtyProperty = RegisterProperty<int>(c => c.POQty);
        public int POQty
        {
            get { return GetProperty(POQtyProperty); }
            set { SetProperty(POQtyProperty, value); }
        }

        public static readonly PropertyInfo<string> POLocTypeProperty = RegisterProperty<string>(c => c.POLocType);
        public string POLocType
        {
            get { return GetProperty(POLocTypeProperty); }
            set { SetProperty(POLocTypeProperty, value); }
        }

        public static readonly PropertyInfo<DateTime> PODateProperty = RegisterProperty<DateTime>(c => c.PODate);
        public DateTime PODate
        {
            get { return GetProperty(PODateProperty); }
            set { SetProperty(PODateProperty, value); }
        }


        public static ClsPurchaseOrder NewPurchaseOrder()
        {
            var newObj = DataPortal.Create<ClsPurchaseOrder>();
            return newObj;
        }

        #region Insert

        WriteLogFile ObjLog = new WriteLogFile();
        string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

        public void InsertPurchaseOrderData(SqlConnection con1)
        {
            try
            {
                if (con1.State == System.Data.ConnectionState.Closed)
                    con1.Open();
                SqlCommand cmd = con1.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@LocationCode", LocationCode.Trim());
                cmd.Parameters.AddWithValue("@PONumber", PONumber.Trim());
                cmd.Parameters.AddWithValue("@MatCode", MatCode.Trim());
                cmd.Parameters.AddWithValue("@MatDesc", MatDesc.Trim());
                cmd.Parameters.AddWithValue("@VendorCode", VendorCode.Trim());
                cmd.Parameters.AddWithValue("@VendorName", VendorName.Trim());
                cmd.Parameters.AddWithValue("@POQty", POQty);
                cmd.Parameters.AddWithValue("@POLocType", POLocType);
                cmd.Parameters.AddWithValue("@PODate", PODate);

                int isExist = CheckExistPODetail(con1);
                if (isExist == 1)
                {
                    cmd.CommandText = UpdatePODataToSQL();
                    cmd.ExecuteNonQuery();
                }
                else if (isExist == 0)
                {
                    cmd.CommandText = InsertPODataToSQL();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                con1.Close();
                ObjLog.WriteLog("Load Purchase Order ==> " + ex.ToString() + " at " + DateTime.Now.ToString());
            }
        }

        private int CheckExistPODetail(SqlConnection con2)
        {
            if (con2.State == System.Data.ConnectionState.Closed)
                con2.Open();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Count(1) FROM [dbo].[tSAPPurchaseOrderData] WHERE LocationCode = @LocationCode AND PONumber = @PONumber AND MatCode = @MatCode");
            SqlCommand cmd = con2.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@LocationCode", LocationCode);
            cmd.Parameters.AddWithValue("@PONumber", PONumber);
            cmd.Parameters.AddWithValue("@MatCode", MatCode);
            cmd.CommandText = sb.ToString();
            int records = (int)cmd.ExecuteScalar();
            return records;
        }

        private string InsertPODataToSQL()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" INSERT INTO [dbo].[tSAPPurchaseOrderData] ");
            sb.AppendLine(" ([LocationCode], [PONumber], [VendorCode], [VendorName], [MatCode], [MatDescription]");
            sb.AppendLine(", [POQty], [POLocType], [PODate], [IsQRCodeGenerated], [GeneratedQty], [POStatus], [IsPrinted], [IsReprinted], [DownloadOn], [DownloadBy] )");
            sb.AppendLine(" VALUES ( @LocationCode, @PONumber, @VendorCode, @VendorName, @MatCode, @MatDesc,");
            sb.AppendLine(" @POQty, @POLocType, @PODate, 0, 0, 'N', 0, 0, GETDATE(), 'WebService')");
            //ObjLog.WriteLog("Insert SAP Material Master Query=> " + sb.ToString() + " at " + DateTime.Now.ToString());
            return sb.ToString();
        }

        private string UpdatePODataToSQL()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" UPDATE [dbo].[tSAPPurchaseOrderData] ");
            sb.AppendLine(" SET [VendorCode] = @VendorCode, [VendorName] = @VendorName, [MatDescription] = @MatDesc, ");
            sb.AppendLine(" [POQty] = @POQty, [POLocType] = @POLocType,");
            sb.AppendLine(" [PODate] = @PODate, [DownloadOn] = GETDATE(), [DownloadBy] = 'WebService'");
            sb.AppendLine(" WHERE LocationCode = @LocationCode AND PONumber = @PONumber AND MatCode = @MatCode");
            //ObjLog.WriteLog("Update SAP Material Master Query => " + sb.ToString() + " at " + DateTime.Now.ToString());
            return sb.ToString();
        }

        #endregion  Insert

    }

    public class ClsPurchaseOrderReturn : BusinessBase<ClsPurchaseOrderReturn>
    {
        public static readonly PropertyInfo<string> PONumberProperty = RegisterProperty<string>(c => c.POReturnNumber);
        [RequiredButNotDefault(ErrorMessage = "Purchase Return Order Number is Mandatory.")]
        public string POReturnNumber
        {
            get { return GetProperty(PONumberProperty); }
            set { SetProperty(PONumberProperty, value); }
        }

        public static readonly PropertyInfo<string> LocationCodeProperty = RegisterProperty<string>(c => c.LocationCode);
        [RequiredButNotDefault(ErrorMessage = "Location Code is Mandatory.")]
        public string LocationCode
        {
            get { return GetProperty(LocationCodeProperty); }
            set { SetProperty(LocationCodeProperty, value); }
        }

        public static readonly PropertyInfo<string> VendorCodeProperty = RegisterProperty<string>(c => c.VendorCode);
        public string VendorCode
        {
            get { return GetProperty(VendorCodeProperty); }
            set { SetProperty(VendorCodeProperty, value); }
        }

        public static readonly PropertyInfo<string> VendorNameProperty = RegisterProperty<string>(c => c.VendorName);
        public string VendorName
        {
            get { return GetProperty(VendorNameProperty); }
            set { SetProperty(VendorNameProperty, value); }
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

        public static readonly PropertyInfo<int> ReturnQtyProperty = RegisterProperty<int>(c => c.ReturnQty);
        public int ReturnQty
        {
            get { return GetProperty(ReturnQtyProperty); }
            set { SetProperty(ReturnQtyProperty, value); }
        }

        public static readonly PropertyInfo<string> POLocTypeProperty = RegisterProperty<string>(c => c.POLocType);
        public string POLocType
        {
            get { return GetProperty(POLocTypeProperty); }
            set { SetProperty(POLocTypeProperty, value); }
        }



        public static ClsPurchaseOrderReturn NewPurchaseOrderReturn()
        {
            var newObj = DataPortal.Create<ClsPurchaseOrderReturn>();
            return newObj;
        }

        #region Insert

        WriteLogFile ObjLog = new WriteLogFile();
        string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

        public void InsertPurchaseOrderReturnData(SqlConnection con1)
        {
            try
            {
                if (con1.State == System.Data.ConnectionState.Closed)
                    con1.Open();
                SqlCommand cmd = con1.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@LocationCode", LocationCode.Trim());
                cmd.Parameters.AddWithValue("@POReturnNumber", POReturnNumber.Trim());
                cmd.Parameters.AddWithValue("@VendorCode", VendorCode.Trim());
                cmd.Parameters.AddWithValue("@VendorName", VendorName.Trim());
                cmd.Parameters.AddWithValue("@MatCode", MatCode.Trim());
                cmd.Parameters.AddWithValue("@MatDesc", MatDesc.Trim());
                cmd.Parameters.AddWithValue("@ReturnQty", ReturnQty);
                cmd.Parameters.AddWithValue("@POLocType", POLocType);

                int isExist = CheckExistReturnPODetail(con1);
                if (isExist == 1)
                {
                    cmd.CommandText = UpdateReturnPODataToSQL();
                    cmd.ExecuteNonQuery();
                }
                else if (isExist == 0)
                {
                    cmd.CommandText = InsertReturnPODataToSQL();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                con1.Close();
                ObjLog.WriteLog("Load Purchase Order ==> " + ex.ToString() + " at " + DateTime.Now.ToString());
            }
        }

        private int CheckExistReturnPODetail(SqlConnection con2)
        {
            if (con2.State == System.Data.ConnectionState.Closed)
                con2.Open();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Count(1) FROM [dbo].[tSAPPurchaseReturnData] WHERE LocationCode = @LocationCode AND POReturnNo = @POReturnNumber AND MatCode = @MatCode");
            SqlCommand cmd = con2.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@LocationCode", LocationCode);
            cmd.Parameters.AddWithValue("@POReturnNumber", POReturnNumber);
            cmd.Parameters.AddWithValue("@MatCode", MatCode);
            cmd.CommandText = sb.ToString();
            int records = (int)cmd.ExecuteScalar();
            return records;
        }

        private string InsertReturnPODataToSQL()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" INSERT INTO [dbo].[tSAPPurchaseReturnData] ");
            sb.AppendLine(" ([LocationCode], [POReturnNo], [VendorCode], [VendorName], [MatCode], [MatDesc],");
            sb.AppendLine(" [ReturnQty], [POLocType], [ScannedQty], [ReturnStatus], [DownloadOn], [DownloadBy] )");
            sb.AppendLine(" VALUES ( @LocationCode, @POReturnNumber, @VendorCode, @VendorName, @MatCode, @MatDesc,");
            sb.AppendLine(" @ReturnQty, @POLocType, 0, 'N', GETDATE(), 'WebService') ");
            //ObjLog.WriteLog("Insert SAP Material Master Query=> " + sb.ToString() + " at " + DateTime.Now.ToString());
            return sb.ToString();
        }

        private string UpdateReturnPODataToSQL()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" UPDATE [dbo].[tSAPPurchaseReturnData] ");
            sb.AppendLine(" SET [VendorCode] = @VendorCode, [VendorName] = @VendorName, [MatDesc] = @MatDesc,");
            sb.AppendLine(" [ReturnQty] = @ReturnQty, [POLocType] = @POLocType, ");
            sb.AppendLine(" [DownloadOn] = GETDATE(), [DownloadBy] = 'WebService'");
            sb.AppendLine(" WHERE LocationCode = @LocationCode AND POReturnNumber = @POReturnNumber AND MatCode = @MatCode");
            //ObjLog.WriteLog("Update SAP Material Master Query => " + sb.ToString() + " at " + DateTime.Now.ToString());
            return sb.ToString();
        }

        #endregion  Insert

    }
}