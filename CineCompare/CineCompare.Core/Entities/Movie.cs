using System;

namespace CineCompare.Core.Entities
{
    public class Movie : BaseEntity
    {
        public string Title { get; set; }
        public string Genre { get; set; }
        public int DurationInMinutes { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string PosterUrl { get; set; }
        public string TrailerUrl { get; set; }

        // Foreign Key (Yabancı Anahtar): Bu film hangi yönetmene ait?
        public int DirectorId { get; set; }

        // Navigation Property: EF Core bu sayede yönetmenin bilgilerini de getirecek
        public Director Director { get; set; }

        public ICollection<Showtime> Showtimes { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<WatchlistItem> WatchlistItems { get; set; }
    }
}