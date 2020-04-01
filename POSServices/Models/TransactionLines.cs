using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class TransactionLines
    {
        public long Id { get; set; }
        public string ArticleId { get; set; }
        public int Qty { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Amount { get; set; }
        public decimal? Discount { get; set; }
        public long TransactionId { get; set; }
        public string ArticleName { get; set; }
        public int? DiscountType { get; set; }
        public string DiscountCode { get; set; }
        public string Spgid { get; set; }
        public string ArticleIdAlias { get; set; }
    }
}
