using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmSearch.Models
{
    public interface ICustomLogger
    {
        void Log(LogEntry entry, string dirPath);

        IEnumerable<LogEntry> Extract(DateTime date, string dirPath);
    }
}
