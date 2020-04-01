using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class DiscountSetupLines
    {
        public int? GroupCode { get; set; }
        public string Code { get; set; }
        public decimal? DiscountPrecentage { get; set; }
        public decimal? DiscountCash { get; set; }
        public int? QtyMin { get; set; }
        public int? QtyMax { get; set; }
        public decimal? AmountMin { get; set; }
        public decimal? AmountMax { get; set; }
        public long Id { get; set; }
        public long DiscountSetupId { get; set; }
        public int? Multi { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public DateTime? ModifiedDatetime { get; set; }        

        public virtual DiscountSetup DiscountSetup { get; set; }
    }

    public class discountSetupLineList
    {
        public List<DiscountSetupLines> discountLines { get; set; }
    }
}
