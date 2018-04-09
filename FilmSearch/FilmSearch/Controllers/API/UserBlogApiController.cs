﻿using System;
using System.Security.Claims;
using FilmSearch.Models;
using FilmSearch.Services;
using Microsoft.AspNetCore.Mvc;

namespace FilmSearch.Controllers.API
{
    [Route("api/post")]
    public class UserBlogApiController: Controller
    {
        private UserBlogService _blogService;

        public UserBlogApiController(UserBlogService blogService)
        {
            _blogService = blogService;
        }
        
        private string GetUserId() => this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        [HttpPost]
        public IActionResult AddBlog([FromBody] PostView postView)
        {
            var post = new Post
            {
                AuthorId = GetUserId(),
                CreationTime = DateTime.Now,
                Title = postView.Title,
                ShortDescription = postView.ShortDescription,
                ImageId = postView.ImageId,
                Text = postView.Text
            };
            
            return new ObjectResult(_blogService.SaveUserPost(post));
        }
    }
}