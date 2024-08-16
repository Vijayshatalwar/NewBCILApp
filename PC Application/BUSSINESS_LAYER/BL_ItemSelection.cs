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
    public class BL_ItemSelection
    {

        #region Decorative

        public DataTable BLVGetMatProduct(string ProductType)
        {
            try
            {
                return new DL_ItemSelection().DLVGetMatProduct(ProductType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLVGetMatCategory(string objProduct)
        {
            try
            {
                return new DL_ItemSelection().DLVGetMatCategory(objProduct);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLVGetMatThickness(string objProduct, string objCat)
        {
            try
            {
                return new DL_ItemSelection().DLVGetMatThickness(objProduct, objCat);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLVGetMatGroup(string objProduct, string objCat, string objThickness)
        {
            try
            {
                return new DL_ItemSelection().DLVGetMatGroup(objProduct, objCat, objThickness);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLVGetMatGrade(string objProduct, string objCat, string objThickness, string objGroup)
        {
            try
            {
                return new DL_ItemSelection().DLVGetMatGrade(objProduct, objCat, objThickness, objGroup);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLVGetMatDesign(string objProduct, string objCat, string objThickness, string objGroup, string objGrade)
        {
            try
            {
                return new DL_ItemSelection().DLVGetMatDesign(objProduct, objCat, objThickness, objGroup, objGrade);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLVGetMatFinishCode(string objProduct, string objCat, string objThickness, string objGroup, string objGrade, string objDesignNo)
        {
            try
            {
                return new DL_ItemSelection().DLVGetMatFinishCode(objProduct, objCat, objThickness, objGroup, objGrade, objDesignNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLVGetMatSize(string objProduct, string objCat, string objThickness, string objGroup, string objGrade, string objDesignNo, string FinishCode)
        {
            try
            {
                return new DL_ItemSelection().DLVGetMatSize(objProduct, objCat, objThickness, objGroup, objGrade, objDesignNo, FinishCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLVGetMatVisionCode(string objProduct, string objCat, string objThickness, string objGroup, string objGrade, string objDesignNo, string objFinishCode, string objSize)
        {
            try
            {
                return new DL_ItemSelection().DLVGetMatVisionCode(objProduct, objCat, objThickness, objGroup, objGrade, objDesignNo, objFinishCode, objSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLVGetMatLippingCode(string objProduct, string objCat, string objThickness, string objGroup, string objGrade, string objDesignNo, string objFinishCode, string objSize, string objVisionCode)
        {
            try
            {
                return new DL_ItemSelection().DLVGetMatLippingCode(objProduct, objCat, objThickness, objGroup, objGrade, objDesignNo, objFinishCode, objSize, objVisionCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLVGetSelectedMatCode(string objProduct, string objCat, string objThickness, string objGroup, string objGrade, string objDesignNo, string objFinishCode, string objSize, string objVisionCode, string objLippingCode)
        {
            try
            {
                return new DL_ItemSelection().DLVGetSelectedMatCode(objProduct, objCat, objThickness, objGroup, objGrade, objDesignNo, objFinishCode, objSize, objVisionCode, objLippingCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLVSaveSelectedMatCode(string objMatCode, string objMatDesc, string objMatSize, string objMatThickness, int LotSize, string sBatchNo)
        {
            try
            {
                return new DL_ItemSelection().DLVSaveSelectedMatCode(objMatCode, objMatDesc, objMatSize, objMatThickness, LotSize, sBatchNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BLVSaveQRCode(string objLocationCode, string objMatCode, string sMatDesc, string sGrade, string sGroup, string sThickness, string sSize, string objQRCode, string sMatStatus, string sDateFormat, string sPrintSection, string sLocType, string oLabelType, string sPONo, string sVendorCode, string sBatchNo)
        {
            try
            {
                return new DL_ItemSelection().DLVSaveQRCode(objLocationCode, objMatCode, sMatDesc, sGrade, sGroup, sThickness, sSize, objQRCode, sMatStatus, sDateFormat, sPrintSection, sLocType, oLabelType, sPONo, sVendorCode, sBatchNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BLVSaveStackQRCode(string objLocationCode, string objMatCode, string objStackQRCode, string sDateFormat, string sPrintingSection, string sLocationType)
        {
            try
            {
                return new DL_ItemSelection().DLVSaveStackQRCode(objLocationCode, objMatCode, objStackQRCode, sDateFormat, sPrintingSection, sLocationType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BLVUpdateQRCodeSAPStatus(string sLocationCode, string sMatCode, string sQRCode, string sStatus, string sPostMsg)
        {
            try
            {
                return new DL_ItemSelection().DLVUpdateQRCodeSAPStatus(sLocationCode, sMatCode, sQRCode, sStatus, sPostMsg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        #endregion

        #region Door/Ply

        public DataTable BLGetMatProduct(string ProductType)
        {
            try
            {
                return new DL_ItemSelection().DLGetMatProduct(ProductType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetMatCategory(string objProduct)
        {
            try
            {
                return new DL_ItemSelection().DLGetMatCategory(objProduct);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetMatGrade(string objProduct, string objCat)
        {
            try
            {
                return new DL_ItemSelection().DLGetMatGrade(objProduct, objCat);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetMatGroup(string objProduct, string objCat, string objGrade)
        {
            try
            {
                return new DL_ItemSelection().DLGetMatGroup(objProduct, objCat, objGrade);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetMatThickness(string objProduct, string objCat, string objGrade, string objGroup)
        {
            try
            {
                return new DL_ItemSelection().DLGetMatThickness(objProduct, objCat, objGrade, objGroup);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetMatSize(string objProduct, string objCat, string objGrade, string objGroup, string objThickness)
        {
            try
            {
                return new DL_ItemSelection().DLGetMatSize(objProduct, objCat, objGrade, objGroup, objThickness);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetMatDesign(string objProduct, string objCat, string objGrade, string objGroup, string objThickness, string objSize)
        {
            try
            {
                return new DL_ItemSelection().DLGetMatDesign(objProduct, objCat, objGrade, objGroup, objThickness, objSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetMatFinishCode(string objProduct, string objCat, string objGrade, string objGroup, string objThickness, string objSize, string objDesignNo)
        {
            try
            {
                return new DL_ItemSelection().DLGetMatFinishCode(objProduct, objCat, objGrade, objGroup, objThickness, objSize, objDesignNo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetMatVisionCode(string objProduct, string objCat, string objGrade, string objGroup, string objThickness, string objSize, string objDesignNo, string objFinishCode)
        {
            try
            {
                return new DL_ItemSelection().DLGetMatVisionCode(objProduct, objCat, objGrade, objGroup, objThickness, objSize, objDesignNo, objFinishCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetMatLippingCode(string objProduct, string objCat, string objGrade, string objGroup, string objThickness, string objSize, string objDesignNo, string objFinishCode, string objVisionCode)
        {
            try
            {
                return new DL_ItemSelection().DLGetMatLippingCode(objProduct, objCat, objGrade, objGroup, objThickness, objSize, objDesignNo, objFinishCode, objVisionCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetSelectedMatCode(string objProduct, string objCat, string objGrade, string objGroup, string objThickness, string objSize, string objDesignNo, string objFinishCode, string objVisionCode, string objLippingCode)
        {
            try
            {
                return new DL_ItemSelection().DLGetSelectedMatCode(objProduct, objCat, objGrade, objGroup, objThickness, objSize, objDesignNo, objFinishCode, objVisionCode, objLippingCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLSaveSelectedMatCode(string objMatCode, string objMatDesc, string objMatSize, string objMatThickness, int LotSize)
        {
            try
            {
                return new DL_ItemSelection().DLSaveSelectedMatCode(objMatCode, objMatDesc, objMatSize, objMatThickness, LotSize);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string BLSaveHubQRCode(string objLocationCode, string objMatCode, string sMatDesc, string sGrade, string sGroup, string sThickness, string sSize, string objQRCode, string sMatStatus, string sDateFormat, string sPrintSection, string sLocType, string oPONo, string oVendorCode, string oLabelType)
        {
            try
            {
                return new DL_ItemSelection().DLSaveHubQRCode(objLocationCode, objMatCode, sMatDesc, sGrade, sGroup, sThickness, sSize, objQRCode, sMatStatus, sDateFormat, sPrintSection, sLocType, oPONo, oVendorCode, oLabelType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public string BLSaveStackQRCode(string objLocationCode, string objMatCode, string objStackQRCode, string sDateFormat, string sPrintingSection, string sLocationType, string MatStatus)
        {
            try
            {
                return new DL_ItemSelection().DLSaveStackQRCode(objLocationCode, objMatCode, objStackQRCode, sDateFormat, sPrintingSection, sLocationType, MatStatus);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BLUpdateQRCodeSAPStatus(string sLocationCode, string sMatCode, string sQRCode, string sStatus, string sPostMsg)
        {
            try
            {
                return new DL_ItemSelection().DLUpdateQRCodeSAPStatus(sLocationCode, sMatCode, sQRCode, sStatus, sPostMsg);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BLSavePRDStackQRCode(string objLocationCode, string objStackQRCode, string sDateFormat, string sPrintingSection, string sLocationType)
        {
            try
            {
                return new DL_ItemSelection().DLSavePRDStackQRCode(objLocationCode, objStackQRCode, sDateFormat, sPrintingSection, sLocationType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        //public DataTable BLGetPrintedStackQRCodes(string oLocationCode, string fromdate, string todate)
        //{
        //    try
        //    {
        //        return new DL_ItemSelection().DLGetPrintedStackQRCodes(oLocationCode, fromdate, todate);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}



        public string BLVGetQRCodeRunningSerialNo(string DateFormat, string sPrintingSection, string sLocationType)
        {
            try
            {
                return new DL_ItemSelection().DLVGetQRCodeRunningSerialNo(DateFormat, sPrintingSection, sLocationType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string BLVGetStackRunningSerialNo(string DateFormat, string sPrintingSection, string sLocationType)
        {
            try
            {
                return new DL_ItemSelection().DLVGetStackRunningSerialNo(DateFormat, sPrintingSection, sLocationType);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        #region Reprinting

        public DataTable BLGetPrintedStackQRCodesData(PLReprinting plActivity)
        {
            try
            {
                return new DL_ItemSelection().DLGetPrintedStackQRCodesData(plActivity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ObservableCollection<PLReprinting> BLGetSelectedStackDetails(PLReprinting _objPLReprinting)
        {
            try
            {
                return new DL_ItemSelection().DLGetSelectedStackDetails(_objPLReprinting);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ObservableCollection<PLReprinting> BLGetDecorSelectedStackDetails(PLReprinting _objPLReprinting)
        {
            try
            {
                return new DL_ItemSelection().DLGetDecorSelectedStackDetails(_objPLReprinting);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetSelectedStackDetails(string StackQRCode)
        {
            try
            {
                return new DL_ItemSelection().DLGetSelectedStackDetails(StackQRCode);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Hub Reprinting

        public DataTable BLGetPrintedHUBPOData(PLReprinting plActivity)
        {
            try
            {
                return new DL_ItemSelection().DLGetPrintedHUBPOData(plActivity);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DataTable BLGetSelectedPOMatDetails(string sPONO)
        {
            try
            {
                return new DL_ItemSelection().DLGetSelectedPOMatDetails(sPONO);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ObservableCollection<PLReprinting> BLGetSelectedMatDetails(PLReprinting _objPLReprinting)
        {
            try
            {
                return new DL_ItemSelection().DLGetSelectedMatDetails(_objPLReprinting);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region

        public ObservableCollection<PLReprinting> BLGetRejectionDetails()
        {
            try
            {
                return new DL_ItemSelection().DLGetRejectionDetails();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
    }
}
