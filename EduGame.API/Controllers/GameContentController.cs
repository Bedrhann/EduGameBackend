using EduGame.Core.Entities;
using EduGame.Service.Abstracts;
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
    public async Task<IActionResult> Create(GameContent content)
    {
        var result = await _gameContentService.CreateAsync(content);
        return Ok(result);
    }

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
        return success ? Ok() : BadRequest("Onaylanamadı usta!");
    }
}