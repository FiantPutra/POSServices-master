using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class InventoryTransactionLines
    {
        public long Id { get; set; }
        public string ArticleId { get; set; }
        public decimal? Qty { get; set; }
        public long? InventoryTransactionId { get; set; }
        public string ArticleName { get; set; }
        public decimal? RecieveQty { get; set; }
        public decimal? Urdlix { get; set; }
        public decimal? Urridl { get; set; }
        public decimal? ValueSalesPrice { get; set; }
        public string PackingNumber { get; set; }
        public string Urridn { get; set; }
    }
}
