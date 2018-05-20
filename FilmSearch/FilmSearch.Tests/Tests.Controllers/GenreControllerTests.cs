using Castle.Core.Logging;
using FilmSearch.Controllers;
using FilmSearch.DAL;
using FilmSearch.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace FilmSearch.Tests.Tests.Controllers
{
    public class GenreControllerTests
    {
        [Fact]
        public void Initialize()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            GenreController GC = new GenreController(uow.Object);
            GC.Should().NotBeNull();
        }

        [Fact]
        public void IndexTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.GenreRepository.GetAll()).Returns(fakeGenres);
            GenreController GC = new GenreController(uow.Object);
            var result = (GC.Index()) as ViewResult;

            result.Model.Should().NotBeNull();
        }
        [Fact]
        public void RemoveTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.GenreRepository.GetAll()).Returns(fakeGenres);
            GenreController GC = new GenreController(uow.Object);
            var result = (GC.Remove(1)) as OkResult;
            uow.Verify(x => x.GenreRepository.Delete(It.IsAny<Object>()));
            uow.Verify(x => x.Save());
            Assert.Equal(200, result.StatusCode);
            result = (GC.Remove(100)) as OkResult;


        }
        [Fact]
        public void AddTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.GenreRepository.GetAll()).Returns(fakeGenres);
            GenreController GC = new GenreController(uow.Object);
           
            var result = (GC.Add(new Genre() { Id = 5, Name = "Horror" })) as StatusCodeResult;
            Assert.Equal(200, result.StatusCode);
            var result2 = (GC.Add(fakeGenres[0])) as ObjectResult;
            Assert.Equal(500,result2.StatusCode);
        }
        List<Genre> fakeGenres = new List<Genre>()
        {
            new Genre()
            {
                Id=1,
                Name="Thriller"
            },
            new Genre()
            {
                Id=2,
                Name="Comedy"
            },
            new Genre()
            {
                Id=3,
                Name="Documentary"
            }
        };
    }
}