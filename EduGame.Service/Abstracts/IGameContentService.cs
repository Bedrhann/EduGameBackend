// EduGame.Service/Abstracts/IGameContentService.cs
using EduGame.Core.Entities;

namespace EduGame.Service.Abstracts;

public interface IGameContentService
{
    Task<GameContent> GetByIdAsync(int id);
    Task<GameContent> CreateAsync(GameContent content);
    Task<string> ProcessAndSummarizeAsync(int contentId); // AI süreci burada tetiklenir
    Task<bool> ApproveContentAsync(int contentId); // Kullanıcı özeti onayladığında
}