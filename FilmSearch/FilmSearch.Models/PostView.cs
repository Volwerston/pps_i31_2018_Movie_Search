using System;

namespace FilmSearch.Models
{
    public class PostView
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Text { get; set; }
        
        public long ImageId { get; set; }
        public DateTime PostDate { get; set; }
        public string AuthorName { get; set; }
    }
}