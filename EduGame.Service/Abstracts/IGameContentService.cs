using EduGame.Service.DTOs;

namespace EduGame.Service.Abstracts;

public interface IGameContentService
{
    Task<GameContentDto> CreateAsync(CreateGameContentDto dto);
    
    Task<GameContentDto> GetByIdAsync(int id);
    
    Task<string> ProcessAndSummarizeAsync(int contentId);
    
    Task<bool> ApproveContentAsync(int contentId);
}