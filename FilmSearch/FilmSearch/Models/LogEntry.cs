using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmSearch.Models
{
    public class LogEntry
    {
        public object Data { get; set; }
        public string Status { get; set; }
        public DateTime AddingTime { get; set; }
    }
}
