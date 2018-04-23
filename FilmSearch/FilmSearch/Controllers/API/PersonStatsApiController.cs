using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
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
        private readonly UserBlogService postService;

        public PersonStatsApiController(IUnitOfWork _uow, UserManager<AppUser> userMgr, PersonService _ps, UserBlogService _ubs)
        {
            unitOfWork = _uow;
            userManager = userMgr;
            personService = _ps;
            postService = _ubs;
        }

        [Route("personCommentStats")]
        public async  Task<IActionResult> PersonCommentStats([FromQuery] string personEmail, [FromQuery] string timeSpan)
        {
            AppUser user = await userManager.FindByEmailAsync(personEmail);

            IEnumerable<PersonComment> pc = unitOfWork.PersonCommentRepository
                .GetAll()
                .Where(x => x.AuthorId == user.Id);

            pc = Filter(timeSpan, pc, x => x.CreationDate);

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

        [Route("postCommentStats")]
        public async Task<IActionResult> PostCommentStats([FromQuery] string personEmail, [FromQuery] string timeSpan)
        {
            AppUser user = await userManager.FindByEmailAsync(personEmail);

            IEnumerable<PostComment> pc = unitOfWork.PostCommentRepository
                .GetAll()
                .Where(x => x.AuthorId == user.Id);

            pc = Filter(timeSpan, pc, x=>x.CreationDate);

            List<LineDateChartDTO> dlc = pc.GroupBy(x => x.CreationDate).Select(y => new LineDateChartDTO()
            {
                Date = DateTime.ParseExact(y.Key, "M/d/yyyy", CultureInfo.InvariantCulture),
                Items = y.Count()
            }).ToList();

            return Json(new StatisticsResult<PostCommentChartDTO>()
            {
                ChartInfo = dlc,
                ChartDtos = postService.GetCommentChartList(pc, personEmail).ToList()
            });
        }

        #region Helper Methods

        private IEnumerable<T> Filter<T>(string timeSpan, IEnumerable<T> toFilter, Func<T, string> selector)
        {
            switch (timeSpan)
            {
                case "day":
                    return toFilter
                        .Where(x => 
                            (DateTime.Now - DateTime.ParseExact(selector(x), "M/d/yyyy", CultureInfo.InvariantCulture)).TotalDays == 0
                            );
                case "week":
                    return toFilter
                        .Where(x =>
                            (DateTime.Now - DateTime.ParseExact(selector(x), "M/d/yyyy", CultureInfo.InvariantCulture)).TotalDays <= 7
                            );
                case "month":
                    return toFilter
                        .Where(x =>
                            (DateTime.Now - DateTime.ParseExact(selector(x), "M/d/yyyy", CultureInfo.InvariantCulture)).TotalDays <= 30
                        );
                case "year":
                    return toFilter
                        .Where(x =>
                            (DateTime.Now - DateTime.ParseExact(selector(x), "M/d/yyyy", CultureInfo.InvariantCulture)).TotalDays <= 365
                        );
                default:
                    return toFilter;
            }
        }
        #endregion
    }
}