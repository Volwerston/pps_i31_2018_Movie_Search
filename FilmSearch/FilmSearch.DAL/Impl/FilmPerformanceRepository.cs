using System.Collections.Generic;
using System.Linq;
using FilmSearch.Models;

namespace FilmSearch.DAL.Impl
{
    public class FilmPerformanceRepository: GenericRepository<FilmPerformance>, IFilmPerformanceRepository
    {
        public FilmPerformanceRepository(FilmSearchContext context) : base(context)
        {
        }

        public IEnumerable<FilmPerformance> GetFilmPerformances(long filmId)
        {
            return _dbSet.Where(fp => fp.FilmId == filmId).ToList();
        }

        public FilmPerformance GetFilmPerformance(long filmId, string userId)
        {
            return _dbSet
                .Where(fp => fp.FilmId == filmId && fp.UserId == userId)
                .FirstOrDefault();
        }
    }
}