using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using IdentityService.Application.ViewModels.ApplicationUsers.Commands;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Domain.DomainEntities.UserAggregate;
using IdentityService.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using Microsoft.EntityFrameworkCore;
using Serilog;
using SharedKernel.DomainContracts;
using SharedKernel.Extensions;
using URF.Core.Abstractions.Trackable;

namespace IdentityService.Application.CommandsAndHandlers.Users.CUD;

public class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, UserDto>
{
    public RegisterUserCommandHandler(
        IMyUnitOfWork unitOfWork,
        ITrackableRepository<User> userRepository,
        ITrackableRepository<Role> roleRepository
    )
    {
        UnitOfWork = unitOfWork;
        UserRepository = userRepository;
        RoleRepository = roleRepository;
    }

    private ITrackableRepository<Role> RoleRepository { get; }
    private IMyUnitOfWork UnitOfWork { get; }
    private ITrackableRepository<User> UserRepository { get; }

    public async Task<UserDto> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
    {
        var greeterActivitySource = new ActivitySource("OtPrGrJa");
        using var activity = greeterActivitySource.StartActivity("RegisterUserCommandHandler");
        activity.SetTag("Command.Email", command.Email);
        activity.SetTag("Command.UserName", command.UserName);
        // cant log the creator user because when registering a new user, we dont have the creator user in the db
        var creator = await UserRepository.Queryable().AsNoTrackingWithIdentityResolution()
            .SingleOrDefaultAsync(user => user.Id == Guid.Parse(ApplicationWideConstants.SYSTEM_USER),
                cancellationToken);

        var doesTheUserAlreadyExist = await UserRepository.Queryable().AsNoTrackingWithIdentityResolution()
            .AnyAsync(user => user.NormalizedEmail == command.Email.Trim().ToUpper(), cancellationToken);
        var doesTheUserAlreadyExistByUsername = await UserRepository.Queryable()
            .AsNoTrackingWithIdentityResolution()
            .AnyAsync(user => user.NormalizedUserName == command.UserName.Trim().ToUpper(), cancellationToken);

        activity?.SetTag("User.ExistsByEmail", doesTheUserAlreadyExist);
        activity?.SetTag("User.ExistsByUsername", doesTheUserAlreadyExistByUsername);

        if (!string.Equals(command.Email.Trim().ToUpper(), "bruno.bozic@gmail.com".ToUpper(),
                StringComparison.Ordinal))
            if (doesTheUserAlreadyExist || doesTheUserAlreadyExistByUsername)
            {
                // user by this email/username already exists, can not create another one!
                Log.Fatal(
                    $"A user with the following account information: Username: [ {command.UserName} ], e-mail: [ {command.Email} ] was found. Unable to create a new user with the same account data.");
                activity.SetTag("Error",
                    $"A user with the following account information: Username: [ {command.UserName} ], e-mail: [ {command.Email} ] was found. Unable to create a new user with the same account data.");
                activity?.SetStatus(ActivityStatusCode.Error); // Or ActivityStatusCode.Error in case of failure

                return new UserDto(
                    null,
                    "User with this email already exists.",
                    default, // Default value for DateTime
                    command.UserId,
                    "Not created",
                    "User with this username already exists.",
                    new List<Role>() // Initialize with an empty list or appropriate default
                );

                throw new UserAlreadyExistsException(command.Email, command.UserName);
            }

        var user = User.NewActiveWithPassword(
            command.UserId
            , command.Email
            , command.UserName
            , command.FirstName
            , command.LastName
            , command.Oib
            , command.DateOfBirth
            , DateTimeOffset.UtcNow
            , DateTimeOffset.UtcNow.AddYears(ApplicationWideConstants.DEFAULT_ACTIVETO_VALUE_FOR_USER_REGISTRATIONS)
            , command.Password
            , creator
            , command.Origin
        );

        UserRepository.Attach(user);
        UserRepository.Insert(user);
        UserRepository.ApplyChanges(user);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        activity?.SetStatus(ActivityStatusCode.Ok); // Or ActivityStatusCode.Error in case of failure
        var ae = new ActivityEvent("User registration successful, user account saved into data store");
        activity?.AddEvent(ae);
        activity?.SetTag("User registration status", user.GetCurrentRegistrationStatus().ToDescriptionString());
        activity?.SetTag("User Id", user.Id);

        // TODO: add user roles
        return new UserDto(
            user.ActiveTo,
            user.Email,
            default, // Default value for DateTime, adjust as needed
            user.Id,
            user.GetCurrentRegistrationStatus().ToDescriptionString(),
            user.UserName,
            new List<Role>() // Assuming an empty list or a relevant default value
        );
    }
}