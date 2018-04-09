using System.Linq;
using FilmSearch.Models;

namespace FilmSearch.DAL.Impl
{
    public class FilmGenreRepository: GenericRepository<FilmGenre>, IFilmGenreRepository
    {
        public FilmGenreRepository(FilmSearchContext context) : base(context)
        {
        }

        public void DeleteFilmGenresByFilmId(long filmId)
        {
            var genres = _dbSet
                .Where(fg => fg.FilmId == filmId);
            foreach (var filmGenre in genres)
            {
                _dbSet.Remove(filmGenre);
            }
        }
    }
}