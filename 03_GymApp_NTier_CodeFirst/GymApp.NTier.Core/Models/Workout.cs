namespace GymApp.NTier.Core.Models
{
    public class Workout
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; }
    }
}
