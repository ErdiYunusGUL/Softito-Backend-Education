namespace ScoutingAPI.DTOs
{
    public class PlayerCreateDto
    {
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public decimal MarketValue { get; set; }
        public int TeamId { get; set; }
    }
}
