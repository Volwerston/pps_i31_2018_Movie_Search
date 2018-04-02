﻿using System;
using System.Collections.Generic;
using System.Linq;
using FilmSearch.DAL;
using FilmSearch.Models;

namespace FilmSearch.Services
{
    public class PersonService
    {
        public const int PageSize = 10;
        
        private IUnitOfWork _unitOfWork;

        public PersonService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public (IEnumerable<Person>, int) GetPersonsByNamePaginated(string name, int page)
        {
            name = name ?? "";
            var persons = _unitOfWork.PersonRepository.PersonsByName(name);
            return (persons.Skip(PageSize * (page - 1)).Take(PageSize).ToList(), persons.Count());
        }
    }
}