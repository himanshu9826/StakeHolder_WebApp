using StakeHolder.Datacontract;
using StakeHolder.Datacontract.Users;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Web.Caching;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.UI;

namespace StakeHolder.UI.API.App.Helper
{
    /// <summary>
    /// Session helper class for maitaining session over the application
    /// </summary>
    public class SessionHelper
    {
        public LoginDataContract LoginDataContract { get; set; }
       
        public int OLEDBDrivers;
        /// <summary>
        /// intializes session helper
        /// </summary>
        public SessionHelper()
        {
            LoginDataContract = new LoginDataContract();
        }
    }
}


