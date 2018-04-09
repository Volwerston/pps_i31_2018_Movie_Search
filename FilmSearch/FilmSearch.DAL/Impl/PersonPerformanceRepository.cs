using FilmSearch.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FilmSearch.DAL.Impl
{
    public class PersonPerformanceRepository : GenericRepository<PersonPerformance>, IPersonPerformanceRepository
    {
        public PersonPerformanceRepository(FilmSearchContext ctx)
            : base(ctx)
        {
        }
    }
}
