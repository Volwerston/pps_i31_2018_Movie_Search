using System;
using System.Collections.Generic;
using System.Linq;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Models.Helper;
using FilmSearch.Models.View;
using FilmSearch.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FilmSearch.Controllers.API
{
    [Route("api/person")]
    public class PersonApiController: Controller
    {
        private PersonService _personService;
        private IUnitOfWork _unitOfWork;
        private IConfiguration Configuration;

        public PersonApiController(PersonService personService, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _personService = personService;
            _unitOfWork = unitOfWork;
            Configuration = configuration;
        }

        [HttpGet]
        public IActionResult GetAllPersons([FromQuery] string q, [FromQuery] int page)
        {
            var (persons, totalCount) = _personService.GetPersonsByNamePaginated(q, page);
            return new ObjectResult(new PaginatedResponse<Person>
            {
                Count = persons.Count(),
                Data = persons.ToList(),
                PageSize = PersonService.PageSize,
                TotalCount = totalCount
            });
        }

        [HttpPost]
        [Route("search")]
        public IActionResult Search([FromBody] PersonSearchParams param)
        {
            IEnumerable<Person> allPerson = _unitOfWork.PersonRepository.GetAll();

            IEnumerable<Person> filtered = new PersonSelectQueryBuilder(allPerson).Filter(param);

            List<Tuple<Person, string>> toReturn = new List<Tuple<Person, string>>();
            
            foreach(var person in filtered)
            {
                File img =  _unitOfWork.FileRepository.GetByKey(person.PhotoId);

                toReturn.Add(new Tuple<Person, string>(person, 
                    $"data:{img.FileType};base64,{FileManager.GetBase64File(img.Path)}"));
            }

            return Json(toReturn);
        }



    }
}