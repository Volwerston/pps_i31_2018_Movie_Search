using System;
using System.Collections.Generic;
using System.Linq;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Utils;

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

        public FilmService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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

        public IEnumerable<Person> GetFilmActors(long id)
        {
            return _unitOfWork.PersonRoleRepository.ActorsByFilmId(id);
        }

        public Person GetFilmDirector(long id)
        {
            return _unitOfWork.PersonRoleRepository.DirectorByFilmId(id);
        }

        public IEnumerable<Genre> GetGenresByName(string name)
        {
            name = name ?? "";
            return _unitOfWork.GenreRepository.GenresByName(name);
        }

        public (IEnumerable<Genre>, int) GetGenresByNamePaginated(string name, int page)
        {
            var genres = GetGenresByName(name);
            return (genres.Skip(PageSize * (page - 1)).Take(PageSize), genres.Count());
        }

        public IEnumerable<Film> GetFilms(SortQuery sortQuery, FilmFilterQuery filmFilterQuery)
        {
            var films = _unitOfWork.FilmRepository.GetAll();
            var sortFunction = GetFilmSortFunction(sortQuery);
            var filterFunction = GetFilmFilterFunction(filmFilterQuery);

            return films.Where(filterFunction).OrderBy(f => f, sortFunction);
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

        public FilmViewModel GetFilmView(long id)
        {
            var film = GetFilm(id);
            var genres = film.Genres?.Select(fg => fg.Genre);
            
            var director = GetFilmDirector(id);
            var actors = GetFilmActors(id);
            
            return new FilmViewModel
            {
                Id = film.Id,
                Title = film.Title,
                ReleaseDate = DateUtils.ParseDate(film.ReleaseDate),
                ShortDescription = film.ShortDescription,
                Actors = actors,
                Director = director,
                Genres = genres
            };
        }

        public FilmViewModel GetFilmView(Film film)
        {
            var genres = film.Genres?.Select(fg => fg.Genre);
            
            var director = GetFilmDirector(film.Id);
            var actors = GetFilmActors(film.Id);
            
            return new FilmViewModel
            {
                Id = film.Id,
                Title = film.Title,
                ReleaseDate = DateUtils.ParseDate(film.ReleaseDate),
                ShortDescription = film.ShortDescription,
                Actors = actors,
                Director = director,
                Genres = genres
            };
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
                _unitOfWork.FilmGenreRepository.Add(new FilmGenre
                {
                    Film = film,
                    Genre = genre
                });
            }
        }
    }
}