using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Threading.Tasks;

namespace FilmSearch.Models
{
    public class AppUser: IdentityUser
    {
        public string Surname { get; set; }
    }
}