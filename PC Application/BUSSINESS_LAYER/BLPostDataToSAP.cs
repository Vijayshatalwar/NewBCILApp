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
    public class BLPostDataToSAP
    {
        public ObservableCollection<PLPostToSAP> BI_GetPostToSAPData(PLPostToSAP _objPLPostToSAP)
        {
            try
            {
                return new DLPostDataToSAP().DL_GetPostToSAPData(_objPLPostToSAP);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public DataTable BlGetItemcode()
        {
            try
            {
                return new DLPostDataToSAP().DlGetItemcode();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       
    }
}
