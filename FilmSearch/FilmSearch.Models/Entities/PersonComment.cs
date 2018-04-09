using System;
using System.Collections.Generic;
using System.Text;

namespace FilmSearch.Models.Entities
{
    public class PersonComment
    {
        public long PersonId { get; set; }
        public string Text { get; set; }
        public string CreationDate { get; set; }
        public long Id { get; set; }
        public string AuthorId { get; set; }

        public AppUser Author { get; set; }
    }
}
