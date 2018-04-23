using FilmSearch.Analytics;
using Microsoft.AspNetCore.Mvc;

namespace FilmSearch.Controllers.API
{
    [Route("api/film/analytics")]
    public class FilmAnalyticsApiController: Controller
    {
        [Route("rate/top/{n}")]
        public IActionResult GetTopRatedFilms(int n)
        {
            return new ObjectResult(FilmAnalytics.topRatedFilms(n));
        }

        [Route("rate/worst/{n}")]
        public IActionResult GetWorstRateFilms(int n)
        {
            return new ObjectResult(FilmAnalytics.worstRatedFilms(n));
        }

        [Route("rate/average")]
        public IActionResult GetAverageFilmRate()
        {
            return new ObjectResult(FilmAnalytics.averageFilmRate());
        }
        
        [Route("rate/median")]
        public IActionResult GetMedianFilmResult()
        {
            return new ObjectResult(FilmAnalytics.medianFilmRate());
        }

        [Route("genres/average")]
        public IActionResult GetAverageRateByGenres()
        {
            return new ObjectResult(FilmAnalytics.averageRateByGenres());
        }
    }
}