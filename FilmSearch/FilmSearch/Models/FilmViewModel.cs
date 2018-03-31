using System;
using System.Collections.Generic;

namespace FilmSearch.Models
{
    public class FilmViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime RealeaseDate { get; set; }
        
        public string ShortDescription { get; set; }
        
        public Person Director { get; set; }
        
        public List<Person> Actors { get; set; }
        
        public ICollection<FilmGenre> Genres { get; set; }
    }
}