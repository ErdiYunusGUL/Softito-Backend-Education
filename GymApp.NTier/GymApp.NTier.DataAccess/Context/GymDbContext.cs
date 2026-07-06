using GymApp.NTier.Core.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace GymApp.NTier.DataAccess.Context
{
    public class GymDbContext : DbContext
    {
        public GymDbContext(DbContextOptions<GymDbContext> options) : base(options)
        {
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<Workout> Workouts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Plans
            modelBuilder.Entity<Plan>().HasData(
                new Plan { Id = 1, Name = "Basic Plan", Price = 250.00m, DurationInMonths = 1 },
                new Plan { Id = 2, Name = "Premium Plan", Price = 600.00m, DurationInMonths = 3 },
                new Plan { Id = 3, Name = "VIP Annual", Price = 2000.00m, DurationInMonths = 12 }
            );

            // Seed Trainers
            modelBuilder.Entity<Trainer>().HasData(
                new Trainer { Id = 1, FirstName = "John", LastName = "Doe", Specialization = "Bodybuilding" },
                new Trainer { Id = 2, FirstName = "Jane", LastName = "Smith", Specialization = "Yoga & Cardio" }
            );

            // Seed Members
            modelBuilder.Entity<Member>().HasData(
                new Member { Id = 1, FirstName = "Ahmet", LastName = "Yilmaz", Email = "ahmet@test.com", PhoneNumber = "555-0001", JoinDate = new DateTime(2023, 1, 1), PlanId = 2 },
                new Member { Id = 2, FirstName = "Ayse", LastName = "Demir", Email = "ayse@test.com", PhoneNumber = "555-0002", JoinDate = new DateTime(2023, 1, 5), PlanId = 1 }
            );
        }
    }
}
