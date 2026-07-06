namespace PaniniDapperApp.Models
{
    public class ReportStats
    {
        public int TotalCollectors { get; set; }
        public int TotalStickersInCirculation { get; set; }
        public int CompletedTrades { get; set; }
    }

    public class TopCollector
    {
        public string Username { get; set; } = string.Empty;
        public int UniqueStickersCount { get; set; }
    }

    public class MostTradedSticker
    {
        public string PlayerName { get; set; } = string.Empty;
        public string CountryName { get; set; } = string.Empty;
        public int TradeCount { get; set; }
    }
}
