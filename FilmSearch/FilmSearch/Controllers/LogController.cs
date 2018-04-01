using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FilmSearch.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace FilmSearch.Controllers
{
    public class LogController : Controller
    {
        private IHostingEnvironment environment;

        public LogController(IHostingEnvironment _env)
        {
            environment = _env;
        }

        public IActionResult View()
        {
            string dirPath = $"{environment.ContentRootPath}/logs";

            IEnumerable<LogEntry> logEntries = CustomFileLogger.Instance.Extract(DateTime.Now, dirPath);

            return View(logEntries);
        }

        public IActionResult GetEntriesByDate(DateTime time)
        {
            string dirPath = $"{environment.ContentRootPath}/logs";

            IEnumerable<LogEntry> logEntries = CustomFileLogger.Instance.Extract(time, dirPath);

            return Json(logEntries);
        }
    }
}