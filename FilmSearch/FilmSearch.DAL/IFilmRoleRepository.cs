using FilmSearch.Models;

namespace FilmSearch.DAL
{
    public interface IFilmRoleRepository: IRepository<FilmRole>
    {
        FilmRole GetByRoleName(string roleName);
    }
}