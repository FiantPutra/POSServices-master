using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class DiscountSetup
    {
        public DiscountSetup()
        {
            DiscountSetupLines = new HashSet<DiscountSetupLines>();
        }

        public string DiscountCode { get; set; }
        public int DiscountCategory { get; set; }
        public string DiscountName { get; set; }
        public int CustomerGroupId { get; set; }
        public int? DiscountType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Status { get; set; }
        public decimal? DiscountCash { get; set; }
        public int? DiscountPercent { get; set; }
        public int? QtyMin { get; set; }
        public int? QtyMax { get; set; }
        public decimal? AmountMin { get; set; }
        public decimal? AmountMax { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public long Id { get; set; }
        public int? Multi { get; set; }
        public int? ApplytoAllstore { get; set; }
        public DateTime? ModifiedDatetime { get; set; }

        public virtual ICollection<DiscountSetupLines> DiscountSetupLines { get; set; }
    }

    public class discountSetupList
    {
        public List<DiscountSetup> discounts { get; set; }
    }
}
