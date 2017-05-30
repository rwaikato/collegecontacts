using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace FarneFunds.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }

    public class HomePageDetails
    {
        public List<CampaignDetails> CampaignList { get; set; }
        public List<ContactDetails> ContactList { get; set; }
        public List<DonationDetails> DontaionList { get; set; }

        public HomePageDetails()
        {
            CampaignList = new List<CampaignDetails>();
            ContactList = new List<ContactDetails>();
            DontaionList = new List<DonationDetails>();
        }
    }
}