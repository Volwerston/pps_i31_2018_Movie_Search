using System.Collections.Generic;
using System.Linq;
using FilmSearch.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmSearch.DAL.Impl
{
    public class FilmRepository: GenericRepository<Film>, IFilmRepository
    {
        public FilmRepository(FilmSearchContext context) : base(context)
        {
        }

        public IEnumerable<Film> GetAll()
        {
            return _dbSet
                .Include(f => f.Photo)
                .Include(f => f.Genres).ThenInclude(fg => fg.Genre)
                .ToList();
        }

        public Film GetByKey(object key)
        {
            return _dbSet
                .Include(f => f.Photo)
                .Include(f => f.Genres).ThenInclude(fg => fg.Genre)
                .FirstOrDefault(f => key.Equals(f.Id));
        }
    }
}