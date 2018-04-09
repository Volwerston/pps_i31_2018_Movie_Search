using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Models.Entities;
using FilmSearch.Models.Entities.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FilmSearch.Controllers.API
{
    [Produces("application/json")]
    [Route("api/personComment")]
    public class PersonCommentApiController : Controller
    {
        private IUnitOfWork _unitOfWork;
        private UserManager<AppUser> _userManager;

        public PersonCommentApiController(IUnitOfWork unitOfWork, UserManager<AppUser> usrMgr)
        {
            _unitOfWork = unitOfWork;
            _userManager = usrMgr;
        }

        [HttpPost]
        [Route("add")]
        public IActionResult Add([FromBody] PersonCommentDTO comment)
        {
            _unitOfWork.PersonCommentRepository.Add(new PersonComment()
            {
                AuthorId = comment.AuthorId,
                CreationDate = comment.CreationDate,
                PersonId = comment.PersonId,
                Text = comment.Text
            });
            _unitOfWork.Save();

            return Ok();
        }

        [Route("get/{personId}")]
        public IActionResult Get(long personId)
        {
            var returned = _unitOfWork.PersonCommentRepository.GetByPersonId(personId);

            foreach(var item in returned)
            {
                item.Author = _userManager.Users.Where(x => x.Id == item.AuthorId).FirstOrDefault();
            }

            return Json(returned);
        }
    }
}