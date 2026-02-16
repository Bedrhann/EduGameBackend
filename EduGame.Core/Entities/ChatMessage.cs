namespace EduGame.Core.Entities
{
    public class ChatMessage : BaseEntity
    {
        public string Message { get; set; } = string.Empty;
        
        // True: Kullanıcı yazdı, False: AI yazdı
        public bool IsUserMessage { get; set; } 

        // Hangi konunun sohbeti?
        public int GameContentId { get; set; }
        public GameContent GameContent { get; set; } = null!;
    }
}