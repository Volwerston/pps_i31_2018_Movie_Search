﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmSearch.Models
{
    public class PersonSearchParams
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Country { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public string Role { get; set; }
        public int LastId { get; set; }
        public int ChunkSize { get; set; }
    }
}
