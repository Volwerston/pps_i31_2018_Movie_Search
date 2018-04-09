using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Services;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FilmSearch.Tests
{
    public class PersonServiceTest
    {
        [Fact]
        public void InitializationTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();

            PersonService PS = new PersonService(uow.Object);

            PS.Should().NotBeNull();
        }
        [Fact]
        public void GetPersonsByNamePaginatedTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            string name = "name";
            uow.Setup(x => x.PersonRepository.PersonsByName(name)).Returns(new List<Person>());

            PersonService PS = new PersonService(uow.Object);
            var result = PS.GetPersonsByNamePaginated("name", 1);
            result.Should().NotBeNull();
        }
    }
}
