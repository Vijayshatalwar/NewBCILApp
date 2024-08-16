using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ENTITY_LAYER;
using System.Data;
using COMMON;
using COMMON_LAYER;

namespace DATA_ACCESS_LAYER
{
    public class DL_TradingData : DlCommon
    {
        DBManager dbManger = null;
        DlCommon dCommon = null;
        DataTable dt = null;
        StringBuilder sb = null;

        public DL_TradingData()
        {
            this.dCommon = new DlCommon();
            this.dbManger = DBProvider();
        }

        public DataTable DlGetPONo()
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(1);
                dbManger.AddParameters(0, "@Type", "GetPONumber");
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_M_TrandingDetails").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DlGetProducts()
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(1);
                dbManger.AddParameters(0, "@Type", "GetProducts");
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_M_TrandingDetails").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DlGetPODetails(string PONum)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(2);
                dbManger.AddParameters(0, "@Type", "GetPODetails");
                dbManger.AddParameters(1, "@PONo", PONum.Trim());
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_M_TrandingDetails").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DlGetPOMatData(string PONum, string VerticalName, string MatDesc)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(4);
                dbManger.AddParameters(0, "@Type", "GetSelectedPOMatData");
                dbManger.AddParameters(1, "@PONo", PONum.Trim());
                dbManger.AddParameters(2, "@VarticalName", VerticalName.Trim());
                dbManger.AddParameters(3, "@ItemDesc", MatDesc.Trim());
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_M_TrandingDetails").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DlGetPOMatQty(string PONum, string VendorName, string MatDesc)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(4);
                dbManger.AddParameters(0, "@Type", "GetSelectedPOMatQty");
                dbManger.AddParameters(1, "@PONo", PONum.Trim());
                dbManger.AddParameters(2, "@VandorName", VendorName.Trim());
                dbManger.AddParameters(3, "@ItemDesc", MatDesc.Trim());
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_M_TrandingDetails").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DlGetMatCode(string ProductName, string MatDesc)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(3);
                dbManger.AddParameters(0, "@Type", "GetMatCode");
                dbManger.AddParameters(1, "@ProductName", ProductName.Trim());
                dbManger.AddParameters(2, "@ItemDesc", MatDesc.Trim());
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_M_TrandingDetails").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DlGetPOVerticalDetails(string VerName)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(2);
                dbManger.AddParameters(0, "@Type", "GetSelectedPOVerticalDetails");
                dbManger.AddParameters(1, "@VarticalName", VerName.Trim());
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_M_TrandingDetails").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DlGetMatDescBySelectedProduct(string ProductName)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(2);
                dbManger.AddParameters(0, "@Type", "GetMatDescBySelectedProduct");
                dbManger.AddParameters(1, "@ProductName", ProductName.Trim());
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_M_TrandingDetails").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DlGetSelectedPOItemsData(string PONum)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(2);
                dbManger.AddParameters(0, "@Type", "GetSelectedPOItemsData");
                dbManger.AddParameters(1, "@PONo", PONum.Trim());
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_M_TrandingDetails").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DlGetSelectedPOVerticalData(string PONum)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(2);
                dbManger.AddParameters(0, "@Type", "GetSelectedPOVerticalData");
                dbManger.AddParameters(1, "@PONo", PONum.Trim());
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_M_TrandingDetails").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }



        public DataTable GetSelectedItem()
        {
            dt = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(1);
                this.dbManger.AddParameters(0, "@Type", "GetSelectedItem");
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_M_TrandingDetails").Tables[0];
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
            return dt;
        }

        public string DL_GetPlantType()
        {
            string PlantType = "";
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(2);
                this.dbManger.AddParameters(0, "@Type", "GETPLANTTYPE");
                this.dbManger.AddParameters(1, "@PlantCode", VariableInfo.mPlantCode);
                PlantType = Convert.ToString(this.dbManger.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USP_M_TrandingDetails"));
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

        public string DL_SaveBarcode(string Plantcode, string Itemcode, string Barcode, string VBarcode)
        {
            string strBarcode = "";
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(5);
                this.dbManger.AddParameters(0, "@Type", "GetNewBarcode");
                this.dbManger.AddParameters(1, "@PlantCode", Plantcode);
                this.dbManger.AddParameters(2, "@ItemCode", Itemcode);
                this.dbManger.AddParameters(3, "@QRcode", Barcode);
                this.dbManger.AddParameters(4, "@Virtualcode", VBarcode);
                strBarcode = Convert.ToString(this.dbManger.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USP_M_TrandingDetails"));
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
            return strBarcode;
        }
    }
}
