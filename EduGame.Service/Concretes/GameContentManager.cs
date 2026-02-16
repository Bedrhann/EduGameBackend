using EduGame.Core.Entities;
using EduGame.Data;
using EduGame.Service.Abstracts;
using Microsoft.EntityFrameworkCore;

namespace EduGame.Service.Concretes;

public class GameContentManager : IGameContentService
{
    private readonly AppDbContext _context;
    private readonly IAIService _aiService; // Yapay Zeka Servisimiz

    // Constructor (Dependency Injection)
    public GameContentManager(AppDbContext context, IAIService aiService)
    {
        _context = context;
        _aiService = aiService;
    }

    // 1. Yeni İçerik Oluşturma
    public async Task<GameContent> CreateAsync(GameContent content)
    {
        content.Status = ContentStatus.Draft;
        content.CreatedDate = DateTime.UtcNow;
        
        await _context.GameContents.AddAsync(content);
        await _context.SaveChangesAsync();
        
        return content;
    }

    // 2. İçeriği İşle ve Özetle (En Kritik Yer)
    public async Task<string> ProcessAndSummarizeAsync(int contentId)
    {
        var content = await _context.GameContents.FindAsync(contentId);
        if (content == null) throw new Exception("İçerik bulunamadı usta!");

        // Durumu güncelle: İşleniyor
        content.Status = ContentStatus.Processing;
        await _context.SaveChangesAsync();

        try
        {
            // Eğer metin yoksa AI'ya boşuna gitmeyelim
            if (string.IsNullOrEmpty(content.RawText))
            {
                throw new Exception("İşlenecek ham metin (RawText) boş!");
            }

            // --- AI DEVREYE GİRİYOR ---
            var summary = await _aiService.GenerateSummaryAsync(content.RawText);
            
            // Sonuçları kaydet
            content.ApprovedSummary = summary;
            content.Status = ContentStatus.WaitingApproval; // Onay bekliyor
            content.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return summary;
        }
        catch (Exception ex)
        {
            // Hata olursa durumu geri al veya logla
            content.Status = ContentStatus.Draft; // Tekrar taslağa çekiyoruz ki düzeltilebilsin
            await _context.SaveChangesAsync();
            
            // Hatayı yukarı fırlat, Controller yakalasın
            throw new Exception($"AI İşlemi sırasında hata: {ex.Message}");
        }
    }

    // 3. Kullanıcı Onayı
    public async Task<bool> ApproveContentAsync(int contentId)
    {
        var content = await _context.GameContents.FindAsync(contentId);
        if (content == null) return false;

        // Sadece özeti çıkmış içerikler onaylanabilir
        if (string.IsNullOrEmpty(content.ApprovedSummary)) return false;

        content.Status = ContentStatus.Approved;
        content.UpdatedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }

    // 4. ID'ye Göre Getir
    public async Task<GameContent> GetByIdAsync(int id)
    {
        return await _context.GameContents
            .Include(x => x.Questions) // Soruları da getir
            .FirstOrDefaultAsync(x => x.Id == id) 
            ?? throw new Exception("İçerik bulunamadı.");
    }
}