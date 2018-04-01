using FilmSearch.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace FilmSearch.DAL
{
    public interface IPersonRepository : IRepository<Person>
    {
        IEnumerable<Person> PersonsByIds(IEnumerable<long> ids);

        IEnumerable<Person> PersonsByName(string name);
    }
}
