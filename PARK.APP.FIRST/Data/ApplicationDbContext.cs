using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PARK.APP.FIRST.Models.ApplicationModel;
using PARK.APP.FIRST.Areas.UserManage.Models;
using PARK.APP.FIRST.Models;
using PARK.APP.FIRST.Areas.SystemManage.Models.Menu;

namespace PARK.APP.FIRST.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, String>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<TMenu>(entity =>
            {
                entity.HasKey(e => e.MenuSeq);

                entity.ToTable("T_MENU");

                entity.Property(e => e.MenuSeq)
                    .HasColumnName("MENU_SEQ")
                    .ValueGeneratedNever();

                entity.Property(e => e.ImagePath)
                    .HasColumnName("IMAGE_PATH")
                    .HasMaxLength(256);

                entity.Property(e => e.MenuDescription)
                    .HasColumnName("MENU_DESCRIPTION")
                    .HasMaxLength(512);

                entity.Property(e => e.MenuName)
                    .HasColumnName("MENU_NAME")
                    .HasMaxLength(128);

                entity.Property(e => e.PgmId)
                    .HasColumnName("PGM_ID")
                    .HasMaxLength(64);

                entity.Property(e => e.SortOrder).HasColumnName("SORT_ORDER");

                entity.Property(e => e.UpperMenuSeq).HasColumnName("UPPER_MENU_SEQ");

                entity.Property(e => e.UseYn)
                    .HasColumnName("USE_YN")
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });
        }

        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public virtual DbSet<ApplicationRole> ApplicationRole { get; set; }
        public virtual DbSet<ManageRoleViewModel> ManageRoleViewModel { get; set; }
        public virtual DbSet<ManageUserListViewModel> ManageUserListViewModel { get; set; }
        public virtual DbSet<ApplicationUserAllViewModel> ApplicationUserAllViewModel { get; set; }
        public virtual DbSet<TMenu> TMenu { get; set; }
        public virtual DbSet<MenuMaster> MenuMaster { get; set; }
    }
}
