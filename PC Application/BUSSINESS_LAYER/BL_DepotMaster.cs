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
    public class BL_DepotMaster
    {

        public ObservableCollection<PL_DepotMaster> BL_GetDepotData(PL_DepotMaster _objDepotMaster)
        {
            try
            {
                return new DL_DepotMaster().DL_GetDepotMaster(_objDepotMaster);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public OperationResult BL_SaveDepotData(PL_DepotMaster _objDepotMaster)
        {
            DL_DepotMaster dlobj = new DL_DepotMaster();
            return dlobj.DL_SaveDepotData(_objDepotMaster);
        }

        public OperationResult BL_UpdateDepotData(PL_DepotMaster _objDepotMaster)
        {

            DL_DepotMaster dlobj = new DL_DepotMaster();
            return dlobj.DL_UpdateDepotData(_objDepotMaster);
        }

        public OperationResult BL_DeleteDepot(PL_DepotMaster _objDepotMaster)
        {
            DL_DepotMaster dlobj = new DL_DepotMaster();
            return dlobj.DL_DeleteDepotData(_objDepotMaster);
        }
    }
}
