using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmSearch.Models.Helper
{
    public interface ISelectQueryFilter<TParams, TRes>
    {
        IEnumerable<TRes> Filter(TParams param);
    }
}
