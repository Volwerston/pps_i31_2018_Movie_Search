using System;
using System.Collections.Generic;

namespace FilmSearch.Models
{
    public class SortedSearchResponse<T, K>
    {
        public List<T> Data { get; set; }
        public K Filter { get; set; }
        public SortQuery SortQuery { get; set; }
        public K AwardFilter { get; set; }
    }
}