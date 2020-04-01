using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class StorePaymentMethod
    {
        public int Id { get; set; }
        public string BankCode { get; set; }
        public bool Active { get; set; }
        public int StoreId { get; set; }
        public string Name { get; set; }
        public string Detailid { get; set; }
        public DateTime? ModifiedDatetime { get; set; }

        public virtual Bank BankCodeNavigation { get; set; }
        public virtual Store Store { get; set; }
    }
}
