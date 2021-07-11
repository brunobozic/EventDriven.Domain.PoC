using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventDriven.Domain.PoC.Application.ViewModels.ApplicationUsers.Response;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate;
using EventDriven.Domain.PoC.Domain.DomainEntities.UserAggregate.RefreshToken;
using URF.Core.Abstractions.Services;

namespace EventDriven.Domain.PoC.Application.DomainServices.RefreshTokenServices
{
    public interface IRefreshTokenService : IService<RefreshToken>
    {
        Task<IEnumerable<RefreshTokenViewModel>> GetAllAsync();
        Task<RefreshTokenViewModel> GetByUserIdAsync(Guid id);
        Task<AuthenticateResponse> RefreshTheTokenAsync(string token, string ipAddress);
        Task<RevokeTokenResponse> RevokeTokenAsync(string token, string ipAddress);
        Task<RefreshTokenResponse> DeleteAsync(int tokenId, User applicationUser);
    }
}