using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Commands;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork.Interfaces;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using EventDriven.Domain.PoC.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using URF.Core.Abstractions.Trackable;

namespace EventDriven.Domain.PoC.Application.CommandsAndHandlers.Users.CUD
{
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
                    activity.SetTag("Error", $"A user with the following account information: Username: [ {command.UserName} ], e-mail: [ {command.Email} ] was found. Unable to create a new user with the same account data.");
                    activity?.SetStatus(ActivityStatusCode.Error); // Or ActivityStatusCode.Error in case of failure

                    return new UserDto(
                        ActiveTo: null,
                        Email: "User with this email already exists.",
                        HasBeenVerified: default, // Default value for DateTime
                        Id: command.UserId,
                        Status: "Not created",
                        UserName: "User with this username already exists.",
                        UserRoles: new List<Role>() // Initialize with an empty list or appropriate default
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
                ActiveTo: user.ActiveTo,
                Email: user.Email,
                HasBeenVerified: default, // Default value for DateTime, adjust as needed
                Id: user.Id,
                Status: user.GetCurrentRegistrationStatus().ToDescriptionString(),
                UserName: user.UserName,
                UserRoles: new List<Role>() // Assuming an empty list or a relevant default value
            );
        }
    }
}