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
    public class BL_ExistingStockPrint
    {
        public DataTable BLGetMatProduct()
        {
            try
            {
                return new DL_ExistingStockPrint().DLGetMatProduct();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetMatSize(string objProduct)
        {
            try
            {
                return new DL_ExistingStockPrint().DLGetMatSize(objProduct);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetMatThickness(string objProduct)
        {
            try
            {
                return new DL_ExistingStockPrint().DLGetMatThickness(objProduct);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetMatGrade(string objProduct)
        {
            try
            {
                return new DL_ExistingStockPrint().DLGetMatGrade(objProduct);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetMatCategory(string objProduct)
        {
            try
            {
                return new DL_ExistingStockPrint().DLGetMatCategory(objProduct);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetMatData(string objProduct)
        {
            try
            {
                return new DL_ExistingStockPrint().DLGetMatData(objProduct);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetMatGroupDesc()
        {
            try
            {
                return new DL_ExistingStockPrint().DLGetMatGroupDesc();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
