using FilmSearch.Models.Entities;
using System;
using System.Collections.Generic;

namespace FilmSearch.Models
{
    public static class FilmConstants
    {
        public const string SortDesc = "desc";
        public const string SortAsc = "asc";

        public const string SortTitle = "title";
        public const string SortDate = "date";
        public const string SortRate = "rate";
    }
    
    public class Film
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string ShortDescription { get; set; }
        public double Performance { get; set; }
        public ICollection<FilmGenre> Genres { get; set; } = new List<FilmGenre>();
        public ICollection<FilmAward> Awards { get; set; } = new List<FilmAward>();

        public long? PhotoId { get; set; }
        public File Photo { get; set; }
    }
}