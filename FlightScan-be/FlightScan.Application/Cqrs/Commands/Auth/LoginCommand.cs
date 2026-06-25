using FlightScan.Core.Responses.Auth;
using FluentValidation;
using MediatR;

namespace FlightScan.Application.Cqrs.Commands.Auth
{
    public class LoginCommand : IRequest<LoginResponse>
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Username).NotEmpty().WithMessage("Username should not be empty.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Password should not be empty.");
        }
    }
}
