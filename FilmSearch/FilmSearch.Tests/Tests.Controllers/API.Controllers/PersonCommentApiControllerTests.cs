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
using Xunit;

namespace FilmSearch.Tests.Tests.Controllers.API.Controllers
{
    public class PersonCommentApiControllerTests
    {
        [Fact]
        public void AddTest()
        {
            var comment = new Models.Entities.DTO.PersonCommentDTO() { AuthorId = "1", Id = 1, PersonId = 1, CreationDate = DateTime.Today.ToString(), Text = "text" };
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PersonCommentRepository.Add(new Models.Entities.PersonComment()
            {
                AuthorId = comment.AuthorId,
                CreationDate = comment.CreationDate,
                PersonId = comment.PersonId,
                Text = comment.Text
            }));
            UserManager<AppUser> um = new FakeUserManager();
            PersonCommentApiController PCC = new PersonCommentApiController(uow.Object, um);
            
            var result = PCC.Add(comment) as OkResult;
            uow.Verify(x => x.Save());
        }
        [Fact]
        public void GetTest()
        {
            long id = 1;
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            UserManager<AppUser> um = new FakeUserManager();
            PersonCommentApiController PCC = new PersonCommentApiController(uow.Object, um);
            uow.Setup(x => x.PersonCommentRepository.GetByPersonId(id)).Returns(new List<PersonComment>()
            {
                new PersonComment()
                {
                    AuthorId="1",
                    CreationDate=DateTime.Today.ToString(),
                    Id=1,
                    Text=""
                }
            });
            var result = PCC.Get(id) as JsonResult;
            result.Value.Should().NotBeNull();
        }
    }
}
