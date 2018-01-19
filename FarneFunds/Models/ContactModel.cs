using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FarneFunds.Database;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;
using FarneFunds.Classes;
using PagedList;

namespace FarneFunds.Models
{
	public class ContactSearchContext
	{
		public int? CampaignId { get; set; }
		public string Query { get; set; }
		public int? Page { get; set; }
		public int? ContactTab { get; set; }
		public int? TagId { get; set; }
		public int? PublicationId { get; set; }
		public int? SupporterType { get; set; }

		public ContactSearchContext( )
		{
			CampaignId = ( int? )null;
			Page = 1;
			ContactTab = 0;
		}

		public ContactSearchContext( int? campaignId, string query, int? page, int? contactTab, int? tagId, int? publicationId, int? supporterType )
		{
			CampaignId = campaignId;
			Query = query;
			Page = page;
			ContactTab = contactTab;
			TagId = tagId;
			PublicationId = publicationId;
			SupporterType = supporterType;
		}

	}

	public class ContactModel
	{
	}

	public class ContactDetails
	{
		public ContactSearchContext SearchOptions { get; set; }

		public int Id { get; set; }

		[StringLength( 150, ErrorMessage = "Max 150 characters." )]
		[DisplayName( "First Name*" )]
		public string FirstName { get; set; }

		[Required]
		[StringLength( 150, ErrorMessage = "Max 150 characters." )]
		[DisplayName( "Last Name*" )]
		public string LastName { get; set; }

		[DisplayName( "Preferred Name" )]
		[StringLength( 150, ErrorMessage = "Max 150 characters." )]
		public string PreferredName { get; set; }

		[DisplayName( "Title" )]
		[StringLength( 30, ErrorMessage = "Max 30 characters." )]
		public string Title { get; set; }

		[DisplayName( "Mobile Phone" )]
		[StringLength( 20, ErrorMessage = "Max 20 characters." )]
		public string MobilePhone { get; set; }

		[DisplayName( "Home Phone" )]
		[StringLength( 20, ErrorMessage = "Max 20 characters." )]
		public string HomePhone { get; set; }

		[Display( Name = "Email address" )]
		[EmailAddress( ErrorMessage = "Invalid Email Address" )]
		[StringLength( 255, ErrorMessage = "Max 255 characters." )]
		public string EmailAddress { get; set; }

		[StringLength( 250, ErrorMessage = "Max 250 characters." )]
		[DisplayName("PO/Street")]
		public string Street { get; set; }

		[StringLength( 150, ErrorMessage = "Max 150 characters." )]
		[DisplayName("RD")]
		public string Town { get; set; }

		[StringLength( 150, ErrorMessage = "Max 150 characters." )]
		[DisplayName("Suburb")]
		public string City { get; set; }

		[StringLength( 150, ErrorMessage = "Max 150 characters." )]
		[DisplayName("City")]
		public string Province { get; set; }

		[StringLength( 150, ErrorMessage = "Max 150 characters." )]
		public string Country { get; set; }

		[Required]
		[DisplayName( "Post Code*" )]
		[StringLength( 10, ErrorMessage = "Max 10 characters." )]
		public string ZipCode { get; set; }

		[DisplayName( "Old Boy" )]
		public bool IsOldBoy { get; set; }

		[DisplayName( "Donor" )]
		public bool IsDonor { get; set; }

		[DisplayName( "Supporter" )]
		public bool IsSupporter { get; set; }

		[DisplayName( "Grandparent" )]
		public bool Grandparent { get; set; }

		[DisplayName( "Old Boy Parent" )]
		public bool OldBoyParent { get; set; }

		[DisplayName( "Feeder School" )]
		public bool FeederSchool { get; set; }

		[DisplayName( "Community" )]
		public bool Community { get; set; }




		public List<ContactTagCheckBoxes> TagCheckBoxes { get; set; }

		[StringLength( 150, ErrorMessage = "Max 150 characters." )]
		[DisplayName( "First Name" )]
		public string PartnerFirstName { get; set; }

		[StringLength( 150, ErrorMessage = "Max 150 characters." )]
		[DisplayName( "Last Name" )]
		public string PartnerLastName { get; set; }

		[DisplayName( "Preferred Name" )]
		[StringLength( 150, ErrorMessage = "Max 150 characters." )]
		public string PartnerPreferredName { get; set; }

