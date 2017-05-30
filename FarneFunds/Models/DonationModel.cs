using FarneFunds.Database;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FarneFunds.Models
{
    public class DonationModel
    {
    }

    public class DonationList
    {
        public List<DonationListItems> Donations { get; private set; }
        public int? CampaignId { get; set; }
        public int? ContactId { get; set; }
        public string DonationTotal { get; set; }
        public DateTime? ExportDate { get; set; }

        public DonationList( List<Donation> Dons, int? campaignId, int? contactId) 
        {
            Donations = new List<DonationListItems>();
            Donations.AddRange(Dons.OrderByDescending(o => o.DateDonated).Select(s => new DonationListItems(s)));

            CampaignId = campaignId;
            ContactId = contactId;
            DonationTotal = Dons.Sum(s => s.Amount).ToString("C");

            ExportDate = DateTime.Now;
        }
    }

    public class DonationListItems
    {
        public int Id { get; set; }
        public int ContactId { get; set; }
        public string ContactName { get; set; }
        public DateTime DateDonated { get; set; }
        public decimal Amount { get; set; }
        public string CampaignName { get; set; }
        public int? CampaignId { get; set; }
        public bool IsContactArchived { get; set; }

        public DonationListItems( Donation Don )
        {
            Id = Don.Id;
            ContactId = Don.ContactId;
            ContactName = Don.Contact.FirstName + " " + Don.Contact.LastName;
            DateDonated = Don.DateDonated;
            Amount = Math.Round((decimal)Don.Amount, 2, MidpointRounding.AwayFromZero);
            CampaignId = Don.CampaignId;
            CampaignName = Don.Campaign.Name;
            IsContactArchived = !Don.Contact.IsActive;
        }

        
    }

    public class DonationDetails
    {
        public int Id { get; set; }

        [DisplayName("Contact Name")]
        public int? ContactId { get; set; }

        public string ContactName { get; set; }
        public SelectList ContactSelectList { get; set; }

        public SelectList CampaignSelectList { get; set; }
        public int? CampaignId { get; set; }

        public decimal Amount { get; set; }

        [DataType(DataType.Date)]
        [DisplayName("Date Donated")]
        public DateTime DateDonated { get; set; }

        public int? ReturnToContactId { get; set; }

        public DonationDetails() { }

        public DonationDetails( Donation Don )
        {
            Id = Don.Id;
            ContactId = Don.ContactId;
            CampaignId = Don.CampaignId;
            Amount = Math.Round((decimal)Don.Amount, 2, MidpointRounding.AwayFromZero);
            DateDonated = Don.DateDonated.Date;
            ContactName = Don.Contact.LastName;
        }
    }
}