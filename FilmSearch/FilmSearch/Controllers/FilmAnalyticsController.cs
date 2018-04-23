using Microsoft.AspNetCore.Mvc;

namespace FilmSearch.Controllers
{
    public class FilmAnalyticsController: Controller
    {
        public IActionResult Analytics()
        {
            return View();
        }
    }
}