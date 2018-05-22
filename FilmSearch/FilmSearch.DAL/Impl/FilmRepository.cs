using System;
using System.Collections.Generic;
using System.Linq;
using FilmSearch.Models;
using Microsoft.EntityFrameworkCore;
using NpgsqlTypes;

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

        public List<Film> GetFilms(SortQuery sortQuery, FilmFilterQuery filmFilterQuery)
        {
            return SortFilms(FilterFilms(
                _dbSet.Include(f => f.Photo)
                      .Include(f => f.Genres).ThenInclude(fg => fg.Genre),
                filmFilterQuery),
                sortQuery).ToList();
        }
        

        private IEnumerable<Film> SortFilms(IEnumerable<Film> films, SortQuery sortQuery)
        {
            var comparer = Comparer<Film>.Create((f1, f2) =>
            {
                if (sortQuery.Order == FilmConstants.SortDesc)
                {
                    var temp = f2;
                    f2 = f1;
                    f1 = temp;
                }
                
                switch (sortQuery.Value)
                {
                    case FilmConstants.SortDate:
                        return f1.ReleaseDate.Date.CompareTo(f2.ReleaseDate.Date);
                    case FilmConstants.SortRate:
                        return f1.Performance.CompareTo(f2.Performance);
                    default:
                        return string.Compare(f1.Title, f2.Title, StringComparison.Ordinal);
                }
            });

            return films.OrderBy(f => f, comparer);
        }
        
        private IEnumerable<Film> FilterFilms(IEnumerable<Film> films, FilmFilterQuery filterQuery)
        {
            return (from f in films
                where filterQuery.Title == null || f.Title.ToLower().Contains(filterQuery.Title.ToLower())
                join playwriterRole in _context.PersonRoles on f.Id equals playwriterRole.FilmId into fpr
                from playwriterRole in fpr.DefaultIfEmpty(new PersonRole())
                join playwriter in _context.Persons on playwriterRole.PersonId equals playwriter.Id into fp
                from playwriter in fp.DefaultIfEmpty(new Person())
                    where filterQuery.Playwriter == null || playwriter.FullName.ToLower().Contains(filterQuery.Playwriter.ToLower())
                    where filterQuery.PlaywriterId == 0 || playwriter.Id == filterQuery.PlaywriterId
                join filmAward in _context.FilmAwards on f.Id equals filmAward.FilmId into ffa
                from filmAward in ffa.DefaultIfEmpty(new FilmAward())
                join award in _context.Awards on filmAward.AwardId equals award.Id into fa
                from award in fa.DefaultIfEmpty(new Award())
                    where filterQuery.Awards == null || filterQuery.Awards.Count == 0 || filterQuery.Awards.Contains(award.Id)
                select f).Distinct();
        }
    }
}