		[DisplayName( "Title" )]
		public string PartnerTitle { get; set; }

		[DisplayName( "Mobile Phone" )]
		[StringLength( 20, ErrorMessage = "Max 20 characters." )]
		public string PartnerMobilePhone { get; set; }

		[Display( Name = "Email address" )]
		[EmailAddress( ErrorMessage = "Invalid Email Address" )]
		[StringLength( 255, ErrorMessage = "Max 255 characters." )]
		public string PartnerEmailAddress { get; set; }

		[DisplayName( "Initials" )]
		[StringLength( 30, ErrorMessage = "Max 30 characters." )]
		public string Initials { get; set; }

		[DisplayName( "Comments" )]
		[StringLength( 1500, ErrorMessage = "Max 1500 characters." )]
		public string Comments { get; set; }

		[DisplayName( "Date of birth" )]
		[StringLength( 50, ErrorMessage = "Max 50 characters." )]
		public string DateOfirth { get; set; }

		[DisplayName( "House" )]
		public int? HouseId { get; set; }
		public SelectList HouseSelectList { get; set; }

		[DisplayName( "Years Attended" )]
		[StringLength( 50, ErrorMessage = "Max 50 characters." )]
		public string Year { get; set; }

		[DisplayName( "From Year" )]
		[StringLength( 4, ErrorMessage = "Max 4 characters." )]
		public string FromYear { get; set; }

		[DisplayName( "To Year" )]
		[StringLength( 4, ErrorMessage = "Max 4 characters." )]
		public string ToYear { get; set; }

		[DisplayName( "Highways" )]
		public bool Highways { get; set; }

		[DisplayName( "Chronicle" )]
		public bool Chronicle { get; set; }

		[DisplayName( "Rector's News" )]
		public bool RectorsNews { get; set; }

		public ContactDetails( ) { }
		public ContactDetails( Contact contact, FarneFundsEntities dal, List<Tag> tags )
		{
			Id = contact.Id;
			FirstName = contact.FirstName;
			LastName = contact.LastName;
			PreferredName = contact.PrefName;
			Title = contact.Title;
			MobilePhone = contact.MobilePhone;
			HomePhone = contact.HomePhone;
			EmailAddress = contact.Email;
			Street = contact.Street;
			Town = contact.Town;
			City = contact.City;
			Province = contact.Province;
			Country = contact.Country;
			ZipCode = contact.ZipCode;
			IsOldBoy = contact.IsOldBoy;
			IsDonor = contact.IsDonor;
			Initials = contact.Initials;

			PartnerFirstName = contact.PartnerFirstName;
			PartnerLastName = contact.PartnerLastName;
			PartnerPreferredName = contact.PartnerPrefName;
			PartnerTitle = contact.PartnerTitle;
			PartnerMobilePhone = contact.PartnerMobilePhone;
			PartnerEmailAddress = contact.PartnerEmail;

			TagCheckBoxes = new List<ContactTagCheckBoxes>( );
			TagCheckBoxes.AddRange( tags.Select( s => new ContactTagCheckBoxes( s, contact ) ) );

			HouseSelectList = SelectLists.HouseSelectList( null );
			HouseId = contact.House;
			Comments = contact.Comments;
			DateOfirth = contact.DateOfBirth;
			Year = contact.Year;

			FromYear = contact.YearFrom;
			ToYear = contact.YearTo;

			Chronicle = contact.Chronicle;
			RectorsNews = contact.RectorsNews;
			Highways = contact.Highways;

			IsSupporter = contact.IsSupporter;
			Grandparent = contact.Grandparent;
			FeederSchool = contact.FeederSchool;
			Community = contact.Community;
			OldBoyParent = contact.OldBoyParent;
		}
	}

	public class ContactTagCheckBoxes
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public bool IsSelected { get; set; }

		public ContactTagCheckBoxes( ) { }

		public ContactTagCheckBoxes( Tag DbTag )
		{
			Id = DbTag.Id;
			Name = DbTag.Name;
			IsSelected = false;
		}

