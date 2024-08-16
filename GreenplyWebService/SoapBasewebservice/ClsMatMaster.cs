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
    public class ClsMatMaster : BusinessBase<ClsMatMaster>
    {

        public static readonly PropertyInfo<string> MatCodeProperty = RegisterProperty<string>(c => c.MatCode);
        [RequiredButNotDefault(ErrorMessage = "Material Code is Mandatory.")]
        public string MatCode
        {
            get { return GetProperty(MatCodeProperty); }
            set { SetProperty(MatCodeProperty, value); }
        }

        public static readonly PropertyInfo<string> PlantCodeProperty = RegisterProperty<string>(c => c.PlantCode);
        [RequiredButNotDefault(ErrorMessage = "Plant Code is Mandatory.")]
        public string PlantCode
        {
            get { return GetProperty(PlantCodeProperty); }
            set { SetProperty(PlantCodeProperty, value); }
        }

        public static readonly PropertyInfo<string> ProductProperty = RegisterProperty<string>(c => c.Product);
        public string Product
        {
            get { return GetProperty(ProductProperty); }
            set { SetProperty(ProductProperty, value); }
        }

        public static readonly PropertyInfo<string> MatDescProperty = RegisterProperty<string>(c => c.MatDesc);
        public string MatDesc
        {
            get { return GetProperty(MatDescProperty); }
            set { SetProperty(MatDescProperty, value); }
        }

        public static readonly PropertyInfo<string> MatThicknessProperty = RegisterProperty<string>(c => c.MatThickness);
        public string MatThickness
        {
            get { return GetProperty(MatThicknessProperty); }
            set { SetProperty(MatThicknessProperty, value); }
        }

        public static readonly PropertyInfo<string> MatThicknessDescProperty = RegisterProperty<string>(c => c.MatThicknessDesc);
        public string MatThicknessDesc
        {
            get { return GetProperty(MatThicknessDescProperty); }
            set { SetProperty(MatThicknessDescProperty, value); }
        }


        public static readonly PropertyInfo<string> MatSizeProperty = RegisterProperty<string>(c => c.MatSize);
        public string MatSize
        {
            get { return GetProperty(MatSizeProperty); }
            set { SetProperty(MatSizeProperty, value); }
        }

        public static readonly PropertyInfo<string> MatGradeProperty = RegisterProperty<string>(c => c.MatGrade);
        public string MatGrade
        {
            get { return GetProperty(MatGradeProperty); }
            set { SetProperty(MatGradeProperty, value); }
        }

        public static readonly PropertyInfo<string> GradeDescProperty = RegisterProperty<string>(c => c.GradeDesc);
        public string GradeDesc
        {
            get { return GetProperty(GradeDescProperty); }
            set { SetProperty(GradeDescProperty, value); }
        }

        public static readonly PropertyInfo<string> CategoryProperty = RegisterProperty<string>(c => c.Category);
        public string Category
        {
            get { return GetProperty(CategoryProperty); }
            set { SetProperty(CategoryProperty, value); }
        }

        public static readonly PropertyInfo<string> CategoryDescProperty = RegisterProperty<string>(c => c.CategoryDesc);
        public string CategoryDesc
        {
            get { return GetProperty(CategoryDescProperty); }
            set { SetProperty(CategoryDescProperty, value); }
        }

        public static readonly PropertyInfo<string> MatGroupProperty = RegisterProperty<string>(c => c.MatGroup);
        public string MatGroup
        {
            get { return GetProperty(MatGroupProperty); }
            set { SetProperty(MatGroupProperty, value); }
        }

        public static readonly PropertyInfo<string> MatGroupDescProperty = RegisterProperty<string>(c => c.MatGroupDesc);
        public string MatGroupDesc
        {
            get { return GetProperty(MatGroupDescProperty); }
            set { SetProperty(MatGroupDescProperty, value); }
        }


        public static readonly PropertyInfo<string> DesignNoProperty = RegisterProperty<string>(c => c.DesignNo);
        public string DesignNo
        {
            get { return GetProperty(DesignNoProperty); }
            set { SetProperty(DesignNoProperty, value); }
        }


        public static readonly PropertyInfo<string> DesignDescProperty = RegisterProperty<string>(c => c.DesignDesc);
        public string DesignDesc
        {
            get { return GetProperty(DesignDescProperty); }
            set { SetProperty(DesignDescProperty, value); }
        }

        public static readonly PropertyInfo<string> FinishCodeProperty = RegisterProperty<string>(c => c.FinishCode);
        public string FinishCode
        {
            get { return GetProperty(FinishCodeProperty); }
            set { SetProperty(FinishCodeProperty, value); }
        }

        public static readonly PropertyInfo<string> FinishDescProperty = RegisterProperty<string>(c => c.FinishDesc);
        public string FinishDesc
        {
            get { return GetProperty(FinishDescProperty); }
            set { SetProperty(FinishDescProperty, value); }
        }

        public static readonly PropertyInfo<string> VisionPanelCodeProperty = RegisterProperty<string>(c => c.VisionPanelCode);
        public string VisionPanelCode
        {
            get { return GetProperty(VisionPanelCodeProperty); }
            set { SetProperty(VisionPanelCodeProperty, value); }
        }

        public static readonly PropertyInfo<string> VisionPanelDescProperty = RegisterProperty<string>(c => c.VisionPanelDesc);
        public string VisionPanelDesc
        {
            get { return GetProperty(VisionPanelDescProperty); }
            set { SetProperty(VisionPanelDescProperty, value); }
        }

        public static readonly PropertyInfo<string> LippingCodeProperty = RegisterProperty<string>(c => c.LippingCode);
        public string LippingCode
        {
            get { return GetProperty(LippingCodeProperty); }
            set { SetProperty(LippingCodeProperty, value); }
        }

        public static readonly PropertyInfo<string> LippingDescProperty = RegisterProperty<string>(c => c.LippingDesc);
        public string LippingDesc
        {
            get { return GetProperty(LippingDescProperty); }
            set { SetProperty(LippingDescProperty, value); }
        }

        public static readonly PropertyInfo<string> UOMProperty = RegisterProperty<string>(c => c.UOM);
        public string UOM
        {
            get { return GetProperty(UOMProperty); }
            set { SetProperty(UOMProperty, value); }
        }

        public static ClsMatMaster NewMatMasterOrder()
        {
            var newObj = DataPortal.Create<ClsMatMaster>();
            return newObj;
        }

        #region Insert

        string conString = ConfigurationManager.ConnectionStrings["myConnectionString"].ConnectionString;
        WriteLogFile ObjLog = new WriteLogFile();

        public void InsertMaterialMasterData(SqlConnection con1)  
        {
            try
            {
                //ObjLog.WriteLog("Transaction State " + con1.State + "");
                if (con1.State == System.Data.ConnectionState.Closed)
                    con1.Open();
                SqlCommand cmd = con1.CreateCommand(); 
                cmd.CommandType = System.Data.CommandType.Text;
                //cmd.Parameters.AddWithValue("@PlantCode", PlantCode);
                cmd.Parameters.AddWithValue("@Product", Product);
                cmd.Parameters.AddWithValue("@MatCode", MatCode);
                cmd.Parameters.AddWithValue("@MatDesc", MatDesc);
                cmd.Parameters.AddWithValue("@MatThickness", MatThickness);
                cmd.Parameters.AddWithValue("@MatThicknessDesc", MatThicknessDesc);
                cmd.Parameters.AddWithValue("@MatSize", MatSize);
                cmd.Parameters.AddWithValue("@MatGrade", MatGrade);
                cmd.Parameters.AddWithValue("@GradeDesc", GradeDesc);
                cmd.Parameters.AddWithValue("@Category", Category);
                cmd.Parameters.AddWithValue("@CategoryDesc", CategoryDesc);
                cmd.Parameters.AddWithValue("@MatGroup", MatGroup);
                cmd.Parameters.AddWithValue("@MatGroupDesc", MatGroupDesc);
                cmd.Parameters.AddWithValue("@DesignNo", DesignNo);
                cmd.Parameters.AddWithValue("@DesignDesc", DesignDesc);
                cmd.Parameters.AddWithValue("@FinishCode", FinishCode);
                cmd.Parameters.AddWithValue("@FinishDesc", FinishDesc);
                cmd.Parameters.AddWithValue("@VisionPanelCode", VisionPanelCode);
                cmd.Parameters.AddWithValue("@VisionPanelDesc", VisionPanelDesc);
                cmd.Parameters.AddWithValue("@LippingCode", LippingCode);
                cmd.Parameters.AddWithValue("@LippingDesc", LippingDesc);
                cmd.Parameters.AddWithValue("@UOM", UOM);

                int isExist = CheckExistMatMasterData(con1);
                if (isExist == 1)
                {
                    cmd.CommandText = UpdateMatMasterToSQL();
                    cmd.ExecuteNonQuery();
                }
                else if (isExist == 0)
                {
                    cmd.CommandText = InsertMatMasterToSQL();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                con1.Close();
                ObjLog.WriteLog("Load Material ==> " + ex.ToString() + " at " + DateTime.Now.ToString());
            }
        }

        private int CheckExistMatMasterData(SqlConnection con2)
        {
            if (con2.State == System.Data.ConnectionState.Closed)
                con2.Open();
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT Count(1) FROM [dbo].[mMaterialMaster] WHERE MatCode = @MatCode");   //PlantCode = @PlantCode AND 
            //ObjLog.WriteLog("Is Material Code exist query => " + sb.ToString() + " at " + DateTime.Now.ToString());
            SqlCommand cmd = con2.CreateCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.Parameters.AddWithValue("@MatCode", MatCode);
            //cmd.Parameters.AddWithValue("@PlantCode", PlantCode);
            cmd.CommandText = sb.ToString();
            int records = (int)cmd.ExecuteScalar();
            return records;
        }

        private string InsertMatMasterToSQL()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" INSERT INTO [dbo].[mMaterialMaster] ");
            sb.AppendLine(" ([Product], [MatCode], [MatDescription], [Thickness], [ThicknessDescription], [Size], [Grade], [GradeDescription], [Category], [CategoryDescription]");  //[PlantCode],
            sb.AppendLine(", [MatGroup], [MatGroupDescription], [DesignNo], [DesignDescription], [FinishCode], [FinishDescription]");
            sb.AppendLine(", [VisionPanelCode], [VisionPanelDescription], [LippingCode], [LippingDescription], [UOM], [DownloadOn], [DownloadBy] )");
            sb.AppendLine(" VALUES ( @Product, @MatCode, @MatDesc, @MatThickness, @MatThicknessDesc, @MatSize, @MatGrade, @GradeDesc, @Category, @CategoryDesc, ");  //@PlantCode, 
            sb.AppendLine(" @MatGroup, @MatGroupDesc, @DesignNo, @DesignDesc, @FinishCode, @FinishDesc, ");
            sb.AppendLine(" @VisionPanelCode, @VisionPanelDesc, @LippingCode, @LippingDesc, @UOM, GETDATE(), 'WebService' )");
            //ObjLog.WriteLog("Insert SAP Material Master Query=> " + sb.ToString() + " at " + DateTime.Now.ToString());
            return sb.ToString();
        }

        private string UpdateMatMasterToSQL()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" UPDATE [dbo].[mMaterialMaster] ");
            sb.AppendLine(" SET [Product] = @Product, [MatDescription] = @MatDesc, [Thickness] = @MatThickness, [ThicknessDescription] = @MatThicknessDesc, [Size] =  @MatSize, ");
            sb.AppendLine(" [Grade] = @MatGrade, [GradeDescription] = @GradeDesc, [Category] = @Category, [CategoryDescription] = @CategoryDesc, ");
            sb.AppendLine(" [MatGroup] = @MatGroup, [MatGroupDescription] = @MatGroupDesc, [DesignNo] = @DesignNo, [DesignDescription] = @DesignDesc, ");
            sb.AppendLine(" [FinishCode] = @FinishCode, [FinishDescription] = @FinishDesc, [VisionPanelCode] = @VisionPanelCode, [VisionPanelDescription] = @VisionPanelDesc, ");
            sb.AppendLine(" [LippingCode] = @LippingCode, [LippingDescription] = @LippingDesc, [UOM] = @UOM, [DownloadOn] = GETDATE(), [DownloadBy] = 'WebService' ");
            sb.AppendLine(" WHERE MatCode = @MatCode ");  //PlantCode = @PlantCode AND 
            //ObjLog.WriteLog("Update SAP Material Master Query => " + sb.ToString() + " at " + DateTime.Now.ToString());
            return sb.ToString();
        }

        #endregion  Insert
    }
}