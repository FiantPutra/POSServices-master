using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class StoreDiscount
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public bool? Active { get; set; }
        public double? DiscountPrecentage { get; set; }
        public double? Margin { get; set; }
        public int? StoreId { get; set; }

        public virtual Store Store { get; set; }
    }
}
