using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EventDriven.Domain.PoC.Application.DomainServices.EmailServices;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Commands;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using EventDriven.Domain.PoC.SharedKernel.Helpers.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using URF.Core.Abstractions.Trackable;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.Users.CUD
{
    public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, UserDto>
    {
        public CreateUserCommandHandler(
            IMyUnitOfWork unitOfWork,
            ITrackableRepository<User> userRepository,
            ITrackableRepository<Role> roleRepository,
            IMapper mapper,
            IOptions<MyConfigurationValues> appSettings,
            IEmailService emailService
        )
        {
            UnitOfWork = unitOfWork;
            UserRepository = userRepository;
            RoleRepository = roleRepository;
            Mapper = mapper;
            AppSettings = appSettings.Value;
            EmailService = emailService;
        }

        private IMyUnitOfWork UnitOfWork { get; }
        private ITrackableRepository<Role> RoleRepository { get; }
        private ITrackableRepository<User> UserRepository { get; }
        private IMapper Mapper { get; }
        private MyConfigurationValues AppSettings { get; }
        private IEmailService EmailService { get; }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var creator = await UserRepository.Queryable().AsNoTrackingWithIdentityResolution()
                .SingleOrDefaultAsync(user => user.Id == request.Creator.Id, cancellationToken);

            var user = User.NewActiveWithPassword(
                request.Email
                , request.UserName
                , request.FirstName
                , request.LastName
                , request.Oib
                , request.DateOfBirth
                , request.ActiveFrom
                , request.ActiveTo
                , request.Password
                , creator
            );

            UserRepository.Attach(user);
            UserRepository.Insert(user);
            UserRepository.ApplyChanges(user);

            await UnitOfWork.SaveChangesAsync(cancellationToken);

            return new UserDto {Id = user.Id, UserName = user.UserName, UserRoles = user.GetUserRoles()};
        }
    }
}