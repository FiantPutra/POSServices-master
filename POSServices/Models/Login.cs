using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class Login
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Code { get; set; }
    }
}
