using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FilmSearch.Models
{
    public class Comment
    {
        /// <summary>
        /// Database id
        /// </summary>
        public long Id { get; set; }
        
        public string AuthorId { get; set; }
        public AppUser Author { get; set; } 
        
        public long CommentRate { get; set; }
        public string Text { get; set; }
        public string CreationDate { get; set; }
        
        public long? ParentCommentId { get; set; }
        [JsonIgnore]
        public Comment ParentComment { get; set; }
        
        public ICollection<Comment> SubComments { get; set; } = new List<Comment>();
    }
}