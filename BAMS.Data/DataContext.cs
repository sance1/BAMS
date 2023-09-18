using BAMS.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System;
using System.Collections.Generic;
using System.Text;

namespace BAMS.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<PageText> PageTexts { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<AdministrativeUnit> AdministrativeUnits { get; set; }
        public DbSet<District> District { get; set; }
        public DbSet<Contract> Contract { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<AccessPermission> AccessPermission { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<ActivationCodeRequest> ActivationCodeRequest { get; set; }
        public DbSet<ActivationCode> ActivationCode { get; set; }
        public DbSet<ActivationCodeUpload> ActivationCodeUpload { get; set; }
        public DbSet<UserAccount> UserAccount { get; set; }
        public DbSet<Text> Text { get; set; }
        public DbSet<Application> Application { get; set; }
        public DbSet<LogTracking> LogTracking { get; set; }

        public DbSet<Changelog> Changelog { get; set; }
        public DbSet<Message> Message { get; set; }
        public DbSet<MessageAttachment> MessageAttachment { get; set; }
        public DbSet<MessageRecipient> MessageRecipient { get; set; }
        public DbSet<Country> Country { get; set; }
        public DbSet<Province> Province { get; set; }
        public DbSet<AdministrativeLevel> AdministrativeLevel { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Role>()
                .HasQueryFilter(e => e.DeleteDate == null);
            modelBuilder.Entity<Account>()
                .HasQueryFilter(e => e.DeleteDate == null);
            modelBuilder.Entity<AccessPermission>()
                .HasQueryFilter(e => e.DeleteDate == null);
            modelBuilder.Entity<School>()
                .HasQueryFilter(e => e.DeleteDate == null);
            modelBuilder.Entity<RolePermission>()
                .HasQueryFilter(e => e.DeleteDate == null);
            modelBuilder.Entity<ActivationCode>()
                .HasQueryFilter(e => e.DeleteDate == null);
            modelBuilder.Entity<UserAccount>()
                .HasQueryFilter(e => e.DeleteDate == null);
            modelBuilder.Entity<Project>()
                .HasQueryFilter(e => e.DeleteDate == null);
            modelBuilder.Entity<Contract>()
                .HasQueryFilter(e => e.DeleteDate == null)
                .HasOne(a => a.ActivationCodeRequest)
                .WithOne(a => a.Contract)
                .HasForeignKey<ActivationCodeRequest>(a => a.ContractId);
            modelBuilder.Entity<District>()
                .HasQueryFilter(e => e.DeleteDate == null);
            modelBuilder.Entity<PageText>()
                .HasQueryFilter(e => e.DeleteDate == null);
        }
    }
}
