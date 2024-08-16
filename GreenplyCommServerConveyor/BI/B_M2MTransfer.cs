using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using GreenplyCommServer.Common;
using TEST;
using BCILCommServer;
using GreenplyCommServer;
//using GreenplyCommServer.GreenplyERPPostingService;
using System.IO;
using System.Data;

namespace GreenplyCommServer.BI
{
    class B_M2MTransfer
    {
        BcilNetwork _bcilNetwork = new BcilNetwork();
        public static int sCount = 0;
        string objLocationCode;
        string objQRCode = string.Empty;
        string objStackQRCode = string.Empty;
        string sPrintStatus = string.Empty;
        string sobjMatCode = string.Empty;
        string sMatStatus = string.Empty;
        string sGradeDesc = string.Empty;
        string sGroupDesc = string.Empty;
        string sThicknessDesc = string.Empty;
        string sMatSize = string.Empty;

        internal string GetM2MMatProducts()
        {
            DataTable dtProduct = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>");
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETMTMMATPRODUCT"),
                                   };
                dtProduct = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialToMaterialTransfer", parma);
                if (dtProduct.Columns.Contains("ErrorMessage") && dtProduct.Rows.Count > 0)
                {
                    _sResult = "GETMTMMATPRODUCT ~ ERROR ~ " + dtProduct.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtProduct.Columns.Contains("Product") && dtProduct.Rows.Count > 0)
                {
                    _sResult = "GETMTMMATPRODUCT ~ SUCCESS ~ " + GlobalVariable.DtToString(dtProduct);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETMTMMATPRODUCT ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string GetM2MMatGroups(string sProduct)
        {
            DataTable dtProduct = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>");
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETMTMMATGROUPS"),
                                        new SqlParameter("@Product", sProduct.Trim().ToString()),
                                   };
                dtProduct = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialToMaterialTransfer", parma);
                if (dtProduct.Columns.Contains("ErrorMessage") && dtProduct.Rows.Count > 0)
                {
                    _sResult = "GETMTMMATGROUPS ~ ERROR ~ " + dtProduct.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtProduct.Columns.Count > 0 && dtProduct.Rows.Count > 0)
                {
                    _sResult = "GETMTMMATGROUPS ~ SUCCESS ~ " + GlobalVariable.DtToString(dtProduct);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETMTMMATGROUPS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string GetM2MMatGrades(string sProduct, string sGroup)
        {
            DataTable dtProduct = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>");
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETMTMMATGRADES"),
                                        new SqlParameter("@Product", sProduct.Trim().ToString()),
                                        new SqlParameter("@Group", sGroup.Trim().ToString()),
                                   };
                dtProduct = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialToMaterialTransfer", parma);
                if (dtProduct.Columns.Contains("ErrorMessage") && dtProduct.Rows.Count > 0)
                {
                    _sResult = "GETMTMMATGRADES ~ ERROR ~ " + dtProduct.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtProduct.Columns.Count > 0 && dtProduct.Rows.Count > 0)
                {
                    _sResult = "GETMTMMATGRADES ~ SUCCESS ~ " + GlobalVariable.DtToString(dtProduct);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETMTMMATGRADES ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string GetM2MMatCategory(string sProduct, string sGroup, string sGrade)
        {
            DataTable dtProduct = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>");
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETMTMMATCATEGORIES"),
                                        new SqlParameter("@Product", sProduct.Trim().ToString()),
                                        new SqlParameter("@Group", sGroup.Trim().ToString()),
                                        new SqlParameter("@Grade", sGrade.Trim().ToString()),
                                   };
                dtProduct = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialToMaterialTransfer", parma);
                if (dtProduct.Columns.Contains("ErrorMessage") && dtProduct.Rows.Count > 0)
                {
                    _sResult = "GETMTMMATCATEGORIES ~ ERROR ~ " + dtProduct.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtProduct.Columns.Count > 0 && dtProduct.Rows.Count > 0)
                {
                    _sResult = "GETMTMMATCATEGORIES ~ SUCCESS ~ " + GlobalVariable.DtToString(dtProduct);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETMTMMATCATEGORIES ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string GetM2MMatThickness(string sProduct, string sGroup, string sGrade, string sCategory)
        {
            DataTable dtProduct = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>");
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETMTMMATTHICKNESS"),
                                        new SqlParameter("@Product", sProduct.Trim().ToString()),
                                        new SqlParameter("@Group", sGroup.Trim().ToString()),
                                        new SqlParameter("@Grade", sGrade.Trim().ToString()),
                                        new SqlParameter("@Category", sCategory.Trim().ToString()),
                                   };
                dtProduct = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialToMaterialTransfer", parma);
                if (dtProduct.Columns.Contains("ErrorMessage") && dtProduct.Rows.Count > 0)
                {
                    _sResult = "GETMTMMATTHICKNESS ~ ERROR ~ " + dtProduct.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtProduct.Columns.Count > 0 && dtProduct.Rows.Count > 0)
                {
                    _sResult = "GETMTMMATTHICKNESS ~ SUCCESS ~ " + GlobalVariable.DtToString(dtProduct);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETMTMMATTHICKNESS ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string GetM2MMatSize(string sProduct, string sGroup, string sGrade, string sCategory, string sThickness)
        {
            DataTable dtProduct = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>");
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETMTMMATSIZE"),
                                        new SqlParameter("@Product", sProduct.Trim().ToString()),
                                        new SqlParameter("@Group", sGroup.Trim().ToString()),
                                        new SqlParameter("@Grade", sGrade.Trim().ToString()),
                                        new SqlParameter("@Category", sCategory.Trim().ToString()),
                                        new SqlParameter("@Thickness", sThickness.Trim().ToString()),
                                   };
                dtProduct = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialToMaterialTransfer", parma);
                if (dtProduct.Columns.Contains("ErrorMessage") && dtProduct.Rows.Count > 0)
                {
                    _sResult = "GETMTMMATSIZE ~ ERROR ~ " + dtProduct.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtProduct.Columns.Count > 0 && dtProduct.Rows.Count > 0)
                {
                    _sResult = "GETMTMMATSIZE ~ SUCCESS ~ " + GlobalVariable.DtToString(dtProduct);
                    return _sResult;
                }
                else
                {
                    _sResult = "GETMTMMATSIZE ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string GetM2MMatDesignNo(string sProduct, string sGroup, string sGrade, string sCategory, string sThickness, string sSize)
        {
            DataTable dtProduct = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>");
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETMTMMATDESIGNNO"),
                                        new SqlParameter("@Product", sProduct.Trim().ToString()),
                                        new SqlParameter("@Group", sGroup.Trim().ToString()),
                                        new SqlParameter("@Grade", sGrade.Trim().ToString()),
                                        new SqlParameter("@Category", sCategory.Trim().ToString()),
                                        new SqlParameter("@Thickness", sThickness.Trim().ToString()),
                                        new SqlParameter("@Size", sSize.Trim().ToString()),
                                   };
                dtProduct = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialToMaterialTransfer", parma);
                if (dtProduct.Columns.Contains("ErrorMessage") && dtProduct.Rows.Count > 0)
                {
                    _sResult = "GETMTMMATDESIGNNO ~ ERROR ~ " + dtProduct.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtProduct.Columns.Count > 0 && dtProduct.Rows.Count > 0)
                {
                    if (dtProduct.Columns.Count > 0 && dtProduct.Rows[0][0].ToString().Contains("-"))
                    {
                        _sResult = "GETMTMMATDESIGNNO ~ SUCCESS";
                        return _sResult;
                    }
                    else
                    {
                        _sResult = "GETMTMMATDESIGNNO ~ SUCCESS ~ " + GlobalVariable.DtToString(dtProduct);
                        return _sResult;
                    }
                }
                else
                {
                    _sResult = "GETMTMMATDESIGNNO ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string GetM2MMatFinishCodes(string sProduct, string sGroup, string sGrade, string sCategory, string sThickness, string sSize, string sDesign)
        {
            DataTable dtProduct = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>");
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETMTMMATFINISHCODE"),
                                        new SqlParameter("@Product", sProduct.Trim().ToString()),
                                        new SqlParameter("@Group", sGroup.Trim().ToString()),
                                        new SqlParameter("@Grade", sGrade.Trim().ToString()),
                                        new SqlParameter("@Category", sCategory.Trim().ToString()),
                                        new SqlParameter("@Thickness", sThickness.Trim().ToString()),
                                        new SqlParameter("@Size", sSize.Trim().ToString()),
                                        new SqlParameter("@DesignNo", sDesign.Trim().ToString()),
                                   };
                dtProduct = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialToMaterialTransfer", parma);
                if (dtProduct.Columns.Contains("ErrorMessage") && dtProduct.Rows.Count > 0)
                {
                    _sResult = "GETMTMMATFINISHCODE ~ ERROR ~ " + dtProduct.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtProduct.Columns.Count > 0 && dtProduct.Rows.Count > 0)
                {
                    if (dtProduct.Columns.Count > 0 && dtProduct.Rows[0][0].ToString().Contains("-"))
                    {
                        _sResult = "GETMTMMATFINISHCODE ~ SUCCESS";
                        return _sResult;
                    }
                    else
                    {
                        _sResult = "GETMTMMATFINISHCODE ~ SUCCESS ~ " + GlobalVariable.DtToString(dtProduct);
                        return _sResult;
                    }
                }
                else
                {
                    _sResult = "GETMTMMATFINISHCODE ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string GetM2MMatVisionCodes(string sProduct, string sGroup, string sGrade, string sCategory, string sThickness, string sSize, string sDesign, string sFinish)
        {
            DataTable dtProduct = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>");
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETMTMMATVISIONPANELCODE"),
                                        new SqlParameter("@Product", sProduct.Trim().ToString()),
                                        new SqlParameter("@Group", sGroup.Trim().ToString()),
                                        new SqlParameter("@Grade", sGrade.Trim().ToString()),
                                        new SqlParameter("@Category", sCategory.Trim().ToString()),
                                        new SqlParameter("@Thickness", sThickness.Trim().ToString()),
                                        new SqlParameter("@Size", sSize.Trim().ToString()),
                                        new SqlParameter("@DesignNo", sDesign.Trim().ToString()),
                                        new SqlParameter("@FinishCode", sFinish.Trim().ToString()),
                                   };
                dtProduct = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialToMaterialTransfer", parma);
                if (dtProduct.Columns.Contains("ErrorMessage") && dtProduct.Rows.Count > 0)
                {
                    _sResult = "GETMTMMATVISIONPANELCODE ~ ERROR ~ " + dtProduct.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtProduct.Columns.Count > 0 && dtProduct.Rows.Count > 0)
                {
                    if (dtProduct.Columns.Count > 0 && dtProduct.Rows[0][0].ToString().Contains("-"))
                    {
                        _sResult = "GETMTMMATVISIONPANELCODE ~ SUCCESS";
                        return _sResult;
                    }
                    else
                    {
                        _sResult = "GETMTMMATVISIONPANELCODE ~ SUCCESS ~ " + GlobalVariable.DtToString(dtProduct);
                        return _sResult;
                    }
                }
                else
                {
                    _sResult = "GETMTMMATVISIONPANELCODE ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string GetM2MMatLippingCodes(string sProduct, string sGroup, string sGrade, string sCategory, string sThickness, string sSize, string sDesign, string sFinish, string sVision)
        {
            DataTable dtProduct = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>");
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETMTMMATLIPPINGCODE"),
                                        new SqlParameter("@Product", sProduct.Trim().ToString()),
                                        new SqlParameter("@Group", sGroup.Trim().ToString()),
                                        new SqlParameter("@Grade", sGrade.Trim().ToString()),
                                        new SqlParameter("@Category", sCategory.Trim().ToString()),
                                        new SqlParameter("@Thickness", sThickness.Trim().ToString()),
                                        new SqlParameter("@Size", sSize.Trim().ToString()),
                                        new SqlParameter("@DesignNo", sDesign.Trim().ToString()),
                                        new SqlParameter("@FinishCode", sFinish.Trim().ToString()),
                                        new SqlParameter("@VisionCode", sVision.Trim().ToString()),
                                   };
                dtProduct = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialToMaterialTransfer", parma);
                if (dtProduct.Columns.Contains("ErrorMessage") && dtProduct.Rows.Count > 0)
                {
                    _sResult = "GETMTMMATLIPPINGCODE ~ ERROR ~ " + dtProduct.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtProduct.Columns.Count > 0 && dtProduct.Rows.Count > 0)
                {
                    if (dtProduct.Columns.Count > 0 && dtProduct.Rows[0][0].ToString().Contains("-"))
                    {
                        _sResult = "GETMTMMATLIPPINGCODE ~ SUCCESS";
                        return _sResult;
                    }
                    else
                    {
                        _sResult = "GETMTMMATLIPPINGCODE ~ SUCCESS ~ " + GlobalVariable.DtToString(dtProduct);
                        return _sResult;
                    }
                }
                else
                {
                    _sResult = "GETMTMMATLIPPINGCODE ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string GetM2MMatGetSelectedMatDetails(string sProduct, string sGroup, string sGrade, string sCategory, string sThickness, string sSize, string sDesign, string sFinish, string sVision, string sLipping)
        {
            DataTable dtMat = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>");
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETMTMSELECTEDMATERIALDATA"),
                                        new SqlParameter("@Product", sProduct.Trim().ToString()),
                                        new SqlParameter("@Group", sGroup.Trim().ToString()),
                                        new SqlParameter("@Grade", sGrade.Trim().ToString()),
                                        new SqlParameter("@Category", sCategory.Trim().ToString()),
                                        new SqlParameter("@Thickness", sThickness.Trim().ToString()),
                                        new SqlParameter("@Size", sSize.Trim().ToString()),
                                        new SqlParameter("@DesignNo", sDesign.Trim().ToString()),
                                        new SqlParameter("@FinishCode", sFinish.Trim().ToString()),
                                        new SqlParameter("@VisionCode", sVision.Trim().ToString()),
                                        new SqlParameter("@LippingCode", sLipping.Trim().ToString()),
                                   };
                dtMat = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialToMaterialTransfer", parma);
                if (dtMat.Columns.Contains("ErrorMessage") && dtMat.Rows.Count > 0)
                {
                    _sResult = "GETMTMSELECTEDMATERIALDATA ~ ERROR ~ " + dtMat.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtMat.Columns.Count > 0 && dtMat.Rows.Count > 0)
                {
                    if (dtMat.Columns.Count > 0 && (dtMat.Rows[0][0].ToString() == "" || dtMat.Rows[0][0].ToString() == string.Empty))
                    {
                        _sResult = "GETMTMSELECTEDMATERIALDATA ~ SUCCESS";
                        return _sResult;
                    }
                    else
                    {
                        _sResult = "GETMTMSELECTEDMATERIALDATA ~ SUCCESS ~ " + GlobalVariable.DtToString(dtMat);
                        return _sResult;
                    }
                }
                else
                {
                    _sResult = "GETMTMSELECTEDMATERIALDATA ~ ERROR ~ " + "NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }

        internal string GetM2MScannedQRStatus(string sLocCode, string sQRCode)
        {
            DataTable dtProduct = new DataTable();
            string _sResult = string.Empty;
            VariableInfo.mAppLog.LogMessage(BcilLib.EventNotice.EventTypes.evtInfo, "Monitring", "Reqest data =>");
            try
            {
                SqlParameter[] parma = { 
                                        new SqlParameter("@Type","GETMTMSCANNEDQRCODESTATUS"),
                                        new SqlParameter("@LocationCode", sLocCode.Trim().ToString()),
                                        new SqlParameter("@QRCode", sQRCode.Trim().ToString()),
                                   };
                dtProduct = GlobalVariable._clsSql.GetDataUsingProcedure("USP_MaterialToMaterialTransfer", parma);
                if (dtProduct.Columns.Contains("ErrorMessage") && dtProduct.Rows.Count > 0)
                {
                    _sResult = "GETMTMSCANNEDQRCODESTATUS ~ ERROR ~ " + dtProduct.Rows[0][0].ToString();
                    return _sResult;
                }
                if (dtProduct.Columns.Count > 0 && dtProduct.Rows.Count > 0 && dtProduct.Rows[0][0].ToString() != "")
                {
                    _sResult = "GETMTMSCANNEDQRCODESTATUS ~ SUCCESS ~ " + GlobalVariable.DtToString(dtProduct);
                    return _sResult;
                }
                else if (dtProduct.Columns.Contains("ERROR") && dtProduct.Rows.Count > 0)
                {
                    _sResult = "GETMTMSCANNEDQRCODESTATUS ~ ERROR ~ " + dtProduct.Rows[0][0].ToString();
                }
                else
                {
                    _sResult = "GETMTMSCANNEDQRCODESTATUS ~ ERROR ~ NO DETAILS FOUND";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return _sResult;
        }
    }
}
