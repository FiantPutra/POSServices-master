using System;
using System.Collections.Generic;

namespace POSServices.PosMsgModels
{
    public partial class JobTabletoSynchDetailUpload
    {
        public long SynchDetail { get; set; }
        public long JobId { get; set; }
        public string StoreId { get; set; }
        public string TableName { get; set; }
        public string UploadPath { get; set; }
        public DateTime Synchdate { get; set; }
        public string CreateTable { get; set; }
        public int? RowFatch { get; set; }
    }

    public class syncUploadDetail
    {
        public int syncDetailsId { get; set; }
        public int JobId { get; set; }
        public string StoreId { get; set; }
        public string TableName { get; set; }
        public string UploadPath { get; set; }
        public DateTime Synchdate { get; set; }
        public string CreateTable { get; set; }
        public int RowFatch { get; set; }
        public int MinId { get; set; }
        public int MaxId { get; set; }
        public string TablePrimaryKey { get; set; }
        public string identityColumn { get; set; }
    }

    public class bracketSyncUploadDetail
    {
        public List<syncUploadDetail> uploadDetails { get; set; }
    }
}
