using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class DiscountSetupStore
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int DiscountId { get; set; }
        public DateTime? ModifiedDatetime { get; set; }

        public virtual Store Store { get; set; }
    }

    public class discountSetupStoreList
    {
        public List<DiscountSetupStore> discSetupStore { get; set; }
    }
}
