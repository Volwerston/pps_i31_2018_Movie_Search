using System;
using System.Collections.Generic;
using System.Linq;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Utils;
using FilmSearch.Utils.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace FilmSearch.Services
{
    public class FilmService
    {
        public const string SortDesc = "desc";
        public const string SortAsc = "asc";

        public const string SortTitle = "title";
        public const string SortDate = "date";
        public const string SortRate = "rate";
        
        public const int PageSize = 10;
        
        private IUnitOfWork _unitOfWork;

        private UserManager<AppUser> _userManager;

        public FilmPerformance GetFilmPerformance(long filmId, string userId)
        {
            return _unitOfWork.FilmPerformanceRepository.GetFilmPerformance(filmId, userId);
        }
        
        public FilmService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public Film AddFilm(Film film, Person directorModel, IEnumerable<Person> actorModels, IEnumerable<Genre> genreModels)
        {
            ValidationUtils.RequireNull(film.Id, "Film id should be null");
            
            _unitOfWork.FilmRepository.Add(film);
            

            
            _unitOfWork.FilmRepository.Add(film);

            if (genreModels != null)
            {
                var genres = _unitOfWork.GenreRepository.GenresByIds(genreModels.Select(g => g.Id));
                AddGenres(genres, film);
            }

            if (actorModels != null)
            {
                var actors = _unitOfWork.PersonRepository.PersonsByIds(actorModels.Select(a => a.Id));
                AddFilmActors(actors, film);
            }

            if (directorModel != null)
            {
                var director = _unitOfWork.PersonRepository.GetByKey(directorModel.Id);
                AddFilmDirector(director, film);
            }

            _unitOfWork.Save();

            return film;
        }

        public Film GetFilm(long id)
        {
            return _unitOfWork.FilmRepository.GetByKey(id);
        }

        public List<Person> GetFilmActors(long id)
        {
            return _unitOfWork.PersonRoleRepository.ActorsByFilmId(id).ToList();
        }

        public Person GetFilmDirector(long id)
        {
            return _unitOfWork.PersonRoleRepository.DirectorByFilmId(id);
        }

        public double RateFilm(long filmId, string userId, long score)
        {
            if (score < 1 || score > 10)
            {
                throw new ValidationException("Score should be in [1, 10]");
            }

            var filmPerformance = GetFilmPerformance(filmId, userId);
            var film = GetFilm(filmId);
            var performances = _unitOfWork.FilmPerformanceRepository.GetFilmPerformances(filmId).ToList();
            
            if (filmPerformance != null)
            {
                filmPerformance.Performance = score;
            }
            else
            {
                filmPerformance = new FilmPerformance
                {
                    FilmId = filmId,
                    UserId = userId,
                    Performance = score
                };
                performances.Add(filmPerformance);
            
                _unitOfWork.FilmPerformanceRepository.Add(filmPerformance);
            }
            var totalPerformance = performances.Sum(p => p.Performance) / performances.Count();

            film.Performance = totalPerformance;
            _unitOfWork.FilmRepository.Update(film);
            
            _unitOfWork.Save();

            return totalPerformance;
        }

        public List<Genre> GetGenresByName(string name)
        {
            name = name ?? "";
            return _unitOfWork.GenreRepository.GenresByName(name).ToList();
        }

        public (List<Genre>, int) GetGenresByNamePaginated(string name, int page)
        {
            var genres = GetGenresByName(name);
            return (genres.Skip(PageSize * (page - 1)).Take(PageSize).ToList(), genres.Count());
        }

        public IEnumerable<Film> GetFilms(SortQuery sortQuery, FilmFilterQuery filmFilterQuery)
        {
            var films = _unitOfWork.FilmRepository.GetAll();
            var sortFunction = GetFilmSortFunction(sortQuery);
            var filterFunction = GetFilmFilterFunction(filmFilterQuery);

            return films.Where(filterFunction).OrderBy(f => f, sortFunction);
        }
        
        public FilmModel GetFilmView(long id)
        {
            var film = GetFilm(id);

            var genres = film.Genres.Select(fg => fg.Genre).ToList();
            var director = GetFilmDirector(id);
            var actors = GetFilmActors(id);

            
            return FilmModel.Of(film, actors, director, genres);
        }

        public FilmModel GetFilmView(Film film)
        {
            var genres = film.Genres.Select(fg => fg.Genre).ToList();
            var director = GetFilmDirector(film.Id);
            var actors = GetFilmActors(film.Id);

            return FilmModel.Of(film, actors, director, genres);
        }

        public void DeleteFilm(long id)
        {
            _unitOfWork.FilmRepository.Delete(id);
        }

        private IComparer<Film> GetFilmSortFunction(SortQuery sortQuery)
        {
            return Comparer<Film>.Create((f1, f2) =>
            {
                if (sortQuery.Order == SortDesc)
                {
                    var temp = f2;
                    f2 = f1;
                    f1 = temp;
                }
                
                switch (sortQuery.Value)
                {
                        case SortDate:
                            return f1.ReleaseDate.Date.CompareTo(f2.ReleaseDate.Date);
                        default:
                            return string.Compare(f1.Title, f2.Title, StringComparison.Ordinal);
                }
            });
        }
        
        private void AddFilmActors(IEnumerable<Person> actors, Film film)
        {
            if (actors == null || !actors.Any()) return;

            var filmRole = _unitOfWork.FilmRoleRepository.GetByRoleName(FilmRole.ACTOR_ROLE);
            
            foreach (var actor in actors)
            {
                _unitOfWork.PersonRoleRepository.Add(new PersonRole
                {
                    Film = film,
                    FilmRole = filmRole,
                    Person = actor,
                    Performance = 0
                });
            }
        }

        private void AddFilmDirector(Person director, Film film)
        {
            if (director == null) return;
            
            var filmRole = _unitOfWork.FilmRoleRepository.GetByRoleName(FilmRole.DIRECTOR_ROLE);
            
            var personRole =  new PersonRole
            {
                Film = film,
                FilmRole = filmRole,
                Person = director,
                Performance = 0
            };
            
            _unitOfWork.PersonRoleRepository.Add(personRole);
        }

        private void AddGenres(IEnumerable<Genre> genres, Film film)
        {
            foreach (var genre in genres)
            {
                film.Genres.Add(new FilmGenre
                {
                    Film = film,
                    Genre = genre
                });
            }
        }

        private Func<Film, bool> GetFilmFilterFunction(FilmFilterQuery filmFilterQuery)
        {
            Func<Film, bool> filmFilter = film => true;
            if (filmFilterQuery.Title != null)
            {
                var filter = filmFilter;
                filmFilter = film => filter(film) && film.Title.ToLower().Contains(filmFilterQuery.Title.ToLower());
            }

            return filmFilter;
        }

    }
}