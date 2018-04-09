using FilmSearch.Controllers;
using FilmSearch.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace FilmSearch.Tests
{
    public class FilmControllerTest
    {
        [Fact]
        public void InitializationTest()
        {
            Mock<IFilmService> mock = new Mock<IFilmService>();
            FilmController FC = new FilmController(mock.Object);
            FC.Should().NotBeNull();
        }
        [Fact]
        public void CreateFilmViewTest()
        {
            Mock<IFilmService> mock = new Mock<IFilmService>();
            FilmController FC = new FilmController(mock.Object);
            // FC.ControllerContext = new ControllerContext();
            //FC.ControllerContext.HttpContext = new DefaultHttpContext();
            try
            {
                var viewResult = FC.CreateFilmView() as ViewResult;
            }
            catch
            {
                Assert.True(false);
                return;
            }
            Assert.True(true);
        }
        [Fact]
        public void ShowFilmViewsTest()
        {
            Mock<IFilmService> mock = new Mock<IFilmService>();
            FilmController FC = new FilmController(mock.Object);
            //FC.ControllerContext = new ControllerContext();
            //FC.ControllerContext.HttpContext = new DefaultHttpContext();
            try
            {
                IActionResult result1 = FC.ShowFilmViews(FilmConstants.SortAsc, FilmConstants.SortTitle, "Title");
                var viewResult1 = result1 as ViewResult;
                IActionResult result2 = FC.ShowFilmViews(FilmConstants.SortDesc, FilmConstants.SortDate, "Date");
                var viewResult2 = result2 as ViewResult;
            }
            catch
            {
                Assert.True(false);
                return;
            }
            Assert.True(true);

        }
        [Fact]
        public void FilmViewTest()
        {
            Mock<IFilmService> mock = new Mock<IFilmService>();
            FilmController FC = new FilmController(mock.Object);
            int id = 1;
            try
            {
                var result = FC.FilmView(id);
                var viewResult = result as ViewResult;
            }
            catch
            {
                Assert.True(true);
                return;
            }
            Assert.True(false);
            //Assert.Equal(200, viewResult.StatusCode);
        }
    }
}
