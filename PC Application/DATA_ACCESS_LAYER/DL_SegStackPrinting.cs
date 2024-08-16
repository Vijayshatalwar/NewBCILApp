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
    public class DL_SegStackPrinting : DlCommon
    {
        DBManager dbManager = null;
        DlCommon dCommon = null;
        DataTable dt = null;
        StringBuilder sb = null;


        public DL_SegStackPrinting()
        {
            this.dCommon = new DlCommon();
            this.dbManager = DBProvider();
        }

        public ObservableCollection<PL_SegStackPrinting> DLGetSegregationStackDetails(string sLocationType)
        {
            try
            {
                ObservableCollection<PL_SegStackPrinting> _objPLprint = new ObservableCollection<PL_SegStackPrinting>();
                this.dbManager.Open();
                this.dbManager.CreateParameters(4);
                this.dbManager.AddParameters(0, "@Type", "GETSEGREGATIONSTACKPRINTINGDATA");
                this.dbManager.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                this.dbManager.AddParameters(2, "@CreatedBy", VariableInfo.mAppUserID.Trim());
                this.dbManager.AddParameters(3, "@LocationType", sLocationType);
                IDataReader dataReader = dbManager.ExecuteReader(System.Data.CommandType.StoredProcedure, "USP_StackSegregationPrinting");
                while (dataReader.Read())
                {
                    _objPLprint.Add(new PL_SegStackPrinting
                    {
                        IsValid = false,
                        UserId = Convert.ToString(dataReader["UserId"]),
                        MatCode = Convert.ToString(dataReader["MatCode"]),
                        MatDescription = Convert.ToString(dataReader["MatDesc"]),
                        Grade = Convert.ToString(dataReader["MatGrade"]),
                        MatGroup = Convert.ToString(dataReader["MatGroup"]),
                        Thickness = Convert.ToString(dataReader["MatThickness"]),
                        ThicknessDesc = Convert.ToString(dataReader["ThicknessDescription"]),
                        Size = Convert.ToString(dataReader["MatSize"]),
                        //QRCode = Convert.ToString(dataReader["QRCode"]),
                        RowsCount = Convert.ToString(dataReader["TotalQty"]),
                    });
                }
                return _objPLprint;
            }
            catch (Exception ex)
            {
                this.dbManager.Close();
                throw ex;
            }
            finally
            {
                this.dbManager.Close();
            }
        }

        public ObservableCollection<PL_SegStackPrinting> DLGetDecorSegregationStackDetails()
        {
            try
            {
                ObservableCollection<PL_SegStackPrinting> _objPLprint = new ObservableCollection<PL_SegStackPrinting>();
                this.dbManager.Open();
                this.dbManager.CreateParameters(3);
                this.dbManager.AddParameters(0, "@Type", "GETDECORSEGREGATIONSTACKPRINTINGDATA");
                this.dbManager.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                this.dbManager.AddParameters(2, "@CreatedBy", VariableInfo.mAppUserID.Trim());
                IDataReader dataReader = dbManager.ExecuteReader(System.Data.CommandType.StoredProcedure, "USP_StackSegregationPrinting");
                while (dataReader.Read())
                {
                    _objPLprint.Add(new PL_SegStackPrinting
                    {
                        IsValid = false,
                        UserId = Convert.ToString(dataReader["UserId"]),
                        MatCode = Convert.ToString(dataReader["MatCode"]),
                        DesignNo = Convert.ToString(dataReader["DesignNo"]),
                        FinishCode = Convert.ToString(dataReader["FinishCode"]),
                        BatchNo = Convert.ToString(dataReader["BatchNo"]),
                        MatDescription = Convert.ToString(dataReader["MatDesc"]),
                        Grade = Convert.ToString(dataReader["MatGrade"]),
                        MatGroup = Convert.ToString(dataReader["MatGroup"]),
                        Thickness = Convert.ToString(dataReader["MatThickness"]),
                        ThicknessDesc = Convert.ToString(dataReader["ThicknessDescription"]),
                        Size = Convert.ToString(dataReader["MatSize"]),
                        RowsCount = Convert.ToString(dataReader["TotalQty"]),
                    });
                }
                return _objPLprint;
            }
            catch (Exception ex)
            {
                this.dbManager.Close();
                throw ex;
            }
            finally
            {
                this.dbManager.Close();
            }
        }

        public DataTable DLSegregationSelectedMatDetails(string LocationCode, string MatCode, string UserId)
        {
            dt = new DataTable();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, "@Type", "GETSEGREGATIONSELECTEDMATCODEDETAILS");
                dbManager.AddParameters(1, "@LocationCode", LocationCode.Trim());
                dbManager.AddParameters(2, "@MatCode", MatCode.Trim());
                dbManager.AddParameters(3, "@CreatedBy", UserId.Trim());
                dt = dbManager.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_StackSegregationPrinting").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbManager.Close();
            }
            return dt;
        }

        public DataTable DLDecorSegregationSelectedMatDetails(string LocationCode, string MatCode, string UserId)
        {
            dt = new DataTable();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, "@Type", "GETDECORSEGREGATIONSELECTEDMATCODEDETAILS");
                dbManager.AddParameters(1, "@LocationCode", LocationCode.Trim());
                dbManager.AddParameters(2, "@MatCode", MatCode.Trim());
                dbManager.AddParameters(3, "@CreatedBy", UserId.Trim());
                dt = dbManager.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_StackSegregationPrinting").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbManager.Close();
            }
            return dt;
        }

        public string DLSegregationSaveStackQRCode(string objLocationCode, string objMatCode, string objQRCode, string objStackQRCode, string sDateFormat, string sPrintingSection, string sLocationType)
        {
            string oStatus = "";
            try
            {
                this.dbManager.Open();
                this.dbManager.CreateParameters(9);
                this.dbManager.AddParameters(0, "@Type", "SAVESEGREGATIONSTACKQRCODEDETAILS");
                this.dbManager.AddParameters(1, "@LocationCode", objLocationCode);
                this.dbManager.AddParameters(2, "@MatCode", objMatCode.Trim());
                this.dbManager.AddParameters(3, "@QRCode", objQRCode.Trim());
                this.dbManager.AddParameters(4, "@StackQRCode", objStackQRCode.Trim());
                this.dbManager.AddParameters(5, "@DateFormat", sDateFormat.Trim());
                this.dbManager.AddParameters(6, "@PrintSection", sPrintingSection.Trim());
                this.dbManager.AddParameters(7, "@LocationType", sLocationType.Trim());
                this.dbManager.AddParameters(8, "@CreatedBy", VariableInfo.mAppUserID.Trim());
                oStatus = Convert.ToString(this.dbManager.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USP_StackSegregationPrinting"));
            }
            catch (Exception ex)
            {
                this.dbManager.Close();
                throw ex;
            }
            finally
            {
                this.dbManager.Close();
            }
            return oStatus;
        }

        public string DLGetStackRunningSerialNo(string sDateFormat, string sPrintingSection, string sLocationType)
        {
            string sResult = string.Empty;
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(5);
                dbManager.AddParameters(0, "@Type", "GETSTACKRUNNINGSERIALNO");
                dbManager.AddParameters(1, "@DateFormat", sDateFormat.Trim());
                dbManager.AddParameters(2, "@LocationCode", VariableInfo.mPlantCode.Trim());
                dbManager.AddParameters(3, "@PrintSection", sPrintingSection.Trim());
                dbManager.AddParameters(4, "@LocationType", sLocationType.Trim());
                sResult = Convert.ToString(this.dbManager.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USP_MaterialMaster"));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbManager.Close();
            }
            return sResult;
        }

        public string DLSegregationUpdateQRCodeSAPStatus(string sLocationCode, string sMatCode, string sQRCode, string sStatus, string sPostMsg)
        {
            string oStatus = "";
            try
            {
                this.dbManager.Open();
                this.dbManager.CreateParameters(6);
                this.dbManager.AddParameters(0, "@Type", "UPDATESEGREGATIONSAPPOSTEDSTATUS");
                this.dbManager.AddParameters(1, "@LocationCode", sLocationCode);
                this.dbManager.AddParameters(2, "@MatCode", sMatCode.Trim());
                this.dbManager.AddParameters(3, "@QRCode", sQRCode.Trim());
                this.dbManager.AddParameters(4, "@MatStatus", sStatus.Trim());
                this.dbManager.AddParameters(5, "@SAPPostMsg", sPostMsg.Trim());
                oStatus = Convert.ToString(this.dbManager.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USP_StackSegregationPrinting"));
            }
            catch (Exception ex)
            {
                this.dbManager.Close();
                throw ex;
            }
            finally
            {
                this.dbManager.Close();
            }
            return oStatus;
        }

        public string DLSegregationDeleteSelectedMatDetails(string LocationCode, string MatCode, string UserId)
        {
            string oStatus = "";
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, "@Type", "SEGREGATIONDELETESELECTEDMATCODEDETAILS");
                dbManager.AddParameters(1, "@LocationCode", LocationCode.Trim());
                dbManager.AddParameters(2, "@MatCode", MatCode.Trim());
                dbManager.AddParameters(3, "@CreatedBy", UserId.Trim());
                oStatus = Convert.ToString(this.dbManager.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USP_StackSegregationPrinting"));
            }
            catch (Exception ex)
            {
                this.dbManager.Close();
                throw ex;
            }
            finally
            {
                dbManager.Close();
            }
            return oStatus;
        }

        //

        #region Delivery Cancelled Stack Data

        public ObservableCollection<PL_SegStackPrinting> DLGetCancelledDeliveryStackDetails()
        {
            try
            {
                ObservableCollection<PL_SegStackPrinting> _objPLprint = new ObservableCollection<PL_SegStackPrinting>();
                this.dbManager.Open();
                this.dbManager.CreateParameters(3);
                this.dbManager.AddParameters(0, "@Type", "GETDELIVERYCANCELLEDSTACKPRINTINGDATA");
                this.dbManager.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                this.dbManager.AddParameters(2, "@CreatedBy", VariableInfo.mAppUserID);
                IDataReader dataReader = dbManager.ExecuteReader(System.Data.CommandType.StoredProcedure, "USP_StackSegregationPrinting");
                while (dataReader.Read())
                {
                    _objPLprint.Add(new PL_SegStackPrinting
                    {
                        IsValid = false,
                        UserId = Convert.ToString(dataReader["UserId"]),
                        MatCode = Convert.ToString(dataReader["MatCode"]),
                        MatDescription = Convert.ToString(dataReader["MatDesc"]),
                        Grade = Convert.ToString(dataReader["MatGrade"]),
                        MatGroup = Convert.ToString(dataReader["MatGroup"]),
                        Thickness = Convert.ToString(dataReader["MatThickness"]),
                        ThicknessDesc = Convert.ToString(dataReader["ThicknessDescription"]),
                        Size = Convert.ToString(dataReader["MatSize"]),
                        //QRCode = Convert.ToString(dataReader["QRCode"]),
                        RowsCount = Convert.ToString(dataReader["TotalQty"]),
                    });
                }
                return _objPLprint;
            }
            catch (Exception ex)
            {
                this.dbManager.Close();
                throw ex;
            }
            finally
            {
                this.dbManager.Close();
            }
        }

        public DataTable DLDeliveryCancelledSelectedMatDetails(string LocationCode, string MatCode, string UserId)
        {
            dt = new DataTable();
            try
            {
                dbManager.Open();
                dbManager.CreateParameters(4);
                dbManager.AddParameters(0, "@Type", "GETDELIVERYCANCELLEDSELECTEDMATCODEDETAILS");
                dbManager.AddParameters(1, "@LocationCode", LocationCode.Trim());
                dbManager.AddParameters(2, "@MatCode", MatCode.Trim());
                dbManager.AddParameters(3, "@CreatedBy", UserId.Trim());
                dt = dbManager.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_StackSegregationPrinting").Tables[0];
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbManager.Close();
            }
            return dt;
        }

        public string DLDeliveryCancelledSaveStackQRCode(string objLocationCode, string objMatCode, string objQRCode, string objStackQRCode, string sDateFormat, string sPrintingSection, string sLocationType)
        {
            string oStatus = "";
            try
            {
                this.dbManager.Open();
                this.dbManager.CreateParameters(8);
                this.dbManager.AddParameters(0, "@Type", "DELIVERYCANCELLEDSAVESEGREGATIONSTACKQRCODEDETAILS");
                this.dbManager.AddParameters(1, "@LocationCode", objLocationCode);
                this.dbManager.AddParameters(2, "@MatCode", objMatCode.Trim());
                this.dbManager.AddParameters(3, "@QRCode", objQRCode.Trim());
                this.dbManager.AddParameters(4, "@StackQRCode", objStackQRCode.Trim());
                this.dbManager.AddParameters(5, "@DateFormat", sDateFormat.Trim());
                this.dbManager.AddParameters(6, "@PrintSection", sPrintingSection.Trim());
                this.dbManager.AddParameters(7, "@LocationType", sLocationType.Trim());
                oStatus = Convert.ToString(this.dbManager.ExecuteScalar(System.Data.CommandType.StoredProcedure, "USP_StackSegregationPrinting"));
            }
            catch (Exception ex)
            {
                this.dbManager.Close();
                throw ex;
            }
            finally
            {
                this.dbManager.Close();
            }
            return oStatus;
        }

        #endregion
    }
}
