using PARK.APP.FIRST.Models.ApplicationModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PARK.APP.FIRST.Areas.UserManage.Models
{
    public class ManageUserViewModel
    {
        public ApplicationUser[] Administrators { get; set; }
        public ApplicationUser[] Everyone { get; set; }
    }
}
