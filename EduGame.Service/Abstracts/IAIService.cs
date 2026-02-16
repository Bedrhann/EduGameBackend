namespace EduGame.Service.Abstracts;

public interface IAIService
{
    // Ham metni alıp özet çıkarır
    Task<string> GenerateSummaryAsync(string rawText);
    
    // Özeti alıp JSON formatında soru üretir (İleride kullanacağız)
    Task<string> GenerateQuestionsAsync(string summary);
}