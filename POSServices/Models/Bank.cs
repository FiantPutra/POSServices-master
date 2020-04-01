using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class Bank
    {
        public Bank()
        {
            StorePaymentMethod = new HashSet<StorePaymentMethod>();
        }

        public int Id { get; set; }
        public string BankId { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public DateTime? ModifiedDatetime { get; set; }

        public virtual ICollection<StorePaymentMethod> StorePaymentMethod { get; set; }
    }
}
