using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmSearch.Models;
using FilmSearch.Models.DTO;
using FilmSearch.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace FilmSearch.Controllers.API
{
    [Produces("application/json")]
    [Route("api/newsletter")]
    public class NewsletterApiController : Controller
    {
        private UserService _userService;
        private MailService _mailService;
        private UserManager<AppUser> _userManager;
        private IConfiguration Configuration;

        public NewsletterApiController(UserService usrService, MailService mailService, UserManager<AppUser> userMgr, IConfiguration config)
        {
            _userService = usrService;
            _mailService = mailService;
            _userManager = userMgr;
            Configuration = config;
        }

        [Route("users")]
        public IActionResult GetAll([FromQuery] string q, [FromQuery] int page)
        {
            var (users, totalCount) = _userService.GetUsersByEmailPaginated(q, page);

            return new ObjectResult(new PaginatedResponse<AppUser>
            {
                Count = users.Count(),
                Data = users.ToList(),
                PageSize = PersonService.PageSize,
                TotalCount = totalCount
            });
        }

        [HttpPost]
        [Route("send")]
        public IActionResult Post([FromBody] NewsletterDTO newsletter)
        {
            List<AppUser> users = _userManager.Users.ToList();

            MailParams parameters = new MailParams()
            {
                Host = Configuration["Mail:Host"],
                Port = int.Parse(Configuration["Mail:Port"]),
                UserName = Configuration["Mail:UserName"],
                UserPassword = Configuration["Mail:UserPassword"]
            };

            Task.Run(() => _mailService.SendNewsletter(newsletter, users, parameters));
            return Ok();
        }
    }
}