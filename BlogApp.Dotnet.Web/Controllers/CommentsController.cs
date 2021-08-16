using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlogApp.Dotnet.ApplicationCore.Interfaces;
using BlogApp.Dotnet.ApplicationCore.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BlogApp.Dotnet.Web.Controllers
{
    [Authorize]
    public class CommentsController : BaseController<CommentsController>
    {
        private readonly ICommentsService _commentsService;
        private readonly IAuthorizationService _authorizationService;

        public CommentsController(ICommentsService commentsService, IAuthorizationService authorizationService)
        {
            _commentsService = commentsService;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ID,PostID,Content,ParentID")] CommentsDTO comment)
        {
            comment.UserID = User.FindFirstValue(ClaimTypes.NameIdentifier);
            comment.Date = DateTime.Now;
            ModelState.Remove("UserID");

            if (ModelState.IsValid)
            {
                var addedComment = await _commentsService.Add(comment);
                TempData["CommID"] = addedComment.ID;
                TempData.Keep("CommID");
                TempData["ParentID"] = addedComment.ParentID ??= 0;
                TempData.Keep("ParentID");
                return RedirectToAction("Details", "Posts", new { id = comment.PostID });
            }

            return RedirectToAction("Details", "Posts", new { id = comment.PostID });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(int id, [Bind("ID,UserID,Date,PostID,Content,ParentID")] CommentsDTO comment, int page = 1)
        {
            if (id != comment.ID)
            {
                return NotFound();
            }

            if (!await IsAuthorized(comment.UserID, _authorizationService))
            {
                return StatusCode(403);
            }

            if (ModelState.IsValid)
            {
                TempData["CommID"] = comment.ID;
                TempData.Keep("CommID");
                TempData["ParentID"] = comment.ParentID;
                TempData.Keep("ParentID");
                try
                {
                    await _commentsService.Update(comment);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CommentExists(comment))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Posts", new { id = comment.PostID, commsPage = page});
            }
            return RedirectToAction("Details", "Posts", new { id = comment.PostID });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comment = await _commentsService.Get(id);

            if (!await IsAuthorized(comment.UserID, _authorizationService))
            {
                return StatusCode(403);
            }

            await _commentsService.Remove(comment.ID);
            return RedirectToAction("Details", "Posts", new { id = comment.PostID });
        }

        private bool CommentExists(CommentsDTO comment)
        {
            return _commentsService.Add(comment) != null;
        }
    }
}
