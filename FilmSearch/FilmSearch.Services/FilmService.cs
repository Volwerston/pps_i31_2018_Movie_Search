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
            IEnumerable<Person> actorModels, IEnumerable<Genre> genreModels, IEnumerable<Award> awardsModels = null)
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

            _unitOfWork.FilmAwardRepository.DeleteFilmAwardsByFilmId(film.Id);

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

            if (awardsModels != null)
            {
                var awards = _unitOfWork.AwardRepository.AwardsByIds(awardsModels.Select(g => g.Id)).ToList();
                AddAwards(awards, film);
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

            if (awardModels != null)
            {
                var awards = _unitOfWork.AwardRepository.AwardsByIds(awardModels.Select(g => g.Id)).ToList();
                AddAwards(awards, film);
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

        public List<Award> GetAwardsByName(string name)
        {
            name = name ?? "";
            return _unitOfWork.AwardRepository.AwardsByName(name).ToList();
        }

        public (List<Genre>, int) GetGenresByNamePaginated(string name, int page)
        {
            var genres = GetGenresByName(name);
            return (genres.Skip(PageSize * (page - 1)).Take(PageSize).ToList(), genres.Count());
        }

        public (List<Award>, int) GetAwardsByNamePaginated(string name, int page)
        {
            var awards = GetAwardsByName(name);
            return (awards.Skip(PageSize * (page - 1)).Take(PageSize).ToList(), awards.Count());
        }

        public SortedSearchResponse<FilmModel, FilmFilterQuery> GetFilms(SortQuery sortQuery, FilmFilterQuery filmFilterQuery)
        {
            return new SortedSearchResponse<FilmModel, FilmFilterQuery>
            {
                Data = _unitOfWork.FilmRepository
                    .GetFilms(sortQuery, filmFilterQuery)
                    .Select(GetFilmView)
                    .ToList(),
                SortQuery = sortQuery,
                Filter = filmFilterQuery
            };
        }
        
        public FilmModel GetFilmView(long id)
        {
            return GetFilmView(GetFilm(id));
        }

        public FilmModel GetFilmView(Film film)
        {
            var genres = film.Genres.Select(fg => fg.Genre).ToList();
            var director = GetFilmDirector(film.Id);
            var playwriter = GetFilmPlaywriter(film.Id);
            var actors = GetFilmActors(film.Id);
            var awards = GetFilmAwards(film.Id);

            return FilmModel.Of(film, actors, director, playwriter, genres, awards);
        }

        public void DeleteFilm(long id)
        {
            _unitOfWork.FilmRepository.Delete(id);
            _unitOfWork.Save();
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

        private void AddAwards(List<Award> awards, Film film)
        {
            foreach (var award in awards)
            {
                film.Awards.Add(new FilmAward
                {
                    Film = film,
                    Award = award
                });
            }
        }


    }
}