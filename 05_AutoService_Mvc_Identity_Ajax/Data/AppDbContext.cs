using AutoService_Mvc_Identity_Ajax.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AutoService_Mvc_Identity_Ajax.Data
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Brand> Brands { get; set; }
        public DbSet<CarModel> CarModels { get; set; }
        public DbSet<ServiceRecord> ServiceRecords { get; set; }
        public DbSet<ServiceAction> ServiceActions { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<ServicePart> ServiceParts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure Delete Behavior if needed (e.g. restrict deletion of brand if there are models)
            builder.Entity<CarModel>()
                .HasOne(m => m.Brand)
                .WithMany(b => b.CarModels)
                .HasForeignKey(m => m.BrandId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ServiceRecord>()
                .HasOne(s => s.CarModel)
                .WithMany()
                .HasForeignKey(s => s.CarModelId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ServiceAction>()
                .HasOne(a => a.ServiceRecord)
                .WithMany(s => s.ServiceActions)
                .HasForeignKey(a => a.ServiceRecordId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ServicePart>()
                .HasOne(sp => sp.ServiceRecord)
                .WithMany(s => s.ServiceParts)
                .HasForeignKey(sp => sp.ServiceRecordId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<ServicePart>()
                .HasOne(sp => sp.Part)
                .WithMany()
                .HasForeignKey(sp => sp.PartId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
