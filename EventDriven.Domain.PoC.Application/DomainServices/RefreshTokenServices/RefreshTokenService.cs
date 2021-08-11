using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EventDriven.Domain.PoC.Application.DomainServices.EmailServices;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Response;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RefreshToken;
using EventDriven.Domain.PoC.Repository.EF.CustomUnitOfWork.Interfaces;
using EventDriven.Domain.PoC.SharedKernel.Helpers.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using URF.Core.Abstractions.Trackable;
using URF.Core.Services;

namespace EventDriven.Domain.PoC.Application.DomainServices.RefreshTokenServices
{
    public class RefreshTokenService : Service<RefreshToken>, IRefreshTokenService
    {
        #region public props

        public ITrackableRepository<User> UserRepository { get; }

        #endregion public props

        #region private props

        private IMyUnitOfWork UnitOfWork { get; }
        private readonly IMapper _mapper;
        private readonly MyConfigurationValues _appSettings;
        private readonly IEmailService _emailService;

        #endregion private props

        #region ctor

        public RefreshTokenService(
            IMyUnitOfWork unitOfWork,
            ITrackableRepository<RefreshToken> repository,
            ITrackableRepository<User> userRepository,
            IMapper mapper,
            IOptions<MyConfigurationValues> appSettings,
            IEmailService emailService) : base(repository)
        {
            UnitOfWork = unitOfWork;
            UserRepository = userRepository;
            _mapper = mapper;
            _appSettings = appSettings.Value;
            _emailService = emailService;
        }

        public async Task<IEnumerable<RefreshTokenViewModel>> GetAllAsync()
        {
            var refreshTokens = await Repository.Queryable().ToListAsync();
            return _mapper.Map<IList<RefreshTokenViewModel>>(refreshTokens);
        }

        public async Task<RefreshTokenViewModel> GetByUserIdAsync(Guid id)
        {
            var refreshToken = await Repository.Queryable().Where(rt => rt.ApplicationUser.Id == id)
                .SingleOrDefaultAsync();

            if (refreshToken == null) throw new KeyNotFoundException(id.ToString());

            return _mapper.Map<RefreshTokenViewModel>(refreshToken);
        }

        public Task<AuthenticateResponse> RefreshTheTokenAsync(string token, string ipAddress)
        {
            throw new NotImplementedException();
        }

        public Task<RevokeTokenResponse> RevokeTokenAsync(string token, string ipAddress)
        {
            throw new NotImplementedException();
        }

        public Task<RefreshTokenResponse> DeleteAsync(int tokenId, User applicationUser)
        {
            throw new NotImplementedException();
        }

        #endregion ctor
    }
}