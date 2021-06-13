using System;
using System.Threading;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Commands;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.AddressSubAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using EventDriven.Domain.PoC.SharedKernel.Extensions;
using Microsoft.EntityFrameworkCore;
using URF.Core.Abstractions.Trackable;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.Users.CUD
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
            var creator = await UserRepository.Queryable().AsNoTrackingWithIdentityResolution()
                .SingleOrDefaultAsync(user => user.Id == command.CreatorId, cancellationToken);

            var user = User.NewActiveWithPassword(
                command.Email
                , command.UserName
                , command.FirstName
                , command.LastName
                , command.Oib
                , command.DateOfBirth
                , DateTimeOffset.UtcNow
                , DateTimeOffset.UtcNow.AddYears(Consts.DEFAULT_ACTIVETO_VALUE_FOR_USER_REGISTRATIONS)
                , command.Password
                , creator
            );

            UserRepository.Attach(user);
            UserRepository.Insert(user);
            UserRepository.ApplyChanges(user);

            await UnitOfWork.SaveChangesAsync(cancellationToken);

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                StartingRole = user.BasicRole,
                Email = user.Email,
                ActiveTo = user.ActiveTo,
                Status = user.GetStatus().ToDescriptionString()
            };
        }
    }
}