using LambdaForums.Domain.Interfaces;
using LambdaForums.Domain.Entities;
using LambdaForums.Web.Models.Forum;
using LambdaForums.Web.Models.Post;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace LambdaForums.Web.Controllers
{
    public class ForumController : Controller //inherit from Controller base class
    {
        private readonly IForum _forumService; //field is always of interface type
        private readonly IPost _postService;
        private readonly IUpload _uploadService;
        private readonly IConfiguration _configuration;

        public ForumController(IForum forumService, IPost postService, IUpload uploadService, IConfiguration configuration) //it's not a concrete instance of our forum service but rather we are passing in this IForum interface and then we are gonna use dependency injection here and we are gonna tell that anytime we as for something that implements this IForum interface to go ahead and pass that concrete instance of our forum service
        {
            _forumService = forumService;
            _postService = postService;
            _uploadService = uploadService;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var forums = _forumService.GetAll() //var is IEnumerable<ForumListingModel>
                .Select(forum => new ForumListingModel {
                    Id = forum.Id,
                    Name = forum.Title,
                    Description = forum.Description,
                    NumberOfPosts = forum.Posts?.Count() ?? 0,
                    NumberOfUsers = _forumService.GetActiveUsers(forum.Id).Count(),
                    ImageUrl = forum.ImageUrl,
                    HasRecentPost = _forumService.HasRecentPost(forum.Id)
                });

            var model = new ForumIndexModel
            {
                ForumList = forums.OrderBy(f => f.Name)
            };

            return View(model); //here we can return a view of name of our controller or status code or some JSON data
        }

        public IActionResult Topic(int id, string searchQuery)
        {
            var forum = _forumService.GetById(id);  //assign the instance of the particular forum
            var posts = new List<Post>();

            posts = _postService.GetFilteredPosts(forum, searchQuery).ToList();

            //posts = forum.Posts.ToList();    //assign all the post from the particular forum
            //var posts = _postService.GetPostsByForum(id);

            var postListings = posts.Select(post => new PostListingModel    //create a new ViewModel instance and map all raw posts into view model named post listing model
            {
                Id = post.Id,
                AuthorId = post.User.Id,
                AuthorName = post.User.UserName,
                AuthorRating = post.User.Rating,
                Title = post.Title,
                DatePosted = post.Created.ToString(), //store as a string because we can control the way that we display this down on the page 
                RepliesCount = post.Replies.Count(),
                Forum = BuildForumListing(post) //it's a reference to this forum
            });

            var model = new ForumTopicModel //create a instance of viewmodel - posts are our postlistingmodel and forum is 
            {
                Posts = postListings,
                Forum = BuildForumListing(forum)
            };

            return View(model);
        }

        [HttpPost]
        public IActionResult Search(int id, string searchQuery) //return the same forum view back only containing posts that only correspond to some search query that we provide that a search form
        {
            return RedirectToAction("Topic", new { id, searchQuery });
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            var model = new AddForumModel();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddForum(AddForumModel model)
        {
            var imageUri = "/images/forum/default.png";

            if (model.ImageUpload != null)
            {
                var blockBlob = UploadForumImage(model.ImageUpload);
                imageUri = blockBlob.Uri.AbsoluteUri;

            }

            var forum = new Forum
            {
                Title = model.Title,
                Description = model.Description,
                Created = DateTime.Now,
                ImageUrl = imageUri
            };

            await _forumService.Create(forum);
            return RedirectToAction("Index", "Forum");
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            await _forumService.Delete(id);
            return RedirectToAction("Index", "Home");
        }

        private CloudBlockBlob UploadForumImage(IFormFile file)
        {
            var connectionString = _configuration.GetConnectionString("AzureStorageAccount");
            var container = _uploadService.GetBlobContainer(connectionString, "forum-images");

            var contentDisposition = ContentDispositionHeaderValue.Parse(file.ContentDisposition);
            var filename = contentDisposition.FileName.Trim('"');

            var blockBlob = container.GetBlockBlobReference(filename);

            blockBlob.UploadFromStreamAsync(file.OpenReadStream());

            return blockBlob;
        }

        private ForumListingModel BuildForumListing(Post post) //take instance of a post and build our a new forum listing model from it
        {
            var forum = post.Forum; //save the forum to which belongs the post
            return BuildForumListing(forum);    //pass the forum to build a new forum listing model
        }

        private ForumListingModel BuildForumListing(Forum forum)    //build new forum listing model depends on forum to which is owner of a particular post
        {
            return new ForumListingModel
            {
                Id = forum.Id,
                Name = forum.Title,
                Description = forum.Description,
                ImageUrl = forum.ImageUrl
            };
        }
    }
}