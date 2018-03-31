using System.Collections.Generic;
using FilmSearch.Models;

namespace FilmSearch.DAL
{
    public interface IGenreRepository: IRepository<Genre>
    {
        IEnumerable<Genre> GenresByIds(IEnumerable<long> ids);
    }
}