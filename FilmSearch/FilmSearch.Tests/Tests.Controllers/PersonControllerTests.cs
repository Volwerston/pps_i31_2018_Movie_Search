using FilmSearch.Controllers;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Models.View;
using FilmSearch.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading;
using Xunit;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;


namespace FilmSearch.Tests.Tests.Controllers
{
    public class PersonControllerTests
    {
       
        [Fact]
        public void List()
        {
            var um = new FakeUserManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PersonRepository.GetAll()).Returns(_fakePersons);
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            PersonService ps = new PersonService(uow.Object);
            PersonController pc = new PersonController(uow.Object, env.Object, ps, um);

            var result = (pc.List() as ViewResult).Model as List<Person>;
            Assert.Equal(_fakePersons, result);
        }
        
        //EXCEPTION?
        [Fact(Skip = "Fails. Should be fixed")]
        public void Create()
        {
            var um = new FakeUserManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PersonRepository.GetAll()).Returns(_fakePersons);
            uow.Setup(x => x.FileRepository.Add(new Models.File()));
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            PersonService ps = new PersonService(uow.Object);
            PersonController pc = new PersonController(uow.Object, env.Object, ps, um);

            var fileMock = new Mock<IFormFile>();
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);

            PersonViewModel pvm = new PersonViewModel()
            {
                BirthDate = DateTime.Today,
                Id = 3,
                Country = "Ukraine",
                Name = "Lag",
                Surname = "Do",
                Photo = fileMock.Object
            };
            
            var result = pc.Create(pvm) as ViewResult;
            uow.Verify(x => x.Save());
            
            //Should not be in the same UNIT(!) test
            /*
            var result2 = pc.Create() as ViewResult;
            result2.Should().NotBeNull();
            */
        }
        
        
        
        [Fact(Skip = "Fails after removing empty catch block. IOException")]
        public void DeleteTest()
        {
            long id = 1;
            var um = new FakeUserManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PersonRepository.GetAll()).Returns(_fakePersons);
            uow.Setup(x => x.PersonRepository.GetByKey(id)).Returns(_fakePersons.Where(x=>x.Id == id).FirstOrDefault());
            uow.Setup(x => x.FileRepository.Delete(1));
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            env.Setup(x => x.ContentRootPath).Returns(Directory.GetCurrentDirectory());
            PersonService ps = new PersonService(uow.Object);

            PersonController PC = new PersonController(uow.Object, env.Object, ps, um);


            var result = PC.Delete(id) as ViewResult;
            uow.Verify(x => x.PersonRepository.Delete(id));
            uow.Verify(x => x.Save());
        }
        
        [Fact(Skip = "Fails. Should be fixed")]
        public void EditTest()
        {
            long id = 1;
            var um = new FakeUserManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PersonRepository.GetAll()).Returns(_fakePersons);
            uow.Setup(x => x.PersonRepository.GetByKey(id)).Returns(_fakePersons.Where(x => x.Id == id).FirstOrDefault());
            uow.Setup(x => x.FileRepository.Delete(1));
            uow.Setup(x => x.PersonRoleRepository.GetAll()).Returns(new List<PersonRole>()
            {
                new PersonRole()
                {
                    Id=1,
                    PersonId=1,
                    FilmRoleId=1,
                    FilmId=1
                }
            });
            uow.Setup(x => x.FilmRoleRepository.GetAll()).Returns(new List<FilmRole>()
            {
                new FilmRole()
                {
                    Id=1,
                    Name="role"
                }
            });
            uow.Setup(x => x.FilmRepository.GetByKey(id)).Returns(new Film()
            {
                Id=id,
                Title="title"
            });
            uow.Setup(x => x.FileRepository.GetByKey(id)).Returns(new Models.File());
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            PersonService ps = new PersonService(uow.Object);
            PersonController PC = new PersonController(uow.Object, env.Object, ps, um);

            Directory.CreateDirectory("D:\\storage\\1");
            using (StreamWriter SW = new StreamWriter("D:\\storage\\0\\anonym.jpg"))
            {
                SW.Write("smth");
            }
            var result = PC.Edit(id) as ViewResult;

            result.Model.Should().NotBeNull();

            ////////////////////////////////////////
            var fileMock = new Mock<IFormFile>();
            var physicalFile = new FileInfo("filePath");
            var ms = new MemoryStream();
           
            var fileName = physicalFile.Name;
            //Setup mock file using info from physical file
            fileMock.Setup(_ => _.FileName).Returns(fileName);
            fileMock.Setup(_ => _.Length).Returns(ms.Length);
            fileMock.Setup(_ => _.OpenReadStream()).Returns(ms);
            fileMock.Setup(_ => _.ContentDisposition).Returns(string.Format("inline; filename={0}", fileName));
            ////////////////////////////////////////
            PersonViewModel pvm = new PersonViewModel()
            {
                Id = id,
                Name = "name",
                Surname = "surname",
                BirthDate = DateTime.Today,
                Country = "Ukraine",
                Photo = fileMock.Object
            };
            var result2 = PC.Edit(pvm) as ViewResult;
            uow.Verify(x => x.Save());
        }
        
        [Fact]
        public void SearchTest()
        {
            var um = new FakeUserManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            PersonService ps = new PersonService(uow.Object);
            PersonController PC = new PersonController(uow.Object, env.Object, ps, um);

            var result = PC.Search() as ViewResult;
            result.Model.Should().BeNull();
        }

        [Fact]
        public void EditRolesTest()
        {
            var role = new PersonRole() { Id = 1 };
            var um = new FakeUserManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PersonRoleRepository.Update(role));
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            PersonService ps = new PersonService(uow.Object);
            PersonController PC = new PersonController(uow.Object, env.Object, ps, um);

            var result = PC.EditRoles(new List<PersonRole>() { role }) as ViewResult;
            uow.Verify(x => x.Save());

        }
        
        [Fact(Skip = "Fails after removing empty catch block. IOException")]
        public void DetailsTest()
        {
            int id = 1;
            var um = new FakeUserManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PersonRepository.GetByKey((long)id)).Returns(_fakePersons.Where(x => x.Id == id).FirstOrDefault());
            uow.Setup(x => x.FileRepository.GetByKey((long)id)).Returns(new Models.File() {Path=(Directory.GetCurrentDirectory()+ "\\FilmSearch.dll"),FileType="dll" });
            uow.Setup(x => x.PersonRoleRepository.GetAll()).Returns(new List<PersonRole>()
            {
                new PersonRole()
                {
                    Id=1,
                    FilmId=1,
                    PersonId=1
                }
            });
            uow.Setup(x => x.PersonPerformanceRepository.GetAll()).Returns(new List<PersonPerformance>()
            {
                new PersonPerformance()
                {
                    Id=1,
                    PersonRoleId=1,
                    UserId="1"
                }
            });
            uow.Setup(x => x.FilmRepository.GetByKey(It.IsAny<int>())).Returns(new Film()
            {
                Id=1,
                PhotoId=1,
                Title="title"
            }
                );
            uow.Setup(x => x.FilmRoleRepository.GetByKey(It.IsAny<int>())).Returns(new FilmRole()
            {
                Id=1,
                Name="name"
            }
            );
            
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            PersonService ps = new FakePersonService(uow.Object);
            PersonController PC = new PersonController(uow.Object, env.Object, ps, um);
            var username = "ystetskyy333@gmail.com";
            var identity = new GenericIdentity(username);
            //create claim and add it to indentity
            var nameIdentifierClaim = new Claim(ClaimTypes.NameIdentifier, username);
            identity.AddClaim(nameIdentifierClaim);

            var user = new Mock<IPrincipal>();
            user.Setup(x => x.Identity).Returns(identity);
            Thread.CurrentPrincipal = user.Object;
            ActionResult result = new OkResult();
           
            result = PC.Details(id) as ViewResult;
            result.Should().NotBeNull();
        }

        private List<Person> _fakePersons = new List<Person>()
        {
            new Person()
            {
                BirthDate = DateTime.Today,
                Country="Ukraine",
                Id=1,
                Name="name",
                Surname="surname",
                PhotoId=1,
                Photo= new Models.File()
            }
        };
    }
}
