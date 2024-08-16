using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using SAP.Middleware.Connector;
using System.Data;


namespace DataScheduler
{
    public class clsLogic
    {
        
        #region Masters

        //public DataSet fnPIGetMaterialMaster(string LastUpdatedDate)
        //{

        //    DataSet ds = new DataSet();
        //    ds.Tables.Add(new DataTable());
        //    ds.Tables.Add(new DataTable());
        //    try
        //    {
        //        ZBCD_WSClient zbcd = new ZBCD_WSClient();

        //        zbcd.ClientCredentials.UserName.UserName = Properties.Settings.Default.WebServiceUserID;
        //        zbcd.ClientCredentials.UserName.Password = Properties.Settings.Default.WebServiceUserPassword;

        //        ZBcdSap2wmsSapMaterials master1 = new ZBcdSap2wmsSapMaterials();
        //        ZBcdSap2wmsSapMaterialsRequest request = new ZBcdSap2wmsSapMaterialsRequest();
        //        ZBcdSap2wmsSapMaterialsResponse response = new ZBcdSap2wmsSapMaterialsResponse();
        //        request.ZBcdSap2wmsSapMaterials = master1;
        //        request.ZBcdSap2wmsSapMaterials.FromDate = LastUpdatedDate;
        //        request.ZBcdSap2wmsSapMaterials.ToDate = DateTime.Now.ToString("yyyy-MM-dd");
        //        response = zbcd.ZBcdSap2wmsSapMaterials(request.ZBcdSap2wmsSapMaterials);

        //        if (response.TbSapMaterials != null)
        //        {
        //            ds.Tables[1].Columns.Add("MATERIAL_TYPE");
        //            ds.Tables[1].Columns.Add("DIVISION");
        //            ds.Tables[1].Columns.Add("MATERIAL_GROUP_DESC");
        //            ds.Tables[1].Columns.Add("MODEL_CODE_CAT");
        //            ds.Tables[1].Columns.Add("OLD_MATERIAL_NUMBER");
        //            ds.Tables[1].Columns.Add("MATERIAL_CODE");
        //            ds.Tables[1].Columns.Add("MODEL_CODE");
        //            ds.Tables[1].Columns.Add("MATERIAL_DESCRIPTION");
        //            ds.Tables[1].Columns.Add("BASE_UOM");
        //            ds.Tables[1].Columns.Add("ALT_UOM_STD_PKG");
        //            ds.Tables[1].Columns.Add("ALT_UOM_STD_PKG_QTY");
        //            ds.Tables[1].Columns.Add("SALES_UNIT");
        //            ds.Tables[1].Columns.Add("PUR_ORD_UNIT");
        //            ds.Tables[1].Columns.Add("BATCH_MANAGED");
        //            ds.Tables[1].Columns.Add("HSN_CODE");
        //            ds.Tables[1].Columns.Add("CRITICAL_PART");
        //            ds.Tables[1].Columns.Add("CATEGORY_CODE");
        //            ds.Tables[1].Columns.Add("PROD_CATEGORY");
        //            ds.Tables[1].Columns.Add("PROD_NAME");
        //            ds.Tables[1].Columns.Add("MODEL_NAME");
        //            ds.Tables[1].Columns.Add("COLOR");
        //            ds.Tables[1].Columns.Add("NET_WEIGHT");
        //            ds.Tables[1].Columns.Add("WEIGHT_UNIT");
        //            ds.Tables[1].Columns.Add("SIZE_DIMENSIONS_MM");
        //            ds.Tables[1].Columns.Add("THICKNESS");
        //            ds.Tables[1].Columns.Add("TANK_CAPACITY");
        //            ds.Tables[1].Columns.Add("RATED_VOLTAGE");
        //            ds.Tables[1].Columns.Add("WATTAGE");
        //            ds.Tables[1].Columns.Add("RPM");
        //            ds.Tables[1].Columns.Add("EAN_CODE");
        //            ds.Tables[1].Columns.Add("HEAT_EXCHANGER_WEIGHT");
        //            ds.Tables[1].Columns.Add("GAS_TYPE");
        //            ds.Tables[1].Columns.Add("WORKING_PRESSURE");
        //            ds.Tables[1].Columns.Add("NO_OF_BURNERS");
        //            ds.Tables[1].Columns.Add("IGNITION_TYPE");
        //            ds.Tables[1].Columns.Add("MAX_SUCTION_M3_HR");
        //            ds.Tables[1].Columns.Add("ENERGY_RATING");
        //            ds.Tables[1].Columns.Add("WARRANTY");
        //            ds.Tables[1].Columns.Add("MAT_BLOCK");
        //            ds.Tables[1].Columns.Add("MBLOCK_EFEC_DATE");
        //            ds.Tables[1].Columns.Add("MATL_STATISTICS_GRP");
        //            ds.Tables[1].Columns.Add("VOLUME_REBATE_GROUP");
        //            ds.Tables[1].Columns.Add("MATERIAL_PRICING_GRP");
        //            ds.Tables[1].Columns.Add("COMMISSION_GROUP");
        //            ds.Tables[1].Columns.Add("MATERIAL_GROUP1");
        //            ds.Tables[1].Columns.Add("MATERIAL_GROUP2");
        //            ds.Tables[1].Columns.Add("ABC_INDICATOR");
        //            ds.Tables[1].Columns.Add("MAT_CLASS_FAST_SLOW");
        //            ds.Tables[1].Columns.Add("MAT_BARCODE_STATUS");

