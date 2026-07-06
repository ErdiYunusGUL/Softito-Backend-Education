namespace PaniniDapperApp.Models
{
    public class Sticker
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public string PlayerName { get; set; } = string.Empty;
        public int TeamId { get; set; }

        public Team? Team { get; set; }
    }
}
