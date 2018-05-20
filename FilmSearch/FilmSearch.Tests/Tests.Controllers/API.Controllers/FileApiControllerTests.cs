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
        private const string TEST_PATH = "./test-files";

        private void CleanDirectory(DirectoryInfo directoryInfo)
        {
            foreach (var file in directoryInfo.GetFiles())
            {
                file.Delete();
            }

            foreach (var dir in directoryInfo.GetDirectories())
            {
                CleanDirectory(dir);
            }
            
            directoryInfo.Delete();
        }
        
        private void CleanTestDirectory()
        {
            System.IO.DirectoryInfo di = new DirectoryInfo(TEST_PATH);
            CleanDirectory(di);
        }
        
        [Fact]
        public void SaveFile()
        {
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.FileRepository.Add(It.IsAny<Models.File>()));
            env.Setup(x => x.ContentRootPath).Returns(TEST_PATH);
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

            var result = FC.SaveFile(file.Object) as ObjectResult;
            CleanTestDirectory();

            result.Should().NotBeNull();
        }
        
        [Fact]
        public void SaveFroalaImageTest()
        {
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            env.Setup(x => x.ContentRootPath).Returns(TEST_PATH);
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
            CleanTestDirectory();

            result.Should().NotBeNull();
        }

        [Fact(Skip = "Not working. Should be fixed")]
        public void GetFileTest()
        {
            long id = 1;
            Mock<IHostingEnvironment> env = new Mock<IHostingEnvironment>();
            Mock<IUnitOfWork> uow = new Mock<IUnitOfWork>();
            uow.Setup(x => x.FileRepository.GetByKey(id)).Returns(new Models.File() { Id = 1, FileName = "name", FileType = "jpg", Path = "D:\\storage\\", UploadDate = DateTime.Today });
            FileApiController FC = new FileApiController(env.Object, uow.Object);

            var result = FC.GetFile(id);
            result.Should().NotBeNull();
            CleanTestDirectory();
        }

    }
}
