using BackendGestionaleBar.Shared.Requests;
using FluentValidation;

namespace BackendGestionaleBar.BusinessLayer.Validators;

internal class LoginRequestValidator : AbstractValidator<LoginRequest>
{
	public LoginRequestValidator()
	{
		RuleFor(l => l.UserName)
			.NotNull()
			.NotEmpty()
			.WithMessage("the username is required");

		RuleFor(l => l.Password)
			.NotNull()
			.NotEmpty()
			.WithMessage("the password is required");
	}
}