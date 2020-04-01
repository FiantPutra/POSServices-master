using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class SequenceNumberLog
    {
        public int Id { get; set; }
        public string StoreCode { get; set; }
        public string LastNumberSequence { get; set; }
        public DateTime? Date { get; set; }
        public string TransactionType { get; set; }
        public string LastTransId { get; set; }
    }
}
