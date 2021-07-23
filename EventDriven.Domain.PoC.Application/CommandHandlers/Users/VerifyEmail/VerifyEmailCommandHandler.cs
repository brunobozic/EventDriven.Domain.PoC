using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Response;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using Microsoft.EntityFrameworkCore;
using Serilog;
using URF.Core.Abstractions.Trackable;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.Users.VerifyEmail
{
    public class VerifyEmailCommandHandler : ICommandHandler<VerifyEmailCommand, VerifyEmailResponse>
    {
        public VerifyEmailCommandHandler(
            IMyUnitOfWork unitOfWork,
            ITrackableRepository<User> userRepository
        )
        {
            UnitOfWork = unitOfWork;
            UserRepository = userRepository;
        }

        private IMyUnitOfWork UnitOfWork { get; }
        private ITrackableRepository<User> UserRepository { get; }

        public async Task<VerifyEmailResponse> Handle(VerifyEmailCommand command, CancellationToken cancellationToken)
        {
            var retVal = new VerifyEmailResponse();

            var user = await UserRepository
                .Queryable()
                .Where(user => user.EmailVerificationToken.Trim() == command.EmailVerificationToken.Trim())
                .SingleOrDefaultAsync(cancellationToken);

            if (user == null)
            {
                var explanation =
                    $"No user (email: [{command.Email}], username: [{command.UserName}]) with provided token was found, likely the email verification token [ {command.EmailVerificationToken} ] had already been consumed.";
                Log.Error(explanation);
                retVal.Message = explanation;

                return retVal;
            }

            var verificationStatus = user.SetEmailIsVerified();

            await UnitOfWork.SaveChangesAsync(cancellationToken);

            if (verificationStatus == "OK")
            {
                retVal.Success = true;
                retVal.Message = $"Your email ([{command.Email}]) has now been verified, you may now log in.";
            }

            retVal.Message = verificationStatus;

            return retVal;
        }
    }
}