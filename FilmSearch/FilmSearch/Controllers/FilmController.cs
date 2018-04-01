using System;
using System.Collections.Generic;
using System.Linq;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Services;
using Microsoft.AspNetCore.Mvc;

namespace FilmSearch.Controllers
{
    public class FilmController : Controller
    {

        private FilmService _filmService;

        public FilmController(FilmService filmService)
        {
            _filmService = filmService;
        }
        
        [HttpGet]
        public IActionResult CreateFilmView()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ShowFilmViews(string sortOrder, string sortValue, string name)
        {
            var sortQuery = new SortQuery
            {
                Order = sortOrder ?? FilmService.SortAsc,
                Value = sortValue ?? FilmService.SortTitle
            };

            var filterQuery = new FilmFilterQuery
            {
                Title = name
            };
            
            return View(
                new SortedSearchResponse<FilmViewModel, FilmFilterQuery> {
                    Data = _filmService
                        .GetFilms(sortQuery, filterQuery)
                        .Select(film => _filmService.GetFilmView(film))
                        .ToList(),
                    SortQuery = sortQuery,
                    Filter = filterQuery
                });
        }

        [HttpGet]
        public IActionResult FilmView(long id)
        {
            return View(_filmService.GetFilmView(id));
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