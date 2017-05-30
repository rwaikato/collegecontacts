using FarneFunds.Classes;
using FarneFunds.Database;
using FarneFunds.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FarneFunds.Controllers
{
    [Authorize]
    public class CampaignController : AurthorizedController
    {
        private Campaign SetCampaignData(CampaignDetails vm)
        {
            Campaign NewCampaign = new Campaign();

            NewCampaign.Name = vm.Name;
            NewCampaign.Goal = vm.Goal.HasValue ? (float)vm.Goal.Value : (float?)null;
            NewCampaign.DateStart = vm.DateStart;
            NewCampaign.DateEnd = vm.DateEnd.HasValue ? vm.DateEnd.Value : (DateTime?)null;
            NewCampaign.DateCreated = DateTime.Now;
            NewCampaign.IsActive = true;
            NewCampaign.IsComplete = false;
            NewCampaign.PinToHomePage = vm.PinToHomePage;

            return NewCampaign;

        }

        private Campaign UpdateCampaignData(CampaignDetails vm, Campaign EditCampaign)
        {
            EditCampaign.Name = vm.Name;
            EditCampaign.Goal = vm.Goal.HasValue ? (float)vm.Goal.Value : (float?)null;
            EditCampaign.DateStart = vm.DateStart;
            EditCampaign.DateEnd = vm.DateEnd.HasValue ? vm.DateEnd.Value : (DateTime?)null;
            EditCampaign.PinToHomePage = vm.PinToHomePage;
            EditCampaign.IsComplete = vm.IsComplete;

            return EditCampaign;
        }

        //
        // GET: /Campaign/
        public ActionResult Index(string query, int? status)
        {
            var CampaignList = Dal.Campaigns.Where(c => c.IsActive || c.IsActive == false);
            if (!String.IsNullOrWhiteSpace(query))
            {
                CampaignList = CampaignList.Where(c=>c.Name.Contains(query));
            }

            if(!status.HasValue)
            {
                status = (int)Enums.CampaignStatus.Current;
            }

            switch (status.Value)
            {
                case (int)Enums.CampaignStatus.Current:
                    CampaignList = CampaignList.Where(c => c.IsActive && c.IsComplete == false);
                    break;

                case (int)Enums.CampaignStatus.Archived:
                    CampaignList = CampaignList.Where(c => c.IsActive == false);
                    break;

                case (int)Enums.CampaignStatus.Completed:
                    CampaignList = CampaignList.Where(c => c.IsActive && c.IsComplete);
                    break;

                case (int)Enums.CampaignStatus.Expired:
                    CampaignList = CampaignList.Where(c => c.IsActive && c.IsComplete == false && c.DateEnd < DateTime.Now);
                    break;

                default:
                    CampaignList = CampaignList.Where(c => c.IsActive && c.IsComplete == false);
                    break;
            }

            CampaignDetailsList vm = new CampaignDetailsList(CampaignList.ToList(), Dal, query, status);
            ViewBag.SelectedTab = (int)Enums.Controller.Campaign;
            return View(vm);
        }

        [HttpPost]
        public ActionResult Index(CampaignDetailsList vm)
        {

            return RedirectToAction("Index", new { query = vm.Query });
        }

        //
        // GET: /Campaign/Create
        public ActionResult Create()
        {
            CampaignDetails vm = new CampaignDetails();
            vm.DateStart = DateTime.Now.Date;
            ViewBag.SelectedTab = (int)Enums.Controller.Campaign;
            return View(vm);
        }

        //
        // POST: /Campaign/Create
        [HttpPost]
        public ActionResult Create(CampaignDetails vm)
        {
            if( ModelState.IsValid)
            {
                Campaign Camp = SetCampaignData(vm);

                Dal.Campaigns.Add(Camp);
                Dal.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SelectedTab = (int)Enums.Controller.Campaign;
            return View(vm);
        }

        //
        // GET: /Campaign/Edit/5
        public ActionResult Edit(int id)
        {
            var Camp = Dal.Campaigns.Where(c => c.Id == id && c.IsActive).FirstOrDefault();
            if (Camp != null)
            {
                CampaignDetails vm = new CampaignDetails(Camp, Dal);
                return View(vm);
            }
            ViewBag.SelectedTab = (int)Enums.Controller.Campaign;
            return RedirectToAction("Index");
        }

        //
        // POST: /Campaign/Edit/5
        [HttpPost]
        public ActionResult Edit(CampaignDetails vm)
        {
            if (ModelState.IsValid)
            {
                Campaign DbCampaign = Dal.Campaigns.Where(c => c.Id == vm.Id && c.IsActive).First();
                Campaign Camp = UpdateCampaignData(vm, DbCampaign);

                Dal.Entry(Camp).State = EntityState.Modified;
                Dal.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SelectedTab = (int)Enums.Controller.Campaign;
            return View(vm);
        }

        // GET: /Contacts/Delete/5
        public ActionResult Archive(int id)
        {
            Campaign Camp = Dal.Campaigns.FirstOrDefault(c => c.Id == id && c.IsActive);
            CampaignDetails vm = new CampaignDetails(Camp, Dal);
            ViewBag.SelectedTab = (int)Enums.Controller.Campaign;
            return View(vm);
        }

        // POST: /Contacts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Archive(CampaignDetails vm)
        {
            if (ModelState.IsValid)
            {
                Campaign Camp = Dal.Campaigns.Where(c => c.Id == vm.Id && c.IsActive).First();
                Camp.IsActive = false;

                Dal.Entry(Camp).State = EntityState.Modified;
                Dal.SaveChanges();

                return RedirectToAction("Index");
            }
            ViewBag.SelectedTab = (int)Enums.Controller.Campaign;
            return View(vm);
        }

        [HttpGet]
        public ActionResult Unarchive(int id)
        {
            Campaign Camp = Dal.Campaigns.FirstOrDefault(c => c.Id == id && c.IsActive == false);

            if (Camp != null)
            {
                Camp.IsActive = true;
                Dal.SaveChanges();
            }

            return RedirectToAction("Index", new { status = (int)Enums.CampaignStatus.Archived });
        }
    }
}
