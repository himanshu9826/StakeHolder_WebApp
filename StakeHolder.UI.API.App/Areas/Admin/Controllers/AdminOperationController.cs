using StakeHolder.BAL.Manager;
using StakeHolder.Common.Enums;
using StakeHolder.Common.Helper;
using StakeHolder.Datacontract.Users;
using StakeHolder.UI.API.App.Controllers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace StakeHolder.UI.API.App.Areas.Admin.Controllers
{
    public class AdminOperationController : BaseController
    {
        UserManager _userManager;

        public ActionResult Index()
        {

            //if (TempData["Success"] != null)
            //{
            //    bool flag = (Boolean)TempData["Success"];
            //    if (flag)
            //        ShowMessage("Saved successfully.", MessageTypeEnum.success);
            //    else
            //        ShowMessage("Error occured: Please contact admin.", MessageTypeEnum.error);
            //}

            return View();
        }

        public JsonResult GetBoardUserList(string txt, int page, int? type)
        {
            _userManager = new UserManager();
            int startRow = (page - 1) * Configurations.GridPageSize;
            int end = page * Configurations.GridPageSize;
            UserDataContractList userDataContractList = new UserDataContractList();
            userDataContractList = _userManager.GetUserList(startRow, page, type, txt);
            int totalRecords = userDataContractList.UserCount;
            var totalPages = (int)Math.Ceiling(totalRecords / (float)Configurations.GridPageSize);
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = userDataContractList.UserDataListContract
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
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


        //public ActionResult DataTable()
        //{
        //    try
        //    {
        //        var userList = _userManager.GetUserDataList();
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //    return View();
        //}
    }
}