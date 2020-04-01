using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class HotransactionType
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public string WarehouseFrom { get; set; }
        public string WarehouseTo { get; set; }
    }
}
