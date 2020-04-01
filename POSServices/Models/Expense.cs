using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class Expense
    {
        public int Id { get; set; }
        public string StoreCode { get; set; }
        public string CostCategoryId { get; set; }
        public string CostCategoryName { get; set; }
        public string ExpenseName { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public int? Qty { get; set; }
        public decimal? Price { get; set; }
        public int? TypeId { get; set; }
        public string TypeName { get; set; }
        public string Itrn { get; set; }
        public string Orno { get; set; }
    }
}
