using EduGame.Core.Entities;
using EduGame.Data;
using EduGame.Service.Abstracts;
using EduGame.Service.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EduGame.Service.Concretes;

public class GameContentManager : IGameContentService
{
    private readonly AppDbContext _context;
    private readonly IAIService _aiService;

    public GameContentManager(AppDbContext context, IAIService aiService)
    {
        _context = context;
        _aiService = aiService;
    }

    public async Task<GameContentDto> CreateAsync(CreateGameContentDto dto)
    {
        // 1. DTO -> Entity Dönüşümü (Mapping)
        var entity = new GameContent
        {
            Title = dto.Title,
            SourceType = dto.SourceType,
            RawText = dto.RawText,
            TargetLanguage = dto.TargetLanguage,
            UserId = dto.UserId,
            Status = ContentStatus.Draft,
            CreatedDate = DateTime.UtcNow
        };

        // 2. Veritabanına Kayıt
        await _context.GameContents.AddAsync(entity);
        await _context.SaveChangesAsync();

        // 3. Entity -> DTO Dönüşümü ve Return
        return MapToDto(entity);
    }

    public async Task<GameContentDto> GetByIdAsync(int id)
    {
        var entity = await _context.GameContents
            .Include(x => x.Questions)
            .FirstOrDefaultAsync(x => x.Id == id)
            ?? throw new Exception("İçerik bulunamadı.");

        return MapToDto(entity);
    }

    public async Task<string> ProcessAndSummarizeAsync(int contentId)
    {
        var content = await _context.GameContents.FindAsync(contentId);
        if (content == null) throw new Exception("İçerik bulunamadı.");

        content.Status = ContentStatus.Processing;
        await _context.SaveChangesAsync();

        try
        {
            if (string.IsNullOrEmpty(content.RawText))
                throw new Exception("RawText boş!");

            var summary = await _aiService.GenerateSummaryAsync(content.RawText);

            content.ApprovedSummary = summary;
            content.Status = ContentStatus.WaitingApproval;
            content.UpdatedDate = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return summary;
        }
        catch (Exception)
        {
            content.Status = ContentStatus.Draft;
            await _context.SaveChangesAsync();
            throw;
        }
    }

    public async Task<bool> ApproveContentAsync(int contentId)
    {
        var content = await _context.GameContents.FindAsync(contentId);
        if (content == null || string.IsNullOrEmpty(content.ApprovedSummary)) return false;

        content.Status = ContentStatus.Approved;
        content.UpdatedDate = DateTime.UtcNow;
        
        await _context.SaveChangesAsync();
        return true;
    }

    // Yardımcı Mapper Metodu
    private static GameContentDto MapToDto(GameContent entity)
    {
        return new GameContentDto
        {
            Id = entity.Id,
            Title = entity.Title,
            SourceType = entity.SourceType,
            Status = entity.Status,
            RawText = entity.RawText,
            ApprovedSummary = entity.ApprovedSummary,
            TargetLanguage = entity.TargetLanguage,
            UserId = entity.UserId,
            CreatedDate = entity.CreatedDate,
            UpdatedDate = entity.UpdatedDate
        };
    }
}