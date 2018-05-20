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
        public void Initialize()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            UserManager<AppUser> um = new FakeUserManager();
            UserBlogService ubs = new UserBlogService(uow.Object, um);
            UserBlogController UBC = new UserBlogController(ubs);

            UBC.Should().NotBeNull();
        }
        [Fact]
        public void CreateBlogTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            UserManager<AppUser> um = new FakeUserManager();
            UserBlogService ubs = new UserBlogService(uow.Object, um);
            UserBlogController UBC = new UserBlogController(ubs);

            var result = UBC.CreateBlog() as ViewResult;
            result.Should().NotBeNull();
        }
        [Fact]
        public void BlogViews()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PostRepository.GetAll()).Returns(fakeposts);
            UserManager<AppUser> um = new FakeUserManager();
            UserBlogService ubs = new UserBlogService(uow.Object, um);
            UserBlogController UBC = new UserBlogController(ubs);

            var result = (UBC.BlogViews() as ViewResult).Model as IEnumerable<PostView>;
            Assert.Equal(fakeposts.Select(x => x.Id), result.Select(x => x.Id));
            //result.Should().NotBeNull();
        }
        [Fact]
        public void BlogView()
        {
            long id = 1;
            UserManager<AppUser> um = new FakeUserManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PostRepository.GetByKey(id)).Returns(fakeposts.Where(x => x.Id == id).FirstOrDefault());
            UserBlogService ubs = new UserBlogService(uow.Object, um);
            UserBlogController UBC = new UserBlogController(ubs);

            var result = (UBC.BlogView(id) as ViewResult).Model as PostView;
            Assert.Equal(id, result.Id);
        }
        [Fact]
        public void UserBlogViewsTest()
        {
            string userId = "1";
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PostRepository.GetAll()).Returns(fakeposts);
            uow.Setup(x => x.PostRepository.PostsByUserId(userId)).Returns(fakeposts);
            UserManager<AppUser> um = new FakeUserManager();
            UserBlogService ubs = new UserBlogService(uow.Object, um);
            UserBlogController UBC = new UserBlogController(ubs);

            var result = (UBC.UserBlogViews("1") as ViewResult);
            result.Model.Should().NotBeNull();
        }
        List<Post> fakeposts = new List<Post>()
            {
                new Post(){Author=new AppUser(),AuthorId="1",CreationTime=DateTime.Today,Id=1,ImageId=1,ShortDescription="",Text="",Title="Title"},
                new Post(){Author=new AppUser(),AuthorId="1",CreationTime=DateTime.Today,Id=2,ImageId=1,ShortDescription="",Text="",Title="Another"}
            };
    }
}
