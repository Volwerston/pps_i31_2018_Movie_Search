using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Services;
using FilmSearch.Utils;
using Microsoft.AspNetCore.Http;

namespace FilmSearch.Controllers.API
{
    [Route("api/film")]
    public class FilmApiController: Controller
    {
        private FilmService _filmService;

        public FilmApiController(FilmService filmService)
        {
            _filmService = filmService;
        }

        [HttpPost]
        public IActionResult AddFilm([FromBody] FilmViewModel filmViewModel)
        {
            var film = _filmService.AddFilm(
                FilmViewModel.To(filmViewModel),
                filmViewModel.Director,
                filmViewModel.Actors,
                filmViewModel.Genres
                );
            
            return CreatedAtRoute("GetFilm", new {id = film.Id}, film);
        }

        [HttpGet("{id}", Name = "GetFilm")]
        public IActionResult GetFilm(long id)
        {
            var film = _filmService.GetFilm(id);
            var genres = film.Genres.Select(fg => fg.Genre);
            
            var director = _filmService.GetFilmDirector(id);
            var actors = _filmService.GetFilmActors(id);
            
            return new ObjectResult(new FilmViewModel
            {
                Id = film.Id,
                Title = film.Title,
                ReleaseDate = DateUtils.ParseDate(film.ReleaseDate),
                ShortDescription = film.ShortDescription,
                Actors = actors,
                Director = director,
                Genres = genres
            });
        }
    }
}