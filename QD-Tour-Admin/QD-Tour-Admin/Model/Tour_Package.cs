//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QD_Tour_Admin.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Tour_Package
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Tour_Package()
        {
            this.Tour_Reservation = new HashSet<Tour_Reservation>();
        }
    
        public string Id { get; set; }
        public string ID_Tour { get; set; }
        public string Photo { get; set; }
        public string Description { get; set; }
    
        public virtual Tour Tour { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tour_Reservation> Tour_Reservation { get; set; }
    }
}
