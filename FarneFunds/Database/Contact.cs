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
    
    public partial class Contact
    {
        public Contact()
        {
            this.Donations = new HashSet<Donation>();
            this.ContactTagMaps = new HashSet<ContactTagMap>();
            this.ContactAudits = new HashSet<ContactAudit>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public string Initials { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PrefName { get; set; }
        public string Street { get; set; }
        public string Town { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string HomePhone { get; set; }
        public string MobilePhone { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string Email { get; set; }
        public bool IsOldBoy { get; set; }
        public bool IsDonor { get; set; }
        public string PartnerTitle { get; set; }
        public string PartnerInitials { get; set; }
        public string PartnerFirstName { get; set; }
        public string PartnerLastName { get; set; }
        public string PartnerPrefName { get; set; }
        public string PartnerMobilePhone { get; set; }
        public string PartnerEmail { get; set; }
        public string DateOfBirth { get; set; }
        public string Comments { get; set; }
        public Nullable<int> House { get; set; }
        public string Year { get; set; }
        public string YearFrom { get; set; }
        public string YearTo { get; set; }
        public bool Highways { get; set; }
        public bool Chronicle { get; set; }
        public bool RectorsNews { get; set; }
        public bool IsSupporter { get; set; }
        public bool Grandparent { get; set; }
        public bool OldBoyParent { get; set; }
        public bool FeederSchool { get; set; }
        public bool Community { get; set; }
    
        public virtual ICollection<Donation> Donations { get; set; }
        public virtual ICollection<ContactTagMap> ContactTagMaps { get; set; }
        public virtual ICollection<ContactAudit> ContactAudits { get; set; }
    }
}
