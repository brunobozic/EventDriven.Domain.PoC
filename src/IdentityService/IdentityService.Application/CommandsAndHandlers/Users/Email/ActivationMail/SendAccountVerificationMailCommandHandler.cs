using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IdentityService.Application.DomainServices.EmailServices;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Domain.DomainEntities.DomainExceptions;
using IdentityService.Domain.DomainEntities.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using SharedKernel.DomainContracts;
using SharedKernel.Helpers.Configuration;
using URF.Core.Abstractions.Trackable;

namespace IdentityService.Application.CommandsAndHandlers.Users.Email.ActivationMail;

public class
    SendAccountVerificationMailCommandHandler : ICommandHandler<SendAccountVerificationMailCommand,
    AccountVerificationMailSentDto>
{
    private readonly MyConfigurationValues _appSettings;
    private readonly IEmailService _emailService;
    private readonly IMyUnitOfWork _unitOfWork;
    private readonly ITrackableRepository<User> _userRepository;

    public SendAccountVerificationMailCommandHandler(
        IMyUnitOfWork unitOfWork,
        ITrackableRepository<User> userRepository,
        IOptions<MyConfigurationValues> appSettings,
        IEmailService emailService
    )
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _appSettings = appSettings.Value;
        _emailService = emailService;
    }

    public async Task<AccountVerificationMailSentDto> Handle(SendAccountVerificationMailCommand command,
        CancellationToken cancellationToken)
    {
        ValidateCommand(command);

        var user = await FindUserAsync(command, cancellationToken);
        var emailContent = ComposeEmailContent(command, user);

        try
        {
            _emailService.Send(user.Email, "Sign-up Verification API - Verify Account", emailContent,
                "admin.poc@gmail.com");
            user.SetAccountActivationMailResent();
            // Uncomment the next line if you decide to save changes in the unit of work
            // await _unitOfWork.SaveChangesAsync(cancellationToken);
            return CreateResultDto(command, true);
        }
        catch (Exception e)
        {
            Log.Fatal(e.Message, e);
            return CreateResultDto(command, false);
        }
    }

    private void ValidateCommand(SendAccountVerificationMailCommand command)
    {
        if (string.IsNullOrEmpty(command.FirstName))
            throw new ArgumentNullException(nameof(command.FirstName));
        if (command.UserId == Guid.Empty)
            throw new ArgumentNullException(nameof(command.UserId));
        if (string.IsNullOrEmpty(command.LastName))
            throw new ArgumentNullException(nameof(command.LastName));
        if (string.IsNullOrEmpty(command.ActivationLink))
            throw new ArgumentNullException(nameof(command.ActivationLink));
        // Add any additional validation as needed
    }

    private async Task<User> FindUserAsync(SendAccountVerificationMailCommand command,
        CancellationToken cancellationToken)
    {
        var user = await _userRepository
            .Queryable()
            .Where(u => u.NormalizedEmail == command.Email.Trim().ToUpper() &&
                        u.NormalizedUserName == command.UserName.Trim().ToUpper())
            .SingleOrDefaultAsync(cancellationToken);

        if (user == null)
            throw new DomainException("Application user not found by requested Id of: [ " + command.UserId + " ]");

        return user;
    }

    private string ComposeEmailContent(SendAccountVerificationMailCommand command, User user)
    {
        var origin = command.Origin;
        var message = !string.IsNullOrEmpty(origin)
            ? $@"<p>Please click the below link to activate your account:</p>
               <p><a href=""{origin}/user/verify-email?token={command.ActivationLink}"">{origin}/user/verify-email?token={command.ActivationLink}</a></p>"
            : $@"<p>Please use the below token to activate your account with the <code>/user/verify-email</code> api route:</p>
               <p><code>{command.ActivationLink}</code></p>";

        return $@"<h4>Verify Account</h4>
                  <p>Thanks for registering!</p>
                  {message}";
    }

    private AccountVerificationMailSentDto CreateResultDto(SendAccountVerificationMailCommand command, bool success)
    {
        return new AccountVerificationMailSentDto(true, command.FirstName, command.LastName, command.Email,
            command.ActivationLink, command.ActivationLinkGenerated, command.UserId);
    }
}