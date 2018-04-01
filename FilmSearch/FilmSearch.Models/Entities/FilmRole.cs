using System;

namespace FilmSearch.Models
{
    public class FilmRole
    {
        public const string ACTOR_ROLE = "Actor";
        public const string DIRECTOR_ROLE = "Director";
        
        public long Id { get; set; }
        public string Name { get; set; }
           
    }
}