using FilmSearch.Models.Entities;
using System.Linq;
using System.Collections.Generic;

namespace FilmSearch.DAL.Impl
{
    public class PersonCommentRepository: GenericRepository<PersonComment>, IPersonCommentRepository
    {
        public PersonCommentRepository(FilmSearchContext context) : base(context)
        { 
        }

        public IEnumerable<PersonComment> GetByPersonId(long id)
        {
            return _dbSet.Where(x => x.PersonId == id)
                         .OrderByDescending(x => x.CreationDate);
        }
    }
}
