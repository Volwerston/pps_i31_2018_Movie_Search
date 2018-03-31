using FilmSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FilmSearch.DAL.Impl
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(FilmSearchContext context) : base(context)
        {
        }

        public IEnumerable<Person> PersonsByIds(IEnumerable<long> ids)
        {
            return _dbSet.Where(p => ids.Contains(p.Id));
        }
    }
}
