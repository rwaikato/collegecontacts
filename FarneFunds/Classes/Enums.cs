using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace FarneFunds.Classes
{
    public static class Enums
    {
        public enum Title
        {
            Mr = 0,
            Miss = 1,
            Mrs = 2,
            Ms = 3,
            Master = 4,
            Rev = 5,
            Fr = 6,
            Dr = 7,
        }

        public enum Controller
        {
            Home = 0,
            Contact,
            Donation,
            Campaign,
            Settings,
            Tag
        }

        public enum CampaignStatus
        {
            Current = 0,
            Archived,
            Completed,
            Expired
        }

        public enum ContactTab
        {
            All = 0,
            OldBoy,
            Donor,
            Archived,
            SetProvince,
			Support
        }

		public enum House
		{
			Aidan = 0,
			Oswald,
			Durham,
			Cuthbert
		}

		public enum Publications
		{
			None = 0,
			[Description( "Highways" )]
			Highways,
			[Description( "Rector's News" )]
			RectorsNews,
			[Description( "Chronicle" )]
			Chronicle
		}

		public enum SupporterTypes
		{
			None = 0,
			[Description( "Grandparent" )]
			Grandparent,
			[Description( "Old Boy Parent" )]
			OldBoyParent,
			[Description( "Feeder School" )]
			FeederSchool,
			[Description( "Community" )]
			Community
		}
    }

    
}