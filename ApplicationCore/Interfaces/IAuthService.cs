using BlogApp.Dotnet.ApplicationCore.DTOs;
using System.Threading.Tasks;

namespace BlogApp.Dotnet.ApplicationCore.Interfaces
{
    public interface IAuthService
    {
        Task<string> GetToken(ClientDTO client);
    }
}
