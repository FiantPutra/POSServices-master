using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class LoginWeb
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public bool? RememberMe { get; set; }
        public int Id { get; set; }
        public int? EmployeeId { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
