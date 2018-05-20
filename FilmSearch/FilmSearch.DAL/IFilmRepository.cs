using System.Collections.Generic;
using FilmSearch.Models;

namespace FilmSearch.DAL
{
    public interface IFilmRepository: IRepository<Film>
    {
        List<Film> GetFilms(SortQuery sortQuery, FilmFilterQuery filmFilterQuery);
    }
}