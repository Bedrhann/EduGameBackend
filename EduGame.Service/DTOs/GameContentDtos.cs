using EduGame.Core.Entities;

namespace EduGame.Service.DTOs;

// İçerik oluştururken istenecek veri (Sadece gerekli alanlar)
public class CreateGameContentDto
{
    public string Title { get; set; } = string.Empty;
    public ContentSourceType SourceType { get; set; }
    public string? RawText { get; set; }
    public string TargetLanguage { get; set; } = "tr-TR";
    public int UserId { get; set; }
}

// Dışarıya döneceğimiz veri (User nesnesi yok, sadece ID var)
public class GameContentDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public ContentSourceType SourceType { get; set; }
    public ContentStatus Status { get; set; }
    public string? RawText { get; set; }
    public string? ApprovedSummary { get; set; }
    public string TargetLanguage { get; set; } = string.Empty;
    public int UserId { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    
    // İleride soru listesi vb. eklenebilir
    // public List<QuestionDto> Questions { get; set; }
}