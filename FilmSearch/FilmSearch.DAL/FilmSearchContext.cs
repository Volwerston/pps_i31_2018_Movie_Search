using FilmSearch.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FilmSearch.DAL
{
    public class FilmSearchContext: IdentityDbContext<AppUser>
    {
        public FilmSearchContext(DbContextOptions<FilmSearchContext> options):
            base(options)
        {
        }

        public DbSet<File> Files { get; set; }
    }
}