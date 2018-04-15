using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Services;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace FilmSearch.Tests.Tests.Services
{
    public class FilmServiceTests
    {
        [Fact]
        public void Initialize()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            FilmService FS = new FilmService(uow.Object);
            FS.Should().NotBeNull();
        }
        [Fact]
        public void GetFilmsTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            FilmService FS = new FilmService(uow.Object);
            uow.Setup(x => x.FilmRepository.GetAll()).Returns(fakeFilms);
            List<Film> result = FS.GetFilms(sortQuery, filterQuery).ToList();
            Assert.Equal(fakeFilms.OrderBy(x => x.Title).ToList(), result);
        }
        [Fact]
        public void GetFilmTest()
        {
            long id = 1;
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            FilmService FS = new FilmService(uow.Object);
            uow.Setup(x => x.FilmRepository.GetAll()).Returns(fakeFilms);
            uow.Setup(x => x.FilmRepository.GetByKey(id)).Returns(fakeFilms.Where(x => x.Id == id).FirstOrDefault());
            Film result = FS.GetFilm(id);
            Assert.Equal(fakeFilms.Where(x=>x.Id == id).FirstOrDefault(), result);
            id = 3;
            result = FS.GetFilm(id);
            result.Should().BeNull();
        }
        List<Film> fakeFilms = new List<Film>()
        {
            new Film()
            {
                Id=1, Performance=5,Title="Title",Photo=new File(),PhotoId=1,ReleaseDate=DateTime.Today,ShortDescription="",Genres = new List<FilmGenre>(){ new FilmGenre() { FilmId=1,GenreId=1} }
            },
            new Film()
            {
                Id=2, Performance=5,Title="Another",Photo=new File(),PhotoId=1,ReleaseDate=DateTime.Today,ShortDescription="",Genres = new List<FilmGenre>(){ new FilmGenre() { FilmId=2,GenreId=2} }
            }
        };
        SortQuery sortQuery = new SortQuery
        {
            Order = FilmConstants.SortAsc,
            Value = FilmConstants.SortTitle
        };

        FilmFilterQuery filterQuery = new FilmFilterQuery
        {
            Title = String.Empty
        };
    }
}
