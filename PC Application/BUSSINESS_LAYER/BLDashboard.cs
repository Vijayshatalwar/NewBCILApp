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
  public class BLDashboard
    {
      public DataTable BLPickingStatus()
      {
          try
          {
              return new DLDashboard().DLPickingStatus();
          }
          catch (Exception ex)
          {
              throw ex;
          }    
      }


      public DataTable BLReceivingStatus()
      {
          try
          {
              return new DLDashboard().DLReceivingStatus();
          }
          catch (Exception ex)
          {
              throw ex;
          }
       }
    }   
}
