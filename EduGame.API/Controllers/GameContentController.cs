using EduGame.Service.Abstracts;
using EduGame.Service.DTOs; // DTO'yu eklemeyi unutma
using Microsoft.AspNetCore.Mvc;

namespace EduGame.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameContentController : ControllerBase
{
    private readonly IGameContentService _gameContentService;

    public GameContentController(IGameContentService gameContentService)
    {
        _gameContentService = gameContentService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateGameContentDto dto) // Artık DTO alıyor
    {
        var result = await _gameContentService.CreateAsync(dto);
        return Ok(result);
    }
    
    // ... Diğer metodlar (GetById, Process, Approve) aynı kalabilir
    // çünkü onlar sadece ID alıyor.
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _gameContentService.GetByIdAsync(id);
        return Ok(result);
    }

    [HttpPost("{id}/process")]
    public async Task<IActionResult> Process(int id)
    {
        var summary = await _gameContentService.ProcessAndSummarizeAsync(id);
        return Ok(new { Summary = summary });
    }

    [HttpPost("{id}/approve")]
    public async Task<IActionResult> Approve(int id)
    {
        var success = await _gameContentService.ApproveContentAsync(id);
        return success ? Ok() : BadRequest("Onaylanamadı.");
    }
}