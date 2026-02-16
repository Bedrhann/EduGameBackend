using EduGame.Service.Abstracts;
using EduGame.Service.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace EduGame.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserDto dto)
    {
        var result = await _userService.CreateUserAsync(dto);
        return Ok(result);
    }

    [HttpPut]
    public async Task<IActionResult> Update(UpdateUserDto dto)
    {
        var result = await _userService.UpdateUserAsync(dto);
        return Ok(result);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _userService.GetAllUsersAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result = await _userService.GetUserByIdAsync(id);
        return result != null ? Ok(result) : NotFound("Kullanıcı yok usta.");
    }
}