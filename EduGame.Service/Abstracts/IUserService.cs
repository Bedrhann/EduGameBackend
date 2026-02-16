using EduGame.Service.DTOs;

namespace EduGame.Service.Abstracts;

public interface IUserService
{
    Task<UserDto> CreateUserAsync(CreateUserDto createUserDto);
    Task<UserDto> UpdateUserAsync(UpdateUserDto updateUserDto);
    Task<UserDto?> GetUserByIdAsync(int id);
    Task<List<UserDto>> GetAllUsersAsync();
}