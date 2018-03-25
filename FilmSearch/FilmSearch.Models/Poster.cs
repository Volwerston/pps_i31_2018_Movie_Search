using System;

namespace FilmSearch.Models
{
    public class Poster
    {
        /// <summary>
        /// Database id
        /// </summary>
        public long Id { get; set; }
        public DateTime AdditionDateTime { get; set; }
        public string Text { get; set; }
    }
}