using DartLeague.CodeFirst.Models;
using System.Collections.Generic;

namespace DartLeague.CodeFirst.ViewModels
{
    public class EditGamesViewModel
    {
        public int MatchId { get; set; }
        public string? HomeTeamName { get; set; }
        public string? AwayTeamName { get; set; }
        public List<Player> HomePlayers { get; set; } = new List<Player>();
        public List<Player> AwayPlayers { get; set; } = new List<Player>();
        
        public List<MatchGameViewModel> Games { get; set; } = new List<MatchGameViewModel>();
    }

    public class MatchGameViewModel
    {
        public int Id { get; set; }
        public int GameOrder { get; set; }
        public bool IsDoubles { get; set; }

        public int? HomePlayer1Id { get; set; }
        public int? HomePlayer2Id { get; set; }
        public int? AwayPlayer1Id { get; set; }
        public int? AwayPlayer2Id { get; set; }

        public int HomeScore { get; set; }
        public int AwayScore { get; set; }
    }

    public class MatchDetailsViewModel
    {
        public Match Match { get; set; } = null!;
        public List<MatchGame> Games { get; set; } = new List<MatchGame>();
    }
}
