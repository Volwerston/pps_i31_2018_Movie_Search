using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FilmSearch.Utils;

namespace FilmSearch.Models
{
    public class FilmModel
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
        
        public double Performance { get; set; }

        public File Photo { get; set; }
        
        public Person Director { get; set; }
        
        public List<Person> Actors { get; set; }
        
        public List<Genre> Genres { get; set; }


        public static Film To(FilmModel filmModel)
        {
            return new Film
            {
                Id = 0,
                Title = filmModel.Title,
                ReleaseDate = DateUtils.ParseDate(filmModel.ReleaseDate),
                ShortDescription = filmModel.ShortDescription,
                Performance = filmModel.Performance,
                Photo = filmModel.Photo
            };
        }
        
        public static FilmModel Of(Film film)
        {
            return new FilmModel
            {
                Id = film.Id,
                Title = film.Title,
                ReleaseDate = DateUtils.ParseDate(film.ReleaseDate),
                Performance = film.Performance,
                ShortDescription = film.ShortDescription,
                Photo = film.Photo
            };
        }
        public static FilmModel Of(Film film, List<Person> actors, Person director, List<Genre> genres)
        {
            var filmViewModel = Of(film);
            filmViewModel.Actors = actors;
            filmViewModel.Director = director;
            filmViewModel.Genres = genres.Select(g => new Genre
            {
                Id = g.Id,
                Name = g.Name
            }).ToList();

            return filmViewModel;
        }
    }
}