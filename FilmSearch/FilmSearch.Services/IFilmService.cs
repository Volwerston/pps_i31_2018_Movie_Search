using FilmSearch.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FilmSearch.Services
{
    public interface IFilmService
    {
         FilmPerformance GetFilmPerformance(long filmId, string userId);

         List<FilmPerformance> GetFilmPerformances(long filmId);

         Film UpdateFilm(Film film, Person directorModel, IEnumerable<Person> actorModels, IEnumerable<Genre> genreModels);

         Film AddFilm(Film film, Person directorModel, IEnumerable<Person> actorModels, IEnumerable<Genre> genreModels);

         Film GetFilm(long id);

         List<Person> GetFilmActors(long id);

         Person GetFilmDirector(long id);

        double RateFilm(long filmId, string userId, long score);

         List<Genre> GetGenresByName(string name);
         (List<Genre>, int) GetGenresByNamePaginated(string name, int page);
         IEnumerable<Film> GetFilms(SortQuery sortQuery, FilmFilterQuery filmFilterQuery);

         FilmModel GetFilmView(long id);

         FilmModel GetFilmView(Film film);

         void DeleteFilm(long id);
    }
}
