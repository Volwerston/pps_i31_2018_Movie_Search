using System;

namespace FilmSearch.Models
{
    /// <summary>
    /// Model class representing some person
    /// in movie industry
    /// </summary>
    public class Person
    {
        /// <summary>
        /// Database id
        /// </summary>
        public long Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        
        public long PhotoId { get; set; }
        public File Photo { get; set; }
        
        public string Country { get; set; }
    }
}