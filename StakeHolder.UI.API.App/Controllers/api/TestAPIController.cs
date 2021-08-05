using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace StakeHolder.UI.API.App.Controllers.api
{
    public class TestAPIController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage GetTest()
        {
            HttpResponseMessage response = null;
            response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<string>("API is working", new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
            };
            return response;
        }

        [HttpPost]
        public HttpResponseMessage PostTest(string megString)
        {
            HttpResponseMessage response = null;
            response = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent<string>("Output" + megString, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
            };
            return response;
        }
    }
}