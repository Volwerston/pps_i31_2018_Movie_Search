using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using FilmSearch.DAL;
using Microsoft.AspNetCore.Mvc;
using FilmSearch.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using FilmSearch.Migrations;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;

namespace FilmSearch.Controllers
{
    public class HomeController : Controller
    {

        private ILogger logger;
        private IHostingEnvironment environment;

        public HomeController(ILogger<HomeController> _logger, IHostingEnvironment _env)
        {
            logger = _logger;
            environment = _env;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Error()
        {

            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            if (exceptionFeature != null)
            {
                string exceptionRoute = exceptionFeature.Path;

                Exception exception = exceptionFeature.Error;

                while(exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }

                CustomFileLogger.Instance.Log(
                entry: new LogEntry()
                {
                    AddingTime = DateTime.Now,
                    Data = exception,
                    Status = LogEntryStatus.ERROR
                },
                dirPath: $"{environment.ContentRootPath}/logs");
            }

            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
