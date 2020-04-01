using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class StockTakeLine
    {
        public int Id { get; set; }
        public string ArticleId { get; set; }
        public string ArticleName { get; set; }
        public int? GoodQty { get; set; }
        public int? RejectQty { get; set; }
        public int? WhGoodQty { get; set; }
        public int? WhRejectQty { get; set; }
        public int StockTakeId { get; set; }
    }
}
