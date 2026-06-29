using FlightScan.Application.Errors;
using FlightScan.Core.Enums;
using FlightScan.Core.Interfaces;
using FlightScan.Core.Responses.Common;
using FluentValidation;
using MediatR;

namespace FlightScan.Application.Cqrs.Commands.User
{
    public class CreateUserCommand : IRequest<CreateResponse>
    {
        public UserRole Role { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        public CreateUserCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            ClassLevelCascadeMode = CascadeMode.Stop;
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage(ValidationMessages.usernameRequired)
                .MinimumLength(8).WithMessage(ValidationMessages.usernameMinChars)
                .MaximumLength(50).WithMessage(ValidationMessages.usernameMaxChars)
                .MustAsync(async (username, ct) => await _userRepository.GetByUsernameAsync(username) == null)
                .WithMessage(ValidationMessages.usernameExists);

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage(ValidationMessages.passwordRequired)
                .MinimumLength(8).WithMessage(ValidationMessages.passwordMinChars)
                .MaximumLength(50).WithMessage(ValidationMessages.passwordMaxChars);

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage(ValidationMessages.roleInvalid)
                .Must(r => r != UserRole.Administrator).WithMessage(ValidationMessages.administratorRoleInvalid);
        }
    }
}
