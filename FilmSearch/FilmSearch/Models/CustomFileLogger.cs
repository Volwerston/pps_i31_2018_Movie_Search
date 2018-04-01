using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FilmSearch.Models
{
    public class CustomFileLogger : ICustomLogger
    {

        private static ICustomLogger _logger;

        private CustomFileLogger()
        {
        }
    
        public static ICustomLogger Instance
        {
            get
            {
                if(_logger == null)
                {
                    _logger = new CustomFileLogger();
                }

                return _logger;
            }
        }

        public IEnumerable<LogEntry> Extract(DateTime date, string dirPath)
        {
            string filePath = $"{date.Year}-{date.Month}-{date.Day}.txt";
            string fullPath = $"{dirPath}/{filePath}";

            List<LogEntry> entries = new List<LogEntry>();

            FillEntries(fullPath, out entries);

            return entries;
        }

        public void Log(LogEntry entry, string dirPath)
        {
            string filePath = $"{DateTime.Now.Year}-{DateTime.Now.Month}-{DateTime.Now.Day}.txt";
            string fullPath = $"{dirPath}/{filePath}";

            List<LogEntry> entries = new List<LogEntry>();

            FillEntries(fullPath, out entries);

            entries.Add(entry);

            SaveToFile(fullPath, entries);
        }


        private void FillEntries(string fullPath, out List<LogEntry> entries)
        {
            using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Read))
            {
                using (StreamReader rdr = new StreamReader(fs))
                {
                    string allData = rdr.ReadToEnd();

                    if (!String.IsNullOrWhiteSpace(allData))
                    {
                        entries = JsonConvert.DeserializeObject<List<LogEntry>>(allData);
                    }
                    else
                    {
                        entries = new List<LogEntry>();
                    }
                }
            }
        }

        private void SaveToFile(string fullPath, List<LogEntry> entries)
        {
            using (FileStream fs = new FileStream(fullPath, FileMode.OpenOrCreate, FileAccess.Write))
            {
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(JsonConvert.SerializeObject(entries));
                }
            }
        }
    }
}
