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
    
    public partial class Golf_Price
    {
        public string Id { get; set; }
        public string Golf_Id { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<decimal> Eighteen_Hole_Price { get; set; }
        public Nullable<decimal> TwentySeven_Hole_Price { get; set; }
        public Nullable<decimal> ThirySix_Hole_Price { get; set; }
    
        public virtual Golf Golf { get; set; }
    }
}
