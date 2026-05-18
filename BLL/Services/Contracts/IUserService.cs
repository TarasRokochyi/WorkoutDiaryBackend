using BLL.DTO;
using BLL.DTO.Identity;
using DAL.Models;

namespace BLL.Services.Contracts;

public interface IUserService
{
    Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();
    Task<UserResponseDTO> UpdateUserAsync(int id, UserRequestDTO user);
    Task<string> UpdatePasswordAsync(int id, UpdatePasswordRequest request);
    Task DeleteUserAsync(int id);
    
    Task<string> RegisterAsync(RegisterModel model);
    Task<AuthenticationModel> GetTokenAsync(TokenRequestModel model);
    Task<string> AddRoleAsync(AddRoleModel model);
    Task<AuthenticationModel> RefreshTokenAsync(string token);
    Task<UserResponseDTO> GetByIdAsync(int id);
    Task<bool> RevokeTokenAsync(string token);
}