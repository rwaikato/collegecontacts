//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FarneFunds.Database
{
    using System;
    using System.Collections.Generic;
    
    public partial class ContactTagMap
    {
        public int ContactId { get; set; }
        public int TagId { get; set; }
        public bool IsActive { get; set; }
        public int Id { get; set; }
    
        public virtual Contact Contact { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
