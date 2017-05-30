using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FarneFunds.Database;
using System.Web.Security;

namespace FarneFunds.Controllers
{
    public class BaseController : Controller
    {
        private FarneFundsEntities _dal = null;

        public FarneFundsEntities Dal
        {
            get
            {
                if (_dal == null)
                    _dal = new FarneFundsEntities();

                return _dal;
            }
        }
	}
}