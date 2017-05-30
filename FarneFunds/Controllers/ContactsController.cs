using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FarneFunds.Database;
using FarneFunds.Models;
using FarneFunds.Classes;
using RazorEngine;
using iTextSharp;
using iTextSharp.text;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.Web.UI.WebControls;
using System.Web.UI;
using ClosedXML.Excel;

namespace FarneFunds.Controllers
{
    [Authorize]
    public class ContactsController : AurthorizedController
    {
        private FarneFundsEntities db = new FarneFundsEntities();

        #region Populating Contact Data
        private Contact SetContactData( ContactDetails vm )
        {
            Contact contact = new Contact();

            contact.FirstName = vm.FirstName;
            contact.LastName = vm.LastName;
            contact.PrefName = vm.PreferredName;
            contact.Title = vm.Title;
            contact.MobilePhone = vm.MobilePhone;
            contact.HomePhone = vm.HomePhone;
            contact.Email = vm.EmailAddress;
            contact.Street = vm.Street;
            contact.Town = vm.Town;
            contact.City = vm.City;
            contact.Province = vm.Province;
            contact.Country = vm.Country;
            contact.ZipCode = vm.ZipCode;
            contact.IsActive = true;
            contact.DateCreated = DateTime.Now;
            contact.IsOldBoy = vm.IsOldBoy;
            contact.IsDonor = vm.IsDonor;
            contact.Initials = vm.Initials;

            contact.PartnerFirstName = vm.PartnerFirstName;
            contact.PartnerLastName = vm.PartnerLastName;
            contact.PartnerPrefName = vm.PartnerPreferredName;
            contact.PartnerTitle = vm.PartnerTitle;
            contact.PartnerMobilePhone = vm.PartnerMobilePhone;
            contact.PartnerEmail = vm.PartnerEmailAddress;

			contact.Comments = vm.Comments;
			contact.House = vm.HouseId;
			contact.DateOfBirth = vm.DateOfirth;
			contact.Year = vm.Year;
			contact.YearFrom = vm.FromYear;
			contact.YearTo = vm.ToYear;

			contact.Chronicle = vm.Chronicle;
			contact.RectorsNews = vm.RectorsNews;
			contact.Highways = vm.Highways;

			contact.IsSupporter = vm.IsSupporter;
			contact.Grandparent = vm.Grandparent;
			contact.FeederSchool = vm.FeederSchool;
			contact.Community = vm.Community;
			contact.OldBoyParent = vm.OldBoyParent;

            if( vm.TagCheckBoxes != null && vm.TagCheckBoxes.Any())
            {
                foreach(var cb in vm.TagCheckBoxes.Where( c=>c.IsSelected ))
                {
                    ContactTagMap ct = new ContactTagMap();

                    ct.TagId = cb.Id;
                    ct.IsActive = true;

                    contact.ContactTagMaps.Add(ct);
                }
            }

            return contact;

        }

        private Contact UpdateContactData(ContactDetails vm, Contact contact)
        {
            contact.FirstName = vm.FirstName;
            contact.LastName = vm.LastName;
            contact.PrefName = vm.PreferredName;
            contact.Title = vm.Title;
            contact.MobilePhone = vm.MobilePhone;
            contact.HomePhone = vm.HomePhone;
            contact.Email = vm.EmailAddress;
            contact.Street = vm.Street;
            contact.Town = vm.Town;
            contact.City = vm.City;
            contact.Province = vm.Province;
            contact.Country = vm.Country;
            contact.ZipCode = vm.ZipCode;
            contact.IsOldBoy = vm.IsOldBoy;
            contact.IsDonor = vm.IsDonor;
            contact.Initials = vm.Initials;

            contact.PartnerFirstName = vm.PartnerFirstName;
            contact.PartnerLastName = vm.PartnerLastName;
            contact.PartnerPrefName = vm.PartnerPreferredName;
            contact.PartnerTitle = vm.PartnerTitle;
            contact.PartnerMobilePhone = vm.PartnerMobilePhone;
            contact.PartnerEmail = vm.PartnerEmailAddress;

			contact.Comments = vm.Comments;
			contact.House = vm.HouseId;
			contact.DateOfBirth = vm.DateOfirth;
			contact.Year = vm.Year;
			contact.YearFrom = vm.FromYear;
			contact.YearTo = vm.ToYear;

			contact.Chronicle = vm.Chronicle;
			contact.RectorsNews = vm.RectorsNews;
			contact.Highways = vm.Highways;

			contact.IsSupporter = vm.IsSupporter;
			contact.Grandparent = vm.Grandparent;
			contact.FeederSchool = vm.FeederSchool;
			contact.Community = vm.Community;
			contact.OldBoyParent = vm.OldBoyParent;

            return contact;
        }

        #endregion

        #region Index
        // GET: /Contacts/
        [HttpGet]
        public ActionResult Index(int? campaignId, string query, int? page, int? contactTab, int? tagId, int? publicationId, int? supporterType )
        {
            var contacts = db.Contacts.Where(c => c.IsActive || c.IsActive == false);
			filterContacts( campaignId, query, ref contactTab, tagId, ref contacts, publicationId, supporterType );
            int pageNumber = (page ?? 1);

			ContactDetailsList vm = new ContactDetailsList( contacts.OrderBy( o => o.LastName ).ToList( ), campaignId, query, Dal, page, contactTab, tagId, publicationId, supporterType );
            ViewBag.SelectedTab = (int)Enums.Controller.Contact;

            return View(vm);
        }

