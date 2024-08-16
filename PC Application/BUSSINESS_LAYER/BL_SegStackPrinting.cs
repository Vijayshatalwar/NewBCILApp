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
    public class BL_SegStackPrinting
    {
        public ObservableCollection<PL_SegStackPrinting> BLGetSegregationStackDetails(string sLocationType)
        {
            try
            {
                return new DL_SegStackPrinting().DLGetSegregationStackDetails(sLocationType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ObservableCollection<PL_SegStackPrinting> BLGetDecorSegregationStackDetails()
        {
            try
            {
                return new DL_SegStackPrinting().DLGetDecorSegregationStackDetails();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLSegregationSelectedMatDetails(string LocationCode, string MatCode, string UserId)
        {
            try
            {
                return new DL_SegStackPrinting().DLSegregationSelectedMatDetails(LocationCode, MatCode, UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLDecorSegregationSelectedMatDetails(string LocationCode, string MatCode, string UserId)
        {
            try
            {
                return new DL_SegStackPrinting().DLDecorSegregationSelectedMatDetails(LocationCode, MatCode, UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BLSegregationSaveStackQRCode(string objLocationCode, string objMatCode, string objQRCode, string objStackQRCode, string sDateFormat, string sPrintingSection, string sLocationType)
        {
            try
            {
                return new DL_SegStackPrinting().DLSegregationSaveStackQRCode(objLocationCode, objMatCode, objQRCode, objStackQRCode, sDateFormat, sPrintingSection, sLocationType);
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
                return new DL_SegStackPrinting().DLGetStackRunningSerialNo(DateFormat, sPrintingSection, sLocationType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BLSegregationUpdateQRCodeSAPStatus(string sLocationCode, string sMatCode, string sQRCode, string sStatus, string sPostMsg)
        {
            try
            {
                return new DL_SegStackPrinting().DLSegregationUpdateQRCodeSAPStatus(sLocationCode, sMatCode, sQRCode, sStatus, sPostMsg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BLSegregationDeleteSelectedMatDetails(string LocationCode, string MatCode, string UserId)
        {
            try
            {
                return new DL_SegStackPrinting().DLSegregationDeleteSelectedMatDetails(LocationCode, MatCode, UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        #region Delivery Cancellation

        public ObservableCollection<PL_SegStackPrinting> BLGetCancelledDeliveryStackDetails()
        {
            try
            {
                return new DL_SegStackPrinting().DLGetCancelledDeliveryStackDetails();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLDeliveryCancelledSelectedMatDetails(string LocationCode, string MatCode, string UserId)
        {
            try
            {
                return new DL_SegStackPrinting().DLDeliveryCancelledSelectedMatDetails(LocationCode, MatCode, UserId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BLDeliveryCancelledSaveStackQRCode(string objLocationCode, string objMatCode, string objQRCode, string objStackQRCode, string sDateFormat, string sPrintingSection, string sLocationType)
        {
            try
            {
                return new DL_SegStackPrinting().DLDeliveryCancelledSaveStackQRCode(objLocationCode, objMatCode, objQRCode, objStackQRCode, sDateFormat, sPrintingSection, sLocationType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
