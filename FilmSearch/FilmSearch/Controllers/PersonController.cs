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
        private IUnitOfWork _unitOfWork;
        private IHostingEnvironment enviroment;

        public PersonController(IUnitOfWork uow, IHostingEnvironment _env)
        {
            this._unitOfWork = uow;
            enviroment = _env;
        }

        public ActionResult List()
        {
            IEnumerable<Person> toList = _unitOfWork.PersonRepository.GetAll();

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
            _unitOfWork.FileRepository.Add(fileToAdd);

            Person personToAdd = new PersonViewModelConverter().Convert(p);
            personToAdd.PhotoId = fileToAdd.Id;
            _unitOfWork.PersonRepository.Add(personToAdd);

            string filePath = $"{enviroment.ContentRootPath}/storage/{fileToAdd.Id}";

            FileManager.Save(p.Photo, filePath);

            fileToAdd.Path = filePath;
            _unitOfWork.FileRepository.Update(fileToAdd);

            return RedirectToAction("List", "Person");
        }

        public ActionResult Delete(long id)
        {
            _unitOfWork.PersonRepository.Delete(id);

            string location = $"{enviroment.ContentRootPath}/storage/{id}";

            FileManager.RemoveDirectory(location);

            return RedirectToAction("List", "Person");
        }
    }
}