using FilmSearch.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FilmSearch.DAL
{
    public interface IFilmAwardRepository : IRepository<FilmAward>
    {
        IEnumerable<FilmAward> FilmAwardsByFilmId(long filmId);
        void DeleteFilmAwardsByFilmId(long filmId);
    }
}
