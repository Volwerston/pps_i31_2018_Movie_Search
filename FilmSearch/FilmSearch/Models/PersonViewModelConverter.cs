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
                Surname = source.Surname,
                Id = source.Id
            };
        }

        public void ConvertExisting(PersonViewModel source, Person current)
        {
            current.BirthDate = source.BirthDate;
            current.Country = source.Country;
            current.Name = source.Name;
            current.Surname = source.Surname;
            current.Id = source.Id;
        }

        public PersonViewModel ConvertToViewModel(Person person)
        {
            PersonViewModel toReturn = new PersonViewModel();

            toReturn.Id = person.Id;
            toReturn.BirthDate = person.BirthDate;
            toReturn.Country = person.Country;
            toReturn.Name = person.Name;
            toReturn.Surname = person.Surname;

            return toReturn;
        }
    }
}
