using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FilmSearch.Models
{
   public class FilmAward
    {
        public long FilmId { get; set; }
        public Film Film { get; set; }

        public long AwardId { get; set; }
        public Award Award { get; set; }
    }
}
