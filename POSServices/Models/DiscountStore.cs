using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class DiscountStore
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int DiscountId { get; set; }

        public virtual Store Store { get; set; }
    }
}
