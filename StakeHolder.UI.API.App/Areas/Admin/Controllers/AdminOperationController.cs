using StakeHolder.BAL.Manager;
using StakeHolder.Common.Helper;
using StakeHolder.Datacontract.Users;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StakeHolder.UI.API.App.Areas.Admin.Controllers
{
    public class AdminOperationController : Controller
    {
        UserManager _userManager;



        //public ActionResult ListOfBoardMembers()
        //{
        //    _userManager = new UserManager();
        //    try
        //    {
        //        var list = _userManager.GetUsersList();
        //    }
        //    catch (Exception ex)
        //    {
        //        LibLogging.WriteErrorToDB("AdminOperation", "ListOfBoardMembers", ex);
        //    }
        //    return View();
        //}






        public ActionResult AddMember()
        {
            return View();
        }

        // GET: Admin/Admin
        public ActionResult AddBoardMember()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddBoardMember(UserDataContract userDataContract)
        {
            try
            {
                _userManager = new UserManager();
                var logedinUserId = Convert.ToInt32(ConfigurationManager.AppSettings["RegisterLoginUserId"]);
                LoginDataContract loginDC = _userManager.RegisterUser(userDataContract, logedinUserId, "");
                if (loginDC != null)
                {
                    return RedirectToAction("ListOfBoardMembers", "AdminOperation", new { Area = "Admin" });
                }
                else
                {
                    return View();
                }
               
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("UserAccountController", "UserLogin", ex);
            }
            return View();
        }
    }
}