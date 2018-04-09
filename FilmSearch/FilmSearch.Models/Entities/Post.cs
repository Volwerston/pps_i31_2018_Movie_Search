using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmSearch.Models
{
    public class Post
    {
        public long Id { get; set; }
        
        public string Title { get; set; }
        
        [Column(TypeName = "text")]
        public string Text { get; set; }
        
        public DateTime CreationTime { get; set; }
        
        public long ImageId { get; set; }
        
        [Column(TypeName = "text")]
        public string ShortDescription { get; set; }
        
        public string AuthorId { get; set; }
        public AppUser Author { get; set; }
    }
}