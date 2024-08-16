using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using COMMON;
using System.Collections.ObjectModel;
using ENTITY_LAYER;
using DATA_ACCESS_LAYER;
using System.Data;

namespace BUSSINESS_LAYER
{
    public class BL_TradingData
    {
        public DataTable BlGetPONo()
        {
            try
            {
                return new DL_TradingData().DlGetPONo();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BlGetProducts()
        {
            try
            {
                return new DL_TradingData().DlGetProducts();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BlGetPODetails(string PoNum)
        {
            try
            {
                return new DL_TradingData().DlGetPODetails(PoNum);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BlGetPOMatData(string PONum, string VerticalName, string MatDesc)
        {
            try
            {
                return new DL_TradingData().DlGetPOMatData(PONum, VerticalName, MatDesc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BlGetPOMatQty(string PONum, string VendorName, string MatDesc)
        {
            try
            {
                return new DL_TradingData().DlGetPOMatQty(PONum, VendorName, MatDesc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BlGetMatCode(string ProductName, string MatDesc)
        {
            try
            {
                return new DL_TradingData().DlGetMatCode(ProductName, MatDesc);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BlGetPOVericalDetails(string VerName)
        {
            try
            {
                return new DL_TradingData().DlGetPOVerticalDetails(VerName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BlGetMatDescBySelectedProduct(string ProdName)
        {
            try
            {
                return new DL_TradingData().DlGetMatDescBySelectedProduct(ProdName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BlGetSelectedPOItemsData(string PoNum)
        {
            try
            {
                return new DL_TradingData().DlGetSelectedPOItemsData(PoNum);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BlGetSelectedPOVerticalData(string PoNum)
        {
            try
            {
                return new DL_TradingData().DlGetSelectedPOVerticalData(PoNum);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BlGetSelectedItem()
        {
            try
            {
                return new DL_TradingData().GetSelectedItem();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BL_GetPlantType()
        {
            DL_TradingData _DL_Trading = new DL_TradingData();
            return _DL_Trading.DL_GetPlantType();
        }

        public string BL_SaveBarcode(string plantcode, string itemcode, string barcode, string Vbarcode)
        {
            DL_TradingData _DL_Trading = new DL_TradingData();
            return _DL_Trading.DL_SaveBarcode(plantcode, itemcode, barcode, Vbarcode);
        }
    }
}
