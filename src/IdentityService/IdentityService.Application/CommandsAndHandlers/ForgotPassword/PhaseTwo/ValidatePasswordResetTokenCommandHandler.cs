using System;
using System.Threading;
using System.Threading.Tasks;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Domain.DomainEntities.UserAggregate;
using Microsoft.EntityFrameworkCore;
using SharedKernel.DomainContracts;
using URF.Core.Abstractions.Trackable;

namespace IdentityService.Application.CommandsAndHandlers.ForgotPassword.PhaseTwo;

public class ValidatePasswordResetTokenCommandHandler : ICommandHandler<ValidatForgotPasswordTokenCommand, bool>
{
    public ValidatePasswordResetTokenCommandHandler(
        IMyUnitOfWork unitOfWork,
        ITrackableRepository<User> userRepository
    )
    {
        UnitOfWork = unitOfWork;
        UserRepository = userRepository;
    }

    private IMyUnitOfWork UnitOfWork { get; }
    private ITrackableRepository<User> UserRepository { get; }

    public async Task<bool> Handle(ValidatForgotPasswordTokenCommand command, CancellationToken cancellationToken)
    {
        if (command == null) throw new ArgumentNullException("ValidateResetTokenRequest invalid");
        if (string.IsNullOrEmpty(command.Token))
            throw new ArgumentNullException("ValidateResetTokenRequest token invalid");

        var applicationUser = await UserRepository.Queryable().SingleOrDefaultAsync(x =>
            x.ResetToken == command.Token, cancellationToken);

        applicationUser.VerifyPasswordResetTokenThenResetPassword(command.Token, command.Origin);

        return applicationUser != null;
    }
}