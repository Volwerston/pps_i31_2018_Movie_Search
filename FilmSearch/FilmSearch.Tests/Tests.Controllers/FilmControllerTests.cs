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
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
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
            var result = (FC.ShowFilmViews(FilmConstants.SortAsc, FilmConstants.SortTitle, name)
                as ViewResult).Model as SortedSearchResponse<FilmModel, FilmFilterQuery>;
            var real = result.Data.Select(x => FilmModel.To(x)).ToList();
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
            uow.Setup(x => x.FilmPerformanceRepository.GetAll()).Returns(perfomances);
            //int param = 1;
            //  string name = "Name";
            uow.Setup(x => x.PersonRoleRepository.DirectorByFilmId(id)).Returns(new Person() { Name = "Dir" });
            FilmService fs = new FilmService(uow.Object);

            FilmController FC = new FilmController(fs);

            ///////////////////////
            var username = "smth@gmail.com";
            var identity = new GenericIdentity(username);
            //create claim and add it to indentity
            var nameIdentifierClaim = new Claim(ClaimTypes.NameIdentifier, username);
            identity.AddClaim(nameIdentifierClaim);

            var user = new Mock<IPrincipal>();
            user.Setup(x => x.Identity).Returns(identity);
            Thread.CurrentPrincipal = user.Object;
            
            ///////////////////////

            var result = (FC.FilmView(id) as ViewResult).Model;
            result.Should().NotBeNull();
            //param++;
        }
        
        [Fact]
        public void CreateFilmView()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.FilmRepository.GetAll()).Returns(fakeFilms);
            FilmService fs = new FilmService(uow.Object);

            FilmController FC = new FilmController(fs);

            var result = FC.CreateFilmView() as ViewResult;
            result.Should().NotBeNull();
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
        static List<FilmPerformance> perfomances = new List<FilmPerformance>()
        {
            new FilmPerformance(){ Id=1,FilmId=1,Performance=5}
        };
    }
}
