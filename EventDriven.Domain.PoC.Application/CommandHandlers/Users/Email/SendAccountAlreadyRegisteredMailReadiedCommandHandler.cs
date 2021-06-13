using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EventDriven.Domain.PoC.Application.DomainServices.EmailServices;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using EventDriven.Domain.PoC.SharedKernel.Helpers.Configuration;
using Microsoft.Extensions.Options;
using URF.Core.Abstractions.Trackable;

namespace EventDriven.Domain.PoC.Application.CommandHandlers.Users.Email
{
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

        private IMyUnitOfWork UnitOfWork { get; }
        private ITrackableRepository<Role> RoleRepository { get; }
        private ITrackableRepository<User> UserRepository { get; }
        private IMapper Mapper { get; }
        private MyConfigurationValues AppSettings { get; }
        private IEmailService EmailService { get; }

        public Task<bool> Handle(SendAccountAlreadyRegisteredMailReadiedCommand request,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}