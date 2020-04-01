using System;
using System.Collections.Generic;

namespace POSServices.PosMsgModels
{
    public partial class JobSynchDetailUploadStatus
    {
        public long SynchDetail { get; set; }
        public int Rowfatch { get; set; }
        public int RowApplied { get; set; }
        public int Status { get; set; }
        public Guid? Downloadsessionid { get; set; }
    }
}
