using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmSearch.Controllers
{
    public class MailParams
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string UserPassword { get; set; }
    }
}
