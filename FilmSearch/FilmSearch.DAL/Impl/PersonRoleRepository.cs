using System.Collections.Generic;
using System.Linq;
using FilmSearch.Models;

namespace FilmSearch.DAL
{
    public class PersonRoleRepository : GenericRepository<PersonRole>, IPersonRoleRepository
    {
        public PersonRoleRepository(FilmSearchContext context) : base(context)
        {
        }
        
        
        public IEnumerable<Person> ActorsByFilmId(long filmId)
        {
            return _dbSet
                .Where(pr => pr.FilmId == filmId && pr.FilmRole.Name == FilmRole.ACTOR_ROLE)
                .Select(pr => pr.Person);
        }

        public Person DirectorByFilmId(long filmId)
        {
            return _dbSet
                .FirstOrDefault(pr => pr.FilmId == filmId && pr.FilmRole.Name == FilmRole.DIRECTOR_ROLE)?.Person;
        }
    }
}