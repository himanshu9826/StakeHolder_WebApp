using StakeHolder.Datacontract.Base;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StakeHolder.Datacontract.Users
{
    public class UserDataContract : BaseDataContract
    {
        [DataMember]
        public int UserId { get; set; }
        [DataMember]
        public string FirstName { get; set; }
        [DataMember]
        public string LastName { get; set; }
        [DataMember]
        public string Gender { get; set; }
        [DataMember]
        public string Email { get; set; }
        [DataMember]
        public string Phone { get; set; }
        [DataMember]
        public string PasswordHash { get; set; }
        [DataMember]
        public string City { get; set; }
        [DataMember]
        public string State { get; set; }
        [DataMember]
        public string Country { get; set; }
        [DataMember]
        public string PostalCode { get; set; }

        private string profileImageURL = string.Empty;
        private string sitePath = ConfigurationManager.AppSettings["BaseURL"].ToString() + ConfigurationManager.AppSettings["User"].ToString();

        [DataMember]
        public string ProfileImageURL
    {
            get
            {
                if (!string.IsNullOrEmpty(profileImageURL))
                    return sitePath + profileImageURL;
                else
                    return string.Empty;
            }
            set
            {
                if (!string.IsNullOrEmpty(value) && value.Contains("DB_Images"))
                    profileImageURL = value.Replace(sitePath, "");
                else
                    profileImageURL = value;
            }
        }


        [DataMember]
        public Nullable<int> AccountStatusId { get; set; }
        [DataMember]
        public string AdminComment { get; set; }
        [DataMember]
        public bool IsEmailVerified { get; set; }
        [DataMember]
        public bool IsAdmin { get; set; }
        [DataMember]
        public int AccessFailedCount { get; set; }
        [DataMember]
        public Nullable<int> RoleId { get; set; }
        [DataMember]
        public Nullable<System.DateTime> LastLogin { get; set; }
        [DataMember]
        public string ImageData { get; set; }
    }

    public class UserDataContractList
    {
        [DataMember]
        public List<UserDataContract> UserDataListContract { get; set; }
        [DataMember]
        public Int32 UserCount { get; set; }
    }
}
