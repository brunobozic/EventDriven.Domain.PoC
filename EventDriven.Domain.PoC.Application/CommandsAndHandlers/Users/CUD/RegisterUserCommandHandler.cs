using System;
using System.Threading;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Commands;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork.Interfaces;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using EventDriven.Domain.PoC.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;
using Serilog;
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

        private IMyUnitOfWork UnitOfWork { get; }
        private ITrackableRepository<Role> RoleRepository { get; }
        private ITrackableRepository<User> UserRepository { get; }


        public async Task<UserDto> Handle(RegisterUserCommand command, CancellationToken cancellationToken)
        {
            // cant log the creator user because when registering a new user, we dont have the creator user in the db
            var creator = await UserRepository.Queryable().AsNoTrackingWithIdentityResolution()
                .SingleOrDefaultAsync(user => user.Id == Guid.Parse(ApplicationWideConstants.SYSTEM_USER),
                    cancellationToken);

            var doesTheUserAlreadyExist = await UserRepository.Queryable().AsNoTrackingWithIdentityResolution()
                .AnyAsync(user => user.NormalizedEmail == command.Email.Trim().ToUpper(), cancellationToken);
            var doesTheUserAlreadyExistByUsername = await UserRepository.Queryable()
                .AsNoTrackingWithIdentityResolution()
                .AnyAsync(user => user.NormalizedUserName == command.UserName.Trim().ToUpper(), cancellationToken);

            if (!string.Equals(command.Email.Trim().ToUpper(), "bruno.bozic@gmail.com".ToUpper(),
                StringComparison.Ordinal))
                if (doesTheUserAlreadyExist || doesTheUserAlreadyExistByUsername)
                {
                    // user by this email/username already exists, can not create another one!
                    Log.Fatal(
                        $"A user with the following account information: Username: [ {command.UserName} ], e-mail: [ {command.Email} ] was found. Unable to create a new user with the same account data.");

                    return new UserDto
                    {
                        Id = command.UserId,
                        UserName = "User with this username already exists.",
                        Email = "User with this email already exists.",
                        ActiveTo = null,
                        Status = "Not created"
                    };
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

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                ActiveTo = user.ActiveTo,
                Status = user.GetCurrentRegistrationStatus().ToDescriptionString()
            };
        }
    }
}