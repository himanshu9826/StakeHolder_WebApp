using StakeHolder.BAL.Manager;
using StakeHolder.Common.Enums;
using StakeHolder.Common.Helper;
using StakeHolder.Datacontract.Users;
using StakeHolder.UI.API.App.Helper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace StakeHolder.UI.API.App.Controllers
{
    public class UserAccountController : Controller
    {
        #region Variable Declaration

        UserManager _userManager;

        #endregion

        #region Action Methods

        /// <summary>
        /// Method to load login page
        /// </summary>
        /// <returns>ActionResult</returns>
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult UserLogin()
        {
            if (TempData["Auth"] != null && Convert.ToBoolean(TempData["Auth"]) == false)
            {
                ShowMessage("Invalid Credential", MessageTypeEnum.error);
            }
            return View();
        }


        /// <summary>
        /// Post method to load loagin page
        /// </summary>
        /// <param name="formCollection">formCollection</param>
        /// <returns>ActionResult</returns>
        [HttpPost]
        public ActionResult UserLogin(FormCollection formCollection)
        {
            _userManager = new UserManager();
            try
            {
                if (formCollection != null && formCollection["username"] != null && formCollection["password"] != null)
                {
                    string username = formCollection["username"] != null ? formCollection["username"].ToString() : "";
                    string password = formCollection["password"] != null ? formCollection["password"].ToString() : "";
                    if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                    {
                        LoginDataContract loginDC = _userManager.GetUserByLogin(username, password);
                        if (loginDC != null && loginDC.UserDataContract != null && loginDC.UserDataContract.UserId != 0)
                        {
                            Session["User"] = FillSession(loginDC);
                            return RedirectToAction("Index", "Dashboard", new { Area = "Admin" });
                        }
                        else
                        {
                            TempData["Auth"] = false;
                            return RedirectToAction("UserLogin", "UserAccount", new { Area = "" });
                        }
                    }
                    else
                    {
                        TempData["Auth"] = false;
                        return RedirectToAction("UserLogin", "UserAccount", new { Area = "" });
                    }
                }
                else
                {
                    TempData["Auth"] = false;
                    return RedirectToAction("UserLogin", "UserAccount", new { Area = "" });
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("UserAccountController", "UserLogin", ex);

            }
            TempData["Auth"] = false;
            return RedirectToAction("UserLogin", "UserAccount", new { Area = "" });
        }

        /// <summary>
        /// method for logout user
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Logout()
        {
            var gg = FormsAuthentication.LoginUrl;

            if (User.Identity.IsAuthenticated)
                FormsAuthentication.SignOut();

            Response.Cookies.Remove(FormsAuthentication.FormsCookieName);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            HttpCookie cookie = HttpContext.Request.Cookies[FormsAuthentication.FormsCookieName];
            if (cookie != null)
            {
                cookie.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(cookie);
            }
            Session.Abandon();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetExpires(DateTime.Now.AddSeconds(-1));
            Response.Cache.SetNoStore();
            return RedirectToAction("UserLogin", "UserAccount", new { Area = "" });
        }



        public ActionResult ForGotPassword()
        {
            return View();
        }
        /// <summary>
        /// Post method to load loagin page
        /// </summary>
        /// <param name="formCollection">formCollection</param>
        /// <returns>ActionResult</returns>     
        /// 

        [HttpPost]
        public ActionResult ForGotPassword(string userName)
        {
            _userManager = new UserManager();
            //string adminMail = ConfigurationManager.AppSettings["AdminEmail"].ToString();
            int flag = _userManager.ForGotPassword(userName);
            if (flag > 0)
            {
                return RedirectToAction("UserLogin", "UserAccount", new { Area = "" });
            }
            return Json(flag, JsonRequestBehavior.AllowGet);
        }




        /// <summary>
        /// Method to fill session
        /// </summary>
        /// <param name="userVM">Agent view model</param>
        /// <returns>SessionHelper</returns>
        public ActionResult IsUserActive(string agentId)
        {
            _userManager = new UserManager();
            short flag = 1; // _userManager.IsAgentActive(agentId);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }

    

      

        #endregion

        #region Methods


        /// <summary>
        /// Method to fill session
        /// </summary>
        /// <param name="userDC">User view model</param>
        /// <returns>SessionHelper</returns>
        public SessionHelper FillSession(LoginDataContract loginDC)
        {
            SessionHelper sessionHelper = new SessionHelper();
            try
            {
                sessionHelper.LoginDataContract = loginDC;
                sessionHelper.LoginDataContract.IsAdmin = Convert.ToBoolean(loginDC.UserDataContract.IsAdmin);
                                
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("HomeController", "FillSession", ex);
            }
            return sessionHelper;
        }


        public void ShowMessage(string message, MessageTypeEnum messageType)
        {
            ViewBag.MessageType = messageType;
            ModelState.AddModelError(string.Empty, message);
        }
        #endregion
    }
}