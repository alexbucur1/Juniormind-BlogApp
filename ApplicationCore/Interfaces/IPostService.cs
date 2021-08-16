using System.Collections.Generic;
using System.Threading.Tasks;
using BlogApp.Dotnet.ApplicationCore.DTOs;

namespace BlogApp.Dotnet.ApplicationCore.Interfaces
{
    public interface IPostService
    {
        Task<PaginatedDTO<BlogPostDTO>> GetAll(int pageNumber, string searchString);
        Task<BlogPostDTO> GetByID(int id);
        Task<int> Add(BlogPostDTO blogPostDTO);
        Task Update(BlogPostDTO blogPostDTO);
        Task Delete(int id);
    }
}
