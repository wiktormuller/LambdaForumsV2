using LambdaForums.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LambdaForums.Domain.Interfaces
{
    public interface IForum //interface with a few methods defined on it
    {
        Forum GetById(int id); //get a particular instance of forum by id
        IEnumerable<Forum> GetAll(); //get a collection of all the forums from the database

        Task Create(Forum forum); //task is sort of like void in this case will actually represent an asynchronus event
        Task Delete(int forumId);
        Task UpdateForumTitle(int forumId, string newTitle);
        Task UpdateForumDescription(int forumId, string newDescription);
        IEnumerable<ApplicationUser> GetActiveUsers(int id);
        bool HasRecentPost(int id);
    }
}
