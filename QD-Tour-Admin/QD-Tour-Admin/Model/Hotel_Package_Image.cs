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
    
    public partial class Hotel_Package_Image
    {
        public string Id { get; set; }
        public string Hotel_Package_Id { get; set; }
        public string Type { get; set; }
        public string ImageUrl { get; set; }
        public string ImageName { get; set; }
    
        public virtual Hotel_Package Hotel_Package { get; set; }
    }
}
