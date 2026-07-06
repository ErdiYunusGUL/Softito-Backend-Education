using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace DartLeague.CodeFirst.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Venue> Venues { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MatchGame> MatchGames { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships to prevent cascade delete conflicts
            // since multiple foreign keys reference the Team table.

            modelBuilder.Entity<Match>()
                .HasOne(m => m.HomeTeam)
                .WithMany()
                .HasForeignKey(m => m.HomeTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.AwayTeam)
                .WithMany()
                .HasForeignKey(m => m.AwayTeamId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure MatchGame Player relationships to avoid cascade delete conflicts
            modelBuilder.Entity<MatchGame>()
                .HasOne(mg => mg.HomePlayer1).WithMany().HasForeignKey(mg => mg.HomePlayer1Id).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<MatchGame>()
                .HasOne(mg => mg.HomePlayer2).WithMany().HasForeignKey(mg => mg.HomePlayer2Id).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<MatchGame>()
                .HasOne(mg => mg.AwayPlayer1).WithMany().HasForeignKey(mg => mg.AwayPlayer1Id).OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<MatchGame>()
                .HasOne(mg => mg.AwayPlayer2).WithMany().HasForeignKey(mg => mg.AwayPlayer2Id).OnDelete(DeleteBehavior.Restrict);

            // Seed initial users
            modelBuilder.Entity<User>().HasData(
                new User { Id = 1, Username = "admin", Password = "123", Role = "Admin" },
                new User { Id = 2, Username = "user", Password = "123", Role = "User" }
            );
        }
    }
}