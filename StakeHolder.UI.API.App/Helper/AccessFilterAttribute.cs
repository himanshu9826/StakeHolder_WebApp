
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;


namespace StakeHolder.UI.API.App.Helper
{
    public class AccessFilterAttribute : ActionFilterAttribute
    {
        #region Variable declaration

       // UserManager _usersManager;

        #endregion

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            int VarifyHeader = Convert.ToInt32(ConfigurationManager.AppSettings["VarifyHeader"]);
            if (VarifyHeader == 1)
            {
                var re = actionContext.Request;
                var headers = re != null ? re.Headers : null;
                var modelState = actionContext.ModelState;
                if (headers != null)
                {
                    const string token = "x-token";
                    const string userid = "x-userid";
                    string tokenHeader = headers.GetValues(token).FirstOrDefault().ToString();
                    int useridHeader = Convert.ToInt32(headers.GetValues(userid).FirstOrDefault());
                    //_usersManager = new UserManager();
                    //if (!_usersManager.VarifyTocken(useridHeader, tokenHeader))
                    //{
                    //    actionContext.Response = actionContext.Request
                    // .CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid User");
                    //}
                }
                else
                {
                    actionContext.Response = actionContext.Request
                    .CreateErrorResponse(HttpStatusCode.BadRequest, "Bad Request");
                }
            }
        }

    }
}