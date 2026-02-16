using System.Collections.Generic;

namespace EduGame.Core.Entities
{
    public class GameContent : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public ContentSourceType SourceType { get; set; }
        public ContentStatus Status { get; set; } = ContentStatus.Draft;

        public string? RawText { get; set; }
        public string? ApprovedSummary { get; set; }
        public string TargetLanguage { get; set; } = "tr-TR"; 

        public int UserId { get; set; }
        
        // DÜZELTME: "= null!;" ekledik. 
        // Anlamı: "Başlangıçta null gibi durabilir ama ben bunu dolduracağım, uyarı verme."
        public User User { get; set; } = null!;

        // DÜZELTME: Listelerin hepsini başlattık.
        public ICollection<ContentAttachment> Attachments { get; set; } = new List<ContentAttachment>();
        public ICollection<Question> Questions { get; set; } = new List<Question>();
        public ICollection<ChatMessage> ChatHistory { get; set; } = new List<ChatMessage>();
    }
}