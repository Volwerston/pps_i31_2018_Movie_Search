using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FilmSearch.Models.View
{
    public class PersonViewModel
    {
        public long Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public DateTime BirthDate { get; set; }

        [Required]
        public IFormFile Photo { get; set; }

        [Required]
        public string Country { get; set; }
    }
}
