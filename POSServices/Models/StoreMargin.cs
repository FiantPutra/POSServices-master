using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class StoreMargin
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public string Description { get; set; }
        public double Discount { get; set; }
        public double DiscountPrecentage { get; set; }
    }
}
