using System;
using System.Collections.Generic;
using System.Linq;
using System.Text; 
using System.Data;
using System.Data.SqlClient;
using COMMON_LAYER;
using ENTITY_LAYER; 

namespace DATA_ACCESS_LAYER
{
    public class DlCommon
    {
        StringBuilder sb = null;

        public DBManager DBProvider()
        {
            try
            {
                DBManager oManager = new DBManager(DataProvider.SqlServer, VariableInfo.mSqlCon);
                return oManager;
            }
            catch (Exception ex)
            { 
                throw ex; 
            }
        }

        public DBManager DBProviderCentral()
        {
            try
            {
                DBManager oManager = new DBManager(DataProvider.SqlServer, PlCommon.StrConCentral);
                return oManager;
            }
            catch (Exception ex)
            { throw ex; }
        }

        public void CreateLog(DBManager oDbm, string lModule, string lMethod, string lDescription)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("Insert into mLog(Module,Method,Description,CreatedBy,CreatedOn,Sysip,PlantCode) ");
            sb.Append("Values ");
            sb.Append("('" + lModule + "','" + lMethod + "','" + lDescription + "','" + PlCommon.gUser + "',GetDate(),'" + PlCommon.gSysip + "','" + PlCommon.gPlantCode + "') ");
            oDbm.ExecuteNonQuery(CommandType.Text, sb.ToString());
        }

        public static string GetSerialNo(string TransType, string TransPrefix, string sNo, DBManager dbManger)
        {
            dbManger.CreateParameters(4);
            dbManger.AddParameters(0, "@TransType", TransType);
            dbManger.AddParameters(1, "@TransPrefix", TransPrefix);
            dbManger.AddParameters(2, "@TransNo", sNo);
            dbManger.AddOutParameters(3, "@Serial", 10, 1);
            dbManger.ExecuteReader(System.Data.CommandType.StoredProcedure, "GETSerialNo");
            return dbManger.GetParameterValue(3);
        }

        public static void UpdateSerial(string TransType, string TransPrefix, DBManager dbManger)
        { 
            dbManger.ExecuteNonQuery(CommandType.Text, "UPDATE MSERIAL SET TransNo=ISNULL(TransNo,0)+1 WHERE TransType='" + TransType + "' AND TransPrefix='" + TransPrefix + "' AND TransYear=YEAR(GETDATE())"); 
        }


    }
}
