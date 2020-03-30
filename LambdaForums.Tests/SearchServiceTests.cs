using LambdaForums.Data;
using LambdaForums.Data.Models;
using LambdaForums.Service;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System;
using System.Linq;

namespace LambdaForums.Tests
{
    [TestFixture]
    public class Post_Service_Should
    {
        //[Test]
        [TestCase("Coffee", 3)]
        [TestCase("Water", 0)]
        [TestCase("TeA", 1)]
        public void Return_Filtered_Results_Corresponding_To_Query(string query, int expected)
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;

            //Arrange -> arrange test scenario (set the stage)
            using (var ctx = new ApplicationDbContext(options))
            {
                ctx.Forums.Add(new Forum
                {
                    Id = 19
                });

                ctx.Posts.Add(new Post
                {
                    Forum = ctx.Forums.Find(19),
                    Id = 23523,
                    Title = "First Post",
                    Content = "Coffee"
                });

                ctx.Posts.Add(new Post
                {
                    Forum = ctx.Forums.Find(19),
                    Id = -523,
                    Title = "Coffee",
                    Content = "Some Content"
                });

                ctx.Posts.Add(new Post
                {
                    Forum = ctx.Forums.Find(19),
                    Id = 23,
                    Title = "Tea",
                    Content = "Coffee"
                });

                ctx.SaveChanges();
            }

            //Act -> perform some action
            using (var ctx = new ApplicationDbContext(options))
            {
                var postService = new PostService(ctx);
                var result = postService.GetFilteredPosts(query);
                var postCount = result.Count();

                //Assert -> assert that what you expected to happen is what happen in your specifically arranged scenario
                Assert.AreEqual(expected, postCount);
            }
        }
    }
}
