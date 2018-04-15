using FilmSearch.Controllers;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace FilmSearch.Tests.Tests.Controllers
{
    public class FilmControllerTests
    {
        [Fact]
        public void Initialize()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.FilmGenreRepository);
            FilmService fs = new FilmService(uow.Object);

            FilmController FC = new FilmController(fs);

            FC.Should().NotBeNull();
        }
        [Fact]
        public void ShowFilmViews()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.FilmRepository.GetAll()).Returns(fakeFilms);
            int param = 1;
            string name = "Name";
            uow.Setup(x => x.PersonRoleRepository.DirectorByFilmId(param)).Returns(new Person() { Name = "Dir" });
            FilmService fs = new FilmService(uow.Object);

            FilmController FC = new FilmController(fs);
            var result = FC.ShowFilmViews(FilmConstants.SortAsc, FilmConstants.SortTitle, name) as ViewResult;
            //var smth = result.ViewData;
            var read = result.Model as SortedSearchResponse<FilmModel,FilmFilterQuery>;
            var real = read.Data.Select(x => FilmModel.To(x)).ToList();
            var expected = fakeFilms.Where(x => x.Title.Contains(name)).ToList();
            Assert.Equal(expected[0].Title, real[0].Title);

        }
        //BREAKS ON GetUserId()
        [Fact]
        public void FilmView()
        {
            long id = 1;
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.FilmRepository.GetAll()).Returns(fakeFilms);
            uow.Setup(x => x.FilmRepository.GetByKey(id)).Returns(fakeFilms.Where(x => x.Id == id).FirstOrDefault());
            //int param = 1;
          //  string name = "Name";
            uow.Setup(x => x.PersonRoleRepository.DirectorByFilmId(id)).Returns(new Person() { Name = "Dir" });
            FilmService fs = new FilmService(uow.Object);

            FilmController FC = new FilmController(fs);
            
            var result = (FC.FilmView(id) as ViewResult).Model;
            Console.ReadKey();
            //param++;
        }
        //Literally can't do something
        [Fact]
        public void CreateFilmView()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.FilmRepository.GetAll()).Returns(fakeFilms);
            FilmService fs = new FilmService(uow.Object);

            FilmController FC = new FilmController(fs);

            var result = FC.CreateFilmView() as ViewResult;
            result.Should().NotBeNull();
           //uow.Verify(x => x.FilmRepository.Add(It.IsAny<Film>()));
        }
        static List<Film> fakeFilms = new List<Film>()
        {
            new Film()
            {
                Title = "Name",
                Id = 1,
                PhotoId = 1,
                Performance = 5,
                ReleaseDate = DateTime.Now,
                ShortDescription = "",
                Genres =new List<FilmGenre>()
                {
                   new FilmGenre()
                   {
                       Genre = new Genre()
                       {
                           Id = 1,
                           Name="Thriller"
                       }
                   }
                }
            },
            new Film()
            {
                Title = "Another one!",
                Id = 2,
                PhotoId = 2,
                Performance = 6,
                ReleaseDate = DateTime.Today,
                ShortDescription = "",
                 Genres =new List<FilmGenre>()
                {
                    
                }
            }
        };
    }
}
