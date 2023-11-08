using AutoMapper;
using EventDriven.Domain.PoC.Application.DomainServices.EmailServices;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork.Interfaces;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using EventDriven.Domain.PoC.SharedKernel.Helpers.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;
using URF.Core.Abstractions.Trackable;

namespace EventDriven.Domain.PoC.Application.CommandsAndHandlers.Users.Email
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
}