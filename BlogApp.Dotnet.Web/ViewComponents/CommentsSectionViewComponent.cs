using BlogApp.Dotnet.ApplicationCore.Interfaces;
using BlogApp.Dotnet.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlogApp.Dotnet.ApplicationCore.DTOs;

namespace BlogApp.Dotnet.Web.ViewComponents
{
    public class CommentsSectionViewComponent : ViewComponent
    {
        private readonly ICommentsService _commentsService;
        private readonly IAuthorizationService _authorizationService;

        public CommentsSectionViewComponent(ICommentsService commentsService, IAuthorizationService authorizationService)
        {
            _commentsService = commentsService;
            _authorizationService = authorizationService;
        }

        public async Task<IViewComponentResult> InvokeAsync(int postID, string searchString, int commsPageNumber = 1)
        {
            var commentDTOs = _commentsService.GetTopComments(postID, commsPageNumber, searchString ??= "");

            var commentViewModels = await GetCommentViewModels(commentDTOs.Items);

            foreach (var commViewModel in commentViewModels)
            {
                var replyDTOs = _commentsService.GetReplies(commViewModel.Comment.PostID, commViewModel.Comment.ID);
                commViewModel.Replies = await GetCommentViewModels(replyDTOs.Items);
            }

            ViewBag.HasNextComm = commentDTOs.HasNextPage;
            ViewBag.HasPreviousComm = commentDTOs.HasPreviousPage;
            ViewBag.PostId = postID;

            return await Task.FromResult((IViewComponentResult)View("CommentsSection", commentViewModels));
        }

        private async Task<bool> IsAuthorized(string userID)
        {
            AuthorizationResult result = await _authorizationService.AuthorizeAsync(HttpContext.User, userID, "SameOwnerPolicy");

            return result.Succeeded || HttpContext.User.IsInRole("Administrator");
        }

        private async Task<IEnumerable<CommentViewModel>> GetCommentViewModels(IEnumerable<CommentsDTO> commentDTOs)
        {
            var commentViewModels = new List<CommentViewModel>();

            foreach (var dto in commentDTOs)
            {
                if (await IsAuthorized(dto.UserID))
                {
                    commentViewModels.Add(new CommentViewModel(dto, null) { IsOwnerOrAdmin = true });
                }
                else
                {
                    commentViewModels.Add(new CommentViewModel(dto, null));
                }
            }

            return commentViewModels;
        }
    }
}