		public ContactTagCheckBoxes( Tag DbTag, Contact DbContact )
		{
			Id = DbTag.Id;
			Name = DbTag.Name;
			IsSelected = DbTag.ContactTagMaps.Any( c => c.ContactId == DbContact.Id && c.TagId == DbTag.Id && c.IsActive );
		}
	}

	public class ContactDetailsList
	{
        public IPagedList<Contact> ContactList { get; private set; }
		public int? CampaignId { get; set; }
		public SelectList CampaignSelectList { get; set; }
		public string Query { get; set; }
		public int? ContactTab { get; set; }
		public int? Page { get; set; }
		public int? TagId { get; set; }
		public SelectList TagSelectList { get; set; }

		public int? PublicationId { get; set; }
		public SelectList Publications { get; set; }

		public int? SupporterType { get; set; }
		public SelectList SupporterTypes { get; set; }

		public ContactDetailsList( ) 
        {
 
        }

		public ContactDetailsList( IQueryable<Contact> contacts, int? campaignId, string query, FarneFundsEntities Dal, int? page, int? contactTab, int? tagId, int? publicationId, int? supporterType )
            : this( )
		{
			int pageNumber = ( page ?? 1 );
			Page = pageNumber;

			ContactTab = contactTab;
			var tags = Dal.Tags.Where( t => t.IsActive ).OrderBy( n => n.Name ).ToList( );
            ContactList = contacts.OrderBy( o => o.LastName ).ToPagedList( pageNumber, 21 );

			CampaignId = campaignId;
			CampaignSelectList = SelectLists.CampaignSelectList( campaignId, Dal );

			TagId = tagId;
			TagSelectList = SelectLists.TagSelectList( tagId, Dal );

			PublicationId = publicationId;
			Publications = SelectLists.PublicationCategory( publicationId );

			SupporterType = supporterType;
			SupporterTypes = SelectLists.SupprterTypeSelectList( supporterType );

			Query = query;
		}
	}

	public class ContactAddressGridList
	{
		public List<ContactAddressGridListItem> ContactList { get; private set; }
		public int? CampaignId { get; set; }
		public string Query { get; set; }

		public ContactAddressGridList( List<Contact> contacts, int? campaignId, string query, FarneFundsEntities Dal )
		{
			ContactList = new List<ContactAddressGridListItem>( );
			ContactList.AddRange( contacts.Select( s => new ContactAddressGridListItem( s ) ) );
			CampaignId = campaignId;
			Query = query;
		}
	}

	public class ContactAddressGridListItem
	{
		public string Title { get; set; }
		public string Initals { get; set; }
		public string LastName { get; set; }
		public string StreetAddress { get; set; }
		public string Town { get; set; }
		public string City { get; set; }
		public string Province { get; set; }
		public string Country { get; set; }
		public string ZipCode { get; set; }

		public ContactAddressGridListItem( Contact Con )
		{
			Title = Con.Title;
			Initals = Con.Initials;
			LastName = Con.LastName;
			StreetAddress = Con.Street;
			Town = Con.Town;
			City = Con.City;
			Province = Con.Province;
			Country = Con.Country;
			ZipCode = Con.ZipCode;
		}
	}

	public class ContactAuditHistoryList
	{
		public List<ContactAuditHistory> AuditHistoryList { get; set; }

		public ContactAuditHistoryList(FarneFundsEntities dal )
		{
			var audits = dal.ContactAudits.Where( c => c.ContactId > 0 ).ToList( );

			var auditHistoryList = new List<ContactAuditHistory>();
			auditHistoryList.AddRange( audits.Select( s => new ContactAuditHistory( s ) ) );

			AuditHistoryList = new List<ContactAuditHistory>( );
			AuditHistoryList = auditHistoryList.OrderByDescending( o => o.DateAction ).ToList( );
		}
	}


	public class ContactAuditHistory
	{
		public int ContactId { get; set; }
		public string ContactName { get; set; }
		public string UserName { get; set; }

		public string Action { get; set; }
		public DateTime DateAction { get; set; }

		public ContactAuditHistory( ContactAudit audit )
		{
			ContactId = audit.ContactId;
			ContactName = audit.Contact.FirstName + " " + audit.Contact.LastName;
			UserName = audit.AspNetUser.UserName;

			if( audit.DateCreated.HasValue )
			{
				Action = "Created";
				DateAction = audit.DateCreated.Value;
			}
			else if( audit.DateArchived.HasValue)
			{
				Action = "Archived";
				DateAction = audit.DateArchived.Value;
			}
			else if( audit.DateUnarchived.HasValue)
			{
				Action = "Unarchived";
				DateAction = audit.DateUnarchived.Value;
			}

		}
	}
}