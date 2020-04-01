using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class ItemDimensionDepartment
    {
        public ItemDimensionDepartment()
        {
            DiscountRetailLines = new HashSet<DiscountRetailLines>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime? ModifiedDatetime { get; set; }

        public virtual ICollection<DiscountRetailLines> DiscountRetailLines { get; set; }
    }
}
