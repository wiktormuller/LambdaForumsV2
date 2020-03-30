using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LambdaForums.Domain.Interfaces;
using LambdaForums.Domain.Entities;

namespace LambdaForums.Infrastructure.Services
{
    public class ApplicationUserService : IApplicationUser
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<ApplicationUser> GetAll()
        {
            return _context.ApplicationUsers;
        }

        public ApplicationUser GetById(string id)
        {
            return _context.ApplicationUsers.FirstOrDefault(user => user.Id == id);
        }

        public async Task UpdateUserRating(string userId, Type type)
        {
            var user = GetById(userId);
            user.Rating = CalculateUserRating(type, user.Rating);
            await _context.SaveChangesAsync();
        }

        private int CalculateUserRating(Type type, int userRating)
        {
            var inc = 0;

            if(type == typeof(Post))
            {
                inc = 3;
            }
            if (type == typeof(PostReply))
            {
                inc = 1;
            }

            return userRating + inc;
        }

        public async Task SetProfileImage(string id, Uri uri)
        {
            var user = GetById(id);
            user.ProfileImageUrl = uri.AbsoluteUri;
            _context.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
