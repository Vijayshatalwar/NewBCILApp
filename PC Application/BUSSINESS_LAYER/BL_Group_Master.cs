using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using ENTITY_LAYER;
using COMMON;
using DATA_ACCESS_LAYER; 

namespace BUSSINESS_LAYER
{
   public class BL_Group_Master
    {
       public OperationResult Save(PL_Group_Master _objPL_Group_Master)
       {
           try
           {
               return new DL_Group_Master().Save(_objPL_Group_Master);
           }
           catch (Exception ex)
           {

               throw ex;
           }
       }
       public ObservableCollection<PL_Group_Master> BI_GetUploadData(PL_Group_Master _objPL_Group_Master)
       {
           try
           {
               return new DL_Group_Master().DL_GetGroupMaster(_objPL_Group_Master);
           }
           catch (Exception ex)
           {

               throw ex;
           }
       }
       public OperationResult Delete(PL_Group_Master _objPL_Group_Master)
       {
           try
           {
               return new DL_Group_Master().Delete(_objPL_Group_Master);
           }
           catch (Exception ex)
           {

               throw ex;
           }
       }
       public OperationResult Update(PL_Group_Master _objPL_Group_Master)
       {
           try
           {
               return new DL_Group_Master().Update(_objPL_Group_Master);
           }
           catch (Exception ex)
           {

               throw ex;
           }
       }
    }
}
