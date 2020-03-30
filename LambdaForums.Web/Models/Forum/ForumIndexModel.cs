using System.Collections.Generic;

namespace LambdaForums.Web.Models.Forum
{
    public class ForumIndexModel
    {
        public IEnumerable<ForumListingModel> ForumList { get; set; }
    }
}
