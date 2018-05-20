using Castle.Core.Configuration;
using FilmSearch.Controllers.API;
using FilmSearch.Models;
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
    public class NewsletterApiControllerTests
    {
        [Fact]
        public void GetAllTest()
        {
            UserManager<AppUser> um = new FakeUserManager();
            Mock<Microsoft.Extensions.Configuration.IConfiguration> conf = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            NewsletterApiController NAC = new NewsletterApiController(new Services.UserService(um), new Services.MailService(), um, conf.Object);

            var result = NAC.GetAll("q", 1) as ViewResult;
            result.Should().BeNull();
        }
        [Fact]
        public void Post()
        {
            UserManager<AppUser> um = new FakeUserManager();
            Mock<Microsoft.Extensions.Configuration.IConfiguration> conf = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            conf.Setup(x => x["Mail:Host"]).Returns("Host");
            conf.Setup(x => x["Mail:Port"]).Returns("3030");
            conf.Setup(x => x["Mail:UserName"]).Returns("UserName");
            conf.Setup(x => x["Mail:UserName"]).Returns("UserPassword");
            NewsletterApiController NAC = new NewsletterApiController(new Services.UserService(um), new Services.MailService(), um, conf.Object);

            var result = NAC.Post(new Models.DTO.NewsletterDTO() { Text="text"}) as OkResult;
            Assert.Equal(200, result.StatusCode);
        }
    }
}
