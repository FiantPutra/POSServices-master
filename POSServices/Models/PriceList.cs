using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class PriceList
    {
        public int Id { get; set; }
        public string ItemId { get; set; }
        public decimal SalesPrice { get; set; }
        public string CustomerId { get; set; }
        public string Currency { get; set; }
        public DateTime? ModifiedDatetime { get; set; }
    }

    public class ListPrice
    {
        public List<PriceList> prices { get; set; }
    }
}
