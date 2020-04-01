using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class StoreTarget
    {
        public int Id { get; set; }
        public DateTime Periode { get; set; }
        public decimal Target { get; set; }
        public string StoreCode { get; set; }
        public int TargetQty { get; set; }
    }
}
