using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class LogRecord
    {
        public int Id { get; set; }
        public DateTime? TimeStamp { get; set; }
        public string Tag { get; set; }
        public string Message { get; set; }
        public string TransactionId { get; set; }
    }
}
