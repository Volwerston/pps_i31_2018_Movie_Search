using System.Collections.Generic;
using System.Linq;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Utils;

namespace FilmSearch.Services
{
    public class FilmService
    {
        private IUnitOfWork _unitOfWork;

        public FilmService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Film AddFilm(Film film, Person directorModel, IEnumerable<Person> actorModels, IEnumerable<Genre> genreModels)
        {
            ValidationUtils.RequireNull(film.Id, "Film id should be null");
            
            _unitOfWork.FilmRepository.Add(film);
            
            var genres = _unitOfWork.GenreRepository.GenresByIds(genreModels.Select(g => g.Id));
            var actors = _unitOfWork.PersonRepository.PersonsByIds(actorModels.Select(a => a.Id));
            var director = _unitOfWork.PersonRepository.GetByKey(directorModel.Id);

            
            _unitOfWork.FilmRepository.Add(film);
            
            AddGenres(genres, film);
            AddFilmActors(actors, film);
            AddFilmDirector(director, film);
            
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