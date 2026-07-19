namespace CineCompare.Core.Entities
{
    public class AiChatLog : BaseEntity
    {
        public string AppUserId { get; set; }
        public string UserPrompt { get; set; } // Kullanıcının sorduğu soru
        public string AiResponse { get; set; } // Gemini/AI'ın döndüğü cevap
    }
}