using System.Collections.Generic;

namespace GymApp.NTier.Core.Models
{
    public class Plan
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int DurationInMonths { get; set; }

        public ICollection<Member> Members { get; set; } = new List<Member>();
    }
}
