namespace CineCompare.Core.Entities
{
    public class Review : BaseEntity
    {
        // ASP.NET Identity kullanacağımız için kullanıcı ID'leri string (Guid) formatında tutulur
        public string AppUserId { get; set; }

        public int Rating { get; set; } // 1 ile 10 arası puanlama
        public string Comment { get; set; }

        // Foreign Key: Hangi filme yorum yapıldı?
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
    }
}