		[HttpGet]
		public ActionResult Audit( )
		{
			ContactAuditHistoryList vm = new ContactAuditHistoryList( Dal );
			ViewBag.SelectedTab = ( int )Enums.Controller.Contact;

			return View( vm );
		}

        [HttpPost]
        public ActionResult Index(ContactDetailsList vm)
        {
			return RedirectToAction( "Index", new { campaignId = vm.CampaignId, query = vm.Query, tagId = vm.TagId, contactTab = vm.ContactTab, publicationId = vm.PublicationId, supporterType = vm.SupporterType } );
        }

        #endregion

        #region PrintAddressGrid

		public ActionResult ExportToExcel( int? campaignId, string query, int? tagId, int? contactTab, int? publicationId, int? supporterType )
		{
			var workbook = new XLWorkbook( );
			var worksheet = workbook.Worksheets.Add( "ContactAddresses" );


			var contacts = new System.Data.DataTable( "contacts" );
			contacts.Columns.Add( "col1", typeof( string ) );
			var filteredContacts = db.Contacts.Where( c => c.Street != null && c.Street != "address unknown" && ( c.IsActive || c.IsActive == false ) && c.ContactTagMaps.Any( t => t.IsActive && ( t.TagId == 15 || t.TagId == 16 ) ) == false );
			filterContacts( campaignId, query, ref contactTab, tagId, ref filteredContacts, publicationId, supporterType );
			ContactAddressGridList vm = new ContactAddressGridList( filteredContacts.OrderBy( o => o.LastName ).ToList( ), campaignId, query, Dal );

			int index = 2;

			worksheet.Row( 1 ).Cell( 1 ).Value = "Unique Identifier";
			worksheet.Row( 1 ).Cell( 2 ).Value = "Mailing Salutation";
			worksheet.Row( 1 ).Cell( 3 ).Value = "Address Line 1";
			worksheet.Row( 1 ).Cell( 4 ).Value = "Address Line 2";
			worksheet.Row( 1 ).Cell( 5 ).Value = "Address Line 3";
			worksheet.Row( 1 ).Cell( 6 ).Value = "Address Line 4";
			worksheet.Row( 1 ).Cell( 7 ).Value = "Address Line 5";
			worksheet.Row( 1 ).Cell( 8 ).Value = "Address Line 6";

			foreach ( var contact in filteredContacts )
			{
				worksheet.Row( index ).Cell( 1 ).Value = contact.Id;
				worksheet.Row( index ).Cell( 2 ).Value = contact.Title + " " + contact.Initials + " " + contact.LastName;
				worksheet.Row( index ).Cell( 3 ).Value = contact.Street;
				worksheet.Row( index ).Cell( 4 ).Value = contact.Town;
				worksheet.Row( index ).Cell( 5 ).Value = contact.City;
				worksheet.Row( index ).Cell( 6 ).Value = contact.Province;
				worksheet.Row( index ).Cell( 7 ).Value = contact.ZipCode;
				worksheet.Row( index ).Cell( 8 ).Value = contact.Country;

				index++;
			}

			Response.Clear( );
			Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
			Response.AddHeader( "content-disposition", "attachment;filename=\"ContactAddresses.xlsx\"" );

			// Flush the workbook to the Response.OutputStream
			using ( MemoryStream memoryStream = new MemoryStream( ) )
			{
				workbook.SaveAs( memoryStream );
				memoryStream.WriteTo( Response.OutputStream );
				memoryStream.Close( );
			}

			Response.End( );

			return new EmptyResult( );
		}

