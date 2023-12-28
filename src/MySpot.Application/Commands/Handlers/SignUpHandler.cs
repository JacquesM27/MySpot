using MySpot.Application.Abstractions;
using MySpot.Application.Exceptions;
using MySpot.Application.Security;
using MySpot.Core.Abstractions;
using MySpot.Core.Entities;
using MySpot.Core.Repositories;
using MySpot.Core.ValueObjects;

namespace MySpot.Application.Commands.Handlers
{
    internal sealed class SignUpHandler(IClock clock,
                                        IPasswordManager passwordManager,
                                        IUserRepository userRepository)
        : ICommandHandler<SignUp>
    {
        public async Task HandleAsync(SignUp command)
        {
            if (await userRepository.GetByEmailAsync(command.Email) is not null)
                throw new EmailAlreadyInUseException(command.Email);

            if (await userRepository.GetByUsernameAsync(command.Username) is not null)
                throw new UsernameAlreadyInUseException(command.Username);

            var role = string.IsNullOrWhiteSpace(command.Role) ? Role.User() : new Role(command.Role);
            var securedPassword = passwordManager.Secure(command.Password);
            var user = new User(command.UserId, command.Email, command.Username, securedPassword, command.Fullname, role, clock.Current());
            await userRepository.AddAsync(user);
        }
    }
}
