using Microsoft.AspNetCore.Identity;
using System;

namespace LambdaForums.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        // we will just continue to expand the definition of our application user directly in this class
        public int Rating { get; set; }
        public string ProfileImageUrl { get; set; }
        public DateTime MemberSince { get; set; }
        public bool IsActive { get; set; }
    }
}
