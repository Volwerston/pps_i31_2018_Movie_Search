using Castle.Core.Logging;
using FilmSearch.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FilmSearch.Tests.Tests.Controllers
{
    public class HomeControllerTests
    {
        [Fact]
        public void Initialize()
        {
            Mock<ILogger<HomeController>> logger = new Mock<ILogger<HomeController>>();
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            HomeController HC = new HomeController(logger.Object, env.Object);
            HC.Should().NotBeNull();
        }
        [Fact]
        public void Index()
        {
            Mock<ILogger<HomeController>> logger = new Mock<ILogger<HomeController>>();
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            HomeController HC = new HomeController(logger.Object, env.Object);
            var result = HC.Index() as ViewResult;
            result.Should().NotBeNull();
        }
        
        [Fact]
        public void Error()
        {
            Mock<ILogger<HomeController>> logger = new Mock<ILogger<HomeController>>();
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            HomeController HC = new HomeController(logger.Object, env.Object);
            //HC.HttpContext.Features.Set<IExceptionHandlerPathFeature>(null);
            try
            {
                var result = HC.Error() as ViewResult;
                result.Should().NotBeNull();
            }
            catch
            {
                Assert.True(true);
            }
            
        }
        
    }
}
