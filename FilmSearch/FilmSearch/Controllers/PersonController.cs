﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Models.View;
using FilmSearch.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FilmSearch.Controllers
{
    public class PersonController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private IHostingEnvironment enviroment;
        private PersonService personService;
        private UserManager<AppUser> _userManager;

       private string GetUserId() => this.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        public PersonController(IUnitOfWork uow, IHostingEnvironment _env, PersonService _personService, UserManager<AppUser> userMgr)
        {
            _unitOfWork = uow;
            enviroment = _env;
            personService = _personService;
            _userManager = userMgr;
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult List()
        {
            IEnumerable<Person> toList = _unitOfWork.PersonRepository.GetAll();

            return View(toList);
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Create()
        {
            PersonViewModel newOne = new PersonViewModel();

            return View(newOne);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Create(PersonViewModel p)
        {
            if (!ModelState.IsValid)
            {
                return View(p);
            }

            File fileToAdd = new FileViewModelConverter().Convert(p.Photo);
            _unitOfWork.FileRepository.Add(fileToAdd);
            _unitOfWork.Save();

            Person personToAdd = new PersonViewModelConverter().Convert(p);
            personToAdd.PhotoId = fileToAdd.Id;

            _unitOfWork.PersonRepository.Add(personToAdd);
            _unitOfWork.Save();

            string dirPath = $"{enviroment.ContentRootPath}/storage/{fileToAdd.Id}";

            if (p.Photo != null)
            {
                FileManager.Save(p.Photo, dirPath);
            }

            fileToAdd.Path = $"{dirPath}/{fileToAdd.FileName}";

            _unitOfWork.FileRepository.Update(fileToAdd);
            _unitOfWork.Save();

            return RedirectToAction("List", "Person");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Delete(long id)
        {
            Person toDelete = _unitOfWork.PersonRepository.GetByKey(id);
            long? imgId = toDelete.PhotoId;

            _unitOfWork.PersonRepository.Delete(id);
            _unitOfWork.FileRepository.Delete(imgId);

            _unitOfWork.Save();

            string location = $"{enviroment.ContentRootPath}/storage/{imgId}";

            FileManager.RemoveDirectory(location);

            return RedirectToAction("List", "Person");
        }

        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(long id)
        {
            Person toEdit = _unitOfWork.PersonRepository.GetByKey(id);

            File photoFile = _unitOfWork.FileRepository.GetByKey(toEdit.PhotoId) ?? new Models.File();

            PersonViewModel toPass = new PersonViewModelConverter().ConvertToViewModel(toEdit);

            string photoSource = string.IsNullOrWhiteSpace(photoFile.Path)
                ? $"{enviroment.ContentRootPath}/storage/0/anonym.jpg"
                : photoFile.Path;

            string base64Image = FileManager.GetBase64File(photoSource);

            string imgSrc = $"data:{photoFile.FileType ?? "image/jpg"};base64, {base64Image}";
            ViewBag.LogoSrc = imgSrc;

            IEnumerable<PersonRole> roles = _unitOfWork.PersonRoleRepository.GetAll().Where(x => x.PersonId == id);

            foreach(var role in roles)
            {
                role.Person = _unitOfWork.PersonRepository.GetByKey(role.PersonId);
                role.FilmRole = _unitOfWork.FilmRoleRepository.GetByKey(role.FilmRoleId);
                role.Film = _unitOfWork.FilmRepository.GetByKey(role.FilmId);
            }

            ViewBag.Roles = roles;

            return View(toPass);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrator")]
        public ActionResult Edit(PersonViewModel model)
        {
            Person  personToUpdate = _unitOfWork.PersonRepository.GetByKey(model.Id);
            new PersonViewModelConverter().ConvertExisting(model, personToUpdate);

            if (model.Photo != null)
            {
                File fileToUpdate = _unitOfWork.FileRepository.GetByKey(personToUpdate.PhotoId);
                new FileViewModelConverter().ConvertExisting(model.Photo, fileToUpdate);

                string dirPath = $"{enviroment.ContentRootPath}/storage/{fileToUpdate.Id}";

                FileManager.RemoveDirectory(dirPath);
                FileManager.Save(model.Photo, dirPath);

                fileToUpdate.Path = $"{dirPath}/{fileToUpdate.FileName}";

                _unitOfWork.FileRepository.Update(fileToUpdate);

                personToUpdate.PhotoId = fileToUpdate.Id;
            }

            _unitOfWork.PersonRepository.Update(personToUpdate);
            _unitOfWork.Save();

            return RedirectToAction("List", "Person");
        }

        public ActionResult Search()
        {
            PersonSearchViewModel viewModel = new PersonSearchViewModel();

            return View();
        }

        [HttpPost]
        public IActionResult EditRoles(IEnumerable<PersonRole> personRoles)
        {

            foreach(var role in personRoles)
            {
                _unitOfWork.PersonRoleRepository.Update(role);
            }

            _unitOfWork.Save();

            return RedirectToAction("List", "Person");
        }
        
        public ActionResult Details(int id)
        {
            (Person, string) toPass = personService.GetPersonData(id);

            ViewBag.Base64Img = toPass.Item2;

            IEnumerable<PersonRole> roles = _unitOfWork.PersonRoleRepository.GetAll().Where(x => x.PersonId == id);

            foreach (var role in roles)
            {
                role.Person = _unitOfWork.PersonRepository.GetByKey(role.PersonId);
                role.FilmRole = _unitOfWork.FilmRoleRepository.GetByKey(role.FilmRoleId);
                role.Film = _unitOfWork.FilmRepository.GetByKey(role.FilmId);
            }

            ViewBag.Roles = roles;

            IEnumerable<PersonPerformance> perf = _unitOfWork.PersonPerformanceRepository.GetAll()
                .Where(x => x.UserId == GetUserId());

            ViewBag.Performances = perf;
            ViewBag.UserId = GetUserId();

            if (User.Identity.IsAuthenticated)
            {
                AppUser usr = _userManager.Users.Where(x => x.Id == GetUserId()).First();

                ViewBag.User = usr;
            }

            return View(toPass.Item1);
        }
    }
}