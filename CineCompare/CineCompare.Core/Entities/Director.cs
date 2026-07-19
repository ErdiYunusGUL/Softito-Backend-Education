using System.Collections.Generic;

namespace CineCompare.Core.Entities
{
    // BaseEntity'den Id ve CreatedDate özelliklerini miras alır
    public class Director : BaseEntity
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Biography { get; set; }
        public string ProfileImageUrl { get; set; }

        // Navigation Property: Bir yönetmenin birden fazla filmi olabilir (One-to-Many)
        public ICollection<Movie> Movies { get; set; }
    }
}