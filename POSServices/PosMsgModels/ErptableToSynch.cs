using System;
using System.Collections.Generic;

namespace POSServices.PosMsgModels
{
    public partial class ErptableToSynch
    {
        public int Id { get; set; }
        public string TableName { get; set; }
        public string Sqlcommand { get; set; }
        public int Status { get; set; }
        public string TablePrimarykey { get; set; }
        public string IdentityColumn { get; set; }
    }
}
