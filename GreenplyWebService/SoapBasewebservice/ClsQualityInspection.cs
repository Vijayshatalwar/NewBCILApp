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
    public class ClsQualityInspection : BusinessBase<ClsQualityInspection>
    {

        public static readonly PropertyInfo<string> LocationCodeProperty = RegisterProperty<string>(c => c.LocationCode);
        [RequiredButNotDefault(ErrorMessage = "Location Code is Mandatory")]
        public string LocationCode
        {
            get { return GetProperty(LocationCodeProperty); }
            set { SetProperty(LocationCodeProperty, value); }
        }

        public static readonly PropertyInfo<string> PurchaseOrderNoProperty = RegisterProperty<string>(c => c.PurchaseOrderNo);
        [RequiredButNotDefault(ErrorMessage = "Purchase Order Number is Mandatory")]
        public string PurchaseOrderNo
        {
            get { return GetProperty(PurchaseOrderNoProperty); }
            set { SetProperty(PurchaseOrderNoProperty, value); }
        }

        public static readonly PropertyInfo<string> MatCodeProperty = RegisterProperty<string>(c => c.MatCode);
        [RequiredButNotDefault(ErrorMessage = "Material Code is Mandatory")]
        public string MatCode
        {
            get { return GetProperty(MatCodeProperty); }
            set { SetProperty(MatCodeProperty, value); }
        }

        public static readonly PropertyInfo<string> QRCodeProperty = RegisterProperty<string>(c => c.QRCode);
        public string QRCode
        {
            get { return GetProperty(QRCodeProperty); }
            set { SetProperty(QRCodeProperty, value); }
        }

        public static readonly PropertyInfo<string> MIGONoProperty = RegisterProperty<string>(c => c.MIGONo);
        public string MIGONo
        {
            get { return GetProperty(MIGONoProperty); }
            set { SetProperty(MIGONoProperty, value); }
        }

        public static readonly PropertyInfo<string> InspLotNoProperty = RegisterProperty<string>(c => c.InspLotNo);
        public string InspLotNo
        {
            get { return GetProperty(InspLotNoProperty); }
            set { SetProperty(InspLotNoProperty, value); }
        }


        public static ClsQualityInspection UpdateQuanlityInspection()
        {
            var newObj = DataPortal.Create<ClsQualityInspection>();
            return newObj;
        }

        WriteLogFile ObjLog = new WriteLogFile();
        string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;

        public void InsertQualityInspData(SqlConnection con1)
        {
            try
            {
                if (con1.State == System.Data.ConnectionState.Closed)
                    con1.Open();
                SqlCommand cmd = con1.CreateCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Parameters.AddWithValue("@LocationCode", LocationCode.Trim());
                cmd.Parameters.AddWithValue("@PONo", PurchaseOrderNo.Trim());
                cmd.Parameters.AddWithValue("@MatCode", MatCode.Trim());
                cmd.Parameters.AddWithValue("@QRCode", QRCode.Trim());
                cmd.Parameters.AddWithValue("@MIGONo", MIGONo.Trim());
                cmd.Parameters.AddWithValue("@InspLotNo", InspLotNo.Trim());

                int isExist = CheckExistQADetail(con1);
                if (isExist == 1)
                {
                    cmd.CommandText = UpdateQADataToSQL();
                    cmd.ExecuteNonQuery();
                    //ObjLog.WriteLog("Updated QualityInspData => QRCode - " + QRCode);
                }
            }
            catch (Exception ex)
            {
                con1.Close();
                ObjLog.WriteLog("Load QualityInspData => Error : " + ex.ToString());
            }
        }


        private int CheckExistQADetail(SqlConnection con2)
        {
            if (con2.State == System.Data.ConnectionState.Closed)
                con2.Open();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Count(1) FROM [dbo].[tLocationPrintingHistory] ");
            sb.Append("WHERE LocationCode = @LocationCode AND PONumber = @PONo AND MatCode = @MatCode ");
            sb.Append("AND QRCode = @QRCode AND ISNULL(MIGONo, '') = '' AND ISNULL(InspectionLotNo, '') = '' ");
            //ObjLog.WriteLog("Load QualityInspData => Select Query : " + sb.ToString());
            SqlCommand cmd = con2.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@LocationCode", LocationCode);
            cmd.Parameters.AddWithValue("@PONo", PurchaseOrderNo);
            cmd.Parameters.AddWithValue("@MatCode", MatCode);
            cmd.Parameters.AddWithValue("@QRCode", QRCode);
            cmd.CommandText = sb.ToString();
            int records = (int)cmd.ExecuteScalar();
            return records;
        }

        private string UpdateQADataToSQL()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UPDATE [dbo].[tLocationPrintingHistory] ");
            sb.AppendLine("SET [MIGONo] = @MIGONo, [InspectionLotNo] = @InspLotNo, [UpdateOn] = GETDATE() ");
            sb.Append("WHERE LocationCode = @LocationCode AND PONumber = @PONo AND MatCode = @MatCode ");
            sb.Append("AND QRCode = @QRCode AND ISNULL(MIGONo, '') = '' AND ISNULL(InspectionLotNo, '') = '' ");
            //ObjLog.WriteLog("Load QualityInspData => Update Query : " + sb.ToString());
            return sb.ToString();
        }
    }
}