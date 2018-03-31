using FilmSearch.Models;

namespace FilmSearch.DAL.Impl
{
    public class FilmGenreRepository: GenericRepository<FilmGenre>, IFilmGenreRepository
    {
        public FilmGenreRepository(FilmSearchContext context) : base(context)
        {
        }
    }
}