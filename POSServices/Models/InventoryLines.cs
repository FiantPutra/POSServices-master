using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class InventoryLines
    {
        public int Id { get; set; }
        public string WarehouseId { get; set; }
        public int ItemId { get; set; }
        public int? Qty { get; set; }
        public DateTime? ModifiedDatetime { get; set; }

        public virtual Item Item { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }
}
