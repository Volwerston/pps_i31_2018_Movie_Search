using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FilmSearch.Models
{
    public class Poster
    {
        /// <summary>
        /// Database id
        /// </summary>
        public long Id { get; set; }
        public DateTime AdditionDateTime { get; set; }
        [Column(TypeName = "text")]
        public string Text { get; set; }
    }
}