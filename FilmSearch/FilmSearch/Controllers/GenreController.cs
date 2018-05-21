using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmSearch.DAL;
using FilmSearch.Models;
using Microsoft.AspNetCore.Mvc;

namespace FilmSearch.Controllers
{
    public class GenreController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public GenreController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Genre> genres = _unitOfWork.GenreRepository.GetAll();
            return View(genres);
        }

        public IActionResult Remove(long id)
        {
            try
            {
                int existing =  _unitOfWork.FilmGenreRepository.GetAll().Where(x => x.GenreId == id).Count();

                if(existing > 0)
                {
                    throw new Exception("You cannot remove genres which are used by films");
                }

                _unitOfWork.GenreRepository.Delete(id);
                _unitOfWork.Save();

                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        public IActionResult Add([FromBody] Genre genre)
        {
            try
            {
                IEnumerable<Genre> genres = _unitOfWork.GenreRepository.GetAll();

                if(genres.Select(x => x.Name).Contains(genre.Name))
                {
                    throw new Exception("Genre already exists");
                }

                _unitOfWork.GenreRepository.Add(genre);
                _unitOfWork.Save();

                return Ok();
            }
            catch(Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}