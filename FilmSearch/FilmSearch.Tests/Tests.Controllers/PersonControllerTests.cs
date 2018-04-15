using FilmSearch.Controllers;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Models.View;
using FilmSearch.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace FilmSearch.Tests.Tests.Controllers
{
    public class PersonControllerTests
    {
        [Fact]
        public void Initialize()
        {
            var um = new FakeUserManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            PersonService ps = new PersonService(uow.Object);
            PersonController PC = new PersonController(uow.Object, env.Object, ps, um);

            PC.Should().NotBeNull();
        }
        [Fact]
        public void List()
        {
            var um = new FakeUserManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PersonRepository.GetAll()).Returns(fakePerson);
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            PersonService ps = new PersonService(uow.Object);
            PersonController PC = new PersonController(uow.Object, env.Object, ps, um);

            var result = (PC.List() as ViewResult).Model as List<Person>;
            Assert.Equal(fakePerson, result);
        }
        //EXCEPTION?
        [Fact]
        public void Create()
        {
            var um = new FakeUserManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PersonRepository.GetAll()).Returns(fakePerson);
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            PersonService ps = new PersonService(uow.Object);
            PersonController PC = new PersonController(uow.Object, env.Object, ps, um);

            var fileMock = new Mock<IFormFile>();
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            PersonViewModel pvm = new PersonViewModel()
            {
                BirthDate = DateTime.Today,
                Id=3,
                Country="Ukraine",
                Name="Lag",
                Surname="Do",
                Photo = fileMock.Object
            };

            var result = (PC.Create(pvm) as ViewResult);
            uow.Verify(x => x.Save());
        }
        [Fact]
        public void Edit()
        {
            long id = 1;
            var um = new FakeUserManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PersonRepository.GetAll()).Returns(fakePerson);
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            PersonService ps = new PersonService(uow.Object);
            PersonController PC = new PersonController(uow.Object, env.Object, ps, um);

            var result = PC.Delete(id) as ViewResult;
        }
        List<Person> fakePerson = new List<Person>()
        {
            new Person()
            {
                BirthDate = DateTime.Today,
                Country="Ukraine",
                Id=1,
                Name="name",
                Surname="surname",
                PhotoId=1
            }
        };
    }
}
