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
   public class BL_MaterialMaster
    {
       public ObservableCollection<PL_MaterialMaster> BI_GetMaterialMasterData(PL_MaterialMaster _objPLMaterialMaster)
        {
            try
            {
                return new DL_MaterialMaster().DL_GetCommonMaster(_objPLMaterialMaster);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

       public OperationResult Save(PL_MaterialMaster _objPLMaterialMaster)
        {
            try
            {
                return new DL_MaterialMaster().Save(_objPLMaterialMaster);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
       public OperationResult Update(PL_MaterialMaster _objPLMaterialMaster)
       {
           try
           {
               return new DL_MaterialMaster().Update(_objPLMaterialMaster);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
       public OperationResult Delete(PL_MaterialMaster _objPLMaterialMaster)
       {
           try
           {
               return new DL_MaterialMaster().Delete(_objPLMaterialMaster);
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }
   
    }
}
