using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class InventoryTransaction
    {
        public long Id { get; set; }
        public int? StoreId { get; set; }
        public int? TransactionTypeId { get; set; }
        public string WarehouseOriginal { get; set; }
        public string WarehouseDestination { get; set; }
        public decimal? TotalQty { get; set; }
        public string TransactionTypeName { get; set; }
        public string TransactionId { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public string Remarks { get; set; }
        public DateTime? TransactionDate { get; set; }
        public int? StatusId { get; set; }
        public string Status { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime? RequestDeliveryDate { get; set; }
        public string Sjlama { get; set; }
        public string Reasons { get; set; }
        public DateTime? SyncDate { get; set; }
        public bool? ToBeConfirmed { get; set; }
        public bool? InforBypass { get; set; }
        public string DeliveryOrderNumber { get; set; }
        public string Urridn { get; set; }
        public int? MutasiType { get; set; }
        public string MutasiTypeName { get; set; }
        public string EmployeeToApprove { get; set; }
        public string EmployeeNameToApprove { get; set; }
        public bool? IsSyncToInfor { get; set; }
    }
}
