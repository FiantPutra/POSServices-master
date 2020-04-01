using System;
using System.Collections.Generic;

namespace POSServices.Models
{
    public partial class Employee
    {
        public Employee()
        {
            LoginWeb = new HashSet<LoginWeb>();
            UserLogin = new HashSet<UserLogin>();
        }

        public int Id { get; set; }
        public int StoreId { get; set; }
        public string EmployeeName { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public bool? Status { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public int PossitionId { get; set; }
        public string EmployeeCode { get; set; }
        public DateTime? ModifiedDatetime { get; set; }

        public virtual EmployeePossition Possition { get; set; }
        public virtual Store Store { get; set; }
        public virtual ICollection<LoginWeb> LoginWeb { get; set; }
        public virtual ICollection<UserLogin> UserLogin { get; set; }
    }
}
