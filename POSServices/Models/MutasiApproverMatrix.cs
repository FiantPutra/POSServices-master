using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class MutasiApproverMatrix
    {
        public int StoreMatrix { get; set; }
        public string ApproverCity { get; set; }
        public string ApproverRegional { get; set; }
        public string ApproverNational { get; set; }

        public virtual Store StoreMatrixNavigation { get; set; }
    }
}
