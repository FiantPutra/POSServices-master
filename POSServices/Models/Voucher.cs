using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class Voucher
    {
        public int Id { get; set; }
        public string VoucherCode { get; set; }
        public string Description { get; set; }
        public int Qty { get; set; }
        public decimal Value { get; set; }
    }
}
