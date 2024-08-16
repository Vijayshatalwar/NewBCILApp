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
    public class BL_WarehouseMaster
    {
        public ObservableCollection<PL_WarehouseMaster> BL_GetWarehouseData(PL_WarehouseMaster _objPL_WHMaster)
        {
            try
            {
                return new DL_WarehouseMaster().DL_GetWarehouseMaster(_objPL_WHMaster);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public OperationResult BL_SaveWarehouseData(PL_WarehouseMaster objPL_WHMaster)
        {
            DL_WarehouseMaster dlobj = new DL_WarehouseMaster();
            return dlobj.DL_SaveWarehouseData(objPL_WHMaster);
        }

        public OperationResult BL_UpdateWarehouseData(PL_WarehouseMaster objPL_WH_Master)
        {

            DL_WarehouseMaster dlobj = new DL_WarehouseMaster();
            return dlobj.DL_UpdateWarehouseData(objPL_WH_Master);
        }

        public OperationResult BL_DeleteWarehose(PL_WarehouseMaster objPL_WMaster)
        {
            DL_WarehouseMaster dlobj = new DL_WarehouseMaster();
            return dlobj.DL_DeleteWarehouseData(objPL_WMaster);
        }
    }
}
