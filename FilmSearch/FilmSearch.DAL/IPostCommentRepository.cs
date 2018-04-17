using System.Collections.Generic;
using FilmSearch.Models;

namespace FilmSearch.DAL
{
    public interface IPostCommentRepository: IRepository<PostComment>
    {
        List<PostComment> GetPostComments(long postId);
    }
}