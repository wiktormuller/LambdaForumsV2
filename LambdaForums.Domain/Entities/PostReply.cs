using System;

namespace LambdaForums.Domain.Entities
{
    public class PostReply
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }

        public virtual ApplicationUser User { get; set; } //who is the owner of reply
        public virtual Post Post { get; set; } //to which post is the reply
    }
}
