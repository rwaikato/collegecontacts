using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FarneFunds.Database;
using System.Web.Security;

namespace FarneFunds.Controllers
{
    public class AurthorizedController : BaseController
    {
        private AspNetUser _currentUser = null;

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Request.IsAjaxRequest())
            {
                AspNetUser currentUser = CurrentUser;
            }
        }

        public AspNetUser CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    var userName = User.Identity.Name;
                    _currentUser = Dal.AspNetUsers.Where(u => u.UserName == userName).FirstOrDefault();
                }
                return _currentUser;
            }
        }
	}
}