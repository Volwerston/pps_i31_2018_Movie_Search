using Microsoft.AspNetCore.Identity;

namespace FilmSearch.Models.Auth
{
    public class AppUser : IdentityUser
    {
        public string Surname { get; set; }
    }
}
