﻿using Microsoft.EntityFrameworkCore;
using BedTrack.Domain.Models;
using System;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace BedTrack.DAL.Data
{
    public class BedTrackContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<User> Users { get; set; } = null;
        public DbSet<Clinic> Clinics { get; set; } = null;
        public DbSet<Department> Departments { get; set; } = null;
        public DbSet<ClinicDepartment> ClinicDepartments { get; set; } = null;
        public DbSet<ClinicDepartmentBed> ClinicDepartmentBeds { get; set; } = null;
        public DbSet<Bed> Beds { get; set; } = null;
        public DbSet<Patient> Patients { get; set; } = null;
        public DbSet<Event> Events { get; set; } = null;
        public DbSet<UserEvent> UserEvents { get; set; } = null;
        public DbSet<UserPatient> UserPatients { get; set; } = null;

        public BedTrackContext(DbContextOptions<BedTrackContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserPatient>()
            .HasOne(up => up.User)
            .WithMany(u => u.Patients)
            .HasForeignKey(up => up.UserId)
            .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserPatient>()
                .HasOne(up => up.Patient)
                .WithMany(p => p.Users)
                .HasForeignKey(up => up.PatientId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Event>()
            .Property(e => e.Alarm)
            .HasColumnType("datetime2");

            base.OnModelCreating(modelBuilder);

        }
    }
}
