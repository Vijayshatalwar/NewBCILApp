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
    public class BL_HubPrinting
    {
        #region Vendor Barcode generation

        public DataTable BLGetSAPPONumbers()
        {
            try
            {
                return new DL_HubPrinting().DLGetSAPPONumbers();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetSelectedPOVendorDetails(string PONo)
        {
            try
            {
                return new DL_HubPrinting().DLGetSelectedPOVendorDetails(PONo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ObservableCollection<PL_HubPrinting> BLGetSelectedPOMatData(PL_HubPrinting _objPLVendor)
        {
            try
            {
                return new DL_HubPrinting().DLGetSelectedPOMatData(_objPLVendor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public OperationResult SaveGeneratedQRCode(PL_HubPrinting _objPLVendor)
        {
            DL_HubPrinting dlobj = new DL_HubPrinting();
            return dlobj.SaveGeneratedQRCode(_objPLVendor);
        }


        #endregion

        #region Vendor labels printing at HUB site

        public DataTable BLGetSAPHubPONumbers()
        {
            try
            {
                return new DL_HubPrinting().DLGetSAPHubPONumbers();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLPrintGetHubPOVendorDetails(string PONo)
        {
            try
            {
                return new DL_HubPrinting().DLPrintGetHubPOVendorDetails(PONo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ObservableCollection<PL_HubPrinting> BLPrintGetHubPOMatData(PL_HubPrinting _objPLVendor)
        {
            try
            {
                return new DL_HubPrinting().DLPrintGetHUBPOMatData(_objPLVendor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetQRCodeRunningSerialNo(string DateFormat, string sPrintingSaction, string sLocationType)
        {
            try
            {
                return new DL_HubPrinting().DLGetQRCodeRunningSerialNo(DateFormat, sPrintingSaction, sLocationType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public OperationResult SaveHUBQRCode(PL_HubPrinting _objPLVendor)
        {
            DL_HubPrinting dlobj = new DL_HubPrinting();
            return dlobj.SaveHUBQRCode(_objPLVendor);
        }

        public DataTable BLGetUnbrandedMatGroups(string oLocationCode)
        {
            try
            {
                return new DL_ItemSelection().DLGetUnbrandedMatGroups(oLocationCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BLGetStackRunningSerialNo(string DateFormat, string sPrintingSection, string sLocationType)
        {
            try
            {
                return new DL_HubPrinting().DLGetStackRunningSerialNo(DateFormat, sPrintingSection, sLocationType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


       

       

        

      
    }
}
