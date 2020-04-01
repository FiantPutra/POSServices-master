using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class DeviceTable
    {
        public int Id { get; set; }
        public string InitialId { get; set; }
        public string DeviceId { get; set; }
        public string TypeDevice { get; set; }
        public string FirstLogin { get; set; }
        public string LastLogin { get; set; }
    }
}
