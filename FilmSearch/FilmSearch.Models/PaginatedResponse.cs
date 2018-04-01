using System.Collections.Generic;

namespace FilmSearch.Models
{
    public class PaginatedResponse<T>
    {
        public List<T> Data { get; set; }
        public int Count { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
    }
}