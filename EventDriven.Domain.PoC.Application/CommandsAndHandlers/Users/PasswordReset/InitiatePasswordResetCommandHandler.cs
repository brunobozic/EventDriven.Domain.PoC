using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EventDriven.Domain.PoC.Application.DomainServices.EmailServices;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RoleSubAggregate;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork.Interfaces;
using EventDriven.Domain.PoC.SharedKernel.DomainContracts;
using EventDriven.Domain.PoC.SharedKernel.Helpers.Configuration;
using Microsoft.Extensions.Options;
using URF.Core.Abstractions.Trackable;

namespace EventDriven.Domain.PoC.Application.CommandsAndHandlers.Users.PasswordReset
{
    public class InitiatePasswordResetCommandHandler : ICommandHandler<InitiatePasswordResetCommand, bool>
    {
        private readonly IEmailService _emailService;

        private readonly IMyUnitOfWork _unitOfWork;
        private readonly ITrackableRepository<User> _userRepository;
        private MyConfigurationValues _appSettings;
        private IMapper _mapper;
        private ITrackableRepository<Role> _roleRepository;

        public InitiatePasswordResetCommandHandler(
            IMyUnitOfWork unitOfWork,
            ITrackableRepository<User> userRepository,
            ITrackableRepository<Role> roleRepository,
            IMapper mapper,
            IOptions<MyConfigurationValues> appSettings,
            IEmailService emailService
        )
        {
            _unitOfWork = unitOfWork;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _emailService = emailService;
        }

        /// <summary>
        ///     Will compose and send the account verification email
        /// </summary>
        public async Task<bool> Handle(InitiatePasswordResetCommand command,
            CancellationToken cancellationToken)
        {
            //var user = await _userRepository
            //    .Queryable()
            //    .Where(u => u.Id == command.UserId)
            //    .SingleOrDefaultAsync(cancellationToken);

            return true;
        }
    }
}