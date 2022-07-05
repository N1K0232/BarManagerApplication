using BackendGestionaleBar.Shared.Requests;
using FluentValidation;

namespace BackendGestionaleBar.BusinessLayer.Validators;

internal class RefreshTokenRequestValidator : AbstractValidator<RefreshTokenRequest>
{
	public RefreshTokenRequestValidator()
	{
		RuleFor(r => r.AccessToken)
			.NotNull()
			.NotEmpty()
			.WithMessage("the access token is required");

		RuleFor(r => r.RefreshToken)
			.NotNull()
			.NotEmpty()
			.WithMessage("the refresh token is required");
	}
}