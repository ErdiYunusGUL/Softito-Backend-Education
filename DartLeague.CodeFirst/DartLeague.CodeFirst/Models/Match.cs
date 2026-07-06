using System;

namespace DartLeague.CodeFirst.Models
{
    public class Match
    {
        public int Id { get; set; }
        public DateTime MatchDate { get; set; }

        // Foreign Key for Venue
        public int VenueId { get; set; }
        public Venue? Venue { get; set; }

        // Foreign Key for Home Team
        public int HomeTeamId { get; set; }
        public Team? HomeTeam { get; set; }

        // Foreign Key for Away Team
        public int AwayTeamId { get; set; }
        public Team? AwayTeam { get; set; }

        // Scores and Match Status
        public int? HomeTeamScore { get; set; }
        public int? AwayTeamScore { get; set; }
        public bool IsPlayed { get; set; }
        
        // League Week (e.g., 1st Week, 2nd Week)
        public int Week { get; set; } = 1;

        // Games inside this match
        public ICollection<MatchGame>? MatchGames { get; set; }
    }
}