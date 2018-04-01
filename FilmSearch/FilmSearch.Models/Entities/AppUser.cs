using Microsoft.AspNetCore.Identity;

namespace FilmSearch.Models
{
    public class AppUser: IdentityUser
    {
        public string Surname { get; set; }
    }
}