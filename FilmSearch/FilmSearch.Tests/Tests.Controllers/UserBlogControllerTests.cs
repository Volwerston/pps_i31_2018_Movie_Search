using FilmSearch.Controllers;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace FilmSearch.Tests.Tests.Controllers
{
    public class UserBlogControllerTests
    {
        
        [Fact]
        public void CreateBlogTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            UserManager<AppUser> um = new FakeUserManager();
            UserBlogService ubs = new UserBlogService(uow.Object, um);
            UserBlogController ubc = new UserBlogController(ubs);

            var result = ubc.CreateBlog() as ViewResult;
            result.Should().NotBeNull();
        }
        
        [Fact]
        public void BlogViews()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PostRepository.GetAll()).Returns(_fakePosts);
            UserManager<AppUser> um = new FakeUserManager();
            UserBlogService ubs = new UserBlogService(uow.Object, um);
            UserBlogController ubc = new UserBlogController(ubs);

            var result = (ubc.BlogViews() as ViewResult)?.Model as IEnumerable<PostView>;
            
            Assert.NotNull(result);
            Assert.Equal(_fakePosts.Select(x => x.Id), result.Select(x => x.Id));
        }
        [Fact]
        public void BlogView()
        {
            const long id = 1
                ;
            UserManager<AppUser> um = new FakeUserManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PostRepository.GetByKey(id)).Returns(_fakePosts.FirstOrDefault(x => x.Id == id));
            UserBlogService ubs = new UserBlogService(uow.Object, um);
            UserBlogController ubc = new UserBlogController(ubs);

            var result = (ubc.BlogView(id) as ViewResult)?.Model as PostView;
            
            Assert.NotNull(result);
            Assert.Equal(id, result.Id);
        }
        [Fact]
        public void UserBlogViewsTest()
        {
            string userId = "1";
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PostRepository.PostsByUserId(userId)).Returns(_fakePosts);
            UserManager<AppUser> um = new FakeUserManager();
            
            UserBlogService ubs = new UserBlogService(uow.Object, um);
            UserBlogController ubc = new UserBlogController(ubs);

            var result = ubc.UserBlogViews("1") as ViewResult;
           
            Assert.NotNull(result);

            var model = result.Model as UserBlogView;
            
            Assert.NotNull(model);
            
            Assert.Equal(model.Posts.Count, _fakePosts.Count);
            Assert.Equal(model.AppUser, FakeUserManager.FakeUser);
        }
        

        private readonly List<Post> _fakePosts = new List<Post>()
            {
                new Post()
                {
                    Author = new AppUser(),
                    AuthorId = "1",
                    CreationTime = new DateTime(1, 1, 1),
                    Id = 1,
                    ImageId = 1,
                    ShortDescription = "",
                    Text = "",
                    Title ="Title"
                },
                new Post()
                {
                    Author = new AppUser(),
                    AuthorId = "1",
                    CreationTime = new DateTime(1, 1, 1),
                    Id = 2,
                    ImageId = 1,
                    ShortDescription = "",
                    Text = "",
                    Title = "Another"
                }
            };
    }
}
