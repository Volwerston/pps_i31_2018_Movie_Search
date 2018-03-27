using FilmSearch.Models;

namespace FilmSearch.DAL.Impl
{
    public class PersonRepository: GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(FilmSearchContext context) : base(context)
        {
        }
    }
}