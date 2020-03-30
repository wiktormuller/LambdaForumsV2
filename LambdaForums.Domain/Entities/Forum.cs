using System;
using System.Collections.Generic;

namespace LambdaForums.Domain.Entities
{
    public class Forum
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string ImageUrl { get; set; }

        public virtual IEnumerable<Post> Posts { get; set; } //it is a enumerable collection of posts / virtual allow us to lazy load the property in EFCore

    }
}
