using System.Collections.Generic;
using System.Linq;
using FilmSearch.Models;
using Microsoft.EntityFrameworkCore;

namespace FilmSearch.DAL.Impl
{
    public class PostCommentRepository: GenericRepository<PostComment>, IPostCommentRepository
    {
        public PostCommentRepository(FilmSearchContext context) : base(context)
        {
        }

        public List<PostComment> GetPostComments(long postId)
        {
            return _dbSet
                .Where(p => p.PostId == postId)
                .Include(p => p.Author)
                .ToList();
        }
    }
}