        //            ds.Tables[1].Columns.Add("NO_OF_PLACE_SETTING");
        //            ds.Tables[1].Columns.Add("NOISE_LEVEL");
        //            ds.Tables[1].Columns.Add("MICROWAVE_INPUT");
        //            ds.Tables[1].Columns.Add("MICROWAVE_OUTPUT");
        //            ds.Tables[1].Columns.Add("GRILL_WATTAGE");
        //            ds.Tables[1].Columns.Add("CONVECTION_WATTAGE");
        //            ds.Tables[1].Columns.Add("NET_CONTENTS");
        //            ds.Tables[1].Columns.Add("SPARE_PRICE");

        //            foreach (var items in response.TbSapMaterials)
        //            {
        //                DataRow dr = ds.Tables[1].NewRow();
        //                dr["MATERIAL_TYPE"] = Convert.ToString(Convert.ToString(items.MaterialType) == null ? "" : Convert.ToString(items.MaterialType));
        //                dr["DIVISION"] = Convert.ToString(Convert.ToString(items.Division) == null ? "" : Convert.ToString(items.Division));
        //                dr["MATERIAL_GROUP_DESC"] = Convert.ToString(Convert.ToString(items.MaterialGroupDesc) == null ? "" : Convert.ToString(items.MaterialGroupDesc));
        //                dr["MODEL_CODE_CAT"] = Convert.ToString(Convert.ToString(items.ModelCodeCat) == null ? "" : Convert.ToString(items.ModelCodeCat));
        //                dr["MATERIAL_CODE"] = Convert.ToString(Convert.ToString(items.SapMaterial) == null ? "" : Convert.ToString(items.SapMaterial));
        //                dr["MATERIAL_DESCRIPTION"] = Convert.ToString(Convert.ToString(items.MaterialDescription) == null ? "" : Convert.ToString(items.MaterialDescription));
        //                dr["BASE_UOM"] = Convert.ToString(Convert.ToString(items.BaseUom) == null ? "" : Convert.ToString(items.BaseUom));
        //                dr["ALT_UOM_STD_PKG"] = Convert.ToString(Convert.ToString(items.AltUomStdPkg) == null ? "" : Convert.ToString(items.AltUomStdPkg));
        //                dr["ALT_UOM_STD_PKG_QTY"] = Convert.ToString(Convert.ToString(items.AltUomStdPkgQty) == null ? "" : Convert.ToString(items.AltUomStdPkgQty));
        //                dr["PUR_ORD_UNIT"] = Convert.ToString(Convert.ToString(items.PurOrdUnit) == null ? "" : Convert.ToString(items.PurOrdUnit));
        //                dr["BATCH_MANAGED"] = Convert.ToString(Convert.ToString(items.BatchManaged) == null ? "" : Convert.ToString(items.BatchManaged));
        //                dr["HSN_CODE"] = Convert.ToString(Convert.ToString(items.HsnCode) == null ? "" : Convert.ToString(items.HsnCode));
        //                dr["CRITICAL_PART"] = Convert.ToString(Convert.ToString(items.CriticalPart) == null ? "" : Convert.ToString(items.CriticalPart));
        //                dr["PROD_CATEGORY"] = Convert.ToString(Convert.ToString(items.ProdCategory) == null ? "" : Convert.ToString(items.ProdCategory));
        //                dr["PROD_NAME"] = Convert.ToString(Convert.ToString(items.ProdName) == null ? "" : Convert.ToString(items.ProdName));
        //                dr["MODEL_NAME"] = Convert.ToString(Convert.ToString(items.ModelName) == null ? "" : Convert.ToString(items.ModelName));
        //                dr["COLOR"] = Convert.ToString(Convert.ToString(items.CpdColour) == null ? "" : Convert.ToString(items.CpdColour));
        //                dr["NET_WEIGHT"] = Convert.ToString(Convert.ToString(items.NetWeight) == null ? "" : Convert.ToString(items.NetWeight));
        //                dr["WEIGHT_UNIT"] = Convert.ToString(Convert.ToString(items.WeightUnit) == null ? "" : Convert.ToString(items.WeightUnit));
        //                dr["SIZE_DIMENSIONS_MM"] = Convert.ToString(Convert.ToString(items.SizeDimensionsMm) == null ? "" : Convert.ToString(items.SizeDimensionsMm));
        //                dr["THICKNESS"] = Convert.ToString(Convert.ToString(items.Thickness) == null ? "" : Convert.ToString(items.Thickness));
        //                dr["TANK_CAPACITY"] = Convert.ToString(Convert.ToString(items.TankCapacity) == null ? "" : Convert.ToString(items.TankCapacity));
        //                dr["RATED_VOLTAGE"] = Convert.ToString(Convert.ToString(items.RatedVoltage) == null ? "" : Convert.ToString(items.RatedVoltage));
        //                dr["WATTAGE"] = Convert.ToString(Convert.ToString(items.Wattage) == null ? "" : Convert.ToString(items.Wattage));
        //                dr["RPM"] = Convert.ToString(Convert.ToString(items.SpeedInRpm) == null ? "" : Convert.ToString(items.SpeedInRpm));
        //                dr["EAN_CODE"] = Convert.ToString(Convert.ToString(items.EanCode) == null ? "" : Convert.ToString(items.EanCode));
        //                dr["HEAT_EXCHANGER_WEIGHT"] = Convert.ToString(Convert.ToString(items.HeatExchangerWeight) == null ? "" : Convert.ToString(items.HeatExchangerWeight));
        //                dr["GAS_TYPE"] = Convert.ToString(Convert.ToString(items.GasType) == null ? "" : Convert.ToString(items.GasType));
        //                dr["WORKING_PRESSURE"] = Convert.ToString(Convert.ToString(items.WorkingPressure) == null ? "" : Convert.ToString(items.WorkingPressure));
        //                dr["NO_OF_BURNERS"] = Convert.ToString(Convert.ToString(items.NoOfBurners) == null ? "" : Convert.ToString(items.NoOfBurners));
        //                dr["IGNITION_TYPE"] = Convert.ToString(Convert.ToString(items.IgnitionType) == null ? "" : Convert.ToString(items.IgnitionType));
        //                dr["MAX_SUCTION_M3_HR"] = Convert.ToString(Convert.ToString(items.MaxSuction) == null ? "" : Convert.ToString(items.MaxSuction));
        //                dr["ENERGY_RATING"] = Convert.ToString(Convert.ToString(items.EnergyRating) == null ? "" : Convert.ToString(items.EnergyRating));
        //                dr["WARRANTY"] = Convert.ToString(Convert.ToString(items.Warranty) == null ? "" : Convert.ToString(items.Warranty));
        //                dr["MAT_BLOCK"] = Convert.ToString(Convert.ToString(items.MatBlock) == null ? "" : Convert.ToString(items.MatBlock));
        //                dr["MBLOCK_EFEC_DATE"] = Convert.ToString(Convert.ToString(items.MblockEfecDate) == null ? "" : Convert.ToString(items.MblockEfecDate));
        //                dr["ABC_INDICATOR"] = Convert.ToString(Convert.ToString(items.AbcIndicator) == null ? "" : Convert.ToString(items.AbcIndicator));
        //                dr["MAT_CLASS_FAST_SLOW"] = Convert.ToString(Convert.ToString(items.MatClassFastSlow) == null ? "" : Convert.ToString(items.MatClassFastSlow));
        //                dr["MAT_BARCODE_STATUS"] = Convert.ToString(Convert.ToString(items.MatBarCodeStatus) == null ? "" : Convert.ToString(items.MatBarCodeStatus));
        //                dr["NO_OF_PLACE_SETTING"] = Convert.ToString(Convert.ToString(items.NoOfPlaceSetting) == null ? "" : Convert.ToString(items.NoOfPlaceSetting));
        //                dr["NOISE_LEVEL"] = Convert.ToString(Convert.ToString(items.NoiseLevel) == null ? "" : Convert.ToString(items.NoiseLevel));
        //                dr["MICROWAVE_INPUT"] = Convert.ToString(Convert.ToString(items.MicrowaveInput) == null ? "" : Convert.ToString(items.MicrowaveInput));
        //                dr["MICROWAVE_OUTPUT"] = Convert.ToString(Convert.ToString(items.MicrowaveOutput) == null ? "" : Convert.ToString(items.MicrowaveOutput));
        //                dr["GRILL_WATTAGE"] = Convert.ToString(Convert.ToString(items.GrillWattage) == null ? "" : Convert.ToString(items.GrillWattage));
        //                dr["CONVECTION_WATTAGE"] = Convert.ToString(Convert.ToString(items.ConvectionWattage) == null ? "" : Convert.ToString(items.ConvectionWattage));
        //                dr["NET_CONTENTS"] = Convert.ToString(Convert.ToString(items.NetContents) == null ? "" : Convert.ToString(items.NetContents));
        //                dr["SPARE_PRICE"] = Convert.ToString(Convert.ToString(items.SparePrice) == null ? "" : Convert.ToString(items.SparePrice));

