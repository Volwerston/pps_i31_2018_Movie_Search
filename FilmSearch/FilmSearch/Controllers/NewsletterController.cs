using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FilmSearch.Controllers
{
    public class NewsletterController : Controller
    {
        [Authorize(Roles="Administrator")]
        public IActionResult Index()
        {
            return View();
        }


    }
}