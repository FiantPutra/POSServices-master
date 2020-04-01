using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class CurrencyDenomination
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public decimal Nominal { get; set; }
        public DateTime? ModifiedDatetime { get; set; }
    }
}
