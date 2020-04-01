using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class DoConfirm
    {
        public int Id { get; set; }
        public string DoNumber { get; set; }
        public string WarehouseOrigin { get; set; }
        public string WarehouseDestination { get; set; }
        public string Status { get; set; }
        public string CheckDll { get; set; }
    }
}
