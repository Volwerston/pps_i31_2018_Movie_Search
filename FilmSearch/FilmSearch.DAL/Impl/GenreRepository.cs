using System.Collections.Generic;
using System.Linq;
using FilmSearch.Models;

namespace FilmSearch.DAL.Impl
{
    public class GenreRepository: GenericRepository<Genre>, IGenreRepository
    {
        public GenreRepository(FilmSearchContext context) : base(context)
        {
        }

        public IEnumerable<Genre> GenresByIds(IEnumerable<long> ids)
        {
            return _dbSet.Where(genre => ids.Contains(genre.Id));
        }
    }
}