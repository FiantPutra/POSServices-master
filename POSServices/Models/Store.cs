using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class Store
    {
        public Store()
        {
            DiscountSetupStore = new HashSet<DiscountSetupStore>();
            DiscountStore = new HashSet<DiscountStore>();
            Employee = new HashSet<Employee>();
            LoginStore = new HashSet<LoginStore>();
            StoreDiscount = new HashSet<StoreDiscount>();
            StorePaymentMethod = new HashSet<StorePaymentMethod>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Regional { get; set; }
        public int? StoreTypeId { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string WarehouseId { get; set; }
        public int? TargetQty { get; set; }
        public decimal? TargetValue { get; set; }
        public DateTime? DateOpen { get; set; }
        public int? MobileStore { get; set; }
        public DateTime? ModifiedDatetime { get; set; }

        public virtual MutasiApproverMatrix MutasiApproverMatrix { get; set; }
        public virtual ICollection<DiscountSetupStore> DiscountSetupStore { get; set; }
        public virtual ICollection<DiscountStore> DiscountStore { get; set; }
        public virtual ICollection<Employee> Employee { get; set; }
        public virtual ICollection<LoginStore> LoginStore { get; set; }
        public virtual ICollection<StoreDiscount> StoreDiscount { get; set; }
        public virtual ICollection<StorePaymentMethod> StorePaymentMethod { get; set; }
    }
}
