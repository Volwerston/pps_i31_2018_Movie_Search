using FilmSearch.DAL;
using FilmSearch.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FilmSearch.Services
{
    public class UserService
    {
        public const int PageSize = 10;

        private UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userMgr)
        {
            _userManager = userMgr;
        }

        public (IEnumerable<AppUser>, int) GetUsersByEmailPaginated(string email, int page)
        {
            email = email ?? "";

            var persons = _userManager.Users;

            if (!string.IsNullOrWhiteSpace(email))
            {
                persons = persons.Where(user => user.Email.ToLower().Contains(email.Trim().ToLower()));
            }

            return (persons.Skip(PageSize * (page - 1)).Take(PageSize).ToList(), persons.Count());
        }
    }
}
