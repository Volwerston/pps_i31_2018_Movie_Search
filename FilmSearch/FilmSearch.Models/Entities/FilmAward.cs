using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace FilmSearch.Models.Entities
{
   public class FilmAward
    {
        public long Id { get; set; }

        public long FilmId { get; set; }

        public long AwardId { get; set; }
    }
}
