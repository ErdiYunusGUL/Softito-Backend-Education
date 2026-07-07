using System.ComponentModel.DataAnnotations;

namespace DartLeague.CodeFirst.Models
{
    public class MatchGame
    {
        public int Id { get; set; }

        public int MatchId { get; set; }
        public Match? Match { get; set; }

        public int GameOrder { get; set; } // 1 to 9
        public bool IsDoubles { get; set; }

        // Home Team Players
        public int? HomePlayer1Id { get; set; }
        public Player? HomePlayer1 { get; set; }

        public int? HomePlayer2Id { get; set; } // Null for singles
        public Player? HomePlayer2 { get; set; }

        // Away Team Players
        public int? AwayPlayer1Id { get; set; }
        public Player? AwayPlayer1 { get; set; }

        public int? AwayPlayer2Id { get; set; } // Null for singles
        public Player? AwayPlayer2 { get; set; }

        // Scores (Legs won in this game)
        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
    }
}
