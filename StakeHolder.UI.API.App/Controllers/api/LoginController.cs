using StakeHolder.BAL.Manager;
using StakeHolder.Datacontract.Base;
using StakeHolder.Datacontract.Users;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;


namespace StakeHolder.UI.API.App.Controllers.api
{
    public class LoginController : BaseAPIController
    {
        #region Variable declaration

        UserManager _userManager;
        #endregion
        public HttpResponseMessage GetLogin(string login, string pwd)

        {
            _userManager = new UserManager();
            HttpResponseMessage response = null;
            LoginDataContract loginDC = null;
            try
            {
                loginDC = _userManager.GetUserByLogin(login, pwd);
                if (loginDC != null)
                {
                    StatusDataContract structObj = new StatusDataContract(true, "Success", loginDC);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "Invalid access");
                    response = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                    };
                }
            }
            catch (Exception ex)
            {
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }

            return response;
        }

        [HttpPost]
        public HttpResponseMessage ForGotPassword(string emailId)
        {
            _userManager = new UserManager();
            HttpResponseMessage response = null;
            try
            {
                int flag = _userManager.ForGotPassword(emailId);
                if (flag > 0)
                {
                    StatusDataContract statusDataContract = new StatusDataContract(true, "Success", flag.ToString());
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(statusDataContract, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else if (flag == -1)
                {
                    StatusDataContract statusDataContract = new StatusDataContract(false, "Invalid Email Id.");
                    response = new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new ObjectContent<StatusDataContract>(statusDataContract, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else if (flag == -2)
                {
                    StatusDataContract statusDataContract = new StatusDataContract(false, "Feature is not available for social user.");
                    response = new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                    {
                        Content = new ObjectContent<StatusDataContract>(statusDataContract, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract statusDataContract = new StatusDataContract(false, "Invalid request");
                    response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                    {
                        Content = new ObjectContent<StatusDataContract>(statusDataContract, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }

            }
            catch (Exception ex)
            {
                StatusDataContract statusDataContract = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(statusDataContract, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                };
            }
            return response;
        }

        [HttpGet]
        public HttpResponseMessage GetUserByEmail(string username)
        {
            _userManager = new UserManager();
            HttpResponseMessage response = null;
            try
            {
                LoginDataContract loginDC = null;
                if (!string.IsNullOrEmpty(username))
                {
                    loginDC = _userManager.GetUsersByEmailId(username);
                }

                if (loginDC != null)
                {
                    StatusDataContract structObj = new StatusDataContract(true, "Success", loginDC);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract structObj = new StatusDataContract(false, "The user is not registered.");
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };

                }
            }
            catch (Exception ex)
            {
                StatusDataContract structObj = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(structObj, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json")) //new StringContent("error"),                    
                };
            }


            return response;
        }

        [HttpPost]
        public HttpResponseMessage RegisterUser(UserDataContract userDataContract)
        {
            HttpResponseMessage response = null;
            try
            {
                _userManager = new UserManager();
                LoginDataContract loginDC = _userManager.RegisterUser(userDataContract, 1, "");
                if (loginDC != null)
                {
                    StatusDataContract statusDataContract = new StatusDataContract(true, "Success", loginDC);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(statusDataContract, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else
                {
                    StatusDataContract statusDataContract = new StatusDataContract(false, "Invalid Parameter");
                    response = new HttpResponseMessage(HttpStatusCode.NotModified)
                    {
                        Content = new ObjectContent<StatusDataContract>(statusDataContract, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
            }
            catch (Exception ex)
            {

                StatusDataContract statusDataContract = new StatusDataContract(false, ex.Message);
                response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new ObjectContent<StatusDataContract>(statusDataContract, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                };
            }
            return response;
        }

    }
}