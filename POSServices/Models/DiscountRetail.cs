using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class DiscountRetail
    {
        public int Id { get; set; }
        public int DiscountCategory { get; set; }
        public string DiscountCode { get; set; }
        public string DiscountName { get; set; }
        public int CustomerGroupId { get; set; }
        public string DiscountPartner { get; set; }
        public string Description { get; set; }
        public int? DiscountType { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool? Status { get; set; }
        public int? DiscountPercent { get; set; }
        public bool? TriggerSt { get; set; }
        public decimal? DiscountCash { get; set; }
        public int? Qty { get; set; }
        public decimal? Amount { get; set; }
        public int? Priority { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? ApprovedDate { get; set; }
        public int? QtyMin { get; set; }
        public int? QtyMax { get; set; }
        public decimal? AmountMin { get; set; }
        public decimal? AmountMax { get; set; }

        public virtual CustomerGroup CustomerGroup { get; set; }
    }
}
