using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class TransactionCopyx
    {
        public long Id { get; set; }
        public int? StoreId { get; set; }
        public string StoreCode { get; set; }
        public int? CustomerId { get; set; }
        public string RecieptCode { get; set; }
        public int? EmployeeId { get; set; }
        public int? Spgid { get; set; }
        public decimal? TotalDiscount { get; set; }
        public decimal? TotalAmounTransaction { get; set; }
        public int? Qty { get; set; }
        public string MethodOfPayment { get; set; }
        public decimal? MarginTransaction { get; set; }
        public string TransactionId { get; set; }
        public string ShiftCode { get; set; }
        public int? TransactionType { get; set; }
        public decimal? Cash { get; set; }
        public decimal? Edc1 { get; set; }
        public decimal? Edc2 { get; set; }
        public decimal? Change { get; set; }
        public string Bank1 { get; set; }
        public string Bank2 { get; set; }
        public string NoRef1 { get; set; }
        public string NoRef2 { get; set; }
        public int? VoucherId { get; set; }
        public string VoucherCode { get; set; }
        public decimal? Voucher { get; set; }
        public DateTime? TransactionDate { get; set; }
        public TimeSpan? TransactionTime { get; set; }
        public string EmployeeCode { get; set; }
        public string EmployeeName { get; set; }
        public string Orno { get; set; }
        public string Text1 { get; set; }
        public string Text2 { get; set; }
        public string Text3 { get; set; }
        public DateTime? TransDateStore { get; set; }
        public string ClosingStoreId { get; set; }
        public string ClosingShiftId { get; set; }
        public string CustomerCode { get; set; }
        public int? Status { get; set; }
    }
}
