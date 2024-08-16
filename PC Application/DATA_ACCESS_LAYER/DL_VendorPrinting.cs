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
    public class DL_VendorPrinting : DlCommon
    {
        DBManager dbManger = null;
        DlCommon dCommon = null;
        DataTable dt = null;
        StringBuilder sb = null;

        public DL_VendorPrinting()
        {
            this.dCommon = new DlCommon();
            this.dbManger = DBProvider();
        }

        #region Vendor Barcode generation

        public DataTable DLGetSAPVendorPONumbers()
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(2);
                dbManger.AddParameters(0, "@Type", "GETSAPVENDORPONUMBER");
                dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode.Trim().ToString());
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_VendorDetails").Tables[0];
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
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_VendorDetails").Tables[0];
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

        public ObservableCollection<PL_VendorMaster> DLGetSelectedPOMatData(PL_VendorMaster _PLVMaster)
        {
            try
            {
                ObservableCollection<PL_VendorMaster> _objPLVendor = new ObservableCollection<PL_VendorMaster>();
                this.dbManger.Open();
                this.dbManger.CreateParameters(3);
                this.dbManger.AddParameters(0, "@Type", "GETSELECTEDPOMATDETAILS");
                this.dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                this.dbManger.AddParameters(2, "@PONo", _PLVMaster.PONumber);
                IDataReader dataReader = dbManger.ExecuteReader(System.Data.CommandType.StoredProcedure, "USP_VendorDetails");
                while (dataReader.Read())
                {
                    _objPLVendor.Add(new PL_VendorMaster
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

        public OperationResult SaveGeneratedQRCode(PL_VendorMaster objVMaster)
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
                    this.dbManger.AddParameters(14, "@LocationType", "VENDOR");  //objVMaster.PrintingLocationType
                    this.dbManger.AddParameters(15, "@PrintSection", objVMaster.PrintingSection);
                    DT = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_VendorDetails").Tables[0];
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

        #region Vendor labels printing at Vendor site

        public DataTable DLGetGeneratedPONumbersPrintingAtVendor()
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(3);
                dbManger.AddParameters(0, "@Type", "GETGENERATEDVENDORPRINTINGPONUMBERS");
                dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode.Trim());
                dbManger.AddParameters(2, "@VendorCode", VariableInfo.mAppUserID.Trim().ToString());
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_VendorDetails").Tables[0];
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

        public DataTable DLGetGeneratedPONumbersPrintingAtHub()
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(2);
                dbManger.AddParameters(0, "@Type", "GETGENERATEDHUBPRINTINGPONUMBERS");
                dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_VendorDetails").Tables[0];
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

        public DataTable DLPrintGetPOVendorDetails(string PONum)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(2);
                dbManger.AddParameters(0, "@Type", "GETPRINTPOVENDORDETAILS");
                dbManger.AddParameters(1, "@PONo", PONum.Trim());
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_VendorDetails").Tables[0];
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

        public ObservableCollection<PL_VendorMaster> DLPrintGetVendorPOMatData(PL_VendorMaster _PLVMaster)
        {
            try
            {
                ObservableCollection<PL_VendorMaster> _objPLVendor = new ObservableCollection<PL_VendorMaster>();
                this.dbManger.Open();
                this.dbManger.CreateParameters(2);
                this.dbManger.AddParameters(0, "@Type", "GETPRINTPOMATDETAILS");
                //this.dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                this.dbManger.AddParameters(1, "@PONo", _PLVMaster.PONumber);
                IDataReader dataReader = dbManger.ExecuteReader(System.Data.CommandType.StoredProcedure, "USP_VendorDetails");
                while (dataReader.Read())
                {
                    _objPLVendor.Add(new PL_VendorMaster
                    {
                        IsValid = false,
                        MatCode = Convert.ToString(dataReader["MatCode"]),
                        MatDesc = Convert.ToString(dataReader["MatDescription"]),
                        MatGrade = Convert.ToString(dataReader["MatGrade"]),
                        MatGroup = Convert.ToString(dataReader["MatGroup"]),
                        MatGroupDesc = Convert.ToString(dataReader["MatGroupDescription"]),
                        MatThickness = Convert.ToString(dataReader["MatThickness"]),
                        MatSize = Convert.ToString(dataReader["MatSize"]),
                        POQty = Convert.ToInt32(dataReader["POQty"]),
                        GeneratedQty = Convert.ToInt32(dataReader["GeneratedQty"]),
                        PrintedQty = Convert.ToInt32(dataReader["PrintedQty"]),
                        InvoiceNo = Convert.ToString(dataReader["MatGroup"]),
                        InvoiceDate = Convert.ToString(dataReader["MatGroup"]),
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

        public DataTable DLPrintGetPrintMatQRCodeDetails(string PONum, string MatCode, string VendorCode)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(5);
                dbManger.AddParameters(0, "@Type", "GETPRINTMATQRCODEDETAILS");
                dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode.Trim());
                dbManger.AddParameters(2, "@PONo", PONum.Trim());
                dbManger.AddParameters(3, "@MatCode", MatCode.Trim());
                dbManger.AddParameters(4, "@VendorCode", VendorCode.Trim());
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_VendorDetails").Tables[0];
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

        public string DLUpdateQRCode(string PONo, string MatCode, string VendorCode, string sInvNo, string sInvDate, string QRCode, string UserId)
        {
            string oStatus = "";
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(9);
                this.dbManger.AddParameters(0, "@Type", "UPDATEQRCODEDETAILS");
                this.dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                this.dbManger.AddParameters(2, "@PONo", PONo.Trim());
                this.dbManger.AddParameters(3, "@MatCode", MatCode.Trim());
                this.dbManger.AddParameters(4, "@VendorCode", VendorCode.Trim());
                this.dbManger.AddParameters(5, "@VendorInv", sInvNo.Trim());
                this.dbManger.AddParameters(6, "@VendorInvDate", sInvDate.Trim());
                this.dbManger.AddParameters(7, "@QRCode", QRCode.Trim());
                this.dbManger.AddParameters(8, "@CreatedBy", UserId.Trim());
                oStatus = Convert.ToString(this.dbManger.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USP_VendorDetails"));
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
                dbManger.AddParameters(4, "@LocationType", "VENDOR");  //sLocationType.Trim().ToString()
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_VendorDetails").Tables[0];
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

        public DataTable DLGetStackRunningSerialNo(string sDateFormat)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(4);
                dbManger.AddParameters(0, "@Type", "GETSTACKRUNNINGSERIALNO");
                dbManger.AddParameters(1, "@DateFormat", sDateFormat.Trim());
                dbManger.AddParameters(2, "@LocationCode", VariableInfo.mPlantCode.Trim());
                dbManger.AddParameters(3, "@PrintType", "Vendor");
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_VendorDetails").Tables[0];
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

        

        private bool CheckQRCodeDuplicate(PL_VendorMaster objVMaster)
        {
            bool isDuplicate = false;
            DataTable dtQrcode = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(4);
                this.dbManger.AddParameters(0, "@Type", "CHECKDUPLICATEQRCODE");
                this.dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                this.dbManger.AddParameters(2, "@MatCode", objVMaster.MatCode);
                this.dbManger.AddParameters(3, "@QRCode", objVMaster.QRCode);
                dtQrcode = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_VendorDetails").Tables[0];
                if (dtQrcode.Rows.Count > 0)
                {
                    isDuplicate = true;
                    VariableInfo.sbDuplicateCount.Append(Convert.ToString(objVMaster) + ",");
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

        public OperationResult SaveStackQRCode(PL_VendorMaster objVMaster)
        {
            OperationResult oPeration = OperationResult.SaveError;
            DataTable DT = new DataTable();
            try
            {
                if (!this.CheckStackQRCodeDuplicate(objVMaster))
                {
                    this.dbManger.Open();
                    this.dbManger.CreateParameters(7);
                    this.dbManger.AddParameters(0, "@Type", "SAVESTACKQRCODE");
                    this.dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode.Trim());
                    this.dbManger.AddParameters(2, "@PONo", objVMaster.PONumber.Trim());
                    this.dbManger.AddParameters(3, "@MatCode", objVMaster.MatCode.Trim());
                    this.dbManger.AddParameters(4, "@StackQRCode", objVMaster.StackQRCCode.Trim());
                    this.dbManger.AddParameters(5, "@DateFormat", objVMaster.DateFormat);
                    this.dbManger.AddParameters(6, "@VendorCode", objVMaster.VendorId);
                    DT = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_VendorDetails").Tables[0];
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

        private bool CheckStackQRCodeDuplicate(PL_VendorMaster objVMaster)
        {
            bool isDuplicate = false;
            DataTable dtQrcode = new DataTable();
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(4);
                this.dbManger.AddParameters(0, "@Type", "CHECKDUPLICATESTACKQRCODE");
                this.dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                this.dbManger.AddParameters(2, "@MatCode", objVMaster.MatCode);
                this.dbManger.AddParameters(3, "@StackQRCode", objVMaster.StackQRCCode);
                dtQrcode = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_VendorDetails").Tables[0];
                if (dtQrcode.Rows.Count > 0)
                {
                    isDuplicate = true;
                    VariableInfo.sbDuplicateCount.Append(Convert.ToString(objVMaster) + ",");
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

        public DataTable DLGetMatStockSize(string sMatCode)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(2);
                dbManger.AddParameters(0, "@Type", "GETMATERIALLOTSIZE");
                dbManger.AddParameters(1, "@MatCode", sMatCode.Trim());
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_VendorDetails").Tables[0];
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

        public DataTable DLGetVendorEmailDetails(string sVendorCode)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(2);
                dbManger.AddParameters(0, "@Type", "GETVENDOREMAILDETAILS");
                dbManger.AddParameters(1, "@VendorCode", sVendorCode.Trim());
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_VendorDetails").Tables[0];
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



        

        

        

        

        

        public string DLUpdateStackQRCode(string PONo, string MatCode, string VendorCode, string VendorInv, string VendorInvDate, string StackQRCode, int TPrintQty)
        {
            string oStatus = "";
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(10);
                string UserId = Convert.ToString(VariableInfo.dtVlidateuser) != "" ? Convert.ToString(VariableInfo.dtVlidateuser.Rows[0]["USER_ID"]) : "";
                this.dbManger.AddParameters(0, "@Type", "UPDATESTACKQRCODEDETAILS");
                this.dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                this.dbManger.AddParameters(2, "@PONo", PONo.Trim());
                this.dbManger.AddParameters(3, "@MatCode", MatCode.Trim());
                this.dbManger.AddParameters(4, "@StackQRCode", StackQRCode.Trim());
                this.dbManger.AddParameters(5, "@VendorCode", VendorCode.Trim());
                this.dbManger.AddParameters(6, "@VendorInv", VendorInv.Trim());
                this.dbManger.AddParameters(7, "@VendorInvDate", VendorInvDate.Trim());
                this.dbManger.AddParameters(8, "@PrintedQty", TPrintQty);
                this.dbManger.AddParameters(9, "@CreatedBy", UserId.Trim());
                oStatus = Convert.ToString(this.dbManger.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USP_VendorDetails"));
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

        public string DLUpdateSAPPostStatus(string LocCode, string PONo, string MatCode, string VendorCode, string QRCode, string Message)
        {
            string oStatus = "";
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(8);
                string UserId = Convert.ToString(VariableInfo.dtVlidateuser) != "" ? Convert.ToString(VariableInfo.dtVlidateuser.Rows[0]["USER_ID"]) : "";
                this.dbManger.AddParameters(0, "@Type", "UPDATESAPPOSTEDSTATUS");
                this.dbManger.AddParameters(1, "@LocationCode", LocCode.Trim());
                this.dbManger.AddParameters(2, "@PONo", PONo.Trim());
                this.dbManger.AddParameters(3, "@MatCode", MatCode.Trim());
                this.dbManger.AddParameters(4, "@VendorCode", VendorCode.Trim());
                this.dbManger.AddParameters(5, "@QRCode", QRCode.Trim());
                this.dbManger.AddParameters(6, "@SAPPostMsg", Message.Trim());
                this.dbManger.AddParameters(7, "@CreatedBy", UserId.Trim());
                oStatus = Convert.ToString(this.dbManger.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USP_VendorDetails"));
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



        public DataTable DLRePrintedGetSAPVendorPOs()
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(2);
                dbManger.AddParameters(0, "@Type", "GETREPRINTEDSAPPONUMBER");
                dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                dt = this.dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_VendorDetails").Tables[0];
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

        public DataTable DLRePrintGetSelectedPOMatData(string PONum)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(3);
                dbManger.AddParameters(0, "@Type", "GETREPRINTSELECTEDPOMATDATA");
                dbManger.AddParameters(1, "@PONo", PONum.Trim());
                dbManger.AddParameters(2, "@LocationCode", VariableInfo.mPlantCode);
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_VendorDetails").Tables[0];
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

        public DataTable DLPrintGetSelectedMatDetails(string PONum)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(3);
                dbManger.AddParameters(0, "@Type", "GETREPRINTSELECTEDMATDETAILS");
                dbManger.AddParameters(1, "@PONo", PONum.Trim());
                dbManger.AddParameters(2, "@LocationCode", VariableInfo.mPlantCode);
                //dbManger.AddParameters(3, "@MatCode", MatCode.Trim());
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_VendorDetails").Tables[0];
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

        public ObservableCollection<PL_VendorMaster> DLRePrintGetSelectedMatQRCodeData(PL_VendorMaster _PLVMaster)
        {
            try
            {
                ObservableCollection<PL_VendorMaster> _objPLVendor = new ObservableCollection<PL_VendorMaster>();
                this.dbManger.Open();
                this.dbManger.CreateParameters(3);
                this.dbManger.AddParameters(0, "@Type", "GETREPRINTPOMATQRCODEDETAILS");
                this.dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                this.dbManger.AddParameters(2, "@PONo", _PLVMaster.PONumber);
                this.dbManger.AddParameters(2, "@MatCode", _PLVMaster.MatCode);
                IDataReader dataReader = dbManger.ExecuteReader(System.Data.CommandType.StoredProcedure, "USP_VendorDetails");
                while (dataReader.Read())
                {
                    _objPLVendor.Add(new PL_VendorMaster
                    {
                        IsValid = false,
                        MatCode = Convert.ToString(dataReader["MatCode"]),
                        MatDesc = Convert.ToString(dataReader["MatDescription"]),
                        QRCode = Convert.ToString(dataReader["QRCode"]),
                        StackQRCCode = Convert.ToString(dataReader["StackQRCode"]),
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

        public string DLUpdateReprintRequest(string PONo, string MatCode, string QRCode, string StackQRCode)
        {
            string oStatus = "";
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(8);
                string UserId = Convert.ToString(VariableInfo.dtVlidateuser) != "" ? Convert.ToString(VariableInfo.dtVlidateuser.Rows[0]["USER_ID"]) : "";
                this.dbManger.AddParameters(0, "@Type", "GETREPRINTPOMATREPRINTREQUEST");
                this.dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode.Trim());
                this.dbManger.AddParameters(2, "@PONo", PONo.Trim());
                this.dbManger.AddParameters(3, "@MatCode", MatCode.Trim());
                this.dbManger.AddParameters(5, "@QRCode", QRCode.Trim());
                this.dbManger.AddParameters(6, "@StackQRCode", StackQRCode.Trim());
                this.dbManger.AddParameters(7, "@CreatedBy", UserId.Trim());
                oStatus = Convert.ToString(this.dbManger.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USP_VendorDetails"));
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

        public string DLUpdateReprintQRCode(string PONo, string MatCode, string QRCode)
        {
            string oStatus = "";
            try
            {
                this.dbManger.Open();
                this.dbManger.CreateParameters(6);
                this.dbManger.AddParameters(0, "@Type", "UPDATEREPRINTQRCODEDETAILS");
                this.dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                this.dbManger.AddParameters(2, "@PONo", PONo.Trim());
                this.dbManger.AddParameters(3, "@MatCode", MatCode.Trim());
                this.dbManger.AddParameters(5, "@QRCode", QRCode.Trim());
                oStatus = Convert.ToString(this.dbManger.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USP_VendorDetails"));
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
    }
}
