using System;
using System.Collections.Generic;
using System.Text;
using FilmSearch.Models;
namespace FilmSearch.DAL
{
    public interface IAwardRepository : IRepository<Award>
    {
        IEnumerable<Award> AwardsByIds(IEnumerable<long> ids);

        IEnumerable<Award> AwardsByName(string name);
    }
}
