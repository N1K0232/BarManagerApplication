using BackendGestionaleBar.Shared.Models.Requests;
using FluentValidation;

namespace BackendGestionaleBar.BusinessLayer.Validators
{
    public class SaveCategoryValidator : AbstractValidator<SaveCategoryRequest>
    {
        public SaveCategoryValidator()
        {
            RuleFor(c => c.Name)
                .NotEmpty()
                .NotNull()
                .WithMessage("can't register a category with no name");
        }
    }
}