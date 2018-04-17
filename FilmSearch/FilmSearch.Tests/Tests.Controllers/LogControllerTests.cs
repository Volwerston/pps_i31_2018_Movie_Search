using FilmSearch.Controllers;
using FilmSearch.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FilmSearch.Tests.Tests.Controllers
{
    public class LogControllerTests
    {
        [Fact]
        public void Initialize()
        {
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            LogController LC = new LogController(env.Object);
            LC.Should().NotBeNull();
        }

        [Fact]
        public void ViewTest()
        {
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            LogController LC = new LogController(env.Object);

            var result = (LC.View() as ViewResult).Model as IEnumerable<LogEntry>;

            result.Should().NotBeNull();
        }
        [Fact]
        public void GetEntriesByDateTest()
        {
            var date = DateTime.Today;
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            LogController LC = new LogController(env.Object);
            var result = (LC.GetEntriesByDate(date) as JsonResult).Value;
            result.Should().NotBeNull();
        }
    }
}
