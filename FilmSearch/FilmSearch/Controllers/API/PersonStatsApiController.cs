using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Models.DTO;
using FilmSearch.Models.Entities;
using FilmSearch.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FilmSearch.Controllers.API
{
    [Produces("application/json")]
    [Route("api/personStats")]
    [Authorize/*(Roles="Administrator")*/]
    public class PersonStatsApiController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager<AppUser> userManager;
        private readonly PersonService personService;

        public PersonStatsApiController(IUnitOfWork _uow, UserManager<AppUser> userMgr, PersonService _ps)
        {
            unitOfWork = _uow;
            userManager = userMgr;
            personService = _ps;
        }

        [Route("personCommentStats")]
        public async  Task<IActionResult> PersonCommentStats([FromQuery] string personEmail, [FromQuery] string timeSpan)
        {
            AppUser user = await userManager.FindByEmailAsync(personEmail);

            IEnumerable<PersonComment> pc = unitOfWork.PersonCommentRepository
                .GetAll()
                .Where(x => x.AuthorId == user.Id);

            pc = Filter(timeSpan, pc);

            List<LineDateChartDTO> dlc = pc.GroupBy(x => x.CreationDate).Select(y => new LineDateChartDTO()
            {
                Date   = DateTime.ParseExact(y.Key, "M/d/yyyy", CultureInfo.InvariantCulture),
                Items = y.Count()
            }).ToList();

            return Json(new StatisticsResult<PersonCommentChartDTO>()
            {
                ChartInfo = dlc,
                ChartDtos = personService.GetCommentChartList(pc, personEmail).ToList()
            });
        }

        #region Helper Methods

        private IEnumerable<PersonComment> Filter(string timeSpan, IEnumerable<PersonComment> toFilter)
        {
            switch (timeSpan)
            {
                case "day":
                    return toFilter
                        .Where(x => 
                            (DateTime.Now - DateTime.ParseExact(x.CreationDate, "M/d/yyyy", CultureInfo.InvariantCulture)).TotalDays == 0
                            );
                case "week":
                    return toFilter
                        .Where(x =>
                            (DateTime.Now - DateTime.ParseExact(x.CreationDate, "M/d/yyyy", CultureInfo.InvariantCulture)).TotalDays <= 7
                            );
                case "month":
                    return toFilter
                        .Where(x =>
                            (DateTime.Now - DateTime.ParseExact(x.CreationDate, "M/d/yyyy", CultureInfo.InvariantCulture)).TotalDays <= 30
                        );
                case "year":
                    return toFilter
                        .Where(x =>
                            (DateTime.Now - DateTime.ParseExact(x.CreationDate, "M/d/yyyy", CultureInfo.InvariantCulture)).TotalDays <= 365
                        );
                default:
                    return toFilter;
            }
        }
        #endregion
    }
}