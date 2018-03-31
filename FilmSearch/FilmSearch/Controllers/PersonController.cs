using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Models.View;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Mvc;

namespace FilmSearch.Controllers
{
    public class PersonController : Controller
    {
        private IPersonRepository personRepository;
        private IFileRepository fileRepository;
        private IHostingEnvironment enviroment;

        public PersonController(IPersonRepository _personRepo, IFileRepository _fileRepo, IHostingEnvironment _env)
        {
            this.personRepository = _personRepo;
            this.fileRepository = _fileRepo;
            enviroment = _env;
        }

        public ActionResult List()
        {
            IEnumerable<Person> toList = personRepository.GetAll();

            return View(toList);
        }

        public ActionResult Create()
        {
            PersonViewModel newOne = new PersonViewModel();

            return View(newOne);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PersonViewModel p)
        {
            File fileToAdd = new FileViewModelConverter().Convert(p.Photo);
            fileRepository.Add(fileToAdd);

            Person personToAdd = new PersonViewModelConverter().Convert(p);
            personToAdd.PhotoId = fileToAdd.Id;
            personRepository.Add(personToAdd);

            string filePath = $"{enviroment.ContentRootPath}/storage/{fileToAdd.Id}";

            FileManager.Save(p.Photo, filePath);

            fileToAdd.Path = filePath;
            fileRepository.Update(fileToAdd);

            return RedirectToAction("List", "Person");
        }
    }
}