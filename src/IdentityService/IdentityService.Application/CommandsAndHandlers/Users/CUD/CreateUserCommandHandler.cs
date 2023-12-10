using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IdentityService.Application.DomainServices.EmailServices;
using IdentityService.Application.ViewModels.ApplicationUsers.Commands;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Domain.DomainEntities.UserAggregate;
using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SharedKernel.DomainContracts;
using SharedKernel.Helpers.Configuration;
using URF.Core.Abstractions.Trackable;

namespace IdentityService.Application.CommandsAndHandlers.Users.CUD;

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

    public async Task<UserDto> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var creator = await UserRepository.Queryable().AsNoTrackingWithIdentityResolution()
            .SingleOrDefaultAsync(user => user.Id == command.Creator.Id, cancellationToken);

        var user = User.NewActiveWithPassword(
            command.Id
            , command.Email
            , command.UserName
            , command.FirstName
            , command.LastName
            , command.Oib
            , command.DateOfBirth
            , command.ActiveFrom
            , command.ActiveTo
            , command.Password
            , creator
            , command.Origin
        );

        UserRepository.Attach(user);
        UserRepository.Insert(user);
        UserRepository.ApplyChanges(user);

        await UnitOfWork.SaveChangesAsync(cancellationToken);

        return new UserDto(
            user.ActiveTo,
            user.Email,
            user.Verified,
            user.Id,
            user.GetCurrentRegistrationStatus().ToString(),
            user.UserName,
            user.GetUserRoles()
        );
    }
}