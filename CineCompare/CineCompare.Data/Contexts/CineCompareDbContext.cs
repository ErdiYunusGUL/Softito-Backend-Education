using CineCompare.Core.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CineCompare.Data.Contexts
{
    public class CineCompareDbContext : IdentityDbContext
    {
        public CineCompareDbContext(DbContextOptions<CineCompareDbContext> options) : base(options)
        {
        }

        public DbSet<Director> Directors { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Theater> Theaters { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Showtime> Showtimes { get; set; }
        public DbSet<TicketPrice> TicketPrices { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<WatchlistItem> WatchlistItems { get; set; }
        public DbSet<AiChatLog> AiChatLogs { get; set; }
        public DbSet<SystemLog> SystemLogs { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder); // Identity tablolarının hatasız kurulması için şart

            // Seans (Showtime) silinirse, ona bağlı fiyatlar (TicketPrice) da silinsin (Cascade Delete)
            builder.Entity<Showtime>()
                .HasMany(s => s.TicketPrices)
                .WithOne(t => t.Showtime)
                .HasForeignKey(t => t.ShowtimeId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Seed(); // Yazdığımız Extension metodunu çağırıyoruz
        }
    }
  

    }
