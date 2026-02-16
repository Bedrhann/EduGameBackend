using EduGame.Core.Entities;
using EduGame.Data;
using EduGame.Service.Abstracts;
using EduGame.Service.DTOs;
using Microsoft.EntityFrameworkCore;

namespace EduGame.Service.Concretes;

public class UserManager : IUserService
{
    private readonly AppDbContext _context;

    public UserManager(AppDbContext context)
    {
        _context = context;
    }

    public async Task<UserDto> CreateUserAsync(CreateUserDto dto)
    {
        // Basit bir validasyon
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            throw new Exception("Bu email zaten kayıtlı usta!");

        var entity = new User
        {
            Username = dto.Username,
            Email = dto.Email,
            // Prodüksiyonda burayı hashlememiz lazım, şimdilik düz geçiyorum
            PasswordHash = dto.Password, 
            CreatedDate = DateTime.UtcNow
        };

        await _context.Users.AddAsync(entity);
        await _context.SaveChangesAsync();

        return MapToDto(entity);
    }

    public async Task<UserDto> UpdateUserAsync(UpdateUserDto dto)
    {
        var user = await _context.Users.FindAsync(dto.Id);
        if (user == null) throw new Exception("Kullanıcı bulunamadı.");

        user.Username = dto.Username;
        user.Email = dto.Email;
        user.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return MapToDto(user);
    }

    public async Task<UserDto?> GetUserByIdAsync(int id)
    {
        var user = await _context.Users.FindAsync(id);
        return user == null ? null : MapToDto(user);
    }
    
    public async Task<List<UserDto>> GetAllUsersAsync()
    {
        var users = await _context.Users.ToListAsync();
        return users.Select(MapToDto).ToList();
    }

    // Yardımcı Dönüştürücü Metod
    private static UserDto MapToDto(User user)
    {
        return new UserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            CreatedDate = user.CreatedDate
        };
    }
}