        //                //clsGlobal.AppLog.WriteLog("Data Scheduler :: fnPIGetMaterialMaster : Item Code : " + dr["ItemCode"] + " Description : " + dr["ItemDescription"] + " UOM : " + dr["UOM"] + " ");
        //                ds.Tables[1].Rows.Add(dr);
        //            }
        //        }
        //    }
        //    catch (Exception exSap)
        //    {
        //        clsGlobal.AppLog.WriteLog("fnPIGetMaterialMaster::" + "PI Transaction Error :: " + exSap.Message.ToString());
        //    }

        //    finally
        //    {

        //    }
        //    return ds;
        //}

        //public DataSet fnPIGetPlantMaster()
        //{
        //    DataSet ds = new DataSet();
        //    ds.Tables.Add(new DataTable());
        //    try
        //    {
        //        ZBCD_WSClient zbcd = new ZBCD_WSClient();

        //        zbcd.ClientCredentials.UserName.UserName = Properties.Settings.Default.WebServiceUserID;
        //        zbcd.ClientCredentials.UserName.Password = Properties.Settings.Default.WebServiceUserPassword;

        //        ZBcdSap2wmsPlantMaster master1 = new ZBcdSap2wmsPlantMaster();
        //        ZBcdSap2wmsPlantMasterResponse response = new ZBcdSap2wmsPlantMasterResponse();
        //        ZBcdSap2wmsPlantMasterResponse1 response1 = new ZBcdSap2wmsPlantMasterResponse1();
        //        ZBcdSap2wmsPlantMasterRequest request = new ZBcdSap2wmsPlantMasterRequest();
        //        request.ZBcdSap2wmsPlantMaster = master1;
        //        response = zbcd.ZBcdSap2wmsPlantMaster(request.ZBcdSap2wmsPlantMaster);
                