        public ActionResult ExportAllContactDetailsToExcel( string passcode )
        {
            if ( passcode != "c9629f0e-ca6f-4d30-98b7-1c4ae9e3cf1d" )
            {
                return RedirectToAction( "Index" );
            }

            var workbook = new XLWorkbook( );
            var worksheet = workbook.Worksheets.Add( "ContactDetails" );


            var contacts = new System.Data.DataTable( "contacts" );
            contacts.Columns.Add( "col1", typeof( string ) );
            var filteredContacts = db.Contacts.ToList( );
            int index = 2;

            worksheet.Row( 1 ).Cell( 1 ).Value = "Unique Identifier(NEVER CHANGE EXISTING IDENTIFIER - LEAVE BLANK FOR NEW CONTACTS)";
            worksheet.Row( 1 ).Cell( 2 ).Value = "Title";
            worksheet.Row( 1 ).Cell( 3 ).Value = "Initials";
            worksheet.Row( 1 ).Cell( 4 ).Value = "First Name*";
            worksheet.Row( 1 ).Cell( 5 ).Value = "Last Name*";
            worksheet.Row( 1 ).Cell( 6 ).Value = "Preferred Name";
            worksheet.Row( 1 ).Cell( 7 ).Value = "Email Address";
            worksheet.Row( 1 ).Cell( 8 ).Value = "Mobile Phone";
            worksheet.Row( 1 ).Cell( 9 ).Value = "Home Phone";
            worksheet.Row( 1 ).Cell( 10 ).Value = "Comments";
            worksheet.Row( 1 ).Cell( 11 ).Value = "Street";
            worksheet.Row( 1 ).Cell( 12 ).Value = "Town";
            worksheet.Row( 1 ).Cell( 13 ).Value = "City";
            worksheet.Row( 1 ).Cell( 14 ).Value = "Province";
            worksheet.Row( 1 ).Cell( 15 ).Value = "Post Code*";
            worksheet.Row( 1 ).Cell( 16 ).Value = "Country";
            worksheet.Row( 1 ).Cell( 17 ).Value = "From Year";
            worksheet.Row( 1 ).Cell( 18 ).Value = "To Year";
            worksheet.Row( 1 ).Cell( 19 ).Value = "DOB";
            worksheet.Row( 1 ).Cell( 20 ).Value = "House (Aiden/Oswald/Durham/Cuthbert)";
            worksheet.Row( 1 ).Cell( 21 ).Value = "Old Boy? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 22 ).Value = "Donor? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 23 ).Value = "Supporter? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 24 ).Value = "Deceased? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 25 ).Value = "No Correspondence? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 26 ).Value = "Anonymous Donor? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 27 ).Value = "Asia? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 28 ).Value = "Auckland? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 29 ).Value = "Australia? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 30 ).Value = "Central Hawke's Bay? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 31 ).Value = "East Coast? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 32 ).Value = "Europe? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 33 ).Value = "Ex Staff? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 34 ).Value = "Hawke's Bay? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 35 ).Value = "Is a College? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 36 ).Value = "Manawatu? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 37 ).Value = "Pacific Islands? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 38 ).Value = "South Island? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 39 ).Value = "Southern Hawke's Bay? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 40 ).Value = "USA? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 41 ).Value = "Waikato - Bay of Plenty? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 42 ).Value = "Wairarapa? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 43 ).Value = "Wellington? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 44 ).Value = "West Coast? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 45 ).Value = "Highways Publication? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 46 ).Value = "Rector's News? (true/false or blank)";
            worksheet.Row( 1 ).Cell( 47 ).Value = "Chronicle? (true/false or blank)";

            foreach ( var contact in filteredContacts )
            {
                worksheet.Row( index ).Cell( 1 ).Value = contact.Id;
                worksheet.Row( index ).Cell( 2 ).Value = contact.Title;
                worksheet.Row( index ).Cell( 3 ).Value = contact.Initials;
                worksheet.Row( index ).Cell( 4 ).Value = contact.FirstName;
                worksheet.Row( index ).Cell( 5 ).Value = contact.LastName;
                worksheet.Row( index ).Cell( 6 ).Value = contact.PrefName;
                worksheet.Row( index ).Cell( 7 ).Value = contact.Email;
                worksheet.Row( index ).Cell( 8 ).Value = contact.MobilePhone;
                worksheet.Row( index ).Cell( 9 ).Value = contact.HomePhone;
                worksheet.Row( index ).Cell( 10 ).Value = contact.Comments;
                worksheet.Row( index ).Cell( 11 ).Value = contact.Street;
                worksheet.Row( index ).Cell( 12 ).Value = contact.Town;
                worksheet.Row( index ).Cell( 13 ).Value = contact.City;
                worksheet.Row( index ).Cell( 14 ).Value = contact.Province;
                worksheet.Row( index ).Cell( 15 ).Value = contact.ZipCode;
                worksheet.Row( index ).Cell( 16 ).Value = contact.Country;
                worksheet.Row( index ).Cell( 17 ).Value = contact.YearFrom;
                worksheet.Row( index ).Cell( 18 ).Value = contact.YearTo;
                worksheet.Row( index ).Cell( 19 ).Value = contact.DateOfBirth;
                worksheet.Row( index ).Cell( 20 ).Value = contact.House.HasValue ? ( ( Enums.House )contact.House.Value ).ToString( ) : string.Empty;
                worksheet.Row( index ).Cell( 21 ).Value = contact.IsOldBoy ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 22 ).Value = contact.IsDonor ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 23 ).Value = contact.IsSupporter ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 24 ).Value = contact.ContactTagMaps.Any( a => a.IsActive && a.Id == 15 ) ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 25 ).Value = contact.ContactTagMaps.Any( a => a.IsActive && a.Id == 16 ) ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 26 ).Value = contact.ContactTagMaps.Any( a => a.IsActive && a.Id == 18 ) ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 27 ).Value = contact.ContactTagMaps.Any( a => a.IsActive && a.Id == 12 ) ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 28 ).Value = contact.ContactTagMaps.Any( a => a.IsActive && a.Id == 9 ) ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 29 ).Value = contact.ContactTagMaps.Any( a => a.IsActive && a.Id == 11 ) ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 30 ).Value = contact.ContactTagMaps.Any( a => a.IsActive && a.Id == 3 ) ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 31 ).Value = contact.ContactTagMaps.Any( a => a.IsActive && a.Id == 2 ) ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 32 ).Value = contact.ContactTagMaps.Any( a => a.IsActive && a.Id == 14 ) ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 33 ).Value = contact.ContactTagMaps.Any( a => a.IsActive && a.Id == 26 ) ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 34 ).Value = contact.ContactTagMaps.Any( a => a.IsActive && a.Id == 1 ) ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 35 ).Value = contact.ContactTagMaps.Any( a => a.IsActive && a.Id == 25 ) ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 36 ).Value = contact.ContactTagMaps.Any( a => a.IsActive && a.Id == 24 ) ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 37 ).Value = contact.ContactTagMaps.Any( a => a.IsActive && a.Id == 21 ) ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 38 ).Value = contact.ContactTagMaps.Any( a => a.IsActive && a.Id == 10 ) ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 39 ).Value = contact.ContactTagMaps.Any( a => a.IsActive && a.Id == 4 ) ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 40 ).Value = contact.ContactTagMaps.Any( a => a.IsActive && a.Id == 13 ) ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 41 ).Value = contact.ContactTagMaps.Any( a => a.IsActive && a.Id == 8 ) ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 42 ).Value = contact.ContactTagMaps.Any( a => a.IsActive && a.Id == 5 ) ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 43 ).Value = contact.ContactTagMaps.Any( a => a.IsActive && a.Id == 6 ) ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 44 ).Value = contact.ContactTagMaps.Any( a => a.IsActive && a.Id == 7 ) ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 45 ).Value = contact.Highways ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 46 ).Value = contact.RectorsNews ? "true" : string.Empty;
                worksheet.Row( index ).Cell( 47 ).Value = contact.Chronicle ? "true" : string.Empty;
            
                index++;
            }

            Response.Clear( );
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader( "content-disposition", "attachment;filename=\"ContactAddresses.xlsx\"" );

            // Flush the workbook to the Response.OutputStream
            using ( MemoryStream memoryStream = new MemoryStream( ) )
            {
                workbook.SaveAs( memoryStream );
                memoryStream.WriteTo( Response.OutputStream );
                memoryStream.Close( );
            }

            Response.End( );

            return new EmptyResult( );
        }

		public ActionResult GetAddressGrid( int? campaignId, string query, int? tagId, int? contactTab, int? publicationId, int? supporterType )
		{
			string html = PrintAddressGrid( campaignId, query, tagId, contactTab, publicationId, supporterType );

			// Create a Document object
			var document = new Document( PageSize.A4, 20, -5, 58, -10 );

			var output = new MemoryStream( );
			var writer = PdfWriter.GetInstance( document, output );

			// Open the Document for writing
			document.Open( );

			// Step 4: Parse the HTML string into a collection of elements...
			var parsedHtmlElements = HTMLWorker.ParseToList( new StringReader( html ), null );

			// Enumerate the elements, adding each one to the Document...
			foreach ( var htmlElement in parsedHtmlElements )
				document.Add( htmlElement as IElement );

			// Close the Document - this saves the document contents to the output stream
			document.Close( );

			Response.ContentType = "application/pdf";
			Response.AddHeader( "Content-Disposition", string.Format( "attachment;filename=AddressLabels-{0}.pdf", "pdfFile" ) );
			Response.BinaryWrite( output.ToArray( ) );

			return new EmptyResult( );
		}

		public string PrintAddressGrid( int? campaignId, string query, int? tagId, int? contactTab, int? publicationId, int? supporterType )
        {
			var contacts = db.Contacts.Where( c => c.Street != null && ( c.IsActive || c.IsActive == false ) && c.ContactTagMaps.Any( t => t.IsActive && ( t.TagId == 15 || t.TagId == 16 ) )== false );
			filterContacts( campaignId, query, ref contactTab, tagId, ref contacts, publicationId, supporterType );
            ContactAddressGridList vm = new ContactAddressGridList(contacts.OrderBy( o=>o.LastName ).ToList(), campaignId, query, Dal);

            string text = Dal.RazorTemplates.Where(r => r.Id == 1).First().Template;

            string renderedText = Razor.Parse(text, vm);
            return renderedText;
        }

		private static void filterContacts( int? campaignId, string query, ref int? contactTab, int? tagId, ref IQueryable<Contact> contacts, int? publicationId, int? supporterType )
		{
			if ( !String.IsNullOrWhiteSpace( query ) )
			{
				contacts = contacts.Where( c => c.FirstName.Contains( query ) || c.LastName.Contains( query ) || ( c.FirstName + " " + c.LastName ).Contains( query ) );
			}

			if ( campaignId.HasValue )
			{
				contacts = contacts.Where( c => c.Donations.Any( d => d.CampaignId == campaignId ) );
			}

			if ( tagId.HasValue )
			{
				contacts = contacts.Where( c => c.ContactTagMaps.Any( t => t.TagId == tagId.Value && t.ContactId == c.Id && t.IsActive ) );
			}

			if ( publicationId.HasValue )
			{
				switch ( publicationId.Value )
				{
					case ( int )Enums.Publications.Highways:
						contacts = contacts.Where( c => c.Highways );
						break;

					case ( int )Enums.Publications.Chronicle:
						contacts = contacts.Where( c => c.Chronicle );
						break;

					case ( int )Enums.Publications.RectorsNews:
						contacts = contacts.Where( c => c.RectorsNews );
						break;
				}

			}

			if ( supporterType.HasValue )
			{
				switch ( supporterType.Value )
				{
					case ( int )Enums.SupporterTypes.Grandparent:
						contacts = contacts.Where( c => c.Grandparent );
						break;

					case ( int )Enums.SupporterTypes.Community:
						contacts = contacts.Where( c => c.Community );
						break;

					case ( int )Enums.SupporterTypes.FeederSchool:
						contacts = contacts.Where( c => c.FeederSchool );
						break;

					case ( int )Enums.SupporterTypes.OldBoyParent:
						contacts = contacts.Where( c => c.OldBoyParent );
						break;
				}

			}


			contactTab = contactTab.HasValue ? contactTab.Value : ( int )Enums.ContactTab.All;

			switch ( contactTab.Value )
			{
				case ( int )Enums.ContactTab.All:
					contacts = contacts.Where( c => c.IsActive );
					break;

				case ( int )Enums.ContactTab.OldBoy:
					contacts = contacts.Where( c => c.IsActive && c.IsOldBoy );
					break;

				case ( int )Enums.ContactTab.Donor:
					contacts = contacts.Where( c => c.IsActive && c.IsDonor );
					break;

				case ( int )Enums.ContactTab.Archived:
					contacts = contacts.Where( c => c.IsActive == false );
					break;

				case ( int )Enums.ContactTab.SetProvince:
					contacts = contacts.Where( c => c.IsActive && ( c.ContactTagMaps.Any( cm => cm.IsActive && cm.TagId > 0 && cm.TagId < 15 || cm.TagId == 21 ) == false ) && c.Street != null );
					break;

				case ( int )Enums.ContactTab.Support:
					contacts = contacts.Where( c => c.IsActive && c.IsSupporter );
					break;

				default:
					break;
			}
		}

        #endregion

        #region CRUD

        // GET: /Contacts/Create
        public ActionResult Create(ContactSearchContext search)
        {
            ContactDetails vm = new ContactDetails();
            vm.SearchOptions = search;
            vm.TagCheckBoxes = new List<ContactTagCheckBoxes>();
            vm.TagCheckBoxes.AddRange(Dal.Tags.Where(t => t.IsActive).OrderBy(n => n.Name).ToList().Select(s => new ContactTagCheckBoxes(s)));
			vm.HouseSelectList = SelectLists.HouseSelectList( null );

            ViewBag.SelectedTab = (int)Enums.Controller.Contact;
            return View(vm);
        }

        // POST: /Contacts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ContactDetails vm, ContactSearchContext search)
        {
            if (ModelState.IsValid)
            {
                Contact contact = SetContactData(vm);

                db.Contacts.Add(contact);
                db.SaveChanges();

				ContactAudit audit = new ContactAudit( );
				audit.UserId = CurrentUser.Id;
				audit.ContactId = contact.Id;
				audit.DateCreated = DateTime.Now;

				db.ContactAudits.Add( audit );
				db.SaveChanges( );

                return RedirectToAction("Index", search);
            }

            //disposables
			vm.HouseSelectList = SelectLists.HouseSelectList( vm.HouseId );
            ViewBag.SelectedTab = (int)Enums.Controller.Contact;
            return View(vm);
        }

        // GET: /Contacts/Edit/5
        public ActionResult Edit(int id, ContactSearchContext search)
        {
            Contact contact = db.Contacts.First(c => c.Id == id && c.IsActive);
            var Tags = Dal.Tags.Where(t => t.IsActive).OrderBy(n=>n.Name).ToList();
            ContactDetails vm = new ContactDetails(contact, Dal, Tags);
            vm.SearchOptions = search;
            ViewBag.SelectedTab = (int)Enums.Controller.Contact;
            return View(vm);
        }

        // POST: /Contacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ContactDetails vm, ContactSearchContext search)
        {
            if (ModelState.IsValid)
            {
                Contact dbContact = db.Contacts.Where(c => c.Id == vm.Id && c.IsActive).First();
                Contact contact = UpdateContactData(vm, dbContact);

                db.SaveChanges();

                foreach (var tag in contact.ContactTagMaps.Where(t => t.IsActive))
                {
                    if (vm.TagCheckBoxes.Any(c => c.Id == tag.TagId && c.IsSelected) == false)
                    {
                        tag.IsActive = false;
                    }
                }

                db.SaveChanges();

                if (vm.TagCheckBoxes != null && vm.TagCheckBoxes.Any())
                {
                    foreach (var cb in vm.TagCheckBoxes.Where(c => c.IsSelected))
                    {
                        if (contact.ContactTagMaps.Any(c => c.TagId == cb.Id && c.IsActive) == false)
                        {
                            ContactTagMap ct = new ContactTagMap();

                            ct.ContactId = contact.Id;
                            ct.TagId = cb.Id;
                            ct.IsActive = true;

                            contact.ContactTagMaps.Add(ct);
                        }
                    }
                }

                if (contact.ContactTagMaps.Any(c => c.IsActive && c.TagId == 15))
                {
                    contact.IsActive = false;
                }

                db.SaveChanges();

                return RedirectToAction("Index", search);
            }

			vm.HouseSelectList = SelectLists.HouseSelectList( vm.HouseId );
            ViewBag.SelectedTab = (int)Enums.Controller.Contact;
            return View(vm);
        }

        // GET: /Contacts/Delete/5
        public ActionResult Archive(int id)
        {
            Contact contact = db.Contacts.First(c => c.Id == id && c.IsActive);
            var Tags = Dal.Tags.Where(t => t.IsActive).ToList();
            ContactDetails vm = new ContactDetails(contact, Dal, Tags);
            if (string.IsNullOrWhiteSpace(contact.FirstName))
            {
                vm.FirstName = "tempName";
            }

			if( string.IsNullOrWhiteSpace(contact.ZipCode))
			{
				vm.ZipCode = "1234";
			}

            ViewBag.SelectedTab = (int)Enums.Controller.Contact;
            return View(vm);
        }

        // POST: /Contacts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Archive(ContactDetails vm)
        {
            if (ModelState.IsValid)
            {
                Contact contact = db.Contacts.Where(c => c.Id == vm.Id && c.IsActive).First();
                contact.IsActive = false;

                db.Entry(contact).State = EntityState.Modified;
                db.SaveChanges();

				ContactAudit audit = new ContactAudit( );
				audit.UserId = CurrentUser.Id;
				audit.ContactId = contact.Id;
				audit.DateArchived = DateTime.Now;

				db.ContactAudits.Add( audit );
				db.SaveChanges( );

                return RedirectToAction("Index");
            }

            ViewBag.SelectedTab = (int)Enums.Controller.Contact;
            return View(vm);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        public ActionResult Unarchive(int id)
        {
            Contact Con = Dal.Contacts.FirstOrDefault(c => c.Id == id && c.IsActive == false);

            if (Con != null)
            {
                Con.IsActive = true;
                Dal.SaveChanges();

				ContactAudit audit = new ContactAudit( );
				audit.UserId = CurrentUser.Id;
				audit.ContactId = Con.Id;
				audit.DateUnarchived = DateTime.Now;

				db.ContactAudits.Add( audit );
				db.SaveChanges( );
            }

            return RedirectToAction("Index", new { status = (int)Enums.ContactTab.Archived});
        }

        #endregion

        [HttpGet]
        public ActionResult UploadNewContacts( bool? fileUploaded = false )
        {
            ViewBag.fileUploaded = fileUploaded;
            return View( );
        }

        [HttpPost]
        public ActionResult UploadNewContacts( )
		{
			List<string> errorList = new List<string>( );
			int counter = 1;
            int contactsToCreate = 0;
			//open file
			if ( Request.Files.Count == 1 )
			{
				//get file
				var postedFile = Request.Files[ 0 ];
				var tags = Dal.Tags.Where( t => t.IsActive ).ToList( );
				if ( postedFile.ContentLength > 0 )
				{
					//read data from input stream
					using ( var csvReader = new System.IO.StreamReader( postedFile.InputStream ) )
					{
						string inputLine = "";

						//read each line
						while ( ( inputLine = csvReader.ReadLine( ) ) != null )
						{
							if ( counter > 1 )
							{
								//get lines values
								string[ ] values = inputLine.Split( new char[ ] { ',' } );

								if ( values[ 0 ].Length > 30 )
								{
									errorList.Add( "Line " + counter + ": Title exceeds 30 characters" );
								}

								if ( values[ 1 ].Length > 30 )
								{
									errorList.Add( "Line " + counter + ": Initials exceeds 30 characters" );
								}

								if ( string.IsNullOrWhiteSpace( values[ 2 ] ) )
								{
									errorList.Add( "Line " + counter + ": First Name is Required" );
								}
								else if ( values[ 2 ].Length > 150 )
								{
									errorList.Add( "Line " + counter + ": First Name exceeds 150 characters" );
								}

								if ( string.IsNullOrWhiteSpace( values[ 3 ] ) )
								{
									errorList.Add( "Line " + counter + ": Last Name is Required" );
								}
								else if ( values[ 3 ].Length > 150 )
								{
									errorList.Add( "Line " + counter + ": Last Name exceeds 150 characters" );
								}

								if ( values[ 4 ].Length > 150 )
								{
									errorList.Add( "Line " + counter + ": Preferred Name exceeds 150 characters" );
								}

								if ( values[ 5 ].Length > 150 )
								{
									errorList.Add( "Line " + counter + ": Street exceeds 150 characters" );
								}

								if ( values[ 6 ].Length > 150 )
								{
									errorList.Add( "Line " + counter + ": Town exceeds 150 characters" );
								}

								if ( values[ 7 ].Length > 150 )
								{
									errorList.Add( "Line " + counter + ": City exceeds 150 characters" );
								}

								if ( values[ 8 ].Length > 150 )
								{
									errorList.Add( "Line " + counter + ": Province exceeds 150 characters" );
								}

								if ( values[ 9 ].Length > 150 )
								{
									errorList.Add( "Line " + counter + ": Country exceeds 150 characters" );
								}

								if ( string.IsNullOrWhiteSpace( values[ 10 ] ) )
								{
									errorList.Add( "Line " + counter + ": Post Code is Required" );
								}
								else if ( values[ 10 ].Length > 20 )
								{
									errorList.Add( "Line " + counter + ": Post Code exceeds 20 characters" );
								}

								if ( values[ 11 ].Length > 50 )
								{
									errorList.Add( "Line " + counter + ": Telephone exceeds 50 characters" );
								}

								if ( values[ 12 ].Length > 50 )
								{
									errorList.Add( "Line " + counter + ": Mobile Phone exceeds 50 characters" );
								}

								if ( values[ 13 ].Length > 255 )
								{
									errorList.Add( "Line " + counter + ": Email exceeds 255 characters" );
								}

								if ( values[ 16 ].Length > 50 )
								{
									errorList.Add( "Line " + counter + ": Date of birth exceeds 50 characters" );
								}

								if ( values[ 17 ].Length > 1500 )
								{
									errorList.Add( "Line " + counter + ": Comments exceeds 1500 characters" );
								}

								if ( !string.IsNullOrWhiteSpace( values[ 18 ] ) )
								{
									if ( values[ 18 ].ToLower( ).Trim( ) != Classes.Enums.House.Aidan.ToString( ).ToLower( ) &&
										values[ 18 ].ToLower( ).Trim( ) != Classes.Enums.House.Cuthbert.ToString( ).ToLower( ) &&
										values[ 18 ].ToLower( ).Trim( ) != Classes.Enums.House.Durham.ToString( ).ToLower( ) &&
										values[ 18 ].ToLower( ).Trim( ) != Classes.Enums.House.Oswald.ToString( ).ToLower( ) )
									{
										errorList.Add( "Line " + counter + ": House must be " + Classes.Enums.House.Aidan.ToString( ) + ", " + Classes.Enums.House.Cuthbert.ToString( )
											+ ", " + Classes.Enums.House.Durham.ToString( ) + " or " + Classes.Enums.House.Oswald.ToString( ) );
									}
								}

								if ( values[ 19 ].Length > 4 )
								{
									errorList.Add( "Line " + counter + ": Year From exceeds 4 characters" );
								}

								if ( values[ 20 ].Length > 4 )
								{
									errorList.Add( "Line " + counter + ": Year To exceeds 4 characters" );
								}

								//tag 29, 30, 31, 32, 33
								if ( !string.IsNullOrWhiteSpace( values[ 29 ] ) && tags.Any( t => t.Name.ToLower( ) == values[ 29 ].ToLower( ).Trim( ) ) == false )
								{
									errorList.Add( "Line " + counter + ": " + values[ 29 ] + " tag doesn't exist in the application" );
								}

								if ( !string.IsNullOrWhiteSpace( values[ 30 ] ) && tags.Any( t => t.Name.ToLower( ) == values[ 30 ].ToLower( ).Trim( ) ) == false )
								{
									errorList.Add( "Line " + counter + ": " + values[ 30 ] + " tag doesn't exist in the application" );
								}

								if ( !string.IsNullOrWhiteSpace( values[ 31 ] ) && tags.Any( t => t.Name.ToLower( ) == values[ 31 ].ToLower( ).Trim( ) ) == false )
								{
									errorList.Add( "Line " + counter + ": " + values[ 31 ] + " tag doesn't exist in the application" );
								}

								if ( !string.IsNullOrWhiteSpace( values[ 32 ] ) && tags.Any( t => t.Name.ToLower( ) == values[ 32 ].ToLower( ).Trim( ) ) == false )
								{
									errorList.Add( "Line " + counter + ": " + values[ 32 ] + " tag doesn't exist in the application" );
								}

								if ( !string.IsNullOrWhiteSpace( values[ 33 ] ) && tags.Any( t => t.Name.ToLower( ) == values[ 33 ].ToLower( ).Trim( ) ) == false )
								{
									errorList.Add( "Line " + counter + ": " + values[ 33 ] + " tag doesn't exist in the application" );
								}

                                contactsToCreate++;
							}

							counter++;
						}

						csvReader.Close( );
					}

                    if ( contactsToCreate == 0 )
                    {
                        errorList.Add( "No valid contacts in CSV file." );
                    }

                    if ( !errorList.Any( ) )
                    {
                        using ( var csvReader = new System.IO.StreamReader( postedFile.InputStream ) )
                        {
                            counter = 1;
                            string inputLine = "";

                            //read each line
                            while ( ( inputLine = csvReader.ReadLine( ) ) != null )
                            {
                                string[ ] values = inputLine.Split( new char[ ] { ',' } );
                                if ( counter > 1 )
                                {
                                    Contact contact = new Contact( );
                                    contact.Title = values[ 0 ].Trim( );
                                    contact.Initials = values[ 1 ].Trim( );
                                    contact.FirstName = values[ 2 ].Trim( );
                                    contact.LastName = values[ 3 ].Trim( );
                                    contact.PrefName = values[ 4 ].Trim( );
                                    contact.Street = values[ 5 ].Trim( );
                                    contact.Town = values[ 6 ].Trim( );
                                    contact.City = values[ 7 ].Trim( );
                                    contact.Province = values[ 8 ].Trim( );
                                    contact.Country = values[ 9 ].Trim( );
                                    contact.ZipCode = values[ 10 ].Trim( );
                                    contact.HomePhone = values[ 11 ].Trim( );
                                    contact.MobilePhone = values[ 12 ].Trim( );
                                    contact.Email = values[ 13 ].Trim( );

                                    contact.IsOldBoy = values[ 14 ].Trim( ).ToLower( ) == "true";
                                    contact.IsDonor = values[ 15 ].Trim( ).ToLower( ) == "true";

                                    contact.DateOfBirth = values[ 16 ].Trim( );
                                    contact.Comments = values[ 17 ].Trim( );

                                    if ( !string.IsNullOrWhiteSpace( values[ 18 ] ) )
                                    {
                                        if ( values[ 18 ].ToLower( ).Trim( ) == Classes.Enums.House.Aidan.ToString( ).ToLower( ) )
                                        {
                                            contact.House = ( int )Classes.Enums.House.Aidan;
                                        }
                                        else if ( values[ 18 ].ToLower( ).Trim( ) == Classes.Enums.House.Cuthbert.ToString( ).ToLower( ) )
                                        {
                                            contact.House = ( int )Classes.Enums.House.Cuthbert;
                                        }
                                        else if ( values[ 18 ].ToLower( ).Trim( ) == Classes.Enums.House.Durham.ToString( ).ToLower( ) )
                                        {
                                            contact.House = ( int )Classes.Enums.House.Durham;
                                        }
                                        else if ( values[ 18 ].ToLower( ).Trim( ) == Classes.Enums.House.Oswald.ToString( ).ToLower( ) )
                                        {
                                            contact.House = ( int )Classes.Enums.House.Oswald;
                                        }
                                    }

                                    contact.YearFrom = values[ 19 ].Trim( );
                                    contact.YearTo = values[ 20 ].Trim( );

                                    contact.Highways = values[ 21 ].Trim( ).ToLower( ) == "true";
                                    contact.Chronicle = values[ 22 ].Trim( ).ToLower( ) == "true";
                                    contact.RectorsNews = values[ 23 ].Trim( ).ToLower( ) == "true";

                                    contact.IsSupporter = values[ 24 ].Trim( ).ToLower( ) == "true";
                                    contact.Grandparent = values[ 25 ].Trim( ).ToLower( ) == "true";
                                    contact.OldBoyParent = values[ 26 ].Trim( ).ToLower( ) == "true";
                                    contact.FeederSchool = values[ 27 ].Trim( ).ToLower( ) == "true";
                                    contact.Community = values[ 28 ].Trim( ).ToLower( ) == "true";

                                    contact.DateCreated = DateTime.Now;
                                    contact.IsActive = true;

                                    Dal.Contacts.Add( contact );
                                    Dal.SaveChanges( );

                                    if ( !string.IsNullOrWhiteSpace( values[ 29 ] ) && tags.Any( t => t.Name.ToLower( ) == values[ 29 ].Trim( ).ToLower( ) ) )
                                    {
                                        ContactTagMap ctm = new ContactTagMap( );
                                        ctm.ContactId = contact.Id;
                                        ctm.IsActive = true;
                                        ctm.TagId = tags.First( t => t.Name.ToLower( ) == values[ 29 ].Trim( ).ToLower( ) ).Id;

                                        Dal.ContactTagMaps.Add( ctm );
                                    }

                                    if ( !string.IsNullOrWhiteSpace( values[ 30 ] ) && tags.Any( t => t.Name.ToLower( ) == values[ 30 ].Trim( ).ToLower( ) ) )
                                    {
                                        ContactTagMap ctm = new ContactTagMap( );
                                        ctm.ContactId = contact.Id;
                                        ctm.IsActive = true;
                                        ctm.TagId = tags.First( t => t.Name.ToLower( ) == values[ 30 ].Trim( ).ToLower( ) ).Id;

                                        Dal.ContactTagMaps.Add( ctm );
                                    }

                                    if ( !string.IsNullOrWhiteSpace( values[ 31 ] ) && tags.Any( t => t.Name.ToLower( ) == values[ 31 ].Trim( ).ToLower( ) ) )
                                    {
                                        ContactTagMap ctm = new ContactTagMap( );
                                        ctm.ContactId = contact.Id;
                                        ctm.IsActive = true;
                                        ctm.TagId = tags.First( t => t.Name.ToLower( ) == values[ 31 ].Trim( ).ToLower( ) ).Id;

                                        Dal.ContactTagMaps.Add( ctm );
                                    }

                                    if ( !string.IsNullOrWhiteSpace( values[ 32 ] ) && tags.Any( t => t.Name.ToLower( ) == values[ 32 ].Trim( ).ToLower( ) ) )
                                    {
                                        ContactTagMap ctm = new ContactTagMap( );
                                        ctm.ContactId = contact.Id;
                                        ctm.IsActive = true;
                                        ctm.TagId = tags.First( t => t.Name.ToLower( ) == values[ 32 ].Trim( ).ToLower( ) ).Id;

                                        Dal.ContactTagMaps.Add( ctm );
                                    }

                                    if ( !string.IsNullOrWhiteSpace( values[ 33 ] ) && tags.Any( t => t.Name.ToLower( ) == values[ 33 ].Trim( ).ToLower( ) ) )
                                    {
                                        ContactTagMap ctm = new ContactTagMap( );
                                        ctm.ContactId = contact.Id;
                                        ctm.IsActive = true;
                                        ctm.TagId = tags.First( t => t.Name.ToLower( ) == values[ 33 ].Trim( ).ToLower( ) ).Id;

                                        Dal.ContactTagMaps.Add( ctm );
                                    }

                                    ContactAudit ca = new ContactAudit( );
                                    ca.ContactId = contact.Id;
                                    ca.DateCreated = DateTime.Now;
                                    ca.UserId = CurrentUser.Id;

                                    Dal.ContactAudits.Add( ca );
                                    Dal.SaveChanges( );
                                }
                            }
                            csvReader.Close( );
                        }
                    }
				}
                else
                {
                    errorList.Add( "CSV file not valid." );
                }
			}
            else
            {
                errorList.Add( "CSV file not valid." );
            }

            if ( errorList.Any( ) )
            {
                ViewBag.fileUploaded = false;
                ViewBag.ErrorList = errorList;
                ViewBag.ReturnUrl = Url.Action( "Index" );
                return View( );
            }

            return RedirectToAction( "UploadNewContacts", new { fileUploaded = true } );
		}

    }
}
