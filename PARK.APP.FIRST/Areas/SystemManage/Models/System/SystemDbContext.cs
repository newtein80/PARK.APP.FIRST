using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PARK.APP.FIRST.Areas.SystemManage.Models.System
{
    public partial class SystemDbContext : DbContext
    {
        public SystemDbContext()
        {
        }

        public SystemDbContext(DbContextOptions<SystemDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TcommonCode> TcommonCode { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TcommonCode>(entity =>
            {
                entity.HasKey(e => new { e.CodeType, e.CodeId });

                entity.ToTable("TCommonCode");

                entity.Property(e => e.CodeType).HasMaxLength(32);

                entity.Property(e => e.CodeId).HasMaxLength(32);

                entity.Property(e => e.CodeComment).HasMaxLength(128);

                entity.Property(e => e.CodeName)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.CodeTypeName)
                    .IsRequired()
                    .HasMaxLength(128);

                entity.Property(e => e.CreateDt).HasColumnType("datetime");

                entity.Property(e => e.CreateUserId).HasMaxLength(16);

                entity.Property(e => e.UseYn)
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });
        }
    }
}
