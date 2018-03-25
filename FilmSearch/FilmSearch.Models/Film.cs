using System;
using System.Collections.Generic;

namespace FilmSearch.Models
{
    public class Film
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime RealeaseDate { get; set; }
        public ICollection<FilmGenre> Genres { get; set; }
    }
}