using StakeHolder.Datacontract.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [Required(ErrorMessage ="Please Enter First Name !") ]
        [MaxLength(20)]
        public string FirstName { get; set; }



        [DataMember]
        [Required(ErrorMessage = "Please Enter Last Name !")]
        [MaxLength(20)]
        public string LastName { get; set; }



        [DataMember]
        [Required(ErrorMessage = "Please Select Gender !")]
        public string Gender { get; set; }




        [DataMember]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email address")]
        [MaxLength(50)]
        [Required(ErrorMessage = "Please Enter Email !")]
        //[RegularExpression(@"[a-z0-9._%+-]+@[a-z0-9.-]+\\.[a-z]{2,4}", ErrorMessage = "Please enter correct email")]
        public string Email { get; set; }


        [DataMember]
        [Display(Name = "Contact Number")]
        [Required(ErrorMessage = "Please Enter Phone Number !")]
        //[DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }


        [DataMember]
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Please Enter Password !")]
        //[DataType(DataType.Password)]
        public string PasswordHash { get; set; }



        [DataMember]
        [MaxLength(15)]
        [Display(Name = "City")]
        [Required(ErrorMessage = "Please Enter City !")]
        public string City { get; set; }


        [DataMember]
        [MaxLength(15)]
        [Required(ErrorMessage = "Please Enter State !")]
        public string State { get; set; }



        [DataMember]
        [MaxLength(15)]
        [Required(ErrorMessage = "Please Enter Country !")]
        public string Country { get; set; }


        [DataMember]
        [MaxLength(15)]
        [Display(Name = "Postal Code")]
        [Required(ErrorMessage = "Please Enter Postal Code !")]
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
        [Required]
        public string AdminComment { get; set; }
        [DataMember]
        [Required]
        public bool IsEmailVerified { get; set; }

     
        public bool IsAdmin { get; set; }
        [DataMember]
        [Required]
        public int AccessFailedCount { get; set; }



        [DataMember]
        [Display(Name = "Select Member")]
        //[Required(ErrorMessage = "Please  Select Role !")]
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
