using FilmSearch.Controllers;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Models.View;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FilmSearch.Tests
{
    public class PersonControllerTest
    {/*
        [Fact]
        public void InitializeTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IHostingEnvironment> he = new Mock<IHostingEnvironment>();
            var userStore = new Mock<IUserStore<AppUser>>();
            UserManager<AppUser> userManager = new UserManager<AppUser>(userStore.Object);
            PersonController PE = new PersonController(uow.Object, he.Object,userManager);
            PE.Should().NotBeNull();
        }
        [Fact]
        public void ListTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IHostingEnvironment> he = new Mock<IHostingEnvironment>();
            IEnumerable<Person> people = new List<Person>()
            {
                new Person
                {
                    BirthDate =DateTime.Now,
                    Id=1,
                    Name="Ivan",
                    Surname="Ivanov",
                    Country="Ukraine",
                    Photo = new File(),
                    PhotoId=1
                }
            };
            uow.Setup(x => x.PersonRepository.GetAll()).Returns(people);
            PersonController PE = new PersonController(uow.Object, he.Object);
            var result = PE.List();
            var code = (result as ViewResult).StatusCode;
            Assert.Equal(200, code);
        }
        [Fact]
        public void CreateTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IHostingEnvironment> he = new Mock<IHostingEnvironment>();
            PersonController PE = new PersonController(uow.Object, he.Object);
            var result = PE.Create();
            var code = (result as ViewResult).StatusCode;
            Assert.Equal(200, code);
        }
        [Fact]
        public void EditTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IHostingEnvironment> he = new Mock<IHostingEnvironment>();
            IEnumerable<Person> people = new List<Person>()
            {

            };
            var key = 1;
            //uow.Setup(x=>x.PersonRepository.Delete(key))
            try
            {
                uow.Setup(x => x.PersonRepository.GetAll()).Returns(people);
                uow.Setup(x => x.PersonRepository.GetByKey(key)).Returns(
                    new Person
                    {
                        BirthDate = DateTime.Now,
                        Id = key,
                        Name = "Ivan",
                        Surname = "Ivanov",
                        Country = "Ukraine",
                        Photo = new File(),
                        PhotoId = 1
                    }
                    );
                uow.Setup(x => x.FileRepository.GetByKey(key)).Returns(new File());
                PersonController PE = new PersonController(uow.Object, he.Object);
                long id = 1;
                var result = PE.Edit(1);
                var code = (result as ViewResult).StatusCode;
            }
            catch
            {
                Assert.True(true);
                return;
            }
            Assert.True(false);
        }

        /// <summary>
        /// I POSTPONE METHODS WHICH USE VIEWMODEL AS I DON'T KNOW HOW USE IT IN TESTS
        /// </summary>
        //NOT CORRECT USING VIEWMODEL
        [Fact]
        public void CreateVMTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IHostingEnvironment> he = new Mock<IHostingEnvironment>();
            PersonController PE = new PersonController(uow.Object, he.Object);

            PersonViewModel pvm = new PersonViewModel();
            try
            {
                var result = PE.Create(pvm).As<RedirectToActionResult>();
                var expected = new RouteValueDictionary
                {
                    {"action", "List"},
                    {"controller", "Person"}
                };
                Assert.Equal(expected, result.RouteValues);
            }
            catch
            {
                Assert.True(true);
                return;
            }
            Assert.True(false);
        }*/

    }
}
