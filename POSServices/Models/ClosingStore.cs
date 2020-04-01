using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class ClosingStore
    {
        public int Id { get; set; }
        public string ClosingStoreId { get; set; }
        public string StoreCode { get; set; }
        public DateTime? OpeningTimeStamp { get; set; }
        public DateTime? ClosingTimeStamp { get; set; }
        public decimal? OpeningTransBal { get; set; }
        public decimal? ClosignTranBal { get; set; }
        public decimal? RealTransBal { get; set; }
        public decimal? DisputeTransBal { get; set; }
        public decimal? OpeningPettyCash { get; set; }
        public decimal? ClosingPettyCash { get; set; }
        public decimal? RealPettyCash { get; set; }
        public decimal? DisputePettyCash { get; set; }
        public decimal? OpeningDeposit { get; set; }
        public decimal? ClosingDeposit { get; set; }
        public decimal? RealDeposit { get; set; }
        public decimal? DisputeDeposit { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string DeviceName { get; set; }
        public string StatusClose { get; set; }
    }
}
