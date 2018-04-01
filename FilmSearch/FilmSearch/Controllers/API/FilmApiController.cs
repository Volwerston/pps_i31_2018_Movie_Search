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

            return new ObjectResult(_filmService.GetFilmView(film));
        }

        [HttpGet("genres")]
        public IActionResult GetGenres([FromQuery]string q, [FromQuery] int page)
        {
            var(genres, totalCount) = _filmService.GetGenresByNamePaginated(q, page);

            return new ObjectResult(new PaginatedResponse<Genre>
            {
                Count = genres.Count(),
                Data = genres.ToList(),
                PageSize = FilmService.PageSize,
                TotalCount = totalCount
            });
        }

        [HttpGet("{id}", Name = "GetFilm")]
        public IActionResult GetFilm([FromRoute] long id)
        {
            return new ObjectResult(_filmService.GetFilmView(id));
        }
    }
}