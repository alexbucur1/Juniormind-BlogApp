using BlogApp.Dotnet.ApplicationCore.DTOs;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BlogApp.Dotnet.ApplicationCore.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> CreateAsync(UserDTO user);

        Task<SignInResult> PasswordSignInAsync(UserDTO user, string Password);

        Task SignOutAsync();

        Task<IdentityResult> UpdateAsync(UserDTO oldUser, UserDTO newUser, bool passwordChanged);

        Task<IdentityError> ValidatePassword(string password);

        Task<PaginatedDTO<UserDTO>> GetAll(string sortOrder = "", string searchString = "", int? pageNumber = 1);

        Task<UserDTO> FindByIdAsync(string id);

        Task<UserDTO> FindByEmailAsync(string email);

        Task<IdentityResult> Delete(string id);
    }
}
