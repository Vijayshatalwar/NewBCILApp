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
    public class BL_PlantMaster
    {
        public ObservableCollection<PL_PlantMaster> BL_GetPlantMasterData(PL_PlantMaster _objPlantMaster)
        {
            try
            {
                return new DL_PlantMaster().DL_GetPlantMastersData(_objPlantMaster);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public OperationResult BL_SavePlantData(PL_PlantMaster _objPlantMaster)
        {
            DL_PlantMaster dlobj = new DL_PlantMaster();
            return dlobj.DL_SavePlantData(_objPlantMaster);
        }

        public OperationResult BL_UpdateDepotData(PL_PlantMaster _objPlantMaster)
        {

            DL_PlantMaster dlobj = new DL_PlantMaster();
            return dlobj.DL_UpdatePlantData(_objPlantMaster);
        }

        public OperationResult BL_DeletePlantData(PL_PlantMaster _objPlantMaster)
        {
            DL_PlantMaster dlobj = new DL_PlantMaster();
            return dlobj.DL_DeletePlantData(_objPlantMaster);
        }
    }
}
