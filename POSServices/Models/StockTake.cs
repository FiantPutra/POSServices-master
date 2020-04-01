using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class StockTake
    {
        public int Id { get; set; }
        public string StoreId { get; set; }
        public DateTime Time { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }
}
