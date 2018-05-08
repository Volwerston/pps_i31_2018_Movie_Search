using FilmSearch.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace FilmSearch.DAL.Impl
{
    public class AwardRepository : GenericRepository<Award>, IAwardRepository
    {
        public AwardRepository(FilmSearchContext context) : base(context)
        {
        }
    }
}
