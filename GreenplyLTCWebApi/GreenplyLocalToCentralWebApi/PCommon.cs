using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GreenplyLocalToCentralWebApi
{
    public class PCommon
    {
        //private static string _strsqlcon = ConfigurationManager.ConnectionStrings["CEATconfig"].ConnectionString;
        // private static string _strsqlcon = @"Data Source=180.151.246.50, 4222;Initial Catalog=CEATDB;User ID=b649;Password=bcil@123";
        static string[]  value = null;

        private static string _strsqlcon = ConfigurationManager.ConnectionStrings["BARCODE"].ConnectionString;
        public static string StrSqlCon
        {
            get { return _strsqlcon; }
            set
            {
                _strsqlcon = value;
            }
        }
    }
}
