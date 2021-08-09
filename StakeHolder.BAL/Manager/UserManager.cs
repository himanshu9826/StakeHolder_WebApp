using StakeHolder.BAL.Common;
using StakeHolder.Common.Helper;
using StakeHolder.Common.Helpers;
using StakeHolder.DAL;
using StakeHolder.DAL.Repository;
using StakeHolder.Datacontract.Users;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StakeHolder.BAL.Manager
{
    public class UserManager
    {
        #region Variable Declaration

        UserRepository _userRepository;
        UserTokenRepository _userTokenRepository;
        MailManager _mailManager;
        DateTime currentDatetime = DateTime.Now.TrimMilliseconds();
        #endregion

        /// <summary>
        /// Method to manage user by id
        /// </summary>
        /// <param name="userDataContract">userDataContract</param>
        /// <param name="logedinUserId">logedinUserId</param>
        /// <returns>int</returns>
        public int ManageUser(UserDataContract userDataContract, int logedinUserId, string body = "")
        {
            _userRepository = new UserRepository();
            _mailManager = new MailManager();
            tblUser tblUserObj = new tblUser();
            try
            {

                tblUserObj = userDataContract != null ? GenericMapper<UserDataContract, tblUser>.MapObject(userDataContract) : null;

                if (tblUserObj != null)
                {
                    string sitePath = ConfigurationManager.AppSettings["BaseURL"].ToString() + ConfigurationManager.AppSettings["User"].ToString();
                    if (!string.IsNullOrEmpty(tblUserObj.ProfileImageURL) && tblUserObj.ProfileImageURL.Contains("DB_Images"))
                        tblUserObj.ProfileImageURL = tblUserObj.ProfileImageURL.Replace(sitePath, "");
                    if (tblUserObj.UserId == 0)
                    {
                        if (!string.IsNullOrEmpty(userDataContract.Email) && _userRepository.GetSingle(x => x.Email == userDataContract.Email && x.IsActive == true) != null)
                            return -1;
                        if (!string.IsNullOrEmpty(userDataContract.Phone) && _userRepository.GetSingle(x => x.IsActive == true && x.Phone == userDataContract.Phone) != null)
                            return -2;

                        tblUserObj.IsActive = true;
                        tblUserObj.CreatedDate = currentDatetime;
                        tblUserObj.ModifiedDate = currentDatetime;
                        if (logedinUserId > 0)
                        {
                            tblUserObj.CreatedBy = logedinUserId;
                            tblUserObj.ModifiedBy = logedinUserId;
                        }
                        _userRepository.Add(tblUserObj);
                    }
                    else
                    {
                        tblUserObj.ModifiedDate = currentDatetime;
                        if (logedinUserId > 0)
                            tblUserObj.ModifiedBy = logedinUserId;
                        _userRepository.Update(tblUserObj);
                    }
                    _userRepository.SaveChanges();
                    if (userDataContract.UserId == 0)
                    {
                        //Code to send mail
                        string projectName = ConfigurationManager.AppSettings["ProjectName"].ToString();
                        string myString = body;
                        myString = myString.Replace("[Name]", userDataContract.FirstName + " " + userDataContract.LastName);
                        myString = myString.Replace("[Password]", userDataContract.PasswordHash.Trim().ToString());

                        _mailManager.SendEmailLocally(userDataContract.Email, myString.ToString(), "Welcome to " + projectName);
                    }
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("UserManager", "ManageUser", ex);
            }

            return tblUserObj.UserId;
        }

        /// <summary>
        /// Method to get user by its email ids
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>LoginDataContract</returns>
        public LoginDataContract GetUsersByEmailId(string email)
        {
            _userRepository = new UserRepository();
            LoginDataContract loginDC = null;
            UserDataContract userDC = null;
            tblUser tblUserObj = null;
            try
            {

                tblUserObj = _userRepository.GetSingle(x => x.IsActive == true && (x.Email != null && x.Email.ToLower().Equals(email.ToLower())));

                userDC = tblUserObj != null ? GenericMapper<tblUser, UserDataContract>.MapObject(tblUserObj) : null;

                if (userDC != null)
                {

                    loginDC = new LoginDataContract();
                    loginDC.UserDataContract = userDC;
                    loginDC.TockenString = loginDC != null && loginDC.UserDataContract != null ? InsertUserToken(loginDC.UserDataContract.UserId) : string.Empty;
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("UserManager", "GetUsersByEmailId", ex);
            }
            return loginDC;
        }


        /// <summary>
        /// Method to change the password 
        /// </summary>
        /// <param name="userId">userId</param>
        /// <param name="password">password</param>
        /// <param name="currentpassword">currentpassword</param>
        /// <returns>int</returns>
        public int ChangePassword(int userId, string password, string currentpassword)
        {
            _userRepository = new UserRepository();
            tblUser userObj = null;
            int result = 0;
            try
            {

                userObj = _userRepository.GetSingle(x => (x.UserId != 0 && x.UserId.Equals(userId)));
                if (userObj != null)
                {
                    if (userObj.PasswordHash.Equals(currentpassword))
                    {
                        userObj.PasswordHash = password;
                        _userRepository.Update(userObj);
                        if (_userRepository.SaveChanges() == 1)
                        {
                            result = 1;//Password changed successfully
                        }
                    }
                    else
                    {
                        result = -1;//Current password is wrong
                    }
                }
                else
                {
                    result = -2;//User does not exists
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("UserManager", "ChangePassword", ex);
            }
            return result;
        }

        /// <summary>
        /// Method to get user by login detail
        /// </summary>
        /// <param name="username">user name</param>
        /// <param name="password">password</param>
        /// <returns>LoginDataContract</returns>
        public LoginDataContract GetUserByLogin(string username, string password)
        {
            _userRepository = new UserRepository();
            LoginDataContract loginDC = null;
            UserDataContract userDC = null;
            tblUser userObj = null;
            try
            {

                if (!string.IsNullOrEmpty(username))
                {
                    userObj = _userRepository.GetSingle(x => x.IsActive == true && (x.Email != null && x.Email.ToLower().Equals(username.ToLower())) && (x.PasswordHash != null && x.PasswordHash.Equals(password)));
                }
                if (userObj != null)
                {
                    userDC = GenericMapper<tblUser, UserDataContract>.MapObject(userObj);

                    if (userDC != null)
                    {
                        userObj.LastLogin = currentDatetime;
                        _userRepository.Update(userObj);
                        _userRepository.SaveChanges();
                        loginDC = new LoginDataContract();
                        loginDC.UserDataContract = userDC;
                        loginDC.TockenString = loginDC != null && loginDC.UserDataContract != null ? InsertUserToken(loginDC.UserDataContract.UserId) : string.Empty;
                    }

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("UserManager", "GetUserByLogin", ex);
            }
            return loginDC;
        }

        /// <summary>
        /// Method to get user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UserDataContract GetUserById(int id)
        {
            _userRepository = new UserRepository();
            UserDataContract userDC = null;
            tblUser tblUserObj = null;
            try
            {
                tblUserObj = _userRepository.GetSingle(x => x.UserId == id);
                if (tblUserObj != null)
                    userDC = GenericMapper<tblUser, UserDataContract>.MapObject(tblUserObj);
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("UserManager", "GetUserById", ex);
            }
            return userDC;
        }
      


        /// <summary>
        /// Method to manage user by id
        /// </summary>
        /// <param name="userDataContract">userDataContract</param>
        /// <param name="logedinUserId">logedinUserId</param>
        /// <returns>int</returns>
        public LoginDataContract RegisterUser(UserDataContract userDataContract, int logedinUserId, string body = "")
        {
            _userRepository = new UserRepository();
            _mailManager = new MailManager();
            tblUser tblUserObj = new tblUser();
            LoginDataContract loginDC = new LoginDataContract(); 
            try
            {
                tblUserObj = userDataContract != null ? GenericMapper<UserDataContract, tblUser>.MapObject(userDataContract) : null;

                if (tblUserObj != null)
                {
                    string sitePath = ConfigurationManager.AppSettings["BaseURL"].ToString() + ConfigurationManager.AppSettings["User"].ToString();
                    if (!string.IsNullOrEmpty(tblUserObj.ProfileImageURL) && tblUserObj.ProfileImageURL.Contains("DB_Images"))
                        tblUserObj.ProfileImageURL = tblUserObj.ProfileImageURL.Replace(sitePath, "");
                    if (tblUserObj.UserId == 0)
                    {
                        if (!string.IsNullOrEmpty(userDataContract.Email) && _userRepository.GetSingle(x => x.Email == userDataContract.Email && x.IsActive == true) != null)
                        {
                            return loginDC;
                        }
                       
                        tblUserObj.IsActive = true;
                        tblUserObj.CreatedDate = currentDatetime;
                        tblUserObj.ModifiedDate = currentDatetime;
                        if (logedinUserId > 0)
                        {
                            tblUserObj.CreatedBy = logedinUserId;
                            tblUserObj.ModifiedBy = logedinUserId;
                        }
                        _userRepository.Add(tblUserObj);
                    }
                    else
                    {
                        tblUserObj.ModifiedDate = currentDatetime;
                        if (logedinUserId > 0)
                            tblUserObj.ModifiedBy = logedinUserId;
                        _userRepository.Update(tblUserObj);
                    }
                    _userRepository.SaveChanges();
                    if (userDataContract.UserId == 0)
                    {
                        //Code to send mail
                        string projectName = ConfigurationManager.AppSettings["ProjectName"].ToString();
                        string myString = body;
                        myString = myString.Replace("[Name]", userDataContract.FirstName + " " + userDataContract.LastName);
                        myString = myString.Replace("[Password]", userDataContract.PasswordHash.Trim().ToString());

                        _mailManager.SendEmailLocally(userDataContract.Email, myString.ToString(), "Welcome to " + projectName);
                    }

                    #region Create Login Data Contract

                    if (tblUserObj.UserId > 0)
                    {
                        UserDataContract userDC = GetUserById(tblUserObj.UserId);

                        if (userDC != null)
                        {
                            loginDC.UserDataContract = userDC;
                            loginDC.TockenString = loginDC != null && loginDC.UserDataContract != null ? InsertUserToken(loginDC.UserDataContract.UserId) : string.Empty;
                        }
                       
                    }
                    #endregion
                }

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("UserManager", "RegisterUser", ex);
            }

            return loginDC;
        }

        /// <summary>
        /// Method to forgot password
        /// </summary>
        /// <param name="emailId"> email id</param>
        /// <returns>int</returns>
        public int ForGotPassword(string username)
        {
            _mailManager = new MailManager();
            _userRepository = new UserRepository();
            tblUser userObj = null;
            string response = string.Empty;
            int result = 0;
            try
            {
                string projectName = ConfigurationManager.AppSettings["ProjectName"].ToString();
                if (!string.IsNullOrEmpty(username))
                {
                    if (username.All(char.IsDigit))
                        userObj = _userRepository.GetSingle(x => x.IsActive == true && (x.Phone != null && x.Phone.ToLower().Equals(username.ToString())));
                    else
                        userObj = _userRepository.GetSingle(x => x.IsActive == true && (x.Email != null && x.Email.ToLower().Equals(username.ToLower())));
                }

                if (userObj != null)
                {
                    userObj.PasswordHash = !string.IsNullOrEmpty(userObj.PasswordHash) ? userObj.PasswordHash : userObj.PasswordHash;
                    if (string.IsNullOrEmpty(userObj.PasswordHash))
                    {
                        userObj.PasswordHash = ConfigurationManager.AppSettings["DefaultPassword"].ToString();
                        _userRepository.Update(userObj);
                        _userRepository.SaveChanges();
                    }
                    string messageString = "Hi " + userObj.FirstName + " , </ br> Your password is '" + userObj.PasswordHash + "'. You can change your password after login.";
                    if (!string.IsNullOrEmpty(userObj.Email))
                        _mailManager.SendEmailLocally(userObj.Email, messageString.ToString(), projectName + ": Membership password");


                    result = userObj.UserId;

                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("UserManager", "ForGotPassword", ex);
            }
            return result;
        }

        /// <summary>
        /// Method to insert tocken in table and generate
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>String</returns>
        public string InsertUserToken(int userId)
        {
            _userTokenRepository = new UserTokenRepository();
            tblUsersToken userTokenObj = new tblUsersToken();
            try
            {
                userTokenObj.UserId = userId;
                userTokenObj.Token = GetTocken();
                _userTokenRepository.Add(userTokenObj);
                _userTokenRepository.SaveChanges();

            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("UserManager", "InsertUserToken", ex);
            }
            return userTokenObj.Token;
        }

        /// <summary>
        /// Method to get the user list by filter criteria
        /// </summary>
        /// <param name="startIndex">startIndex</param>
        /// <param name="maxRow">maxRow</param>
        /// <param name="sidx">sidx</param>
        /// <param name="sord">sord</param>
        /// <param name="type">type</param>
        /// <param name="txt">txt</param>
        /// <returns>UserDataContractList</returns>
        public UserDataContractList GetUserList(int startIndex, int maxRow, int? type, string txt)
        {
            _userRepository = new UserRepository();
            UserDataContractList userListDC = new UserDataContractList();
            DataSet ds = new DataSet();
            try
            {
                ds = _userRepository.GetUsersByFilter(type, txt, startIndex, maxRow);
                if (ds != null && ds.Tables.Count > 0)
                {
                    foreach (DataTable item in ds.Tables)
                    {
                        string tableName = item.Rows != null && item.Rows.Count > 0 ? item.Rows[0][0].ToString() : string.Empty;
                        switch (tableName)
                        {
                            case "ListObj":
                                userListDC.UserDataListContract = GenericConversionHelper.DataTableToList<UserDataContract>(item);
                                break;
                            case "CountObj":
                                userListDC.UserCount = item.Rows[0][1] != null ? Convert.ToInt32(item.Rows[0][1]) : 0;
                                break;
                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LibLogging.WriteErrorToDB("UserManager", "GetUserList", ex);
            }
            return userListDC;
        }


        //public List<DataTable> GetUserDataList()
        //{
        //    var userList = new List<DataTable>();
        //    try
        //    {
        //        var data = _userRepository.GetBoardUserList();
        //        //return data.Con;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}








        /// <summary>
        /// Method to get tocken
        /// </summary>
        /// <returns>String</returns>
        private string GetTocken()
        {
            byte[] time = BitConverter.GetBytes(DateTime.UtcNow.ToBinary());
            byte[] key = Guid.NewGuid().ToByteArray();
            string token = Convert.ToBase64String(time.Concat(key).ToArray());
            //save this token to DB 
            return token;
        }

    }
}
