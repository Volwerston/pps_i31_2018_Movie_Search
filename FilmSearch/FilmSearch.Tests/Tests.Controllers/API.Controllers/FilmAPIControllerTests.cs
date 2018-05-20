using FilmSearch.Controllers.API;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xunit;

namespace FilmSearch.Tests.Tests.Controllers.API.Controllers
{
    public class FilmAPIControllerTests
    {
        [Fact]
        public void Initialize()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            FilmService fs = new FilmService(uow.Object);
            FilmApiController FC = new FilmApiController(fs);

            FC.Should().NotBeNull();
        }
        //NOT WORJING
        [Fact(Skip = "Doesn't check anything. Should be either removed or fixed")]
        public void AddFilmTest()
        {
           
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            FilmService fs = new FilmService(uow.Object);
            FilmApiController FC = new FilmApiController(fs);

            try
            {
                var result = (FC.AddFilm(fm) as ObjectResult).Value as Film;
                var expected = FilmModel.To(fm);
                Assert.Equal(expected, result);
            }
            catch
            {
                Assert.True(true);
            }


        }
        
        [Fact(Skip = "Doesn't check anything. Should be either removed or fixed")]
        public void UpdateFilmTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            FilmService fs = new FilmService(uow.Object);
            FilmApiController FC = new FilmApiController(fs);
            try
            {
                var result = (FC.UpdateFilm(fm) as ObjectResult).Value as Film;
                var expected = FilmModel.To(fm);
                Assert.Equal(expected, result);
            }
            catch
            {
                Assert.True(true);
            }

        }
        [Fact]
        public void GetGenresTest()
        {
            string q = "drama";
            int page = 1;
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.GenreRepository.GenresByName(q)).Returns(new List<Genre>()
            {
                new Genre()
                {
                    Id=1,
                    Name="genre1"
                }
            });
            FilmService fs = new FilmService(uow.Object);
            FilmApiController FC = new FilmApiController(fs);

            
            var result = FC.GetGenres(q,page) as ObjectResult;
            result.Value.Should().NotBeNull();

        }

        [Fact(Skip = "Fails. Should be fixed")]
        public void CRUDFilmTest()
        {
            string q = "drama";
            int page = 1;
            long id = 1;
            long rate = 5;
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.GenreRepository.GenresByName(q)).Returns(new List<Genre>()
            {
                new Genre()
                {
                    Id=1,
                    Name="genre1"
                }
            });
            uow.Setup(x => x.FilmRepository.GetByKey(id)).Returns(new Film()
            {
                Id=1,
                Title="title",
                Performance=5
            });
            uow.Setup(x => x.PersonRoleRepository.DirectorByFilmId(id)).Returns(new Person() { Id=1,Name="name1"});
            FilmService fs = new FilmService(uow.Object);
            FilmApiController FC = new FilmApiController(fs);


            var result = FC.GetFilm(id) as ObjectResult;
            result = FC.DeleteFilm(id) as ObjectResult;
            //IS NOT POSSIBLE
            //result = FC.RateFilm(id,rate) as ObjectResult;

        }
        FilmModel fm = new FilmModel()
        {
            Id = 1,
            Title = "title",
            Director = new Person(),
            Actors = new List<Person>(),
            Genres = new List<Genre>(),
            ReleaseDate = DateTime.ParseExact("06/15/2008", "d", CultureInfo.InvariantCulture).ToString().Replace('.','/')
        };
    }
}
