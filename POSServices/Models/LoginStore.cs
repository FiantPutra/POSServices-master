using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class LoginStore
    {
        public int Id { get; set; }
        public int StoreId { get; set; }
        public int LoginId { get; set; }
        public string StoreCode { get; set; }

        public virtual Store Store { get; set; }
    }
}
