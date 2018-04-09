using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmSearch.Models.Helper
{
    public class PersonSelectQueryBuilder : ISelectQueryFilter<PersonSearchParams, Person>
    {
        private IEnumerable<Person> toFilter;

        public PersonSelectQueryBuilder(IEnumerable<Person> _toFilter)
        {
            toFilter = _toFilter;
        }

        public IEnumerable<Person> Filter(PersonSearchParams param)
        {
            if (!String.IsNullOrWhiteSpace(param.Name))
            {
                toFilter = toFilter.Where(x => x.Name == param.Name);
            }

            if (!String.IsNullOrWhiteSpace(param.Surname))
            {
                toFilter = toFilter.Where(x => x.Surname == param.Surname);
            }

            if (!String.IsNullOrWhiteSpace(param.Country))
            {
                toFilter = toFilter.Where(x => x.Country == param.Country);
            }

            toFilter = toFilter.Where(x => x.Id > param.LastId);

            return toFilter.Take(Math.Min(toFilter.Count(), param.ChunkSize));
        }

    }
}
