namespace ScoutingAPI.Entities
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public decimal MarketValue { get; set; }
        
        public int TeamId { get; set; }
        public Team Team { get; set; } = null!;
    }
}
