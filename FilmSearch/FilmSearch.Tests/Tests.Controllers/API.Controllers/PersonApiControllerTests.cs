using Castle.Core.Configuration;
using FilmSearch.Controllers.API;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace FilmSearch.Tests.Tests.Controllers.API.Controllers
{
    public class PersonApiControllerTests
    {
        [Fact]
        public void GetAllPersonsTest()
        {
            string q = String.Empty;
            int page = 1;
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PersonRepository.PersonsByName("")).Returns(new List<Person>() { new Person() { Id = 1 } });
            PersonService ps = new PersonService(uow.Object);
            Mock<Microsoft.Extensions.Configuration.IConfiguration> conf = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            PersonApiController pc = new PersonApiController(ps, uow.Object, conf.Object);

            var result = (pc.GetAllPersons(q, page) as ObjectResult).Value;
            result.Should().NotBeNull();

        }
        [Fact]
        public void SearchTest()
        {
            PersonSearchParams pss = new PersonSearchParams()
            {
                Name = "name",
                Country = "Ukraine"
            };
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PersonRepository.GetAll()).Returns(new List<Person>() { new Person() { Id = 1, Name = "name", Country = "Ukraine" } });
            PersonService ps = new PersonService(uow.Object);
            Mock<Microsoft.Extensions.Configuration.IConfiguration> conf = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            PersonApiController pc = new PersonApiController(ps, uow.Object, conf.Object);

            var result = pc.Search(pss) as JsonResult;
            result.Value.Should().NotBeNull();
        }
        [Fact]
        public void RateTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PersonPerformanceRepository.GetAll()).Returns(fakePerfomances);
            uow.Setup(x => x.PersonRoleRepository.GetByKey(It.IsAny<long>())).Returns(new PersonRole() {Id=1,PersonId=1,FilmId=1,Performance=5 });
            PersonService ps = new PersonService(uow.Object);
            Mock<Microsoft.Extensions.Configuration.IConfiguration> conf = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            PersonApiController pc = new PersonApiController(ps, uow.Object, conf.Object);
            pc.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
           {
                new Claim(ClaimTypes.Name, "username")
           }, "someAuthTypeName"))
                }
            };
            var result = pc.Rate(1, 5) as ObjectResult;
            result.Value.Should().NotBeNull();
        }
        List<PersonPerformance> fakePerfomances = new List<PersonPerformance>()
        {
            new PersonPerformance()
            {
                Id=1,
                Performance=10,
                PersonRoleId=1,
                UserId="1"
            }
        };
    }
}
