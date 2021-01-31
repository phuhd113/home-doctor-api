using HomeDoctor.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeDoctor.Data.DBContext
{
    public class HomeDoctorContext : DbContext
    {
        public HomeDoctorContext(DbContextOptions<HomeDoctorContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer("Data Source=45.76.186.233,1433;Initial Catalog=HomeDoctor;Persist Security Info=True;User ID=sa;Password=Admin@123;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
            //relationship User and Role
            modelBuilder.Entity<Account>(entity =>
            entity.HasOne(a => a.Role)
            .WithMany(a => a.Users)
            .HasForeignKey(a => a.RoleId)
            );

            //relationship doctor, patient and Contract
            modelBuilder.Entity<Contract>(entity =>
            {
                entity.HasOne(a => a.Doctor).WithMany(a => a.Contracts).HasForeignKey(a => a.DoctorId);
                entity.HasOne(a => a.Patient).WithOne(a => a.Contract).HasForeignKey<Contract>(a => a.PatientId);
            }
            );
            //relationshop Contract and ContractRelation
            modelBuilder.Entity<ContractRelative>(entity =>
            {
                entity.HasOne(a => a.Contract).WithMany(a => a.ContractRelatives).HasForeignKey(a => a.ContractId);
                entity.HasOne(a => a.Relative).WithMany(a => a.ContractRelatives).HasForeignKey(a => a.RelativeId);
            }
            );
            //relationshop User and Patient
            modelBuilder.Entity<Patient>(entity =>
            entity.HasOne(a => a.User).WithOne(a => a.Patient).HasForeignKey<Patient>(a => a.UserId));

            //relationshop User and Doctor
            modelBuilder.Entity<Doctor>(entity =>
            entity.HasOne(a => a.User).WithOne(a => a.Doctor).HasForeignKey<Doctor>(a => a.UserId));
            */
        }

        // entities
        public DbSet<Account> Account { get; set; }
        public DbSet<Contract> Contract { get; set; }
        public DbSet<Doctor> Doctor { get; set; }
        public DbSet<Patient> Patient { get; set; }
        public DbSet<Relative> Relative { get; set; }
        public DbSet<License> License { get; set; }
        public DbSet<Disease> Disease { get; set; }




    }
}
