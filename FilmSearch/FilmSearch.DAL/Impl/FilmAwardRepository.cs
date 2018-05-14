using FilmSearch.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FilmSearch.DAL.Impl
{
    public class FilmAwardRepository : GenericRepository<FilmAward>, IFilmAwardRepository
    {
        public FilmAwardRepository(FilmSearchContext context) : base(context)
        {
        }

        public IEnumerable<FilmAward> FilmAwardsByFilmId(long filmId)
        {
            return _context.FilmAwards.Where(x => x.FilmId == filmId);
        }
        public void DeleteFilmAwardsByFilmId(long filmId)
        {
            var awards = _dbSet
                .Where(fa => fa.FilmId == filmId);
            foreach (var award in awards)
            {
                _dbSet.Remove(award);
            }
        }
    }
}
