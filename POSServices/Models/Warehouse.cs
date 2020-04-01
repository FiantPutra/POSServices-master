using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class Warehouse
    {
        public Warehouse()
        {
            InventoryLines = new HashSet<InventoryLines>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string City { get; set; }
        public string Regional { get; set; }
        public string Division { get; set; }
        public int? StoreId { get; set; }
        public DateTime? ModifiedDatetime { get; set; }

        public virtual ICollection<InventoryLines> InventoryLines { get; set; }
    }
}
