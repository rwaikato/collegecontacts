using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarneFunds.Database;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FarneFunds.Models
{
    public class CampaignModel
    {
        
    }

    public class CampaignDetailsList
    {
        public string Query { get; set; }
        public int? Status { get; set; }
        public List<CampaignDetails> CampaignList { get; private set; }
        public CampaignDetailsList() { }
        public CampaignDetailsList( List<Campaign> campaignList, FarneFundsEntities dal, string query, int? status ) 
        {
            Status = status.HasValue ? status.Value : (int)Classes.Enums.CampaignStatus.Current;
            Query = query;
            CampaignList = new List<CampaignDetails>();
            CampaignList.AddRange(campaignList.Select(s => new CampaignDetails(s, dal)));
        }
    }

    public class CampaignDetails
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "Max 150 characters.")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        public DateTime DateStart { get; set; }

       
        [DataType(DataType.Date)]
        [Display(Name = "End Date")]
        public DateTime? DateEnd { get; set; }

        public decimal? Goal { get; set; }

        public decimal Raised { get; set; }

        [DisplayName("Pin Campaign to Home Page")]
        public bool PinToHomePage { get; set; }

        [DisplayName("Campaign is Complete")]
        public bool IsComplete { get; set; }

        public CampaignDetails() 
        {
            Raised = 0;
        }
        public CampaignDetails( Campaign campaign, FarneFundsEntities dal )
        {
            Id = campaign.Id;
            Name = campaign.Name;
            DateStart = campaign.DateStart;
            DateEnd = campaign.DateEnd;
            Goal = campaign.Goal.HasValue ? Math.Round((decimal)campaign.Goal.Value, 2, MidpointRounding.AwayFromZero) : (decimal?)null;
            Raised = campaign.Donations.Any( d=>d.IsActive ) ? Math.Round((decimal)campaign.Donations.Where(d => d.IsActive && d.CampaignId == campaign.Id).Sum(s => s.Amount), 2, MidpointRounding.AwayFromZero) : 0;
            PinToHomePage = campaign.PinToHomePage;
            IsComplete = campaign.IsComplete;
        }
    }
}