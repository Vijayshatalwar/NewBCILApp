using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ENTITY_LAYER
{
   public class PL_Group_Master
   {
       public bool IsValid { get; set; }
        public string GroupID { get; set; }
        public string GroupName { get; set; }
        public string GroupDesc { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string MODULE_DESC { get; set; }
        public string MODULE_ID { get; set; }
        public bool VIEW_RIGHTS { get; set; }
        public bool SAVE_RIGHTS { get; set; }
        public bool EDIT_RIGHTS { get; set; }
        public bool DELETE_RIGHTS { get; set; }
        public bool DOWNLOAD_RIGHTS { get; set; }
    }
}
