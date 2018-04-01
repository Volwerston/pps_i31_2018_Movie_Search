using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FilmSearch.Utils;

namespace FilmSearch.Models
{
    public class FilmViewModel
    {
        public long Id { get; set; }
        [StringLength(128, MinimumLength = 1)]
        [Required]
        public string Title { get; set; }
        [Required]
        public string ReleaseDate { get; set; }
        [StringLength(1024, MinimumLength = 1)]
        [Required]
        public string ShortDescription { get; set; }

        public File Photo { get; set; }
        
        public Person Director { get; set; }
        
        public IEnumerable<Person> Actors { get; set; }
        
        public IEnumerable<Genre> Genres { get; set; }


        public static Film To(FilmViewModel filmViewModel)
        {
            return new Film
            {
                Id = 0,
                Title = filmViewModel.Title,
                ReleaseDate = DateUtils.ParseDate(filmViewModel.ReleaseDate),
                ShortDescription = filmViewModel.ShortDescription
            };
        }
    }
}