        //        if (response.TbPlantMaster != null)
        //        {
        //            //clsGlobal.AppLog.WriteLog("Data Scheduler :: fnPIGetStorageLocationMaster : Data received from SAP : Date : " + ds.Tables[0].Rows[0][0] + " Status : " + ds.Tables[0].Rows[0][1] + " No Of Records : " + response.TbStorageLocation.Length + " ");
        //            ds.Tables[0].Columns.Add("PLANT_CODE");
        //            ds.Tables[0].Columns.Add("CITY");
        //            ds.Tables[0].Columns.Add("COUNTRY");
        //            ds.Tables[0].Columns.Add("EMAIL");
        //            ds.Tables[0].Columns.Add("GSTIN");
        //            ds.Tables[0].Columns.Add("NAME");
        //            ds.Tables[0].Columns.Add("STATE");
        //            ds.Tables[0].Columns.Add("STREET2");
        //            ds.Tables[0].Columns.Add("STREET3");
        //            ds.Tables[0].Columns.Add("STREET4");
        //            ds.Tables[0].Columns.Add("STREET5");
        //            ds.Tables[0].Columns.Add("STREET_HNO");
        //            ds.Tables[0].Columns.Add("TELNO");
        //            foreach (var items in response.TbPlantMaster)
        //            {
        //                DataRow dr = ds.Tables[0].NewRow();
        //                dr["PLANT_CODE"] = Convert.ToString(Convert.ToString(items.PlantCode) == null ? "" : Convert.ToString(items.PlantCode));
        //                dr["CITY"] = Convert.ToString(Convert.ToString(items.City) == null ? "" : Convert.ToString(items.City));
        //                dr["COUNTRY"] = Convert.ToString(Convert.ToString(items.Country) == null ? "" : Convert.ToString(items.Country));
        //                dr["EMAIL"] = Convert.ToString(Convert.ToString(items.Email) == null ? "" : Convert.ToString(items.Email));
        //                dr["GSTIN"] = Convert.ToString(Convert.ToString(items.Gstin) == null ? "" : Convert.ToString(items.Gstin));
        //                dr["NAME"] = Convert.ToString(Convert.ToString(items.Name) == null ? "" : Convert.ToString(items.Name));
        //                dr["STATE"] = Convert.ToString(Convert.ToString(items.State) == null ? "" : Convert.ToString(items.State));
        //                dr["STREET2"] = Convert.ToString(Convert.ToString(items.Street2) == null ? "" : Convert.ToString(items.Street2));
        //                dr["STREET3"] = Convert.ToString(Convert.ToString(items.Street3) == null ? "" : Convert.ToString(items.Street3));
        //                dr["STREET4"] = Convert.ToString(Convert.ToString(items.Street4) == null ? "" : Convert.ToString(items.Street4));
        //                dr["STREET5"] = Convert.ToString(Convert.ToString(items.Street5) == null ? "" : Convert.ToString(items.Street5));
        //                dr["STREET_HNO"] = Convert.ToString(Convert.ToString(items.StreetHno) == null ? "" : Convert.ToString(items.StreetHno));
        //                dr["TELNO"] = Convert.ToString(Convert.ToString(items.TelNo) == null ? "" : Convert.ToString(items.TelNo));
        //                //clsGlobal.AppLog.WriteLog("Data Scheduler :: fnPIGetStorageLocationMaster : Plant Code : " + dr["PLANT_CODE"] + " PLANT NAME : " + dr["NAME"] + " PLANT CITY : " + dr["CITY"] + " ");
        //                ds.Tables[0].Rows.Add(dr);
        //            }
        //        }
        //    }

        //    catch (Exception exSap)
        //    {
        //        clsGlobal.AppLog.WriteLog("fnPIGetStorageLocationMaster ::" + " PI Transaction Error :: " + exSap.Message.ToString());
        //    }

        //    finally
        //    {

        //    }
        //    return ds;
        //}

        //public DataSet fnPIGetCompanyMaster()
        //{
        //    DataSet ds = new DataSet();
        //    ds.Tables.Add(new DataTable());
        //    try
        //    {
        //        ZBCD_WSClient zbcd = new ZBCD_WSClient();

        //        zbcd.ClientCredentials.UserName.UserName = Properties.Settings.Default.WebServiceUserID;
        //        zbcd.ClientCredentials.UserName.Password = Properties.Settings.Default.WebServiceUserPassword;

        //        ZBcdSap2wmsCoMaster master1 = new ZBcdSap2wmsCoMaster();
        //        ZBcdSap2wmsCoMasterResponse response = new ZBcdSap2wmsCoMasterResponse();
        //        ZBcdSap2wmsCoMasterResponse1 response1 = new ZBcdSap2wmsCoMasterResponse1();
        //        ZBcdSap2wmsCoMasterRequest request = new ZBcdSap2wmsCoMasterRequest();
        //        request.ZBcdSap2wmsCoMaster = master1;
        //        response = zbcd.ZBcdSap2wmsCoMaster(request.ZBcdSap2wmsCoMaster);

