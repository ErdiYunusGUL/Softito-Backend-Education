using CineCompare.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace CineCompare.Data.Contexts
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var defaultDate = new DateTime(2026, 1, 1);

            // 1. Yönetmenler (Dikkat: Entity<Director> şeklinde yazıldı)
            modelBuilder.Entity<Director>().HasData(
                new Director { Id = 1, FirstName = "Christopher", LastName = "Nolan", Biography = "Zamanın efendisi.", ProfileImageUrl = "nolan.jpg", CreatedDate = defaultDate, IsActive = true },
                new Director { Id = 2, FirstName = "Denis", LastName = "Villeneuve", Biography = "Bilimkurgu vizyoneri.", ProfileImageUrl = "denis.jpg", CreatedDate = defaultDate, IsActive = true }
            );

            // 2. Filmler (Dikkat: Entity<Movie> şeklinde yazıldı)
            modelBuilder.Entity<Movie>().HasData(
                new Movie { Id = 1, Title = "Interstellar", Genre = "Sci-Fi", DurationInMinutes = 169, ReleaseDate = new DateTime(2014, 11, 7), PosterUrl = "interstellar.jpg", TrailerUrl = "url", DirectorId = 1, CreatedDate = defaultDate, IsActive = true },
                new Movie { Id = 2, Title = "Dune: Part Two", Genre = "Sci-Fi", DurationInMinutes = 166, ReleaseDate = new DateTime(2024, 3, 1), PosterUrl = "dune2.jpg", TrailerUrl = "url", DirectorId = 2, CreatedDate = defaultDate, IsActive = true }
            );

            // 3. Sinema Şubeleri
            modelBuilder.Entity<Theater>().HasData(
                new Theater { Id = 1, Name = "Kadıköy Cineverse", City = "İstanbul", Address = "Kadıköy Meydan", CreatedDate = defaultDate, IsActive = true },
                new Theater { Id = 2, Name = "Zorlu Center Cinens", City = "İstanbul", Address = "Beşiktaş", CreatedDate = defaultDate, IsActive = true }
            );

            // 4. Salonlar
            modelBuilder.Entity<Hall>().HasData(
                new Hall { Id = 1, Name = "Salon 1", HardwareType = "IMAX", Capacity = 150, TheaterId = 1, CreatedDate = defaultDate, IsActive = true },
                new Hall { Id = 2, Name = "VIP Salon", HardwareType = "VIP 2D", Capacity = 40, TheaterId = 2, CreatedDate = defaultDate, IsActive = true }
            );

            // 5. Seanslar
            modelBuilder.Entity<Showtime>().HasData(
                new Showtime { Id = 1, MovieId = 1, HallId = 1, StartTime = DateTime.Today.AddHours(20), LanguageOption = "Altyazılı", ScreeningFormat = "IMAX", CreatedDate = defaultDate, IsActive = true },
                new Showtime { Id = 2, MovieId = 2, HallId = 2, StartTime = DateTime.Today.AddHours(21), LanguageOption = "Altyazılı", ScreeningFormat = "2D", CreatedDate = defaultDate, IsActive = true }
            );

            // 6. Fiyatlar
            modelBuilder.Entity<TicketPrice>().HasData(
                new TicketPrice { Id = 1, ShowtimeId = 1, TicketCategory = "Student", Price = 200m, CreatedDate = defaultDate, IsActive = true },
                new TicketPrice { Id = 2, ShowtimeId = 1, TicketCategory = "Adult", Price = 250m, CreatedDate = defaultDate, IsActive = true },
                new TicketPrice { Id = 3, ShowtimeId = 2, TicketCategory = "Student", Price = 300m, CreatedDate = defaultDate, IsActive = true },
                new TicketPrice { Id = 4, ShowtimeId = 2, TicketCategory = "Adult", Price = 350m, CreatedDate = defaultDate, IsActive = true }
            );
        }
    }
}