using BlogApp.Dotnet.ApplicationCore.DTOs;
using BlogApp.Dotnet.ApplicationCore.Interfaces;
using BlogApp.Dotnet.ApplicationCore.Models;
using BlogApp.Dotnet.ApplicationCore.Settings;
using BlogApp.Dotnet.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlogApp.Dotnet.Services
{
    public class PostService : IPostService
    {
        private readonly ApplicationContext _context;
        private readonly IOptions<AppSettings> _appSettings;
        public PostService(ApplicationContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings;
        }

        public async Task<PaginatedDTO<BlogPostDTO>> GetAll(int pageIndex, string searchString = "")
        {
            var truncationTreshold = _appSettings.Value.ContentPreviewTruncationTreshold;
            var pageSize = _appSettings.Value.PageSize;

            var users = _context.Users;

            var blogPostDTOs = await _context.BlogPosts
                     .Join(users,
                     p => p.UserID,
                     u => u.Id,
                     (p, u) => new
                     {
                         ID = p.ID,
                         Title = p.Title,
                         Content = p.Content,
                         CreatedAt = p.CreatedAt,
                         ModifiedAt = p.ModifiedAt,
                         ImageURL = p.ImageURL,
                         UserID = p.UserID,
                         Owner = u.FirstName + " " + u.LastName
                     })
                     .Where(postWithOwner => postWithOwner.Content.Contains(searchString)
                     || postWithOwner.Title.Contains(searchString)
                     || postWithOwner.Owner.Contains(searchString))
                     .OrderByDescending(post => post.CreatedAt)
                     .Skip((pageIndex - 1) * pageSize)
                     .Take(pageSize + 1)
                     .Select(postWithOwner => new BlogPostDTO()
                     {
                         ID = postWithOwner.ID,
                         Title = postWithOwner.Title,
                         CreatedAt = postWithOwner.CreatedAt.ToString(),
                         ModifiedAt = postWithOwner.ModifiedAt.ToString(),
                         ImageURL = postWithOwner.ImageURL,
                         UserID = postWithOwner.UserID,
                         Content = postWithOwner.Content.Length <= truncationTreshold ? postWithOwner.Content : postWithOwner.Content.Substring(0, truncationTreshold) + "...",
                         Owner = postWithOwner.Owner
                     })
                     .ToListAsync();

            return GetPaginatedDTOs(blogPostDTOs, pageIndex, pageSize);
        }

        public async Task<BlogPostDTO> GetByID(int id)
        {
            var post = _context.BlogPosts.Where(m => m.ID == id);
            var users = _context.Users;

            var postDTO = await post.Join(users,
                p => p.UserID,
                u => u.Id,
                (p, u) => new BlogPostDTO(p)
                {
                    Owner = u.FirstName + " " + u.LastName
                })
                .FirstOrDefaultAsync();

            if (postDTO == null)
            {
                return null;
            }

            return postDTO;
        }

        public async Task<int> Add(BlogPostDTO blogPostDTO)
        {
            var blogPost = ConvertDTOToBlogPost(blogPostDTO);

            blogPost.UserID = blogPostDTO.UserID;

            var added = _context.Add(blogPost);
            await _context.SaveChangesAsync();

            return added.CurrentValues.GetValue<int>("ID");
        }

        public async Task Update(BlogPostDTO blogPostDTO)
        {
            var post = await _context.BlogPosts.Where(post => post.ID == blogPostDTO.ID).FirstOrDefaultAsync();

            post.ImageURL = blogPostDTO.ImageURL;
            post.Title = blogPostDTO.Title;
            post.ModifiedAt = DateTime.Parse(blogPostDTO.ModifiedAt);
            post.CreatedAt = DateTime.Parse(blogPostDTO.CreatedAt);
            post.Content = blogPostDTO.Content;
            post.UserID = blogPostDTO.UserID;

            await _context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            var post = await _context.BlogPosts.FirstOrDefaultAsync(post => post.ID == id);
            _context.BlogPosts.Remove(post);
            await _context.SaveChangesAsync();
        }

        private static BlogPost ConvertDTOToBlogPost(BlogPostDTO dto)
        {
            return new BlogPost()
            {
                ID = dto.ID,
                Title = dto.Title,
                Content = dto.Content,
                ModifiedAt = DateTime.Parse(dto.ModifiedAt),
                CreatedAt = DateTime.Parse(dto.CreatedAt),
                ImageURL = dto.ImageURL
            };
        }

        private static PaginatedDTO<BlogPostDTO> GetPaginatedDTOs(IEnumerable<BlogPostDTO> blogPostDTOs, int pageIndex, int pageSize)
        {
            bool hasNextPage = false;

            if (blogPostDTOs.Count() > pageSize)
            {
                hasNextPage = true;
                blogPostDTOs = blogPostDTOs.SkipLast(1);
            }

            return new PaginatedDTO<BlogPostDTO>(blogPostDTOs, pageIndex, hasNextPage, pageIndex > 1, pageSize);
        }
    }
}

