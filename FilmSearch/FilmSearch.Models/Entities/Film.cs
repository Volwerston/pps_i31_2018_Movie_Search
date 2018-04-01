using System;
using System.Collections.Generic;

namespace FilmSearch.Models
{
    public class Film
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ShortDescription { get; set; }
        public double Performance { get; set; }
        public ICollection<FilmGenre> Genres { get; set; } = new List<FilmGenre>();
        
        public long? PhotoId { get; set; }
        public File Photo { get; set; }
    }
}