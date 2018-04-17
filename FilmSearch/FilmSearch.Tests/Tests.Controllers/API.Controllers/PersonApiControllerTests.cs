using Castle.Core.Configuration;
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
    public class PersonApiControllerTests
    {
        [Fact]
        public void Initialize()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            PersonService ps = new PersonService(uow.Object);
            Mock<Microsoft.Extensions.Configuration.IConfiguration> conf = new Mock<Microsoft.Extensions.Configuration.IConfiguration>();
            PersonApiController pc = new PersonApiController(ps, uow.Object, conf.Object);

            pc.Should().NotBeNull();
        }
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
    }
}
