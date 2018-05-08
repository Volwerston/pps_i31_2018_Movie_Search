using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FilmSearch.Models.Entities
{
    public class Award
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
