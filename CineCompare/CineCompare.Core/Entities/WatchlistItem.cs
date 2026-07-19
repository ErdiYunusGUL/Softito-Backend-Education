namespace CineCompare.Core.Entities
{
    public class WatchlistItem : BaseEntity
    {
        public string AppUserId { get; set; } // Hangi kullanıcının listesi?

        public int MovieId { get; set; }
        public Movie Movie { get; set; }

        public bool HasWatched { get; set; } // "İzledim" veya "İzlenecekler" ayrımı için
        public int? PersonalScore { get; set; } // Kişisel puanı (Opsiyonel)
    }
}