using System;
using System.Collections;
using System.Collections.Generic;

namespace FilmSearch.Models
{
    public class Genre
    {
        /// <summary>
        /// Database id
        /// </summary>
        public long Id { get; set; }
        public string Name { get; set; }
        
        public List<FilmGenre> Films { get; set; } = new List<FilmGenre>();
    }
}