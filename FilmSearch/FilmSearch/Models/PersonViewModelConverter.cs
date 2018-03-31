using FilmSearch.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmSearch.Models
{
    public class PersonViewModelConverter : IViewModelConverter<PersonViewModel, Person>
    {
        public Person Convert(PersonViewModel source)
        {
            return new Person()
            {
                 BirthDate = source.BirthDate,
                 Country = source.Country,
                 Name = source.Name,
                 Surname = source.Surname
            };
        }
    }
}
