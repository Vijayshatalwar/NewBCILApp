using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Data;
using ENTITY_LAYER;
using COMMON;
using DATA_ACCESS_LAYER;

namespace BUSSINESS_LAYER
{
    public class BL_VendorMaster
    {
        public ObservableCollection<PL_VendorMaster> BL_GetVendorMasterData(PL_VendorMaster _objPL_VendorMaster)
        {
            try
            {
                return new DL_VendorMaster().DL_GetVendorMasterData(_objPL_VendorMaster);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public OperationResult BL_SaveVendorData(PL_VendorMaster _objPL_VendorMaster)
        {
            DL_VendorMaster dlobj = new DL_VendorMaster();
            return dlobj.DL_SaveVendorData(_objPL_VendorMaster);
        }

        public OperationResult BL_UpdateVendorData(PL_VendorMaster _objPL_VendorMaster)
        {

            DL_VendorMaster dlobj = new DL_VendorMaster();
            return dlobj.DL_UpdateVendorData(_objPL_VendorMaster);
        }

        public OperationResult BL_DeleteVendor(PL_VendorMaster _objPL_VendorMaster)
        {
            DL_VendorMaster dlobj = new DL_VendorMaster();
            return dlobj.DL_DeleteVendorData(_objPL_VendorMaster);
        }
    }
}
