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
   public class BL_Group_Rights
    {
       public DataSet GetDropDownData(PL_Group_Master _objPL_Group_Master, string sType)
       {
           try
           {
               DL_Group_Rights dlobj = new DL_Group_Rights();
               return dlobj.GetDropDownData(_objPL_Group_Master, sType);
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
               return new DL_Group_Rights().DL_GetGroupRoghts(_objPL_Group_Master);
           }
           catch (Exception ex)
           {

               throw ex;
           }
       }
       public OperationResult SaveUpdateGroupRights( PL_Group_Master objPL_Group_Master)
       {
           DL_Group_Rights dlobj = new DL_Group_Rights();
           return dlobj.SaveUpdateGroupRights( objPL_Group_Master);
       }
    }
}
