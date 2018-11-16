using Microsoft.AspNetCore.Mvc.Rendering;
using PARK.APP.FIRST.Models.ApplicationModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace PARK.APP.FIRST.Areas.UserManage.Models
{
    /// <summary>
    /// User and Role
    /// </summary>
    public partial class ManageUserViewModel
    {
        public ApplicationUser[] Administrators { get; set; }
        public ApplicationUser[] Everyone { get; set; }
    }

    /// <summary>
    /// User List ViewModel
    /// </summary>
    public partial class ManageUserListViewModel
    {
        public  string PhoneNumber { get; set; }
        public  string ConcurrencyStamp { get; set; }
        public  string SecurityStamp { get; set; }
        public  bool EmailConfirmed { get; set; }
        public  string NormalizedEmail { get; set; }
        public  string Email { get; set; }
        public  string NormalizedUserName { get; set; }
        public  string UserName { get; set; }
        public  string Id { get; set; }
        public  bool LockoutEnabled { get; set; }
    }

    /// <summary>
    /// User Info - ApplicationUser : IdentityUser
    /// </summary>
    public partial class ApplicationUserAllViewModel
    {
        public  DateTimeOffset? LockoutEnd { get; set; }
        public  bool TwoFactorEnabled { get; set; }
        public  bool PhoneNumberConfirmed { get; set; }
        public  string PhoneNumber { get; set; }
        public  string ConcurrencyStamp { get; set; }
        public  string SecurityStamp { get; set; }
        public  string PasswordHash { get; set; }
        public  bool EmailConfirmed { get; set; }
        public  string NormalizedEmail { get; set; }
        [Required]
        [DataType(DataType.EmailAddress)]
        public  string Email { get; set; }
        public  string NormalizedUserName { get; set; }
        public  string UserName { get; set; }
        public  string Id { get; set; }
        public  bool LockoutEnabled { get; set; }
        public  int AccessFailedCount { get; set; }
    }
}
