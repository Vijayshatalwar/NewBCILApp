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
    public class DL_MaterialMaster : DlCommon
    {
        DBManager dbManger = null;
        DlCommon dCommon = null;

        public DL_MaterialMaster()
        {
            this.dCommon = new DlCommon();
            this.dbManger = DBProvider();
        }

        public ObservableCollection<PL_MaterialMaster> DL_GetCommonMaster(PL_MaterialMaster _PLMaterialMaster)
        {
            try
            {
                ObservableCollection<PL_MaterialMaster> _obj_PlCommonMaster = new ObservableCollection<PL_MaterialMaster>();
                this.dbManger.Open();
                this.dbManger.CreateParameters(1);
                this.dbManger.AddParameters(0, "@Type", "SELECT");
                //this.dbManger.AddParameters(1, "@PlantCode",  VariableInfo.mPlantCode);
                IDataReader dataReader = dbManger.ExecuteReader(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster");
                while (dataReader.Read())
                {
                    _obj_PlCommonMaster.Add(new PL_MaterialMaster
                    {
                        IsValid = false,
                        Product = Convert.ToString(dataReader["Product"]),
                        MatCode = Convert.ToString(dataReader["MatCode"]),
                        MatDescription = Convert.ToString(dataReader["MatDescription"]),
                        Thickness = Convert.ToString(dataReader["Thickness"]),
                        Size = Convert.ToString(dataReader["Size"]),
                        Grade = Convert.ToString(dataReader["Grade"]),
                        GradeDescription = Convert.ToString(dataReader["GradeDescription"]),
                        Category = Convert.ToString(dataReader["Category"]),
                        CategoryDescription = Convert.ToString(dataReader["CategoryDescription"]),
                        MatGroup = Convert.ToString(dataReader["MatGroup"]),
                        MatGroupDescription = Convert.ToString(dataReader["MatGroupDescription"]),
                        DesignNo = Convert.ToString(dataReader["DesignNo"]),
                        DesignDescription = Convert.ToString(dataReader["DesignDescription"]),
                        FinishCode = Convert.ToString(dataReader["FinishCode"]),
                        FinishDescription = Convert.ToString(dataReader["FinishDescription"]),

                        VisionPanelCode = Convert.ToString(dataReader["VisionPanelCode"]),
                        VisionPanelDescription = Convert.ToString(dataReader["VisionPanelDescription"]),
                        LippingCode = Convert.ToString(dataReader["LippingCode"]),
                        LippingDescription = Convert.ToString(dataReader["LippingDescription"]),
                        UOM = Convert.ToString(dataReader["UOM"]),
                    });
                }
                return _obj_PlCommonMaster;
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

        public OperationResult Save(PL_MaterialMaster _objPLMaterialMaster)
        {
            OperationResult oPeration = OperationResult.SaveSuccess;
            DataTable DT = new DataTable();
            try
            {
                if (!this.CheckDuplicate(_objPLMaterialMaster))
                {
                    this.dbManger.Open();
                    this.dbManger.CreateParameters(8);
                    this.dbManger.AddParameters(0, "@Type", "INSERT");
                    this.dbManger.AddParameters(1, "@Product", _objPLMaterialMaster.Product);
                    this.dbManger.AddParameters(2, "@Thickness", _objPLMaterialMaster.Thickness);
                    this.dbManger.AddParameters(3, "@Size", _objPLMaterialMaster.Size);
                    this.dbManger.AddParameters(4, "@MatCode", _objPLMaterialMaster.MatCode);
                    this.dbManger.AddParameters(5, "@MatDesc", _objPLMaterialMaster.MatDescription);                  
                    this.dbManger.AddParameters(6, "@CreatedBy", _objPLMaterialMaster.CreatedBy);
                    this.dbManger.AddParameters(7, "@PlantCode", VariableInfo.mPlantCode);
                    int result = this.dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster");
                    if (result > 0)
                    {
                        oPeration = OperationResult.SaveSuccess;
                    }
                    else
                    {
                        oPeration = OperationResult.SaveSuccess;
                    }
                }
                else
                {
                    oPeration = OperationResult.Duplicate;
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
        public bool CheckDuplicate(PL_MaterialMaster _objPLMaterialMaster)
        {
            bool isDuplicate = false;
            DataTable regionDT = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(3);
                this.dbManger.AddParameters(0, "@Type", "CHECKDUP");
                this.dbManger.AddParameters(1, "@MatCode", _objPLMaterialMaster.MatCode);
                this.dbManger.AddParameters(2, "@PlantCode", VariableInfo.mPlantCode);
                regionDT = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster").Tables[0];
                if (regionDT.Rows.Count > 0)
                {
                    isDuplicate = true;
                    VariableInfo.sbDuplicateCount.Append(Convert.ToString(_objPLMaterialMaster.MatCode) + ",");
                }
                else
                    VariableInfo.sbDuplicateCount.Append(Convert.ToString(_objPLMaterialMaster.MatCode) + ",");
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

        public OperationResult Update(PL_MaterialMaster _objPLMaterialMaster)
        {
            OperationResult oPeration = OperationResult.UpdateError;
            DataTable DT = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(5);
                this.dbManger.AddParameters(0, "@Type", "UPDATE");
                this.dbManger.AddParameters(1, "@MatCode", _objPLMaterialMaster.MatCode);
                this.dbManger.AddParameters(2, "@MatDesc", _objPLMaterialMaster.MatDescription);
                this.dbManger.AddParameters(3, "@CreatedBy", _objPLMaterialMaster.CreatedBy);
                this.dbManger.AddParameters(4, "@PlantCode", VariableInfo.mPlantCode);
                int result = this.dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster");
                if (result > 0)
                {
                    oPeration = OperationResult.UpdateSuccess;
                }
                else
                {
                    oPeration = OperationResult.UpdateError;
                }
                return oPeration;
            }
            catch (Exception ex)
            {

                throw;
            }
            finally
            {
                this.dbManger.Close();
            }

        }
        public OperationResult Delete(PL_MaterialMaster _objPLMaterialMaster)
        {
            OperationResult oPeration = OperationResult.DeleteError;
            DataTable DT = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(2);
                this.dbManger.AddParameters(0, "@Type", "DELETE");
                this.dbManger.AddParameters(1, "@MatCode", _objPLMaterialMaster.MatCode);
                int result = this.dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster");
                if (result > 0)
                {
                    oPeration = OperationResult.DeleteSuccess;
                }

            }
            catch (Exception ex)
            {
                if (ex.ToString().Contains("Reference"))
                {
                    oPeration = OperationResult.DeleteRefference;
                }
                else
                {
                    this.dbManger.Close();
                    throw ex;
                }
            }
            finally
            {
                this.dbManger.Close();
            }
            return oPeration;
        }

    }
}

