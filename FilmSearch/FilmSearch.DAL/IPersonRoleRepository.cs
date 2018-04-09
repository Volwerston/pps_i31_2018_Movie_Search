using System.Collections.Generic;
using FilmSearch.Models;

namespace FilmSearch.DAL
{
    public interface IPersonRoleRepository: IRepository<PersonRole>
    {
        IEnumerable<Person> ActorsByFilmId(long filmId);

        Person DirectorByFilmId(long filmId);

        void DeletePersonRolesByFilm(long filmId);
    }
}