        //        if (response.TbCoCode != null)
        //        {
        //            //clsGlobal.AppLog.WriteLog("Data Scheduler :: fnPIGetStorageLocationMaster : Data received from SAP : Date : " + ds.Tables[0].Rows[0][0] + " Status : " + ds.Tables[0].Rows[0][1] + " No Of Records : " + response.TbStorageLocation.Length + " ");
        //            ds.Tables[0].Columns.Add("COMCODE");
        //            ds.Tables[0].Columns.Add("CITY");
        //            ds.Tables[0].Columns.Add("COUNTRY");
        //            ds.Tables[0].Columns.Add("EMAIL");
        //            ds.Tables[0].Columns.Add("CIN");
        //            ds.Tables[0].Columns.Add("NAME");
        //            ds.Tables[0].Columns.Add("STATE");
        //            ds.Tables[0].Columns.Add("STREET2");
        //            ds.Tables[0].Columns.Add("STREET_HNO");
        //            ds.Tables[0].Columns.Add("TELNO");
        //            foreach (var items in response.TbCoCode)
        //            {
        //                DataRow dr = ds.Tables[0].NewRow();
        //                dr["COMCODE"] = Convert.ToString(Convert.ToString(items.CoCode) == null ? "" : Convert.ToString(items.CoCode));
        //                dr["CITY"] = Convert.ToString(Convert.ToString(items.City) == null ? "" : Convert.ToString(items.City));
        //                dr["COUNTRY"] = Convert.ToString(Convert.ToString(items.Country) == null ? "" : Convert.ToString(items.Country));
        //                dr["EMAIL"] = Convert.ToString(Convert.ToString(items.Email) == null ? "" : Convert.ToString(items.Email));
        //                dr["CIN"] = Convert.ToString(Convert.ToString(items.Cin) == null ? "" : Convert.ToString(items.Cin));
        //                dr["NAME"] = Convert.ToString(Convert.ToString(items.Name) == null ? "" : Convert.ToString(items.Name));
        //                dr["STATE"] = Convert.ToString(Convert.ToString(items.State) == null ? "" : Convert.ToString(items.State));
        //                dr["STREET2"] = Convert.ToString(Convert.ToString(items.Street2) == null ? "" : Convert.ToString(items.Street2));
        //                dr["STREET_HNO"] = Convert.ToString(Convert.ToString(items.StreetHno) == null ? "" : Convert.ToString(items.StreetHno));
        //                dr["TELNO"] = Convert.ToString(Convert.ToString(items.TelNo) == null ? "" : Convert.ToString(items.TelNo));
        //                //clsGlobal.AppLog.WriteLog("Data Scheduler :: fnPIGetStorageLocationMaster : Plant Code : " + dr["PLANT_CODE"] + " PLANT NAME : " + dr["NAME"] + " PLANT CITY : " + dr["CITY"] + " ");
        //                ds.Tables[0].Rows.Add(dr);
        //            }
        //        }
        //    }

        //    catch (Exception exSap)
        //    {
        //        clsGlobal.AppLog.WriteLog("fnPIGetStorageLocationMaster ::" + " PI Transaction Error :: " + exSap.Message.ToString());
        //    }
        //    finally
        //    {

        //    }
        //    return ds;
        //}

        //public DataSet fnPIGetCustomerMaster(string LastUpdatedDate)
        //{
        //    DataSet ds = new DataSet();
        //    ds.Tables.Add(new DataTable());
        //    try
        //    {
        //        ZBCD_WSClient zbcd = new ZBCD_WSClient();

        //        zbcd.ClientCredentials.UserName.UserName = Properties.Settings.Default.WebServiceUserID;
        //        zbcd.ClientCredentials.UserName.Password = Properties.Settings.Default.WebServiceUserPassword;

        //        ZBcdSap2wmsCustomerMaster master1 = new ZBcdSap2wmsCustomerMaster();
        //        ZBcdSap2wmsCustomerMasterResponse response = new ZBcdSap2wmsCustomerMasterResponse();
        //        ZBcdSap2wmsCustomerMasterResponse1 response1 = new ZBcdSap2wmsCustomerMasterResponse1();
        //        ZBcdSap2wmsCustomerMasterRequest request = new ZBcdSap2wmsCustomerMasterRequest();
        //        request.ZBcdSap2wmsCustomerMaster = master1;
        //        request.ZBcdSap2wmsCustomerMaster.ImFromDate = LastUpdatedDate;
        //        request.ZBcdSap2wmsCustomerMaster.ImToDate = DateTime.Now.ToString("yyyy-MM-dd");
        //        request.ZBcdSap2wmsCustomerMaster.AccountGroup = "ZSOL";
        //        response = zbcd.ZBcdSap2wmsCustomerMaster(request.ZBcdSap2wmsCustomerMaster);

        //        if (response.TbCustomerMaster != null)
        //        {
        //            //clsGlobal.AppLog.WriteLog("Data Scheduler :: fnPIGetStorageLocationMaster : Data received from SAP : Date : " + ds.Tables[0].Rows[0][0] + " Status : " + ds.Tables[0].Rows[0][1] + " No Of Records : " + response.TbStorageLocation.Length + " ");
        //            ds.Tables[0].Columns.Add("CUSTOMER_CODE");
        //            ds.Tables[0].Columns.Add("NAME");
        //            ds.Tables[0].Columns.Add("HOUSE_NUMBER");
        //            ds.Tables[0].Columns.Add("STREET_1");
        //            ds.Tables[0].Columns.Add("STREET_2");
        //            ds.Tables[0].Columns.Add("CITY");
        //            ds.Tables[0].Columns.Add("STATE_CODE");
        //            ds.Tables[0].Columns.Add("POST_CODE");
        //            ds.Tables[0].Columns.Add("COUNTRY");
        //            ds.Tables[0].Columns.Add("TEL_NO");
        //            ds.Tables[0].Columns.Add("EMAIL");
        //            ds.Tables[0].Columns.Add("MOB_NO");
        //            ds.Tables[0].Columns.Add("GSTIN");
        //            ds.Tables[0].Columns.Add("PAN");
        //            foreach (var items in response.TbCustomerMaster)
        //            {
        //                DataRow dr = ds.Tables[0].NewRow();
        //                dr["CITY"] = Convert.ToString(Convert.ToString(items.City) == null ? "" : Convert.ToString(items.City));
        //                dr["COUNTRY"] = Convert.ToString(Convert.ToString(items.Country) == null ? "" : Convert.ToString(items.Country));
        //                dr["CUSTOMER_CODE"] = Convert.ToString(Convert.ToString(items.CustomerCode) == null ? "" : Convert.ToString(items.CustomerCode));
        //                dr["NAME"] = Convert.ToString(Convert.ToString(items.CustomerName) == null ? "" : Convert.ToString(items.CustomerName));
        //                dr["EMAIL"] = Convert.ToString(Convert.ToString(items.Email) == null ? "" : Convert.ToString(items.Email));
        //                dr["GSTIN"] = Convert.ToString(Convert.ToString(items.Gstin) == null ? "" : Convert.ToString(items.Gstin));
        //                dr["HOUSE_NUMBER"] = Convert.ToString(Convert.ToString(items.HouseNumber) == null ? "" : Convert.ToString(items.HouseNumber));
        //                dr["MOB_NO"] = Convert.ToString(Convert.ToString(items.MobNo) == null ? "" : Convert.ToString(items.MobNo));
        //                dr["PAN"] = Convert.ToString(Convert.ToString(items.Pan) == null ? "" : Convert.ToString(items.Pan));
        //                dr["POST_CODE"] = Convert.ToString(Convert.ToString(items.PostCode) == null ? "" : Convert.ToString(items.PostCode));
        //                dr["STATE_CODE"] = Convert.ToString(Convert.ToString(items.StateCode) == null ? "" : Convert.ToString(items.StateCode));
        //                dr["STREET_1"] = Convert.ToString(Convert.ToString(items.Street1) == null ? "" : Convert.ToString(items.Street1));
        //                dr["STREET_2"] = Convert.ToString(Convert.ToString(items.Street2) == null ? "" : Convert.ToString(items.Street2));
        //                dr["TEL_NO"] = Convert.ToString(Convert.ToString(items.TelNumber) == null ? "" : Convert.ToString(items.TelNumber));
        //                //clsGlobal.AppLog.WriteLog("Data Scheduler :: fnPIGetStorageLocationMaster : Plant Code : " + dr["PLANT_CODE"] + " PLANT NAME : " + dr["NAME"] + " PLANT CITY : " + dr["CITY"] + " ");
        //                ds.Tables[0].Rows.Add(dr);
        //            }
        //        }
        //    }

