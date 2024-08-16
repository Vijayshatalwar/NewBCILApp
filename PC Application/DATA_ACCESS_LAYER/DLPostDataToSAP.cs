using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ENTITY_LAYER;
using System.Data;
using COMMON;
using COMMON_LAYER;
using System.Net;
using System.IO;


namespace DATA_ACCESS_LAYER
{
    public class DLPostDataToSAP : DlCommon
    {
        DBManager dbManger = null;
        DlCommon dCommon = null;
        DataTable dt = null;
        StringBuilder sb = null;
        public DLPostDataToSAP()
        {
            this.dCommon = new DlCommon();
            this.dbManger = DBProvider();
        }
        //uoiuoiu
        public ObservableCollection<PLPostToSAP> DL_GetPostToSAPData(PLPostToSAP objPLPostToSAP)
        {
            try
            {
                ObservableCollection<PLPostToSAP> _obj_PLPostToSAP = new ObservableCollection<PLPostToSAP>();
                dbManger.Open();
                dbManger.CreateParameters(4);
                dbManger.AddParameters(0, "@Type", "GetDataToPost");
                dbManger.AddParameters(1, "@PlantCode", VariableInfo.mPlantCode);
                if (!String.IsNullOrEmpty(objPLPostToSAP.PostingDate))
                {
                    dbManger.AddParameters(2, "@FromDate", Convert.ToDateTime(objPLPostToSAP.PostingDate).ToString("yyyy-MM-dd"));
                }
                if (!String.IsNullOrEmpty(objPLPostToSAP.ToDate))
                {
                    dbManger.AddParameters(3, "@ToDate", Convert.ToDateTime(objPLPostToSAP.ToDate).ToString("yyyy-MM-dd"));
                }
                IDataReader dataReader = dbManger.ExecuteReader(System.Data.CommandType.StoredProcedure, "USP_PostToSap");
                while (dataReader.Read())
                {
                    _obj_PLPostToSAP.Add(new PLPostToSAP
                    {
                        PlantCode= Convert.ToString(dataReader["PlantCode"]),
                        MaterialCode = Convert.ToString(dataReader["Itemcode"]),
                        MaterialDescription = Convert.ToString(dataReader["ItemDescription"]),
                        Barcode = Convert.ToString(dataReader["Barcode"]),
                        VisualBarcode = Convert.ToString(dataReader["VisualBarcode"]),
                        Createdon = Convert.ToString(dataReader["Createdon"]),
                        Createdby = Convert.ToString(dataReader["Createdby"]),
                        Quantity = Convert.ToInt32(dataReader["Quantity"]),
                        SerialNo = Convert.ToString(dataReader["SerialNo"]),
                        TotalQty = Convert.ToInt32(dataReader["TotalQty"])
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
      
    
        public DataTable DlGetItemcode()
        {
            dt = new DataTable();
            string strOutParm = string.Empty;
            try
            {
                dbManger.Open();
                dbManger.CreateParameters(2);
                dbManger.AddParameters(0, "@Type", "GetItemcode");
                dbManger.AddParameters(1, "@PlantCode", VariableInfo.mPlantCode);
                dt = dbManger.ExecuteDataSet(System.Data.CommandType.StoredProcedure, "USP_PostToSap").Tables[0];
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

        
    }
}
