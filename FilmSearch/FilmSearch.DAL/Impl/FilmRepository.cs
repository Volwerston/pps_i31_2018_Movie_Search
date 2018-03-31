using FilmSearch.Models;

namespace FilmSearch.DAL.Impl
{
    public class FilmRepository: GenericRepository<Film>, IFilmRepository
    {
        public FilmRepository(FilmSearchContext context) : base(context)
        {
        }
    }
}