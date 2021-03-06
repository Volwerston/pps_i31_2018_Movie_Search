﻿namespace FilmSearch.Models
{
    public class PersonRole
    {
        public long Id { get; set; }
        public double Performance { get; set; }
        
        public long FilmRoleId { get; set; }
        public FilmRole FilmRole { get; set; }
        
        public long PersonId { get; set; }
        public Person Person { get; set; }
        
        public long FilmId { get; set; }
        public Film Film { get; set; }

        public string Description { get; set; }
    }
}