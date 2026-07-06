using System;
using System.Collections.Generic;

namespace GymApp.NTier.Core.Models
{
    public class Member
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime JoinDate { get; set; }

        public int PlanId { get; set; }
        public Plan Plan { get; set; }

        public ICollection<Workout> Workouts { get; set; } = new List<Workout>();
    }
}
