using StakeHolder.Common.Enums;
using StakeHolder.Common.Helper;
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
    public class BaseController : Controller
    {
        public SessionHelper LoggedInUser
        {
            get
            {
                var session = Session["User"] as SessionHelper;

                if (session == null)
                    throw new Exception("Session variable is empty");

                return session;
            }
        }

        public SessionHelper _sessionHelper;

        private void InitRepository()
        {
            _sessionHelper = (SessionHelper)Session["User"];

        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            try
            {


                InitRepository();
                base.OnActionExecuting(filterContext);
               
                bool checkSession = Convert.ToBoolean(ConfigurationManager.AppSettings["ValidateSession"].ToString());
                if (!checkSession)
                    return;
                if (_sessionHelper != null)
                {

                    if (_sessionHelper.LoginDataContract.IsAdmin == true) // && filterContext.RequestContext. (checked for user have access to admin or not ?)
                    {

                    }
                    else
                    {
                        //Call session abendont      
                        //Session.Abandon();
                        //RedirectToAction("Index", "Home", new { Area = "" });
                    }
                }
                else
                {
                    Session.Abandon();
                    SessionTimeout(filterContext);

                    RedirectToAction("Index", "Home", new { Area = "" });
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("BaseController", "OnActionExecuting", ex);
            }
        }

        public void ShowMessage(string message, MessageTypeEnum messageType)
        {
            ViewBag.MessageType = messageType;
            ModelState.AddModelError(string.Empty, message);
        }

        private void SessionTimeout(ActionExecutingContext filterContext)
        {
            if (filterContext.ActionDescriptor.ActionName.ToLower() != "logout")
            {
                if (filterContext.HttpContext.Request.IsAjaxRequest())
                {
                    filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
                    filterContext.HttpContext.Response.StatusCode = 401;
                    filterContext.HttpContext.Response.End();
                    filterContext.Result = new HttpUnauthorizedResult();
                }
                else
                {
                    filterContext.Result = new HttpUnauthorizedResult();
                    Response.Headers.Add("Access-Control-Allow-Origin", "*");
                    FormsAuthentication.SignOut();
                    FormsAuthentication.RedirectToLoginPage();
                }
            }

        }

        public virtual ActionResult AccessDenied()
        {
            return PartialView("_AccessDeniedPartial");
        }

        
    }
}