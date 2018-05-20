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
    public class AwardControllerTests
    {
        [Fact]
        public void IndexTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.AwardRepository.GetAll()).Returns(fakeAwards);
            AwardController AC = new AwardController(uow.Object);
            var result = (AC.Index()) as ViewResult;

            result.Model.Should().NotBeNull();
        }

        [Fact]
        public void RemoveTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.AwardRepository.GetAll()).Returns(fakeAwards);
            AwardController GC = new AwardController(uow.Object);
            var result = (GC.Remove(1)) as OkResult;
            uow.Verify(x => x.AwardRepository.Delete(It.IsAny<Object>()));
            uow.Verify(x => x.Save());
            Assert.Equal(200, result.StatusCode);
            result = (GC.Remove(100)) as OkResult;


        }
        [Fact]
        public void AddTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.AwardRepository.GetAll()).Returns(fakeAwards);
            AwardController GC = new AwardController(uow.Object);

            var result = (GC.Add(new Award() { Id = 5, Name = "Granny" })) as StatusCodeResult;
            Assert.Equal(200, result.StatusCode);
            var result2 = (GC.Add(fakeAwards[0])) as ObjectResult;
            Assert.Equal(500, result2.StatusCode);
        }
        List<Award> fakeAwards = new List<Award>()
        {
            new Award()
            {
                Id=1,
                Name="Oscar"
            },
            new Award()
            {
                Id = 2,
                Name="Saturn"
            }

        };
    }
}