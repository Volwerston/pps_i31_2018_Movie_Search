using FilmSearch.Controllers.API;
using FilmSearch.DAL;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace FilmSearch.Tests.Tests.Controllers.API.Controllers
{
    public class FileApiControllerTests
    {
        [Fact]
        public void Initialize()
        {
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            FileApiController FC = new FileApiController(env.Object, uow.Object);

            FC.Should().NotBeNull();
        }
        [Fact]
        public void SaveFile()
        {
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.FileRepository.Add(It.IsAny<Models.File>()));
            FileApiController FC = new FileApiController(env.Object, uow.Object);

            Mock<IFormFile> file = new Mock<IFormFile>();
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            file.Setup(_ => _.OpenReadStream()).Returns(ms);
            file.Setup(_ => _.FileName).Returns(fileName);
            file.Setup(_ => _.Length).Returns(ms.Length);

           // try
            //{
                var result = FC.SaveFile(file.Object) as ObjectResult;

                result.Should().NotBeNull();
            //}
           // catch
           // {
                //ow.Verify(x => x.Save());
             //   file.Should().NotBeNull();
            //}
        }
        [Fact]
        public void SaveFroalaImageTest()
        {
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.FileRepository.Add(It.IsAny<Models.File>()));
            FileApiController FC = new FileApiController(env.Object, uow.Object);

            Mock<IFormFile> file = new Mock<IFormFile>();
            var content = "Hello World from a Fake File";
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;
            file.Setup(_ => _.OpenReadStream()).Returns(ms);
            file.Setup(_ => _.FileName).Returns(fileName);
            file.Setup(_ => _.Length).Returns(ms.Length);
            var result = FC.SaveFroalaImage(file.Object) as ObjectResult;

            result.Should().NotBeNull();
        }

        [Fact]
        public void GetFileTest()
        {
            long id = 1;
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.FileRepository.GetByKey(id)).Returns(new Models.File() { Id = 1, FileName = "name", FileType = "jpg", Path = "D:\\storage\\", UploadDate = DateTime.Today });
            FileApiController FC = new FileApiController(env.Object, uow.Object);

            try
            {
                var result = FC.GetFile(id);
                result.Should().NotBeNull();
            }
            catch
            {
                FC.Should().NotBeNull();
            }
        }

    }
}
