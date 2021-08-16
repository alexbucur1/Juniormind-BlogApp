using BlogApp.Dotnet.API.Controllers;
using BlogApp.Dotnet.ApplicationCore.DTOs;
using BlogApp.Dotnet.ApplicationCore.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BlogApp.Dotnet.API.Tests
{
    public class CommentsControllerFacts
    {
        [Fact]
        public void GetCommentsReturnsPaginatedCommentsDTO()
        {
            int postId = 1;
            int page = 1;
            string search = "";

            var mockCommentsService = new Mock<ICommentsService>();
            mockCommentsService.Setup(service => service.GetTopComments(postId, page, search)).Returns(GetCommentsDTO());

            var controller = new CommentsController(mockCommentsService.Object);

            var result = controller.GetComments(postId, search, page);
            var listResult = Assert.IsType<PaginatedDTO<CommentsDTO>>(result);
            var models = Assert.IsAssignableFrom<PaginatedDTO<CommentsDTO>>(listResult);
            Assert.Equal(2, models.Items.Count());
            var comm = models.Items.First();
            Assert.Null(comm.ParentID);
            Assert.Equal("1This is a test comment.", comm.Content);
        }
        
        [Fact]
        public void GetReplyesReturnPaginatedCommentsDTO()
        {
            int postId = 1;
            int parrentId = 1;
            int page = -1;

            var mockCommentsService = new Mock<ICommentsService>();
            mockCommentsService.Setup(service => service.GetReplies(postId, parrentId, page)).Returns(GetRepliesDTO());

            var controller = new CommentsController(mockCommentsService.Object);
            var result = controller.GetReplies(parrentId, postId, page);
            var listResult = Assert.IsType<PaginatedDTO<CommentsDTO>>(result);
            var models = Assert.IsAssignableFrom<PaginatedDTO<CommentsDTO>>(listResult);

            Assert.Equal(2, models.Items.Count());
            var reply = models.Items.First();
            Assert.Equal(1, reply.ParentID);
        }

        [Fact]
        public async Task PutCommentReturnsOK()
        {
            var mockCommentsService = new Mock<ICommentsService>();
            mockCommentsService.Setup(service => service.Update(GetCommentDTO())).Verifiable();

            var controller = new CommentsController(mockCommentsService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };

            var result = await controller.PutComment(1, GetCommentDTO());
            mockCommentsService.Verify(service => service.Update(It.IsAny<CommentsDTO>()));
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task PutCommentThrowsAndCatchExceptionReturnsNotFound()
        {
            var mockCommentsService = new Mock<ICommentsService>();
            mockCommentsService.Setup(service => service.Update(It.IsAny<CommentsDTO>())).ThrowsAsync(new DbUpdateConcurrencyException()).Verifiable();
            mockCommentsService.Setup(service => service.Add(It.IsAny<CommentsDTO>())).Returns(Task.FromResult<CommentsDTO>(null)).Verifiable();

            var controller = new CommentsController(mockCommentsService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };

            var result = await controller.PutComment(1, GetCommentDTO());

            mockCommentsService.Verify(service => service.Update(It.IsAny<CommentsDTO>()));
            mockCommentsService.Verify(service => service.Add(It.IsAny<CommentsDTO>()));
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task PutCommentThrowsExceptionReturnsExcetion()
        {
            var mockCommentsService = new Mock<ICommentsService>();
            mockCommentsService.Setup(service => service.Update(It.IsAny<CommentsDTO>())).ThrowsAsync(new DbUpdateConcurrencyException()).Verifiable();
            mockCommentsService.Setup(service => service.Add(It.IsAny<CommentsDTO>())).ReturnsAsync(GetCommentDTO()).Verifiable();

            var controller = new CommentsController(mockCommentsService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };

            var result = await Assert.ThrowsAsync<DbUpdateConcurrencyException>(() => controller.PutComment(1, GetCommentDTO()));
            mockCommentsService.Verify(service => service.Update(It.IsAny<CommentsDTO>()));
            mockCommentsService.Verify(service => service.Add(It.IsAny<CommentsDTO>()));
        }

        [Fact]
        public async Task PutCommentReturnsBadRequestWhenIdNotFound()
        {
            var mockCommentsService = new Mock<ICommentsService>();
            var controller = new CommentsController(mockCommentsService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };

            var result = await controller.PutComment(123, GetCommentDTO());
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PutCommentReturnsBadRequestWhenModelStateInvalid()
        {
            var mockCommentsService = new Mock<ICommentsService>();

            var controller = new CommentsController(mockCommentsService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };

            controller.ModelState.AddModelError("Content", "Required");

            var result = await controller.PutComment(123, GetCommentDTO());
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task PostCommentsReturnsNoContent()
        {
            var mockCommentsService = new Mock<ICommentsService>();
            mockCommentsService.Setup(service => service.Add(It.IsAny<CommentsDTO>())).ReturnsAsync(GetCommentDTO()).Verifiable();

            var controller = new CommentsController(mockCommentsService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };

            var result = await controller.PostComment(GetCommentDTO());
            mockCommentsService.Verify(service => service.Add(It.IsAny<CommentsDTO>()));
            Assert.IsType<NoContentResult>(result.Result);
        }

        [Fact]
        public async Task PutCommentsReturnsBadRequest()
        {
            var mockCommentsService = new Mock<ICommentsService>();
            var controller = new CommentsController(mockCommentsService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };
            controller.ModelState.AddModelError("Content", "Required");

            var result = await controller.PostComment(GetCommentDTO());
            Assert.IsType<BadRequestResult>(result.Result);
        }

        [Fact]
        public async Task DeleteComentReturnsOK()
        {
            var mockCommentsService = new Mock<ICommentsService>();
            mockCommentsService.Setup(service => service.Get(It.IsAny<int>())).ReturnsAsync(GetCommentDTO()).Verifiable();

            var controller = new CommentsController(mockCommentsService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };

            var result = await controller.DeleteComment(1);
            mockCommentsService.Verify(service => service.Get(It.IsAny<int>()));
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public async Task DeleteCommentReturnNotFound()
        {
            var mockCommentsService = new Mock<ICommentsService>();
            mockCommentsService.Setup(service => service.Get(1)).Returns(Task.FromResult<CommentsDTO>(null));

            var controller = new CommentsController(mockCommentsService.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = Helpers.Helpers.GetContext()
            };

            var result = await controller.DeleteComment(1);
            Assert.IsType<NotFoundResult>(result);
        }

        private static PaginatedDTO<CommentsDTO> GetCommentsDTO()
        {
            var comms = new List<CommentsDTO> {
                new CommentsDTO
                {
                ID = 1,
                PostID = 1,
                UserID = "694c5e11-15ce-487a-bda6-d070a298b6dd",
                Content = "1This is a test comment.",
                Date = DateTime.Now
                },

                new CommentsDTO
                {
                ID = 2,
                PostID = 1,
                UserID = "694c5e11-15ce-487a-bda6-d070a298b6dd",
                Content = "2This is a test comment.",
                Date = DateTime.Now
                }
            };


            return new PaginatedDTO<CommentsDTO>(comms, 1, false, false, 5);
        }

        private static PaginatedDTO<CommentsDTO> GetRepliesDTO()
        {
            var comms = new List<CommentsDTO> {
                new CommentsDTO
                {
                ID = 3,
                PostID = 1,
                ParentID = 1,
                UserID = "694c5e11-15ce-487a-bda6-d070a298b6dd",
                Content = "This is a test comment.",
                Date = DateTime.Now
                },

                new CommentsDTO
                {
                ID = 4,
                PostID = 1,
                ParentID = 1,
                UserID = "694c5e11-15ce-487a-bda6-d070a298b6dd",
                Content = "This is a test comment.",
                Date = DateTime.Now
                }
            };


            return new PaginatedDTO<CommentsDTO>(comms, 1, false, false, 5);
        }

        private static CommentsDTO GetCommentDTO()
        {

            return new CommentsDTO
            {
                ID = 1,
                PostID = 1,
                UserID = "694c5e11-15ce-487a-bda6-d070a298b6dd",
                Content = "This is a test comment.",
                Date = DateTime.Now
            };

        }

        private static CommentsDTO GetInvalidDTO()
        {
            return new CommentsDTO()
            {
                ID = 1,
                PostID = 1,
                UserID = "694c5e11-15ce-487a-bda6-d070a298b6dd",
                Content = null,
                Date = DateTime.Now
            };
        }
    }
}
