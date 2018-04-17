using System.Collections.Generic;
using FilmSearch.Models;

namespace FilmSearch.DAL
{
    public interface IPostRepository: IRepository<Post>
    {
        List<Post> PostsByUserId(string userId);
    }
}