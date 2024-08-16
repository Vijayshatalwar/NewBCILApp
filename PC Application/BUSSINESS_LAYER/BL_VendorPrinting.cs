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
    public class BL_VendorPrinting
    {
        #region Vendor Barcode generation

        public DataTable BLGetSAPVendorPONumbers()
        {
            try
            {
                return new DL_VendorPrinting().DLGetSAPVendorPONumbers();
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
                return new DL_VendorPrinting().DLGetSelectedPOVendorDetails(PONo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ObservableCollection<PL_VendorMaster> BLGetSelectedPOMatData(PL_VendorMaster _objPLVendor)
        {
            try
            {
                return new DL_VendorPrinting().DLGetSelectedPOMatData(_objPLVendor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public OperationResult SaveGeneratedQRCode(PL_VendorMaster _objPLVendor)
        {
            DL_VendorPrinting dlobj = new DL_VendorPrinting();
            return dlobj.SaveGeneratedQRCode(_objPLVendor);
        }

        #endregion


        #region Vendor labels printing at Vendor site

        public DataTable BLGetGeneratedPONumbersPrintingAtVendor()
        {
            try
            {
                return new DL_VendorPrinting().DLGetGeneratedPONumbersPrintingAtVendor();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetGeneratedPONumbersPrintingAtHub()
        {
            try
            {
                return new DL_VendorPrinting().DLGetGeneratedPONumbersPrintingAtHub();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLPrintGetVendorPODetails(string PONo)
        {
            try
            {
                return new DL_VendorPrinting().DLPrintGetPOVendorDetails(PONo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ObservableCollection<PL_VendorMaster> BLPrintGetVendorPOMatData(PL_VendorMaster _objPLVendor)
        {
            try
            {
                return new DL_VendorPrinting().DLPrintGetVendorPOMatData(_objPLVendor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLPrintGetPrintMatQRCodeDetails(string PONo, string MatCode, string VendorCode)
        {
            try
            {
                return new DL_VendorPrinting().DLPrintGetPrintMatQRCodeDetails(PONo, MatCode, VendorCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BLUpdateQRCode(string PONo, string MatCode, string VendorCode, string sInvNo, string sInvDate, string QRCode, string UserId)
        {
            try
            {
                return new DL_VendorPrinting().DLUpdateQRCode(PONo, MatCode, VendorCode, sInvNo, sInvDate, QRCode, UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion



        public DataTable BLGetQRCodeRunningSerialNo(string DateFormat, string sPrintingSaction, string sLocationType)
        {
            try
            {
                return new DL_VendorPrinting().DLGetQRCodeRunningSerialNo(DateFormat, sPrintingSaction, sLocationType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetStackRunningSerialNo(string DateFormat)
        {
            try
            {
                return new DL_VendorPrinting().DLGetStackRunningSerialNo(DateFormat);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        

       

        public OperationResult SaveStackQRCode(PL_VendorMaster _objPLVendor)
        {
            DL_VendorPrinting dlobj = new DL_VendorPrinting();
            return dlobj.SaveStackQRCode(_objPLVendor);
        }

        public DataTable BLGetMatStockSize(string MatCode)
        {
            try
            {
                return new DL_VendorPrinting().DLGetMatStockSize(MatCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetVendorEmailDetails(string VendorCode)
        {
            try
            {
                return new DL_VendorPrinting().DLGetVendorEmailDetails(VendorCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        
        

        

        

        

        public string BLUpdateStackQRCode(string PONo, string MatCode, string VendorCode, string VendorInv, string VendorInvDate, string stackQRCode, int TPrintQty)
        {
            try
            {
                return new DL_VendorPrinting().DLUpdateStackQRCode(PONo, MatCode, VendorCode, VendorInv, VendorInvDate, stackQRCode, TPrintQty);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BLUpdateSAPPostStatus(string LocCode, string PONo, string MatCode, string VendorCode, string QRCode, string Message)
        {
            try
            {
                return new DL_VendorPrinting().DLUpdateSAPPostStatus(LocCode, PONo, MatCode, VendorCode, QRCode, Message);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        public DataTable BLRePrintedGetSAPVendorPOs()
        {
            try
            {
                return new DL_VendorPrinting().DLRePrintedGetSAPVendorPOs();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLRePrintGetSelectedPOMatData(string PONo)
        {
            try
            {
                return new DL_VendorPrinting().DLRePrintGetSelectedPOMatData(PONo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLPrintGetSelectedMatDetails(string PONum)
        {
            try
            {
                return new DL_VendorPrinting().DLPrintGetSelectedMatDetails(PONum);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ObservableCollection<PL_VendorMaster> BLRePrintGetSelectedMatQRCodeData(PL_VendorMaster _objPLVendor)
        {
            try
            {
                return new DL_VendorPrinting().DLRePrintGetSelectedMatQRCodeData(_objPLVendor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BLUpdateReprintRequest(string PONo, string MatCode, string QRCode, string StackQRCode)
        {
            try
            {
                return new DL_VendorPrinting().DLUpdateReprintRequest(PONo, MatCode, QRCode, StackQRCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BLUpdateRePrintQRCode(string PONo, string MatCode, string QRCode)
        {
            try
            {
                return new DL_VendorPrinting().DLUpdateReprintQRCode(PONo, MatCode, QRCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
