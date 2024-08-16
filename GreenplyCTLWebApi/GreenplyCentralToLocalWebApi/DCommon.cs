using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GreenplyCentralToLocalWebApi
{
    public class DCommon
    {
        PCommon oPCommon = new PCommon();
        public DBManager SqlDBProvider()
        {
            try
            {
                DBManager oManager = new DBManager(DataProvider.SqlServer, PCommon.StrSqlCon);
                return oManager;
            }
            catch (Exception ex)
            { throw ex; }
        }
    }
}
