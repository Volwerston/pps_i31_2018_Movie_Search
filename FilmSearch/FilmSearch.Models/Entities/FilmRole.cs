using System;
using System.Collections.Generic;

namespace FilmSearch.Models
{
    public class FilmRole
    {
        public const string ACTOR_ROLE = "Actor";
        public const string DIRECTOR_ROLE = "Director";
        public const string PLAYWRITER_ROLE = "Playwriter";
        
        public long Id { get; set; }
        public string Name { get; set; }

        public List<PersonRole> PersonRoles { get; set; } = new List<PersonRole>();
    }
}