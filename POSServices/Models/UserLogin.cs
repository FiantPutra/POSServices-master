using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class UserLogin
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool? Status { get; set; }
        public DateTime? LastLogin { get; set; }
        public string Role { get; set; }
        public string NewPassword { get; set; }
        public string OldPassword { get; set; }
        public string ConfirmPassword { get; set; }
        public DateTime? ModifiedDatetime { get; set; }

        public virtual Employee User { get; set; }
    }

    public class userLoginList
    {
        public List<UserLogin> UserList { get; set; }
    }
}
