using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FilmSearch.Models
{
    public class Award
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public List<FilmAward> Films { get; set; } = new List<FilmAward>();
    }
}
