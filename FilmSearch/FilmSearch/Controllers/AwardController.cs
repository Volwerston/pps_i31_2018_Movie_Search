using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmSearch.DAL;
using FilmSearch.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmSearch.Controllers
{
    public class AwardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AwardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Award> awards = _unitOfWork.AwardRepository.GetAll();
            return View(awards);
        }

        public IActionResult Remove(long id)
        {
            try
            {
                _unitOfWork.AwardRepository.Delete(id);
                _unitOfWork.Save();

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public IActionResult Add([FromBody] Award award)
        {
            try
            {
                IEnumerable<Award> awards = _unitOfWork.AwardRepository.GetAll();

                if (awards.Select(x => x.Name).Contains(award.Name))
                {
                    throw new Exception("Award already exists");
                }

                _unitOfWork.AwardRepository.Add(award);
                _unitOfWork.Save();

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}