        //    catch (Exception exSap)
        //    {
        //        clsGlobal.AppLog.WriteLog("fnPIGetStorageLocationMaster ::" + " PI Transaction Error :: " + exSap.Message.ToString());
        //    }

        //    finally
        //    {

        //    }
        //    return ds;
        //}

        //public DataSet fnPIGetVendorMaster(string LastUpdatedDate)
        //{
        //    DataSet ds = new DataSet();
        //    ds.Tables.Add(new DataTable());
        //    try
        //    {
        //        ZBCD_WSClient zbcd = new ZBCD_WSClient();

        //        zbcd.ClientCredentials.UserName.UserName = Properties.Settings.Default.WebServiceUserID;
        //        zbcd.ClientCredentials.UserName.Password = Properties.Settings.Default.WebServiceUserPassword;

        //        ZBcdSap2wmsVendorMaster master1 = new ZBcdSap2wmsVendorMaster();
        //        ZBcdSap2wmsVendorMasterResponse response = new ZBcdSap2wmsVendorMasterResponse();
        //        ZBcdSap2wmsVendorMasterResponse1 response1 = new ZBcdSap2wmsVendorMasterResponse1();
        //        ZBcdSap2wmsVendorMasterRequest request = new ZBcdSap2wmsVendorMasterRequest();
        //        request.ZBcdSap2wmsVendorMaster = master1;
        //        request.ZBcdSap2wmsVendorMaster.FromDate = LastUpdatedDate.Trim();
        //        request.ZBcdSap2wmsVendorMaster.ToDate = DateTime.Now.ToString("yyyy-MM-dd");
        //        response = zbcd.ZBcdSap2wmsVendorMaster(request.ZBcdSap2wmsVendorMaster);

        //        if (response.TbVendorMaster != null)
        //        {
        //            string message = response.Return.ToString(); ;
        //            //clsGlobal.AppLog.WriteLog("Data Scheduler :: fnPIGetStorageLocationMaster : Data received from SAP : Date : " + ds.Tables[0].Rows[0][0] + " Status : " + ds.Tables[0].Rows[0][1] + " No Of Records : " + response.TbStorageLocation.Length + " ");
        //            ds.Tables[0].Columns.Add("PUR_ORG");
        //            ds.Tables[0].Columns.Add("SAP_VENDOR_CODE");
        //            ds.Tables[0].Columns.Add("VENDOR_NAME");
        //            ds.Tables[0].Columns.Add("STREET_HNO");
        //            ds.Tables[0].Columns.Add("STREET1");
        //            ds.Tables[0].Columns.Add("STREET2");
        //            ds.Tables[0].Columns.Add("EMAIL");
        //            ds.Tables[0].Columns.Add("TEL_NO");
        //            ds.Tables[0].Columns.Add("MOB_NO");
        //            ds.Tables[0].Columns.Add("CITY");
        //            ds.Tables[0].Columns.Add("REGION_STATE");
        //            ds.Tables[0].Columns.Add("GST_REGD");
        //            ds.Tables[0].Columns.Add("GSTIN");
        //            ds.Tables[0].Columns.Add("PAN");
        //            ds.Tables[0].Columns.Add("MSME_TYPE");
        //            ds.Tables[0].Columns.Add("MSME_NO");
        //            ds.Tables[0].Columns.Add("VENDOR_TYPE");
        //            ds.Tables[0].Columns.Add("COUNTRY");
        //            ds.Tables[0].Columns.Add("CONTACT_PERSON");
        //            ds.Tables[0].Columns.Add("CONTACT_NUMBER");
        //            ds.Tables[0].Columns.Add("VENDOR_BLOCK");
        //            ds.Tables[0].Columns.Add("TYPE_OF_BUSINESS");
        //            ds.Tables[0].Columns.Add("POSTALCODE");

