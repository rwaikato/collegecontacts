using FarneFunds.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FarneFunds.Classes;
using FarneFunds.Database;
using System.Data.Entity;
using System.Web.Services;
using System.Web.Script.Services;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using RazorEngine;

namespace FarneFunds.Controllers
{
    [Authorize]
    public class DonationController : AurthorizedController
    {
        private Donation SetDonationData(DonationDetails vm)
        {
            Donation Don = new Donation();

            Don.Amount = (float)vm.Amount;
            Don.CampaignId = vm.CampaignId.Value;
            Don.ContactId = vm.ContactId.Value;
            Don.DateDonated = vm.DateDonated;
            Don.IsActive = true;
            Don.DateCreated = DateTime.Now;

            return Don;
        }

        private Donation UpdateDonationData(DonationDetails vm, Donation Don)
        {
            Don.Amount = (float)vm.Amount;
            Don.ContactId = vm.ContactId.Value;
            Don.DateDonated = vm.DateDonated;

            return Don;
        }

        //
        // GET: /Donation/
        public ActionResult Index(int? campaignId, int? contactId)
        {

            var Dons = Dal.Donations.Where( d=>d.IsActive);

            if (campaignId.HasValue)
            {
                Dons = Dons.Where(d => d.CampaignId == campaignId);
            }

            if( contactId.HasValue)
            {
                Dons = Dons.Where(d => d.ContactId == contactId);
            }

            DonationList vm = new DonationList(Dons.ToList(), campaignId, contactId);
            ViewBag.SelectedTab = (int)Enums.Controller.Donation;
            return View(vm);
        }

        //
        // GET: /Donation/Create
        public ActionResult Create(int? CampaignId, int? ContactId)
        {
            DonationDetails vm = new DonationDetails();

            var contacts = Dal.Contacts.Where(c => ( c.IsActive && c.IsDonor ) || ( ContactId.HasValue && c.Id == ContactId ) ).ToList();
            vm.ContactId = ContactId;
            vm.ReturnToContactId = ContactId;
            vm.ContactSelectList = SelectLists.ContactSelectList(contacts, null);
            
            vm.CampaignId = CampaignId;
            vm.CampaignSelectList = SelectLists.CampaignSelectList(null, Dal);
            
            vm.DateDonated = DateTime.Now.Date;

            ViewBag.SelectedTab = (int)Enums.Controller.Donation;
            return View(vm);
        }

        //
        // POST: /Donation/Create
        [HttpPost]
        public ActionResult Create(DonationDetails vm)
        {
            ValidateModelState(vm);

            if (ModelState.IsValid)
            {
                Donation Don = SetDonationData(vm);

                Contact Con = Dal.Contacts.Where(c => c.IsActive && c.Id == vm.ContactId).First();
                Con.IsDonor = true;

                Dal.Donations.Add(Don);
                Dal.SaveChanges();

                if( vm.ReturnToContactId.HasValue && vm.ReturnToContactId > 0)
                {
                    return RedirectToAction("Edit", "Contacts", new { id = vm.ReturnToContactId });
                }

                return RedirectToAction("Index", new { campaignId = vm.CampaignId });
            }

            var contacts = Dal.Contacts.Where(c => ( c.IsActive && c.IsDonor ) || ( vm.ContactId.HasValue && c.Id == vm.ContactId.Value)).ToList();
            vm.ContactSelectList = SelectLists.ContactSelectList(contacts, vm.ContactId);
            ViewBag.SelectedTab = (int)Enums.Controller.Donation;
            vm.CampaignSelectList = SelectLists.CampaignSelectList(vm.CampaignId, Dal);
            return View(vm);
        }

        // GET: /Donation/Edit/5
        public ActionResult Edit(int id, int? returnToContactId)
        {
            Donation Don = Dal.Donations.First(c => c.Id == id && c.IsActive);
            DonationDetails vm = new DonationDetails(Don);

            var contacts = Dal.Contacts.Where(c => c.IsActive && c.IsDonor).ToList();
            vm.ContactSelectList = SelectLists.ContactSelectList(contacts, Don.ContactId);
            vm.ReturnToContactId = returnToContactId;

            vm.CampaignId = Don.CampaignId;
            vm.CampaignSelectList = SelectLists.CampaignSelectList(null, Dal);

            ViewBag.SelectedTab = (int)Enums.Controller.Donation;
            return View(vm);
        }

