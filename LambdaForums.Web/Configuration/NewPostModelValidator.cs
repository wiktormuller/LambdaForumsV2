using FluentValidation;
using LambdaForums.Web.Models.Post;

namespace LambdaForums.Web.Configuration
{
    public class NewPostModelValidator : AbstractValidator<NewPostModel>
    {
        public NewPostModelValidator()
        {
            RuleFor(x => x.Title).Length(1,30).NotNull().WithMessage("Type the title.");
            RuleFor(x => x.Content).Length(1,5000).NotNull().WithMessage("Type the content.");
        }
    }
}
