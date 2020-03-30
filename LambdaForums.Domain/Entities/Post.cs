using System;
using System.Collections.Generic;

namespace LambdaForums.Domain.Entities
{
    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }

        public virtual ApplicationUser User { get; set; } //it is a creator of post
        public virtual Forum Forum { get; set; } //each post have to be related to particular forum
        
        public virtual IEnumerable <PostReply> Replies { get; set; } //each post can have collection of replies
    }
}
