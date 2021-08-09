using StakeHolder.BAL.Manager;
using StakeHolder.Datacontract.DataContract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StakeHolder.UI.API.App.Areas.Admin.Controllers
{
    public class CompanyController : Controller
    {
        UserManager _userManager;
        // GET: Admin/Company
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult AddCompany()
        {
            return View();
        }


        //public ActionResult AddCompany(CompanyDataContract companyDataContract)
        //{
        //    try
        //    {
        //        _userManager = new UserManager();
        //        var logedinUserId = Convert.ToInt32(ConfigurationManager.AppSettings["RegisterLoginUserId"]);
        //        LoginDataContract loginDC = _userManager.RegisterUser(userDataContract, logedinUserId, "");
        //        if (loginDC != null)
        //        {
        //            return RedirectToAction("ListOfBoardMembers", "AdminOperation", new { Area = "Admin" });
        //        }
        //        else
        //        {
        //            return View();
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        LibLogging.WriteErrorToDB("UserAccountController", "UserLogin", ex);
        //    }
        //    return View();
        //}
    }
}