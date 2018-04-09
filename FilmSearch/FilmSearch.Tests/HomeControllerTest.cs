using FilmSearch.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FilmSearch.Tests
{
    public class HomeControllerTest
    {
        [Fact]
        public void InitializeTest()
        {
            Mock<ILogger<HomeController>> logger = new Mock<ILogger<HomeController>>();
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();

            HomeController HC = new HomeController(logger.Object, env.Object);

            HC.Should().NotBeNull();
        }
        [Fact]
        public void IndexTest()
        {
            Mock<ILogger<HomeController>> logger = new Mock<ILogger<HomeController>>();
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();

            HomeController HC = new HomeController(logger.Object, env.Object);
            try
            {
                var result = HC.Index() as ViewResult;
            }
            catch
            {
                Assert.True(false);
                return;
            }
            Assert.True(true);
        }
    }
}
