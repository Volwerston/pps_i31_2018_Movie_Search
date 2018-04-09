using FilmSearch.Controllers;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FilmSearch.Tests
{
    public class LogControllerTest
    {
        [Fact]
        public void InitializeTest()
        {
            Mock<IHostingEnvironment> mock = new Mock<IHostingEnvironment>();
            LogController LC = new LogController(mock.Object);
            LC.Should().NotBeNull();
        }
        [Fact]
        public void ViewTest()
        {
            Mock<IHostingEnvironment> mock = new Mock<IHostingEnvironment>();
            LogController LC = new LogController(mock.Object);
            try
            {
                var viewResult = LC.View() as ViewResult;
            }
            catch
            {
                Assert.True(false);
                return;
            }
            Assert.True(true);

        }
        [Fact]
        public void GetEntriesByDateTest()
        {
            Mock<IHostingEnvironment> mock = new Mock<IHostingEnvironment>();
            LogController LC = new LogController(mock.Object);
            DateTime date = DateTime.Now;
            try
            {
                var viewResult = LC.GetEntriesByDate(date) as JsonResult;
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
