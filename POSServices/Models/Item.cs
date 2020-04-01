using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class Item
    {
        public Item()
        {
            DiscountItemSelected = new HashSet<DiscountItemSelected>();
            DiscountRetailLinesArticle = new HashSet<DiscountRetailLines>();
            DiscountRetailLinesArticleIdDiscountNavigation = new HashSet<DiscountRetailLines>();
            InventoryLines = new HashSet<InventoryLines>();
        }

        public int Id { get; set; }
        public string ItemId { get; set; }
        public string Name { get; set; }
        public int? BrandId { get; set; }
        public int? DepartmentId { get; set; }
        public int? DepartmentTypeId { get; set; }
        public int? SizeId { get; set; }
        public int? ColorId { get; set; }
        public int? GenderId { get; set; }
        public string ItemGroup { get; set; }
        public string ItemIdAlias { get; set; }
        public bool? IsServiceItem { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public DateTime? ModifiedDatetime { get; set; }

        public virtual ICollection<DiscountItemSelected> DiscountItemSelected { get; set; }
        public virtual ICollection<DiscountRetailLines> DiscountRetailLinesArticle { get; set; }
        public virtual ICollection<DiscountRetailLines> DiscountRetailLinesArticleIdDiscountNavigation { get; set; }
        public virtual ICollection<InventoryLines> InventoryLines { get; set; }
    }
}
