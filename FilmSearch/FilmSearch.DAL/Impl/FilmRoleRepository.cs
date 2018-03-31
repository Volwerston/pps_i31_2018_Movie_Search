using System.Linq;
using FilmSearch.Models;

namespace FilmSearch.DAL.Impl
{
    public class FilmRoleRepository: GenericRepository<FilmRole>, IFilmRoleRepository
    {
        public FilmRoleRepository(FilmSearchContext context) : base(context)
        {
        }

        public FilmRole GetByRoleName(string roleName)
        {
            return _dbSet.FirstOrDefault(fr => roleName.Equals(fr.Name));
        }
    }
}