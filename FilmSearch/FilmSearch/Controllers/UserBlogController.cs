using FilmSearch.Services;
using Microsoft.AspNetCore.Mvc;

namespace FilmSearch.Controllers
{
    public class UserBlogController: Controller
    {
        private readonly UserBlogService _userBlogService;

        public UserBlogController(UserBlogService userBlogService)
        {
            _userBlogService = userBlogService;
        }

        [HttpGet]
        public IActionResult CreateBlog()
        {
            return View();
        }
        
        [HttpGet]
        public IActionResult BlogViews()
        {
            return View(_userBlogService.GetPosts());
        }
        
        [HttpGet]
        public IActionResult BlogView(long id)
        {
            return View(_userBlogService.GetPost(id));
        }
    }
}