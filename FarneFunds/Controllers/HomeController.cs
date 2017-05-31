using FarneFunds.Classes;
using FarneFunds.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace FarneFunds.Controllers
{
    [Authorize]
    public class HomeController : AurthorizedController
    {
        public ActionResult Index()
        {
            HomePageDetails vm = new HomePageDetails();

            var Camps = Dal.Campaigns.Where(c => c.IsActive).OrderByDescending(o => o.PinToHomePage).ThenByDescending(o => o.DateCreated).Take(10).ToList();
            var Cons = Dal.Contacts.Where(c => c.IsActive && c.DateCreated.HasValue).OrderByDescending(o => o.DateCreated).Take(10).ToList();
            var Dons = Dal.Donations.Include(i => i.Contact).Where(d => d.Contact.IsActive && d.IsActive && d.Campaign.IsActive).OrderByDescending(o => o.DateCreated).Take(10).ToList();
            var Tags = Dal.Tags.Where(t => t.IsActive).ToList();

            var pastYear = DateTime.Now.AddYears( -1 );
            vm.DonationCount = (decimal)Dal.Donations.Where( d => d.IsActive && d.Campaign.IsActive && d.Contact.IsActive && d.DateDonated > pastYear ).Select( s => s.Amount ).ToList( ).Sum( s => s );
            vm.ContactCount = Dal.Contacts.Where( c => c.IsActive ).Count( );
            vm.CampaignCount = Dal.Campaigns.Where( c => c.IsActive && !c.IsComplete ).Count( );

            vm.CampaignList.AddRange(Camps.Select(s => new CampaignDetails(s, Dal)));
            vm.ContactList.AddRange(Cons.Select(s => new ContactDetails(s, Dal, Tags)));
            vm.DontaionList.AddRange(Dons.Select(s => new DonationDetails(s)));

            ViewBag.SelectedTab = (int)Enums.Controller.Home;
            return View(vm);
        }
    }
}