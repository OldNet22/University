﻿#nullable disable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using University.Entities;

namespace University.Data.Data
{
    public class UniversityContext : DbContext
    {
        public DbSet<Student> Student { get; set; }
       // public DbSet<Enrollment> Enrollment { get; set; }

        public UniversityContext (DbContextOptions<UniversityContext> options)
            : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           // modelBuilder.Entity<Enrollment>().HasKey(e => new { e.StudentId, e.CourseId });
            modelBuilder.Entity<Course>().Property(c => c.Title).IsRequired();

            modelBuilder.Entity<Student>()
                        .HasMany(s => s.Courses)
                        .WithMany(c => c.Students)
                        .UsingEntity<Enrollment>(
                            e => e.HasOne(e => e.Course).WithMany(c => c.Enrollments),
                            e => e.HasOne(e => e.Student).WithMany(s => s.Enrollments)//,
                           // e => e.HasKey(e => new { e.StudentId, e.CourseId })
                            );

        }

    }
}