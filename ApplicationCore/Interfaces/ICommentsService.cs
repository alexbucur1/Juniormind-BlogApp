using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BlogApp.Dotnet.ApplicationCore.Models;
using BlogApp.Dotnet.ApplicationCore.DTOs;

namespace BlogApp.Dotnet.ApplicationCore.Interfaces
{
    public interface ICommentsService
    {
        Task<CommentsDTO> Get(int id);
        Task<CommentsDTO> Add(CommentsDTO commentDTO);
        Task Update(CommentsDTO commentDTO);
        Task Remove(int id);
        PaginatedDTO<CommentsDTO> GetTopComments(int postID, int pageNumber, string searchString);
        PaginatedDTO<CommentsDTO> GetReplies(int postID, int parentID, int repliesPageNumber = -1);
    }
}
