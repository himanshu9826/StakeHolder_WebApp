using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StakeHolder.Common.Enums
{
    public enum RoleEnum
    {
       
        [Description("SuperAdmin")]
        SuperAdmin = 1,
        [Description("Admin")]
        Admin = 2,
        [Description("Board Member")]
        BoardMember = 3,
        [Description("Stake Holder")]
        StakeHolder = 4,


    }

    public enum AdvanceRoleEnum
    {
        [Description("Super Admin")]
        SuperAdmin = 1,
        [Description("Admin")]
        Admin = 2,


    }
}
