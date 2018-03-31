using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmSearch.Models
{
    public interface IViewModelConverter<TSource, TResult>
    {
        TResult Convert(TSource source);
    }
}
