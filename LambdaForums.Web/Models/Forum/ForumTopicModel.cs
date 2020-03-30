using LambdaForums.Web.Models.Post;
using System.Collections.Generic;

namespace LambdaForums.Web.Models.Forum
{
    //represents our forum build as a forumlistingmodel to get some basic info about forum
    //and our posts collection build as a postlisting model
    public class ForumTopicModel
    {
        public ForumListingModel Forum { get; set; }
        public IEnumerable<PostListingModel> Posts { get; set; }
        public string SearchQuery { get; set; }
    }
}
