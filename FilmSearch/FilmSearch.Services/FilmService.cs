using System;
using System.Collections.Generic;
using System.Linq;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Models.Entities;
using FilmSearch.Utils;
using FilmSearch.Utils.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace FilmSearch.Services
{
    public static class FilmConstants
    {
        public const string SortDesc = "desc";
        public const string SortAsc = "asc";

        public const string SortTitle = "title";
        public const string SortDate = "date";
        public const string SortRate = "rate";
    }
    
    public class FilmService
    {
        
        public const int PageSize = 10;
        
        private readonly IUnitOfWork _unitOfWork;
        
        public FilmService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public FilmPerformance GetFilmPerformance(long filmId, string userId)
        {
            return _unitOfWork.FilmPerformanceRepository.GetFilmPerformance(filmId, userId);
        }
        
        public List<FilmPerformance> GetFilmPerformances(long filmId)
        {
            return _unitOfWork.FilmPerformanceRepository.GetFilmPerformances(filmId).ToList();;
        }

        public Film UpdateFilm(Film film, Person directorModel, Person playwriterModel,
            IEnumerable<Person> actorModels, IEnumerable<Genre> genreModels)
        {
            if (film.Photo?.Id != null && film.Photo?.Id != 0)
            {
                var photo = _unitOfWork.FileRepository.GetByKey(film.Photo.Id);
                film.Photo = photo;
            }
            else
            {
                film.Photo = null;
            }

            _unitOfWork.PersonRoleRepository.DeletePersonRolesByFilm(film.Id);
            _unitOfWork.FilmGenreRepository.DeleteFilmGenresByFilmId(film.Id);
            
            _unitOfWork.FilmRepository.Update(film);
            _unitOfWork.Save();

            if (genreModels != null)
            {
                var genres = _unitOfWork.GenreRepository.GenresByIds(genreModels.Select(g => g.Id)).ToList();
                AddGenres(genres, film);
            }

            if (actorModels != null)
            {
                var actors = _unitOfWork.PersonRepository.PersonsByIds(actorModels.Select(a => a.Id)).ToList();
                AddFilmActors(actors, film);
            }

            if (directorModel != null)
            {
                var director = _unitOfWork.PersonRepository.GetByKey(directorModel.Id);
                AddFilmDirector(director, film);
            }
            
            if (playwriterModel != null)
            {
                var playwriter = _unitOfWork.PersonRepository.GetByKey(playwriterModel.Id);
                AddFilmPlaywriter(playwriter, film);
            }

            _unitOfWork.Save();

            return film;
        }

        public Film AddFilm(Film film, Person directorModel, Person playwriterModel,
            IEnumerable<Person> actorModels, IEnumerable<Genre> genreModels, IEnumerable<Award> awardModels)
        {
            if (film.Photo?.Id != 0)
            {
                var photo = _unitOfWork.FileRepository.GetByKey(film.Photo.Id);
                film.Photo = photo;
            }
            else
            {
                film.Photo = null;
            }
            
            _unitOfWork.FilmRepository.Add(film);

            if (genreModels != null)
            {
                var genres = _unitOfWork.GenreRepository.GenresByIds(genreModels.Select(g => g.Id)).ToList();
                AddGenres(genres, film);
            }

            if (actorModels != null)
            {
                var actors = _unitOfWork.PersonRepository.PersonsByIds(actorModels.Select(a => a.Id)).ToList();
                AddFilmActors(actors, film);
            }

            if (directorModel != null)
            {
                var director = _unitOfWork.PersonRepository.GetByKey(directorModel.Id);
                AddFilmDirector(director, film);
            }
            
            if (playwriterModel != null)
            {
                var playwriter = _unitOfWork.PersonRepository.GetByKey(playwriterModel.Id);
                AddFilmPlaywriter(playwriter, film);
            }

            /*
            if (awardModels != null)
            {
                var awards = _unitOfWork.AwardRepository.GetByKey(awardModels.Select)
            }
            */
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

        public List<Award> GetFilmAwards(long id)
        {
            var filmAwards = _unitOfWork.FilmAwardRepository.GetAll().Where(x => x.FilmId == id).ToList();
            var awards = new List<Award>();
            foreach (var fa in filmAwards)
            {
                awards.Add(_unitOfWork.AwardRepository.GetByKey(fa.AwardId));
            }
            return awards;
        }

        public Person GetFilmDirector(long id)
        {
            return _unitOfWork.PersonRoleRepository.DirectorByFilmId(id);
        }
        
        public Person GetFilmPlaywriter(long id)
        {
            return _unitOfWork.PersonRoleRepository.PlaywriterByFilmId(id);
        }

        public double RateFilm(long filmId, string userId, long score)
        {
            if (score < 1 || score > 10)
            {
                throw new ValidationException("Score should be in [1, 10]");
            }

            var filmPerformance = GetFilmPerformance(filmId, userId);
            var film = GetFilm(filmId);
            var performances = GetFilmPerformances(filmId);
            
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

            var dbResult = films.Where(filterFunction).OrderBy(f => f, sortFunction).ToList();

            if (filmFilterQuery.PlaywriterId != 0)
            {
                dbResult = dbResult.Where(f => GetFilmPlaywriter(f.Id)?.Id == filmFilterQuery.PlaywriterId).ToList();
            }
            
            return dbResult;
        }
        
        public FilmModel GetFilmView(long id)
        {
            var film = GetFilm(id);

            var genres = film.Genres.Select(fg => fg.Genre).ToList();
            var director = GetFilmDirector(id);
            var playwriter = GetFilmPlaywriter(id);
            var actors = GetFilmActors(id);

            var awards = GetFilmAwards(id);
            return FilmModel.Of(film, actors, director, playwriter, genres,awards);
        }

        public FilmModel GetFilmView(Film film)
        {
            var genres = film.Genres.Select(fg => fg.Genre).ToList();
            var director = GetFilmDirector(film.Id);
            var playwriter = GetFilmPlaywriter(film.Id);
            var actors = GetFilmActors(film.Id);
            var awards = GetFilmAwards(film.Id);

            return FilmModel.Of(film, actors, director, playwriter, genres,awards);
        }

        public void DeleteFilm(long id)
        {
            _unitOfWork.FilmRepository.Delete(id);
            _unitOfWork.Save();
        }

        private IComparer<Film> GetFilmSortFunction(SortQuery sortQuery)
        {
            return Comparer<Film>.Create((f1, f2) =>
            {
                if (sortQuery.Order == FilmConstants.SortDesc)
                {
                    var temp = f2;
                    f2 = f1;
                    f1 = temp;
                }
                
                switch (sortQuery.Value)
                {
                        case FilmConstants.SortDate:
                            return f1.ReleaseDate.Date.CompareTo(f2.ReleaseDate.Date);
                        default:
                            return string.Compare(f1.Title, f2.Title, StringComparison.Ordinal);
                }
            });
        }
        
        private void AddFilmActors(List<Person> actors, Film film)
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
        
        private void AddFilmPlaywriter(Person playwriter, Film film)
        {
            if (playwriter == null) return;
            
            var filmRole = _unitOfWork.FilmRoleRepository.GetByRoleName(FilmRole.PLAYWRITER_ROLE);
            
            var personRole =  new PersonRole
            {
                Film = film,
                FilmRole = filmRole,
                Person = playwriter,
                Performance = 0
            };
            
            _unitOfWork.PersonRoleRepository.Add(personRole);
        }

        private void AddGenres(List<Genre> genres, Film film)
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