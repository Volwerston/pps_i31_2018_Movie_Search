using FilmSearch.Controllers.API;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Models.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FilmSearch.Tests.Tests.Controllers.API.Controllers
{
    public class PersonStatsApiControllerTests
    {
        [Fact]
        public void Initialize()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            UserManager<AppUser> um = new FakeUserManager();
            PersonStatsApiController PAC = new PersonStatsApiController(uow.Object, um, new Services.PersonService(uow.Object), new Services.UserBlogService(uow.Object, um));

            PAC.Should().NotBeNull();
        }

        [Fact]
        public void PersonCommentStatsTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PersonCommentRepository.GetAll()).Returns(fakePeopleComments);
            UserManager<AppUser> um = new FakeUserManager();
            PersonStatsApiController PAC = new PersonStatsApiController(uow.Object, um, new Services.PersonService(uow.Object), new Services.UserBlogService(uow.Object, um));

            var result = PAC.PersonCommentStats("email", "1/1/2011");// as Task<IActionResult>;
            result.Should().NotBeNull(); ;
            result.Status.Should().Be(TaskStatus.Faulted);
            //PAC.Should().NotBeNull();
        }
        [Fact]
        public void PostCommentStatsTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PostCommentRepository.GetAll()).Returns(fakePostsComments);
            UserManager<AppUser> um = new FakeUserManager();
            PersonStatsApiController PAC = new PersonStatsApiController(uow.Object, um, new Services.PersonService(uow.Object), new Services.UserBlogService(uow.Object, um));

            var result = PAC.PostCommentStats("email", "1/1/2011");// as Task<IActionResult>;
            result.Should().NotBeNull();
            result.Status.Should().Be(TaskStatus.Faulted);
            //PAC.Should().NotBeNull();
        }
        List<PersonComment> fakePeopleComments = new List<PersonComment>()
        {
           new PersonComment()
           {
               AuthorId="1",
               CreationDate="10/10/2017",//DateTime.Today.ToString(),
               Id=1,
               Text="text",
               PersonId=1
           }
        };
        List<PostComment> fakePostsComments = new List<PostComment>()
        {
            new PostComment()
            {
                Id=1,
                AuthorId="1",
                Text="text",
                PostId=1,
                CommentRate=5,
                CreationDate="10/10/2017"
            }
        };
    }
}
