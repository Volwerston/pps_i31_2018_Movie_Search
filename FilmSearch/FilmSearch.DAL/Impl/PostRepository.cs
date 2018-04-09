using System.Collections.Generic;
using System.Linq;
using FilmSearch.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmSearch.DAL.Impl
{
    public class PostRepository: GenericRepository<Post>, IPostRepository
    {
        public PostRepository(FilmSearchContext context) : base(context)
        {
        }

        public IEnumerable<Post> GetAll()
        {
            return _dbSet.Include(p => p.Author);
        }

        public Post GetByKey(object key)
        {
            return _dbSet.Include(p => p.Author).First(p => p.Id == (long) key);
        }
    }
}