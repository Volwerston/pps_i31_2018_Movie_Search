using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace FilmSearch.Tests
{
    class FakePersonService : PersonService
    {
        public FakePersonService(IUnitOfWork uow) : base(uow) { }
        public (Person, string) GetPersonData(long id)
        {
            Person toRet = new Person() { Id = id, Name = (id == 1) ? "Name" : "another", Surname = "Surname", BirthDate = DateTime.Today };
            return (toRet, "smth");
        }
    }
}
