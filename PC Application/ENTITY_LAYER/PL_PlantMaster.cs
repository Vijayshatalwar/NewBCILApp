using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ENTITY_LAYER
{
    public class PL_PlantMaster
    {
        public bool IsValid { get; set; }
        public string PlantCode { get; set; }
        public string PlantDesc { get; set; }
        public string StackPrintRequired { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
    }
}
