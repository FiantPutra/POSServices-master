using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class EmployeePossition
    {
        public EmployeePossition()
        {
            Employee = new HashSet<Employee>();
        }

        public int Id { get; set; }
        public string PossitionId { get; set; }
        public string Name { get; set; }
        public bool? CanCreateTransaction { get; set; }
        public bool? CanVoidTransaction { get; set; }
        public bool? CanViewHistoryTransaction { get; set; }
        public bool? CanViewPromoDiscount { get; set; }
        public bool? CanViewInventory { get; set; }
        public bool? CanRequestInventory { get; set; }
        public bool? CanConfirmDo { get; set; }
        public bool? CanClosingStore { get; set; }
        public DateTime? ModifiedDatetime { get; set; }

        public virtual ICollection<Employee> Employee { get; set; }
    }
}
