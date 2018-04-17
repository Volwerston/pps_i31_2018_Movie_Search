using System;

namespace FilmSearch.Models
{
    public class UserBlogCommentModel
    {
        public string Text { get; set; }
        public long ParentId { get; set; }
        public long BlogId { get; set; }
    }
}