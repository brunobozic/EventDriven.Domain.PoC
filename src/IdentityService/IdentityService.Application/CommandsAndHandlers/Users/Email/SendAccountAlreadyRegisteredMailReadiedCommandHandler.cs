using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IdentityService.Application.DomainServices.EmailServices;
using IdentityService.Data.CustomUnitOfWork.Interfaces;
using IdentityService.Domain.DomainEntities.UserAggregate;
using IdentityService.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using Microsoft.Extensions.Options;
using SharedKernel.DomainContracts;
using SharedKernel.Helpers.Configuration;
using URF.Core.Abstractions.Trackable;

namespace IdentityService.Application.CommandsAndHandlers.Users.Email;

public class
    SendAccountAlreadyRegisteredMailReadiedCommandHandler : ICommandHandler<
    SendAccountAlreadyRegisteredMailReadiedCommand, bool>
{
    public SendAccountAlreadyRegisteredMailReadiedCommandHandler(
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

    private MyConfigurationValues AppSettings { get; }
    private IEmailService EmailService { get; }
    private IMapper Mapper { get; }
    private ITrackableRepository<Role> RoleRepository { get; }
    private IMyUnitOfWork UnitOfWork { get; }
    private ITrackableRepository<User> UserRepository { get; }

    public Task<bool> Handle(SendAccountAlreadyRegisteredMailReadiedCommand request,
        CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}