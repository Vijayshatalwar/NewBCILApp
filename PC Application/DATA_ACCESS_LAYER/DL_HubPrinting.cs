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
    public class DL_HubPrinting : DlCommon
    {
        DBManager dbManger = null;
        DlCommon dCommon = null;
        DataTable dt = null;
        StringBuilder sb = null;

        public DL_HubPrinting()
        {
            this.dCommon = new DlCommon();
            this.dbManger = DBProvider();
        }

        #region Vendor Barcode generation

        public DataTable DLGetSAPPONumbers()
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(2);
                dbManger.AddParameters(0, "@Type", "GETSAPPONUMBER");
                dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode.Trim().ToString());
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_LocationPrinting").Tables[0];
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

        public DataTable DLGetSelectedPOVendorDetails(string PONum)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(3);
                dbManger.AddParameters(0, "@Type", "GETSELECTEDPOVENDORDETAILS");
                dbManger.AddParameters(1, "@PONo", PONum.Trim());
                dbManger.AddParameters(2, "@LocationCode", VariableInfo.mPlantCode);
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_LocationPrinting").Tables[0];
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

        public ObservableCollection<PL_HubPrinting> DLGetSelectedPOMatData(PL_HubPrinting _PLVMaster)
        {
            try
            {
                ObservableCollection<PL_HubPrinting> _objPLVendor = new ObservableCollection<PL_HubPrinting>();
                this.dbManger.Open();
                this.dbManger.CreateParameters(3);
                this.dbManger.AddParameters(0, "@Type", "GETPOMATDETAILS");
                this.dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                this.dbManger.AddParameters(2, "@PONo", _PLVMaster.PONumber);
                IDataReader dataReader = dbManger.ExecuteReader(System.Data.CommandType.StoredProcedure, "USP_LocationPrinting");
                while (dataReader.Read())
                {
                    _objPLVendor.Add(new PL_HubPrinting
                    {
                        IsValid = false,
                        MatCode = Convert.ToString(dataReader["MatCode"]),
                        MatDesc = Convert.ToString(dataReader["MatDescription"]),
                        MatThickness = Convert.ToString(dataReader["MatThickness"]),
                        MatSize = Convert.ToString(dataReader["MatSize"]),
                        MatGrade = Convert.ToString(dataReader["MatGrade"]),
                        Category = Convert.ToString(dataReader["Category"]),
                        MatGroup = Convert.ToString(dataReader["MatGroup"]),
                        POQty = Convert.ToInt32(dataReader["POQty"]),
                        PrintedQty = Convert.ToInt32(dataReader["PrintedQty"]),
                        RemaningQty = Convert.ToInt32(dataReader["RemainingQty"]),
                    });
                }
                return _objPLVendor;
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

        public OperationResult SaveGeneratedQRCode(PL_HubPrinting objVMaster)
        {
            OperationResult oPeration = OperationResult.SaveError;
            DataTable DT = new DataTable();
            try
            {
                if (!this.CheckQRCodeDuplicate(objVMaster))
                {
                    this.dbManger.Open();
                    this.dbManger.CreateParameters(16);
                    this.dbManger.AddParameters(0, "@Type", "SAVEGENERATEDQRCODE");
                    this.dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                    this.dbManger.AddParameters(2, "@PONo", objVMaster.PONumber);
                    this.dbManger.AddParameters(3, "@MatCode", objVMaster.MatCode);
                    this.dbManger.AddParameters(4, "@MatDesc", objVMaster.MatDesc);
                    this.dbManger.AddParameters(5, "@MatGrade", objVMaster.MatGrade);
                    this.dbManger.AddParameters(6, "@MatGroup", objVMaster.MatGroup);
                    this.dbManger.AddParameters(7, "@MatThickness", objVMaster.MatThickness);
                    this.dbManger.AddParameters(8, "@MatSize", objVMaster.MatSize);
                    this.dbManger.AddParameters(9, "@Vendorcode", objVMaster.VendorId);
                    this.dbManger.AddParameters(10, "@QRCode", objVMaster.QRCode);
                    this.dbManger.AddParameters(11, "@CreatedBy", objVMaster.CreatedBy);
                    this.dbManger.AddParameters(12, "@PrintedQty", objVMaster.PrintedQty);
                    this.dbManger.AddParameters(13, "@DateFormat", objVMaster.DateFormat);
                    this.dbManger.AddParameters(14, "@LocationType", objVMaster.PrintingLocationType);
                    this.dbManger.AddParameters(15, "@PrintSection", objVMaster.PrintingSection);
                    DT = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_LocationPrinting").Tables[0];
                    //int Result = dbManger.ExecuteNonQuery(System.Data.CommandType.StoredProcedure, "USP_VendorDetails");
                    //if (Result > 0)
                    if (DT.Columns.Contains("STATUS") && DT.Rows.Count > 0)
                        oPeration = OperationResult.SaveSuccess;
                    else if (DT.Columns.Contains("ERROR") && DT.Rows.Count > 0)
                        oPeration = OperationResult.SaveError;
                }
                else
                    oPeration = OperationResult.Duplicate;
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

        #endregion


        #region Vendor labels printing at HUB site

        public DataTable DLGetSAPHubPONumbers()
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(2);
                dbManger.AddParameters(0, "@Type", "GETSAPHUBPONUMBER");
                dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode.Trim().ToString());
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_LocationPrinting").Tables[0];
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

        public DataTable DLPrintGetHubPOVendorDetails(string PONum)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(2);
                dbManger.AddParameters(0, "@Type", "GETHUBPOVENDORDETAILS");
                dbManger.AddParameters(1, "@PONo", PONum.Trim());
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_LocationPrinting").Tables[0];
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

        public ObservableCollection<PL_HubPrinting> DLPrintGetHUBPOMatData(PL_HubPrinting _PLVMaster)
        {
            try
            {
                ObservableCollection<PL_HubPrinting> _objPLVendor = new ObservableCollection<PL_HubPrinting>();
                this.dbManger.Open();
                this.dbManger.CreateParameters(3);
                this.dbManger.AddParameters(0, "@Type", "GETSELECTEDPOMATDETAILS");
                this.dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                this.dbManger.AddParameters(2, "@PONo", _PLVMaster.PONumber);
                IDataReader dataReader = dbManger.ExecuteReader(System.Data.CommandType.StoredProcedure, "USP_LocationPrinting");
                while (dataReader.Read())
                {
                    _objPLVendor.Add(new PL_HubPrinting
                    {
                        IsValid = false,
                        MatCode = Convert.ToString(dataReader["MatCode"]),
                        MatDesc = Convert.ToString(dataReader["MatDescription"]),
                        MatThickness = Convert.ToString(dataReader["MatThickness"]),
                        MatThicknessDesc = Convert.ToString(dataReader["MatThicknessDesc"]),
                        MatSize = Convert.ToString(dataReader["MatSize"]),
                        MatGrade = Convert.ToString(dataReader["MatGrade"]),
                        MatGroup = Convert.ToString(dataReader["MatGroup"]),
                        MatGroupDesc = Convert.ToString(dataReader["MatGroupDescription"]),
                        POQty = Convert.ToInt32(dataReader["POQty"]),
                        PrintedQty = Convert.ToInt32(dataReader["PrintedQty"]),
                        RemaningQty = Convert.ToInt32(dataReader["RemainingQty"]),
                        MatGradeDesc = Convert.ToString(dataReader["MatGradeDesc"]),
                        //InvoiceDate = Convert.ToString(dataReader["MatGroup"]),
                    });
                }
                return _objPLVendor;
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

        public DataTable DLGetQRCodeRunningSerialNo(string sDateFormat, string sPrintingSaction, string sLocationType)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(5);
                dbManger.AddParameters(0, "@Type", "GETQRCODERUNNINGSERIALNO");
                dbManger.AddParameters(1, "@DateFormat", sDateFormat.Trim());
                dbManger.AddParameters(2, "@LocationCode", VariableInfo.mPlantCode.Trim());
                dbManger.AddParameters(3, "@PrintSection", sPrintingSaction.Trim().ToString());
                dbManger.AddParameters(4, "@LocationType", sLocationType.Trim().ToString());
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_LocationPrinting").Tables[0];
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

        public string DLGetStackRunningSerialNo(string sDateFormat, string sPrintingSection, string sLocationType)
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

        public OperationResult SaveHUBQRCode(PL_HubPrinting objHub)
        {
            OperationResult oPeration = OperationResult.SaveError;
            DataTable DT = new DataTable();
            try
            {
                if (!this.CheckQRCodeDuplicate(objHub))
                {
                    this.dbManger.Open();
                    this.dbManger.CreateParameters(14);
                    this.dbManger.AddParameters(0, "@Type", "SAVEQRCODE");
                    this.dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                    this.dbManger.AddParameters(2, "@PONo", objHub.PONumber);
                    this.dbManger.AddParameters(3, "@MatCode", objHub.MatCode);
                    this.dbManger.AddParameters(4, "@MatStatus", objHub.MatStatus);
                    this.dbManger.AddParameters(5, "@QRCode", objHub.QRCode);
                    this.dbManger.AddParameters(6, "@CreatedBy", objHub.CreatedBy);
                    this.dbManger.AddParameters(7, "@PrintedQty", objHub.PrintedQty);
                    this.dbManger.AddParameters(8, "@DateFormat", objHub.DateFormat);
                    this.dbManger.AddParameters(9, "@LocationType", objHub.PrintingLocationType);
                    this.dbManger.AddParameters(10, "@PrintSection", objHub.PrintingSection);
                    this.dbManger.AddParameters(11, "@VendorInv", objHub.VendorInvoiceNo);
                    this.dbManger.AddParameters(12, "@VendorInvDate", objHub.VendorInvoiceDate);
                    this.dbManger.AddParameters(13, "@VendorCode", objHub.VendorCode);

                    DT = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_LocationPrinting").Tables[0];
                    if (DT.Columns.Contains("STATUS") && DT.Rows.Count > 0)
                        oPeration = OperationResult.SaveSuccess;
                    else if (DT.Columns.Contains("ERROR") && DT.Rows.Count > 0)
                        oPeration = OperationResult.SaveError;
                }
                else
                    oPeration = OperationResult.Duplicate;
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

        #endregion

        

        

       

        private bool CheckQRCodeDuplicate(PL_HubPrinting objHub)
        {
            bool isDuplicate = false;
            DataTable dtQrcode = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(5);
                this.dbManger.AddParameters(0, "@Type", "CHECKDUPLICATEQRCODE");
                this.dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                this.dbManger.AddParameters(2, "@PONo", objHub.PONumber);
                this.dbManger.AddParameters(3, "@MatCode", objHub.MatCode);
                this.dbManger.AddParameters(4, "@QRCode", objHub.QRCode);
                dtQrcode = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_LocationPrinting").Tables[0];
                if (dtQrcode.Rows.Count > 0)
                {
                    isDuplicate = true;
                    VariableInfo.sbDuplicateCount.Append(Convert.ToString(objHub) + ",");
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
            return isDuplicate;
        }

        public DataTable BLGetVendorEmailDetails(string VendorCode)
        {
            try
            {
                return new DL_VendorPrinting().DLGetVendorEmailDetails(VendorCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
