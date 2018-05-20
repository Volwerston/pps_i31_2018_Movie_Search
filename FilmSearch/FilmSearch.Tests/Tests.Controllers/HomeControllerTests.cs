using Castle.Core.Logging;
using FilmSearch.Controllers;
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
            result.Should().BeNull();
        }
        
        [Fact(Skip = "Doesn't check anything. Should be either fixed or removed (more preferable)")]
        public void Error()
        {
            Mock<ILogger<HomeController>> logger = new Mock<ILogger<HomeController>>();
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            HomeController HC = new HomeController(logger.Object, env.Object);
            //HC.HttpContext.Features.Set<IExceptionHandlerPathFeature>(null);
            //HC.ControllerContext.HttpContext = new HttpContext();
            //HC.ControllerContext = new ControllerContext() { HttpContext = new HttpContext() };// (context.Object, new RouteData(), controller);
            //
            //HC.ControllerContext = new ControllerContext();
            //HC.ControllerContext.HttpContext = new DefaultHttpContext();

            //var result = HC.Error() as ViewResult;
            //result.Should().NotBeNull();
            HC.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
                      {
                                new Claim(ClaimTypes.Name, "username")
                      }, "someAuthTypeName"))
                }
            };
            HC.ControllerContext.HttpContext.Request.Headers["device-id"] = "20317";
            HC.HttpContext.Features.Set(new FakeExceptionHandlerPathFeature() as IExceptionHandlerPathFeature);
            var b = HC.HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewResult result;
            bool toRet = true;
            try
            {
                result = HC.Error() as ViewResult;
            }
            catch
            {
                toRet = true;
            }

            Assert.True(toRet);

        }
        
    }
    class FakeExceptionHandlerPathFeature : IExceptionHandlerPathFeature
    {
        public string Path => "path";
        public string ConnectionId = "test";
        public Exception Error => new Exception("cup", new Exception("inner"));
        public FakeExceptionHandlerPathFeature() : base() { }
    }
   
}
