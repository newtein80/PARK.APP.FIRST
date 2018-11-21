using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PARK.APP.FIRST.Areas.VulnManage.Models.Vuln
{
    public partial class VulnDbContext : DbContext
    {
        public VulnDbContext()
        {
        }

        public VulnDbContext(DbContextOptions<VulnDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TcompInfo> TcompInfo { get; set; }
        public virtual DbSet<Tvuln> Tvuln { get; set; }
        public virtual DbSet<TvulnGroup> TvulnGroup { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TcompInfo>(entity =>
            {
                entity.HasKey(e => e.CompSeq);

                entity.ToTable("TCompInfo");

                entity.Property(e => e.CompSeq).HasColumnName("COMP_SEQ");

                entity.Property(e => e.CompDescription)
                    .HasColumnName("COMP_DESCRIPTION")
                    .HasMaxLength(1024);

                entity.Property(e => e.CompDetailGubun)
                    .HasColumnName("COMP_DETAIL_GUBUN")
                    .HasMaxLength(32)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.CompName)
                    .IsRequired()
                    .HasColumnName("COMP_NAME")
                    .HasMaxLength(128);

                entity.Property(e => e.CompRef)
                    .HasColumnName("COMP_REF")
                    .HasMaxLength(256);

                entity.Property(e => e.ConfirmYn)
                    .HasColumnName("CONFIRM_YN")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.CreateDt)
                    .HasColumnName("CREATE_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreateUserId)
                    .HasColumnName("CREATE_USER_ID")
                    .HasMaxLength(16);

                entity.Property(e => e.DiagType)
                    .IsRequired()
                    .HasColumnName("DIAG_TYPE")
                    .HasMaxLength(32);

                entity.Property(e => e.StandardYear)
                    .HasColumnName("STANDARD_YEAR")
                    .HasMaxLength(4);

                entity.Property(e => e.UpdateDt)
                    .HasColumnName("UPDATE_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdateUserId)
                    .HasColumnName("UPDATE_USER_ID")
                    .HasMaxLength(16);

                entity.Property(e => e.UseYn)
                    .HasColumnName("USE_YN")
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Tvuln>(entity =>
            {
                entity.HasKey(e => e.VulnSeq);

                entity.ToTable("TVuln");

                entity.Property(e => e.VulnSeq)
                    .HasColumnName("VULN_SEQ")
                    .ValueGeneratedNever();

                entity.Property(e => e.ApplyTarget)
                    .HasColumnName("APPLY_TARGET")
                    .HasMaxLength(1024);

                entity.Property(e => e.ApplyTime)
                    .HasColumnName("APPLY_TIME")
                    .HasMaxLength(2);

                entity.Property(e => e.AutoYn)
                    .HasColumnName("AUTO_YN")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ClientStandardId)
                    .HasColumnName("CLIENT_STANDARD_ID")
                    .HasMaxLength(32);

                entity.Property(e => e.CreateDt)
                    .HasColumnName("CREATE_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreateUserId)
                    .HasColumnName("CREATE_USER_ID")
                    .HasMaxLength(16);

                entity.Property(e => e.Detail)
                    .HasColumnName("DETAIL")
                    .HasColumnType("text");

                entity.Property(e => e.DetailPath)
                    .HasColumnName("DETAIL_PATH")
                    .HasMaxLength(256);

                entity.Property(e => e.Effect)
                    .HasColumnName("EFFECT")
                    .HasMaxLength(2048);

                entity.Property(e => e.ExceptCd)
                    .HasColumnName("EXCEPT_CD")
                    .HasMaxLength(2);

                entity.Property(e => e.ExceptDt)
                    .HasColumnName("EXCEPT_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.ExceptReason)
                    .HasColumnName("EXCEPT_REASON")
                    .HasMaxLength(1024);

                entity.Property(e => e.ExceptTermFr)
                    .HasColumnName("EXCEPT_TERM_FR")
                    .HasMaxLength(8);

                entity.Property(e => e.ExceptTermTo)
                    .HasColumnName("EXCEPT_TERM_TO")
                    .HasMaxLength(8);

                entity.Property(e => e.ExceptTermType)
                    .HasColumnName("EXCEPT_TERM_TYPE")
                    .HasMaxLength(2);

                entity.Property(e => e.ExceptUserId)
                    .HasColumnName("EXCEPT_USER_ID")
                    .HasMaxLength(16);

                entity.Property(e => e.GroupSeq).HasColumnName("GROUP_SEQ");

                entity.Property(e => e.Judgement)
                    .HasColumnName("JUDGEMENT")
                    .HasMaxLength(2048);

                entity.Property(e => e.ManageId)
                    .HasColumnName("MANAGE_ID")
                    .HasMaxLength(32);

                entity.Property(e => e.ManagementVulnYn)
                    .HasColumnName("MANAGEMENT_VULN_YN")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ManualYn)
                    .HasColumnName("MANUAL_YN")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.OrgParserContents)
                    .HasColumnName("ORG_PARSER_CONTENTS")
                    .HasColumnType("text");

                entity.Property(e => e.Overview).HasColumnName("OVERVIEW");

                entity.Property(e => e.ParserContents)
                    .HasColumnName("PARSER_CONTENTS")
                    .HasColumnType("text");

                entity.Property(e => e.Rate)
                    .HasColumnName("RATE")
                    .HasMaxLength(32);

                entity.Property(e => e.Refrrence)
                    .HasColumnName("REFRRENCE")
                    .HasMaxLength(2048);

                entity.Property(e => e.Remedy)
                    .HasColumnName("REMEDY")
                    .HasColumnType("text");

                entity.Property(e => e.RemedyDetail)
                    .HasColumnName("REMEDY_DETAIL")
                    .HasColumnType("text");

                entity.Property(e => e.RemedyPath)
                    .HasColumnName("REMEDY_PATH")
                    .HasMaxLength(256);

                entity.Property(e => e.RuleYn)
                    .HasColumnName("RULE_YN")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Score)
                    .HasColumnName("SCORE")
                    .HasMaxLength(2);

                entity.Property(e => e.SortOrder).HasColumnName("SORT_ORDER");

                entity.Property(e => e.UpdateDt)
                    .HasColumnName("UPDATE_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdateUserId)
                    .HasColumnName("UPDATE_USER_ID")
                    .HasMaxLength(16);

                entity.Property(e => e.UseYn)
                    .HasColumnName("USE_YN")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Vulgroup)
                    .HasColumnName("VULGROUP")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.VulnName)
                    .HasColumnName("VULN_NAME")
                    .HasMaxLength(128);

                entity.Property(e => e.Vulno)
                    .HasColumnName("VULNO")
                    .HasMaxLength(32)
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.GroupSeqNavigation)
                    .WithMany(p => p.Tvuln)
                    .HasForeignKey(d => d.GroupSeq)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TVuln__GROUP_SEQ__536E27D9");
            });

            modelBuilder.Entity<TvulnGroup>(entity =>
            {
                entity.HasKey(e => e.GroupSeq);

                entity.ToTable("TVulnGroup");

                entity.Property(e => e.GroupSeq)
                    .HasColumnName("GROUP_SEQ")
                    .ValueGeneratedNever();

                entity.Property(e => e.CompSeq).HasColumnName("COMP_SEQ");

                entity.Property(e => e.CreateDt)
                    .HasColumnName("CREATE_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.CreateUserId)
                    .HasColumnName("CREATE_USER_ID")
                    .HasMaxLength(16);

                entity.Property(e => e.DiagKind)
                    .HasColumnName("DIAG_KIND")
                    .HasMaxLength(32);

                entity.Property(e => e.DiagTerm)
                    .HasColumnName("DIAG_TERM")
                    .HasMaxLength(32);

                entity.Property(e => e.DiagTool)
                    .HasColumnName("DIAG_TOOL")
                    .HasMaxLength(32);

                entity.Property(e => e.GroupId)
                    .HasColumnName("GROUP_ID")
                    .HasMaxLength(32);

                entity.Property(e => e.GroupName)
                    .HasColumnName("GROUP_NAME")
                    .HasMaxLength(128);

                entity.Property(e => e.GroupType)
                    .IsRequired()
                    .HasColumnName("GROUP_TYPE")
                    .HasMaxLength(32);

                entity.Property(e => e.Level).HasColumnName("LEVEL");

                entity.Property(e => e.SortOrder).HasColumnName("SORT_ORDER");

                entity.Property(e => e.UpdateDt)
                    .HasColumnName("UPDATE_DT")
                    .HasColumnType("datetime");

                entity.Property(e => e.UpdateUserId)
                    .HasColumnName("UPDATE_USER_ID")
                    .HasMaxLength(16);

                entity.Property(e => e.UpperSeq).HasColumnName("UPPER_SEQ");

                entity.HasOne(d => d.CompSeqNavigation)
                    .WithMany(p => p.TvulnGroup)
                    .HasForeignKey(d => d.CompSeq)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TVulnGrou__COMP___5091BB2E");
            });
        }
    }
}
