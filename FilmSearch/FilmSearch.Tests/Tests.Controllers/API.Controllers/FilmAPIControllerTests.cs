using FilmSearch.Controllers.API;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Xunit;

namespace FilmSearch.Tests.Tests.Controllers.API.Controllers
{
    public class FilmAPIControllerTests
    {

        //NOT WORJING
        [Fact]
        public void AddFilmTest()
        {

            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            FilmService fs = new FilmService(uow.Object);
            FilmApiController FC = new FilmApiController(fs);

            uow.Setup(x => x.PersonRoleRepository.DeletePersonRolesByFilm(1));
            uow.Setup(x => x.FilmGenreRepository.DeleteFilmGenresByFilmId(1));
            uow.Setup(x => x.FilmAwardRepository.DeleteFilmAwardsByFilmId(1));
            uow.Setup(x => x.FilmRepository.Update(FilmModel.To(fm)));
            uow.Setup(x => x.Save());
            uow.Setup(x => x.GenreRepository.GenresByIds(It.IsAny<List<long>>())).Returns(fakeGenres);
            uow.Setup(x => x.PersonRepository.PersonsByIds(It.IsAny<List<long>>())).Returns(fakePeople);
            uow.Setup(x => x.PersonRepository.GetByKey(It.IsAny<long>())).Returns(fakePeople[0]);
            uow.Setup(x => x.AwardRepository.AwardsByIds(It.IsAny<List<long>>())).Returns(fakeAwards);
            uow.Setup(x => x.FilmRoleRepository.GetByRoleName(It.IsAny<string>())).Returns(fakeFilmRoles[0]);
            uow.Setup(x => x.FileRepository.GetByKey(1)).Returns(new File());

            var result = (FC.AddFilm(fm) as ObjectResult).Value as FilmModel;
            result.Should().NotBeNull();
                //var expected = FilmModel.To(fm);
                //Assert.Equal(expected, result);
            


        }

        [Fact]
        public void UpdateFilmTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            FilmService fs = new FilmService(uow.Object);
            FilmApiController FC = new FilmApiController(fs);
            uow.Setup(x => x.PersonRoleRepository.DeletePersonRolesByFilm(1));
            uow.Setup(x => x.FilmGenreRepository.DeleteFilmGenresByFilmId(1));
            uow.Setup(x => x.FilmAwardRepository.DeleteFilmAwardsByFilmId(1));
            uow.Setup(x => x.FilmRepository.Update(FilmModel.To(fm)));
            uow.Setup(x => x.Save());
            uow.Setup(x => x.GenreRepository.GenresByIds(It.IsAny<List<long>>())).Returns(fakeGenres);
            uow.Setup(x => x.PersonRepository.PersonsByIds(It.IsAny<List<long>>())).Returns(fakePeople);
            uow.Setup(x => x.PersonRepository.GetByKey(It.IsAny<long>())).Returns(fakePeople[0]);
            uow.Setup(x => x.AwardRepository.AwardsByIds(It.IsAny<List<long>>())).Returns(fakeAwards);
            uow.Setup(x => x.FilmRoleRepository.GetByRoleName(It.IsAny<string>())).Returns(fakeFilmRoles[0]);
            uow.Setup(x => x.FileRepository.GetByKey(1)).Returns(new File() { Id = 1 });
            var result = (FC.UpdateFilm(fm) as ObjectResult).Value as FilmModel;
            result.Should().NotBeNull();
            //var expected = fm;
            //Assert.Equal(expected, result);


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


            var result = FC.GetGenres(q, page) as ObjectResult;
            result.Value.Should().NotBeNull();

        }

        [Fact]
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
                Id = 1,
                Title = "title",
                Performance = 5
            });
            uow.Setup(x => x.PersonRoleRepository.DirectorByFilmId(id)).Returns(new Person() { Id = 1, Name = "name1" });
            FilmService fs = new FilmService(uow.Object);
            FilmApiController FC = new FilmApiController(fs);
            uow.Setup(x => x.FilmAwardRepository.GetAll()).Returns(new List<FilmAward>());

            var result = FC.GetFilm(id) as ObjectResult;
            result.Should().NotBeNull();
            result = FC.DeleteFilm(id) as ObjectResult;
            uow.Verify(x => x.FilmRepository.Delete((long)1));
            //IS NOT POSSIBLE
            //result = FC.RateFilm(id,rate) as ObjectResult;

        }
        [Fact]
        public void GetAwardsTest()
        {
            int page = 1;
            string q = "Name";
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.AwardRepository.AwardsByName(It.IsAny<string>())).Returns(fakeAwards);
            FilmService fs = new FilmService(uow.Object);
            FilmApiController FC = new FilmApiController(fs);
            var result = (FC.GetAwards(q, page)) as ObjectResult;
            result.Value.Should().NotBeNull();
        }
        [Fact]
        public void RateFilmTest()
        {
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.FilmPerformanceRepository.GetFilmPerformance(It.IsAny<long>(), It.IsAny<string>())).Returns(new FilmPerformance() { FilmId = 1, Id = 1, UserId = "1", Performance = 5 });
            uow.Setup(x => x.FilmPerformanceRepository.GetFilmPerformances(1)).Returns(fakePerfomances);
            uow.Setup(x => x.FilmRepository.GetByKey(It.IsAny<Object>())).Returns(fakeFilm);
            FilmService fs = new FilmService(uow.Object);
            FilmApiController FC = new FilmApiController(fs);
            FC.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "username")
            }, "someAuthTypeName"))
                }
            };
            var result = (FC.RateFilm(1, 5)) as ObjectResult;
            uow.Verify(x => x.Save());
            //result.Value.Should().NotBeNull();
        }
        List<FilmPerformance> fakePerfomances = new List<FilmPerformance>()
        {
            new FilmPerformance()
            {
                FilmId=1,
                Id=1,
                Performance=5,
                UserId="1"
            }
        };
        Film fakeFilm = new Film()
        {
            Id = 1,
            Title = "Name",
            Performance = 6
        };
        List<Award> fakeAwards = new List<Award>()
        {
            new Award()
            {
                Id=1,
                Name="Oscar"
            }
        };
        FilmModel fm = new FilmModel()
        {
            Id = 1,
            Title = "title",
            Director = new Person()
            {
                Id = 1,
                Name = "Person"
            },
            Playwriter = new Person()
            {
                Id = 1,
                Name = "Person"
            },
            Photo = new File() { Id=1},
            Performance=5,
            ShortDescription="Short Description",
            Actors = new List<Person>(),
            Genres = new List<Genre>(),
            Awards = new List<Award>(),
            ReleaseDate = "12/12/1990"//DateTime.ParseExact("06/15/2008", "d", CultureInfo.InvariantCulture).ToString().Replace('/','.')
        };
        List<Genre> fakeGenres = new List<Genre>()
        {
            new Genre()
            {
                Id=1,
                Name="Drama"
            }

        };
        List<Person> fakePeople = new List<Person>()
        {
            new Person()
            {
                Id=1,
                Name="Person",
                Country="USA"
            }
        };
        List<FilmRole> fakeFilmRoles = new List<FilmRole>()
        {
            new FilmRole()
            {
                Id=1,
                Name="DIRECTOR"
            }

        };
    }
}
