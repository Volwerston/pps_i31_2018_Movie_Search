using System.Collections;
using System.Collections.Generic;
using FilmSearch.Models;

namespace FilmSearch.DAL
{
    public interface IFilmPerformanceRepository: IRepository<FilmPerformance>
    {
        IEnumerable<FilmPerformance> GetFilmPerformances(long filmId);
        
        FilmPerformance GetFilmPerformance(long filmId, string userId);
    }
}