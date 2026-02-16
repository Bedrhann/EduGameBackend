using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using EduGame.Service.Abstracts;
using Microsoft.Extensions.Configuration;

namespace EduGame.Service.Concretes;

public class GeminiAIService : IAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly string _baseUrl;

    public GeminiAIService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["GeminiSettings:ApiKey"] ?? throw new ArgumentNullException("API Key bulunamadı usta!");
        _baseUrl = configuration["GeminiSettings:BaseUrl"] ?? "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent";
    }

    public async Task<string> GenerateSummaryAsync(string rawText)
    {
        // 1. Prompt Hazırlığı
        var prompt = $"Aşağıdaki metni bir eğitim materyali olarak kullanmak üzere özetle. Özet anlaşılır, net ve öğretici olsun. Metin: {rawText}";

        // 2. Request Gövdesini Oluştur
        var requestBody = new
        {
            contents = new[]
            {
                new { parts = new[] { new { text = prompt } } }
            }
        };

        var jsonContent = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");

        // 3. İsteği Gönder (URL'e API Key ekliyoruz)
        var response = await _httpClient.PostAsync($"{_baseUrl}?key={_apiKey}", jsonContent);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Gemini patladı usta! Hata: {error}");
        }

        // 4. Cevabı İşle
        var jsonResponse = await response.Content.ReadAsStringAsync();
        var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(jsonResponse);

        // İç içe JSON yapısından metni çekiyoruz
        return geminiResponse?.Candidates?.FirstOrDefault()?.Content?.Parts?.FirstOrDefault()?.Text 
               ?? "Özet çıkarılamadı.";
    }

    public Task<string> GenerateQuestionsAsync(string summary)
    {
        // Bunu bir sonraki aşamada dolduracağız, şimdilik boş dursun.
        throw new NotImplementedException();
    }
}

// Gemini'den dönen karmaşık JSON'u karşılamak için yardımcı sınıflar
public class GeminiResponse
{
    [JsonPropertyName("candidates")]
    public List<Candidate>? Candidates { get; set; }
}

public class Candidate
{
    [JsonPropertyName("content")]
    public Content? Content { get; set; }
}

public class Content
{
    [JsonPropertyName("parts")]
    public List<Part>? Parts { get; set; }
}

public class Part
{
    [JsonPropertyName("text")]
    public string? Text { get; set; }
}