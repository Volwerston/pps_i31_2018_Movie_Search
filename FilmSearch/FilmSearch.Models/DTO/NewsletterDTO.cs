using System;
using System.Collections.Generic;
using System.Text;

namespace FilmSearch.Models.DTO
{
    public class NewsletterDTO
    {
        public List<UserIdDTO> UserIds { get; set; }
        public string Text { get; set; }
    }
}
