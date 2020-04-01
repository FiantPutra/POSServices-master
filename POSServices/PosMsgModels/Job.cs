using System;
using System.Collections.Generic;

namespace POSServices.PosMsgModels
{
    public partial class Job
    {
        public long JobId { get; set; }
        public string Description { get; set; }
        public int Synctype { get; set; }
        public string StoreCode { get; set; }
        public DateTime Synchdate { get; set; }
        public string TableName { get; set; }
        public DateTime LastSynch { get; set; }
        public string Statusjob { get; set; }
        public int? Erpjob { get; set; }
    }

    public class JobList
    {
        public List<Job> Jobs { get; set; }
    }
}
