using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ENTITY_LAYER
{
    public class PL_WarehouseMaster
    {
        public bool IsValid { get; set; }
        public string WarehouseId { get; set; }
        public string WarehouseDesc { get; set; }
        public string WarehouseAdd { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
    }
}
