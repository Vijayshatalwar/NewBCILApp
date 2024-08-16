using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ENTITY_LAYER
{
    public class PL_EmailConfigMaster
    {
        public bool IsValid { get; set; }
        public string LocationCode { get; set; }
        public string SmtpHost { get; set; }
        public string EmailId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string PortNo { get; set; }
        public string Subject { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string ModifiedBy { get; set; }
        public string ModifiedOn { get; set; }
    }
}
