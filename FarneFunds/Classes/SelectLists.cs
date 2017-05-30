using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FarneFunds.Classes;
using FarneFunds.Database;

namespace FarneFunds.Classes
{
    public static class SelectLists
    {
        public static SelectList TitleSelectList(int? defaultTitle)
        {
            var list = new List<SelectListItem>();
            foreach(Enums.Title item in Enum.GetValues( typeof(Enums.Title)))
            {
                list.Add(new SelectListItem() { Text = item.ToString(), Value = ((int)item).ToString() });
            }
            return new SelectList( list, "Value", "Text", defaultTitle);
        }

        public static SelectList ContactSelectList( List<Contact> contacts, int? defaultContact)
        {
            var list = new List<SelectListItem>();
            foreach (var contact in contacts)
            {

                var contactName = "";
                if( !String.IsNullOrWhiteSpace(contact.FirstName) )
                {
                    contactName += contact.FirstName + " " + contact.LastName;
                }
                else
                {
                    contactName = contact.LastName;
                }

                list.Add(new SelectListItem() { Text = contactName, Value = (contact.Id).ToString() });
            }
            return new SelectList(list, "Value", "Text", defaultContact);
        }

        public static SelectList CampaignSelectList(int? CampaignId, FarneFundsEntities Dal)
        {
            var list = new List<SelectListItem>();
            var Campaigns = Dal.Campaigns.Where(c => c.IsActive).ToList();

            foreach (var campaign in Campaigns)
            {
                list.Add(new SelectListItem() { Text = campaign.Name.ToString(), Value = campaign.Id.ToString() });
            }
            return new SelectList(list, "Value", "Text", CampaignId);
        }

        public static SelectList TagSelectList(int? tagId, FarneFundsEntities Dal)
        {
            var list = new List<SelectListItem>();
            var Tags = Dal.Tags.Where(c => c.IsActive).OrderBy(n=>n.Name).ToList();

            foreach (var tag in Tags)
            {
                list.Add(new SelectListItem() { Text = tag.Name.ToString(), Value = tag.Id.ToString() });
            }
            return new SelectList(list, "Value", "Text", tagId);
        }

		public static SelectList HouseSelectList( int? defaultHouse )
		{
			var list = new List<SelectListItem>( );
			foreach ( Enums.House item in Enum.GetValues( typeof( Enums.House ) ) )
			{
				list.Add( new SelectListItem( ) { Text = item.ToString( ), Value = ( ( int )item ).ToString( ) } );
			}
			return new SelectList( list, "Value", "Text", defaultHouse );
		}

		public static SelectList PublicationCategory( int? defaultPublication )
		{
			var list = new List<SelectListItem>( );
			foreach ( Enums.Publications item in Enum.GetValues( typeof( Enums.Publications ) ) )
			{
				if ( item != Enums.Publications.None )
				{
					if ( item == Enums.Publications.RectorsNews )
					{
						list.Add( new SelectListItem( ) { Text = "Rector's News", Value = ( ( int )item ).ToString( ) } );
					}
					else
					{
						list.Add( new SelectListItem( ) { Text = item.ToString( ), Value = ( ( int )item ).ToString( ) } );
					}
				}
			}

			return new SelectList( list, "Value", "Text", defaultPublication );
		}

		public static SelectList SupprterTypeSelectList( int? defaultType )
		{
			var list = new List<SelectListItem>( );
			foreach ( Enums.SupporterTypes item in Enum.GetValues( typeof( Enums.SupporterTypes ) ) )
			{
				if( item != Enums.SupporterTypes.None)
				{
					if( item == Enums.SupporterTypes.OldBoyParent )
					{
						list.Add( new SelectListItem( ) { Text = "Old Boy Parent", Value = ( ( int )item ).ToString( ) } );
					}
					else if ( item == Enums.SupporterTypes.FeederSchool )
					{
						list.Add( new SelectListItem( ) { Text = "Feeder School", Value = ( ( int )item ).ToString( ) } );
					}
					else
					{
						list.Add( new SelectListItem( ) { Text = item.ToString( ), Value = ( ( int )item ).ToString( ) } );
					}
				}
			}
			return new SelectList( list, "Value", "Text", defaultType );
		}
		
    }
}