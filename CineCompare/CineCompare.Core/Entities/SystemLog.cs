namespace CineCompare.Core.Entities
{
    public class SystemLog : BaseEntity
    {
        public string LogType { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public string UserId { get; set; }
    }
}