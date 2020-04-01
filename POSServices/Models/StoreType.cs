using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class StoreType
    {
        public int Id { get; set; }
        public string TypeId { get; set; }
        public string Name { get; set; }
        public bool? StoreInStore { get; set; }
        public string InforOrderTypeNormal { get; set; }
        public string InforOrderTypeRetur { get; set; }
        public string InforXrcdnormal { get; set; }
        public string InforXrcdretur { get; set; }
    }
}
