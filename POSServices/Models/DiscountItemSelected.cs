using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class DiscountItemSelected
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public int DiscountId { get; set; }

        public virtual Item Item { get; set; }
    }
}
