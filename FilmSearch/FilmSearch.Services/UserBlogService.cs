using System.Collections.Generic;
using System.Linq;
using FilmSearch.DAL;
using FilmSearch.Models;

namespace FilmSearch.Services
{
    public class UserBlogService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserBlogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public Post SaveUserPost(Post post)
        {
            _unitOfWork.PostRepository.Add(post);
            _unitOfWork.Save();

            return post;
        }

        public List<PostView> GetPosts()
        {
            return _unitOfWork.PostRepository.GetAll().Select(ConvertPostToView).ToList();
        }

        public PostView GetPost(long id)
        {
            return ConvertPostToView(_unitOfWork.PostRepository.GetByKey(id));
        }

        private PostView ConvertPostToView(Post p)
        {
            return new PostView
            {
                Id = p.Id,
                Title = p.Title,
                ImageId = p.ImageId,
                ShortDescription = p.ShortDescription,
                Text = p.Text,
                AuthorName = $"{p.Author.UserName} {p.Author.Surname}",
                PostDate = p.CreationTime
            };
        }
    }
}