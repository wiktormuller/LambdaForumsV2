using FluentValidation;
using LambdaForums.Domain.Entities;

namespace LambdaForums.Infrastructure.Configuration
{
    public class PostValidator : AbstractValidator<Post>
    {
        public PostValidator()
        {
            RuleFor(x => x.Id).NotNull();
            RuleFor(x => x.Title).Length(1,30).NotNull();
            RuleFor(x => x.Content).Length(1,5000).NotNull();
            RuleFor(x => x.Created).NotNull();
            RuleFor(x => x.User).NotNull();
            RuleFor(x => x.Forum).NotNull();
            RuleForEach(x => x.Replies).NotNull();
        }
    }
}
