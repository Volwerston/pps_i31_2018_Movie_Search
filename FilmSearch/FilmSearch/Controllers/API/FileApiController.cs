using System;
using System.Threading.Tasks;
using FilmSearch.DAL;
using FilmSearch.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FilmSearch.Controllers.API
{
    [Route("api/file")]
    public class FileApiController: Controller
    {
        private readonly IHostingEnvironment _enviroment;
        private readonly IUnitOfWork _unitOfWork;

        private string GetPath()
        {
            return $"{_enviroment.ContentRootPath}/storage/files";
        }
        
        private string GetPath(string fileName)
        {
            return $"{GetPath()}/{fileName}";
        }
        
        public FileApiController(IHostingEnvironment enviroment, IUnitOfWork unitOfWork)
        {
            _enviroment = enviroment;
            _unitOfWork = unitOfWork;
        }
        
        [HttpPost]
        public IActionResult SaveFile(IFormFile file)
        {
            return new ObjectResult(HandleFileSave(file));
        }

        [HttpPost("froala-image")]
        public IActionResult SaveFroalaImage(IFormFile file)
        {
            var fileData = HandleFileSave(file);
            
            return new ObjectResult(new
            {
                Link = $"/api/file/{fileData.Id}"
            });
        }
        
        [HttpGet("{id}")]
        public IActionResult GetFile(long id)
        {
            var fileData = _unitOfWork.FileRepository.GetByKey(id);

            return File(FileManager.Read(fileData.Path), fileData.FileType);
        }

        
        private File HandleFileSave(IFormFile file)
        {
            var fileData = new File
            {
                FileName = System.Guid.NewGuid() + "_" + file.FileName,
                FileType = file.ContentType,
                UploadDate = DateTime.Now
            };
            
            fileData.Path = GetPath(fileData.FileName);
            FileManager.Save(file, GetPath(), fileData.FileName);
            
            _unitOfWork.FileRepository.Add(fileData);
            _unitOfWork.Save();

            return fileData;
        }
       
    }
}