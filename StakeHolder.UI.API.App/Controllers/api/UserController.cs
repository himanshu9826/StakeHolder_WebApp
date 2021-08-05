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
    public class UserController : BaseAPIController
    {
        #region Variable declaration
        UserManager _userManager;

        #endregion
        [HttpPost]
        public HttpResponseMessage ManageUser(UserDataContract userDataContract)
        {
            HttpResponseMessage response = null;
            _userManager = new UserManager();
            try
            {

                #region Update the image file
                if (!string.IsNullOrEmpty(userDataContract.ImageData))
                {

                    var imageDataByteArray = Convert.FromBase64String(userDataContract.ImageData);
                    using (Image image = Image.FromStream(new MemoryStream(imageDataByteArray)))
                    {
                        string fileName = userDataContract.FirstName.Trim() + DateTime.Now.ToString("yyyyMMddHHmmssFFF") + ".jpg";
                        string filePath = ConfigurationManager.AppSettings["UserImage"].ToString() + fileName;
                        userDataContract.ProfileImageURL = fileName;
                        if (!File.Exists(filePath))
                            image.Save(filePath, ImageFormat.Jpeg);  // Or Png
                    }
                }
                #endregion

                //Read welcome email content
                string body = string.Empty;
                if (userDataContract.UserId == 0)
                {
                    StreamReader reader = new StreamReader(HttpContext.Current.Server.MapPath("~/Content/EmailTemplate/RegisterUser.html"));
                    body = reader.ReadToEnd();
                }

                #region Set result 

                int result = _userManager.ManageUser(userDataContract, 1, body);
                if (result > 0)
                {
                    StatusDataContract statusDataContract = new StatusDataContract(true, "Success", result.ToString());
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(statusDataContract, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }
                else if (result < 0)
                {

                    if (result == -1)
                    {
                        StatusDataContract statusDataContract = new StatusDataContract(false, "Email already register.");
                        response = new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                        {
                            Content = new ObjectContent<StatusDataContract>(statusDataContract, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };
                    }
                    else if (result == -2)
                    {
                        StatusDataContract statusDataContract = new StatusDataContract(false, "Phone number already register.");
                        response = new HttpResponseMessage(HttpStatusCode.NotAcceptable)
                        {
                            Content = new ObjectContent<StatusDataContract>(statusDataContract, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                        };
                    }
                }
                else
                {
                    StatusDataContract statusDataContract = new StatusDataContract(false, "Invalid parameter");
                    response = new HttpResponseMessage(HttpStatusCode.NotModified)
                    {
                        Content = new ObjectContent<StatusDataContract>(statusDataContract, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                }

                #endregion

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
        public HttpResponseMessage GetUserById(int id)
        {
            _userManager = new UserManager();
            HttpResponseMessage response = null;
            try
            {
                UserDataContract userDC = _userManager.GetUserById(id);

                if (userDC != null)

                {
                    StatusDataContract statusDataContract = new StatusDataContract(true, "Success.", userDC);
                    response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new ObjectContent<StatusDataContract>(statusDataContract, new JsonMediaTypeFormatter(), new MediaTypeWithQualityHeaderValue("application/json"))
                    };
                };
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