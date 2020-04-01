using System;
using System.Collections.Generic;

namespace POSServices.PosMsgModels
{
    public partial class JobTabletoSynchDetailErpintegration
    {
        public long SynchDetail { get; set; }
        public long JobId { get; set; }
        public string TableName { get; set; }
        public DateTime Synchdate { get; set; }
        public int? RowFatch { get; set; }
        public int? RowApplied { get; set; }
        public int? TableToSynchId { get; set; }
    }
}
