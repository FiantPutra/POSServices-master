using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POSServices.Models
{
    public class UploadSyncDetailTable
    {
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
    }

    public class bracketSyncUploadDetail
    {
        public List<syncUploadDetail> uploadDetails { get; set; }
    }
}
