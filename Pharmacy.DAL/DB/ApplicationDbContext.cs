using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Pharmacy.DAL.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Pharmacy.DAL.DB
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Doctor> Doctors { get; set; }
        public DbSet<Patient> Patients { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<ManualReview> ManualReviews { get; set; }
        public DbSet<Pharmacist> Pharmacists { get; set; }
        public DbSet<PatientManagement> PatientManagements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ManualReview>().HasNoKey();
            modelBuilder.Entity<PatientManagement>().HasNoKey();


            // Prescription
            modelBuilder.Entity<Prescription>()
                .HasOne(p => p.Patient)
                .WithMany(p => p.Prescriptions)
                .HasForeignKey(p => p.PatientID);

            modelBuilder.Entity<Prescription>()
                .HasOne(p => p.Doctor)
                .WithMany(d => d.Prescriptions)
                .HasForeignKey(p => p.DoctorID);

            modelBuilder.Entity<Prescription>()
                .HasOne(p => p.Medicine)
                .WithMany()
                .HasForeignKey(p => p.MedicineID);

            // Order
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Patient)
                .WithMany(p => p.Orders)
                .HasForeignKey(o => o.PatientID);

            modelBuilder.Entity<Order>()
                .HasOne(o => o.Medicine)
                .WithMany()
                .HasForeignKey(o => o.MedicineID);

            // Payment
            modelBuilder.Entity<Payment>()
            .HasOne(p => p.Patient)
            .WithMany(p => p.Payments)
            .HasForeignKey(p => p.PatientID)
            .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<Payment>()
            .HasOne(p => p.Order)
            .WithOne(o => o.Payment)
            .HasForeignKey<Payment>(p => p.OrderID)
            .OnDelete(DeleteBehavior.Restrict);


            // ManualReview
            modelBuilder.Entity<ManualReview>()
                .HasOne(m => m.Patient)
                .WithMany()
                .HasForeignKey(m => m.PatientID)
                .OnDelete(DeleteBehavior.Restrict);


            modelBuilder.Entity<ManualReview>()
                .HasOne(m => m.Prescription)
                .WithMany()
                .HasForeignKey(m => m.PrescriptionID)
                .OnDelete(DeleteBehavior.Restrict);


            base.OnModelCreating(modelBuilder);
        }
    }
}
