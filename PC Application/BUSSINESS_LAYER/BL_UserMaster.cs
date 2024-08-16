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
   public class BL_UserMaster
    {
       public ObservableCollection<PL_UserMaster> BI_GetUploadData(PL_UserMaster _objPL_UserMaster)
       {
           try
           {
               return new DL_UserMaster().DL_GetUserMasterData(_objPL_UserMaster);
           }
           catch (Exception ex)
           {

               throw ex;
           }
       }

       public DataTable ValidateUser(PL_UserMaster objPL_UserMaster)
       {
            DL_UserMaster _DL_UserMaster = new DL_UserMaster();
            return _DL_UserMaster.ValidateUser(objPL_UserMaster);
       }

       public DataTable ValidateVendorUser(PL_UserMaster objPL_UserMaster)
       {
           DL_UserMaster _DL_UserMaster = new DL_UserMaster();
           return _DL_UserMaster.ValidateVendorUser(objPL_UserMaster);
       }
     
       public OperationResult Save(PL_UserMaster objPL_UserMaster)
       {
           DL_UserMaster dlobj = new DL_UserMaster();
           return dlobj.Save(objPL_UserMaster);
       }

       public OperationResult Update(PL_UserMaster objPL_UserMaster)
       {
           
          DL_UserMaster dlobj = new DL_UserMaster();
          return dlobj.Update(objPL_UserMaster);
       }

       public OperationResult Delete(PL_UserMaster objPL_UserMaster)
       {
           DL_UserMaster dlobj = new DL_UserMaster();
           return dlobj.Delete(objPL_UserMaster);
       }

       public OperationResult UpdatePassword(PL_UserMaster objPL_UserMaster)
       {
           DL_UserMaster dlobj = new DL_UserMaster();
           return dlobj.UpdatePassword(objPL_UserMaster);
       }

       public DataSet GetDropDownData(PL_UserMaster _objPL_UserMaster, string sType)
       {
           try
           {
               DL_UserMaster dlobj = new DL_UserMaster();
               return dlobj.GetDropDownData(_objPL_UserMaster, sType);
           }
           catch (Exception ex)
           {

               throw ex;
           }
       }

       public DataSet GetLocationCodeData()
       {
           try
           {
               DL_UserMaster dlobj = new DL_UserMaster();
               return dlobj.GetLocationCodeData();
           }
           catch (Exception ex)
           {

               throw ex;
           }
       }

       public DataTable GetGroupRights(string _UserID)
       {
           DL_UserMaster _objDL_UserMaster = new DL_UserMaster();
           return _objDL_UserMaster.GetGroupRights(_UserID);
       }

       public bool AppStartFirstTime()
       {
           try
           {
               return new DL_UserMaster().AppStartFirstTime();
           }
           catch (Exception ex)
           {
               throw ex;
           }
       }

       public string GetPlantCode()
       {
           DL_UserMaster _DL_UserMaster = new DL_UserMaster();
           return _DL_UserMaster.GetPlantCode();
       }
       public string GetPlantType()
       {
           DL_UserMaster _DL_UserMaster = new DL_UserMaster();
           return _DL_UserMaster.GetPlantType();
       }

       public DataTable BLGetDBConnectionDetails()
       {
           DL_UserMaster _DL_UserMaster = new DL_UserMaster();
           return _DL_UserMaster.DLGetDBConnectionDetails();
       }
     
    }
}
