using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class CustomerGroup
    {
        public CustomerGroup()
        {
            Customer = new HashSet<Customer>();
            DiscountRetail = new HashSet<DiscountRetail>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public DateTime? ModifiedDatetime { get; set; }

        public virtual ICollection<Customer> Customer { get; set; }
        public virtual ICollection<DiscountRetail> DiscountRetail { get; set; }
    }
}
