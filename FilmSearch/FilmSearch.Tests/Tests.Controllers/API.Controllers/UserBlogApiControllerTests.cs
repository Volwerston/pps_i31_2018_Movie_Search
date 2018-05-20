using FilmSearch.Controllers.API;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace FilmSearch.Tests.Tests.Controllers.API.Controllers
{
    public class UserBlogApiControllerTests
    {
        [Fact]
        public void GetCommentsToPostTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PostCommentRepository.GetPostComments(1)).Returns(fakePostComments);
            UserManager<AppUser> um = new FakeUserManager();
            UserBlogService ubs = new UserBlogService(uow.Object, um);
            UserBlogApiController UBC = new UserBlogApiController(ubs);

            var result = UBC.GetCommentsToPost(1) as ObjectResult;

            result.Value.Should().NotBeNull();
        }
        [Fact]
        public void DeleteCommentsToPost()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PostCommentRepository.GetByKey((long)10)).Returns(new PostComment()
            {
                Id=1
            }
            );
            uow.Setup(x => x.PostCommentRepository.GetPostComments((long)10)).Returns(new List<PostComment>()
            {
                new PostComment()
                {
                    Id=10
                }
            }
            );
            
            UserManager<AppUser> um = new FakeUserManager();
            UserBlogService ubs = new UserBlogService(uow.Object, um);
            UserBlogApiController UBC = new UserBlogApiController(ubs);

            var result = UBC.DeleteCommentsToPost(10) as EmptyResult;
            uow.Verify(x => x.PostCommentRepository.Delete(It.IsAny<long>()));
            
        }
        [Fact]
        public void AddCommentToPost()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PostCommentRepository.GetByKey(1)).Returns(fakePostComments.Where(x => x.Id == 1).FirstOrDefault());
            uow.Setup(x => x.PostCommentRepository.GetPostComments(1)).Returns(fakePostComments);

            UserManager<AppUser> um = new FakeUserManager();
            UserBlogService ubs = new UserBlogService(uow.Object, um);
            UserBlogApiController UBC = new UserBlogApiController(ubs);
            UBC.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.Name, "username")
           }, "someAuthTypeName"))
                }
            };

            var result = UBC.AddCommentToPost(new UserBlogCommentModel() { BlogId=1,ParentId=1,Text="text"}) as ObjectResult;
            result.Value.Should().NotBeNull();
            //uow.Verify(x => x.PostCommentRepository.Delete(It.IsAny<long>()));

        }
        
        [Fact]
        public void AddBlogTest()
        {
            PostView pv = new PostView()
            {
                AuthorName="illya",
                Id=1,
                Title="title",
                PostDate=DateTime.Today,
                Text="text",
                AuthorId="1",
                ImageId=1,
                ShortDescription="shdesc"
            };
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PostRepository.Add(It.IsAny<Post>()));
            uow.Setup(x => x.Save());
            UserManager<AppUser> um = new FakeUserManager();
            UserBlogService ubs = new UserBlogService(uow.Object,um);
            UserBlogApiController UBC = new UserBlogApiController(ubs);
            UBC.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.Name, "username")
           }, "someAuthTypeName"))
                }
            };

            var result = UBC.AddBlog(pv) as ObjectResult;
            uow.Verify(x => x.PostRepository.Add(It.IsAny<Post>()));
            uow.Verify(x => x.Save());
            //result.Value.Should().NotBeNull();
        }
        
        List<PostComment> fakePostComments = new List<PostComment>()
        {
            new PostComment()
            {
                Id=1,
                Text="text",
                AuthorId="1",
                PostId=1,
                SubComments = new List<Comment>()
                {
                    new Comment()
                    {
                        Id=1
                    }
                }
            }
        };
    }
}