        //            foreach (var items in response.TbVendorMaster)
        //            {
        //                DataRow dr = ds.Tables[0].NewRow();
        //                dr["PUR_ORG"] = Convert.ToString(Convert.ToString(items.PurOrg) == null ? "" : Convert.ToString(items.PurOrg));
        //                dr["SAP_VENDOR_CODE"] = Convert.ToString(Convert.ToString(items.VendorCode) == null ? "" : Convert.ToString(items.VendorCode));
        //                dr["VENDOR_NAME"] = Convert.ToString(Convert.ToString(items.Name) == null ? "" : Convert.ToString(items.Name));
        //                dr["STREET_HNO"] = Convert.ToString(Convert.ToString(items.StreetHno) == null ? "" : Convert.ToString(items.StreetHno));
        //                dr["STREET1"] = Convert.ToString(Convert.ToString(items.Street1) == null ? "" : Convert.ToString(items.Street1));
        //                dr["STREET2"] = Convert.ToString(Convert.ToString(items.Street2) == null ? "" : Convert.ToString(items.Street2));
        //                dr["EMAIL"] = Convert.ToString(Convert.ToString(items.Email) == null ? "" : Convert.ToString(items.Email));
        //                dr["TEL_NO"] = Convert.ToString(Convert.ToString(items.TelNo) == null ? "" : Convert.ToString(items.TelNo));
        //                dr["MOB_NO"] = Convert.ToString(Convert.ToString(items.MobNo) == null ? "" : Convert.ToString(items.MobNo));
        //                dr["CITY"] = Convert.ToString(Convert.ToString(items.City) == null ? "" : Convert.ToString(items.City));
        //                dr["REGION_STATE"] = Convert.ToString(Convert.ToString(items.RegionState) == null ? "" : Convert.ToString(items.RegionState));
        //                dr["GST_REGD"] = Convert.ToString(Convert.ToString(items.GstRegd) == null ? "" : Convert.ToString(items.GstRegd));
        //                dr["GSTIN"] = Convert.ToString(Convert.ToString(items.Gstin) == null ? "" : Convert.ToString(items.Gstin));
        //                dr["PAN"] = Convert.ToString(Convert.ToString(items.Pan) == null ? "" : Convert.ToString(items.Pan));
        //                dr["MSME_TYPE"] = Convert.ToString(Convert.ToString(items.MsmeType) == null ? "" : Convert.ToString(items.MsmeType));
        //                dr["MSME_NO"] = Convert.ToString(Convert.ToString(items.MsmeNo) == null ? "" : Convert.ToString(items.MsmeNo));
        //                dr["VENDOR_TYPE"] = Convert.ToString(Convert.ToString(items.VendorType) == null ? "" : Convert.ToString(items.VendorType));
        //                dr["COUNTRY"] = Convert.ToString(Convert.ToString(items.Country) == null ? "" : Convert.ToString(items.Country));
        //                dr["CONTACT_PERSON"] = Convert.ToString(Convert.ToString(items.ContactPerson) == null ? "" : Convert.ToString(items.ContactPerson));
        //                dr["CONTACT_NUMBER"] = Convert.ToString(Convert.ToString(items.ContactNumber) == null ? "" : Convert.ToString(items.ContactNumber));
        //                dr["VENDOR_BLOCK"] = Convert.ToString(Convert.ToString(items.VendorBlock) == null ? "" : Convert.ToString(items.VendorBlock));
        //                dr["TYPE_OF_BUSINESS"] = Convert.ToString(Convert.ToString(items.TypeOfBusiness) == null ? "" : Convert.ToString(items.TypeOfBusiness));
        //                dr["POSTALCODE"] = Convert.ToString(Convert.ToString(items.PinCode) == null ? "" : Convert.ToString(items.PinCode));
        //                //clsGlobal.AppLog.WriteLog("Data Scheduler :: fnPIGetStorageLocationMaster : Plant Code : " + dr["PLANT_CODE"] + " PLANT NAME : " + dr["NAME"] + " PLANT CITY : " + dr["CITY"] + " ");
        //                ds.Tables[0].Rows.Add(dr);
        //            }
        //        }
        //    }

        //    catch (Exception exSap)
        //    {
        //        clsGlobal.AppLog.WriteLog("fnPIGetStorageLocationMaster ::" + " PI Transaction Error :: " + exSap.Message.ToString());
        //    }

        //    finally
        //    {

        //    }
        //    return ds;
        //}


        #endregion


        #region Transaction

        //public DataSet fnPIGetVendorPOData(string FromDate)  //string FromDate
        //{
        //    DataSet ds = new DataSet();
        //    ds.Tables.Add(new DataTable());
        //    try
        //    {
        //        ZBCD_WSClient zbcd = new ZBCD_WSClient();

        //        zbcd.ClientCredentials.UserName.UserName = Properties.Settings.Default.WebServiceUserID;
        //        zbcd.ClientCredentials.UserName.Password = Properties.Settings.Default.WebServiceUserPassword;

        //        ZBcdSapWmsPoDetails master1 = new ZBcdSapWmsPoDetails();
        //        ZBcdSapWmsPoDetailsResponse response = new ZBcdSapWmsPoDetailsResponse();
        //        ZBcdSapWmsPoDetailsResponse1 response1 = new ZBcdSapWmsPoDetailsResponse1();
        //        ZBcdSapWmsPoDetailsRequest request = new ZBcdSapWmsPoDetailsRequest();
        //        request.ZBcdSapWmsPoDetails = master1;
        //        request.ZBcdSapWmsPoDetails.ImFromDate = FromDate.ToString().Trim();
        //        request.ZBcdSapWmsPoDetails.ImToDate = DateTime.Now.ToString("yyyy-MM-dd");
        //        request.ZBcdSapWmsPoDetails.PurOrg = "2400";
                
