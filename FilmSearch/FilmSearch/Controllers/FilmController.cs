using System.Collections.Generic;
using System.Linq;
using FilmSearch.DAL;
using FilmSearch.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmSearch.Controllers
{
    public class FilmController : Controller
    {
        private IUnitOfWork _unitOfWork;

        public FilmController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        
        [HttpGet]
        public IActionResult CreateFilmView()
        {
            return View();
        }
        
//        [HttpPost]
//        public IActionResult CreateFilmView(Film film)
//        {
//            if (film == null || film.Id != 0)
//            {
//                return BadRequest();
//            }
//            
//            _unitOfWork.FilmRepository.Add(film);
//            _unitOfWork.Save();
//
//            return RedirectToAction("AllFilmsView", "Film");
//        }

//        [HttpGet("/all")]
//        public IActionResult AllFilmsView()
//        {
//            var films = _unitOfWork.FilmRepository.GetAll().ToList();
//
//            return View(films);
//        }
    }
}