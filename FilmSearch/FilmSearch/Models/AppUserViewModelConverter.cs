using FilmSearch.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FilmSearch.Models
{
    public class AppUserViewModelConverter : IViewModelConverter<AppUser, AppUserViewModel>
    {
        public AppUserViewModel Convert(AppUser source)
        {
            return new AppUserViewModel()
            {
                Name = source.UserName,
                Surname = source.Surname
            };
        }
    }
}
