using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ENTITY_LAYER
{
    public class PL_DepotMaster
    {
        public bool IsValid { get; set; }
        public string LocationCode { get; set; }
        public string DepotId { get; set; }
        public string DepotDesc { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
    }
}
