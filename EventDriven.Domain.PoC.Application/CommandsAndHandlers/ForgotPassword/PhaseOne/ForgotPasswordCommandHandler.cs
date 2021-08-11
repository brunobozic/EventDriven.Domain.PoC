using System;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Application.DomainServices.EmailServices;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork.Interfaces;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using Microsoft.EntityFrameworkCore;
using Serilog;
using URF.Core.Abstractions.Trackable;

namespace EventDriven.Domain.PoC.Application.CommandsAndHandlers.ForgotPassword.PhaseOne
{
    public class ForgotPasswordCommandHandler : ICommandHandler<InitiateForgotPasswordCommand, bool>
    {
        public ForgotPasswordCommandHandler(
            IMyUnitOfWork unitOfWork,
            ITrackableRepository<User> userRepository,
            ITrackableRepository<Role> roleRepository,
            IEmailService emailService
        )
        {
            UnitOfWork = unitOfWork;
            UserRepository = userRepository;
            RoleRepository = roleRepository;
            EmailService = emailService;
        }

        private IMyUnitOfWork UnitOfWork { get; }
        private ITrackableRepository<Role> RoleRepository { get; }
        public IEmailService EmailService { get; }
        private ITrackableRepository<User> UserRepository { get; }

        public async Task<bool> Handle(InitiateForgotPasswordCommand command, CancellationToken cancellationToken)
        {
            User user = null;

            if (!string.IsNullOrEmpty(command.Email))
                user = await UserRepository.Queryable().AsNoTrackingWithIdentityResolution()
                    .SingleOrDefaultAsync(user => user.NormalizedEmail == command.Email, cancellationToken);
            else if (!string.IsNullOrEmpty(command.UserName))
                user = await UserRepository.Queryable().AsNoTrackingWithIdentityResolution()
                    .SingleOrDefaultAsync(user => user.NormalizedUserName == command.UserName, cancellationToken);

            try
            {
                // create reset token that expires after 1 day
                user.CreatePasswordResetToken(RandomTokenString());
                UserRepository.Update(user);
                var saveResult = await UnitOfWork.SaveChangesAsync(cancellationToken);

                // send email
                SendPasswordResetEmail(user, command.Origin);

                return true;
            }
            catch (Exception ex)
            {
                Log.Error("Problem recovering password for user with email: [ " + user.Email + " ]", ex);

                return false;
            }
        }

        private void SendPasswordResetEmail(User applicationUser, string origin)
        {
            string message;

            if (!string.IsNullOrEmpty(origin))
            {
                var resetUrl = $"{origin}/applicationUser/reset-password?token={applicationUser.ResetToken}";
                message =
                    $@"<p>Please click the below link to reset your password, the link will be valid for 1 day:</p>
                             <p><a href=""{resetUrl}"">{resetUrl}</a></p>";
            }
            else
            {
                message =
                    $@"<p>Please use the below token to reset your password with the <code>/applicationUsers/reset-password</code> api route:</p>
                             <p><code>{applicationUser.ResetToken}</code></p>";
            }

            EmailService.Send(
                applicationUser.Email,
                "Sign-up Verification API - Reset Password",
                $@"<h4>Reset Password Email</h4>
                         {message}"
            );
        }

        private string RandomTokenString()
        {
            using var rngCryptoServiceProvider = new RNGCryptoServiceProvider();
            var randomBytes = new byte[40];
            rngCryptoServiceProvider.GetBytes(randomBytes);

            // convert random bytes to hex string
            return BitConverter.ToString(randomBytes).Replace("-", "");
        }
    }
}