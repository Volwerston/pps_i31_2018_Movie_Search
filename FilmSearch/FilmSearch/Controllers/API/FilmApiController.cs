using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Services;
using FilmSearch.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace FilmSearch.Controllers.API
{
    [Route("api/film")]
    public class FilmApiController: Controller
    {
        private FilmService _filmService;

        private string GetUserId() => this.User.FindFirstValue(ClaimTypes.NameIdentifier);

        public FilmApiController(FilmService filmService)
        {
            _filmService = filmService;
        }

        [HttpPost]
        [Authorize(Roles ="Administrator")]
        public IActionResult AddFilm([FromBody] FilmModel filmModel)
        {
            var film = _filmService.AddFilm(
                FilmModel.To(filmModel),
                filmModel.Director,
                filmModel.Playwriter,
                filmModel.Actors,
                filmModel.Genres
                );

            return new ObjectResult(_filmService.GetFilmView(film));
        }
        
        [HttpPut]
        [Authorize(Roles ="Administrator")]
        public IActionResult UpdateFilm([FromBody] FilmModel filmModel)
        {
            var film = _filmService.UpdateFilm(
                FilmModel.To(filmModel),
                filmModel.Director,
                filmModel.Playwriter,
                filmModel.Actors,
                filmModel.Genres
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
        
        [HttpDelete("{id}", Name = "DeleteFilm")]
        [Authorize(Roles ="Administrator")]
        public IActionResult DeleteFilm([FromRoute] long id)
        {
            _filmService.DeleteFilm(id);
            return Ok();
        }

        [HttpPut("rate/{filmId}")]
        public IActionResult RateFilm([FromRoute] long filmId, [FromQuery] long rate)
        {
            return new ObjectResult(_filmService.RateFilm(filmId, GetUserId(), rate));
        }
    }
}