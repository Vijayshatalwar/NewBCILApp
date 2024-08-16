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
    public class BL_EmailConfigMaster
    {

        public ObservableCollection<PL_EmailConfigMaster> BL_GetEmailConfigMasterData(PL_EmailConfigMaster _objEConfigMaster)
        {
            try
            {
                return new DL_EmailConfigMaster().DL_GetEmailConfigMasterData(_objEConfigMaster);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public OperationResult BL_SaveEmailConfigMasterData(PL_EmailConfigMaster _objEConfigMaster)
        {
            DL_EmailConfigMaster dlobj = new DL_EmailConfigMaster();
            return dlobj.DL_SaveEmailConfigMaster(_objEConfigMaster);
        }

        public OperationResult BL_UpdateEmailConfigMasterData(PL_EmailConfigMaster _objEConfigMaster)
        {

            DL_EmailConfigMaster dlobj = new DL_EmailConfigMaster();
            return dlobj.DL_UpdateEmailConfigMaster(_objEConfigMaster);
        }

        public OperationResult BL_DeleteEmailConfigMasterData(PL_EmailConfigMaster _objEConfigMaster)
        {
            DL_EmailConfigMaster dlobj = new DL_EmailConfigMaster();
            return dlobj.DL_DeleteEmailConfigMaster(_objEConfigMaster);
        }
    }
}
