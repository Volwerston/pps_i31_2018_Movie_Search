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
        
        public string AuthorId { get; set; }
        public string AuthorName { get; set; }

        public static PostView from(Post p)
        {
            return new PostView
            {
                Id = p.Id,
                Title = p.Title,
                ImageId = p.ImageId,
                ShortDescription = p.ShortDescription,
                Text = p.Text,
                AuthorId = p.AuthorId,
                AuthorName = $"{p.Author.UserName} {p.Author.Surname}",
                PostDate = p.CreationTime
            };
        }
    }
}