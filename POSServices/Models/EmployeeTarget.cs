using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class EmployeeTarget
    {
        public int Id { get; set; }
        public DateTime Periode { get; set; }
        public decimal Target { get; set; }
        public string EmployeeCode { get; set; }
    }
}
