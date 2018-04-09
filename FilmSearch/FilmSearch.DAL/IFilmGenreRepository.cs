using FilmSearch.Models;

namespace FilmSearch.DAL
{
    public interface IFilmGenreRepository: IRepository<FilmGenre>
    {
        void DeleteFilmGenresByFilmId(long filmId);
    }
}