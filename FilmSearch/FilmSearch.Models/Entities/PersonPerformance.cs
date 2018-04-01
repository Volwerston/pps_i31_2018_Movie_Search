using System;

namespace FilmSearch.Models
{
    public class PersonPerformance
    {
        public long Id { get; set; }
        
        public string UserId { get; set; }
        public AppUser User { get; set; }
        
        public long Performance { get; set; }
        
        public long PersonRoleId { get; set; }
        public PersonRole PersonRole { get; set; }
    }
}