using System;

namespace LambdaForums.Web.Models.Reply
{
    public class PostReplyModel //we cannot post replies to replies
    {
        public int Id { get; set; }

        public string AuthorId { get; set; }
        public string AuthorName { get; set; }
        public int AuthorRating { get; set; }
        public string AuthorImageUrl { get; set; }
        public bool IsAuthorAdmin { get; set; }

        public DateTime Created { get; set; }
        public string ReplyContent { get; set; }

        public int PostId { get; set; } //to which post this reply belongs
        public string PostTitle { get; set; }
        public string PostContent { get; set; }

        public string ForumName { get; set; }
        public string ForumImageUrl { get; set; }
        public int ForumId { get; set; }
    }
}
