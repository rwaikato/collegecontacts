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
    
    public partial class Donation
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public int CampaignId { get; set; }
        public System.DateTime DateDonated { get; set; }
        public float Amount { get; set; }
        public bool IsActive { get; set; }
        public System.DateTime DateCreated { get; set; }
    
        public virtual Campaign Campaign { get; set; }
        public virtual Contact Contact { get; set; }
    }
}
