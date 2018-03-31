using FilmSearch.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FilmSearch.DAL.Impl
{
    public class PersonRepository : GenericRepository<Person>, IPersonRepository
    {
        public PersonRepository(FilmSearchContext ctx) :
        base(ctx)
        {

        }
    }
}
