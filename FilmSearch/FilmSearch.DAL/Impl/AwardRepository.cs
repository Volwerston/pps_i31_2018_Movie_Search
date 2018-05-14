using FilmSearch.Models;
using FilmSearch.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FilmSearch.DAL.Impl
{
    public class AwardRepository : GenericRepository<Award>, IAwardRepository
    {
        public AwardRepository(FilmSearchContext context) : base(context)
        {
        }
        public IEnumerable<Award> AwardsByIds(IEnumerable<long> ids)
        {
            return _dbSet.Where(genre => ids.Contains(genre.Id)).ToList(); ;
        }

        public IEnumerable<Award> AwardsByName(string name)
        {
            return _dbSet.Where(g => g.Name.ToLower().Contains(name.ToLower())).ToList(); ;
        }
    }
}
