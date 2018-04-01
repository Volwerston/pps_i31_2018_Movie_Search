using System;
using System.Linq;
using FilmSearch.Models;
using FilmSearch.Services;
using Microsoft.AspNetCore.Mvc;

namespace FilmSearch.Controllers.API
{
    [Route("api/person")]
    public class PersonApiController: Controller
    {
        private PersonService _personService;

        public PersonApiController(PersonService personService)
        {
            _personService = personService;
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
    }
}