using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class DiscountRetailLines
    {
        public int Id { get; set; }
        public int DiscountRetailId { get; set; }
        public int? ArticleId { get; set; }
        public int? BrandCode { get; set; }
        public int? Gender { get; set; }
        public int? Department { get; set; }
        public int? Size { get; set; }
        public int? Color { get; set; }
        public decimal? DiscountPrecentage { get; set; }
        public decimal? CashDiscount { get; set; }
        public decimal? DiscountPrice { get; set; }
        public int? Qty { get; set; }
        public decimal? AmountTransaction { get; set; }
        public int? DepartmentType { get; set; }
        public int ArticleIdDiscount { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? QtyMin { get; set; }
        public int? QtyMax { get; set; }
        public decimal? AmountMin { get; set; }
        public decimal? AmountMax { get; set; }

        public virtual Item Article { get; set; }
        public virtual Item ArticleIdDiscountNavigation { get; set; }
        public virtual ItemDimensionBrand BrandCodeNavigation { get; set; }
        public virtual ItemDimensionColor ColorNavigation { get; set; }
        public virtual ItemDimensionDepartment DepartmentNavigation { get; set; }
        public virtual ItemDimensionDepartmentType DepartmentTypeNavigation { get; set; }
        public virtual ItemDimensionGender GenderNavigation { get; set; }
        public virtual ItemDimensionSize SizeNavigation { get; set; }
    }
}
