using System.Collections;
using System.Collections.Generic;
using FilmSearch.Models;

namespace FilmSearch.DAL
{
    public interface IPersonRepository: IRepository<Person>
    {
        IEnumerable<Person> PersonsByIds(IEnumerable<long> ids);
    }
}