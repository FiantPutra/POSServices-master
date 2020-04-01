using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class Customer
    {
        public int Id { get; set; }
        public string CustId { get; set; }
        public string Name { get; set; }
        public int CustGroupId { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public int? StoreId { get; set; }
        public string DefaultCurr { get; set; }
        public DateTime? ModifiedDatetime { get; set; }

        public virtual CustomerGroup CustGroup { get; set; }
    }
}
