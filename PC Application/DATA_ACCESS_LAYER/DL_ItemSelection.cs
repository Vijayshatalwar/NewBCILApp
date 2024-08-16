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
    public class DL_ItemSelection : DlCommon
    {
        DBManager dbManger = null;
        DlCommon dCommon = null;
        DataTable dt = null;
        StringBuilder sb = null;

        public DL_ItemSelection()
        {
            this.dCommon = new DlCommon();
            this.dbManger = DBProvider();
        }

        #region Decorative

        public DataTable DLVGetMatProduct(string ProductType)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(3);
                dbManger.AddParameters(0, "@Type", "GETVMATPRODUCT");
                dbManger.AddParameters(1, "@Product", ProductType.Trim());
                dbManger.AddParameters(2, "@LocationCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLVGetMatCategory(string objProduct)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(3);
                dbManger.AddParameters(0, "@Type", "GETVMATCATEGORY");
                dbManger.AddParameters(1, "@Product", objProduct);
                dbManger.AddParameters(2, "@LocationCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLVGetMatThickness(string objProduct, string objCat)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(4);
                dbManger.AddParameters(0, "@Type", "GETVMATTHICKNESS");
                dbManger.AddParameters(1, "@Product", objProduct);
                dbManger.AddParameters(2, "@Category", objCat);
                dbManger.AddParameters(3, "@LocationCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLVGetMatGroup(string objProduct, string objCat, string objThickness)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(5);
                dbManger.AddParameters(0, "@Type", "GETVMATGROUP");
                dbManger.AddParameters(1, "@Product", objProduct);
                dbManger.AddParameters(2, "@Category", objCat);
                dbManger.AddParameters(3, "@Thickness", objThickness);
                dbManger.AddParameters(4, "@LocationCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLVGetMatGrade(string objProduct, string objCat, string objThickness, string objGroup)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(6);
                dbManger.AddParameters(0, "@Type", "GETVMATGRADE");
                dbManger.AddParameters(1, "@Product", objProduct);
                dbManger.AddParameters(2, "@Category", objCat);
                dbManger.AddParameters(3, "@Thickness", objThickness);
                dbManger.AddParameters(4, "@Group", objGroup);
                dbManger.AddParameters(5, "@LocationCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLVGetMatDesign(string objProduct, string objCat, string objThickness, string objGroup, string objGrade)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(7);
                dbManger.AddParameters(0, "@Type", "GETVMATDESIGNNO");
                dbManger.AddParameters(1, "@Product", objProduct);
                dbManger.AddParameters(2, "@Category", objCat);
                dbManger.AddParameters(3, "@Thickness", objThickness);
                dbManger.AddParameters(4, "@Group", objGroup);
                dbManger.AddParameters(5, "@Grade", objGrade);
                dbManger.AddParameters(6, "@LocationCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLVGetMatFinishCode(string objProduct, string objCat, string objThickness, string objGroup, string objGrade, string objDesignNo)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(8);
                dbManger.AddParameters(0, "@Type", "GETVMATFINISHCODE");
                dbManger.AddParameters(1, "@Product", objProduct);
                dbManger.AddParameters(2, "@Category", objCat);
                dbManger.AddParameters(3, "@Thickness", objThickness);
                dbManger.AddParameters(4, "@Group", objGroup);
                dbManger.AddParameters(5, "@Grade", objGrade);
                dbManger.AddParameters(6, "@DesignNo", objDesignNo);
                dbManger.AddParameters(7, "@LocationCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLVGetMatSize(string objProduct, string objCat, string objThickness, string objGroup, string objGrade, string objDesignNo, string FinishCode)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(9);
                dbManger.AddParameters(0, "@Type", "GETVMATSIZE");
                dbManger.AddParameters(1, "@Product", objProduct);
                dbManger.AddParameters(2, "@Category", objCat);
                dbManger.AddParameters(3, "@Thickness", objThickness);
                dbManger.AddParameters(4, "@Group", objGroup);
                dbManger.AddParameters(5, "@Grade", objGrade);
                dbManger.AddParameters(6, "@DesignNo", objDesignNo);
                dbManger.AddParameters(7, "@FinishCode", FinishCode);
                dbManger.AddParameters(8, "@LocationCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLVGetMatVisionCode(string objProduct, string objCat, string objThickness, string objGroup, string objGrade, string objDesignNo, string objFinishCode, string objSize)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(10);
                dbManger.AddParameters(0, "@Type", "GETVMATVISIONPANELCODE");
                dbManger.AddParameters(1, "@Product", objProduct);
                dbManger.AddParameters(2, "@Category", objCat);
                dbManger.AddParameters(3, "@Thickness", objThickness);
                dbManger.AddParameters(4, "@Group", objGroup);
                dbManger.AddParameters(5, "@Grade", objGrade);
                dbManger.AddParameters(6, "@DesignNo", objDesignNo);
                dbManger.AddParameters(7, "@FinishCode", objFinishCode);
                dbManger.AddParameters(8, "@Size", objSize);
                dbManger.AddParameters(9, "@LocationCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLVGetMatLippingCode(string objProduct, string objCat, string objThickness, string objGroup, string objGrade, string objDesignNo, string objFinishCode, string objSize, string objVisionCode)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(11);
                dbManger.AddParameters(0, "@Type", "GETVMATLIPPINGCODE");
                dbManger.AddParameters(1, "@Product", objProduct);
                dbManger.AddParameters(2, "@Category", objCat);
                dbManger.AddParameters(3, "@Thickness", objThickness);
                dbManger.AddParameters(4, "@Group", objGroup);
                dbManger.AddParameters(5, "@Grade", objGrade);
                dbManger.AddParameters(6, "@DesignNo", objDesignNo);
                dbManger.AddParameters(7, "@FinishCode", objFinishCode);
                dbManger.AddParameters(8, "@Size", objSize);
                dbManger.AddParameters(9, "@VisionCode", objVisionCode);
                dbManger.AddParameters(10, "@LocationCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLVGetSelectedMatCode(string objProduct, string objCat, string objThickness, string objGroup, string objGrade, string objDesignNo, string objFinishCode, string objSize, string objVisionCode, string objLippingCode)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(12);
                dbManger.AddParameters(0, "@Type", "GETSELECTEDMATERIALDATA");
                dbManger.AddParameters(1, "@Product", objProduct.Trim());
                dbManger.AddParameters(2, "@Group", objGroup.Trim());
                dbManger.AddParameters(3, "@Grade", objGrade.Trim());
                dbManger.AddParameters(4, "@Category", objCat.Trim());
                dbManger.AddParameters(5, "@Thickness", objThickness.Trim());
                dbManger.AddParameters(6, "@Size", objSize.Trim());
                dbManger.AddParameters(7, "@DesignNo", objDesignNo.Trim());
                dbManger.AddParameters(8, "@FinishCode", objFinishCode.Trim());
                dbManger.AddParameters(9, "@VisionCode", objVisionCode.Trim());
                dbManger.AddParameters(10, "@LippingCode", objLippingCode.Trim());
                dbManger.AddParameters(11, "@LocationCode", VariableInfo.mPlantCode.Trim());
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLVSaveSelectedMatCode(string objMatCode, string objMatDesc, string objMatSize, string objMatThickness, int LotSize, string sBatchNo)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(7);
                dbManger.AddParameters(0, "@Type", "UPDATEDECORSELECTEDMATERIALCODESTATUS");
                dbManger.AddParameters(1, "@MatCode", objMatCode);
                dbManger.AddParameters(2, "@MatDesc", objMatDesc);
                dbManger.AddParameters(3, "@Size", objMatSize);
                dbManger.AddParameters(4, "@Thickness", objMatThickness);
                dbManger.AddParameters(5, "@LotSize", LotSize);
                dbManger.AddParameters(6, "@VBatchNo", sBatchNo);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public string DLVSaveQRCode(string objLocationCode, string objMatCode, string sMatDesc, string sGrade, string sGroup, string sThickness, string sSize, string objQRCode, string sMatStatus, string sDateFormat, string sPrintSection, string sLocType, string oLabelType, string sPONo, string sVendorCode, string sBatchNo)
        {
            string oStatus = "";
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(18);
                this.dbManger.AddParameters(0, "@Type", "SAVEQRCODE");
                this.dbManger.AddParameters(1, "@LocationCode", objLocationCode);
                this.dbManger.AddParameters(2, "@MatCode", objMatCode.Trim());
                this.dbManger.AddParameters(3, "@MatDesc", sMatDesc.Trim());
                this.dbManger.AddParameters(4, "@Grade", sGrade.Trim());
                this.dbManger.AddParameters(5, "@Group", sGroup.Trim());
                this.dbManger.AddParameters(6, "@Thickness", sThickness.Trim());
                this.dbManger.AddParameters(7, "@Size", sSize.Trim());
                this.dbManger.AddParameters(8, "@QRCode", objQRCode.Trim());
                this.dbManger.AddParameters(9, "@MatStatus", sMatStatus.Trim());
                this.dbManger.AddParameters(10, "@DateFormat", sDateFormat.Trim());
                this.dbManger.AddParameters(11, "@PrintSection", sPrintSection.Trim());
                this.dbManger.AddParameters(12, "@LocationType", sLocType.Trim());
                this.dbManger.AddParameters(13, "@CreatedBy", VariableInfo.mAppUserID.Trim().ToString());
                this.dbManger.AddParameters(14, "@PONumber", sPONo.Trim().ToString());
                this.dbManger.AddParameters(15, "@VendorCode", sVendorCode.Trim().ToString());
                this.dbManger.AddParameters(16, "@LabelType", oLabelType.Trim().ToString());
                this.dbManger.AddParameters(17, "@VBatchNo", sBatchNo.Trim().ToString());
                oStatus = Convert.ToString(this.dbManger.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster"));
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
            return oStatus;
        }

        public string DLVSaveStackQRCode(string objLocationCode, string objMatCode, string objStackQRCode, string sDateFormat, string sPrintingSection, string sLocationType)
        {
            string oStatus = "";
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(9);
                this.dbManger.AddParameters(0, "@Type", "SAVESTACKQRCODE");
                this.dbManger.AddParameters(1, "@LocationCode", objLocationCode);
                this.dbManger.AddParameters(2, "@MatCode", objMatCode.Trim());
                this.dbManger.AddParameters(3, "@StackQRCode", objStackQRCode.Trim());
                this.dbManger.AddParameters(4, "@DateFormat", sDateFormat.Trim());
                this.dbManger.AddParameters(5, "@PrintSection", sPrintingSection.Trim());
                this.dbManger.AddParameters(6, "@LocationType", sLocationType.Trim());
                this.dbManger.AddParameters(7, "@MatStatus", "E");
                this.dbManger.AddParameters(8, "@CreatedBy", VariableInfo.mAppUserID.Trim().ToString());
                oStatus = Convert.ToString(this.dbManger.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster"));
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
            return oStatus;
        }

        public string DLVUpdateQRCodeSAPStatus(string sLocationCode, string sMatCode, string sQRCode, string sStatus, string sPostMsg)
        {
            string oStatus = "";
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(6);
                this.dbManger.AddParameters(0, "@Type", "UPDATESAPPOSTEDSTATUS");
                this.dbManger.AddParameters(1, "@LocationCode", sLocationCode);
                this.dbManger.AddParameters(2, "@MatCode", sMatCode.Trim());
                this.dbManger.AddParameters(3, "@QRCode", sQRCode.Trim());
                this.dbManger.AddParameters(4, "@MatStatus", sStatus.Trim());
                this.dbManger.AddParameters(5, "@SAPPostMsg", sPostMsg.Trim());
                oStatus = Convert.ToString(this.dbManger.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster"));
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
            return oStatus;
        }

        

        #endregion

        #region Door/Ply

        public DataTable DLGetMatProduct(string ProductType)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(3);
                dbManger.AddParameters(0, "@Type", "GETMATPRODUCT");
                dbManger.AddParameters(1, "@Product", ProductType.Trim());
                dbManger.AddParameters(2, "@LocationCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLGetMatCategory(string objProduct)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(3);
                dbManger.AddParameters(0, "@Type", "GETMATCATEGORY");
                dbManger.AddParameters(1, "@Product", objProduct);
                dbManger.AddParameters(2, "@LocationCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLGetMatGrade(string objProduct, string objCat)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(4);
                dbManger.AddParameters(0, "@Type", "GETMATGRADE");
                dbManger.AddParameters(1, "@Product", objProduct.Trim());
                dbManger.AddParameters(2, "@Category", objCat.Trim());
                dbManger.AddParameters(3, "@LocationCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLGetMatGroup(string objProduct, string objCat, string objGrade)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(5);
                dbManger.AddParameters(0, "@Type", "GETMATGROUP");
                dbManger.AddParameters(1, "@Product", objProduct.Trim());
                dbManger.AddParameters(2, "@Category", objCat.Trim());
                dbManger.AddParameters(3, "@Grade", objGrade.Trim());
                dbManger.AddParameters(4, "@LocationCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLGetMatThickness(string objProduct, string objCat, string objGrade, string objGroup)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(6);
                dbManger.AddParameters(0, "@Type", "GETMATTHICKNESS");
                dbManger.AddParameters(1, "@Product", objProduct);
                dbManger.AddParameters(2, "@Category", objCat);
                dbManger.AddParameters(3, "@Grade", objGrade);
                dbManger.AddParameters(4, "@Group", objGroup);
                dbManger.AddParameters(5, "@LocationCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLGetMatSize(string objProduct, string objCat, string objGrade, string objGroup, string objThickness)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(7);
                dbManger.AddParameters(0, "@Type", "GETMATSIZE");
                dbManger.AddParameters(1, "@Product", objProduct);
                dbManger.AddParameters(2, "@Category", objCat);
                dbManger.AddParameters(3, "@Grade", objGrade);
                dbManger.AddParameters(4, "@Group", objGroup);
                dbManger.AddParameters(5, "@Thickness", objThickness);
                dbManger.AddParameters(6, "@LocationCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLGetMatDesign(string objProduct, string objCat, string objGrade, string objGroup, string objThickness, string objSize)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(8);
                dbManger.AddParameters(0, "@Type", "GETMATDESIGNNO");
                dbManger.AddParameters(1, "@Product", objProduct);
                dbManger.AddParameters(2, "@Group", objGroup);
                dbManger.AddParameters(3, "@Grade", objGrade);
                dbManger.AddParameters(4, "@Category", objCat);
                dbManger.AddParameters(5, "@Thickness", objThickness);
                dbManger.AddParameters(6, "@Size", objSize);
                dbManger.AddParameters(7, "@LocationCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLGetMatFinishCode(string objProduct, string objCat, string objGrade, string objGroup, string objThickness, string objSize, string objDesignNo)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(9);
                dbManger.AddParameters(0, "@Type", "GETMATFINISHCODE");
                dbManger.AddParameters(1, "@Product", objProduct);
                dbManger.AddParameters(2, "@Category", objCat);
                dbManger.AddParameters(3, "@Grade", objGrade);
                dbManger.AddParameters(4, "@Group", objGroup);
                dbManger.AddParameters(5, "@Thickness", objThickness);
                dbManger.AddParameters(6, "@Size", objSize);
                dbManger.AddParameters(7, "@DesignNo", objDesignNo);
                dbManger.AddParameters(8, "@LocationCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLGetMatVisionCode(string objProduct, string objCat, string objGrade, string objGroup, string objThickness, string objSize, string objDesignNo, string objFinishCode)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(10);
                dbManger.AddParameters(0, "@Type", "GETMATVISIONPANELCODE");
                dbManger.AddParameters(1, "@Product", objProduct);
                dbManger.AddParameters(2, "@Category", objCat);
                dbManger.AddParameters(3, "@Grade", objGrade);
                dbManger.AddParameters(4, "@Group", objGroup);
                dbManger.AddParameters(5, "@Thickness", objThickness);
                dbManger.AddParameters(6, "@Size", objSize);
                dbManger.AddParameters(7, "@DesignNo", objDesignNo);
                dbManger.AddParameters(8, "@FinishCode", objFinishCode);
                dbManger.AddParameters(9, "@LocationCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLGetMatLippingCode(string objProduct, string objGroup, string objGrade, string objCat, string objThickness, string objSize, string objDesignNo, string objFinishCode, string objVisionCode)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(11);
                dbManger.AddParameters(0, "@Type", "GETMATLIPPINGCODE");
                dbManger.AddParameters(1, "@Product", objProduct);
                dbManger.AddParameters(2, "@Category", objCat);
                dbManger.AddParameters(3, "@Grade", objGrade);
                dbManger.AddParameters(4, "@Group", objGroup);
                dbManger.AddParameters(5, "@Thickness", objThickness);
                dbManger.AddParameters(6, "@Size", objSize);
                dbManger.AddParameters(7, "@DesignNo", objDesignNo);
                dbManger.AddParameters(8, "@FinishCode", objFinishCode);
                dbManger.AddParameters(9, "@VisionCode", objVisionCode);
                dbManger.AddParameters(10, "@LocationCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLGetSelectedMatCode(string objProduct, string objCat, string objGrade, string objGroup, string objThickness, string objSize, string objDesignNo, string objFinishCode, string objVisionCode, string objLippingCode)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(12);
                dbManger.AddParameters(0, "@Type", "GETSELECTEDMATERIALDATA");
                dbManger.AddParameters(1, "@Product", objProduct.Trim());
                dbManger.AddParameters(2, "@Group", objGroup.Trim());
                dbManger.AddParameters(3, "@Grade", objGrade.Trim());
                dbManger.AddParameters(4, "@Category", objCat.Trim());
                dbManger.AddParameters(5, "@Thickness", objThickness.Trim());
                dbManger.AddParameters(6, "@Size", objSize.Trim());
                dbManger.AddParameters(7, "@DesignNo", objDesignNo.Trim());
                dbManger.AddParameters(8, "@FinishCode", objFinishCode.Trim());
                dbManger.AddParameters(9, "@VisionCode", objVisionCode.Trim());
                dbManger.AddParameters(10, "@LippingCode", objLippingCode.Trim());
                dbManger.AddParameters(11, "@LocationCode", VariableInfo.mPlantCode.Trim());
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLSaveSelectedMatCode(string objMatCode, string objMatDesc, string objMatSize, string objMatThickness, int LotSize)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(6);
                dbManger.AddParameters(0, "@Type", "UPDATESELECTEDMATERIALCODESTATUS");
                dbManger.AddParameters(1, "@MatCode", objMatCode);
                dbManger.AddParameters(2, "@MatDesc", objMatDesc);
                dbManger.AddParameters(3, "@Size", objMatSize);
                dbManger.AddParameters(4, "@Thickness", objMatThickness);
                dbManger.AddParameters(5, "@LotSize", LotSize);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public string DLSaveHubQRCode(string objLocationCode, string objMatCode, string sMatDesc, string sGrade, string sGroup, string sThickness, string sSize, string objQRCode, string sMatStatus, string sDateFormat, string sPrintSection, string sLocType, string sPONo, string oVendorCode, string oLabelType)
        {
            string oStatus = "";
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(12);
                this.dbManger.AddParameters(0, "@Type", "SAVEQRCODE");
                this.dbManger.AddParameters(1, "@LocationCode", objLocationCode);
                this.dbManger.AddParameters(2, "@MatCode", objMatCode.Trim());
                //this.dbManger.AddParameters(3, "@MatDesc", sMatDesc.Trim());
                //this.dbManger.AddParameters(4, "@Grade", sGrade.Trim());
                //this.dbManger.AddParameters(5, "@Group", sGroup.Trim());
                //this.dbManger.AddParameters(6, "@Thickness", sThickness.Trim());
                //this.dbManger.AddParameters(7, "@Size", sSize.Trim());
                this.dbManger.AddParameters(3, "@QRCode", objQRCode.Trim());
                this.dbManger.AddParameters(4, "@MatStatus", sMatStatus.Trim());
                this.dbManger.AddParameters(5, "@DateFormat", sDateFormat.Trim());
                this.dbManger.AddParameters(6, "@PrintSection", sPrintSection.Trim());
                this.dbManger.AddParameters(7, "@LocationType", sLocType.Trim());
                this.dbManger.AddParameters(8, "@CreatedBy", VariableInfo.mAppUserID.Trim().ToString());
                this.dbManger.AddParameters(9, "@PONumber", sPONo.Trim().ToString());
                this.dbManger.AddParameters(10, "@VendorCode", oVendorCode.Trim().ToString());
                this.dbManger.AddParameters(11, "@LabelType", oLabelType.Trim().ToString());
                oStatus = Convert.ToString(this.dbManger.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster"));
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
            return oStatus;
        }

        public string DLSaveStackQRCode(string objLocationCode, string objMatCode, string objStackQRCode, string sDateFormat, string sPrintingSection, string sLocationType, string MatStatus)
        {
            string oStatus = "";
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(9);
                this.dbManger.AddParameters(0, "@Type", "SAVESTACKQRCODE");
                this.dbManger.AddParameters(1, "@LocationCode", objLocationCode);
                this.dbManger.AddParameters(2, "@MatCode", objMatCode.Trim());
                this.dbManger.AddParameters(3, "@StackQRCode", objStackQRCode.Trim());
                this.dbManger.AddParameters(4, "@DateFormat", sDateFormat.Trim());
                this.dbManger.AddParameters(5, "@PrintSection", sPrintingSection.Trim());
                this.dbManger.AddParameters(6, "@LocationType", sLocationType.Trim());
                this.dbManger.AddParameters(7, "@MatStatus", MatStatus.ToString());
                this.dbManger.AddParameters(8, "@CreatedBy", VariableInfo.mAppUserID.Trim().ToString());
                oStatus = Convert.ToString(this.dbManger.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster"));
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
            return oStatus;
        }

        public string DLUpdateQRCodeSAPStatus(string sLocationCode, string sMatCode, string sQRCode, string sStatus, string sPostMsg)
        {
            string oStatus = "";
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(6);
                this.dbManger.AddParameters(0, "@Type", "UPDATESAPPOSTEDSTATUS");
                this.dbManger.AddParameters(1, "@LocationCode", sLocationCode);
                this.dbManger.AddParameters(2, "@MatCode", sMatCode.Trim());
                this.dbManger.AddParameters(3, "@QRCode", sQRCode.Trim());
                this.dbManger.AddParameters(4, "@MatStatus", sStatus.Trim());
                this.dbManger.AddParameters(5, "@SAPPostMsg", sPostMsg.Trim());
                oStatus = Convert.ToString(this.dbManger.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster"));
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
            return oStatus;
        }

        public string DLSavePRDStackQRCode(string objLocationCode, string objStackQRCode, string sDateFormat, string sPrintingSection, string sLocationType)
        {
            string oStatus = "";
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(7);
                this.dbManger.AddParameters(0, "@Type", "SAVEPRDSTACKQRCODE");
                this.dbManger.AddParameters(1, "@LocationCode", objLocationCode);
                this.dbManger.AddParameters(2, "@StackQRCode", objStackQRCode.Trim());
                this.dbManger.AddParameters(3, "@DateFormat", sDateFormat.Trim());
                this.dbManger.AddParameters(4, "@PrintSection", sPrintingSection.Trim());
                this.dbManger.AddParameters(5, "@LocationType", sLocationType.Trim());
                this.dbManger.AddParameters(6, "@CreatedBy", VariableInfo.mAppUserID.Trim().ToString());
                oStatus = Convert.ToString(this.dbManger.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster"));
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
            return oStatus;
        }

        #endregion


        // get MatGRoups For Labels Printing
        public DataTable DLGetUnbrandedMatGroups(string oLocationCode)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(2);
                dbManger.AddParameters(0, "@Type", "GETUNBRANDEDMATGROUPS");
                dbManger.AddParameters(1, "@LocationCode", oLocationCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
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

        public string DLVGetQRCodeRunningSerialNo(string sDateFormat, string sPrintingSection, string sLocationType)
        {
            string sResult = string.Empty;
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(5);
                dbManger.AddParameters(0, "@Type", "GETQRCODERUNNINGSERIALNO");
                dbManger.AddParameters(1, "@DateFormat", sDateFormat.Trim());
                dbManger.AddParameters(2, "@LocationCode", VariableInfo.mPlantCode.Trim());
                dbManger.AddParameters(3, "@PrintSection", sPrintingSection.Trim());
                dbManger.AddParameters(4, "@LocationType", sLocationType.Trim());
                sResult = Convert.ToString(this.dbManger.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster"));
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return sResult;
        }

        public string DLVGetStackRunningSerialNo(string sDateFormat, string sPrintingSection, string sLocationType)
        {
            string sResult = string.Empty;
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(5);
                dbManger.AddParameters(0, "@Type", "GETSTACKRUNNINGSERIALNO");
                dbManger.AddParameters(1, "@DateFormat", sDateFormat.Trim());
                dbManger.AddParameters(2, "@LocationCode", VariableInfo.mPlantCode.Trim());
                dbManger.AddParameters(3, "@PrintSection", sPrintingSection.Trim());
                dbManger.AddParameters(4, "@LocationType", sLocationType.Trim());
                sResult = Convert.ToString(this.dbManger.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster"));
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return sResult;
        }


        #region Reprinting

        public DataTable DLGetPrintedStackQRCodesData(PLReprinting _plAcDetails)
        {
            DataTable dt = new DataTable();
            string strOutParm = string.Empty;
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(4);
                dbManger.AddParameters(0, "@Type", "GETPRINTEDSTACKQRCODES");
                dbManger.AddParameters(1, "@LocationCode", _plAcDetails.LocationCode);
                dbManger.AddParameters(2, "@FromDate", _plAcDetails.Fromdate.Trim().ToString()); //DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd"));
                //dbManger.AddParameters(3, "@ToDate", _plAcDetails.Todate.Trim().ToString()); //DateTime.Now.ToString("yyyy-MM-dd")); VariableInfo.mAppUserID
                dbManger.AddParameters(3, "@CreatedBy", VariableInfo.mAppUserID);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public ObservableCollection<PLReprinting> DLGetSelectedStackDetails(PLReprinting _PLVMaster)
        {
            try
            {
                ObservableCollection<PLReprinting> _objPLReprint = new ObservableCollection<PLReprinting>();
                this.dbManger.Open();
                this.dbManger.CreateParameters(4);
                this.dbManger.AddParameters(0, "@Type", "GETSELECTEDSTACKQRCODEDETAILS");
                this.dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                this.dbManger.AddParameters(2, "@StackQRCode", _PLVMaster.StackQRCode);
                this.dbManger.AddParameters(3, "@CreatedBy", VariableInfo.mAppUserID);
                IDataReader dataReader = dbManger.ExecuteReader(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster");
                while (dataReader.Read())
                {
                    _objPLReprint.Add(new PLReprinting
                    {
                        IsValid = false,
                        MatCode = Convert.ToString(dataReader["MatCode"]),
                        MatDescription = Convert.ToString(dataReader["MatDesc"]),
                        Grade = Convert.ToString(dataReader["MatGrade"]),
                        GradeDescription = Convert.ToString(dataReader["GradeDescription"]),
                        MatGroup = Convert.ToString(dataReader["MatGroup"]),
                        MatGroupDescription = Convert.ToString(dataReader["MatGroupDescription"]),
                        Thickness = Convert.ToString(dataReader["MatThickness"]),
                        ThicknessDescription = Convert.ToString(dataReader["ThicknessDescription"]),
                        Size = Convert.ToString(dataReader["MatSize"]),
                        Category = Convert.ToString(dataReader["Category"]),
                        DesignNo = Convert.ToString(dataReader["DesignNo"]),
                        FinishCode = Convert.ToString(dataReader["FinishCode"]),
                        VisionPanelCode = Convert.ToString(dataReader["VisionPanelCode"]),
                        LippingCode = Convert.ToString(dataReader["LippingCode"]),
                        QRCode = Convert.ToString(dataReader["QRCode"]),
                        StackQRCode = Convert.ToString(dataReader["StackQRCode"]),
                        IsSAPPosted = Convert.ToString(dataReader["IsSAPPosted"]),
                        QRCodeCount = Convert.ToString(dataReader["QRCodeCount"]),
                    });
                }
                return _objPLReprint;
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

        public ObservableCollection<PLReprinting> DLGetDecorSelectedStackDetails(PLReprinting _PLVMaster)
        {
            try
            {
                ObservableCollection<PLReprinting> _objPLReprint = new ObservableCollection<PLReprinting>();
                this.dbManger.Open();
                this.dbManger.CreateParameters(4);
                this.dbManger.AddParameters(0, "@Type", "GETSELECTEDDECORSTACKQRCODEDETAILS");
                this.dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                this.dbManger.AddParameters(2, "@StackQRCode", _PLVMaster.StackQRCode);
                this.dbManger.AddParameters(3, "@CreatedBy", VariableInfo.mAppUserID);
                IDataReader dataReader = dbManger.ExecuteReader(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster");
                while (dataReader.Read())
                {
                    _objPLReprint.Add(new PLReprinting
                    {
                        IsValid = false,
                        MatCode = Convert.ToString(dataReader["MatCode"]),
                        MatDescription = Convert.ToString(dataReader["MatDesc"]),
                        Grade = Convert.ToString(dataReader["MatGrade"]),
                        GradeDescription = Convert.ToString(dataReader["GradeDescription"]),
                        MatGroup = Convert.ToString(dataReader["MatGroup"]),
                        MatGroupDescription = Convert.ToString(dataReader["MatGroupDescription"]),
                        Thickness = Convert.ToString(dataReader["MatThickness"]),
                        ThicknessDescription = Convert.ToString(dataReader["ThicknessDescription"]),
                        Size = Convert.ToString(dataReader["MatSize"]),
                        Category = Convert.ToString(dataReader["Category"]),
                        DesignNo = Convert.ToString(dataReader["DesignNo"]),
                        FinishCode = Convert.ToString(dataReader["FinishCode"]),
                        VisionPanelCode = Convert.ToString(dataReader["VisionPanelCode"]),
                        LippingCode = Convert.ToString(dataReader["LippingCode"]),
                        QRCode = Convert.ToString(dataReader["QRCode"]),
                        StackQRCode = Convert.ToString(dataReader["StackQRCode"]),
                        IsSAPPosted = Convert.ToString(dataReader["IsSAPPosted"]),
                        BatchNo = Convert.ToString(dataReader["BatchNo"]),
                        QRCodeCount = Convert.ToString(dataReader["QRCodeCount"]),
                    });
                }
                return _objPLReprint;
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

        public DataTable DLGetSelectedStackDetails(string StackQRCode)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(3);
                dbManger.AddParameters(0, "@Type", "GETSELECTEDSTACKQRCODEDETAILS");
                dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                dbManger.AddParameters(2, "@StackQRCode", StackQRCode.Trim());
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        #endregion


        #region HUB PO Reprinting

        public DataTable DLGetPrintedHUBPOData(PLReprinting _plAcDetails)
        {
            DataTable dt = new DataTable();
            string strOutParm = string.Empty;
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(5);
                dbManger.AddParameters(0, "@Type", "GETPRINTEDHUBPOs");
                dbManger.AddParameters(1, "@LocationCode", _plAcDetails.LocationCode);
                dbManger.AddParameters(2, "@FromDate", _plAcDetails.Fromdate.Trim().ToString()); 
                dbManger.AddParameters(3, "@ToDate", _plAcDetails.Todate.Trim().ToString());
                dbManger.AddParameters(4, "@CreatedBy", VariableInfo.mAppUserID);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public DataTable DLGetSelectedPOMatDetails(string sPONO)
        {
            DataTable dt = new DataTable();
            string strOutParm = string.Empty;
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(4);
                dbManger.AddParameters(0, "@Type", "GETSELECTEDPOMATCODES");
                dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode.ToString());
                dbManger.AddParameters(2, "@PONumber", sPONO);
                dbManger.AddParameters(3, "@CreatedBy", VariableInfo.mAppUserID);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
            }
            catch (Exception ex)
            {
                dbManger.Close();
                throw ex;
            }
            finally
            {
                dbManger.Close();
            }
            return dt;
        }

        public ObservableCollection<PLReprinting> DLGetSelectedMatDetails(PLReprinting _PLVMaster)
        {
            try
            {
                ObservableCollection<PLReprinting> _objPLReprint = new ObservableCollection<PLReprinting>();
                this.dbManger.Open();
                this.dbManger.CreateParameters(5);
                this.dbManger.AddParameters(0, "@Type", "GETSELECTEDMATCODEDETAILS");
                this.dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                this.dbManger.AddParameters(2, "@PONumber", _PLVMaster.PONumber);
                this.dbManger.AddParameters(3, "@MatCode", _PLVMaster.MatCode);
                this.dbManger.AddParameters(4, "@CreatedBy", VariableInfo.mAppUserID);
                IDataReader dataReader = dbManger.ExecuteReader(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster");
                while (dataReader.Read())
                {
                    _objPLReprint.Add(new PLReprinting
                    {
                        IsValid = false,
                        MatCode = Convert.ToString(dataReader["MatCode"]),
                        MatDescription = Convert.ToString(dataReader["MatDesc"]),
                        Grade = Convert.ToString(dataReader["MatGrade"]),
                        GradeDescription = Convert.ToString(dataReader["GradeDescription"]),
                        MatGroup = Convert.ToString(dataReader["MatGroup"]),
                        MatGroupDescription = Convert.ToString(dataReader["MatGroupDescription"]),
                        Thickness = Convert.ToString(dataReader["MatThickness"]),
                        ThicknessDescription = Convert.ToString(dataReader["ThicknessDescription"]),
                        Size = Convert.ToString(dataReader["MatSize"]),
                        Category = Convert.ToString(dataReader["Category"]),
                        CategoryDescription = Convert.ToString(dataReader["CategoryDescription"]),
                        DesignNo = Convert.ToString(dataReader["DesignNo"]),
                        FinishCode = Convert.ToString(dataReader["FinishCode"]),
                        VisionPanelCode = Convert.ToString(dataReader["VisionPanelCode"]),
                        LippingCode = Convert.ToString(dataReader["LippingCode"]),
                        QRCode = Convert.ToString(dataReader["QRCode"]),
                        StackQRCode = Convert.ToString(dataReader["StackQRCode"]),
                        //IsSAPPosted = Convert.ToString(dataReader["IsSAPPosted"]),
                        QRCodeCount = Convert.ToString(dataReader["QRCodeCount"]),
                    });
                }
                return _objPLReprint;
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

        #endregion


        #region RejectionMaster

        public ObservableCollection<PLReprinting> DLGetRejectionDetails()
        {
            try
            {
                ObservableCollection<PLReprinting> _objPLReprint = new ObservableCollection<PLReprinting>();
                this.dbManger.Open();
                this.dbManger.CreateParameters(1);
                this.dbManger.AddParameters(0, "@Type", "SELECT");
                IDataReader dataReader = dbManger.ExecuteReader(System.Data.CommandType.StoredProcedure, "USP_RejectionMaster");
                while (dataReader.Read())
                {
                    _objPLReprint.Add(new PLReprinting
                    {
                        IsValid = false,
                        RejCode = Convert.ToString(dataReader["RejCode"]),
                        RejDescription = Convert.ToString(dataReader["RejDescription"]),
                    });
                }
                return _objPLReprint;
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

    #endregion
    }
}