        //        response = zbcd.ZBcdSapWmsPoDetails(request.ZBcdSapWmsPoDetails);
                
        //        if (response.TbPoDetails != null)
        //        {
        //            //clsGlobal.AppLog.WriteLog("Data Scheduler :: fnPIGetCostCenterMaster : Data received from SAP : Date : " + ds.Tables[0].Rows[0][0] + " Status : " + ds.Tables[0].Rows[0][1] + " No Of Records : " + response.TbCostCenter.Length + " ");
        //            ds.Tables[0].Columns.Add("PLANT_CODE");//
        //            ds.Tables[0].Columns.Add("PO_NUMBER");//
        //            ds.Tables[0].Columns.Add("PO_DATE");//
        //            ds.Tables[0].Columns.Add("VENDOR_CODE");//
        //            ds.Tables[0].Columns.Add("VENDOR_TYPE");//
        //            //ds.Tables[0].Columns.Add("PO_TYPE");
        //            ds.Tables[0].Columns.Add("PO_CATEGORY");//
        //            ds.Tables[0].Columns.Add("LINE_ITEM_NUMBER");//
        //            ds.Tables[0].Columns.Add("MATERIAL_CODE");//
        //            ds.Tables[0].Columns.Add("PO_QUANTITY");//
        //            //ds.Tables[0].Columns.Add("BASE_UOM");//
        //            //ds.Tables[0].Columns.Add("STD_PGK_QTY");
        //            ds.Tables[0].Columns.Add("ORDER_UOM");
        //            ds.Tables[0].Columns.Add("DELIVERY_DATE");//
        //            ds.Tables[0].Columns.Add("PO_ITEM_TEXT");//
        //            ds.Tables[0].Columns.Add("HSN_CODE");  //
        //            ds.Tables[0].Columns.Add("FREIGHT_VENDOR");//
        //            ds.Tables[0].Columns.Add("PO_RELEASE_STATUS");//
        //            ds.Tables[0].Columns.Add("RETURN_ITEM_FLAG");//

        //            foreach (var items in response.TbPoDetails)
        //            {
        //                DataRow dr = ds.Tables[0].NewRow();
        //                dr["DELIVERY_DATE"] = Convert.ToString(Convert.ToString(items.DeliveryDate) == null ? "" : Convert.ToString(items.DeliveryDate));
        //                dr["FREIGHT_VENDOR"] = Convert.ToString(Convert.ToString(items.FreightVendor) == null ? "" : Convert.ToString(items.FreightVendor));
        //                dr["HSN_CODE"] = Convert.ToString(Convert.ToString(items.HsnCode) == null ? "" : Convert.ToString(items.HsnCode));
        //                dr["LINE_ITEM_NUMBER"] = Convert.ToString(Convert.ToString(items.LineItemNumber) == null ? "" : Convert.ToString(items.LineItemNumber));
        //                dr["ORDER_UOM"] = Convert.ToString(Convert.ToString(items.OrderUnit) == null ? "" : Convert.ToString(items.OrderUnit));
        //                dr["PLANT_CODE"] = Convert.ToString(Convert.ToString(items.Plant) == null ? "" : Convert.ToString(items.Plant));
        //                dr["PO_CATEGORY"] = Convert.ToString(Convert.ToString(items.PoCategory) == null ? "" : Convert.ToString(items.PoCategory));
        //                dr["PO_DATE"] = Convert.ToString(Convert.ToString(items.PoDate) == null ? "" : Convert.ToString(items.PoDate));
        //                dr["PO_ITEM_TEXT"] = Convert.ToString(Convert.ToString(items.PoItemText) == null ? "" : Convert.ToString(items.PoItemText));
        //                dr["PO_NUMBER"] = Convert.ToString(Convert.ToString(items.PoNumber) == null ? "" : Convert.ToString(items.PoNumber));
        //                dr["PO_QUANTITY"] = Convert.ToString(Convert.ToString(items.Quantity) == null ? "" : Convert.ToString(items.Quantity));
        //                dr["PO_RELEASE_STATUS"] = Convert.ToString(Convert.ToString(items.ReleaseStatus) == null ? "" : Convert.ToString(items.ReleaseStatus));
        //                dr["RETURN_ITEM_FLAG"] = Convert.ToString(Convert.ToString(items.ReturnItemFlag) == null ? "" : Convert.ToString(items.ReturnItemFlag));
        //                dr["MATERIAL_CODE"] = Convert.ToString(Convert.ToString(items.SapMaterialCode) == null ? "" : Convert.ToString(items.SapMaterialCode));
        //                dr["VENDOR_CODE"] = Convert.ToString(Convert.ToString(items.VendorCode) == null ? "" : Convert.ToString(items.VendorCode));
        //                dr["VENDOR_TYPE"] = Convert.ToString(Convert.ToString(items.VendorType) == null ? "" : Convert.ToString(items.VendorType));
        //                //clsGlobal.AppLog.WriteLog("Data Scheduler :: fnPIGetCostCenterMaster : Plant Code : " + dr["PLANT_CODE"] + " PLANT NAME : " + dr["NAME"] + " PLANT CITY : " + dr["CITY"] + " ");
        //                ds.Tables[0].Rows.Add(dr);
        //            }
        //        }
        //    }

        //    catch (Exception exSap)
        //    {
        //        clsGlobal.AppLog.WriteLog("fnPIGetCostCenterMaster ::" + " PI Transaction Error :: " + exSap.Message.ToString());
        //    }

        //    finally
        //    {

        //    }
        //    return ds;
        //}  

        #endregion

    }
}
