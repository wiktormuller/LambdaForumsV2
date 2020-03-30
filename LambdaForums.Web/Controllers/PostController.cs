using LambdaForums.Domain.Interfaces;
using LambdaForums.Domain.Entities;
using LambdaForums.Web.Models.Post;
using LambdaForums.Web.Models.Reply;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using LambdaForums.Infrastructure.Configuration;
using FluentValidation.Results;

namespace LambdaForums.Web.Controllers
{
    public class PostController : Controller
    {
        private readonly IPost _postService;
        private readonly IForum _forumService;
        private readonly IApplicationUser _userService;

        private static UserManager<ApplicationUser> _userManager;

        public PostController(IPost postService, IForum forumService, UserManager<ApplicationUser> userManager, IApplicationUser userService)
        {
            _postService = postService;
            _forumService = forumService;
            _userManager = userManager;
            _userService = userService;
        }

        public IActionResult Index(int id)
        {
            var post = _postService.GetById(id);    //getting a post back from our post service by its id 

            var replies = BuildPostReplies(post.Replies);   //we dont want to pass a collection of our reply entity model down to the views so want to do method to build postreplymodel

            var model = new PostIndexModel  //map post entity into postindexmodel - this view model depeneds on replies made by reply entity into postreply model
            {
                Id = post.Id,
                Title = post.Title,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorImageUrl = post.User.ProfileImageUrl,
                AuthorRating = post.User.Rating,
                Created = post.Created,
                PostContent = post.Content,
                Replies = replies,   //store replies as a variable
                ForumId = post.Forum.Id,
                ForumName = post.Forum.Title,
                IsAuthorAdmin = IsAuthorAdmin(post.User)
            };

            return View(model);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var post = _postService.GetById(id);

            await _postService.Delete(id);
            return RedirectToAction("Index", "Forum");
        }

        [HttpGet]
        [Authorize]
        public IActionResult Create(int id) //this is forum id
        {
            var forum = _forumService.GetById(id);

            var model = new NewPostModel {
                ForumName = forum.Title,
                ForumId = forum.Id,
                ForumImageUrl = forum.ImageUrl,
                AuthorName = User.Identity.Name //take the user name from actual HTTP context in which we were working so when I am logged in and visiting the page this represents me
            };

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddPost(NewPostModel model)    //the NewPostModel is passed from the view based on empty NewPostModel passed there by get method and fulfilled by user
        {
            if (!ModelState.IsValid)    // re-render the view when validation failed.
            {
                return RedirectToAction("Create", "Post", model.ForumId);
            }

            var userId = _userManager.GetUserId(User);  //take the user id by UserManager service
            var user = _userManager.FindByIdAsync(userId).Result;    //to find the instance of that application user object from our data store
            var post = BuildPost(model, user);

            await _postService.Add(post); //use our post service to invoke EF to make that insert of our post into data store  | should block the current thread until the task is complete
            await _userService.UpdateUserRating(userId, typeof(Post));

            return RedirectToAction("Index", "Post", new { id = post.Id });  //to redirect to action we are gonna specify (action, controller, id)
        }

        private Post BuildPost(NewPostModel model, ApplicationUser user)
        {
            var forum = _forumService.GetById(model.ForumId);

            return new Post
            {
                Title = model.Title,
                Content = model.Content,
                Created = DateTime.Now,
                User = user,
                Forum = forum   //what forum it is getting posted to
            };
        }

        private bool IsAuthorAdmin(ApplicationUser user)    //if roles of the user contains Admin value then return a true
        {
            return _userManager.GetRolesAsync(user)
                .Result.Contains("Admin");
        }

        private IEnumerable<PostReplyModel> BuildPostReplies(IEnumerable<PostReply> replies)
        {
            return replies.Select(reply => new PostReplyModel {
                Id = reply.Id,
                AuthorName = reply.User.UserName,
                AuthorId = reply.User.Id,
                AuthorImageUrl = reply.User.ProfileImageUrl,
                AuthorRating = reply.User.Rating,
                Created = reply.Created,
                ReplyContent = reply.Content,
                IsAuthorAdmin = IsAuthorAdmin(reply.User)
            });
        }
    }
}