        // POST: /Donation/Edit/5
        [HttpPost]
        public ActionResult Edit(DonationDetails vm)
        {
            ValidateModelState(vm);

            if (ModelState.IsValid)
            {
                Donation DbDon = Dal.Donations.Where(c => c.Id == vm.Id && c.IsActive).First();
                Donation Don = UpdateDonationData(vm, DbDon);
                Don.Contact.IsDonor = true;
                Dal.Entry(Don).State = EntityState.Modified;
                Dal.SaveChanges();

                if (vm.ReturnToContactId.HasValue && vm.ReturnToContactId > 0)
                {
                    return RedirectToAction("Edit", "Contacts", new { id = vm.ReturnToContactId });
                }

                return RedirectToAction("Index", new { campaignId = vm.CampaignId });
            }

            var contacts = Dal.Contacts.Where(c => c.IsActive && c.IsDonor).ToList();
            vm.ContactSelectList = SelectLists.ContactSelectList(contacts, vm.ContactId);
            vm.CampaignSelectList = SelectLists.CampaignSelectList(vm.CampaignId, Dal);
            ViewBag.SelectedTab = (int)Enums.Controller.Donation;
            return View(vm);
        }

        private void ValidateModelState(DonationDetails vm)
        {
            if (!vm.CampaignId.HasValue)
            {
                ModelState.AddModelError("CampaignId", "Required");
            }

            if (!vm.ContactId.HasValue)
            {
                ModelState.AddModelError("ContactId", "Required");
            }

            if (vm.Amount < 0)
            {
                ModelState.AddModelError("Amount", "Must be greater than zero");
            }
        }

        // GET: /Donation/Delete/5
        public ActionResult Archive(int id)
        {
            Donation Don = Dal.Donations.First(c => c.Id == id && c.IsActive);
            DonationDetails vm = new DonationDetails(Don);
            ViewBag.SelectedTab = (int)Enums.Controller.Donation;
            return View(vm);
        }

        // POST: /Donation/Delete/5
        [HttpPost]
        public ActionResult Archive(DonationDetails vm)
        {
            if (ModelState.IsValid)
            {
                Donation DbDon = Dal.Donations.Where(c => c.Id == vm.Id && c.IsActive).First();
                DbDon.IsActive = false;

                Dal.Entry(DbDon).State = EntityState.Modified;
                Dal.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.SelectedTab = (int)Enums.Controller.Donation;
            return View(vm);
        }

        public JsonResult AutoCompleteContacts(string term)
        {
            var result = Dal.Contacts.Where(f => f.FirstName.StartsWith(term) || f.LastName.StartsWith(term)).Select(s => s.LastName).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetCampaignDonations(int campaignId)
        {
            string html = getCampaignDonationsString(campaignId);

            // Create a Document object
            var document = new Document(PageSize.A4, 20, 20, 42, 0);

            var output = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, output);

            // Open the Document for writing
            document.Open();

            // Step 4: Parse the HTML string into a collection of elements...
            var parsedHtmlElements = HTMLWorker.ParseToList(new StringReader(html), null);

            // Enumerate the elements, adding each one to the Document...
            foreach (var htmlElement in parsedHtmlElements)
                document.Add(htmlElement as IElement);

            // Close the Document - this saves the document contents to the output stream
            document.Close();

            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename=Receipt-{0}.pdf", "pdfFile"));
            Response.BinaryWrite(output.ToArray());

            return new EmptyResult();
        }

        public string getCampaignDonationsString(int campaignId)
        {
            var Dons = Dal.Donations.Where(d => d.IsActive && d.CampaignId == campaignId);

            DonationList vm = new DonationList(Dons.ToList(), campaignId, null);

            string text = Dal.RazorTemplates.Where(R => R.Id == 2).First().Template;
            string renderedText = Razor.Parse(text, vm);
            return renderedText;
        }
    }
}
