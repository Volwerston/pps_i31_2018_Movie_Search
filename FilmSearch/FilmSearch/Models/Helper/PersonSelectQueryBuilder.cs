using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmSearch.DAL;

namespace FilmSearch.Models.Helper
{
    public class PersonSelectQueryBuilder 
    {
        private IEnumerable<Person> toFilter;

        public PersonSelectQueryBuilder(IEnumerable<Person> _toFilter)
        {
            toFilter = _toFilter;
        }

        public IEnumerable<Person> Filter(PersonSearchParams param, IUnitOfWork uow)
        {
            DateTime startDate = default(DateTime);
            DateTime endDate = default(DateTime);

            if (!string.IsNullOrWhiteSpace(param.StartDate) && !string.IsNullOrWhiteSpace(param.EndDate))
            {
                startDate = DateTime.Parse(param.StartDate);
                endDate = DateTime.Parse(param.EndDate);

                toFilter = toFilter.Where(x => x.BirthDate >= startDate && x.BirthDate <= endDate);
            }


            if (!string.IsNullOrWhiteSpace(param.Role))
            {
                List<Person> a = new List<Person>();
                List<PersonRole> personRoles = uow.PersonRoleRepository.GetAll().ToList();
                List<FilmRole> filmRoles = uow.FilmRoleRepository.GetAll().ToList();

                foreach (var person in toFilter)
                {
                    var personRolesCurr = personRoles.Where(x => x.PersonId == person.Id);

                    bool success = false;

                    foreach (var pr in personRolesCurr)
                    {
                        FilmRole fr = filmRoles.Where(x => x.Id == pr.FilmRoleId).First();

                        if (fr.Name.ToLower().Equals(param.Role.Trim().ToLower()))
                        {
                            success = true;
                            break;
                        }
                    }

                    if (success)
                    {
                        a.Add(person);
                    }
                }

                toFilter = a;
            }

            if (!String.IsNullOrWhiteSpace(param.Name))
            {
                toFilter = toFilter.Where(x => x.Name.ToLower().Contains(param.Name.Trim().ToLower()));
            }

            if (!String.IsNullOrWhiteSpace(param.Surname))
            {
                toFilter = toFilter.Where(x => x.Surname.ToLower().Contains(param.Surname.ToLower().Trim()));
            }

            if (!String.IsNullOrWhiteSpace(param.Country))
            {
                toFilter = toFilter.Where(x => x.Country.ToLower().Contains(param.Country.ToLower().Trim()));
            }

            toFilter = toFilter.Where(x => x.Id > param.LastId);

            return toFilter.Take(Math.Min(toFilter.Count(), param.ChunkSize));
        }

    }
}
