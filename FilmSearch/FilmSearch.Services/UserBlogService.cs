using System;
using System.Collections.Generic;
using System.Linq;
using FilmSearch.DAL;
using FilmSearch.Models;
using FilmSearch.Models.DTO;
using FilmSearch.Utils;
using Microsoft.AspNetCore.Identity;

namespace FilmSearch.Services
{
    public class UserBlogService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public UserBlogService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public Post SaveUserPost(Post post)
        {
            _unitOfWork.PostRepository.Add(post);
            _unitOfWork.Save();

            return post;
        }

        public List<PostView> GetPosts()
        {
            return _unitOfWork.PostRepository.GetAll().Select(PostView.from).ToList();
        }

        public PostView GetPost(long id)
        {
            return PostView.from(_unitOfWork.PostRepository.GetByKey(id));
        }

        public AppUser GerPostWriter(long id)
        {
            var post = _unitOfWork.PostRepository.GetByKey(id);

            return _userManager.Users.Where(x => x.Id == post.AuthorId).First();
        }

        public UserBlogView GetUserBlogView(string userId)
        {
            var userPosts = _unitOfWork.PostRepository.PostsByUserId(userId);
            var user = _userManager.FindByIdAsync(userId).Result;

            return new UserBlogView()
            {
                AppUser = user,
                Posts = userPosts.Select(PostView.from).ToList()
            };
        }

        public void AddComment(UserBlogCommentModel commentModel, String userId)
        {
            var comment = new PostComment
            {
                AuthorId = userId,
                CommentRate = 0,
                CreationDate = DateUtils.ParseDate(DateTime.Now),
                SubComments = new List<Comment>(),
                Text = commentModel.Text,
                PostId = commentModel.BlogId
            };
            if (commentModel.ParentId != 0)
            {
                comment.ParentCommentId = commentModel.ParentId;
            }
            
            _unitOfWork.PostCommentRepository.Add(comment);
            _unitOfWork.Save();
        }

        public List<PostCommentChartDTO> GetCommentChartList(IEnumerable<PostComment> pc, string personEmail)
        {
            List<PostCommentChartDTO> toReturn = new List<PostCommentChartDTO>();

            foreach (var postComment in pc)
            {
                Post post = _unitOfWork.PostRepository.GetByKey(postComment.PostId);
                AppUser postAuthor = _userManager.FindByIdAsync(postComment.AuthorId).Result;

                PostCommentChartDTO toAdd = new PostCommentChartDTO()
                {
                    Author =  personEmail,
                    Date =  postComment.CreationDate,
                    PostTitle = post.Title,
                    WrittenBy = $"{postAuthor.UserName} {postAuthor.Surname}",
                    Text = postComment.Text
                };

                toReturn.Add(toAdd);
            }

            return toReturn;
        }

        public List<PostComment> GetPostComments(long postId)
        {
            var comments = _unitOfWork.PostCommentRepository.GetPostComments(postId);
            var commentsMap = comments.ToDictionary(c => c.Id);
            
            comments.ForEach(c =>
            {
                c.SubComments = new List<Comment>();
                foreach (var postComment in comments)
                {
                    if (postComment.ParentCommentId == c.Id)
                    {
                        c.SubComments.Add(postComment);
                    }
                }
            });

            return comments.Where(c => c.ParentCommentId == null).ToList();
        }

        public void DeleteComment(long commentId)
        {
            var comment = _unitOfWork.PostCommentRepository.GetByKey(commentId);
            if (comment.SubComments?.Count == 0)
            {
                _unitOfWork.PostCommentRepository.Delete(commentId);
                _unitOfWork.Save();
            }
        }
    }
}