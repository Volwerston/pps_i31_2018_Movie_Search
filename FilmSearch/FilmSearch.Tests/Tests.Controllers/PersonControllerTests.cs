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
using System.Text;
using Xunit;

namespace FilmSearch.Tests.Tests.Controllers
{
    public class PersonControllerTests
    {
        [Fact]
        public void Initialize()
        {
            var um = new FakeUserManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            PersonService ps = new PersonService(uow.Object);
            PersonController PC = new PersonController(uow.Object, env.Object, ps, um);

            PC.Should().NotBeNull();
        }
        [Fact]
        public void List()
        {
            var um = new FakeUserManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PersonRepository.GetAll()).Returns(fakePerson);
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            PersonService ps = new PersonService(uow.Object);
            PersonController PC = new PersonController(uow.Object, env.Object, ps, um);

            var result = (PC.List() as ViewResult).Model as List<Person>;
            Assert.Equal(fakePerson, result);
        }
        //EXCEPTION?
        [Fact]
        public void Create()
        {
            var um = new FakeUserManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PersonRepository.GetAll()).Returns(fakePerson);
            uow.Setup(x => x.FileRepository.Add(new Models.File()));
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            PersonService ps = new PersonService(uow.Object);
            PersonController PC = new PersonController(uow.Object, env.Object, ps, um);

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

            var result = (PC.Create(pvm) as ViewResult);
            uow.Verify(x => x.Save());
        }
        [Fact]
        public void DeleteTest()
        {
            long id = 1;
            var um = new FakeUserManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PersonRepository.GetAll()).Returns(fakePerson);
            uow.Setup(x => x.PersonRepository.GetByKey(id)).Returns(fakePerson.Where(x=>x.Id == id).FirstOrDefault());
            uow.Setup(x => x.FileRepository.Delete(1));
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            PersonService ps = new PersonService(uow.Object);
            PersonController PC = new PersonController(uow.Object, env.Object, ps, um);

            Directory.CreateDirectory("D:\\storage\\1");

            var result = PC.Delete(id) as ViewResult;
            
            uow.Verify(x => x.PersonRepository.Delete(id));
            uow.Verify(x => x.Save());
        }
        [Fact]
        public void EditTest()
        {
            long id = 1;
            var um = new FakeUserManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PersonRepository.GetAll()).Returns(fakePerson);
            uow.Setup(x => x.PersonRepository.GetByKey(id)).Returns(fakePerson.Where(x => x.Id == id).FirstOrDefault());
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
           
            using (StreamWriter SW = new StreamWriter(@"D:\net-core_1\net-core\FilmSearch\FilmSearch.Tests\bin\Debug\netcoreapp2.0\filePath"))
            {
                SW.Write("smth");
            }
            var writer = new StreamWriter(ms);
            writer.Write(physicalFile.OpenRead());
            //D:\repos\night\pps_i31_2018_Movie_Search\FilmSearch\FilmSearch.Tests\bin\Debug\netcoreapp2.0\filePath
            writer.Flush();
            ms.Position = 0;
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
        /*
        [Fact]
        public void DetailsTest()
        {
            int id = 1;
            var um = new FakeUserManager();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.PersonRepository.GetByKey((long)id)).Returns(fakePerson.Where(x => x.Id == id).FirstOrDefault());
            uow.Setup(x => x.FileRepository.GetByKey((long)id)).Returns(new Models.File() {Path="D:\\storage",FileType="jpg" });
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            PersonService ps = new PersonService(uow.Object);
            PersonController PC = new PersonController(uow.Object, env.Object, ps, um);

            var result = PC.Details(id) as ViewResult;
            result.Should().NotBeNull();
        }*/
        List<Person> fakePerson = new List<Person>()
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
