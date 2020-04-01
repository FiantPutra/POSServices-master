using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class CashierShift
    {
        public int Id { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public decimal? OpeningBalance { get; set; }
        public decimal? ClosingBalance { get; set; }
        public DateTime? OpeningTime { get; set; }
        public DateTime? ClosingTime { get; set; }
        public string DeviceName { get; set; }
        public string ShiftName { get; set; }
        public string ShiftCode { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string CashierShiftId { get; set; }
    }
}
