using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Data;
using COMMON;
using ENTITY_LAYER;
using COMMON_LAYER; 

namespace DATA_ACCESS_LAYER
{
    public class DL_Reports : DlCommon
    {
        StringBuilder _sbQuery = new StringBuilder();
        DBManager dbManger = null;
        DlCommon dCommon = null;
        DataTable dt = new DataTable();

        public DL_Reports()
        {
            this.dCommon = new DlCommon();
            this.dbManger = DBProvider();
        }

        public ObservableCollection<PL_Reports> DLSerialNoGenerationReportData(PL_Reports objPLPostToSAP)
        {
            try
            {
                ObservableCollection<PL_Reports> _obj_PLPostToSAP = new ObservableCollection<PL_Reports>();
                dbManger.Open();
                dbManger.CreateParameters(4);
                dbManger.AddParameters(0, "@Type", "GETSERIALNOGENERATIONREPORTDATA");
                dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                if (!String.IsNullOrEmpty(objPLPostToSAP.FromDate))
                {
                    dbManger.AddParameters(2, "@FromDate", Convert.ToDateTime(objPLPostToSAP.FromDate).ToString("yyyy-MM-dd"));
                }
                if (!String.IsNullOrEmpty(objPLPostToSAP.ToDate))
                {
                    dbManger.AddParameters(3, "@ToDate", Convert.ToDateTime(objPLPostToSAP.ToDate).ToString("yyyy-MM-dd"));
                }
                IDataReader dataReader = dbManger.ExecuteReader(System.Data.CommandType.StoredProcedure, "USP_Reports");
                while (dataReader.Read())
                {
                    _obj_PLPostToSAP.Add(new PL_Reports
                    {
                        PlantCode = Convert.ToString(dataReader["PlantCode"]),
                        MaterialCode = Convert.ToString(dataReader["MatCode"]),
                        MaterialDescription = Convert.ToString(dataReader["MatDescription"]),
                        QRCode = Convert.ToString(dataReader["QRCode"]),
                        StackQRCode = Convert.ToString(dataReader["StackQRCode"]),
                        SerialNo = Convert.ToString(dataReader["SerialNo"]),
                        Quantity = Convert.ToInt32(dataReader["Quantity"]),
                        MatStatus = Convert.ToString(dataReader["MatStatus"]),
                        CreatedOn = Convert.ToString(dataReader["CreatedOn"]),
                    });
                }
                return _obj_PLPostToSAP;
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

        public ObservableCollection<PL_Reports> DLStockCountReportData(PL_Reports objPLPostToSAP)
        {
            try
            {
                ObservableCollection<PL_Reports> _obj_PLPostToSAP = new ObservableCollection<PL_Reports>();
                dbManger.Open();
                dbManger.CreateParameters(4);
                dbManger.AddParameters(0, "@Type", "GETSTOCKCOUNTREPORTDATA");
                dbManger.AddParameters(1, "@LocationCode", VariableInfo.mPlantCode);
                if (!String.IsNullOrEmpty(objPLPostToSAP.FromDate))
                {
                    dbManger.AddParameters(2, "@FromDate", Convert.ToDateTime(objPLPostToSAP.FromDate).ToString("yyyy-MM-dd"));
                }
                if (!String.IsNullOrEmpty(objPLPostToSAP.ToDate))
                {
                    dbManger.AddParameters(3, "@ToDate", Convert.ToDateTime(objPLPostToSAP.ToDate).ToString("yyyy-MM-dd"));
                }
                IDataReader dataReader = dbManger.ExecuteReader(System.Data.CommandType.StoredProcedure, "USP_Reports");
                while (dataReader.Read())
                {
                    _obj_PLPostToSAP.Add(new PL_Reports
                    {
                        PlantCode = Convert.ToString(dataReader["PlantCode"]),
                        MaterialCode = Convert.ToString(dataReader["MatCode"]),
                        MaterialDescription = Convert.ToString(dataReader["MatDescription"]),
                        QRCode = Convert.ToString(dataReader["QRCode"]),
                        StackQRCode = Convert.ToString(dataReader["StackQRCode"]),
                        CreatedOn = Convert.ToString(dataReader["CreatedOn"]),
                    });
                }
                return _obj_PLPostToSAP;
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

        public DataTable DLSerialNoGenerationReport(PL_Reports _objlm)
        {
            dt = new DataTable();
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(3);
                dbManger.AddParameters(0, "@Type", "RackWiseStockRpt");
                dbManger.AddParameters(1, "@FromDate", _objlm.FromDate);
                dbManger.AddParameters(2, "@ToDate", _objlm.ToDate);
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_MstReport").Tables[0];
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
    }
}
