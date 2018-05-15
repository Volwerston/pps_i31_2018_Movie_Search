using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmSearch.Controllers
{
    public class FilmController : Controller
    {
        private string GetUserId() => this.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        private readonly FilmService _filmService;

        public FilmController(FilmService filmService)
        {
            _filmService = filmService;
        }
        
        [HttpGet]
        [Authorize(Roles ="Administrator")]
        public IActionResult CreateFilmView()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ShowFilmViews(string sortOrder, string sortValue, string name, string award, string playwriter, long playwriterId = 0)
        {
            var sortQuery = new SortQuery
            {
                Order = sortOrder ?? FilmConstants.SortAsc,
                Value = sortValue ?? FilmConstants.SortTitle
            };

            var filterQuery = new FilmFilterQuery
            {
                Title = name,
                PlaywriterId = playwriterId,
                Playwriter = playwriter
                
            };
            var awardQuery = new FilmFilterQuery
            {
                Title = award,
                PlaywriterId = playwriterId
            };
            return View(
                new SortedSearchResponse<FilmModel, FilmFilterQuery> {
                    Data = _filmService
                        .GetFilms(sortQuery, filterQuery, awardQuery)
                        .Select(film => _filmService.GetFilmView(film))
                        .ToList(),
                    SortQuery = sortQuery,
                    Filter = filterQuery,
                    AwardFilter= awardQuery
                });
        }

        [HttpGet]
        public IActionResult FilmView(long id)
        {
            var filmModel = _filmService.GetFilmView(id);
            return View(new FilmViewModel
            {
                Film = filmModel,
                FilmPerformance = _filmService.GetFilmPerformance(id, GetUserId())
            });
        }
    
    }
}