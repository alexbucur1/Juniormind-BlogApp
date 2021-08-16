using BlogApp.Dotnet.ApplicationCore.DTOs;
using BlogApp.Dotnet.ApplicationCore.Interfaces;
using BlogApp.Dotnet.ApplicationCore.Models;
using BlogApp.Dotnet.ApplicationCore.Settings;
using BlogApp.Dotnet.DAL;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Dotnet.Services
{
    public class CommentsService : ICommentsService
    {
        private readonly ApplicationContext _context;
        private readonly IOptions<AppSettings> _appSettings;
        public CommentsService(ApplicationContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings;
        }

        public async Task<CommentsDTO> Add(CommentsDTO commentDTO)
        {
            var comment = _context.Add(ConvertToComment(commentDTO)).Entity;
            await SaveAsync();
            return new CommentsDTO(comment, null);
        }

        public async Task<CommentsDTO> Get(int id)
        {
            return new CommentsDTO(await _context.Comments.FindAsync(id), null);
        }

        public async Task Remove(int id)
        {
            foreach (var comment in _context.Comments.Where(comment => comment.ParentID == id || comment.ID == id))
            {
                _context.Comments.Remove(comment);
            }

            await SaveAsync();
        }
        
        public async Task Update(CommentsDTO commentDTO)
        {
            var comment = await _context.Comments.FindAsync(commentDTO.ID);
            comment.Content = commentDTO.Content;
            _context.Update(comment);
            await SaveAsync();
        }

        public PaginatedDTO<CommentsDTO> GetTopComments(int postID, int pageNumber, string searchString = null)
        {
            var pageSize = _appSettings.Value.PageSize;

            var comments = _context.Comments
                .Where(comment => comment.PostID == postID && comment.ParentID == null && (comment.User.FirstName.Contains(searchString) || comment.User.LastName.Contains(searchString)
                || comment.Content.Contains(searchString))).Skip((pageNumber - 1) * pageSize).Take(pageSize + 1);
            var comms = comments
                .Join(_context.Users,
                comment => comment.UserID,
                user => user.Id,
                (comment, user) => new CommentsDTO(comment, GenerateFullName(user), Convert.ToInt32(_context.Comments.Where(reply => reply.ParentID == comment.ID).Count())));

            //Something is wrong when using inMemoryDatabase in tests
            //Used Convert.ToInt32 as a workaround for https://stackoverflow.com/questions/66000656/invalidcastexception-running-integration-tests-using-inmemorydatabase-ef-core
            return GetPaginatedDTOs(comms, pageNumber, pageSize);
        }

        public PaginatedDTO<CommentsDTO> GetReplies(int postID, int parentID, int repliesPageNumber = -1)
        {
            var pageSize = _appSettings.Value.PageSize;
            IQueryable<Comment> replies;

            if (repliesPageNumber != -1)
            {
                replies = _context.Comments
                    .Where(comment => comment.PostID == postID && comment.ParentID == parentID)
                    .Skip((repliesPageNumber - 1) * pageSize).Take(pageSize + 1);
            }
            else
            {
                pageSize = -1;
                replies = _context.Comments
                    .Where(comment => comment.PostID == postID && comment.ParentID == parentID);
            }

            var rep = replies
                .Join(_context.Users,
                reply => reply.UserID,
                user => user.Id,
                (comment, user) => new CommentsDTO(comment, GenerateFullName(user), 0));

            return GetPaginatedDTOs(rep, repliesPageNumber, pageSize);
        }

        private static Comment ConvertToComment(CommentsDTO dto)
        {

            return new Comment()
            {
                ID = dto.ID,
                ParentID = dto.ParentID,
                PostID = dto.PostID,
                UserID = dto.UserID,
                Content = dto.Content,
                Date = dto.Date
            };
        }

        private async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        private static string GenerateFullName(User user)
        {
            if (user == default(User))
            {
                return "Anonymous";
            }

            return user.FirstName + " " + user.LastName;
        }

        private static PaginatedDTO<CommentsDTO> GetPaginatedDTOs(IEnumerable<CommentsDTO> commsPostDTOs, int pageIndex, int pageSize)
        {
            bool hasNextPage = false;

            if (commsPostDTOs.Count() > pageSize && pageSize != -1)
            {
                hasNextPage = true;
                commsPostDTOs = commsPostDTOs.SkipLast(1);
            }

            return new PaginatedDTO<CommentsDTO>(commsPostDTOs, pageIndex, hasNextPage, pageIndex > 1, pageSize);
        }
    }
}
