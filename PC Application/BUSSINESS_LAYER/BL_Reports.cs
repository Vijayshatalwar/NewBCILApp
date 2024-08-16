using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DATA_ACCESS_LAYER;
using ENTITY_LAYER;
using System.Collections.ObjectModel;
using COMMON;

namespace BUSSINESS_LAYER
{
    public class BL_Reports
    {
        public ObservableCollection<PL_Reports> BLSerialNoGenerationReportData(PL_Reports _objPLPostToSAP)
        {
            try
            {
                return new DL_Reports().DLSerialNoGenerationReportData(_objPLPostToSAP);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ObservableCollection<PL_Reports> BLStockCountReportData(PL_Reports _objPLPostToSAP)
        {
            try
            {
                return new DL_Reports().DLStockCountReportData(_objPLPostToSAP);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLSerialNoGenerationReport(PL_Reports _objlm)
        {
            try
            {
                return new DL_Reports().DLSerialNoGenerationReport(_objlm);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
