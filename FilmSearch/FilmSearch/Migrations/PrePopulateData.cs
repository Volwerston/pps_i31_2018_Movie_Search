using System;
using System.Linq;
using FilmSearch.DAL;
using FilmSearch.Models;
using Microsoft.AspNetCore.Identity;

namespace FilmSearch.Migrations
{
    public class PrePopulateData
    {
        
        private IUnitOfWork _unitOfWork;

        private RoleManager<AppUser> _roleManager;
        
        public PrePopulateData(IUnitOfWork unitOfWork,
            RoleManager<AppUser> roleManager)
        {
            _unitOfWork = unitOfWork;
            _roleManager = roleManager;
        }

        public void PrePopulate()
        {
            PrePopulateGenres();
            PrePopulatePersons();
            PerPopulateFilmRoles();
            
            _unitOfWork.Save();
        }

        private void PrePopulateGenres()
        {
            if (!_unitOfWork.GenreRepository.Empty()) return;
            
            _unitOfWork.GenreRepository.Add(new Genre {Name = "Fantasy"});
            _unitOfWork.GenreRepository.Add(new Genre {Name = "Action"});
            _unitOfWork.GenreRepository.Add(new Genre {Name = "Thriller"});
            _unitOfWork.GenreRepository.Add(new Genre {Name = "Crime"});
            _unitOfWork.GenreRepository.Add(new Genre {Name = "Science fiction"});
            _unitOfWork.GenreRepository.Add(new Genre {Name = "War"});
            _unitOfWork.GenreRepository.Add(new Genre {Name = "History"});
            _unitOfWork.GenreRepository.Add(new Genre {Name = "Documentary"});
        }

        private void PrePopulateRoles()
        {
        }

        private void PerPopulateFilmRoles()
        {
            if (!_unitOfWork.FilmRoleRepository.Empty()) return;
            
            _unitOfWork.FilmRoleRepository.Add(new FilmRole {Name = "Actor"});
            _unitOfWork.FilmRoleRepository.Add(new FilmRole {Name = "Director"});
        }

        private void PrePopulatePersons()
        {
            if (!_unitOfWork.PersonRepository.Empty()) return;
            
            _unitOfWork.PersonRepository.Add(new Person
            {
                Name = "Saoirse",
                Surname = "Ronan",
                BirthDate = new DateTime(1994, 4, 12),
                Country = "Ireland, USA",
                Photo = null
            });
            _unitOfWork.PersonRepository.Add(new Person
            {
                Name = "Gary",
                Surname = "Oldman",
                BirthDate = new DateTime(1958, 3, 21),
                Country = "UK",
                Photo = null
            });
            _unitOfWork.PersonRepository.Add(new Person
            {
                Name = "Stanley",
                Surname = "Kubrick",
                BirthDate = new DateTime(1928, 1, 26),
                Country = "USA",
                Photo = null
            });
            _unitOfWork.PersonRepository.Add(new Person
            {
                Name = "Jennifer",
                Surname = "Lawrence",
                BirthDate = new DateTime(1990, 8, 15),
                Country = "USA",
                Photo = null
            });
            _unitOfWork.PersonRepository.Add(new Person
            {
                Name = "Emma",
                Surname = "Stone",
                BirthDate = new DateTime(1990, 8, 15),
                Country = "USA",
                Photo = null
            });
            _unitOfWork.PersonRepository.Add(new Person
            {
                Name = "Wes",
                Surname = "Anderson",
                BirthDate = new DateTime(1969, 5, 1),
                Country = "USA",
                Photo = null
            });
            _unitOfWork.PersonRepository.Add(new Person
            {
                Name = "Martin",
                Surname = "McDonagh",
                BirthDate = new DateTime(1970, 3, 26),
                Country = "Ireland",
                Photo = null
            });
            _unitOfWork.PersonRepository.Add(new Person
            {
                Name = "Nicolas",
                Surname = "Cage",
                BirthDate = new DateTime(1964, 1, 7),
                Country = "Ireland",
                Photo = null
            });
        }
    }
}