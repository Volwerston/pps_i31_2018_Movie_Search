using FilmSearch.Controllers.API;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FilmSearch.Tests.Tests.Controllers.API.Controllers
{
    public class UserBlogApiControllerTests
    {
        [Fact]
        public void Initialize()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            UserBlogService ubs = new UserBlogService(uow.Object);
            UserBlogApiController UBC = new UserBlogApiController(ubs);

            UBC.Should().NotBeNull();
        }
        /*
        [Fact]
        public void AddBlogTest()
        {
            PostView pv = new PostView()
            {
                AuthorName="illya",
                Id=1,
                Title="title",
                PostDate=DateTime.Today
            };
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            UserBlogService ubs = new UserBlogService(uow.Object);
            UserBlogApiController UBC = new UserBlogApiController(ubs);


            var result = UBC.AddBlog(pv) as ObjectResult;
            result.Value.Should().NotBeNull();
        }
        */
    }
}
