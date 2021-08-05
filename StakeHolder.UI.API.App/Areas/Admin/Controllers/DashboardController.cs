using StakeHolder.UI.API.App.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StakeHolder.UI.API.App.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            ViewBag.UserName = _sessionHelper.LoginDataContract.UserDataContract.FirstName;
            return View();
        }
    }
}