//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QD_Tour_Web.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class Vehicle_Reservation
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Vehicle_Reservation()
        {
            this.Members = new HashSet<Member>();
        }
    
        public string Id { get; set; }
        public System.DateTime Start_Time { get; set; }
        public System.DateTime End_Time { get; set; }
        public int People_Number { get; set; }
        public string ID_Vehicle_Package { get; set; }
        public string ID_Vehicle { get; set; }
        public Nullable<int> IsPaid { get; set; }
        public Nullable<decimal> TotalPrice { get; set; }
        public string Type { get; set; }
    
        public virtual Vehicle Vehicle { get; set; }
        public virtual Vehicle_Package Vehicle_Package { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Member> Members { get; set; }
    }
}