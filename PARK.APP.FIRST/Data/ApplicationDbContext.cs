using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PARK.APP.FIRST.Models.ApplicationModel;
using PARK.APP.FIRST.Areas.UserManage.Models;

namespace PARK.APP.FIRST.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, String>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<ApplicationRole> ApplicationRole { get; set; }
        public DbSet<PARK.APP.FIRST.Areas.UserManage.Models.ManageRoleViewModel> ManageRoleViewModel { get; set; }
    }
}
