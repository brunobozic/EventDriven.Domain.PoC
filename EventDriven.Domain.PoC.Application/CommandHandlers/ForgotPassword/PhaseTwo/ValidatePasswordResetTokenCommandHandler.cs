using System;
using System.Threading;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using Microsoft.EntityFrameworkCore;
using URF.Core.Abstractions.Trackable;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.ForgotPassword.PhaseTwo
{
    public class ValidatePasswordResetTokenCommandHandler : ICommandHandler<ValidatePasswordResetTokenCommand, bool>
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


        public async Task<bool> Handle(ValidatePasswordResetTokenCommand command, CancellationToken cancellationToken)
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
}