using System.Collections.Generic;

namespace CineCompare.Core.Entities
{
    public class Theater : BaseEntity
    {
        public string Name { get; set; } // Örn: Kadıköy Cineverse
        public string City { get; set; }
        public string Address { get; set; }

        // Hiyerarşi: Bir sinema şubesinin içinde birden fazla salon bulunur
        public ICollection<Hall> Halls { get; set; }
    }
}