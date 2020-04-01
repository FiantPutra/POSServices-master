using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class ExpenseStore
    {
        public int Id { get; set; }
        public string StoreCode { get; set; }
        public decimal? RemaingBudget { get; set; }
        public decimal? TotalExpense { get; set; }
        public int? Year { get; set; }
        public decimal? TotalBudget { get; set; }
    }
}
