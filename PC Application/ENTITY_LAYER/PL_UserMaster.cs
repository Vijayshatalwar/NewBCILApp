using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ENTITY_LAYER
{
    public class PL_UserMaster
    {
        public bool IsValid { get; set; }
        public string USER_NAME { get; set; }
        public string USER_ID { get; set; }
        public string GroupID { get; set; }
        public string Password { get; set; }
        public string GroupName { get; set; }
        public string PlantCode { get; set; }
        public string USER_EMAIL { get; set; }
        public string CREATED_BY { get; set; }
        public string LOCATION_TYPE { get; set; }
        public string LOCATION_CODE { get; set; }
        public string MODIFIED_BY { get; set; }
        public string NewPswd { get; set; }
        public string USER_TYPE { get; set; }
        public string CREATED_ON { get; set; }
        public string MODIFIED_ON { get; set; }
        public string NEWPASSWORD { get; set; }
        public string CONFIRMPASSWORD { get; set; }
        public string PlantType { get; set; }
    }
}
