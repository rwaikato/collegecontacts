using FarneFunds.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FarneFunds.Classes
{
    public class Extensions
    {

        public static string GetContactAddress(Contact contact)
        {
            string contactAddress = "";

            if( !String.IsNullOrWhiteSpace( contact.Street ) )
            {
                contactAddress += contact.Street + Environment.NewLine;
            }

            if (!String.IsNullOrWhiteSpace(contact.Town))
            {
                contactAddress += contact.Town + Environment.NewLine;
            }

            if (!String.IsNullOrWhiteSpace(contact.City))
            {
                contactAddress += contact.City + Environment.NewLine;
            }

            if (!String.IsNullOrWhiteSpace(contact.Province))
            {
                contactAddress += contact.Province + Environment.NewLine;
            }

            if (!String.IsNullOrWhiteSpace(contact.Country))
            {
                contactAddress += contact.Country + Environment.NewLine;
            }

            if (!String.IsNullOrWhiteSpace(contact.ZipCode))
            {
                contactAddress += contact.ZipCode + Environment.NewLine;
            }

            return contactAddress;

        }
    }


}