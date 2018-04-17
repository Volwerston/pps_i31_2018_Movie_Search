using System.Collections.Generic;

namespace FilmSearch.Models
{
    public class UserBlogView
    {
        public AppUser AppUser { get; set; }
        public List<PostView> Posts { get; set; }
    }
}