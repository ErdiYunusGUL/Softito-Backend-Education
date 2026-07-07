using System.Collections.Generic;

namespace GymApp.NTier.Core.Models
{
    public class Trainer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }

        public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
    }
}
