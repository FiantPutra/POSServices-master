using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class Budget
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string StoreCode { get; set; }
        public string StoreName { get; set; }
        public decimal Amount { get; set; }
        public string JournalNumber { get; set; }
        public DateTime? TransactionDate { get; set; }
    }

    public class BudgetList
    {
        public List<Budget> Budgets { get; set; }
    }
}
