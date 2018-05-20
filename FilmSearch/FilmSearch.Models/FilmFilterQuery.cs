using System.Collections.Generic;

namespace FilmSearch.Models
{
    public class FilmFilterQuery
    {
        public string Title { get; set; }
        public long PlaywriterId { get; set; }
        public string Playwriter { get; set; }
        public List<long> Awards { get; set; }
    }
}