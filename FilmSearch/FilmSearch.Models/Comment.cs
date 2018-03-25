using System;
using System.Collections.Generic;

namespace FilmSearch.Models
{
    public class Comment
    {
        /// <summary>
        /// Database id
        /// </summary>
        public long Id { get; set; }
        
        public long AuthorId { get; set; }
        public AppUser Author { get; set; } 
        
        public long CommentRate { get; set; }
        public string Text { get; set; }
        public string CreationDate { get; set; }
        
        public long ParentCommentId { get; set; }
        public Comment ParentComment { get; set; }
        
        public List<Comment> SubComments { get; set; }
    }
}