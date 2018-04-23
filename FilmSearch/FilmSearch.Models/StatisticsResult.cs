using System;
using System.Collections.Generic;
using System.Text;
using FilmSearch.Models.DTO;

namespace FilmSearch.Models
{
    public class StatisticsResult<ChartDTO>
    {
        public List<LineDateChartDTO> ChartInfo;
        public List<ChartDTO> ChartDtos;
    }
}
