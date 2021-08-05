using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace StakeHolder.Datacontract.Users
{
    public class LoginDataContract
    {
        [DataMember]
        public UserDataContract UserDataContract { get; set; }
        [DataMember]
        public string TockenString { get; set; }
        [DataMember]
        public bool IsAdmin { get; set; }
      
    }
}
